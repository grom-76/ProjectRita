namespace RitaEngine.Base.Math;

using static RitaEngine.Base.Math.Helper;

[SkipLocalsInit,StructLayout(LayoutKind.Sequential, Pack = 4 )]//pack 4 aligned on float => 
public struct Vector2 : IEquatable<Vector2> , IDisposable
{
#region Default value        
    private static readonly Vector2 zero = AllocNew(0.0f,0.0f);

    public static ref readonly Vector2 Zero => ref zero;

    public static readonly Vector2 one = AllocNew(1.0f,1.0f);

    public static ref readonly Vector2 One  => ref one;

    public static readonly int SizeOf = Marshal.SizeOf<Vector2>();

#endregion
#region PRIVATE
    // private float _z;
    #endregion
    #region ACCESSOR

    /// <summary>Axe  Abcisse X  </summary>
    public float X { get; set; }
    /// <summary> Axe Ordonnée  </summary>
    public float Y { get; set; }
  

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
            _ => float.NaN
        };
        set => _ = index switch
        {
            0 => X = value,
            1 => Y = value,
            _ => float.NaN
        };
    }
#endregion        
#region OVERRIDE        
    /// <summary>
    /// Vector 4 sous forme de chaine de caractère avec  le mot Vector2 et passage a la ligne pour les valeurs
    /// </summary>
    /// <returns></returns>
    public override string ToString()  => $"[{X:G3};{Y:G3}]";

    public override int GetHashCode() =>HashCode.Combine( X,Y);

    public override bool Equals(object? obj) => obj is Vector2 vec && this.Equals(vec)  ;

    public bool Equals(Vector2 other) => (Abs(X - other.X) <= ZeroTolerance) && (Abs(Y - other.Y) <= ZeroTolerance)  ;

    public static bool operator ==(Vector2 left, Vector2 right) => left.Equals(right);

    public static bool operator !=(Vector2 left, Vector2 right) => !(left.Equals(right));
#endregion
#region ALLLOCATE
    /// <summary>
    /// Allocation sur la pile
    /// </summary>
    /// <param name="value_x"></param>
    /// <param name="value_y"></param>
    /// <param name="value_z"></param>
    /// <returns></returns>
    public static Vector2 Alloc( float value_x , float value_y  )
    {
        Vector2 vec = default;
        vec.X = value_x;
        vec.Y = value_y;
        // vec._z = value_z;
        return vec;
    }
    /// <summary>
    /// Allocation sur le tas ( usage de new )
    /// </summary>
    /// <returns></returns>
    public static Vector2 AllocNew( float value_x, float value_y)
        => new (value_x,  value_y);
#endregion        
#region OPERATION BINAIRE

    public static Vector2 operator *(Vector2 left, Vector2 right)
        => Alloc(left.X * right.X, left.Y * right.Y);

    public static void MultiplyVec2ByVec2(ref Vector2 result, ref Vector2 left, ref Vector2 right)
    => ( result.X,result.Y )=(left.X * right.X, left.Y * right.Y);

    public static Vector2 operator *(float scale, Vector2 value)
        => Alloc(value.X * scale, value.Y * scale);

    public static Vector2 operator *(Vector2 value, float scale)
        => Alloc(value.X * scale, value.Y * scale);

    public static void MultiplyVector2ByScalar( ref Vector2 result, ref Vector2 value , float scalar)
        => (result.X, result.Y) = (value.X*scalar, value.Y*scalar);

    public static Vector2 operator +(Vector2 left, Vector2 right)
        => Alloc(left.X + right.X, left.Y + right.Y );

    public static void AddVec2WithVec2(ref Vector2 result, ref Vector2 left, ref Vector2 right)
        => ( result.X,result.Y)=(left.X + right.X, left.Y + right.Y);

    public static Vector2 operator +(Vector2 value, float scalar)
        => Alloc(value.X + scalar, value.Y + scalar) ;

    public static Vector2 operator +(float scalar, Vector2 value)
        => Alloc(scalar + value.X, scalar + value.Y);

    public static Vector2 operator /(Vector2 value, float scale)
        => Alloc( value.X/scale , value.Y/ scale   );

    public static Vector2 operator -(Vector2 value, float scalar)
        => Alloc(value.X - scalar, value.Y - scalar);

    public static Vector2 operator -(float scalar, Vector2 value)
        => Alloc(scalar - value.X, scalar - value.Y);

    public static Vector2 operator - (Vector2 left, Vector2 right)
        => Alloc(left.X - right.X, left.Y - right.Y);

    public static Vector2 operator - (Vector2 value)
    {
        value.X = -value.X;
        value.Y = - value.Y;
        return value;
    }
