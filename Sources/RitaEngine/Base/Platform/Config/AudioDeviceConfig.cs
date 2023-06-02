namespace RitaEngine.Base.Platform.Config; 

   
[StructLayout(LayoutKind.Sequential, Pack = 4),SkipLocalsInit]
public sealed class AudioDeviceConfig : IDisposable
{ 
    public string XAudioDllName ="xaudio2_9.dll";
    public  AudioChannels Channels = AudioChannels.stereo;
    public  AudioCategory Category =  AudioCategory.GameMedia;

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

public enum AudioChannels
{
    Mono=1,
    stereo =2,
    _7point1 =7,
}

public enum AudioCategory
{
    Other = 0,
    ForegroundOnlyMedia = 1,
    Communications = 3,
    Alerts = 4,
    SoundEffects = 5,
    GameEffects = 6,
    GameMedia = 7,
    GameChat = 8,
    Speech = 9,
    Movie = 10,
    Media = 11,
}

public enum AudioDeviceBackEnd
{
    None,
    Dsound,
    Xaudio,
    CoreAudio,
    Pulse,
    Alsa,
    Winmm,
    OpenAL,
    MegaDrive,
    //system decide for you
    AutoDetect,
    Web,
    PS4,
    XBOX,
SWITCH,
}

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
