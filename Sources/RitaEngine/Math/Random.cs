namespace RitaEngine.Math.Random;

   
/// <summary>
/// Random Number Generator based on Permutation-Congruential-Generator (PCG), which is a fancy wording to
/// describe a family of RNG which are simple, fast, statistically excellent, and hardly predictable.
/// 
/// More interestingly, PCG allows to generate multiple sequences with the same seed, which is very handy
/// in game development to have a unique seed per game session while using different streams for each
/// RNG which requires an isolated context (e.g. generating a procedural level, but we don't want the loot
/// generation to affect subsequent level generations).
/// 
/// https://www.pcg-random.org/
/// 
/// This code is derived from the minimal C implementation.
/// https://github.com/imneme/pcg-c-basic
/// take on site : //https://gist.github.com/mrhelmut
/// </summary>
[SkipLocalsInit, StructLayout(LayoutKind.Sequential, Pack = 4)]
public sealed class MiniPCG32 : IDisposable , IEquatable<MiniPCG32>
{
    // state
    private ulong _state;
    private readonly ulong _inc;

    /// <summary>
    /// Initiaze a random number generator.
    /// </summary>
    public MiniPCG32() : this((ulong)System.Environment.TickCount) { }

    /// <summary>
    /// Initiaze a random number generator. Specified in two parts, state initializer (a.k.a. seed) and a sequence selection constant (a.k.a. stream id).
    /// </summary>
    /// <param name="state">State initializer (a.k.a. seed).</param>
    /// <param name="streamID">Sequence selection constant (a.k.a. stream id). Defaults to 0.</param>
    public MiniPCG32(ulong state, ulong streamID = 0)
    {
        _state = 0ul;
        _inc = (streamID << 1) | 1ul;
        PCG32();
        _state += state;
        PCG32();
    }

    /// <summary>
    /// Generate a uniformly distributed number.
    /// </summary>
    /// <returns>A uniformly distributed 32bit unsigned integer.</returns>
    [ MethodImpl(  MethodImplOptions.AggressiveOptimization)  ]
    private uint PCG32()
    {
        ulong oldState = _state;
        // Advance public state
        _state = unchecked(_state * 6364136223846793005ul + _inc);
        // Calculate output function (XSH RR), uses old state for max ILP
        uint xorshifted = (uint)(((oldState >> 18) ^ oldState) >> 27);
        int rot = (int)(oldState >> 59);
        return (xorshifted >> rot) | (xorshifted << ((-rot) & 31));
    }

    /// <summary>
    /// Generate a uniformly distributed number, r, where 0 inferieur ou egal à r et inferieur à  <paramref name="bound"/>.
    /// </summary>
    /// <param name="bound">Exclusive upper bound of the number to generate.</param>
    /// <returns>A uniformly distributed 32bit unsigned integer strictly less than <paramref name="bound"/>.</returns>
    [ MethodImpl(  MethodImplOptions.AggressiveOptimization)  ]
    private uint PCG32(uint bound)
    {
        // To avoid bias, we need to make the range of the RNG a multiple of
        // bound, which we do by dropping output less than a threshold.
        uint threshold = ((uint)-bound) % bound;

        // Uniformity guarantees that this loop will terminate.  In practice, it
        // should usually terminate quickly; on average (assuming all bounds are
        // equally likely), 82.25% of the time, we can expect it to require just
        // one iteration.  In the worst case, someone passes a bound of 2^31 + 1
        // (i.e., 2147483649), which invalidates almost 50% of the range.  In 
        // practice, bounds are typically small and only a tiny amount of the range
        // is eliminated.
        while (true)
        {
            uint r = PCG32();
            if (r >= threshold)
                return r % bound;
        }
    }

    /// <summary>
    /// Generate a random positive number.
    /// </summary>
    /// <returns>A random positive number.</returns>
    public int Next() => Next(int.MaxValue);

    /// <summary>
    /// Generate a random number with an exclusive <paramref name="maxValue"/> upperbound.
    /// </summary>
    /// <param name="maxValue">Exclusive upper bound.</param>
    /// <returns>A random number with an exclusive <paramref name="maxValue"/> upperbound.</returns>
    public int Next(int maxValue)
    {
        if (maxValue < 0)
            maxValue = 0;

        return (int)PCG32((uint)maxValue);
    }

