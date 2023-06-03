namespace RitaEngine.Base.Platform.API.DirectX.XAudio;


using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using DWORD = System.UInt32; // A 32-bit unsigned integer. The range is 0 through 4294967295 decimal.
using LPCWSTR = System.Char;//An LPCWSTR is a 32-bit pointer to a constant string of 16-bit Unicode characters, which MAY be null-terminated.
using SHORT = System.Int16;//A 16-bit integer. The range is -32768 through 32767 decimal.
using System;
using System.Security;
using BOOL = System.Int32;
using LONG = System.Int32;
using WORD = System.UInt16;
using BYTE = System.Byte;
using HRESULT = System.UInt32;

#region AUDIO


[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = 4), SkipLocalsInit]
public unsafe static class Constants
{
    


    #region CONSTANTS
    // Numeric boundary values
    public const uint  XAUDIO2_MAX_BUFFER_BYTES        =0x80000000;    // Maximum bytes allowed in a source buffer
    public const int  XAUDIO2_MAX_QUEUED_BUFFERS      = 64    ;        // Maximum buffers allowed in a voice queue
    public const int  XAUDIO2_MAX_BUFFERS_SYSTEM      = 2    ;         // Maximum buffers allowed for system threads (Xbox 360 only)
    public const int  XAUDIO2_MAX_AUDIO_CHANNELS      = 64   ;         // Maximum channels in an audio stream
    public const int  XAUDIO2_MIN_SAMPLE_RATE         = 1000   ;       // Minimum audio sample rate supported
    public const int  XAUDIO2_MAX_SAMPLE_RATE         = 200000 ;       // Maximum audio sample rate supported
    public const float  XAUDIO2_MAX_VOLUME_LEVEL        = 16777216.0f;   // Maximum acceptable volume level (2^24)
    public const float  XAUDIO2_MIN_FREQ_RATIO          = (1/1024.0f) ;  // Minimum SetFrequencyRatio argument
    public const float  XAUDIO2_MAX_FREQ_RATIO          = 1024.0f   ;    // Maximum MaxFrequencyRatio argument
    public const float  XAUDIO2_DEFAULT_FREQ_RATIO      = 2.0f ;         // Default MaxFrequencyRatio argument
    public const float  XAUDIO2_MAX_FILTER_ONEOVERQ     = 1.5f ;         // Maximum XAUDIO2_FILTER_PARAMETERS.OneOverQ
    public const float  XAUDIO2_MAX_FILTER_FREQUENCY    = 1.0f ;         // Maximum XAUDIO2_FILTER_PARAMETERS.Frequency
    public const int  XAUDIO2_MAX_LOOP_COUNT          = 254 ;          // Maximum non-infinite XAUDIO2_BUFFER.LoopCount
    public const int  XAUDIO2_MAX_INSTANCES           = 8 ;            // Maximum simultaneous XAudio2 objects on Xbox 360
    //// For XMA voices on Xbox 360 there is an additional restriction on the MaxFrequencyRatio
    //// argument and the voice's sample rate: the product of these numbers cannot exceed 600000
    //// for one-channel voices or 300000 for voices with more than one channel.
    public const int  XAUDIO2_MAX_RATIO_TIMES_RATE_XMA_MONO         =600000;
    public const int  XAUDIO2_MAX_RATIO_TIMES_RATE_XMA_MULTICHANNEL =300000;

    // // Numeric values with special meanings
    public const int  XAUDIO2_COMMIT_NOW              = 0;             // Used as an OperationSet argument
    public const int  XAUDIO2_COMMIT_ALL              = 0 ;            // Used in IXAudio2::CommitChanges
    public const int  XAUDIO2_INVALID_OPSET           = (-1);  // Not allowed for OperationSet arguments
    public const int  XAUDIO2_NO_LOOP_REGION          = 0  ;           // Used in XAUDIO2_BUFFER.LoopCount
    public const int  XAUDIO2_LOOP_INFINITE           = 255  ;         // Used in XAUDIO2_BUFFER.LoopCount
    public const int  XAUDIO2_DEFAULT_CHANNELS        = 0    ;         // Used in CreateMasteringVoice
    public const int  XAUDIO2_DEFAULT_SAMPLERATE      = 0   ;          // Used in CreateMasteringVoice

    // // Flags
    public const uint  XAUDIO2_DEBUG_ENGINE                  =0x0001;    // Used in XAudio2Create
    public const uint  XAUDIO2_VOICE_NOPITCH                 =0x0002;    // Used in IXAudio2::CreateSourceVoice
    public const uint  XAUDIO2_VOICE_NOSRC                   =0x0004;    // Used in IXAudio2::CreateSourceVoice
    public const uint  XAUDIO2_VOICE_USEFILTER               =0x0008;    // Used in IXAudio2::CreateSource/SubmixVoice
    public const uint  XAUDIO2_PLAY_TAILS                    =0x0020;    // Used in IXAudio2SourceVoice::Stop
    public const uint  XAUDIO2_END_OF_STREAM                 =0x0040;    // Used in XAUDIO2_BUFFER.Flags
    public const uint  XAUDIO2_SEND_USEFILTER                =0x0080;    // Used in XAUDIO2_SEND_DESCRIPTOR.Flags
    public const uint  XAUDIO2_VOICE_NOSAMPLESPLAYED         =0x0100;    // Used in IXAudio2SourceVoice::GetState
    public const uint  XAUDIO2_STOP_ENGINE_WHEN_IDLE         =0x2000;    // Used in XAudio2Create to force the engine to Stop when no source voices are Started, and Start when a voice is Started
    public const uint  XAUDIO2_1024_QUANTUM                  =0x8000;    // Used in XAudio2Create to specify nondefault processing quantum of 21.33 ms (1024 samples at 48KHz)
    public const uint  XAUDIO2_NO_VIRTUAL_AUDIO_CLIENT       =0x10000;   // Used in CreateMasteringVoice to create a virtual audio client

    // // Default parameters for the built-in filter
    // public const int  XAUDIO2_DEFAULT_FILTER_TYPE     LowPassFilter
    public const float  XAUDIO2_DEFAULT_FILTER_FREQUENCY  = XAUDIO2_MAX_FILTER_FREQUENCY;
    public const float  XAUDIO2_DEFAULT_FILTER_ONEOVERQ =1.0f;

    // // public XAudio2 constants
    // // The audio frame quantum can be calculated by reducing the fraction:
    // //     SamplesPerAudioFrame / SamplesPerSecond
    // public const int  XAUDIO2_QUANTUM_NUMERATOR   1                 // On Windows, XAudio2 processes audio
    // public const int  XAUDIO2_QUANTUM_DENOMINATOR 100               //  in 10ms chunks (= 1/100 seconds)
    // public const int  XAUDIO2_QUANTUM_MS (1000.0f * XAUDIO2_QUANTUM_NUMERATOR / XAUDIO2_QUANTUM_DENOMINATOR)

    // // XAudio2 error codes
    public const int  FACILITY_XAUDIO2 =0x896;
    public const uint  XAUDIO2_E_INVALID_CALL                       = (0x88960001);   // An API call or one of its arguments was illegal
    public const uint  XAUDIO2_E_XMA_DECODER_ERROR                  =(0x88960002) ;   // The XMA hardware suffered an unrecoverable error
    public const uint  XAUDIO2_E_XAPO_CREATION_FAILED               =(0x88960003) ;   // XAudio2 failed to initialize an XAPO effect
    public const uint  XAUDIO2_E_DEVICE_INVALIDATED                 =(0x88960004) ;   // An audio device became unusable (unplugged, etc)

    public const uint  Processor1  =0x00000001;
    public const uint  Processor2  =0x00000002;
    public const uint  Processor3  =0x00000004;
    public const uint  Processor4  =0x00000008;
    public const uint  Processor5  =0x00000010;
    public const uint  Processor6  =0x00000020;
    public const uint  Processor7  =0x00000040;
    public const uint  Processor8  =0x00000080;
    public const uint  Processor9  =0x00000100;
    public const uint  Processor10 =0x00000200;
    public const uint  Processor11 =0x00000400;
    public const uint  Processor12 =0x00000800;
    public const uint  Processor13 =0x00001000;
    public const uint  Processor14 =0x00002000;
    public const uint  Processor15 =0x00004000;
    public const uint  Processor16 =0x00008000;
    public const uint  Processor17 =0x00010000;
    public const uint  Processor18 =0x00020000;
    public const uint  Processor19 =0x00040000;
    public const uint  Processor20 =0x00080000;
    public const uint  Processor21 =0x00100000;
    public const uint  Processor22 =0x00200000;
    public const uint  Processor23 =0x00400000;
    public const uint  Processor24 =0x00800000;
    public const uint  Processor25 =0x01000000;
    public const uint  Processor26 =0x02000000;
    public const uint  Processor27 =0x04000000;
    public const uint  Processor28 =0x08000000;
    public const uint  Processor29 =0x10000000;
    public const uint  Processor30 =0x20000000;
    public const uint  Processor31 =0x40000000;
    public const uint  Processor32 =0x80000000;
    public const uint  XAUDIO2_ANY_PROCESSOR =0xffffffff;

    // This value indicates that XAudio2 will choose the default processor by itself. The actual value chosen
    // may vary depending on the hardware platform.
    public const uint XAUDIO2_USE_DEFAULT_PROCESSOR =0x00000000;

    // This definition is included for backwards compatibilty. New implementations should use
    // XAUDIO2_USE_DEFAULT_PROCESSOR instead to let XAudio2 select the appropriate default processor for the hardware platform.
    public const uint XAUDIO2_DEFAULT_PROCESSOR =Processor1;

    // Values for the TraceMask and BreakMask bitmaps.  Only ERRORS and WARNINGS
    // are valid in BreakMask.  WARNINGS implies ERRORS, DETAIL implies INFO, and
    // FUNC_CALLS implies API_CALLS.  By default, TraceMask is ERRORS and WARNINGS
    // and all the other settings are zero.
    public const uint XAUDIO2_LOG_ERRORS     = 0x0001;   // For handled errors with serious effects.
    public const uint XAUDIO2_LOG_WARNINGS   = 0x0002;   // For handled errors that may be recoverable.
    public const uint XAUDIO2_LOG_INFO       = 0x0004;   // Informational chit-chat (e.g. state changes).
    public const uint XAUDIO2_LOG_DETAIL     = 0x0008;   // More detailed chit-chat.
    public const uint XAUDIO2_LOG_API_CALLS  = 0x0010;   // Public API function entries and exits.
    public const uint XAUDIO2_LOG_FUNC_CALLS = 0x0020;   // public function entries and exits.
    public const uint XAUDIO2_LOG_TIMING     = 0x0040;   // Delays detected and other timing data.
    public const uint XAUDIO2_LOG_LOCKS      = 0x0080;   // Usage of critical sections and mutexes.
    public const uint XAUDIO2_LOG_MEMORY     = 0x0100;   // Memory heap usage information.
    public const uint XAUDIO2_LOG_STREAMING  = 0x1000;   // Audio streaming information.

