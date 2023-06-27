

namespace RitaEngine.Math
{

    namespace Linear
    {
        public struct Quaternion{}
    }
}
// namespace Rita.Engine.Math;

// using mat4 = Rita.Engine.Math.Matrix;
// using static Rita.Engine.Math.Helper;
// /// <summary>
// /// Src glm 
// /// 
// /// </summary>
// [SkipLocalsInit,StructLayout(LayoutKind.Sequential)]
// public class Quaternion
// {
//         #region Fields       
//         /// <summary> x-component </summary>
//         public float X;
//         /// <summary> y-component </summary>
//         public float Y;
//         /// <summary> z-component </summary>
//         public float Z;
//         /// <summary> w-component </summary>
//         public float W;
//         #endregion

//         #region Constructors
        
//         /// <summary>
//         /// Component-wise constructor
//         /// </summary>
//         public Quaternion(float x, float y, float z, float w)
//         {
//             this.X = x;
//             this.Y = y;
//             this.Z = z;
//             this.W = w;
//         }
        
//         /// <summary>
//         /// all-same-value constructor
//         /// </summary>
//         public Quaternion(float v)
//         {
//             this.X = v;
//             this.Y = v;
//             this.Z = v;
//             this.W = v;
//         }
        
//         /// <summary>
//         /// copy constructor
//         /// </summary>
//         public Quaternion(Quaternion q)
//         {
//             this.X = q.X;
//             this.Y = q.Y;
//             this.Z = q.Z;
//             this.W = q.W;
//         }
        
//         /// <summary>
//         /// vector-and-scalar constructor (CAUTION: not angle-axis, use FromAngleAxis instead)
//         /// </summary>
//         public Quaternion(Vector3 v, float s)
//         {
//             this.X = v.X;
//             this.Y = v.Y;
//             this.Z = v.Z;
//             this.W = s;
//         }
        
//         /// <summary>
//         /// Create a Quaternionernion from two normalized axis (http://lolengine.net/blog/2013/09/18/beautiful-maths-Quaternionernion-from-vectors)
//         /// </summary>
//         public Quaternion(Vector3 u, Vector3 v)
//         {
//             var localW = Vector3.Cross(ref u,ref v);
//             var dot = Vector3.Dot(ref u,ref v);
//             var q = new Quaternion(localW.X, localW.Y, localW.Z, 1f + dot).Normalized;
//             this.X = q.X;
//             this.Y = q.Y;
//             this.Z = q.Z;
//             this.W = q.W;
//         }
        
//         /// <summary>
//         /// Create a Quaternionernion from two normalized axis (http://lolengine.net/blog/2013/09/18/beautiful-maths-Quaternionernion-from-vectors)
//         /// </summary>
//         public Quaternion(Vector3 eulerAngle)
//         {
//             var c = Vector3.Cos(eulerAngle / 2);
//             var s = Vector3.Sin(eulerAngle / 2);
//             this.X = s.X * c.Y * c.Z - c.X * s.Y * s.Z;
//             this.Y = c.X * s.Y * c.Z + s.X * c.Y * s.Z;
//             this.Z = c.X * c.Y * s.Z - s.X * s.Y * c.Z;
//             this.W = c.X * c.Y * c.Z + s.X * s.Y * s.Z;
//         }
        
//         /// <summary>
//         /// Creates a Quaternionernion from the rotational part of a mat4.
//         /// </summary>
//         public Quaternion(mat4 m)
//             : this(FromMat4(m))
//         {
//         }

//         #endregion

//         #region Explicit Operators
        
//         /// <summary>
//         /// Explicitly converts this to a Vector4.
//         /// </summary>
//         public static explicit operator Vector4(Quaternion v) => new Vector4((float)v.X, (float)v.Y, (float)v.Z, (float)v.W);
        
//         /// <summary>
//         /// Creates a Quaternionernion from the rotational part of a mat4.
//         /// </summary>
//         public static explicit operator Quaternion(mat4 m) => FromMat4(m);

//         #endregion


//         #region Indexer
        
//         /// <summary>
//         /// Gets/Sets a specific indexed component (a bit slower than direct access).
//         /// </summary>
//         public float this[int index]
//         {
//             get
//             {
//                 switch (index)
//                 {
//                     case 0: return X;
//                     case 1: return y;
//                     case 2: return z;
//                     case 3: return w;
//                     default: throw new ArgumentOutOfRangeException("index");
//                 }
//             }
//             set
//             {
//                 switch (index)
//                 {
//                     case 0: x = value; break;
//                     case 1: y = value; break;
//                     case 2: z = value; break;
//                     case 3: w = value; break;
//                     default: throw new ArgumentOutOfRangeException("index");
//                 }
//             }
//         }

//         #endregion

//         #region Properties
        
//         /// <summary>
//         /// Returns an array with all values
//         /// </summary>
//         public float[] Values => new[] { x, y, z, w };
        
//         /// <summary>
//         /// Returns the number of components (4).
//         /// </summary>
//         public int Count => 4;
        
//         /// <summary>
//         /// Returns the euclidean length of this Quaternionernion.
//         /// </summary>
//         public float Length => (float)Sqrt(((x*x + y*y) + (z*z + w*w)));
        
//         /// <summary>
//         /// Returns the squared euclidean length of this Quaternionernion.
//         /// </summary>
//         public float LengthSqr => ((x*x + y*y) + (z*z + w*w));
        
//         /// <summary>
//         /// Returns a copy of this Quaternionernion with length one (undefined if this has zero length).
//         /// </summary>
//         public Quaternion Normalized => this / (float)Length;
        
//         /// <summary>
//         /// Returns a copy of this Quaternionernion with length one (returns zero if length is zero).
//         /// </summary>
//         public Quaternion NormalizedSafe => this == Zero ? Identity : this / (float)Length;
        
