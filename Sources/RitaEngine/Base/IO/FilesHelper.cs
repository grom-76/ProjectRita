
namespace RitaEngine.Base.IO;



using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using RitaEngine.Base.IO.Formats.LZMA.SevenZip;

        // public static class FilesHelper{
        //     public static class Compression{
        //         public static void Compress(){}
        //         public static void DeCompress(){}
        //     }
        //     public static class FormatJSON{ 
        //         public static void Serialize(){}
        //         public static void DeSerialize(){}
        //     }
        //     public static class FormatYAML{ 
        //         public static void Read(){}
        //         public static void Write(){}
        //     }
        //     public static class FormatXML{ 
        //         public static void Read(){}
        //         public static void Write(){}
        //     }
        //     //helper

        //     //native win64 create file 
        //     //native linux openFile 
        // }

[SkipLocalsInit, StructLayout(LayoutKind.Sequential )]
public static class FilesHelper
{


//  public static string Serialize<Tobject>(Tobject obj, bool withIdent = false) where Tobject : class
//     => withIdent 
//         ? Native.Assets.Files.fastJSON.JSON.ToNiceJSON(obj!)
//         : Native.Assets.Files.fastJSON.JSON.ToJSON(obj!);

//     public static Tobject Deserialize<Tobject>(string json) where Tobject : class
//         => Native.Assets.Files.fastJSON.JSON.ToObject<Tobject>(json);

//     public static string ReadFile( string file)
//         => System.IO.File.ReadAllText(file);

//     public static void SaveFile( string file , string content)
//         => System.IO.File.WriteAllText( file,content);

//     public static TObject ReadFileAndDeserialize<TObject>(string filename )where TObject : class
//     {
//         var json = ReadFile( filename);
//         return Deserialize<TObject>(json);
//     }

//     public static void SerializeAndSaveFile<TObject>( TObject obj, string filename ,bool withIdent = false )where TObject : class
//     {
//         string str = Serialize<TObject>(obj, withIdent);
//         SaveFile( filename, str);
//     }

    // public static TType CreateInstance<TType>( string name, string? parameter= null)
    // {
    //     Type type = Type.GetType( name /*assemblyQualifiedName*/)!;
    //     // Guard.IsNull(type );

    //     if( parameter != null)
    //         return  (TType)Activator.CreateInstance(type, parameter)!;
        
    //     return  (TType)Activator.CreateInstance(type)!;
    // }

    public static void SearchAllFiles( List<string> result, string sDir)
    {
        try
        {
            foreach (string d in Directory.GetDirectories(sDir))
            {
                foreach (string f in Directory.GetFiles(d))
                {
                    result.Add(f);
                }
                SearchAllFiles(result,d);
            }
        }
        catch (System.Exception excpt)
        {
           Log.Critical(excpt.Message);
        }
    }

      public  static byte[] GetBytecode(string name,string assetPath="")
    {
        return File.ReadAllBytes(Path.Combine(assetPath, $"{name}.spv"));
    }

    public static bool IsFileExist(string file)
    {
        #if WIN 
        return System.IO.File.Exists(file);
        #else
return System.IO.File.Exists(file);
        #endif
    }

    public static bool DeleteFile(string file)
    {
        #if WIN 
        System.IO.File.Delete(file);
        return true;
        #else
        return true;
        #endif
    }

        // <summary>
    ///  Fonction indépendante ajoute en fin de fichier une chaine de caractère, human readable
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="content"></param>
    public static void StringToFile(string filename, string content)
        => System.IO.File.AppendAllTextAsync(filename, content, System.Text.Encoding.Unicode, new System.Threading.CancellationToken());


   



