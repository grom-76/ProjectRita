namespace RitaEngine.Base.Math.Vertex;

public struct Uniform_MVP 
{
    public Matrix Model;
    public Matrix View;
    public Matrix Projection;

    public unsafe void* AddressOfPtrThis( ){fixed (void* pointer = &this)  { return( pointer ) ; }  }

    public Uniform_MVP(Matrix model , Matrix view , Matrix projection)
        => (Model, View,Projection )=(model,view,projection);
    
    public Uniform_MVP() { Model = new() ; View = new(); Projection = new(); }
}

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

public struct Position3f_Color3f_UV2f
{
    public Vector3 Position;
    public Vector3 Color;
    public Vector2 UV;

    public static readonly int Stride = sizeof(float)*8;
    public static readonly int OffsetPosition =  Marshal.OffsetOf<Position3f_Color3f_UV2f>( nameof(Position)).ToInt32();
    public static readonly int OffsetColor =  Marshal.OffsetOf<Position3f_Color3f_UV2f>( nameof(Color)).ToInt32();
    public static readonly int OffsetUV =  Marshal.OffsetOf<Position3f_Color3f_UV2f>( nameof(UV)).ToInt32();
    public uint FormatPosition = (uint)RitaEngine.Base.Platform.API.Vulkan.VkFormat.VK_FORMAT_R32G32B32_SFLOAT;
    public uint FormatColor =(uint)RitaEngine.Base.Platform.API.Vulkan.VkFormat.VK_FORMAT_R32G32B32_SFLOAT;
    public uint FormatUV =(uint)RitaEngine.Base.Platform.API.Vulkan.VkFormat.VK_FORMAT_R32G32_SFLOAT;


    public Position3f_Color3f_UV2f( float x , float y , float r , float g , float b , float u , float v)
    {
        Position = new( x,y);
        Color = new( r,g,b);
        UV = new( u,v);

    }

    public Position3f_Color3f_UV2f( Vector3 position , Vector3 color , Vector2 uv)
    {
        Position = new( position.X,position.X);
        Color = new( color.X,color.Y, color.Z);
        UV = new(uv.X,uv.Y );
    }

    public float[] ToArray => new float[]{ Position.X,Position.Y, Position.Z, Color.R,Color.G,Color.B ,UV.X ,UV.Y };
    
}