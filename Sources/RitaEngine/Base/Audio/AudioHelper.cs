


namespace RitaEngine.Base.Audio;

// see  AudioEmitter  :  https://www.maxicours.com/se/cours/caracteristiques-des-sons-musicaux/
public struct AudioHelper
{
 //refaire https://github.com/raysan5/rfxgen
} 
   
/*

AUDIO
    Native/Backend/Interop/ 
    
    
    Sounds
        Ogg
        Wav
        Mp3
        Flac
        Xm
        ....
        NoiseGenerator
    Filters/ see miniaudio engine???             
  

    AudioEngine.cs(Need AudioDevice )
        Init( ILoader? AudioConfig )
        AudioDevice.cs ( load() ,init() , release() , config , capabilities/infos , data )
        AudioRender
        
        => SetMasterVolume(0.0 to1.0f log );
        => var sound = ImportSound2D(waveFile) [ use struct Sound ]
        => var source = CreateSound2D(source) [ use struct Source ]
        => PlaySound2D(source, loop, volume) 
        => advanced system for game 
        => sttream music , load many sources ...





*/


/* Toujours utiliser les using en global  pour les clr sauf pour le framework */
// global using System;

// namespace Rita.FrameWork.Audio
// {

//     public class Engine
//     {
//         //AudioDevice => Init  IN
//         //AudioRendder =>  Render OUT

//         // private readonly AudioEngineData _data=null!;
//         // private readonly AudioEngineConfig _config=null!;
//     }

//     public class Voice
//     {
//         //Speech recognition
//     }

//     public class Capture
//     {

//     }

//        /// <summary>
//     /// Applique un effets que pour les buffers ? 
//     /// </summary>
//     public class Effects
//     {

//     }

//     /// <summary>
//     ///  creé des bruits spécifique 
//     /// </summary>
//     public class Noises
//     {
//         /// <summary>
//         /// Bruit blanc bruit rose ..
//         /// </summary>
//         public static class WaveForm
//         {

//         }
//     }

//     /// <summary>
//     ///  applique des filtre sur le son 
//     /// </summary>
//     public class Filter
//     {

//     }

//     /// <summary>
//     /// Mixe le sons des différents sources 
//     /// Applique effets et filtres
//     /// </summary>
//     public class Mixer
//     {
//         private Equalizer Equalizer=null!;
//         private Filter[] Filters=null!;
//         private Effects[] Effects=null!;
//     }

//     /// <summary>
//     /// Class Harmonisation du son pour le mixer
//     /// </summary>
//     public class Equalizer
//     {

//     }

//     /// <summary>
//     /// Ecouter obligatoire au moins par scène
//     /// </summary>
//     public class Listener
//     {

//     }

//     /// <summary>
//     /// persone ou objet ou autre qui emet un son( source dans openal)
//     /// </summary>
//     public class Emiter
//     {
//         private Source[] Sources = null!;
//     }

//     /// <summary>
//     /// Sons 
//     /// </summary>
//     public class Source
//     {
//         private Buffer[] Buffers = null!;

//         public enum Format{ F32,S16,S24,S32,U8 }
        
//     }

//     public class Buffer
//     {
//         public enum BufferState{ Compressed, stream, normal, fixe , circle}

//         //USe sounds 
//         public void Create( BufferState state)
//         {
//             // Sounds.LoadSound();
//         }

//         public void RingBuffer(){}
        
//     }

//     public enum BackEnd
//     {
//         None,
//         Dsound,
//         Xaudio,
//         CoreAudio,
//         Pulse,
//         Alsa,
//         Winmm,
//         //system decide for you
//         Platform

//     }
// }

// namespace Rita.FrameWork.Audio.Structures
// {
//     public class DeviceData
//     {
//         public IntPtr Module=IntPtr.Zero;
//     }

//     public class RenderData
//     {
//         public IntPtr Instance=IntPtr.Zero;
//     }

//     public class DeviceConfig
//     {
//         public string ModuleName="XAUDIO_2.dll";
//         //selec backend
//     }


// }

// namespace Rita.FrameWork.Audio.Features
// {
//     using Rita.FrameWork.Audio.Structures;
//     public static class Devices
//     {
//         public static void Init(ref DeviceData device, ref RenderData render,ref DeviceConfig config)
//         {
//             //SELECTED BACKEND
//         }

