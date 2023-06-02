

namespace RitaEngine.Base.Audio;

using RitaEngine.Base.Math;
/// <summary>
///  AUdio source see limit ? 
/// </summary>
public struct AudioEmitter
{
    public uint Id;
    public Vector3 Position;// position de départ en 3D
    public Vector3 Velocity;// vitesse deplacement de la source en 3D
    public float Pitch = 1.0f; // vitesse play
    public float Gain =1.0f; // egal volume
    public bool Loop =false;// repeat
    public bool IsBufferAttached = false;
    public uint BufferID=0;
    //         public uint Order;
//         public int Id;
//         public bool IsPlayable;//indique si l'emitter n'est pas disparu/mort => mise en cache en cas de réutilisation
//         public bool IsPlaying;
//         public long Loop;
//         public float Volume;
//         public float Pitch;
//         public float Pan;
//         public float Reverb;//For effects

    public AudioEmitter()
    {
    }
} // need source and buffer exemple bird  sifflement => in forest has many birds many emitters but one buffer and source
   