    // speaker geometry configuration flags, specifies assignment of channels to speaker positions, defined as per WAVEFORMATEXTENSIBLE.dwChannelMask
    public const bool _SPEAKER_POSITIONS_ = true;
    public const uint SPEAKER_FRONT_LEFT            =0x00000001;
    public const uint SPEAKER_FRONT_RIGHT           =0x00000002;
    public const uint SPEAKER_FRONT_CENTER          =0x00000004;
    public const uint SPEAKER_LOW_FREQUENCY         =0x00000008;
    public const uint SPEAKER_BACK_LEFT             =0x00000010;
    public const uint SPEAKER_BACK_RIGHT            =0x00000020;
    public const uint SPEAKER_FRONT_LEFT_OF_CENTER  =0x00000040;
    public const uint SPEAKER_FRONT_RIGHT_OF_CENTER =0x00000080;
    public const uint SPEAKER_BACK_CENTER           =0x00000100;
    public const uint SPEAKER_SIDE_LEFT             =0x00000200;
    public const uint SPEAKER_SIDE_RIGHT            =0x00000400;
    public const uint SPEAKER_TOP_CENTER            =0x00000800;
    public const uint SPEAKER_TOP_FRONT_LEFT        =0x00001000;
    public const uint SPEAKER_TOP_FRONT_CENTER      =0x00002000;
    public const uint SPEAKER_TOP_FRONT_RIGHT       =0x00004000;
    public const uint SPEAKER_TOP_BACK_LEFT         =0x00008000;
    public const uint SPEAKER_TOP_BACK_CENTER       =0x00010000;
    public const uint SPEAKER_TOP_BACK_RIGHT        =0x00020000;
    public const uint SPEAKER_RESERVED              =0x7FFC0000; // bit mask locations reserved for future use
    public const uint SPEAKER_ALL                   =0x80000000; // used to specify that any possible permutation of speaker configurations

