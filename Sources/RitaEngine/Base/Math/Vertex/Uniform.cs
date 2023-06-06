namespace RitaEngine.Base.Math.Vertex;

    // public struct TextureUV{}
    //      public struct Color3{}   
       

    //      public struct Scalar{} // 1D 
    //      public struct Position3D{}
    //         public struct Position2D{}
    //           public struct Color4{}

    //            public struct Normal{}

    //             public struct Layout{} // 1D 

public struct Uniform_MVP 
{
    public Matrix Model;
    public Matrix View;
    public Matrix Projection;

    public Uniform_MVP(Matrix model , Matrix view , Matrix projection)
        => (Model, View,Projection )=(model,view,projection);
};


public struct Position2f_Color3f
{
    public Vector2 Position;
    public Vector3 Color;

    public int Stride = Unsafe.SizeOf<Position2f_Color3f>();
    public nint OffsetPosition =  Marshal.OffsetOf<Position2f_Color3f>( nameof(Position));
    public nint OffsetColor =  Marshal.OffsetOf<Position2f_Color3f>( nameof(Color));
    public uint FormatPosition = (uint)RitaEngine.Base.Platform.API.Vulkan.VkFormat.VK_FORMAT_R32G32_SFLOAT;
    public uint FormatColor =(uint)RitaEngine.Base.Platform.API.Vulkan.VkFormat.VK_FORMAT_R32G32B32_SFLOAT;


    public Position2f_Color3f( float x , float y , float r , float g , float b)
    {
        Position = new( x,y);
        Color = new( r,g,b);
    }

    public Position2f_Color3f( Vector2 position , Vector3 color)
    {
        Position = new( position.X,position.X);
        Color = new( color.X,color.Y, color.Z);
    }

    public float[] ToArray => new float[]{ Position.X,Position.Y, Color.R,Color.G,Color.B};
    
}
