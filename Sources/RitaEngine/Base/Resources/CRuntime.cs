namespace RitaEngine.Base.Resources;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

internal static unsafe class CRuntime
{
	public static T[][] CreateArray<T>(int d1, int d2)
	{
		var result = new T[d1][];
		for (int i = 0; i < d1; i++)
		{
			result[i] = new T[d2];
		}

		return result;
	}
	public static byte* ToBytePointer(this IntPtr ptr)
	{
		return (byte*) ptr.ToPointer();
	}
	internal static float Abs(float value) => System.MathF.Abs(value);
	
	internal static void* malloc(ulong size) 
		=>	malloc((long)size);

	internal static void* malloc(long size)
	{
		var ptr = Marshal.AllocHGlobal((int)size);

		// AtomicStats.Allocated();

		return ptr.ToPointer();
	}

	internal static void memcpy(void* a, void* b, long size)
	{
		var ap = (byte*)a;
		var bp = (byte*)b;
		for (long i = 0; i < size; ++i)
			*ap++ = *bp++;
	}

	internal static void memcpy(void* a, void* b, ulong size) 
		=> memcpy(a, b, (long)size);

	internal static void free(void* a)
	{
		if (a == null)
			return;

		var ptr = new IntPtr(a);
		Marshal.FreeHGlobal(ptr);
		// AtomicStats.Freed();
	}

	internal static void memset(void* ptr, int value, long size)
	{
		var bptr = (byte*)ptr;
		var bval = (byte)value;
		for (long i = 0; i < size; ++i)
			*bptr++ = bval;
	}

	internal static void memset(void* ptr, int value, ulong size)
		=>memset(ptr, value, (long)size);

	internal static void memmove(void* a, void* b, long size)
	{
		void* temp = null;

		try
		{
			temp = malloc(size);
			memcpy(temp, b, size);
			memcpy(a, temp, size);
		}

		finally
		{
			if (temp != null)
				free(temp);
		}
	}

	internal static void memmove(void* a, void* b, ulong size)
		=> memmove(a, b, (long)size);

	internal static int memcmp(void* a, void* b, long size)
	{
		var result = 0;
		var ap = (byte*)a;
		var bp = (byte*)b;
		for (long i = 0; i < size; ++i)
		{
			if (*ap != *bp)
				result += 1;

			ap++;
			bp++;
		}

		return result;
	}

	internal static int memcmp(void* a, void* b, ulong size)
		=> memcmp(a, b, (long)size);

	internal static int memcmp(byte* a, byte[] b, ulong size)
	{
		fixed (void* bptr = b)
		{
			return memcmp(a, bptr, (long)size);
		}
	}

	internal static void* realloc(void* a, long newSize)
	{
		if (a == null)
			return malloc(newSize);

		var ptr = new IntPtr(a);
		var result = Marshal.ReAllocHGlobal(ptr, new IntPtr(newSize));

		return result.ToPointer();
	}

	internal static void* realloc(void* a, ulong newSize)
		=> realloc(a, (long)newSize);

	/// <summary>
	/// Fill all items with value
	/// </summary>
	/// <param name="data"></param>
	/// <param name="value"></param>
	/// <typeparam name="T"></typeparam>
	internal static void SetArray<T>(T[] data, T value)
	{
		System.Array.Fill<T>(data,value);
		// for (var i = 0; i < data.Length; ++i)
		// 	data[i] = value;
	}

//STRING
	private static readonly string numbers = "0123456789";

	internal static int strcmp(sbyte *src, string token)
	{
		var result = 0;

		for(var i = 0; i < token.Length; ++i)
		{
			if (src[i] != token[i])
			{
				++result;
			}
		}

		return result;
	}

	internal static int strncmp(sbyte* src, string token, ulong size)
	{
		var result = 0;

		for (var i = 0; i < Math.Min(token.Length, (int)size); ++i)
		{
			if (src[i] != token[i])
			{
				++result;
			}
		}

		return result;
	}