  public static void Compress(byte[] @in, byte[] @out, LzmaSpeed speed = LzmaSpeed.Fastest, DictionarySize dictionarySize = DictionarySize.VerySmall, Action<long, long> onProgress = null!)
        {
            using var input = new MemoryStream(@in);
			using var output = new MemoryStream(@out);
            const int posStateBits = 2; // default: 2
			const int litContextBits = 3; // 3 for normal files, 0; for 32-bit data
			const int litPosBits = 0; // 0 for 64-bit data, 2 for 32-bit.
			var numFastBytes = (int)speed;
			const string matchFinder = "BT4"; // default: BT4
			const bool endMarker = true;

			CoderPropID[] propIDs =
			{
				CoderPropID.DictionarySize,
				CoderPropID.PosStateBits, // (0 <= x <= 4).
				CoderPropID.LitContextBits, // (0 <= x <= 8).
				CoderPropID.LitPosBits, // (0 <= x <= 4).
				CoderPropID.NumFastBytes,
				CoderPropID.MatchFinder, // "BT2", "BT4".
				CoderPropID.EndMarker
			};

			object[] properties =
			{
				(int)dictionarySize,
				posStateBits,
				(int)litContextBits,
				(int)litPosBits,
				numFastBytes,
				matchFinder,
				endMarker
			};

			var lzmaEncoder = new RitaEngine.Base.IO.Formats.LZMA.SevenZip.Compression.LZMA.Encoder();

			lzmaEncoder.SetCoderProperties(propIDs, properties);
			lzmaEncoder.WriteCoderProperties(output);
			var fileSize = input.Length;
			for (int i = 0; i < 8; i++)
				output.WriteByte((byte)(fileSize >> (8 * i)));

			ICodeProgress? prg = null;
			if (onProgress != null)
			{
				prg = new DelegateCodeProgress(onProgress);
			}
			lzmaEncoder.Code(input, output, -1, -1, prg!);
        }

        public static void Decompress(byte[] @in, byte[] @out, Action<long, long> onProgress = null!)
        {
            using var input = new MemoryStream(@in);
			using var output = new MemoryStream(@out);
            var decoder = new  RitaEngine.Base.IO.Formats.LZMA.SevenZip.Compression.LZMA.Decoder();

			byte[] properties = new byte[5];
			if (input.Read(properties, 0, 5) != 5)
			{
				throw new Exception("input .lzma is too short");
			}
			decoder.SetDecoderProperties(properties);

			long fileLength = 0;
			for (int i = 0; i < 8; i++)
			{
				int v = input.ReadByte();
				if (v < 0) throw new Exception("Can't Read 1");
				fileLength |= ((long)(byte)v) << (8 * i);
			}

			ICodeProgress? prg = null;
			if (onProgress != null)
			{
				prg = new DelegateCodeProgress(onProgress);
			}
			long compressedSize = input.Length - input.Position;

			decoder.Code(input, output, compressedSize, fileLength, prg!);
        }

        private class DelegateCodeProgress : ICodeProgress
		{
			private readonly Action<long, long> handler;
			public DelegateCodeProgress(Action<long, long> handler) => this.handler = handler;
			public void SetProgress(long inSize, long outSize) => handler(inSize, outSize);
		}

        /// <summary>
		/// .
		/// </summary>
		public enum LzmaSpeed : int
		{
			/// <summary> . </summary>
			Fastest = 5,
			/// <summary> . </summary>
			VeryFast = 8,
			/// <summary> . </summary>
			Fast = 16,
			/// <summary> . </summary>
			Medium = 32,
			/// <summary> . </summary>
			Slow = 64,
			/// <summary> . </summary>
			VerySlow = 128,
		}

		/// <summary>
		/// .
		/// </summary>
		public enum DictionarySize : int
		{
			///<summary>64 KiB</summary>
			VerySmall = 1 << 16,
			///<summary>1 MiB</summary>
			Small = 1 << 20,
			///<summary>4 MiB</summary>
			Medium = 1 << 22,
			///<summary>8 MiB</summary>
			Large = 1 << 23,
			///<summary>16 MiB</summary>
			Larger = 1 << 24,
			///<summary>64 MiB</summary>
			VeryLarge = 1 << 26,
		}



        
    public static class XMLHelper
    {
        #region XMLDocuments.cs
    /// <summary>
    /// 
    /// </summary>
    public class XmlDocument {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public XmlNode Root { get; }

