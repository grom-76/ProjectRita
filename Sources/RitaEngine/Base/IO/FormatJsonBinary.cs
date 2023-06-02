/*
BINARY JSOn

Serialize et deserialze objet en binaire bytes[]

site : https://github.com/mgholam/fastBinaryJSON

used in Uneral Engine
Recup le 10/09/2022 ver : v1.5.2

Exemples :
    byte[] bytes = fastBinaryJSON.BJSON.ToJSON(obj);
    byte[] bytes = fastBinaryJSON.BJSON.ToJSON(obj, true, true); // optimized dataset, unicode strings

    object obj = fastBinaryJSON.BJSON.ToObject(bytes);
    SalesInvoice obj = fastBinaryJSON.BJSON.ToObject<SalesInvoice>(bytes); // type is known 

Not optimised => 2920
*/
namespace RitaEngine.Base.IO.Formats.JSONBinary;


#pragma warning disable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

#region FILE BJSON.CS
    public sealed class TOKENS
    {
        public const byte DOC_START = 1;
        public const byte DOC_END = 2;
        public const byte ARRAY_START = 3;
        public const byte ARRAY_END = 4;
        public const byte COLON = 5;
        public const byte COMMA = 6;
        public const byte NAME = 7;
        public const byte STRING = 8;
        public const byte BYTE = 9;
        public const byte INT = 10;
        public const byte UINT = 11;
        public const byte LONG = 12;
        public const byte ULONG = 13;
        public const byte SHORT = 14;
        public const byte USHORT = 15;
        public const byte DATETIME = 16;
        public const byte GUID = 17;
        public const byte DOUBLE = 18;
        public const byte FLOAT = 19;
        public const byte DECIMAL = 20;
        public const byte CHAR = 21;
        public const byte BYTEARRAY = 22;
        public const byte NULL = 23;
        public const byte TRUE = 24;
        public const byte FALSE = 25;
        public const byte UNICODE_STRING = 26;
        public const byte DATETIMEOFFSET = 27;
        public const byte ARRAY_TYPED = 28;
        public const byte TYPES_POINTER = 29;
        public const byte TIMESPAN = 30;
        public const byte ARRAY_TYPED_LONG = 31;
        public const byte NAME_UNI = 32;
    }

    public class typedarray
    {
        public string typename;
        public int count;
        public List<object> data = new List<object>();
    }

    public sealed class BJSONParameters
    {
        /// <summary> 
        /// Optimize the schema for Datasets (default = True)
        /// </summary>
        public bool UseOptimizedDatasetSchema = true;
        /// <summary>
        /// Serialize readonly properties (default = False)
        /// </summary>
        public bool ShowReadOnlyProperties = false;
        /// <summary>
        /// Use global types $types for more compact size when using a lot of classes (default = True)
        /// </summary>
        public bool UsingGlobalTypes = true;
        /// <summary>
        /// Use Unicode strings = T (faster), Use UTF8 strings = F (smaller) (default = True)
        /// </summary>
        public bool UseUnicodeStrings = true;
        /// <summary>
        /// Serialize Null values to the output (default = False)
        /// </summary>
        public bool SerializeNulls = false;
        /// <summary>
        /// Enable fastBinaryJSON extensions $types, $type, $map (default = True)
        /// </summary>
        public bool UseExtensions = true;
        /// <summary>
        /// Anonymous types have read only properties 
        /// </summary>
        public bool EnableAnonymousTypes = false;
        /// <summary>
        /// Use the UTC date format (default = False)
        /// </summary>
        public bool UseUTCDateTime = false;
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
        /// Maximum depth the serializer will go to to avoid loops (default = 20 levels)
        /// </summary>
        public short SerializerMaxDepth = 20;
        /// <summary>
        /// Use typed arrays t[] into object = t[] not object[] (default = true)
        /// </summary>
        public bool UseTypedArrays = true;
        /// <summary>
        /// Backward compatible Typed array type name as UTF8 (default = false -> fast v1.5 unicode)
        /// </summary>
        public bool v1_4TypedArray = false;

        //public bool OptimizeSize = false;


        public void FixValues()
        {
            if (UseExtensions == false) // disable conflicting params
                UsingGlobalTypes = false;

            if (EnableAnonymousTypes)
                ShowReadOnlyProperties = true;
        }

        internal BJSONParameters MakeCopy()
        {
            return new BJSONParameters
            {
                UseOptimizedDatasetSchema = UseOptimizedDatasetSchema,
                ShowReadOnlyProperties = ShowReadOnlyProperties,
                EnableAnonymousTypes = EnableAnonymousTypes,
                UsingGlobalTypes = UsingGlobalTypes,
                IgnoreAttributes = new List<Type>(IgnoreAttributes),
                UseUnicodeStrings = UseUnicodeStrings,
                SerializeNulls = SerializeNulls,
                ParametricConstructorOverride = ParametricConstructorOverride,
                SerializerMaxDepth = SerializerMaxDepth,
                UseTypedArrays = UseTypedArrays,
                UseExtensions = UseExtensions,
                UseUTCDateTime = UseUTCDateTime,
                v1_4TypedArray = v1_4TypedArray//,
                //OptimizeSize = OptimizeSize

            };
        }
    }

    public static class BJSON
    {
        /// <summary>
        /// Globally set-able parameters for controlling the serializer
        /// </summary>
        public static BJSONParameters Parameters = new BJSONParameters();
        /// <summary>
        /// Parse a json and generate a Dictionary&lt;string,object&gt; or List&lt;object&gt; structure
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object Parse(byte[] json)
        {
            return new BJsonParser(json, Parameters.UseUTCDateTime, Parameters.v1_4TypedArray).Decode();
        }
        /// <summary>
        /// Register custom type handlers for your own types not natively handled by fastBinaryJSON
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serializer"></param>
        /// <param name="deserializer"></param>
        public static void RegisterCustomType(Type type, Reflection.Serialize serializer, Reflection.Deserialize deserializer)
        {
            Reflection.Instance.RegisterCustomType(type, serializer, deserializer);
        }
        /// <summary>
        /// Create a binary json representation for an object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ToBJSON(object obj)
        {
            return ToBJSON(obj, Parameters);
        }
        /// <summary>
        /// Create a binary json representation for an object with parameter override on this call
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static byte[] ToBJSON(object obj, BJSONParameters param)
        {
            param.FixValues();
            param = param.MakeCopy();
            Type t = null;
            if (obj == null)
                return new byte[] { TOKENS.NULL };
            if (obj.GetType().IsGenericType)
                t = Reflection.Instance.GetGenericTypeDefinition(obj.GetType());// obj.GetType().GetGenericTypeDefinition();
            if (t == typeof(Dictionary<,>) || t == typeof(List<>))
                param.UsingGlobalTypes = false;
            // FEATURE : enable extensions when you can deserialize anon types
            if (param.EnableAnonymousTypes) { param.UseExtensions = false; param.UsingGlobalTypes = false; }

            return new BJSONSerializer(param).ConvertToBJSON(obj);
        }
        /// <summary>
        /// Fill a given object with the binary json represenation
        /// </summary>
        /// <param name="input"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object FillObject(object input, byte[] json)
        {
            return new deserializer(Parameters).FillObject(input, json);
        }
        /// <summary>
        /// Create a generic object from the json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToObject<T>(byte[] json)
        {
            return new deserializer(Parameters).ToObject<T>(json);
        }
        /// <summary>
        /// Create a generic object from the json with parameter override on this call
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T ToObject<T>(byte[] json, BJSONParameters param)
        {
            return new deserializer(param).ToObject<T>(json);
        }
        /// <summary>
        /// Create an object from the json 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object ToObject(byte[] json)
        {
            return new deserializer(Parameters).ToObject(json, null);
        }
        /// <summary>
        /// Create an object from the json with parameter override on this call
        /// </summary>
        /// <param name="json"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static object ToObject(byte[] json, BJSONParameters param)
        {
            param.FixValues();
            param = param.MakeCopy();
            return new deserializer(param).ToObject(json, null);
        }
        /// <summary>
        /// Create a typed object from the json
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ToObject(byte[] json, Type type)
        {
            return new deserializer(Parameters).ToObject(json, type);
        }
        /// <summary>
        /// Clear the internal reflection cache so you can start from new (you will loose performance)
        /// </summary>
        public static void ClearReflectionCache()
        {
            Reflection.Instance.ClearReflectionCache();
        }
        /// <summary>
        /// Deep copy an object i.e. clone to a new object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object DeepCopy(object obj)
        {
            return new deserializer(Parameters).ToObject(ToBJSON(obj));
        }
    }

    internal class deserializer
    {
        public deserializer(BJSONParameters param)
        {
            _params = param;
            _params = param.MakeCopy();
        }

        private BJSONParameters _params;
        private Dictionary<object, int> _circobj = new Dictionary<object, int>();
        private Dictionary<int, object> _cirrev = new Dictionary<int, object>();

        public T ToObject<T>(byte[] json)
        {
            return (T)ToObject(json, typeof(T));
        }

        public object ToObject(byte[] json)
        {
            return ToObject(json, null);
        }

        public object ToObject(byte[] json, Type type)
        {
            //_params.FixValues();
            Type t = null;
            if (type != null && type.IsGenericType)
                t = Reflection.Instance.GetGenericTypeDefinition(type);// type.GetGenericTypeDefinition();
            _globalTypes = _params.UsingGlobalTypes;
            if (t == typeof(Dictionary<,>) || t == typeof(List<>))
                _globalTypes = false;

            var o = new BJsonParser(json, _params.UseUTCDateTime, _params.v1_4TypedArray).Decode();
            if (type?.IsEnum == true) return CreateEnum(type, o);

            if (type != null && type == typeof(DataSet))
                return CreateDataset(o as Dictionary<string, object>, null);

            if (type != null && type == typeof(DataTable))
                return CreateDataTable(o as Dictionary<string, object>, null);

            if (o is typedarray)
            {
                return ParseTypedArray(new Dictionary<string, object>(), o);
            }
            if (o is IDictionary)
            {
                if (type != null && t == typeof(Dictionary<,>)) // deserialize a dictionary
                    return RootDictionary(o, type);
                else // deserialize an object
                    return ParseDictionary(o as Dictionary<string, object>, null, type, null);
            }

            if (o is List<object>)
            {
                if (type != null && t == typeof(Dictionary<,>)) // kv format
                    return RootDictionary(o, type);

                if (type != null && t == typeof(List<>)) // deserialize to generic list
                    return RootList(o, type);

                if (type == typeof(Hashtable))
                    return RootHashTable((List<object>)o);
                else if (type == null)
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

        private object ChangeType(object o, Type type)
        {
            if (Reflection.Instance.IsTypeRegistered(type))
                return Reflection.Instance.CreateCustom((string)o, type);
            else
                return o;
        }

        public object FillObject(object input, byte[] json)
        {
            _params.FixValues();
            Dictionary<string, object> ht = new BJsonParser(json, _params.UseUTCDateTime, _params.v1_4TypedArray).Decode() as Dictionary<string, object>;
            if (ht == null) return null;
            return ParseDictionary(ht, null, input.GetType(), input);
        }

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

        private object RootList(object parse, Type type)
        {
            Type[] gtypes = Reflection.Instance.GetGenericArguments(type);// type.GetGenericArguments();
            IList o = (IList)Reflection.Instance.FastCreateList(type, ((IList)parse).Count);
            Dictionary<string, object> globals = new Dictionary<string, object>();

            foreach (var k in (IList)parse)
            {
                _globalTypes = false;
                object v = k;
                if (k is Dictionary<string, object>)
                    v = ParseDictionary(k as Dictionary<string, object>, globals, gtypes[0], null);
                else
                    v = k;

                o.Add(v);
            }
            return o;
        }

        private object RootDictionary(object parse, Type type)
        {
            Type[] gtypes = Reflection.Instance.GetGenericArguments(type);
            Type t1 = null;
            Type t2 = null;
            if (gtypes != null)
            {
                t1 = gtypes[0];
                t2 = gtypes[1];
            }
            var arraytype = t2.GetElementType();

            if (parse is Dictionary<string, object>)
            {
                IDictionary o = (IDictionary)Reflection.Instance.FastCreateInstance(type);

                foreach (var kv in (Dictionary<string, object>)parse)
                {
                    _globalTypes = false;
                    object v;
                    object k = kv.Key;
                    if (t2.Name.StartsWith("Dictionary")) // deserialize a dictionary
                        v = RootDictionary(kv.Value, t2);

                    else if (kv.Value is Dictionary<string, object>)
                        v = ParseDictionary(kv.Value as Dictionary<string, object>, null, t2, null);

                    else if (t2 == typeof(byte[]))
                        v = kv.Value;

                    else if (gtypes != null && t2.IsArray)
                        v = CreateArray((List<object>)kv.Value, t2, arraytype, null);

                    else if (kv.Value is IList)
                        v = CreateGenericList((List<object>)kv.Value, t2, t1, null);

                    else
                        v = kv.Value;

                    o.Add(k, v);
                }

                return o;
            }
            if (parse is List<object>)
                return CreateDictionary(parse as List<object>, type, gtypes, null);

            return null;
        }

        private bool _globalTypes = false;
        private object ParseDictionary(Dictionary<string, object> d, Dictionary<string, object> globaltypes, Type type, object input)
        {
            object tn = "";
            if (type == typeof(NameValueCollection))
                return CreateNV(d);
            if (type == typeof(StringDictionary))
                return CreateSD(d);

            if (d.TryGetValue("$i", out tn))
            {
                object v = null;
                _cirrev.TryGetValue((int)tn, out v);
                return v;
            }

            if (d.TryGetValue("$types", out tn))
            {
                _globalTypes = true;
                if (globaltypes == null)
                    globaltypes = new Dictionary<string, object>();
                foreach (var kv in (Dictionary<string, object>)tn)
                {
                    globaltypes.Add((string)kv.Key, kv.Value);
                }
            }

            if (globaltypes != null)
                _globalTypes = true;

            bool found = d.TryGetValue("$type", out tn);

            if (found == false && type == typeof(System.Object))
            {
                return d;  // CreateDataset(d, globaltypes);
            }

            if (found)
            {
                if (_globalTypes && globaltypes != null)
                {
                    object tname = "";
                    if (globaltypes != null && globaltypes.TryGetValue((string)tn, out tname))
                        tn = tname;
                }
                type = Reflection.Instance.GetTypeFromCache((string)tn, true);
            }

            if (type == null)
                throw new Exception("Cannot determine type");

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

            Dictionary<string, myPropInfo> props = Reflection.Instance.Getproperties(type, typename, _params.ShowReadOnlyProperties);//, Reflection.Instance.IsTypeRegistered(type));
            foreach (var kv in d)
            {
                var n = kv.Key;
                var v = kv.Value;
                string name = n.ToLowerInvariant();
                myPropInfo pi;
                if (props.TryGetValue(name, out pi) == false)
                    continue;
                if (pi.CanWrite)
                {
                    //object v = d[n];

                    if (v != null)
                    {
                        object oset = v;
                        if (v is typedarray)
                        {
                            oset = ParseTypedArray(globaltypes, v);
                        }
                        else
                        {
                            switch (pi.Type)
                            {
                                case myPropInfoType.DataSet:
                                    oset = CreateDataset((Dictionary<string, object>)v, globaltypes);
                                    break;
                                case myPropInfoType.DataTable:
                                    oset = CreateDataTable((Dictionary<string, object>)v, globaltypes);
                                    break;
                                case myPropInfoType.Custom:
                                    oset = Reflection.Instance.CreateCustom((string)v, pi.pt);
                                    break;
                                case myPropInfoType.Enum:
                                    oset = CreateEnum(pi.pt, v);
                                    break;
                                case myPropInfoType.StringKeyDictionary:
                                    oset = CreateStringKeyDictionary((Dictionary<string, object>)v, pi.pt, pi.GenericTypes, globaltypes);
                                    break;
                                case myPropInfoType.Hashtable:
                                case myPropInfoType.Dictionary:
                                    oset = CreateDictionary((List<object>)v, pi.pt, pi.GenericTypes, globaltypes);
                                    break;
                                case myPropInfoType.NameValue: oset = CreateNV((Dictionary<string, object>)v); break;
                                case myPropInfoType.StringDictionary: oset = CreateSD((Dictionary<string, object>)v); break;
                                case myPropInfoType.Array:
                                    oset = CreateArray((List<object>)v, pi.pt, pi.bt, globaltypes);
                                    break;
                                default:
                                    {
                                        if (pi.IsGenericType && pi.IsValueType == false)
                                            oset = CreateGenericList((List<object>)v, pi.pt, pi.bt, globaltypes);
                                        else if ((pi.IsClass || pi.IsStruct || pi.IsInterface) && v is Dictionary<string, object>)
                                        {
                                            var oo = (Dictionary<string, object>)v;
                                            if (oo.ContainsKey("$schema"))
                                                oset = CreateDataset(oo, globaltypes);
                                            else
                                                oset = ParseDictionary(oo, globaltypes, pi.pt, input);
                                        }
                                        else if (v is List<object>)
                                            oset = CreateArray((List<object>)v, pi.pt, typeof(object), globaltypes);
                                        break;
                                    }
                            }
                        }
                        o = pi.setter(o, oset);
                    }
                }
            }
            return o;
        }

        private object ParseTypedArray(Dictionary<string, object> globaltypes, object v)
        {
            object oset;
            var ta = (typedarray)v;
            var t = Reflection.Instance.GetTypeFromCache(ta.typename, true);
            IList a = Array.CreateInstance(t, ta.count);
            int i = 0;
            foreach (var dd in ta.data)
            {
                object oo = null;
                if (dd == null)
                    oo = null;
                else if (dd is typedarray)
                    oo = ParseTypedArray(globaltypes, dd);
                else if (dd is Dictionary<string, object>)
                    oo = ParseDictionary((Dictionary<string, object>)dd, globaltypes, t, null);
                else if (dd is List<object>)
                    oo = CreateArray((List<object>)dd, t, t.GetElementType(), globaltypes);
                else
                    oo = dd;
                a[i++] = oo;
            }
            oset = a;
            return oset;
        }

        private StringDictionary CreateSD(Dictionary<string, object> d)
        {
            StringDictionary nv = new StringDictionary();

            foreach (var o in d)
                nv.Add(o.Key, (string)o.Value);

            return nv;
        }

        private NameValueCollection CreateNV(Dictionary<string, object> d)
        {
            NameValueCollection nv = new NameValueCollection();

            foreach (var o in d)
                nv.Add(o.Key, (string)o.Value);

            return nv;
        }

        private object CreateEnum(Type pt, object v)
        {
            // FEATURE : optimize create enum
            return Enum.Parse(pt, v.ToString());
        }

        private object CreateArray(List<object> data, Type pt, Type bt, Dictionary<string, object> globalTypes)
        {
            if (bt == null)
                bt = typeof(object);

            Array col = Array.CreateInstance(bt, data.Count);
            var arraytype = bt.GetElementType();
            // create an array of objects
            for (int i = 0; i < data.Count; i++)// each (object ob in data)
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
                    col.SetValue(ob, i);
            }

            return col;
        }

        private object CreateGenericList(List<object> data, Type pt, Type bt, Dictionary<string, object> globalTypes)
        {
            if (pt != typeof(object))
            {
                IList col = (IList)Reflection.Instance.FastCreateList(pt, data.Count);
                // create an array of objects
                foreach (object ob in data)
                {
                    if (ob is IDictionary)
                        col.Add(ParseDictionary((Dictionary<string, object>)ob, globalTypes, bt, null));

                    else if (ob is List<object>)
                    {
                        if (bt.IsGenericType)
                            col.Add((List<object>)ob);
                        else
                            col.Add(((List<object>)ob).ToArray());
                    }
                    else if (ob is typedarray)
                        col.Add(((typedarray)ob).data.ToArray());
                    else
                        col.Add(ob);
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
            var ga = Reflection.Instance.GetGenericArguments(t2);
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
                    val = values.Value;

                col.Add(key, val);
            }

            return col;
        }

        private object CreateDictionary(List<object> reader, Type pt, Type[] types, Dictionary<string, object> globalTypes)
        {
            IDictionary col = (IDictionary)Reflection.Instance.FastCreateInstance(pt);
            Type t1 = null;
            Type t2 = null;
            if (types != null)
            {
                t1 = types[0];
                t2 = types[1];
            }

            foreach (Dictionary<string, object> values in reader)
            {
                object key = values["k"];
                object val = values["v"];

                if (key is Dictionary<string, object>)
                    key = ParseDictionary((Dictionary<string, object>)key, globalTypes, t1, null);

                if (typeof(IDictionary).IsAssignableFrom(t2))
                    val = RootDictionary(val, t2);

                else if (val is Dictionary<string, object>)
                    val = ParseDictionary((Dictionary<string, object>)val, globalTypes, t2, null);

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
                    ds.Tables[ms.Info[i]].Columns.Add(ms.Info[i + 1], Type.GetType(ms.Info[i + 2]));
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

            foreach (List<object> row in rows)
            {
                object[] v = new object[row.Count];
                row.CopyTo(v, 0);
                dt.Rows.Add(v);
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
                var ms = (DatasetSchema)this.ParseDictionary((Dictionary<string, object>)schema, globalTypes, typeof(DatasetSchema), null);
                dt.TableName = ms.Info[0];
                for (int i = 0; i < ms.Info.Count; i += 3)
                {
                    dt.Columns.Add(ms.Info[i + 1], Type.GetType(ms.Info[i + 2]));
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

    }

#endregion

#region BJSONPArser
    internal sealed class BJsonParser
    {
        readonly byte[] _json;
        int _index;
        bool _useUTC = true;
        bool _v1_4TA = false;

        internal BJsonParser(byte[] json, bool useUTC, bool v1_4TA)
        {
            this._json = json;
            _v1_4TA = v1_4TA;
            _useUTC = useUTC;
        }

        public object Decode()
        {
            bool b = false;
            return ParseValue(out b);
        }

        private Dictionary<string, object> ParseObject()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(10);
            bool breakparse = false;
            while (!breakparse)
            {
                byte t = GetToken();
                if (t == TOKENS.COMMA)
                    continue;
                if (t == TOKENS.DOC_END)
                    break;
                if (t == TOKENS.TYPES_POINTER)
                {
                    // save curr index position
                    int savedindex = _index;
                    // set index = pointer 
                    _index = ParseInt();
                    t = GetToken();
                    // read $types
                    breakparse = readkeyvalue(dic, ref t);
                    // set index = saved + 4
                    _index = savedindex + 4;
                }
                else
                    breakparse = readkeyvalue(dic, ref t);
            }
            return dic;
        }

        private bool readkeyvalue(Dictionary<string, object> dic, ref byte t)
        {
            bool breakparse;
            string key = "";
            //if (t != TOKENS.NAME)
            if (t == TOKENS.NAME)
                key = ParseName();
            else if (t == TOKENS.NAME_UNI)
                key = ParseName2();
            else
                throw new Exception("excpecting a name field");

            t = GetToken();
            if (t != TOKENS.COLON)
                throw new Exception("expecting a colon");
            object val = ParseValue(out breakparse);

            if (breakparse == false)
                dic.Add(key, val);

            return breakparse;
        }

        private string ParseName2() // unicode byte len string -> <128 len chars
        {
            byte c = _json[_index++];
            string s = Reflection.UnicodeGetString(_json, _index, c);
            _index += c;
            return s;
        }

        private string ParseName()
        {
            byte c = _json[_index++];
            string s = Reflection.UTF8GetString(_json, _index, c);
            _index += c;
            return s;
        }

        private List<object> ParseArray()
        {
            List<object> array = new List<object>();

            bool breakparse = false;
            while (!breakparse)
            {
                object o = ParseValue(out breakparse);
                byte t = 0;
                if (breakparse == false)
                {
                    array.Add(o);
                    t = GetToken();
                }
                else t = (byte)o;

                if (t == TOKENS.COMMA)
                    continue;
                if (t == TOKENS.ARRAY_END)
                    break;
            }
            return array;
        }

        private object ParseValue(out bool breakparse)
        {
            byte t = GetToken();
            breakparse = false;
            switch (t)
            {
                case TOKENS.BYTE:
                    return ParseByte();
                case TOKENS.BYTEARRAY:
                    return ParseByteArray();
                case TOKENS.CHAR:
                    return ParseChar();
                case TOKENS.DATETIME:
                    return ParseDateTime();
                case TOKENS.DECIMAL:
                    return ParseDecimal();
                case TOKENS.DOUBLE:
                    return ParseDouble();
                case TOKENS.FLOAT:
                    return ParseFloat();
                case TOKENS.GUID:
                    return ParseGuid();
                case TOKENS.INT:
                    return ParseInt();
                case TOKENS.LONG:
                    return ParseLong();
                case TOKENS.SHORT:
                    return ParseShort();
                case TOKENS.UINT:
                    return ParseUint();
                case TOKENS.ULONG:
                    return ParseULong();
                case TOKENS.USHORT:
                    return ParseUShort();
                case TOKENS.UNICODE_STRING:
                    return ParseUnicodeString();
                case TOKENS.STRING:
                    return ParseString();
                case TOKENS.DOC_START:
                    return ParseObject();
                case TOKENS.ARRAY_START:
                    return ParseArray();
                case TOKENS.TRUE:
                    return true;
                case TOKENS.FALSE:
                    return false;
                case TOKENS.NULL:
                    return null;
                case TOKENS.ARRAY_END:
                    breakparse = true;
                    return TOKENS.ARRAY_END;
                case TOKENS.DOC_END:
                    breakparse = true;
                    return TOKENS.DOC_END;
                case TOKENS.COMMA:
                    breakparse = true;
                    return TOKENS.COMMA;
                case TOKENS.ARRAY_TYPED:
                case TOKENS.ARRAY_TYPED_LONG:
                    return ParseTypedArray(t);
                case TOKENS.TIMESPAN:
                    return ParsTimeSpan();
            }

            throw new Exception("Unrecognized token at index = " + _index);
        }

        private TimeSpan ParsTimeSpan()
        {
            long l = Helper.ToInt64(_json, _index);
            _index += 8;

            TimeSpan dt = new TimeSpan(l);

            return dt;
        }

        private object ParseTypedArray(byte token)
        {
            typedarray ar = new typedarray();
            if (token == TOKENS.ARRAY_TYPED)
            {
                if (_v1_4TA)
                    ar.typename = ParseName(); 
                else
                    ar.typename = ParseName2();
            }
            else
                ar.typename = ParseNameLong();

            ar.count = ParseInt();

            bool breakparse = false;
            while (!breakparse)
            {
                object o = ParseValue(out breakparse);
                byte b = 0;
                if (breakparse == false)
                {
                    ar.data.Add(o);
                    b = GetToken();
                }
                else b = (byte)o;

                if (b == TOKENS.COMMA)
                    continue;
                if (b == TOKENS.ARRAY_END)
                    break;
            }
            return ar;
        }

        private string ParseNameLong() // unicode short len string -> <32k chars
        {
            short c = Helper.ToInt16(_json, _index);
            _index += 2;
            string s = Reflection.UnicodeGetString(_json, _index, c);
            _index += c;
            return s;
        }

        private object ParseChar()
        {
            short u = Helper.ToInt16(_json, _index);
            _index += 2;
            return u;
        }

        private Guid ParseGuid()
        {
            byte[] b = new byte[16];
            Buffer.BlockCopy(_json, _index, b, 0, 16);
            _index += 16;
            return new Guid(b);
        }

        private float ParseFloat()
        {
            float f = BitConverter.ToSingle(_json, _index);
            _index += 4;
            return f;
        }

        private ushort ParseUShort()
        {
            ushort u = (ushort)Helper.ToInt16(_json, _index);
            _index += 2;
            return u;
        }

        private ulong ParseULong()
        {
            ulong u = (ulong)Helper.ToInt64(_json, _index);
            _index += 8;
            return u;
        }

        private uint ParseUint()
        {
            uint u = (uint)Helper.ToInt32(_json, _index);
            _index += 4;
            return u;
        }

        private short ParseShort()
        {
            short u = (short)Helper.ToInt16(_json, _index);
            _index += 2;
            return u;
        }

        private long ParseLong()
        {
            long u = (long)Helper.ToInt64(_json, _index);
            _index += 8;
            return u;
        }

        private int ParseInt()
        {
            int u = (int)Helper.ToInt32(_json, _index);
            _index += 4;
            return u;
        }

        private double ParseDouble()
        {
            double d = BitConverter.ToDouble(_json, _index);
            _index += 8;
            return d;
        }

        private object ParseUnicodeString()
        {
            int c = Helper.ToInt32(_json, _index);
            _index += 4;

            string s = Reflection.UnicodeGetString(_json, _index, c);
            _index += c;
            return s;
        }

        private string ParseString()
        {
            int c = Helper.ToInt32(_json, _index);
            _index += 4;

            string s = Reflection.UTF8GetString(_json, _index, c);
            _index += c;
            return s;
        }

        private decimal ParseDecimal()
        {
            int[] i = new int[4];
            i[0] = Helper.ToInt32(_json, _index);
            _index += 4;
            i[1] = Helper.ToInt32(_json, _index);
            _index += 4;
            i[2] = Helper.ToInt32(_json, _index);
            _index += 4;
            i[3] = Helper.ToInt32(_json, _index);
            _index += 4;

            return new decimal(i);
        }

        private DateTime ParseDateTime()
        {
            long l = Helper.ToInt64(_json, _index);
            _index += 8;

            DateTime dt = new DateTime(l);
            if (_useUTC)
                dt = dt.ToLocalTime(); // to local time

            return dt;
        }

        private byte[] ParseByteArray()
        {
            int c = Helper.ToInt32(_json, _index);
            _index += 4;
            byte[] b = new byte[c];
            Buffer.BlockCopy(_json, _index, b, 0, c);
            _index += c;
            return b;
        }

        private byte ParseByte()
        {
            return _json[_index++];
        }

        private byte GetToken()
        {
            byte b = _json[_index++];
            return b;
        }
    }

#endregion

#region BJsonSerializer
    internal sealed class BJSONSerializer : IDisposable
    {
        private MemoryStream _output = new MemoryStream();
        //private MemoryStream _before = new MemoryStream();
        private int _typespointer = 0;
        private int _MAX_DEPTH = 20;
        int _current_depth = 0;
        private Dictionary<string, int> _globalTypes = new Dictionary<string, int>();
        private Dictionary<object, int> _cirobj = new Dictionary<object, int>();
        private BJSONParameters _params;

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _output.Close();
                //_before.Close();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal BJSONSerializer(BJSONParameters param)
        {
            _params = param;
            _MAX_DEPTH = param.SerializerMaxDepth;
        }

        internal byte[] ConvertToBJSON(object obj)
        {
            WriteValue(obj);

            // add $types
            if (_params.UsingGlobalTypes && _globalTypes != null && _globalTypes.Count > 0)
            {
                var pointer = (int)_output.Length;
                WriteName("$types");
                WriteColon();
                WriteTypes(_globalTypes);
                //var i = _output.Length;
                _output.Seek(_typespointer, SeekOrigin.Begin);
                _output.Write(Helper.GetBytes(pointer, false), 0, 4);

                return _output.ToArray();
            }

            return _output.ToArray();
        }

        private void WriteTypes(Dictionary<string, int> dic)
        {
            _output.WriteByte(TOKENS.DOC_START);

            bool pendingSeparator = false;

            foreach (var entry in dic)
            {
                if (pendingSeparator) WriteComma();

                WritePair(entry.Value.ToString(), entry.Key);

                pendingSeparator = true;
            }
            _output.WriteByte(TOKENS.DOC_END);
        }

        private void WriteValue(object obj)
        {
            if (obj == null || obj is DBNull)
                WriteNull();

            else if (obj is string)
                WriteString((string)obj);

            else if (obj is char)
                WriteChar((char)obj);

            else if (obj is Guid)
                WriteGuid((Guid)obj);

            else if (obj is bool)
                WriteBool((bool)obj);

            else if (obj is int)
                WriteInt((int)obj);

            else if (obj is uint)
                WriteUInt((uint)obj);

            else if (obj is long)
                WriteLong((long)obj);

            else if (obj is ulong)
                WriteULong((ulong)obj);

            else if (obj is decimal)
                WriteDecimal((decimal)obj);

            else if (obj is sbyte)
                WriteSByte((sbyte)obj);

            else if (obj is byte)
                WriteByte((byte)obj);

            else if (obj is double)
                WriteDouble((double)obj);

            else if (obj is float)
                WriteFloat((float)obj);

            else if (obj is short)
                WriteShort((short)obj);

            else if (obj is ushort)
                WriteUShort((ushort)obj);

            else if (obj is DateTime)
                WriteDateTime((DateTime)obj);

            else if (obj is TimeSpan)
                WriteTimeSpan((TimeSpan)obj);
            else if (obj is IDictionary && obj.GetType().IsGenericType && obj.GetType().GetGenericArguments()[0] == typeof(string))
                WriteStringDictionary((IDictionary)obj);

            else if (obj is IDictionary)
                WriteDictionary((IDictionary)obj);
            else if (obj is DataSet)
                WriteDataset((DataSet)obj);

            else if (obj is DataTable)
                WriteDataTable((DataTable)obj);
            else if (obj is byte[])
                WriteBytes((byte[])obj);

            else if (obj is StringDictionary)
                WriteSD((StringDictionary)obj);

            else if (obj is NameValueCollection)
                WriteNV((NameValueCollection)obj);

            else if (_params.UseTypedArrays && obj is Array)
                WriteTypedArray((ICollection)obj);

            else if (obj is IEnumerable)
                WriteArray((IEnumerable)obj);

            else if (obj is Enum)
                WriteEnum((Enum)obj);

            else if (Reflection.Instance.IsTypeRegistered(obj.GetType()))
                WriteCustom(obj);

            else
                WriteObject(obj);
        }

        private void WriteSByte(sbyte p)
        {
            _output.WriteByte(TOKENS.BYTE);
            byte i = (byte)p;
            _output.WriteByte(i);
        }

        private void WriteTimeSpan(TimeSpan obj)
        {
            _output.WriteByte(TOKENS.TIMESPAN);
            byte[] b = Helper.GetBytes(obj.Ticks, false);
            _output.Write(b, 0, b.Length);
        }

        private void WriteTypedArray(ICollection array)
        {
            bool pendingSeperator = false;
            bool token = true;
            var t = array.GetType();
            if (t.IsGenericType == false)// != null) // non generic array
            {
                //if (t.GetElementType().IsClass)
                {
                    token = false;
                    byte[] b;
                    // array type name
                    if (_params.v1_4TypedArray)
                        b = Reflection.UTF8GetBytes(Reflection.Instance.GetTypeAssemblyName(t.GetElementType()));
                    else
                        b = Reflection.UnicodeGetBytes(Reflection.Instance.GetTypeAssemblyName(t.GetElementType()));
                    if (b.Length < 256)
                    {
                        _output.WriteByte(TOKENS.ARRAY_TYPED);
                        _output.WriteByte((byte)b.Length);
                        _output.Write(b, 0, b.Length);
                    }
                    else
                    {
                        _output.WriteByte(TOKENS.ARRAY_TYPED_LONG);
                        _output.Write(Helper.GetBytes(b.Length, false), 0, 2);
                        _output.Write(b, 0, b.Length);
                    }
                    // array count
                    _output.Write(Helper.GetBytes(array.Count, false), 0, 4); //count
                }
            }
            if (token)
                _output.WriteByte(TOKENS.ARRAY_START);

            foreach (object obj in array)
            {
                if (pendingSeperator) WriteComma();

                WriteValue(obj);

                pendingSeperator = true;
            }
            _output.WriteByte(TOKENS.ARRAY_END);
        }

        private void WriteNV(NameValueCollection nameValueCollection)
        {
            _output.WriteByte(TOKENS.DOC_START);

            bool pendingSeparator = false;

            foreach (string key in nameValueCollection)
            {
                if (pendingSeparator) _output.WriteByte(TOKENS.COMMA);

                WritePair(key, nameValueCollection[key]);

                pendingSeparator = true;
            }
            _output.WriteByte(TOKENS.DOC_END);
        }

        private void WriteSD(StringDictionary stringDictionary)
        {
            _output.WriteByte(TOKENS.DOC_START);

            bool pendingSeparator = false;

            foreach (DictionaryEntry entry in stringDictionary)
            {
                if (pendingSeparator) _output.WriteByte(TOKENS.COMMA);

                WritePair((string)entry.Key, entry.Value);

                pendingSeparator = true;
            }
            _output.WriteByte(TOKENS.DOC_END);
        }

        private void WriteUShort(ushort p)
        {
            _output.WriteByte(TOKENS.USHORT);
            _output.Write(Helper.GetBytes(p, false), 0, 2);
        }

        private void WriteShort(short p)
        {
            _output.WriteByte(TOKENS.SHORT);
            _output.Write(Helper.GetBytes(p, false), 0, 2);
        }

        private void WriteFloat(float p)
        {
            _output.WriteByte(TOKENS.FLOAT);
            byte[] b = BitConverter.GetBytes(p);
            _output.Write(b, 0, b.Length);
        }

        private void WriteDouble(double p)
        {
            _output.WriteByte(TOKENS.DOUBLE);
            var b = BitConverter.GetBytes(p);
            _output.Write(b, 0, b.Length);
        }

        private void WriteByte(byte p)
        {
            _output.WriteByte(TOKENS.BYTE);
            _output.WriteByte(p);
        }

        private void WriteDecimal(decimal p)
        {
            _output.WriteByte(TOKENS.DECIMAL);
            var b = decimal.GetBits(p);
            foreach (var c in b)
                _output.Write(Helper.GetBytes(c, false), 0, 4);
        }

        private void WriteULong(ulong p)
        {
            _output.WriteByte(TOKENS.ULONG);
            _output.Write(Helper.GetBytes((long)p, false), 0, 8);
        }

        private void WriteUInt(uint p)
        {
            _output.WriteByte(TOKENS.UINT);
            _output.Write(Helper.GetBytes(p, false), 0, 4);
        }

        private void WriteLong(long p)
        {
            _output.WriteByte(TOKENS.LONG);
            _output.Write(Helper.GetBytes(p, false), 0, 8);
        }

        private void WriteChar(char p)
        {
            _output.WriteByte(TOKENS.CHAR);
            _output.Write(Helper.GetBytes((short)p, false), 0, 2);
        }

        private void WriteBytes(byte[] p)
        {
            _output.WriteByte(TOKENS.BYTEARRAY);
            _output.Write(Helper.GetBytes(p.Length, false), 0, 4);
            _output.Write(p, 0, p.Length);
        }

        private void WriteBool(bool p)
        {
            if (p)
                _output.WriteByte(TOKENS.TRUE);
            else
                _output.WriteByte(TOKENS.FALSE);
        }

        private void WriteNull()
        {
            _output.WriteByte(TOKENS.NULL);
        }


        private void WriteCustom(object obj)
        {
            Reflection.Serialize s;
            Reflection.Instance._customSerializer.TryGetValue(obj.GetType(), out s);
            WriteString(s(obj));
        }

        private void WriteColon()
        {
            _output.WriteByte(TOKENS.COLON);
        }

        private void WriteComma()
        {
            _output.WriteByte(TOKENS.COMMA);
        }

        private void WriteEnum(Enum e)
        {
            WriteString(e.ToString());
        }

        private void WriteInt(int i)
        {
            //if (_params.OptimizeSize)
            //{
            //    if (i < 256)
            //    {
            //        WriteByte((byte)i);
            //        return;
            //    }
            //    else if (i < 65536)
            //    {
            //        WriteUShort((ushort)i);
            //        return;
            //    }
            //}
            _output.WriteByte(TOKENS.INT);
            _output.Write(Helper.GetBytes(i, false), 0, 4);
        }

        private void WriteGuid(Guid g)
        {
            _output.WriteByte(TOKENS.GUID);
            _output.Write(g.ToByteArray(), 0, 16);
        }

        private void WriteDateTime(DateTime dateTime)
        {
            DateTime dt = dateTime;
            if (_params.UseUTCDateTime)
                dt = dateTime.ToUniversalTime();

            _output.WriteByte(TOKENS.DATETIME);
            byte[] b = Helper.GetBytes(dt.Ticks, false);
            _output.Write(b, 0, b.Length);
        }

#if !SILVERLIGHT
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
            _output.WriteByte(TOKENS.DOC_START);
            {
                WritePair("$schema", _params.UseOptimizedDatasetSchema ? (object)GetSchema(ds) : ds.GetXmlSchema());
                WriteComma();
            }
            bool tablesep = false;
            foreach (DataTable table in ds.Tables)
            {
                if (tablesep) WriteComma();
                tablesep = true;
                WriteDataTableData(table);
            }
            // end dataset
            _output.WriteByte(TOKENS.DOC_END);
        }

        private void WriteDataTableData(DataTable table)
        {
            WriteName(table.TableName);
            WriteColon();
            _output.WriteByte(TOKENS.ARRAY_START);
            DataColumnCollection cols = table.Columns;
            bool rowseparator = false;
            foreach (DataRow row in table.Rows)
            {
                if (rowseparator) WriteComma();
                rowseparator = true;
                _output.WriteByte(TOKENS.ARRAY_START);

                bool pendingSeperator = false;
                foreach (DataColumn column in cols)
                {
                    if (pendingSeperator) WriteComma();
                    WriteValue(row[column]);
                    pendingSeperator = true;
                }
                _output.WriteByte(TOKENS.ARRAY_END);
            }

            _output.WriteByte(TOKENS.ARRAY_END);
        }

        void WriteDataTable(DataTable dt)
        {
            _output.WriteByte(TOKENS.DOC_START);
            //if (this.useExtension)
            {
                this.WritePair("$schema", _params.UseOptimizedDatasetSchema ? (object)this.GetSchema(dt) : this.GetXmlSchema(dt));
                WriteComma();
            }

            WriteDataTableData(dt);

            // end datatable
            _output.WriteByte(TOKENS.DOC_END);
        }
#endif
        bool _TypesWritten = false;

        private void WriteObject(object obj)
        {
            int i = 0;
            if (_cirobj.TryGetValue(obj, out i) == false)
                _cirobj.Add(obj, _cirobj.Count + 1);
            else
            {
                if (_current_depth > 0)
                {
                    //_circular = true;
                    _output.WriteByte(TOKENS.DOC_START);
                    WriteName("$i");
                    WriteColon();
                    WriteValue(i);
                    _output.WriteByte(TOKENS.DOC_END);
                    return;
                }
            }
            if (_params.UsingGlobalTypes == false)
                _output.WriteByte(TOKENS.DOC_START);
            else
            {
                if (_TypesWritten == false)
                {
                    _output.WriteByte(TOKENS.DOC_START);
                    // write pointer to $types position
                    _output.WriteByte(TOKENS.TYPES_POINTER);
                    _typespointer = (int)_output.Length; // place holder
                    _output.Write(new byte[4], 0, 4); // zero pointer for now
                                                      //_output = new MemoryStream();
                    _TypesWritten = true;
                }
                else
                    _output.WriteByte(TOKENS.DOC_START);

            }
            _current_depth++;
            if (_current_depth > _MAX_DEPTH)
                throw new Exception("Serializer encountered maximum depth of " + _MAX_DEPTH);

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
                var o = p.Getter(obj);
                if (_params.SerializeNulls == false && (o == null || o is DBNull))
                {

                }
                else
                {
                    if (append)
                        WriteComma();
                    WritePair(p.Name, o);
                    append = true;
                }
            }
            _output.WriteByte(TOKENS.DOC_END);
            _current_depth--;
        }

        private void WritePairFast(string name, string value)
        {
            if (_params.SerializeNulls == false && (value == null))
                return;
            WriteName(name);

            WriteColon();

            WriteString(value);
        }

        private void WritePair(string name, object value)
        {
            if (_params.SerializeNulls == false && (value == null || value is DBNull))
                return;
            WriteName(name);

            WriteColon();

            WriteValue(value);
        }

        private void WriteArray(IEnumerable array)
        {
            _output.WriteByte(TOKENS.ARRAY_START);

            bool pendingSeperator = false;

            foreach (object obj in array)
            {
                if (pendingSeperator) WriteComma();

                WriteValue(obj);

                pendingSeperator = true;
            }
            _output.WriteByte(TOKENS.ARRAY_END);
        }

        private void WriteStringDictionary(IDictionary dic)
        {
            _output.WriteByte(TOKENS.DOC_START);

            bool pendingSeparator = false;

            foreach (DictionaryEntry entry in dic)
            {
                if (pendingSeparator) WriteComma();

                WritePair((string)entry.Key, entry.Value);

                pendingSeparator = true;
            }
            _output.WriteByte(TOKENS.DOC_END);
        }

        private void WriteStringDictionary(IDictionary<string, object> dic)
        {
            _output.WriteByte(TOKENS.DOC_START);

            bool pendingSeparator = false;

            foreach (KeyValuePair<string, object> entry in dic)
            {
                if (pendingSeparator) WriteComma();

                WritePair((string)entry.Key, entry.Value);

                pendingSeparator = true;
            }
            _output.WriteByte(TOKENS.DOC_END);
        }

        private void WriteDictionary(IDictionary dic)
        {
            _output.WriteByte(TOKENS.ARRAY_START);

            bool pendingSeparator = false;

            foreach (DictionaryEntry entry in dic)
            {
                if (pendingSeparator) WriteComma();
                _output.WriteByte(TOKENS.DOC_START);
                WritePair("k", entry.Key);
                WriteComma();
                WritePair("v", entry.Value);
                _output.WriteByte(TOKENS.DOC_END);

                pendingSeparator = true;
            }
            _output.WriteByte(TOKENS.ARRAY_END);
        }

        private void WriteName(string s)
        {
            byte[] b;
            if (_params.UseUnicodeStrings == false)
            {
                _output.WriteByte(TOKENS.NAME);
                b = Reflection.UTF8GetBytes(s);
            }
            else
            {
                _output.WriteByte(TOKENS.NAME_UNI);
                b = Reflection.UnicodeGetBytes(s);
            }
            _output.WriteByte((byte)b.Length);
            _output.Write(b, 0, b.Length % 256);
        }

        private void WriteString(string s)
        {
            byte[] b = null;
            if (_params.UseUnicodeStrings)
            {
                _output.WriteByte(TOKENS.UNICODE_STRING);
                b = Reflection.UnicodeGetBytes(s);
            }
            else
            {
                _output.WriteByte(TOKENS.STRING);
                b = Reflection.UTF8GetBytes(s);
            }
            _output.Write(Helper.GetBytes(b.Length, false), 0, 4);
            _output.Write(b, 0, b.Length);
        }
    }

#endregion

#region FILE GETTER.CS    
    public sealed class DatasetSchema
    {
        public List<string> Info ;//{ get; set; }
        public string Name ;//{ get; set; }
    }
#endregion
  
#region FILE Reflection.cs    
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
#if !SILVERLIGHT
        Hashtable,
        DataSet,
        DataTable,
#endif
        Custom,
        Unknown,
    }

    public class myPropInfo
    {
        public Type pt;
        public Type bt;
        public Type changeType;
        public Reflection.GenericSetter setter;
        public Reflection.GenericGetter getter;
        public Type[] GenericTypes;
        public string Name;
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
        static Reflection()
        {
        }
        private Reflection()
        {
        }
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
        private static SafeDictionary<short, OpCode> _opCodes;
        private static List<string> _blacklistTypes = new List<string>()
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
        public static byte[] UTF8GetBytes(string str)
        {
            return utf8.GetBytes(str);
        }

        public static string UTF8GetString(byte[] bytes, int offset, int len)
        {
            return utf8.GetString(bytes, offset, len);
        }

        public unsafe static byte[] UnicodeGetBytes(string str)
        {
            int len = str.Length * 2;
            byte[] b = new byte[len];
            fixed (void* ptr = str)
            {
                System.Runtime.InteropServices.Marshal.Copy(new IntPtr(ptr), b, 0, len);
            }
            return b;
        }

        public static string UnicodeGetString(byte[] b)
        {
            return UnicodeGetString(b, 0, b.Length);
        }

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
            if (_propertycache.TryGetValue(typename, out sd))
            {
                return sd;
            }
            else
            {
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

                    sd.Add(p.Name.ToLowerInvariant(), d);
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

                        sd.Add(f.Name.ToLowerInvariant(), d);
                    }
                }

                _propertycache.Add(typename, sd);
                return sd;
            }
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
                d.bt = t.GetElementType();
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
#if !SILVERLIGHT
            else if (t == typeof(Hashtable)) d_type = myPropInfoType.Hashtable;
            else if (t == typeof(DataSet)) d_type = myPropInfoType.DataSet;
            else if (t == typeof(DataTable)) d_type = myPropInfoType.DataTable;
#endif
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
                string s = t.AssemblyQualifiedName;
                _tyname.Add(t, s);
                return s;
            }
        }

        internal Type GetTypeFromCache(string typename, bool blacklistChecking)
        {
            Type val = null;
            if (_typecache.TryGetValue(typename, out val))
                return val;
            else
            {
                // check for BLACK LIST types -> more secure when using $type
                if (blacklistChecking)
                {
                    var tn = typename.Trim().ToLowerInvariant();
                    foreach (var s in _blacklistTypes)
                        if (tn.StartsWith(s, StringComparison.Ordinal))
                            throw new Exception("Black list type encountered, possible attack vector when using $type : " + typename);
                }

                Type t = Type.GetType(typename);

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
                if (_constrcache.TryGetValue(objtype, out c))
                {
                    return c();
                }
                else
                {
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

        internal void ResetPropertyCache()
        {
            _propertycache = new SafeDictionary<string, Dictionary<string, myPropInfo>>();
        }

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

#region FILE SafeDictioary
   internal sealed class SafeDictionary<TKey, TValue>
    {
        private readonly object _Padlock = new object();
        private readonly Dictionary<TKey, TValue> _Dictionary;

        public SafeDictionary(int capacity)
        {
            _Dictionary = new Dictionary<TKey, TValue>(capacity);
        }

        public SafeDictionary()
        {
            _Dictionary = new Dictionary<TKey, TValue>();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (_Padlock)
                return _Dictionary.TryGetValue(key, out value);
        }

        public int Count()
        {
            lock (_Padlock) return _Dictionary.Count;
        }

        public TValue this[TKey key]
        {
            get
            {
                lock (_Padlock)
                    return _Dictionary[key];
            }
            set
            {
                lock (_Padlock)
                    _Dictionary[key] = value;
            }
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

    internal static class Helper
    {
        internal static unsafe int ToInt32(byte[] value, int startIndex, bool reverse)
        {
            if (reverse)
            {
                byte[] b = new byte[4];
                Buffer.BlockCopy(value, startIndex, b, 0, 4);
                Array.Reverse(b);
                return ToInt32(b, 0);
            }

            return ToInt32(value, startIndex);
        }

        internal static unsafe int ToInt32(byte[] value, int startIndex)
        {
            fixed (byte* numRef = &(value[startIndex]))
            {
                return *((int*)numRef);
            }
        }

        internal static unsafe long ToInt64(byte[] value, int startIndex, bool reverse)
        {
            if (reverse)
            {
                byte[] b = new byte[8];
                Buffer.BlockCopy(value, startIndex, b, 0, 8);
                Array.Reverse(b);
                return ToInt64(b, 0);
            }
            return ToInt64(value, startIndex);
        }

        internal static unsafe long ToInt64(byte[] value, int startIndex)
        {
            fixed (byte* numRef = &(value[startIndex]))
            {
                return *(((long*)numRef));
            }
        }

        internal static unsafe short ToInt16(byte[] value, int startIndex, bool reverse)
        {
            if (reverse)
            {
                byte[] b = new byte[2];
                Buffer.BlockCopy(value, startIndex, b, 0, 2);
                Array.Reverse(b);
                return ToInt16(b, 0);
            }
            return ToInt16(value, startIndex);
        }

        internal static unsafe short ToInt16(byte[] value, int startIndex)
        {
            fixed (byte* numRef = &(value[startIndex]))
            {
                return *(((short*)numRef));
            }
        }

        internal static unsafe byte[] GetBytes(long num, bool reverse)
        {
            byte[] buffer = new byte[8];
            fixed (byte* numRef = buffer)
            {
                *((long*)numRef) = num;
            }
            if (reverse)
                Array.Reverse(buffer);
            return buffer;
        }

        public static unsafe byte[] GetBytes(int num, bool reverse)
        {
            byte[] buffer = new byte[4];
            fixed (byte* numRef = buffer)
            {
                *((int*)numRef) = num;
            }
            if (reverse)
                Array.Reverse(buffer);
            return buffer;
        }
    }

#endregion

#pragma warning restore