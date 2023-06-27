namespace RitaEngine.Audio;


using RitaEngine.Math;

public struct AudioListener
{
    /// <summary>
    /// Determine la position point dans l'esapce de l'écouteur
    /// </summary>
    public Vector3 Position;

    /// <summary>
    ///  determine la vitesse de déplacement de l'écouteur
    /// exemple dans un fps l'écouteur c'est le joueur et il ce déplace
    /// </summary>
    public Vector3 Velocity;

    /// <summary>
    /// Détermine l'orientation de l'écouteur ( cone comme la caméra )
    /// </summary>    
    public Vector3 Target;
    /// <summary>
    /// Détermine le postionnnement tête haute de l'écouteur ( cone comme la caméra )
    /// </summary>    
    public Vector3 Up;
        
    /// <summary>
    ///  Volume général du son mini 0.0f, maxi 1.0f
    /// </summary>
    public float Volume;
}
   

