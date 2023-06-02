/*
JSON 

Serialize et deserialze objet ... string ( Attention pas binaire)

site : https://www.codeproject.com/Articles/159450/fastJSON-Smallest-Fastest-Polymorphic-JSON-Seriali
used in Uneral Engine
Recup le 10/09/2022 ver :  2.4.0.2

Exemples : 
// to serialize an object to string
string jsonText = fastJSON.JSON.Instance.ToJSON(c);

// to deserialize a string to an object
var newobj = fastJSON.JSON.Instance.ToObject(jsonText);

Not Optimised => 4402lines
*/
namespace RitaEngine.Base.IO.Formats.JSON;


#pragma warning disable
using System.Text; //Formater.cs , Reflection.cs, JsonParser.cs, JsonSerializer.cs
using System; // Getter.cs , Helper.cs , Reflection.cs , JsonParser.cs, JsonSerializer.cs
using System.Collections.Generic;// Getter.cs, Helper.cs , Reflection.cs, JsonParser.cs, JsonSerializer.cs
using System.Collections.Specialized; // Helper.Cs , Reflection.cs, JsonParser.cs
using System.Reflection.Emit; // Reflection.cs
using System.Reflection; // Reflection.cs, JsonParser.cs, JsonSerializer.cs
using System.Collections; // Reflection.cs
// using System.Runtime.Serialization;// Reflection.cs, JsonSerializer.cs
using System.Data;// Reflection.cs, JsonParser.cs, JsonSerializer.cs
using System.Runtime.CompilerServices;// SafeDictionary.cs
using System.Globalization;// , JsonParser.cs, JsonSerializer.cs
using System.IO; // , JsonSerializer.cs

using fastJSON = RitaEngine.Base.IO.Formats.JSON;

#region FILE_SAFEDICTIONARY.cs
    public class ReferenceEqualityComparer : IEqualityComparer, IEqualityComparer<object>
    {
        public static ReferenceEqualityComparer Default { get; } = new ReferenceEqualityComparer();

        public new bool Equals(object? x, object? y) => x!.Equals(y);
        public int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj); 
    }

    public sealed class SafeDictionary<TKey, TValue> where TKey : notnull
    {
        private readonly object _Padlock = new object();
        private readonly Dictionary<TKey, TValue> _Dictionary;

        public SafeDictionary(int capacity) =>          _Dictionary = new Dictionary<TKey, TValue>(capacity);

        public SafeDictionary() => _Dictionary = new Dictionary<TKey, TValue>();

        public bool TryGetValue(TKey key, out TValue value)  { lock (_Padlock) return _Dictionary.TryGetValue(key, out value!); }

        public int Count(){ lock (_Padlock) return _Dictionary.Count;  }

        public TValue this[TKey key] {
            get { lock (_Padlock) return _Dictionary[key];  }
            set { lock (_Padlock) _Dictionary[key] = value; }
        }

        public void Add(TKey key, TValue value)
        {
            lock (_Padlock)
            {
                if (_Dictionary.ContainsKey(key) == false)
                    _Dictionary.Add(key, value);
            }
        }
    }

#endregion

