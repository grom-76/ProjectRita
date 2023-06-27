using System;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading;
using System.Diagnostics;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Collections.Concurrent;
using System.Linq;
using System.IO;

#pragma warning disable
#region DOCUMENT

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Represent a 12-bytes BSON type used in document Id
    /// </summary>
    public class ObjectId : IComparable<ObjectId>, IEquatable<ObjectId>
    {
        /// <summary>
        /// A zero 12-bytes ObjectId
        /// </summary>
        public static ObjectId Empty => new ObjectId();

        #region Properties

        /// <summary>
        /// Get timestamp
        /// </summary>
        public int Timestamp { get; }

        /// <summary>
        /// Get machine number
        /// </summary>
        public int Machine { get; }

        /// <summary>
        /// Get pid number
        /// </summary>
        public short Pid { get; }

        /// <summary>
        /// Get increment
        /// </summary>
        public int Increment { get; }

        /// <summary>
        /// Get creation time
        /// </summary>
        public DateTime CreationTime
        {
            get { return BsonValue.UnixEpoch.AddSeconds(this.Timestamp); }
        }

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new empty instance of the ObjectId class.
        /// </summary>
        public ObjectId()
        {
            this.Timestamp = 0;
            this.Machine = 0;
            this.Pid = 0;
            this.Increment = 0;
        }

        /// <summary>
        /// Initializes a new instance of the ObjectId class from ObjectId vars.
        /// </summary>
        public ObjectId(int timestamp, int machine, short pid, int increment)
        {
            this.Timestamp = timestamp;
            this.Machine = machine;
            this.Pid = pid;
            this.Increment = increment;
        }

        /// <summary>
        /// Initializes a new instance of ObjectId class from another ObjectId.
        /// </summary>
        public ObjectId(ObjectId from)
        {
            this.Timestamp = from.Timestamp;
            this.Machine = from.Machine;
            this.Pid = from.Pid;
            this.Increment = from.Increment;
        }

        /// <summary>
        /// Initializes a new instance of the ObjectId class from hex string.
        /// </summary>
        public ObjectId(string value)
            : this(FromHex(value))
        {
        }

        /// <summary>
        /// Initializes a new instance of the ObjectId class from byte array.
        /// </summary>
        public ObjectId(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));

            this.Timestamp = 
                (bytes[startIndex + 0] << 24) + 
                (bytes[startIndex + 1] << 16) + 
                (bytes[startIndex + 2] << 8) + 
                bytes[startIndex + 3];

            this.Machine = 
                (bytes[startIndex + 4] << 16) + 
                (bytes[startIndex + 5] << 8) + 
                bytes[startIndex + 6];

            this.Pid = (short)
                ((bytes[startIndex + 7] << 8) + 
                bytes[startIndex + 8]);

            this.Increment = 
                (bytes[startIndex + 9] << 16) + 
                (bytes[startIndex + 10] << 8) + 
                bytes[startIndex + 11];
        }

        /// <summary>
        /// Convert hex value string in byte array
        /// </summary>
        private static byte[] FromHex(string value)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
            if (value.Length != 24) throw new ArgumentException(string.Format("ObjectId strings should be 24 hex characters, got {0} : \"{1}\"", value.Length, value));

            var bytes = new byte[12];

            for (var i = 0; i < 24; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(value.Substring(i, 2), 16);
            }

            return bytes;
        }

        #endregion

        #region Equals/CompareTo/ToString

        /// <summary>
        /// Checks if this ObjectId is equal to the given object. Returns true
        /// if the given object is equal to the value of this instance. 
        /// Returns false otherwise.
        /// </summary>
        public bool Equals(ObjectId? other)
        {
            return other! != null! && 
                this.Timestamp == other.Timestamp &&
                this.Machine == other.Machine &&
                this.Pid == other.Pid &&
                this.Increment == other.Increment;
        }

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        public override bool Equals(object? other)
        {
            return Equals(other as ObjectId);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            int hash = 17;
            hash = 37 * hash + this.Timestamp.GetHashCode();
            hash = 37 * hash + this.Machine.GetHashCode();
            hash = 37 * hash + this.Pid.GetHashCode();
            hash = 37 * hash + this.Increment.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Compares two instances of ObjectId
        /// </summary>
        public int CompareTo(ObjectId other)
        {
            var r = this.Timestamp.CompareTo(other.Timestamp);
            if (r != 0) return r;

            r = this.Machine.CompareTo(other.Machine);
            if (r != 0) return r;

            r = this.Pid.CompareTo(other.Pid);
            if (r != 0) return r < 0 ? -1 : 1;

            return this.Increment.CompareTo(other.Increment);
        }

        /// <summary>
        /// Represent ObjectId as 12 bytes array
        /// </summary>
        public void ToByteArray(byte[] bytes, int startIndex)
        {
            bytes[startIndex + 0] = (byte)(this.Timestamp >> 24);
            bytes[startIndex + 1] = (byte)(this.Timestamp >> 16);
            bytes[startIndex + 2] = (byte)(this.Timestamp >> 8);
            bytes[startIndex + 3] = (byte)(this.Timestamp);
            bytes[startIndex + 4] = (byte)(this.Machine >> 16);
            bytes[startIndex + 5] = (byte)(this.Machine >> 8);
            bytes[startIndex + 6] = (byte)(this.Machine);
            bytes[startIndex + 7] = (byte)(this.Pid >> 8);
            bytes[startIndex + 8] = (byte)(this.Pid);
            bytes[startIndex + 9] = (byte)(this.Increment >> 16);
            bytes[startIndex + 10] = (byte)(this.Increment >> 8);
            bytes[startIndex + 11] = (byte)(this.Increment);
        }

        public byte[] ToByteArray()
        {
            var bytes = new byte[12];

            this.ToByteArray(bytes, 0);

            return bytes;
        }

        public override string ToString()
        {
            return BitConverter.ToString(this.ToByteArray()).Replace("-", "").ToLower();
        }

        #endregion

        #region Operators

        public static bool operator ==(ObjectId lhs, ObjectId rhs)
        {
            if (lhs is null) return rhs is null;
            if (rhs is null) return false; // don't check type because sometimes different types can be ==

            return lhs.Equals(rhs);
        }

        public static bool operator !=(ObjectId lhs, ObjectId rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator >=(ObjectId lhs, ObjectId rhs)
        {
            return lhs.CompareTo(rhs) >= 0;
        }

        public static bool operator >(ObjectId lhs, ObjectId rhs)
        {
            return lhs.CompareTo(rhs) > 0;
        }

        public static bool operator <(ObjectId lhs, ObjectId rhs)
        {
            return lhs.CompareTo(rhs) < 0;
        }

        public static bool operator <=(ObjectId lhs, ObjectId rhs)
        {
            return lhs.CompareTo(rhs) <= 0;
        }

        #endregion

        #region Static methods

        private static int _machine;
        private static short _pid;
        private static int _increment;

        // static constructor
        static ObjectId()
        {
            _machine = (GetMachineHash() +
                10000 // Magic number
                ) & 0x00ffffff;
            _increment = (new Random()).Next();

            try
            {
                _pid = (short)GetCurrentProcessId();
            }
            catch (SecurityException)
            {
                _pid = 0;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int GetCurrentProcessId()
        {
            return (new Random()).Next(0, 5000); // Any same number for this process
        }

        private static int GetMachineHash()
        {
            var hostName =
                "SOMENAME";
            return 0x00ffffff & hostName.GetHashCode(); // use first 3 bytes of hash
        }

        /// <summary>
        /// Creates a new ObjectId.
        /// </summary>
        public static ObjectId NewObjectId()
        {
            var timestamp = (long)System.Math.Floor((DateTime.UtcNow - BsonValue.UnixEpoch).TotalSeconds);
            var inc = Interlocked.Increment(ref _increment) & 0x00ffffff;

            return new ObjectId((int)timestamp, _machine, _pid, inc);
        }

        #endregion
    }
}

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Represent a Bson Value used in BsonDocument
    /// </summary>
    public class BsonValue : IComparable<BsonValue>, IEquatable<BsonValue>
    {
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Represent a Null bson type
        /// </summary>
        public static BsonValue Null = new BsonValue(BsonType.Null, null);

        /// <summary>
        /// Represent a MinValue bson type
        /// </summary>
        public static BsonValue MinValue = new BsonValue(BsonType.MinValue, "-oo");

        /// <summary>
        /// Represent a MaxValue bson type
        /// </summary>
        public static BsonValue MaxValue = new BsonValue(BsonType.MaxValue, "+oo");

        /// <summary>
        /// Indicate BsonType of this BsonValue
        /// </summary>
        public BsonType Type { get; }

        /// <summary>
        /// Get internal .NET value object
        /// </summary>
        internal virtual object RawValue { get; }

        #region Constructor

        public BsonValue()
        {
            this.Type = BsonType.Null;
            this.RawValue = null;
        }

        public BsonValue(Int32 value)
        {
            this.Type = BsonType.Int32;
            this.RawValue = value;
        }

        public BsonValue(Int64 value)
        {
            this.Type = BsonType.Int64;
            this.RawValue = value;
        }

        public BsonValue(Single value)
        {
            this.Type = BsonType.Double;
            this.RawValue = (Double)value;
        }

        public BsonValue(Double value)
        {
            this.Type = BsonType.Double;
            this.RawValue = value;
        }

        public BsonValue(Decimal value)
        {
            this.Type = BsonType.Decimal;
            this.RawValue = value;
        }

        public BsonValue(String value)
        {
            this.Type = value == null ? BsonType.Null : BsonType.String;
            this.RawValue = value;
        }

        public BsonValue(Byte[] value)
        {
            this.Type = value == null ? BsonType.Null : BsonType.Binary;
            this.RawValue = value;
        }

        public BsonValue(ObjectId value)
        {
            this.Type = value == null ? BsonType.Null : BsonType.ObjectId;
            this.RawValue = value;
        }

        public BsonValue(Guid value)
        {
            this.Type = BsonType.Guid;
            this.RawValue = value;
        }

        public BsonValue(Boolean value)
        {
            this.Type = BsonType.Boolean;
            this.RawValue = value;
        }

        public BsonValue(DateTime value)
        {
            this.Type = BsonType.DateTime;
            this.RawValue = value.Truncate();
        }

        public BsonValue(BsonValue value)
        {
            this.Type = value == null ? BsonType.Null : value.Type;
            this.RawValue = value.RawValue;
        }
        
        public static BsonValue FromObject(object value)
        {
            if (value == null) return new BsonValue(BsonType.Null, null);
            else if (value is Int32) return new BsonValue((Int32)value);
            else if (value is Int64) return new BsonValue((Int64)value);
            else if (value is Single) return new BsonValue((Double)value);
            else if (value is Double) return new BsonValue((Double)value);
            else if (value is Decimal) return new BsonValue((Decimal)value);
            else if (value is String) return new BsonValue((String)value);
            else if (value is IDictionary<string, BsonValue>) return new BsonDocument((IDictionary<string, BsonValue>)value);
            else if (value is IDictionary) return new BsonDocument((IDictionary)value);
            else if (value is List<BsonValue>) return new BsonArray((List<BsonValue>)value);
            else if (value is IEnumerable) return new BsonArray((IEnumerable)value);
            else if (value is Byte[]) return new BsonValue((Byte[])value);
            else if (value is ObjectId) return new BsonValue((ObjectId)value);
            else if (value is Guid) return new BsonValue((Guid)value);
            else if (value is Boolean) return new BsonValue((Boolean)value);
            else if (value is DateTime) return new BsonValue((DateTime)value);
            else if (value is BsonValue) return new BsonValue((BsonValue)value);
            else
            {
                throw new InvalidCastException("Value is not a valid BSON data type - Use Mapper.ToDocument for more complex types converts");
            }
        }


        protected BsonValue(BsonType type, object rawValue)
        {
            this.Type = type;
            this.RawValue = rawValue;
        }

        #endregion

        #region Index "this" property

        /// <summary>
        /// Get/Set a field for document. Fields are case sensitive - Works only when value are document
        /// </summary>
        public virtual BsonValue this[string name]
        {
            get => throw new InvalidOperationException("Cannot access non-document type value on " + this.RawValue);
            set => throw new InvalidOperationException("Cannot access non-document type value on " + this.RawValue);
        }

        /// <summary>
        /// Get/Set value in array position. Works only when value are array
        /// </summary>
        public virtual BsonValue this[int index]
        {
            get => throw new InvalidOperationException("Cannot access non-array type value on " + this.RawValue);
            set => throw new InvalidOperationException("Cannot access non-array type value on " + this.RawValue);
        }

        #endregion

        #region Convert types

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public BsonArray AsArray => this as BsonArray;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public BsonDocument AsDocument => this as BsonDocument;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Byte[] AsBinary => this.RawValue as Byte[];

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool AsBoolean => (bool)this.RawValue;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string AsString => (string)this.RawValue;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int AsInt32 => Convert.ToInt32(this.RawValue);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long AsInt64 => Convert.ToInt64(this.RawValue);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public float AsSingle => Convert.ToSingle(this.RawValue);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public double AsDouble => Convert.ToDouble(this.RawValue);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public decimal AsDecimal => Convert.ToDecimal(this.RawValue);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public DateTime AsDateTime => (DateTime)this.RawValue;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ObjectId AsObjectId => (ObjectId)this.RawValue;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Guid AsGuid => (Guid)this.RawValue;

        #endregion

        #region IsTypes

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsNull => this.Type == BsonType.Null;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsArray => this.Type == BsonType.Array;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsDocument => this.Type == BsonType.Document;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsInt32 => this.Type == BsonType.Int32;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsInt64 => this.Type == BsonType.Int64;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsSingle => this.Type == BsonType.Double;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsDouble => this.Type == BsonType.Double;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsDecimal => this.Type == BsonType.Decimal;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsNumber => this.IsInt32 || this.IsInt64 || this.IsDouble || this.IsDecimal;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsBinary => this.Type == BsonType.Binary;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsBoolean => this.Type == BsonType.Boolean;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsString => this.Type == BsonType.String;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsObjectId => this.Type == BsonType.ObjectId;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsGuid => this.Type == BsonType.Guid;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsDateTime => this.Type == BsonType.DateTime;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsMinValue => this.Type == BsonType.MinValue;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsMaxValue => this.Type == BsonType.MaxValue;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsDefaultValue
        {
            get
            {
                switch (this.Type)
                {
                    case BsonType.Null: return true;
                    case BsonType.MinValue:
                    case BsonType.MaxValue:
                        return false;
                    case BsonType.Int32: return this.AsInt32 != 0;
                    case BsonType.Int64: return this.AsInt64 != 0L;
                    case BsonType.Double: return this.AsDouble != 0.0;
                    case BsonType.Decimal: return this.AsDecimal != 0L;
                    case BsonType.String: return this.AsString.IsNullOrEmpty();
                    case BsonType.Document: return this.AsDocument.Count > 0;
                    case BsonType.Array: return this.AsArray.Count > 0;
                    case BsonType.Binary: return this.AsBinary.Length > 0;
                    case BsonType.ObjectId: return this.AsObjectId == ObjectId.Empty;
                    case BsonType.Guid: return this.AsGuid == Guid.Empty;
                    case BsonType.Boolean: return this.AsBoolean == false;
                    case BsonType.DateTime: return this.AsDateTime == DateTime.MinValue;
                    default: throw new NotImplementedException();
                }
            }
        }

        #endregion

        #region Implicit Ctor

        // Int32
        public static implicit operator Int32(BsonValue value)
        {
            return (Int32)value.RawValue;
        }

        // Int32
        public static implicit operator BsonValue(Int32 value)
        {
            return new BsonValue(value);
        }

        // Int64
        public static implicit operator Int64(BsonValue value)
        {
            return (Int64)value.RawValue;
        }

        // Int64
        public static implicit operator BsonValue(Int64 value)
        {
            return new BsonValue(value);
        }

        // Single
        public static implicit operator Single(BsonValue value)
        {
            return (Single)value.RawValue;
        }

        // Double
        public static implicit operator Double(BsonValue value)
        {
            return (Double)value.RawValue;
        }

        // Double
        public static implicit operator BsonValue(Double value)
        {
            return new BsonValue(value);
        }

        // Decimal
        public static implicit operator Decimal(BsonValue value)
        {
            return (Decimal)value.RawValue;
        }

        // Decimal
        public static implicit operator BsonValue(Decimal value)
        {
            return new BsonValue(value);
        }

        // UInt64 (to avoid ambigous between Double-Decimal)
        public static implicit operator UInt64(BsonValue value)
        {
            return (UInt64)value.RawValue;
        }

        // Decimal
        public static implicit operator BsonValue(UInt64 value)
        {
            return new BsonValue((Double)value);
        }

        // String
        public static implicit operator String(BsonValue value)
        {
            return (String)value.RawValue;
        }

        // String
        public static implicit operator BsonValue(String value)
        {
            return new BsonValue(value);
        }

        // Binary
        public static implicit operator Byte[] (BsonValue value)
        {
            return (Byte[])value.RawValue;
        }

        // Binary
        public static implicit operator BsonValue(Byte[] value)
        {
            return new BsonValue(value);
        }

        // ObjectId
        public static implicit operator ObjectId(BsonValue value)
        {
            return (ObjectId)value.RawValue;
        }

        // ObjectId
        public static implicit operator BsonValue(ObjectId value)
        {
            return new BsonValue(value);
        }

        // Guid
        public static implicit operator Guid(BsonValue value)
        {
            return (Guid)value.RawValue;
        }

        // Guid
        public static implicit operator BsonValue(Guid value)
        {
            return new BsonValue(value);
        }

        // Boolean
        public static implicit operator Boolean(BsonValue value)
        {
            return (Boolean)value.RawValue;
        }

        // Boolean
        public static implicit operator BsonValue(Boolean value)
        {
            return new BsonValue(value);
        }

        // DateTime
        public static implicit operator DateTime(BsonValue value)
        {
            return (DateTime)value.RawValue;
        }

        // DateTime
        public static implicit operator BsonValue(DateTime value)
        {
            return new BsonValue(value);
        }

        // +
        public static BsonValue operator +(BsonValue left, BsonValue right)
        {
            if (!left.IsNumber || !right.IsNumber) return BsonValue.Null;

            if (left.IsInt32 && right.IsInt32) return left.AsInt32 + right.AsInt32;
            if (left.IsInt64 && right.IsInt64) return left.AsInt64 + right.AsInt64;
            if (left.IsDouble && right.IsDouble) return left.AsDouble + right.AsDouble;
            if (left.IsDecimal && right.IsDecimal) return left.AsDecimal + right.AsDecimal;

            var result = left.AsDecimal + right.AsDecimal;
            var type = (BsonType)System.Math.Max((int)left.Type, (int)right.Type);

            return
                type == BsonType.Int64 ? new BsonValue((Int64)result) :
                type == BsonType.Double ? new BsonValue((Double)result) :
                new BsonValue(result);
        }

        // -
        public static BsonValue operator -(BsonValue left, BsonValue right)
        {
            if (!left.IsNumber || !right.IsNumber) return BsonValue.Null;

            if (left.IsInt32 && right.IsInt32) return left.AsInt32 - right.AsInt32;
            if (left.IsInt64 && right.IsInt64) return left.AsInt64 - right.AsInt64;
            if (left.IsDouble && right.IsDouble) return left.AsDouble - right.AsDouble;
            if (left.IsDecimal && right.IsDecimal) return left.AsDecimal - right.AsDecimal;

            var result = left.AsDecimal - right.AsDecimal;
            var type = (BsonType)System.Math.Max((int)left.Type, (int)right.Type);

            return
                type == BsonType.Int64 ? new BsonValue((Int64)result) :
                type == BsonType.Double ? new BsonValue((Double)result) :
                new BsonValue(result);
        }

        // *
        public static BsonValue operator *(BsonValue left, BsonValue right)
        {
            if (!left.IsNumber || !right.IsNumber) return BsonValue.Null;

            if (left.IsInt32 && right.IsInt32) return left.AsInt32 * right.AsInt32;
            if (left.IsInt64 && right.IsInt64) return left.AsInt64 * right.AsInt64;
            if (left.IsDouble && right.IsDouble) return left.AsDouble * right.AsDouble;
            if (left.IsDecimal && right.IsDecimal) return left.AsDecimal * right.AsDecimal;

            var result = left.AsDecimal * right.AsDecimal;
            var type = (BsonType)System.Math.Max((int)left.Type, (int)right.Type);

            return
                type == BsonType.Int64 ? new BsonValue((Int64)result) :
                type == BsonType.Double ? new BsonValue((Double)result) :
                new BsonValue(result);
        }

        // /
        public static BsonValue operator /(BsonValue left, BsonValue right)
        {
            if (!left.IsNumber || !right.IsNumber) return BsonValue.Null;
            if (left.IsDecimal || right.IsDecimal) return left.AsDecimal / right.AsDecimal;

            return left.AsDouble / right.AsDouble;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        #endregion

        #region IComparable<BsonValue>, IEquatable<BsonValue>

        public virtual int CompareTo(BsonValue other)
        {
            return this.CompareTo(other, Collation.Binary);
        }

        public virtual int CompareTo(BsonValue other, Collation collation)
        {
            // first, test if types are different
            if (this.Type != other.Type)
            {
                // if both values are number, convert them to Decimal (128 bits) to compare
                // it's the slowest way, but more secure
                if (this.IsNumber && other.IsNumber)
                {
                    return Convert.ToDecimal(this.RawValue).CompareTo(Convert.ToDecimal(other.RawValue));
                }
                // if not, order by sort type order
                else
                {
                    return this.Type.CompareTo(other.Type);
                }
            }

            // for both values with same data type just compare
            switch (this.Type)
            {
                case BsonType.Null:
                case BsonType.MinValue:
                case BsonType.MaxValue:
                    return 0;

                case BsonType.Int32: return this.AsInt32.CompareTo(other.AsInt32);
                case BsonType.Int64: return this.AsInt64.CompareTo(other.AsInt64);
                case BsonType.Double: return this.AsDouble.CompareTo(other.AsDouble);
                case BsonType.Decimal: return this.AsDecimal.CompareTo(other.AsDecimal);

                case BsonType.String: return collation.Compare(this.AsString, other.AsString);

                case BsonType.Document: return this.AsDocument.CompareTo(other);
                case BsonType.Array: return this.AsArray.CompareTo(other);

                case BsonType.Binary: return this.AsBinary.BinaryCompareTo(other.AsBinary);
                case BsonType.ObjectId: return this.AsObjectId.CompareTo(other.AsObjectId);
                case BsonType.Guid: return this.AsGuid.CompareTo(other.AsGuid);

                case BsonType.Boolean: return this.AsBoolean.CompareTo(other.AsBoolean);
                case BsonType.DateTime:
                    var d0 = this.AsDateTime;
                    var d1 = other.AsDateTime;
                    if (d0.Kind != DateTimeKind.Utc) d0 = d0.ToUniversalTime();
                    if (d1.Kind != DateTimeKind.Utc) d1 = d1.ToUniversalTime();
                    return d0.CompareTo(d1);

                default: throw new NotImplementedException();
            }
        }

        public bool Equals(BsonValue other)
        {
            return this.CompareTo(other) == 0;
        }

        #endregion

        #region Operators

        public static bool operator ==(BsonValue lhs, BsonValue rhs)
        {
            if (object.ReferenceEquals(lhs, null)) return object.ReferenceEquals(rhs, null) || rhs.IsNull;
            if (object.ReferenceEquals(rhs, null)) return object.ReferenceEquals(lhs, null) || lhs.IsNull;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(BsonValue lhs, BsonValue rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator >=(BsonValue lhs, BsonValue rhs)
        {
            return lhs.CompareTo(rhs) >= 0;
        }

        public static bool operator >(BsonValue lhs, BsonValue rhs)
        {
            return lhs.CompareTo(rhs) > 0;
        }

        public static bool operator <(BsonValue lhs, BsonValue rhs)
        {
            return lhs.CompareTo(rhs) < 0;
        }

        public static bool operator <=(BsonValue lhs, BsonValue rhs)
        {
            return lhs.CompareTo(rhs) <= 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is BsonValue other)
            {
                return this.Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            var hash = 17;
            hash = 37 * hash + this.Type.GetHashCode();
            hash = 37 * hash + this.RawValue.GetHashCode();
            return hash;
        }

        #endregion

        #region GetBytesCount()

        /// <summary>
        /// Returns how many bytes this BsonValue will consume when converted into binary BSON
        /// If recalc = false, use cached length value (from Array/Document only)
        /// </summary>
        public virtual int GetBytesCount(bool recalc)
        {
            switch (this.Type)
            {
                case BsonType.Null:
                case BsonType.MinValue:
                case BsonType.MaxValue: return 0;

                case BsonType.Int32: return 4;
                case BsonType.Int64: return 8;
                case BsonType.Double: return 8;
                case BsonType.Decimal: return 16;

                case BsonType.String: return Encoding.UTF8.GetByteCount(this.AsString);

                case BsonType.Binary: return this.AsBinary.Length;
                case BsonType.ObjectId: return 12;
                case BsonType.Guid: return 16;

                case BsonType.Boolean: return 1;
                case BsonType.DateTime: return 8;

                case BsonType.Document: return this.AsDocument.GetBytesCount(recalc);
                case BsonType.Array: return this.AsArray.GetBytesCount(recalc);
            }

            throw new ArgumentException();
        }

        /// <summary>
        /// Get how many bytes one single element will used in BSON format
        /// </summary>
        protected int GetBytesCountElement(string key, BsonValue value)
        {
            // check if data type is variant
            var variant = value.Type == BsonType.String || value.Type == BsonType.Binary || value.Type == BsonType.Guid;

            return
                1 + // element type
                Encoding.UTF8.GetByteCount(key) + // CString
                1 + // CString \0
                value.GetBytesCount(true) +
                (variant ? 5 : 0); // bytes.Length + 0x??
        }

        #endregion

    }
}

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// All supported BsonTypes in sort order
    /// </summary>
    public enum BsonType
    {
        MinValue = 0,

        Null = 1,

        Int32 = 2,
        Int64 = 3,
        Double = 4,
        Decimal = 5,

        String = 6,

        Document = 7,
        Array = 8,

        Binary = 9,
        ObjectId = 10,
        Guid = 11,

        Boolean = 12,
        DateTime = 13,

        MaxValue = 14
    }
}

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
	public class BsonFields
	{
		private string Field;

		public BsonFields(string field)
		{
			Field = field;
		}
		public IEnumerable<BsonValue> Execute(BsonDocument doc, bool includeNullIfEmpty = true)
		{
			var index = 0;
			BsonValue value=null;
			if(doc.TryGetValue(Field, out value))
			{
				index++;
				yield return value;
			}

			if(index == 0 && includeNullIfEmpty) yield return BsonValue.Null;
		}
	}
}

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
    public class BsonDocument : BsonValue, IDictionary<string, BsonValue>
    {
        public BsonDocument()
            : base(BsonType.Document, new Dictionary<string, BsonValue>(StringComparer.OrdinalIgnoreCase))
        {
        }

        public BsonDocument(ConcurrentDictionary<string, BsonValue> dict)
            : this()
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));

            foreach(var element in dict)
            {
                this.Add(element);
            }
        }

        public BsonDocument(IDictionary<string, BsonValue> dict)
            : this()
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));

            foreach (var element in dict)
            {
                this.Add(element);
            }
        }

        public BsonDocument(IDictionary dict)
            : this()
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));

            foreach (var key in dict.Keys)
            {
                this.Add(key.ToString(), BsonValue.FromObject(dict[key]));
            }
        }

        internal new Dictionary<string, BsonValue> RawValue => base.RawValue as Dictionary<string, BsonValue>;

        /// <summary>
        /// Get/Set position of this document inside database. It's filled when used in Find operation.
        /// </summary>
        internal PageAddress RawId { get; set; } = PageAddress.Empty;

        /// <summary>
        /// Get/Set a field for document. Fields are case sensitive
        /// </summary>
        public override BsonValue this[string key]
        {
            get
            {
                return this.RawValue.GetOrDefault(key, BsonValue.Null);
            }
            set
            {
                this.RawValue[key] = value ?? BsonValue.Null;
            }
        }

        public bool? GetBool(string key)
        {
            BsonValue value;
            if(this.RawValue.TryGetValue(key, out value))
            {
                if(value.IsBoolean) return value;
            }
            return null;
        }

        public bool GetBoolOrDefault(string key, bool def)
        {
            BsonValue value;
            if(this.RawValue.TryGetValue(key, out value))
            {
                if(value.IsBoolean) return value;
            }
            return def;
        }

        public string GetString(string key)
        {
            BsonValue value;
            if(this.RawValue.TryGetValue(key, out value))
            {
                if(value.IsString) return value;
            }
            return null;
        }

        public string GetStringOrDefault(string key, string def)
        {
            BsonValue value;
            if(this.RawValue.TryGetValue(key, out value))
            {
                if(value.IsString) return value;
            }
            return def;
        }

        public int? GetInt32(string key)
        {
            BsonValue value;
            if(this.RawValue.TryGetValue(key, out value))
            {
                if(value.IsNumber) return value.AsInt32;
            }
            return null;
        }

        public int GetInt32OrDefault(string key, int def)
        {
            BsonValue value;
            if(this.RawValue.TryGetValue(key, out value))
            {
                if(value.IsNumber) return value.AsInt32;
            }
            return def;
        }

        public long? GetInt64(string key)
        {
            BsonValue value;
            if(this.RawValue.TryGetValue(key, out value))
            {
                if(value.IsNumber) return value.AsInt64;
            }
            return null;
        }

        public long GetInt64OrDefault(string key, long def)
        {
            BsonValue value;
            if(this.RawValue.TryGetValue(key, out value))
            {
                if(value.IsNumber) return value.AsInt64;
            }
            return def;
        }

        public float? GetSingle(string key)
        {
            BsonValue value;
            if(this.RawValue.TryGetValue(key, out value))
            {
                if(value.IsNumber) return value.AsSingle;
            }
            return null;
        }

        public float GetSingleOrDefault(string key, float def)
        {
            BsonValue value;
            if(this.RawValue.TryGetValue(key, out value))
            {
                if(value.IsNumber) return value.AsSingle;
            }
            return def;
        }

        public double? GetDouble(string key)
        {
            BsonValue value;
            if(this.RawValue.TryGetValue(key, out value))
            {
                if(value.IsNumber) return value.AsDouble;
            }
            return null;
        }
        public double GetDoubleOrDefault(string key, double def)
        {
            BsonValue value;
            if(this.RawValue.TryGetValue(key, out value))
            {
                if(value.IsNumber) return value.AsDouble;
            }
            return def;
        }

        #region CompareTo

        public override int CompareTo(BsonValue other)
        {
            // if types are different, returns sort type order
            if (other.Type != BsonType.Document) return this.Type.CompareTo(other.Type);

            var thisKeys = this.Keys.ToArray();
            var thisLength = thisKeys.Length;

            var otherDoc = other.AsDocument;
            var otherKeys = otherDoc.Keys.ToArray();
            var otherLength = otherKeys.Length;

            var result = 0;
            var i = 0;
            var stop = System.Math.Min(thisLength, otherLength);

            for (; 0 == result && i < stop; i++)
                result = this[thisKeys[i]].CompareTo(otherDoc[thisKeys[i]]);

            // are different
            if (result != 0) return result;

            // test keys length to check which is bigger
            if (i == thisLength) return i == otherLength ? 0 : -1;

            return 1;
        }

        #endregion

        #region IDictionary

        public ICollection<string> Keys => this.RawValue.Keys;

        public ICollection<BsonValue> Values => this.RawValue.Values;

        public int Count => this.RawValue.Count;

        public bool IsReadOnly => false;

        public bool ContainsKey(string key) => this.RawValue.ContainsKey(key);

        /// <summary>
        /// Get all document elements - Return "_id" as first of all (if exists)
        /// </summary>
        public IEnumerable<KeyValuePair<string, BsonValue>> GetElements()
        {
            if(this.RawValue.TryGetValue("_id", out var id))
            {
                yield return new KeyValuePair<string, BsonValue>("_id", id);
            }

            foreach(var item in this.RawValue.Where(x => x.Key != "_id"))
            {
                yield return item;
            }
        }

        public void Add(string key, BsonValue value) => this.RawValue.Add(key, value ?? BsonValue.Null);

        public bool Remove(string key) => this.RawValue.Remove(key);

        public void Clear() => this.RawValue.Clear();

        public bool TryGetValue(string key, out BsonValue value) => this.RawValue.TryGetValue(key, out value);

        public void Add(KeyValuePair<string, BsonValue> item) => this.Add(item.Key, item.Value);
        public void Add(KeyValuePair<string, object> item) => this.Add(item.Key, BsonValue.FromObject(item.Value));

        public bool Contains(KeyValuePair<string, BsonValue> item) => this.RawValue.Contains(item);

        public bool Remove(KeyValuePair<string, BsonValue> item) => this.Remove(item.Key);

        public IEnumerator<KeyValuePair<string, BsonValue>> GetEnumerator() => this.RawValue.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.RawValue.GetEnumerator();

        public void CopyTo(KeyValuePair<string, BsonValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, BsonValue>>)this.RawValue).CopyTo(array, arrayIndex);
        }

        public void CopyTo(BsonDocument other)
        {
            foreach(var element in this)
            {
                other[element.Key] = element.Value;
            }
        }

        #endregion

        private int _length = 0;

        public override int GetBytesCount(bool recalc)
        {
            if (recalc == false && _length > 0) return _length;

            var length = 5;

            foreach(var element in this.RawValue)
            {
                length += this.GetBytesCountElement(element.Key, element.Value);
            }

            return _length = length;
        }
    }
}

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// All supported BsonTypes supported in AutoId insert operation
    /// </summary>
    public enum BsonAutoId
    {
        Int32 = 2,
        Int64 = 3,
        ObjectId = 10,
        Guid = 11
    }
}

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
    public class BsonArray : BsonValue, IList<BsonValue>
    {
        public BsonArray()
            : base(BsonType.Array, new List<BsonValue>())
        {
        }

        public BsonArray(List<BsonValue> array)
            : this()
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            this.AddRange(array);
        }

        public BsonArray(BsonValue[] array)
            : this()
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            this.AddRange(array);
        }

        public BsonArray(IEnumerable<BsonValue> items)
            : this()
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            this.AddRange(items);
        }

         public BsonArray(IEnumerable items)
            : this()
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            this.AddRange(items);
        }

        internal new IList<BsonValue> RawValue => (List<BsonValue>)base.RawValue;

        public override BsonValue this[int index]
        {
            get
            {
                return this.RawValue[index];
            }
            set
            {
                this.RawValue[index] = value ?? BsonValue.Null;
            }
        }

        public int Count => this.RawValue.Count;

        public bool IsReadOnly => false;

        public void Add(BsonValue item) => this.RawValue.Add(item ?? BsonValue.Null);

        public void AddRange<TCollection>(TCollection collection)
            where TCollection : ICollection<BsonValue>
        {
            if(collection == null)
                throw new ArgumentNullException(nameof(collection));

            var list = (List<BsonValue>)base.RawValue;

            var listEmptySpace = list.Capacity - list.Count;
            if (listEmptySpace < collection.Count)
            {
                list.Capacity += collection.Count;
            }

            foreach (var bsonValue in collection)
            {
                list.Add(bsonValue ?? Null);    
            }
            
        }
        
        public void AddRange(IEnumerable<BsonValue> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
            {
                this.Add(item ?? BsonValue.Null);
            }
        }

        public void AddRange(IEnumerable items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
            {
                this.Add(BsonValue.FromObject(item));
            }
        }

        public void Clear() => this.RawValue.Clear();

        public bool Contains(BsonValue item) => this.RawValue.Contains(item ?? BsonValue.Null);

        public void CopyTo(BsonValue[] array, int arrayIndex) => this.RawValue.CopyTo(array, arrayIndex);

        public IEnumerator<BsonValue> GetEnumerator() => this.RawValue.GetEnumerator();

        public int IndexOf(BsonValue item) => this.RawValue.IndexOf(item ?? BsonValue.Null);

        public void Insert(int index, BsonValue item) => this.RawValue.Insert(index, item ?? BsonValue.Null);

        public bool Remove(BsonValue item) => this.RawValue.Remove(item);

        public void RemoveAt(int index) => this.RawValue.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var value in this.RawValue)
            {
                yield return value;
            }
        }

        public override int CompareTo(BsonValue other)
        {
            // if types are different, returns sort type order
            if (other.Type != BsonType.Array) return this.Type.CompareTo(other.Type);

            var otherArray = other.AsArray;

            var result = 0;
            var i = 0;
            var stop = System.Math.Min(this.Count, otherArray.Count);

            // compare each element
            for (; 0 == result && i < stop; i++)
                result = this[i].CompareTo(otherArray[i]);

            if (result != 0) return result;
            if (i == this.Count) return i == otherArray.Count ? 0 : -1;
            return 1;
        }

        private int _length;

        public override int GetBytesCount(bool recalc)
        {
            if (recalc == false && _length > 0) return _length;

            var length = 5;
            var array = this.RawValue;
            
            for (var i = 0; i < array.Count; i++)
            {
                length += this.GetBytesCountElement(i.ToString(), array[i]);
            }

            return _length = length;
        }
    }
}

