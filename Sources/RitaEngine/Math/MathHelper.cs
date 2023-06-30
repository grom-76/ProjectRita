namespace RitaEngine.Math;

     
    // public static class Exponential
    // {
    //     //squre exp log pow
    // }

    // public static class Trigonometric
    // {
    //     //cos sin acos tan ...
    // }

    
    // public static class TransformHelper //OR USE NAMESPACE ?
    // {
    //     //Helper
    //     public static class Rotation{}
    //     public static class Translation{}
    //     public static class World{}
    // }
    // public static class Constants{}

    // public static class Common{}// see Helper

//  MATH https://github.com/aldriangintingsuka/GlmSharp
// https://github.com/FlaxEngine/FlaxEngine/tree/master/Source/Engine/Core/Math
//https://github.com/Syncaidius/MoltenEngine/blob/master/Molten.Math/MathHelper.cs for GAUSS 
/// <summary> WRAPPER FOR MATH METHODS
/// 
/// All functions or constantes need for math algebra
/// https://github.com/MachineCognitis/C.math.NET/blob/master/C.math/math.cs
/// </summary>
[ SkipLocalsInit, StructLayout(LayoutKind.Sequential )]
public static class Helper
{
#region COnstant    
    /// <summary>  constante universel ration de la circonférence d'un cercle </summary>
    public const float PI = 3.14159265358979323846264338327950288f;
    /// <summary>Représente une valeur proche de zéro , la plus petite valeur possible d'un float </summary>
    public const float Epsilon = float.Epsilon;
    /// <summary>Repréente une valeur proche de zéro </summary>
    public const float ZeroTolerance = 1E-06f;
    /// <summary>2 x PI  </summary>
    public const float TWOPI = 6.28318530718f;
    /// <summary>PI / 2 </summary>
    public const float HALF_PI = 1.57079632679489661923132169163975144f;
    /// <summary>PI x PI </summary>
    public const float PI_CARRE = 9.86960440109f;
    /// <summary>Racine carré de PI  </summary>
    public const float SQRT_PI = 1.772453850905516027f;
    /// <summary>1 / PI  </summary>
    public const float INV_PI = 0.318309886183790671537767526745028724f;
    /// <summary>PI / 180 </summary>
    public const float PIOVER180 = 0.01745329252165517991444444444444f;
    /// <summary>PI / 2  </summary>
    public const float PI_OVER2 = 1.57079632679489661923132169163975144f;
    /// <summary>PI / 4 </summary>
    public const float PI_OVER4 = PI / 4;

    #region Float Types proprizetes

      /// <summary>
        /// Determines whether the specified value is close to zero (0.0f).
        /// </summary>
        /// <param name="a">The floating value.</param>
        /// <returns><c>true</c> if the specified value is close to zero (0.0f); otherwise, <c>false</c>.</returns>
        public static bool IsZero(float a) => Abs(a) < Epsilon;
        
