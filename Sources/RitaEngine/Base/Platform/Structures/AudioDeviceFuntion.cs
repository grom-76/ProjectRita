namespace RitaEngine.Base.Platform.Structures;


using System;
using RitaEngine.Base.Platform.Native.DirectX.XAudio;


[ StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public unsafe readonly struct AudioDeviceFunction : IEquatable<AudioDeviceFunction> , IDisposable
{
    public readonly delegate* unmanaged< void**, uint, uint, uint> PFN_XAudio2Create=null;
    public readonly delegate* unmanaged<void*, uint, int> PFN_CreateAudioReverb=null;
    public readonly delegate* unmanaged<void*, uint, int> PFN_CreateAudioVolumeMeter=null;
    public readonly delegate* unmanaged<uint, float, X3DAUDIO_HANDLE**, int> PFN_X3DAudioInitialize=null;
    public readonly delegate* unmanaged<void*, void*, void*, int, void*, void> PFN_X3DAudioCalculate=null;
    
    public  AudioDeviceFunction( Base.Platform.PFN_GetSymbolPointer load,IntPtr module)
    {
        PFN_XAudio2Create=(delegate* unmanaged<void**, uint, uint, uint>)load( module,"XAudio2Create" );
        PFN_CreateAudioReverb=(delegate* unmanaged<void*, uint, int>)load( module,"CreateAudioReverb");
        PFN_CreateAudioVolumeMeter=(delegate* unmanaged<void*, uint, int>) load( module,"CreateAudioVolumeMeter");
        PFN_X3DAudioInitialize=(delegate* unmanaged<uint, float, X3DAUDIO_HANDLE**, int>) load( module,"X3DAudioInitialize");
        PFN_X3DAudioCalculate=(delegate* unmanaged<void*, void*, void*, int, void*, void>)load( module,"X3DAudioCalculate");
    }

    public uint XAudio2Create( ref IXAudio2 iXAudio2, uint flags=0, uint processor = Constants.XAUDIO2_DEFAULT_PROCESSOR)
    {
        void* ptr= null;
        uint err =PFN_XAudio2Create(&ptr ,0, Constants.XAUDIO2_DEFAULT_PROCESSOR);
        iXAudio2 = new(ptr) ;
        return err;
    }

    public unsafe nint AddressOfPtrThis( ){fixed (void* pointer = &this)  { return((nint) pointer ) ; }  }
    #region OVERRIDE
    public override string ToString() => string.Format($"Vector" );
    public unsafe override int GetHashCode() => HashCode.Combine(((nint)PFN_XAudio2Create)  ,  (nint)PFN_X3DAudioInitialize  )  ;
    public override bool Equals(object? obj) => obj is AudioDeviceFunction context && this.Equals(context) ;
    public unsafe bool Equals(AudioDeviceFunction other)=> other is AudioDeviceFunction input && ((nint)PFN_XAudio2Create).ToInt64().Equals( ((nint)input.PFN_XAudio2Create)) ;
    public static bool operator ==(AudioDeviceFunction  left, AudioDeviceFunction right) => left.Equals(right);
    public static bool operator !=(AudioDeviceFunction  left, AudioDeviceFunction  right) => !left.Equals(right);
    public void Dispose() {  }
    #endregion

}