#endregion

#region DOCUMENT BSON

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Internal class to deserialize a byte[] into a BsonDocument using BSON data format
    /// </summary>
    public static class BsonReader
    {
        /// <summary>
        /// Main method - deserialize using ByteReader helper
        /// </summary>
        public static BsonDocument Deserialize(byte[] bson, bool utcDate = true, int offset = 0)
        {
            ByteReader reader = new ByteReader(bson);
            reader.Skip(offset);
            return ReadDocument(reader, utcDate);
        }

        public static BsonDocument Deserialize(ArraySegment<byte> bson, bool utcDate = true)
        {
            return ReadDocument(new ByteReader(bson), utcDate);
        }

        /// <summary>
        /// Read a BsonDocument from reader
        /// </summary>
        public static BsonDocument ReadDocument(ByteReader reader, bool utcDate = true)
        {
            var length = reader.ReadInt32();
            var end = reader.Position + length - 5;
            var obj = new BsonDocument();

            while (reader.Position < end)
            {
                var value = ReadElement(reader, out string name, utcDate);
                obj.RawValue[name] = value;
            }

            reader.ReadByte(); // zero

            return obj;
        }

        /// <summary>
        /// Read an BsonArray from reader
        /// </summary>
        public static BsonArray ReadArray(ByteReader reader, bool utcDate = true)
        {
            var length = reader.ReadInt32();
            var end = reader.Position + length - 5;
            var arr = new BsonArray();

            while (reader.Position < end)
            {
                var value = ReadElement(reader, out string name, utcDate);
                arr.Add(value);
            }

            reader.ReadByte(); // zero

            return arr;
        }

        /// <summary>
        /// Reads an element (key-value) from an reader
        /// </summary>
        private static BsonValue ReadElement(ByteReader reader, out string name, bool utcDate)
        {
            var type = reader.ReadByte();
            name = reader.ReadCString();

            if (type == 0x01) // Double
            {
                return reader.ReadDouble();
            }
            else if (type == 0x02) // String
            {
                return reader.ReadBsonString();
            }
            else if (type == 0x03) // Document
            {
                return ReadDocument(reader, utcDate);
            }
            else if (type == 0x04) // Array
            {
                return ReadArray(reader, utcDate);
            }
            else if (type == 0x05) // Binary
            {
                var length = reader.ReadInt32();
                var subType = reader.ReadByte();
                var bytes = reader.ReadBytes(length);

                switch (subType)
                {
                    case 0x00: return bytes;
                    case 0x04: return new Guid(bytes);
                }
            }
            else if (type == 0x07) // ObjectId
            {
                return new ObjectId(reader.ReadBytes(12));
            }
            else if (type == 0x08) // Boolean
            {
                return reader.ReadBoolean();
            }
            else if (type == 0x09) // DateTime
            {
                var ts = reader.ReadInt64();

                // catch specific values for MaxValue / MinValue #19
                if (ts == 253402300800000) return DateTime.MaxValue;
                if (ts == -62135596800000) return DateTime.MinValue;

                var date = BsonValue.UnixEpoch.AddMilliseconds(ts);

                return utcDate ? date : date.ToLocalTime();
            }
            else if (type == 0x0A) // Null
            {
                return BsonValue.Null;
            }
            else if (type == 0x10) // Int32
            {
                return reader.ReadInt32();
            }
            else if (type == 0x12) // Int64
            {
                return reader.ReadInt64();
            }
            else if (type == 0x13) // Decimal
            {
                return reader.ReadDecimal();
            }
            else if (type == 0xFF) // MinKey
            {
                return BsonValue.MinValue;
            }
            else if (type == 0x7F) // MaxKey
            {
                return BsonValue.MaxValue;
            }

            throw new NotSupportedException("BSON type not supported");
        }
    }
}

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Class to call method for convert BsonDocument to/from byte[] - based on http://bsonspec.org/spec.html
    /// </summary>
    public static class BsonSerializer
    {
        public static byte[] Serialize(BsonDocument doc)
        {
            if (doc == null) throw new ArgumentNullException(nameof(doc));

            return BsonWriter.Serialize(doc);
        }

        public static int SerializeTo(BsonDocument doc, byte[] array, int offset = 0)
        {
            if (doc == null) throw new ArgumentNullException(nameof(doc));

            return BsonWriter.SerializeTo(doc, array, offset);
        }

        public static BsonDocument Deserialize(byte[] buffer, bool utcDate = true, int offset = 0)
        {
            if (buffer == null || buffer.Length == 0) throw new ArgumentNullException(nameof(buffer));

            return BsonReader.Deserialize(buffer, utcDate, offset);
        }

        public static BsonDocument Deserialize(ArraySegment<byte> buffer, bool utcDate = true)
        {
            if (buffer == null || buffer.Count == 0) throw new ArgumentNullException(nameof(buffer));

            return BsonReader.Deserialize(buffer, utcDate);
        }
    }
}

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Internal class to serialize a BsonDocument to BSON data format (byte[])
    /// </summary>
    public static class BsonWriter
    {
        /// <summary>
        /// Main method - serialize document. Uses ByteWriter
        /// </summary>
        public static byte[] Serialize(BsonDocument doc)
        {
            var count = doc.GetBytesCount(true);
            var writer = new ByteWriter(count);

            WriteDocument(writer, doc);

            return writer.Buffer;
        }

        public static int SerializeTo(BsonDocument doc, byte[] array, int offset = 0)
        {
            var writer = new ByteWriter(array);
            writer.Skip(offset);
            
            WriteDocument(writer, doc);

            return writer.Position;
        }

        /// <summary>
        /// Write a bson document
        /// </summary>
        public static void WriteDocument(ByteWriter writer, BsonDocument doc)
        {
            writer.Write(doc.GetBytesCount(false));

            foreach (var key in doc.Keys)
            {
                WriteElement(writer, key, doc[key] ?? BsonValue.Null);
            }

            writer.Write((byte)0x00);
        }

        public static void WriteArray(ByteWriter writer, BsonArray array)
        {
            writer.Write(array.GetBytesCount(false));

            for (var i = 0; i < array.Count; i++)
            {
                WriteElement(writer, i.ToString(), array[i] ?? BsonValue.Null);
            }

            writer.Write((byte)0x00);
        }

        private static void WriteElement(ByteWriter writer, string key, BsonValue value)
        {
            // cast RawValue to avoid one if on As<Type>
            switch (value.Type)
            {
                case BsonType.Double:
                    writer.Write((byte)0x01);
                    WriteCString(writer, key);
                    writer.Write((Double)value.RawValue);
                    break;

                case BsonType.String:
                    writer.Write((byte)0x02);
                    WriteCString(writer, key);
                    WriteString(writer, (String)value.RawValue);
                    break;

                case BsonType.Document:
                    writer.Write((byte)0x03);
                    WriteCString(writer, key);
                    WriteDocument(writer, (BsonDocument)value);
                    break;

                case BsonType.Array:
                    writer.Write((byte)0x04);
                    WriteCString(writer, key);
                    WriteArray(writer, new BsonArray((List<BsonValue>)value.RawValue));
                    break;

                case BsonType.Binary:
                    writer.Write((byte)0x05);
                    WriteCString(writer, key);
                    var bytes = (byte[])value.RawValue;
                    writer.Write(bytes.Length);
                    writer.Write((byte)0x00); // subtype 00 - Generic binary subtype
                    writer.Write(bytes);
                    break;

                case BsonType.Guid:
                    writer.Write((byte)0x05);
                    WriteCString(writer, key);
                    var guid = ((Guid)value.RawValue).ToByteArray();
                    writer.Write(guid.Length);
                    writer.Write((byte)0x04); // UUID
                    writer.Write(guid);
                    break;

                case BsonType.ObjectId:
                    writer.Write((byte)0x07);
                    WriteCString(writer, key);
                    writer.Write(((ObjectId)value.RawValue).ToByteArray());
                    break;

                case BsonType.Boolean:
                    writer.Write((byte)0x08);
                    WriteCString(writer, key);
                    writer.Write((byte)(((Boolean)value.RawValue) ? 0x01 : 0x00));
                    break;

                case BsonType.DateTime:
                    writer.Write((byte)0x09);
                    WriteCString(writer, key);
                    var date = (DateTime)value.RawValue;
                    // do not convert to UTC min/max date values - #19
                    var utc = (date == DateTime.MinValue || date == DateTime.MaxValue) ? date : date.ToUniversalTime();
                    var ts = utc - BsonValue.UnixEpoch;
                    writer.Write(Convert.ToInt64(ts.TotalMilliseconds));
                    break;

                case BsonType.Null:
                    writer.Write((byte)0x0A);
                    WriteCString(writer, key);
                    break;

                case BsonType.Int32:
                    writer.Write((byte)0x10);
                    WriteCString(writer, key);
                    writer.Write((Int32)value.RawValue);
                    break;

                case BsonType.Int64:
                    writer.Write((byte)0x12);
                    WriteCString(writer, key);
                    writer.Write((Int64)value.RawValue);
                    break;

                case BsonType.Decimal:
                    writer.Write((byte)0x13);
                    WriteCString(writer, key);
                    writer.Write((Decimal)value.RawValue);
                    break;

                case BsonType.MinValue:
                    writer.Write((byte)0xFF);
                    WriteCString(writer, key);
                    break;

                case BsonType.MaxValue:
                    writer.Write((byte)0x7F);
                    WriteCString(writer, key);
                    break;
            }
        }

        private static void WriteString(ByteWriter writer, string s)
        {
            var bytes = Encoding.UTF8.GetBytes(s);
            writer.Write(bytes.Length + 1);
            writer.Write(bytes);
            writer.Write((byte)0x00);
        }

        private static void WriteCString(ByteWriter writer, string s)
        {
            var bytes = Encoding.UTF8.GetBytes(s);
            writer.Write(bytes);
            writer.Write((byte)0x00);
        }
    }
}

