namespace RitaEngine.Base.Math;

using static RitaEngine.Base.Math.Helper;
        
[StructLayout(LayoutKind.Sequential ),SkipLocalsInit]
public struct Vector4 : IEquatable<Vector4> 
{
#region Default value        
    private static readonly Vector4 zero = new(0.0f,0.0f,0.0f,0.0f);
    public static ref readonly Vector4 Zero => ref zero;

    public static readonly Vector4 one = new(1.0f,1.0f,1.0f,1.0f);
    public static ref readonly Vector4 One  => ref one;
    //     /// <summary>
//     /// vector 4  tous a 0.0f sauf X
//     /// </summary>
//     public static readonly Vector4 UnitX = new(1.0f, 0.0f, 0.0f, 0.0f);
//     /// <summary>
//     /// vector 4  tous a 0.0f sauf Y
//     /// </summary>
//     public static readonly Vector4 UnitY = new(0.0f, 1.0f, 0.0f, 0.0f);
//     /// <summary>
//     /// vector 4  tous a 0.0f sauf z
//     /// </summary>
//     public static readonly Vector4 UnitZ = new(0.0f, 0.0f, 1.0f, 0.0f);
//     /// <summary>
//     /// vector 4  tous a 0.0f sauf W
//     /// </summary>
//     public static readonly Vector4 UnitW = new(0.0f, 0.0f, 0.0f, 0.0f);
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static readonly int SizeOf = sizeof(float)* 4 ; // Marshal.SizeOf<Vector4>();
#endregion
    /// <summary>Axe  Abcisse X  </summary>
    public float X;
    /// <summary> Axe Ordonnée  </summary>
    public float Y;
    /// <summary> Axe Z ( 3D )  </summary>
    public float Z;
    /// <summary> Direction  </summary>
    public float W;

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
            3 => W,
            _ => float.NaN
        };
        set => _ = index switch
        {
            0 => X = value,
            1 => Y = value,
            2 => Z = value,
            3 => W = value,
            _ => float.NaN
        };
    }
    
#region OVERRIDE        
    /// <summary>
    /// Vector 4 sous forme de chaine de caractère avec  le mot Vector4 et passage a la ligne pour les valeurs
    /// </summary>
    /// <returns></returns>
    public override string ToString()  => $"[X={X:G3};Y={Y:G3};Z={Z:G3};W={W:G3}]";
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => (this.X.GetHashCode() ) + this.Y.GetHashCode() + this.Z.GetHashCode() + this.W.GetHashCode();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => obj is Vector4 vec && this.Equals(vec)  ;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Vector4 other) 
        => (Abs(X - other.X) <= ZeroTolerance) && (Abs(Y - other.Y) <= ZeroTolerance) && (Abs(Z - other.Z) <= ZeroTolerance) && (Abs(W - other.W) <= ZeroTolerance);

    public static bool operator ==(Vector4 left, Vector4 right) => left.Equals(right);
    public static bool operator !=(Vector4 left, Vector4 right) => !(left.Equals(right));
#endregion
#region ALLLOCATE
    /// <summary>
    /// Allocation sur la pile
    /// </summary>
    /// <param name="valueX"></param>
    /// <param name="valueY"></param>
    /// <param name="valueZ"></param>
    /// <param name="valueW"></param>
    /// <returns></returns>
    public static Vector4 Alloc( float valueX , float valueY , float valueZ , float valueW)
    {
        Vector4 vec = default;
        vec.X = valueX;
        vec.Y = valueY;
        vec.Z = valueZ;
        vec.W = valueW;
        return vec;
    }