//         public static void Release(ref DeviceData device, ref RenderData render)
//         {
            
//         }

//         private static void InitHelper(ref DeviceData device)
//         {

//         }
//     }

//     public  class Renders
//     {
//         //Receive all data need to play , mix add effect paus stop 
//         //Use BSP and Graph node 
//         public void Add()
//         {

//         }
//         public void Play()
//         {

//         }

//         public void Update()
//         {

//         }
//     }

    


// }

// namespace Rita.FrameWork.Audio.Platform
// {
//     public static class XAUDIO{
//         public static void Load(IntPtr module){}
//         public static void UnLoad(IntPtr module){}
//     }

//     public static class Pulse{
//         public static void Load(IntPtr module){}
//         public static void UnLoad(IntPtr module){}

//     }

//     public static class DSound{
//         public static void Load(IntPtr module){}
//         public static void UnLoad(IntPtr module){}
//     }

// }

// namespace Rita.FrameWork.Audio.Tools
// {


// public class Sounds
// {

//     public void LoadSound()
//     {

//     }

//     public void Decode(){}

//     public void Encode(){}


// }

// }

// namespace Rita.FrameWork.Audio.Tools.OGG{}

// namespace Rita.FrameWork.Audio.Tools.WAV{}

// namespace Rita.FrameWork.Audio.Tools.MP3 {}

// namespace Rita.FrameWork.Audio.Tools.FLAC{}

// namespace Rita.FrameWork.Audio.Tools.XM{}

// namespace Rita.FrameWork.Audio.Tools.Midi{}


// /*
// BLOCkS
//     Segment branch
//     Unit => layer
//         fraction
//         component
//         detail
//         feature

// BLOKS
//     CHUNK
//         Piece
//         part
//         portion

// Section AREAS     Region

// Groups | parts

// Support = platform = device



// building block = monad
//     component
//     data

// */



// /* Toujours utiliser les using en global  pour les clr sauf pour le Framework */

//     /*
//     INIT XAUDIO 2 :
//         Load DLL
//         LoadInstances   xauio2Create => store audioinstance ( xaudio2 ptr)audioHandle
//         create IXaudio2 interface instance with xaudioInstance ptr
//             CreateMastering voice  => store MAsteringvoice ptr
//             GetperformanceData => store capabilities
//             Release
//             Start
//             stop

//         Audio Graph :
//             Input Audio Data
//             Source Voice
//             MasteringVoice
//             Audio Device
//     */
    
// //GLOBALS USINGS FOR AUDIO ENGINE 
// global using System;
// global using System.Runtime.InteropServices;

// namespace RitaEngine.Framework.Audio
// {
//     // RitaEngine.Framework NEEDED
//     // Math: sin, cos, ...
//     // Resource RSX => ReadFromRsx
//     // IO: read write File
//     //

//     public class Engine
//     {
//         //AudioDevice => Init  IN
//         //AudioRendder =>  Render OUT
//         //AudioVoice
//         //AudioCapture

//         public void Init(){}

//         public void Dispose(){ 
//             //Release
//         }
//         public bool IsReady => false;
//         public void Start(){ }
//         public void Stop(){ }

//     }

//     public class Voice
//     {
//         //Speech recognition
//     }

//     public class Capture
//     {

//     }

//     /// <summary>
//     /// Applique un effets que pour les buffers ? 
//     /// </summary>
//     public class Effects
//     {

//     }

//     /// <summary>
//     ///  applique des filtre sur le son 
//     /// </summary>
//     public class Filter
//     {
//         //Low Pass first second high order

//         //High Pass 

//         //band pass

//         //Notch filtering

//         // Peaking EQ filtering

//         //Low shelf filtering 

//         //High shelf filtering

//     }

//     /// <summary>
//     /// Mixe le sons des différents sources 
//     /// Applique effets et filtres
//     /// </summary>
//     public class Mixer
//     {
//         // private Equalizer Equalizer=null!;
//         // private Filter[] Filters=null!;
//         // private Effects[] Effects=null!;
//     }

//     /// <summary>
//     /// Class Harmonisation du son pour le mixer
//     /// </summary>
//     public class Equalizer
//     {

//     }

//     /// <summary>
//     /// Ecouter obligatoire au moins par scène
//     /// </summary>
//     public class Listener
//     {