        /// <summary>
        /// Checks if a and b are almost equals, taking into account the magnitude of floating point numbers (unlike <see cref="WithinEpsilon(float,float,float)" /> method).
        /// </summary>
        /// <param name="a">The left value to compare.</param>
        /// <param name="b">The right value to compare.</param>
        /// <returns><c>true</c> if a almost equal to b, <c>false</c> otherwise</returns>
        /// <remarks>The code is using the technique described by Bruce Dawson in <a href="http://randomascii.wordpress.com/2012/02/25/comparing-floating-point-numbers-2012-edition/">Comparing Floating point numbers 2012 edition</a>.</remarks>
        public static unsafe bool NearEqual(float a, float b)
        {
            // Check if the numbers are really close -- needed when comparing numbers near zero.
            if (Abs(a - b) < Epsilon)
                return true;

            // Original from Bruce Dawson: http://randomascii.wordpress.com/2012/02/25/comparing-floating-point-numbers-2012-edition/
            int aInt = *(int*)&a;
            int bInt = *(int*)&b;

            // Different signs means they do not match.
            if (aInt < 0 != bInt < 0)
                return false;

            // Find the difference in ULPs.
            int ulp = Abs(aInt - bInt);

            // Choose of maxUlp = 4
            // according to http://code.google.com/p/googletest/source/browse/trunk/include/gtest/internal/gtest-internal.h
            const int maxUlp = 4;
            return ulp <= maxUlp;
        }
    /// <summary>
    /// Masquage 
    /// </summary>
    public const long DBL_EXP_MASK = 0x7ff0000000000000L;
    /// <summary>
    /// Mnatisse exponentiel
    /// </summary>
    public const int DBL_MANT_BITS = 52;
    /// <summary> Bit-mask used for extracting the sign bit of a <see cref="Double"/> (<c>0x8000000000000000</c>).</summary>
    public const long DBL_SGN_MASK = -1 - 0x7fffffffffffffffL;
    /// <summary>
    /// 
    /// </summary>
    public const long DBL_MANT_MASK = 0x000fffffffffffffL;
    /// <summary>
    /// 
    /// </summary>
    public const long DBL_EXP_CLR_MASK = DBL_SGN_MASK | DBL_MANT_MASK;
    /// <summary>
    /// The exponent bias of a <see cref="Double"/>, i.e. value to subtract from the stored exponent to get the real exponent (<c>1023</c>).
    /// </summary>
    public const int DBL_EXP_BIAS = 1023;

    /// <summary>
    /// The number of bits in the exponent of a <see cref="Double"/> (<c>11</c>).
    /// </summary>
    public const int DBL_EXP_BITS = 11;

    /// <summary>
    /// The maximum (unbiased) exponent of a <see cref="Double"/> (<c>1023</c>).
    /// </summary>
    public const int DBL_EXP_MAX = 1023;

    /// <summary>
    /// The minimum (unbiased) exponent of a <see cref="Double"/> (<c>-1022</c>).
    /// </summary>
    public const int DBL_EXP_MIN = -1022;

    /// <summary>
    /// Bit-mask used for clearing the exponent bits of a <see cref="Double"/> (<c>0x800fffffffffffff</c>).
    /// </summary>
    // public const long DBL_EXP_CLR_MASK = DBL_SGN_MASK | DBL_MANT_MASK;

    /// <summary>
    /// Bit-mask used for extracting the exponent bits of a <see cref="Double"/> (<c>0x7ff0000000000000</c>).
    /// </summary>
    // public const long DBL_EXP_MASK = 0x7ff0000000000000L;

    /// <summary>
    /// The number of bits in the mantissa of a <see cref="Double"/>, excludes the implicit leading <c>1</c> bit (<c>52</c>).
    /// </summary>
    // public const int DBL_MANT_BITS = 52;

    /// <summary>
    /// Bit-mask used for clearing the mantissa bits of a <see cref="Double"/> (<c>0xfff0000000000000</c>).
    /// </summary>
    // public const long DBL_MANT_CLR_MASK = DBL_SGN_MASK | DBL_EXP_MASK;

    /// <summary>
    /// Bit-mask used for extracting the mantissa bits of a <see cref="Double"/> (<c>0x000fffffffffffff</c>).
    /// </summary>
    // public const long DBL_MANT_MASK = 0x000fffffffffffffL;

    /// <summary>
    /// Maximum positive, normal value of a <see cref="Double"/> (<c>1.7976931348623157E+308</c>).
    /// </summary>
    public const double DBL_MAX = System.Double.MaxValue;

    /// <summary>
    /// Minimum positive, normal value of a <see cref="Double"/> (<c>2.2250738585072014e-308</c>).
    /// </summary>
    public const double DBL_MIN = 2.2250738585072014e-308D;

    /// <summary>
    /// Maximum positive, subnormal value of a <see cref="Double"/> (<c>2.2250738585072009e-308</c>).
    /// </summary>
    public const double DBL_DENORM_MAX = DBL_MIN - DBL_DENORM_MIN;

    /// <summary>
    /// Minimum positive, subnormal value of a <see cref="Double"/> (<c>4.94065645841247E-324</c>).
    /// </summary>
    public const double DBL_DENORM_MIN = System.Double.Epsilon;

