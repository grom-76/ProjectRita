using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Globalization;
using System.Text;
using System.Linq;
using System.Linq.Expressions;




#region UTILS EXTENSIONS

#pragma warning disable
namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    internal static class BinaryExtensions
    {
        // https://code.google.com/p/freshbooks-api/source/browse/depend/NClassify.Generator/content/ByteArray.cs?r=bbb6c13ec7a01eae082796550f1ddc05f61694b8
        public static int BinaryCompareTo(this byte[] lh, byte[] rh)
        {
            if (lh == null) return rh == null ? 0 : -1;
            if (rh == null) return 1;

            var result = 0;
            var i = 0;
            var stop = System.Math.Min(lh.Length, rh.Length);

            for (; 0 == result && i < stop; i++)
                result = lh[i].CompareTo(rh[i]);

            if (result != 0) return result < 0 ? -1 : 1;
            if (i == lh.Length) return i == rh.Length ? 0 : -1;
            return 1;
        }
    }

    internal static class DateExtensions
    {
        /// <summary>
        /// Truncate DateTime in milliseconds
        /// </summary>
        public static DateTime Truncate(this DateTime dt)
        {
            if (dt == DateTime.MaxValue || dt == DateTime.MinValue)
            {
                return dt;
            }

            return new DateTime(dt.Year, dt.Month, dt.Day,
                dt.Hour, dt.Minute, dt.Second, dt.Millisecond, 
                dt.Kind);
        }

        public static int MonthDifference(this DateTime startDate, DateTime endDate)
        {
            // https://stackoverflow.com/a/1526116/3286260

            int compMonth = (endDate.Month + endDate.Year * 12) - (startDate.Month + startDate.Year * 12);
            double daysInEndMonth = (endDate - endDate.AddMonths(1)).Days;
            double months = compMonth + (startDate.Day - endDate.Day) / daysInEndMonth;

            return Convert.ToInt32(System.Math.Truncate(months));
        }

        public static int YearDifference(this DateTime startDate, DateTime endDate)
        {
            // https://stackoverflow.com/a/28444291/3286260

            //Excel documentation says "COMPLETE calendar years in between dates"
            int years = endDate.Year - startDate.Year;

            if (startDate.Month == endDate.Month &&// if the start month and the end month are the same
                endDate.Day < startDate.Day)// BUT the end day is less than the start day
            {
                years--;
            }
            else if (endDate.Month < startDate.Month)// if the end month is less than the start month
            {
                years--;
            }

            return years;
        }
    }

    internal static class DictionaryExtensions
    {
        /// <summary>
        /// Get free index based on dictionary count number (if is in use, move to next number)
        /// </summary>
        public static ushort NextIndex<T>(this Dictionary<ushort, T> dict)
        {
            if (dict.Count == 0) return 0;

            var next = (ushort)dict.Count;

            while (dict.ContainsKey(next))
            {
                next++;
            }

            return next;
        }

        public static T GetOrDefault<K, T>(this IDictionary<K, T> dict, K key, T defaultValue = default(T))
        {
            if (dict.TryGetValue(key, out T result))
            {
                return result;
            }

            return defaultValue;
        }

        public static void ParseKeyValue(this IDictionary<string, string> dict, string connectionString)
        {
            var s = new StringScanner(connectionString);

            while(!s.HasTerminated)
            {
                var key = s.Scan(@"(.*?)=", 1).Trim();
                var value = "";
                s.Scan(@"\s*");

                if (s.Match("\""))
                {
                    // read a value inside an string " (remove escapes)
                    value = s.Scan(@"""((?:\\""|.)*?)""", 1).Replace("\\\"", "\"");
                    s.Scan(@"\s*;?\s*");
                }
                else
                {
                    // read value
                    value = s.Scan(@"(.*?);\s*", 1).Trim();

                    // read last part
                    if (value.Length == 0)
                    {
                        value = s.Scan(".*").Trim();
                    }
                }

                dict[key] = value;
            }
        }

        /// <summary>
        /// Get value from dictionary converting datatype T
        /// </summary>
        public static T GetValue<T>(this Dictionary<string, string> dict, string key, T defaultValue)
        {
            try
            {
                string value;

                if (dict.TryGetValue(key, out value) == false) return defaultValue;

                if (typeof(T) == typeof(TimeSpan))
                {
                    return (T)(object)TimeSpan.Parse(value);
                }
                else if (typeof(T).GetTypeInfo().IsEnum)
                {
                    return (T)Enum.Parse(typeof(T), value, true);
                }
                else
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
            }
            catch (Exception)
            {
                throw new UltraLiteException("Invalid connection string value type for [" + key + "]");
            }
        }

        /// <summary>
        /// Get a value from a key converted in file size format: "1gb", "10 mb", "80000"
        /// </summary>
        public static long GetFileSize(this Dictionary<string, string> dict, string key, long defaultValue)
        {
            var size = dict.GetValue<string>(key, null);

            if (size == null) return defaultValue;

            var match = Regex.Match(size, @"^(\d+)\s*([tgmk])?(b|byte|bytes)?$", RegexOptions.IgnoreCase);

            if (!match.Success) return 0;

            var num = Convert.ToInt64(match.Groups[1].Value);

            switch (match.Groups[2].Value.ToLower())
            {
                case "t": return num * 1024L * 1024L * 1024L * 1024L;
                case "g": return num * 1024L * 1024L * 1024L;
                case "m": return num * 1024L * 1024L;
                case "k": return num * 1024L;
                case "": return num;
            }

            return 0;
        }
    }

    internal static class ExpressionExtensions
    {
        // more dirty as possible: removing ".Select(x => x." sentence
        private static Regex _removeSelect = new Regex(@"\.Select\s*\(\s*\w+\s*=>\s*\w+\.", RegexOptions.Compiled);
        private static Regex _removeArray = new Regex(@"\.get_Item\(\d+\)", RegexOptions.Compiled);

        /// <summary>
        /// Get Path (better ToString) from an Expression.
        /// Support multi levels: x => x.Customer.Address
        /// Support list levels: x => x.Addresses.Select(z => z.StreetName)
        /// </summary>
        public static string GetPath(this Expression expr)
        {
            // quick and dirty solution to support x.Name.SubName
            // http://stackoverflow.com/questions/671968/retrieving-property-name-from-lambda-expression

            var str = expr.ToString(); // gives you: "o => o.Whatever"
            var firstDelim = str.IndexOf('.'); // make sure there is a beginning property indicator; the "." in "o.Whatever" -- this may not be necessary?

            var path = firstDelim < 0 ? str : str.Substring(firstDelim + 1).TrimEnd(')');

            path = _removeArray.Replace(path, "");
            path = _removeSelect.Replace(path, ".")
                .Replace(")", "");

            return path;
        }
    }

    internal static class IOExceptionExtensions
    {
        private const int ERROR_SHARING_VIOLATION = 32;
        private const int ERROR_LOCK_VIOLATION = 33;

        /// <summary>
        /// Detect if exception is an Locked exception
        /// </summary>
        public static bool IsLocked(this IOException ex)
        {
            var errorCode = Marshal.GetHRForException(ex) & ((1 << 16) - 1);

            return errorCode == ERROR_SHARING_VIOLATION ||
                errorCode == ERROR_LOCK_VIOLATION;
        }

        public static void WaitIfLocked(this IOException ex, int timer)
        {
            if (ex.IsLocked())
            {
                if (timer > 0)
                {
                    WaitFor(timer);
                }
            }
            else
            {
                throw ex;
            }
        }

        /// <summary>
        /// WaitFor function used in all platforms
        /// </summary>
        public static void WaitFor(int ms)
        {
            // http://stackoverflow.com/questions/12641223/thread-sleep-replacement-in-net-for-windows-store
            System.Threading.Tasks.Task.Delay(ms).Wait();
        }
    }

    internal static class LinqExtensions
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(
            this IEnumerable<T> source, int batchSize)
        {
            using (var enumerator = source.GetEnumerator())
                while (enumerator.MoveNext())
                    yield return YieldBatchElements(enumerator, batchSize - 1);
        }

        private static IEnumerable<T> YieldBatchElements<T>(
            IEnumerator<T> source, int batchSize)
        {
            yield return source.Current;
            for (int i = 0; i < batchSize && source.MoveNext(); i++)
                yield return source.Current;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            return _(); IEnumerable<TSource> _()
            {
                var knownKeys = new HashSet<TKey>(comparer);
                foreach (var element in source)
                {
                    if (knownKeys.Add(keySelector(element)))
                        yield return element;
                }
            }
        }
    }

    internal static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return str == null || str.Trim().Length == 0;
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return str == null || str.Length == 0;
        }


    }
}
#pragma warning restore