//         /// <summary>
//         /// Returns the represented angle of this Quaternionernion.
//         /// </summary>
//         public double Angle => Acos((double)w) * 2.0;
        
//         /// <summary>
//         /// Returns the represented axis of this Quaternionernion.
//         /// </summary>
//         public Vector3 Axis
//         {
//             get
//             {
//                 var s1 = 1 - w * w;
//                 if (s1 < 0) return Vector3.UnitZ;
//                 var s2 = 1 / Sqrt(s1);
//                 return new Vector3((float)(x * s2), (float)(y * s2), (float)(z * s2));
//             }
//         }
        
//         /// <summary>
//         /// Returns the represented yaw angle of this Quaternionernion.
//         /// </summary>
//         public double Yaw => Asin(-2.0 * (double)(x * z - w * y));
        
//         /// <summary>
//         /// Returns the represented pitch angle of this Quaternionernion.
//         /// </summary>
//         public double Pitch => Atan2(2.0 * (double)(y * z + w * x), (double)(w * w - x * x - y * y + z * z));
        
//         /// <summary>
//         /// Returns the represented roll angle of this Quaternionernion.
//         /// </summary>
//         public double Roll => Atan2(2.0 * (double)(x * y + w * z), (double)(w * w + x * x - y * y - z * z));
        
//         /// <summary>
//         /// Returns the represented euler angles (pitch, yaw, roll) of this Quaternionernion.
//         /// </summary>
//         public Vector3 EulerAngles => new Vector3(Pitch, Yaw, Roll);
        
//         /// <summary>
//         /// Creates a mat3 that realizes the rotation of this Quaternionernion
//         /// </summary>
//         public mat3 ToMat3 => new mat3(1 - 2 * (y*y + z*z), 2 * (x*y + w*z), 2 * (x*z - w*y), 2 * (x*y - w*z), 1 - 2 * (x*x + z*z), 2 * (y*z + w*x), 2 * (x*z + w*y), 2 * (y*z - w*x), 1 - 2 * (x*x + y*y));
        
//         /// <summary>
//         /// Creates a mat4 that realizes the rotation of this Quaternionernion
//         /// </summary>
//         public mat4 ToMat4 => new mat4(ToMat3);
        
//         /// <summary>
//         /// Returns the conjugated Quaternionernion
//         /// </summary>
//         public Quaternion Conjugate => new Quaternion(-x, -y, -z, w);
        
//         /// <summary>
//         /// Returns the inverse Quaternionernion
//         /// </summary>
//         public Quaternion Inverse => Conjugate / LengthSqr;

//         #endregion


//         #region Static Properties
        
//         /// <summary>
//         /// Predefined all-zero Quaternionernion
//         /// </summary>
//         public static Quaternion Zero { get; } = new Quaternion(0f, 0f, 0f, 0f);
        
//         /// <summary>
//         /// Predefined all-ones Quaternionernion
//         /// </summary>
//         public static Quaternion Ones { get; } = new Quaternion(1f, 1f, 1f, 1f);
        
//         /// <summary>
//         /// Predefined identity Quaternionernion
//         /// </summary>
//         public static Quaternion Identity { get; } = new Quaternion(0f, 0f, 0f, 1f);
        
//         /// <summary>
//         /// Predefined unit-X Quaternionernion
//         /// </summary>
//         public static Quaternion UnitX { get; } = new Quaternion(1f, 0f, 0f, 0f);
        
//         /// <summary>
//         /// Predefined unit-Y Quaternionernion
//         /// </summary>
//         public static Quaternion UnitY { get; } = new Quaternion(0f, 1f, 0f, 0f);
        
//         /// <summary>
//         /// Predefined unit-Z Quaternionernion
//         /// </summary>
//         public static Quaternion UnitZ { get; } = new Quaternion(0f, 0f, 1f, 0f);
        
//         /// <summary>
//         /// Predefined unit-W Quaternionernion
//         /// </summary>
//         public static Quaternion UnitW { get; } = new Quaternion(0f, 0f, 0f, 1f);
        
//         /// <summary>
//         /// Predefined all-MaxValue Quaternionernion
//         /// </summary>
//         public static Quaternion MaxValue { get; } = new Quaternion(float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue);
        
//         /// <summary>
//         /// Predefined all-MinValue Quaternionernion
//         /// </summary>
//         public static Quaternion MinValue { get; } = new Quaternion(float.MinValue, float.MinValue, float.MinValue, float.MinValue);
        
//         /// <summary>
//         /// Predefined all-Epsilon Quaternionernion
//         /// </summary>
//         public static Quaternion Epsilon { get; } = new Quaternion(float.Epsilon, float.Epsilon, float.Epsilon, float.Epsilon);
        
//         /// <summary>
//         /// Predefined all-NaN Quaternionernion
//         /// </summary>
//         public static Quaternion NaN { get; } = new Quaternion(float.NaN, float.NaN, float.NaN, float.NaN);
        
//         /// <summary>
//         /// Predefined all-NegativeInfinity Quaternionernion
//         /// </summary>
//         public static Quaternion NegativeInfinity { get; } = new Quaternion(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
        
//         /// <summary>
//         /// Predefined all-PositiveInfinity Quaternionernion
//         /// </summary>
//         public static Quaternion PositiveInfinity { get; } = new Quaternion(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

//         #endregion


//         #region Operators
        
//         /// <summary>
//         /// Returns true iff this equals rhs component-wise.
//         /// </summary>
//         public static bool operator==(Quaternion lhs, Quaternion rhs) => lhs.Equals(rhs);
        
//         /// <summary>
//         /// Returns true iff this does not equal rhs (component-wise).
//         /// </summary>
//         public static bool operator!=(Quaternion lhs, Quaternion rhs) => !lhs.Equals(rhs);
        