    /// <summary>
    /// Bit-mask used for clearing the sign bit of a <see cref="Double"/> (<c>0x7fffffffffffffff</c>).
    /// </summary>
    public const long DBL_SGN_CLR_MASK = 0x7fffffffffffffffL;

    /// <summary>
    /// Bit-mask used for extracting the sign bit of a <see cref="Double"/> (<c>0x8000000000000000</c>).
    /// </summary>
    // public const long DBL_SGN_MASK = -1 - 0x7fffffffffffffffL;

    /// <summary>
    /// The exponent bias of a <see cref="Single"/>, i.e. value to subtract from the stored exponent to get the real exponent (<c>127</c>).
    /// </summary>
    public const int FLT_EXP_BIAS = 127;

    /// <summary>
    /// The number of bits in the exponent of a <see cref="Single"/> (<c>8</c>).
    /// </summary>
    public const int FLT_EXP_BITS = 8;

    /// <summary>
    /// The maximum (unbiased) exponent of a <see cref="Single"/> (<c>127</c>).
    /// </summary>
    public const int FLT_EXP_MAX = 127;

    /// <summary>
    /// The minimum (unbiased) exponent of a <see cref="Single"/> (<c>-126</c>).
    /// </summary>
    public const int FLT_EXP_MIN = -126;

    /// <summary>
    /// Bit-mask used for clearing the exponent bits of a <see cref="Single"/> (<c>0x807fffff</c>).
    /// </summary>
    public const int FLT_EXP_CLR_MASK = FLT_SGN_MASK | FLT_MANT_MASK;

    /// <summary>
    /// Bit-mask used for extracting the exponent bits of a <see cref="Single"/> (<c>0x7f800000</c>).
    /// </summary>
    public const int FLT_EXP_MASK = 0x7f800000;

    /// <summary>
    /// The number of bits in the mantissa of a <see cref="Single"/>, excludes the implicit leading <c>1</c> bit (<c>23</c>).
    /// </summary>
    public const int FLT_MANT_BITS = 23;

    /// <summary>
    /// Bit-mask used for clearing the mantissa bits of a <see cref="Single"/> (<c>0xff800000</c>).
    /// </summary>
    public const int FLT_MANT_CLR_MASK = FLT_SGN_MASK | FLT_EXP_MASK;

    /// <summary>
    /// Bit-mask used for extracting the mantissa bits of a <see cref="Single"/> (<c>0x007fffff</c>).
    /// </summary>
    public const int FLT_MANT_MASK = 0x007fffff;

    /// <summary>
    /// Maximum positive, normal value of a <see cref="Single"/> (<c>3.40282347e+38</c>).
    /// </summary>
    public const float FLT_MAX = System.Single.MaxValue;

    /// <summary>
    /// Minimum positive, normal value of a <see cref="Single"/> (<c>1.17549435e-38</c>).
    /// </summary>
    public const float FLT_MIN = 1.17549435e-38F;

    /// <summary>
    /// Maximum positive, subnormal value of a <see cref="Single"/> (<c>1.17549421e-38</c>).
    /// </summary>
    public const float FLT_DENORM_MAX = FLT_MIN - FLT_DENORM_MIN;

    /// <summary>
    /// Minimum positive, subnormal value of a <see cref="Single"/> (<c>1.401298E-45</c>).
    /// </summary>
    public const float FLT_DENORM_MIN = System.Single.Epsilon;

    /// <summary>
    /// Bit-mask used for clearing the sign bit of a <see cref="Single"/> (<c>0x7fffffff</c>).
    /// </summary>
    public const int FLT_SGN_CLR_MASK = 0x7fffffff;

    /// <summary>
    /// Bit-mask used for extracting the sign bit of a <see cref="Single"/> (<c>0x80000000</c>).
    /// </summary>
    public const int FLT_SGN_MASK = -1 - 0x7fffffff;
    #endregion
#endregion

#region Angles
    /// <summary> Converti valeur  de degré vers radians  </summary>
    /// <param name="degree"> float de 0 a 360  </param>
    /// <returns>simple precision between 0 to PI </returns>
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static float ToRadians(float degree) => degree * 0.01745329251994329576923690768489f;
    