    #endregion

#region XAUDIO_FX


    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_DEFAULT
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0xF0, 0xD8, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x80, 0x3F,
                    0x00, 0x00, 0x00, 0x3F,
                    0xF0, 0xD8, 0xFF, 0xFF,
                    0x0A, 0xD7, 0xA3, 0x3C,
                    0xF0, 0xD8, 0xFF, 0xFF,
                    0x0A, 0xD7, 0x23, 0x3D,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_GENERIC
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x9C, 0xFF, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x52, 0xB8, 0xBE, 0x3F,
                    0xE1, 0x7A, 0x54, 0x3F,
                    0xD6, 0xF5, 0xFF, 0xFF,
                    0x42, 0x60, 0xE5, 0x3B,
                    0xC8, 0x00, 0x00, 0x00,
                    0x58, 0x39, 0x34, 0x3C,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_PADDEDCELL
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x90, 0xE8, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x7B, 0x14, 0x2E, 0x3E,
                    0xCD, 0xCC, 0xCC, 0x3D,
                    0x4C, 0xFB, 0xFF, 0xFF,
                    0x6F, 0x12, 0x83, 0x3A,
                    0xCF, 0x00, 0x00, 0x00,
                    0x6F, 0x12, 0x03, 0x3B,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_ROOM
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x3A, 0xFE, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0xCD, 0xCC, 0xCC, 0x3E,
                    0xE1, 0x7A, 0x54, 0x3F,
                    0x92, 0xF9, 0xFF, 0xFF,
                    0x6F, 0x12, 0x03, 0x3B,
                    0x35, 0x00, 0x00, 0x00,
                    0xA6, 0x9B, 0x44, 0x3B,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_BATHROOM
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x50, 0xFB, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x52, 0xB8, 0xBE, 0x3F,
                    0x71, 0x3D, 0x0A, 0x3F,
                    0x8E, 0xFE, 0xFF, 0xFF,
                    0x42, 0x60, 0xE5, 0x3B,
                    0x06, 0x04, 0x00, 0x00,
                    0x58, 0x39, 0x34, 0x3C,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0x70, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_LIVINGROOM
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x90, 0xE8, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x3F,
                    0xCD, 0xCC, 0xCC, 0x3D,
                    0xA0, 0xFA, 0xFF, 0xFF,
                    0xA6, 0x9B, 0x44, 0x3B,
                    0xB0, 0xFB, 0xFF, 0xFF,
                    0x6F, 0x12, 0x83, 0x3B,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_STONEROOM
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0xD4, 0xFE, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x0A, 0xD7, 0x13, 0x40,
                    0x0A, 0xD7, 0x23, 0x3F,
                    0x39, 0xFD, 0xFF, 0xFF,
                    0xA6, 0x9B, 0x44, 0x3C,
                    0x53, 0x00, 0x00, 0x00,
                    0x96, 0x43, 0x8B, 0x3C,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_AUDITORIUM
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x24, 0xFE, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x71, 0x3D, 0x8A, 0x40,
                    0x3D, 0x0A, 0x17, 0x3F,
                    0xEB, 0xFC, 0xFF, 0xFF,
                    0x0A, 0xD7, 0xA3, 0x3C,
                    0xDF, 0xFE, 0xFF, 0xFF,
                    0x8F, 0xC2, 0xF5, 0x3C,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_CONCERTHALL
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x0C, 0xFE, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x48, 0xE1, 0x7A, 0x40,
                    0x33, 0x33, 0x33, 0x3F,
                    0x32, 0xFB, 0xFF, 0xFF,
                    0x0A, 0xD7, 0xA3, 0x3C,
                    0xFE, 0xFF, 0xFF, 0xFF,
                    0x68, 0x91, 0xED, 0x3C,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_CAVE
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00,
                    0x71, 0x3D, 0x3A, 0x40,
                    0x66, 0x66, 0xA6, 0x3F,
                    0xA6, 0xFD, 0xFF, 0xFF,
                    0x8F, 0xC2, 0x75, 0x3C,
                    0xD2, 0xFE, 0xFF, 0xFF,
                    0x58, 0x39, 0xB4, 0x3C,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_ARENA
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x46, 0xFD, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x14, 0xAE, 0xE7, 0x40,
                    0xC3, 0xF5, 0xA8, 0x3E,
                    0x72, 0xFB, 0xFF, 0xFF,
                    0x0A, 0xD7, 0xA3, 0x3C,
                    0x10, 0x00, 0x00, 0x00,
                    0x8F, 0xC2, 0xF5, 0x3C,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_HANGAR
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0xCD, 0xCC, 0x20, 0x41,
                    0x1F, 0x85, 0x6B, 0x3E,
                    0xA6, 0xFD, 0xFF, 0xFF,
                    0x0A, 0xD7, 0xA3, 0x3C,
                    0xC6, 0x00, 0x00, 0x00,
                    0x8F, 0xC2, 0xF5, 0x3C,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_CARPETEDHALLWAY
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x60, 0xF0, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x9A, 0x99, 0x99, 0x3E,
                    0xCD, 0xCC, 0xCC, 0x3D,
                    0xD9, 0xF8, 0xFF, 0xFF,
                    0x6F, 0x12, 0x03, 0x3B,
                    0xA2, 0xF9, 0xFF, 0xFF,
                    0x8F, 0xC2, 0xF5, 0x3C,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_HALLWAY
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0xD4, 0xFE, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x52, 0xB8, 0xBE, 0x3F,
                    0x3D, 0x0A, 0x17, 0x3F,
                    0x3D, 0xFB, 0xFF, 0xFF,
                    0x42, 0x60, 0xE5, 0x3B,
                    0xB9, 0x01, 0x00, 0x00,
                    0x58, 0x39, 0x34, 0x3C,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_STONECORRIDOR
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x13, 0xFF, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0xCD, 0xCC, 0x2C, 0x40,
                    0x71, 0x3D, 0x4A, 0x3F,
                    0x42, 0xFB, 0xFF, 0xFF,
                    0xF4, 0xFD, 0x54, 0x3C,
                    0x8B, 0x01, 0x00, 0x00,
                    0x0A, 0xD7, 0xA3, 0x3C,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_ALLEY
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0xF2, 0xFE, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x52, 0xB8, 0xBE, 0x3F,
                    0xF6, 0x28, 0x5C, 0x3F,
                    0x4C, 0xFB, 0xFF, 0xFF,
                    0x42, 0x60, 0xE5, 0x3B,
                    0xFC, 0xFF, 0xFF, 0xFF,
                    0x58, 0x39, 0x34, 0x3C,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_FOREST
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x1C, 0xF3, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x52, 0xB8, 0xBE, 0x3F,
                    0x71, 0x3D, 0x0A, 0x3F,
                    0x00, 0xF6, 0xFF, 0xFF,
                    0x54, 0xE3, 0x25, 0x3E,
                    0x9B, 0xFD, 0xFF, 0xFF,
                    0x58, 0x39, 0xB4, 0x3D,
                    0x00, 0x00, 0x9E, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_CITY
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0xE0, 0xFC, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x52, 0xB8, 0xBE, 0x3F,
                    0x1F, 0x85, 0x2B, 0x3F,
                    0x1F, 0xF7, 0xFF, 0xFF,
                    0x42, 0x60, 0xE5, 0x3B,
                    0x57, 0xF7, 0xFF, 0xFF,
                    0x58, 0x39, 0x34, 0x3C,
                    0x00, 0x00, 0x48, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_MOUNTAINS
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x3C, 0xF6, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x52, 0xB8, 0xBE, 0x3F,
                    0x3D, 0x0A, 0x57, 0x3E,
                    0x24, 0xF5, 0xFF, 0xFF,
                    0x9A, 0x99, 0x99, 0x3E,
                    0x22, 0xF8, 0xFF, 0xFF,
                    0xCD, 0xCC, 0xCC, 0x3D,
                    0x00, 0x00, 0xD8, 0x41,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_QUARRY
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x52, 0xB8, 0xBE, 0x3F,
                    0xE1, 0x7A, 0x54, 0x3F,
                    0xF0, 0xD8, 0xFF, 0xFF,
                    0x23, 0xDB, 0x79, 0x3D,
                    0xF4, 0x01, 0x00, 0x00,
                    0xCD, 0xCC, 0xCC, 0x3C,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_PLAIN
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x30, 0xF8, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x52, 0xB8, 0xBE, 0x3F,
                    0x00, 0x00, 0x00, 0x3F,
                    0x5E, 0xF6, 0xFF, 0xFF,
                    0xC7, 0x4B, 0x37, 0x3E,
                    0x2E, 0xF6, 0xFF, 0xFF,
                    0xCD, 0xCC, 0xCC, 0x3D,
                    0x00, 0x00, 0xA8, 0x41,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_PARKINGLOT
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00,
                    0x33, 0x33, 0xD3, 0x3F,
                    0x00, 0x00, 0xC0, 0x3F,
                    0xAD, 0xFA, 0xFF, 0xFF,
                    0x6F, 0x12, 0x03, 0x3C,
                    0x7F, 0xFB, 0xFF, 0xFF,
                    0xA6, 0x9B, 0x44, 0x3C,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_SEWERPIPE
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x0A, 0xD7, 0x33, 0x40,
                    0x29, 0x5C, 0x0F, 0x3E,
                    0xAD, 0x01, 0x00, 0x00,
                    0x42, 0x60, 0x65, 0x3C,
                    0x88, 0x02, 0x00, 0x00,
                    0x31, 0x08, 0xAC, 0x3C,
                    0x00, 0x00, 0xA0, 0x42,
                    0x00, 0x00, 0x70, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_UNDERWATER
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x60, 0xF0, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x52, 0xB8, 0xBE, 0x3F,
                    0xCD, 0xCC, 0xCC, 0x3D,
                    0x3F, 0xFE, 0xFF, 0xFF,
                    0x42, 0x60, 0xE5, 0x3B,
                    0xA4, 0x06, 0x00, 0x00,
                    0x58, 0x39, 0x34, 0x3C,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_SMALLROOM
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0xA8, 0xFD, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0xCD, 0xCC, 0x8C, 0x3F,
                    0xE1, 0x7A, 0x54, 0x3F,
                    0x70, 0xFE, 0xFF, 0xFF,
                    0x0A, 0xD7, 0xA3, 0x3B,
                    0xF4, 0x01, 0x00, 0x00,
                    0x0A, 0xD7, 0x23, 0x3C,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_MEDIUMROOM
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0xA8, 0xFD, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x66, 0x66, 0xA6, 0x3F,
                    0xE1, 0x7A, 0x54, 0x3F,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x0A, 0xD7, 0x23, 0x3C,
                    0x38, 0xFF, 0xFF, 0xFF,
                    0x0A, 0xD7, 0xA3, 0x3C,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_LARGEROOM
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0xA8, 0xFD, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0xC0, 0x3F,
                    0xE1, 0x7A, 0x54, 0x3F,
                    0xC0, 0xF9, 0xFF, 0xFF,
                    0x0A, 0xD7, 0xA3, 0x3C,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x0A, 0xD7, 0x23, 0x3D,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_MEDIUMHALL
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0xA8, 0xFD, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x66, 0x66, 0xE6, 0x3F,
                    0x33, 0x33, 0x33, 0x3F,
                    0xEC, 0xFA, 0xFF, 0xFF,
                    0x8F, 0xC2, 0x75, 0x3C,
                    0xE0, 0xFC, 0xFF, 0xFF,
                    0x8F, 0xC2, 0xF5, 0x3C,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_LARGEHALL
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0xA8, 0xFD, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x66, 0x66, 0xE6, 0x3F,
                    0x33, 0x33, 0x33, 0x3F,
                    0x30, 0xF8, 0xFF, 0xFF,
                    0x8F, 0xC2, 0xF5, 0x3C,
                    0x88, 0xFA, 0xFF, 0xFF,
                    0x8F, 0xC2, 0x75, 0x3D,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public static ref readonly XAUDIO2FX_REVERB_I3DL2_PARAMETERS XAUDIO2FX_I3DL2_PRESET_PLATE
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                    0x00, 0x00, 0xC8, 0x42,
                    0x18, 0xFC, 0xFF, 0xFF,
                    0x38, 0xFF, 0xFF, 0xFF,
                    0x00, 0x00, 0x00, 0x00,
                    0x66, 0x66, 0xA6, 0x3F,
                    0x66, 0x66, 0x66, 0x3F,
                    0x00, 0x00, 0x00, 0x00,
                    0x6F, 0x12, 0x03, 0x3B,
                    0x00, 0x00, 0x00, 0x00,
                    0x0A, 0xD7, 0x23, 0x3C,
                    0x00, 0x00, 0xC8, 0x42,
                    0x00, 0x00, 0x96, 0x42,
                    0x00, 0x40, 0x9C, 0x45
                };

            return ref Unsafe.As<byte, XAUDIO2FX_REVERB_I3DL2_PARAMETERS>(ref MemoryMarshal.GetReference(data));
        }
    }

    public const int XAUDIO2FX_REVERB_MIN_FRAMERATE =20000;
    public const int XAUDIO2FX_REVERB_MAX_FRAMERATE =48000;
    public const float  XAUDIO2FX_REVERB_MIN_WET_DRY_MIX=            0.0f;
    public const float  XAUDIO2FX_REVERB_MIN_REFLECTIONS_DELAY=      0;
    public const float  XAUDIO2FX_REVERB_MIN_REVERB_DELAY=           0;
    public const float  XAUDIO2FX_REVERB_MIN_REAR_DELAY=             0;
    public const float  XAUDIO2FX_REVERB_MIN_POSITION=               0;
    public const float  XAUDIO2FX_REVERB_MIN_DIFFUSION=              0;
    public const float  XAUDIO2FX_REVERB_MIN_LOW_EQ_GAIN=            0;
    public const float  XAUDIO2FX_REVERB_MIN_LOW_EQ_CUTOFF=          0;
    public const float  XAUDIO2FX_REVERB_MIN_HIGH_EQ_GAIN=           0;
    public const float  XAUDIO2FX_REVERB_MIN_HIGH_EQ_CUTOFF=         0;
    public const float  XAUDIO2FX_REVERB_MIN_ROOM_FILTER_FREQ=       20.0f;
    public const float  XAUDIO2FX_REVERB_MIN_ROOM_FILTER_MAIN=       -100.0f;
    public const float  XAUDIO2FX_REVERB_MIN_ROOM_FILTER_HF=         -100.0f;
    public const float  XAUDIO2FX_REVERB_MIN_REFLECTIONS_GAIN=       -100.0f;
    public const float  XAUDIO2FX_REVERB_MIN_REVERB_GAIN=            -100.0f;
    public const float  XAUDIO2FX_REVERB_MIN_DECAY_TIME=             0.1f;
    public const float  XAUDIO2FX_REVERB_MIN_DENSITY=                0.0f;
    public const float  XAUDIO2FX_REVERB_MIN_ROOM_SIZE=              0.0f;
    public const float  XAUDIO2FX_REVERB_MAX_WET_DRY_MIX=            100.0f;
    public const float  XAUDIO2FX_REVERB_MAX_REFLECTIONS_DELAY=      300;
    public const float  XAUDIO2FX_REVERB_MAX_REVERB_DELAY=           85;
    public const float  XAUDIO2FX_REVERB_MAX_REAR_DELAY=             5;
    public const float  XAUDIO2FX_REVERB_MAX_POSITION=               30;
    public const float  XAUDIO2FX_REVERB_MAX_DIFFUSION=              15;
    public const float  XAUDIO2FX_REVERB_MAX_LOW_EQ_GAIN=            12;
    public const float  XAUDIO2FX_REVERB_MAX_LOW_EQ_CUTOFF=          9;
    public const float  XAUDIO2FX_REVERB_MAX_HIGH_EQ_GAIN=           8;
    public const float  XAUDIO2FX_REVERB_MAX_HIGH_EQ_CUTOFF=         14;
    public const float  XAUDIO2FX_REVERB_MAX_ROOM_FILTER_FREQ=       20000.0f;
    public const float  XAUDIO2FX_REVERB_MAX_ROOM_FILTER_MAIN=       0.0f;
    public const float  XAUDIO2FX_REVERB_MAX_ROOM_FILTER_HF=         0.0f;
    public const float  XAUDIO2FX_REVERB_MAX_REFLECTIONS_GAIN=       20.0f;
    public const float  XAUDIO2FX_REVERB_MAX_REVERB_GAIN=            20.0f;
    public const float  XAUDIO2FX_REVERB_MAX_DENSITY=                100.0f;
    public const float  XAUDIO2FX_REVERB_MAX_ROOM_SIZE=              100.0f;
    public const float  XAUDIO2FX_REVERB_DEFAULT_WET_DRY_MIX=        100.0f;
    public const float  XAUDIO2FX_REVERB_DEFAULT_REFLECTIONS_DELAY=  5;
    public const float  XAUDIO2FX_REVERB_DEFAULT_REVERB_DELAY=       5;
    public const float  XAUDIO2FX_REVERB_DEFAULT_REAR_DELAY=         5;
    public const float  XAUDIO2FX_REVERB_DEFAULT_POSITION=           6;
    public const float  XAUDIO2FX_REVERB_DEFAULT_POSITION_MATRIX=    27;
    public const float  XAUDIO2FX_REVERB_DEFAULT_EARLY_DIFFUSION=    8;
    public const float  XAUDIO2FX_REVERB_DEFAULT_LATE_DIFFUSION=     8;
    public const float  XAUDIO2FX_REVERB_DEFAULT_LOW_EQ_GAIN=        8;
    public const float  XAUDIO2FX_REVERB_DEFAULT_LOW_EQ_CUTOFF=      4;
    public const float  XAUDIO2FX_REVERB_DEFAULT_HIGH_EQ_GAIN=       8;
    public const float  XAUDIO2FX_REVERB_DEFAULT_HIGH_EQ_CUTOFF=     4;
    public const float  XAUDIO2FX_REVERB_DEFAULT_ROOM_FILTER_FREQ=   5000.0f;
    public const float  XAUDIO2FX_REVERB_DEFAULT_ROOM_FILTER_MAIN=   0.0f;
    public const float  XAUDIO2FX_REVERB_DEFAULT_ROOM_FILTER_HF=     0.0f;
    public const float  XAUDIO2FX_REVERB_DEFAULT_REFLECTIONS_GAIN=   0.0f;
    public const float  XAUDIO2FX_REVERB_DEFAULT_REVERB_GAIN=        0.0f;
    public const float  XAUDIO2FX_REVERB_DEFAULT_DECAY_TIME=         1.0f;
    public const float  XAUDIO2FX_REVERB_DEFAULT_DENSITY=            100.0f;
    public const float  XAUDIO2FX_REVERB_DEFAULT_ROOM_SIZE=          100.0f;