//         /// <summary>
//         /// Returns proper multiplication of two Quaternionernions.
//         /// </summary>
//         public static Quaternion operator*(Quaternion p, Quaternion q) => new Quaternion(p.W * q.X + p.X * q.W + p.Y * q.Z - p.Z * q.Y, p.W * q.Y + p.Y * q.W + p.Z * q.X - p.X * q.Z, p.W * q.Z + p.Z * q.W + p.X * q.Y - p.Y * q.X, p.W * q.W - p.X * q.X - p.Y * q.Y - p.Z * q.Z);
        
//         /// <summary>
//         /// Returns a vector rotated by the Quaternionernion.
//         /// </summary>
//         public static Vector3 operator*(Quaternion q, Vector3 v)
//         {
//             var qv = new Vector3(q.X, q.Y, q.Z);
//             var uv = Vector3.Cross(qv, v);
//             var uuv = Vector3.Cross(qv, uv);
//             return v + ((uv * q.W) + uuv) * 2;
//         }
        
//         /// <summary>
//         /// Returns a vector rotated by the Quaternionernion (preserves v.W).
//         /// </summary>
//         public static Vector4 operator*(Quaternion q, Vector4 v) => new Vector4(q * new Vector3(v), v.W);
        
//         /// <summary>
//         /// Returns a vector rotated by the inverted Quaternionernion.
//         /// </summary>
//         public static Vector3 operator*(Vector3 v, Quaternion q) => q.Inverse * v;
        
//         /// <summary>
//         /// Returns a vector rotated by the inverted Quaternionernion (preserves v.W).
//         /// </summary>
//         public static Vector4 operator*(Vector4 v, Quaternion q) => q.Inverse * v;

//         #endregion


//         #region Functions
        
//         /// <summary>
//         /// Returns an enumerator that iterates through all components.
//         /// </summary>
//         public IEnumerator<float> GetEnumerator()
//         {
//             yield return x;
//             yield return y;
//             yield return z;
//             yield return w;
//         }
        
//         /// <summary>
//         /// Returns an enumerator that iterates through all components.
//         /// </summary>
//         IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
//         /// <summary>
//         /// Returns a string representation of this Quaternionernion using ', ' as a seperator.
//         /// </summary>
//         public override string ToString() => ToString(", ");
        
//         /// <summary>
//         /// Returns a string representation of this Quaternionernion using a provided seperator.
//         /// </summary>
//         public string ToString(string sep) => ((x + sep + y) + sep + (z + sep + w));
        
//         /// <summary>
//         /// Returns a string representation of this Quaternionernion using a provided seperator and a format provider for each component.
//         /// </summary>
//         public string ToString(string sep, IFormatProvider provider) => ((x.ToString(provider) + sep + y.ToString(provider)) + sep + (z.ToString(provider) + sep + w.ToString(provider)));
        
//         /// <summary>
//         /// Returns a string representation of this Quaternionernion using a provided seperator and a format for each component.
//         /// </summary>
//         public string ToString(string sep, string format) => ((x.ToString(format) + sep + y.ToString(format)) + sep + (z.ToString(format) + sep + w.ToString(format)));
        
//         /// <summary>
//         /// Returns a string representation of this Quaternionernion using a provided seperator and a format and format provider for each component.
//         /// </summary>
//         public string ToString(string sep, string format, IFormatProvider provider) => ((x.ToString(format, provider) + sep + y.ToString(format, provider)) + sep + (z.ToString(format, provider) + sep + w.ToString(format, provider)));
        
//         /// <summary>
//         /// Returns true iff this equals rhs component-wise.
//         /// </summary>
//         public bool Equals(Quaternion rhs) => ((x.Equals(rhs.X) && y.Equals(rhs.Y)) && (z.Equals(rhs.Z) && w.Equals(rhs.W)));
        
//         /// <summary>
//         /// Returns true iff this equals rhs type- and component-wise.
//         /// </summary>
//         public override bool Equals(object obj)
//         {
//             if (ReferenceEquals(null, obj)) return false;
//             return obj is Quaternion && Equals((Quaternion) obj);
//         }
        
//         /// <summary>
//         /// Returns a hash code for this instance.
//         /// </summary>
//         public override int GetHashCode()
//         {
//             unchecked
//             {
//                 return ((((((x.GetHashCode()) * 397) ^ y.GetHashCode()) * 397) ^ z.GetHashCode()) * 397) ^ w.GetHashCode();
//             }
//         }
        
//         /// <summary>
//         /// Rotates this Quaternionernion from an axis and an angle (in radians).
//         /// </summary>
//         public Quaternion Rotated(float angle, Vector3 v) => this * FromAxisAngle(angle, v);

//         #endregion


//         #region Static Functions
        
//         /// <summary>
//         /// Converts the string representation of the Quaternionernion into a Quaternionernion representation (using ', ' as a separator).
//         /// </summary>
//         public static Quaternion Parse(string s) => Parse(s, ", ");
        
//         /// <summary>
//         /// Converts the string representation of the Quaternionernion into a Quaternionernion representation (using a designated separator).
//         /// </summary>
//         public static Quaternion Parse(string s, string sep)
//         {
//             var kvp = s.Split(new[] { sep }, StringSplitOptions.None);
//             if (kvp.Length != 4) throw new FormatException("input has not exactly 4 parts");
//             return new Quaternion(float.Parse(kvp[0].Trim()), float.Parse(kvp[1].Trim()), float.Parse(kvp[2].Trim()), float.Parse(kvp[3].Trim()));
//         }
        
//         /// <summary>
//         /// Converts the string representation of the Quaternionernion into a Quaternionernion representation (using a designated separator and a type provider).
//         /// </summary>
//         public static Quaternion Parse(string s, string sep, IFormatProvider provider)
//         {
//             var kvp = s.Split(new[] { sep }, StringSplitOptions.None);
//             if (kvp.Length != 4) throw new FormatException("input has not exactly 4 parts");
//             return new Quaternion(float.Parse(kvp[0].Trim(), provider), float.Parse(kvp[1].Trim(), provider), float.Parse(kvp[2].Trim(), provider), float.Parse(kvp[3].Trim(), provider));
//         }
        