#endregion

#region UTILS




namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Encryption AES wrapper to encrypt data pages
    /// </summary>
    internal class AesEncryption
    {
        private Aes _aes;

        public AesEncryption(string password, byte[] salt)
        {
            _aes = Aes.Create();
            _aes.Padding = PaddingMode.Zeros;

            #pragma warning disable 
            var pdb = new Rfc2898DeriveBytes(password, salt,1000);
            #pragma warning restore

            using (pdb as IDisposable)
            {
                _aes.Key = pdb.GetBytes(32);
                _aes.IV = pdb.GetBytes(16);
            }
        }

        /// <summary>
        /// Encrypt byte array returning new encrypted byte array with same length of original array (PAGE_SIZE)
        /// </summary>
        public byte[] Encrypt(byte[] bytes)
        {
            using (var encryptor = _aes.CreateEncryptor())
            using (var stream = new MemoryStream())
            using (var crypto = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
            {
                crypto.Write(bytes, 0, bytes.Length);
                crypto.FlushFinalBlock();
                stream.Position = 0;
                var encrypted = new byte[stream.Length];
                stream.Read(encrypted, 0, encrypted.Length);

                return encrypted;
            }
        }

        /// <summary>
        /// Decrypt and byte array returning a new byte array
        /// </summary>
        public byte[] Decrypt(byte[] encryptedValue)
        {
            using (var decryptor = _aes.CreateDecryptor())
            using (var stream = new MemoryStream())
            using (var crypto = new CryptoStream(stream, decryptor, CryptoStreamMode.Write))
            {
                crypto.Write(encryptedValue, 0, encryptedValue.Length);
                crypto.FlushFinalBlock();
                stream.Position = 0;
                var decryptedBytes = new Byte[stream.Length];
                stream.Read(decryptedBytes, 0, decryptedBytes.Length);

                return decryptedBytes;
            }
        }

        /// <summary>
        /// Hash a password using SHA1 just to verify password
        /// </summary>
        public static byte[] HashSHA1(string password)
        {
            var sha = SHA1.Create();
            var shaBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return shaBytes;
        }

        /// <summary>
        /// Generate a salt key that will be stored inside first page database
        /// </summary>
        /// <returns></returns>
        public static byte[] Salt(int maxLength = 16)
        {
            var salt = new byte[maxLength];
            {
                var rng = RandomNumberGenerator.Create();
                using (rng as IDisposable)
                    rng.GetBytes(salt);
            }
            return salt;
        }

        public void Dispose()
        {
            if (_aes != null)
            {
                _aes = null!;
            }
        }
    }
}

