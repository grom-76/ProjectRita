namespace RitaEngine.Base.Platform.Config;

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