//         /// <summary>
//         /// Converts the string representation of the Quaternionernion into a Quaternionernion representation (using a designated separator and a number style).
//         /// </summary>
//         public static Quaternion Parse(string s, string sep, NumberStyles style)
//         {
//             var kvp = s.Split(new[] { sep }, StringSplitOptions.None);
//             if (kvp.Length != 4) throw new FormatException("input has not exactly 4 parts");
//             return new Quaternion(float.Parse(kvp[0].Trim(), style), float.Parse(kvp[1].Trim(), style), float.Parse(kvp[2].Trim(), style), float.Parse(kvp[3].Trim(), style));
//         }
        
//         /// <summary>
//         /// Converts the string representation of the Quaternionernion into a Quaternionernion representation (using a designated separator and a number style and a format provider).
//         /// </summary>
//         public static Quaternion Parse(string s, string sep, NumberStyles style, IFormatProvider provider)
//         {
//             var kvp = s.Split(new[] { sep }, StringSplitOptions.None);
//             if (kvp.Length != 4) throw new FormatException("input has not exactly 4 parts");
//             return new Quaternion(float.Parse(kvp[0].Trim(), style, provider), float.Parse(kvp[1].Trim(), style, provider), float.Parse(kvp[2].Trim(), style, provider), float.Parse(kvp[3].Trim(), style, provider));
//         }
        
//         /// <summary>
//         /// Tries to convert the string representation of the Quaternionernion into a Quaternionernion representation (using ', ' as a separator), returns false if string was invalid.
//         /// </summary>
//         public static bool TryParse(string s, out Quaternion result) => TryParse(s, ", ", out result);
        
//         /// <summary>
//         /// Tries to convert the string representation of the Quaternionernion into a Quaternionernion representation (using a designated separator), returns false if string was invalid.
//         /// </summary>
//         public static bool TryParse(string s, string sep, out Quaternion result)
//         {
//             result = Zero;
//             if (string.IsNullOrEmpty(s)) return false;
//             var kvp = s.Split(new[] { sep }, StringSplitOptions.None);
//             if (kvp.Length != 4) return false;
//             float x = 0f, y = 0f, z = 0f, w = 0f;
//             var ok = ((float.TryParse(kvp[0].Trim(), out x) && float.TryParse(kvp[1].Trim(), out y)) && (float.TryParse(kvp[2].Trim(), out z) && float.TryParse(kvp[3].Trim(), out w)));
//             result = ok ? new Quaternion(x, y, z, w) : Zero;
//             return ok;
//         }
        
//         /// <summary>
//         /// Tries to convert the string representation of the Quaternionernion into a Quaternionernion representation (using a designated separator and a number style and a format provider), returns false if string was invalid.
//         /// </summary>
//         public static bool TryParse(string s, string sep, NumberStyles style, IFormatProvider provider, out Quaternion result)
//         {
//             result = Zero;
//             if (string.IsNullOrEmpty(s)) return false;
//             var kvp = s.Split(new[] { sep }, StringSplitOptions.None);
//             if (kvp.Length != 4) return false;
//             float x = 0f, y = 0f, z = 0f, w = 0f;
//             var ok = ((float.TryParse(kvp[0].Trim(), style, provider, out x) && float.TryParse(kvp[1].Trim(), style, provider, out y)) && (float.TryParse(kvp[2].Trim(), style, provider, out z) && float.TryParse(kvp[3].Trim(), style, provider, out w)));
//             result = ok ? new Quaternion(x, y, z, w) : Zero;
//             return ok;
//         }
        
//         /// <summary>
//         /// Returns the inner product (dot product, scalar product) of the two Quaternionernions.
//         /// </summary>
//         public static float Dot(Quaternion lhs, Quaternion rhs) => ((lhs.X * rhs.X + lhs.Y * rhs.Y) + (lhs.Z * rhs.Z + lhs.W * rhs.W));
        
//         /// <summary>
//         /// Creates a Quaternionernion from an axis and an angle (in radians).
//         /// </summary>
//         public static Quaternion FromAxisAngle(float angle, Vector3 v)
//         {
//             var s = Math.Sin((double)angle * 0.5);
//             var c = Math.Cos((double)angle * 0.5);
//             return new Quaternion((float)((double)v.X * s), (float)((double)v.Y * s), (float)((double)v.Z * s), (float)c);
//         }
        
//         /// <summary>
//         /// Creates a Quaternionernion from the rotational part of a mat4.
//         /// </summary>
//         public static Quaternion FromMat3(mat3 m)
//         {
//             var fourXSquaredMinus1 = m.m00 - m.m11 - m.m22;
//             var fourYSquaredMinus1 = m.m11 - m.m00 - m.m22;
//             var fourZSquaredMinus1 = m.m22 - m.m00 - m.m11;
//             var fourWSquaredMinus1 = m.m00 + m.m11 + m.m22;
//             var biggestIndex = 0;
//             var fourBiggestSquaredMinus1 = fourWSquaredMinus1;
//             if(fourXSquaredMinus1 > fourBiggestSquaredMinus1)
//             {
//                 fourBiggestSquaredMinus1 = fourXSquaredMinus1;
//                 biggestIndex = 1;
//             }
//             if(fourYSquaredMinus1 > fourBiggestSquaredMinus1)
//             {
//                 fourBiggestSquaredMinus1 = fourYSquaredMinus1;
//                 biggestIndex = 2;
//             }
//             if(fourZSquaredMinus1 > fourBiggestSquaredMinus1)
//             {
//                 fourBiggestSquaredMinus1 = fourZSquaredMinus1;
//                 biggestIndex = 3;
//             }
//             var biggestVal = Math.Sqrt((double)fourBiggestSquaredMinus1 + 1.0) * 0.5;
//             var mult = 0.25 / biggestVal;
//             switch(biggestIndex)
//             {
//                 case 0: return new Quaternion((float)((double)(m.m12 - m.m21) * mult), (float)((double)(m.m20 - m.m02) * mult), (float)((double)(m.m01 - m.m10) * mult), (float)(biggestVal));
//                 case 1: return new Quaternion((float)(biggestVal), (float)((double)(m.m01 + m.m10) * mult), (float)((double)(m.m20 + m.m02) * mult), (float)((double)(m.m12 - m.m21) * mult));
//                 case 2: return new Quaternion((float)((double)(m.m01 + m.m10) * mult), (float)(biggestVal), (float)((double)(m.m12 + m.m21) * mult), (float)((double)(m.m20 - m.m02) * mult));
//                 default: return new Quaternion((float)((double)(m.m20 + m.m02) * mult), (float)((double)(m.m12 + m.m21) * mult), (float)(biggestVal), (float)((double)(m.m01 - m.m10) * mult));
//             }
//         }
        
