using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
#pragma warning disable
#region MAPPER ATTRIBUTES

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Set a name to this property in BsonDocument
    /// </summary>
    public class BsonFieldAttribute : Attribute
    {
        public string Name { get; set; }

        public BsonFieldAttribute(string name)
        {
            this.Name = name;
        }

        public BsonFieldAttribute()
        {
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Indicate that property will be used as BsonDocument Id
    /// </summary>
    public class BsonIdAttribute : Attribute
    {
        public bool AutoId { get; private set; }

        public BsonIdAttribute()
        {
            this.AutoId = true;
        }

        public BsonIdAttribute(bool autoId)
        {
            this.AutoId = autoId;
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Indicate that property will not be persist in Bson serialization
    /// </summary>
    public class BsonIgnoreAttribute : Attribute
    {
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Add an index in this entity property.
    /// </summary>
    [Obsolete("Do not use Index attribute, use EnsureIndex on database creation")]
    public class BsonIndexAttribute : Attribute
    {
        public bool Unique { get; private set; }

        public BsonIndexAttribute()
            : this(false)
        {
        }

        public BsonIndexAttribute(bool unique)
        {
            this.Unique = unique;
        }
    }
}

#endregion

#region MAPPER  REFLEXION

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    #region Delegates

    internal delegate object CreateObject();

    public delegate void GenericSetter(object target, object value);

    public delegate object GenericGetter(object obj);

    #endregion

    /// <summary>
    /// Helper class to get entity properties and map as BsonValue
    /// </summary>
    internal class Reflection
    {
        private static Dictionary<Type, CreateObject> _cacheCtor = new Dictionary<Type, CreateObject>();

        #region CreateInstance

        /// <summary>
        /// Create a new instance from a Type
        /// </summary>
        public static object CreateInstance(Type type)
        {
            try
            {
                if (_cacheCtor.TryGetValue(type, out CreateObject c))
                {
                    return c();
                }
            }
            catch (Exception ex)
            {
                throw UltraLiteException.InvalidCtor(type, ex);
            }

            lock (_cacheCtor)
            {
                try
                {
                    if (_cacheCtor.TryGetValue(type, out CreateObject c))
                    {
                        return c();
                    }

                    if (type.GetTypeInfo().IsClass)
                    {
                        _cacheCtor.Add(type, c = CreateClass(type));
                    }
                    else if (type.GetTypeInfo().IsInterface) // some know interfaces
                    {
                        if(type.GetTypeInfo().IsGenericType)
                        {
                            var typeDef = type.GetGenericTypeDefinition();

                            if (typeDef == typeof(IList<>) || 
                                typeDef == typeof(ICollection<>) ||
                                typeDef == typeof(IEnumerable<>))
                            {
                                return CreateInstance(GetGenericListOfType(UnderlyingTypeOf(type)));
                            }
                            else if (typeDef == typeof(IDictionary<,>))
                            {
                                var k = type.GetTypeInfo().GetGenericArguments()[0];
                                var v = type.GetTypeInfo().GetGenericArguments()[1];

                                return CreateInstance(GetGenericDictionaryOfType(k, v));
                            }
                        }

                        throw UltraLiteException.InvalidCtor(type, null);
                    }
                    else // structs
                    {
                        _cacheCtor.Add(type, c = CreateStruct(type));
                    }

                    return c();
                }
                catch (Exception ex)
                {
                    throw UltraLiteException.InvalidCtor(type, ex);
                }
            }
        }

        #endregion

        #region Utils

        public static bool IsNullable(Type type)
        {
            if (!type.GetTypeInfo().IsGenericType) return false;
            var g = type.GetGenericTypeDefinition();
            return (g.Equals(typeof(Nullable<>)));
        }

        /// <summary>
        /// Get underlying get - using to get inner Type from Nullable type
        /// </summary>
        public static Type UnderlyingTypeOf(Type type)
        {
            // works only for generics (if type is not generic, returns same type)
            var t = type.GetTypeInfo();

            if (!type.GetTypeInfo().IsGenericType) return type;

            return type.GetTypeInfo().GetGenericArguments()[0];
        }

        public static Type GetGenericListOfType(Type type)
        {
            var listType = typeof(List<>);
            return listType.MakeGenericType(type);
        }

        public static Type GetGenericDictionaryOfType(Type k, Type v)
        {
            var listType = typeof(Dictionary<,>);
            return listType.MakeGenericType(k, v);
        }

        /// <summary>
        /// Get item type from a generic List or Array
        /// </summary>
        public static Type GetListItemType(Type listType)
        {
            if (listType.IsArray) return listType.GetElementType();

            foreach (var i in listType.GetInterfaces())
            {
                if (i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    return i.GetTypeInfo().GetGenericArguments()[0];
                }
                // if interface is IEnumerable (non-generic), let's get from listType and not from interface
                // from #395
                else if(listType.GetTypeInfo().IsGenericType && i == typeof(IEnumerable))
                {
                    return listType.GetTypeInfo().GetGenericArguments()[0];
                }
            }

            return typeof(object);
        }

        /// <summary>
        /// Returns true if Type is any kind of Array/IList/ICollection/....
        /// </summary>
        public static bool IsList(Type type)
        {
            if (type.IsArray) return true;
            if (type == typeof(string)) return false; // do not define "String" as IEnumerable<char>

            foreach (var @interface in type.GetInterfaces())
            {
                if (@interface.GetTypeInfo().IsGenericType)
                {
                    if (@interface.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    {
                        // if needed, you can also return the type used as generic argument
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Select member from a list of member using predicate order function to select
        /// </summary>
        public static MemberInfo SelectMember(IEnumerable<MemberInfo> members, params Func<MemberInfo, bool>[] predicates)
        {
            foreach (var predicate in predicates)
            {
                var member = members.FirstOrDefault(predicate);

                if (member != null)
                {
                    return member;
                }
            }

            return null;
        }

        #endregion

        public static CreateObject CreateClass(Type type)
        {
            return () => Activator.CreateInstance(type);
        }

        public static CreateObject CreateStruct(Type type)
        {
            return () => Activator.CreateInstance(type);
        }

        public static GenericGetter CreateGenericGetter(Type type, MemberInfo memberInfo)
        {
            // when member is a field, use simple Reflection
            if (memberInfo is FieldInfo)
            {
                var fieldInfo = memberInfo as FieldInfo;

                return fieldInfo.GetValue;
            }

            // if is property, use Emit IL code
            var propertyInfo = memberInfo as PropertyInfo;
            var getMethod = propertyInfo.GetGetMethod(true);

            if (getMethod == null) return null;

            return target => getMethod.Invoke(target, null);
        }

        public static GenericSetter CreateGenericSetter(Type type, MemberInfo memberInfo)
        {
            // when member is a field, use simple Reflection
            if (memberInfo is FieldInfo)
            {
                var fieldInfo = memberInfo as FieldInfo;

                return fieldInfo.SetValue;
            }

            // if is property, use Emit IL code
            var propertyInfo = memberInfo as PropertyInfo;
            var setMethod = propertyInfo.GetSetMethod(true);

            if (setMethod == null) return null;

            return (target, value) => setMethod.Invoke(target, new[] { value });
        }
    
    }
}

#endregion

#region MAPPER

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Class that converts your entity class to/from BsonDocument
    /// If you prefer use a new instance of BsonMapper (not Global), be sure cache this instance for better performance
    /// Serialization rules:
    ///     - Classes must be "public" with a public constructor (without parameters)
    ///     - Properties must have public getter (can be read-only)
    ///     - Entity class must have Id property, [ClassName]Id property or [BsonId] attribute
    ///     - No circular references
    ///     - Fields are not valid
    ///     - IList, Array supports
    ///     - IDictionary supports (Key must be a simple datatype - converted by ChangeType)
    /// </summary>
    public partial class BsonMapper
    {
        private const int MAX_DEPTH = 20;

        #region Properties

        /// <summary>
        /// Mapping cache between Class/BsonDocument
        /// </summary>
        private Dictionary<Type, EntityMapper> _entities = new Dictionary<Type, EntityMapper>();

        /// <summary>
        /// Map serializer/deserialize for custom types
        /// </summary>
        private Dictionary<Type, Func<object, BsonValue>> _customSerializer = new Dictionary<Type, Func<object, BsonValue>>();

        private Dictionary<Type, Func<BsonValue, object>> _customDeserializer = new Dictionary<Type, Func<BsonValue, object>>();

        /// <summary>
        /// Type instantiator function to support IoC
        /// </summary>
        private readonly Func<Type, object> _typeInstantiator;

        /// <summary>
        /// Global instance used when no BsonMapper are passed in UltraLiteDatabase ctor
        /// </summary>
        public static BsonMapper Global = new BsonMapper();

        /// <summary>
        /// A resolver name for field
        /// </summary>
        public Func<string, string> ResolveFieldName;

        /// <summary>
        /// Indicate that mapper do not serialize null values (default false)
        /// </summary>
        public bool SerializeNullValues { get; set; }

        /// <summary>
        /// Apply .Trim() in strings when serialize (default true)
        /// </summary>
        public bool TrimWhitespace { get; set; }

        /// <summary>
        /// Convert EmptyString to Null (default true)
        /// </summary>
        public bool EmptyStringToNull { get; set; }

        /// <summary>
        /// Get/Set that mapper must include fields (default: false)
        /// </summary>
        public bool IncludeFields { get; set; }

        /// <summary>
        /// Get/Set that mapper must include non public (private, protected and internal) (default: false)
        /// </summary>
        public bool IncludeNonPublic { get; set; }

        /// <summary>
        /// A custom callback to change MemberInfo behavior when converting to MemberMapper.
        /// Use mapper.ResolveMember(Type entity, MemberInfo property, MemberMapper documentMappedField)
        /// Set FieldName to null if you want remove from mapped document
        /// </summary>
        public Action<Type, MemberInfo, MemberMapper> ResolveMember;

        /// <summary>
        /// Custom resolve name collection based on Type 
        /// </summary>
        public Func<Type, string> ResolveCollectionName;

        #endregion

        public BsonMapper(Func<Type, object> customTypeInstantiator = null)
        {
            this.SerializeNullValues = false;
            this.TrimWhitespace = true;
            this.EmptyStringToNull = true;
            this.ResolveFieldName = (s) => s;
            this.ResolveMember = (t, mi, mm) => { };
            this.ResolveCollectionName = (t) => Reflection.IsList(t) ? Reflection.GetListItemType(t).Name : t.Name;
            this.IncludeFields = false;

            _typeInstantiator = customTypeInstantiator ?? Reflection.CreateInstance;

            #region Register CustomTypes

            RegisterType<Uri>(uri => uri.AbsoluteUri, bson => new Uri(bson.AsString));
            RegisterType<DateTimeOffset>(value => new BsonValue(value.UtcDateTime), bson => bson.AsDateTime.ToUniversalTime());
            RegisterType<TimeSpan>(value => new BsonValue(value.Ticks), bson => new TimeSpan(bson.AsInt64));
            RegisterType<Regex>(
                r => r.Options == RegexOptions.None ? new BsonValue(r.ToString()) : new BsonDocument { { "p", r.ToString() }, { "o", (int)r.Options } },
                value => value.IsString ? new Regex(value) : new Regex(value.AsDocument["p"].AsString, (RegexOptions)value.AsDocument["o"].AsInt32)
            );


            #endregion

        }

        #region Register CustomType

        /// <summary>
        /// Register a custom type serializer/deserialize function
        /// </summary>
        public void RegisterType<T>(Func<T, BsonValue> serialize, Func<BsonValue, T> deserialize)
        {
            _customSerializer[typeof(T)] = (o) => serialize((T)o);
            _customDeserializer[typeof(T)] = (b) => (T)deserialize(b);
        }

        /// <summary>
        /// Register a custom type serializer/deserialize function
        /// </summary>
        public void RegisterType(Type type, Func<object, BsonValue> serialize, Func<BsonValue, object> deserialize)
        {
            _customSerializer[type] = (o) => serialize(o);
            _customDeserializer[type] = (b) => deserialize(b);
        }

        #endregion

        /// <summary>
        /// Map your entity class to BsonDocument using fluent API
        /// </summary>
        public EntityBuilder<T> Entity<T>()
        {
            return new EntityBuilder<T>(this);
        }


        #region Predefinded Property Resolvers

        /// <summary>
        /// Use lower camel case resolution for convert property names to field names
        /// </summary>
        public BsonMapper UseCamelCase()
        {
            this.ResolveFieldName = (s) => char.ToLower(s[0]) + s.Substring(1);

            return this;
        }

        private Regex _lowerCaseDelimiter = new Regex("(?!(^[A-Z]))([A-Z])", RegexOptions.Compiled);

        /// <summary>
        /// Uses lower camel case with delimiter to convert property names to field names
        /// </summary>
        public BsonMapper UseLowerCaseDelimiter(char delimiter = '_')
        {
            this.ResolveFieldName = (s) => _lowerCaseDelimiter.Replace(s, delimiter + "$2").ToLower();

            return this;
        }

        #endregion

        #region GetEntityMapper

        /// <summary>
        /// Get property mapper between typed .NET class and BsonDocument - Cache results
        /// </summary>
        internal EntityMapper GetEntityMapper(Type type)
        {
            //TODO: needs check if Type if BsonDocument? Returns empty EntityMapper?

            if (!_entities.TryGetValue(type, out EntityMapper mapper))
            {
                lock (_entities)
                {
                    if (!_entities.TryGetValue(type, out mapper))
                    {
                        return _entities[type] = BuildEntityMapper(type);
                    }
                }
            }

            return mapper;
        }

        /// <summary>
        /// Use this method to override how your class can be, by default, mapped from entity to Bson document.
        /// Returns an EntityMapper from each requested Type
        /// </summary>
        protected virtual EntityMapper BuildEntityMapper(Type type)
        {
            var mapper = new EntityMapper
            {
                Members = new List<MemberMapper>(),
                ForType = type
            };

            var idAttr = typeof(BsonIdAttribute);
            var ignoreAttr = typeof(BsonIgnoreAttribute);
            var fieldAttr = typeof(BsonFieldAttribute);
            // var indexAttr = typeof(BsonIndexAttribute);

            var members = this.GetTypeMembers(type);
            var id = this.GetIdMember(members);

            foreach (var memberInfo in members)
            {
                // checks [BsonIgnore]
                if (memberInfo.IsDefined(ignoreAttr, true)) continue;

                // checks field name conversion
                var name = this.ResolveFieldName(memberInfo.Name);

                // check if property has [BsonField]
                var field = (BsonFieldAttribute)memberInfo.GetCustomAttributes(fieldAttr, false).FirstOrDefault();

                // check if property has [BsonField] with a custom field name
                if (field != null && field.Name != null)
                {
                    name = field.Name;
                }

                // checks if memberInfo is id field
                if (memberInfo == id)
                {
                    name = "_id";
                }

                // create getter/setter function
                var getter = Reflection.CreateGenericGetter(type, memberInfo);
                var setter = Reflection.CreateGenericSetter(type, memberInfo);

                // check if property has [BsonId] to get with was setted AutoId = true
                var autoId = (BsonIdAttribute)memberInfo.GetCustomAttributes(idAttr, false).FirstOrDefault();

                // get data type
                var dataType = memberInfo is PropertyInfo ?
                    (memberInfo as PropertyInfo).PropertyType :
                    (memberInfo as FieldInfo).FieldType;

                // check if datatype is list/array
                var isList = Reflection.IsList(dataType);

                // create a property mapper
                var member = new MemberMapper
                {
                    AutoId = autoId == null ? true : autoId.AutoId,
                    FieldName = name,
                    MemberName = memberInfo.Name,
                    DataType = dataType,
                    IsList = isList,
                    UnderlyingType = isList ? Reflection.GetListItemType(dataType) : dataType,
                    Getter = getter,
                    Setter = setter
                };

                // support callback to user modify member mapper
                if (this.ResolveMember != null)
                {
                    this.ResolveMember(type, memberInfo, member);
                }

                // test if has name and there is no duplicate field
                if (member.FieldName != null && mapper.Members.Any(x => x.FieldName == name) == false)
                {
                    mapper.Members.Add(member);
                }
            }

            return mapper;
        }

        /// <summary>
        /// Gets MemberInfo that refers to Id from a document object.
        /// </summary>
        protected virtual MemberInfo GetIdMember(IEnumerable<MemberInfo> members)
        {
            return Reflection.SelectMember(members,
                x => Attribute.IsDefined(x, typeof(BsonIdAttribute), true),
                x => x.Name.Equals("Id", StringComparison.OrdinalIgnoreCase),
                x => x.Name.Equals(x.DeclaringType.Name + "Id", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Returns all member that will be have mapper between POCO class to document
        /// </summary>
        protected virtual IEnumerable<MemberInfo> GetTypeMembers(Type type)
        {
            var members = new List<MemberInfo>();

            var flags = this.IncludeNonPublic ?
                (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) :
                (BindingFlags.Public | BindingFlags.Instance);

            members.AddRange(type.GetProperties(flags)
                .Where(x => x.CanRead && x.GetIndexParameters().Length == 0)
                .Select(x => x as MemberInfo));

            if(this.IncludeFields)
            {
                members.AddRange(type.GetFields(flags).Where(x => !x.Name.EndsWith("k__BackingField") && x.IsStatic == false).Select(x => x as MemberInfo));
            }

            return members;
        }

        #endregion

     
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public partial class BsonMapper
    {
        /// <summary>
        /// Deserialize a BsonDocument to entity class
        /// </summary>
        public virtual object ToObject(Type type, BsonDocument doc)
        {
            if (doc == null) throw new ArgumentNullException(nameof(doc));

            // if T is BsonDocument, just return them
            if (type == typeof(BsonDocument)) return doc;

            return this.Deserialize(type, doc);
        }

        /// <summary>
        /// Deserialize a BsonDocument to entity class
        /// </summary>
        public virtual T ToObject<T>(BsonDocument doc)
        {
            return (T)this.ToObject(typeof(T), doc);
        }

        /// <summary>
        /// Deserialize an BsonValue to .NET object typed in T
        /// </summary>
        internal T Deserialize<T>(BsonValue value)
        {
            if (value == null) return default(T);

            var result = this.Deserialize(typeof(T), value);

            return (T)result;
        }

        #region Basic direct .NET convert types

        // direct bson types
        private HashSet<Type> _bsonTypes = new HashSet<Type>
        {
            typeof(String),
            typeof(Int32),
            typeof(Int64),
            typeof(Boolean),
            typeof(Guid),
            typeof(DateTime),
            typeof(Byte[]),
            typeof(ObjectId),
            typeof(Double),
            typeof(Decimal)
        };

        // simple convert types
        private HashSet<Type> _basicTypes = new HashSet<Type>
        {
            typeof(Int16),
            typeof(UInt16),
            typeof(UInt32),
            typeof(Single),
            typeof(Char),
            typeof(Byte),
            typeof(SByte)
        };

        #endregion

        internal object Deserialize(Type type, BsonValue value)
        {
            Func<BsonValue, object> custom;

            // null value - null returns
            if (value.IsNull) return null;

            // if is nullable, get underlying type
            else if (Reflection.IsNullable(type))
            {
                type = Reflection.UnderlyingTypeOf(type);
            }

            // check if your type is already a BsonValue/BsonDocument/BsonArray
            if (type == typeof(BsonValue))
            {
                return new BsonValue(value);
            }
            else if (type == typeof(BsonDocument))
            {
                return value.AsDocument;
            }
            else if (type == typeof(BsonArray))
            {
                return value.AsArray;
            }

            // raw values to native bson values
            else if (_bsonTypes.Contains(type))
            {
                return value.RawValue;
            }

            // simple ConvertTo to basic .NET types
            else if (_basicTypes.Contains(type))
            {
                return Convert.ChangeType(value.RawValue, type);
            }

            // special cast to UInt64 to Int64
            else if (type == typeof(UInt64))
            {
                return unchecked((UInt64)((Int64)value.RawValue));
            }

            // enum value is an int
            else if (type.GetTypeInfo().IsEnum)
            {
                return Enum.Parse(type, value.AsString);
            }

            // test if has a custom type implementation
            else if (_customDeserializer.TryGetValue(type, out custom))
            {
                return custom(value);
            }

            // if value is array, deserialize as array
            else if (value.IsArray)
            {
                // when array are from an object (like in Dictionary<string, object> { ["array"] = new string[] { "a", "b" } 
                if (type == typeof(object))
                {
                    return this.DeserializeArray(typeof(object), value.AsArray);
                }
                if (type.IsArray)
                {
                    return this.DeserializeArray(type.GetElementType(), value.AsArray);
                }
                else
                {
                    return this.DeserializeList(type, value.AsArray);
                }
            }

            // if value is document, deserialize as document
            else if (value.IsDocument)
            {
                BsonValue typeField;
                var doc = value.AsDocument;

                // test if value is object and has _type
                if (doc.RawValue.TryGetValue("_type", out typeField))
                {
                    type = Type.GetType(typeField.AsString);

                    if (type == null) throw UltraLiteException.InvalidTypedName(typeField.AsString);
                }
                // when complex type has no definition (== typeof(object)) use Dictionary<string, object> to better set values
                else if (type == typeof(object))
                {
                    type = typeof(Dictionary<string, object>);
                }

                var o = _typeInstantiator(type);

                if (o is IDictionary && type.GetTypeInfo().IsGenericType)
                {
                    var k = type.GetTypeInfo().GetGenericArguments()[0];
                    var t = type.GetTypeInfo().GetGenericArguments()[1];

                    this.DeserializeDictionary(k, t, (IDictionary)o, value.AsDocument);
                }
                else
                {
                    this.DeserializeObject(type, o, doc);
                }

                return o;
            }

            // in last case, return value as-is - can cause "cast error"
            // it's used for "public object MyInt { get; set; }"
            return value.RawValue;
        }

        private object DeserializeArray(Type type, BsonArray array)
        {
            var arr = Array.CreateInstance(type, array.Count);
            var idx = 0;

            foreach (var item in array)
            {
                arr.SetValue(this.Deserialize(type, item), idx++);
            }

            return arr;
        }

        private object DeserializeList(Type type, BsonArray value)
        {
            var itemType = Reflection.GetListItemType(type);
            var enumerable = (IEnumerable)Reflection.CreateInstance(type);
            var list = enumerable as IList;

            if (list != null)
            {
                foreach (BsonValue item in value)
                {
                    list.Add(Deserialize(itemType, item));
                }
            }
            else
            {
                var addMethod = type.GetMethod("Add");

                foreach (BsonValue item in value)
                {
                    addMethod.Invoke(enumerable, new[] { Deserialize(itemType, item) });
                }
            }

            return enumerable;
        }

        private void DeserializeDictionary(Type K, Type T, IDictionary dict, BsonDocument value)
        {
            foreach (var key in value.Keys)
            {
                var k = K.GetTypeInfo().IsEnum ? Enum.Parse(K, key) : Convert.ChangeType(key, K);
                var v = this.Deserialize(T, value[key]);

                dict.Add(k, v);
            }
        }

        public void DeserializeObject(Type type, object obj, BsonDocument value)
        {
            var entity = this.GetEntityMapper(type);

            foreach (var member in entity.Members.Where(x => x.Setter != null))
            {
                var val = value[member.FieldName];

                if (!val.IsNull)
                {
                    // check if has a custom deserialize function
                    if (member.Deserialize != null)
                    {
                        member.Setter(obj, member.Deserialize(val, this));
                    }
                    else
                    {
                        member.Setter(obj, this.Deserialize(member.DataType, val));
                    }
                }
            }
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public partial class BsonMapper
    {
        /// <summary>
        /// Serialize a entity class to BsonDocument
        /// </summary>
        public virtual BsonDocument ToDocument(Type type, object entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            // if object is BsonDocument, just return them
            if (entity is BsonDocument) return (BsonDocument)entity;

            return this.Serialize(type, entity, 0).AsDocument;
        }

        /// <summary>
        /// Serialize a entity class to BsonDocument
        /// </summary>
        public virtual BsonDocument ToDocument<T>(T entity)
        {
            return this.ToDocument(typeof(T), entity);
        }

        internal BsonValue Serialize(Type type, object obj, int depth)
        {
            if (++depth > MAX_DEPTH) throw UltraLiteException.DocumentMaxDepth(MAX_DEPTH, type);

            if (obj == null) return BsonValue.Null;

            Func<object, BsonValue> custom;

            // if is already a bson value
            if (obj is BsonValue) return new BsonValue((BsonValue)obj);

            // test string - mapper has some special options
            else if (obj is String)
            {
                var str = this.TrimWhitespace ? (obj as String).Trim() : (String)obj;

                if (this.EmptyStringToNull && str.Length == 0)
                {
                    return BsonValue.Null;
                }
                else
                {
                    return new BsonValue(str);
                }
            }
            // basic Bson data types (cast datatype for better performance optimization)
            else if (obj is Int32) return new BsonValue((Int32)obj);
            else if (obj is Int64) return new BsonValue((Int64)obj);
            else if (obj is Double) return new BsonValue((Double)obj);
            else if (obj is Decimal) return new BsonValue((Decimal)obj);
            else if (obj is Byte[]) return new BsonValue((Byte[])obj);
            else if (obj is ObjectId) return new BsonValue((ObjectId)obj);
            else if (obj is Guid) return new BsonValue((Guid)obj);
            else if (obj is Boolean) return new BsonValue((Boolean)obj);
            else if (obj is DateTime) return new BsonValue((DateTime)obj);
            // basic .net type to convert to bson
            else if (obj is Int16 || obj is UInt16 || obj is Byte || obj is SByte)
            {
                return new BsonValue(Convert.ToInt32(obj));
            }
            else if (obj is UInt32)
            {
                return new BsonValue(Convert.ToInt64(obj));
            }
            else if (obj is UInt64)
            {
                var ulng = ((UInt64)obj);
                var lng = unchecked((Int64)ulng);

                return new BsonValue(lng);
            }
            else if (obj is Single)
            {
                return new BsonValue(Convert.ToDouble(obj));
            }
            else if (obj is Char || obj is Enum)
            {
                return new BsonValue(obj.ToString());
            }
            // check if is a custom type
            else if (_customSerializer.TryGetValue(type, out custom) || _customSerializer.TryGetValue(obj.GetType(), out custom))
            {
                return custom(obj);
            }
            // for dictionary
            else if (obj is IDictionary)
            {
                // when you are converting Dictionary<string, object>
                if (type == typeof(object))
                {
                    type = obj.GetType();
                }

                var itemType = type.GetTypeInfo().GetGenericArguments()[1];

                return this.SerializeDictionary(itemType, obj as IDictionary, depth);
            }
            // check if is a list or array
            else if (obj is IEnumerable)
            {
                return this.SerializeArray(Reflection.GetListItemType(obj.GetType()), obj as IEnumerable, depth);
            }
            // otherwise serialize as a plain object
            else
            {
                return this.SerializeObject(type, obj, depth);
            }
        }

        private BsonArray SerializeArray(Type type, IEnumerable array, int depth)
        {
            var arr = new BsonArray();

            foreach (var item in array)
            {
                arr.Add(this.Serialize(type, item, depth));
            }

            return arr;
        }

        private BsonDocument SerializeDictionary(Type type, IDictionary dict, int depth)
        {
            var o = new BsonDocument();

            foreach (var key in dict.Keys)
            {
                var value = dict[key];

                o.RawValue[key.ToString()] = this.Serialize(type, value, depth);
            }

            return o;
        }

        public BsonDocument SerializeObject(Type type, object obj, int depth)
        {
            var o = new BsonDocument();
            var t = obj.GetType();
            var entity = this.GetEntityMapper(t);
            var dict = o.RawValue;

            // adding _type only where property Type is not same as object instance type
            if (type != t)
            {
                dict["_type"] = new BsonValue(t.FullName + ", " + t.GetTypeInfo().Assembly.GetName().Name);
            }

            foreach (var member in entity.Members.Where(x => x.Getter != null))
            {
                // get member value
                var value = member.Getter(obj);

                if (value == null && this.SerializeNullValues == false && member.FieldName != "_id") continue;

                // if member has a custom serialization, use it
                if (member.Serialize != null)
                {
                    dict[member.FieldName] = member.Serialize(value, this);
                }
                else
                {
                    dict[member.FieldName] = this.Serialize(member.DataType, value, depth);
                }
            }

            return o;
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Helper class to modify your entity mapping to document. Can be used instead attribute decorates
    /// </summary>
    public class EntityBuilder<T>
    {
        private BsonMapper _mapper;
        private EntityMapper _entity;

        internal EntityBuilder(BsonMapper mapper)
        {
            _mapper = mapper;
            _entity = mapper.GetEntityMapper(typeof(T));
        }

        /// <summary>
        /// Define which property will not be mapped to document
        /// </summary>
        public EntityBuilder<T> Ignore<K>(Expression<Func<T, K>> property)
        {
            return this.GetProperty(property, (p) =>
            {
                _entity.Members.Remove(p);
            });
        }

        /// <summary>
        /// Define a custom name for a property when mapping to document
        /// </summary>
        public EntityBuilder<T> Field<K>(Expression<Func<T, K>> property, string field)
        {
            if (field.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(field));

            return this.GetProperty(property, (p) =>
            {
                p.FieldName = field;
            });
        }

        /// <summary>
        /// Define which property is your document id (primary key). Define if this property supports auto-id
        /// </summary>
        public EntityBuilder<T> Id<K>(Expression<Func<T, K>> property, bool autoId = true)
        {
            return this.GetProperty(property, (p) =>
            {
                p.FieldName = "_id";
                p.AutoId = autoId;
            });
        }

        /// <summary>
        /// Get a property based on a expression. Eg.: 'x => x.UserId' return string "UserId"
        /// </summary>
        private EntityBuilder<T> GetProperty<TK, K>(Expression<Func<TK, K>> property, Action<MemberMapper> action)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var prop = _entity.GetMember(property);

            if (prop == null) throw new ArgumentNullException(property.GetPath());

            action(prop);

            return this;
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Class to map entity class to BsonDocument
    /// </summary>
    public class EntityMapper
    {
        /// <summary>
        /// List all type members that will be mapped to/from BsonDocument
        /// </summary>
        public List<MemberMapper> Members { get; set; }

        /// <summary>
        /// Indicate which member is _id
        /// </summary>
        public MemberMapper Id { get { return this.Members.SingleOrDefault(x => x.FieldName == "_id"); } }

        /// <summary>
        /// Indicate which Type this entity mapper is
        /// </summary>
        public Type ForType { get; set; }

        /// <summary>
        /// Resolve expression to get member mapped
        /// </summary>
        public MemberMapper GetMember(Expression expr)
        {
            return this.Members.FirstOrDefault(x => x.MemberName == expr.GetPath());
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Internal representation for a .NET member mapped to BsonDocument
    /// </summary>
    public class MemberMapper
    {
        /// <summary>
        /// If member is Id, indicate that are AutoId
        /// </summary>
        public bool AutoId { get; set; }

        /// <summary>
        /// Member name
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// Member returns data type
        /// </summary>
        public Type DataType { get; set; }

        /// <summary>
        /// Converted document field name
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Delegate method to get value from entity instance
        /// </summary>
        public GenericGetter Getter { get; set; }

        /// <summary>
        /// Delegate method to set value to entity instance
        /// </summary>
        public GenericSetter Setter { get; set; }

        /// <summary>
        /// When used, can be define a serialization function from entity class to bson value
        /// </summary>
        public Func<object, BsonMapper, BsonValue> Serialize { get; set; }

        /// <summary>
        /// When used, can define a deserialization function from bson value
        /// </summary>
        public Func<BsonValue, BsonMapper, object> Deserialize { get; set; }

        /// <summary>
        /// Indicate that this property contains an list of elements (IEnumerable)
        /// </summary>
        public bool IsList { get; set; }

        /// <summary>
        /// When property is an array of items, gets underlying type (otherwise is same type of PropertyType)
        /// </summary>
        public Type UnderlyingType { get; set; }
    }
}

#endregion
#pragma warning restore