        XmlDocument(XmlNode root) {
            Root = root;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public static XmlDocument FromRoot(XmlNode root) {
            return new XmlDocument(root);
        }
    }
    #endregion

    #region XmlxNode
    /// <summary>
    /// 
    /// </summary>
    public class XmlNode 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Name { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public List<XmlNode> Childs { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public Dictionary<string, string> Attributes { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nodes"></param>
        public XmlNode(string name, List<XmlNode> nodes = null!) {
            Name = name;
            Childs = nodes ?? new List<XmlNode>();
            Attributes = new Dictionary<string, string>();
        }
    }

    #endregion

    #region XmlFormatException
    /// <summary>
    /// 
    /// </summary>
    public class XmlFormatException : Exception 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public XmlFormatException(string message) : base(message) {}
    }

    #endregion

    #region XmlReader.cs
    /// <summary>
    /// 
    /// </summary>
    public static class XmlReader 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static XmlDocument FromText(string xml) 
        {
            var cursor = 0;
            while ( true ) {
                // Looking for <
                if ( xml[cursor] == '<' ) {
                    var root = TryParseNode(xml, ref cursor);
                    if ( root != null ) {
                        return XmlDocument.FromRoot(root);
                    }
                }
                cursor++;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="cursor"></param>
        /// <returns></returns>
        private static XmlNode TryParseNode(string xml, ref int cursor) 
        {
            cursor++; // <_
            var charAtCursor = xml[cursor];
            // Skipping <? and <! nodes
            if ( (charAtCursor == '?') || (charAtCursor == '!') ) {
                while ( xml[cursor] != '>' ) {
                    cursor++;
                }
                return null!;
            }
            // Skipping close tags
            if ( charAtCursor == '/' ) {
                return null!;
            }
            XmlNode node = null!;
            var nameStart = cursor;
            while ( true ) {
                charAtCursor = xml[cursor];
                if ( IsWhiteSpace(charAtCursor) ) {
                    if ( node == null ) {
                        // <node_
                        node = CreateNode(xml.Substring(nameStart, cursor - nameStart));
                    }
                    cursor++;
                    charAtCursor = xml[cursor];
                    if ( (charAtCursor != '/') && (charAtCursor != '<') ) {
                        AddAttribute(xml, ref cursor, node);
                    }
                    continue;
                }
                if ( charAtCursor == '>' ) {
                    if ( node == null ) {
                        // <node>_
                        node = CreateNode(xml.Substring(nameStart, cursor - nameStart));
                    }
                    cursor++;
                    continue;
                }
                if ( charAtCursor == '<' ) {
                    if ( node != null ) {
                        var subNode = TryParseNode(xml, ref cursor);
                        if ( subNode != null ) {
                            node.Childs.Add(subNode);
                            continue;
                        }
                    }
                    cursor++; // </_
                    var closeNameStart = cursor;
                    while ( xml[cursor] != '>' ) {
                        cursor++;
                    }
                    if ( node == null ) {
                        throw new XmlFormatException("Closing tag without open tag");
                    }
                    if ( !CompareStringWithSubstring(node.Name, xml, closeNameStart, cursor - closeNameStart) ) {
                        throw new XmlFormatException("Closing tag does not match open tag");
                    }
                    cursor++;
                    return node;
                }
                if ( charAtCursor == '/' ) {
                    cursor++;
                    if ( xml[cursor] != '>' ) {
                        throw new XmlFormatException("Unexpected token #3");
                    }
                    // <node />_
                    if ( node == null ) {
                        node = CreateNode(xml.Substring(nameStart, cursor - nameStart - 1));
                    }
                    return node;
                }
                cursor++;
            }
        }

        private static XmlNode CreateNode(string name) =>  new XmlNode(name);
        
        private static void AddAttribute(string xml, ref int cursor, XmlNode node) 
        {
            var nameStart = cursor;
            string name = null!;
            char charAtCursor;
            while ( true ) {
                charAtCursor = xml[cursor];
                if ( IsWhiteSpace(charAtCursor) || (charAtCursor == '=') ) {
                    name = xml.Substring(nameStart, cursor - nameStart);
                    while ( xml[cursor] != '=' ) {
                        cursor++;
                    }
                    break;
                }
                cursor++;
            }
            char valueBrace;
            while ( true ) {
                charAtCursor = xml[cursor];
                if ( (charAtCursor == '"') || (charAtCursor == '\'') ) {
                    valueBrace = xml[cursor];
                    cursor++;
                    break;
                }
                cursor++;
            }
            var valueStart = cursor;
            while ( true ) {
                if ( xml[cursor] == valueBrace ) {
                    var value = xml.Substring(valueStart, cursor - valueStart);
                    node.Attributes.Add(name, value);
                    return;
                }
                cursor++;
            }
        }

        private static bool CompareStringWithSubstring(string str, string strForSubString, int startIndex, int length)
        {
            if ( str.Length != length ) { return false; }

            for ( var i = 0; i < length; i++ ) {
                if ( str[i] != strForSubString[startIndex + i] ) {
                    return false;
                }
            }
            return true;
        }

        static bool IsWhiteSpace(char c) => ( (c == ' ') || (c == '\t') || (c == '\n') || (c == '\r') );
        
    }