//         /// <summary>
//         /// Creates a Quaternionernion from the rotational part of a mat3.
//         /// </summary>
//         public static Quaternion FromMat4(mat4 m) => FromMat3(new mat3(m));
        
//         /// <summary>
//         /// Returns the cross product between two Quaternionernions.
//         /// </summary>
//         public static Quaternion Cross(Quaternion q1, Quaternion q2) => new Quaternion(q1.W * q2.X + q1.X * q2.W + q1.Y * q2.Z - q1.Z * q2.Y, q1.W * q2.Y + q1.Y * q2.W + q1.Z * q2.X - q1.X * q2.Z, q1.W * q2.Z + q1.Z * q2.W + q1.X * q2.Y - q1.Y * q2.X, q1.W * q2.W - q1.X * q2.X - q1.Y * q2.Y - q1.Z * q2.Z);
        
//         /// <summary>
//         /// Calculates a proper spherical interpolation between two Quaternionernions (only works for normalized Quaternionernions).
//         /// </summary>
//         public static Quaternion Mix(Quaternion x, Quaternion y, float a)
//         {
//             var cosTheta = (double)Dot(x, y);
//             if (cosTheta > 1 - float.Epsilon)
//                 return Lerp(x, y, a);
//             else
//             {
//                 var angle = Math.Acos((double)cosTheta);
//                 return (Quaternion)( (Math.Sin((1 - (double)a) * angle) * (dQuaternion)x + Math.Sin((double)a * angle) * (dQuaternion)y) / Math.Sin(angle) );
//             }
//         }
        
//         /// <summary>
//         /// Calculates a proper spherical interpolation between two Quaternionernions (only works for normalized Quaternionernions).
//         /// </summary>
//         public static Quaternion SLerp(Quaternion x, Quaternion y, float a)
//         {
//             var z = y;
//             var cosTheta = (double)Dot(x, y);
//             if (cosTheta < 0) { z = -y; cosTheta = -cosTheta; }
//             if (cosTheta > 1 - float.Epsilon)
//                 return Lerp(x, z, a);
//             else
//             {
//                 var angle = Math.Acos((double)cosTheta);
//                 return (Quaternion)( (Math.Sin((1 - (double)a) * angle) * (dQuaternion)x + Math.Sin((double)a * angle) * (dQuaternion)z) / Math.Sin(angle) );
//             }
//         }
        
//         /// <summary>
//         /// Applies squad interpolation of these Quaternionernions
//         /// </summary>
//         public static Quaternion Squad(Quaternion q1, Quaternion q2, Quaternion s1, Quaternion s2, float h) => Mix(Mix(q1, q2, h), Mix(s1, s2, h), 2 * (1 - h) * h);

//         #endregion


//         #region Component-Wise Static Functions
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of IsInfinity (float.IsInfinity(v)).
//         /// </summary>
//         public static bVector4 IsInfinity(Quaternion v) => new bVector4(float.IsInfinity(v.X), float.IsInfinity(v.Y), float.IsInfinity(v.Z), float.IsInfinity(v.W));
        
//         /// <summary>
//         /// Returns a bvec from the application of IsInfinity (float.IsInfinity(v)).
//         /// </summary>
//         public static bVector4 IsInfinity(float v) => new bVector4(float.IsInfinity(v));
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of IsFinite (!float.IsNaN(v) &amp;&amp; !float.IsInfinity(v)).
//         /// </summary>
//         public static bVector4 IsFinite(Quaternion v) => new bVector4(!float.IsNaN(v.X) && !float.IsInfinity(v.X), !float.IsNaN(v.Y) && !float.IsInfinity(v.Y), !float.IsNaN(v.Z) && !float.IsInfinity(v.Z), !float.IsNaN(v.W) && !float.IsInfinity(v.W));
        
//         /// <summary>
//         /// Returns a bvec from the application of IsFinite (!float.IsNaN(v) &amp;&amp; !float.IsInfinity(v)).
//         /// </summary>
//         public static bVector4 IsFinite(float v) => new bVector4(!float.IsNaN(v) && !float.IsInfinity(v));
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of IsNaN (float.IsNaN(v)).
//         /// </summary>
//         public static bVector4 IsNaN(Quaternion v) => new bVector4(float.IsNaN(v.X), float.IsNaN(v.Y), float.IsNaN(v.Z), float.IsNaN(v.W));
        
//         /// <summary>
//         /// Returns a bvec from the application of IsNaN (float.IsNaN(v)).
//         /// </summary>
//         public static bVector4 IsNaN(float v) => new bVector4(float.IsNaN(v));
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of IsNegativeInfinity (float.IsNegativeInfinity(v)).
//         /// </summary>
//         public static bVector4 IsNegativeInfinity(Quaternion v) => new bVector4(float.IsNegativeInfinity(v.X), float.IsNegativeInfinity(v.Y), float.IsNegativeInfinity(v.Z), float.IsNegativeInfinity(v.W));
        