#endregion

#region X3DAUDIO


    //https://github.com/terrafx/terrafx.interop.windows/blob/main/sources/Interop/Windows/DirectX/um/x3daudio/DirectX.cs
    public static ReadOnlySpan<X3DAUDIO_DISTANCE_CURVE_POINT> X3DAudioDefault_LinearCurvePoints
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x80, 0x3F,
                0x00, 0x00, 0x80, 0x3F,
                0x00, 0x00, 0x00, 0x00
            };

            // Debug.Assert(data.Length == (Unsafe.SizeOf<X3DAUDIO_DISTANCE_CURVE_POINT>() * 2));
            return MemoryMarshal.CreateReadOnlySpan<X3DAUDIO_DISTANCE_CURVE_POINT>(ref Unsafe.As<byte, X3DAUDIO_DISTANCE_CURVE_POINT>(ref MemoryMarshal.GetReference(data)), 2);
        }
    }

    public static ref readonly X3DAUDIO_CONE X3DAudioDefault_DirectionalCone
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ReadOnlySpan<byte> data = new byte[] {
                0xDB, 0x0F, 0xC9, 0x3F,
                0xDB, 0x0F, 0x49, 0x40,
                0x00, 0x00, 0x80, 0x3F,
                0x7D, 0x3F, 0x35, 0x3F,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x80, 0x3E,
                0x7D, 0x3F, 0x35, 0x3F,
                0x00, 0x00, 0x80, 0x3F
            };

            // Debug.Assert(data.Length == Unsafe.SizeOf<X3DAUDIO_CONE>());
            return ref Unsafe.As<byte, X3DAUDIO_CONE>(ref MemoryMarshal.GetReference(data));
        }
    }

        // standard speaker geometry configurations, used with X3DAudioInitialize
    public const uint X3DAUDIO_SPEAKER_MONO             =SPEAKER_FRONT_CENTER;
    public const uint X3DAUDIO_SPEAKER_STEREO           =(SPEAKER_FRONT_LEFT | SPEAKER_FRONT_RIGHT);
    public const uint X3DAUDIO_SPEAKER_2POINT1          =(SPEAKER_FRONT_LEFT | SPEAKER_FRONT_RIGHT | SPEAKER_LOW_FREQUENCY);
    public const uint X3DAUDIO_SPEAKER_SURROUND         =(SPEAKER_FRONT_LEFT | SPEAKER_FRONT_RIGHT | SPEAKER_FRONT_CENTER | SPEAKER_BACK_CENTER);
    public const uint X3DAUDIO_SPEAKER_QUAD             =(SPEAKER_FRONT_LEFT | SPEAKER_FRONT_RIGHT | SPEAKER_BACK_LEFT | SPEAKER_BACK_RIGHT);
    public const uint X3DAUDIO_SPEAKER_4POINT1          =(SPEAKER_FRONT_LEFT | SPEAKER_FRONT_RIGHT | SPEAKER_LOW_FREQUENCY | SPEAKER_BACK_LEFT | SPEAKER_BACK_RIGHT);
    public const uint X3DAUDIO_SPEAKER_5POINT1          =(SPEAKER_FRONT_LEFT | SPEAKER_FRONT_RIGHT | SPEAKER_FRONT_CENTER | SPEAKER_LOW_FREQUENCY | SPEAKER_BACK_LEFT | SPEAKER_BACK_RIGHT);
    public const uint X3DAUDIO_SPEAKER_7POINT1          =(SPEAKER_FRONT_LEFT | SPEAKER_FRONT_RIGHT | SPEAKER_FRONT_CENTER | SPEAKER_LOW_FREQUENCY | SPEAKER_BACK_LEFT | SPEAKER_BACK_RIGHT | SPEAKER_FRONT_LEFT_OF_CENTER | SPEAKER_FRONT_RIGHT_OF_CENTER);
    public const uint X3DAUDIO_SPEAKER_5POINT1_SURROUND =(SPEAKER_FRONT_LEFT | SPEAKER_FRONT_RIGHT | SPEAKER_FRONT_CENTER | SPEAKER_LOW_FREQUENCY | SPEAKER_SIDE_LEFT  | SPEAKER_SIDE_RIGHT);
    public const uint X3DAUDIO_SPEAKER_7POINT1_SURROUND =(SPEAKER_FRONT_LEFT | SPEAKER_FRONT_RIGHT | SPEAKER_FRONT_CENTER | SPEAKER_LOW_FREQUENCY | SPEAKER_BACK_LEFT | SPEAKER_BACK_RIGHT | SPEAKER_SIDE_LEFT  | SPEAKER_SIDE_RIGHT);

    // size of instance handle in bytes
    public const uint X3DAUDIO_HANDLE_BYTESIZE =20;

    // float math constants
    public const float X3DAUDIO_PI  =3.141592654f;
    public const float X3DAUDIO_2PI =6.283185307f;

    // speed of sound in meters per second for dry air at approximately 20C, used with X3DAudioInitialize
    public const float X3DAUDIO_SPEED_OF_SOUND =343.5f;

    // calculation control flags, used with X3DAudioCalculate
    public const uint X3DAUDIO_CALCULATE_MATRIX          =0x00000001; // enable matrix coefficient table calculation
    public const uint X3DAUDIO_CALCULATE_DELAY           =0x00000002; // enable delay time array calculation (stereo final mix only)
    public const uint X3DAUDIO_CALCULATE_LPF_DIRECT      =0x00000004; // enable LPF direct-path coefficient calculation
    public const uint X3DAUDIO_CALCULATE_LPF_REVERB      =0x00000008; // enable LPF reverb-path coefficient calculation
    public const uint X3DAUDIO_CALCULATE_REVERB          =0x00000010; // enable reverb send level calculation
    public const uint X3DAUDIO_CALCULATE_DOPPLER         =0x00000020; // enable doppler shift factor calculation
    public const uint X3DAUDIO_CALCULATE_EMITTER_ANGLE   =0x00000040; // enable emitter-to-listener interior angle calculation
    public const uint X3DAUDIO_CALCULATE_ZEROCENTER      =0x00010000; // do not position to front center speaker, signal positioned to remaining speakers instead, front center destination channel will be zero in returned matrix coefficient table, valid only for matrix calculations with final mix formats that have a front center channel
    public const uint X3DAUDIO_CALCULATE_REDIRECT_TO_LFE =0x00020000; // apply equal mix of all source channels to LFE destination channel, valid only for matrix calculations with sources that have no LFE channel and final mix formats that have an LFE channel

#endregion
}

public enum AUDIO_STREAM_CATEGORY
{
    AudioCategory_Other = 0,
    AudioCategory_ForegroundOnlyMedia = 1,
    AudioCategory_Communications = 3,
    AudioCategory_Alerts = 4,
    AudioCategory_SoundEffects = 5,
    AudioCategory_GameEffects = 6,
    AudioCategory_GameMedia = 7,
    AudioCategory_GameChat = 8,
    AudioCategory_Speech = 9,
    AudioCategory_Movie = 10,
    AudioCategory_Media = 11,
} 

#region STRUCTURES

[StructLayout(LayoutKind.Explicit, Size = 20)] public struct X3DAUDIO_HANDLE { }

[StructLayout(LayoutKind.Sequential)]
public struct XAUDIO2_VOICE_DETAILS
{
    public uint CreationFlags;               // Flags the voice was created with.
    public uint ActiveFlags;                 // Flags currently active.
    public uint InputChannels;               // Channels in the voice's input audio.
    public uint InputSampleRate;             // Sample rate of the voice's input audio.
};

// Used in XAUDIO2_VOICE_SENDS below
[StructLayout(LayoutKind.Sequential)]
public struct XAUDIO2_SEND_DESCRIPTOR
{
    public uint Flags;                       // Either 0 or XAUDIO2_SEND_USEFILTER.
    public IXAudio2Voice pOutputVoice;        // This send's destination voice.
} 

// Used in the voice creation functions and in IXAudio2Voice::SetOutputVoices
public struct XAUDIO2_VOICE_SENDS
{
    public uint SendCount;                   // Number of sends from this voice.
    public XAUDIO2_SEND_DESCRIPTOR pSends;    // Array of SendCount send descriptors.
} 

// Used in XAUDIO2_EFFECT_CHAIN below
[StructLayout(LayoutKind.Sequential)]
public unsafe struct XAUDIO2_EFFECT_DESCRIPTOR
{
    public void* pEffect;                  // Pointer to the effect object's IUnknown interface.
    public int InitialState;                  // TRUE if the effect should begin in the enabled state.
    public uint OutputChannels;              // How many output channels the effect should produce.
} 

// Used in the voice creation functions and in IXAudio2Voice::SetEffectChain
    [StructLayout(LayoutKind.Sequential)]
public unsafe struct XAUDIO2_EFFECT_CHAIN
{
    public uint EffectCount;                 // Number of effects in this voice's effect chain.
    public XAUDIO2_EFFECT_DESCRIPTOR* pEffectDescriptors; // Array of effect descriptors.
} 

// Used in XAUDIO2_FILTER_PARAMETERS below
public enum XAUDIO2_FILTER_TYPE
{
    LowPassFilter,                      // Attenuates frequencies above the cutoff frequency (state-variable filter).
    BandPassFilter,                     // Attenuates frequencies outside a given range      (state-variable filter).
    HighPassFilter,                     // Attenuates frequencies below the cutoff frequency (state-variable filter).
    NotchFilter,                        // Attenuates frequencies inside a given range       (state-variable filter).
    LowPassOnePoleFilter,               // Attenuates frequencies above the cutoff frequency (one-pole filter, XAUDIO2_FILTER_PARAMETERS.OneOverQ has no effect)
    HighPassOnePoleFilter               // Attenuates frequencies below the cutoff frequency (one-pole filter, XAUDIO2_FILTER_PARAMETERS.OneOverQ has no effect)
} 

// Used in IXAudio2Voice::Set/GetFilterParameters and Set/GetOutputFilterParameters
public unsafe struct XAUDIO2_FILTER_PARAMETERS
{
    public XAUDIO2_FILTER_TYPE Type;           // Filter type.
    public float Frequency;                    // Filter coefficient. must be >= 0 and <= XAUDIO2_MAX_FILTER_FREQUENCY See XAudio2CutoffFrequencyToRadians() for state-variable filter types and
                                        //  XAudio2CutoffFrequencyToOnePoleCoefficient() for one-pole filter types.
    public float OneOverQ;                     // Reciprocal of the filter's quality factor Q; must be > 0 and <= XAUDIO2_MAX_FILTER_ONEOVERQ. Has no effect for one-pole filters.
} 

