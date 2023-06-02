namespace RitaEngine.Base.Memory;

using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;

public unsafe static class CRuntime
{
    internal static ConcurrentDictionary<long, Pointer> Pointers = new ConcurrentDictionary<long, Pointer>();

        internal static void* Malloc(long size)
        {
            var result = new PinnedArray<byte>(size);
            Pointers[(long)result.Ptr] = result;

            return result.Ptr;
        }

        internal static void Memcpy(void* a, void* b, long size)
        {
            var ap = (byte*)a;
            var bp = (byte*)b;
            for (long i = 0; i < size; ++i)
            {
                *ap++ = *bp++;
            }
        }

        internal static void MemMove(void* a, void* b, long size)
        {
            using (var temp = new PinnedArray<byte>(size))
            {
                Memcpy(temp.Ptr, b, size);
                Memcpy(a, temp.Ptr, size);
            }
        }

        internal static void Free(void* a)
        {
            Pointer pointer;
            if (!Pointers.TryRemove((long)a, out pointer!))
            {
                return;
            }

            pointer.Dispose();
        }

        internal static void* Realloc(void* a, long newSize)
        {
            Pointer pointer;
            if (!Pointers.TryGetValue((long)a, out pointer!))
            {
                // Allocate new
                return Malloc(newSize);
            }

            if (newSize <= pointer.Size)
            {
                // Realloc not required
                return a;
            }

            var result = Malloc(newSize);
            Memcpy(result, a, pointer.Size);

            Pointers.TryRemove((long)pointer.Ptr, out pointer!);
            pointer.Dispose();

            return result;
        }

        internal static int Memcmp(void* a, void* b, long size)
        {
            var result = 0;
            var ap = (byte*)a;
            var bp = (byte*)b;
            for (long i = 0; i < size; ++i)
            {
                if (*ap != *bp)
                {
                    result += 1;
                }
                ap++;
                bp++;
            }

            return result;
        }
    
     public const long DblExpMask = 0x7ff0000000000000L;
        public const int DblMantBits = 52;
        public const long DblSgnMask = -1 - 0x7fffffffffffffffL;
        public const long DblMantMask = 0x000fffffffffffffL;
        public const long DblExpClrMask = DblSgnMask | DblMantMask;

        // public static void* Malloc(ulong size)
        // {
        //     return Operations.Malloc((long)size);
        // }

        // public static void Memcpy(void* a, void* b, long size)
        // {
        //     Operations.Memcpy(a, b, size);
        // }

        // public static void Memcpy(void* a, void* b, ulong size)
        // {
        //     Memcpy(a, b, (long)size);
        // }

        // public static void Memmove(void* a, void* b, long size)
        // {
        //     Operations.MemMove(a, b, size);
        // }

        // public static void Memmove(void* a, void* b, ulong size)
        // {
        //     Memmove(a, b, (long)size);
        // }

        // public static int Memcmp(void* a, void* b, long size)
        // {
        //     return Operations.Memcmp(a, b, size);
        // }

        public static int Memcmp(void* a, void* b, ulong size)
        {
            return Memcmp(a, b, (long)size);
        }

        // public static void Free(void* a)
        // {
        //     Operations.Free(a);
        // }

        public static void Memset(void* ptr, int value, long size)
        {
            byte* bptr = (byte*)ptr;
            var bval = (byte)value;
            for (long i = 0; i < size; ++i)
            {
                *bptr++ = bval;
            }
        }

        public static void Memset(void* ptr, int value, ulong size)
        {
            Memset(ptr, value, (long)size);
        }

        public static uint _lrotl(uint x, int y)
        {
            return x << y | x >> 32 - y;
        }

        // public static void* Realloc(void* ptr, long newSize)
        // {
        //     return Operations.Realloc(ptr, newSize);
        // }

        public static void* Realloc(void* ptr, ulong newSize)
        {
            return Realloc(ptr, (long)newSize);
        }

