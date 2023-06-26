namespace RitaEngine.Base.Math;

using static RitaEngine.Base.Math.Helper;

[StructLayout(LayoutKind.Sequential),SkipLocalsInit]
public struct Vector3 : IEquatable<Vector3>
{
    private static readonly Vector3 zero = new(0.0f,0.0f,0.0f);
    public static ref readonly Vector3 Zero => ref zero;
    public static readonly Vector3 one = new(1.0f,1.0f,1.0f);
    public static ref readonly Vector3 One  => ref one;
    public static readonly int SizeOf =sizeof(float) * 3 ;

    /// <summary>Axe  Abcisse X  </summary>
    public float X ;
    /// <summary> Axe Ordonnée  </summary>
    public float Y ;
    /// <summary> Axe Z ( 3D )  </summary>
    public float Z ;

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

#region OVERRIDE        
    public override string ToString()  => $"[X={X:G3};Y={Y:G3};Z={Z:G3}]";
    public override int GetHashCode() => HashCode.Combine(X,Y,Z);
    public override bool Equals(object? obj) => obj is Vector3 vec && this.Equals(vec)  ;
    public bool Equals(Vector3 other) => (Abs(X - other.X) <= ZeroTolerance) && (Abs(Y - other.Y) <= ZeroTolerance) && (Abs(Z - other.Z) <= ZeroTolerance) ;
    public static bool operator ==(Vector3 left, Vector3 right) => left.Equals(right);
    public static bool operator !=(Vector3 left, Vector3 right) => !left.Equals(right);
#endregion
#region OPERATION BINAIRE

    public static void Multiply(ref Vector3 result, ref Vector3 left, ref Vector3 right)
    => ( result.X,result.Y,result.Z )=(left.X * right.X, left.Y * right.Y, left.Z* right.Z);
    
    public static void Multiply( ref Vector3 result, ref Vector3 value , float scalar)
        => (result.X, result.Y, result.Z) = (value.X*scalar, value.Y*scalar, value.Z*scalar);

    public static Vector3 operator *(float scale, Vector3 value)
        => new(value.X * scale, value.Y * scale, value.Z *scale);

    public static Vector3 operator *(Vector3 value, float scale)
        => new(value.X * scale, value.Y * scale, value.Z *scale);
    
