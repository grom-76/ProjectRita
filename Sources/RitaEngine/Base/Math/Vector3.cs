namespace RitaEngine.Base.Math;

using static RitaEngine.Base.Math.Helper;

[StructLayout(LayoutKind.Sequential),SkipLocalsInit]
public struct Vector3 : IEquatable<Vector3> , IDisposable
{
#region Default value        
    private static readonly Vector3 zero = AllocNew(0.0f,0.0f,0.0f);
    public static ref readonly Vector3 Zero => ref zero;
    public static readonly Vector3 one = AllocNew(1.0f,1.0f,1.0f);
    public static ref readonly Vector3 One  => ref one;
    public static readonly int SizeOf = Marshal.SizeOf<Vector3>();

#endregion
#region PRIVATE
#endregion        
#region ACCESSOR
    /// <summary> Color R  </summary>
    public  readonly float R => X;
    /// <summary> Color G  </summary>
    public readonly float G => Y;
    /// <summary> Color B  </summary>
    public readonly float B => Z;
    /// <summary>Viewport Left   </summary>
    public readonly float Left => X;
    /// <summary>Viewport Top   </summary>
    public readonly float Top => Y;
    /// <summary>Viewport Width   </summary>
    public readonly float Width => Z;

    /// <summary>Axe  Abcisse X  </summary>
    public float X { get; set; }
    /// <summary> Axe Ordonnée  </summary>
    public float Y { get; set; }
    /// <summary> Axe Z ( 3D )  </summary>
    public float Z { get; set; }

    /// <summary>
    /// Retourne la valeur abcisse ou ordonée utilisation comme un tableau  vecteur2[0] ou vecteur2[1]
    /// </summary>
    /// <value></value>
    public float this[int index]
    {
        get => index switch
        {
            0 => X,
            1 => Y,
            2 => Z,
            _ => float.NaN
        };
        set => _ = index switch
        {
            0 => X = value,
            1 => Y = value,
            2 => Z = value,
            _ => float.NaN
        };
    }
#endregion        
#region OVERRIDE        
    public override string ToString()  => $"[X={X:G3};Y={Y:G3};Z={Z:G3}]";
    public override int GetHashCode() => this.X.GetHashCode() + this.Y.GetHashCode() + this.Z.GetHashCode() ;
    public override bool Equals(object? obj) => obj is Vector3 vec && this.Equals(vec)  ;
    public bool Equals(Vector3 other) => (Abs(X - other.X) <= ZeroTolerance) && (Abs(Y - other.Y) <= ZeroTolerance) && (Abs(Z - other.Z) <= ZeroTolerance) ;
    public static bool operator ==(Vector3 left, Vector3 right) => left.Equals(right);
    public static bool operator !=(Vector3 left, Vector3 right) => !left.Equals(right);
#endregion
#region ALLLOCATE
    /// <summary>
    /// Allocation sur la pile
    /// </summary>
    /// <param name="value_x"></param>
    /// <param name="value_y"></param>
    /// <param name="value_z"></param>
    /// <returns></returns>
    public static Vector3 Alloc( float value_x , float value_y , float value_z )
    {
        Vector3 vec = default;
        vec.X = value_x;
        vec.Y = value_y;
        vec.Z = value_z;
        return vec;
    }
    /// <summary>
    /// Allocation sur le tas ( usage de new )
    /// </summary>
    /// <returns></returns>
    public static Vector3 AllocNew( float value_x, float value_y, float value_z )
        => new(value_x,  value_y, value_z);
#endregion        
#region OPERATION BINAIRE

    public static Vector3 operator *(Vector3 left, Vector3 right)
        => Alloc(left.X * right.X, left.Y * right.Y, left.Z* right.Z);

    public static void MultiplyVec4ByVec4(ref Vector3 result, ref Vector3 left, ref Vector3 right)
    => ( result.X,result.Y,result.Z )=(left.X * right.X, left.Y * right.Y, left.Z* right.Z);

    public static Vector3 operator *(float scale, Vector3 value)
        => Alloc(value.X * scale, value.Y * scale, value.Z *scale);

    public static Vector3 operator *(Vector3 value, float scale)
        => Alloc(value.X * scale, value.Y * scale, value.Z *scale);

    public static void MultiplyVector3ByScalar( ref Vector3 result, ref Vector3 value , float scalar)
        => (result.X, result.Y, result.Z) = (value.X*scalar, value.Y*scalar, value.Z*scalar);

    public static Vector3 operator +(Vector3 left, Vector3 right)
        => Alloc(left.X + right.X, left.Y + right.Y , left.Z+right.Z);

