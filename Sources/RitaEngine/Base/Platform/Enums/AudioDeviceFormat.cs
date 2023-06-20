namespace RitaEngine.Base.Platform.Config;

public enum AudioDeviceFormat
{
    /// <summary>16 bit floating point [-1;1]  need HalFloat</summary>
    F16,
    /// <summary>32 bit floating point [-1;1] float </summary>
    F32,
    
    /// <summary> 8 bit  unsigned integer [0, 255]  corespond a byte</summary>
    U8,
    /// <summary> 16 bit signed integer [0, 65535 ]  ushort </summary>
    U16,

    U32,
    /// <summary> 8 bit  unsigned integer [-125, +125] sbyte </summary>
    S8,
    /// <summary> 16 bit signed integer [-32768, 32767]  short </summary>
    S16,
    S24,
    S32,

}