//     }

//     /// <summary>
//     /// persone ou objet ou autre qui emet un son( source dans openal)
//     /// </summary>
//     public class Emiter//source
//     {
//         // private Buffers[] Buffers = null!;
//         // void Play();                                    // Play a sound
//         // void Stop(Sound sound);                                    // Stop playing a sound
//         // void Pause(Sound sound);                                   // Pause a sound
//         // void Resume(Sound sound);                                  // Resume a paused sound
//         // all functions add effects .....
//         // Create, Cache , dispose , 
//     }

//     public class Buffers
//     {
//         public enum BufferState{ Compressed, stream, normal, fixe , circular, ring}

//         //USe sounds 
//         public void Create( BufferState state)
//         {
//             // Sounds.Decode.ReadPCMFrame();
//         }

//         public void RingBuffer(){}

//         public void CreatePool(){
//             //Size
//             //id or name
//             //used
//             // sample, format ,...
//             // length
//         }

//     }

//     /// <summary>
//     /// Simple wrapper class to decode or encode sound data format ( see namespace) and just play sound 
//     /// Resource management for loading and streaming sounds.
//     /// </summary>
//     public class Sound
//     {
//         public void SimmplePlaySound( Audio.Engine engine, string filename, bool loop)
//         {
//             //Decode sound
//         }

//         public static void LoadSound() {}

//         public static void WriteSound(){}
//     //      Wave LoadWave(const char *fileName);                            // Load wave data from file
//     // Wave LoadWaveFromMemory(const char *fileType, const unsigned char *fileData, int dataSize); // Load wave from memory buffer, fileType refers to extension: i.e. '.wav'
//     // Sound LoadSound(const char *fileName);                          // Load sound from file
//     // Sound LoadSoundFromWave(Wave wave);                             // Load sound from wave data
//     // void UpdateSound(Sound sound, const void *data, int sampleCount); // Update sound buffer with new data
//     // void UnloadWave(Wave wave);                                     // Unload wave data
//     // void UnloadSound(Sound sound);                                  // Unload sound
//     // bool ExportWave(Wave wave, const char *fileName);               // Export wave data to file, returns true on success
//     // bool ExportWaveAsCode(Wave wave, const char *fileName);         // Export wave sample data to code (.h), returns true on success
//     }

//     /// <summary>
//     /// 
//     /// A node graph system for advanced mixing and effect processing.
//     ///  TODO : like blueprint 
//     /// </summary>
//     public class NodesGraph
//     {
//         //See pipeline audio ???? 
//     //      void PlaySound(Sound sound);                                    // Play a sound
//     // void StopSound(Sound sound);                                    // Stop playing a sound
//     // void PauseSound(Sound sound);                                   // Pause a sound
//     // void ResumeSound(Sound sound);                                  // Resume a paused sound
//     // void PlaySoundMulti(Sound sound);                               // Play a sound (using multichannel buffer pool)
//     // void StopSoundMulti(void);                                      // Stop any sound playing (using multichannel buffer pool)
//     // int GetSoundsPlaying(void);                                     // Get number of sounds playing in the multichannel
//     // bool IsSoundPlaying(Sound sound);                               // Check if a sound is currently playing
//     // void SetSoundVolume(Sound sound, float volume);                 // Set volume for a sound (1.0 is max level)
//     // void SetSoundPitch(Sound sound, float pitch);                   // Set pitch for a sound (1.0 is base level)
//     // void SetSoundPan(Sound sound, float pan);                       // Set pan for a sound (0.5 is center)
//     // Wave WaveCopy(Wave wave);                                       // Copy a wave to a new wave
//     // void WaveCrop(Wave *wave, int initSample, int finalSample);     // Crop a wave to defined samples range
//     // void WaveFormat(Wave *wave, int sampleRate, int sampleSize, int channels); // Convert wave data to desired format
//     // float *LoadWaveSamples(Wave wave);                              // Load samples data from wave as a 32bit float data array
//     // void UnloadWaveSamples(float *samples);                         // Unload samples data loaded with LoadWaveSamples()

//     }
// }

// namespace RitaEngine.Framework.Audio.Common//SHARED
// {

