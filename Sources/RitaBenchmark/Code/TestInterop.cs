

namespace RitaBenchmark
{
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
    // using AdvancedDLSupport;
    using BenchmarkDotNet.Attributes;

    // [SuppressUnmanagedCodeSecurity]
    // public interface INativeLibrary
    // {
    //     bool QueryPerformanceCounter(out long lpPerformanceCount);
    //     bool QueryPerformanceFrequency(out long frequency);
    // }
    
    [SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack =4),SkipLocalsInit]
    public static class DLLImport
    {
        [DllImport("kernel32.dll", SetLastError=true)][SuppressUnmanagedCodeSecurity][SuppressGCTransition]
        public static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("kernel32.dll", SetLastError=true)][SuppressUnmanagedCodeSecurity][SuppressGCTransition]
        public static extern bool QueryPerformanceFrequency(out long frequency);
    }

    [StructLayout(LayoutKind.Sequential, Pack =4),SkipLocalsInit]
    public unsafe readonly struct Commands : IDisposable, IEquatable<Commands>
    {
        
        public readonly unsafe delegate* unmanaged<out UInt64, int> QueryPerformanceCounter = null;
        public readonly unsafe delegate* unmanaged<out UInt64, int> QueryPerformanceFrequency = null;  

        public Commands()
        {
            var moduleKernel = NativeLibrary.Load("kernel32");

            QueryPerformanceCounter = (delegate* unmanaged<out UInt64, int>) NativeLibrary.GetExport(moduleKernel, "QueryPerformanceCounter");
            QueryPerformanceFrequency = (delegate* unmanaged<out UInt64, int>)NativeLibrary.GetExport(moduleKernel, "QueryPerformanceFrequency");
        }

        public bool Equals(Commands other) => false;
        public override string ToString()  => string.Format($"Clock Commands" );
	    public override int GetHashCode() => (int) 0;
	    public override bool Equals(object? obj) => false;

        public void Dispose() { }

        public static bool operator ==(Commands  left, Commands right) => left.Equals(right);
	    public static bool operator !=(Commands  left, Commands  right) => !left.Equals(right);

    }

     [StructLayout(LayoutKind.Sequential, Pack =4),SkipLocalsInit]
    public unsafe readonly struct Commands2 
    {
        public readonly unsafe delegate* unmanaged<out UInt64, int> QueryPerformanceCounter = null;
        public readonly unsafe delegate* unmanaged<out UInt64, int> QueryPerformanceFrequency = null;  

        public Commands2()
        {
            var moduleKernel = NativeLibrary.Load("kernel32");

            QueryPerformanceCounter = (delegate* unmanaged<out UInt64, int>) NativeLibrary.GetExport(moduleKernel, "QueryPerformanceCounter");
            QueryPerformanceFrequency = (delegate* unmanaged<out UInt64, int>)NativeLibrary.GetExport(moduleKernel, "QueryPerformanceFrequency");
        }
    }


    [SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack =4),SkipLocalsInit]
    public unsafe static class Function
    {
        public unsafe static delegate* unmanaged<out UInt64, int> QueryPerformanceCounter = null;
        public unsafe static delegate* unmanaged<out UInt64, int> QueryPerformanceFrequency = null;  

        public  static void Loader()
        {
            var moduleKernel = NativeLibrary.Load("kernel32");

            QueryPerformanceCounter = (delegate* unmanaged<out UInt64, int>) NativeLibrary.GetExport(moduleKernel, "QueryPerformanceCounter");
            QueryPerformanceFrequency = (delegate* unmanaged<out UInt64, int>)NativeLibrary.GetExport(moduleKernel, "QueryPerformanceFrequency");
        }
    }

    [SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack =4),SkipLocalsInit]
    public class TestInterop
    {
        Commands cmd ;
        Commands2 cmd2;


        [Params(10000, 100000)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            Marshal.PrelinkAll(typeof(DLLImport));

            cmd = new();

            cmd2 = new();

            Function.Loader();
        }


        [Benchmark]
        public unsafe void Use_ReadonlyStructWithOveride()
        {
            ulong elpased = 0L;
            ulong previous =0L;
            for ( int i = 0 ; i < N ; i++)
            {
                _= cmd.QueryPerformanceCounter( out elpased);
                previous -= elpased;

            }
        }

        [Benchmark]
        public unsafe void Use_ReadonlyStruct()
        {
            ulong elpased = 0L;
            ulong previous =0L;
            for ( int i = 0 ; i < N ; i++)
            {
                _= cmd2.QueryPerformanceCounter( out elpased);
                previous -= elpased;

            }
        }

        [Benchmark]
        public unsafe void Use_StaticDelegate()
        {
            ulong elpased = 0L;
            ulong previous =0L;
            for ( int i = 0 ; i < N ; i++)
            {
                _=  Function.QueryPerformanceCounter( out elpased);
                previous -= elpased;

            }
        }

        [Benchmark]
        public void Use_DLLImport()
        {
            long elpased = 0L;
            long previous =0L;
            for ( int i = 0 ; i < N ; i++)
            {
                _= DLLImport.QueryPerformanceCounter( out elpased);
                previous -= elpased;

            }
        }


    }


}


