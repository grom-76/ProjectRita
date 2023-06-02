namespace RitaEngine.Base.Audio;


public struct AudioBuffer<TDataFormat>
{
    // Buffer sound
    public uint Id = 0;// TODO : ALL Id Are GUID 
    public TDataFormat[] data = null!;
    public int Frequence=0;
    //         public int BufferId;//Pool stream size  name, crop  +>start  end  size
//         public long SizeKBytes;
//         public int SampleRate;
//         public int SampleSize;
//         public int CurrentPosition;
//         public int Channel ; //always 1 MOno for 3D sound

    public AudioBuffer()
    {
    }

    // public AudioFormat Format = 0;

    /// <summary>
    /// Tailledu buffer en byte
    /// </summary>
    public int Length => data==null ? 0: data.Length;
}
  