    #endregion

    #region xmlwriter.cs
    /// <summary>
    /// 
    /// </summary>
    public static class XmlWriter 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="sb"></param>
        /// <returns></returns>
        public static string ToText(XmlDocument xmlDoc, StringBuilder sb = null!) 
        {
            if ( sb == null ) {
                sb = new StringBuilder();
            } else {
                sb.Clear();
            }
            AppendNode(sb, xmlDoc.Root);
            return sb.ToString();
        }

        private static void AppendNode(StringBuilder sb, XmlNode node) 
        {
            sb.Append("<").Append(node.Name);
            if ( node.Childs.Count > 0 ) {
                AddAttributes(sb, node);
                sb.Append(">");
                foreach ( var child in node.Childs ) {
                    AppendNode(sb, child);
                }
                sb.Append("</").Append(node.Name).Append(">");
            } else {
                AddAttributes(sb, node);
                sb.Append(" />");
            }
        }

        private static void AddAttributes(StringBuilder sb, XmlNode node) {
            foreach ( var attr in node.Attributes ) {
                sb.Append(" ");
                sb.Append(attr.Key);
                sb.Append("=");
                sb.Append("\"").Append(attr.Value).Append("\"");
            }
        }
    }

    #endregion
    }
        /*

    https://github.com/aaubry/YamlDotNet

    Forked from YamlDonet


    https://github.com/xoofx/SharpYaml
    */
    public static class YAMLHelper
    {

    /// <summary>
    /// Generic implementation of object pooling pattern with predefined pool size limit. The main
    /// purpose is that limited number of frequently used objects can be kept in the pool for
    /// further recycling.
    ///
    /// Notes:
    /// 1) it is not the goal to keep all returned objects. Pool is not meant for storage. If there
    ///    is no space in the pool, extra returned objects will be dropped.
    ///
    /// 2) it is implied that if object was obtained from a pool, the caller will return it back in
    ///    a relatively short time. Keeping checked out objects for long durations is ok, but
    ///    reduces usefulness of pooling. Just new up your own.
    ///
    /// Not returning objects to the pool in not detrimental to the pool's work, but is a bad practice.
    /// Rationale:
    ///    If there is no intent for reusing the object, do not use pool - just use "new".
    /// </summary>
    [DebuggerStepThrough]
    internal sealed class ConcurrentObjectPool<T> where T : class
    {
        [DebuggerDisplay("{value,nq}")]
        private struct Element
        {
            internal T? value;
        }

        /// <remarks>
        /// Not using System.Func{T} because this file is linked into the (debugger) Formatter,
        /// which does not have that type (since it compiles against .NET 2.0).
        /// </remarks>
        internal delegate T Factory();

        // Storage for the pool objects. The first item is stored in a dedicated field because we
        // expect to be able to satisfy most requests from it.
        private T? firstItem;
        private readonly Element[] items;

