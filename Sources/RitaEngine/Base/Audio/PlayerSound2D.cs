namespace RitaEngine.Base.Audio;

using System;
using RitaEngine.Base.Platform;
using RitaEngine.Base.Platform.Native.DirectX;
using RitaEngine.Base.Platform.Native.DirectX.XAudio;
using RitaEngine.Base.Platform.Structures;
using RitaEngine.Base.Resources.Sound;

/// <summary>
/// Class de base pour jouer un son stéréo à valeur de test
/// </summary>
public unsafe sealed class PlayerSound2D : IDisposable
{
    private string filename=string.Empty;
    // private XAUDIO2_BUFFER Buffer= new();
    // private WAVEFORMATEX wfx=new();
    private IXAudio2SourceVoice sourcevoice=new();
    // private byte[] Data= null!;
    // private uint Size =0;

    private readonly AudioDeviceData _context;

    public PlayerSound2D(in AudioDevice audiodevice, string filename)
    {
        Log.Info("Init Source Win32");
        _context = audiodevice.GetData();
        //  DECODE WAV 
        WaveReader wav = new(filename!);
        wav.ReadHeader();
        //use own readfile wav ?
        // uint wavSizeInBytes = wav.DataSize;
        uint Size =  wav.DataSize;
        byte[] Data =new byte[Size];
        wav.ReadChunk(ref Data, Size);
        Log.Info(wav.ToString());
#if WIN64       
        // CREATE SOURCE
        WAVEFORMATEX wfx=new();
        wfx.cbSize =0;//no extra info
        wfx.nChannels = (ushort)wav.Nbrcanaux; // 1;//2 = stereo
        wfx.nSamplesPerSec = wav.Frequence; // 44100;
        wfx.wBitsPerSample =(ushort)wav.BitsPerSample;//16; //8 or 24 or 32
        wfx.nBlockAlign = (ushort)wav.BytePerBloc;
        wfx.nAvgBytesPerSec = wav.BytePerSec;// wfx.nBlockAlign * wfx.nSamplesPerSec;
        wfx.wFormatTag =(ushort)wav.AudioFormat ;// (ushort)wav.AudioFormat;// 3;//WAVE_FORMAT_PCM;? see list ?
        
        //CREATE BUFFER AND SATTACH TO SOURC
        XAUDIO2_BUFFER Buffer= new();
        Buffer.AudioBytes = Size;
        fixed( byte* bytes = &Data[0] ){
            Buffer.pAudioData = bytes;
        }
        Buffer.Flags = Constants.XAUDIO2_END_OF_STREAM;

        // Buffer.LoopBegin =1 ;
        // Buffer.LoopLength =0;
        // Buffer.LoopCount =XAUDIO2_LOOP_INFINITE;
        // Buffer.pContext = null;
#endif      
        uint err =_context.Instance.CreateSourceVoice( ref sourcevoice ,wfx);
        Log.Info ($"Create Source voice Error Code : {err} adr : 0X{sourcevoice.AddressHexa}");

        err = sourcevoice.SubmitSourceBuffer(ref Buffer);
        Log.Info ($"Submit source buffer  Error Code : {err} ");
    }

    public void PlaySource()=> sourcevoice.Start();
    public void Stop()=> sourcevoice.Stop();

    public void Dispose()
    {
        Log.Info(" Dispose Source Win32");
        sourcevoice.Stop();
        sourcevoice.DestroyVoice();

        GC.SuppressFinalize(this);
    }
}