//     public enum AudioBackEnd
//     {
//         None,
//         Dsound,
//         Xaudio,
//         CoreAudio,
//         Pulse,
//         Alsa,
//         Winmm,
//         OpenAL,
//         MegaDrive,
//         //system decide for you
//         Platform,
//         Web,
//         PS4,
//         XBOX,
//         SWITCH,

//     }

//     public enum AudioFormat{
//         /// <summary>32 bit floating point [-1;1] </summary>
//         F32,
//         /// <summary> 16 bit signed integer [-32768, 32767] </summary>
//         S16,
//         S24,
//         S32,
//         U8
//     }

//     public enum SoundDithering
//     { 
//         None,
//         Rectangle,
//         Triangle
//     }
// }

// namespace RitaEngine.Framework.Audio.Structures
// {
//     using RitaEngine.Framework.Audio.Common;
//     //see https://github.com/raysan5/rfxgen/blob/master/src/rfxgen.h
//     public struct EmitterParams//WAVE/DATA/RAW PARAMS 
//     {
//         public string Name;

//         public int size;

//         // Random seed used to generate the wave
//         public int randSeed;

//         // Wave type (square, sawtooth, sine, noise)
//         public int waveTypeValue;

//         // Wave envelope parameters
//         public float attackTimeValue;
//         public float sustainTimeValue;
//         public float sustainPunchValue;
//         public float decayTimeValue;

//         // Frequency parameters
//         public float startFrequencyValue;
//         public float minFrequencyValue;
//         public float slideValue;
//         public float deltaSlideValue;
//         public float vibratoDepthValue;
//         public float vibratoSpeedValue;
//         //float vibratoPhaseDelayValue;

//         // Tone change parameters
//         public float changeAmountValue;
//         public float changeSpeedValue;

//         // Square wave parameters
//         public float squareDutyValue;
//         public float dutySweepValue;

//         // Repeat parameters
//         public float repeatSpeedValue;

//         // Phaser parameters
//         public float phaserOffsetValue;
//         public float phaserSweepValue;

//         // Filter parameters
//         public float lpfCutoffValue;
//         public float lpfCutoffSweepValue;
//         public float lpfResonanceValue;
//         public float hpfCutoffValue;
//         public float hpfCutoffSweepValue;
 
//     }

//     public class DeviceData
//     {
//         public IntPtr Module=IntPtr.Zero;
//     }

//     public class RenderData
//     {
//         public IntPtr Instance=IntPtr.Zero;
//     }

//     public class DeviceConfig
//     {
//         public string ModuleName="XAUDIO9_2.dll";
//         //selec backend
//         public int Sample;
//         public int PCMFrame;
//         public int Channel;
//         public int SampleRate;
//         // public Format Format;
//         // public BackEnd BackEnd;
//     }

//     public struct ListenersData
//     {
//         public int[] Id;
//         public float[] Positions;
//         public float[] Directions;
//         public float[] WorldUps;
//         public float[] Cones; //innerangle, outerangle, outergain
//         public float[] Velocities;
//         public int[] Attenuationmodel;
//         public bool[] Activated;
//         public int[] SortedOrder;
//         /*
//             Gain minMAx
//             Distance MinMax
//             Dopplerfactor
//             Fade in milisecond

//         */

//     }

//     public struct EmitterData
//     {


//     }

// }

// namespace RitaEngine.Framework.Audio.Implement
// {
//     using RitaEngine.Framework.Audio.Structures;
//     using RitaEngine.Framework.Audio.Platform;

//     public static class DevicesFeatures
//     {
//         // public static void Init(ref DeviceData device, ref RenderData render,ref DeviceConfig config)
//         // {
//         //     //SELECTED BACKEND
//         // }

//         // public static void Release(ref DeviceData device, ref RenderData render)
//         // {
            
//         // }

//         // private static void InitHelper(ref DeviceData device)
//         // {

//         // }
//     }

//     public  class RendersFeatures
//     {
//         //Receive all data need to play , mix add effect paus stop 
//         //Use BSP and Graph node 
//         public void Add()
//         {

//         }
//         public void Play()
//         {

//         }

//         public void Update()
//         {

//         }
//     }


//     public static class Xaudio2Features
//     {
//         public static void Init( ref Structures.DeviceData device )
//         {
            
//         }
//     }

//     public static class DSOUND{}

//     public static class Pulse{}
// }

