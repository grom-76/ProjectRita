namespace RitaEngine.Math;


[StructLayout(LayoutKind.Sequential)]
public struct Vertex
{
    public static readonly int Stride = sizeof(float)*8;
    public static readonly int OffsetPosition =  Marshal.OffsetOf<Vertex>( nameof(Position)).ToInt32();
    public static readonly int OffsetNormal =  Marshal.OffsetOf<Vertex>( nameof(Normal)).ToInt32();
    public static readonly int OffsetTexCoord =  Marshal.OffsetOf<Vertex>( nameof(TexCoord)).ToInt32();
    public static readonly uint FormatPosition = (uint)RitaEngine.API.Vulkan.VkFormat.VK_FORMAT_R32G32B32_SFLOAT;
    public static readonly uint FormatNormal =(uint)RitaEngine.API.Vulkan.VkFormat.VK_FORMAT_R32G32B32_SFLOAT;
    public static readonly uint FormatUV =(uint)RitaEngine.API.Vulkan.VkFormat.VK_FORMAT_R32G32_SFLOAT;

    public Vector3 Position;
    public Vector3 Normal;
    public Vector2 TexCoord;

    public Vector3 Color => Normal;

    public Vertex(Vector3 p, Vector3 n, Vector2 uv)
    {
        Position = p;
        Normal = n;
        TexCoord = uv;
    }

    public Vertex(
        float px, float py, float pz,
        float nx, float ny, float nz,
        float u, float v)
    {
        Position = new Vector3(px, py, pz);
        Normal = new Vector3(nx, ny, nz);
        TexCoord = new Vector2(u, v);
    }

    public float[] ToArray => new float[]{ Position.X,Position.Y, Position.Z, Normal.X,Normal.Y,Normal.Z ,TexCoord.X ,TexCoord.Y };
}

public class GeometricPrimitive : IDisposable
{
    public Vertex[] Vertices { get; }
    public int[] Indices { get; }

    public GeometricPrimitive(Vertex[] vertices, int[] indices)
    {
        Vertices = vertices;
        Indices = indices;
    }
    public GeometricPrimitive( )
    {
        Vertices = null!;
        Indices = null!;
    }

    public short[] IndicesToArray()
    {
        short[] indices = new short[ Indices.Length];
        for(int i = 0 ; i < Indices.Length ; i++)
        {
            indices[i] = (short)Indices[i];
        }
        return indices; 
    }

    public float[] VertexToArray()
    {
        float[] vertices = new float[ Vertices.Length * 8 ];
 
        for( int i =0 ; i <  Vertices.Length ; i++ )
        {
            var start  = 8 * i ;
            for( int y =0 ; y < 8 ; y++ )
            {
                vertices[start + y] = Vertices[i].ToArray[y];
            }
        }


        return vertices;
    }
    
    public static GeometricPrimitive CreateBox(float width, float height, float depth)
    {
        //Code from Discosultan
        float w2 = 0.5f * width;
        float h2 = 0.5f * height;
        float d2 = 0.5f * depth;

        Vertex[] vertices =
        {
            // Fill in the front face vertex data.
            new Vertex(-w2, +h2, -d2, +0, +0, -1, +0, +0),
            new Vertex(-w2, -h2, -d2, +0, +0, -1, +0, +1),
            new Vertex(+w2, -h2, -d2, +0, +0, -1, +1, +1),
            new Vertex(+w2, +h2, -d2, +0, +0, -1, +1, +0),
            // Fill in the back face vertex data.
            new Vertex(-w2, +h2, +d2, +0, +0, +1, +1, +0),
            new Vertex(+w2, +h2, +d2, +0, +0, +1, +0, +0),
            new Vertex(+w2, -h2, +d2, +0, +0, +1, +0, +1),
            new Vertex(-w2, -h2, +d2, +0, +0, +1, +1, +1),
            // Fill in the top face vertex data.
            new Vertex(-w2, -h2, -d2, +0, +1, +0, +0, +0),
            new Vertex(-w2, -h2, +d2, +0, +1, +0, +0, +1),
            new Vertex(+w2, -h2, +d2, +0, +1, +0, +1, +1),
            new Vertex(+w2, -h2, -d2, +0, +1, +0, +1, +0),
            // Fill in the bottom face vertex data.
            new Vertex(-w2, +h2, -d2, +0, -1, +0, +1, +0),
            new Vertex(+w2, +h2, -d2, +0, -1, +0, +0, +0),
            new Vertex(+w2, +h2, +d2, +0, -1, +0, +0, +1),
            new Vertex(-w2, +h2, +d2, +0, -1, +0, +1, +1),
            // Fill in the left face vertex data.
            new Vertex(-w2, +h2, +d2, -1, +0, +0, +0, +0),
            new Vertex(-w2, -h2, +d2, -1, +0, +0, +0, +1),
            new Vertex(-w2, -h2, -d2, -1, +0, +0, +1, +1),
            new Vertex(-w2, +h2, -d2, -1, +0, +0, +1, +0),
            // Fill in the right face vertex data.
            new Vertex(+w2, +h2, -d2, +1, +0, +0, +0, +0),
            new Vertex(+w2, -h2, -d2, +1, +0, +0, +0, +1),
            new Vertex(+w2, -h2, +d2, +1, +0, +0, +1, +1),
            new Vertex(+w2, +h2, +d2, +1, +0, +0, +1, +0)
        };

        int[] indices =
        {
            // Fill in the front face index data.
            0, 1, 2, 0, 2, 3,
            // Fill in the back face index data.
            4, 5, 6, 4, 6, 7,
            // Fill in the top face index data.
            8, 9, 10, 8, 10, 11,
            // Fill in the bottom face index data.
            12, 13, 14, 12, 14, 15,
            // Fill in the left face index data
            16, 17, 18, 16, 18, 19,
            // Fill in the right face index data
            20, 21, 22, 20, 22, 23
        };

        return new GeometricPrimitive(vertices, indices);
    }

        public static GeometricPrimitive CreateQuad(float width, float height, float depth=0.0f)
    {
        float w2 = 0.5f * width;
        float h2 = 0.5f * height;
        float d2 = 0.5f * depth;

        Vertex[] vertices =
        {
            // Fill in the front face vertex data.
            new Vertex(-w2, +h2, -d2, +0, +0, -1, +0, +0),
            new Vertex(-w2, -h2, -d2, +0, +0, -1, +0, +1),
            new Vertex(+w2, -h2, -d2, +0, +0, -1, +1, +1),
            new Vertex(+w2, +h2, -d2, +0, +0, -1, +1, +0),
          //     new(-0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f),
    //     new(0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f),
    //     new(0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f),
    //     new(-0.5f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f)
        };

        int[] indices =
        {
            // Fill in the front face index data.
            0, 1, 2, 0, 2, 3,
   
        };

        return new GeometricPrimitive(vertices, indices);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }


    // public Position3f_Color3f_UV2f[] Vertices = new Position3f_Color3f_UV2f[] 
    // {
    //     new(-0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f),
    //     new(0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f),
    //     new(0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f),
    //     new(-0.5f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f)
    // };
    // public short[] Indices = new short[] { 0, 1, 2, 2, 3, 0};
}
