namespace RitaEngine.Base.Graphic;


using RitaEngine.Base.Math;



public struct Camera :IEquatable<Camera>
{
    Matrix Model = Matrix.Identity;
    Matrix View=Matrix.Identity;
    Matrix Projection=Matrix.Identity;

    /// <summary>
    /// Called too Position
    /// </summary>
    /// <returns></returns>
    public Vector3 Eye =new(2.0f,2.0f,2.0f);
    public Vector3 Target =new(0.00f,0.00f,0.00f);
    public Vector3 Up =new(0.0f,-1.0f,1.0f);
    public float FieldOfViewInDegree = 45.0f;

    
    public Camera() { }

    public void BuildCamera( )
    {
        Model =  RitaEngine.Base.Math.Matrix.Identity;
        Matrix.CreateLookAt( ref Eye ,ref Target, ref Up, out View);
        Matrix.CreatePerspectiveFieldOfView( Helper.ToRadians( FieldOfViewInDegree) ,(1280.0f/720.0f), 0.1f,100.0f,out Projection );
        Projection.M22 *= -1;
        //https://computergraphics.stackexchange.com/questions/12448/vulkan-perspective-matrix-vs-opengl-perspective-matrix
        // Matrix.MakeProjectionMatrixWithoutFlipYAxis( Helper.ToRadians( FieldOfViewInDegree) ,(1280.0f/720.0f), 0.1f,100.0f,out Projection );
    }

    public void ScalingWorld( Vector3 scale)
    {
        Model= RitaEngine.Base.Math.Matrix.Scaling(scale);
    }

    public float[] ToArray => new float[ ]
    {
        Model[0],Model[1],Model[2],Model[3],Model[4],Model[5],Model[6],Model[7],Model[8],Model[9],Model[10],Model[11],Model[12],Model[13],Model[14],Model[15],
        View[0],View[1],View[2],View[3],View[4],View[5],View[6],View[7],View[8],View[9],View[10],View[11],View[12],View[13],View[14],View[15],
        Projection[0],Projection[1],Projection[2],Projection[3],Projection[4],Projection[5],Projection[6],Projection[7],Projection[8],Projection[9],Projection[10],Projection[11],Projection[12],Projection[13],Projection[14],Projection[15],
    };

    public void Release()
    {
        
    }

    #region OVERRIDE    
    public override string ToString() => string.Format($"Camera Manager? " );
    public override int GetHashCode() => HashCode.Combine(  Eye, Target );
    public override bool Equals(object? obj) => obj is Camera  camera && this.Equals(camera) ;
    public bool Equals(Camera other)=>  Eye.Equals(other.Eye) ;
    public static bool operator ==(Camera  left,Camera right) => left.Equals(right);
    public static bool operator !=(Camera  left,Camera right) => !left.Equals(right);
    #endregion
}
