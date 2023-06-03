namespace RitaEngine.Base.Platform.Structures;

using RitaEngine.Base.Platform.API.DirectX.XAudio;

[ StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public struct AudioDeviceData: IEquatable<AudioDeviceData>
{
    public XAUDIO2_PERFORMANCE_DATA Performance;
    public IXAudio2 Instance;
    public IXAudio2MasteringVoice MasterVoice;
    public nint xaudioModule = nint.Zero;
    private nint _address = nint.Zero;
    public uint InputChannels = 2;
    public AUDIO_STREAM_CATEGORY Categorie= AUDIO_STREAM_CATEGORY.AudioCategory_GameMedia;

    public AudioDeviceData()  { _address = AddressOfPtrThis( ) ;  }
    public unsafe nint AddressOfPtrThis( ) { 
            #pragma warning disable CS8500
        fixed (void* pointer = &this )  { return((nint) pointer ) ; }  
        #pragma warning restore
    }

     #region OVERRIDE    
    public override string ToString() => string.Format($"Data Input " );
    public unsafe override int GetHashCode() => HashCode.Combine( 0);
    public override bool Equals(object? obj) => obj is AudioDeviceData window && this.Equals(window) ;
    public unsafe bool Equals(AudioDeviceData other)=>  false;
    public static bool operator ==(AudioDeviceData  left, AudioDeviceData right) => left.Equals(right);
    public static bool operator !=(AudioDeviceData  left, AudioDeviceData right) => !left.Equals(right);
    #endregion
    public unsafe void Dispose()  { }
}