#pragma warning disable
namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public class ByteReader
    {
        private byte[] _buffer;
        private int _length;
        private int _pos;

        public int Position { get { return _pos; } set { _pos = value; } }

         public ByteReader()
        {
            _buffer = null;
            _length = 0;
            _pos = 0;
        }

        public ByteReader(byte[] buffer)
        {
            _buffer = buffer;
            _length = buffer.Length;
            _pos = 0;
        }

        public ByteReader(ArraySegment<byte> buffer)
        {
            _buffer = buffer.Array;
            _length = buffer.Offset+buffer.Count;
            _pos = buffer.Offset;
        }

        public void Clear()
        {
            _buffer = null;
            _length = 0;
            _pos = 0;
        }

        public void Reset(byte[] buffer)
        {
            _buffer = buffer;
            _length = buffer.Length;
            _pos = 0;
        }

        public void Reset(ArraySegment<byte> buffer)
        {
            _buffer = buffer.Array;
            _length = buffer.Offset+buffer.Count;
            _pos = buffer.Offset;
        }


        public void Skip(int length)
        {
            _pos += length;
        }

        #region Native data types

        public Byte ReadByte()
        {
            var value = _buffer[_pos];

            _pos++;

            return value;
        }

        public Boolean ReadBoolean()
        {
            var value = _buffer[_pos];

            _pos++;

            return value == 0 ? false : true;
        }

        public UInt16 ReadUInt16()
        {
            _pos += 2;
            return BitConverter.ToUInt16(_buffer, _pos - 2);
        }

        public UInt32 ReadUInt32()
        {
            _pos += 4;
            return BitConverter.ToUInt32(_buffer, _pos - 4);
        }

        public UInt64 ReadUInt64()
        {
            _pos += 8;
            return BitConverter.ToUInt64(_buffer, _pos - 8);
        }

        public Int16 ReadInt16()
        {
            _pos += 2;
            return BitConverter.ToInt16(_buffer, _pos - 2);
        }

        public Int32 ReadInt32()
        {
            _pos += 4;
            return BitConverter.ToInt32(_buffer, _pos - 4);
        }

        public Int64 ReadInt64()
        {
            _pos += 8;
            return BitConverter.ToInt64(_buffer, _pos - 8);
        }

        public Single ReadSingle()
        {
            _pos += 4;
            return BitConverter.ToSingle(_buffer, _pos - 4);
        }

        public Double ReadDouble()
        {
            _pos += 8;
            return BitConverter.ToDouble(_buffer, _pos - 8);
        }

        public Decimal ReadDecimal()
        {
            _pos += 16;
            var a = BitConverter.ToInt32(_buffer, _pos - 16);
            var b = BitConverter.ToInt32(_buffer, _pos - 12);
            var c = BitConverter.ToInt32(_buffer, _pos - 8);
            var d = BitConverter.ToInt32(_buffer, _pos - 4);
            return new Decimal(new int[] {  a, b, c, d });
        }

        public Byte[] ReadBytes(int count)
        {
            var buffer = new byte[count];

            System.Buffer.BlockCopy(_buffer, _pos, buffer, 0, count);

            _pos += count;

            return buffer;
        }

        #endregion

        #region Extended types

        public string ReadString()
        {
            var length = this.ReadInt32();
            var str = Encoding.UTF8.GetString(_buffer, _pos, length);
            _pos += length;

            return str;
        }

        public string ReadString(int length)
        {
            var str = Encoding.UTF8.GetString(_buffer, _pos, length);
            _pos += length;

            return str;
        }

        /// <summary>
        /// Read BSON string add \0x00 at and of string and add this char in length before
        /// </summary>
        public string ReadBsonString()
        {
            var length = this.ReadInt32();
            var str = Encoding.UTF8.GetString(_buffer, _pos, length - 1);
            _pos += length;

            return str;
        }

        public string ReadCString()
        {
            var pos = _pos;
            var length = 0;

            while (true)
            {
                if (_buffer[pos] == 0x00)
                {
                    var str = Encoding.UTF8.GetString(_buffer, _pos, length);
                    _pos += length + 1; // read last 0x00
                    return str;
                }
                else if (pos > _length)
                {
                    return "_";
                }

                pos++;
                length++;
            }
        }

        public DateTime ReadDateTime()
        {
            // fix #921 converting index key into LocalTime
            // this is not best solution because uctDate must be a global parameter
            // this will be review in v5
            var date = new DateTime(this.ReadInt64(), DateTimeKind.Utc);

            return date.ToLocalTime();
        }

        public Guid ReadGuid()
        {
            return new Guid(this.ReadBytes(16));
        }

        public ObjectId ReadObjectId()
        {
            return new ObjectId(this.ReadBytes(12));
        }

        internal PageAddress ReadPageAddress()
        {
            return new PageAddress(this.ReadUInt32(), this.ReadUInt16());
        }

        public BsonValue ReadBsonValue(ushort length)
        {
            var type = (BsonType)this.ReadByte();

            switch (type)
            {
                case BsonType.Null: return BsonValue.Null;

                case BsonType.Int32: return this.ReadInt32();
                case BsonType.Int64: return this.ReadInt64();
                case BsonType.Double: return this.ReadDouble();
                case BsonType.Decimal: return this.ReadDecimal();

                case BsonType.String: return this.ReadString(length);

                case BsonType.Document: return BsonReader.ReadDocument(this);
                case BsonType.Array: return BsonReader.ReadArray(this);

                case BsonType.Binary: return this.ReadBytes(length);
                case BsonType.ObjectId: return this.ReadObjectId();
                case BsonType.Guid: return this.ReadGuid();

                case BsonType.Boolean: return this.ReadBoolean();
                case BsonType.DateTime: return this.ReadDateTime();

                case BsonType.MinValue: return BsonValue.MinValue;
                case BsonType.MaxValue: return BsonValue.MaxValue;
            }

            throw new NotImplementedException();
        }

        #endregion
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public class ByteWriter
    {
        private byte[] _buffer;
        private int _pos;

        public byte[] Buffer { get { return _buffer; } }

        public int Position { get { return _pos; } set { _pos = value; } }

        public ByteWriter()
        {
            _buffer = null;
            _pos = 0;
        }

        public ByteWriter(int length)
        {
            _buffer = new byte[length];
            _pos = 0;
        }

        public ByteWriter(byte[] buffer)
        {
            _buffer = buffer;
            _pos = 0;
        }

        public ByteWriter(ArraySegment<byte> buffer)
        {
            _buffer = buffer.Array;
            _pos = buffer.Offset;
        }

        public void Clear()
        {
            _buffer = null;
            _pos = 0;
        }

        public void Reset(byte[] buffer)
        {
            _buffer = buffer;
            _pos = 0;
        }

        public void Reset(ArraySegment<byte> buffer)
        {
            _buffer = buffer.Array;
            _pos = buffer.Offset;
        }

        public void Skip(int length)
        {
            _pos += length;
        }

        #region Native data types

        public void Write(Byte value)
        {
            _buffer[_pos] = value;

            _pos++;
        }

        public void Write(Boolean value)
        {
            _buffer[_pos] = value ? (byte)1 : (byte)0;

            _pos++;
        }

        public void Write(UInt16 value)
        {
            var pi = BitConverter.GetBytes(value);

            _buffer[_pos + 0] = pi[0];
            _buffer[_pos + 1] = pi[1];

            _pos += 2;
        }

        public void Write(UInt32 value)
        {
            var pi = BitConverter.GetBytes(value);

            _buffer[_pos + 0] = pi[0];
            _buffer[_pos + 1] = pi[1];
            _buffer[_pos + 2] = pi[2];
            _buffer[_pos + 3] = pi[3];

            _pos += 4;
        }

        public void Write(UInt64 value)
        {
            var pi = BitConverter.GetBytes(value);

            _buffer[_pos + 0] = pi[0];
            _buffer[_pos + 1] = pi[1];
            _buffer[_pos + 2] = pi[2];
            _buffer[_pos + 3] = pi[3];
            _buffer[_pos + 4] = pi[4];
            _buffer[_pos + 5] = pi[5];
            _buffer[_pos + 6] = pi[6];
            _buffer[_pos + 7] = pi[7];

            _pos += 8;
        }

        public void Write(Int16 value)
        {
            var pi = BitConverter.GetBytes(value);

            _buffer[_pos + 0] = pi[0];
            _buffer[_pos + 1] = pi[1];

            _pos += 2;
        }

        public void Write(Int32 value)
        {
            var pi = BitConverter.GetBytes(value);

            _buffer[_pos + 0] = pi[0];
            _buffer[_pos + 1] = pi[1];
            _buffer[_pos + 2] = pi[2];
            _buffer[_pos + 3] = pi[3];

            _pos += 4;
        }

        public void Write(Int64 value)
        {
            var pi = BitConverter.GetBytes(value);

            _buffer[_pos + 0] = pi[0];
            _buffer[_pos + 1] = pi[1];
            _buffer[_pos + 2] = pi[2];
            _buffer[_pos + 3] = pi[3];
            _buffer[_pos + 4] = pi[4];
            _buffer[_pos + 5] = pi[5];
            _buffer[_pos + 6] = pi[6];
            _buffer[_pos + 7] = pi[7];

            _pos += 8;
        }

        public void Write(Single value)
        {
            var pi = BitConverter.GetBytes(value);

            _buffer[_pos + 0] = pi[0];
            _buffer[_pos + 1] = pi[1];
            _buffer[_pos + 2] = pi[2];
            _buffer[_pos + 3] = pi[3];

            _pos += 4;
        }

        public void Write(Double value)
        {
            var pi = BitConverter.GetBytes(value);

            _buffer[_pos + 0] = pi[0];
            _buffer[_pos + 1] = pi[1];
            _buffer[_pos + 2] = pi[2];
            _buffer[_pos + 3] = pi[3];
            _buffer[_pos + 4] = pi[4];
            _buffer[_pos + 5] = pi[5];
            _buffer[_pos + 6] = pi[6];
            _buffer[_pos + 7] = pi[7];

            _pos += 8;
        }

        public void Write(Decimal value)
        {
            var array = Decimal.GetBits(value);

            this.Write(array[0]);
            this.Write(array[1]);
            this.Write(array[2]);
            this.Write(array[3]);
        }

        public void Write(Byte[] value)
        {
            System.Buffer.BlockCopy(value, 0, _buffer, _pos, value.Length);

            _pos += value.Length;
        }

        #endregion

        #region Extended types

        public void Write(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            this.Write(bytes.Length);
            this.Write(bytes);
        }

        public void Write(string value, int length)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            if (bytes.Length != length) throw new ArgumentException("Invalid string length");
            this.Write(bytes);
        }

        public void Write(DateTime value)
        {
            this.Write(value.ToUniversalTime().Ticks);
        }

        public void Write(Guid value)
        {
            this.Write(value.ToByteArray());
        }

        public void Write(ObjectId value)
        {
            this.Write(value.ToByteArray());
        }

        internal void Write(PageAddress value)
        {
            this.Write(value.PageID);
            this.Write(value.Index);
        }

        public void WriteBsonValue(BsonValue value, ushort length)
        {
            this.Write((byte)value.Type);

            switch (value.Type)
            {
                case BsonType.Null:
                case BsonType.MinValue:
                case BsonType.MaxValue:
                    break;

                case BsonType.Int32: this.Write((Int32)value.RawValue); break;
                case BsonType.Int64: this.Write((Int64)value.RawValue); break;
                case BsonType.Double: this.Write((Double)value.RawValue); break;
                case BsonType.Decimal: this.Write((Decimal)value.RawValue); break;

                case BsonType.String: this.Write((String)value.RawValue, length); break;

                case BsonType.Document: BsonWriter.WriteDocument(this, value.AsDocument); break;
                case BsonType.Array: BsonWriter.WriteArray(this, value.AsArray); break;

                case BsonType.Binary: this.Write((Byte[])value.RawValue); break;
                case BsonType.ObjectId: this.Write((ObjectId)value.RawValue); break;
                case BsonType.Guid: this.Write((Guid)value.RawValue); break;

                case BsonType.Boolean: this.Write((Boolean)value.RawValue); break;
                case BsonType.DateTime: this.Write((DateTime)value.RawValue); break;

                default: throw new NotImplementedException();
            }
        }

        #endregion
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Implement how database will compare to order by/find strings according defined culture/compare options
    /// If not set, default is CurrentCulture with IgnoreCase
    /// </summary>
    public class Collation : IComparer<BsonValue>, IComparer<string>, IEqualityComparer<BsonValue>
    {
        private readonly CompareInfo _compareInfo;


        public Collation(CompareOptions sortOptions)
        {
            this.SortOptions = sortOptions;
            this.Culture = new CultureInfo("");

            _compareInfo = this.Culture.CompareInfo;
        }

        public static Collation Default = new Collation(CompareOptions.IgnoreCase);

        public static Collation Binary = new Collation(CompareOptions.Ordinal);


        /// <summary>
        /// Get database language culture
        /// </summary>
        public CultureInfo Culture { get; }

        /// <summary>
        /// Get options to how string should be compared in sort
        /// </summary>
        public CompareOptions SortOptions { get; }

        /// <summary>
        /// Compare 2 string values using current culture/compare options
        /// </summary>
        public int Compare(string left, string right)
        {
            var result = _compareInfo.Compare(left, right, this.SortOptions);

            return result < 0 ? -1 : result > 0 ? +1 : 0;
        }

        /// <summary>
        /// Compare 2 chars values using current culture/compare options
        /// </summary>
        public int Compare(char left, char right)
        {
            //TODO implementar o compare corretamente
            return char.ToUpper(left) == char.ToUpper(right) ? 0 : 1;
        }

        public int Compare(BsonValue left, BsonValue rigth)
        {
            return left.CompareTo(rigth, this);
        }

        public bool Equals(BsonValue x, BsonValue y)
        {
            return this.Compare(x, y) == 0;
        }

        public int GetHashCode(BsonValue obj)
        {
            return obj.GetHashCode();
        }

        public override string ToString()
        {
            return this.Culture.Name + "/" + this.SortOptions.ToString();
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Manage ConnectionString to connect and create databases. Connection string are NameValue using Name1=Value1; Name2=Value2
    /// </summary>
    public class ConnectionString
    {
        /// <summary>
        /// "filename": Full path or relative path from DLL directory
        /// </summary>
        public string Filename { get; set; } = "";

        /// <summary>
        /// "journal": Enabled or disable double write check to ensure durability (default: true)
        /// </summary>
        public bool Journal { get; set; } = true;

        /// <summary>
        /// "password": Encrypt (using AES) your datafile with a password (default: null - no encryption)
        /// </summary>
        public string Password { get; set; } = null;

        /// <summary>
        /// "cache size": Max number of pages in cache. After this size, flush data to disk to avoid too memory usage (default: 5000)
        /// </summary>
        public int CacheSize { get; set; } = 5000;

        /// <summary>
        /// "timeout": Timeout for waiting unlock operations (default: 1 minute)
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(1);


        /// <summary>
        /// "initial size": If database is new, initialize with allocated space - support KB, MB, GB (default: 0 bytes)
        /// </summary>
        public long InitialSize { get; set; } = 0;

        /// <summary>
        /// "limit size": Max limit of datafile - support KB, MB, GB (default: long.MaxValue - no limit)
        /// </summary>
        public long LimitSize { get; set; } = long.MaxValue;

        /// <summary>
        /// "log": Debug messages from database - use `UltraLiteDatabase.Log` (default: Logger.NONE)
        /// </summary>
        public byte Log { get; set; } = Logger.NONE;

        /// <summary>
        /// "utc": Returns date in UTC timezone from BSON deserialization (default: true - UTC)
        /// </summary>
        public bool UtcDate { get; set; } = true;

        /// <summary>
        /// "async": Use "sync over async" to UWP apps access any directory (default: false)
        /// </summary>
        public bool Async { get; set; } = false;

        /// <summary>
        /// "flush": If true, apply flush direct to disk, ignoring OS cache [FileStream.Flush(true)]
        /// </summary>
        public bool Flush { get; set; } = false;

        /// <summary>
        /// Initialize empty connection string
        /// </summary>
        public ConnectionString()
        {
        }

        /// <summary>
        /// Initialize connection string parsing string in "key1=value1;key2=value2;...." format or only "filename" as default (when no ; char found)
        /// </summary>
        public ConnectionString(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // create a dictionary from string name=value collection
            if (connectionString.Contains("="))
            {
                values.ParseKeyValue(connectionString);
            }
            else
            {
                values["filename"] = connectionString;
            }

            // setting values to properties
            this.Filename = values.GetValue("filename", this.Filename);
            this.Journal = values.GetValue("journal", this.Journal);
            this.Password = values.GetValue<string>("password", this.Password);
            this.CacheSize = values.GetValue(@"cache size", this.CacheSize);
            this.Timeout = values.GetValue("timeout", this.Timeout);
            this.InitialSize = values.GetFileSize(@"initial size", this.InitialSize);
            this.LimitSize = values.GetFileSize(@"limit size", this.LimitSize);
            this.Log = values.GetValue("log", this.Log);
            this.UtcDate = values.GetValue("utc", this.UtcDate);
            this.Async = values.GetValue("async", this.Async);
            this.Flush = values.GetValue("flush", this.Flush);

        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
	/// <summary>
	/// A simple file helper tool with static methods
	/// </summary>
	internal static class FileHelper
    {
        /// <summary>
        /// Create a temp filename based on original filename - checks if file exists (if exists, append counter number)
        /// </summary>
        public static string GetTempFile(string filename, string suffix = "-temp", bool checkIfExists = true)
        {
            var count = 0;
            var temp = Path.Combine(Path.GetDirectoryName(filename), 
                Path.GetFileNameWithoutExtension(filename) + suffix + 
                Path.GetExtension(filename));

            while(checkIfExists && File.Exists(temp))
            {
                temp = Path.Combine(Path.GetDirectoryName(filename),
                    Path.GetFileNameWithoutExtension(filename) + suffix +
                    "-" + (++count) +
                    Path.GetExtension(filename));
            }

            return temp;
        }

        /// <summary>
        /// Try delete a file that can be in use by another
        /// </summary>
        public static bool TryDelete(string filename)
        {
            try
            {
                File.Delete(filename);
                return true;
            }
            catch(IOException)
            {
                return false;
            }
        }

        /// <summary>
        /// Try execute some action while has lock exception
        /// </summary>
        public static void TryExec(Action action, TimeSpan timeout)
        {
            var timer = DateTime.UtcNow.Add(timeout);

            do
            {
                try
                {
                    action();
                    return;
                }
                catch (IOException ex)
                {
                    ex.WaitIfLocked(25);
                }
            }
            while (DateTime.UtcNow < timer);

            throw UltraLiteException.LockTimeout(timeout);
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
	/// <summary>
	/// Datafile open options (for FileDiskService)
	/// </summary>
	public class FileOptions
    {
        public bool Journal { get; set; }
        public long InitialSize { get; set; }
        public long LimitSize { get; set; }
        public bool Async { get; set; }
        public bool Flush { get; set; } = false;

        public FileOptions()
        {
            this.Journal = true;
            this.InitialSize = 0;
            this.LimitSize = long.MaxValue;
            this.Flush = false;
        }
    }


}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
	internal class KeyDocument
    {
        public BsonValue Key { get; set; }
        public BsonDocument Document { get; set; }
    }

    internal class KeyDocumentComparer : IComparer<KeyDocument>
    {
        public int Compare(KeyDocument x, KeyDocument y)
        {
            return x.Key.CompareTo(y.Key);
        }

        public int GetHashCode(KeyDocument obj)
        {
            return obj.Key.GetHashCode();
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// LazyLoad class for .NET 3.5
    /// http://stackoverflow.com/questions/3207580/implementation-of-lazyt-for-net-3-5
    /// </summary>
    public class LazyLoad<T>
        where T : class
    {
        private readonly object _locker = new object();
        private readonly Func<T> _createValue;
        private bool _isValueCreated;
        private T _value;

        /// <summary>
        /// Gets the lazily initialized value of the current Lazy{T} instance.
        /// </summary>
        public T Value
        {
            get
            {
                if (!_isValueCreated)
                {
                    lock (_locker)
                    {
                        if (!_isValueCreated)
                        {
                            _value = _createValue();
                            _isValueCreated = true;
                        }
                    }
                }
                return _value;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether a value has been created for this Lazy{T} instance.
        /// </summary>
        public bool IsValueCreated
        {
            get
            {
                lock (_locker)
                {
                    return _isValueCreated;
                }
            }
        }


        /// <summary>
        /// Initializes a new instance of the Lazy{T} class.
        /// </summary>
        /// <param name="createValue">The delegate that produces the value when it is needed.</param>
        public LazyLoad(Func<T> createValue)
        {
            if (createValue == null) throw new ArgumentNullException(nameof(createValue));

            _createValue = createValue;
        }

        /// <summary>
        /// Creates and returns a string representation of the Lazy{T}.Value.
        /// </summary>
        /// <returns>The string representation of the Lazy{T}.Value property.</returns>
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// A logger class to log all information about database. Used with levels. Level = 0 - 255
    /// All log will be trigger before operation execute (better for debug)
    /// </summary>
    public class Logger
    {
        public const byte NONE = 0;
        public const byte ERROR = 1;
        public const byte RECOVERY = 2;
        public const byte COMMAND = 4;
        public const byte LOCK = 8;
        public const byte QUERY = 16;
        public const byte JOURNAL = 32;
        public const byte CACHE = 64;
        public const byte DISK = 128;
        public const byte FULL = 255;

        /// <summary>
        /// Initialize logger class using a custom logging level (see Logger.NONE to Logger.FULL)
        /// </summary>
        public Logger(byte level = NONE, Action<string> logging = null)
        {
            this.Level = level;

            if (logging != null)
            {
                this.Logging += logging;
            }
        }

        /// <summary>
        /// Event when log writes a message. Fire on each log message
        /// </summary>
        public event Action<string> Logging = null;

        /// <summary>
        /// To full logger use Logger.FULL or any combination of Logger constants like Level = Logger.ERROR | Logger.COMMAND | Logger.DISK
        /// </summary>
        public byte Level { get; set; }

        public Logger()
        {
            this.Level = NONE;
        }

        /// <summary>
        /// Execute msg function only if level are enabled
        /// </summary>
        public void Write(byte level, Func<string> fn)
        {
            if ((level & this.Level) == 0) return;

            this.Write(level, fn());
        }

        /// <summary>
        /// Write log text to output using inside a component (statics const of Logger)
        /// </summary>
        public void Write(byte level, string message, params object[] args)
        {
            if ((level & this.Level) == 0 || string.IsNullOrEmpty(message)) return;

            if (this.Logging != null)
            {
                var text = string.Format(message, args);

                var str =
                    level == ERROR ? "ERROR" :
                    level == RECOVERY ? "RECOVERY" :
                    level == COMMAND ? "COMMAND" :
                    level == JOURNAL ? "JOURNAL" :
                    level == LOCK ? "LOCK" :
                    level == QUERY ? "QUERY" :
                    level == CACHE ? "CACHE" : 
                    level == DISK ? "DISK" : "";

                var msg = DateTime.Now.ToString("HH:mm:ss.ffff") + " [" + str + "] " + text;

                try
                {
                    this.Logging(msg);
                }
                catch
                {
                }
            }
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
	/// <summary>
	/// Parse/Format storage unit format (kb/mb/gb)
	/// </summary>
	internal class StorageUnitHelper
    {


        /// <summary>
        /// Format a long file length to pretty file unit
        /// </summary>
        public static string FormatFileSize(long byteCount)
        {
            var suf = new string[] { "B", "KB", "MB", "GB", "TB" }; //Longs run out around EB
            if (byteCount == 0) return "0" + suf[0];
            var bytes = System.Math.Abs(byteCount);
            var place = Convert.ToInt32(System.Math.Floor(System.Math.Log(bytes, 1024)));
            var num = System.Math.Round(bytes / System.Math.Pow(1024, place), 1);
            return (System.Math.Sign(byteCount) * num).ToString() + suf[place];
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
	/// <summary>
	/// A StringScanner is state machine used in text parsers based on regular expressions
	/// </summary>
	public class StringScanner
    {
        public string Source { get; private set; }
        public int Index { get; set; }

        /// <summary>
        /// Initialize scanner with a string to be parsed
        /// </summary>
        public StringScanner(string source)
        {
            this.Source = source;
            this.Index = 0;
        }

        public override string ToString()
        {
            return this.HasTerminated ? "<EOF>" : this.Source.Substring(this.Index);
        }

        /// <summary>
        /// Reset cursor position
        /// </summary>
        public void Reset()
        {
            this.Index = 0;
        }

        /// <summary>
        /// Skip cursor position in string source
        /// </summary>
        public void Seek(int length)
        {
            this.Index += length;
        }

        /// <summary>
        /// Indicate that cursor is EOF
        /// </summary>
        public bool HasTerminated
        {
            get { return this.Index >= this.Source.Length; }
        }

        /// <summary>
        /// Scan in current cursor position for this patterns. If found, returns string and run with cursor
        /// </summary>
        public string Scan(string pattern)
        {
            return this.Scan(new Regex((pattern.StartsWith("^") ? "" : "^") + pattern, RegexOptions.IgnorePatternWhitespace));
        }

        /// <summary>
        /// Scan in current cursor position for this patterns. If found, returns string and run with cursor
        /// </summary>
        public string Scan(Regex regex)
        {
            var match = regex.Match(this.Source, this.Index, this.Source.Length - this.Index);

            if (match.Success)
            {
                this.Index += match.Length;
                return match.Value;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Scan pattern and returns group string index 1 based
        /// </summary>
        public string Scan(string pattern, int group)
        {
            return this.Scan(new Regex((pattern.StartsWith("^") ? "" : "^") + pattern, RegexOptions.IgnorePatternWhitespace), group);
        }

        public string Scan(Regex regex, int group)
        {
            var match = regex.Match(this.Source, this.Index, this.Source.Length - this.Index);

            if (match.Success)
            {
                this.Index += match.Length;
                return group >= match.Groups.Count ? "" : match.Groups[group].Value;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Match if pattern is true in current cursor position. Do not change cursor position
        /// </summary>
        public bool Match(string pattern)
        {
            return this.Match(new Regex((pattern.StartsWith("^") ? "" : "^") + pattern, RegexOptions.IgnorePatternWhitespace));
        }

        /// <summary>
        /// Match if pattern is true in current cursor position. Do not change cursor position
        /// </summary>
        public bool Match(Regex regex)
        {
            var match = regex.Match(this.Source, this.Index, this.Source.Length - this.Index);
            return match.Success;
        }

        /// <summary>
        /// Throw syntax exception if not terminate string
        /// </summary>
        public void ThrowIfNotFinish()
        {
            this.Scan(@"\s*");

            if (!this.HasTerminated) throw UltraLiteException.SyntaxError(this);
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    #region TokenType definition

    /// <summary>
    /// ASCII char names: https://www.ascii.cl/htmlcodes.htm
    /// </summary>
    internal enum TokenType
    {
        /// <summary> { </summary>
        OpenBrace,
        /// <summary> } </summary>
        CloseBrace,
        /// <summary> [ </summary>
        OpenBracket,
        /// <summary> ] </summary>
        CloseBracket,
        /// <summary> ( </summary>
        OpenParenthesis,
        /// <summary> ) </summary>
        CloseParenthesis,
        /// <summary> , </summary>
        Comma,
        /// <summary> : </summary>
        Colon,
        /// <summary> ; </summary>
        SemiColon,
        /// <summary> @ </summary>
        At,
        /// <summary> # </summary>
        Hashtag,
        /// <summary> ~ </summary>
        Til,
        /// <summary> . </summary>
        Period,
        /// <summary> &amp; </summary>
        Ampersand,
        /// <summary> $ </summary>
        Dollar,
        /// <summary> ! </summary>
        Exclamation,
        /// <summary> != </summary>
        NotEquals,
        /// <summary> = </summary>
        Equals,
        /// <summary> &gt; </summary>
        Greater,
        /// <summary> &gt;= </summary>
        GreaterOrEquals,
        /// <summary> &lt; </summary>
        Less,
        /// <summary> &lt;= </summary>
        LessOrEquals,
        /// <summary> - </summary>
        Minus,
        /// <summary> + </summary>
        Plus,
        /// <summary> * </summary>
        Asterisk,
        /// <summary> / </summary>
        Slash,
        /// <summary> \ </summary>
        Backslash,
        /// <summary> % </summary>
        Percent,
        /// <summary> "..." or '...' </summary>
        String,
        /// <summary> [0-9]+ </summary>
        Int,
        /// <summary> [0-9]+.[0-9] </summary>
        Double,
        /// <summary> \n\r\t \u0032 </summary>
        Whitespace,
        /// <summary> [a-Z_$]+[a-Z0-9_$] </summary>
        Word,
        EOF,
        Unknown
    }

    #endregion

    #region Token definition

    /// <summary>
    /// Represent a single string token
    /// </summary>
    internal class Token
    {
        private static readonly HashSet<string> _keywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "BETWEEN",
            "LIKE",
            "IN",
            "AND",
            "OR"
        };

        public Token(TokenType tokenType, string value, long position)
        {
            this.Position = position;
            this.Value = value;
            this.Type = tokenType;
        }

        public TokenType Type { get; private set; }
        public string Value { get; private set; }
        public long Position { get; private set; }

        /// <summary>
        /// Expect if token is type (if not, throw UnexpectedToken)
        /// </summary>
        public Token Expect(TokenType type)
        {
            if (this.Type != type)
            {
                throw UltraLiteException.UnexpectedToken(this);
            }

            return this;
        }

        /// <summary>
        /// Expect for type1 OR type2 (if not, throw UnexpectedToken)
        /// </summary>
        public Token Expect(TokenType type1, TokenType type2)
        {
            if (this.Type != type1 && this.Type != type2)
            {
                throw UltraLiteException.UnexpectedToken(this);
            }

            return this;
        }

        /// <summary>
        /// Expect for type1 OR type2 OR type3 (if not, throw UnexpectedToken)
        /// </summary>
        public Token Expect(TokenType type1, TokenType type2, TokenType type3)
        {
            if (this.Type != type1 && this.Type != type2 && this.Type != type3)
            {
                throw UltraLiteException.UnexpectedToken(this);
            }

            return this;
        }

        public Token Expect(string value, bool ignoreCase = true)
        {
            if (!this.Is(value, ignoreCase))
            {
                throw UltraLiteException.UnexpectedToken(this, value);
            }

            return this;
        }

        public bool Is(string value, bool ignoreCase = true)
        {
            return 
                this.Type == TokenType.Word &&
                value.Equals(this.Value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }

        public bool IsOperand
        {
            get
            {
                switch (this.Type)
                {
                    case TokenType.Percent:
                    case TokenType.Slash:
                    case TokenType.Asterisk:
                    case TokenType.Plus:
                    case TokenType.Minus:
                    case TokenType.Equals:
                    case TokenType.Greater:
                    case TokenType.GreaterOrEquals:
                    case TokenType.Less:
                    case TokenType.LessOrEquals:
                    case TokenType.NotEquals:
                        return true;
                    case TokenType.Word:
                        return _keywords.Contains(Value);
                    default:
                        return false;
                }
            }
        }

        public override string ToString()
        {
            return this.Value + " (" + this.Type + ")";
        }
    }

    #endregion

    /// <summary>
    /// Class to tokenize TextReader input used in JsonRead/BsonExpressions
    /// This class are not thread safe
    /// </summary>
    internal class Tokenizer
    {
        private TextReader _reader;
        private char _char = '\0';
        private Token _ahead = null;
        private bool _eof = false;

        public bool EOF => _eof && _ahead == null;
        public long Position { get; private set; }
        public Token Current { get; private set; }

        /// <summary>
        /// If EOF throw an invalid token exception (used in while()) otherwise return "false" (not EOF)
        /// </summary>
        public bool CheckEOF()
        {
            if (_eof) throw UltraLiteException.UnexpectedToken(this.Current);

            return false;
        }

        public Tokenizer(string source)
            : this(new StringReader(source))
        {
        }

        public Tokenizer(TextReader reader)
        {
            _reader = reader;

            this.Position = 0;
            this.ReadChar();
        }

        /// <summary>
        /// Checks if char is an valid part of a word [a-Z_]+[a-Z0-9_$]*
        /// </summary>
        public static bool IsWordChar(char c, bool first)
        {
            if (first)
            {
                return char.IsLetter(c) || c == '_' || c == '$';
            }

            return char.IsLetterOrDigit(c) || c == '_' || c == '$';
        }

        /// <summary>
        /// Read next char in stream and set in _current
        /// </summary>
        private char ReadChar()
        {
            if (_eof) return '\0';

            var c = _reader.Read();

            this.Position++;

            if (c == -1)
            {
                _char = '\0';
                _eof = true;
            }
            else
            {
                _char = (char)c;
            }

            return _char;
        }

        /// <summary>
        /// Look for next token but keeps in buffer when run "ReadToken()" again.
        /// </summary>
        public Token LookAhead(bool eatWhitespace = true)
        {
            if (_ahead != null)
            {
                if (eatWhitespace && _ahead.Type == TokenType.Whitespace)
                {
                    _ahead = this.ReadNext(eatWhitespace);
                }

                return _ahead;
            }

            return _ahead = this.ReadNext(eatWhitespace);
        }

        /// <summary>
        /// Read next token (or from ahead buffer).
        /// </summary>
        public Token ReadToken(bool eatWhitespace = true)
        {
            if (_ahead == null)
            {
                return this.Current = this.ReadNext(eatWhitespace);
            }

            if (eatWhitespace && _ahead.Type == TokenType.Whitespace)
            {
                _ahead = this.ReadNext(eatWhitespace);
            }

            this.Current = _ahead;
            _ahead = null;
            return this.Current;
        }

        /// <summary>
        /// Read next token from reader
        /// </summary>
        private Token ReadNext(bool eatWhitespace)
        {
            // remove whitespace before get next token
            if (eatWhitespace) this.EatWhitespace();

            if (_eof)
            {
                return new Token(TokenType.EOF, null, this.Position);
            }

            Token token = null;

            switch (_char)
            {
                case '{':
                    token = new Token(TokenType.OpenBrace, "{", this.Position);
                    this.ReadChar();
                    break;

                case '}':
                    token = new Token(TokenType.CloseBrace, "}", this.Position);
                    this.ReadChar();
                    break;

                case '[':
                    token = new Token(TokenType.OpenBracket, "[", this.Position);
                    this.ReadChar();
                    break;

                case ']':
                    token = new Token(TokenType.CloseBracket, "]", this.Position);
                    this.ReadChar();
                    break;

                case '(':
                    token = new Token(TokenType.OpenParenthesis, "(", this.Position);
                    this.ReadChar();
                    break;

                case ')':
                    token = new Token(TokenType.CloseParenthesis, ")", this.Position);
                    this.ReadChar();
                    break;

                case ',':
                    token = new Token(TokenType.Comma, ",", this.Position);
                    this.ReadChar();
                    break;

                case ':':
                    token = new Token(TokenType.Colon, ":", this.Position);
                    this.ReadChar();
                    break;

                case ';':
                    token = new Token(TokenType.SemiColon, ";", this.Position);
                    this.ReadChar();
                    break;

                case '@':
                    token = new Token(TokenType.At, "@", this.Position);
                    this.ReadChar();
                    break;

                case '#':
                    token = new Token(TokenType.Hashtag, "#", this.Position);
                    this.ReadChar();
                    break;

                case '~':
                    token = new Token(TokenType.Til, "~", this.Position);
                    this.ReadChar();
                    break;

                case '.':
                    token = new Token(TokenType.Period, ".", this.Position);
                    this.ReadChar();
                    break;

                case '&':
                    token = new Token(TokenType.Ampersand, "&", this.Position);
                    this.ReadChar();
                    break;

                case '$':
                    this.ReadChar();
                    if (IsWordChar(_char, true))
                    {
                        token = new Token(TokenType.Word, "$" + this.ReadWord(), this.Position);
                    }
                    else
                    {
                        token = new Token(TokenType.Dollar, "$", this.Position);
                    }
                    break;

                case '!':
                    this.ReadChar();
                    if (_char == '=')
                    {
                        token = new Token(TokenType.NotEquals, "!=", this.Position);
                        this.ReadChar();
                    }
                    else
                    {
                        token = new Token(TokenType.Exclamation, "!", this.Position);
                    }
                    break;

                case '=':
                    token = new Token(TokenType.Equals, "=", this.Position);
                    this.ReadChar();
                    break;

                case '>':
                    this.ReadChar();
                    if (_char == '=')
                    {
                        token = new Token(TokenType.GreaterOrEquals, ">=", this.Position);
                        this.ReadChar();
                    }
                    else
                    {
                        token = new Token(TokenType.Greater, ">", this.Position);
                    }
                    break;

                case '<':
                    this.ReadChar();
                    if (_char == '=')
                    {
                        token = new Token(TokenType.LessOrEquals, "<=", this.Position);
                        this.ReadChar();
                    }
                    else
                    {
                        token = new Token(TokenType.Less, "<", this.Position);
                    }
                    break;

                case '-':
                    this.ReadChar();
                    if (_char == '-')
                    {
                        this.ReadLine();
                        token = this.ReadNext(eatWhitespace);
                    }
                    else
                    {
                        token = new Token(TokenType.Minus, "-", this.Position);
                    }
                    break;

                case '+':
                    token = new Token(TokenType.Plus, "+", this.Position);
                    this.ReadChar();
                    break;

                case '*':
                    token = new Token(TokenType.Asterisk, "*", this.Position);
                    this.ReadChar();
                    break;

                case '/':
                    token = new Token(TokenType.Slash, "/", this.Position);
                    this.ReadChar();
                    break;
                case '\\':
                    token = new Token(TokenType.Backslash, @"\", this.Position);
                    this.ReadChar();
                    break;

                case '%':
                    token = new Token(TokenType.Percent, "%", this.Position);
                    this.ReadChar();
                    break;

                case '\"':
                case '\'':
                    token = new Token(TokenType.String, this.ReadString(_char), this.Position);
                    break;

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
                    var dbl = false;
                    var number = this.ReadNumber(ref dbl);
                    token = new Token(dbl ? TokenType.Double : TokenType.Int, number, this.Position);
                    break;

                case ' ':
                case '\n':
                case '\r':
                case '\t':
                    var sb = new StringBuilder();
                    while(char.IsWhiteSpace(_char) && !_eof)
                    {
                        sb.Append(_char);
                        this.ReadChar();
                    }
                    token = new Token(TokenType.Whitespace, sb.ToString(), this.Position);
                    break;

                default:
                    // test if first char is an word 
                    if (IsWordChar(_char, true))
                    {
                        token = new Token(TokenType.Word, this.ReadWord(), this.Position);
                    }
                    else
                    {
                        this.ReadChar();
                    }
                    break;
            }

            return token ?? new Token(TokenType.Unknown, _char.ToString(), this.Position);
        }

        /// <summary>
        /// Eat all whitespace - used before a valid token
        /// </summary>
        private void EatWhitespace()
        {
            while (char.IsWhiteSpace(_char) && !_eof)
            {
                this.ReadChar();
            }
        }

        /// <summary>
        /// Read a word (word = [\w$]+)
        /// </summary>
        private string ReadWord()
        {
            var sb = new StringBuilder();
            sb.Append(_char);

            this.ReadChar();

            while (!_eof && IsWordChar(_char, false))
            {
                sb.Append(_char);
                this.ReadChar();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Read a number - it's accepts all number char, but not validate. When run Convert, .NET will check if number is correct
        /// </summary>
        private string ReadNumber(ref bool dbl)
        {
            var sb = new StringBuilder();
            sb.Append(_char);

            var canDot = true;
            var canE = true;
            var canSign = false;

            this.ReadChar();

            while (!_eof &&
                (char.IsDigit(_char) || _char == '+' || _char == '-' || _char == '.' || _char == 'e' || _char == 'E'))
            {
                if (_char == '.')
                {
                    if (canDot == false) break;
                    dbl = true;
                    canDot = false;
                }
                else if (_char == 'e' || _char == 'E')
                {
                    if (canE == false) break;
                    canE = false;
                    canSign = true;
                }
                else if (_char == '-' || _char == '+')
                {
                    if (canSign == false) break;
                    canSign = false;
                }

                sb.Append(_char);
                this.ReadChar();
            }

            return sb.ToString();
        }
        
        /// <summary>
        /// Read a string removing open and close " or '
        /// </summary>
        private string ReadString(char quote)
        {
            var sb = new StringBuilder();
            this.ReadChar(); // remove first " or '

            while (_char != quote && !_eof)
            {
                if (_char == '\\')
                {
                    this.ReadChar();

                    if (_char == quote) sb.Append(quote);

                    switch (_char)
                    {
                        case '\\': sb.Append('\\'); break;
                        case '/': sb.Append('/'); break;
                        case 'b': sb.Append('\b'); break;
                        case 'f': sb.Append('\f'); break;
                        case 'n': sb.Append('\n'); break;
                        case 'r': sb.Append('\r'); break;
                        case 't': sb.Append('\t'); break;
                        case 'u':
                            var codePoint = ParseUnicode(this.ReadChar(), this.ReadChar(), this.ReadChar(), this.ReadChar());
                            sb.Append((char)codePoint);
                            break;
                    }
                }
                else
                {
                    sb.Append(_char);
                }

                this.ReadChar();
            }

            this.ReadChar(); // read last " or '

            return sb.ToString();
        }

        /// <summary>
        /// Read all chars to end of LINE
        /// </summary>
        private void ReadLine()
        {
            // remove all char until new line
            while (_char != '\n' && !_eof)
            {
                this.ReadChar();
            }
            if (_char == '\n') this.ReadChar();
        }

        public static uint ParseUnicode(char c1, char c2, char c3, char c4)
        {
            uint p1 = ParseSingleChar(c1, 0x1000);
            uint p2 = ParseSingleChar(c2, 0x100);
            uint p3 = ParseSingleChar(c3, 0x10);
            uint p4 = ParseSingleChar(c4, 1);

            return p1 + p2 + p3 + p4;
        }

        public static uint ParseSingleChar(char c1, uint multiplier)
        {
            uint p1 = 0;
            if (c1 >= '0' && c1 <= '9')
                p1 = (uint)(c1 - '0') * multiplier;
            else if (c1 >= 'A' && c1 <= 'F')
                p1 = (uint)((c1 - 'A') + 10) * multiplier;
            else if (c1 >= 'a' && c1 <= 'f')
                p1 = (uint)((c1 - 'a') + 10) * multiplier;
            return p1;
        }

        public override string ToString()
        {
            return this.Current?.ToString() + " [ahead: " + _ahead?.ToString() + "] - position: " + this.Position;
        }
    }
}

namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// The main exception for LiteDB
    /// </summary>
    public class UltraLiteException : Exception
    {
        #region Errors code

        public const int FILE_NOT_FOUND = 101;
        public const int INVALID_DATABASE = 103;
        public const int INVALID_DATABASE_VERSION = 104;
        public const int FILE_SIZE_EXCEEDED = 105;
        public const int COLLECTION_LIMIT_EXCEEDED = 106;
        public const int INDEX_DROP_IP = 108;
        public const int INDEX_LIMIT_EXCEEDED = 109;
        public const int INDEX_DUPLICATE_KEY = 110;
        public const int INDEX_KEY_TOO_LONG = 111;
        public const int INDEX_NOT_FOUND = 112;
        public const int LOCK_TIMEOUT = 120;
        public const int INVALID_COMMAND = 121;
        public const int ALREADY_EXISTS_COLLECTION_NAME = 122;
        public const int DATABASE_WRONG_PASSWORD = 123;
        public const int SYNTAX_ERROR = 127;

        public const int INVALID_FORMAT = 200;
        public const int DOCUMENT_MAX_DEPTH = 201;
        public const int INVALID_CTOR = 202;
        public const int UNEXPECTED_TOKEN = 203;
        public const int INVALID_DATA_TYPE = 204;
        public const int PROPERTY_NOT_MAPPED = 206;
        public const int INVALID_TYPED_NAME = 207;



        #endregion

        #region Ctor

        public int ErrorCode { get; private set; }
        public string Line { get; private set; }
        public long Position { get; private set; }

        public UltraLiteException(string message)
            : base(message)
        {
        }

        internal UltraLiteException(int code, string message, params object[] args)
            : base(string.Format(message, args))
        {
            this.ErrorCode = code;
        }

        internal UltraLiteException (int code, Exception inner, string message, params object[] args)
        : base (string.Format (message, args), inner)
        {
            this.ErrorCode = code;
        }

        #endregion

        #region Method Errors

        internal static UltraLiteException FileNotFound(string fileId)
        {
            return new UltraLiteException(FILE_NOT_FOUND, "File '{0}' not found.", fileId);
        }

        internal static UltraLiteException InvalidDatabase()
        {
            return new UltraLiteException(INVALID_DATABASE, "Datafile is not a LiteDB database.");
        }

        internal static UltraLiteException InvalidDatabaseVersion(int version)
        {
            return new UltraLiteException(INVALID_DATABASE_VERSION, "Invalid database version: {0}", version);
        }

        internal static UltraLiteException FileSizeExceeded(long limit)
        {
            return new UltraLiteException(FILE_SIZE_EXCEEDED, "Database size exceeds limit of {0}.", StorageUnitHelper.FormatFileSize(limit));
        }

        internal static UltraLiteException CollectionLimitExceeded(int limit)
        {
            return new UltraLiteException(COLLECTION_LIMIT_EXCEEDED, "This database exceeded the maximum limit of collection names size: {0} bytes", limit);
        }

        internal static UltraLiteException IndexDropId()
        {
            return new UltraLiteException(INDEX_DROP_IP, "Primary key index '_id' can't be dropped.");
        }

        internal static UltraLiteException IndexLimitExceeded(string collection)
        {
            return new UltraLiteException(INDEX_LIMIT_EXCEEDED, "Collection '{0}' exceeded the maximum limit of indices: {1}", collection, CollectionIndex.INDEX_PER_COLLECTION);
        }

        internal static UltraLiteException IndexDuplicateKey(string field, BsonValue key)
        {
            return new UltraLiteException(INDEX_DUPLICATE_KEY, "Cannot insert duplicate key in unique index '{0}'. The duplicate value is '{1}'.", field, key);
        }

        internal static UltraLiteException IndexKeyTooLong()
        {
            return new UltraLiteException(INDEX_KEY_TOO_LONG, "Index key must be less than {0} bytes.", IndexService.MAX_INDEX_LENGTH);
        }

        internal static UltraLiteException IndexNotFound(string collection, string field)
        {
            return new UltraLiteException(INDEX_NOT_FOUND, "Index not found on '{0}.{1}'.", collection, field);
        }

        internal static UltraLiteException LockTimeout(TimeSpan ts)
        {
            return new UltraLiteException(LOCK_TIMEOUT, "Timeout. Database is locked for more than {0}.", ts.ToString());
        }

        internal static UltraLiteException AlreadyExistsCollectionName(string newName)
        {
            return new UltraLiteException(ALREADY_EXISTS_COLLECTION_NAME, "New collection name '{0}' already exists.", newName);
        }

        internal static UltraLiteException DatabaseWrongPassword()
        {
            return new UltraLiteException(DATABASE_WRONG_PASSWORD, "Invalid database password.");
        }

        internal static UltraLiteException InvalidFormat(string field)
        {
            return new UltraLiteException(INVALID_FORMAT, "Invalid format: {0}", field);
        }

        internal static UltraLiteException SyntaxError(StringScanner s, string message = "Unexpected token")
        {
            return new UltraLiteException(SYNTAX_ERROR, message)
            {
                Line = s.Source,
                Position = s.Index
            };
        }

        internal static UltraLiteException UnexpectedToken(Token token, string expected = null)
        {
            var position = (token?.Position - (token?.Value?.Length ?? 0)) ?? 0;
            var str = token?.Type == TokenType.EOF ? "[EOF]" : token?.Value ?? "";
            var exp = expected == null ? "" : $" Expected `{expected}`.";

            return new UltraLiteException(UNEXPECTED_TOKEN, $"Unexpected token `{str}` in position {position}.{exp}")
            {
                Position = position
            };
        }

        #endregion

        #region Document/Mapper Errors

        internal static UltraLiteException InvalidFormat(string field, string format)
        {
            return new UltraLiteException(INVALID_FORMAT, "Invalid format: {0}", field);
        }

        internal static UltraLiteException DocumentMaxDepth(int depth, Type type)
        {
            return new UltraLiteException(DOCUMENT_MAX_DEPTH, "Document has more than {0} nested documents in '{1}'. Check for circular references.", depth, type == null ? "-" : type.Name);
        }

        internal static UltraLiteException InvalidCtor(Type type, Exception inner)
        {
            return new UltraLiteException(INVALID_CTOR, inner, "Failed to create instance for type '{0}' from assembly '{1}'. Checks if the class has a public constructor with no parameters.", type.FullName, type.AssemblyQualifiedName);
        }

        internal static UltraLiteException UnexpectedToken(string token)
        {
            return new UltraLiteException(UNEXPECTED_TOKEN, "Unexpected JSON token: {0}", token);
        }

        internal static UltraLiteException InvalidDataType(string field, BsonValue value)
        {
            return new UltraLiteException(INVALID_DATA_TYPE, "Invalid BSON data type '{0}' on field '{1}'.", value.Type, field);
        }

        public const int PROPERTY_READ_WRITE = 204;

        internal static UltraLiteException PropertyReadWrite(PropertyInfo prop)
        {
            return new UltraLiteException(PROPERTY_READ_WRITE, "'{0}' property must have public getter and setter.", prop.Name);
        }

        internal static UltraLiteException PropertyNotMapped(string name)
        {
            return new UltraLiteException(PROPERTY_NOT_MAPPED, "Property '{0}' was not mapped into BsonDocument.", name);
        }

        internal static UltraLiteException InvalidTypedName(string type)
        {
            return new UltraLiteException(INVALID_TYPED_NAME, "Type '{0}' not found in current domain (_type format is 'Type.FullName, AssemblyName').", type);
        }

        #endregion
    }
}
#pragma warning restore

#endregion

#region RECOVERY
#pragma warning disable
namespace RitaEngine.Base.Resources.Storage.UltraLiteDB
{
    public partial class UltraLiteEngine
    {
        /// <summary>
        /// Try recovery data from current datafile into a new datafile.
        /// </summary>
        public static string Recovery(string filename)
        {
            // if not exists, just exit
            if (!File.Exists(filename)) return "";

            var log = new StringBuilder();
            var newfilename = FileHelper.GetTempFile(filename, "-recovery", true);
            var count = 0;

            using (var olddb = new UltraLiteEngine(filename))
            using (var newdb = new UltraLiteEngine(newfilename, false))
            {
                // get header from old database (this must must be possible to read)
                var header = olddb._pager.GetPage<HeaderPage>(0);

                var collections = RecoveryCollectionPages(olddb, header, log);

                // try recovery all data pages
                for (uint i = 1; i < header.LastPageID; i++)
                {
                    DataPage dataPage = null;

                    try
                    {
                        var buffer = olddb._disk.ReadPage(i);

                        // searching only for DataPage (PageType == 4)
                        if (buffer[4] != 4) continue;

                        dataPage = BasePage.ReadPage(buffer) as DataPage;
                    }
                    catch (Exception ex)
                    {
                        log.AppendLine($"Page {i} (DataPage) Error: {ex.Message}");
                        continue;
                    }

                    // try find collectionName using pageID map (use fixed name if not found)
                    if (collections.TryGetValue(i, out var colname) == false)
                    {
                        colname = "_recovery";
                    }

                    foreach (var block in dataPage.DataBlocks)
                    {
                        try
                        {
                            // read bytes
                            var bson = olddb._data.Read(block.Value.Position);

                            // deserialize as document
                            var doc = BsonSerializer.Deserialize(bson);

                            // and insert into new database
                            newdb.Insert(colname, doc);

                            count++;
                        }
                        catch (Exception ex)
                        {
                            log.AppendLine($"Document {block.Value.Position} Error: {ex.Message}");
                            continue;
                        }
                    }
                }
            }

            log.Insert(0, $"Document recovery count: {count}\n");

            return log.ToString();
        }

        private static Dictionary<uint, string> RecoveryCollectionPages(UltraLiteEngine engine, HeaderPage header, StringBuilder log)
        {
            var result = new Dictionary<uint, string>();

            // get collection page
            foreach (var col in header.CollectionPages)
            {
                CollectionPage colPage = null;

                try
                {
                    // read collection page
                    var buffer = engine._disk.ReadPage(col.Value);
                    var page = BasePage.ReadPage(buffer);

                    if (page.PageType != PageType.Collection) continue;

                    colPage = page as CollectionPage;
                }
                catch (Exception ex)
                {
                    log.AppendLine($"Page {col.Value} (Collection) Error: {ex.Message}");
                    continue;
                }

                // get all pageID from all valid indexes
                var pagesID = new HashSet<uint>(colPage.Indexes.Where(x => x.IsEmpty == false && x.HeadNode.PageID != uint.MaxValue).Select(x => x.HeadNode.PageID));

                // load all dataPages from this initial index pageIDs
                var dataPages = RecoveryDetectCollectionByIndexPages(engine, pagesID, log);

                // populate resultset with this collection name/data page
                foreach(var page in dataPages)
                {
                    result[page] = col.Key;
                }
            }

            return result;
        }

        private static HashSet<uint> RecoveryDetectCollectionByIndexPages(UltraLiteEngine engine, HashSet<uint> initialPagesID, StringBuilder log)
        {
            var indexPages = new Dictionary<uint, bool>();
            var dataPages = new HashSet<uint>();

            foreach(var pageID in initialPagesID)
            {
                indexPages.Add(pageID, false);
            }

            // discover all indexes pages related with this current indexPage (all of them are in same collection)
            while (indexPages.Count(x => x.Value == false) > 0)
            {
                var item = indexPages.First(x => x.Value == false);

                // mark page as readed
                indexPages[item.Key] = true;
                IndexPage indexPage = null;

                try
                {
                    // try read page from disk and deserialize as IndexPage
                    var buffer = engine._disk.ReadPage(item.Key);
                    var page = BasePage.ReadPage(buffer);

                    if (page.PageType != PageType.Index) continue;

                    indexPage = page as IndexPage;
                }
                catch(Exception ex)
                {
                    log.AppendLine($"Page {item.Key} (Collection) Error: {ex.Message}");
                    continue;
                }

                // now, check for all nodes to get dataPages
                foreach (var node in indexPage.Nodes.Values)
                {
                    if (node.DataBlock.PageID != uint.MaxValue)
                    {
                        dataPages.Add(node.DataBlock.PageID);
                    }

                    // add into indexPages all possible indexPages
                    if (!indexPages.ContainsKey(node.PrevNode.PageID) && node.PrevNode.PageID != uint.MaxValue)
                    {
                        indexPages.Add(node.PrevNode.PageID, false);
                    }

                    if (!indexPages.ContainsKey(node.NextNode.PageID) && node.NextNode.PageID != uint.MaxValue)
                    {
                        indexPages.Add(node.NextNode.PageID, false);
                    }

                    foreach (var pos in node.Prev.Where(x => !x.IsEmpty && x.PageID != uint.MaxValue))
                    {
                        if (!indexPages.ContainsKey(pos.PageID)) indexPages.Add(pos.PageID, false);
                    }

                    foreach (var pos in node.Next.Where(x => !x.IsEmpty && x.PageID != uint.MaxValue))
                    {
                        if (!indexPages.ContainsKey(pos.PageID)) indexPages.Add(pos.PageID, false);
                    }
                }
            }

            return dataPages;
        }
    }
}

#endregion

#pragma warning restore