#region FILE_REFLECTION.cs
    public struct Getters
    {
        public string Name;
        public string lcName;
        public string memberName;
        public Reflection.GenericGetter Getter;
        public bool ReadOnly;
    }

    public enum myPropInfoType
    {
        Int,
        Long,
        String,
        Bool,
        DateTime,
        Enum,
        Guid,

        Array,
        ByteArray,
        Dictionary,
        StringKeyDictionary,
        NameValue,
        StringDictionary,

        Hashtable,
        DataSet,
        DataTable,

        Custom,
        Unknown,
    }

    public class myPropInfo
    {
        public Type pt = null!;
        public Type bt= null!;
        public Type changeType= null!;
        public Reflection.GenericSetter setter= null!;
        public Reflection.GenericGetter getter= null!;
        public Type[] GenericTypes= null!;
        public string Name= null!;
        public string memberName= null!;
        public myPropInfoType Type;
        public bool CanWrite;

        public bool IsClass;
        public bool IsValueType;
        public bool IsGenericType;
        public bool IsStruct;
        public bool IsInterface;
    }

    public sealed class Reflection
    {
        // Singleton pattern 4 from : http://csharpindepth.com/articles/general/singleton.aspx
        private static readonly Reflection instance = new Reflection();
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Reflection()  { }
        private Reflection() { }
        public static Reflection Instance { get { return instance; } }

        public static bool RDBMode = false;

        public delegate string Serialize(object data);
        public delegate object Deserialize(string data);

        public delegate object GenericSetter(object target, object value);
        public delegate object GenericGetter(object obj);
        private delegate object CreateObject();
        private delegate object CreateList(int capacity);

        private SafeDictionary<Type, string> _tyname = new SafeDictionary<Type, string>(10);
        private SafeDictionary<string, Type> _typecache = new SafeDictionary<string, Type>(10);
        private SafeDictionary<Type, CreateObject> _constrcache = new SafeDictionary<Type, CreateObject>(10);
        private SafeDictionary<Type, CreateList> _conlistcache = new SafeDictionary<Type, CreateList>(10);
        private SafeDictionary<Type, Getters[]> _getterscache = new SafeDictionary<Type, Getters[]>(10);
        private SafeDictionary<string, Dictionary<string, myPropInfo>> _propertycache = new SafeDictionary<string, Dictionary<string, myPropInfo>>(10);
        private SafeDictionary<Type, Type[]> _genericTypes = new SafeDictionary<Type, Type[]>(10);
        private SafeDictionary<Type, Type> _genericTypeDef = new SafeDictionary<Type, Type>(10);
        private static SafeDictionary<short, OpCode> _opCodes= null!;
        private static List<string> _badlistTypes = new List<string>()
        {
            "system.configuration.install.assemblyinstaller",
            "system.activities.presentation.workflowdesigner",
            "system.windows.resourcedictionary",
            "system.windows.data.objectdataprovider",
            "system.windows.forms.bindingsource",
            "microsoft.exchange.management.systemmanager.winforms.exchangesettingsprovider"
        };

        private static bool TryGetOpCode(short code, out OpCode opCode)
        {
            if (_opCodes != null)
                return _opCodes.TryGetValue(code, out opCode);
            var dict = new SafeDictionary<short, OpCode>();
            foreach (var fi in typeof(OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (!typeof(OpCode).IsAssignableFrom(fi.FieldType)) continue;
                var innerOpCode = (OpCode)fi.GetValue(null);
                if (innerOpCode.OpCodeType != OpCodeType.Nternal)
                    dict.Add(innerOpCode.Value, innerOpCode);
            }
            _opCodes = dict;
            return _opCodes.TryGetValue(code, out opCode);
        }

        #region bjson custom types
        //internal UnicodeEncoding unicode = new UnicodeEncoding();
        private static UTF8Encoding utf8 = new UTF8Encoding();

        // TODO : optimize utf8 
        public static byte[] UTF8GetBytes(string str) => utf8.GetBytes(str);

        public static string UTF8GetString(byte[] bytes, int offset, int len)
            => utf8.GetString(bytes, offset, len);

        public unsafe static byte[] UnicodeGetBytes(string str)
        {
            int len = str.Length * 2;
            byte[] b = new byte[len];

            fixed (void* ptr = str) {
                System.Runtime.InteropServices.Marshal.Copy(new IntPtr(ptr), b, 0, len);
            }
            return b;
        }

        public static string UnicodeGetString(byte[] b) =>UnicodeGetString(b, 0, b.Length);

        public unsafe static string UnicodeGetString(byte[] bytes, int offset, int buflen)
        {
            string str = "";
            fixed (byte* bptr = bytes)
            {
                char* cptr = (char*)(bptr + offset);
                str = new string(cptr, 0, buflen / 2);
            }
            return str;
        }
        #endregion

        #region json custom types
        // JSON custom
        internal SafeDictionary<Type, Serialize> _customSerializer = new SafeDictionary<Type, Serialize>();
        internal SafeDictionary<Type, Deserialize> _customDeserializer = new SafeDictionary<Type, Deserialize>();

        internal object CreateCustom(string v, Type type)
        {
            Deserialize d;
            _customDeserializer.TryGetValue(type, out d);
            return d(v);
        }

        internal void RegisterCustomType(Type type, Serialize serializer, Deserialize deserializer)
        {
            if (type != null && serializer != null && deserializer != null)
            {
                _customSerializer.Add(type, serializer);
                _customDeserializer.Add(type, deserializer);
                // reset property cache
                Instance.ResetPropertyCache();
            }
        }

        internal bool IsTypeRegistered(Type t)
        {
            if (_customSerializer.Count() == 0)
                return false;
            Serialize s;
            return _customSerializer.TryGetValue(t, out s);
        }
        #endregion

        public Type GetGenericTypeDefinition(Type t)
        {
            Type tt = null;
            if (_genericTypeDef.TryGetValue(t, out tt))
                return tt;
            else
            {
                tt = t.GetGenericTypeDefinition();
                _genericTypeDef.Add(t, tt);
                return tt;
            }
        }

        public Type[] GetGenericArguments(Type t)
        {
            Type[] tt = null;
            if (_genericTypes.TryGetValue(t, out tt))
                return tt;
            else
            {
                tt = t.GetGenericArguments();
                _genericTypes.Add(t, tt);
                return tt;
            }
        }

        public Dictionary<string, myPropInfo> Getproperties(Type type, string typename, bool ShowReadOnlyProperties)
        {
            Dictionary<string, myPropInfo> sd = null;
            if (_propertycache.TryGetValue(typename, out sd)) return sd;

            sd = new Dictionary<string, myPropInfo>(10);
            var bf = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
            PropertyInfo[] pr = type.GetProperties(bf);
            foreach (PropertyInfo p in pr)
            {
                if (p.GetIndexParameters().Length > 0)// Property is an indexer
                    continue;

                myPropInfo d = CreateMyProp(p.PropertyType, p.Name);
                d.setter = Reflection.CreateSetMethod(type, p, ShowReadOnlyProperties);
                if (d.setter != null)
                    d.CanWrite = true;
                d.getter = Reflection.CreateGetMethod(type, p);
                var att = p.GetCustomAttributes(true);
                foreach (var at in att)
                {
                    if (at is fastJSON.DataMemberAttribute)
                    {
                        var dm = (fastJSON.DataMemberAttribute)at;
                        if (dm.Name != "")
                            d.memberName = dm.Name;
                    }
                }

                sd.Add(d.memberName != null ?  p.Name.ToLowerInvariant() :  p.Name, d);
            }
            FieldInfo[] fi = type.GetFields(bf);
            foreach (FieldInfo f in fi)
            {
                myPropInfo d = CreateMyProp(f.FieldType, f.Name);
                if (f.IsLiteral == false)
                {
                    if (f.IsInitOnly == false)
                        d.setter = Reflection.CreateSetField(type, f);
                    if (d.setter != null)
                        d.CanWrite = true;
                    d.getter = Reflection.CreateGetField(type, f);
                    var att = f.GetCustomAttributes(true);
                    foreach (var at in att)
                    {

                        if (at is fastJSON.DataMemberAttribute)
                        {
                            var dm = (fastJSON.DataMemberAttribute)at;
                            if (dm.Name != "")
                                d.memberName = dm.Name;
                        }
                    }
                    sd.Add(d.memberName != null ? d.memberName: f.Name.ToLowerInvariant()  , d);
                }
            }

            _propertycache.Add(typename, sd);
            return sd;
        
        }

        private myPropInfo CreateMyProp(Type t, string name)
        {
            myPropInfo d = new myPropInfo();
            myPropInfoType d_type = myPropInfoType.Unknown;

            if (t == typeof(int) || t == typeof(int?)) d_type = myPropInfoType.Int;
            else if (t == typeof(long) || t == typeof(long?)) d_type = myPropInfoType.Long;
            else if (t == typeof(string)) d_type = myPropInfoType.String;
            else if (t == typeof(bool) || t == typeof(bool?)) d_type = myPropInfoType.Bool;
            else if (t == typeof(DateTime) || t == typeof(DateTime?)) d_type = myPropInfoType.DateTime;
            else if (t.IsEnum) d_type = myPropInfoType.Enum;
            else if (t == typeof(Guid) || t == typeof(Guid?)) d_type = myPropInfoType.Guid;
            else if (t == typeof(StringDictionary)) d_type = myPropInfoType.StringDictionary;
            else if (t == typeof(NameValueCollection)) d_type = myPropInfoType.NameValue;
            else if (t.IsArray)
            {
                d.bt = t.GetElementType()!;
                if (t == typeof(byte[]))
                    d_type = myPropInfoType.ByteArray;
                else
                    d_type = myPropInfoType.Array;
            }
            else if (t.Name.Contains("Dictionary"))
            {
                d.GenericTypes = Reflection.Instance.GetGenericArguments(t);
                if (d.GenericTypes.Length > 0 && d.GenericTypes[0] == typeof(string))
                    d_type = myPropInfoType.StringKeyDictionary;
                else
                    d_type = myPropInfoType.Dictionary;
            }
            else if (t == typeof(Hashtable)) d_type = myPropInfoType.Hashtable;
            else if (t == typeof(DataSet)) d_type = myPropInfoType.DataSet;
            else if (t == typeof(DataTable)) d_type = myPropInfoType.DataTable;
            else if (IsTypeRegistered(t))
                d_type = myPropInfoType.Custom;

            if (t.IsValueType && !t.IsPrimitive && !t.IsEnum && t != typeof(decimal))
                d.IsStruct = true;

            d.IsInterface = t.IsInterface;
            d.IsClass = t.IsClass;
            d.IsValueType = t.IsValueType;
            if (t.IsGenericType)
            {
                d.IsGenericType = true;
                d.bt = Reflection.Instance.GetGenericArguments(t)[0];
            }

            d.pt = t;
            d.Name = name;
            d.changeType = GetChangeType(t);
            d.Type = d_type;

            return d;
        }

        private Type GetChangeType(Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                return Reflection.Instance.GetGenericArguments(conversionType)[0];

            return conversionType;
        }

        #region [   PROPERTY GET SET   ]

        public string GetTypeAssemblyName(Type t)
        {
            string val = "";
            if (_tyname.TryGetValue(t, out val))
                return val;
            else
            {
                string s = t.AssemblyQualifiedName!;
                _tyname.Add(t, s);
                return s;
            }
        }

        internal Type GetTypeFromCache(string typename, bool badlistChecking)
        {
            Type val = null!;
            if (_typecache.TryGetValue(typename, out val))
                return val;
            else
            {
                // check for BLACK LIST types -> more secure when using $type
                if (badlistChecking)
                {
                    var tn = typename.Trim().ToLowerInvariant();
                    foreach (var s in _badlistTypes)
                        if (tn.StartsWith(s, StringComparison.Ordinal))
                            throw new Exception("Black list type encountered, possible attack vector when using $type : " + typename);
                }

                Type t = Type.GetType(typename)!;

                _typecache.Add(typename, t);
                return t;
            }
        }

        internal object FastCreateList(Type objtype, int capacity)
        {
            try
            {
                int count = 10;
                if (capacity > 10)
                    count = capacity;
                CreateList c = null;
                if (_conlistcache.TryGetValue(objtype, out c))
                {
                    if (c != null) // kludge : non capacity lists
                        return c(count);
                    else
                        return FastCreateInstance(objtype);
                }
                else
                {
                    var cinfo = objtype.GetConstructor(new Type[] { typeof(int) });
                    if (cinfo != null)
                    {
                        DynamicMethod dynMethod = new DynamicMethod("_fcil", objtype, new Type[] { typeof(int) }, true);
                        ILGenerator ilGen = dynMethod.GetILGenerator();
                        ilGen.Emit(OpCodes.Ldarg_0);
                        ilGen.Emit(OpCodes.Newobj, objtype.GetConstructor(new Type[] { typeof(int) }));
                        ilGen.Emit(OpCodes.Ret);
                        c = (CreateList)dynMethod.CreateDelegate(typeof(CreateList));
                        _conlistcache.Add(objtype, c);
                        return c(count);
                    }
                    else
                    {
                        _conlistcache.Add(objtype, null);// kludge : non capacity lists
                        return FastCreateInstance(objtype);
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Failed to fast create instance for type '{0}' from assembly '{1}'",
                    objtype.FullName, objtype.AssemblyQualifiedName), exc);
            }
        }

        internal object FastCreateInstance(Type objtype)
        {
            try
            {
                CreateObject c = null;
                if (_constrcache.TryGetValue(objtype, out c))  return c();
                

                if (objtype.IsClass)
                {
                    DynamicMethod dynMethod = new DynamicMethod("_fcic", objtype, null, true);
                    ILGenerator ilGen = dynMethod.GetILGenerator();
                    ilGen.Emit(OpCodes.Newobj, objtype.GetConstructor(Type.EmptyTypes));
                    ilGen.Emit(OpCodes.Ret);
                    c = (CreateObject)dynMethod.CreateDelegate(typeof(CreateObject));
                    _constrcache.Add(objtype, c);
                }
                else // structs
                {
                    DynamicMethod dynMethod = new DynamicMethod("_fcis", typeof(object), null, true);
                    ILGenerator ilGen = dynMethod.GetILGenerator();
                    var lv = ilGen.DeclareLocal(objtype);
                    ilGen.Emit(OpCodes.Ldloca_S, lv);
                    ilGen.Emit(OpCodes.Initobj, objtype);
                    ilGen.Emit(OpCodes.Ldloc_0);
                    ilGen.Emit(OpCodes.Box, objtype);
                    ilGen.Emit(OpCodes.Ret);
                    c = (CreateObject)dynMethod.CreateDelegate(typeof(CreateObject));
                    _constrcache.Add(objtype, c);
                }
                return c();
            
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Failed to fast create instance for type '{0}' from assembly '{1}'",
                    objtype.FullName, objtype.AssemblyQualifiedName), exc);
            }
        }

        internal static GenericSetter CreateSetField(Type type, FieldInfo fieldInfo)
        {
            Type[] arguments = new Type[2];
            arguments[0] = arguments[1] = typeof(object);

            DynamicMethod dynamicSet = new DynamicMethod("_csf", typeof(object), arguments, type, true);

            ILGenerator il = dynamicSet.GetILGenerator();

            if (!type.IsClass) // structs
            {
                var lv = il.DeclareLocal(type);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Unbox_Any, type);
                il.Emit(OpCodes.Stloc_0);
                il.Emit(OpCodes.Ldloca_S, lv);
                il.Emit(OpCodes.Ldarg_1);
                if (fieldInfo.FieldType.IsClass)
                    il.Emit(OpCodes.Castclass, fieldInfo.FieldType);
                else
                    il.Emit(OpCodes.Unbox_Any, fieldInfo.FieldType);
                il.Emit(OpCodes.Stfld, fieldInfo);
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Box, type);
                il.Emit(OpCodes.Ret);
            }
            else
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                if (fieldInfo.FieldType.IsValueType)
                    il.Emit(OpCodes.Unbox_Any, fieldInfo.FieldType);
                il.Emit(OpCodes.Stfld, fieldInfo);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ret);
            }
            return (GenericSetter)dynamicSet.CreateDelegate(typeof(GenericSetter));
        }

        internal static FieldInfo GetGetterBackingField(PropertyInfo autoProperty)
        {
            var getMethod = autoProperty.GetGetMethod();
            // Restrict operation to auto properties to avoid risking errors if a getter does not contain exactly one field read instruction (such as with calculated properties).
            if (!getMethod.IsDefined(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false)) return null;

            var byteCode = getMethod.GetMethodBody()?.GetILAsByteArray() ?? new byte[0];
            //var byteCode = getMethod.GetMethodBody().GetILAsByteArray();
            int pos = 0;
            // Find the first LdFld instruction and parse its operand to a FieldInfo object.
            while (pos < byteCode.Length)
            {
                // Read and parse the OpCode (it can be 1 or 2 bytes in size).
                byte code = byteCode[pos++];
                if (!(TryGetOpCode(code, out var opCode) || pos < byteCode.Length && TryGetOpCode((short)(code * 0x100 + byteCode[pos++]), out opCode)))
                    throw new NotSupportedException("Unknown IL code detected.");
                // If it is a LdFld, read its operand, parse it to a FieldInfo and return it.
                if (opCode == OpCodes.Ldfld && opCode.OperandType == OperandType.InlineField && pos + sizeof(int) <= byteCode.Length)
                {
                    return getMethod.Module.ResolveMember(BitConverter.ToInt32(byteCode, pos), getMethod.DeclaringType?.GetGenericArguments(), null) as FieldInfo;
                }
                // Otherwise, set the current position to the start of the next instruction, if any (we need to know how much bytes are used by operands).
                pos += opCode.OperandType == OperandType.InlineNone
                            ? 0
                            : opCode.OperandType == OperandType.ShortInlineBrTarget ||
                              opCode.OperandType == OperandType.ShortInlineI ||
                              opCode.OperandType == OperandType.ShortInlineVar
                                ? 1
                                : opCode.OperandType == OperandType.InlineVar
                                    ? 2
                                    : opCode.OperandType == OperandType.InlineI8 ||
                                      opCode.OperandType == OperandType.InlineR
                                        ? 8
                                        : opCode.OperandType == OperandType.InlineSwitch
                                            ? 4 * (BitConverter.ToInt32(byteCode, pos) + 1)
                                            : 4;
            }
            return null;
        }

        internal static GenericSetter CreateSetMethod(Type type, PropertyInfo propertyInfo, bool ShowReadOnlyProperties)
        {
            MethodInfo setMethod = propertyInfo.GetSetMethod(ShowReadOnlyProperties);
            if (setMethod == null)
            {
                if (!ShowReadOnlyProperties) return null;
                // If the property has no setter and it is an auto property, try and create a setter for its backing field instead 
                var fld = GetGetterBackingField(propertyInfo);
                return fld != null ? CreateSetField(type, fld) : null;
            }

            Type[] arguments = new Type[2];
            arguments[0] = arguments[1] = typeof(object);

            DynamicMethod setter = new DynamicMethod("_csm", typeof(object), arguments, true);// !setMethod.IsPublic); // fix: skipverify
            ILGenerator il = setter.GetILGenerator();

            if (!type.IsClass) // structs
            {
                var lv = il.DeclareLocal(type);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Unbox_Any, type);
                il.Emit(OpCodes.Stloc_0);
                il.Emit(OpCodes.Ldloca_S, lv);
                il.Emit(OpCodes.Ldarg_1);
                if (propertyInfo.PropertyType.IsClass)
                    il.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
                else
                    il.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
                il.EmitCall(OpCodes.Call, setMethod, null);
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Box, type);
            }
            else
            {
                if (!setMethod.IsStatic)
                {
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
                    il.Emit(OpCodes.Ldarg_1);
                    if (propertyInfo.PropertyType.IsClass)
                        il.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
                    else
                        il.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
                    il.EmitCall(OpCodes.Callvirt, setMethod, null);
                    il.Emit(OpCodes.Ldarg_0);
                }
                else
                {
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldarg_1);
                    if (propertyInfo.PropertyType.IsClass)
                        il.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
                    else
                        il.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
                    il.Emit(OpCodes.Call, setMethod);
                }
            }

            il.Emit(OpCodes.Ret);

            return (GenericSetter)setter.CreateDelegate(typeof(GenericSetter));
        }

        internal static GenericGetter CreateGetField(Type type, FieldInfo fieldInfo)
        {
            DynamicMethod dynamicGet = new DynamicMethod("_cgf", typeof(object), new Type[] { typeof(object) }, type, true);

            ILGenerator il = dynamicGet.GetILGenerator();

            if (!type.IsClass) // structs
            {
                var lv = il.DeclareLocal(type);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Unbox_Any, type);
                il.Emit(OpCodes.Stloc_0);
                il.Emit(OpCodes.Ldloca_S, lv);
                il.Emit(OpCodes.Ldfld, fieldInfo);
                if (fieldInfo.FieldType.IsValueType)
                    il.Emit(OpCodes.Box, fieldInfo.FieldType);
            }
            else
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, fieldInfo);
                if (fieldInfo.FieldType.IsValueType)
                    il.Emit(OpCodes.Box, fieldInfo.FieldType);
            }

            il.Emit(OpCodes.Ret);

            return (GenericGetter)dynamicGet.CreateDelegate(typeof(GenericGetter));
        }

        internal static GenericGetter CreateGetMethod(Type type, PropertyInfo propertyInfo)
        {
            MethodInfo getMethod = propertyInfo.GetGetMethod();
            if (getMethod == null)
                return null;

            DynamicMethod getter = new DynamicMethod("_cgm", typeof(object), new Type[] { typeof(object) }, type, true);

            ILGenerator il = getter.GetILGenerator();

            if (!type.IsClass) // structs
            {
                var lv = il.DeclareLocal(type);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Unbox_Any, type);
                il.Emit(OpCodes.Stloc_0);
                il.Emit(OpCodes.Ldloca_S, lv);
                il.EmitCall(OpCodes.Call, getMethod, null);
                if (propertyInfo.PropertyType.IsValueType)
                    il.Emit(OpCodes.Box, propertyInfo.PropertyType);
            }
            else
            {
                if (!getMethod.IsStatic)
                {
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
                    il.EmitCall(OpCodes.Callvirt, getMethod, null);
                }
                else
                    il.Emit(OpCodes.Call, getMethod);

                if (propertyInfo.PropertyType.IsValueType)
                    il.Emit(OpCodes.Box, propertyInfo.PropertyType);
            }

            il.Emit(OpCodes.Ret);

            return (GenericGetter)getter.CreateDelegate(typeof(GenericGetter));
        }

        public Getters[] GetGetters(Type type, /*bool ShowReadOnlyProperties,*/ List<Type> IgnoreAttributes)
        {
            Getters[] val = null;
            if (_getterscache.TryGetValue(type, out val))
                return val;

            var bf = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
            if (type.IsClass == false)
                bf = BindingFlags.Public | BindingFlags.Instance;
            //if (ShowReadOnlyProperties)
            //    bf |= BindingFlags.NonPublic;
            PropertyInfo[] props = type.GetProperties(bf);
            List<Getters> getters = new List<Getters>();
            foreach (PropertyInfo p in props)
            {
                bool read_only = false;
                if (p.GetIndexParameters().Length > 0)
                {// Property is an indexer
                    continue;
                }
                if (!p.CanWrite)// && (ShowReadOnlyProperties == false))//|| isAnonymous == false))
                    read_only = true; //continue;
                if (IgnoreAttributes != null)
                {
                    bool found = false;
                    foreach (var ignoreAttr in IgnoreAttributes)
                    {
                        if (p.IsDefined(ignoreAttr, false))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                        continue;
                }
                string mName = null;
                var att = p.GetCustomAttributes(true);
                foreach (var at in att)
                {

                    if (at is fastJSON.DataMemberAttribute)
                    {
                        var dm = (fastJSON.DataMemberAttribute)at;
                        if (dm.Name != "")
                        {
                            mName = dm.Name;
                        }
                    }
                }
                GenericGetter g = CreateGetMethod(type, p);
                if (g != null)
                    getters.Add(new Getters { Getter = g, Name = p.Name, lcName = p.Name.ToLowerInvariant(), memberName = mName, ReadOnly = read_only });
            }

            FieldInfo[] fi = type.GetFields(bf);
            foreach (var f in fi)
            {
                bool read_only = false;
                if (f.IsInitOnly) // && (ShowReadOnlyProperties == false))//|| isAnonymous == false))
                    read_only = true;//continue;
                if (IgnoreAttributes != null)
                {
                    bool found = false;
                    foreach (var ignoreAttr in IgnoreAttributes)
                    {
                        if (f.IsDefined(ignoreAttr, false))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                        continue;
                }
                string mName = null;
                var att = f.GetCustomAttributes(true);
                foreach (var at in att)
                {

                    if (at is fastJSON.DataMemberAttribute)
                    {
                        var dm = (fastJSON.DataMemberAttribute)at;
                        if (dm.Name != "")
                        {
                            mName = dm.Name;
                        }
                    }
                }
                if (f.IsLiteral == false)
                {
                    GenericGetter g = CreateGetField(type, f);
                    if (g != null)
                        getters.Add(new Getters { Getter = g, Name = f.Name, lcName = f.Name.ToLowerInvariant(), memberName = mName, ReadOnly = read_only });
                }
            }
            val = getters.ToArray();
            _getterscache.Add(type, val);
            return val;
        }

        #endregion

        internal void ResetPropertyCache() => _propertycache = new SafeDictionary<string, Dictionary<string, myPropInfo>>();

        internal void ClearReflectionCache()
        {
            _tyname = new SafeDictionary<Type, string>(10);
            _typecache = new SafeDictionary<string, Type>(10);
            _constrcache = new SafeDictionary<Type, CreateObject>(10);
            _getterscache = new SafeDictionary<Type, Getters[]>(10);
            _propertycache = new SafeDictionary<string, Dictionary<string, myPropInfo>>(10);
            _genericTypes = new SafeDictionary<Type, Type[]>(10);
            _genericTypeDef = new SafeDictionary<Type, Type>(10);
        }
    }

#endregion

#region FILE_HELPER.CS
    public class Helper
    {
        public static bool IsNullable(Type t)
        {
            if (!t.IsGenericType) return false;
            Type g = t.GetGenericTypeDefinition();
            return (g.Equals(typeof(Nullable<>)));
        }

        public static Type UnderlyingTypeOf(Type t) => Reflection.Instance.GetGenericArguments(t)[0];

        public static DateTimeOffset CreateDateTimeOffset(int year, int month, int day, int hour, int min, int sec, int milli, int extraTicks, TimeSpan offset)
        {
            var dt = new DateTimeOffset(year, month, day, hour, min, sec, milli, offset);

            return (extraTicks > 0)? dt +  TimeSpan.FromTicks(extraTicks) : dt;
        }

        public static bool BoolConv(object v)
        {
            bool oset = false;
            if (v is bool)
                oset = (bool)v;
            else if (v is long)
                oset = (long)v > 0 ? true : false;
            else if (v is string)
            {
                var s = (string)v;
                s = s.ToLowerInvariant();
                if (s == "1" || s == "true" || s == "yes" || s == "on")
                    oset = true;
            }

            return oset;
        }

        public static long AutoConv(object value, JSONParameters param)
        {
            if (value is string)
            {
                if (param.AutoConvertStringToNumbers == true)
                {
                    string s = (string)value;
                    return CreateLong(s, 0, s.Length);
                }
                else
                    throw new Exception("AutoConvertStringToNumbers is disabled for converting string : " + value);
            }
            else if (value is long)
                return (long)value;
            else
                return Convert.ToInt64(value);
        }

        public static unsafe long CreateLong(string s, int index, int count)
        {
            long num = 0;
            int neg = 1;
            fixed (char* v = s)
            {
                char* str = v;
                str += index;
                if (*str == '-')
                {
                    neg = -1;
                    str++;
                    count--;
                }
                if (*str == '+')
                {
                    str++;
                    count--;
                }
                while (count > 0)
                {
                    num = num * 10  + (*str - '0');
                    str++;
                    count--;
                }
            }
            return num * neg;
        }

        public static unsafe long CreateLong(char[] s, int index, int count)
        {
            long num = 0;
            int neg = 1;
            fixed (char* v = s)
            {
                char* str = v;
                str += index;
                if (*str == '-')
                {
                    neg = -1;
                    str++;
                    count--;
                }
                if (*str == '+')
                {
                    str++;
                    count--;
                }
                while (count > 0)
                {
                    num = num * 10  + (*str - '0');
                    str++;
                    count--;
                }
            }
            return num * neg;
        }

        public static unsafe int CreateInteger(string s, int index, int count)
        {
            int num = 0;
            int neg = 1;
            fixed (char* v = s)
            {
                char* str = v;
                str += index;
                if (*str == '-')
                {
                    neg = -1;
                    str++;
                    count--;
                }
                if (*str == '+')
                {
                    str++;
                    count--;
                }
                while (count > 0)
                {
                    num = num * 10 + (*str - '0');
                    str++;
                    count--;
                }
            }
            return num * neg;
        }


        public static object CreateEnum(Type pt, object v) => Enum.Parse(pt, v.ToString(), true);

        public static Guid CreateGuid(string s)
            => (s.Length > 30) ? new Guid(s) : new Guid(Convert.FromBase64String(s));

        public static StringDictionary CreateSD(Dictionary<string, object> d)
        {
            StringDictionary nv = new StringDictionary();

            foreach (var o in d)
                nv.Add(o.Key, (string)o.Value);

            return nv;
        }

        public static NameValueCollection CreateNV(Dictionary<string, object> d)
        {
            NameValueCollection nv = new NameValueCollection();

            foreach (var o in d)
                nv.Add(o.Key, (string)o.Value);

            return nv;
        }

        public static object CreateDateTimeOffset(string value)
        {
            int year;       int month;         int day;
            int hour;        int min;        int sec;         int ms = 0;
            int usTicks = 0; // ticks for xxx.x microseconds
            int th = 0;
            int tm = 0;

            year = CreateInteger(value, 0, 4);
            month = CreateInteger(value, 5, 2);
            day = CreateInteger(value, 8, 2);
            hour = CreateInteger(value, 11, 2);
            min = CreateInteger(value, 14, 2);
            sec = CreateInteger(value, 17, 2);

            int p = 20;

            if (value.Length > 21 && value[19] == '.')
            {
                ms = CreateInteger(value, p, 3);
                p = 23;

                // handle 7 digit case
                if (value.Length > 25 && char.IsDigit(value[p]))
                {
                    usTicks = CreateInteger(value, p, 4);
                    p = 27;
                }
            }

            if (value[p] == 'Z')
                // UTC
                return CreateDateTimeOffset(year, month, day, hour, min, sec, ms, usTicks, TimeSpan.Zero);

            if (value[p] == ' ')
                ++p;

            // +00:00
            th = CreateInteger(value, p + 1, 2);
            tm = CreateInteger(value, p + 1 + 2 + 1, 2);

            if (value[p] == '-')
                th = -th;

            return CreateDateTimeOffset(year, month, day, hour, min, sec, ms, usTicks, new TimeSpan(th, tm, 0));
        }

        public static DateTime CreateDateTime(string value, bool UseUTCDateTime)
        {
            if (value.Length < 19)
                return DateTime.MinValue;

            bool utc = false;
            //                   0123456789012345678 9012 9/3
            // datetime format = yyyy-MM-ddTHH:mm:ss .nnn  Z
            int year; int month; int day;
            int hour;  int min;   int sec;  int ms = 0;

            year = CreateInteger(value, 0, 4);
            month = CreateInteger(value, 5, 2);
            day = CreateInteger(value, 8, 2);
            hour = CreateInteger(value, 11, 2);
            min = CreateInteger(value, 14, 2);
            sec = CreateInteger(value, 17, 2);
            if (value.Length > 21 && value[19] == '.')
                ms = CreateInteger(value, 20, 3);

            if (value[value.Length - 1] == 'Z')
                utc = true;

            if (UseUTCDateTime == false && utc == false)
                return new DateTime(year, month, day, hour, min, sec, ms);
            else if (UseUTCDateTime && utc)
                return new DateTime(year, month, day, hour, min, sec, ms, DateTimeKind.Utc);
            else
                return new DateTime(year, month, day, hour, min, sec, ms, DateTimeKind.Utc).ToLocalTime();
        }

    }

#endregion

#region FILE_FORMATTER.CS    
    /// <summary>
    /// 
    /// </summary>
    internal static class Formatter
    {

        private static void AppendIndent(StringBuilder sb, int count, string indent)
        {
            for (; count > 0; --count) sb.Append(indent);
        }

        public static string PrettyPrint(string input)
        => PrettyPrint(input, new string(' ', JSON.Parameters.FormatterIndentSpaces));// "   ");
        
        public static string PrettyPrint(string input, string spaces)
        {
            var output = new StringBuilder();
            int depth = 0;
            int len = input.Length;
            char[] chars = input.ToCharArray();
            for (int i = 0; i < len; ++i)
            {
                char ch = chars[i];

                if (ch == '\"') // found string span
                {
                    bool str = true;
                    while (str)
                    {
                        output.Append(ch);
                        ch = chars[++i];
                        if (ch == '\\')
                        {
                            output.Append(ch);
                            ch = chars[++i];
                        }
                        else if (ch == '\"')
                            str = false;
                    }
                }

                switch (ch)
                {
                    case '{':
                    case '[':
                        output.Append(ch);
                        output.AppendLine();
                        AppendIndent(output, ++depth, spaces);
                        break;
                    case '}':
                    case ']':
                        output.AppendLine();
                        AppendIndent(output, --depth, spaces);
                        output.Append(ch);
                        break;
                    case ',':
                        output.Append(ch);
                        output.AppendLine();
                        AppendIndent(output, depth, spaces);
                        break;
                    case ':':
                        output.Append(" : ");
                        break;
                    default:
                        if (!char.IsWhiteSpace(ch))
                            output.Append(ch);
                        break;
                }
            }

            return output.ToString();
        }
    }

#endregion

#region FILE_GETTER.CS
    public sealed class DatasetSchema
    {
        public List<string> Info ;//{ get; set; }
        public string Name ;//{ get; set; }
    }

    /// <summary>
    /// DataMember attribute clone for .net v2 v3.5
    /// </summary>
    public class DataMemberAttribute : Attribute
    {
        public string Name { get; set; }
    }
#endregion

#region FILE_JSONPARSER.cs
    /// <summary>
    /// This class encodes and decodes JSON strings.
    /// Spec. details, see http://www.json.org/
    /// </summary>
    internal sealed class JsonParser
    {
        enum Token
        {
            None = -1,           // Used to denote no Lookahead available
            Curly_Open,
            Curly_Close,
            Squared_Open,
            Squared_Close,
            Colon,
            Comma,
            String,
            Number,
            True,
            False,
            Null//, 
            //Key
            ,
            PosInfinity,
            NegInfinity,
            NaN
        }

        readonly char[] json;
        readonly StringBuilder s = new StringBuilder(); // used for inner string parsing " \"\r\n\u1234\'\t " 

        Token lookAheadToken = Token.None;
        int index;
        bool allownonquotedkey = false;
        //bool AllowJson5String = false;
        int _len = 0;
        SafeDictionary<string, bool> _lookup;
        SafeDictionary<Type, bool> _seen;
        bool _parseJsonType = false;

        internal JsonParser(string json, bool AllowNonQuotedKeys)//, bool AllowJson5String)
        {
            allownonquotedkey = AllowNonQuotedKeys;
            this.json = json.ToCharArray();
            _len = json.Length;
        }

        private void SetupLookup()
        {
            _lookup = new SafeDictionary<string, bool>();
            _seen = new SafeDictionary<Type, bool>();
            _lookup.Add("$types", true);
            _lookup.Add("$type", true);
            _lookup.Add("$i", true);
            _lookup.Add("$map", true);
            _lookup.Add("$schema", true);
            _lookup.Add("k", true);
            _lookup.Add("v", true);
        }

        public unsafe object Decode(Type objtype)
        {
            fixed (char* p = json)
            {
                if (objtype != null)
                {
                    if (CheckForTypeInJson(p) == false)
                    {
                        _parseJsonType = true;
                        SetupLookup();

                        BuildLookup(objtype);

                        // reset if no properties found
                        if (_parseJsonType == false || _lookup.Count() == 7)
                            _lookup = null;
                    }
                }
                return ParseValue(p);
            }
        }

        private unsafe bool CheckForTypeInJson(char* p)
        {
            int idx = 0;
            int len = _len > 1000 ? 1000 : _len;
            while (idx < len)
            {
                if (p[idx + 0] == '$' &&
                    p[idx + 1] == 't' &&
                    p[idx + 2] == 'y' &&
                    p[idx + 3] == 'p' &&
                    p[idx + 4] == 'e' &&
                    p[idx + 5] == 's'
                    )
                    return true;
                idx++;
            }

            return false;
        }

        private void BuildGenericTypeLookup(Type t)
        {
            if (_seen.TryGetValue(t, out bool _))
                return;

            foreach (var e in t.GetGenericArguments())
            {
                if (e.IsPrimitive)
                    continue;

                bool isstruct = e.IsValueType && !e.IsEnum;

                if ((e.IsClass || isstruct || e.IsAbstract) && e != typeof(string) && e != typeof(DateTime) && e != typeof(Guid))
                {
                    BuildLookup(e);
                }
            }
        }

        private void BuildArrayTypeLookup(Type t)
        {
            if (_seen.TryGetValue(t, out bool _))
                return;

            bool isstruct = t.IsValueType && !t.IsEnum;

            if ((t.IsClass || isstruct) && t != typeof(string) && t != typeof(DateTime) && t != typeof(Guid))
            {
                BuildLookup(t.GetElementType());
            }
        }

        private void BuildLookup(Type objtype)
        {
            // build lookup
            if (objtype == null)
                return;

            if (objtype == typeof(NameValueCollection) || objtype == typeof(StringDictionary))
                return;

            if (typeof(IDictionary).IsAssignableFrom(objtype))
                return;

            if (_seen.TryGetValue(objtype, out bool _))
                return;

            if (objtype.IsGenericType)
                BuildGenericTypeLookup(objtype);

            else if (objtype.IsArray)
            {
                Type t = objtype;
                BuildArrayTypeLookup(objtype);
            }
            else
            {
                _seen.Add(objtype, true);

                foreach (var m in Reflection.Instance.Getproperties(objtype, objtype.FullName, true))
                {
                    Type t = m.Value.pt;

                    _lookup.Add(m.Key.ToLowerInvariant(), true);

                    if (t.IsArray)
                        BuildArrayTypeLookup(t);

                    if (t.IsGenericType)
                    {
                        // skip if dictionary
                        if (typeof(IDictionary).IsAssignableFrom(t))
                        {
                            _parseJsonType = false;
                            return;
                        }
                        BuildGenericTypeLookup(t);
                    }
                    if (t.FullName.IndexOf("System.") == -1)
                        BuildLookup(t);
                }
            }
        }

        private bool InLookup(string name)
        {
            if (_lookup == null)
                return true;

            return _lookup.TryGetValue(name.ToLowerInvariant(), out bool v);
        }

        bool _parseType = false;
        private unsafe Dictionary<string, object> ParseObject(char* p)
        {
            Dictionary<string, object> obj = new Dictionary<string, object>();

            ConsumeToken(); // {

            while (true)
            {
                switch (LookAhead(p))
                {

                    case Token.Comma:
                        ConsumeToken();
                        break;

                    case Token.Curly_Close:
                        ConsumeToken();
                        return obj;

                    default:
                        // name
                        string name = ParseKey(p);

                        var n = NextToken(p);
                        // :
                        if (n != Token.Colon)
                        {
                            throw new Exception("Expected colon at index " + index);
                        }

                        if (_parseJsonType)
                        {
                            if (name == "$types")
                            {
                                _parseType = true;
                                Dictionary<string, object> types = (Dictionary<string, object>)ParseValue(p);
                                _parseType = false;
                                // parse $types 
                                // performance hit here
                                if (_lookup == null)
                                    SetupLookup();

                                foreach (var v in types.Keys)
                                    BuildLookup(Reflection.Instance.GetTypeFromCache(v, true));

                                obj[name] = types;

                                break;
                            }

                            if (name == "$schema")
                            {
                                _parseType = true;
                                var value = ParseValue(p);
                                _parseType = false;
                                obj[name] = value;
                                break;
                            }

                            if (_parseType || InLookup(name))
                                obj[name] = ParseValue(p);
                            else
                                SkipValue(p);
                        }
                        else
                        {
                            obj[name] = ParseValue(p);
                        }
                        break;
                }
            }
        }

        private unsafe void SkipValue(char* p)
        {
            // optimize skipping
            switch (LookAhead(p))
            {
                case Token.Number:
                    ParseNumber(p, true);
                    break;

                case Token.String:
                    SkipString(p);
                    break;

                case Token.Curly_Open:
                    SkipObject(p);
                    break;

                case Token.Squared_Open:
                    SkipArray(p);
                    break;

                case Token.True:
                case Token.False:
                case Token.Null:
                case Token.PosInfinity:
                case Token.NegInfinity:
                case Token.NaN:
                    ConsumeToken();
                    break;
            }
        }

        private unsafe void SkipObject(char* p)
        {
            ConsumeToken(); // {

            while (true)
            {
                switch (LookAhead(p))
                {

                    case Token.Comma:
                        ConsumeToken();
                        break;

                    case Token.Curly_Close:
                        ConsumeToken();
                        return;

                    default:
                        // name
                        SkipString(p);

                        var n = NextToken(p);
                        // :
                        if (n != Token.Colon)
                        {
                            throw new Exception("Expected colon at index " + index);
                        }
                        SkipValue(p);
                        break;
                }
            }
        }

        private unsafe void SkipArray(char* p)
        {
            ConsumeToken(); // [

            while (true)
            {
                switch (LookAhead(p))
                {
                    case Token.Comma:
                        ConsumeToken();
                        break;

                    case Token.Squared_Close:
                        ConsumeToken();
                        return;

                    default:
                        SkipValue(p);
                        break;
                }
            }
        }

        private unsafe void SkipString(char* p)
        {
            ConsumeToken();

            int len = _len;

            // escaped string
            while (index < len)
            {
                var c = p[index++];
                if (c == '"')
                    return;

                if (c == '\\')
                {
                    c = p[index++];

                    if (c == 'u')
                        index += 4;
                }
            }
        }

        private unsafe List<object> ParseArray(char* p)
        {
            List<object> array = new List<object>();
            ConsumeToken(); // [

            while (true)
            {
                switch (LookAhead(p))
                {
                    case Token.Comma:
                        ConsumeToken();
                        break;

                    case Token.Squared_Close:
                        ConsumeToken();
                        return array;

                    default:
                        array.Add(ParseValue(p));
                        break;
                }
            }
        }

        private unsafe object ParseValue(char* p)//, bool val)
        {
            switch (LookAhead(p))
            {
                case Token.Number:
                    return ParseNumber(p, false);

                case Token.String:
                    return ParseString(p);

                case Token.Curly_Open:
                    return ParseObject(p);

                case Token.Squared_Open:
                    return ParseArray(p);

                case Token.True:
                    ConsumeToken();
                    return true;

                case Token.False:
                    ConsumeToken();
                    return false;

                case Token.Null:
                    ConsumeToken();
                    return null;

                case Token.PosInfinity:
                    ConsumeToken();
                    return double.PositiveInfinity;

                case Token.NegInfinity:
                    ConsumeToken();
                    return double.NegativeInfinity;

                case Token.NaN:
                    ConsumeToken();
                    return double.NaN;
            }

            throw new Exception("Unrecognized token at index " + index);
        }

        private unsafe string ParseKey(char* p)
        {
            if (allownonquotedkey == false || p[index - 1] == '"')
                return ParseString(p);

            ConsumeToken();

            int len = _len;
            int run = 0;
            while (index + run < len)
            {
                var c = p[index + run++];

                if (c == ':')
                {
                    var str = UnsafeSubstring(p, index, run - 1).Trim();
                    index += run - 1;
                    return str;
                }
            }
            throw new Exception("Unable to read key");
        }

        private unsafe string ParseString(char* p)
        {
            char quote = p[index - 1];
            ConsumeToken();

            if (s.Length > 0)
                s.Length = 0;

            int len = _len;
            int run = 0;

            // non escaped string
            while (index + run < len)
            {
                var c = p[index + run++];
                if (c == '\\')
                    break;
                if (c == quote)//'\"')
                {
                    var str = UnsafeSubstring(p, index, run - 1);
                    index += run;
                    return str;
                }
            }

            // escaped string
            while (index < len)
            {
                var c = p[index++];
                if (c == quote)//'"')
                    return s.ToString();

                if (c != '\\')
                    s.Append(c);
                else
                {
                    c = p[index++];
                    switch (c)
                    {
                        case 'b':
                            s.Append('\b');
                            break;

                        case 'f':
                            s.Append('\f');
                            break;

                        case 'n':
                            s.Append('\n');
                            break;

                        case 'r':
                            s.Append('\r');
                            break;

                        case 't':
                            s.Append('\t');
                            break;

                        case 'u':
                            {
                                //int remainingLength = l - index;
                                //if (remainingLength < 4) break;

                                // parse the 32 bit hex into an integer codepoint
                                uint codePoint = ParseUnicode(p[index], p[index + 1], p[index + 2], p[index + 3]);
                                s.Append((char)codePoint);

                                // skip 4 chars
                                index += 4;
                            }
                            break;
                        default:
                            if (c == '\r' || c == '\n' || c == ' ' || c == '\t')
                            {
                                // json5 skip ending whitespace till new line
                                while (c == '\r' || c == '\n' || c == ' ' || c == '\t')
                                {
                                    index++;
                                    c = p[index];
                                    if (c == '\r' || c == '\n')
                                    {
                                        c = p[index + 1];
                                        if (c == '\r' || c == '\n')
                                        {
                                            index += 2;
                                            c = p[index];
                                        }
                                        break;
                                    }
                                }
                            }
                            else
                                s.Append(c);
                            break;
                    }
                }
            }


            return s.ToString();
        }

        private uint ParseSingleChar(char c1, uint multipliyer)
        {
            uint p1 = 0;
            if (c1 >= '0' && c1 <= '9')
                p1 = (uint)(c1 - '0') * multipliyer;
            else if (c1 >= 'A' && c1 <= 'F')
                p1 = (uint)((c1 - 'A') + 10) * multipliyer;
            else if (c1 >= 'a' && c1 <= 'f')
                p1 = (uint)((c1 - 'a') + 10) * multipliyer;
            return p1;
        }

        private uint ParseUnicode(char c1, char c2, char c3, char c4)
        {
            uint p1 = ParseSingleChar(c1, 0x1000);
            uint p2 = ParseSingleChar(c2, 0x100);
            uint p3 = ParseSingleChar(c3, 0x10);
            uint p4 = ParseSingleChar(c4, 1);

            return p1 + p2 + p3 + p4;
        }

        private unsafe object ParseNumber(char* p, bool skip)
        {
            ConsumeToken();

            // Need to start back one place because the first digit is also a token and would have been consumed
            var startIndex = index - 1;
            bool dec = false;
            bool dob = false;
            bool run = true;
            if (p[startIndex] == '.') dec = true;
            do
            {
                if (index == _len)
                    break;
                var c = p[index];

                if (c == 'x' || c == 'X')
                {
                    index++;
                    return ReadHexNumber(p);
                }

                switch (c)
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '+':
                    case '-':
                        index++;
                        break;
                    case 'e':
                    case 'E':
                        dob = true;
                        index++;
                        break;
                    case '.':
                        index++;
                        dec = true;
                        break;
                    case 'n':
                    case 'N':
                        index += 3;
                        return double.NaN;
                    //break;
                    default:
                        run = false;
                        break;
                }

                if (index == _len)
                    run = false;

            } while (run);

            if (skip)
                return 0;
            var len = index - startIndex;
            if (dob || len > 31)
            {
                string s = UnsafeSubstring(p, startIndex, len);
                return double.Parse(s, NumberFormatInfo.InvariantInfo);
            }
            if (dec == false && len < 20)
                return Helper.CreateLong(json, startIndex, len);
            else
            {
                string s = UnsafeSubstring(p, startIndex, len);
                return decimal.Parse(s, NumberFormatInfo.InvariantInfo);
            }
        }

        private unsafe object ReadHexNumber(char* p)
        {
            long num = 0L;
            bool run = true;
            while (run && index < _len)
            {
                char c = p[index];
                switch (c)
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        index++;
                        num = (num << 4) + (c - '0');
                        break;
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                        index++;
                        num = (num << 4) + (c - 'a') + 10;
                        break;
                    case 'A':
                    case 'B':
                    case 'C':
                    case 'D':
                    case 'E':
                    case 'F':
                        index++;
                        num = (num << 4) + (c - 'A') + 10;
                        break;
                    default:
                        run = false;
                        break;
                }
            }

            return num;
        }

        private unsafe Token LookAhead(char* p)
        {
            if (lookAheadToken != Token.None) return lookAheadToken;

            return lookAheadToken = NextTokenCore(p);
        }

        private void ConsumeToken() =>   lookAheadToken = Token.None;

        private unsafe Token NextToken(char* p)
        {
            var result = lookAheadToken != Token.None ? lookAheadToken : NextTokenCore(p);

            lookAheadToken = Token.None;

            return result;
        }

        private unsafe void SkipWhitespace(char* p)
        {
            // Skip past whitespace
            do
            {
                var c = p[index];

                if (c == '/' && p[index + 1] == '/') // c++ style single line comments
                {
                    index++;
                    index++;
                    do
                    {
                        c = p[index];
                        if (c == '\r' || c == '\n') break; // read till end of line
                    }
                    while (++index < _len);
                }

                if (c == '/' && p[index + 1] == '*') // c style multi line comments
                {
                    index++;
                    index++;
                    do
                    {
                        c = p[index];
                        if (c == '*' && p[index + 1] == '/')
                        {
                            index += 2;
                            c = p[index];
                            break; // read till end of comment
                        }
                    }
                    while (++index < _len);
                }

                if (c != ' ' && c != '\t' && c != '\n' && c != '\r')
                    break;
            } while (++index < _len);
        }

        private unsafe Token NextTokenCore(char* p)
        {
            char c;
            int len = _len;

            SkipWhitespace(p);

            if (index == len)
            {
                throw new Exception("Reached end of string unexpectedly");
            }

            c = p[index];

            index++;

            switch (c)
            {
                case '{':
                    return Token.Curly_Open;

                case '}':
                    return Token.Curly_Close;

                case '[':
                    return Token.Squared_Open;

                case ']':
                    return Token.Squared_Close;

                case ',':
                    return Token.Comma;

                case '\'':
                case '"':
                    return Token.String;

                case '-':
                    if (p[index] == 'i' || p[index] == 'I') // TODO : check all chars ??
                    {
                        index += 8;
                        return Token.NegInfinity;
                    }
                    return Token.Number;
                case '+':
                    if (p[index] == 'i' || p[index] == 'I') // TODO : check all chars ??
                    {
                        index += 8;
                        return Token.PosInfinity;
                    }
                    return Token.Number;

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '.':
                    return Token.Number;

                case ':':
                    return Token.Colon;
                case 'I':
                case 'i': // TODO : check complete infinity chars
                    index += 7;
                    return Token.PosInfinity;

                case 'f':
                    if (len - index >= 4 &&
                        p[index + 0] == 'a' &&
                        p[index + 1] == 'l' &&
                        p[index + 2] == 's' &&
                        p[index + 3] == 'e')
                    {
                        index += 4;
                        return Token.False;
                    }
                    break;

                case 't':
                    if (len - index >= 3 &&
                        p[index + 0] == 'r' &&
                        p[index + 1] == 'u' &&
                        p[index + 2] == 'e')
                    {
                        index += 3;
                        return Token.True;
                    }
                    break;

                case 'n':
                case 'N':
                    if (len - index >= 3 &&
                        p[index + 0] == 'u' &&
                        p[index + 1] == 'l' &&
                        p[index + 2] == 'l')
                    {
                        index += 3;
                        return Token.Null;
                    }
                    else if (len - index >= 2 &&
                         p[index] == 'a' &&
                         (p[index + 1] == 'n' || p[index + 1] == 'N'))
                    {
                        index += 2;
                        return Token.NaN;
                    }
                    break;
            }

            if (allownonquotedkey)//&& tok == Token.String)
            {
                index--;
                return Token.String;
            }

            //return tok;
            else
                throw new Exception("Could not find token at index " + --index + " got '" + p[index] + "'");
        }

        private static unsafe string UnsafeSubstring(char* p, int startIndex, int length)
        =>  new string(p, startIndex, length);
    }

