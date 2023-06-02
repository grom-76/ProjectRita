namespace RitaEngine.Base.Math.Hash;

/// <summary>
/// Code adapter de https://github.com/ssg/HashDepot
///
/// https://github.com/ssg/HashDepot/tree/main/src ( update 2020)
/// </summary>
public static class xxHash64
{
    private const ulong prime64v1 = 11400714785074694791ul;
    private const ulong prime64v2 = 14029467366897019727ul;
    private const ulong prime64v3 = 1609587929392839161ul;
    private const ulong prime64v4 = 9650029242287828579ul;
    private const ulong prime64v5 = 2870177450012600261ul;

    /// <summary>
    /// Generate a 64-bit xxHash value.
    /// </summary>
    /// <param name="buffer">Input buffer.</param>
    /// <param name="seed">Optional seed.</param>
    /// <returns>Computed 64-bit hash value.</returns>
    public static unsafe ulong Hash64(ReadOnlySpan<byte> buffer, ulong seed = 0)
    {
        const int stripeLength = 32;

        bool bigEndian = !System.BitConverter.IsLittleEndian;

        int len = buffer.Length;
        int remainingLen = len;
        ulong acc;

        fixed (byte* inputPtr = buffer)
        {
            byte* pInput = inputPtr;
            if (len >= stripeLength)
            {
                var (acc1, acc2, acc3, acc4) = InitAccumulators64(seed);
                do
                {
                    acc = ProcessStripe64(ref pInput, ref acc1, ref acc2, ref acc3, ref acc4, bigEndian);
                    remainingLen -= stripeLength;
                }
                while (remainingLen >= stripeLength);
            }
            else
            {
                acc = seed + prime64v5;
            }

            acc += (ulong)len;
            acc = ProcessRemaining64(pInput, acc, remainingLen, bigEndian);
        }

        return Avalanche64(acc);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe (ulong, ulong, ulong, ulong) InitAccumulators64(ulong seed)
        => (seed + prime64v1 + prime64v2, seed + prime64v2, seed, seed - prime64v1);


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe ulong ProcessStripe64(ref byte* pInput,ref ulong acc1,ref ulong acc2,ref ulong acc3, ref ulong acc4,
        bool bigEndian)
    {
        if (bigEndian)
        {
            ProcessLaneBigEndian64(ref acc1, ref pInput);
            ProcessLaneBigEndian64(ref acc2, ref pInput);
            ProcessLaneBigEndian64(ref acc3, ref pInput);
            ProcessLaneBigEndian64(ref acc4, ref pInput);
        }
        else
        {
            ProcessLane64(ref acc1, ref pInput);
            ProcessLane64(ref acc2, ref pInput);
            ProcessLane64(ref acc3, ref pInput);
            ProcessLane64(ref acc4, ref pInput);
        }

        ulong acc = RotateLeft(acc1, 1)
                    + RotateLeft(acc2, 7)
                    + RotateLeft(acc3, 12)
                    + RotateLeft(acc4, 18);

        MergeAccumulator64(ref acc, acc1);
        MergeAccumulator64(ref acc, acc2);
        MergeAccumulator64(ref acc, acc3);
        MergeAccumulator64(ref acc, acc4);
        return acc;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void ProcessLane64(ref ulong accn, ref byte* pInput)
    {
        ulong lane = *(ulong*)pInput;
        accn = Round64(accn, lane);
        pInput += 8;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void ProcessLaneBigEndian64(ref ulong accn, ref byte* pInput)
    {
        ulong lane = *(ulong*)pInput;
        lane = Swap64(lane);
        accn = Round64(accn, lane);
        pInput += 8;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe ulong ProcessRemaining64( byte* pInput, ulong acc,int remainingLen, bool bigEndian)
    {
        for (ulong lane; remainingLen >= 8; remainingLen -= 8, pInput += 8)
        {
            lane = *(ulong*)pInput;
            if (bigEndian)
            {
                lane = Swap64(lane);
            }

            acc ^= Round64(0, lane);
            acc = RotateLeft(acc, 27) * prime64v1;
            acc += prime64v4;
        }

        for (uint lane32; remainingLen >= 4; remainingLen -= 4, pInput += 4)
        {
            lane32 = *(uint*)pInput;
            if (bigEndian)
            {
                lane32 = Swap32(lane32);
            }

            acc ^= lane32 * prime64v1;
            acc = RotateLeft(acc, 23) * prime64v2;
            acc += prime64v3;
        }

        for (byte lane8; remainingLen >= 1; remainingLen--, pInput++)
        {
            lane8 = *pInput;
            acc ^= lane8 * prime64v5;
            acc = RotateLeft(acc, 11) * prime64v1;
        }

        return acc;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong Round64(ulong accn, ulong lane)
    {
        accn += lane * prime64v2;
        return RotateLeft(accn, 31) * prime64v1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void MergeAccumulator64(ref ulong acc, ulong accn)
    {
        acc ^= Round64(0, accn);
        acc *= prime64v1;
        acc += prime64v4;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong Avalanche64(ulong acc)
    {
        acc ^= acc >> 33;
        acc *= prime64v2;
        acc ^= acc >> 29;
        acc *= prime64v3;
        acc ^= acc >> 32;
        return acc;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong RotateLeft(ulong value, int offset)
        =>  (value << offset) | (value >> (64 - offset));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint  Swap32( uint n)
        =>  ((n & 0xFF000000) >> 24) |
            ((n & 0x00FF0000) >>  8) |
            ((n & 0x0000FF00) <<  8) |
            ((n & 0x000000FF) << 24);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong  Swap64( ulong n)
        => ((n & ( ulong)0xFF00000000000000) >> 56) |
            ((n & ( ulong)0x00FF000000000000) >> 40) |
            ((n & ( ulong)0x0000FF0000000000) >> 24) |
            ((n & ( ulong)0x000000FF00000000) >>  8) |
            ((n & ( ulong)0x00000000FF000000) <<  8) |
            ((n & ( ulong)0x0000000000FF0000) << 24) |
            ((n & ( ulong)0x000000000000FF00) << 40) |
            ((n & ( ulong)0x00000000000000FF) << 56);
}

/// <summary>
/// pomper dans MCJEngine.Tools.U8XmlParser
/// </summary>
public static unsafe class XXHash32
{
    private static readonly uint _seed = (uint)DateTime.UtcNow.Ticks;

    private const uint Prime1 = 2654435761U;
    private const uint Prime2 = 2246822519U;
    private const uint Prime3 = 3266489917U;
    private const uint Prime4 = 668265263U;
    private const uint Prime5 = 374761393U;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="byteLength"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ComputeHash(byte* data, int byteLength)
    {
        if(byteLength == 0) { return 0; }

        return (byteLength < 16) ? 
                  (int)ComputeHashShort(data, byteLength,_seed + Prime5 + (uint)byteLength)
                : (int)ComputeHashFull(data, byteLength);
    }

    private static uint ComputeHashShort(byte* data, int byteLength, uint acc)
    {
        var laneCount = System.Math.DivRem(byteLength, 4, out int mod4);
        if(laneCount == 1) {
            acc = AccumRemainingLane(acc, *(uint*)data);
        }
        else if(laneCount == 2) {
            acc = AccumRemainingLane(acc, *(uint*)data);
            acc = AccumRemainingLane(acc, *(uint*)(data + 4));
        }
        else if(laneCount == 3) {
            acc = AccumRemainingLane(acc, *(uint*)data);
            acc = AccumRemainingLane(acc, *(uint*)(data + 4));
            acc = AccumRemainingLane(acc, *(uint*)(data + 8));
        }

        byte* bytes = data + 4 * laneCount;
        if(mod4 == 1) {
            acc = AccumByte(acc, bytes[0]);
        }
        else if(mod4 == 2) {
            acc = AccumByte(acc, bytes[0]);
            acc = AccumByte(acc, bytes[1]);
        }
        else if(mod4 == 3) {
            acc = AccumByte(acc, bytes[0]);
            acc = AccumByte(acc, bytes[1]);
            acc = AccumByte(acc, bytes[2]);
        }
        return MixFinal(acc);
    }

    private static uint ComputeHashFull(byte* data, int byteLength)
    {
        var blockCount = System.Math.DivRem(byteLength, 16, out int mod16);
        Initialize(out uint acc1, out uint acc2, out uint acc3, out uint acc4);
        for(int i = 0; i < blockCount; i++) {
            uint lane1 = *(uint*)(data + 16 * i);
            uint lane2 = *(uint*)(data + 16 * i + 4);
            uint lane3 = *(uint*)(data + 16 * i + 8);
            uint lane4 = *(uint*)(data + 16 * i + 12);
            acc1 = AccumBlockLane(acc1, lane1);
            acc2 = AccumBlockLane(acc2, lane2);
            acc3 = AccumBlockLane(acc3, lane3);
            acc4 = AccumBlockLane(acc4, lane4);
        }
        uint acc = MixState(acc1, acc2, acc3, acc4) + (uint)byteLength;
        return ComputeHashShort(data + byteLength - mod16, mod16, acc);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Initialize(out uint v1, out uint v2, out uint v3, out uint v4)
    {
        v1 = _seed + Prime1 + Prime2;
        v2 = _seed + Prime2;
        v3 = _seed;
        v4 = _seed - Prime1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint AccumBlockLane(uint hash, uint lane)
        => RotateLeft(hash + lane * Prime2, 13) * Prime1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint AccumRemainingLane(uint hash, uint lane)
        => RotateLeft(hash + lane * Prime3, 17) * Prime4;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint AccumByte(uint hash, byte b)
        => RotateLeft(hash + (uint)b * Prime5, 11) * Prime1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint MixState(uint v1, uint v2, uint v3, uint v4)
        => RotateLeft(v1, 1) +  RotateLeft(v2, 7) +  RotateLeft(v3, 12) + RotateLeft(v4, 18);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint MixFinal(uint hash)
    {
        hash ^= hash >> 15;
        hash *= Prime2;
        hash ^= hash >> 13;
        hash *= Prime3;
        hash ^= hash >> 16;
        return hash;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint RotateLeft(uint value, int offset)
        =>  (value << offset) | (value >> (32 - offset));
}

/// <summary>
/// Utilise la technique dite murmur 
///  voir : https://landman-code.blogspot.com/2009/02/c-superfasthash-and-murmurhash2.html
/// </summary>
public static class Murmur32
{
    /// <summary>
    /// hashage d'un tableau de bytes
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static UInt32 Hash(Byte[] data)
        =>  Hash(data, 0xc58f1a7b);

    private const UInt32 m = 0x5bd1e995;
    private const Int32 r = 24;

    /// <summary>
    /// hash un tableau de bytes avec un seed ( sel )
    /// </summary>
    /// <param name="data"></param>
    /// <param name="seed"></param>
    /// <returns></returns>
    public static unsafe UInt32 Hash(Byte[] data, UInt32 seed)
    {
        Int32 length = data.Length;
        if (length == 0)
            return 0;
        UInt32 h = seed ^ (UInt32)length;
        Int32 remainingBytes = length & 3; // mod 4
        Int32 numberOfLoops = length >> 2; // div 4
        fixed (byte* firstByte = &(data[0]))
        {
            UInt32* realData = (UInt32*)firstByte;
            while (numberOfLoops != 0)
            {
                UInt32 k = *realData;
                k *= m;
                k ^= k >> r;
                k *= m;

                h *= m;
                h ^= k;
                numberOfLoops--;
                realData++;
            }
            switch (remainingBytes)
            {
                case 3:
                    h ^= (UInt16)(*realData);
                    h ^= ((UInt32)(*(((Byte*)(realData)) + 2))) << 16;
                    h *= m;
                    break;
                case 2:
                    h ^= (UInt16)(*realData);
                    h *= m;
                    break;
                case 1:
                    h ^= *((Byte*)realData);
                    h *= m;
                    break;
                default:
                    break;
            }
        }

        // Do a few final mixes of the hash to ensure the last few
        // bytes are well-incorporated.

        h ^= h >> 13;
        h *= m;
        h ^= h >> 15;

        return h;
    }
}

public static class HashFNV
{
    /// <summary>
    /// Computes a hash code using the <a href="http://bretm.home.comcast.net/~bretm/hash/6.html">FNV modified algorithm</a>m.
    /// </summary>
    /// <param name="data">Byte data to hash.</param>
    /// <returns>Hash code for the data.</returns>
    public static int FNV(byte[] data)
    {
        if(data == null || data.Length == 0)
            return 0;

        unchecked
        {
            uint p = 16777619;
            uint hash = 2166136261;

            for(int i = 0; i < data.Length; i++)
                hash = (hash ^ data[i]) * p;

            hash += hash << 13;
            hash ^= hash >> 7;
            hash += hash << 3;
            hash ^= hash >> 17;
            hash += hash << 5;

            return (int) hash;
        }
    } 
}