    /// <summary> Convertion de radians vers degrée </summary>
    /// <param name="radian">float 0 PI</param>
    /// <returns>simple precision angle between 0° to 360° </returns>
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static float ToDegree(float radian) => radian * 57.295779513082320876798154814105f;

     /// <summary> Converti valeur  de degré vers radians  </summary>
    /// <param name="degree"> float de 0 a 360  </param>
    /// <returns>simple precision between 0 to PI </returns>
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static double ToRadians(double degree) => degree * 0.01745329251994329576923690768489;
    
    /// <summary> Convertion de radians vers degrée </summary>
    /// <param name="radian">double 0 PI</param>
    /// <returns>simple precision angle between 0° to 360° </returns>
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static double ToDegree(double radian) => radian * 57.295779513082320876798154814105;

     public static float ToRad(this float degree) => degree * 0.01745329251994329576923690768489f;

     public static float ToDeg(this float radian) => radian * 57.295779513082320876798154814105f;
#endregion

#region Approx
    /// <summary>   Arrondi à la valeur la plus proche  </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [ MethodImpl(  MethodImplOptions.AggressiveOptimization)  ]
    public static float Round(float value) => MathF.Round( value);

    [ MethodImpl(  MethodImplOptions.AggressiveOptimization)  ]
    public static double Round(double value) => System.Math.Round( value);

    [ MethodImpl(  MethodImplOptions.AggressiveOptimization)  ]
    public static float Round(float value, int digits) => MathF.Round( value, digits);


    /// <summary>  Retourne la valeur absolue d'un float   </summary>
    /// <param name="scalar">float valeur comprise entre 0 et float.MaxValue </param>
    /// <returns></returns>
    [ MethodImpl(  MethodImplOptions.AggressiveOptimization)  ]
    public static float Abs( float scalar ) => MathF.Abs(scalar);
    /// <summary>  Retourne la valeur absolue d'un float   </summary>
    /// <param name="scalar">float valeur comprise entre 0 et float.MaxValue </param>
    /// <returns></returns>
    [ MethodImpl(  MethodImplOptions.AggressiveOptimization)  ]
    public static double Abs( double scalar ) => System.Math.Abs(scalar);
      /// <summary>
    /// Valur absolue d'un réel simple préciion
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static float AbsFast(float f) => (float) ( (int)f & 0x7FFFFFFF) ;

    /// <summary>
    /// Retourne a valeur absolue ( sans le signe) d'un entier
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static int Abs(int val)
    {
        int mask = val >> (sizeof(int) * CH_BIT) - 1;
        return (val + mask) ^ mask;
        // Patented variation:  r = (valeur ^ mask) - mask;   
    }