        // factory is stored for the lifetime of the pool. We will call this only when pool needs to
        // expand. compared to "new T()", Func gives more flexibility to implementers and faster
        // than "new T()".
        private readonly Factory factory;

        internal ConcurrentObjectPool(Factory factory)
            : this(factory, Environment.ProcessorCount * 2)
        {
        }

        internal ConcurrentObjectPool(Factory factory, int size)
        {
            // Debug.Assert(size >= 1);
            this.factory = factory;
            items = new Element[size - 1];
        }

        private T CreateInstance()
        {
            var inst = factory();
            return inst;
        }

        /// <summary>
        /// Produces an instance.
        /// </summary>
        /// <remarks>
        /// Search strategy is a simple linear probing which is chosen for it cache-friendliness.
        /// Note that Free will try to store recycled objects close to the start thus statistically
        /// reducing how far we will typically search.
        /// </remarks>
        internal T Allocate()
        {
            // PERF: Examine the first element. If that fails, AllocateSlow will look at the remaining elements.
            // Note that the initial read is optimistically not synchronized. That is intentional.
            // We will interlock only when we have a candidate. in a worst case we may miss some
            // recently returned objects. Not a big deal.
            var inst = firstItem;
            if (inst == null || inst != Interlocked.CompareExchange(ref firstItem, null, inst))
            {
                inst = AllocateSlow();
            }

            return inst;
        }

        private T AllocateSlow()
        {
            var elements = items;

            for (var i = 0; i < elements.Length; i++)
            {
                // Note that the initial read is optimistically not synchronized. That is intentional.
                // We will interlock only when we have a candidate. in a worst case we may miss some
                // recently returned objects. Not a big deal.
                var inst = elements[i].value;
                if (inst != null)
                {
                    if (inst == Interlocked.CompareExchange(ref elements[i].value, null, inst))
                    {
                        return inst;
                    }
                }
            }

            return CreateInstance();
        }

        /// <summary>
        /// Returns objects to the pool.
        /// </summary>
        /// <remarks>
        /// Search strategy is a simple linear probing which is chosen for it cache-friendliness.
        /// Note that Free will try to store recycled objects close to the start thus statistically
        /// reducing how far we will typically search in Allocate.
        /// </remarks>
        internal void Free(T obj)
        {
            Validate(obj);

            if (firstItem == null)
            {
                // Intentionally not using interlocked here.
                // In a worst case scenario two objects may be stored into same slot.
                // It is very unlikely to happen and will only mean that one of the objects will get collected.
                firstItem = obj;
            }
            else
            {
                FreeSlow(obj);
            }
        }

        private void FreeSlow(T obj)
        {
            var elements = items;
            for (var i = 0; i < elements.Length; i++)
            {
                if (elements[i].value == null)
                {
                    // Intentionally not using interlocked here.
                    // In a worst case scenario two objects may be stored into same slot.
                    // It is very unlikely to happen and will only mean that one of the objects will get collected.
                    elements[i].value = obj;
                    break;
                }
            }
        }

        [Conditional("DEBUG")]
        private void Validate(object obj)
        {
            System.Diagnostics.Debug.Assert(obj != null, "freeing null?");

            System.Diagnostics.Debug.Assert(firstItem != obj, "freeing twice?");

            var elements = items;
            for (var i = 0; i < elements.Length; i++)
            {
                var value = elements[i].value;
                if (value == null)
                {
                    return;
                }

                System.Diagnostics.Debug.Assert(value != obj, "freeing twice?");
            }
        }
    }

    // public static class NumberExtensions
    // {
    //     public static bool IsPowerOfTwo(this int value)
    //     {
    //         return (value & (value - 1)) == 0;
    //     }
    // }
    // public static class ExpressionExtensions
    // {
    //     /// <summary>
    //     /// Returns the <see cref="PropertyInfo" /> that describes the property that
    //     /// is being returned in an expression in the form:
    //     /// <code>
    //     ///   x => x.SomeProperty
    //     /// </code>
    //     /// </summary>
    //     public static PropertyInfo AsProperty(this LambdaExpression propertyAccessor)
    //     {
    //         var property = TryGetMemberExpression<PropertyInfo>(propertyAccessor);
    //         if (property == null)
    //         {
    //             throw new ArgumentException("Expected a lambda expression in the form: x => x.SomeProperty", nameof(propertyAccessor));
    //         }