#endregion

#region DOCUMENT JSON

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// A class that read a json string using a tokenizer (without regex)
    /// </summary>
    public class JsonReader
    {
        private readonly static IFormatProvider _numberFormat = CultureInfo.InvariantCulture.NumberFormat;

        private Tokenizer _tokenizer = null;

        public long Position { get { return _tokenizer.Position; } }

        public JsonReader(TextReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            _tokenizer = new Tokenizer(reader);
        }

        internal JsonReader(Tokenizer tokenizer)
        {
            _tokenizer = tokenizer ?? throw new ArgumentNullException(nameof(tokenizer));
        }

        public BsonValue Deserialize()
        {
            var token = _tokenizer.ReadToken();

            if (token.Type == TokenType.EOF) return BsonValue.Null;

            var value = this.ReadValue(token);

            return value;
        }

        public IEnumerable<BsonValue> DeserializeArray()
        {
            var token = _tokenizer.ReadToken();

            if (token.Type == TokenType.EOF) yield break;

            token.Expect(TokenType.OpenBracket);

            token = _tokenizer.ReadToken();

            while (token.Type != TokenType.CloseBracket)
            {
                yield return this.ReadValue(token);

                token = _tokenizer.ReadToken();

                if (token.Type == TokenType.Comma)
                {
                    token = _tokenizer.ReadToken();
                }
            }

            token.Expect(TokenType.CloseBracket);

            yield break;
        }

        internal BsonValue ReadValue(Token token)
        {
            switch (token.Type)
            {
                case TokenType.String: return token.Value;
                case TokenType.OpenBrace: return this.ReadObject();
                case TokenType.OpenBracket: return this.ReadArray();
                case TokenType.Minus:
                    // read next token (must be a number)
                    var number = _tokenizer.ReadToken(false).Expect(TokenType.Int, TokenType.Double);
                    return number.Type == TokenType.Double ?
                        new BsonValue(-Convert.ToDouble(number.Value, _numberFormat)) :
                        new BsonValue(-Convert.ToInt32(number.Value, _numberFormat));
                case TokenType.Int: return new BsonValue(Convert.ToInt32(token.Value, _numberFormat));
                case TokenType.Double: return new BsonValue(Convert.ToDouble(token.Value, _numberFormat));
                case TokenType.Word:
                    switch (token.Value.ToLower())
                    {
                        case "null": return BsonValue.Null;
                        case "true": return true;
                        case "false": return false;
                        default: throw UltraLiteException.UnexpectedToken(token);
                    }
            }

            throw UltraLiteException.UnexpectedToken(token);
        }

        private BsonValue ReadObject()
        {
            var obj = new BsonDocument();

            var token = _tokenizer.ReadToken(); // read "<key>"

            while (token.Type != TokenType.CloseBrace)
            {
                token.Expect(TokenType.String, TokenType.Word);

                var key = token.Value;

                token = _tokenizer.ReadToken(); // read ":"

                token.Expect(TokenType.Colon);

                token = _tokenizer.ReadToken(); // read "<value>"

                // check if not a special data type - only if is first attribute
                if (key[0] == '$' && obj.Count == 0)
                {
                    var val = this.ReadExtendedDataType(key, token.Value);

                    // if val is null then it's not a extended data type - it's just a object with $ attribute
                    if (!val.IsNull) return val;
                }

                obj[key] = this.ReadValue(token); // read "," or "}"

                token = _tokenizer.ReadToken();

                if (token.Type == TokenType.Comma)
                {
                    token = _tokenizer.ReadToken(); // read "<key>"
                }
            }

            return obj;
        }

        private BsonArray ReadArray()
        {
            var arr = new BsonArray();

            var token = _tokenizer.ReadToken();

            while (token.Type != TokenType.CloseBracket)
            {
                var value = this.ReadValue(token);

                arr.Add(value);

                token = _tokenizer.ReadToken();

                if (token.Type == TokenType.Comma)
                {
                    token = _tokenizer.ReadToken();
                }
            }

            return arr;
        }

        private BsonValue ReadExtendedDataType(string key, string value)
        {
            BsonValue val;

            switch (key)
            {
                case "$binary": val = new BsonValue(Convert.FromBase64String(value)); break;
                case "$oid": val = new BsonValue(new ObjectId(value)); break;
                case "$guid": val = new BsonValue(new Guid(value)); break;
                case "$date": val = new BsonValue(DateTime.Parse(value).ToLocalTime()); break;
                case "$numberLong": val = new BsonValue(Convert.ToInt64(value, _numberFormat)); break;
                case "$numberDecimal": val = new BsonValue(Convert.ToDecimal(value, _numberFormat)); break;
                case "$minValue": val = BsonValue.MinValue; break;
                case "$maxValue": val = BsonValue.MaxValue; break;

                default: return BsonValue.Null; // is not a special data type
            }

            _tokenizer.ReadToken().Expect(TokenType.CloseBrace);

            return val;
        }
    }
}

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
    /// <summary>
    /// Static class for serialize/deserialize BsonDocuments into json extended format
    /// </summary>
    public static class JsonSerializer
    {
        #region Serialize

        /// <summary>
        /// Json serialize a BsonValue into a String
        /// </summary>
        public static string Serialize(BsonValue value)
        {
            var sb = new StringBuilder();

            Serialize(value, sb);

            return sb.ToString();
        }

        /// <summary>
        /// Json serialize a BsonValue into a TextWriter
        /// </summary>
        public static void Serialize(BsonValue value, TextWriter writer)
        {
            var json = new JsonWriter(writer);

            json.Serialize(value ?? BsonValue.Null);
        }

        /// <summary>
        /// Json serialize a BsonValue into a StringBuilder
        /// </summary>
        public static void Serialize(BsonValue value, StringBuilder sb)
        {
            using (var writer = new StringWriter(sb))
            {
                var w = new JsonWriter(writer);

                w.Serialize(value ?? BsonValue.Null);
            }
        }

        #endregion

        #region Deserialize

        /// <summary>
        /// Deserialize a Json string into a BsonValue
        /// </summary>
        public static BsonValue Deserialize(string json)
        {
            if (json == null) throw new ArgumentNullException(nameof(json));

            using (var sr = new StringReader(json))
            {
                var reader = new JsonReader(sr);

                return reader.Deserialize();
            }
        }

        /// <summary>
        /// Deserialize a Json TextReader into a BsonValue
        /// </summary>
        public static BsonValue Deserialize(TextReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            var jr = new JsonReader(reader);

            return jr.Deserialize();
        }

        /// <summary>
        /// Deserialize a json array as an IEnumerable of BsonValue
        /// </summary>
        public static IEnumerable<BsonValue> DeserializeArray(string json)
        {
            if (json == null) throw new ArgumentNullException(nameof(json));

            var sr = new StringReader(json);
            var reader = new JsonReader(sr);
            return reader.DeserializeArray();
        }

        /// <summary>
        /// Deserialize a json array as an IEnumerable of BsonValue reading on demand TextReader
        /// </summary>
        public static IEnumerable<BsonValue> DeserializeArray(TextReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            var jr = new JsonReader(reader);

            return jr.DeserializeArray();
        }

        #endregion
    }
}

