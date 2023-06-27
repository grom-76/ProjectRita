namespace RitaEngine.Platform.Structures;


using System;
using RitaEngine.Base;
using RitaEngine.Platform.API.DirectX.XAudio;


[ StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public unsafe readonly struct AudioDeviceFunction : IEquatable<AudioDeviceFunction> 
{
    public readonly delegate* unmanaged< void**, uint, uint, uint> PFN_XAudio2Create=null;
    public readonly delegate* unmanaged<void*, uint, int> PFN_CreateAudioReverb=null;
    public readonly delegate* unmanaged<void*, uint, int> PFN_CreateAudioVolumeMeter=null;
    public readonly delegate* unmanaged<uint, float, X3DAUDIO_HANDLE**, int> PFN_X3DAudioInitialize=null;
    public readonly delegate* unmanaged<void*, void*, void*, int, void*, void> PFN_X3DAudioCalculate=null;
    public readonly nint xaudio = nint.Zero;
    
    public  AudioDeviceFunction( string  moduleName)
    {
        xaudio = Libraries.Load( moduleName);
        PFN_XAudio2Create=(delegate* unmanaged<void**, uint, uint, uint>)Libraries.GetUnsafeSymbol( xaudio,"XAudio2Create" );
        PFN_CreateAudioReverb=(delegate* unmanaged<void*, uint, int>)Libraries.GetUnsafeSymbol( xaudio,"CreateAudioReverb");
        PFN_CreateAudioVolumeMeter=(delegate* unmanaged<void*, uint, int>) Libraries.GetUnsafeSymbol( xaudio,"CreateAudioVolumeMeter");
        PFN_X3DAudioInitialize=(delegate* unmanaged<uint, float, X3DAUDIO_HANDLE**, int>) Libraries.GetUnsafeSymbol( xaudio,"X3DAudioInitialize");
        PFN_X3DAudioCalculate=(delegate* unmanaged<void*, void*, void*, int, void*, void>)Libraries.GetUnsafeSymbol( xaudio,"X3DAudioCalculate");
    }

    public uint XAudio2Create( ref IXAudio2 iXAudio2, uint flags=0, uint processor = Constants.XAUDIO2_DEFAULT_PROCESSOR)
    {
        void* ptr= null;
        uint err =PFN_XAudio2Create(&ptr ,0, Constants.XAUDIO2_DEFAULT_PROCESSOR);
        iXAudio2 = new(ptr) ;
        return err;
    }

   
    #region OVERRIDE
    public override string ToString() => string.Format($"Vector" );
    public unsafe override int GetHashCode() => HashCode.Combine(((nint)PFN_XAudio2Create)  ,  (nint)PFN_X3DAudioInitialize  )  ;
    public override bool Equals(object? obj) => obj is AudioDeviceFunction context && this.Equals(context) ;
    public unsafe bool Equals(AudioDeviceFunction other)=> other is AudioDeviceFunction input && ((nint)PFN_XAudio2Create).ToInt64().Equals( ((nint)input.PFN_XAudio2Create)) ;
    public static bool operator ==(AudioDeviceFunction  left, AudioDeviceFunction right) => left.Equals(right);
    public static bool operator !=(AudioDeviceFunction  left, AudioDeviceFunction  right) => !left.Equals(right);
   
    #endregion

}