// Used in IXAudio2SourceVoice::SubmitSourceBuffer
public unsafe struct XAUDIO2_BUFFER
{
    public uint Flags;                       // Either 0 or XAUDIO2_END_OF_STREAM.
    public uint AudioBytes;                  // Size of the audio data buffer in bytes.
    public byte* pAudioData;             // Pointer to the audio data buffer.
    public uint PlayBegin;                   // First sample in this buffer to be played.
    public uint PlayLength;                  // Length of the region to be played in samples,
                                //  or 0 to play the whole buffer.
    public uint LoopBegin;                   // First sample of the region to be looped.
    public uint LoopLength;                  // Length of the desired loop region in samples,
                                //  or 0 to loop the entire buffer.
    public uint LoopCount;                   // Number of times to repeat the loop region,
                                //  or XAUDIO2_LOOP_INFINITE to loop forever.
    public void* pContext;                     // Context value to be passed back in callbacks.
} 

// Used in IXAudio2SourceVoice::SubmitSourceBuffer when submitting XWMA data.
// NOTE: If an XWMA sound is submitted in more than one buffer, each buffer's
// pDecodedPacketCumulativeBytes[PacketCount-1] value must be subtracted from
// all the entries in the next buffer's pDecodedPacketCumulativeBytes array.
// And whether a sound is submitted in more than one buffer or not, the final
// buffer of the sound should use the XAUDIO2_END_OF_STREAM flag, or else the
// client must call IXAudio2SourceVoice::Discontinuity after submitting it.
public unsafe struct XAUDIO2_BUFFER_WMA
{
    public uint* pDecodedPacketCumulativeBytes; // Decoded packet's cumulative size array.
                                                //  Each element is the number of bytes accumulated
                                                //  when the corresponding XWMA packet is decoded in
                                                //  order.  The array must have PacketCount elements.
    public uint PacketCount;                          // Number of XWMA packets submitted. Must be >= 1 and
                                                //  divide evenly into XAUDIO2_BUFFER.AudioBytes.
} 

// Returned by IXAudio2SourceVoice::GetState
public unsafe struct XAUDIO2_VOICE_STATE
{
    public void* pCurrentBufferContext;        // The pContext value provided in the XAUDIO2_BUFFER
                                        //  that is currently being processed, or NULL if
                                        //  there are no buffers in the queue.
    public uint BuffersQueued;               // Number of buffers currently queued on the voice
                                        //  (including the one that is being processed).
    public ulong SamplesPlayed;               // Total number of samples produced by the voice since
                                        //  it began processing the current audio stream.
                                        //  If XAUDIO2_VOICE_NOSAMPLESPLAYED is specified
                                        //  in the call to IXAudio2SourceVoice::GetState,
                                        //  this member will not be calculated, saving CPU.
} 

// Returned by IXAudio2::GetPerformanceData
[StructLayout(LayoutKind.Sequential,Pack =1)]
public struct XAUDIO2_PERFORMANCE_DATA
{
    // CPU usage information
    public ulong AudioCyclesSinceLastQuery;   // CPU cycles spent on audio processing since the
                                        //  last call to StartEngine or GetPerformanceData.
    public ulong TotalCyclesSinceLastQuery;   // Total CPU cycles elapsed since the last call
                                        //  (only counts the CPU XAudio2 is running on).
    public uint MinimumCyclesPerQuantum;     // Fewest CPU cycles spent processing any one
                                        //  audio quantum since the last call.
    public uint MaximumCyclesPerQuantum;     // Most CPU cycles spent processing any one
                                        //  audio quantum since the last call.

    // Memory usage information
    public uint MemoryUsageInBytes;          // Total heap space currently in use.

    // Audio latency and glitching information
    public uint CurrentLatencyInSamples;     // Minimum delay from when a sample is read from a
                                        //  source buffer to when it reaches the speakers.
    public uint GlitchesSinceEngineStarted;  // Audio dropouts since the engine was started.

    // Data about XAudio2's current workload
    public uint ActiveSourceVoiceCount;      // Source voices currently playing.
    public uint TotalSourceVoiceCount;       // Source voices currently existing.
    public uint ActiveSubmixVoiceCount;      // Submix voices currently playing/existing.

    public uint ActiveResamplerCount;        // Resample xAPOs currently active.
    public uint ActiveMatrixMixCount;        // MatrixMix xAPOs currently active.

    // Usage of the hardware XMA decoder (Xbox 360 only)
    public uint ActiveXmaSourceVoices;       // Number of source voices decoding XMA data.
    public uint ActiveXmaStreams;            // A voice can use more than one XMA stream.
} 

// Used in IXAudio2::SetDebugConfiguration
public struct XAUDIO2_DEBUG_CONFIGURATION
{
    public uint TraceMask;                   // Bitmap of enabled debug message types.
    public uint BreakMask;                   // Message types that will break into the debugger.
    public int LogThreadID;                   // Whether to log the thread ID with each message.
    public int LogFileline;                   // Whether to log the source file and line number.
    public int LogFunctionName;               // Whether to log the function name.
    public int LogTiming;                     // Whether to log message timestamps.
};

[StructLayout(LayoutKind.Sequential)]
public unsafe struct IXAudio2
{
    public static readonly GUID IID_IXAudio2 = new( 0x2B02E3CF, 0x2E0B, 0x4ec3, 0xBE, 0x45, 0x1B, 0x2A, 0x3F, 0xE7, 0x21, 0x0D);
    private void* _lpVtbl = null;

    private delegate* unmanaged<void*, IXAudio2MasteringVoice**, uint, uint, uint, byte*, XAUDIO2_EFFECT_CHAIN*,AUDIO_STREAM_CATEGORY, HRESULT> PFN_CreateMasteringVoice=null;
    private delegate* unmanaged<void*, uint> PFN_Release=null;       
    private delegate* unmanaged<void*, XAUDIO2_PERFORMANCE_DATA*, void> PFN_GetPerformanceData=null;
    private delegate* unmanaged<void*,HRESULT> PFN_StartEngine=null;
    private delegate* unmanaged<void*,void> PFN_StopEngine=null;
    private delegate* unmanaged<void*, void**, WAVEFORMATEX*, uint, float, IXAudio2VoiceCallback*, XAUDIO2_VOICE_SENDS*, XAUDIO2_EFFECT_CHAIN*, uint> PFN_CreateSourceVoice=null;
    
    public IXAudio2(void* ixaudio2)
    {
        _lpVtbl = ixaudio2; 
        PFN_Release=(delegate* unmanaged<void*,uint>)  ((void**)(*(void**)_lpVtbl))[2] ;
        PFN_CreateSourceVoice=(delegate* unmanaged<void*, void**, WAVEFORMATEX*, uint, float, IXAudio2VoiceCallback*, XAUDIO2_VOICE_SENDS*, XAUDIO2_EFFECT_CHAIN*,uint>) ((void**)(*(void**)_lpVtbl))[5];
        PFN_CreateMasteringVoice=(delegate* unmanaged<void*, IXAudio2MasteringVoice**, uint, uint, uint, byte*,  XAUDIO2_EFFECT_CHAIN*, AUDIO_STREAM_CATEGORY,HRESULT>) ((void**)(*(void**)_lpVtbl))[7];
        PFN_StartEngine= (delegate* unmanaged<void*,HRESULT>)   ( (void**)(*(void**)_lpVtbl))[8] ; 
        PFN_StopEngine=(delegate* unmanaged<void*,void>) ( (void**)(*(void**)_lpVtbl) )[9];
        PFN_GetPerformanceData= (delegate* unmanaged<void*, XAUDIO2_PERFORMANCE_DATA*, void>)  ((void**)(*(void**)_lpVtbl))[11] ; 
    }

    public void Dispose()
    {
        PFN_CreateMasteringVoice=null;
        PFN_CreateSourceVoice=null;
        PFN_Release=null;
        PFN_GetPerformanceData=null;
        PFN_StartEngine=null;
        PFN_StopEngine=null;
        _lpVtbl = null;
    }

    public unsafe HRESULT CreateMasteringVoice( ref IXAudio2MasteringVoice ppMasteringVoice, uint InputChannels, uint InputSampleRate, uint Flags, byte* szDeviceId, XAUDIO2_EFFECT_CHAIN* pEffectChain, AUDIO_STREAM_CATEGORY StreamCategory)
    {
        IXAudio2MasteringVoice* result = null;
        uint ret = PFN_CreateMasteringVoice(_lpVtbl, &result, InputChannels, InputSampleRate, Flags, szDeviceId, pEffectChain, StreamCategory);
        ppMasteringVoice = new (result);
        return ret;
    }

    public unsafe void GetPerformanceData(ref XAUDIO2_PERFORMANCE_DATA perfomance)
    {
        fixed(   XAUDIO2_PERFORMANCE_DATA* perform = &perfomance ){
            PFN_GetPerformanceData(_lpVtbl, perform);  }
    }

    public unsafe HRESULT CreateSourceVoice(ref IXAudio2SourceVoice sourcevoice,WAVEFORMATEX sourceformat, uint Flags = 0,  float MaxFrequencyRatio = 1.0f, IXAudio2VoiceCallback* pCallback = null, XAUDIO2_VOICE_SENDS* pSendList = null, XAUDIO2_EFFECT_CHAIN* pEffectChain = null  )
    {
        void* ppSourcevoice=null;
        XAUDIO2_VOICE_SENDS* send =pSendList;
        uint err=PFN_CreateSourceVoice(_lpVtbl, &ppSourcevoice ,&sourceformat,0,MaxFrequencyRatio,null, send,null);
        sourcevoice = new(ppSourcevoice);       
        return (uint)err;
    }
    public unsafe ulong Release( )=> PFN_Release (_lpVtbl);

    public unsafe HRESULT StartEngine()=> PFN_StartEngine(_lpVtbl);

    public unsafe void StopEngine()=> PFN_StopEngine(_lpVtbl);

}

[StructLayout(LayoutKind.Sequential)]
public unsafe class IXAudio2Extension
{
    public static readonly GUID IID_IXAudio2Extension = new(  0x84ac29bb, 0xd619, 0x44d2, 0xb1, 0x97, 0xe4, 0xac, 0xf7, 0xdf, 0x3e, 0xd6);
    private void* _lpVtbl = null;
    private delegate* unmanaged<void*, ulong> PFN_Release=null;