#endregion        
#region OPERATION BINAIRE
    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>        
    public static Vector4 operator *(Vector4 left, Vector4 right)
        => Alloc(left.X * right.X, left.Y * right.Y, left.Z* right.Z, left.W*right.W);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="result"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    public static void MultiplyVec4ByVec4(ref Vector4 result, ref Vector4 left, ref Vector4 right)
    => ( result.X,result.Y,result.Z,result.W )=(left.X * right.X, left.Y * right.Y, left.Z* right.Z, left.W*right.W);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="scale"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Vector4 operator *(float scale, Vector4 value)
        => Alloc(value.X * scale, value.Y * scale, value.Z *scale,value.W*scale);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    public static Vector4 operator *(Vector4 value, float scale)
        => Alloc(value.X * scale, value.Y * scale, value.Z *scale,value.W*scale);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="result"></param>
    /// <param name="value"></param>
    /// <param name="scalar"></param>
    public static void MultiplyVector4ByScalar( ref Vector4 result, ref Vector4 value , float scalar)
        => (result.X, result.Y, result.Z, result.W) = (value.X*scalar, value.Y*scalar, value.Z*scalar, value.W*scalar);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Vector4 operator +(Vector4 left, Vector4 right)
        => Alloc(left.X + right.X, left.Y + right.Y , left.Z+right.Z, left.W + right.W);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="result"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    public static void AddVec4WithVec4(ref Vector4 result, ref Vector4 left, ref Vector4 right)
        => ( result.X,result.Y,result.Z,result.W )=(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="scalar"></param>
    /// <returns></returns>
    public static Vector4 operator +(Vector4 value, float scalar)
        => Alloc(value.X + scalar, value.Y + scalar , value.Z + scalar, value.W + scalar) ;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="scalar"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Vector4 operator +(float scalar, Vector4 value)
        => Alloc(scalar + value.X, scalar + value.Y, scalar + value.Z, scalar + value.W);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    public static Vector4 operator /(Vector4 value, float scale)
        => Alloc( value.X/scale , value.Y/ scale , value.Z/scale , value.W/scale );
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="scalar"></param>
    /// <returns></returns>
    public static Vector4 operator -(Vector4 value, float scalar)
        => Alloc(value.X - scalar, value.Y - scalar, value.Z - scalar, value.W- scalar);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="scalar"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Vector4 operator -(float scalar, Vector4 value)
        => Alloc(scalar - value.X, scalar - value.Y, scalar- value.Z, scalar-value.W);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Vector4 operator - (Vector4 left, Vector4 right)
        => Alloc(left.X - right.X, left.Y - right.Y,  left.Z - right.Z, left.W- right.W);

    /// <summary>
    /// Unary operator 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Vector4 operator - (Vector4 value)
    {
        value.X = -value.X;
        value.Y = - value.Y;
        value.Z = - value.Z; 
        value.W = - value.W;
        return value;
    }
#endregion
#region Trigonometrie    
    /// <summary>
    /// Normalize n vector ( perdendiculaire )
    /// 
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector4 Normalize( Vector4 v)
    {
        float length = 1/ v.Length();
        return Alloc( v.X*length, v.Y*length,v.Z*length , v.W*length);
    }

    /// <summary>
    /// Produit de 2 vecteur  cross product pour trouver la perpendiculaire a 2 vecteur ( normale)
    /// The cross product is only defined in 3D space and takes two non-parallel vectors as input and produces a third vector that is orthogonal to both the input vectors. If both the input vectors are orthogonal to each other as well, a cross product would result in 3 orthogonal vectors; this will prove useful in the upcoming chapters. The following image shows what this looks like in 3D space:
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    public static Vector4 Cross(ref Vector4 v1,ref Vector4 v2)
        => Alloc( (v1.Y * v2.Z) - (v1.Z * v2.Y),
                (v1.Z * v2.X) - (v1.X * v2.Z),
                (v1.X * v2.Y) - (v1.Y * v2.X),
                (v1.W * v2.Z) - v1.Z);
    /// <summary>
    /// Produit de 2 vecteur  cross product pour trouver la perpendiculaire a 2 vecteur ( normale)
    /// </summary>
    public static void Cross(ref Vector4 result , ref Vector4 v1,ref  Vector4 v2)
        => ( result.X,result.Y,result.Z,result.W ) = (
            (v1.Y * v2.Z) - (v1.Z * v2.Y),
            (v1.Z * v2.X) - (v1.X * v2.Z),
            (v1.X * v2.Y) - (v1.Y * v2.X),
            (v1.W * v2.Z) - v1.Z);

    /// <summary>
    /// dot product cosinus de l'angle entre les deux vecteurs
    /// The dot product of two vectors is equal to the scalar product of their lengths times the cosine of the angle between them.
    /// si le resultat est 1 ( cos 90°) les deux vecteur sont orthogonaux  si result =0  angle =0 il sont parallel
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Dot(ref Vector4  v1,ref Vector4  v2)
        => (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z) + (v1.W*v2.W);

    /// <summary>
    /// Distance entre deux vecteur 
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(ref Vector4 value1,ref Vector4 value2)
        =>  Sqrt(((value1.X - value2.X) * (value1.X - value2.X)) + ((value1.Y - value2.Y) * (value1.Y - value2.Y)) + ((value1.Z - value2.Z) * (value1.Z - value2.Z)) + ((value1.W - value2.W) * (value1.W - value2.W)));
#endregion
#region Public
            /// <summary>
    /// Constructeur complet 
    /// </summary>
    /// <param name="x"> axe x </param>
    /// <param name="y">axe y </param>
    /// <param name="z"></param>
    /// <param name="w"></param>
    /// <returns></returns>
    public Vector4(float x=0.0f, float y=0.0f, float z=0.0f, float w=0.0f) 
        =>(X,Y,Z,W) = (x,y,z,w);

    /// <summary>
    /// Constrcuteur a partir d'un tableau de flotant
    /// </summary>
    /// <param name="floats"></param>
    /// <returns></returns>
    public Vector4( float[] floats)
        =>(X,Y,Z,W)=( floats[0],floats[1],floats[2],floats[3] );
    
    /// <summary>
    /// Instanciate a partir d'un scalaire
    /// </summary>
    /// <param name="scalar"></param>
    public Vector4(float scalar)
        =>(X,Y,Z,W) = ( scalar,scalar,scalar,scalar);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vec4"></param>
    public Vector4( Vector4 vec4)
        => (X,Y,Z,W) = ( vec4.X,vec4.Y,vec4.Z,vec4.W);
    /// <summary>
    /// Normalize un vecteur !!!! change les valeur du vecteur sinon utilisé ToNormalize
    /// </summary>
    public void Normalize()
    {
        float length = 1/ this.Length();
        this.X *= length;
        this.Y *= length;
        this.Z *= length;
        this.W *= length;
    }

    /// <summary>
    ///  le negatif de chaque valeurs
    /// </summary>
    public void Negate()
        =>(X,Y,Z,W)=(-X,-Y,-Z,-W);
    
    /// <summary>
    /// To retrieve the length/magnitude of a vector we use the Pythagoras theorem that you may remember from your math classes. 
    /// A vector forms a triangle when you visualize its individual x and y component as two sides of a triangle:
    /// </summary>
    /// <returns>Magnitude</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Length()
        =>  Sqrt((X * X) + (Y * Y) + (Z * Z) + (W *W));

    /// <summary>
    /// retourn vector4 sous forme de tableu de reel float[4] sans utiliser new
    /// </summary>
    /// <returns></returns>
    public float[] ToArrayStaticalloc()
    {
        float[] r ={X,Y,Z,W }; 
        return r;
    }
    /// <summary>
    /// retourn vector4 sous forme de tableu de reel float[4]
    /// </summary>
    /// <value></value>
    public float[] ToArray => new float[]{ X,Y,Z,W};

#endregion  


//     /// <summary>
//     /// Initialise une nouvelle instance de ce vector avec toutes les valeurs a zero
//     /// </summary>
//     //public Vector4(){ }

//     public Vector4(float x, float y, float z , float w)
//         =>(X,Y,Z,W)=(x,y,z,w);
    
//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="x"></param>
//     /// <param name="y"></param>
//     /// <param name="z"></param>
//     public Vector4(float x, float y, float z )
//         =>(X,Y,Z,W)=(x,y,z,0.0f);

//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="v"></param>
//     public Vector4( Vector4 v)
//         =>(X,Y,Z,W) = ( v.X,v.Y,v.Z,v.W);

//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="floats"></param>
//     public Vector4( float[] floats)
//         =>(X,Y,Z,W) = ( floats[0],floats[1],floats[2],floats[3] );

//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="scalar"></param>
//     public Vector4(float scalar)
//         =>( X,Y,Z,W) = ( scalar,scalar,scalar,scalar);

//     // public Vector4(Vector3 vec3)
//     //     =>( X,Y,Z,W) = ( vec3.X,vec3.Y,vec3.Z, 1.0f);

//     /// <summary>
//     /// 
//     /// </summary>
//     /// <value></value>
//     public float this[int index]
//     {
//         get => index switch
//         {
//                 0=>X,
//                 1=>Y,
//                 2=>Z,
//                 3=>W,
//                 _ => 0//throw new System.ArgumentOutOfRangeException("Vector  indeX invalide")
//         };
//         set => _ = index switch
//         {
//                 0=> X = value,
//                 1=> Y = value,
//                 2=> Z = value,
//                 3=> W = value,
//                 _ => 0//throw new System.ArgumentOutOfRangeException("Vector  indeX invalide")
//         };
//     }

//     /// <summary>
//     /// Normalize n vector ( perdendiculaire )
//     /// </summary>
//     /// <param name="v"></param>
//     /// <returns></returns>
//     public static Vector4 Normalize( Vector4 v)
//     {
//         float length = 1/ v.Length();
//         return new Vector4( v.X*length, v.Y*length,v.Z*length , v.W*length);
//     }
//     /// <summary>
//     /// 
//     /// </summary>
//     public void Normalize()
//     {
//         float length = 1/ this.Length();
//         this.X *= length;
//         this.Y *= length;
//         this.Z *= length;
//         this.W *= length;
//     }

//     /// <summary>
//     /// 
//     /// </summary>
//     public void Negative()
//     {
//         this.X = -X;
//         this.Y = -Y;
//         this.Z = -Z;
//         this.W = -W;
//         //return this;
//     }
//     /// <summary>
//     /// Produit de 2 vecteur  cross product pour trouver la perpendiculaire a 2 vecteur ( normale)
//     /// </summary>
//     /// <param name="v1"></param>
//     /// <param name="v2"></param>
//     /// <returns></returns>
//     public static Vector4 Cross(ref Vector4 v1,ref Vector4 v2)
//         => new(x: (v1.Y * v2.Z) - (v1.Z * v2.Y),
//             y: (v1.Z * v2.X) - (v1.X * v2.Z),
//             z: (v1.X * v2.Y) - (v1.Y * v2.X),
//             w: (v1.W * v2.Z) - v1.Z);

//     /// <summary>
//     /// Produit de 2 vecteur  cross product pour trouver la perpendiculaire a 2 vecteur ( normale)
//     /// </summary>
//     /// <param name="result"></param>
//     /// <param name="v1"></param>
//     /// <param name="v2"></param>
//     public static void Cross(ref Vector4 result , ref Vector4 v1,ref  Vector4 v2)
//         => ( result.X,result.Y,result.Z,result.W ) = (
//             (v1.Y * v2.Z) - (v1.Z * v2.Y),
//             (v1.Z * v2.X) - (v1.X * v2.Z),
//             (v1.X * v2.Y) - (v1.Y * v2.X),
//             (v1.W * v2.Z) - v1.Z);

//     /// <summary>
//     /// dot product cosinus de l'angle entre les deux vecteurs
//     /// </summary>
//     /// <param name="v1"></param>
//     /// <param name="v2"></param>
//     /// <returns></returns>
//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     public static float Dot(ref Vector4  v1,ref Vector4  v2)
//         => (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z) + (v1.W*v2.W);

//     /// <summary>
//     /// 
//     /// </summary>
//     /// <returns></returns>
//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     public readonly float Length()
//         =>  Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W));

//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="value1"></param>
//     /// <param name="value2"></param>
//     /// <returns></returns>
//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     public static float Distance(ref Vector4 value1,ref Vector4 value2)
//         =>  Sqrt(((value1.X - value2.X) * (value1.X - value2.X)) + ((value1.Y - value2.Y) * (value1.Y - value2.Y)) + ((value1.Z - value2.Z) * (value1.Z - value2.Z)) + ((value1.W - value2.W) * (value1.W - value2.W)));

//     /// <summary>
//     /// Simply multiply vector by matrix( attention au sens)
//     /// </summary>
//     /// <param name="result"></param>
//     /// <param name="value"></param>
//     /// <param name="matrix"></param>
//     public static void Transform(ref Vector4 result,ref Vector4 value, ref Matrix matrix)
//         => (result.X, result.Y , result.Z, result.W) =
//         ((value.X * matrix.Right.X) + (value.Y * matrix.Up.X) + (value.Z * matrix.Forward.X) + (value.W * matrix.Translation.X),
//         (value.X * matrix.Right.Y) + (value.Y * matrix.Up.Y) + (value.Z * matrix.Forward.Y) + (value.W * matrix.Translation.Y),
//         (value.X * matrix.Right.Z) + (value.Y * matrix.Up.Z) + (value.Z * matrix.Forward.Z) + (value.W * matrix.Translation.Z),
//         (value.X * matrix.Right.W) + (value.Y * matrix.Up.W) + (value.Z * matrix.Forward.W) + (value.W * matrix.Translation.W));

//     /// <inheritdoc />
//     public override string ToString()  => "Vector4["+X+";"+Y+";"+Z+";"+W+ "]";
//     /// <inheritdoc />
//     public override int GetHashCode() => (int)(X+Y+Z+W)^32;
//     /// <inheritdoc />
//     public override bool Equals(object? obj)
//         => obj is Vector4 vec4 && vec4.X == this.X && vec4.Y== this.Y && vec4.Z==this.Z && vec4.W==this.W;

// #region OVERRIDE OPERATOR
//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="left"></param>
//     /// <param name="right"></param>
//     /// <returns></returns>
//     public static bool operator ==(Vector4 left, Vector4 right)
//         => left.Equals(right);

//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="left"></param>
//     /// <param name="right"></param>
//     /// <returns></returns>
//     public static bool operator !=(Vector4 left, Vector4 right)
//         => !(left == right);

//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="left"></param>
//     /// <param name="right"></param>
//     /// <returns></returns>
//     public static Vector4 operator *(Vector4 left, Vector4 right)
//         => new(left.X * right.X, left.Y * right.Y, left.Z* right.Z, left.W*right.W);
    
//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="result"></param>
//     /// <param name="left"></param>
//     /// <param name="right"></param>
//     public static void MultiplyVec4ByVec4(ref Vector4 result, ref Vector4 left, ref Vector4 right)
//         => ( result.X,result.Y,result.Z,result.W )=(left.X * right.X, left.Y * right.Y, left.Z* right.Z, left.W*right.W);

//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="scale"></param>
//     /// <param name="value"></param>
//     /// <returns></returns>
//     public static Vector4 operator *(float scale, Vector4 value)
//         => new(value.X * scale, value.Y * scale, value.Z *scale,value.W*scale);
    
//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="value"></param>
//     /// <param name="scale"></param>
//     /// <returns></returns>
//     public static Vector4 operator *(Vector4 value, float scale)
//         => new(value.X * scale, value.Y * scale, value.Z *scale,value.W*scale);

//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="result"></param>
//     /// <param name="value"></param>
//     /// <param name="scalar"></param>
//     public static void MultiplyVector4ByScalar( ref Vector4 result, ref Vector4 value , float scalar)
//         => (result.X, result.Y, result.Z, result.W) = (value.X*scalar, value.Y*scalar, value.Z*scalar, value.W*scalar);

//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="left"></param>
//     /// <param name="right"></param>
//     /// <returns></returns>
//     public static Vector4 operator +(Vector4 left, Vector4 right)
//         => new(left.X + right.X, left.Y + right.Y , left.Z+right.Z, left.W + right.W);
    
//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="result"></param>
//     /// <param name="left"></param>
//     /// <param name="right"></param>
//     public static void AddVec4WithVec4(ref Vector4 result, ref Vector4 left, ref Vector4 right)
//         => ( result.X,result.Y,result.Z,result.W )=(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="value"></param>
//     /// <param name="scalar"></param>
//     /// <returns></returns>
//     public static Vector4 operator +(Vector4 value, float scalar)
//         => new(value.X + scalar, value.Y + scalar , value.Z + scalar, value.W + scalar) ;
//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="scalar"></param>
//     /// <param name="value"></param>
//     /// <returns></returns>
//     public static Vector4 operator +(float scalar, Vector4 value)
//         => new(scalar + value.X, scalar + value.Y, scalar + value.Z, scalar + value.W);
//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="value"></param>
//     /// <param name="scale"></param>
//     /// <returns></returns>
//     public static Vector4 operator /(Vector4 value, float scale)
//         => new( value.X/scale , value.Y/ scale , value.Z/scale , value.W/scale );
//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="scalar"></param>
//     /// <param name="vec"></param>
//     /// <returns></returns>
//     public static Vector4 operator /(float scalar, Vector4 vec)
//         => new( scalar/vec.X , scalar/vec.Y, scalar/vec.Z , scalar/vec.W);
//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="value"></param>
//     /// <param name="scalar"></param>
//     /// <returns></returns>
//     public static Vector4 operator -(Vector4 value, float scalar)
//         => new(value.X - scalar, value.Y - scalar, value.Z - scalar, value.W- scalar);
//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="scalar"></param>
//     /// <param name="value"></param>
//     /// <returns></returns>
//     public static Vector4 operator -(float scalar, Vector4 value)
//         => new(scalar - value.X, scalar - value.Y, scalar- value.Z, scalar-value.W);
//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="left"></param>
//     /// <param name="right"></param>
//     /// <returns></returns>
//     public static Vector4 operator - (Vector4 left, Vector4 right)
//         => new(left.X - right.X, left.Y - right.Y,  left.Z - right.Z, left.W- right.W);
// #endregion
// }

}