    /// <summary>
    /// Generate a random number ranging from <paramref name="minValue"/> to <paramref name="maxValue"/>.
    /// </summary>
    /// <param name="minValue">Lower bound.</param>
    /// <param name="maxValue">Upper bound.</param>
    /// <returns>A random number ranging from <paramref name="minValue"/> to <paramref name="maxValue"/>.</returns>
    public int Next(int minValue, int maxValue)
    {
        if (maxValue < minValue)
            maxValue = minValue;

        return (int)(minValue + PCG32((uint)((long)maxValue - minValue)));
    }

    /// <summary>
    /// Generate a random float ranging from 0.0f to 1.0f.
    /// </summary>
    /// <returns>A random float ranging from 0.0f to 1.0f.</returns>
    public float NextFloat()
    {
        // This is quite hackish because we want to avoid BitConverter, but who cares?
        int bound = int.MaxValue / 2 - 1;
        return Next(bound) * 1.0f / bound;
    }

    /// <summary>
    /// Generate a random float ranging from <paramref name="minValue"/> to <paramref name="maxValue"/>.
    /// </summary>
    /// <param name="minValue">Lower bound.</param>
    /// <param name="maxValue">Upper bound.</param>
    /// <returns>A random float ranging from <paramref name="minValue"/> to <paramref name="maxValue"/>.</returns>
    public float NextFloat(float minValue, float maxValue)
    {
        if (maxValue < minValue)
            maxValue = minValue;

        return minValue + (maxValue - minValue) * NextFloat();
    }

    // private T CheckMinMax<T>(T minValue, T maxValue ) => (maxValue < minValue) ? minValue : maxValue ;

    ///<inherit />
    public void Dispose() => GC.SuppressFinalize(this);

    /// <summary>
    /// Generate a random bool.
    /// </summary>
    /// <returns>A random bool.</returns>
    public bool NextBool()  => NextFloat() <= 0.5f;

    #region [ public OVERRIDE ]
        ///<inherit />
        public bool Equals(MiniPCG32? other) => false;
        ///<inherit />
        public static bool operator ==(MiniPCG32 left , MiniPCG32 right)=> false;
        ///<inherit />
        public static bool operator !=( MiniPCG32 left , MiniPCG32 right)=> true;
        ///<inherit />
        public override bool Equals(object? obj) => false;
        ///<inherit />
        public override string ToString() => $"-Random Generator  : MinPCG32 - {this._state} ";
        ///<inherit />
        public override int GetHashCode() => this._state.GetHashCode() ^ 32  + this._inc.GetHashCode() ^ 325 ;
    
    #endregion
}


/// <summary>
/// Class permet de generer des nombre pseudo aléatoire a besoin d'une graine pour etre aléatoir (seed )
/// Utilise => XorShift128Generator
/// </summary>
/// 
[SkipLocalsInit, StructLayout(LayoutKind.Sequential, Pack = 4)]
public sealed class XorShift128 : IDisposable , IEquatable<XorShift128>
{
    #region class fields
    /// <summary>
    /// Represents the seed for the <see cref="y"/> variable. This field is constant.
    /// </summary>
    /// <remarks>The value of this constant is 362436069.</remarks>
    private const uint SeedY = 362436069;

    /// <summary>
    /// Represents the seed for the <see cref="z"/> variable. This field is constant.
    /// </summary>
    /// <remarks>The value of this constant is 521288629.</remarks>
    private const uint SeedZ = 521288629;

    /// <summary>
    /// Represents the seed for the <see cref="w"/> variable. This field is constant.
    /// </summary>
    /// <remarks>The value of this constant is 88675123.</remarks>
    private const uint SeedW = 88675123;

    /// <summary>
    /// Represents the multiplier that computes a double-precision floating point number greater than or equal to 0.0 
    ///   and less than 1.0 when it gets applied to a nonnegative 32-bit signed integer.
    /// </summary>
    private const double IntToDoubleMultiplier = 1.0 / ((double)int.MaxValue + 1.0);

    /// <summary>
    /// Represents the multiplier that computes a double-precision floating point number greater than or equal to 0.0 
    ///   and less than 1.0  when it gets applied to a 32-bit unsigned integer.
    /// </summary>
    private const double UIntToDoubleMultiplier = 1.0 / ((double)uint.MaxValue + 1.0);
    #endregion