#endregion
#region Trigonometrie    

    public static Vector2 Normalize( Vector2 v)
    {
        float length = 1/ v.Length();
        return Alloc( v.X*length, v.Y*length );
    }

    // /// <summary>
    // /// Produit de 2 vecteur  cross product pour trouver la perpendiculaire a 2 vecteur ( normale)
    // /// The cross product is only defined in 3D space and takes two non-parallel vectors as input and produces a third vector that is orthogonal to both the input vectors. If both the input vectors are orthogonal to each other as well, a cross product would result in 3 orthogonal vectors; this will prove useful in the upcoming chapters. The following image shows what this looks like in 3D space:
    // /// </summary>
    // /// <param name="v1"></param>
    // /// <param name="v2"></param>
    // /// <returns></returns>
    // public static Vector2 Cross(ref Vector2 v1,ref Vector2 v2)
    //     => Alloc( (v1.Y * v2.Z) - (v1.Z * v2.Y),
    //             (v1.Z * v2.X) - (v1.X * v2.Z),
    //             (v1.X * v2.Y) - (v1.Y * v2.X));
    // /// <summary>
    // /// Produit de 2 vecteur  cross product pour trouver la perpendiculaire a 2 vecteur ( normale)
    // /// </summary>
    // public static void Cross(ref Vector2 result , ref Vector2 v1,ref  Vector2 v2)
    //     => ( result.X,result.Y,result.Z ) = (
    //         (v1.Y * v2.Z) - (v1.Z * v2.Y),
    //         (v1.Z * v2.X) - (v1.X * v2.Z),
    //         (v1.X * v2.Y) - (v1.Y * v2.X));

    /// <summary>
    /// dot product cosinus de l'angle entre les deux vecteurs
    /// The dot product of two vectors is equal to the scalar product of their lengths times the cosine of the angle between them.
    /// si le resultat est 1 ( cos 90°) les deux vecteur sont orthogonaux  si result =0  angle =0 il sont parallel
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(ref Vector2  v1,ref Vector2  v2)
        => (v1.X * v2.X) + (v1.Y * v2.Y) ;

    /// <summary>
    /// Distance entre deux vecteur 
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(ref Vector2 value1,ref Vector2 value2)
        =>  Sqrt(((value1.X - value2.X) * (value1.X - value2.X)) + ((value1.Y - value2.Y) * (value1.Y - value2.Y)) );
#endregion
#region Public
            /// <summary>
    /// Constructeur complet 
    /// </summary>
    /// <param name="x"> axe x </param>
    /// <param name="y">axe y </param>
    /// <param name="z"></param>
    /// <returns></returns>
    public Vector2(float x=0.0f, float y=0.0f) 
        =>(X, Y) = (x,y);

    /// <summary>
    /// Equivelent construteur de Copie
    /// </summary>
    /// <param name="v"></param>
    /// <param name="z"></param>
    /// <param name="w"></param>
    /// <returns></returns>
    public Vector2( Vector2 v)
        =>(X,Y)=( v.X,v.Y);

    /// <summary>
    /// Constrcuteur a partir d'un tableau de flotant
    /// </summary>
    /// <param name="floats"></param>
    /// <returns></returns>
    public Vector2( float[] floats)
        =>(X, Y)=( floats[0],floats[1] );

    /// <summary>
    /// Instanciate a partir d'un scalaire
    /// </summary>
    /// <param name="scalar"></param>
    public Vector2(float scalar)
        =>(X, Y) = ( scalar,scalar);

   
    /// <summary>
    /// Normalize un vecteur
    /// </summary>
    public void Normalize()
    {
        float length = 1/ this.Length();
        this.X *= length;
        this.Y *= length;
     
    }
    /// <summary>
    ///  le negatif de chaque valeurs
    /// </summary>
    public void Negate() =>(X, Y)=(-X, -Y);
    
    /// <summary>
    /// To retrieve the length/magnitude of a vector we use the Pythagoras theorem that you may remember from your math classes. 
    /// A vector forms a triangle when you visualize its individual x and y component as two sides of a triangle:
    /// </summary>
    /// <returns>Magnitude</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Length()
        =>  Sqrt((X * X) + (Y * Y)  );

    /// <summary>
    /// retourn Vector2 sous forme de tableu de reel float[4] sans utiliser new
    /// </summary>
    /// <returns></returns>
    public float[] ToArrayStaticalloc()
    {
        float[] r ={ X, Y }; 
        return r;
    }
    /// <summary>
    /// retourn Vector2 sous forme de tableu de reel float[4]
    /// </summary>
    /// <value></value>
    public float[] ToArray => new float[]{ X, Y};
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
