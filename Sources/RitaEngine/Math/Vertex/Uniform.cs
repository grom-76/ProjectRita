namespace RitaEngine.Math;

public struct Uniform_MVP 
{
    public static readonly int  SizeInBytes = 3 * Matrix.SizeInBytes;
    public Matrix Model;
    public Matrix View;
    public Matrix Projection;

    public Uniform_MVP(Matrix model , Matrix view , Matrix projection)
        => (Model, View,Projection )=(model,view,projection);
    
    public Uniform_MVP() { Model = new() ; View = new(); Projection = new(); }

    public void Clone(Uniform_MVP clone )
    {
        Model = clone.Model;
        View = clone.View;
        Projection = clone.Projection ;
    }

    public float[] ToArray => new float[ ]
    {
        Model[0],Model[1],Model[2],Model[3],Model[4],Model[5],Model[6],Model[7],Model[8],Model[9],Model[10],Model[11],Model[12],Model[13],Model[14],Model[15],
        View[0],View[1],View[2],View[3],View[4],View[5],View[6],View[7],View[8],View[9],View[10],View[11],View[12],View[13],View[14],View[15],
        Projection[0],Projection[1],Projection[2],Projection[3],Projection[4],Projection[5],Projection[6],Projection[7],Projection[8],Projection[9],Projection[10],Projection[11],Projection[12],Projection[13],Projection[14],Projection[15],
    };
}


public struct PushConstantsMesh
{
    public Matrix Model = Matrix.Identity;
    public Vector4 Data = new(0.0f);

    public PushConstantsMesh()
    {
    }
}