        public static int Abs(int v)
        {
            return Math.Abs(v);
        }

        /// <summary>
        /// This code had been borrowed from here: https://github.com/MachineCognitis/C.math.NET
        /// </summary>
        /// <param name="number"></param>
        /// <param name="exponent"></param>
        /// <returns></returns>
        public static double Frexp(double number, int* exponent)
        {
            var bits = BitConverter.DoubleToInt64Bits(number);
            var exp = (int)((bits & DblExpMask) >> DblMantBits);
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
                    exp = (int)((bits & DblExpMask) >> DblMantBits);
                    *exponent = exp - 1022 - 54;
                }
                // Set exponent to -1 so that number is in [0.5, 1).
                number = BitConverter.Int64BitsToDouble(bits & DblExpClrMask | 0x3fe0000000000000L);
            }

            return number;
        }

        public static double Pow(double a, double b)
        {
            return Math.Pow(a, b);
        }

        public static float Fabs(double a)
        {
            return (float)Math.Abs(a);
        }

        public static double Ceil(double a)
        {
            return Math.Ceiling(a);
        }


        public static double Floor(double a)
        {
            return Math.Floor(a);
        }

        public static double Log(double value)
        {
            return Math.Log(value);
        }

        public static double Exp(double value)
        {
            return Math.Exp(value);
        }

        public static double Cos(double value)
        {
            return Math.Cos(value);
        }

        public static double Acos(double value)
        {
            return Math.Acos(value);
        }

        public static double Sin(double value)
        {
            return Math.Sin(value);
        }

        public static int Memcmp(byte* a, byte[] b, ulong size)
        {
            fixed (void* bptr = b)
            {
                return Memcmp(a, bptr, (long)size);
            }
        }

        public static double Ldexp(double number, int exponent)
        {
            return number * Math.Pow(2, exponent);
        }

        public delegate int QSortComparer(void* a, void* b);

        private static void QsortSwap(byte* data, long size, long pos1, long pos2)
        {
            var a = data + size * pos1;
            var b = data + size * pos2;

            for (long k = 0; k < size; ++k)
            {
                var tmp = *a;
                *a = *b;
                *b = tmp;

                a++;
                b++;
            }
        }

        private static long QsortPartition(byte* data, long size, QSortComparer comparer, long left, long right)
        {
            void* pivot = data + size * left;
            var i = left - 1;
            var j = right + 1;
            for (; ; )
            {
                do
                {
                    ++i;
                } while (comparer(data + size * i, pivot) < 0);

                do
                {
                    --j;
                } while (comparer(data + size * j, pivot) > 0);

                if (i >= j)
                {
                    return j;
                }

                QsortSwap(data, size, i, j);
            }
        }


        private static void QsortInternal(byte* data, long size, QSortComparer comparer, long left, long right)
        {
            if (left < right)
            {
                var p = QsortPartition(data, size, comparer, left, right);

                QsortInternal(data, size, comparer, left, p);
                QsortInternal(data, size, comparer, p + 1, right);
            }
        }

        public static void Qsort(void* data, ulong count, ulong size, QSortComparer comparer)
        {
            QsortInternal((byte*)data, (long)size, comparer, 0, (long)count - 1);
        }

        public static double Sqrt(double val)
        {
            return Math.Sqrt(val);
        }

        public static double Fmod(double x, double y)
        {
            return x % y;
        }

        public static ulong Strlen(sbyte* str)
        {
            var ptr = str;

            while (*ptr != '\0')
            {
                ptr++;
            }

            return (ulong)ptr - (ulong)str - 1;
        }
}

  public abstract unsafe class Pointer : IDisposable
    {
        protected static long _allocatedTotal;
        protected static object Lock = new object();

        public abstract long Size { get; }
        public abstract void* Ptr { get; }

        public static long AllocatedTotal
        {
            get { return _allocatedTotal; }
        }

        public abstract void Dispose();

        public static implicit operator void* (Pointer ptr)
        {
            return ptr.Ptr;
        }

        public static implicit operator char* (Pointer ptr)
        {
            return (char*)ptr.Ptr;
        }

        public static implicit operator uint* (Pointer ptr)
        {
            return (uint*)ptr.Ptr;
        }
    }

    public unsafe class PinnedArray<T> : Pointer
    {
        private GCHandle _handle;
        private bool _disposed;
        private void* _ptr;
        private long _size;

        public GCHandle Handle
        {
            get { return _handle; }
        }

        public override void* Ptr
        {
            get { return _ptr; }
        }

        public T[] Data { get; private set; }

        public T this[long index]
        {
            get { return Data[index]; }
            set { Data[index] = value; }
        }

        public T this[ulong index]
        {
            get { return Data[index]; }
            set { Data[index] = value; }
        }

        public long Count { get; private set; }

        public ulong Length
        {
            get { return (ulong)Count; }
        }

        public override long Size
        {
            get { return _size; }
        }

        public long ElementSize { get; private set; }

        public PinnedArray(long size)
            : this(new T[size])
        {
        }

        public PinnedArray(T[] data)
        {
            Data = data;

            _ptr = null;
            if (data != null)
            {
                _handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                var addr = _handle.AddrOfPinnedObject();
                _ptr = addr.ToPointer();
                ElementSize = Marshal.SizeOf(typeof(T));
                Count = data.Length;
                _size = ElementSize * data.Length;
            }
            else
            {
                ElementSize = 0;
                Count = 0;
                _size = 0;
            }

            lock (Lock)
            {
                _allocatedTotal += _size;
            }
        }

        ~PinnedArray()
        {
            Dispose(false);
        }

        public override void Dispose()
        {
            Dispose(true);

            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            lock (Lock)
            {
                _allocatedTotal -= Size;
            }

            if (Data != null)
            {
                _handle.Free();
                _ptr = null;
                Data = null!;
                _size = 0;
            }

            _disposed = true;
        }
    }


    