#endregion

#region FILE_JSONSERIALIZER.cs
    internal sealed class JSONSerializer
    {
        private StringBuilder _output = new StringBuilder();
        //private StringBuilder _before = new StringBuilder();
        private int _before;
        private int _MAX_DEPTH = 20;
        int _current_depth = 0;
        private Dictionary<string, int> _globalTypes = new Dictionary<string, int>();
        private Dictionary<object, int> _cirobj;
        private JSONParameters _params;
        private bool _useEscapedUnicode = false;

        internal JSONSerializer(JSONParameters param)
        {
            _cirobj = (param.OverrideObjectHashCodeChecking) ? 
                new Dictionary<object, int>(10, ReferenceEqualityComparer.Default)
                : new Dictionary<object, int>();
            _params = param;
            _useEscapedUnicode = _params.UseEscapedUnicode;
            _MAX_DEPTH = _params.SerializerMaxDepth;
        }

        internal string ConvertToJSON(object obj)
        {
            WriteValue(obj);

            if (_params.UsingGlobalTypes && _globalTypes != null && _globalTypes.Count > 0)
            {
                var sb = new StringBuilder();
                sb.Append("\"$types\":{");
                var pendingSeparator = false;
                foreach (var kv in _globalTypes)
                {
                    if (pendingSeparator) sb.Append(',');
                    pendingSeparator = true;
                    sb.Append('\"');
                    sb.Append(kv.Key);
                    sb.Append("\":\"");
                    sb.Append(kv.Value);
                    sb.Append('\"');
                }
                sb.Append("},");
                _output.Insert(_before, sb.ToString());
            }
            return _output.ToString();
        }

        private void WriteValue(object obj)
        {
            if (obj == null || obj is DBNull)
                _output.Append("null");

            else if (obj is string || obj is char)
                WriteString(obj.ToString());

            else if (obj is Guid)
                WriteGuid((Guid)obj);

            else if (obj is bool)
                _output.Append(((bool)obj) ? "true" : "false"); // conform to standard

            else if (
                obj is int || obj is long ||
                obj is decimal ||
                obj is byte || obj is short ||
                obj is sbyte || obj is ushort ||
                obj is uint || obj is ulong
            )
                _output.Append(((IConvertible)obj).ToString(NumberFormatInfo.InvariantInfo));

            else if (obj is double || obj is Double)
            {
                double d = (double)obj;
                if (double.IsNaN(d))
                    _output.Append("\"NaN\"");
                else if (double.IsInfinity(d))
                {
                    _output.Append('\"');
                    _output.Append(((IConvertible)obj).ToString(NumberFormatInfo.InvariantInfo));
                    _output.Append('\"');
                }
                else
                    _output.Append(((IConvertible)obj).ToString(NumberFormatInfo.InvariantInfo));
            }
            else if (obj is float || obj is Single)
            {
                float d = (float)obj;
                if (float.IsNaN(d))
                    _output.Append("\"NaN\"");
                else if (float.IsInfinity(d))
                {
                    _output.Append('\"');
                    _output.Append(((IConvertible)obj).ToString(NumberFormatInfo.InvariantInfo));
                    _output.Append('\"');
                }
                else
                    _output.Append(((IConvertible)obj).ToString(NumberFormatInfo.InvariantInfo));
            }

            else if (obj is DateTime)
                WriteDateTime((DateTime)obj);

            else if (obj is DateTimeOffset)
                WriteDateTimeOffset((DateTimeOffset)obj);

            else if (obj is TimeSpan)
                _output.Append(((TimeSpan)obj).Ticks);

            else if (_params.KVStyleStringDictionary == false &&
                obj is IEnumerable<KeyValuePair<string, object>>)

                WriteStringDictionary((IEnumerable<KeyValuePair<string, object>>)obj);

            else if (_params.KVStyleStringDictionary == false && obj is IDictionary &&
                obj.GetType().IsGenericType && Reflection.Instance.GetGenericArguments(obj.GetType())[0] == typeof(string))

                WriteStringDictionary((IDictionary)obj);
            else if (obj is IDictionary)
                WriteDictionary((IDictionary)obj);

            else if (obj is DataSet)
                WriteDataset((DataSet)obj);

            else if (obj is DataTable)
                this.WriteDataTable((DataTable)obj);
            else if (obj is byte[])
                WriteBytes((byte[])obj);

            else if (obj is StringDictionary)
                WriteSD((StringDictionary)obj);

            else if (obj is NameValueCollection)
                WriteNV((NameValueCollection)obj);

            else if (obj is Array)
                WriteArrayRanked((Array)obj);

            else if (obj is IEnumerable)
                WriteArray((IEnumerable)obj);

            else if (obj is Enum)
                WriteEnum((Enum)obj);

            else if (Reflection.Instance.IsTypeRegistered(obj.GetType()))
                WriteCustom(obj);

            else
                WriteObject(obj);
        }

        private void WriteDateTimeOffset(DateTimeOffset d)
        {
            DateTime dt = _params.UseUTCDateTime ? d.UtcDateTime : d.DateTime;

            write_date_value(dt);

            var ticks = dt.Ticks % TimeSpan.TicksPerSecond;
            _output.Append('.');
            _output.Append(ticks.ToString("0000000", NumberFormatInfo.InvariantInfo));

            if (_params.UseUTCDateTime)
                _output.Append('Z');
            else
            {
                if (d.Offset.Hours > 0)
                    _output.Append('+');
                else
                    _output.Append('-');
                _output.Append(d.Offset.Hours.ToString("00", NumberFormatInfo.InvariantInfo));
                _output.Append(':');
                _output.Append(d.Offset.Minutes.ToString("00", NumberFormatInfo.InvariantInfo));
            }

            _output.Append('\"');
        }

        private void WriteNV(NameValueCollection nameValueCollection)
        {
            _output.Append('{');

            bool pendingSeparator = false;

            foreach (string key in nameValueCollection)
            {
                // if (_params.SerializeNullValues == false && (nameValueCollection[key] == null))
                // {
                // }
                // else
                if (  !( _params.SerializeNullValues == false && (nameValueCollection[key] == null) ) ) 
                {
                    if (pendingSeparator) _output.Append(',');
                    if (_params.SerializeToLowerCaseNames)
                        WritePair(key.ToLowerInvariant(), nameValueCollection[key]);
                    else
                        WritePair(key, nameValueCollection[key]);
                    pendingSeparator = true;
                }
            }
            _output.Append('}');
        }

        private void WriteSD(StringDictionary stringDictionary)
        {
            _output.Append('{');

            bool pendingSeparator = false;

            foreach (DictionaryEntry entry in stringDictionary)
            {
                if (_params.SerializeNullValues == false && (entry.Value == null))
                {
                }
                else
                {
                    if (pendingSeparator) _output.Append(',');

                    string k = (string)entry.Key;
                    if (_params.SerializeToLowerCaseNames)
                        WritePair(k.ToLowerInvariant(), entry.Value);
                    else
                        WritePair(k, entry.Value);
                    pendingSeparator = true;
                }
            }
            _output.Append('}');
        }

        private void WriteCustom(object obj)
        {
            Reflection.Serialize s;
            Reflection.Instance._customSerializer.TryGetValue(obj.GetType(), out s);
            WriteStringFast(s(obj));
        }

        private void WriteEnum(Enum e)
        {
            // FEATURE : optimize enum write
            if (_params.UseValuesOfEnums)
                WriteValue(Convert.ToInt32(e));
            else
                WriteStringFast(e.ToString());
        }

        private void WriteGuid(Guid g)
        {
            if (_params.UseFastGuid == false)
                WriteStringFast(g.ToString());
            else
                WriteBytes(g.ToByteArray());
        }

        private void WriteBytes(byte[] bytes) => WriteStringFast(Convert.ToBase64String(bytes, 0, bytes.Length, Base64FormattingOptions.None));

        private void WriteDateTime(DateTime dateTime)
        {
            // datetime format standard : yyyy-MM-dd HH:mm:ss
            DateTime dt = dateTime;
            if (_params.UseUTCDateTime)
                dt = dateTime.ToUniversalTime();

            write_date_value(dt);

            if (_params.DateTimeMilliseconds)
            {
                _output.Append('.');
                _output.Append(dt.Millisecond.ToString("000", NumberFormatInfo.InvariantInfo));
            }

            if (_params.UseUTCDateTime)
                _output.Append('Z');

            _output.Append('\"');
        }

        private void write_date_value(DateTime dt)
        {
            _output.Append('\"');
            _output.Append(dt.Year.ToString("0000", NumberFormatInfo.InvariantInfo));
            _output.Append('-');
            _output.Append(dt.Month.ToString("00", NumberFormatInfo.InvariantInfo));
            _output.Append('-');
            _output.Append(dt.Day.ToString("00", NumberFormatInfo.InvariantInfo));
            _output.Append('T'); // strict ISO date compliance 
            _output.Append(dt.Hour.ToString("00", NumberFormatInfo.InvariantInfo));
            _output.Append(':');
            _output.Append(dt.Minute.ToString("00", NumberFormatInfo.InvariantInfo));
            _output.Append(':');
            _output.Append(dt.Second.ToString("00", NumberFormatInfo.InvariantInfo));
        }

        private DatasetSchema GetSchema(DataTable ds)
        {
            if (ds == null) return null;

            DatasetSchema m = new DatasetSchema();
            m.Info = new List<string>();
            m.Name = ds.TableName;

            foreach (DataColumn c in ds.Columns)
            {
                m.Info.Add(ds.TableName);
                m.Info.Add(c.ColumnName);
                if (_params.FullyQualifiedDataSetSchema)
                    m.Info.Add(c.DataType.AssemblyQualifiedName);
                else
                    m.Info.Add(c.DataType.ToString());
            }
            // FEATURE : serialize relations and constraints here
            return m;
        }

        private DatasetSchema GetSchema(DataSet ds)
        {
            if (ds == null) return null;

            DatasetSchema m = new DatasetSchema();
            m.Info = new List<string>();
            m.Name = ds.DataSetName;

            foreach (DataTable t in ds.Tables)
            {
                foreach (DataColumn c in t.Columns)
                {
                    m.Info.Add(t.TableName);
                    m.Info.Add(c.ColumnName);
                    if (_params.FullyQualifiedDataSetSchema)
                        m.Info.Add(c.DataType.AssemblyQualifiedName);
                    else
                        m.Info.Add(c.DataType.ToString());
                }
            }
            // FEATURE : serialize relations and constraints here
            return m;
        }

        private string GetXmlSchema(DataTable dt)
        {
            using (var writer = new StringWriter())
            {
                dt.WriteXmlSchema(writer);
                return dt.ToString();
            }
        }

        private void WriteDataset(DataSet ds)
        {
            _output.Append('{');
            if (_params.UseExtensions)
            {
                WritePair("$schema", _params.UseOptimizedDatasetSchema ? (object)GetSchema(ds) : ds.GetXmlSchema());
                _output.Append(',');
            }
            bool tablesep = false;
            foreach (DataTable table in ds.Tables)
            {
                if (tablesep) _output.Append(',');
                tablesep = true;
                WriteDataTableData(table);
            }
            // end dataset
            _output.Append('}');
        }

        private void WriteDataTableData(DataTable table)
        {
            _output.Append('\"');
            _output.Append(table.TableName);
            _output.Append("\":[");
            DataColumnCollection cols = table.Columns;
            bool rowseparator = false;
            foreach (DataRow row in table.Rows)
            {   
                if (rowseparator) _output.Append(',');
                rowseparator = true;
                _output.Append('[');

                bool pendingSeperator = false;
                foreach (DataColumn column in cols)
                {
                    if (pendingSeperator) _output.Append(',');
                    WriteValue(row[column]);
                    pendingSeperator = true;
                }
                _output.Append(']');
            }

            _output.Append(']');
        }

        void WriteDataTable(DataTable dt)
        {
            this._output.Append('{');
            if (_params.UseExtensions)
            {
                this.WritePair("$schema", _params.UseOptimizedDatasetSchema ? (object)this.GetSchema(dt) : this.GetXmlSchema(dt));
                this._output.Append(',');
            }

            WriteDataTableData(dt);

            // end datatable
            this._output.Append('}');
        }

        bool _TypesWritten = false;
        private void WriteObject(object obj)
        {
            int i = 0;
            if (_cirobj.TryGetValue(obj, out i) == false)
                _cirobj.Add(obj, _cirobj.Count + 1);
            else
            {
                if (_current_depth > 0 && _params.InlineCircularReferences == false)
                {
                    //_circular = true;
                    _output.Append("{\"$i\":");
                    _output.Append(i.ToString());
                    _output.Append('}');
                    return;
                }
            }
            if (_params.UsingGlobalTypes == false)
                _output.Append('{');
            else
            {
                if (_TypesWritten == false)
                {
                    _output.Append('{');
                    _before = _output.Length;
                    //_output = new StringBuilder();
                }
                else
                    _output.Append('{');
            }
            _TypesWritten = true;
            _current_depth++;
            if (_current_depth > _MAX_DEPTH)
                throw new Exception("Serializer encountered maximum depth of " + _MAX_DEPTH);


            Dictionary<string, string> map = new Dictionary<string, string>();
            Type t = obj.GetType();
            bool append = false;
            if (_params.UseExtensions)
            {
                if (_params.UsingGlobalTypes == false)
                    WritePairFast("$type", Reflection.Instance.GetTypeAssemblyName(t));
                else
                {
                    int dt = 0;
                    string ct = Reflection.Instance.GetTypeAssemblyName(t);
                    if (_globalTypes.TryGetValue(ct, out dt) == false)
                    {
                        dt = _globalTypes.Count + 1;
                        _globalTypes.Add(ct, dt);
                    }
                    WritePairFast("$type", dt.ToString());
                }
                append = true;
            }

            Getters[] g = Reflection.Instance.GetGetters(t, /*_params.ShowReadOnlyProperties,*/ _params.IgnoreAttributes);
            int c = g.Length;
            for (int ii = 0; ii < c; ii++)
            {
                var p = g[ii];
                if (_params.ShowReadOnlyProperties == false && p.ReadOnly)
                    continue;
                object o = p.Getter(obj);
                if (_params.SerializeNullValues == false && (o == null || o is DBNull))
                {
                    //append = false;
                }
                else
                {
                    if (append)
                        _output.Append(',');
                    if (p.memberName != null)
                        WritePair(p.memberName, o);
                    else if (_params.SerializeToLowerCaseNames)
                        WritePair(p.lcName, o);
                    else
                        WritePair(p.Name, o);
                    if (o != null && _params.UseExtensions)
                    {
                        Type tt = o.GetType();
                        if (tt == typeof(object))
                            map.Add(p.Name, tt.ToString());
                    }
                    append = true;
                }
            }
            if (map.Count > 0 && _params.UseExtensions)
            {
                _output.Append(",\"$map\":");
                WriteStringDictionary(map);
            }
            _output.Append('}');
            _current_depth--;
        }

        private void WritePairFast(string name, string value)
        {
            WriteStringFast(name);

            _output.Append(':');

            WriteStringFast(value);
        }

        private void WritePair(string name, object value)
        {
            WriteString(name);

            _output.Append(':');

            WriteValue(value);
        }

        private void WriteArray(IEnumerable array)
        {
            _output.Append('[');

            bool pendingSeperator = false;

            foreach (object obj in array)
            {
                if (pendingSeperator) _output.Append(',');

                WriteValue(obj);

                pendingSeperator = true;
            }
            _output.Append(']');
        }

        private void WriteArrayRanked(Array array)
        {
            if (array.Rank == 1)
                WriteArray(array);
            else
            {
                // FIXx : use getlength 
                //var x = array.GetLength(0);
                //var y = array.GetLength(1);

                _output.Append('[');

                bool pendingSeperator = false;

                foreach (object obj in array)
                {
                    if (pendingSeperator) _output.Append(',');

                    WriteValue(obj);

                    pendingSeperator = true;
                }
                _output.Append(']');
            }
        }

        private void WriteStringDictionary(IDictionary dic)
        {
            _output.Append('{');

            bool pendingSeparator = false;

            foreach (DictionaryEntry entry in dic)
            {
                if (_params.SerializeNullValues == false && (entry.Value == null))
                {
                }
                else
                {
                    if (pendingSeparator) _output.Append(',');

                    string k = (string)entry.Key;
                    if (_params.SerializeToLowerCaseNames)
                        WritePair(k.ToLowerInvariant(), entry.Value);
                    else
                        WritePair(k, entry.Value);
                    pendingSeparator = true;
                }
            }
            _output.Append('}');
        }

        private void WriteStringDictionary(IEnumerable<KeyValuePair<string, object>> dic)
        {
            _output.Append('{');
            bool pendingSeparator = false;
            foreach (KeyValuePair<string, object> entry in dic)
            {
                if (_params.SerializeNullValues == false && (entry.Value == null))
                {
                }
                else
                {
                    if (pendingSeparator) _output.Append(',');
                    string k = entry.Key;

                    if (_params.SerializeToLowerCaseNames)
                        WritePair(k.ToLowerInvariant(), entry.Value);
                    else
                        WritePair(k, entry.Value);
                    pendingSeparator = true;
                }
            }
            _output.Append('}');
        }

        private void WriteDictionary(IDictionary dic)
        {
            _output.Append('[');

            bool pendingSeparator = false;

            foreach (DictionaryEntry entry in dic)
            {
                if (pendingSeparator) _output.Append(',');
                _output.Append('{');
                WritePair("k", entry.Key);
                _output.Append(',');
                WritePair("v", entry.Value);
                _output.Append('}');

                pendingSeparator = true;
            }
            _output.Append(']');
        }

        private void WriteStringFast(string s)
        {
            _output.Append('\"');
            _output.Append(s);
            _output.Append('\"');
        }

        private void WriteString(string s)
        {
            _output.Append('\"');

            int runIndex = -1;
            int l = s.Length;
            for (var index = 0; index < l; ++index)
            {
                var c = s[index];

                if (_useEscapedUnicode)
                {
                    if (c >= ' ' && c < 128 && c != '\"' && c != '\\')
                    {
                        if (runIndex == -1)
                            runIndex = index;

                        continue;
                    }
                }
                else
                {
                    if (c != '\t' && c != '\n' && c != '\r' && c != '\"' && c != '\\' && c != '\0')// && c != ':' && c!=',')
                    {
                        if (runIndex == -1)
                            runIndex = index;

                        continue;
                    }
                }

                if (runIndex != -1)
                {
                    _output.Append(s, runIndex, index - runIndex);
                    runIndex = -1;
                }

                switch (c)
                {
                    case '\t': _output.Append('\\').Append('t'); break;
                    case '\r': _output.Append('\\').Append('r'); break;
                    case '\n': _output.Append('\\').Append('n'); break;
                    case '"':
                    case '\\': _output.Append('\\'); _output.Append(c); break;
                    case '\0': _output.Append("\\u0000"); break;
                    default:
                        if (_useEscapedUnicode)
                        {
                            _output.Append("\\u");
                            _output.Append(((int)c).ToString("X4", NumberFormatInfo.InvariantInfo));
                        }
                        else
                            _output.Append(c);

                        break;
                }
            }

            if (runIndex != -1)
                _output.Append(s, runIndex, s.Length - runIndex);

            _output.Append('\"');
        }
    }

