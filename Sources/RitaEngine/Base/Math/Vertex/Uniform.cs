namespace RitaEngine.Base.Math;

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




//called PRIMITIVE

// public struct Position2f_Color3f
// {
//     public Vector2 Position;
//     public Vector3 Color;

//     public int Stride = Unsafe.SizeOf<Position2f_Color3f>();
//     public nint OffsetPosition =  Marshal.OffsetOf<Position2f_Color3f>( nameof(Position));
//     public nint OffsetColor =  Marshal.OffsetOf<Position2f_Color3f>( nameof(Color));
//     public uint FormatPosition = (uint)RitaEngine.Base.Platform.API.Vulkan.VkFormat.VK_FORMAT_R32G32_SFLOAT;
//     public uint FormatColor =(uint)RitaEngine.Base.Platform.API.Vulkan.VkFormat.VK_FORMAT_R32G32B32_SFLOAT;


//     public Position2f_Color3f( float x , float y , float r , float g , float b)
//     {
//         Position = new( x,y);
//         Color = new( r,g,b);
//     }

//     public Position2f_Color3f( Vector2 position , Vector3 color)
//     {
//         Position = new( position.X,position.X);
//         Color = new( color.X,color.Y, color.Z);
//     }

//     public float[] ToArray => new float[]{ Position.X,Position.Y, Color.R,Color.G,Color.B};
    
// }

// public struct Position3f_Color3f_UV2f
// {
//     public Vector3 Position;
//     public Vector3 Color;
//     public Vector2 UV;

//     public static readonly int Stride = sizeof(float)*8;
//     public static readonly int OffsetPosition =  Marshal.OffsetOf<Position3f_Color3f_UV2f>( nameof(Position)).ToInt32();
//     public static readonly int OffsetColor =  Marshal.OffsetOf<Position3f_Color3f_UV2f>( nameof(Color)).ToInt32();
//     public static readonly int OffsetUV =  Marshal.OffsetOf<Position3f_Color3f_UV2f>( nameof(UV)).ToInt32();
//     public uint FormatPosition = (uint)RitaEngine.Base.Platform.API.Vulkan.VkFormat.VK_FORMAT_R32G32B32_SFLOAT;
//     public uint FormatColor =(uint)RitaEngine.Base.Platform.API.Vulkan.VkFormat.VK_FORMAT_R32G32B32_SFLOAT;
//     public uint FormatUV =(uint)RitaEngine.Base.Platform.API.Vulkan.VkFormat.VK_FORMAT_R32G32_SFLOAT;


//     public Position3f_Color3f_UV2f( float x , float y , float r , float g , float b , float u , float v)
//     {
//         Position = new( x,y);
//         Color = new( r,g,b);
//         UV = new( u,v);

//     }

//     public Position3f_Color3f_UV2f( Vector3 position , Vector3 color , Vector2 uv)
//     {
//         Position = new( position.X,position.X);
//         Color = new( color.X,color.Y, color.Z);
//         UV = new(uv.X,uv.Y );
//     }

//     public float[] ToArray => new float[]{ Position.X,Position.Y, Position.Z, Color.R,Color.G,Color.B ,UV.X ,UV.Y };

        
// }


