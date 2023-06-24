

namespace RitaEngine.Base.Platform ;


using RitaEngine.Base.Platform.API.DirectX.XAudio;
using RitaEngine.Base.Platform.Structures;
using RitaEngine.Base.Platform.Config;

[ StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public struct AudioDevice: IEquatable<AudioDevice>
{
    private AudioDeviceData _data = new();
    private AudioDeviceFunction _funcs;

    public AudioDeviceData GetData() => _data;
    public unsafe void Init( PlatformConfig Config )
    {
        #if WIN64     

        _funcs = new(Config.LibraryName_xaudio);
        
        uint err = _funcs.XAudio2Create(ref _data.Instance);
        Log.WarnWhenConditionIsFalse (err != 0,  $"Create Xaudio 2 INSTANCE : {_data.Instance:X8}  ");

        err = _data.Instance.StartEngine();
        Log.WarnWhenConditionIsFalse (err != 0, $"Start engine  ");

        err = _data.Instance.CreateMasteringVoice(ref _data.MasterVoice,(uint)Config.Audio_Channels,0,0,null,null, (AUDIO_STREAM_CATEGORY)Config.Audio_Category );
        Log.WarnWhenConditionIsFalse (err != 0, $"Create MasterVoice { _data.MasterVoice:X} ");  

        #endif

    }

    public void Release() => ReleaseAudioDevice(ref _data);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="volume">clamp between 0.0f - 1.0f</param>
    public void SetMasterVolume( float volume)=> SetVolume(ref _data,volume);

    public AudioDevice()
    {
         
    }
  
    private unsafe static  void GetPerformance(ref AudioDeviceData data )
    {
        data.Instance.GetPerformanceData(ref data.Performance );
    }

    public unsafe static void ReleaseAudioDevice(ref AudioDeviceData data)
    {
    #if WIN64
        data.MasterVoice.DestroyVoice();
        data.Instance.StopEngine();
        data.Instance.Release();
    #endif
    }
    public static void SetVolume(ref AudioDeviceData data ,float volume)
    #if WIN64
        => _ = data.MasterVoice.SetVolume(volume < 0.0f ? 0.0f: volume > 1.0f? 1.0f: volume  );
    #else
        => _ = volume;
    #endif

       #region OVERRIDE    
    public override string ToString() => string.Format($"Audio Device" );
    public override int GetHashCode() => HashCode.Combine( _data.GetHashCode(), _funcs.GetHashCode());
    public override bool Equals(object? obj) => obj is AudioDevice  window && this.Equals(window) ;
    public bool Equals(AudioDevice other)=>  _data.Equals(other._data) ;
    public static bool operator ==(AudioDevice  left, AudioDevice right) => left.Equals(right);
    public static bool operator !=(AudioDevice  left, AudioDevice right) => !left.Equals(right);
    #endregion
}