//         /// <summary>
//         /// Returns a bvec from the application of IsNegativeInfinity (float.IsNegativeInfinity(v)).
//         /// </summary>
//         public static bVector4 IsNegativeInfinity(float v) => new bVector4(float.IsNegativeInfinity(v));
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of IsPositiveInfinity (float.IsPositiveInfinity(v)).
//         /// </summary>
//         public static bVector4 IsPositiveInfinity(Quaternion v) => new bVector4(float.IsPositiveInfinity(v.X), float.IsPositiveInfinity(v.Y), float.IsPositiveInfinity(v.Z), float.IsPositiveInfinity(v.W));
        
//         /// <summary>
//         /// Returns a bvec from the application of IsPositiveInfinity (float.IsPositiveInfinity(v)).
//         /// </summary>
//         public static bVector4 IsPositiveInfinity(float v) => new bVector4(float.IsPositiveInfinity(v));
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of Equal (lhs == rhs).
//         /// </summary>
//         public static bVector4 Equal(Quaternion lhs, Quaternion rhs) => new bVector4(lhs.X == rhs.X, lhs.Y == rhs.Y, lhs.Z == rhs.Z, lhs.W == rhs.W);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of Equal (lhs == rhs).
//         /// </summary>
//         public static bVector4 Equal(Quaternion lhs, float rhs) => new bVector4(lhs.X == rhs, lhs.Y == rhs, lhs.Z == rhs, lhs.W == rhs);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of Equal (lhs == rhs).
//         /// </summary>
//         public static bVector4 Equal(float lhs, Quaternion rhs) => new bVector4(lhs == rhs.X, lhs == rhs.Y, lhs == rhs.Z, lhs == rhs.W);
        
//         /// <summary>
//         /// Returns a bvec from the application of Equal (lhs == rhs).
//         /// </summary>
//         public static bVector4 Equal(float lhs, float rhs) => new bVector4(lhs == rhs);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of NotEqual (lhs != rhs).
//         /// </summary>
//         public static bVector4 NotEqual(Quaternion lhs, Quaternion rhs) => new bVector4(lhs.X != rhs.X, lhs.Y != rhs.Y, lhs.Z != rhs.Z, lhs.W != rhs.W);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of NotEqual (lhs != rhs).
//         /// </summary>
//         public static bVector4 NotEqual(Quaternion lhs, float rhs) => new bVector4(lhs.X != rhs, lhs.Y != rhs, lhs.Z != rhs, lhs.W != rhs);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of NotEqual (lhs != rhs).
//         /// </summary>
//         public static bVector4 NotEqual(float lhs, Quaternion rhs) => new bVector4(lhs != rhs.X, lhs != rhs.Y, lhs != rhs.Z, lhs != rhs.W);
        
//         /// <summary>
//         /// Returns a bvec from the application of NotEqual (lhs != rhs).
//         /// </summary>
//         public static bVector4 NotEqual(float lhs, float rhs) => new bVector4(lhs != rhs);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of GreaterThan (lhs &gt; rhs).
//         /// </summary>
//         public static bVector4 GreaterThan(Quaternion lhs, Quaternion rhs) => new bVector4(lhs.X > rhs.X, lhs.Y > rhs.Y, lhs.Z > rhs.Z, lhs.W > rhs.W);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of GreaterThan (lhs &gt; rhs).
//         /// </summary>
//         public static bVector4 GreaterThan(Quaternion lhs, float rhs) => new bVector4(lhs.X > rhs, lhs.Y > rhs, lhs.Z > rhs, lhs.W > rhs);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of GreaterThan (lhs &gt; rhs).
//         /// </summary>
//         public static bVector4 GreaterThan(float lhs, Quaternion rhs) => new bVector4(lhs > rhs.X, lhs > rhs.Y, lhs > rhs.Z, lhs > rhs.W);
        
//         /// <summary>
//         /// Returns a bvec from the application of GreaterThan (lhs &gt; rhs).
//         /// </summary>
//         public static bVector4 GreaterThan(float lhs, float rhs) => new bVector4(lhs > rhs);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of GreaterThanEqual (lhs &gt;= rhs).
//         /// </summary>
//         public static bVector4 GreaterThanEqual(Quaternion lhs, Quaternion rhs) => new bVector4(lhs.X >= rhs.X, lhs.Y >= rhs.Y, lhs.Z >= rhs.Z, lhs.W >= rhs.W);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of GreaterThanEqual (lhs &gt;= rhs).
//         /// </summary>
//         public static bVector4 GreaterThanEqual(Quaternion lhs, float rhs) => new bVector4(lhs.X >= rhs, lhs.Y >= rhs, lhs.Z >= rhs, lhs.W >= rhs);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of GreaterThanEqual (lhs &gt;= rhs).
//         /// </summary>
//         public static bVector4 GreaterThanEqual(float lhs, Quaternion rhs) => new bVector4(lhs >= rhs.X, lhs >= rhs.Y, lhs >= rhs.Z, lhs >= rhs.W);
        
//         /// <summary>
//         /// Returns a bvec from the application of GreaterThanEqual (lhs &gt;= rhs).
//         /// </summary>
//         public static bVector4 GreaterThanEqual(float lhs, float rhs) => new bVector4(lhs >= rhs);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of LesserThan (lhs &lt; rhs).
//         /// </summary>
//         public static bVector4 LesserThan(Quaternion lhs, Quaternion rhs) => new bVector4(lhs.X < rhs.X, lhs.Y < rhs.Y, lhs.Z < rhs.Z, lhs.W < rhs.W);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of LesserThan (lhs &lt; rhs).
//         /// </summary>
//         public static bVector4 LesserThan(Quaternion lhs, float rhs) => new bVector4(lhs.X < rhs, lhs.Y < rhs, lhs.Z < rhs, lhs.W < rhs);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of LesserThan (lhs &lt; rhs).
//         /// </summary>
//         public static bVector4 LesserThan(float lhs, Quaternion rhs) => new bVector4(lhs < rhs.X, lhs < rhs.Y, lhs < rhs.Z, lhs < rhs.W);
        