    public static Vector3 operator *(Vector3 left, Vector3 right)
        => new(left.X * right.X, left.Y * right.Y, left.Z* right.Z);

    
    public static void Add(ref Vector3 result, ref Vector3 left, ref Vector3 right)
        => ( result.X,result.Y,result.Z)=(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    
    public static void Add(ref Vector3 result, ref Vector3 left, float scalar)
        => ( result.X,result.Y,result.Z)=(left.X + scalar, left.Y + scalar, left.Z + scalar);
    
    public static Vector3 operator +(Vector3 left, Vector3 right)
        => new(left.X + right.X, left.Y + right.Y , left.Z+right.Z);

    public static Vector3 operator +(Vector3 value, float scalar)
        => new(value.X + scalar, value.Y + scalar , value.Z + scalar) ;

    public static Vector3 operator +(float scalar, Vector3 value)
        => new(scalar + value.X, scalar + value.Y, scalar + value.Z);

     public static void Divide(ref Vector3 result, ref Vector3 left, ref Vector3 right)
        => ( result.X,result.Y,result.Z)=(left.X/ right.X, left.Y / right.Y, left.Z / right.Z);
    
    public static void Divide(ref Vector3 result, ref Vector3 left, float scalar)
        => ( result.X,result.Y,result.Z)=(left.X / scalar, left.Y / scalar, left.Z / scalar);

    public static Vector3 operator /(Vector3 value, float scale)
        => new( value.X/scale , value.Y/ scale , value.Z/scale  );
    
    public static Vector3 operator /(Vector3 left,Vector3 right)
        => new( left.X/ right.X , left.Y / right.Y , left.Z / right.Z  );

    public static Vector3 operator -(Vector3 value, float scalar)
        => new(value.X - scalar, value.Y - scalar, value.Z - scalar);

    public static Vector3 operator -(float scalar, Vector3 value)
        => new(scalar - value.X, scalar - value.Y, scalar- value.Z);

    public static Vector3 operator - (Vector3 left, Vector3 right)
        => new(left.X - right.X, left.Y - right.Y,  left.Z - right.Z);

    public static Vector3 operator - (Vector3 value)
    {
        value.Negate();
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
        float length = 1/ v.Length;
        return new( v.X*length, v.Y*length,v.Z*length );
    }

    /// <summary>
    /// Produit de 2 vecteur  cross product pour trouver la perpendiculaire a 2 vecteur ( normale)
    /// The cross product is only defined in 3D space and takes two non-parallel vectors as input and produces a third vector that is orthogonal to both the input vectors. If both the input vectors are orthogonal to each other as well, a cross product would result in 3 orthogonal vectors; this will prove useful in the upcoming chapters. The following image shows what this looks like in 3D space:
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    public static Vector3 Cross(ref Vector3 left,ref Vector3 right) 
        => new (left.Y * right.Z - left.Z * right.Y, 
                left.Z * right.X - left.X * right.Z, 
                left.X * right.Y - left.Y * right.X);
    
//code source from :         https://github.com/Philip-Trettner/GlmSharp/blob/master/GlmSharp/GlmSharp/Vec3/vec3.cs
        /// <summary>
        /// Calculate the reflection direction for an incident vector (N should be normalized in order to achieve the desired result).
        /// </summary>
        public static Vector3 Reflect(ref Vector3 I,ref Vector3 N) => I - 2 * Dot(ref N,ref I) * N;
        
        /// <summary>
        /// Calculate the refraction direction for an incident vector (The input parameters I and N should be normalized in order to achieve the desired result).
        /// </summary>
        public static Vector3 Refract(ref Vector3 I,ref Vector3 N, float eta)
        {
            var dNI = Dot(ref N,ref I);
            var k = 1 - eta * eta * (1 - dNI * dNI);
            if (k < 0) return Zero;
            return eta * I - (eta * dNI + (float)Helper.Sqrt(k)) * N;
        }
        
        /// <summary>
        /// Returns a vector pointing in the same direction as another (faceforward orients a vector to point away from a surface as defined by its normal. If dot(Nref, I) is negative faceforward returns N, otherwise it returns -N).
        /// </summary>
        public static Vector3 FaceForward(ref Vector3 N,ref Vector3 I,ref Vector3 Nref) => Dot(ref Nref,ref I) < 0 ? N : -N;
        
   
    /// <summary>
    /// dot product cosinus de l'angle entre les deux vecteurs
    /// The dot product of two vectors is equal to the scalar product of their lengths times the cosine of the angle between them.
    /// si le resultat est 1 ( cos 90°) les deux vecteur sont orthogonaux  si result =0  angle =0 il sont parallel
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(ref Vector3  left,ref Vector3  right)
        => (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z);

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

    /// <summary>
    /// Equivelent construteur de Copie
    /// </summary>
    /// <param name="v"></param>
    /// <param name="z"></param>
    /// <param name="w"></param>
    /// <returns></returns>
    public Vector3( Vector2 v, float z=0.0f)
        =>(X,Y,Z)=( v.X,v.Y, 0.0f);

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

    public Vector3( Vector3 vec3)
        => (X, Y, Z) = ( vec3.X,vec3.Y,vec3.Z);
    
    
    /// <summary>
    /// Normalize un vecteur
    /// </summary>
    public void Normalize()
    {
        if (Length >= Helper.Epsilon)return ;

        float length = 1/ this.Length;
        this.X *= length;
        this.Y *= length;
        this.Z *= length;
    }
    /// <summary>
    ///  le negatif de chaque valeurs
    /// </summary>
    public void Negate() =>(X, Y, Z) =(-X, -Y, -Z);
    
    /// <summary>
    /// To retrieve the length/magnitude of a vector we use the Pythagoras theorem that you may remember from your math classes. 
    /// A vector forms a triangle when you visualize its individual x and y component as two sides of a triangle:
    /// </summary>
    /// <returns>Magnitude</returns>
    public float Length  =>  Sqrt( LengthSquared );
    /// <summary>
    /// Calculates the squared length of the vector.
    /// </summary>
    /// <returns>The squared length of the vector.</returns>
    /// <remarks>This method may be preferred to <see cref="Vector3.Length" /> when only a relative length is needed and speed is of the essence.</remarks>
    public float LengthSquared => X * X + Y * Y + Z * Z;

    /// <summary>
    /// retourn Vector3 sous forme de tableu de reel float[4] sans utiliser new
    /// </summary>
    /// <returns></returns>
    public float[] ToArray()
    {
        float[] r ={ X, Y, Z }; 
        return r;
    }
    // /// <summary>
    // /// retourn Vector3 sous forme de tableu de reel float[4]
    // /// </summary>
    // /// <value></value>
    // public float[] ToArray => new float[]{ X, Y, Z };
#endregion        
}