namespace RitaEngine.Resources.Storage.UltraLiteDB
{
    public class JsonWriter
    {
        private readonly static IFormatProvider _numberFormat = CultureInfo.InvariantCulture.NumberFormat;

        private TextWriter _writer;
        private int _indent;
        private string _spacer = "";

        /// <summary>
        /// Get/Set indent size
        /// </summary>
        public int Indent { get; set; } = 4;

        /// <summary>
        /// Get/Set if writer must print pretty (with new line/indent)
        /// </summary>
        public bool Pretty { get; set; } = false;

        public JsonWriter(TextWriter writer)
        {
            _writer = writer;
        }

        /// <summary>
        /// Serialize value into text writer
        /// </summary>
        public void Serialize(BsonValue value)
        {
            _indent = 0;
            _spacer = this.Pretty ? " " : "";

            this.WriteValue(value ?? BsonValue.Null);
        }

        private void WriteValue(BsonValue value)
        {
            // use direct cast to better performance
            switch (value.Type)
            {
                case BsonType.Null:
                    _writer.Write("null");
                    break;

                case BsonType.Array:
                    this.WriteArray(value.AsArray);
                    break;

                case BsonType.Document:
                    this.WriteObject(value.AsDocument);
                    break;

                case BsonType.Boolean:
                    _writer.Write(value.AsBoolean.ToString().ToLower());
                    break;

                case BsonType.String:
                    this.WriteString(value.AsString);
                    break;

                case BsonType.Int32:
                    _writer.Write(value.AsInt32.ToString(_numberFormat));
                    break;

                case BsonType.Double:
                    _writer.Write(value.AsDouble.ToString("0.0########", _numberFormat));
                    break;

                case BsonType.Binary:
                    var bytes = value.AsBinary;
                    this.WriteExtendDataType("$binary", Convert.ToBase64String(bytes, 0, bytes.Length));
                    break;

                case BsonType.ObjectId:
                    this.WriteExtendDataType("$oid", value.AsObjectId.ToString());
                    break;

                case BsonType.Guid:
                    this.WriteExtendDataType("$guid", value.AsGuid.ToString());
                    break;

                case BsonType.DateTime:
                    this.WriteExtendDataType("$date", value.AsDateTime.ToUniversalTime().ToString("o"));
                    break;

                case BsonType.Int64:
                    this.WriteExtendDataType("$numberLong", value.AsInt64.ToString(_numberFormat));
                    break;

                case BsonType.Decimal:
                    this.WriteExtendDataType("$numberDecimal", value.AsDecimal.ToString(_numberFormat));
                    break;

                case BsonType.MinValue:
                    this.WriteExtendDataType("$minValue", "1");
                    break;

                case BsonType.MaxValue:
                    this.WriteExtendDataType("$maxValue", "1");
                    break;
            }
        }