//         /// <summary>
//         /// Returns a bvec from the application of LesserThan (lhs &lt; rhs).
//         /// </summary>
//         public static bVector4 LesserThan(float lhs, float rhs) => new bVector4(lhs < rhs);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of LesserThanEqual (lhs &lt;= rhs).
//         /// </summary>
//         public static bVector4 LesserThanEqual(Quaternion lhs, Quaternion rhs) => new bVector4(lhs.X <= rhs.X, lhs.Y <= rhs.Y, lhs.Z <= rhs.Z, lhs.W <= rhs.W);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of LesserThanEqual (lhs &lt;= rhs).
//         /// </summary>
//         public static bVector4 LesserThanEqual(Quaternion lhs, float rhs) => new bVector4(lhs.X <= rhs, lhs.Y <= rhs, lhs.Z <= rhs, lhs.W <= rhs);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of LesserThanEqual (lhs &lt;= rhs).
//         /// </summary>
//         public static bVector4 LesserThanEqual(float lhs, Quaternion rhs) => new bVector4(lhs <= rhs.X, lhs <= rhs.Y, lhs <= rhs.Z, lhs <= rhs.W);
        
//         /// <summary>
//         /// Returns a bvec from the application of LesserThanEqual (lhs &lt;= rhs).
//         /// </summary>
//         public static bVector4 LesserThanEqual(float lhs, float rhs) => new bVector4(lhs <= rhs);
        
//         /// <summary>
//         /// Returns a Quaternion from component-wise application of Lerp (min * (1-a) + max * a).
//         /// </summary>
//         public static Quaternion Lerp(Quaternion min, Quaternion max, Quaternion a) => new Quaternion(min.X * (1-a.X) + max.X * a.X, min.Y * (1-a.Y) + max.Y * a.Y, min.Z * (1-a.Z) + max.Z * a.Z, min.W * (1-a.W) + max.W * a.W);
        
//         /// <summary>
//         /// Returns a Quaternion from component-wise application of Lerp (min * (1-a) + max * a).
//         /// </summary>
//         public static Quaternion Lerp(Quaternion min, Quaternion max, float a) => new Quaternion(min.X * (1-a) + max.X * a, min.Y * (1-a) + max.Y * a, min.Z * (1-a) + max.Z * a, min.W * (1-a) + max.W * a);
        
//         /// <summary>
//         /// Returns a Quaternion from component-wise application of Lerp (min * (1-a) + max * a).
//         /// </summary>
//         public static Quaternion Lerp(Quaternion min, float max, Quaternion a) => new Quaternion(min.X * (1-a.X) + max * a.X, min.Y * (1-a.Y) + max * a.Y, min.Z * (1-a.Z) + max * a.Z, min.W * (1-a.W) + max * a.W);
        
//         /// <summary>
//         /// Returns a Quaternion from component-wise application of Lerp (min * (1-a) + max * a).
//         /// </summary>
//         public static Quaternion Lerp(Quaternion min, float max, float a) => new Quaternion(min.X * (1-a) + max * a, min.Y * (1-a) + max * a, min.Z * (1-a) + max * a, min.W * (1-a) + max * a);
        
//         /// <summary>
//         /// Returns a Quaternion from component-wise application of Lerp (min * (1-a) + max * a).
//         /// </summary>
//         public static Quaternion Lerp(float min, Quaternion max, Quaternion a) => new Quaternion(min * (1-a.X) + max.X * a.X, min * (1-a.Y) + max.Y * a.Y, min * (1-a.Z) + max.Z * a.Z, min * (1-a.W) + max.W * a.W);
        
//         /// <summary>
//         /// Returns a Quaternion from component-wise application of Lerp (min * (1-a) + max * a).
//         /// </summary>
//         public static Quaternion Lerp(float min, Quaternion max, float a) => new Quaternion(min * (1-a) + max.X * a, min * (1-a) + max.Y * a, min * (1-a) + max.Z * a, min * (1-a) + max.W * a);
        
//         /// <summary>
//         /// Returns a Quaternion from component-wise application of Lerp (min * (1-a) + max * a).
//         /// </summary>
//         public static Quaternion Lerp(float min, float max, Quaternion a) => new Quaternion(min * (1-a.X) + max * a.X, min * (1-a.Y) + max * a.Y, min * (1-a.Z) + max * a.Z, min * (1-a.W) + max * a.W);
        
//         /// <summary>
//         /// Returns a Quaternion from the application of Lerp (min * (1-a) + max * a).
//         /// </summary>
//         public static Quaternion Lerp(float min, float max, float a) => new Quaternion(min * (1-a) + max * a);

//         #endregion