internal class ArrayBuffer2D<T>
{
	public T[] Array { get; private set; }

	public int Capacity1 { get; private set; }

	public int Capacity2 { get; private set; }

	public T this[int index1, int index2]
	{
		get => Array[index1 * Capacity2 + index2];
		set => Array[index1 * Capacity2 + index2] = value;
	}

	public ArrayBuffer2D(int capacity1, int capacity2)
	{
		Capacity1 = capacity1;
		Capacity2 = capacity2;
		Array = new T[capacity1 * capacity2];
	}

	public void EnsureSize(int capacity1, int capacity2)
	{
		Capacity1 = capacity1;
		Capacity2 = capacity2;

		var required = capacity1 * capacity2;
		if (Array.Length >= required) return;

		// Realloc
		var oldData = Array;

		var newSize = Array.Length;
		while (newSize < required) newSize *= 2;

		Array = new T[newSize];

		System.Array.Copy(oldData, Array, oldData.Length);
	}
}

internal class ArrayBuffer<T>
{
	public T[] Array { get; private set; }

	public int Capacity => Array.Length;

	public T this[int index]
	{
		get => Array[index];
		set => Array[index] = value;
	}

	public T this[ulong index]
	{
		get => Array[index];
		set => Array[index] = value;
	}

	public ArrayBuffer(int capacity)
	{
		Array = new T[capacity];
	}

	public void EnsureSize(int required)
	{
		if (Array.Length >= required) return;

		// Realloc
		var oldData = Array;

		var newSize = Array.Length;
		while (newSize < required) newSize *= 2;

		Array = new T[newSize];

		System.Array.Copy(oldData, Array, oldData.Length);
	}
}


[SkipLocalsInit, StructLayout(LayoutKind.Sequential )]
internal static class AtomicStats
{
    private static int _allocations;
    internal static int Allocations => _allocations;
    internal static void Allocated()=>	Interlocked.Increment(ref _allocations);
    internal static void Freed() => Interlocked.Decrement(ref _allocations);
}