/*
/------------------------  ------------------------------------------------

BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.1702/22H2/2022Update/SunValley2)
AMD Ryzen 5 3400G with Radeon Vega Graphics, 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.203
  [Host]     : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2


|                        Method |      N |       Mean |    Error |    StdDev |     Median |
|------------------------------ |------- |-----------:|---------:|----------:|-----------:|
| Use_ReadonlyStructWithOveride |  10000 |   253.6 μs |  5.92 μs |  16.78 μs |   253.3 μs |
|            Use_ReadonlyStruct |  10000 |   257.9 μs |  6.92 μs |  19.76 μs |   254.7 μs |
|            Use_StaticDelegate |  10000 |   258.0 μs |  5.11 μs |  12.45 μs |   253.3 μs |
|                 Use_DLLImport |  10000 |   239.8 μs |  4.69 μs |   6.87 μs |   236.9 μs |
| Use_ReadonlyStructWithOveride | 100000 | 2,349.9 μs | 38.90 μs |  34.49 μs | 2,332.8 μs |
|            Use_ReadonlyStruct | 100000 | 2,404.0 μs | 47.54 μs | 122.71 μs | 2,363.7 μs |
|            Use_StaticDelegate | 100000 | 2,354.4 μs | 45.01 μs |  46.22 μs | 2,335.5 μs |
|                 Use_DLLImport | 100000 | 2,397.3 μs | 38.36 μs |  34.00 μs | 2,387.2 μs |



|                        Method |      N |       Mean |    Error |   StdDev |
|------------------------------ |------- |-----------:|---------:|---------:|
| Use_ReadonlyStructWithOveride |  10000 |   237.1 μs |  4.53 μs |  5.40 μs |
|            Use_ReadonlyStruct |  10000 |   239.0 μs |  4.61 μs |  4.94 μs |
|            Use_StaticDelegate |  10000 |   236.0 μs |  4.09 μs |  4.87 μs |
|                 Use_DLLImport |  10000 |   239.8 μs |  4.13 μs |  3.87 μs |
| Use_ReadonlyStructWithOveride | 100000 | 2,316.1 μs | 18.77 μs | 15.67 μs |
|            Use_ReadonlyStruct | 100000 | 2,351.5 μs | 46.67 μs | 41.37 μs |
|            Use_StaticDelegate | 100000 | 2,339.7 μs | 27.49 μs | 24.37 μs |
|                 Use_DLLImport | 100000 | 2,399.9 μs | 46.00 μs | 51.13 μs |



|                        Method |      N |       Mean |    Error |   StdDev |     Median |
|------------------------------ |------- |-----------:|---------:|---------:|-----------:|
| Use_ReadonlyStructWithOveride |  10000 |   249.2 μs |  2.72 μs |  2.55 μs |   248.8 μs |
|            Use_ReadonlyStruct |  10000 |   238.2 μs |  4.53 μs |  6.20 μs |   235.8 μs |
|            Use_StaticDelegate |  10000 |   237.3 μs |  4.09 μs |  4.54 μs |   237.2 μs |
|                 Use_DLLImport |  10000 |   257.8 μs |  7.88 μs | 22.74 μs |   250.0 μs |
| Use_ReadonlyStructWithOveride | 100000 | 2,322.1 μs | 20.58 μs | 18.24 μs | 2,320.4 μs |
|            Use_ReadonlyStruct | 100000 | 2,345.8 μs | 21.45 μs | 23.85 μs | 2,339.7 μs |
|            Use_StaticDelegate | 100000 | 2,331.5 μs | 17.13 μs | 16.02 μs | 2,328.8 μs |
|                 Use_DLLImport | 100000 | 2,342.7 μs | 18.85 μs | 14.71 μs | 2,343.4 μs |

*/