        private void WriteObject(BsonDocument obj)
        {
            var length = obj.Keys.Count();
            var hasData = length > 0;

            this.WriteStartBlock("{", hasData);

            var index = 0;

            foreach (var el in obj.GetElements())
            {
                this.WriteKeyValue(el.Key, el.Value, index++ < length - 1);
            }

            this.WriteEndBlock("}", hasData);
        }

        private void WriteArray(BsonArray arr)
        {
            var hasData = arr.Count > 0;

            this.WriteStartBlock("[", hasData);

            for (var i = 0; i < arr.Count; i++)
            {
                var item = arr[i];

                // do not do this tests if is not pretty format - to better performance
                if (this.Pretty && item != null)
                {
                    if (!((item.IsDocument && item.AsDocument.Keys.Any()) || (item.IsArray && item.AsArray.Count > 0)))
                    {
                        this.WriteIndent();
                    }
                }

                this.WriteValue(item ?? BsonValue.Null);

                if (i < arr.Count - 1)
                {
                    _writer.Write(',');
                }
                this.WriteNewLine();
            }

            this.WriteEndBlock("]", hasData);
        }

        private void WriteString(string s)
        {
            _writer.Write('\"');
            int l = s.Length;
            for (var index = 0; index < l; index++)
            {
                var c = s[index];
                switch (c)
                {
                    case '\"':
                        _writer.Write("\\\"");
                        break;

                    case '\\':
                        _writer.Write("\\\\");
                        break;

                    case '\b':
                        _writer.Write("\\b");
                        break;

                    case '\f':
                        _writer.Write("\\f");
                        break;

                    case '\n':
                        _writer.Write("\\n");
                        break;

                    case '\r':
                        _writer.Write("\\r");
                        break;

                    case '\t':
                        _writer.Write("\\t");
                        break;

                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            _writer.Write("\\u");
                            _writer.Write(i.ToString("x04"));
                        }
                        else
                        {
                            _writer.Write(c);
                        }
                        break;
                }
            }
            _writer.Write('\"');
        }