    //         return property;
    //     }

    //     [return: MaybeNull]
    //     private static TMemberInfo TryGetMemberExpression<TMemberInfo>(LambdaExpression lambdaExpression)
    //         where TMemberInfo : MemberInfo
    //     {
    //         if (lambdaExpression.Parameters.Count != 1)
    //         {
    //             return null;
    //         }

    //         var body = lambdaExpression.Body;

    //         if (body is UnaryExpression castExpression)
    //         {
    //             if (castExpression.NodeType != ExpressionType.Convert)
    //             {
    //                 return null;
    //             }

    //             body = castExpression.Operand;
    //         }

    //         if (body is MemberExpression memberExpression)
    //         {
    //             if (memberExpression.Expression != lambdaExpression.Parameters[0])
    //             {
    //                 return null;
    //             }

    //             return memberExpression.Member as TMemberInfo;
    //         }
    //         return null;
    //     }
    // }

    /// <summary>
    /// Adapts an <see cref="System.Collections.Generic.ICollection{T}" /> to <see cref="IList" />
    /// because not all generic collections implement <see cref="IList" />.
    /// </summary>
    internal sealed class GenericCollectionToNonGenericAdapter<T> : IList
    {
        private readonly ICollection<T> genericCollection;

        public GenericCollectionToNonGenericAdapter(ICollection<T> genericCollection)
        {
            this.genericCollection = genericCollection ?? throw new ArgumentNullException(nameof(genericCollection));
        }

        public int Add(object? value)
        {
            var index = genericCollection.Count;
            genericCollection.Add((T)value!);
            return index;
        }

        public void Clear()
        {
            genericCollection.Clear();
        }

        public bool Contains(object? value)
        {
            throw new NotSupportedException();
        }

        public int IndexOf(object? value)
        {
            throw new NotSupportedException();
        }

        public void Insert(int index, object? value)
        {
            throw new NotSupportedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotSupportedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotSupportedException(); }
        }

        public void Remove(object? value)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public object? this[int index]
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                ((IList<T>)genericCollection)[index] = (T)value!;
            }
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotSupportedException();
        }

        public int Count
        {
            get { throw new NotSupportedException(); }
        }

        public bool IsSynchronized
        {
            get { throw new NotSupportedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotSupportedException(); }
        }

        public IEnumerator GetEnumerator()
        {
            return genericCollection.GetEnumerator();
        }
    }

