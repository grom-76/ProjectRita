

namespace RitaEngine.Base.Platform ;


using RitaEngine.Base.Platform.Native.DirectX.XAudio;
using RitaEngine.Base.Platform.Structures;
using RitaEngine.Base.Platform.Config;

[ StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public struct AudioDevice: IEquatable<AudioDevice>
{
    private AudioDeviceData _data;
    private AudioDeviceFunction _funcs;
    public AudioDeviceConfig Config = new();
    private nint _address = nint.Zero;

    public AudioDeviceData GetData() => _data;
    public unsafe void Init()
    {
        #if WIN64     

        _data = new()  ;

        _data.xaudioModule = Libraries.Load(Config.XAudioDllName);
        _funcs = new( Libraries.GetUnsafeSymbol , _data.xaudioModule);
        // Log.Info(" Init Audio Win32");
        uint err = _funcs.XAudio2Create(ref _data.Instance);
        // Log.Info ($"Create Xaudio 2 INSTANCE Error Code : {err} ");

        err = _data.Instance.StartEngine();
        // Log.Info ($"Start engine  Error Code : {err} ");

        err = _data.Instance.CreateMasteringVoice(ref _data.MasterVoice,(uint)Config.Channels,0,0,null,null, (AUDIO_STREAM_CATEGORY)Config.Category );
        // Log.Info ($"Create MasterVoice INSTANCE Error Code : {err} ");  
        
        
        // infos.Categorie = config.Categorie.ToString();
        // infos.InputChannels = config.InputChannels;
    #endif
        Config.Dispose();
    }

    public void Release() => ReleaseAudioDevice(ref _data);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="volume">clamp between 0.0f - 1.0f</param>
    public void SetMasterVolume( float volume)=> SetVolume(ref _data,volume);

    public AudioDevice()
    {
          _address = AddressOfPtrThis( ) ;
    }
    public unsafe nint AddressOfPtrThis( )
    { 
        #pragma warning disable CS8500
        fixed (void* pointer = &this )  { return((nint) pointer ) ; }  
        #pragma warning restore
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
    public override string ToString() => string.Format($"Data Window " );
    public override int GetHashCode() => HashCode.Combine( _data.GetHashCode(), _funcs.GetHashCode());
    public override bool Equals(object? obj) => obj is AudioDevice  window && this.Equals(window) ;
    public bool Equals(AudioDevice other)=>  _data.Equals(other._data) ;
    public static bool operator ==(AudioDevice  left, AudioDevice right) => left.Equals(right);
    public static bool operator !=(AudioDevice  left, AudioDevice right) => !left.Equals(right);
    #endregion
}