    /// <summary>
    /// Returns the largest integral value less than or equal to the specified single-precision floating-point number.
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    [ MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Floor(float f) => System.MathF.Floor(f);

    /// <summary>
    /// Returns the largest integral value less than or equal to the specified single-precision floating-point number.
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    [ MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Floor(double d) => System.Math.Floor(d);

    /// <summary>
    /// Donne l'arrondie au chiffre supérieur
    /// </summary>
    /// <param name="fp"></param>
    /// <returns></returns>
    public static int FloorFast(float fp) => (int)(fp + 32768.0f) - 32768;

    /// <summary>
    /// Donne l'arrondie au chiffre inférieur
    /// </summary>
    /// <param name="fp"></param>
    /// <returns></returns>
    public static int Ceiling(float fp) => 32768 - (int)(32768.0f - fp);

  
    /// <summary>
    /// Donne l'arrondie au chiffre inférieur
    /// </summary>
    /// <param name="fp"></param>
    /// <returns></returns>
    public static double Ceiling(double fp) =>System.Math.Ceiling(fp);

#endregion

#region Trigo
    /// <summary> Retourne la Tangeante de l'angle spécifié ( Cos / Sin )  </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    [ MethodImpl(  MethodImplOptions.AggressiveOptimization)  ]
    public static float Tan(float x) => MathF.Tan(x);
    
    /// <summary> Retourne le cosinus de l'angle spécifié  </summary>
    /// <param name="x"></param>
    /// <returns>The cosine of d. If d is equal to double.NaN, double.NegativeInfinity, or double.PositiveInfinity, this method returns double.NaN</returns>
    [ MethodImpl(  MethodImplOptions.AggressiveOptimization)  ]
    public static float Cos(float x) => MathF.Cos(x);
     /// <summary> Retourne le cosinus de l'angle spécifié  </summary>
    /// <param name="x"></param>
    /// <returns>The cosine of d. If d is equal to double.NaN, double.NegativeInfinity, or double.PositiveInfinity, this method returns double.NaN</returns>
    [ MethodImpl(  MethodImplOptions.AggressiveOptimization)  ]
    public static double Cos(double x) => System.Math.Cos(x);
    
    /// <summary> Retourne le sinuss de l'angle spécifié  </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    [ MethodImpl(  MethodImplOptions.AggressiveOptimization)  ]
    public static float Sin(float x) => MathF.Sin(x);
      /// <summary> Retourne le sinuss de l'angle spécifié  </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    [ MethodImpl(  MethodImplOptions.AggressiveOptimization)  ]
    public static double Sin(double x) => System.Math.Sin(x);
    
     /// <summary>
    /// entre pi et -pi
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static float ATan2(float x, float y) => MathF.Atan2(x,y);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static float ATan(float x) => MathF.Atan(x);

    /// <summary>
    /// Returns the angle whose cosine is the specified number.
    /// </summary>
    /// <param name="x"></param>
    /// <returns>An angle, θ, measured in radians, such that 0 ≤ θ ≤ π. -or- double.NaN if d < -1 or d > 1 or d equals double.NaN</returns>
    public static float ACos(float x) => MathF.Acos(x);
    /// <summary>
    /// Returns the angle whose cosine is the specified number.
    /// </summary>
    /// <param name="x"></param>
    /// <returns>An angle, θ, measured in radians, such that 0 ≤ θ ≤ π. -or- double.NaN if d < -1 or d > 1 or d equals double.NaN</returns>
    public static double ACos(double x) => System.Math.Acos(x);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static float ASin(float x) => MathF.Asin(x);
     /// <summary>
    /// 
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public	static float FastSin(float val)
    {
        float angleSqr = val * val;
        float result = -2.39e-08f;
        result *= angleSqr;
        result += 2.7526e-06f;
        result *= angleSqr;
        result -= 1.98409e-04f;
        result *= angleSqr;
        result += 8.3333315e-03f;
        result *= angleSqr;
        result -= 1.666666664e-01f;
        result *= angleSqr;
        result++;
        result *= val;

        return result;
    }
      /// <summary>
    /// Fast sinus en valeur approché <see href="http://forum.devmaster.net/t/fast-and-accurate-sine-cosine/9648" />
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static   float FasterSin(float x) {
        float y = (1.27323954474f * x) - (0.40528473456f * x * Abs(x));
        return (0.225f * ((y * Abs(y)) - y)) + y;   // Q * y + P * y * abs(y)
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static float FastCos(float val)
    {
        float angleSqr = val * val;
        float result = -2.605e-07f;
        result *= angleSqr;
        result += 2.47609e-05f;
        result *= angleSqr;
        result -= 1.3888397e-03f;
        result *= angleSqr;
        result += 4.16666418e-02f;
        result *= angleSqr;
        result -= 4.999999963e-01f;
        result *= angleSqr;
        result++;

        return result;
    }
    /// <summary>
    /// Fast tangante
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static float FastTan(float val)
    {
        float angleSqr = val * val;
        float result = 9.5168091e-03f;

        result *= angleSqr;
        result += 2.900525e-03f;
        result *= angleSqr;
        result += 2.45650893e-02f;
        result *= angleSqr;
        result += 5.33740603e-02f;
        result *= angleSqr;
        result += 1.333923995e-01f;
        result *= angleSqr;
        result += 3.333314036e-01f;
        result *= angleSqr;
        result++;
        result *= val;

        return result;
    }

#endregion

#region SQUARE LOG POW EXP
    /// <summary>  Retourne la racine carré du nombre spécifié  </summary>
    /// <param name="scalar"></param>
    /// <returns></returns>
    [ MethodImpl(  MethodImplOptions.AggressiveOptimization)  ]
    public static float Sqrt( float scalar )  => MathF.Sqrt( scalar);

     /// <summary>  Retourne la racine carré du nombre spécifié  </summary>
    /// <param name="scalar"></param>
    /// <returns></returns>
    [ MethodImpl(  MethodImplOptions.AggressiveOptimization)  ]
    public static double Sqrt( double scalar )  => System.Math.Sqrt( scalar);

    /// <summary> Retourne la valeur x élevé  à la puissance de y  </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    [ MethodImpl(  MethodImplOptions.AggressiveOptimization)  ]
    public static float Pow(float x , float y ) => MathF.Pow(x,y);

     /// <summary> Retourne la valeur x élevé  à la puissance de y  </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    [ MethodImpl(  MethodImplOptions.AggressiveOptimization)  ]
    public static double Pow(double x , double y ) => System.Math.Pow(x,y);
    /// <summary>
    /// eleve la valeur e a la puissance
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public static float Exp(float e) => MathF.Exp(e);
    /// <summary>
    /// eleve la valeur e a la puissance
    /// </summary>
    /// <param name="e"></param>
    /// <returns>The number e raised to the power d. If d equals double.NaN or double.PositiveInfinity, that value is returned. If d equals double.NegativeInfinity, 0 is returned</returns>
    public static double Exp(double e) => System.Math.Exp(e);
    
   
    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static float Log(float x) => MathF.Log( x);
    /// <summary>
    /// Returns the natural (base e) logarithm of a specified number.
    /// Retourne :One of the values in the following table.
    /// d parameter – Return value
    /// Positive – The natural logarithm of d; that is, ln d, or log e d
    /// Zero –double.NegativeInfinity
    /// Negative –double.NaN
    /// Equal to double.NaN –double.NaN
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static double Log(double x) => System.Math.Log( x);
    /// <summary>
    /// Utilisé dans quake , aproximatif
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static unsafe float InverseSqrtFast(float x)
    {
        var xhalf = 0.5f * x;
        var i = *(int*)&x; // Read bits as integer.
        i = 0x5f375a86 - (i >> 1); // Make an initial guess for Newton-Raphson approximation
        x = *(float*)&i; // Convert bits back to float
        x *= (1.5f - (xhalf * x * x)); // Perform left single Newton-Raphson step.
        x *= (1.5f - (xhalf * x * x)); // repeat for better precision
        x *= (1.5f - (xhalf * x * x)); // 
        return x;
        // code take in : https://github.com/opentk/opentk/blob/master/src/OpenTK.Mathematics/MathHelper.cs
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    public static unsafe float FasterPow2(float p)
    {
        float clipp = (p < -126) ? -126.0f : p;
        uint ui = *(uint*)&clipp;
        return (ui<< 23) * (clipp + 126.94269504f);
    }

    public static float FasterExp(float p) => FasterPow2(1.442695040f * p);

    
    public static unsafe float FasterLog2(float x) 
    {
        var i = *(int*)&x;
        float y = *(float*)&i;
        y *= 1.1920928955078125e-7f;
        return y - 126.94269504f;
    }

    public static  unsafe float FasterLog(float x) =>  0.69314718f * FasterLog2 (x);


    public static unsafe float Loga(float x) =>  Log(x);


    // public static float fastlog2(float x) {
    // 	union { float f; uint32_t i; } vx = { x };
    // 	union { uint32_t i; float f; } mx = { (vx.i & 0x007FFFFF) | 0x3f000000 };
    // 	float y = vx.i;
    // 	y *= 1.1920928955078125e-7f;

    // 	return y - 124.22551499f
    // 		- 1.498030302f * mx.f
    // 		- 1.72587999f / (0.3520887068f + mx.f);
    // }

    // public static float fastlog(float x) {
    // 	return 0.69314718f * fastlog2(x);
    // }

    /// <summary>
    /// ROTATE LEFT  _lrotl 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>

#endregion

#region CLAMP
    /// <summary>
    /// Borne une valeur coprise entre min et max 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float Clamp(float value, float min , float max)
        => value < min ? min : value > max ? max : value;

    public static double Clamp(double value, double min , double max)
        => value < min ? min : value > max ? max : value;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static short Clamp(short value, short min, short max)
        => value < min ? min : value > max ? max : value;
    
    
    public static int Clamp(int value, int min, int max)
        => value < min ? min : value > max ? max : value;

    public static uint Clamp(uint value, uint min, uint max) 
        => value < min ? min : value > max ? max : value;

    /// <summary>
    /// borne une valeur entre min et max en utilisant une reférence
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public static void ClampRef(ref float value, float min, float max)
        => value =	value < min ? min : value > max ? max : value;
    
   

#endregion

    /// <summary>
    /// Clamp (borne ) angle en radian entre 0 et 2PI
    /// </summary>
    /// <param name="radian">angle en radian</param>
    /// <returns>angle borné entre 0 et 2PI</returns>
    public static float ClampRadian(float radian)=> Abs( radian %= 2 * PI);

    /// <summary>
    /// Clamp (borne ) angle en degré entre 0 et 360
    /// </summary>
    /// <param name="degre">angle en degre</param>
    /// <returns>angle borné entre 0 et 360</returns>
    public static float ClampDegree(float degre) => Abs( degre %= 360.000f);
    
    /// <summary>
    /// Borne entre -180° et 180°
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static float NormalizeDegre(float angle) => angle -= MathF.Ceiling(angle / 360.0f - 0.5f) * 360.0f;

    /// <summary>
    /// borne entre -PI et PI
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static float NormalizeRadian(float angle)
    {
        angle = ClampRadian(angle);
        // shift angle to range (-π, π]
        return (angle > PI_OVER2) ?  angle - ( TWOPI ) : angle;
    }



    /// <summary>
    /// Linear interpolation
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public static float Lerp(float from, float to, float amount) => ((1 - amount) * from) + (amount * to);
    
    

    /// <summary>
    /// Performs smooth (cubic Hermite) interpolation between 0 and 1.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public static float SmoothStep(float amount)
        => (amount <= 0) ? 0
            : (amount >= 1) ? 1
            : amount * amount * (3 - (2 * amount));

    /// <summary>
    /// Performs a smooth(er) interpolation between 0 and 1 with 1st and 2nd order derivatives of zero at endpoints.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public static float SmootherStep(float amount) => (amount <= 0) ? 0
            : (amount >= 1) ? 1
            : amount * amount * amount * ((amount * ((amount * 6) - 15)) + 10);
    /// <summary>
    /// modulo 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="modulo"></param>
    /// <returns></returns>
    public static float Modulo(float value, float modulo) => (modulo==0.0f) ? value : value % modulo ;
  
    /// <summary>
    /// 
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static float NegF(float f) => (float) (  (int)f ^ 0x80000000   );
   
    /// <summary>
    /// redonne +1 pour 0 ou des nombres positifs , -1 pour les nombres n�gatifs
    /// </summary>
    /// <param name="valeur"></param>
    /// <returns></returns>
    public static int SgnF( float valeur) =>  1 + ((((int)valeur) >> 31) << 1);

    

#region LIMIT
    private const int CH_BIT = 8;
    
    /// <summary>
    /// Donne la valeur Min entre deux entiers
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static int Min(int x, int y)
        =>  y + ((x - y) & ((x - y) >> ((sizeof(int) * CH_BIT) - 1))); // min(x, y)

    /// <summary>
    /// Donne la valeur max entre deux entiers
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static int Max(int x , int y )
        =>x - ((x - y) & ((x - y) >> ((sizeof(int) * CH_BIT) - 1))); // max(x, y)



    public static float Max(float x, float y) => MathF.Max(x,y);
    public static float Min(float x, float y) => MathF.Min(x,y);

    public static double Max(double x, double y) => System.Math.Max(x,y);
    public static double Min(double x, double y) => System.Math.Min(x,y);

#endregion
    
  

    public static uint LeftRotate(uint x, int y)   =>  (x << y) | (x >> (32 - y));

    public static double Ldexp(double number, int exponent)=> number * System.Math.Pow(2, exponent);

    public static double PowOfExponent(double number, int exponent)   => number * System.Math.Pow(2, exponent);

    /// <summary>
    /// This code had been borrowed from here: https://github.com/MachineCognitis/C.math.NET
    /// </summary>
    /// <param name="number"></param>
    /// <param name="exponent"></param>
    /// <returns></returns>
    public static unsafe double Frexp(double number, int* exponent)
    {
        var bits = BitConverter.DoubleToInt64Bits(number);
        var exp = (int) ((bits & DBL_EXP_MASK) >> DBL_MANT_BITS);
        *exponent = 0;

        if (exp == 0x7ff || number == 0D)
            number += number;
        else
        {
            // Not zero and finite.
            *exponent = exp - 1022;
            if (exp == 0)
            {
                // Subnormal, scale number so that it is in [1, 2).
                number *= BitConverter.Int64BitsToDouble(0x4350000000000000L); // 2^54
                bits = BitConverter.DoubleToInt64Bits(number);
                exp = (int) ((bits & DBL_EXP_MASK) >> DBL_MANT_BITS);
                *exponent = exp - 1022 - 54;
            }

            // Set exponent to -1 so that number is in [0.5, 1).
            number = BitConverter.Int64BitsToDouble((bits & DBL_EXP_CLR_MASK) | 0x3fe0000000000000L);
        }

        return number;
    }

      public static float Frexp(float number, ref int exponent)
        {
            int bits = BitConverter.SingleToInt32Bits(number);
            int exp = (int)((bits & FLT_EXP_MASK) >> FLT_MANT_BITS);
            exponent = 0;

            if (exp == 0xff || number == 0F)
                number += number;
            else
            {
                // Not zero and finite.
                exponent = exp - 126;
                if (exp == 0)
                {
                    // Subnormal, scale number so that it is in [1, 2).
                    number *= BitConverter.Int32BitsToSingle(0x4c000000); // 2^25
                    bits = BitConverter.SingleToInt32Bits(number);
                    exp = (int)((bits & FLT_EXP_MASK) >>FLT_MANT_BITS);
                    exponent = exp - 126 - 25;
                }
                // Set exponent to -1 so that number is in [0.5, 1).
                number = BitConverter.Int32BitsToSingle((bits & FLT_EXP_CLR_MASK) | 0x3f000000);
            }

            return number;
        }

#region Exponenet

        // <summary>
        /// Gets the exponent bits of the specified floating-point <paramref name="number"/>.
        /// </summary>
        public static int Exponent(double number)
            => System.Convert.ToInt32((System.BitConverter.DoubleToInt64Bits(number) & DBL_EXP_MASK) >> DBL_MANT_BITS);
        // <summary>
        /// Gets the exponent bits of the specified floating-point <paramref name="number"/>.
        /// </summary>
        public static int Exponent(float number)
            => System.Convert.ToInt32((System.BitConverter.SingleToInt32Bits(number) & FLT_EXP_MASK) >> FLT_MANT_BITS);
        
        /// <summary>
        /// Gets the mantissa bits of the specified floating-point <paramref name="number"/> without the implicit leading <c>1</c> bit
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static long Mantisse(double number)=>System.BitConverter.DoubleToInt64Bits(number) & DBL_MANT_MASK;

          /// <summary>
        /// Gets the mantissa bits of the specified floating-point <paramref name="number"/> without the implicit leading <c>1</c> bit
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int Mantisse(float number)=>System.BitConverter.SingleToInt32Bits(number) & FLT_MANT_MASK;        
            
#endregion

        /// <summary>
        /// Converts the specified single-precision floating point number to a 32-bit signed integer.
        /// </summary>
        public unsafe static int FloatToInt(float value) => *((int*)&value);
        /// <summary>
        /// Converts the specified 32-bit signed integer to a single-precision floating point number.
        /// </summary>
        public unsafe static float IntToFloat(int value)=> *((float*)&value);
}

