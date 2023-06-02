
namespace RitaEngine.Base.Platform.Native.DirectX;


using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using DWORD = System.UInt32; // A 32-bit unsigned integer. The range is 0 through 4294967295 decimal.
using LPCWSTR = System.Char;//An LPCWSTR is a 32-bit pointer to a constant string of 16-bit Unicode characters, which MAY be null-terminated.
using SHORT = System.Int16;//A 16-bit integer. The range is -32768 through 32767 decimal.
using System;
using System.Security;
using BOOL = System.Int32;
using LONG = System.Int32;
using WORD  = System.UInt16;
using BYTE = System.Byte;
using HRESULT = System.UInt32;


[StructLayout(LayoutKind.Sequential)]
public unsafe struct  GUID{
    public ulong  Data1;
    public ushort Data2;
    public ushort Data3;
    public fixed byte           Data4[ 8 ];

    public GUID( ulong d1, ushort d2, ushort d3, byte b1 , byte b2 , byte b3 , byte b4 , byte b5 , byte b6 ,byte b7 ,byte b8)
    {
        Data1 = d1;
        Data2 = d2;
        Data3 = d3;
        Data4[0] = b1;
        Data4[1] = b2;
        Data4[2] = b3;
        Data4[3] = b4;
        Data4[4] = b5;
        Data4[5] = b6;
        Data4[6] = b7;
        Data4[7] = b8;
    }
};

[StructLayout(LayoutKind.Sequential)]
public struct WAVEFORMATEX 
{
    public WORD wFormatTag;
    public WORD nChannels;
    public DWORD nSamplesPerSec;
    public DWORD nAvgBytesPerSec;
    public WORD nBlockAlign;
    public WORD wBitsPerSample;
    public WORD cbSize;
} 