// namespace RitaEngine.Framework.Audio.Platform//BAckEnd//OS//Sys//Hardware//INterop // Librairies//Native? for *.h
// {

//     using FLOAT32 = System.Decimal;
//     using UINT32 = System.UInt32;


//     using static RitaEngine.Framework.Audio.Platform.Common;
//     public static unsafe class Common
//     {
//         [StructLayout(LayoutKind.Sequential)]
//         public unsafe struct  GUID{
//             public ulong  Data1;
//             public ushort Data2;
//             public ushort Data3;
//             public fixed byte           Data4[ 8 ];

//             public GUID( ulong d1, ushort d2, ushort d3, byte b1 , byte b2 , byte b3 , byte b4 , byte b5 , byte b6 ,byte b7 ,byte b8)
//             {
//                 Data1 = d1;
//                 Data2 = d2;
//                 Data3 = d3;
//                 Data4[0] = b1;
//                 Data4[1] = b2;
//                 Data4[2] = b3;
//                 Data4[3] = b4;
//                 Data4[4] = b5;
//                 Data4[5] = b6;
//                 Data4[6] = b7;
//                 Data4[7] = b8;
//             }
//         };

//         public enum DSERR : uint
//         {
//             DS_OK                                 = 0,
//             DS_NO_VIRTUALIZATION                   = (0x0878000A),
//             DSERR_ALLOCATED                        = (0x8878000A),
//             DSERR_CONTROLUNAVAIL                   = (0x8878001E),
//             DSERR_INVALIDPARAM                     = (0x80070057),
//             DSERR_INVALIDCALL                      = (0x88780032),
//             DSERR_GENERIC                          = (0x80004005),
//             DSERR_PRIOLEVELNEEDED                  = (0x88780046),
//             DSERR_OUTOFMEMORY                      = (0x8007000E),
//             DSERR_BADFORMAT                        = (0x88780064),
//             DSERR_UNSUPPORTED                      = (0x80004001),
//             DSERR_NODRIVER                         = (0x88780078),
//             DSERR_ALREADYINITIALIZED               = (0x88780082),
//             DSERR_NOAGGREGATION                    = (0x80040110),
//             DSERR_BUFFERLOST                       = (0x88780096),
//             DSERR_OTHERAPPHASPRIO                  = (0x887800A0),
//             DSERR_UNINITIALIZED                    = (0x887800AA),
//             DSERR_NOINTERFACE                      = (0x80004002),
//             DSERR_ACCESSDENIED                     = (0x80070005),
//             DSERR_BUFFERTOOSMALL                   = (0x887800B4),
//             DSERR_DS8_REQUIRED                     = (0x887800BE),
//             DSERR_SENDLOOP                         = (0x887800C8),
//             DSERR_BADSENDBUFFERGUID                = (0x887800D2),
//             DSERR_OBJECTNOTFOUND                   = (0x88781161),
//             DSERR_FXUNAVAILABLE                    = (0x887800DC),
//         }

//     }
    
//     public static unsafe class XAUDIO
//     {

    
//     }

//     public static unsafe class Pulse{
//         public static void Load(IntPtr module){}
//         public static void UnLoad(IntPtr module){}

//     }

//     public static unsafe class DSound{
//         public static void Load(IntPtr module){}
//         public static void UnLoad(IntPtr module){}
//     }

// }

// namespace RitaEngine.Framework.Audio.Data //Tools
// {
//     public static class Convert
//     {
        

//         */
//     }

 

//     public static class Encode{}

//     public static class Decode
//     {
//         public static void ReadPCMFrame(){}
//     }

//     /// <summary>
//     /// Bruit blanc bruit rose ..
//     /// </summary>
//     public static class WaveFormGenerator
//     {

//     }

//     public static class NoiseGenerator{

//     }

//     public static class OGG{}
//     public static class WAV{}
//     public static class MP3 {}
//     public static class FLAC{}
//     public static class XM{}
//     public static class Midi{}
//     public static class OPUS{}
//     public static class MOD{}
// }


//     public class FX
//     {
//        
//     }
//     public enum NetworkConnectionStatus{ None,Disconnected }// see : https://github.com/Syncaidius/MoltenEngine/tree/master/Molten.Engine/Network/Message
//     public static class Console{} //https://github.com/TasThief/ConsoleRenderingSystem/tree/master/FinalGame

