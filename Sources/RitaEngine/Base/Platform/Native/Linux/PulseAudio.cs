namespace RitaEngine.Base.Platform.API.Pulse;


using HRESULT = System.UInt32;
using DWORD = System.UInt32;
using WORD  = System.UInt16;
using uint8_t = System.Byte;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System;


/// <summary>
/// Inspired by : https://github.com/terrafx/terrafx.interop.pulseaudio
/// </summary>
public unsafe static class PULSE
{
    
    public const uint PA_VOLUME_NORM = ((uint)(0x10000U));
    public const uint PA_VOLUME_MUTED = ((uint)(0U));
    public const uint PA_VOLUME_MAX = ((uint)(4294967295U) / 2);
    // public static uint PA_VOLUME_UI_MAX => (pa_sw_volume_from_dB(+11.0));
    public const uint PA_VOLUME_INVALID = ((uint)(4294967295U));
    public const int PA_CVOLUME_SNPRINT_MAX = 320;
    public const int PA_SW_CVOLUME_SNPRINT_DB_MAX = 448;
    public const int PA_CVOLUME_SNPRINT_VERBOSE_MAX = 1984;
    public const int PA_VOLUME_SNPRINT_MAX = 10;
    public const int PA_SW_VOLUME_SNPRINT_DB_MAX = 11;
    public const int PA_VOLUME_SNPRINT_VERBOSE_MAX = 35;
    public const double PA_DECIBEL_MININFTY = ((double)(-200.0));
}

public unsafe partial struct pa_cvolume
{
    public uint8_t channels;
    public fixed uint values[32];
}