        private void WriteExtendDataType(string type, string value)
        {
            // format: { "$type": "string-value" }
            // no string.Format to better performance
            _writer.Write("{\"");
            _writer.Write(type);
            _writer.Write("\":");
            _writer.Write(_spacer);
            _writer.Write("\"");
            _writer.Write(value);
            _writer.Write("\"}");
        }

        private void WriteKeyValue(string key, BsonValue value, bool comma)
        {
            this.WriteIndent();

            _writer.Write('\"');
            _writer.Write(key);
            _writer.Write("\":");

            // do not do this tests if is not pretty format - to better performance
            if (this.Pretty)
            {
                _writer.Write(' ');

                if (value != null && ((value.IsDocument && value.AsDocument.Keys.Any()) || (value.IsArray && value.AsArray.Count > 0)))
                {
                    this.WriteNewLine();
                }
            }

            this.WriteValue(value ?? BsonValue.Null);

            if (comma)
            {
                _writer.Write(',');
            }

            this.WriteNewLine();
        }

        private void WriteStartBlock(string str, bool hasData)
        {
            if (hasData)
            {
                this.WriteIndent();
                _writer.Write(str);
                this.WriteNewLine();
                _indent++;
            }
            else
            {
                _writer.Write(str);
            }
        }

        private void WriteEndBlock(string str, bool hasData)
        {
            if (hasData)
            {
                _indent--;
                this.WriteIndent();
                _writer.Write(str);
            }
            else
            {
                _writer.Write(str);
            }
        }

        private void WriteNewLine()
        {
            if (this.Pretty)
            {
                _writer.WriteLine();
            }
        }

        private void WriteIndent()
        {
            if (this.Pretty)
            {
                _writer.Write("".PadRight(_indent * this.Indent, ' '));
            }
        }
    }
}

#endregion

#pragma warning restore