    /// <summary>
    /// Adapts an <see cref="System.Collections.Generic.IDictionary{TKey, TValue}" /> to <see cref="IDictionary" />
    /// because not all generic dictionaries implement <see cref="IDictionary" />.
    /// </summary>
    internal sealed class GenericDictionaryToNonGenericAdapter<TKey, TValue> : IDictionary
        where TKey : notnull
    {
        private readonly IDictionary<TKey, TValue> genericDictionary;

        public GenericDictionaryToNonGenericAdapter(IDictionary<TKey, TValue> genericDictionary)
        {
            this.genericDictionary = genericDictionary ?? throw new ArgumentNullException(nameof(genericDictionary));
        }

        public void Add(object key, object? value)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(object key)
        {
            throw new NotSupportedException();
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return new DictionaryEnumerator(genericDictionary.GetEnumerator());
        }

        public bool IsFixedSize
        {
            get { throw new NotSupportedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotSupportedException(); }
        }

        public ICollection Keys
        {
            get { throw new NotSupportedException(); }
        }

        public void Remove(object key)
        {
            throw new NotSupportedException();
        }

        public ICollection Values
        {
            get { throw new NotSupportedException(); }
        }

        public object? this[object key]
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                genericDictionary[(TKey)key] = (TValue)value!;
            }
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotSupportedException();
        }

        public int Count
        {
            get { throw new NotSupportedException(); }
        }

        public bool IsSynchronized
        {
            get { throw new NotSupportedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotSupportedException(); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class DictionaryEnumerator : IDictionaryEnumerator
        {
            private readonly IEnumerator<KeyValuePair<TKey, TValue>> enumerator;

            public DictionaryEnumerator(IEnumerator<KeyValuePair<TKey, TValue>> enumerator)
            {
                this.enumerator = enumerator;
            }

            public DictionaryEntry Entry
            {
                get
                {
                    return new DictionaryEntry(Key, Value);
                }
            }

            public object Key
            {
                get { return enumerator.Current.Key!; }
            }

            public object? Value
            {
                get { return enumerator.Current.Value; }
            }

            public object Current
            {
                get { return Entry; }
            }

            public bool MoveNext()
            {
                return enumerator.MoveNext();
            }

            public void Reset()
            {
                enumerator.Reset();
            }
        }
    }

    public interface IOrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
        where TKey : notnull
    {
        /// <summary>
        /// Gets or sets the element with the specified index.
        /// </summary>
        /// <param name="index">The index of the element to get or set.</param>
        /// <returns>The element with the specified index.</returns>
        KeyValuePair<TKey, TValue> this[int index]
        {
            get;
            set;
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="IOrderedDictionary{TKey, TValue}"/>
        /// at the given index.
        /// </summary>
        /// <param name="index">The zero-based index at which the item should be inserted.</param>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        void Insert(int index, TKey key, TValue value);

        /// <summary>
        /// Removes the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        void RemoveAt(int index);
    }



    [Serializable]
    internal sealed class OrderedDictionary<TKey, TValue> : IOrderedDictionary<TKey, TValue>
        where TKey : notnull
    {
        [NonSerialized]
        private Dictionary<TKey, TValue> dictionary;
        private readonly List<KeyValuePair<TKey, TValue>> list;
        private readonly IEqualityComparer<TKey> comparer;

        public TValue this[TKey key]
        {
            get => dictionary[key];
            set
            {
                if (dictionary.ContainsKey(key))
                {
                    var index = list.FindIndex(kvp => comparer.Equals(kvp.Key, key));
                    dictionary[key] = value;
                    list[index] = new KeyValuePair<TKey, TValue>(key, value);
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        public ICollection<TKey> Keys => new KeyCollection(this);

        public ICollection<TValue> Values => new ValueCollection(this);

        public int Count => dictionary.Count;

        public bool IsReadOnly => false;

        public KeyValuePair<TKey, TValue> this[int index]
        {
            get => list[index];
            set => list[index] = value;
        }

        public OrderedDictionary() : this(EqualityComparer<TKey>.Default)
        {
        }

        public OrderedDictionary(IEqualityComparer<TKey> comparer)
        {
            list = new List<KeyValuePair<TKey, TValue>>();
            dictionary = new Dictionary<TKey, TValue>(comparer);
            this.comparer = comparer;
        }

        public void Add(TKey key, TValue value) => Add(new KeyValuePair<TKey, TValue>(key, value));

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            dictionary.Add(item.Key, item.Value);
            list.Add(item);
        }

        public void Clear()
        {
            dictionary.Clear();
            list.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) => dictionary.Contains(item);

        public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) =>
            list.CopyTo(array, arrayIndex);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => list.GetEnumerator();

        public void Insert(int index, TKey key, TValue value)
        {
            dictionary.Add(key, value);
            list.Insert(index, new KeyValuePair<TKey, TValue>(key, value));
        }

        public bool Remove(TKey key)
        {
            if (dictionary.ContainsKey(key))
            {
                var index = list.FindIndex(kvp => comparer.Equals(kvp.Key, key));
                list.RemoveAt(index);
                if (!dictionary.Remove(key))
                {
                    throw new InvalidOperationException();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);

        public void RemoveAt(int index)
        {
            var key = list[index].Key;
            dictionary.Remove(key);
            list.RemoveAt(index);
        }

    #if !(NETCOREAPP3_1)
    #pragma warning disable 8767 // Nullability of reference types in type of parameter ... doesn't match implicitly implemented member
    #endif

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) =>
            dictionary.TryGetValue(key, out value);

    #if !(NETCOREAPP3_1)
    #pragma warning restore 8767
    #endif

        IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();


        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            // Reconstruct the dictionary from the serialized list
            dictionary = new Dictionary<TKey, TValue>();
            foreach (var kvp in list)
            {
                dictionary[kvp.Key] = kvp.Value;
            }
        }

        private class KeyCollection : ICollection<TKey>
        {
            private readonly OrderedDictionary<TKey, TValue> orderedDictionary;

            public int Count => orderedDictionary.list.Count;

            public bool IsReadOnly => true;

            public void Add(TKey item) => throw new NotSupportedException();

            public void Clear() => throw new NotSupportedException();

            public bool Contains(TKey item) => orderedDictionary.dictionary.Keys.Contains(item);

            public KeyCollection(OrderedDictionary<TKey, TValue> orderedDictionary)
            {
                this.orderedDictionary = orderedDictionary;
            }

            public void CopyTo(TKey[] array, int arrayIndex)
            {
                for (var i = 0; i < orderedDictionary.list.Count; i++)
                {
                    array[i] = orderedDictionary.list[i + arrayIndex].Key;
                }
            }

            public IEnumerator<TKey> GetEnumerator() =>
                orderedDictionary.list.Select(kvp => kvp.Key).GetEnumerator();

            public bool Remove(TKey item) => throw new NotSupportedException();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private class ValueCollection : ICollection<TValue>
        {
            private readonly OrderedDictionary<TKey, TValue> orderedDictionary;

            public int Count => orderedDictionary.list.Count;

            public bool IsReadOnly => true;

            public void Add(TValue item) => throw new NotSupportedException();

            public void Clear() => throw new NotSupportedException();

            public bool Contains(TValue item) => orderedDictionary.dictionary.Values.Contains(item);

            public ValueCollection(OrderedDictionary<TKey, TValue> orderedDictionary)
            {
                this.orderedDictionary = orderedDictionary;
            }

            public void CopyTo(TValue[] array, int arrayIndex)
            {
                for (var i = 0; i < orderedDictionary.list.Count; i++)
                {
                    array[i] = orderedDictionary.list[i + arrayIndex].Value;
                }
            }

            public IEnumerator<TValue> GetEnumerator() =>
                orderedDictionary.list.Select(kvp => kvp.Value).GetEnumerator();

            public bool Remove(TValue item) => throw new NotSupportedException();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }

    /// <summary>
    /// Pooling of StringBuilder instances.
    /// </summary>
    [DebuggerStepThrough]
    internal static class StringBuilderPool
    {
        private static readonly ConcurrentObjectPool<StringBuilder> Pool;

        static StringBuilderPool()
        {
            Pool = new ConcurrentObjectPool<StringBuilder>(() => new StringBuilder());
        }

        public static BuilderWrapper Rent()
        {
            var builder = Pool.Allocate();
            System.Diagnostics.Debug.Assert(builder.Length == 0);
            return new BuilderWrapper(builder, Pool);
        }

        internal readonly struct BuilderWrapper : IDisposable
        {
            public readonly StringBuilder Builder;
            private readonly ConcurrentObjectPool<StringBuilder> _pool;

            public BuilderWrapper(StringBuilder builder, ConcurrentObjectPool<StringBuilder> pool)
            {
                Builder = builder;
                _pool = pool;
            }

            public override string ToString()
            {
                return Builder.ToString();
            }

            public void Dispose()
            {
                var builder = Builder;

                // do not store builders that are too large.
                if (builder.Capacity <= 1024)
                {
                    builder.Length = 0;
                    _pool.Free(builder);
                }
            }
        }
    }
    
    internal static class ThrowHelper
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ThrowArgumentOutOfRangeException(string paramName, string message)
        {
            throw new ArgumentOutOfRangeException(paramName, message);
        }
    }


    }




}



