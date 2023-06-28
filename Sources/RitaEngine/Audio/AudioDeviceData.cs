namespace RitaEngine.Audio;

using RitaEngine.Audio;
using RitaEngine.Base;
using RitaEngine.Platform.API.DirectX.XAudio;

[ StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public struct AudioDeviceData: IEquatable<AudioDeviceData>
{
    public XAUDIO2_PERFORMANCE_DATA Performance;
    public IXAudio2 Instance;
    public IXAudio2MasteringVoice MasterVoice;
    public uint InputChannels = 2;
    public AUDIO_STREAM_CATEGORY Categorie= AUDIO_STREAM_CATEGORY.AudioCategory_GameMedia;
    public AudioDeviceBackEnd BackEnd = AudioDeviceBackEnd.Xaudio;


    public AudioDeviceData()  {   }
  
     #region OVERRIDE    
    public override string ToString() => string.Format($"Audio Device Data" );
    public unsafe override int GetHashCode() => HashCode.Combine( 0);
    public override bool Equals(object? obj) => obj is AudioDeviceData window && this.Equals(window) ;
    public unsafe bool Equals(AudioDeviceData other)=>  false;
    public static bool operator ==(AudioDeviceData  left, AudioDeviceData right) => left.Equals(right);
    public static bool operator !=(AudioDeviceData  left, AudioDeviceData right) => !left.Equals(right);
    #endregion
    public unsafe void Dispose()  { }
}