	public static uint _lrotl(uint x, int y)
	{
		return (x << y) | (x >> (32 - y));
	}

	public static double fmod(double x, double y)
	{
		return x % y;
	}

	public static double sqrt(double val)
	{
		return  Math.Sqrt(val);
	}

	internal static long strtol(sbyte *start, sbyte **end, int radix)
	{
		// First step - determine length
		var length = 0;
		sbyte* ptr = start;
		while(numbers.IndexOf((char)*ptr) != -1)
		{
			++ptr;
			++length;
		}

		long result = 0;

		// Now build up the number
		ptr = start;
		while(length > 0)
		{
			long num = numbers.IndexOf((char)*ptr);
			long pow = (long)Math.Pow(10, length - 1);
			result += num * pow;

			++ptr;
			--length;
		}

		if (end != null)
		{
			*end = ptr;
		}

		return result;
	}

	internal static ulong strlen(sbyte* str)
	{
		var ptr = str;

		while (*ptr != '\0')
			ptr++;

		return (ulong)ptr - (ulong)str - 1;
	}

//Q SORT
	internal delegate int QSortComparer(void* a, void* b);

	private static void qsortSwap(byte* data, long size, long pos1, long pos2)
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

	private static long qsortPartition(byte* data, long size, QSortComparer comparer, long left, long right)
	{
		void* pivot = data + size * left;
		var i = left - 1;
		var j = right + 1;
		for (;;)
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

			qsortSwap(data, size, i, j);
		}
	}

	private static void qsortInternal(byte* data, long size, QSortComparer comparer, long left, long right)
	{
		if (left < right)
		{
			var p = qsortPartition(data, size, comparer, left, right);

			qsortInternal(data, size, comparer, left, p);
			qsortInternal(data, size, comparer, p + 1, right);
		}
	}

	internal static void qsort(void* data, ulong count, ulong size, QSortComparer comparer)
	{
		qsortInternal((byte*) data, (long) size, comparer, 0, (long) count - 1);
	}

public const long DBL_EXP_MASK = 0x7ff0000000000000L;
		public const int DBL_MANT_BITS = 52;
		public const long DBL_SGN_MASK = -1 - 0x7fffffffffffffffL;
		public const long DBL_MANT_MASK = 0x000fffffffffffffL;
		public const long DBL_EXP_CLR_MASK = DBL_SGN_MASK | DBL_MANT_MASK;

		public static int abs(int v)
		{
			return Math.Abs(v);
		}

		/// <summary>
		///     This code had been borrowed from here: https://github.com/MachineCognitis/C.math.NET
		/// </summary>
		/// <param name="number"></param>
		/// <param name="exponent"></param>
		/// <returns></returns>
		public static double frexp(double number, int* exponent)
		{
			var bits = BitConverter.DoubleToInt64Bits(number);
			var exp = (int) ((bits & DBL_EXP_MASK) >> DBL_MANT_BITS);
			*exponent = 0;

			if (exp == 0x7ff || number == 0D)
			{
				number += number;
			}
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

		public static double pow(double a, double b)
		{
			return Math.Pow(a, b);
		}

		public static float fabs(double a)
		{
			return (float) Math.Abs(a);
		}

		public static double ceil(double a)
		{
			return Math.Ceiling(a);
		}


		public static double floor(double a)
		{
			return Math.Floor(a);
		}

		public static double log(double value)
		{
			return Math.Log(value);
		}

		public static double exp(double value)
		{
			return Math.Exp(value);
		}

		public static double cos(double value)
		{
			return Math.Cos(value);
		}

		public static double acos(double value)
		{
			return Math.Acos(value);
		}

		public static double sin(double value)
		{
			return Math.Sin(value);
		}

		public static double ldexp(double number, int exponent)
		{
			return number * Math.Pow(2, exponent);
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