    public IXAudio2Extension(void* lpVtbl)
    {
        _lpVtbl = lpVtbl;
        PFN_Release = (delegate* unmanaged<void*, ulong>)   ((void**)(*(void**)_lpVtbl))[3] ;
        
    }   
    
    public unsafe ulong Release( )=> PFN_Release (_lpVtbl);
    // // NAME: IXAudio2Extension::GetProcessingQuantum
    // // DESCRIPTION: Returns the processing quantum
    // //              quantumMilliseconds = (1000.0f * quantumNumerator / quantumDenominator)
    // //
    // // ARGUMENTS:
    // //  quantumNumerator - Quantum numerator
    // //  quantumDenominator - Quantum denominator
    // //
    // STDMETHOD_(void, GetProcessingQuantum)(THIS_ _Out_ UINT32* quantumNumerator, _Out_range_(!= , 0) UINT32* quantumDenominator);

    // // NAME: IXAudio2Extension::GetProcessor
    // // DESCRIPTION: Returns the number of the processor used by XAudio2
    // //
    // // ARGUMENTS:
    // //  processor - Non-zero Processor number
    // //
    // STDMETHOD_(void, GetProcessor)(THIS_ _Out_range_(!= , 0) XAUDIO2_PROCESSOR* processor);
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct IXAudio2Voice
{
    private void* _lpVtbl = null;
    private delegate* unmanaged<void*, ulong> PFN_Release=null;

    public IXAudio2Voice(void* lpVtbl)
    {
        // ds8 =  LPDIRECTSOUND8 ;
        _lpVtbl = lpVtbl;
        PFN_Release = (delegate* unmanaged<void*, ulong>)   ((void**)(*(void**)_lpVtbl))[3] ;
    }

    public unsafe ulong Release( )=> PFN_Release (_lpVtbl);

    //    /* NAME: IXAudio2Voice::GetVoiceDetails
    // // DESCRIPTION: Returns the basic characteristics of this voice.
    // //
    // // ARGUMENTS:
    // //  pVoiceDetails - Returns the voice's details.
    // */\
    // STDMETHOD_(void, GetVoiceDetails) (THIS_ _Out_ XAUDIO2_VOICE_DETAILS* pVoiceDetails) PURE; \
    // \
    // /* NAME: IXAudio2Voice::SetOutputVoices
    // // DESCRIPTION: Replaces the set of submix/mastering voices that receive
    // //              this voice's output.
    // //
    // // ARGUMENTS:
    // //  pSendList - Optional list of voices this voice should send audio to.
    // */\
    // STDMETHOD(SetOutputVoices) (THIS_ _In_opt_ const XAUDIO2_VOICE_SENDS* pSendList) PURE; \
    // \
    // /* NAME: IXAudio2Voice::SetEffectChain
    // // DESCRIPTION: Replaces this voice's current effect chain with a new one.
    // //
    // // ARGUMENTS:
    // //  pEffectChain - Structure describing the new effect chain to be used.
    // */\
    // STDMETHOD(SetEffectChain) (THIS_ _In_opt_ const XAUDIO2_EFFECT_CHAIN* pEffectChain) PURE; \
    // \
    // /* NAME: IXAudio2Voice::EnableEffect
    // // DESCRIPTION: Enables an effect in this voice's effect chain.
    // //
    // // ARGUMENTS:
    // //  EffectIndex - Index of an effect within this voice's effect chain.
    // //  OperationSet - Used to identify this call as part of a deferred batch.
    // */\
    // STDMETHOD(EnableEffect) (THIS_ UINT32 EffectIndex, \
    //                          UINT32 OperationSet X2DEFAULT(XAUDIO2_COMMIT_NOW)) PURE; \
    // \
    // /* NAME: IXAudio2Voice::DisableEffect
    // // DESCRIPTION: Disables an effect in this voice's effect chain.
    // //
    // // ARGUMENTS:
    // //  EffectIndex - Index of an effect within this voice's effect chain.
    // //  OperationSet - Used to identify this call as part of a deferred batch.
    // */\
    // STDMETHOD(DisableEffect) (THIS_ UINT32 EffectIndex, \
    //                           UINT32 OperationSet X2DEFAULT(XAUDIO2_COMMIT_NOW)) PURE; \
    // \
    // /* NAME: IXAudio2Voice::GetEffectState
    // // DESCRIPTION: Returns the running state of an effect.
    // //
    // // ARGUMENTS:
    // //  EffectIndex - Index of an effect within this voice's effect chain.
    // //  pEnabled - Returns the enabled/disabled state of the given effect.
    // */\
    // STDMETHOD_(void, GetEffectState) (THIS_ UINT32 EffectIndex, _Out_ BOOL* pEnabled) PURE; \
    // \
    // /* NAME: IXAudio2Voice::SetEffectParameters
    // // DESCRIPTION: Sets effect-specific parameters.
    // //
    // // REMARKS: Unlike IXAPOParameters::SetParameters, this method may
    // //          be called from any thread.  XAudio2 implements
    // //          appropriate synchronization to copy the parameters to the
    // //          realtime audio processing thread.
    // //
    // // ARGUMENTS:
    // //  EffectIndex - Index of an effect within this voice's effect chain.
    // //  pParameters - Pointer to an effect-specific parameters block.
    // //  ParametersByteSize - Size of the pParameters array  in bytes.
    // //  OperationSet - Used to identify this call as part of a deferred batch.
    // */\
    // STDMETHOD(SetEffectParameters) (THIS_ UINT32 EffectIndex, \
    //                                 _In_reads_bytes_(ParametersByteSize) const void* pParameters, \
    //                                 UINT32 ParametersByteSize, \
    //                                 UINT32 OperationSet X2DEFAULT(XAUDIO2_COMMIT_NOW)) PURE; \
    // \
    // /* NAME: IXAudio2Voice::GetEffectParameters
    // // DESCRIPTION: Obtains the current effect-specific parameters.
    // //
    // // ARGUMENTS:
    // //  EffectIndex - Index of an effect within this voice's effect chain.
    // //  pParameters - Returns the current values of the effect-specific parameters.
    // //  ParametersByteSize - Size of the pParameters array in bytes.
    // */\
    // STDMETHOD(GetEffectParameters) (THIS_ UINT32 EffectIndex, \
    //                                 _Out_writes_bytes_(ParametersByteSize) void* pParameters, \
    //                                 UINT32 ParametersByteSize) PURE; \
    // \
    // /* NAME: IXAudio2Voice::SetFilterParameters
    // // DESCRIPTION: Sets this voice's filter parameters.
    // //
    // // ARGUMENTS:
    // //  pParameters - Pointer to the filter's parameter structure.
    // //  OperationSet - Used to identify this call as part of a deferred batch.
    // */\
    // STDMETHOD(SetFilterParameters) (THIS_ _In_ const XAUDIO2_FILTER_PARAMETERS* pParameters, \
    //                                 UINT32 OperationSet X2DEFAULT(XAUDIO2_COMMIT_NOW)) PURE; \
    // \
    // /* NAME: IXAudio2Voice::GetFilterParameters
    // // DESCRIPTION: Returns this voice's current filter parameters.
    // //
    // // ARGUMENTS:
    // //  pParameters - Returns the filter parameters.
    // */\
    // STDMETHOD_(void, GetFilterParameters) (THIS_ _Out_ XAUDIO2_FILTER_PARAMETERS* pParameters) PURE; \
    // \
    // /* NAME: IXAudio2Voice::SetOutputFilterParameters
    // // DESCRIPTION: Sets the filter parameters on one of this voice's sends.
    // //
    // // ARGUMENTS:
    // //  pDestinationVoice - Destination voice of the send whose filter parameters will be set.
    // //  pParameters - Pointer to the filter's parameter structure.
    // //  OperationSet - Used to identify this call as part of a deferred batch.
    // */\
    // STDMETHOD(SetOutputFilterParameters) (THIS_ _In_opt_ IXAudio2Voice* pDestinationVoice, \
    //                                       _In_ const XAUDIO2_FILTER_PARAMETERS* pParameters, \
    //                                       UINT32 OperationSet X2DEFAULT(XAUDIO2_COMMIT_NOW)) PURE; \
    // \
    // /* NAME: IXAudio2Voice::GetOutputFilterParameters
    // // DESCRIPTION: Returns the filter parameters from one of this voice's sends.
    // //
    // // ARGUMENTS:
    // //  pDestinationVoice - Destination voice of the send whose filter parameters will be read.
    // //  pParameters - Returns the filter parameters.
    // */\
    // STDMETHOD_(void, GetOutputFilterParameters) (THIS_ _In_opt_ IXAudio2Voice* pDestinationVoice, \
    //                                              _Out_ XAUDIO2_FILTER_PARAMETERS* pParameters) PURE; \
    // \
    // /* NAME: IXAudio2Voice::SetVolume
    // // DESCRIPTION: Sets this voice's overall volume level.
    // //
    // // ARGUMENTS:
    // //  Volume - New overall volume level to be used, as an amplitude factor.
    // //  OperationSet - Used to identify this call as part of a deferred batch.
    // */\
    // STDMETHOD(SetVolume) (THIS_ float Volume, \
    //                       UINT32 OperationSet X2DEFAULT(XAUDIO2_COMMIT_NOW)) PURE; \
    // \
    // /* NAME: IXAudio2Voice::GetVolume
    // // DESCRIPTION: Obtains this voice's current overall volume level.
    // //
    // // ARGUMENTS:
    // //  pVolume: Returns the voice's current overall volume level.
    // */\
    // STDMETHOD_(void, GetVolume) (THIS_ _Out_ float* pVolume) PURE; \
    // \
    // /* NAME: IXAudio2Voice::SetChannelVolumes
    // // DESCRIPTION: Sets this voice's per-channel volume levels.
    // //
    // // ARGUMENTS:
    // //  Channels - Used to confirm the voice's channel count.
    // //  pVolumes - Array of per-channel volume levels to be used.
    // //  OperationSet - Used to identify this call as part of a deferred batch.
    // */\
    // STDMETHOD(SetChannelVolumes) (THIS_ UINT32 Channels, _In_reads_(Channels) const float* pVolumes, \
    //                               UINT32 OperationSet X2DEFAULT(XAUDIO2_COMMIT_NOW)) PURE; \
    // \
    // /* NAME: IXAudio2Voice::GetChannelVolumes
    // // DESCRIPTION: Returns this voice's current per-channel volume levels.
    // //
    // // ARGUMENTS:
    // //  Channels - Used to confirm the voice's channel count.
    // //  pVolumes - Returns an array of the current per-channel volume levels.
    // */\
    // STDMETHOD_(void, GetChannelVolumes) (THIS_ UINT32 Channels, _Out_writes_(Channels) float* pVolumes) PURE; \
    // \
    // /* NAME: IXAudio2Voice::SetOutputMatrix
    // // DESCRIPTION: Sets the volume levels used to mix from each channel of this
    // //              voice's output audio to each channel of a given destination
    // //              voice's input audio.
    // //
    // // ARGUMENTS:
    // //  pDestinationVoice - The destination voice whose mix matrix to change.
    // //  SourceChannels - Used to confirm this voice's output channel count
    // //   (the number of channels produced by the last effect in the chain).
    // //  DestinationChannels - Confirms the destination voice's input channels.
    // //  pLevelMatrix - Array of [SourceChannels * DestinationChannels] send
    // //   levels.  The level used to send from source channel S to destination
    // //   channel D should be in pLevelMatrix[S + SourceChannels * D].
    // //  OperationSet - Used to identify this call as part of a deferred batch.
    // */\
    // STDMETHOD(SetOutputMatrix) (THIS_ _In_opt_ IXAudio2Voice* pDestinationVoice, \
    //                             UINT32 SourceChannels, UINT32 DestinationChannels, \
    //                             _In_reads_(SourceChannels * DestinationChannels) const float* pLevelMatrix, \
    //                             UINT32 OperationSet X2DEFAULT(XAUDIO2_COMMIT_NOW)) PURE; \
    // \
    // /* NAME: IXAudio2Voice::GetOutputMatrix
    // // DESCRIPTION: Obtains the volume levels used to send each channel of this
    // //              voice's output audio to each channel of a given destination
    // //              voice's input audio.
    // //
    // // ARGUMENTS:
    // //  pDestinationVoice - The destination voice whose mix matrix to obtain.
    // //  SourceChannels - Used to confirm this voice's output channel count
    // //   (the number of channels produced by the last effect in the chain).
    // //  DestinationChannels - Confirms the destination voice's input channels.
    // //  pLevelMatrix - Array of send levels, as above.
    // */\
    // STDMETHOD_(void, GetOutputMatrix) (THIS_ _In_opt_ IXAudio2Voice* pDestinationVoice, \
    //                                    UINT32 SourceChannels, UINT32 DestinationChannels, \
    //                                    _Out_writes_(SourceChannels * DestinationChannels) float* pLevelMatrix) PURE; \
    // \
    // /* NAME: IXAudio2Voice::DestroyVoice
    // // DESCRIPTION: Destroys this voice, stopping it if necessary and removing
    // //              it from the XAudio2 graph.
    // */\
    // STDMETHOD_(void, DestroyVoice) (THIS) PURE
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct IXAudio2SourceVoice
{
    private void* _lpVtbl = null;
    private delegate* unmanaged<void* ,uint,int> PFN_SetCooperativeLevel = null;
    private delegate* unmanaged<void*, uint, uint, uint> PFN_Start = null;
    private delegate* unmanaged<void*, uint, uint, int> PFN_Stop = null;
    private delegate* unmanaged<void*, XAUDIO2_BUFFER*, XAUDIO2_BUFFER_WMA*, HRESULT> PFN_SubmitSourceBuffer = null;

    private delegate* unmanaged<void*, void > PFN_DestroyVoice = null;

    public IXAudio2SourceVoice(void* lpvtbl)
    {
        _lpVtbl = lpvtbl;
        PFN_DestroyVoice = (delegate* unmanaged<void*, void>)  ((void**)(*(void**)_lpVtbl))[18]  ;
        PFN_Start =(delegate* unmanaged<void*, uint, uint, uint>)  ((void**)(*(void**)_lpVtbl))[19]  ;
        PFN_Stop = (delegate* unmanaged<void*, uint, uint, int>) ((void**)(*(void**)_lpVtbl))[20]  ;
        PFN_SubmitSourceBuffer =(delegate* unmanaged<void*, XAUDIO2_BUFFER*, XAUDIO2_BUFFER_WMA*, HRESULT>)  ((void**)(*(void**)_lpVtbl))[21]  ;
    }

    public string AddressHexa => new IntPtr( _lpVtbl ).ToString("0:X");

    public void Dispose()
    {
        PFN_Start = null;
        _lpVtbl = null;
        PFN_Stop = null;
        PFN_SubmitSourceBuffer = null;
    }

    public uint Start( uint Flags = 0, uint OperationSet = 0)   => PFN_Start (_lpVtbl,Flags,OperationSet);
    
    public int Stop( uint Flags = 0, uint OperationSet = 0)  => PFN_Stop (_lpVtbl,Flags,OperationSet);

    public void DestroyVoice() => PFN_DestroyVoice( _lpVtbl);

    public unsafe uint SubmitSourceBuffer(ref XAUDIO2_BUFFER pBuffer, XAUDIO2_BUFFER_WMA* pBufferWMA =null)
    {
        uint err =0;
        fixed( XAUDIO2_BUFFER* pb = &pBuffer) {
            err =  PFN_SubmitSourceBuffer(_lpVtbl, pb,  pBufferWMA );
        }
        return err;
    }

}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct IXAudio2SubmixVoice
{
    // // Methods from IXAudio2Voice base interface
    // Declare_IXAudio2Voice_Methods();

    // // There are currently no methods specific to submix voices.
};

[StructLayout(LayoutKind.Sequential)]
public unsafe struct IXAudio2MasteringVoice
{
    private void* _lpVtbl = null;
    private delegate* unmanaged<void*, void>  PFN_DestroyVoice = null;
    private delegate* unmanaged<IXAudio2MasteringVoice*, float, uint, uint> PFN_SetVolume = null;
    private delegate*  unmanaged<void*, uint*,  int> PFN_GetChannelMask = null;
    private delegate* unmanaged<IXAudio2MasteringVoice*, XAUDIO2_VOICE_DETAILS*, void> PFN_GetVoiceDetails = null;

    public IXAudio2MasteringVoice(void* xaudio2MasteringVoice)
    {
        _lpVtbl =  xaudio2MasteringVoice;
        PFN_SetVolume  = (delegate* unmanaged<IXAudio2MasteringVoice*, float, uint, uint>)((void**)(*(void**)_lpVtbl))[12]  ;
        PFN_DestroyVoice = (delegate* unmanaged<void*, void>) ((void**)(*(void**)_lpVtbl))[18]  ;
        PFN_GetChannelMask = (delegate* unmanaged<void*, uint*,  int>) ((void**)(*(void**)_lpVtbl))[19] ;
        PFN_GetVoiceDetails = ((delegate* unmanaged<IXAudio2MasteringVoice*, XAUDIO2_VOICE_DETAILS*, void>)       ((void**)(*(void**)_lpVtbl))[19] ) ;
    }

    public void DestroyVoice() => PFN_DestroyVoice(_lpVtbl);

    public uint SetVolume(float volume , uint operationSet = 0)
        => PFN_SetVolume((IXAudio2MasteringVoice*)_lpVtbl, volume , operationSet);
    
    public int GetChannelMask(DWORD* pChannelmask)=> PFN_GetChannelMask (_lpVtbl, pChannelmask );

    public void GetVoiceDetails(XAUDIO2_VOICE_DETAILS* pVoiceDetails)
        => PFN_GetVoiceDetails( (IXAudio2MasteringVoice*)_lpVtbl , pVoiceDetails);

    public void Release()
    {
        PFN_SetVolume  = null;
        PFN_DestroyVoice = null;
        PFN_GetChannelMask = null;
        PFN_GetVoiceDetails = null ;
    }
};

[StructLayout(LayoutKind.Sequential)]
public unsafe class IXAudio2EngineCallback
{
    // // Called by XAudio2 just before an audio processing pass begins.
    // STDMETHOD_(void, OnProcessingPassStart) (THIS) PURE;

    // // Called just after an audio processing pass ends.
    // STDMETHOD_(void, OnProcessingPassEnd) (THIS) PURE;

    // // Called in the event of a critical system error which requires XAudio2
    // // to be closed down and restarted.  The error code is given in Error.
    // STDMETHOD_(void, OnCriticalError) (THIS_ HRESULT Error) PURE;
};

[StructLayout(LayoutKind.Sequential)]
public unsafe struct IXAudio2VoiceCallback
{
    // // Called just before this voice's processing pass begins.
    // STDMETHOD_(void, OnVoiceProcessingPassStart) (THIS_ UINT32 BytesRequired) PURE;

    // // Called just after this voice's processing pass ends.
    // STDMETHOD_(void, OnVoiceProcessingPassEnd) (THIS) PURE;

    // // Called when this voice has just finished playing a buffer stream
    // // (as marked with the XAUDIO2_END_OF_STREAM flag on the last buffer).
    // STDMETHOD_(void, OnStreamEnd) (THIS) PURE;

    // // Called when this voice is about to start processing a new buffer.
    // STDMETHOD_(void, OnBufferStart) (THIS_ void* pBufferContext) PURE;

    // // Called when this voice has just finished processing a buffer.
    // // The buffer can now be reused or destroyed.
    // STDMETHOD_(void, OnBufferEnd) (THIS_ void* pBufferContext) PURE;

    // // Called when this voice has just reached the end position of a loop.
    // STDMETHOD_(void, OnLoopEnd) (THIS_ void* pBufferContext) PURE;

    // // Called in the event of a critical error during voice processing,
    // // such as a failing xAPO or an error from the hardware XMA decoder.
    // // The voice may have to be destroyed and re-created to recover from
    // // the error.  The callback arguments report which buffer was being
    // // processed when the error occurred, and its HRESULT code.
    // STDMETHOD_(void, OnVoiceError) (THIS_ void* pBufferContext, HRESULT Error) PURE;
};

#endregion


[StructLayout(LayoutKind.Sequential, Pack = 1)]
public partial struct X3DAUDIO_CONE
{
    public float InnerAngle;
    public float OuterAngle;
    public float InnerVolume;
    public float OuterVolume;
    public float InnerLPF;
    public float OuterLPF;
    public float InnerReverb;
    public float OuterReverb;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe partial struct X3DAUDIO_DISTANCE_CURVE
{
    public X3DAUDIO_DISTANCE_CURVE_POINT* pPoints;
    public uint PointCount;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public partial struct X3DAUDIO_DISTANCE_CURVE_POINT
{
    public float Distance;
    public float DSPSetting;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe partial struct X3DAUDIO_DSP_SETTINGS
{
    public float* pMatrixCoefficients;
    public float* pDelayTimes;
    public uint SrcChannelCount;
    public uint DstChannelCount;
    public float LPFDirectCoefficient;
    public float LPFReverbCoefficient;
    public float ReverbLevel;
    public float DopplerFactor;
    public float EmitterToListenerAngle;
    public float EmitterToListenerDistance;
    public float EmitterVelocityComponent;
    public float ListenerVelocityComponent;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe partial struct X3DAUDIO_EMITTER
{
    public X3DAUDIO_CONE* pCone;
    public X3DAUDIO_VECTOR OrientFront;
    public X3DAUDIO_VECTOR OrientTop;
    public X3DAUDIO_VECTOR Position;
    public X3DAUDIO_VECTOR Velocity;
    public float InnerRadius;
    public float InnerRadiusAngle;
    public uint ChannelCount;
    public float ChannelRadius;
    public float* pChannelAzimuths;
    public X3DAUDIO_DISTANCE_CURVE* pVolumeCurve;
    public X3DAUDIO_DISTANCE_CURVE* pLFECurve;
    public X3DAUDIO_DISTANCE_CURVE* pLPFDirectCurve;
    public X3DAUDIO_DISTANCE_CURVE* pLPFReverbCurve;
    public X3DAUDIO_DISTANCE_CURVE* pReverbCurve;
    public float CurveDistanceScaler;
    public float DopplerScaler;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe partial struct X3DAUDIO_VECTOR
{
    public float X;
    public float Y;
    public float Z;
    public X3DAUDIO_VECTOR(float x , float y , float z)=> (X,Y,Z)=(x,y,z);
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe partial struct X3DAUDIO_LISTENER
{
    public X3DAUDIO_VECTOR OrientFront;
    public X3DAUDIO_VECTOR OrientTop;
    public X3DAUDIO_VECTOR Position;
    public X3DAUDIO_VECTOR Velocity;
    public X3DAUDIO_CONE* pCone;
}




    [StructLayout(LayoutKind.Sequential, Pack =1 ), SkipLocalsInit]
    public unsafe struct XAUDIO2FX_VOLUMEMETER_LEVELS
    {
        public float* pPeakLevels;
        public float* pRMSLevels;
        public uint ChannelCount;
    }

    [StructLayout(LayoutKind.Sequential, Pack =1 ), SkipLocalsInit]
    public unsafe struct XAUDIO2FX_REVERB_PARAMETERS
    {
        
        public float WetDryMix;
        public uint ReflectionsDelay;
        public byte ReverbDelay;
        public byte RearDelay;
        public byte SideDelay;
        public byte PositionLeft;
        public byte PositionRight;
        public byte PositionMatrixLeft;
        public byte PositionMatrixRight;
        public byte EarlyDiffusion;
        public byte LateDiffusion;
        public byte LowEQGain;
        public byte LowEQCutoff;
        public byte HighEQGain;
        public byte HighEQCutoff;
        public float RoomFilterFreq;
        public float RoomFilterMain;
        public float RoomFilterHF;
        public float ReflectionsGain;
        public float ReverbGain;
        public float DecayTime;
        public float Density;
        public float RoomSize;
        public BOOL DisableLateField;
    }

    [StructLayout(LayoutKind.Sequential, Pack =1 ), SkipLocalsInit]
    public partial struct XAUDIO2FX_REVERB_I3DL2_PARAMETERS
    {
        public float WetDryMix;
        public int Room;
        public int RoomHF;
        public float RoomRolloffFactor;
        public float DecayTime;
        public float DecayHFRatio;
        public int Reflections;
        public float ReflectionsDelay;
        public int Reverb;
        public float ReverbDelay;
        public float Diffusion;
        public float Density;
        public float HFReference;
    }

    [SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = 1), SkipLocalsInit]
    public static class XAUDIO2FX
    {
        public const int XAUDIO2FX_REVERB_MIN_FRAMERATE = 20000;
        public const int XAUDIO2FX_REVERB_MAX_FRAMERATE = 48000;
        public const float XAUDIO2FX_REVERB_MIN_WET_DRY_MIX = 0.0f;
        public const int XAUDIO2FX_REVERB_MIN_REFLECTIONS_DELAY = 0;
        public const int XAUDIO2FX_REVERB_MIN_REVERB_DELAY = 0;
        public const int XAUDIO2FX_REVERB_MIN_REAR_DELAY = 0;
        public const int XAUDIO2FX_REVERB_MIN_7POINT1_SIDE_DELAY = 0;
        public const int XAUDIO2FX_REVERB_MIN_7POINT1_REAR_DELAY = 0;
        public const int XAUDIO2FX_REVERB_MIN_POSITION = 0;
        public const int XAUDIO2FX_REVERB_MIN_DIFFUSION = 0;
        public const int XAUDIO2FX_REVERB_MIN_LOW_EQ_GAIN = 0;
        public const int XAUDIO2FX_REVERB_MIN_LOW_EQ_CUTOFF = 0;
        public const int XAUDIO2FX_REVERB_MIN_HIGH_EQ_GAIN = 0;
        public const int XAUDIO2FX_REVERB_MIN_HIGH_EQ_CUTOFF = 0;
        public const float XAUDIO2FX_REVERB_MIN_ROOM_FILTER_FREQ = 20.0f;
        public const float XAUDIO2FX_REVERB_MIN_ROOM_FILTER_MAIN = -100.0f;
        public const float XAUDIO2FX_REVERB_MIN_ROOM_FILTER_HF = -100.0f;
        public const float XAUDIO2FX_REVERB_MIN_REFLECTIONS_GAIN = -100.0f;
        public const float XAUDIO2FX_REVERB_MIN_REVERB_GAIN = -100.0f;
        public const float XAUDIO2FX_REVERB_MIN_DECAY_TIME = 0.1f;
        public const float XAUDIO2FX_REVERB_MIN_DENSITY = 0.0f;
        public const float XAUDIO2FX_REVERB_MIN_ROOM_SIZE = 0.0f;
        public const float XAUDIO2FX_REVERB_MAX_WET_DRY_MIX = 100.0f;
        public const int XAUDIO2FX_REVERB_MAX_REFLECTIONS_DELAY = 300;
        public const int XAUDIO2FX_REVERB_MAX_REVERB_DELAY = 85;
        public const int XAUDIO2FX_REVERB_MAX_REAR_DELAY = 5;
        public const int XAUDIO2FX_REVERB_MAX_7POINT1_SIDE_DELAY = 5;
        public const int XAUDIO2FX_REVERB_MAX_7POINT1_REAR_DELAY = 20;
        public const int XAUDIO2FX_REVERB_MAX_POSITION = 30;
        public const int XAUDIO2FX_REVERB_MAX_DIFFUSION = 15;
        public const int XAUDIO2FX_REVERB_MAX_LOW_EQ_GAIN = 12;
        public const int XAUDIO2FX_REVERB_MAX_LOW_EQ_CUTOFF = 9;
        public const int XAUDIO2FX_REVERB_MAX_HIGH_EQ_GAIN = 8;
        public const int XAUDIO2FX_REVERB_MAX_HIGH_EQ_CUTOFF = 14;
        public const float XAUDIO2FX_REVERB_MAX_ROOM_FILTER_FREQ = 20000.0f;
        public const float XAUDIO2FX_REVERB_MAX_ROOM_FILTER_MAIN = 0.0f;
        public const float XAUDIO2FX_REVERB_MAX_ROOM_FILTER_HF = 0.0f;
        public const float XAUDIO2FX_REVERB_MAX_REFLECTIONS_GAIN = 20.0f;
        public const float XAUDIO2FX_REVERB_MAX_REVERB_GAIN = 20.0f;
        public const float XAUDIO2FX_REVERB_MAX_DENSITY = 100.0f;
        public const float XAUDIO2FX_REVERB_MAX_ROOM_SIZE = 100.0f;
        public const float XAUDIO2FX_REVERB_DEFAULT_WET_DRY_MIX = 100.0f;
        public const int XAUDIO2FX_REVERB_DEFAULT_REFLECTIONS_DELAY = 5;
        public const int XAUDIO2FX_REVERB_DEFAULT_REVERB_DELAY = 5;
        public const int XAUDIO2FX_REVERB_DEFAULT_REAR_DELAY = 5;
        public const int XAUDIO2FX_REVERB_DEFAULT_7POINT1_SIDE_DELAY = 5;
        public const int XAUDIO2FX_REVERB_DEFAULT_7POINT1_REAR_DELAY = 20;
        public const int XAUDIO2FX_REVERB_DEFAULT_POSITION = 6;
        public const int XAUDIO2FX_REVERB_DEFAULT_POSITION_MATRIX = 27;
        public const int XAUDIO2FX_REVERB_DEFAULT_EARLY_DIFFUSION = 8;
        public const int XAUDIO2FX_REVERB_DEFAULT_LATE_DIFFUSION = 8;
        public const int XAUDIO2FX_REVERB_DEFAULT_LOW_EQ_GAIN = 8;
        public const int XAUDIO2FX_REVERB_DEFAULT_LOW_EQ_CUTOFF = 4;
        public const int XAUDIO2FX_REVERB_DEFAULT_HIGH_EQ_GAIN = 8;
        public const int XAUDIO2FX_REVERB_DEFAULT_HIGH_EQ_CUTOFF = 4;
        public const float XAUDIO2FX_REVERB_DEFAULT_ROOM_FILTER_FREQ = 5000.0f;
        public const float XAUDIO2FX_REVERB_DEFAULT_ROOM_FILTER_MAIN = 0.0f;
        public const float XAUDIO2FX_REVERB_DEFAULT_ROOM_FILTER_HF = 0.0f;
        public const float XAUDIO2FX_REVERB_DEFAULT_REFLECTIONS_GAIN = 0.0f;
        public const float XAUDIO2FX_REVERB_DEFAULT_REVERB_GAIN = 0.0f;
        public const float XAUDIO2FX_REVERB_DEFAULT_DECAY_TIME = 1.0f;
        public const float XAUDIO2FX_REVERB_DEFAULT_DENSITY = 100.0f;
        public const float XAUDIO2FX_REVERB_DEFAULT_ROOM_SIZE = 100.0f;
        public const int XAUDIO2FX_REVERB_DEFAULT_DISABLE_LATE_FIELD = 0;
    }





#endregion