    public static void AddVec4WithVec4(ref Vector3 result, ref Vector3 left, ref Vector3 right)
        => ( result.X,result.Y,result.Z)=(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

    public static Vector3 operator +(Vector3 value, float scalar)
        => Alloc(value.X + scalar, value.Y + scalar , value.Z + scalar) ;

    public static Vector3 operator +(float scalar, Vector3 value)
        => Alloc(scalar + value.X, scalar + value.Y, scalar + value.Z);

    public static Vector3 operator /(Vector3 value, float scale)
        => Alloc( value.X/scale , value.Y/ scale , value.Z/scale  );

    public static Vector3 operator -(Vector3 value, float scalar)
        => Alloc(value.X - scalar, value.Y - scalar, value.Z - scalar);

    public static Vector3 operator -(float scalar, Vector3 value)
        => Alloc(scalar - value.X, scalar - value.Y, scalar- value.Z);

    public static Vector3 operator - (Vector3 left, Vector3 right)
        => Alloc(left.X - right.X, left.Y - right.Y,  left.Z - right.Z);

    public static Vector3 operator - (Vector3 value)
    {
        value.X = -value.X;
        value.Y = - value.Y;
        value.Z = - value.Z;
        return value;
    }
#endregion
#region Trigonometrie    
    /// <summary>
    /// Normalize n vector ( perdendiculaire )
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector3 Normalize( Vector3 v)
    {
        float length = 1/ v.Length();
        return Alloc( v.X*length, v.Y*length,v.Z*length );
    }

    /// <summary>
    /// Produit de 2 vecteur  cross product pour trouver la perpendiculaire a 2 vecteur ( normale)
    /// The cross product is only defined in 3D space and takes two non-parallel vectors as input and produces a third vector that is orthogonal to both the input vectors. If both the input vectors are orthogonal to each other as well, a cross product would result in 3 orthogonal vectors; this will prove useful in the upcoming chapters. The following image shows what this looks like in 3D space:
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    public static Vector3 Cross(ref Vector3 v1,ref Vector3 v2)
        => Alloc( (v1.Y * v2.Z) - (v1.Z * v2.Y),
                (v1.Z * v2.X) - (v1.X * v2.Z),
                (v1.X * v2.Y) - (v1.Y * v2.X));
    /// <summary>
    /// Produit de 2 vecteur  cross product pour trouver la perpendiculaire a 2 vecteur ( normale)
    /// </summary>
    public static void Cross(ref Vector3 result , ref Vector3 v1,ref  Vector3 v2)
        => ( result.X,result.Y,result.Z ) = (
            (v1.Y * v2.Z) - (v1.Z * v2.Y),
            (v1.Z * v2.X) - (v1.X * v2.Z),
            (v1.X * v2.Y) - (v1.Y * v2.X));

    /// <summary>
    /// dot product cosinus de l'angle entre les deux vecteurs
    /// The dot product of two vectors is equal to the scalar product of their lengths times the cosine of the angle between them.
    /// si le resultat est 1 ( cos 90°) les deux vecteur sont orthogonaux  si result =0  angle =0 il sont parallel
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(ref Vector3  v1,ref Vector3  v2)
        => (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z);

    /// <summary>
    /// Distance entre deux vecteur 
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(ref Vector3 value1,ref Vector3 value2)
        =>  Sqrt(((value1.X - value2.X) * (value1.X - value2.X)) + ((value1.Y - value2.Y) * (value1.Y - value2.Y)) + ((value1.Z - value2.Z) * (value1.Z - value2.Z)) );
#endregion
#region Public
            /// <summary>
    /// Constructeur complet 
    /// </summary>
    /// <param name="x"> axe x </param>
    /// <param name="y">axe y </param>
    /// <param name="z"></param>
    /// <returns></returns>
    public Vector3(float x=0.0f, float y=0.0f, float z=0.0f) 
        =>(X, Y, Z) = (x,y,z);

    // /// <summary>
    // /// Equivelent construteur de Copie
    // /// </summary>
    // /// <param name="v"></param>
    // /// <param name="z"></param>
    // /// <param name="w"></param>
    // /// <returns></returns>
    // public Vector3( Vector2 v, float z=0.0f, float w=0.0f)
    //     =>(_x,_y,_z,_w)=( v.X,v.Y, z,w);

    /// <summary>
    /// Constrcuteur a partir d'un tableau de flotant
    /// </summary>
    /// <param name="floats"></param>
    /// <returns></returns>
    public Vector3( float[] floats)
        =>(X, Y, Z) =( floats[0],floats[1],floats[2] );

    /// <summary>
    /// Instanciate a partir d'un scalaire
    /// </summary>
    /// <param name="scalar"></param>
    public Vector3(float scalar)
        =>(X, Y, Z) = ( scalar,scalar,scalar);

    public Vector3( Vector3 vec4)
        => (X, Y, Z) = ( vec4.X,vec4.Y,vec4.Z);
    /// <summary>
    /// Normalize un vecteur
    /// </summary>
    public void Normalize()
    {
        float length = 1/ this.Length();
        this.X *= length;
        this.Y *= length;
        this.Z *= length;
    }
    /// <summary>
    ///  le negatif de chaque valeurs
    /// </summary>
    public void Negate()
        =>(X, Y, Z) =(-X, -Y, -Z);
    
    /// <summary>
    /// To retrieve the length/magnitude of a vector we use the Pythagoras theorem that you may remember from your math classes. 
    /// A vector forms a triangle when you visualize its individual x and y component as two sides of a triangle:
    /// </summary>
    /// <returns>Magnitude</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Length()
        =>  Sqrt((X * X) + (Y * Y) + (Z * Z) );

    /// <summary>
    /// retourn Vector3 sous forme de tableu de reel float[4] sans utiliser new
    /// </summary>
    /// <returns></returns>
    public float[] ToArrayStaticalloc()
    {
        float[] r ={ X, Y, Z }; 
        return r;
    }
    /// <summary>
    /// retourn Vector3 sous forme de tableu de reel float[4]
    /// </summary>
    /// <value></value>
    public float[] ToArray => new float[]{ X, Y, Z };
    /// <summary>
    /// normalement sa devrait pas marché car Suppressfinalize ne fonctionne pas sur une structure 
    /// </summary>
    public void Dispose()
    {
        #pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        GC.SuppressFinalize(this);
        #pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    }
#endregion        
}