    #region instance fields
    /// <summary>
    /// Stores the last but three unsigned random number. 
    /// </summary>
    private uint x;

    /// <summary>
    /// Stores the last but two unsigned random number. 
    /// </summary>
    private uint y;
    
    /// <summary>
    /// Stores the last but one unsigned random number. 
    /// </summary>
    private uint z;

    /// <summary>
    /// Stores the last generated unsigned random number. 
    /// </summary>
    private uint w;

    /// <summary>
    /// Stores the used seed value.
    /// </summary>
    private uint seed;

    /// <summary>
    /// Stores an <see cref="uint"/> used to generate up to 32 random <see cref="Boolean"/> values.
    /// </summary>
    private uint bitBuffer;

    /// <summary>
    /// Stores how many random <see cref="Boolean"/> values still can be generated from <see cref="bitBuffer"/>.
    /// </summary>
    private int bitCount;
    #endregion

    #region construction
    /// <summary>
    /// Initializes a new instance of the <see cref="Random"/> class, using a time-dependent default 
    ///   seed value.
    /// </summary>
    public XorShift128()
    {
        this.seed = (uint)System.Math.Abs(System.Environment.TickCount);
        this.ResetGenerator();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Random"/> class, using the specified seed value.
    /// </summary>
    /// <param name="seed">
    /// A number used to calculate a starting value for the pseudo-random number sequence.
    /// If a negative number is specified, the absolute value of the number is used. 
    /// </param>
    public XorShift128(int seed)
    {
        this.seed = (uint)System.Math.Abs(seed);
        this.ResetGenerator();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Random"/> class, using the specified seed value.
    /// </summary>
    /// <param name="seed">
    /// An unsigned number used to calculate a starting value for the pseudo-random number sequence.
    /// </param>
    public XorShift128(uint seed)
    {
        this.seed = seed;
        this.ResetGenerator();
    }

    ///<inherit />
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
   
    #endregion

    #region instance methods
    /// <summary>
    /// Resets the <see cref="Random"/>, so that it produces the same pseudo-random number sequence again.
    /// </summary>
    private void ResetGenerator()
    {
        // "The seed set for xor128 is four 32-bit integers x,y,z,w not all 0, ..." (George Marsaglia)
        // To meet that requirement the y, z, w seeds are constant values greater 0.
        this.x = this.seed;
        this.y = XorShift128.SeedY;
        this.z = XorShift128.SeedZ;
        this.w = XorShift128.SeedW;

        // Reset helper variables used for generation of random bools.
        this.bitBuffer = 0;
        this.bitCount = 0;
    }

    /// <summary>
    /// Returns an unsigned random number.
    /// </summary>
    /// <returns>
    /// A 32-bit unsigned integer greater than or equal to <see cref="UInt32.MinValue"/> and 
    ///   less than or equal to <see cref="UInt32.MaxValue"/>.
    /// </returns>
    public uint NextUInt()
    {
        uint t = (this.x ^ (this.x << 11));
        this.x = this.y;
        this.y = this.z;
        this.z = this.w;
        return (this.w = (this.w ^ (this.w >> 19)) ^ (t ^ (t >> 8)));
    }

    /// <summary>
    /// Returns a nonnegative random number less than or equal to <see cref="Int32.MaxValue"/>.
    /// </summary>
    /// <returns>
    /// A 32-bit signed integer greater than or equal to 0, and less than or equal to <see cref="Int32.MaxValue"/>; 
    ///   that is, the range of return values includes 0 and "Int32.MaxValue"/>.
    /// </returns>
    public int NextInclusiveMaxValue()
    {
        // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
        uint t = (this.x ^ (this.x << 11));
        this.x = this.y;
        this.y = this.z;
        this.z = this.w;
        uint w = (this.w = (this.w ^ (this.w >> 19)) ^ (t ^ (t >> 8)));

        return (int)(w >> 1);
    }
    #endregion

    #region overridden Generator members
    
    /// <summary>
    /// Resets the <see cref="Random"/>, so that it produces the same pseudo-random number sequence again.
    /// </summary>
    /// <returns><see langword="true"/>.</returns>
    public  bool Reset()
    {
        this.ResetGenerator();
        return true;
    }

    /// <summary>
    /// Returns a nonnegative random number less than <see cref="Int32.MaxValue"/>.
    /// </summary>
    /// <returns>
    /// A 32-bit signed integer greater than or equal to 0, and less than <see cref="Int32.MaxValue"/>; that is, 
    ///   the range of return values includes 0 but not "Int32.MaxValue">.
    /// </returns>
    public int Next()
    {
        // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
        uint t = (this.x ^ (this.x << 11));
        this.x = this.y;
        this.y = this.z;
        this.z = this.w;
        uint w = (this.w = (this.w ^ (this.w >> 19)) ^ (t ^ (t >> 8)));

        int result = (int)(w >> 1);
        // Exclude Int32.MaxValue from the range of return values.
        if (result == Int32.MaxValue)
        {
            return this.Next();
        }
        else
        {
            return result;
        }
    }

    /// <summary>
    /// Returns a nonnegative random number less than the specified maximum.
    /// </summary>
    /// <param name="maxValue">
    /// The exclusive upper bound of the random number to be generated. 
    /// <paramref name="maxValue"/> must be greater than or equal to 0. 
    /// </param>
    /// <returns>
    /// A 32-bit signed integer greater than or equal to 0, and less than <paramref name="maxValue"/>; that is, 
    ///   the range of return values includes 0 but not <paramref name="maxValue"/>. 
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="maxValue"/> is less than 0. 
    /// </exception>
    public int Next(int maxValue)
    {
        if (maxValue < 0)
        {
            // string message = string.Format(null, ExceptionMessages.ArgumentOutOfRangeGreaterEqual,
            //     "maxValue", "0");
            // throw new ArgumentOutOfRangeException("maxValue", maxValue, message);
            return 0;
        }

        // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
        uint t = (this.x ^ (this.x << 11));
        this.x = this.y;
        this.y = this.z;
        this.z = this.w;
        uint w = (this.w = (this.w ^ (this.w >> 19)) ^ (t ^ (t >> 8)));

        // The shift operation and extra int cast before the first multiplication give better performance.
        // See comment in NextDouble().
        return (int)((double)(int)(w >> 1) * XorShift128.IntToDoubleMultiplier * (double)maxValue);
    }

    /// <summary>
    /// Returns a random number within the specified range. 
    /// </summary>
    /// <param name="minValue">
    /// The inclusive lower bound of the random number to be generated. 
    /// </param>
    /// <param name="maxValue">
    /// The exclusive upper bound of the random number to be generated. 
    /// <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>. 
    /// </param>
    /// <returns>
    /// A 32-bit signed integer greater than or equal to <paramref name="minValue"/>, and less than 
    ///   <paramref name="maxValue"/>; that is, the range of return values includes <paramref name="minValue"/> but 
    ///   not <paramref name="maxValue"/>. 
    /// If <paramref name="minValue"/> equals <paramref name="maxValue"/>, <paramref name="minValue"/> is returned.  
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="minValue"/> is greater than <paramref name="maxValue"/>.
    /// </exception>
    public  int Next(int minValue, int maxValue)
    {
        if (minValue > maxValue)
        {
            // string message = string.Format(null, ExceptionMessages.ArgumentOutOfRangeGreaterEqual,
            //    "maxValue", "minValue");
            // throw new ArgumentOutOfRangeException("maxValue", maxValue, message);
            return 0;
        }

        // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
        uint t = (this.x ^ (this.x << 11));
        this.x = this.y;
        this.y = this.z;
        this.z = this.w;
        uint w = (this.w = (this.w ^ (this.w >> 19)) ^ (t ^ (t >> 8)));

        int range = maxValue - minValue;
        if (range < 0)
        {	
            // The range is greater than Int32.MaxValue, so we have to use slower floating point arithmetic.
            // Also all 32 random bits (uint) have to be used which again is slower (See comment in NextDouble()).
            return minValue + (int)
                ((double)w * XorShift128.UIntToDoubleMultiplier * ((double)maxValue - (double)minValue));
        }
        else
        {   
            // 31 random bits (int) will suffice which allows us to shift and cast to an int before the first multiplication and gain better performance.
            // See comment in NextDouble().
            return minValue + (int)((double)(int)(w >> 1) * XorShift128.IntToDoubleMultiplier * (double)range);
        }
    }

    /// <summary>
    /// Returns a nonnegative floating point random number less than 1.0.
    /// </summary>
    /// <returns>
    /// A double-precision floating point number greater than or equal to 0.0, and less than 1.0; that is, 
    ///   the range of return values includes 0.0 but not 1.0.
    /// </returns>
    public  double NextDouble()
    {
        // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
        uint t = (this.x ^ (this.x << 11));
        this.x = this.y;
        this.y = this.z;
        this.z = this.w;
        uint w = (this.w = (this.w ^ (this.w >> 19)) ^ (t ^ (t >> 8)));

        // Here a ~2x speed improvement is gained by computing a value that can be cast to an int 
        //   before casting to a double to perform the multiplication.
        // Casting a double from an int is a lot faster than from an uint and the extra shift operation 
        //   and cast to an int are very fast (the allocated bits remain the same), so overall there's 
        //   a significant performance improvement.
        return (double)(int)(w >> 1) * XorShift128.IntToDoubleMultiplier;
    }

    /// <summary>
    /// Returns a nonnegative floating point random number less than the specified maximum.
    /// </summary>
    /// <param name="maxValue">
    /// The exclusive upper bound of the random number to be generated. 
    /// <paramref name="maxValue"/> must be greater than or equal to 0.0. 
    /// </param>
    /// <returns>
    /// A double-precision floating point number greater than or equal to 0.0, and less than <paramref name="maxValue"/>; 
    ///   that is, the range of return values includes 0 but not <paramref name="maxValue"/>. 
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="maxValue"/> is less than 0. 
    /// </exception>
    public double NextDouble(double maxValue)
    {
        if (maxValue < 0.0)
        {
            // string message = string.Format(null, ExceptionMessages.ArgumentOutOfRangeGreaterEqual,
            //     "maxValue", "0.0");
            // throw new ArgumentOutOfRangeException("maxValue", maxValue, message);
            return 0.0;
        }

        // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
        uint t = (this.x ^ (this.x << 11));
        this.x = this.y;
        this.y = this.z;
        this.z = this.w;
        uint w = (this.w = (this.w ^ (this.w >> 19)) ^ (t ^ (t >> 8)));

        // The shift operation and extra int cast before the first multiplication give better performance.
        // See comment in NextDouble().
        return (double)(int)(w >> 1) * XorShift128.IntToDoubleMultiplier * maxValue;
    }

    /// <summary>
    /// Returns a floating point random number within the specified range. 
    /// </summary>
    /// <param name="minValue">
    /// The inclusive lower bound of the random number to be generated. 
    /// The range between <paramref name="minValue"/> and <paramref name="maxValue"/> must be less than or equal to
    ///   <see cref="Double.MaxValue"/>
    /// </param>
    /// <param name="maxValue">
    /// The exclusive upper bound of the random number to be generated. 
    /// <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.
    /// The range between <paramref name="minValue"/> and <paramref name="maxValue"/> must be less than or equal to
    ///   <see cref="Double.MaxValue"/>.
    /// </param>
    /// <returns>
    /// A double-precision floating point number greater than or equal to <paramref name="minValue"/>, and less than 
    ///   <paramref name="maxValue"/>; that is, the range of return values includes <paramref name="minValue"/> but 
    ///   not <paramref name="maxValue"/>. 
    /// If <paramref name="minValue"/> equals <paramref name="maxValue"/>, <paramref name="minValue"/> is returned.  
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="minValue"/> is greater than <paramref name="maxValue"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The range between <paramref name="minValue"/> and <paramref name="maxValue"/> is greater than
    ///   <see cref="Double.MaxValue"/>.
    /// </exception>
    public  double NextDouble(double minValue, double maxValue)
    {
        if (minValue > maxValue)
        {
            // string message = string.Format(null, ExceptionMessages.ArgumentOutOfRangeGreaterEqual,
            //     "maxValue", "minValue");
            // throw new ArgumentOutOfRangeException("maxValue", maxValue, message);
            return 0.0;
        }

        double range = maxValue - minValue;

        if (range == double.PositiveInfinity)
        {
            // string message = string.Format(null, ExceptionMessages.ArgumentRangeLessEqual,
            //     "minValue", "maxValue", "Double.MaxValue");
            // throw new ArgumentException(message);
            return 0.0;
        }

        // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
        uint t = (this.x ^ (this.x << 11));
        this.x = this.y;
        this.y = this.z;
        this.z = this.w;
        uint w = (this.w = (this.w ^ (this.w >> 19)) ^ (t ^ (t >> 8)));

        // The shift operation and extra int cast before the first multiplication give better performance.
        // See comment in NextDouble().
        return minValue + (double)(int)(w >> 1) * XorShift128.IntToDoubleMultiplier * range;
    }

    /// <summary>
    /// Returns a random Boolean value.
    /// </summary>
    /// <remarks>
    /// <remarks>
    /// Buffers 32 random bits (1 uint) for future calls, so a new random number is only generated every 32 calls.
    /// </remarks>
    /// </remarks>
    /// <returns>A <see cref="Boolean"/> value.</returns>
    public bool NextBoolean()
    {
        if (this.bitCount == 0)
        {
            // Generate 32 more bits (1 uint) and store it for future calls.
            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            uint t = (this.x ^ (this.x << 11));
            this.x = this.y;
            this.y = this.z;
            this.z = this.w;
            this.bitBuffer = (this.w = (this.w ^ (this.w >> 19)) ^ (t ^ (t >> 8)));

            // Reset the bitCount and use rightmost bit of buffer to generate random bool.
            this.bitCount = 31;
            return (this.bitBuffer & 0x1) == 1;
        }

        // Decrease the bitCount and use rightmost bit of shifted buffer to generate random bool.
        this.bitCount--;
        return ((this.bitBuffer >>= 1) & 0x1) == 1;
    }

    /// <summary>
    /// Fills the elements of a specified array of bytes with random numbers. 
    /// </summary>
    /// <remarks>
    /// Each element of the array of bytes is set to a random number greater than or equal to 0, and less than or 
    ///   equal to <see cref="byte.MaxValue"/>.
    /// </remarks>
    /// <param name="buffer">An array of bytes to contain random numbers.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="buffer"/> is a null reference (<see langword="Nothing"/> in Visual Basic). 
    /// </exception>
    public void NextBytes(byte[] buffer)
    {
        if (buffer == null)
        {
            // string message = string.Format(null, ExceptionMessages.ArgumentNull, "buffer");
            // throw new ArgumentNullException("buffer", message);
        }

        // Use local copies of x,y,z and w for better performance.
        uint x = this.x;
        uint y = this.y;
        uint z = this.z;
        uint w = this.w;

        // Fill the buffer with 4 bytes (1 uint) at a time.
        int i = 0;
        uint t;
        while (i < buffer!.Length - 3)
        {
            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            t = (x ^ (x << 11));
            x = y;
            y = z;
            z = w;
            w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

            buffer[i++] = (byte)w;
            buffer[i++] = (byte)(w >> 8);
            buffer[i++] = (byte)(w >> 16);
            buffer[i++] = (byte)(w >> 24);
        }

        // Fill up any remaining bytes in the buffer.
        if (i < buffer.Length)
        {
            // Its faster to explicitly calculate the unsigned random number than simply call NextUInt().
            t = (x ^ (x << 11));
            x = y;
            y = z;
            z = w;
            w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

            buffer[i++] = (byte)w;
            if (i < buffer.Length)
            {
                buffer[i++] = (byte)(w >> 8);
                if (i < buffer.Length)
                {
                    buffer[i++] = (byte)(w >> 16);
                    if (i < buffer.Length)
                    {
                        buffer[i] = (byte)(w >> 24);
                    }
                }
            }
        }
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    
    #endregion

    #region [ public OVERRIDE ]
        ///<inherit />
        public bool Equals(XorShift128? other) => false;
        ///<inherit />
        public static bool operator ==(XorShift128 left , XorShift128 right)=> false;
        ///<inherit />
        public static bool operator !=( XorShift128 left , XorShift128 right)=> true;
        ///<inherit />
        public override bool Equals(object? obj) => false;
        ///<inherit />
        public override string ToString() => $"-Random Generator  : XorShift128 - {this.seed} ";
        ///<inherit />
        public override int GetHashCode() => this.seed.GetHashCode() ^ 32  + this.x.GetHashCode() ^ 325 ;

    #endregion
}