#endregion

#region FILE_JSON
    public sealed class JSONParameters
    {
        /// <summary>
        /// Use the optimized fast Dataset Schema format (default = True)
        /// </summary>
        public bool UseOptimizedDatasetSchema = true;
        /// <summary>
        /// Use the fast GUID format (default = True)
        /// </summary>
        public bool UseFastGuid = true;
        /// <summary>
        /// Serialize null values to the output (default = True)
        /// </summary>
        public bool SerializeNullValues = true;
        /// <summary>
        /// Use the UTC date format (default = True)
        /// </summary>
        public bool UseUTCDateTime = true;
        /// <summary>
        /// Show the readonly properties of types in the output (default = False)
        /// </summary>
        public bool ShowReadOnlyProperties = false;
        /// <summary>
        /// Use the $types extension to optimise the output json (default = True)
        /// </summary>
        public bool UsingGlobalTypes = true;
        /// <summary>
        /// Ignore case when processing json and deserializing 
        /// </summary>
        [Obsolete("Not needed anymore and will always match")]
        public bool IgnoreCaseOnDeserialize = false;
        /// <summary>
        /// Anonymous types have read only properties 
        /// </summary>
        public bool EnableAnonymousTypes = false;
        /// <summary>
        /// Enable fastJSON extensions $types, $type, $map (default = True)
        /// </summary>
        public bool UseExtensions = true;
        /// <summary>
        /// Use escaped unicode i.e. \uXXXX format for non ASCII characters (default = True)
        /// </summary>
        public bool UseEscapedUnicode = true;
        /// <summary>
        /// Output string key dictionaries as "k"/"v" format (default = False) 
        /// </summary>
        public bool KVStyleStringDictionary = false;
        /// <summary>
        /// Output Enum values instead of names (default = False)
        /// </summary>
        public bool UseValuesOfEnums = false;
        /// <summary>
        /// Ignore attributes to check for (default : XmlIgnoreAttribute, NonSerialized)
        /// </summary>
        public List<Type> IgnoreAttributes = new List<Type> { typeof(System.Xml.Serialization.XmlIgnoreAttribute), typeof(NonSerializedAttribute) };
        /// <summary>
        /// If you have parametric and no default constructor for you classes (default = False)
        /// 
        /// IMPORTANT NOTE : If True then all initial values within the class will be ignored and will be not set
        /// </summary>
        public bool ParametricConstructorOverride = false;
        /// <summary>
        /// Serialize DateTime milliseconds i.e. yyyy-MM-dd HH:mm:ss.nnn (default = false)
        /// </summary>
        public bool DateTimeMilliseconds = false;
        /// <summary>
        /// Maximum depth for circular references in inline mode (default = 20)
        /// </summary>
        public byte SerializerMaxDepth = 20;
        /// <summary>
        /// Inline circular or already seen objects instead of replacement with $i (default = false) 
        /// </summary>
        public bool InlineCircularReferences = false;
        /// <summary>
        /// Save property/field names as lowercase (default = false)
        /// </summary>
        public bool SerializeToLowerCaseNames = false;
        /// <summary>
        /// Formatter indent spaces (default = 3)
        /// </summary>
        public byte FormatterIndentSpaces = 3;
        /// <summary>
        /// TESTING - allow non quoted keys in the json like javascript (default = false)
        /// </summary>
        public bool AllowNonQuotedKeys = false;
        /// <summary>
        /// Auto convert string values to numbers when needed (default = true)
        /// 
        /// When disabled you will get an exception if the types don't match
        /// </summary>
        public bool AutoConvertStringToNumbers = true;
        /// <summary>
        /// Override object equality hash code checking (default = false)
        /// </summary>
        public bool OverrideObjectHashCodeChecking = false;
        [Obsolete("Racist term removed, please use BadListTypeChecking")]
        public bool BlackListTypeChecking = true;
        /// <summary>
        /// Checking list of bad types to prevent friday 13th json attacks (default = true)
        /// 
        /// Will throw an exception if encountered and set
        /// </summary>
        public bool BadListTypeChecking = true;
        /// <summary>
        /// Fully Qualify the DataSet Schema (default = false)
        /// 
        /// If you get deserialize errors with DataSets and DataTables
        /// </summary>
        public bool FullyQualifiedDataSetSchema = false;

        ///// <summary>
        ///// PENDING : Allow json5 (default = false)
        ///// </summary>
        //public bool AllowJsonFive = false;

        public void FixValues()
        {
            if (UseExtensions == false) // disable conflicting params
            {
                UsingGlobalTypes = false;
                InlineCircularReferences = true;
            }
            if (EnableAnonymousTypes)
                ShowReadOnlyProperties = true;

            //if (AllowJsonFive)
            //    AllowNonQuotedKeys = true;
        }

        public JSONParameters MakeCopy()
        {
            return new JSONParameters
            {
                AllowNonQuotedKeys = AllowNonQuotedKeys,
                DateTimeMilliseconds = DateTimeMilliseconds,
                EnableAnonymousTypes = EnableAnonymousTypes,
                FormatterIndentSpaces = FormatterIndentSpaces,
                IgnoreAttributes = new List<Type>(IgnoreAttributes),
                //IgnoreCaseOnDeserialize = IgnoreCaseOnDeserialize,
                InlineCircularReferences = InlineCircularReferences,
                KVStyleStringDictionary = KVStyleStringDictionary,
                ParametricConstructorOverride = ParametricConstructorOverride,
                SerializeNullValues = SerializeNullValues,
                SerializerMaxDepth = SerializerMaxDepth,
                SerializeToLowerCaseNames = SerializeToLowerCaseNames,
                ShowReadOnlyProperties = ShowReadOnlyProperties,
                UseEscapedUnicode = UseEscapedUnicode,
                UseExtensions = UseExtensions,
                UseFastGuid = UseFastGuid,
                UseOptimizedDatasetSchema = UseOptimizedDatasetSchema,
                UseUTCDateTime = UseUTCDateTime,
                UseValuesOfEnums = UseValuesOfEnums,
                UsingGlobalTypes = UsingGlobalTypes,
                AutoConvertStringToNumbers = AutoConvertStringToNumbers,
                OverrideObjectHashCodeChecking = OverrideObjectHashCodeChecking,
                //BlackListTypeChecking = BlackListTypeChecking,
                FullyQualifiedDataSetSchema = FullyQualifiedDataSetSchema,
                BadListTypeChecking = BadListTypeChecking,
                //AllowJsonFive = AllowJsonFive
            };
        }
    }

    public static class JSON
    {
        /// <summary>
        /// Globally set-able parameters for controlling the serializer
        /// </summary>
        public static JSONParameters Parameters = new JSONParameters();
        /// <summary>
        /// Create a formatted json string (beautified) from an object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToNiceJSON(object obj)
        {
            string s = ToJSON(obj, Parameters); // use default params

            return Beautify(s);
        }
        /// <summary>
        /// Create a formatted json string (beautified) from an object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string ToNiceJSON(object obj, JSONParameters param)
        {
            string s = ToJSON(obj, param);

            return Beautify(s, param.FormatterIndentSpaces);
        }
        /// <summary>
        /// Create a json representation for an object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJSON(object obj) => ToJSON(obj, Parameters);
        /// <summary>
        /// Create a json representation for an object with parameter override on this call
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string ToJSON(object obj, JSONParameters param)
        {
            param.FixValues();
            param = param.MakeCopy();
            Type t = null;

            if (obj == null)
                return "null";

            if (obj.GetType().IsGenericType)
                t = Reflection.Instance.GetGenericTypeDefinition(obj.GetType());
            if (typeof(IDictionary).IsAssignableFrom(t) || typeof(List<>).IsAssignableFrom(t))
                param.UsingGlobalTypes = false;

            // FEATURE : enable extensions when you can deserialize anon types
            if (param.EnableAnonymousTypes) { param.UseExtensions = false; param.UsingGlobalTypes = false; }
            return new JSONSerializer(param).ConvertToJSON(obj);
        }
        /// <summary>
        /// Parse a json string and generate a Dictionary&lt;string,object&gt; or List&lt;object&gt; structure
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object Parse(string json)
        {
            return new JsonParser(json, Parameters.AllowNonQuotedKeys).Decode(null);
        }
        /// <summary>
        /// Create a typed generic object from the json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToObject<T>(string json)
        {
            return new deserializer(Parameters).ToObject<T>(json);
        }
        /// <summary>
        /// Create a typed generic object from the json with parameter override on this call
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T ToObject<T>(string json, JSONParameters param)
        {
            return new deserializer(param).ToObject<T>(json);
        }
        /// <summary>
        /// Create an object from the json
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object ToObject(string json)
        {
            return new deserializer(Parameters).ToObject(json, null);
        }
        /// <summary>
        /// Create an object from the json with parameter override on this call
        /// </summary>
        /// <param name="json"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static object ToObject(string json, JSONParameters param)
        {
            return new deserializer(param).ToObject(json, null);
        }
        /// <summary>
        /// Create an object of type from the json
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ToObject(string json, Type type)
        {
            return new deserializer(Parameters).ToObject(json, type);
        }
        /// <summary>
        /// Create an object of type from the json with parameter override on this call
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <param name="par"></param>
        /// <returns></returns>
        public static object ToObject(string json, Type type, JSONParameters par)
        {
            return new deserializer(par).ToObject(json, type);
        }
        /// <summary>
        /// Fill a given object with the json represenation
        /// </summary>
        /// <param name="input"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object FillObject(object input, string json)
        {
            Dictionary<string, object> ht = new JsonParser(json, Parameters.AllowNonQuotedKeys).Decode(input.GetType()) as Dictionary<string, object>;
            if (ht == null) return null;
            return new deserializer(Parameters).ParseDictionary(ht, null, input.GetType(), input);
        }
        /// <summary>
        /// Deep copy an object i.e. clone to a new object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object DeepCopy(object obj)
        {
            return new deserializer(Parameters).ToObject(ToJSON(obj));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(T obj)
        {
            return new deserializer(Parameters).ToObject<T>(ToJSON(obj));
        }

        /// <summary>
        /// Create a human readable string from the json 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Beautify(string input)
        {
            var i = new string(' ', JSON.Parameters.FormatterIndentSpaces);
            return Formatter.PrettyPrint(input, i);
        }
        /// <summary>
        /// Create a human readable string from the json with specified indent spaces
        /// </summary>
        /// <param name="input"></param>
        /// <param name="spaces"></param>
        /// <returns></returns>
        public static string Beautify(string input, byte spaces)
        {
            var i = new string(' ', spaces);
            return Formatter.PrettyPrint(input, i);
        }
        /// <summary>
        /// Register custom type handlers for your own types not natively handled by fastJSON
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serializer"></param>
        /// <param name="deserializer"></param>
        public static void RegisterCustomType(Type type, Reflection.Serialize serializer, Reflection.Deserialize deserializer)
        {
            Reflection.Instance.RegisterCustomType(type, serializer, deserializer);
        }
        /// <summary>
        /// Clear the internal reflection cache so you can start from new (you will loose performance)
        /// </summary>
        public static void ClearReflectionCache()
        {
            Reflection.Instance.ClearReflectionCache();
        }
    }

    internal class deserializer
    {
        public deserializer(JSONParameters param)
        {
            if (param.OverrideObjectHashCodeChecking)
                _circobj = new Dictionary<object, int>(10, ReferenceEqualityComparer.Default);
            else
                _circobj = new Dictionary<object, int>();

            param.FixValues();
            _params = param.MakeCopy();
        }

        private JSONParameters _params;
        private bool _usingglobals = false;
        private Dictionary<object, int> _circobj;// = new Dictionary<object, int>();
        private Dictionary<int, object> _cirrev = new Dictionary<int, object>();

        public T ToObject<T>(string json)
        {
            Type t = typeof(T);
            var o = ToObject(json, t);

            if (t.IsArray)
            {
                if ((o as ICollection).Count == 0) // edge case for "[]" -> T[]
                {
                    Type tt = t.GetElementType();
                    object oo = Array.CreateInstance(tt, 0);
                    return (T)oo;
                }
                else
                    return (T)o;
            }
            else
                return (T)o;
        }

        public object ToObject(string json)
        {
            return ToObject(json, null);
        }

        public object ToObject(string json, Type type)
        {
            //_params.FixValues();
            Type t = null;
            if (type != null && type.IsGenericType)
                t = Reflection.Instance.GetGenericTypeDefinition(type);
            _usingglobals = _params.UsingGlobalTypes;
            if (typeof(IDictionary).IsAssignableFrom(t) || typeof(List<>).IsAssignableFrom(t))
                _usingglobals = false;

            object o = new JsonParser(json, _params.AllowNonQuotedKeys).Decode(type);

            if (o == null)   return null;

            if (type != null)
            {
                if (type == typeof(DataSet))
                    return CreateDataset(o as Dictionary<string, object>, null);
                else if (type == typeof(DataTable))
                    return CreateDataTable(o as Dictionary<string, object>, null);
            }

            if (o is IDictionary)
            {
                if (type != null && typeof(Dictionary<,>).IsAssignableFrom(t)) // deserialize a dictionary
                    return RootDictionary(o, type);
                else // deserialize an object
                    return ParseDictionary(o as Dictionary<string, object>, null, type, null);
            }
            else if (o is List<object>)
            {
                if (type != null)
                {
                    if (typeof(Dictionary<,>).IsAssignableFrom(t)) // kv format
                        return RootDictionary(o, type);
                    else if (t == typeof(List<>)) // deserialize to generic list
                        return RootList(o, type);
                    else if (type.IsArray)
                        return RootArray(o, type);
                    else if (type == typeof(Hashtable))
                        return RootHashTable((List<object>)o);
                }
                else //if (type == null)
                {
                    List<object> l = (List<object>)o;
                    if (l.Count > 0 && l[0].GetType() == typeof(Dictionary<string, object>))
                    {
                        Dictionary<string, object> globals = new Dictionary<string, object>();
                        List<object> op = new List<object>();
                        // try to get $types 
                        foreach (var i in l)
                            op.Add(ParseDictionary((Dictionary<string, object>)i, globals, null, null));
                        return op;
                    }
                    return l.ToArray();
                }
            }
            else if (type != null && o.GetType() != type)
                return ChangeType(o, type);

            return o;
        }

        #region [   p r i v a t e   m e t h o d s   ]
        private object RootHashTable(List<object> o)
        {
            Hashtable h = new Hashtable();

            foreach (Dictionary<string, object> values in o)
            {
                object key = values["k"];
                object val = values["v"];
                if (key is Dictionary<string, object>)
                    key = ParseDictionary((Dictionary<string, object>)key, null, typeof(object), null);

                if (val is Dictionary<string, object>)
                    val = ParseDictionary((Dictionary<string, object>)val, null, typeof(object), null);

                h.Add(key, val);
            }

            return h;
        }

        private object ChangeType(object value, Type conversionType)
        {
            if (conversionType == typeof(object))
                return value;

            if (conversionType == typeof(int))
            {
                string s = value as string;
                if (s == null)
                    return (int)((long)value);
                else if (_params.AutoConvertStringToNumbers)
                    return Helper.CreateInteger(s, 0, s.Length);
                else
                    throw new Exception("AutoConvertStringToNumbers is disabled for converting string : " + value);
            }
            else if (conversionType == typeof(long))
            {
                string s = value as string;
                if (s == null)
                    return (long)value;
                else if (_params.AutoConvertStringToNumbers)
                    return Helper.CreateLong(s, 0, s.Length);
                else
                    throw new Exception("AutoConvertStringToNumbers is disabled for converting string : " + value);
            }
            else if (conversionType == typeof(string))
                return (string)value;

            else if (conversionType.IsEnum)
                return Helper.CreateEnum(conversionType, value);

            else if (conversionType == typeof(DateTime))
                return Helper.CreateDateTime((string)value, _params.UseUTCDateTime);

            else if (conversionType == typeof(DateTimeOffset))
                return Helper.CreateDateTimeOffset((string)value);

            else if (Reflection.Instance.IsTypeRegistered(conversionType))
                return Reflection.Instance.CreateCustom((string)value, conversionType);

            // 8-30-2014 - James Brooks - Added code for nullable types.
            if (Helper.IsNullable(conversionType))
            {
                if (value == null)
                    return value;
                conversionType = Helper.UnderlyingTypeOf(conversionType);
            }

            // 8-30-2014 - James Brooks - Nullable Guid is a special case so it was moved after the "IsNullable" check.
            if (conversionType == typeof(Guid))
                return Helper.CreateGuid((string)value);

            // 2016-04-02 - Enrico Padovani - proper conversion of byte[] back from string
            if (conversionType == typeof(byte[]))
                return Convert.FromBase64String((string)value);

            if (conversionType == typeof(TimeSpan))
                return new TimeSpan((long)value);

            return Convert.ChangeType(value, conversionType, CultureInfo.InvariantCulture);
        }

        private object RootList(object parse, Type type)
        {
            Type[] gtypes = Reflection.Instance.GetGenericArguments(type);
            var o = (IList)Reflection.Instance.FastCreateList(type, ((IList)parse).Count);
            DoParseList((IList)parse, gtypes[0], o);
            return o;
        }

        private void DoParseList(IList parse, Type it, IList o)
        {
            Dictionary<string, object> globals = new Dictionary<string, object>();

            foreach (var k in parse)
            {
                _usingglobals = false;
                object v = k;
                var a = k as Dictionary<string, object>;
                if (a != null)
                    v = ParseDictionary(a, globals, it, null);
                else
                    v = ChangeType(k, it);

                o.Add(v);
            }
        }

        private object RootArray(object parse, Type type)
        {
            Type it = type.GetElementType();
            var o = (IList)Reflection.Instance.FastCreateInstance(typeof(List<>).MakeGenericType(it));
            DoParseList((IList)parse, it, o);
            var array = Array.CreateInstance(it, o.Count);
            o.CopyTo(array, 0);
            return array;
        }

        private object RootDictionary(object parse, Type type)
        {
            Type[] gtypes = Reflection.Instance.GetGenericArguments(type);
            Type t1 = null;
            Type t2 = null;
            bool dictionary = false;
            if (gtypes != null)
            {
                t1 = gtypes[0];
                t2 = gtypes[1];
                if (t2 != null)
                    dictionary = t2.Name.StartsWith("Dictionary");
            }

            var arraytype = t2.GetElementType();
            if (parse is Dictionary<string, object>)
            {
                IDictionary o = (IDictionary)Reflection.Instance.FastCreateInstance(type);

                foreach (var kv in (Dictionary<string, object>)parse)
                {
                    object v;
                    object k = ChangeType(kv.Key, t1);

                    if (dictionary) // deserialize a dictionary
                        v = RootDictionary(kv.Value, t2);

                    else if (kv.Value is Dictionary<string, object>)
                        v = ParseDictionary(kv.Value as Dictionary<string, object>, null, t2, null);

                    else if (t2.IsArray && t2 != typeof(byte[]))
                        v = CreateArray((List<object>)kv.Value, t2, arraytype, null);

                    else if (kv.Value is IList)
                        v = CreateGenericList((List<object>)kv.Value, t2, t1, null);

                    else
                        v = ChangeType(kv.Value, t2);

                    o.Add(k, v);
                }

                return o;
            }
            if (parse is List<object>)
                return CreateDictionary(parse as List<object>, type, gtypes, null);

            return null;
        }

        internal object ParseDictionary(Dictionary<string, object> d, Dictionary<string, object> globaltypes, Type type, object input)
        {
            object tn = "";
            if (type == typeof(NameValueCollection))
                return Helper.CreateNV(d);
            if (type == typeof(StringDictionary))
                return Helper.CreateSD(d);

            if (d.TryGetValue("$i", out tn))
            {
                object v = null;
                _cirrev.TryGetValue((int)(long)tn, out v);
                return v;
            }

            if (d.TryGetValue("$types", out tn))
            {
                _usingglobals = true;
                if (globaltypes == null)
                    globaltypes = new Dictionary<string, object>();
                foreach (var kv in (Dictionary<string, object>)tn)
                {
                    globaltypes.Add((string)kv.Value, kv.Key);
                }
            }
            if (globaltypes != null)
                _usingglobals = true;

            bool found = d.TryGetValue("$type", out tn);

            if (found == false && type == typeof(System.Object))
            {
                return d;   // CreateDataset(d, globaltypes);
            }
            if (found)
            {
                if (_usingglobals)
                {
                    object tname = "";
                    if (globaltypes != null && globaltypes.TryGetValue((string)tn, out tname))
                        tn = tname;
                }
                type = Reflection.Instance.GetTypeFromCache((string)tn, _params.BadListTypeChecking);
            }

            if (type == null)
                throw new Exception("Cannot determine type : " + tn);

            string typename = type.FullName;
            object o = input;
            if (o == null)
            {
                if (_params.ParametricConstructorOverride)
                    o = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
                else
                    o = Reflection.Instance.FastCreateInstance(type);
            }
            int circount = 0;
            if (_circobj.TryGetValue(o, out circount) == false)
            {
                circount = _circobj.Count + 1;
                _circobj.Add(o, circount);
                _cirrev.Add(circount, o);
            }

            var props = Reflection.Instance.Getproperties(type, typename, _params.ShowReadOnlyProperties);
            foreach (var kv in d)
            {
                var n = kv.Key;
                var v = kv.Value;

                string name = n;//.ToLower();
                if (name == "$map")
                {
                    ProcessMap(o, props, (Dictionary<string, object>)d[name]);
                    continue;
                }
                myPropInfo pi;
                if (props.TryGetValue(name, out pi) == false)
                    if (props.TryGetValue(name.ToLowerInvariant(), out pi) == false)
                        continue;

                if (pi.CanWrite)
                {
                    object oset = null;
                    if (v != null)
                    {
                        switch (pi.Type)
                        {
                            case myPropInfoType.Int: oset = (int)Helper.AutoConv(v, _params); break;
                            case myPropInfoType.Long: oset = Helper.AutoConv(v, _params); break;
                            case myPropInfoType.String: oset = v.ToString(); break;
                            case myPropInfoType.Bool: oset = Helper.BoolConv(v); break;
                            case myPropInfoType.DateTime: oset = Helper.CreateDateTime((string)v, _params.UseUTCDateTime); break;
                            case myPropInfoType.Enum: oset = Helper.CreateEnum(pi.pt, v); break;
                            case myPropInfoType.Guid: oset = Helper.CreateGuid((string)v); break;

                            case myPropInfoType.Array:
                                if (!pi.IsValueType)
                                    oset = CreateArray((List<object>)v, pi.pt, pi.bt, globaltypes);
                                // what about 'else'?
                                break;
                            case myPropInfoType.ByteArray: oset = Convert.FromBase64String((string)v); break;

                            case myPropInfoType.DataSet: oset = CreateDataset((Dictionary<string, object>)v, globaltypes); break;
                            case myPropInfoType.DataTable: oset = CreateDataTable((Dictionary<string, object>)v, globaltypes); break;
                            case myPropInfoType.Hashtable: // same case as Dictionary

                            case myPropInfoType.Dictionary: oset = CreateDictionary((List<object>)v, pi.pt, pi.GenericTypes, globaltypes); break;
                            case myPropInfoType.StringKeyDictionary: oset = CreateStringKeyDictionary((Dictionary<string, object>)v, pi.pt, pi.GenericTypes, globaltypes); break;
                            case myPropInfoType.NameValue: oset = Helper.CreateNV((Dictionary<string, object>)v); break;
                            case myPropInfoType.StringDictionary: oset = Helper.CreateSD((Dictionary<string, object>)v); break;
                            case myPropInfoType.Custom: oset = Reflection.Instance.CreateCustom((string)v, pi.pt); break;
                            default:
                                {
                                    if (pi.IsGenericType && pi.IsValueType == false && v is List<object>)
                                        oset = CreateGenericList((List<object>)v, pi.pt, pi.bt, globaltypes);

                                    else if ((pi.IsClass || pi.IsStruct || pi.IsInterface) && v is Dictionary<string, object>)
                                        oset = ParseDictionary((Dictionary<string, object>)v, globaltypes, pi.pt, null);// pi.getter(o));

                                    else if (v is List<object>)
                                        oset = CreateArray((List<object>)v, pi.pt, typeof(object), globaltypes);

                                    else if (pi.IsValueType)
                                        oset = ChangeType(v, pi.changeType);

                                    else
                                        oset = v;
                                }
                                break;
                        }

                    }
                    o = pi.setter(o, oset);
                }
            }
            return o;
        }

        private static void ProcessMap(object obj, Dictionary<string, myPropInfo> props, Dictionary<string, object> dic)
        {
            foreach (KeyValuePair<string, object> kv in dic)
            {
                myPropInfo p = props[kv.Key];
                object o = p.getter(obj);
                // blacklist checking
                Type t = //Type.GetType((string)kv.Value);
                        Reflection.Instance.GetTypeFromCache((string)kv.Value, true);
                if (t == typeof(Guid))
                    p.setter(obj, Helper.CreateGuid((string)o));
            }
        }

        private object CreateArray(List<object> data, Type pt, Type bt, Dictionary<string, object> globalTypes)
        {
            if (bt == null)
                bt = typeof(object);

            Array col = Array.CreateInstance(bt, data.Count);
            var arraytype = bt.GetElementType();
            // create an array of objects
            for (int i = 0; i < data.Count; i++)
            {
                object ob = data[i];
                if (ob == null)
                {
                    continue;
                }
                if (ob is IDictionary)
                    col.SetValue(ParseDictionary((Dictionary<string, object>)ob, globalTypes, bt, null), i);
                else if (ob is ICollection)
                    col.SetValue(CreateArray((List<object>)ob, bt, arraytype, globalTypes), i);
                else
                    col.SetValue(ChangeType(ob, bt), i);
            }

            return col;
        }

        private object CreateGenericList(List<object> data, Type pt, Type bt, Dictionary<string, object> globalTypes)
        {
            if (pt != typeof(object))
            {
                IList col = (IList)Reflection.Instance.FastCreateList(pt, data.Count);
                var it = Reflection.Instance.GetGenericArguments(pt)[0];// pt.GetGenericArguments()[0];
                // create an array of objects
                foreach (object ob in data)
                {
                    if (ob is IDictionary)
                        col.Add(ParseDictionary((Dictionary<string, object>)ob, globalTypes, it, null));

                    else if (ob is List<object>)
                    {
                        if (bt.IsGenericType)
                            col.Add((List<object>)ob);//).ToArray());
                        else
                            col.Add(((List<object>)ob).ToArray());
                    }
                    else
                        col.Add(ChangeType(ob, it));
                }
                return col;
            }
            return data;
        }

        private object CreateStringKeyDictionary(Dictionary<string, object> reader, Type pt, Type[] types, Dictionary<string, object> globalTypes)
        {
            var col = (IDictionary)Reflection.Instance.FastCreateInstance(pt);
            Type arraytype = null;
            Type t2 = null;
            if (types != null)
                t2 = types[1];

            Type generictype = null;
            var ga = Reflection.Instance.GetGenericArguments(t2);// t2.GetGenericArguments();
            if (ga.Length > 0)
                generictype = ga[0];
            arraytype = t2.GetElementType();

            foreach (KeyValuePair<string, object> values in reader)
            {
                var key = values.Key;
                object val = null;

                if (values.Value is Dictionary<string, object>)
                    val = ParseDictionary((Dictionary<string, object>)values.Value, globalTypes, t2, null);

                else if (types != null && t2.IsArray)
                {
                    if (values.Value is Array)
                        val = values.Value;
                    else
                        val = CreateArray((List<object>)values.Value, t2, arraytype, globalTypes);
                }
                else if (values.Value is IList)
                    val = CreateGenericList((List<object>)values.Value, t2, generictype, globalTypes);

                else
                    val = ChangeType(values.Value, t2);

                col.Add(key, val);
            }

            return col;
        }

        private object CreateDictionary(List<object> reader, Type pt, Type[] types, Dictionary<string, object> globalTypes)
        {
            IDictionary col = (IDictionary)Reflection.Instance.FastCreateInstance(pt);
            Type t1 = null!;
            Type t2 = null!;
            Type generictype = null!;
            if (types != null)
            {
                t1 = types[0];
                t2 = types[1];
            }
            Type arraytype = t2;
            if (t2 != null)
            {
                var ga = Reflection.Instance.GetGenericArguments(t2);// t2.GetGenericArguments();
                if (ga.Length > 0)
                    generictype = ga[0];
                arraytype = t2.GetElementType()!;
            }
            bool root = typeof(IDictionary).IsAssignableFrom(t2);

            foreach (Dictionary<string, object> values in reader)
            {
                object key = values["k"];
                object val = values["v"];

                if (key is Dictionary<string, object>)
                    key = ParseDictionary((Dictionary<string, object>)key, globalTypes, t1, null!);
                else
                    key = ChangeType(key, t1);

                if (root)
                    val = RootDictionary(val, t2);

                else if (val is Dictionary<string, object>)
                    val = ParseDictionary((Dictionary<string, object>)val, globalTypes, t2, null!);

                else if (types != null && t2.IsArray)
                    val = CreateArray((List<object>)val, t2, arraytype, globalTypes);

                else if (val is IList)
                    val = CreateGenericList((List<object>)val, t2, generictype, globalTypes);

                else
                    val = ChangeType(val, t2);

                col.Add(key, val);
            }

            return col;
        }

        private DataSet CreateDataset(Dictionary<string, object> reader, Dictionary<string, object> globalTypes)
        {
            DataSet ds = new DataSet();
            ds.EnforceConstraints = false;
            ds.BeginInit();

            // read dataset schema here
            var schema = reader["$schema"];

            if (schema is string)
            {
                TextReader tr = new StringReader((string)schema);
                ds.ReadXmlSchema(tr);
            }
            else
            {
                DatasetSchema ms = (DatasetSchema)ParseDictionary((Dictionary<string, object>)schema, globalTypes, typeof(DatasetSchema), null);
                ds.DataSetName = ms.Name;
                for (int i = 0; i < ms.Info.Count; i += 3)
                {
                    if (ds.Tables.Contains(ms.Info[i]) == false)
                        ds.Tables.Add(ms.Info[i]);
                    // blacklist checking
                    var t = //Type.GetType(ms.Info[i + 2]);
                            Reflection.Instance.GetTypeFromCache(ms.Info[i + 2], true);
                    ds.Tables[ms.Info[i]].Columns.Add(ms.Info[i + 1], t);
                }
            }

            foreach (KeyValuePair<string, object> pair in reader)
            {
                if (pair.Key == "$type" || pair.Key == "$schema") continue;

                List<object> rows = (List<object>)pair.Value;
                if (rows == null) continue;

                DataTable dt = ds.Tables[pair.Key];
                ReadDataTable(rows, dt);
            }

            ds.EndInit();

            return ds;
        }

        private void ReadDataTable(List<object> rows, DataTable dt)
        {
            dt.BeginInit();
            dt.BeginLoadData();
            List<int> guidcols = new List<int>();
            List<int> datecol = new List<int>();
            List<int> bytearraycol = new List<int>();

            foreach (DataColumn c in dt.Columns)
            {
                if (c.DataType == typeof(Guid) || c.DataType == typeof(Guid?))
                    guidcols.Add(c.Ordinal);
                if (_params.UseUTCDateTime && (c.DataType == typeof(DateTime) || c.DataType == typeof(DateTime?)))
                    datecol.Add(c.Ordinal);
                if (c.DataType == typeof(byte[]))
                    bytearraycol.Add(c.Ordinal);
            }

            foreach (List<object> row in rows)
            {
                //object[] v = row.ToArray(); //new object[row.Count];
                //row.CopyTo(v, 0);
                var v = row;
                foreach (int i in guidcols)
                {
                    string s = (string)v[i];
                    if (s != null && s.Length < 36)
                        v[i] = new Guid(Convert.FromBase64String(s));
                }
                foreach (int i in bytearraycol)
                {
                    string s = (string)v[i];
                    if (s != null)
                        v[i] = Convert.FromBase64String(s);
                }
                if (_params.UseUTCDateTime)
                {
                    foreach (int i in datecol)
                    {
                        string s = (string)v[i];
                        if (s != null)
                            v[i] = Helper.CreateDateTime(s, _params.UseUTCDateTime);
                    }
                }
                dt.Rows.Add(v.ToArray());
            }

            dt.EndLoadData();
            dt.EndInit();
        }

        DataTable CreateDataTable(Dictionary<string, object> reader, Dictionary<string, object> globalTypes)
        {
            var dt = new DataTable();

            // read dataset schema here
            var schema = reader["$schema"];

            if (schema is string)
            {
                TextReader tr = new StringReader((string)schema);
                dt.ReadXmlSchema(tr);
            }
            else
            {
                var ms = (DatasetSchema)ParseDictionary((Dictionary<string, object>)schema, globalTypes, typeof(DatasetSchema), null);
                dt.TableName = ms.Info[0];
                for (int i = 0; i < ms.Info.Count; i += 3)
                {
                    // blacklist checking
                    var t = //Type.GetType(ms.Info[i + 2]);
                            Reflection.Instance.GetTypeFromCache(ms.Info[i + 2], true);
                    dt.Columns.Add(ms.Info[i + 1], t);
                }
            }

            foreach (var pair in reader)
            {
                if (pair.Key == "$type" || pair.Key == "$schema")
                    continue;

                var rows = (List<object>)pair.Value;
                if (rows == null)
                    continue;

                if (!dt.TableName.Equals(pair.Key, StringComparison.InvariantCultureIgnoreCase))
                    continue;

                ReadDataTable(rows, dt);
            }

            return dt;
        }

        #endregion
    }

#endregion
#pragma warning restore