//         #region Component-Wise Operator Overloads
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of operator&lt; (lhs &lt; rhs).
//         /// </summary>
//         public static bVector4 operator<(Quaternion lhs, Quaternion rhs) => new bVector4(lhs.X < rhs.X, lhs.Y < rhs.Y, lhs.Z < rhs.Z, lhs.W < rhs.W);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of operator&lt; (lhs &lt; rhs).
//         /// </summary>
//         public static bVector4 operator<(Quaternion lhs, float rhs) => new bVector4(lhs.X < rhs, lhs.Y < rhs, lhs.Z < rhs, lhs.W < rhs);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of operator&lt; (lhs &lt; rhs).
//         /// </summary>
//         public static bVector4 operator<(float lhs, Quaternion rhs) => new bVector4(lhs < rhs.X, lhs < rhs.Y, lhs < rhs.Z, lhs < rhs.W);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of operator&lt;= (lhs &lt;= rhs).
//         /// </summary>
//         public static bVector4 operator<=(Quaternion lhs, Quaternion rhs) => new bVector4(lhs.X <= rhs.X, lhs.Y <= rhs.Y, lhs.Z <= rhs.Z, lhs.W <= rhs.W);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of operator&lt;= (lhs &lt;= rhs).
//         /// </summary>
//         public static bVector4 operator<=(Quaternion lhs, float rhs) => new bVector4(lhs.X <= rhs, lhs.Y <= rhs, lhs.Z <= rhs, lhs.W <= rhs);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of operator&lt;= (lhs &lt;= rhs).
//         /// </summary>
//         public static bVector4 operator<=(float lhs, Quaternion rhs) => new bVector4(lhs <= rhs.X, lhs <= rhs.Y, lhs <= rhs.Z, lhs <= rhs.W);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of operator&gt; (lhs &gt; rhs).
//         /// </summary>
//         public static bVector4 operator>(Quaternion lhs, Quaternion rhs) => new bVector4(lhs.X > rhs.X, lhs.Y > rhs.Y, lhs.Z > rhs.Z, lhs.W > rhs.W);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of operator&gt; (lhs &gt; rhs).
//         /// </summary>
//         public static bVector4 operator>(Quaternion lhs, float rhs) => new bVector4(lhs.X > rhs, lhs.Y > rhs, lhs.Z > rhs, lhs.W > rhs);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of operator&gt; (lhs &gt; rhs).
//         /// </summary>
//         public static bVector4 operator>(float lhs, Quaternion rhs) => new bVector4(lhs > rhs.X, lhs > rhs.Y, lhs > rhs.Z, lhs > rhs.W);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of operator&gt;= (lhs &gt;= rhs).
//         /// </summary>
//         public static bVector4 operator>=(Quaternion lhs, Quaternion rhs) => new bVector4(lhs.X >= rhs.X, lhs.Y >= rhs.Y, lhs.Z >= rhs.Z, lhs.W >= rhs.W);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of operator&gt;= (lhs &gt;= rhs).
//         /// </summary>
//         public static bVector4 operator>=(Quaternion lhs, float rhs) => new bVector4(lhs.X >= rhs, lhs.Y >= rhs, lhs.Z >= rhs, lhs.W >= rhs);
        
//         /// <summary>
//         /// Returns a bVector4 from component-wise application of operator&gt;= (lhs &gt;= rhs).
//         /// </summary>
//         public static bVector4 operator>=(float lhs, Quaternion rhs) => new bVector4(lhs >= rhs.X, lhs >= rhs.Y, lhs >= rhs.Z, lhs >= rhs.W);
        
//         /// <summary>
//         /// Returns a Quaternion from component-wise application of operator+ (identity).
//         /// </summary>
//         public static Quaternion operator+(Quaternion v) => v;
        
//         /// <summary>
//         /// Returns a Quaternion from component-wise application of operator- (-v).
//         /// </summary>
//         public static Quaternion operator-(Quaternion v) => new Quaternion(-v.X, -v.Y, -v.Z, -v.W);
        
//         /// <summary>
//         /// Returns a Quaternion from component-wise application of operator+ (lhs + rhs).
//         /// </summary>
//         public static Quaternion operator+(Quaternion lhs, Quaternion rhs) => new Quaternion(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z, lhs.W + rhs.W);
        
//         /// <summary>
//         /// Returns a Quaternion from component-wise application of operator+ (lhs + rhs).
//         /// </summary>
//         public static Quaternion operator+(Quaternion lhs, float rhs) => new Quaternion(lhs.X + rhs, lhs.Y + rhs, lhs.Z + rhs, lhs.W + rhs);
        
//         /// <summary>
//         /// Returns a Quaternion from component-wise application of operator+ (lhs + rhs).
//         /// </summary>
//         public static Quaternion operator+(float lhs, Quaternion rhs) => new Quaternion(lhs + rhs.X, lhs + rhs.Y, lhs + rhs.Z, lhs + rhs.W);
        
//         /// <summary>
//         /// Returns a Quaternion from component-wise application of operator- (lhs - rhs).
//         /// </summary>
//         public static Quaternion operator-(Quaternion lhs, Quaternion rhs) => new Quaternion(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z, lhs.W - rhs.W);
        
//         /// <summary>
//         /// Returns a Quaternion from component-wise application of operator- (lhs - rhs).
//         /// </summary>
//         public static Quaternion operator-(Quaternion lhs, float rhs) => new Quaternion(lhs.X - rhs, lhs.Y - rhs, lhs.Z - rhs, lhs.W - rhs);
        
//         /// <summary>
//         /// Returns a Quaternion from component-wise application of operator- (lhs - rhs).
//         /// </summary>
//         public static Quaternion operator-(float lhs, Quaternion rhs) => new Quaternion(lhs - rhs.X, lhs - rhs.Y, lhs - rhs.Z, lhs - rhs.W);
        
//         /// <summary>
//         /// Returns a Quaternion from component-wise application of operator* (lhs * rhs).
//         /// </summary>
//         public static Quaternion operator*(Quaternion lhs, float rhs) => new Quaternion(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs, lhs.W * rhs);
        
//         /// <summary>
//         /// Returns a Quaternion from component-wise application of operator* (lhs * rhs).
//         /// </summary>
//         public static Quaternion operator*(float lhs, Quaternion rhs) => new Quaternion(lhs * rhs.X, lhs * rhs.Y, lhs * rhs.Z, lhs * rhs.W);
        
//         /// <summary>
//         /// Returns a Quaternion from component-wise application of operator/ (lhs / rhs).
//         /// </summary>
//         public static Quaternion operator/(Quaternion lhs, float rhs) => new Quaternion(lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs, lhs.W / rhs);

//         #endregion

// }
