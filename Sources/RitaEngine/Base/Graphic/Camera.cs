

namespace RitaEngine.Base.Graphic;


using RitaEngine.Base.Math;

public struct Camera
{
    Matrix Model = Matrix.Identity;
    Matrix View=Matrix.Identity;
    Matrix Projection=Matrix.Identity;

    /// <summary>
    /// Called too Position
    /// </summary>
    /// <returns></returns>
    public Vector3 Eye =new(0.0f,0.0f,5.0f);
    public Vector3 Target =new(0.0f,0.0f,0.0f);
    public Vector3 Up =new(0.0f,1.0f,0.0f);
    public float FieldOfViewInDegree = 45.0f;

    
    public Camera()
    {

    }



    public void BuildCamera( )
    {
        Model =  RitaEngine.Base.Math.Matrix.Identity;
        View =  Matrix.LookAtTo(Eye,Target,Up); //Matrix.LookAt( Eye,  Target ,  Up );
        Projection =   Matrix.PerspectiveFOV(45.0f,1280.0f,720.0f,0.1f,100.0f ) ; //Matrix.PerspectiveFov(Helper.ToRadians( 90.0f ) ,1.0f,0.0f,1000.0f )  ; 
        Projection[1,1] *= -1;
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


}
