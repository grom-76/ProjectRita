namespace RitaEngine.Math;

using static RitaEngine.Math.Helper;

#if !SMID
        // public struct Matrix4x4
        // {
        // //https://github.com/FlaxEngine/FlaxEngine/blob/master/Source/Engine/Core/Math/Matrix.cs
        
        // }

/// <summary>
/// colonne type like glm attention ne pas mélanger Matrix4 et Matrix ( logique complètement différentes)
/// https://github.com/dwmkerr/glmnet/blob/master/source/GlmNet/
/// </summary>
[SkipLocalsInit,StructLayout(LayoutKind.Sequential)]
public struct Matrix4 //: IDsposable, IEquatable<Matrix>
{
#region Private + Attributs
    private Vector4[] _cols;
    private static Matrix4 identity = new(1.0f);
    private float[] ToArrayColumn()
    {
        float[] array = {
        _cols[0].X , _cols[0].Y , _cols[0].Z , _cols[0].W ,
        _cols[1].X , _cols[1].Y , _cols[1].Z , _cols[1].W ,
        _cols[2].X , _cols[2].Y , _cols[2].Z , _cols[2].W ,
        _cols[3].X , _cols[3].Y , _cols[3].Z , _cols[3].W ,  };
        return array;
    }
    private static Matrix4 AllocNew(float m11=1.0f, float m12=0.0f, float m13=0.0f, float m14=0.0f,
                            float m21=0.0f, float m22=1.0f, float m23=0.0f, float m24=0.0f,
                            float m31=0.0f, float m32=0.0f, float m33=1.0f, float m34=0.0f,
                            float m41=0.0f, float m42=0.0f, float m43=0.0f, float m44=1.0f)
        =>new( m11, m12,m13, m14, m21, m22,m23, m24 ,m31, m32,m33, m34,m41, m42,m43, m44);

    // private static Matrix AllocateDefault(float m11=1.0f, float m12=0.0f, float m13=0.0f, float m14=0.0f, 
    //                         float m21=0.0f, float m22=1.0f, float m23=0.0f, float m24=0.0f, 
    //                         float m31=0.0f, float m32=0.0f, float m33=1.0f, float m34=0.0f, 
    //                         float m41=0.0f, float m42=0.0f, float m43=0.0f, float m44=1.0f)
    // {
    //     Matrix m = default;
    //     return m;
    // }
    #endregion
#region Initialisation / Destruction
    /// <summary>
    /// Constructeur avec une scalaire  par défault ( constructeur vide il est initialisé comme une Identity matrix)
    /// sinon mettre 0.0f pour inisialiser toutes les valeurs a zéro
    /// </summary>
    /// <param name="value"></param>
    public Matrix4(float value =1.0f )
        =>  _cols = ( new Vector4[4]{ new Vector4(value, 0.0f,0.0f,0.0f ) ,
                                        new Vector4(0.0f, value,0.0f,0.0f ) ,
                                        new Vector4(0.0f, 0.0f,value,0.0f ) ,
                                        new Vector4(0.0f, 0.0f,0.0f,value )   });
    /// <summary>
    /// comme constructeur de copie
    /// </summary>
    /// <param name="m"></param>
    public Matrix4( Matrix4 m)
        => _cols = (new Vector4[4]{ m[0],m[1],m[2],m[3] });
    /// <summary>
    /// .
    /// </summary>
    /// <param name="col0"></param>
    /// <param name="col1"></param>
    /// <param name="col2"></param>
    /// <param name="col3"></param>
    public Matrix4 ( Vector4 col0 , Vector4 col1 , Vector4 col2 , Vector4 col3 )
        =>  _cols =(new Vector4[4]{ col0,col1,col2,col3  } ) ;

    /// <summary>
    /// Cosntructeur personalisé une valeur pour chaque valeur dans la matrice
    /// </summary>
    /// <param name="m11"></param>
    /// <param name="m12"></param>
    /// <param name="m13"></param>
    /// <param name="m14"></param>
    /// <param name="m21"></param>
    /// <param name="m22"></param>
    /// <param name="m23"></param>
    /// <param name="m24"></param>
    /// <param name="m31"></param>
    /// <param name="m32"></param>
    /// <param name="m33"></param>
    /// <param name="m34"></param>
    /// <param name="m41"></param>
    /// <param name="m42"></param>
    /// <param name="m43"></param>
    /// <param name="m44"></param>
    public Matrix4( float m11=1.0f, float m12=0.0f, float m13=0.0f, float m14=0.0f,
                    float m21=0.0f, float m22=1.0f, float m23=0.0f, float m24=0.0f,
                    float m31=0.0f, float m32=0.0f, float m33=1.0f, float m34=0.0f,
                    float m41=0.0f, float m42=0.0f, float m43=0.0f, float m44=1.0f)
        =>  _cols = ( new Vector4[4]{ new Vector4(m11, m12,m13, m14 ) ,
                                        new Vector4(m21, m22,m23, m24 ) ,
                                        new Vector4(m31, m32,m33, m34 ) ,
                                        new Vector4(m41, m42,m43, m44 )   });
    /// <summary>
    /// normalement sa devrait pas marché car Suppressfinalize ne fonctionne pas sur une structure
    /// </summary>
    public void Dispose()
    {
        // _cols[0].Dispose;
        _cols = null!;
        #pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        GC.SuppressFinalize(this);
        #pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    }
#endregion
#region Accesseur        
    /// <summary>
    /// modifie toutes les valeur ( perte ) pour devenir une matrice identité
    /// retourne une nouvelle matrice
    /// </summary>
    public static Matrix4 Identite => identity;
    /// <summary>
    /// modifie toutes les valeur ( perte ) pour devenir une matrice identité
    /// </summary>
    public void Identity()
    => (_cols[0].X , _cols[0].Y , _cols[0].Z, _cols[0].W ,
        _cols[1].X , _cols[1].Y , _cols[1].Z, _cols[1].W ,
        _cols[2].X , _cols[2].Y , _cols[2].Z, _cols[2].W ,
        _cols[3].X , _cols[3].Y , _cols[3].Z, _cols[3].W ) =
        (
            1.0f, 0.0f,0.0f,0.0f ,
            0.0f, 1.0f,0.0f,0.0f ,
            0.0f, 0.0f,1.0f,0.0f ,
            0.0f, 0.0f,0.0f,1.0f
        );
    /// <summary>
    /// Retourne toutes les valeurs de la matrice sous forme de tableau de réel(float)
    /// </summary>
    /// <returns></returns>
    public float[] ToArray => ToArrayColumn();
            /// <summary>
    ///  utilisé un objet Matrix comme un tableau 2 dimension => matrice4x4[ligne ,colonne] valeur de zéro à 4 sinon exception
    /// </summary>
    /// <value></value>
    public float this[int column ,int row]
    {
        get => _cols[column][row];
        set => _cols[column][row] = value;
    }
    /// <summary>
    /// utilisé un objet Matrix comme un tableau => matrice4x4[colonne] valeur de zéro à 4 sinon exception
    /// </summary>
    /// <value></value>
    public  Vector4 this[int column]
    {
        get => _cols[column];
        set => _cols[column] = value ;
    }
    /// <summary>
    /// Retreive se translation vector 3 column in matrix
    /// </summary>
    /// <returns></returns>
    public Vector3 Translation => new( this[3,0] , this[3,1], this[3,2]);

#endregion        
#region OPerateur binaire
/*
AVOID GIMBAL LOCK WHITOUT QUATERNION
// MCJ.Engine.Maths.Vector4 all = (orientation.Row1 * sprite.Axis.X) + (orientation.Row2 * sprite.Axis.Y) + (orientation.Row3 * sprite.Axis.Z);
// float overallAngular =  all.Length();
// all.Normalize();
// MCJ.Engine.Maths.Matrix.Rotate( ref orientation, overallAngular,ref all);
            
*/
    /// <summary>
    /// muliplication de deux matrice ! l'ordre
    /// https://github.com/g-truc/glm/blob/master/glm/detail/type_mat4x4.inl
    /// https://github.com/dwmkerr/glmnet/blob/master/source/GlmNet/GlmNet/mat4.cs
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Matrix4 operator * ( Matrix4 lhs, Matrix4 rhs)
        => new(
            (rhs[0][0] * lhs[0]) + (rhs[0][1] * lhs[1]) + (rhs[0][2] * lhs[2]) + (rhs[0][3] * lhs[3]),
            (rhs[1][0] * lhs[0]) + (rhs[1][1] * lhs[1]) + (rhs[1][2] * lhs[2]) + (rhs[1][3] * lhs[3]),
            (rhs[2][0] * lhs[0]) + (rhs[2][1] * lhs[1]) + (rhs[2][2] * lhs[2]) + (rhs[2][3] * lhs[3]),
            (rhs[3][0] * lhs[0]) + (rhs[3][1] * lhs[1]) + (rhs[3][2] * lhs[2]) + (rhs[3][3] * lhs[3])
        );
    /// <summary>
    /// multiplication d'une matrice par un scalaire
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="s"></param>
    /// <returns></returns>
    public static Matrix4 operator * ( Matrix4 lhs , float s)
        => new( lhs[0]*s ,lhs[1]*s ,lhs[2]*s ,lhs[3]*s );

    /// <summary>
    /// Multiplies the <paramref name="lhs"/> matrix by the <paramref name="rhs"/> vector.
    /// </summary>
    /// <param name="lhs">The LHS matrix.</param>
    /// <param name="rhs">The RHS vector.</param>
    /// <returns>The product of <paramref name="lhs"/> and <paramref name="rhs"/>.</returns>
    public static Vector4 operator *(Matrix4 lhs, Vector4 rhs)
        =>new (
            (lhs[0, 0] * rhs[0]) + (lhs[1, 0] * rhs[1]) + (lhs[2, 0] * rhs[2]) + (lhs[3, 0] * rhs[3]),
            (lhs[0, 1] * rhs[0]) + (lhs[1, 1] * rhs[1]) + (lhs[2, 1] * rhs[2]) + (lhs[3, 1] * rhs[3]),
            (lhs[0, 2] * rhs[0]) + (lhs[1, 2] * rhs[1]) + (lhs[2, 2] * rhs[2]) + (lhs[3, 2] * rhs[3]),
            (lhs[0, 3] * rhs[0]) + (lhs[1, 3] * rhs[1]) + (lhs[2, 3] * rhs[2]) + (lhs[3, 3] * rhs[3])
        );
    /// <summary>
    ///  is not a number
    /// </summary>
    /// <returns></returns>
    public bool IsNaN =>float.IsNaN( this[0,0] ) || float.IsNaN( this[0,1]) || float.IsNaN( this[0,2]) || float.IsNaN( this[0,3]) ||
                float.IsNaN( this[1,0] ) || float.IsNaN( this[1,1]) || float.IsNaN( this[1,2]) || float.IsNaN( this[1,3]) ||
                float.IsNaN( this[2,0] ) || float.IsNaN( this[2,1]) || float.IsNaN( this[2,2]) || float.IsNaN( this[2,3]) ||
                float.IsNaN( this[3,0] ) || float.IsNaN( this[3,1]) || float.IsNaN( this[3,2]) || float.IsNaN( this[3,3]) ;
#endregion        

#region transform
    /// <summary>
    ///  translation  faire var newMat4 = Matrix.Translation( m , nouvellePosition)
    /// </summary>
    /// <param name="m"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Matrix4 Translate(Matrix4 m, Vector3 v)
        => new( m[0], m[1] , m[2],( m[0] * v[0]) + (m[1] * v[1]) +( m[2] * v[2]) + m[3] );

    /// <summary>
    /// Creates a new <see cref="Matrix"/> which contains the rotation moment around specified axis.
    /// </summary>
    /// <param name="m"></param>
    /// <param name="angle">In radians </param>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Matrix4 Rotate(Matrix4 m,in float angle,Vector3 v)
    {
        var c = Cos(  angle  );
        var s = Sin(  angle  );

        Vector3 axis = new(v);
        axis.Normalize();
        Vector3 temp = new ((1 - c) * axis);

        return new(
            (m[0] * (c + (temp[0] * axis[0]))) + (m[1] * ((temp[0] * axis[1]) + (s * axis[2]))) + (m[2] * ((temp[0] * axis[2]) - (s * axis[1]))),
            (m[0] * ((temp[1] * axis[0]) -( s * axis[2])))  + (m[1] * (c + (temp[1] * axis[1]))) +( m[2] * ((temp[1] * axis[2]) + (s * axis[0]))),
            (m[0] * ((temp[2] * axis[0]) + (s * axis[1]))) +( m[1] * ((temp[2] * axis[1]) -( s * axis[0]))) + (m[2] * (c + (temp[2] * axis[2]))),
            m[3]);
    }
    /// <summary>
    /// Mise à l'échelle
    /// </summary>
    /// <param name="m"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Matrix4 Scale(Matrix4 m , Vector3 v)
        => new (m[0] * v[0] ,m[1] * v[1] ,m[2] * v[2] , m[3]  );

#endregion
#region projection
        /// <summary>
    /// Creates a frustrum projection matrix.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <param name="bottom">The bottom.</param>
    /// <param name="top">The top.</param>
    /// <param name="nearVal">The near val.</param>
    /// <param name="farVal">The far val.</param>
    /// <returns></returns>
    public static Matrix4 Frustum(float left, float right, float bottom, float top, float nearVal, float farVal)
    {
        Matrix4 result = new(1.0f);//identity
        result[0, 0] = (2.0f * nearVal) / (right - left);
        result[1, 1] = (2.0f * nearVal) / (top - bottom);
        result[2, 0] = (right + left) / (right - left);
        result[2, 1] = (top + bottom) / (top - bottom);
        result[2, 2] = -(farVal + nearVal) / (farVal - nearVal);
        result[2, 3] = -1.0f;
        result[3, 2] = -(2.0f * farVal * nearVal) / (farVal - nearVal);
        return result;
    }
        /// <summary>
    /// Creates a matrix for a symmetric perspective-view frustum with far plane at infinite.
    /// </summary>
    /// <param name="fovy">The fovy.</param>
    /// <param name="aspect">The aspect.</param>
    /// <param name="zNear">The z near.</param>
    /// <returns></returns>
    public static Matrix4 InfinitePerspective(float fovy, float aspect, float zNear)
    {
        float range = Tan(fovy / 2.0f) * zNear;

        float left = -range * aspect;
        float right = range * aspect;
        float bottom = -range;
        float top = range;

        Matrix4 result = new(0.0f);
        result[0, 0] = 2.0f * zNear / (right - left);
        result[1, 1] = 2.0f * zNear / (top - bottom);
        result[2, 2] = -1.0f;
        result[2, 3] = -1.0f;
        result[3, 2] = -2.0f * zNear;
        return result;
    }
    /// <summary>
    /// Creates a matrix for a right handed, symetric perspective-view frustum.
    /// The near and far clip planes correspond to z normalized device coordinates of -1 and +1 respectively. (OpenGL clip volume definition)
    /// @param fovy Specifies the field of view angle, in degrees, in the y direction. Expressed in radians.
    /// @param aspect Specifies the aspect ratio that determines the field of view in the x direction. The aspect ratio is the ratio of x (width) to y (height).
    /// @param near Specifies the distance from the viewer to the near clipping plane (always positive).
    /// @param far Specifies the distance from the viewer to the far clipping plane (always positive).
    /// @tparam T A floating-point scalar type
    /// </summary>
    /// <param name="fovdegree"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="zNear"></param>
    /// <param name="zFar"></param>
    /// <returns></returns>
    public static Matrix4 Perspective(float fovdegree,float width,float height,float zNear,float zFar )
    {
        float aspect = width / height;
        float tanHalfFovy = Tan( ToRadians(fovdegree) / 2.0f);

        Matrix4 Result = new(1.0f);
        Result[0,0] = 1.0f / (aspect * tanHalfFovy);
        Result[1,1] = 1.0f / tanHalfFovy;
        Result[2,2] = - (zFar + zNear) / (zFar - zNear);
        Result[2,3] = - 1.0f;
        Result[3,2] = - (2.0f * zFar * zNear) / (zFar - zNear);
        Result[3,3] = 0.0f;
        return Result;
    }
    /// <summary>
    /// Builds a perspective projection matrix based on a field of view using right-handed coordinates.
    /// The near and far clip planes correspond to z normalized device coordinates of -1 and +1 respectively. (OpenGL clip volume definition)
    /// @param fov Expressed in radians.
    /// @param width Width of the viewport
    /// @param height Height of the viewport
    /// @param near Specifies the distance from the viewer to the near clipping plane (always positive).
    /// @param far Specifies the distance from the viewer to the far clipping plane (always positive).
    /// @tparam T A floating-point scalar type
    /// </summary>
    /// <param name="fovdegree"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="zNear"></param>
    /// <param name="zFar"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Matrix4 PerspectiveFOV(float fovdegree,float width,float height,float zNear,float zFar )
    {
        if (width <= 0 || height <= 0 || fovdegree <= 0) throw new ArgumentOutOfRangeException("");

        float rad =ToRadians( fovdegree ) ;
        float h = Cos(  0.5f * rad) / Sin( 0.5f * rad);
        float w = h * height / width; //todo max(width , Height) / min(width , Height)?

        return new(  w ,    0.0f,   0.0f,                                0.0f,
                    0.0f,   h,      0.0f,                                0.0f,
                    0.0f,   0.0f,- (zFar + zNear) / (zFar - zNear) ,    -1.0f,
                    0.0f,   0.0f,- (2.0f * zFar * zNear) / (zFar - zNear),0.0f        );
    }
    /// <summary>
    /// Creates a matrix for an orthographic parallel viewing volume, using right-handed coordinates.
    /// The near and far clip planes correspond to z normalized device coordinates of -1 and +1 respectively. (OpenGL clip volume definition)
    /// @tparam T A floating-point scalar type
    /// _RHNO
    /// @see - glm::ortho(T  left, T  right, T  bottom, T  top)
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="top"></param>
    /// <param name="bottom"></param>
    /// <param name="zNear"></param>
    /// <param name="zFar"></param>
    /// <returns></returns>
    public static Matrix4 Ortho(float left,float right,float top,float bottom,float zNear,float zFar )
    {
        Matrix4 Result = new(1.0f);
        Result[0,0] = 2.0f / (right - left);
        Result[1,1] = 2.0f / (top - bottom);
        Result[2,2] = - 2.0f / (zFar - zNear);
        Result[3,0] = - (right + left) / (right - left);
        Result[3,1] = - (top + bottom) / (top - bottom);
        Result[3,2] = - (zFar + zNear) / (zFar - zNear);
        return Result;
    }

    /// <summary>
    /// Creates a matrix for projecting two-dimensional coordinates onto the screen.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <param name="bottom">The bottom.</param>
    /// <param name="top">The top.</param>
    /// <returns></returns>
    public static Matrix4 Ortho2D(float left, float right, float bottom, float top)
    {
        Matrix4 result = new(1.0f);
        result[0, 0] = (2f) / (right - left);
        result[1, 1] = (2f) / (top - bottom);
        result[2, 2] = -(1f);
        result[3, 0] = -(right + left) / (right - left);
        result[3, 1] = -(top + bottom) / (top - bottom);
        return result;
    }

    /// <summary>
    /// Creates a matrix for a symmetric perspective-view frustum with far plane 
    /// at infinite for graphics hardware that doesn't support depth clamping.
    /// </summary>
    /// <param name="fovy">The fovy. in radian</param>
    /// <param name="aspect">The aspect.</param>
    /// <param name="zNear">The z near.</param>
    /// <returns></returns>
    public static Matrix4 TweakedInfinitePerspective(float fovy, float aspect, float zNear)
    {
        float range = Tan(fovy / (2)) * zNear;
        float left = -range * aspect;
        float right = range * aspect;
        float bottom = -range;
        float top = range;

        Matrix4 Result = new(0.0f);
        Result[0, 0] = 2.0f * zNear / (right - left);
        Result[1, 1] = 2.0f * zNear / (top - bottom);
        Result[2, 2] = 0.0001f - 1.0f;
        Result[2, 3] = -1;
        Result[3, 2] = -(0.0001f - 2) * zNear;
        return Result;
    }

    /// <summary>
    /// Define a picking region.
    /// </summary>
    /// <param name="center">The center.</param>
    /// <param name="delta">The delta.</param>
    /// <param name="viewport">The viewport.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public static Matrix4 PickMatrix(Vector2 center, Vector2 delta, Vector4 viewport)
    {
        if (delta.X <= 0 || delta.Y <= 0)
            throw new ArgumentOutOfRangeException();
        var Result = new Matrix4(1.0f);

        if (!(delta.X > (0f) && delta.Y > (0f)))
            return Result; // Error

        Vector3 Temp = new(
            ((viewport[2]) - (2f) * (center.X - (viewport[0]))) / delta.X,
            ((viewport[3]) - (2f) * (center.Y - (viewport[1]))) / delta.Y,
            (0f));

        // Translate and scale the picked region to the entire window
        Result = Matrix4.Translate(Result, Temp);
        return  Matrix4.Scale(Result, new((viewport[2]) / delta.X, (viewport[3]) / delta.Y, (1)));
    }

    /// <summary>
    /// Map the specified object coordinates (obj.x, obj.y, obj.z) into window coordinates.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="model">The model.</param>
    /// <param name="proj">The proj.</param>
    /// <param name="viewport">The viewport.</param>
    /// <returns></returns>
    public static Vector3 Project(Vector3 obj, Matrix4 model, Matrix4 proj, Vector4 viewport)
    {
        Vector4 tmp = new(obj.X,obj.Y,obj.Z, 1.0f);
        tmp = model * tmp;
        tmp = proj * tmp;

        tmp /= tmp.W;
        tmp = tmp * 0.5f + 0.5f;
        tmp[0] = tmp[0] * viewport[2] + viewport[0];
        tmp[1] = tmp[1] * viewport[3] + viewport[1];

        return new(tmp.X, tmp.Y, tmp.Z);
    }
    /// <summary>
    /// Map the specified window coordinates (win.x, win.y, win.z) into object coordinates.
    /// The near and far clip planes correspond to z normalized device coordinates of -1 and +1 respectively. (OpenGL clip volume definition)
    /// @see <a href="https://www.khronos.org/registry/OpenGL-Refpages/gl2.1/xhtml/gluUnProject.xml">gluUnProject man page</a>
    /// </summary>
    /// <param name="win">The win.win Specify the window coordinates to be mapped.</param>
    /// <param name="model">The model.model Specifies the modelview Matrix4 </param>
    /// <param name="proj">The proj.proj Specifies the projection Matrix4</param>
    /// <param name="viewport">The viewport. viewport Specifies the viewport</param>
    /// <returns> the computed object coordinates. 0.0f in Z</returns>
    public static Vector3 UnProject(Vector3 win, Matrix4 model, Matrix4 proj , Vector4 viewport)
    {
        // var tmp = (new Vector4( (win.X - viewport[0]) / viewport[2] ,
        //                         ((viewport[3] - win.Y) - viewport[1]) / viewport[3]  ,
        //                         win.Z,  1.0f  ) *2.0f) -1.0f ;
        Vector4 tmp = new Vector4(1.0f);
        tmp.X = (2.0f*(win.X-0)/(viewport[2]  )) -1.0f;
        tmp.Y = 1.0f-(2.0f*( viewport[3] - win.Y-0)/(viewport[3] ));
        tmp.Z = 2.0f* win.Z -1.0f ;
        tmp.W = 1.0f;

        Vector4 obj =  Matrix4.Inverse(proj * model) * tmp ;
        obj.W = 1.0f / obj.W;
        obj.X *= obj.W;
        obj.Y *= obj.W;
        obj.Z *= obj.W;
        return new Vector3(obj.X, obj.Y,obj.Z);
    }
    

    // /// <summary>
    // /// 
    // /// </summary>
    // /// <param name="x"></param>
    // /// <param name="y"></param>
    // /// <param name="camera"></param>
    // /// <param name="viewport"></param>
    // /// <returns></returns>
    // public static Vector3 MyUnproject(float x, float y, Camera camera ,Vector4 viewport)
    // {
    //         //  http://nehe.gamedev.net/article/using_gluunproject/16013/
    //     float nearCoordonate = 0.01f ;// assunming  in camera
    //     Vector4 v = new( 2 * ( x +0.5f)/ viewport[2], 1.0f - 2.0f *(  y +0.5f)/ viewport[3] , nearCoordonate ,1.0f     );
    //     Vector4 nearPlane = new(
    //         camera.ClipPlanes[3,0] + camera.ClipPlanes[2,0],
    //         camera.ClipPlanes[3,1] + camera.ClipPlanes[2,1],
    //         camera.ClipPlanes[3,2] + camera.ClipPlanes[2,2],
    //         camera.ClipPlanes[3,3] + camera.ClipPlanes[2,3]);
    //     Vector4 v1 = nearPlane * Vector4.Normalize(v);
    //     Vector4 v2 = camera.ClipPlanes  * v1;
    //     return new  Vector3( v2.X,v2.Y, v2.Z);
    //     /*
    //     https://computergraphics.stackexchange.com/questions/4503/screen-to-world-coordinates-glmunproject
    //     You can calculate the world position of the pixel on near plane quite easily by first defining Normalized Device Coordinates (NDC) for the point and then transforming the NDC back to the world space. You can calculate NDC for your point as follows:
    //     v=[2∗(x+0.5)/width−1,1−2∗(y+0.5)/height,0,1]
    //     I'm using 0 here for the z-component assuming near-plane in NDC is 0, but it could be something else as well, so check with your projection Matrix4 what values comes out at the near-plane. This vector must be then transformed to clip-space by multiplying by near-plane distance:
    //     v′=np∗v
    //     Now that you have clip-space coordinates, you just need to transform this back to world space with the standard clip→world transformation Matrix4:
    //     v′′=M∗v′
    //     */
    // }
#endregion    
#region view
    /// <summary>
    /// RH
    /// </summary>
    /// <param name="eye">ou position </param>
    /// <param name="center"> target/ front</param>
    /// <param name="up"> toujours vers le haut</param>
    /// <returns></returns>
    public static Matrix4 LookAt( Vector3 eye, Vector3 center, Vector3 up)
    {
        Vector3 f = Vector3.Normalize( center - eye  );
        Vector3 s = Vector3.Normalize( Vector3.Cross(ref f, ref up   ));
        Vector3 u = Vector3.Cross( ref s , ref f);
        Matrix4 Result = new(1.0f);
        Result[0,0] = s.X;
        Result[1,0] = s.Y;
        Result[2,0] = s.Z;
        Result[0,1] = u.X;
        Result[1,1] = u.Y;
        Result[2,1] = u.Z;
        Result[0,2] =-f.X;
        Result[1,2] =-f.Y;
        Result[2,2] =-f.Z;
        Result[3,0] =- Vector3.Dot(ref s,ref eye);
        Result[3,1] =- Vector3.Dot(ref u,ref eye);
        Result[3,2] =  Vector3.Dot(ref f,ref eye);
        return Result;
    }

#endregion    
#region Inverse        
    /// <summary>
    /// Calcul de la matrice inverse ( utilie pour project et unproject ...)
    /// </summary>
    /// <param name="m"> matrice a calculer l'inverse </param>
    /// <returns></returns>
    public static Matrix4 Inverse(Matrix4 m)
    {
        float Coef00 = m[2][2] * m[3][3] - m[3][2] * m[2][3];
        float Coef02 = m[1][2] * m[3][3] - m[3][2] * m[1][3];
        float Coef03 = m[1][2] * m[2][3] - m[2][2] * m[1][3];

        float Coef04 = m[2][1] * m[3][3] - m[3][1] * m[2][3];
        float Coef06 = m[1][1] * m[3][3] - m[3][1] * m[1][3];
        float Coef07 = m[1][1] * m[2][3] - m[2][1] * m[1][3];

        float Coef08 = m[2][1] * m[3][2] - m[3][1] * m[2][2];
        float Coef10 = m[1][1] * m[3][2] - m[3][1] * m[1][2];
        float Coef11 = m[1][1] * m[2][2] - m[2][1] * m[1][2];

        float Coef12 = m[2][0] * m[3][3] - m[3][0] * m[2][3];
        float Coef14 = m[1][0] * m[3][3] - m[3][0] * m[1][3];
        float Coef15 = m[1][0] * m[2][3] - m[2][0] * m[1][3];

        float Coef16 = m[2][0] * m[3][2] - m[3][0] * m[2][2];
        float Coef18 = m[1][0] * m[3][2] - m[3][0] * m[1][2];
        float Coef19 = m[1][0] * m[2][2] - m[2][0] * m[1][2];

        float Coef20 = m[2][0] * m[3][1] - m[3][0] * m[2][1];
        float Coef22 = m[1][0] * m[3][1] - m[3][0] * m[1][1];
        float Coef23 = m[1][0] * m[2][1] - m[2][0] * m[1][1];

        Vector4 Fac0 = new(Coef00, Coef00, Coef02, Coef03);
        Vector4 Fac1 = new(Coef04, Coef04, Coef06, Coef07);
        Vector4 Fac2 = new(Coef08, Coef08, Coef10, Coef11);
        Vector4 Fac3 = new(Coef12, Coef12, Coef14, Coef15);
        Vector4 Fac4 = new(Coef16, Coef16, Coef18, Coef19);
        Vector4 Fac5 = new(Coef20, Coef20, Coef22, Coef23);

        Vector4 Vec0 = new(m[1,0], m[0,0], m[0,0], m[0,0]);
        Vector4 Vec1 = new(m[1,1], m[0,1], m[0,1], m[0,1]);
        Vector4 Vec2 = new(m[1,2], m[0,2], m[0,2], m[0,2]);
        Vector4 Vec3 = new(m[1,3], m[0,3], m[0,3], m[0,3]);

        Vector4 Inv0 = new(Vec1 * Fac0 - Vec2 * Fac1 + Vec3 * Fac2);
        Vector4 Inv1 = new(Vec0 * Fac0 - Vec2 * Fac3 + Vec3 * Fac4);
        Vector4 Inv2 = new(Vec0 * Fac1 - Vec1 * Fac3 + Vec3 * Fac5);
        Vector4 Inv3 = new(Vec0 * Fac2 - Vec1 * Fac4 + Vec2 * Fac5);

        Vector4 SignA = new(+1, -1, +1, -1);
        Vector4 SignB = new(-1, +1, -1, +1);
        Matrix4 Inverse = new(Inv0 * SignA, Inv1 * SignB, Inv2 * SignA, Inv3 * SignB);

        Vector4 Row0 = new(Inverse[0][0], Inverse[1][0], Inverse[2][0], Inverse[3][0]);

        Vector4 Dot0 = new(m[0] * Row0);
        float Dot1 = (Dot0.X + Dot0.Y) + (Dot0.Z + Dot0.W);

        float OneOverDeterminant = (1.0f) / Dot1;

        return Inverse * OneOverDeterminant;
    }
#endregion

#region OVERRIDE
    /// <inheritdoc />
    public override string ToString() => $"\n\t{_cols[0]}\n\t{_cols[1]}\n\t{_cols[2]}\n\t{_cols[3]} ";
    /// <inheritdoc />
    public override int GetHashCode() => this.ToArray.GetHashCode() ;
    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Matrix4 mat && this.Equals(mat)  ;
    /// <inheritdoc />
    public bool Equals(Matrix4 other)
        => this._cols[0].Equals( other._cols[0]) && this._cols[1].Equals( other._cols[1]) &&this._cols[2].Equals( other._cols[2]) &&this._cols[3].Equals( other._cols[3]) ;
        // => (Maths.Abs(M11 - other.M11) <=  Maths.ZeroTolerance) && (Maths.Abs(M12 - other.M13) <= Maths.ZeroTolerance) && (Maths.Abs(M13 - other.M13) <= Maths.ZeroTolerance) && (Maths.Abs(M14 - other.M14) <= Maths.ZeroTolerance);
    /// <inheritdoc />
    public static bool operator ==(Matrix4 left, Matrix4 right) => left.Equals(right);
   /// <inheritdoc />
    public static bool operator !=(Matrix4 left, Matrix4 right) => !(left.Equals(right));
#endregion
    /// <summary>
    /// https://stackoverflow.com/questions/42256657/glmunproject-doesnt-work-and-incorrect-ray-position-on-screen
    /// </summary>
    /// <param name="view"></param>
    /// <param name="projection"></param>
    /// <param name="viewport"></param>
    /// <param name="posx"></param>
    /// <param name="posy"></param>
    public static (Vector3,Vector3) Ray(Matrix4 view, Matrix4 projection,Vector4 viewport, int posx, int posy )
    {
        Matrix4 mvp = view * projection ;
        var x = posx;
        var y = viewport[3] - 1.0f - posy;

        Vector4 localNear = new( x,  y,0.0f, 1.0f );
        Vector4 localFar = new( x, y, 1.0f, 1.0f );

        Vector4 wsn = Matrix4.Inverse(mvp) * localNear;
        Vector3 worldSpaceNear = new(wsn.X, wsn.Y, wsn.Z);

        Vector4 wsf = Matrix4.Inverse(mvp) * localFar;
        Vector3 worldSpaceFar = new(wsf.X, wsf.Y, wsf.Z);

        return new( worldSpaceNear, worldSpaceFar);
    }

    /// <summary>
    /// .
    /// </summary>
    /// <param name="v"></param>
    /// <param name="mat"></param>
    /// <returns></returns>
    public static Vector4 Vector4Transform( Vector4 v , Matrix4 mat)
    {
        Vector4 result =default;

        result.X = (mat[0,0]*v.X) + (mat[1,0]* v.Y) + (mat[2,0]* v.Z) + (mat[3,0]* v.W);
        result.Y = (mat[0,1]*v.X) + (mat[1,1]* v.Y) + (mat[2,1]* v.Z) + (mat[3,1]* v.W);
        result.Z = (mat[0,2]*v.X) + (mat[1,2]* v.Y) + (mat[2,2]* v.Z) + (mat[3,2]* v.W);
        result.W = (mat[0,3]*v.X) + (mat[1,3]* v.Y) + (mat[2,3]* v.Z) + (mat[3,3]* v.W);

        return result;
    }
}



#endif



// // ███    ███  █████  ████████ ██████  ██ ██   ██     ██   ██ ██   ██ ██   ██ 
// // ████  ████ ██   ██    ██    ██   ██ ██  ██ ██      ██   ██  ██ ██  ██   ██ 
// // ██ ████ ██ ███████    ██    ██████  ██   ███       ███████   ███   ███████ 
// // ██  ██  ██ ██   ██    ██    ██   ██ ██  ██ ██           ██  ██ ██       ██ 
// // ██      ██ ██   ██    ██    ██   ██ ██ ██   ██          ██ ██   ██      ██ 

// */
//     /// <summary>
//     /// muliplication de deux matrice ! l'ordre 
//     /// https://github.com/g-truc/glm/blob/master/glm/detail/type_mat4x4.inl
//     /// https://github.com/dwmkerr/glmnet/blob/master/source/GlmNet/GlmNet/mat4.cs
//     /// </summary>
//     /// <param name="lhs"></param>
//     /// <param name="rhs"></param>
//     /// <returns></returns>
//     public static Matrix operator * ( Matrix lhs, Matrix rhs)
//         => new( 
//             rhs[0][0] * lhs[0] + rhs[0][1] * lhs[1] + rhs[0][2] * lhs[2] + rhs[0][3] * lhs[3],
//             rhs[1][0] * lhs[0] + rhs[1][1] * lhs[1] + rhs[1][2] * lhs[2] + rhs[1][3] * lhs[3],
//             rhs[2][0] * lhs[0] + rhs[2][1] * lhs[1] + rhs[2][2] * lhs[2] + rhs[2][3] * lhs[3],
//             rhs[3][0] * lhs[0] + rhs[3][1] * lhs[1] + rhs[3][2] * lhs[2] + rhs[3][3] * lhs[3]
//         );
//     /// <summary>
//     /// multiplication d'une matrice par un scalaire
//     /// </summary>
//     /// <param name="lhs"></param>
//     /// <param name="s"></param>
//     /// <returns></returns>
//     public static Matrix operator * ( Matrix lhs , float s)
//         => new( lhs[0]*s ,lhs[1]*s ,lhs[2]*s ,lhs[3]*s );
    
//     /// <summary>
//     /// Multiplies the <paramref name="lhs"/> matrix by the <paramref name="rhs"/> vector.
//     /// </summary>
//     /// <param name="lhs">The LHS matrix.</param>
//     /// <param name="rhs">The RHS vector.</param>
//     /// <returns>The product of <paramref name="lhs"/> and <paramref name="rhs"/>.</returns>
//     public static Vector4 operator *(Matrix lhs, Vector4 rhs)
//         =>new (
//             lhs[0, 0] * rhs[0] + lhs[1, 0] * rhs[1] + lhs[2, 0] * rhs[2] + lhs[3, 0] * rhs[3],
//             lhs[0, 1] * rhs[0] + lhs[1, 1] * rhs[1] + lhs[2, 1] * rhs[2] + lhs[3, 1] * rhs[3],
//             lhs[0, 2] * rhs[0] + lhs[1, 2] * rhs[1] + lhs[2, 2] * rhs[2] + lhs[3, 2] * rhs[3],
//             lhs[0, 3] * rhs[0] + lhs[1, 3] * rhs[1] + lhs[2, 3] * rhs[2] + lhs[3, 3] * rhs[3]
//         );
//     /// <summary>
//     ///  is not a number
//     /// </summary>
//     /// <returns></returns>
//     public bool IsNaN =>float.IsNaN( this[0,0] ) || float.IsNaN( this[0,1]) || float.IsNaN( this[0,2]) || float.IsNaN( this[0,3]) ||
//                 float.IsNaN( this[1,0] ) || float.IsNaN( this[1,1]) || float.IsNaN( this[1,2]) || float.IsNaN( this[1,3]) ||
//                 float.IsNaN( this[2,0] ) || float.IsNaN( this[2,1]) || float.IsNaN( this[2,2]) || float.IsNaN( this[2,3]) ||
//                 float.IsNaN( this[3,0] ) || float.IsNaN( this[3,1]) || float.IsNaN( this[3,2]) || float.IsNaN( this[3,3]) ;
// #endregion        

// #region transform
//     /// <summary>
//     ///  translation  faire var newMat4 = Matrix.Translation( m , nouvellePosition)
//     /// </summary>
//     /// <param name="m"></param>
//     /// <param name="v"></param>
//     /// <returns></returns>
//     public static Matrix Translate(Matrix m, Vector3 v) 
//         => new( m[0], m[1] , m[2], m[0] * v[0] + m[1] * v[1] + m[2] * v[2] + m[3] );
    
//     /// <summary>
//     /// Creates a new <see cref="Matrix"/> which contains the rotation moment around specified axis.
//     /// </summary>
//     /// <param name="m"></param>
//     /// <param name="angle">In radians </param>
//     /// <param name="v"></param>
//     /// <returns></returns>
//     public static Matrix Rotate(Matrix m,in float angle,Vector3 v)
//     {
//         var c = Cos(  angle  );
//         var s = Sin(  angle  );

//         Vector3 axis = new(v);
//         axis.Normalize();
//         Vector3 temp = new ((1 - c) * axis);

//         return new(
//             m[0] * (c + temp[0] * axis[0]) + m[1] * (temp[0] * axis[1] + s * axis[2]) + m[2] * (temp[0] * axis[2] - s * axis[1]),
//             m[0] * (temp[1] * axis[0] - s * axis[2])  + m[1] * (c + temp[1] * axis[1]) + m[2] * (temp[1] * axis[2] + s * axis[0]),
//             m[0] * (temp[2] * axis[0] + s * axis[1]) + m[1] * (temp[2] * axis[1] - s * axis[0]) + m[2] * (c + temp[2] * axis[2]),
//             m[3]);
//     }
//     /// <summary>
//     /// Mise à l'échelle
//     /// </summary>
//     /// <param name="m"></param>
//     /// <param name="v"></param>
//     /// <returns></returns>
//     public static Matrix Scale(Matrix m , Vector3 v)
//         => new (m[0] * v[0] ,m[1] * v[1] ,m[2] * v[2] , m[3]  );
    
// #endregion
// #region projection
//         /// <summary>
//     /// Creates a frustrum projection matrix.
//     /// </summary>
//     /// <param name="left">The left.</param>
//     /// <param name="right">The right.</param>
//     /// <param name="bottom">The bottom.</param>
//     /// <param name="top">The top.</param>
//     /// <param name="nearVal">The near val.</param>
//     /// <param name="farVal">The far val.</param>
//     /// <returns></returns>
//     public static Matrix Frustum(float left, float right, float bottom, float top, float nearVal, float farVal)
//     {
//         Matrix result = new(1.0f);//identity
//         result[0, 0] = (2.0f * nearVal) / (right - left);
//         result[1, 1] = (2.0f * nearVal) / (top - bottom);
//         result[2, 0] = (right + left) / (right - left);
//         result[2, 1] = (top + bottom) / (top - bottom);
//         result[2, 2] = -(farVal + nearVal) / (farVal - nearVal);
//         result[2, 3] = -1.0f;
//         result[3, 2] = -(2.0f * farVal * nearVal) / (farVal - nearVal);
//         return result;
//     }
//         /// <summary>
//     /// Creates a matrix for a symmetric perspective-view frustum with far plane at infinite.
//     /// </summary>
//     /// <param name="fovy">The fovy.</param>
//     /// <param name="aspect">The aspect.</param>
//     /// <param name="zNear">The z near.</param>
//     /// <returns></returns>
//     public static Matrix InfinitePerspective(float fovy, float aspect, float zNear)
//     {
//         float range = Tan(fovy / (2f)) * zNear;

//         float left = -range * aspect;
//         float right = range * aspect;
//         float bottom = -range;
//         float top = range;

//         Matrix result = new(0.0f);
//         result[0, 0] = ((2f) * zNear) / (right - left);
//         result[1, 1] = ((2f) * zNear) / (top - bottom);
//         result[2, 2] = -(1f);
//         result[2, 3] = -(1f);
//         result[3, 2] = -(2f) * zNear;
//         return result;
//     }
//     /// <summary>
//     /// Creates a matrix for a right handed, symetric perspective-view frustum.
//     /// The near and far clip planes correspond to z normalized device coordinates of -1 and +1 respectively. (OpenGL clip volume definition)
//     ///
//     /// @param fovy Specifies the field of view angle, in degrees, in the y direction. Expressed in radians.
//     /// @param aspect Specifies the aspect ratio that determines the field of view in the x direction. The aspect ratio is the ratio of x (width) to y (height).
//     /// @param near Specifies the distance from the viewer to the near clipping plane (always positive).
//     /// @param far Specifies the distance from the viewer to the far clipping plane (always positive).
//     ///
//     /// @tparam T A floating-point scalar type 
//     /// </summary>
//     /// <param name="fovdegree"></param>
//     /// <param name="width"></param>
//     /// <param name="height"></param>
//     /// <param name="zNear"></param>
//     /// <param name="zFar"></param>
//     /// <returns></returns>
//     public static Matrix Perspective(float fovdegree,float width,float height,float zNear,float zFar )
//     {
//         float aspect = width / height;
//         float tanHalfFovy = Tan( ToRadians(fovdegree) / 2.0f);

//         Matrix Result = new(1.0f);
//         Result[0,0] = 1.0f / (aspect * tanHalfFovy);
//         Result[1,1] = 1.0f / (tanHalfFovy);
//         Result[2,2] = - (zFar + zNear) / (zFar - zNear);
//         Result[2,3] = - 1.0f;
//         Result[3,2] = - (2.0f * zFar * zNear) / (zFar - zNear);
//         Result[3,3] = 0.0f;
//         return Result;
//     }
//     /// <summary>
//     /// Builds a perspective projection matrix based on a field of view using right-handed coordinates.
//     /// The near and far clip planes correspond to z normalized device coordinates of -1 and +1 respectively. (OpenGL clip volume definition)
//     ///
//     /// @param fov Expressed in radians.
//     /// @param width Width of the viewport
//     /// @param height Height of the viewport
//     /// @param near Specifies the distance from the viewer to the near clipping plane (always positive).
//     /// @param far Specifies the distance from the viewer to the far clipping plane (always positive).
//     ///
//     /// @tparam T A floating-point scalar type 
//     /// </summary>
//     /// <param name="fovdegree"></param>
//     /// <param name="width"></param>
//     /// <param name="height"></param>
//     /// <param name="zNear"></param>
//     /// <param name="zFar"></param>
//     /// <returns></returns>
//     public static Matrix PerspectiveFOV(float fovdegree,float width,float height,float zNear,float zFar )
//     {
//         if (width <= 0 || height <= 0 || fovdegree <= 0) throw new ArgumentOutOfRangeException();
        
//         float rad =ToRadians( fovdegree ) ;
//         float h = Cos(  0.5f * rad) / Sin( 0.5f * rad);
//         float w = h * height / width; //todo max(width , Height) / min(width , Height)?

//         return new(  w ,    0.0f,   0.0f,                                0.0f,
//                     0.0f,   h,      0.0f,                                0.0f,
//                     0.0f,   0.0f,- (zFar + zNear) / (zFar - zNear) ,    -1.0f,
//                     0.0f,   0.0f,- (2.0f * zFar * zNear) / (zFar - zNear),0.0f        );
//     }
//     /// <summary>
//     /// Creates a matrix for an orthographic parallel viewing volume, using right-handed coordinates.
//     /// The near and far clip planes correspond to z normalized device coordinates of -1 and +1 respectively. (OpenGL clip volume definition)
//     ///
//     /// @tparam T A floating-point scalar type
//     /// _RHNO
//     /// @see - glm::ortho(T  left, T  right, T  bottom, T  top) 
//     /// </summary>
//     /// <param name="left"></param>
//     /// <param name="right"></param>
//     /// <param name="top"></param>
//     /// <param name="bottom"></param>
//     /// <param name="zNear"></param>
//     /// <param name="zFar"></param>
//     /// <returns></returns>
//     public static Matrix Ortho(float left,float right,float top,float bottom,float zNear,float zFar )
//     {   
//         Matrix Result = new(1.0f);
//         Result[0,0] = 2.0f / (right - left);
//         Result[1,1] = 2.0f / (top - bottom);
//         Result[2,2] = - 2.0f / (zFar - zNear);
//         Result[3,0] = - (right + left) / (right - left);
//         Result[3,1] = - (top + bottom) / (top - bottom);
//         Result[3,2] = - (zFar + zNear) / (zFar - zNear);
//         return Result;
//     }

//     /// <summary>
//     /// Creates a matrix for projecting two-dimensional coordinates onto the screen.
//     /// </summary>
//     /// <param name="left">The left.</param>
//     /// <param name="right">The right.</param>
//     /// <param name="bottom">The bottom.</param>
//     /// <param name="top">The top.</param>
//     /// <returns></returns>
//     public static Matrix Ortho2D(float left, float right, float bottom, float top)
//     {
//         Matrix result = new(1.0f);
//         result[0, 0] = (2f) / (right - left);
//         result[1, 1] = (2f) / (top - bottom);
//         result[2, 2] = -(1f);
//         result[3, 0] = -(right + left) / (right - left);
//         result[3, 1] = -(top + bottom) / (top - bottom);
//         return result;
//     }

//     /// <summary>
//     /// Creates a matrix for a symmetric perspective-view frustum with far plane 
//     /// at infinite for graphics hardware that doesn't support depth clamping.
//     /// </summary>
//     /// <param name="fovy">The fovy. in radian</param>
//     /// <param name="aspect">The aspect.</param>
//     /// <param name="zNear">The z near.</param>
//     /// <returns></returns>
//     public static Matrix TweakedInfinitePerspective(float fovy, float aspect, float zNear)
//     {
//         float range = Tan(fovy / (2)) * zNear;
//         float left = -range * aspect;
//         float right = range * aspect;
//         float bottom = -range;
//         float top = range;

//         Matrix Result = new(0.0f);
//         Result[0, 0] = ((2) * zNear) / (right - left);
//         Result[1, 1] = ((2) * zNear) / (top - bottom);
//         Result[2, 2] = (0.0001f) - (1f);
//         Result[2, 3] = (-1);
//         Result[3, 2] = -((0.0001f) - (2)) * zNear;
//         return Result;
//     }

//     /// <summary>
//     /// Define a picking region.
//     /// </summary>
//     /// <param name="center">The center.</param>
//     /// <param name="delta">The delta.</param>
//     /// <param name="viewport">The viewport.</param>
//     /// <returns></returns>
//     /// <exception cref="System.ArgumentOutOfRangeException"></exception>
//     public static Matrix PickMatrix(Vector2 center, Vector2 delta, Vector4 viewport)
//     {
//         if (delta.X <= 0 || delta.Y <= 0)
//             throw new ArgumentOutOfRangeException();
//         var Result = new Matrix(1.0f);

//         if (!(delta.X > (0f) && delta.Y > (0f)))
//             return Result; // Error

//         Vector3 Temp = new(
//             ((viewport[2]) - (2f) * (center.X - (viewport[0]))) / delta.X,
//             ((viewport[3]) - (2f) * (center.Y - (viewport[1]))) / delta.Y,
//             (0f));

//         // Translate and scale the picked region to the entire window
//         Result = Matrix.Translate(Result, Temp);
//         return  Matrix.Scale(Result, new((viewport[2]) / delta.X, (viewport[3]) / delta.Y, (1)));
//     }

//     /// <summary>
//     /// Map the specified object coordinates (obj.x, obj.y, obj.z) into window coordinates.
//     /// </summary>
//     /// <param name="obj">The object.</param>
//     /// <param name="model">The model.</param>
//     /// <param name="proj">The proj.</param>
//     /// <param name="viewport">The viewport.</param>
//     /// <returns></returns>
//     public static Vector3 Project(Vector3 obj, Matrix model, Matrix proj, Vector4 viewport)
//     {
//         Vector4 tmp = new(obj.X,obj.Y,obj.Z, 1.0f);
//         tmp = model * tmp;
//         tmp = proj * tmp;

//         tmp /= tmp.W;
//         tmp = tmp * 0.5f + 0.5f;
//         tmp[0] = tmp[0] * viewport[2] + viewport[0];
//         tmp[1] = tmp[1] * viewport[3] + viewport[1];

//         return new(tmp.X, tmp.Y, tmp.Z);
//     }
//     /// <summary>
//     /// Map the specified window coordinates (win.x, win.y, win.z) into object coordinates.
//     /// The near and far clip planes correspond to z normalized device coordinates of -1 and +1 respectively. (OpenGL clip volume definition)
//     /// @see <a href="https://www.khronos.org/registry/OpenGL-Refpages/gl2.1/xhtml/gluUnProject.xml">gluUnProject man page</a>
//     /// </summary>
//     /// <param name="win">The win.win Specify the window coordinates to be mapped.</param>
//     /// <param name="model">The model.model Specifies the modelview matrix </param>
//     /// <param name="proj">The proj.proj Specifies the projection matrix</param>
//     /// <param name="viewport">The viewport. viewport Specifies the viewport</param>
//     /// <returns> the computed object coordinates. 0.0f in Z</returns>
//     public static Vector3 UnProject(Vector3 win, Matrix model, Matrix proj , Vector4 viewport)
//     {
//         // var tmp = (new Vector4( (win.X - viewport[0]) / viewport[2] ,
//         //                         ((viewport[3] - win.Y) - viewport[1]) / viewport[3]  ,
//         //                         win.Z,  1.0f  ) *2.0f) -1.0f ;
//         Vector4 tmp = new Vector4(1.0f);
//         tmp.X = (2.0f*(win.X-0)/(viewport[2]  )) -1.0f;
//         tmp.Y = 1.0f-(2.0f*( viewport[3] - win.Y-0)/(viewport[3] ));
//         tmp.Z = 2.0f* win.Z -1.0f ;
//         tmp.W = 1.0f;

//         Vector4 obj =  Matrix.Inverse(proj * model) * tmp ;
//         obj.W = 1.0f / obj.W;
//         obj.X *= obj.W;
//         obj.Y *= obj.W;
//         obj.Z *= obj.W;
//         return new Vector3(obj.X, obj.Y,obj.Z);
//     }
    

//     // /// <summary>
//     // /// 
//     // /// </summary>
//     // /// <param name="x"></param>
//     // /// <param name="y"></param>
//     // /// <param name="camera"></param>
//     // /// <param name="viewport"></param>
//     // /// <returns></returns>
//     // public static Vector3 MyUnproject(float x, float y, Camera camera ,Vector4 viewport)
//     // {
//     //         //  http://nehe.gamedev.net/article/using_gluunproject/16013/
//     //     float nearCoordonate = 0.01f ;// assunming  in camera
//     //     Vector4 v = new( 2 * ( x +0.5f)/ viewport[2], 1.0f - 2.0f *(  y +0.5f)/ viewport[3] , nearCoordonate ,1.0f     );
//     //     Vector4 nearPlane = new(
//     //         camera.ClipPlanes[3,0] + camera.ClipPlanes[2,0],
//     //         camera.ClipPlanes[3,1] + camera.ClipPlanes[2,1],
//     //         camera.ClipPlanes[3,2] + camera.ClipPlanes[2,2],
//     //         camera.ClipPlanes[3,3] + camera.ClipPlanes[2,3]);
//     //     Vector4 v1 = nearPlane * Vector4.Normalize(v);
//     //     Vector4 v2 = camera.ClipPlanes  * v1;
//     //     return new  Vector3( v2.X,v2.Y, v2.Z);
//     //     /*
//     //     https://computergraphics.stackexchange.com/questions/4503/screen-to-world-coordinates-glmunproject
//     //     You can calculate the world position of the pixel on near plane quite easily by first defining Normalized Device Coordinates (NDC) for the point and then transforming the NDC back to the world space. You can calculate NDC for your point as follows:
//     //     v=[2∗(x+0.5)/width−1,1−2∗(y+0.5)/height,0,1]
//     //     I'm using 0 here for the z-component assuming near-plane in NDC is 0, but it could be something else as well, so check with your projection matrix what values comes out at the near-plane. This vector must be then transformed to clip-space by multiplying by near-plane distance:
//     //     v′=np∗v
//     //     Now that you have clip-space coordinates, you just need to transform this back to world space with the standard clip→world transformation matrix:
//     //     v′′=M∗v′
//     //     */
//     // }
// #endregion    
// #region view
//     /// <summary>
//     /// RH
//     /// </summary>
//     /// <param name="eye">ou position </param>
//     /// <param name="center"> target/ front</param>
//     /// <param name="up"> toujours vers le haut</param>
//     /// <returns></returns>
//     public static Matrix LookAt( Vector3 eye, Vector3 center, Vector3 up)
//     {
//         Vector3 f = Vector3.Normalize( center - eye  );
//         Vector3 s = Vector3.Normalize( Vector3.Cross(ref f, ref up   ));
//         Vector3 u = Vector3.Cross( ref s , ref f);
//         Matrix Result = new(1.0f);
//         Result[0,0] = s.X;
//         Result[1,0] = s.Y;
//         Result[2,0] = s.Z;
//         Result[0,1] = u.X;
//         Result[1,1] = u.Y;
//         Result[2,1] = u.Z;
//         Result[0,2] =-f.X;
//         Result[1,2] =-f.Y;
//         Result[2,2] =-f.Z;
//         Result[3,0] =- Vector3.Dot(ref s,ref eye);
//         Result[3,1] =- Vector3.Dot(ref u,ref eye);
//         Result[3,2] =  Vector3.Dot(ref f,ref eye);
//         return Result;
//     }

// #endregion    
// #region Inverse        
//     /// <summary>
//     /// Calcul de la matrice inverse ( utilie pour project et unproject ...)
//     /// </summary>
//     /// <param name="m"> matrice a calculer l'inverse </param>
//     /// <returns></returns>
//     public static Matrix Inverse(Matrix m)
//     {
//         float Coef00 = m[2][2] * m[3][3] - m[3][2] * m[2][3];
//         float Coef02 = m[1][2] * m[3][3] - m[3][2] * m[1][3];
//         float Coef03 = m[1][2] * m[2][3] - m[2][2] * m[1][3];

//         float Coef04 = m[2][1] * m[3][3] - m[3][1] * m[2][3];
//         float Coef06 = m[1][1] * m[3][3] - m[3][1] * m[1][3];
//         float Coef07 = m[1][1] * m[2][3] - m[2][1] * m[1][3];

//         float Coef08 = m[2][1] * m[3][2] - m[3][1] * m[2][2];
//         float Coef10 = m[1][1] * m[3][2] - m[3][1] * m[1][2];
//         float Coef11 = m[1][1] * m[2][2] - m[2][1] * m[1][2];

//         float Coef12 = m[2][0] * m[3][3] - m[3][0] * m[2][3];
//         float Coef14 = m[1][0] * m[3][3] - m[3][0] * m[1][3];
//         float Coef15 = m[1][0] * m[2][3] - m[2][0] * m[1][3];

//         float Coef16 = m[2][0] * m[3][2] - m[3][0] * m[2][2];
//         float Coef18 = m[1][0] * m[3][2] - m[3][0] * m[1][2];
//         float Coef19 = m[1][0] * m[2][2] - m[2][0] * m[1][2];

//         float Coef20 = m[2][0] * m[3][1] - m[3][0] * m[2][1];
//         float Coef22 = m[1][0] * m[3][1] - m[3][0] * m[1][1];
//         float Coef23 = m[1][0] * m[2][1] - m[2][0] * m[1][1];

//         Vector4 Fac0 = new(Coef00, Coef00, Coef02, Coef03);
//         Vector4 Fac1 = new(Coef04, Coef04, Coef06, Coef07);
//         Vector4 Fac2 = new(Coef08, Coef08, Coef10, Coef11);
//         Vector4 Fac3 = new(Coef12, Coef12, Coef14, Coef15);
//         Vector4 Fac4 = new(Coef16, Coef16, Coef18, Coef19);
//         Vector4 Fac5 = new(Coef20, Coef20, Coef22, Coef23);

//         Vector4 Vec0 = new(m[1,0], m[0,0], m[0,0], m[0,0]);
//         Vector4 Vec1 = new(m[1,1], m[0,1], m[0,1], m[0,1]);
//         Vector4 Vec2 = new(m[1,2], m[0,2], m[0,2], m[0,2]);
//         Vector4 Vec3 = new(m[1,3], m[0,3], m[0,3], m[0,3]);

//         Vector4 Inv0 = new(Vec1 * Fac0 - Vec2 * Fac1 + Vec3 * Fac2);
//         Vector4 Inv1 = new(Vec0 * Fac0 - Vec2 * Fac3 + Vec3 * Fac4);
//         Vector4 Inv2 = new(Vec0 * Fac1 - Vec1 * Fac3 + Vec3 * Fac5);
//         Vector4 Inv3 = new(Vec0 * Fac2 - Vec1 * Fac4 + Vec2 * Fac5);

//         Vector4 SignA = new(+1, -1, +1, -1);
//         Vector4 SignB = new(-1, +1, -1, +1);
//         Matrix Inverse = new(Inv0 * SignA, Inv1 * SignB, Inv2 * SignA, Inv3 * SignB);

//         Vector4 Row0 = new(Inverse[0][0], Inverse[1][0], Inverse[2][0], Inverse[3][0]);

//         Vector4 Dot0 = new(m[0] * Row0);
//         float Dot1 = (Dot0.X + Dot0.Y) + (Dot0.Z + Dot0.W);

//         float OneOverDeterminant = (1.0f) / Dot1;

//         return Inverse * OneOverDeterminant;
//     }
// #endregion
//     /// <summary>
//     /// https://stackoverflow.com/questions/42256657/glmunproject-doesnt-work-and-incorrect-ray-position-on-screen
//     /// </summary>
//     /// <param name="view"></param>
//     /// <param name="projection"></param>
//     /// <param name="viewport"></param>
//     /// <param name="posx"></param>
//     /// <param name="posy"></param>
//     public static (Vector3,Vector3) Ray(Matrix view, Matrix projection,Vector4 viewport, int posx, int posy )
//     {
//         Matrix mvp = view * projection ;
//         var x = posx;
//         var y = viewport[3] - 1.0f - posy;

//         Vector4 localNear = new( x,  y,0.0f, 1.0f );
//         Vector4 localFar = new( x, y, 1.0f, 1.0f );

//         Vector4 wsn = Matrix.Inverse(mvp) * localNear;
//         Vector3 worldSpaceNear = new(wsn.X, wsn.Y, wsn.Z);

//         Vector4 wsf = Matrix.Inverse(mvp) * localFar;
//         Vector3 worldSpaceFar = new(wsf.X, wsf.Y, wsf.Z);

//         return new( worldSpaceNear, worldSpaceFar);
//     }

//     /// <summary>
//     /// 
//     /// </summary>
//     /// <param name="v"></param>
//     /// <param name="mat"></param>
//     /// <returns></returns>
//     public static Vector4 Vector4Transform( Vector4 v , Matrix mat)
//     {
//         Vector4 result =default;

//         result.X = mat[0,0]*v.X + mat[1,0]* v.Y + mat[2,0]* v.Z + mat[3,0]* v.W;
//         result.Y = mat[0,1]*v.X + mat[1,1]* v.Y + mat[2,1]* v.Z + mat[3,1]* v.W;
//         result.Z = mat[0,2]*v.X + mat[1,2]* v.Y + mat[2,2]* v.Z + mat[3,2]* v.W;
//         result.W = mat[0,3]*v.X + mat[1,3]* v.Y + mat[2,3]* v.Z + mat[3,3]* v.W;

//         return result;
//     }
    
//     // // Get camera 2d transform matrix
//     // Matrix GetCameraMatrix2D(Camera camera)
//     // {
//     //     Matrix matTransform = new(0.0f);
//     //     // The camera in world-space is set by
//     //     //   1. Move it to target
//     //     //   2. Rotate by -rotation and scale by (1/zoom)
//     //     //      When setting higher scale, it's more intuitive for the world to become bigger (= camera become smaller),
//     //     //      not for the camera getting bigger, hence the invert. Same deal with rotation.
//     //     //   3. Move it by (-offset);
//     //     //      Offset defines target transform relative to screen, but since we're effectively "moving" screen (camera)
//     //     //      we need to do it into opposite direction (inverse transform)

//     //     // Having camera transform in world-space, inverse of it gives the modelview transform.
//     //     // Since (A*B*C)' = C'*B'*A', the modelview is
//     //     //   1. Move to offset
//     //     //   2. Rotate and Scale
//     //     //   3. Move by -target
        
//     //     // Matrix matOrigin = Matrix.Translate(-camera.Target.X, -camera.Target.Y, 0.0f);
//     //     // Matrix matRotation = Matrix.Rotate((Vector3){ 0.0f, 0.0f, 1.0f }, camera.rotation*DEG2RAD);
//     //     // Matrix matScale = Matrix.Scale(camera.zoom, camera.zoom, 1.0f);
//     //     // Matrix matTranslation = Matrix.Translate(camera.offset.x, camera.offset.y, 0.0f);

//     //     // matTransform = matOrigin * matScale * matRotation * matTranslation;

//     //     return matTransform;
//     // }
    
//     // // Get the screen space position for a 2d camera world space position
//     // Vector2 GetWorldToScreen2D(Vector3 position, Camera camera)
//     // {
//     //     Matrix invMatCamera = Matrix.Inverse(GetCameraMatrix2D(camera));
//     //     Vector4 transform = Vector4Transform(new Vector4( position.X, position.Y, 0.0f,1.0f), invMatCamera);

//     //     return new Vector2(transform.X, transform.Y );
//     // }
// }




























// // // namespace MCJGame.Engine.Math;

// // // using System;
// // // using System.Runtime.InteropServices;
// // // using System.Runtime.CompilerServices;

// // // /// <summary>
// // // /// delimité les objets dans la surface d'affichage
// // // /// </summary>
// // // [SkipLocalsInit][StructLayout(LayoutKind.Sequential, Pack = 4 )]
// // // public struct ClipPlanes : IDisposable, IEquatable<ClipPlanes>
// // // {
// // // #region Attributs        
// // //     private Vector4 _left;
// // //     private Vector4 _right;
// // //     private Vector4 _top;
// // //     private Vector4 _bottom;
// // //     private Vector4 _near;
// // //     private Vector4 _far;
// // // #endregion
    
// // // #region Limits        
// // //     /// <summary>
// // //     /// 
// // //     /// </summary>
// // //     /// <param name="p"></param>
// // //     /// <param name="radius"></param>
// // //     /// <returns></returns>     
// // //     public bool IsLeftLimit(  Vector3 p,in float radius=0.0f)
// // //         => ((_left.X * p.X) + (_left.Y * p.Y) + (_left.Z * p.Z)  + (_left.W + radius) ) <= 0   ;
    
// // //     /// <summary>
// // //     /// Detecte si le point est dans le fustrum
// // //     /// </summary>
// // //     /// <param name="p"></param>
// // //     /// <param name="radius"></param>
// // //     /// <returns></returns>
// // //     public bool IsRightLimit( Vector3 p,in float radius=0.0f)
// // //         => ((_right.X * p.X) + (_right.Y * p.Y) + (_right.Z * p.Z) + _right.W + radius )<= 0   ;

// // //     /// <summary>
// // //     /// 
// // //     /// </summary>
// // //     /// <param name="p"></param>
// // //     /// <param name="radius"></param>
// // //     /// <returns></returns>
// // //     public bool IsBottomLimit(  Vector3 p,in float radius=0.0f)
// // //         => ((_bottom.X * p.X) + (_bottom.Y * p.Y) + (_bottom.Z * p.Z) + _bottom.W + radius )<= 0   ;

// // //     /// <summary>
// // //     /// 
// // //     /// </summary>
// // //     /// <param name="p"></param>
// // //     /// <param name="radius"></param>
// // //     /// <returns></returns>
// // //     public bool IsTopLimit( Vector3 p,in float radius=0.0f)
// // //         => ((_top.X * p.X) + (_top.Y * p.Y) + (_top.Z * p.Z) + _top.W + radius) <= 0   ;
    
// // //     /// <summary>
// // //     /// 
// // //     /// </summary>
// // //     /// <param name="p"></param>
// // //     /// <param name="radius"></param>
// // //     /// <returns></returns>
// // //     public bool IsNearLimit( Vector3 p,in float radius=0.0f)
// // //         => ((_near.X * p.X) + (_near.Y * p.Y) + (_near.Z * p.Z) + _near.W + radius )<= 0   ;

// // //     /// <summary>
// // //     /// 
// // //     /// </summary>
// // //     /// <param name="p"></param>
// // //     /// <param name="radius"></param>
// // //     /// <returns></returns>
// // //     public bool IsFarLimit( Vector3 p,in float radius)
// // //         => ((_far.X * p.X) + (_far.Y * p.Y) + (_far.Z * p.Z) + _far.W + radius )<= 0   ;
    
// // //     /// <summary>
// // //     ///  Determin si un objet et contenu dans le fustrum
// // //     /// </summary>
// // //     /// <param name="p"></param>
// // //     /// <param name="radius"></param>
// // //     /// <returns></returns>
// // //     public bool Culling( Vector3 p,in float radius =0.0f)
// // //         => IsLeftLimit(   p,  radius)
// // //         || IsRightLimit(  p,  radius)
// // //         || IsTopLimit(   p,  radius)
// // //         || IsBottomLimit(   p,  radius)
// // //         || IsNearLimit(  p,  radius)
// // //         || IsFarLimit(   p,  radius);

// // // #endregion
// // //     /// <summary>
// // //     /// Constructeur avec valeur init
// // //     /// </summary>
// // //     /// <param name="initValue"></param>
// // //     public ClipPlanes(float initValue=0.0f)
// // //         =>(_left,_right,_top,_bottom,_near,_far ) = (new(initValue),new(initValue),new(initValue),new(initValue),new(initValue),new(initValue) );

// // //     /// <summary>
// // //     /// /// (Re)Calculates the frustum planes. https://github.com/FUSEEProjectTeam/Fusee/blob/master/src/Math/Core/FrustumF.cs
// // //         /// If feeded with a projection matrix, the frustum planes are in View Space.
// // //         /// If feeded with a view projection matrix, the frustum planes are in World Space.
// // //         /// If feeded with a model view projection matrix, the frustum planes are in Model Space.
// // //         /// See: http://www8.cs.umu.se/kurser/5DV051/HT12/lab/plane_extraction.pdf
// // //     /// Calcul du fustrum
// // //     /// https://www.gamedevs.org/uploads/fast-extraction-viewing-frustum-planes-from-world-view-projection-matrix.pdf
// // //     /// Matrixx clip = projection  * modelview
// // //     /// </summary>
// // //     /// <param name="clip"></param>
// // //     /// <param name="normalize"></param>
// // //     public void CalculateFrustumPlanes( Matrix clip, bool normalize=true )
// // //     {
// // //         // Extract the numbers for the _right plane  ( col3 - col0 )
// // //         _right.X = clip[3,0] - clip[0,0];
// // //         _right.Y = clip[3,1] - clip[0,1];
// // //         _right.Z = clip[3,2] - clip[0,2];
// // //         _right.W = clip[3,3] - clip[0,3];

// // //         // Extract the numbers for the _left plane (col3 + col0 )
// // //         _left.X = clip[3,0] + clip[0,0];
// // //         _left.Y = clip[3,1] + clip[0,1];
// // //         _left.Z = clip[3,2] + clip[0,2];
// // //         _left.W = clip[3,3] + clip[0,3];

// // //         // Extract the _bottom plane     (  col3 + col1 )
// // //         _bottom.X = clip[3,0] + clip[1,0];
// // //         _bottom.Y = clip[3,1] + clip[1,1];
// // //         _bottom.Z = clip[3,2] + clip[1,2];
// // //         _bottom.W = clip[3,3] + clip[1,3];

// // //         // Extract the _top plane      (  col3  - col1 )
// // //         _top.X = clip[3,0] - clip[1,0];
// // //         _top.Y = clip[3,1] - clip[1,1];
// // //         _top.Z = clip[3,2] - clip[1,2];
// // //         _top.W = clip[3,3] - clip[1,3];

// // //         // Extract the _far plane   (   col3 -  col2  )
// // //         _near.X = clip[3,0] + clip[2,0];
// // //         _near.Y = clip[3,1] + clip[2,1];
// // //         _near.Z = clip[3,2] + clip[2,2];
// // //         _near.W = clip[3,3] + clip[2,3];

// // //         // Extract the _near plane ( col3 - col2 )
// // //         _far.X = clip[3,0] - clip[2,0];
// // //         _far.Y = clip[3,1] - clip[2,1];
// // //         _far.Z = clip[3,2] - clip[2,2];
// // //         _far.W = clip[3,3] - clip[2,3];

// // //         if ( normalize)
// // //         {
// // //             _right.Normalize();// Plane.Normalize( frustum[0]);
// // //             _left.Normalize();// Plane.Normalize( frustum[1]);
// // //             _bottom.Normalize();// Plane.Normalize( frustum[2]);
// // //             _top.Normalize();// Plane.Normalize( frustum[3]);
// // //             _near.Normalize();// Plane.Normalize( frustum[4]);
// // //             _far.Normalize();// Plane.Normalize( frustum[5]);
// // //         }
// // //     }
// // //     /// <summary>
// // //     /// Calculates the eight frustum corners from an input matrix. In most cases this matrix will be the View-Projection-Matrix.
// // //     /// https://github.com/FUSEEProjectTeam/Fusee/blob/master/src/Math/Core/FrustumF.cs
// // //     /// </summary>
// // //     /// <param name="mat">The matrix from which to calculate the frustum corners.</param>
// // //     /// <returns></returns>
// // //     public static IEnumerable<Vector3> CalculateFrustumCorners(Matrix mat)
// // //     {
// // //         //1. Calculate the 8 corners of the view frustum in world space. This can be done by using the inverse view-projection matrix to transform the 8 corners of the NDC cube (which in OpenGL is [‒1, 1] along each axis).
// // //         //2. Transform the frustum corners to a space aligned with the shadow map axes.This would commonly be the directional light object's local space. 
// // //         //In fact, steps 1 and 2 can be done in one step by combining the inverse view-projection matrix of the camera with the inverse world matrix of the light.
// // //         var invViewProjection = float4x4.Invert(mat);

// // //         var frustumCorners = new float4[8];

// // //         frustumCorners[0] = invViewProjection * new float4(-1, -1, -1, 1); //nbl
// // //         frustumCorners[1] = invViewProjection * new float4(1, -1, -1, 1); //nbr
// // //         frustumCorners[2] = invViewProjection * new float4(-1, 1, -1, 1); //ntl
// // //         frustumCorners[3] = invViewProjection * new float4(1, 1, -1, 1); //ntr
// // //         frustumCorners[4] = invViewProjection * new float4(-1, -1, 1, 1); //fbl
// // //         frustumCorners[5] = invViewProjection * new float4(1, -1, 1, 1); //fbr
// // //         frustumCorners[6] = invViewProjection * new float4(-1, 1, 1, 1); //ftl
// // //         frustumCorners[7] = invViewProjection * new float4(1, 1, 1, 1); //ftr

// // //         for (int i = 0; i < frustumCorners.Length; i++)
// // //         {
// // //             var corner = frustumCorners[i];
// // //             corner /= corner.w; //world space frustum corners
// // //             frustumCorners[i] = corner;
// // //             yield return frustumCorners[i].xyz;
// // //         }

// // //     }

// // // #region Override      
// // //     /// <summary>
// // //     /// 
// // //     /// </summary>
// // //     /// <returns></returns>
// // //     public override string ToString() => "ClipPlanes["+_left.ToString()+";"+_right.ToString()+";"+_top.ToString()+";"+_bottom.ToString()+"]";
// // //     /// <summary>
// // //     /// 
// // //     /// </summary>
// // //     /// <returns></returns>
// // //     public override int GetHashCode() => (int)(_left.GetHashCode()+_right.GetHashCode()+_top.GetHashCode()+_bottom.GetHashCode())^32;
// // //     /// <summary>
// // //     /// 
// // //     /// </summary>
// // //     /// <param name="obj"></param>
// // //     /// <returns></returns>
// // //     public override bool Equals(object obj) => obj is ClipPlanes clip && this.Equals(clip) ;
// // //     /// <summary>
// // //     /// 
// // //     /// </summary>
// // //     /// <param name="clip"></param>
// // //     /// <returns></returns>
// // //     public bool Equals(ClipPlanes clip) =>this._left.Equals(clip._left) && this._right.Equals(clip._right) && this._top.Equals(clip._top) && this._bottom.Equals(clip._bottom);
// // //     /// <summary>
// // //     /// 
// // //     /// </summary>
// // //     /// <param name="left"></param>
// // //     /// <param name="right"></param>
// // //     /// <returns></returns>
// // //     public static bool operator ==(ClipPlanes left, ClipPlanes right)  => left.Equals(right);
// // //     /// <summary>
// // //     /// 
// // //     /// </summary>
// // //     /// <param name="left"></param>
// // //     /// <param name="right"></param>
// // //     /// <returns></returns>
// // //     public static bool operator !=(ClipPlanes left, ClipPlanes right)=> !(left.Equals(right));   
// // //     /// <summary>
// // //     /// Delete all 
// // //     /// </summary>
// // //     public void Dispose()
// // //     {
// // //         _left=default;
// // //         _right=default;
// // //         _top =default;
// // //         _bottom =default;
// // //         _near=default;
// // //         _far =default;
// // //         GC.SuppressFinalize(this);
// // //     }
// // // #endregion        
// // // }





// // //  // {
// // //         //     _cols[0].X =1.0f; _cols[0].Y = 0.0f; _cols[0].Z=0.0f; _cols[0].W =0.0f;
// // //         //     _cols[1].X =0.0f; _cols[1].Y = 1.0f; _cols[1].Z=0.0f; _cols[1].W =0.0f;
// // //         //     _cols[2].X =0.0f; _cols[2].Y = 0.0f; _cols[2].Z=1.0f; _cols[2].W =0.0f;
// // //         //     _cols[3].X =0.0f; _cols[3].Y = 0.0f; _cols[3].Z=0.0f; _cols[3].W =1.0f;
// // //         // }
// // //   // private float[] ToArrayRow() 
// // //         // {
// // //         //     float[] array = { 
// // //         //     _cols[0].X , _cols[1].X , _cols[2].X , _cols[3].X ,
// // //         //     _cols[0].Y , _cols[1].Y , _cols[2].Y , _cols[3].Y ,
// // //         //     _cols[0].Z , _cols[1].Z , _cols[2].Z , _cols[3].Z ,
// // //         //     _cols[0].W , _cols[1].W , _cols[2].W , _cols[3].W ,  };
// // //         //     return array; 
// // //         // }
// // // // ########### FOR UPROJECT METHOD #####################
// // // // //l * length(C-P)
// // // // https://stackoverflow.com/questions/42256657/glmunproject-doesnt-work-and-incorrect-ray-position-on-screen
// // // // void mRay(glm::mat4 view, glm::mat4 projection
// // // // // float x = win.X;
// // // // // float y = viewport[3] - 1.0f - win.Y;
// // // // // Vector4 tmp = new(  x , y ,-1.0f , 1.0f    );
// // // // // tmp *= 100.0f;           
// // // // {
// // // //     x = (posx / screenWidth - 0.5f) * 2.0f; 
// // // //     y = (posy / screenHeight - 0.5f) * -2.0f;

// // // //     glm::vec4 localNear{ x,  y, 0.0, 1.0 };
// // // //     glm::vec4 localFar{ x, y, 1.0, 1.0 };

// // // //     auto wsn = glm::inverse(projection * view) * localNear;
// // // //     worldSpaceNear = glm::vec3(wsn.x, wsn.y, wsn.z);

// // // //     auto wsf = glm::inverse(projection * view) * localFar;
// // // //     worldSpaceFar = glm::vec3(wsf.x, wsf.y, wsf.z);
// // // // }
// // // // int[] viewport = new int[4];
// // // // MCJ.Engine.External.Adapters.Graphic.GetViewportVector4InGPU( ref viewport);
// // // //             double[] modelview = new double[16];
// // // //             MCJ.Engine.External.Adapters.Graphic.GetModelViewMatrixInGPU(ref modelview);
// // // //             double[] projection = new double[16];
// // // //             MCJ.Engine.External.Adapters.Graphic.GetProjectionMatrixInGPU(ref projection);
// // // // //glReadPixels( x, int(winY), 1, 1, GL_DEPTH_COMPONENT, GL_FLOAT, &winZ );
// // // //             MCJ.Engine.External.Adapters.Graphic.GetDepthAxeZ( (int)win.X, (int)win.Y ) ;

// // // // public float this[int column ,int row]
// // //         // {
// // //         //     get =>  column switch {
// // //         //         0 => _cols[0][row],
// // //         //         1 => _cols[1][row],
// // //         //         2 => _cols[2][row],
// // //         //         3 => _cols[3][row],
// // //         //         _ => throw new ArgumentOutOfRangeException("index invalide for set this[row,column] ")
// // //         //     };
// // //         //     set => _= column switch {
// // //         //         0 => _cols[0][row] = value,
// // //         //         1 => _cols[1][row] = value,
// // //         //         2 => _cols[2][row] = value,
// // //         //         3 => _cols[3][row] = value,
// // //         //         _ => throw new ArgumentOutOfRangeException("index invalide for set this[row,column]")
// // //         //     };
// // //         // }
// // // // public  Vector4 this[int column]
// // //         // {
// // //         //     get => column switch
// // //         //     {
// // //         //         0 => _cols[0],
// // //         //         1 => _cols[1],
// // //         //         2 => _cols[2],
// // //         //         3 => _cols[3],
// // //         //         _ => throw new ArgumentOutOfRangeException("index invalide for get this[index]")
// // //         //     };
// // //         //     set => _ = column switch
// // //         //     {
// // //         //         0 => _cols[0] = value,
// // //         //         1 => _cols[1] = value,
// // //         //         2 => _cols[2] = value,
// // //         //         3 => _cols[3] = value,
// // //         //         _ => throw new ArgumentOutOfRangeException("index invalide for set this[index]")
// // //         //     };
// // //         // }
// // // //     [StructLayout(LayoutKind.Sequential, Pack = 4)]
// // // //     [SkipLocalsInit]
// // // //     public struct Matrix4
// // // //     {
        
// // // //         /// <summary>
// // // //         /// Value at row 1, column 1 of the matrix
// // // //         /// </summary>
// // // //         public float M11;

// // // //         /// <summary>
// // // //         /// Value at row 1, column 2 of the matrix
// // // //         /// </summary>
// // // //         public float M12;

// // // //         /// <summary>
// // // //         /// Value at row 1, column 3 of the matrix
// // // //         /// </summary>
// // // //         public float M13;

// // // //         /// <summary>
// // // //         /// Value at row 1, column 4 of the matrix
// // // //         /// </summary>
// // // //         public float M14;

// // // //         /// <summary>
// // // //         /// Value at row 2, column 1 of the matrix
// // // //         /// </summary>
// // // //         public float M21;

// // // //         /// <summary>
// // // //         /// Value at row 2, column 2 of the matrix
// // // //         /// </summary>
// // // //         public float M22;

// // // //         /// <summary>
// // // //         /// Value at row 2, column 3 of the matrix
// // // //         /// </summary>
// // // //         public float M23;

// // // //         /// <summary>
// // // //         /// Value at row 2, column 4 of the matrix
// // // //         /// </summary>
// // // //         public float M24;

// // // //         /// <summary>
// // // //         /// Value at row 3, column 1 of the matrix
// // // //         /// </summary>
// // // //         public float M31;

// // // //         /// <summary>
// // // //         /// Value at row 3, column 2 of the matrix
// // // //         /// </summary>
// // // //         public float M32;

// // // //         /// <summary>
// // // //         /// Value at row 3, column 3 of the matrix
// // // //         /// </summary>
// // // //         public float M33;

// // // //         /// <summary>
// // // //         /// Value at row 3, column 4 of the matrix
// // // //         /// </summary>
// // // //         public float M34;

// // // //         /// <summary>
// // // //         /// Value at row 4, column 1 of the matrix
// // // //         /// </summary>
// // // //         public float M41;

// // // //         /// <summary>
// // // //         /// Value at row 4, column 2 of the matrix
// // // //         /// </summary>
// // // //         public float M42;

// // // //         /// <summary>
// // // //         /// Value at row 4, column 3 of the matrix
// // // //         /// </summary>
// // // //         public float M43;

// // // //         /// <summary>
// // // //         /// Value at row 4, column 4 of the matrix
// // // //         /// </summary>
// // // //         public float M44;

// // // //         private static readonly Matrix4 identity = Alloc(
// // // //             1.0f, 0.0f, 0.0f, 0.0f, 
// // // //             0.0f, 1.0f, 0.0f, 0.0f, 
// // // //             0.0f, 0.0f, 1.0f, 0.0f, 
// // // //             0.0f, 0.0f, 0.0f, 1.0f);
// // // //         /// <summary>
// // // //         /// retourne une matrice dite a un 
// // // //         /// </summary>
// // // //         public static ref readonly Matrix4 Identite => ref identity;

// // // //         private static readonly int SizeOf = Marshal.SizeOf<Matrix4>();

// // // //         /// <summary>
// // // //         /// 
// // // //         /// </summary>
// // // //         public void Identity() => this = Matrix4.Identite;
        
// // // //         /// <summary>
// // // //         /// 
// // // //         /// </summary>
// // // //         /// <param name="m11"></param>
// // // //         /// <param name="m12"></param>
// // // //         /// <param name="m13"></param>
// // // //         /// <param name="m14"></param>
// // // //         /// <param name="m21"></param>
// // // //         /// <param name="m22"></param>
// // // //         /// <param name="m23"></param>
// // // //         /// <param name="m24"></param>
// // // //         /// <param name="m31"></param>
// // // //         /// <param name="m32"></param>
// // // //         /// <param name="m33"></param>
// // // //         /// <param name="m34"></param>
// // // //         /// <param name="m41"></param>
// // // //         /// <param name="m42"></param>
// // // //         /// <param name="m43"></param>
// // // //         /// <param name="m44"></param>
// // // //         public Matrix4( float m11=1.0f, float m12=0.0f, float m13=0.0f, float m14=0.0f, 
// // // //                         float m21=0.0f, float m22=1.0f, float m23=0.0f, float m24=0.0f, 
// // // //                         float m31=0.0f, float m32=0.0f, float m33=1.0f, float m34=0.0f, 
// // // //                         float m41=0.0f, float m42=0.0f, float m43=0.0f, float m44=1.0f)
// // // //             => (M11,M12,M13,M14,M21,M22,M23,M24,M31,M32,M33,M34,M41,M42,M43,M44)
// // // //                 =(m11,m12,m13,m14,m21,m22,m23,m24,m31,m32,m33,m34,m41,m42,m43,m44);
// // // //         /// <summary>
// // // //         /// 
// // // //         /// </summary>
// // // //         /// <param name="row1"></param>
// // // //         /// <param name="row2"></param>
// // // //         /// <param name="row3"></param>
// // // //         /// <param name="row4"></param>
// // // //         public Matrix4 ( Vector4 row1, Vector4 row2, Vector4 row3 , Vector4 row4)
// // // //             =>(M11,M12,M13,M14,M21,M22,M23,M24,M31,M32,M33,M34,M41,M42,M43,M44)
// // // //                 =(row1.X,row1.Y,row1.Z,row1.W, row2.X,row2.Y,row2.Z,row2.W, row3.X,row3.Y,row3.Z,row3.W, row4.X,row4.Y,row4.Z,row4.W);
// // // //         /// <summary>
// // // //         /// 
// // // //         /// </summary>
// // // //         /// <param name="scalar"></param>
// // // //         public Matrix4 ( float scalar)
// // // //             => (M11,M12,M13,M14,M21,M22,M23,M24,M31,M32,M33,M34,M41,M42,M43,M44)
// // // //                 =(scalar,scalar,scalar,scalar,scalar,scalar,scalar,scalar,scalar,scalar,scalar,scalar,scalar,scalar,scalar,scalar);
// // // //         /// <summary>
// // // //         /// 
// // // //         /// </summary>
// // // //         /// <param name="mat"></param>
// // // //         public Matrix4 ( Matrix4 mat )
// // // //             => (M11,M12,M13,M14,M21,M22,M23,M24,M31,M32,M33,M34,M41,M42,M43,M44)
// // // //                 = (mat.M11,mat.M12,mat.M13,mat.M14,mat.M21,mat.M22,mat.M23,mat.M24,mat.M31,mat.M32,mat.M33,mat.M34,mat.M41,mat.M42,mat.M43,mat.M44);
// // // //         /// <summary>
// // // //         /// 
// // // //         /// </summary>
// // // //         /// <value></value>
// // // //         public float this[int index]
// // // //         {
// // // //             get => index switch
// // // //             {
// // // //                 0 => M11, 1 => M12, 2 => M13, 3 => M14,
// // // //                 4 => M21, 5 => M22, 6 => M23, 7 => M24,
// // // //                 8 => M31, 9 => M32, 10 => M33, 11 => M34,
// // // //                 12 => M41, 13 => M42, 14 => M43, 15 => M44,
// // // //                 _ => throw new ArgumentOutOfRangeException("index invalide")
// // // //             };
// // // //             set => _ = index switch
// // // //             {
// // // //                 0 => M11 = value, 1 => M12 = value, 2 => M13 = value,  3 => M14 = value,
// // // //                 4 => M11 = value, 5 => M12 = value, 6 => M13 = value,  7 => M14 = value,
// // // //                 8 => M11 = value, 9 => M12 = value, 10 => M13 = value,  11 => M14 = value,
// // // //                 12 => M11 = value, 13 => M12 = value, 14 => M13 = value,  15 => M14 = value,
// // // //                 _ => throw new ArgumentOutOfRangeException("index invalide")
// // // //             };
// // // //         }
// // // //         /// <summary>
// // // //         ///  
// // // //         /// </summary>
// // // //         /// <value></value>
// // // //         public float this[int row ,int column]
// // // //         {
// // // //             get => this[(row * 4) + column];
// // // //             set => this[(row * 4) + column] = value;
// // // //         }
// // // //         /// <summary>
// // // //         /// matrix[2] de - M31 à M34 
// // // //         /// </summary>
// // // //         /// <value></value>
// // // //         public Vector4 Forward
// // // //         {
// // // //             get =>  Vector4.HeapAlloc(-this.M31, -this.M32, -this.M33, -this.M34);
// // // //             set => (M31,M32,M33,M34) = (-value.X,-value.Y,-value.Z,-value.W);
// // // //         }

// // // //         /// <summary>
// // // //         /// matrix[2] de  M31 à M34 
// // // //         /// </summary>
// // // //         /// <value></value>
// // // //         public Vector4 Backward
// // // //         {
// // // //             get =>  Vector4.HeapAlloc(this.M31, this.M32, this.M33, this.M34);
// // // //             set => (M31,M32,M33,M34) = (value.X,value.Y,value.Z,value.W);
// // // //         }

// // // //         /// <summary>
// // // //         /// matrix[3] de M41 à M44 
// // // //         /// </summary>
// // // //         /// <value></value>
// // // //         public Vector4 Translation
// // // //         {
// // // //             get =>  Vector4.HeapAlloc(this.M41, this.M42, this.M43, this.M44);
// // // //             set => (M41,M42,M43,M44) = (value.X,value.Y,value.Z,value.W);
// // // //         }
// // // //         /// <summary>
// // // //         /// matrix[1] de M21 à M24 
// // // //         /// </summary>
// // // //         /// <value></value>
// // // //         public Vector4 Up
// // // //         {
// // // //             get =>  Vector4.HeapAlloc(this.M21, this.M22, this.M23, this.M24);
// // // //             set => (M21,M22,M23,M24) = (value.X,value.Y,value.Z,value.W);
// // // //         }
// // // //         /// <summary>
// // // //         /// matrix[0] de -  M11 à M14 
// // // //         /// </summary>
// // // //         /// <value></value>
// // // //         public Vector4 Left
// // // //         {
// // // //             get =>  Vector4.HeapAlloc(-this.M11, -this.M12, -this.M13, -this.M14);
// // // //             set => (M11,M12,M13,M14) = (-value.X,-value.Y,-value.Z,-value.W);
// // // //         }
// // // //         /// <summary>
// // // //         /// matrix[0] de M11 à M14
// // // //         /// </summary>
// // // //         /// <value></value>
// // // //         public Vector4 Right
// // // //         {
// // // //             get =>  Vector4.HeapAlloc(this.M11, this.M12, this.M13, this.M14);
// // // //             set => (M11,M12,M13,M14) = (value.X,value.Y,value.Z,value.W);
// // // //         }

// // // //         /// <summary>
// // // //         ///  is not a number
// // // //         /// </summary>
// // // //         /// <returns></returns>
// // // //         public bool IsNaN =>float.IsNaN(M11) || float.IsNaN(M12) || float.IsNaN(M13) || float.IsNaN(M14) ||
// // // //                     float.IsNaN(M21) || float.IsNaN(M22) || float.IsNaN(M23) || float.IsNaN(M24) ||
// // // //                     float.IsNaN(M31) || float.IsNaN(M32) || float.IsNaN(M33) || float.IsNaN(M34) ||
// // // //                     float.IsNaN(M41) || float.IsNaN(M42) || float.IsNaN(M43) || float.IsNaN(M44);

// // // //         /// <summary>
// // // //         /// 
// // // //         /// </summary>
// // // //         /// <returns></returns>
// // // //         public override string ToString()  => "Matrix4 ["+M11.ToString()+";"+M12.ToString()+";"+M13.ToString()+";"+M14.ToString()+"]";
// // // //         /// <summary>
// // // //         /// 
// // // //         /// </summary>
// // // //         /// <returns></returns>
// // // //         public override int GetHashCode() => HashCode.Get(this.M11) + HashCode.Get(this.M12)+ HashCode.Get(this.M13)+ HashCode.Get(this.M14);
// // // //         /// <summary>
// // // //         /// 
// // // //         /// </summary>
// // // //         /// <param name="obj"></param>
// // // //         /// <returns></returns>
// // // //         public override bool Equals(object obj) => obj is Matrix4 mat && this.Equals(mat)  ;
// // // //         /// <summary>
// // // //         /// 
// // // //         /// </summary>
// // // //         /// <param name="other"></param>
// // // //         /// <returns></returns>
// // // //         public bool Equals(Matrix4 other) 
// // // //             => (Maths.Abs(M11 - other.M11) <=  Maths.ZeroTolerance) && (Maths.Abs(M12 - other.M13) <= Maths.ZeroTolerance) && (Maths.Abs(M13 - other.M13) <= Maths.ZeroTolerance) && (Maths.Abs(M14 - other.M14) <= Maths.ZeroTolerance);
// // // //         /// <summary>
// // // //         /// 
// // // //         /// </summary>
// // // //         /// <param name="left"></param>
// // // //         /// <param name="right"></param>
// // // //         /// <returns></returns>
// // // //         public static bool operator ==(Matrix4 left, Matrix4 right) => left.Equals(right);
// // // //         /// <summary>
// // // //         /// 
// // // //         /// </summary>
// // // //         /// <param name="left"></param>
// // // //         /// <param name="right"></param>
// // // //         /// <returns></returns>
// // // //         public static bool operator !=(Matrix4 left, Matrix4 right) => !(left.Equals(right));

// // // //         /// <summary>
// // // //         /// Allocation sur la pile Stack
// // // //         /// </summary>
// // // //         public static Matrix4 Alloc( float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31,
// // // //                       float m32, float m33, float m34, float m41, float m42, float m43, float m44)
// // // //         {
// // // //             Matrix4 mat = default;
// // // //             mat.M11 = m11 ; mat.M12 =m12 ; mat.M13 =m13 ; mat.M14 = m14;
// // // //             mat.M21 = m21 ; mat.M22 =m22 ; mat.M23 =m23 ; mat.M24 = m24;
// // // //             mat.M31 = m31 ; mat.M32 =m32 ; mat.M33 =m33 ; mat.M34 = m34;
// // // //             mat.M41 = m41 ; mat.M42 =m42 ; mat.M43 =m43 ; mat.M44 = m44;
// // // //             return mat;
// // // //         }
// // // //         /// <summary>
// // // //         /// Allocation sur le tas  Heap ( usage de new )
// // // //         /// </summary>
// // // //         /// <returns></returns>
// // // //         public static Matrix4 AllocNew( float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31,
// // // //                       float m32, float m33, float m34, float m41, float m42, float m43, float m44)
// // // //             =>new Matrix4(m11,m12,m13,m14,
// // // //                                  m21,m22,m23,m24,
// // // //                                  m31,m32,m33,m34,
// // // //                                  m41,m42,m43,m44);
// // // //         /// <summary>
// // // //         /// Return les valeurs de la matrives sous forme de tableau utile pour les uniform
// // // //         /// </summary>
// // // //         /// <value></value>
// // // //         public float[] ToArray
// // // //             => new float[]{ M11,M12,M13,M14,M21,M22,M23,M24,M31,M32,M33,M34,M41,M42,M43,M44};

// // // //         /// <summary>
// // // //         /// 
// // // //         /// </summary>
// // // //         /// <returns></returns>
// // // //         public float[] ToArrayStaticalloc()
// // // //         {
// // // //             float[] r ={M11,M12,M13,M14,M21,M22,M23,M24,M31,M32,M33,M34,M41,M42,M43,M44}; 
// // // //             return r;
// // // //         }
        
// // // // #region OPERATION

// // // // #endregion
// // // //         /// <summary>
// // // //         /// Create a matrix world with all element position, target,Up
// // // //         /// </summary>
// // // //         /// <param name="result"></param>
// // // //         /// <param name="position"></param>
// // // //         /// <param name="forward"></param>
// // // //         /// <param name="up"></param>
// // // //         public static void CreateWorld( ref Matrix4 result,ref Vector4 position,ref  Vector4 forward, ref Vector4 up)
// // // //         {
// // // //             Vector4 z = (forward);
// // // //             z.Normalize();
// // // //             Vector4 x = Vector4.Cross(ref forward,ref up);
// // // //             Vector4 y = Vector4.Cross(ref x,ref forward);
// // // //             x.Normalize();
// // // //             y.Normalize();

// // // //             result = Matrix4.Identite;
// // // //             result.Right = x;
// // // //             result.Up=y;
// // // //             result.Translation = position;
// // // //             result.Forward= z;
// // // //             // result.Translation.W = 1.0f;
// // // //         }

// // // //          /// <summary>
// // // //         /// Rotation angle X en radian
// // // //         /// </summary>
// // // //         /// <param name="result"></param>
// // // //         /// <param name="radian">in radians </param>
// // // //         public static void RotationX(ref Matrix4 result , in float radian)
// // // //         {
// // // //             float cosx = Maths.Cos(  radian  );
// // // //             float sinx = Maths.Sin( radian );
           
// // // //             result.M22 = cosx; //Up.X
// // // //             result.M23 = sinx; //Up.Z
// // // //             result.M32 = -sinx; //forward
// // // //             result.M33 = cosx;
// // // //         }
// // // //         /// <summary>
// // // //         /// Rotation angle Y en radian
// // // //         /// </summary>
// // // //         /// <param name="result"></param>
// // // //         /// <param name="radian"></param>
// // // //         public static void RotationY(ref Matrix4 result,in float radian )
// // // //         {
// // // //             float cosy = Maths.Cos(radian);
// // // //             float siny = Maths.Sin(radian);

// // // //             result.M11 = cosy;
// // // //             result.M13= -siny;
// // // //             result.M31 = siny;
// // // //             result.M33 = cosy;        
// // // //         }
// // // //         /// <summary>
// // // //         /// Rotation angle Z en radian ( faire identity avant )
// // // //         /// </summary>
// // // //         /// <param name="result"></param>
// // // //         /// <param name="radian"></param>
// // // //         public static void RotationZ( ref Matrix4 result, float radian)
// // // //         {
// // // // 			var val1 = Maths.Cos(radian);
// // // // 			var val2 = Maths.Sin(radian);

// // // //             result.M11 = val1;
// // // //             result.M12 = val2;
// // // //             result.M21 = -val2;
// // // //             result.M23 = val1;
// // // //         }

// // // //         /// <summary>
// // // //         /// Rotation angle Z en radian ( faire identity avant )
// // // //         /// </summary>
// // // //         /// <param name="result"></param>
// // // //         /// <param name="vec3"></param>
// // // //         public static void Translate( ref Matrix4 result, Vector3 vec3)
// // // //         {
// // // //             result.M41 += vec3.X;
// // // //             result.M42 += vec3.Y;
// // // //             result.M41 += vec3.Z;
// // // //             result.M43 += 1.0f;
// // // //         }
// // // //         /// <summary>
// // // //         /// 
// // // //         /// </summary>
// // // //         /// <param name="m1"></param>
// // // //         /// <param name="m2"></param>
// // // //         /// <returns></returns>
// // // //         public static Matrix4 operator * ( Matrix4 m1 , Matrix4 m2 )
// // // //             => new(
// // // //                 (m1.Right * m2.Right.X) + (m1.Up * m2.Right.Y) + (m1.Forward * m2.Right.Z) + (m1.Translation * m2.Right.W),
// // // //                 (m1.Right * m2.Up.X) + (m1.Up * m2.Up.Y) + (m1.Forward * m2.Up.Z) + (m1.Translation * m2.Up.W),
// // // //                 (m1.Right * m2.Forward.X) + (m1.Up * m2.Forward.Y) + (m1.Forward * m2.Forward.Z) + (m1.Translation * m2.Forward.W),
// // // //                 (m1.Right * m2.Translation.X) + (m1.Up * m2.Translation.Y) + (m1.Forward * m2.Translation.Z) + (m1.Translation * m2.Translation.W));
// // // //         /// <summary>
// // // //         /// 
// // // //         /// </summary>
// // // //         /// <param name="result"></param>
// // // //         /// <param name="m1"></param>
// // // //         /// <param name="m2"></param>
// // // //         public static void MultiplyMat4byMat4(ref Matrix4 result ,ref Matrix4 m1 , ref Matrix4 m2)
// // // //         {
// // // //             result.Right = (m1.Right * m2.Right.X) + (m1.Up * m2.Right.Y) + (m1.Forward * m2.Right.Z) + (m1.Translation * m2.Right.W);
// // // //             result.Up = (m1.Right * m2.Up.X) + (m1.Up * m2.Up.Y) + (m1.Forward * m2.Up.Z) + (m1.Translation * m2.Up.W);
// // // //             result.Forward = (m1.Right * m2.Forward.X) + (m1.Up * m2.Forward.Y) + (m1.Forward * m2.Forward.Z) + (m1.Translation * m2.Forward.W);
// // // //             result.Translation = (m1.Right * m2.Translation.X) + (m1.Up * m2.Translation.Y) + (m1.Forward * m2.Translation.Z) + (m1.Translation * m2.Translation.W);
// // // //         }
// // // //     }
// // // // {
// // // //     using System;
// // // //     using  System.Runtime.InteropServices;

// // // //     /// <summary>
// // // //     /// Special unique matrix for all engine ( not used matrix 3x3 )
// // // //     /// </summary>
// // // //     [StructLayout(LayoutKind.Sequential, Pack = 4 )]
// // // //     public struct Matrix :IEquatable<Matrix>
// // // //     {
// // // //         /// <summary>
// // // //         /// Called Right M11 à M14 for left is negative (ROW1)
// // // //         /// </summary>
// // // //         public Vector4 Right;
// // // //         /// <summary>
// // // //         /// Called Up M21 à M24 or Down for negative (ROW2)
// // // //         /// </summary>
// // // //         public Vector4 Up;
// // // //         /// <summary>
// // // //         /// Called Forward (negative) M31 à M34 ROW3
// // // //         /// </summary>
// // // //         public Vector4 Forward;
// // // //         /// <summary>
// // // //         /// Called Translation M41 à M44 (ROW4)
// // // //         /// </summary>
// // // //         public Vector4 Translation;

// // // //         // public Vector4 Translation  => Translation;
// // // //         // public Vector4 Forward => Forward;
// // // //         // public Vector4 Up => Up;
// // // //         // public Vector4 Right => Right;

// // // //         public Matrix(Matrix m)
// // // //             =>( Right,Up,Forward,Translation) = (m.Right,m.Up,m.Forward,m.Translation);

// // // //         public Matrix(float f)
// // // //             => ( Right,Up,Forward,Translation) = (new Vector4(f,0.0f,0.0f,0.0f),new Vector4(0.0f,f,0.0f,0.0f),new Vector4(0.0f,0.0f,f,0.0f),new Vector4(0.0f,0.0f,0.0f,f));

// // // //         public Matrix(Vector4 right, Vector4 up, Vector4 forward, Vector4 translation)
// // // //             =>( Right,Up,Forward,Translation) = (right,up,forward,translation);

// // // //         public Matrix(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31,
// // // //                       float m32, float m33, float m34, float m41, float m42, float m43, float m44)
// // // //             =>( Right,Up,Forward,Translation)= (new Vector4(m11,m12,m13,m14),new Vector4(m21,m22,m23,m24),new Vector4(m31,m32,m33,m34),new Vector4(m41,m42,m43,m44));

// // // //         public void Identity()
// // // //         {
// // // //             this.Right.X =1.0f;this.Right.Y =0.0f;this.Right.Z =0.0f;this.Right.W =0.0f;
// // // //             this.Up.X =0.0f;this.Up.Y =1.0f;this.Up.Z =0.0f;this.Up.W =0.0f;
// // // //             this.Forward.X =0.0f;this.Forward.Y =0.0f;this.Forward.Z =1.0f;this.Forward.W =0.0f;
// // // //             this.Translation.X =0.0f;this.Translation.Y =0.0f;this.Translation.Z =0.0f;this.Translation.W =1.0f;
// // // //         }

// // // //         public void Zero()
// // // //         {
// // // //             this.Right.X =0.0f;this.Right.Y =0.0f;this.Right.Z =0.0f;this.Right.W =0.0f;
// // // //             this.Up.X =0.0f;this.Up.Y =0.0f;this.Up.Z =0.0f;this.Up.W =0.0f;
// // // //             this.Forward.X =0.0f;this.Forward.Y =0.0f;this.Forward.Z =0.0f;this.Forward.W =0.0f;
// // // //             this.Translation.X =0.0f;this.Translation.Y =0.0f;this.Translation.Z =0.0f;this.Translation.W =0.0f;
// // // //         }

// // // // #region OVERRIDE OPERATOR
// // // //         public static Matrix operator * ( Matrix m , float scalar)
// // // //         {
// // // //             m.Right *= scalar;
// // // //             m.Up *= scalar;
// // // //             m.Forward *= scalar;
// // // //             m.Translation *= scalar;
// // // //             return m;
// // // //         }

// // // //         public static Matrix operator * ( Matrix m1 , Matrix m2 )
// // // //             => new(
// // // //                 (m1.Right * m2.Right.X) + (m1.Up * m2.Right.Y) + (m1.Forward * m2.Right.Z) + (m1.Translation * m2.Right.W),
// // // //                 (m1.Right * m2.Up.X) + (m1.Up * m2.Up.Y) + (m1.Forward * m2.Up.Z) + (m1.Translation * m2.Up.W),
// // // //                 (m1.Right * m2.Forward.X) + (m1.Up * m2.Forward.Y) + (m1.Forward * m2.Forward.Z) + (m1.Translation * m2.Forward.W),
// // // //                 (m1.Right * m2.Translation.X) + (m1.Up * m2.Translation.Y) + (m1.Forward * m2.Translation.Z) + (m1.Translation * m2.Translation.W));
// // // //         public static void MultiplyMat4byMat4(ref Matrix result ,ref Matrix m1 , ref Matrix m2)
// // // //         {
// // // //             result.Right = (m1.Right * m2.Right.X) + (m1.Up * m2.Right.Y) + (m1.Forward * m2.Right.Z) + (m1.Translation * m2.Right.W);
// // // //             result.Up = (m1.Right * m2.Up.X) + (m1.Up * m2.Up.Y) + (m1.Forward * m2.Up.Z) + (m1.Translation * m2.Up.W);
// // // //             result.Forward = (m1.Right * m2.Forward.X) + (m1.Up * m2.Forward.Y) + (m1.Forward * m2.Forward.Z) + (m1.Translation * m2.Forward.W);
// // // //             result.Translation = (m1.Right * m2.Translation.X) + (m1.Up * m2.Translation.Y) + (m1.Forward * m2.Translation.Z) + (m1.Translation * m2.Translation.W);
// // // //         }

// // // //         public static Vector4  operator * ( Matrix mat4 ,  Vector4 vec4  )
// // // //             =>new(
// // // //                 (vec4.X * mat4.Right.X) + (vec4.Y * mat4.Right.Y) + (vec4.Z * mat4.Right.Z) + (vec4.W * mat4.Right.W),
// // // //                 (vec4.X * mat4.Up.X) + (vec4.Y * mat4.Up.Y) + (vec4.Z * mat4.Up.Z) + (vec4.W * mat4.Up.W),
// // // //                 (vec4.X * mat4.Forward.X) + (vec4.Y * mat4.Forward.Y) + (vec4.Z * mat4.Forward.Z) + (vec4.W * mat4.Forward.W),
// // // //                 (vec4.X * mat4.Translation.X) + (vec4.Y * mat4.Translation.Y) + (vec4.Z * mat4.Translation.Z) + (vec4.W * mat4.Translation.W));

// // // //         public static void MultiplyMat4ByVec4(ref Vector4 result,ref Matrix mat4 , ref Vector4 vec4 )
// // // //         {
// // // //             result.X = (vec4.X * mat4.Right.X) + (vec4.Y * mat4.Right.Y) + (vec4.Z * mat4.Right.Z) + (vec4.W * mat4.Right.W);
// // // //             result.Y = (vec4.X * mat4.Up.X) + (vec4.Y * mat4.Up.Y) + (vec4.Z * mat4.Up.Z) + (vec4.W * mat4.Up.W);
// // // //             result.Z = (vec4.X * mat4.Forward.X) + (vec4.Y * mat4.Forward.Y) + (vec4.Z * mat4.Forward.Z) + (vec4.W * mat4.Forward.W);
// // // //             result.W = (vec4.X * mat4.Translation.X) + (vec4.Y * mat4.Translation.Y) + (vec4.Z * mat4.Translation.Z) + (vec4.W * mat4.Translation.W);
// // // //         }

// // // //         public static Vector4  operator * (Vector4 vec4, Matrix mat4)
// // // //             =>new(
// // // //                 (vec4.X * mat4.Right.X) + (vec4.Y * mat4.Up.X) + ( vec4.Z * mat4.Forward.X) + ( vec4.W * mat4.Translation.X),
// // // //                 (vec4.X * mat4.Right.Y) + (vec4.Y * mat4.Up.Y) + ( vec4.Z * mat4.Forward.Y) + ( vec4.W * mat4.Translation.Y),
// // // //                 (vec4.X * mat4.Right.Z) + (vec4.Y * mat4.Up.Z) + ( vec4.Z * mat4.Forward.Z) + ( vec4.W * mat4.Translation.Z),
// // // //                 (vec4.X * mat4.Right.W) + (vec4.Y * mat4.Up.W) + ( vec4.Z * mat4.Forward.W) + ( vec4.W * mat4.Translation.W));

// // // //         /// <summary>
// // // //         /// Aussi appeler TransForm( Attention ) see in VEctor4 Transform
// // // //         /// </summary>
// // // //         /// <param name="result"></param>
// // // //         /// <param name="vec4"></param>
// // // //         /// <param name="mat4"></param>
// // // //         public static void MultiplyVec4ByMat4(ref Vector4 result,ref Vector4 vec4,ref Matrix mat4  )
// // // //         {
// // // //             result.X = (vec4.X * mat4.Right.X) + (vec4.Y * mat4.Up.X) + ( vec4.Z * mat4.Forward.X) + ( vec4.W * mat4.Translation.X);
// // // //             result.Y = (vec4.X * mat4.Right.Y) + (vec4.Y * mat4.Up.Y) + ( vec4.Z * mat4.Forward.Y) + ( vec4.W * mat4.Translation.Y);
// // // //             result.Z = (vec4.X * mat4.Right.Z) + (vec4.Y * mat4.Up.Z) + ( vec4.Z * mat4.Forward.Z) + ( vec4.W * mat4.Translation.Z);
// // // //             result.W = (vec4.X * mat4.Right.W) + (vec4.Y * mat4.Up.W) + ( vec4.Z * mat4.Forward.W) + ( vec4.W * mat4.Translation.W);
// // // //         }
// // // // #endregion
// // // //         public readonly float[] ToArray
// // // //             => new float[] { Right.X,Right.Y,Right.Z,Right.W,Up.X,Up.Y,Up.Z,Up.W,Forward.X,Forward.Y,Forward.Z,Forward.W,Translation.X,Translation.Y,Translation.Z,Translation.W    };

// // // //         public override string ToString()
// // // //             =>"Mat4 {\n1["+this.Right+"]\n2["+this.Up+"]\n3["+this.Forward+"]\n4["+this.Translation+"]}";

// // // //         public override int GetHashCode()  => (int)(Right.X+Right.Y+Right.Z+Right.W+Up.X+Up.Y+Up.Z+Up.W+Forward.X+Forward.Y+Forward.Z+Forward.W+Translation.X+Translation.Y+Translation.Z+Translation.W );

// // // //         public override bool Equals(object obj)
// // // //             => obj is Matrix mat && mat.Right == this.Right && mat.Up == this.Up && mat.Forward == this.Forward && mat.Translation == this.Translation ;

// // // //         public static bool operator ==(Matrix left, Matrix right)
// // // //             => left.Equals(right);

// // // //         public static bool operator !=(Matrix left, Matrix right)
// // // //             => !(left == right);

// // // //         public static void Translate(ref Matrix mat ,ref Vector4 vec)
// // // //             => mat.Translation += (mat.Right * vec.X) + (mat.Up * vec.Y) + (mat.Forward* vec.Z  ) ;

// // // //         public static void Scale(ref Matrix mat,ref Vector4 v)
// // // //         {
// // // //             Vector4.MultiplyVector4ByScalar(ref mat.Right, ref mat.Right, v.X);
// // // //             Vector4.MultiplyVector4ByScalar(ref mat.Up, ref mat.Up, v.Y);
// // // //             Vector4.MultiplyVector4ByScalar(ref mat.Forward, ref mat.Forward, v.Z);
// // // //             // mat.Right *= v.X;
// // // //             // mat.Up *= v.Y;
// // // //             // mat.Forward *= v.Z;
// // // //         }

// // // //         /// <summary>
// // // //         /// Rotate around arbitrary axis
// // // //         /// </summary>
// // // //         /// <param name="m"></param>
// // // //         /// <param name="angle">in radians 0 to 2PI </param>
// // // //         /// <param name="axis"></param>
// // // //         public static void Rotate(ref  Matrix result,in float angle,ref Vector4 axis)
// // // //         {
// // // //             // var c = Math.Cos(  angle  );
// // // //             // var s = Math.Sin(  angle  );

// // // //             // axis.Normalize();
// // // //             // Vector4 temp = new ((1 - c) * axis);

// // // //             // Matrix Rotate = new (1.0f);
// // // //             // Rotate.Right.X = c + (temp[0] * axis[0]);
// // // //             // Rotate.Right.Y = (temp[0] * axis[1]) + (s * axis[2]);
// // // //             // Rotate.Right.Z = (temp[0] * axis[2]) - (s * axis[1]);

// // // //             // Rotate.Up.X = (temp[1] * axis[0]) - (s * axis[2]);
// // // //             // Rotate.Up.Y = c + (temp[1] * axis[1]);
// // // //             // Rotate.Up.Z = (temp[1] * axis[2]) + (s * axis[0]);

// // // //             // Rotate.Forward.X = (temp[2] * axis[0]) + (s * axis[1]);
// // // //             // Rotate.Forward.Y = (temp[2] * axis[1]) - (s * axis[0]);
// // // //             // Rotate.Forward.Z = c + (temp[2] * axis[2]);
// // // //             // m.Right = new ((m.Right * Rotate.Right[0]) + (m.Up * Rotate.Right[1]) + (m.Forward * Rotate.Right[2])); // row 1
// // // //             // m.Up = new ((m.Right * Rotate.Up[0]) + (m.Up * Rotate.Up[1]) + (m.Forward * Rotate.Up[2])); // row 2
// // // //             // m.Forward = new ((m.Right * Rotate.Forward[0]) + (m.Up * Rotate.Forward[1]) + (m.Forward * Rotate.Forward[2])); // row 3
// // // //             float x = axis.X;
// // // // 		    float y = axis.Y;
// // // // 		    float z = axis.Z;
// // // // 		    float num2 = Math.Sin(angle);
// // // // 		    float num = Math.Cos(angle);
// // // // 		    float num11 = x * x;
// // // // 		    float num10 = y * y;
// // // // 		    float num9 = z * z;
// // // // 		    float num8 = x * y;
// // // // 		    float num7 = x * z;
// // // // 		    float num6 = y * z;
// // // // 		    result.Right.X = num11 + (num * (1f - num11));
// // // // 		    result.Right.Y = (num8 - (num * num8)) + (num2 * z);
// // // // 		    result.Right.Z = num7 - (num * num7) - (num2 * y);
// // // // 		    result.Right.W = 0;
// // // // 		    result.Up.X = num8 - (num * num8) - (num2 * z);
// // // // 		    result.Up.Y = num10 + (num * (1f - num10));
// // // // 		    result.Up.Z = (num6 - (num * num6)) + (num2 * x);
// // // // 		    result.Up.W = 0;
// // // // 		    result.Forward.X = (num7 - (num * num7)) + (num2 * y);
// // // // 		    result.Forward.Y = num6 - (num * num6) - (num2 * x);
// // // // 		    result.Forward.Z = num9 + (num * (1f - num9));
// // // //         }

// // // //         /// <summary>
// // // //         /// Rotation angle X
// // // //         /// </summary>
// // // //         /// <param name="result"></param>
// // // //         /// <param name="angle">in radians </param>
// // // //         public static void RotationX(ref Matrix result , in float angle)
// // // //         {
// // // //             float cosx = Math.Cos(  angle  );
// // // //             float sinx = Math.Sin(angle );

// // // //             result.Identity();
// // // //             result.Up.Y = cosx;
// // // //             result.Up.Z = sinx;
// // // //             result.Forward.Y = -sinx;
// // // //             result.Forward.Z = cosx;
// // // //         }

// // // //         public static void RotationY(ref Matrix result,in float angle , bool identity = true)
// // // //         {
// // // //             float cosy = Math.Cos(angle);
// // // //             float siny = Math.Sin(angle);

// // // //             if ( identity)
// // // //                 result.Identity();

// // // //             result.Right.X = cosy;
// // // //             result.Right.Z= -siny;
// // // //             result.Forward.X = siny;
// // // //             result.Forward.Z = cosy;
// // // //         }

// // // //         public static void RotationZ( ref Matrix result, float angle)
// // // //         {
// // // // 			var val1 = Math.Cos(angle);
// // // // 			var val2 = Math.Sin(angle);

// // // //             result.Identity();
// // // //             result.Right.X = val1;
// // // //             result.Right.Y = val2;
// // // //             result.Up.X = -val2;
// // // //             result.Up.Y = val1;
// // // //         }

// // // //         /// <summary>
// // // //         /// Code from GLM
// // // //         /// </summary>
// // // //         public void ComputeInverse()
// // // //         {
// // // //             float Coef00 = (Forward[2] * Translation[3]) - (Translation[2] * Forward[3]);
// // // // 			float Coef02 = (Up[2] * Translation[3]) - (Translation[2] * Up[3]);
// // // // 			float Coef03 = (Up[2] * Forward[3]) - (Forward[2] * Up[3]);

// // // // 			float Coef04 = (Forward[1] * Translation[3]) - (Translation[1] * Forward[3]);
// // // // 			float Coef06 = (Up[1] * Translation[3]) - (Translation[1] * Up[3]);
// // // // 			float Coef07 = (Up[1] * Forward[3]) - (Forward[1] * Up[3]);

// // // // 			float Coef08 = (Forward[1] * Translation[2]) - (Translation[1] * Forward[2]);
// // // // 			float Coef10 = (Up[1] * Translation[2]) - (Translation[1] * Up[2]);
// // // // 			float Coef11 = (Up[1] * Forward[2]) - (Forward[1] * Up[2]);

// // // // 			float Coef12 = (Forward[0] * Translation[3]) - (Translation[0] * Forward[3]);
// // // // 			float Coef14 = (Up[0] * Translation[3]) - (Translation[0] * Up[3]);
// // // // 			float Coef15 = (Up[0] * Forward[3]) - (Forward[0] * Up[3]);

// // // // 			float Coef16 = (Forward[0] * Translation[2]) - (Translation[0] * Forward[2]);
// // // // 			float Coef18 = (Up[0] * Translation[2]) - (Translation[0] * Up[2]);
// // // // 			float Coef19 = (Up[0] * Forward[2]) - (Forward[0] * Up[2]);

// // // // 			float Coef20 = (Forward[0] * Translation[1]) - (Translation[0] * Forward[1]);
// // // // 			float Coef22 = (Up[0] * Translation[1]) - (Translation[0] * Up[1]);
// // // // 			float Coef23 = (Up[0] * Forward[1]) - (Forward[0] * Up[1]);

// // // // 			Vector4 Fac0 = new(Coef00, Coef00, Coef02, Coef03);
// // // // 			Vector4 Fac1 = new(Coef04, Coef04, Coef06, Coef07);
// // // // 			Vector4 Fac2 = new(Coef08, Coef08, Coef10, Coef11);
// // // // 			Vector4 Fac3 = new(Coef12, Coef12, Coef14, Coef15);
// // // // 			Vector4 Fac4 = new(Coef16, Coef16, Coef18, Coef19);
// // // // 			Vector4 Fac5 = new(Coef20, Coef20, Coef22, Coef23);

// // // // 			Vector4 Vec0 = new(Up[0], Right[0], Right[0], Right[0]);
// // // // 			Vector4 Vec1 = new(Up[1], Right[1], Right[1], Right[1]);
// // // // 			Vector4 Vec2 = new(Up[2], Right[2], Right[2], Right[2]);
// // // // 			Vector4 Vec3 = new(Up[3], Right[3], Right[3], Right[3]);

// // // // 			Vector4 Inv0 = new((Vec1 * Fac0) - (Vec2 * Fac1) + (Vec3 * Fac2));
// // // // 			Vector4 Inv1 = new((Vec0 * Fac0) - (Vec2 * Fac3) + (Vec3 * Fac4));
// // // // 			Vector4 Inv2 = new((Vec0 * Fac1) - (Vec1 * Fac3) + (Vec3 * Fac5));
// // // // 			Vector4 Inv3 = new((Vec0 * Fac2) - (Vec1 * Fac4) + (Vec2 * Fac5));

// // // // 			Vector4 SignA = new(+1, -1, +1, -1);
// // // // 			Vector4 SignB = new(-1, +1, -1, +1);
// // // // 			Matrix Inverse = new(Inv0 * SignA, Inv1 * SignB, Inv2 * SignA, Inv3 * SignB);

// // // // 			Vector4 Row0 = new(Inverse.Right[0], Inverse.Up[0], Inverse.Forward[0], Inverse.Translation[0]);

// // // // 			Vector4 Dot0 = new(Right * Row0);
// // // // 			float Dot1 = Dot0.X + Dot0.Y + (Dot0.Z + Dot0.W);

// // // // 			float OneOverDeterminant = 1.0f / Dot1;

// // // // 			this = Inverse * OneOverDeterminant;
// // // //         }

// // // //         /// <summary>
// // // //         /// Code monogame
// // // //         /// </summary>
// // // //         /// <param name="matrix"></param>
// // // //         /// <param name="result"></param>
// // // //         public static void Invert(ref Matrix matrix, out Matrix result)
// // // //         {
// // // // 			float num1 = matrix.Right.X;
// // // // 			float num2 = matrix.Right.Y;
// // // // 			float num3 = matrix.Right.Z;
// // // // 			float num4 = matrix.Right.W;
// // // // 			float num5 = matrix.Up.X;
// // // // 			float num6 = matrix.Up.Y;
// // // // 			float num7 = matrix.Up.Z;
// // // // 			float num8 = matrix.Up.W;
// // // // 			float num9 =  matrix.Forward.X;
// // // // 			float num10 = matrix.Forward.Y;
// // // // 			float num11 = matrix.Forward.Z;
// // // // 			float num12 = matrix.Forward.W;
// // // // 			float num13 = matrix.Translation.X;
// // // // 			float num14 = matrix.Translation.Y;
// // // // 			float num15 = matrix.Translation.Z;
// // // // 			float num16 = matrix.Translation.W;
// // // // 			float num17 = (float) ((num11 *  num16) - (num12 *  num15));
// // // // 			float num18 = (float) ((num10 *  num16) - (num12 *  num14));
// // // // 			float num19 = (float) ((num10 *  num15) - (num11 *  num14));
// // // // 			float num20 = (float) ((num9 *  num16) - (num12 *  num13));
// // // // 			float num21 = (float) ((num9 *  num15) - (num11 *  num13));
// // // // 			float num22 = (float) ((num9 *  num14) - (num10 *  num13));

// // // // 			float num23 = (float) ((num6 *  num17) - (num7 *  num18) + (num8 *  num19));
// // // // 			float num24 = (float) -((num5 *  num17) - (num7 *  num20) + (num8 *  num21));
// // // // 			float num25 = (float) ((num5 *  num18) - (num6 *  num20) + (num8 *  num22));
// // // // 			float num26 = (float) -((num5 *  num19) - (num6 *  num21) + (num7 *  num22));
// // // // 			float num27 = (float) (1.0 / ((num1 *  num23) + (num2 *  num24) + (num3 *  num25) + (num4 *  num26)));

// // // // 			result.Right.X = num23 * num27;
// // // // 			result.Up.X = num24 * num27;
// // // // 			result.Forward.X = num25 * num27;
// // // // 			result.Translation.X = num26 * num27;
// // // // 			result.Right.Y = (float) -((num2 *  num17) - (num3 *  num18) + (num4 *  num19)) * num27;
// // // // 			result.Up.Y = (float) ((num1 *  num17) - (num3 *  num20) + (num4 *  num21)) * num27;
// // // // 			result.Forward.Y = (float) -((num1 *  num18) - (num2 *  num20) + (num4 *  num22)) * num27;
// // // // 			result.Translation.Y = (float) ((num1 *  num19) - (num2 *  num21) + (num3 *  num22)) * num27;
// // // // 			float num28 = (float) ((num7 *  num16) - (num8 *  num15));
// // // // 			float num29 = (float) ((num6 *  num16) - (num8 *  num14));
// // // // 			float num30 = (float) ((num6 *  num15) - (num7 *  num14));
// // // // 			float num31 = (float) ((num5 *  num16) - (num8 *  num13));
// // // // 			float num32 = (float) ((num5 *  num15) - (num7 *  num13));
// // // // 			float num33 = (float) ((num5 *  num14) - (num6 *  num13));
// // // // 			result.Right.Z = (float) ((num2 *  num28) - (num3 *  num29) + (num4 *  num30)) * num27;
// // // // 			result.Up.Z = (float) -((num1 *  num28) - (num3 *  num31) + (num4 *  num32)) * num27;
// // // // 			result.Forward.Z = (float) ((num1 *  num29) - (num2 *  num31) + (num4 *  num33)) * num27;
// // // // 			result.Translation.Z = (float) -((num1 *  num30) - (num2 *  num32) + (num3 *  num33)) * num27;
// // // // 			float num34 = (float) ((num7 *  num12) - (num8 *  num11));
// // // // 			float num35 = (float) ((num6 *  num12) - (num8 *  num10));
// // // // 			float num36 = (float) ((num6 *  num11) - (num7 *  num10));
// // // // 			float num37 = (float) ((num5 *  num12) - (num8 *  num9));
// // // // 			float num38 = (float) ((num5 *  num11) - (num7 *  num9));
// // // // 			float num39 = (float) ((num5 *  num10) - (num6 *  num9));
// // // // 			result.Right.W = (float) -((num2 *  num34) - (num3 *  num35) + (num4 *  num36)) * num27;
// // // // 			result.Up.W = (float) ((num1 *  num34) - (num3 *  num37) + (num4 *  num38)) * num27;
// // // // 			result.Forward.W = (float) -((num1 *  num35) - (num2 *  num37) + (num4 *  num39)) * num27;
// // // // 			result.Translation.W = (float) ((num1 *  num36) - (num2 *  num38) + (num3 *  num39)) * num27;
// // // //         }
// // // //         /// <summary>
// // // //         /// Create a matrix world with all element position, target,Up
// // // //         /// </summary>
// // // //         /// <param name="result"></param>
// // // //         /// <param name="position"></param>
// // // //         /// <param name="forward"></param>
// // // //         /// <param name="up"></param>
// // // //         public static void CreateWorld( ref Matrix result,ref Vector4 position,ref  Vector4 forward, ref Vector4 up)
// // // //         {
// // // //             Vector4 z = new(forward);
// // // //             z.Normalize();
// // // //             Vector4 x = Vector4.Cross(ref forward,ref up);
// // // //             Vector4 y = Vector4.Cross(ref x,ref forward);
// // // //             x.Normalize();
// // // //             y.Normalize();

// // // //             result.Identity();
// // // //             result.Right = x;
// // // //             result.Up=y;
// // // //             result.Translation =position;
// // // //             result.Forward= z;
// // // //             result.Translation.W = 1.0f;
// // // //         }

// // // //         public bool Equals(Matrix other) => false;
// // // //     }
// // // // }









// // // namespace MCJ.Engine.Math
// // // {
// // //     using System;
// // //     using System.Runtime.InteropServices;
// // //     using System.Runtime.CompilerServices;
// // //     /// <summary>
// // //     /// Représente un vecteur 2 dimensions
// // //     /// </summary>
// // //     [StructLayout(LayoutKind.Sequential, Pack = 4 )]//pack 4 aligned on float => 
// // //     [SkipLocalsInit]
// // //     public struct Vector2 : IEquatable<Vector2> , IDisposable
// // //     {
// // //         private static readonly Vector2 zero = HeapAlloc(0.0f,0.0f);
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         public static ref readonly Vector2 Zero => ref zero;
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <returns></returns>
// // //         public static readonly Vector2 one = HeapAlloc(1.0f,1.0f);
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         public static ref readonly Vector2 One  => ref one;
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <returns></returns>
// // //         public static readonly int SizeOf = Marshal.SizeOf<Vector2>();

// // //         /// <summary>Axe  Abcisse  
// // //         /// </summary>
// // //         public float X;
// // //         /// <summary> Axe Ordonnée 
// // //         /// </summary>
// // //         public float Y;

// // //         /// <summary>
// // //         /// Constructeur complet 
// // //         /// </summary>
// // //         /// <param name="x"> axe x </param>
// // //         /// <param name="y">axe y </param>
// // //         /// <returns></returns>
// // //         public Vector2(float x=0.0f, float y=0.0f)  =>(X,Y) = (x,y);

// // //         /// <summary>
// // //         /// Equivelent construteur de Copie
// // //         /// </summary>
// // //         /// <param name="v"></param>
// // //         /// <returns></returns>
// // //         public Vector2( Vector2 v) =>(X,Y)=( v.X,v.Y);

// // //         /// <summary>
// // //         /// Constrcuteur a partir d'un tableau de flotant
// // //         /// </summary>
// // //         /// <param name="floats"></param>
// // //         /// <returns></returns>
// // //         public Vector2( float[] floats) =>(X,Y)=( floats[0],floats[1] );
        
// // //         /// <summary>
// // //         /// Retourne la valeur abcisse ou ordonée utilisation comme un tableau  vecteur2[0] ou vecteur2[1]
// // //         /// </summary>
// // //         /// <value></value>
// // //         public float this[int index]
// // //         {
// // //             get => index switch
// // //             {
// // //                 0 => X,
// // //                 1 => Y,
// // //                 _ => throw new ArgumentOutOfRangeException("index invalide")
// // //             };
// // //             set => _ = index switch
// // //             {
// // //                 0 => X = value,
// // //                 1 => Y = value,
// // //                 _ => throw new ArgumentOutOfRangeException("index invalide")
// // //             };
// // //         }
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <returns></returns>
// // //         public override string ToString()  => "Vector2["+X.ToString()+";"+Y.ToString()+"]";
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <returns></returns>
// // //         public override int GetHashCode() => MCJ.Engine.Math.HashCode.Get(this.X) + MCJ.Engine.Math.HashCode.Get(this.Y);
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="obj"></param>
// // //         /// <returns></returns>
// // //         public override bool Equals(object obj) => obj is Vector2 vec && this.Equals(vec)  ;
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="other"></param>
// // //         /// <returns></returns>
// // //         public bool Equals(Vector2 other) 
// // //             => (Maths.Abs(X - other.X) <= Maths.ZeroTolerance) && (Maths.Abs(Y - other.Y) <= Maths.ZeroTolerance);
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="left"></param>
// // //         /// <param name="right"></param>
// // //         /// <returns></returns>
// // //         public static bool operator ==(Vector2 left, Vector2 right) => left.Equals(right);
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="left"></param>
// // //         /// <param name="right"></param>
// // //         /// <returns></returns>
// // //         public static bool operator !=(Vector2 left, Vector2 right) => !(left.Equals(right));
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="scalar"></param>
// // //         /// <param name="value"></param>
// // //         /// <returns></returns>
// // //         public static Vector2 operator -(float scalar, Vector2 value) => StackAlloc(scalar - value.X, scalar - value.Y);
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="scalar"></param>
// // //         /// <param name="value"></param>
// // //         /// <returns></returns>
// // //         public static Vector2 operator +(float scalar, Vector2 value) => StackAlloc( scalar + value.X , scalar + value.Y);


// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <returns></returns>
// // //         public float[] ToArrayStaticalloc()
// // //         {
// // //             float[] r ={X,Y }; 
// // //             return r;
// // //         }
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <value></value>
// // //         public float[] ToArray => new float[]{ X,Y};

// // //         /// <summary>
// // //         /// Allocation sur la pile
// // //         /// </summary>
// // //         /// <param name="value_x"></param>
// // //         /// <param name="value_y"></param>
// // //         /// <returns></returns>
// // //         public static Vector2 StackAlloc( float value_x , float value_y)
// // //         {
// // //             Vector2 vector2 = default;
// // //             vector2.X = value_x;
// // //             vector2.Y = value_y;
// // //             return vector2;
// // //         }
// // //         /// <summary>
// // //         /// Allocation sur le tas ( usage de new )
// // //         /// </summary>
// // //         /// <param name="value_x"></param>
// // //         /// <param name="value_y"></param>
// // //         /// <returns></returns>
// // //         public static Vector2 HeapAlloc( float value_x, float value_y)
// // //             => new Vector2(value_x,  value_y);
// // //         /// <summary>
// // //         /// normalement sa devrait pas marché car Suppressfinalize ne fonctionne pas sur une structure 
// // //         /// </summary>
// // //         public void Dispose()
// // //         {
            
// // //             #pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
// // //             GC.SuppressFinalize(this);
// // //             #pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
// // //         }
// // //     }
// // // }










// // // namespace MCJ.Engine.Math
// // // {
// // //     using System;
// // //     using System.Runtime.InteropServices;
// // //     using System.Runtime.CompilerServices;
// // //     /// <summary>
// // //     /// Représente un vecteur 2 dimensions
// // //     /// </summary>
// // //     [StructLayout(LayoutKind.Sequential, Pack = 4 )]//pack 4 aligned on float => 
// // //     [SkipLocalsInit]
// // //     public struct Vector3 : IEquatable<Vector3> , IDisposable
// // //     {
// // //         private static readonly Vector3 zero = HeapAlloc(0.0f,0.0f,0.0f);
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         public static ref readonly Vector3 Zero => ref zero;
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <returns></returns>
// // //         public static readonly Vector3 one = HeapAlloc(1.0f,1.0f,1.0f);
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         public static ref readonly Vector3 One  => ref one;
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <returns></returns>
// // //         public static readonly int SizeOf = Marshal.SizeOf<Vector3>();
// // //         /// <summary>Axe  Abcisse  
// // //         /// </summary>
// // //         public float X;
// // //         /// <summary> Axe Ordonnée  </summary>
// // //         public float Y;
// // //         /// <summary> Axe Z ( 3D )  </summary>
// // //         public float Z;
        
// // //         /// <summary>
// // //         /// Constructeur complet 
// // //         /// </summary>
// // //         /// <param name="x"> axe x </param>
// // //         /// <param name="y">axe y </param>
// // //         /// <param name="z">axe y </param>
// // //         /// <returns></returns>
// // //         public Vector3(float x=0.0f, float y=0.0f, float z=0.0f)  =>(X,Y,Z) = (x,y,z);

// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="v"></param>
// // //         /// <param name="z"></param>
// // //         /// <param name="w"></param>
// // //         /// <returns></returns>
// // //         public Vector3( Vector2 v, float z=0.0f, float w=0.0f) =>(X,Y,Z)=( v.X,v.Y, z);

// // //         /// <summary>
// // //         /// Constrcuteur a partir d'un tableau de flotant
// // //         /// </summary>
// // //         /// <param name="floats"></param>
// // //         /// <returns></returns>
// // //         public Vector3( float[] floats) =>(X,Y,Z)=( floats[0],floats[1],floats[2] );
        
// // //         /// <summary>
// // //         /// Instanciate a partir d'un scalaire
// // //         /// </summary>
// // //         /// <param name="scalar"></param>
// // //         public Vector3(float scalar)
// // //             =>( X,Y,Z) = ( scalar,scalar,scalar);
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="vec4"></param>
// // //         public Vector3( Vector3 vec4)
// // //             => ( X,Y,Z) = ( vec4.X,vec4.Y,vec4.Z);
// // //         /// <summary>
// // //         /// Retourne la valeur abcisse ou ordonée utilisation comme un tableau  vecteur2[0] ou vecteur2[1]
// // //         /// </summary>
// // //         /// <value></value>
// // //         public float this[int index]
// // //         {
// // //             get => index switch
// // //             {
// // //                 0 => X,
// // //                 1 => Y,
// // //                 2 => Z,
// // //                 _ => throw new ArgumentOutOfRangeException("index invalide")
// // //             };
// // //             set => _ = index switch
// // //             {
// // //                 0 => X = value,
// // //                 1 => Y = value,
// // //                 2 => Z = value,
// // //                 _ => throw new ArgumentOutOfRangeException("index invalide")
// // //             };
// // //         }
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <returns></returns>
// // //         public override string ToString()  => $"[X={X:G3};Y={Y:G3};Z={Z:G3}]";
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <returns></returns>
// // //         public override int GetHashCode() => MCJ.Engine.Math.HashCode.Get(this.X) + MCJ.Engine.Math.HashCode.Get(this.Y)+ MCJ.Engine.Math.HashCode.Get(this.Z);
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="obj"></param>
// // //         /// <returns></returns>
// // //         public override bool Equals(object obj) => obj is Vector3 vec && this.Equals(vec)  ;
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="other"></param>
// // //         /// <returns></returns>
// // //         public bool Equals(Vector3 other) 
// // //             => (Maths.Abs(X - other.X) <= Maths.ZeroTolerance) && (Maths.Abs(Y - other.Y) <= Maths.ZeroTolerance) && (Maths.Abs(Z - other.Z) <= Maths.ZeroTolerance) ;
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="left"></param>
// // //         /// <param name="right"></param>
// // //         /// <returns></returns>
// // //         public static bool operator ==(Vector3 left, Vector3 right) => left.Equals(right);
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="left"></param>
// // //         /// <param name="right"></param>
// // //         /// <returns></returns>
// // //         public static bool operator !=(Vector3 left, Vector3 right) => !(left.Equals(right));

// // //         /// <summary>
// // //         /// Allocation sur la pile
// // //         /// </summary>
// // //         /// <param name="value_x"></param>
// // //         /// <param name="value_y"></param>
// // //         /// <param name="value_z"></param>
// // //         /// <returns></returns>
// // //         public static Vector3 StackAlloc( float value_x , float value_y , float value_z )
// // //         {
// // //             Vector3 vec = default;
// // //             vec.X = value_x;
// // //             vec.Y = value_y;
// // //             vec.Z = value_z;
// // //             return vec;
// // //         }
// // //         /// <summary>
// // //         /// Allocation sur le tas ( usage de new )
// // //         /// </summary>
// // //         /// <param name="value_x"></param>
// // //         /// <param name="value_y"></param>
// // //         /// <param name="value_z"></param>
// // //         /// <returns></returns>
// // //         public static Vector3 HeapAlloc( float value_x, float value_y, float value_z)
// // //             =>new Vector3(value_x,  value_y, value_z);
        
// // // #region OVERRIDE OPERATOR
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="left"></param>
// // //         /// <param name="right"></param>
// // //         /// <returns></returns>
// // //         public static Vector3 operator *(Vector3 left, Vector3 right)
// // //             => StackAlloc(left.X * right.X, left.Y * right.Y, left.Z* right.Z);
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="result"></param>
// // //         /// <param name="left"></param>
// // //         /// <param name="right"></param>
// // //         public static void MultiplyVec4ByVec4(ref Vector3 result, ref Vector3 left, ref Vector3 right)
// // //         => ( result.X,result.Y,result.Z )=(left.X * right.X, left.Y * right.Y, left.Z* right.Z);
// // //         /// <summary>
// // //         /// scalaire x vecteur 
// // //         /// </summary>
// // //         /// <param name="scale"></param>
// // //         /// <param name="value"></param>
// // //         /// <returns></returns>
// // //         public static Vector3 operator *(float scale, Vector3 value)
// // //             => StackAlloc(value.X * scale, value.Y * scale, value.Z *scale);
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="value"></param>
// // //         /// <param name="scale"></param>
// // //         /// <returns></returns>
// // //         public static Vector3 operator *(Vector3 value, float scale)
// // //             => StackAlloc(value.X * scale, value.Y * scale, value.Z *scale);
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="result"></param>
// // //         /// <param name="value"></param>
// // //         /// <param name="scalar"></param>
// // //         public static void MultiplyVector3ByScalar( ref Vector3 result, ref Vector3 value , float scalar)
// // //             => (result.X, result.Y, result.Z) = (value.X*scalar, value.Y*scalar, value.Z*scalar);
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="left"></param>
// // //         /// <param name="right"></param>
// // //         /// <returns></returns>
// // //         public static Vector3 operator +(Vector3 left, Vector3 right)
// // //             => StackAlloc(left.X + right.X, left.Y + right.Y , left.Z+right.Z);
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="result"></param>
// // //         /// <param name="left"></param>
// // //         /// <param name="right"></param>
// // //         public static void AddVec4WithVec4(ref Vector3 result, ref Vector3 left, ref Vector3 right)
// // //         => ( result.X,result.Y,result.Z)=(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="value"></param>
// // //         /// <param name="scalar"></param>
// // //         /// <returns></returns>
// // //         public static Vector3 operator +(Vector3 value, float scalar)
// // //             => StackAlloc(value.X + scalar, value.Y + scalar , value.Z + scalar) ;
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="scalar"></param>
// // //         /// <param name="value"></param>
// // //         /// <returns></returns>
// // //         public static Vector3 operator +(float scalar, Vector3 value)
// // //             => StackAlloc(scalar + value.X, scalar + value.Y, scalar + value.Z);
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="value"></param>
// // //         /// <param name="scale"></param>
// // //         /// <returns></returns>
// // //         public static Vector3 operator /(Vector3 value, float scale)
// // //             => StackAlloc( value.X/scale , value.Y/ scale , value.Z/scale );
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="scalar"></param>
// // //         /// <param name="vec"></param>
// // //         /// <returns></returns>
// // //         public static Vector3 operator /(float scalar, Vector3 vec)
// // //             => StackAlloc( scalar/vec.X , scalar/vec.Y, scalar/vec.Z );
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="value"></param>
// // //         /// <param name="scalar"></param>
// // //         /// <returns></returns>
// // //         public static Vector3 operator -(Vector3 value, float scalar)
// // //             => StackAlloc(value.X - scalar, value.Y - scalar, value.Z - scalar);
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="scalar"></param>
// // //         /// <param name="value"></param>
// // //         /// <returns></returns>
// // //         public static Vector3 operator -(float scalar, Vector3 value)
// // //             => StackAlloc(scalar - value.X, scalar - value.Y, scalar- value.Z);
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <param name="left"></param>
// // //         /// <param name="right"></param>
// // //         /// <returns></returns>
// // //         public static Vector3 operator - (Vector3 left, Vector3 right)
// // //             => StackAlloc(left.X - right.X, left.Y - right.Y,  left.Z - right.Z);
// // //     #endregion
    
// // //         /// <summary>
// // //         /// Normalize n vector ( perdendiculaire )
// // //         /// </summary>
// // //         /// <param name="v"></param>
// // //         /// <returns></returns>
// // //         public static Vector3 Normalize( Vector3 v)
// // //         {
// // //             float length = 1/ v.Length();
// // //             return Vector3.HeapAlloc( v.X*length, v.Y*length,v.Z*length );
// // //         }
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         public void Normalize()
// // //         {
// // //             float length = 1/ this.Length();
// // //             this.X *= length;
// // //             this.Y *= length;
// // //             this.Z *= length;
// // //         }
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         public void Negate()
// // //         {
// // //             this.X = -X;
// // //             this.Y = -Y;
// // //             this.Z = -Z;
// // //             //return this;
// // //         }
// // //         /// <summary>
// // //         /// Produit de 2 vecteur  cross product pour trouver la perpendiculaire a 2 vecteur ( normale)
// // //         /// </summary>
// // //         /// <param name="v1"></param>
// // //         /// <param name="v2"></param>
// // //         /// <returns></returns>
// // //         public static Vector3 Cross(ref Vector3 v1,ref Vector3 v2)
// // //             => StackAlloc( (v1.Y * v2.Z) - (v1.Z * v2.Y),
// // //                  (v1.Z * v2.X) - (v1.X * v2.Z),
// // //                  (v1.X * v2.Y) - (v1.Y * v2.X));
// // //         /// <summary>
// // //         /// Produit de 2 vecteur  cross product pour trouver la perpendiculaire a 2 vecteur ( normale)
// // //         /// </summary>
// // //         /// <param name="result"></param>
// // //         /// <param name="v1"></param>
// // //         /// <param name="v2"></param>
// // //         public static void Cross(ref Vector3 result , ref Vector3 v1,ref  Vector3 v2)
// // //             => ( result.X,result.Y,result.Z ) = (
// // //                 (v1.Y * v2.Z) - (v1.Z * v2.Y),
// // //                 (v1.Z * v2.X) - (v1.X * v2.Z),
// // //                 (v1.X * v2.Y) - (v1.Y * v2.X))                ;

// // //         /// <summary>
// // //         /// dot product cosinus de l'angle entre les deux vecteurs
// // //         /// </summary>
// // //         /// <param name="v1"></param>
// // //         /// <param name="v2"></param>
// // //         /// <returns></returns>
// // //         [MethodImpl(MethodImplOptions.AggressiveInlining)]
// // //         public static float Dot(ref Vector3  v1,ref Vector3  v2)
// // //             => (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z) ;

// // //         /// <summary>
// // //         /// longueur d'un vecteur 
// // //         /// </summary>
// // //         /// <returns></returns>
// // //         [MethodImpl(MethodImplOptions.AggressiveInlining)]
// // //         public float Length()
// // //             =>  Maths.Sqrt((X * X) + (Y * Y) + (Z * Z) );

// // //         /// <summary>
// // //         /// distance entre deux vecteur
// // //         /// </summary>
// // //         /// <param name="value1"></param>
// // //         /// <param name="value2"></param>
// // //         /// <returns></returns>
// // //         [MethodImpl(MethodImplOptions.AggressiveInlining)]
// // //         public static float Distance(ref Vector3 value1,ref Vector3 value2)
// // //             =>  Maths.Sqrt(((value1.X - value2.X) * (value1.X - value2.X)) + ((value1.Y - value2.Y) * (value1.Y - value2.Y)) + ((value1.Z - value2.Z) * (value1.Z - value2.Z)) );

// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <returns></returns>
// // //         public float[] ToArrayStaticalloc()
// // //         {
// // //             float[] r ={X,Y,Z }; 
// // //             return r;
// // //         }
// // //         /// <summary>
// // //         /// 
// // //         /// </summary>
// // //         /// <value></value>
// // //         public float[] ToArray => new float[]{ X,Y,Z};

// // //         /// <summary>
// // //         /// normalement sa devrait pas marché car Suppressfinalize ne fonctionne pas sur une structure 
// // //         /// </summary>
// // //         public void Dispose()
// // //         {
            
// // //             #pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
// // //             GC.SuppressFinalize(this);
// // //             #pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
// // //         }
// // //     }
// // // }

        
// // //         // public static void Transform(ref Vector3 result,ref Vector3 value, ref Matrix matrix)
// // //         //     => (result.X, result.Y , result.Z, result.W) =
// // //         //     ((value.X * matrix.Right.X) + (value.Y * matrix.Up.X) + (value.Z * matrix.Forward.X) + (value.W * matrix.Translation.X),
// // //         //     (value.X * matrix.Right.Y) + (value.Y * matrix.Up.Y) + (value.Z * matrix.Forward.Y) + (value.W * matrix.Translation.Y),
// // //         //     (value.X * matrix.Right.Z) + (value.Y * matrix.Up.Z) + (value.Z * matrix.Forward.Z) + (value.W * matrix.Translation.Z),
// // //         //     (value.X * matrix.Right.W) + (value.Y * matrix.Up.W) + (value.Z * matrix.Forward.W) + (value.W * matrix.Translation.W));

// // // // {
// // // //     using System.Runtime.InteropServices;
// // // //     using System;
// // // //     [StructLayout(LayoutKind.Sequential, Pack = 8)]
// // // //     public struct Vector3 : IEquatable<Vector3>
// // // //     {
// // // //         /// <summary>
// // // //         /// The size of the <see cref="Stride.Core.Mathematics.Vector3"/> type, in bytes.
// // // //         /// </summary>
// // // //         public static readonly int SizeInBytes =System.Runtime.InteropServices.Marshal.SizeOf<Vector3>();

// // // //         /// <summary>
// // // //         /// A <see cref="Stride.Core.Mathematics.Vector3"/> with all of its components set to zero.
// // // //         /// </summary>
// // // //         public static readonly Vector3 Zero = new();

// // // //         /// <summary>
// // // //         /// The X unit <see cref="Stride.Core.Mathematics.Vector3"/> (1, 0, 0, 0).
// // // //         /// </summary>
// // // //         public static readonly Vector3 UnitX = new(1.0f, 0.0f, 0.0f);

// // // //         /// <summary>
// // // //         /// The Y unit <see cref="Stride.Core.Mathematics.Vector3"/> (0, 1, 0, 0).
// // // //         /// </summary>
// // // //         public static readonly Vector3 UnitY = new(0.0f, 1.0f, 0.0f);

// // // //         /// <summary>
// // // //         /// The Z unit <see cref="Stride.Core.Mathematics.Vector3"/> (0, 0, 1, 0).
// // // //         /// </summary>
// // // //         public static readonly Vector3 UnitZ = new(0.0f, 0.0f, 1.0f);

// // // //         /// <summary>
// // // //         /// A <see cref="Stride.Core.Mathematics.Vector3"/> with all of its components set to one.
// // // //         /// </summary>
// // // //         public static readonly Vector3 One = new(1.0f, 1.0f, 1.0f);

// // // //         public static readonly Vector3 CameraUp = new(0.0f,1.0f,0.0f);

// // // //         /// <summary>
// // // //         /// The X component of the vector.
// // // //         /// </summary>
// // // //         public float X;

// // // //         /// <summary>
// // // //         /// The Y component of the vector.
// // // //         /// </summary>
// // // //         public float Y;
// // // //         /// <summary>
// // // //         /// The Z component of the vector.
// // // //         /// </summary>
// // // //         public float Z;

// // // //         /// <summary>
// // // //         /// Initialise une nouvelle instance de ce vector avec toutes les valeurs a zero
// // // //         /// </summary>
// // // //         //public Vector3(){ }

// // // //         public Vector3(float x, float y, float z )
// // // //             =>(X,Y,Z)=(x,y,z);

// // // //         public Vector3( Vector3 v)
// // // //             =>(X,Y,Z) = ( v.X,v.Y,v.Z);

// // // //         public Vector3( float[] floats)
// // // //             =>(X,Y,Z) = ( floats[0],floats[1],floats[2] );

// // // //         public float this[int index]
// // // //         {
// // // //             get => index switch
// // // //             {
// // // //                     0=>X,
// // // //                     1=>Y,
// // // //                     2=>Z,
// // // //                     _ => 0//throw new System.ArgumentOutOfRangeException("Vector  indeX invalide")
// // // //             };
// // // //             set => _ = index switch
// // // //             {
// // // //                     0=> X = value,
// // // //                     1=> Y = value,
// // // //                     2=> Z = value,
// // // //                     _ => 0//throw new System.ArgumentOutOfRangeException("Vector  indeX invalide")
// // // //             };
// // // //         }

// // // //         /// <summary>
// // // //         /// Normalize n vector ( perdendiculaire )
// // // //         /// </summary>
// // // //         /// <param name="x"></param>
// // // //         /// <returns></returns>
// // // //         public static Vector3 Normalize(Vector3 v)
// // // //         {
// // // //             float length = 1/ v.Length();
// // // //             return new Vector3( v.X*length, v.Y*length,v.Z*length );
// // // //         }
// // // //         /// <summary>
// // // //         /// Produit de 2 vecteur  cross product
// // // //         /// </summary>
// // // //         /// <param name="x"></param>
// // // //         /// <param name="y"></param>
// // // //         /// <returns></returns>
// // // //         public static Vector3 Cross(Vector3 v1, Vector3 v2)
// // // //             => new(x: (v1.Y * v2.Z) - (v1.Z * v2.Y),
// // // //                 y: (v1.Z * v2.X) - (v1.X * v2.Z),
// // // //                 z: (v1.X * v2.Y) - (v1.Y * v2.X));

// // // //         /// <summary>
// // // //         /// dot product
// // // //         /// </summary>
// // // //         /// <param name="v1"></param>
// // // //         /// <param name="v2"></param>
// // // //         /// <returns></returns>
// // // //         [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
// // // //         public static float Dot(Vector3  v1, Vector3  v2)
// // // //             => (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z) ;

// // // //         [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
// // // //         public float Length()
// // // //             => (float)System.Math.Sqrt((X * X) + (Y * Y) + (Z * Z) );

// // // //         /// <summary>
// // // //         /// Calculates the distance between two vectors.
// // // //         /// </summary>
// // // //         /// <param name="value1">The first vector.</param>
// // // //         /// <param name="value2">The second vector.</param>
// // // //         /// <returns>The distance between the two vectors.</returns>
// // // //         public static float Distance(Vector3 value1, Vector3 value2)
// // // //         {
// // // //             float x = value1.X - value2.X;
// // // //             float y = value1.Y - value2.Y;
// // // //             float z = value1.Z - value2.Z;

// // // //             return (float)System.Math.Sqrt((x * x) + (y * y) + (z * z) );
// // // //         }

// // // //         /// <summary>
// // // //         /// Get perpendicular of vector
// // // //         /// </summary>
// // // //         /// <returns></returns>
// // // //         public Vector3 Normalize()
// // // //         {
// // // //             float inv = 1.0f / Length();
// // // //             X *= inv;
// // // //             Y *= inv;
// // // //             Z *=inv;
// // // //             return this ;
// // // //         }

// // // //         public static Vector3 Abs(Vector3 value)
// // // //             => new(value.X > 0.0f ? value.X : -value.X, value.Y > 0.0f ? value.Y : -value.Y, value.Z >0.0f ? value.Z:-value.Z);

// // // //         public Vector3 Abs() => new( X > 0.0f ? X : -X ,Y> 0.0f ? Y:-Y,Z >0.0f? Z:-Z);

// // // //         public override string ToString() => "Vector3["+X+";"+Y+";"+Z+"]";
// // // //         public override int GetHashCode() => (int)(X+Y+Z)^32;
// // // //         public override bool Equals(object obj) => obj is Vector3 vec && vec.X == this.X && vec.Y== this.Y && vec.Z==this.Z ;
// // // //         public bool Equals(Vector3 vec) => vec.X == this.X && vec.Y== this.Y && vec.Z==this.Z ;

// // // //         public static bool operator ==(Vector3 left, Vector3 right)  => left.Equals(right);
// // // //         public static bool operator !=(Vector3 left, Vector3 right)=> !(left.Equals(right));

// // // // #region OVERRIDE OPERATOR
        
// // // //         public static Vector3 operator *(Vector3 left, Vector3 right)
// // // //             => new(left.X * right.X, left.Y * right.Y, left.Z* right.Z);

// // // //         public static Vector3 operator *(float scale, Vector3 value)
// // // //             => new(value.X * scale, value.Y * scale, value.Z *scale);

// // // //         public static Vector3 operator *(Vector3 value, float scale)
// // // //             => new(value.X * scale, value.Y * scale, value.Z *scale);

// // // //         public static Vector3 operator +(Vector3 left, Vector3 right)
// // // //             => new(left.X + right.X, left.Y + right.Y , left.Z+right.Z);
// // // //         public static Vector3 operator +(Vector3 value, float scalar)
// // // //             => new(value.X + scalar, value.Y + scalar , value.Z + scalar) ;
// // // //         public static Vector3 operator +(float scalar, Vector3 value)
// // // //             => new(scalar + value.X, scalar + value.Y, scalar + value.Z);
// // // //         public static Vector3 operator /(Vector3 value, float scale)
// // // //             => new( value.X/scale , value.Y/ scale , value.Z/scale );

// // // //         public static Vector3 operator /(float scalar, Vector3 vec)
// // // //             => new( scalar/vec.X , scalar/vec.Y, scalar/vec.Z );

// // // //         public static Vector3 operator -(Vector3 value, float scalar)
// // // //             => new(value.X - scalar, value.Y - scalar, value.Z - scalar);

// // // //         public static Vector3 operator -(float scalar, Vector3 value)
// // // //             => new(scalar - value.X, scalar - value.Y, scalar- value.Z);

// // // //         public static Vector3 operator - (Vector3 left, Vector3 right)
// // // //             => new(left.X - right.X, left.Y - right.Y,  left.Z - right.Z);
// // // // #endregion

// // // //         /// <summary>
// // // //         /// Creates a new <see cref="Vector3"/> that contains a transformation of 3d-vector by the specified <see cref="Matrix"/>.
// // // //         /// </summary>
// // // //         /// <param name="position">Source <see cref="Vector3"/>.</param>
// // // //         /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
// // // //         /// <param name="result">Transformed <see cref="Vector3"/> as an output parameter.</param>
// // // //         // public static void Transform(ref Vector3 position, ref Matrix4 matrix, ref Vector3 result)
// // // //         // {
// // // //         //     var x = (position.X * matrix.Row1[1]) + (position.Y * matrix.Row2[1]) + (position.Z * matrix.Row3[1]) + matrix.Row4[1];
// // // //         //     var y = (position.X * matrix.Row1[2]) + (position.Y * matrix.Row2[2]) + (position.Z * matrix.Row3[2]) + matrix.Row4[2];
// // // //         //     var z = (position.X * matrix.Row1[3]) + (position.Y * matrix.Row2[3]) + (position.Z * matrix.Row3[3]) + matrix.Row4[3];
// // // //         //     result.X = x;
// // // //         //     result.Y = y;
// // // //         //     result.Z = z;
// // // //         // }

// // // //     }
// // // // }

// // // // /*
// // // // public float Length() => MCJEngine.Maths.Functions.Sqrt ( (X*X) + (Y*Y)+(Z*Z) );

        

        
// // // //         public static Vector3 operator -(Vector3 left, Vector3 right)
// // // //         {
// // // //             return new Vector3(left.X - right.X, left.Y - right.Y, left.Z-right.Z);
// // // //         }

// // // //         public static Vector3 operator -(Vector3 value)
// // // //         {
// // // //             return new Vector3(-value.X, -value.Y, -value.Z);
// // // //         }

// // // //         public static Vector3 operator /(Vector3 value, float scale)
// // // //         {
// // // //             return new Vector3(value.X / scale, value.Y / scale , value.Z/ scale);
// // // //         }

// // // //         public static Vector3 operator /(float scale , Vector3 value)
// // // //         {
// // // //             return new Vector3(scale / value.X, scale / value.Y, scale/value.Z);
// // // //         }

// // // //         public static Vector3 operator /(Vector3 value, Vector3 scale)
// // // //         {
// // // //             return new Vector3(value.X / scale.X, value.Y / scale.Y , value.Z / scale.Z);
// // // //         }

        
// // // //         public static Vector3 operator -(Vector3 value, float scalar)
// // // //         {
// // // //             return new Vector3(value.X - scalar, value.Y - scalar, value.Z - scalar);
// // // //         }

// // // //         public static Vector3 operator -(float scalar, Vector3 value)
// // // //         {
// // // //             return new Vector3(scalar - value.X, scalar - value.Y, scalar- value.Z);
// // // //         }


//     // // Get camera 2d transform matrix
//     // Matrix GetCameraMatrix2D(Camera camera)
//     // {
//     //     Matrix matTransform = new(0.0f);
//     //     // The camera in world-space is set by
//     //     //   1. Move it to target
//     //     //   2. Rotate by -rotation and scale by (1/zoom)
//     //     //      When setting higher scale, it's more intuitive for the world to become bigger (= camera become smaller),
//     //     //      not for the camera getting bigger, hence the invert. Same deal with rotation.
//     //     //   3. Move it by (-offset);
//     //     //      Offset defines target transform relative to screen, but since we're effectively "moving" screen (camera)
//     //     //      we need to do it into opposite direction (inverse transform)

//     //     // Having camera transform in world-space, inverse of it gives the modelview transform.
//     //     // Since (A*B*C)' = C'*B'*A', the modelview is
//     //     //   1. Move to offset
//     //     //   2. Rotate and Scale
//     //     //   3. Move by -target
        
//     //     // Matrix matOrigin = Matrix.Translate(-camera.Target.X, -camera.Target.Y, 0.0f);
//     //     // Matrix matRotation = Matrix.Rotate((Vector3){ 0.0f, 0.0f, 1.0f }, camera.rotation*DEG2RAD);
//     //     // Matrix matScale = Matrix.Scale(camera.zoom, camera.zoom, 1.0f);
//     //     // Matrix matTranslation = Matrix.Translate(camera.offset.x, camera.offset.y, 0.0f);

//     //     // matTransform = matOrigin * matScale * matRotation * matTranslation;

//     //     return matTransform;
//     // }
    
//     // // Get the screen space position for a 2d camera world space position
//     // Vector2 GetWorldToScreen2D(Vector3 position, Camera camera)
//     // {
//     //     Matrix invMatCamera = Matrix.Inverse(GetCameraMatrix2D(camera));
//     //     Vector4 transform = Vector4Transform(new Vector4( position.X, position.Y, 0.0f,1.0f), invMatCamera);

//     //     return new Vector2(transform.X, transform.Y );
//     // }
// }



    
// /// <summary>
// /// colonne type like glm attention ne pas mélanger Matrix4 et Matrix ( logique complètement différentes)
// /// https://github.com/dwmkerr/glmnet/blob/master/source/GlmNet/
// /// </summary>


// #endregion        
// #region OPerateur binaire
// /*
// AVOID GIMBAL LOCK WHITOUT QUATERNION
// // MCJ.Engine.Maths.Vector4 all = (orientation.Row1 * sprite.Axis.X) + (orientation.Row2 * sprite.Axis.Y) + (orientation.Row3 * sprite.Axis.Z);
// // float overallAngular =  all.Length();
// // all.Normalize();
// // MCJ.Engine.Maths.Matrix.Rotate( ref orientation, overallAngular,ref all);
            
// */
//     /// <summary>
//     /// muliplication de deux matrice ! l'ordre 
//     /// https://github.com/g-truc/glm/blob/master/glm/detail/type_mat4x4.inl
//     /// https://github.com/dwmkerr/glmnet/blob/master/source/GlmNet/GlmNet/mat4.cs
//  
    



// // // //         /// <summary>
// // // //         /// Rotate around arbitrary axis
// // // //         /// </summary>
// // // //         /// <param name="m"></param>
// // // //         /// <param name="angle">in radians 0 to 2PI </param>
// // // //         /// <param name="axis"></param>
// // // //         public static void Rotate(ref  Matrix result,in float angle,ref Vector4 axis)
// // // //         {
// // // //             // var c = Math.Cos(  angle  );
// // // //             // var s = Math.Sin(  angle  );

// // // //             // axis.Normalize();
// // // //             // Vector4 temp = new ((1 - c) * axis);

// // // //             // Matrix Rotate = new (1.0f);
// // // //             // Rotate.Right.X = c + (temp[0] * axis[0]);
// // // //             // Rotate.Right.Y = (temp[0] * axis[1]) + (s * axis[2]);
// // // //             // Rotate.Right.Z = (temp[0] * axis[2]) - (s * axis[1]);

// // // //             // Rotate.Up.X = (temp[1] * axis[0]) - (s * axis[2]);
// // // //             // Rotate.Up.Y = c + (temp[1] * axis[1]);
// // // //             // Rotate.Up.Z = (temp[1] * axis[2]) + (s * axis[0]);

// // // //             // Rotate.Forward.X = (temp[2] * axis[0]) + (s * axis[1]);
// // // //             // Rotate.Forward.Y = (temp[2] * axis[1]) - (s * axis[0]);
// // // //             // Rotate.Forward.Z = c + (temp[2] * axis[2]);
// // // //             // m.Right = new ((m.Right * Rotate.Right[0]) + (m.Up * Rotate.Right[1]) + (m.Forward * Rotate.Right[2])); // row 1
// // // //             // m.Up = new ((m.Right * Rotate.Up[0]) + (m.Up * Rotate.Up[1]) + (m.Forward * Rotate.Up[2])); // row 2
// // // //             // m.Forward = new ((m.Right * Rotate.Forward[0]) + (m.Up * Rotate.Forward[1]) + (m.Forward * Rotate.Forward[2])); // row 3
// // // //             float x = axis.X;
// // // // 		    float y = axis.Y;
// // // // 		    float z = axis.Z;
// // // // 		    float num2 = Math.Sin(angle);
// // // // 		    float num = Math.Cos(angle);
// // // // 		    float num11 = x * x;
// // // // 		    float num10 = y * y;
// // // // 		    float num9 = z * z;
// // // // 		    float num8 = x * y;
// // // // 		    float num7 = x * z;
// // // // 		    float num6 = y * z;
// // // // 		    result.Right.X = num11 + (num * (1f - num11));
// // // // 		    result.Right.Y = (num8 - (num * num8)) + (num2 * z);
// // // // 		    result.Right.Z = num7 - (num * num7) - (num2 * y);
// // // // 		    result.Right.W = 0;
// // // // 		    result.Up.X = num8 - (num * num8) - (num2 * z);
// // // // 		    result.Up.Y = num10 + (num * (1f - num10));
// // // // 		    result.Up.Z = (num6 - (num * num6)) + (num2 * x);
// // // // 		    result.Up.W = 0;
// // // // 		    result.Forward.X = (num7 - (num * num7)) + (num2 * y);
// // // // 		    result.Forward.Y = num6 - (num * num6) - (num2 * x);
// // // // 		    result.Forward.Z = num9 + (num * (1f - num9));
// // // //         }

// // // //         /// <summary>
// // // //         /// Rotation angle X
// // // //         /// </summary>
// // // //         /// <param name="result"></param>
// // // //         /// <param name="angle">in radians </param>
// // // //         public static void RotationX(ref Matrix result , in float angle)
// // // //         {
// // // //             float cosx = Math.Cos(  angle  );
// // // //             float sinx = Math.Sin(angle );

// // // //             result.Identity();
// // // //             result.Up.Y = cosx;
// // // //             result.Up.Z = sinx;
// // // //             result.Forward.Y = -sinx;
// // // //             result.Forward.Z = cosx;
// // // //         }

// // // //         public static void RotationY(ref Matrix result,in float angle , bool identity = true)
// // // //         {
// // // //             float cosy = Math.Cos(angle);
// // // //             float siny = Math.Sin(angle);

// // // //             if ( identity)
// // // //                 result.Identity();

// // // //             result.Right.X = cosy;
// // // //             result.Right.Z= -siny;
// // // //             result.Forward.X = siny;
// // // //             result.Forward.Z = cosy;
// // // //         }

// // // //         public static void RotationZ( ref Matrix result, float angle)
// // // //         {
// // // // 			var val1 = Math.Cos(angle);
// // // // 			var val2 = Math.Sin(angle);

// // // //             result.Identity();
// // // //             result.Right.X = val1;
// // // //             result.Right.Y = val2;
// // // //             result.Up.X = -val2;
// // // //             result.Up.Y = val1;
// // // //         }


// // // //         /// <summary>
// // // //         /// Code monogame
// // // //         /// </summary>
// // // //         /// <param name="matrix"></param>
// // // //         /// <param name="result"></param>
// // // //         public static void Invert(ref Matrix matrix, out Matrix result)
// // // //         {
// // // // 			float num1 = matrix.Right.X;
// // // // 			float num2 = matrix.Right.Y;
// // // // 			float num3 = matrix.Right.Z;
// // // // 			float num4 = matrix.Right.W;
// // // // 			float num5 = matrix.Up.X;
// // // // 			float num6 = matrix.Up.Y;
// // // // 			float num7 = matrix.Up.Z;
// // // // 			float num8 = matrix.Up.W;
// // // // 			float num9 =  matrix.Forward.X;
// // // // 			float num10 = matrix.Forward.Y;
// // // // 			float num11 = matrix.Forward.Z;
// // // // 			float num12 = matrix.Forward.W;
// // // // 			float num13 = matrix.Translation.X;
// // // // 			float num14 = matrix.Translation.Y;
// // // // 			float num15 = matrix.Translation.Z;
// // // // 			float num16 = matrix.Translation.W;
// // // // 			float num17 = (float) ((num11 *  num16) - (num12 *  num15));
// // // // 			float num18 = (float) ((num10 *  num16) - (num12 *  num14));
// // // // 			float num19 = (float) ((num10 *  num15) - (num11 *  num14));
// // // // 			float num20 = (float) ((num9 *  num16) - (num12 *  num13));
// // // // 			float num21 = (float) ((num9 *  num15) - (num11 *  num13));
// // // // 			float num22 = (float) ((num9 *  num14) - (num10 *  num13));

// // // // 			float num23 = (float) ((num6 *  num17) - (num7 *  num18) + (num8 *  num19));
// // // // 			float num24 = (float) -((num5 *  num17) - (num7 *  num20) + (num8 *  num21));
// // // // 			float num25 = (float) ((num5 *  num18) - (num6 *  num20) + (num8 *  num22));
// // // // 			float num26 = (float) -((num5 *  num19) - (num6 *  num21) + (num7 *  num22));
// // // // 			float num27 = (float) (1.0 / ((num1 *  num23) + (num2 *  num24) + (num3 *  num25) + (num4 *  num26)));

// // // // 			result.Right.X = num23 * num27;
// // // // 			result.Up.X = num24 * num27;
// // // // 			result.Forward.X = num25 * num27;
// // // // 			result.Translation.X = num26 * num27;
// // // // 			result.Right.Y = (float) -((num2 *  num17) - (num3 *  num18) + (num4 *  num19)) * num27;
// // // // 			result.Up.Y = (float) ((num1 *  num17) - (num3 *  num20) + (num4 *  num21)) * num27;
// // // // 			result.Forward.Y = (float) -((num1 *  num18) - (num2 *  num20) + (num4 *  num22)) * num27;
// // // // 			result.Translation.Y = (float) ((num1 *  num19) - (num2 *  num21) + (num3 *  num22)) * num27;
// // // // 			float num28 = (float) ((num7 *  num16) - (num8 *  num15));
// // // // 			float num29 = (float) ((num6 *  num16) - (num8 *  num14));
// // // // 			float num30 = (float) ((num6 *  num15) - (num7 *  num14));
// // // // 			float num31 = (float) ((num5 *  num16) - (num8 *  num13));
// // // // 			float num32 = (float) ((num5 *  num15) - (num7 *  num13));
// // // // 			float num33 = (float) ((num5 *  num14) - (num6 *  num13));
// // // // 			result.Right.Z = (float) ((num2 *  num28) - (num3 *  num29) + (num4 *  num30)) * num27;
// // // // 			result.Up.Z = (float) -((num1 *  num28) - (num3 *  num31) + (num4 *  num32)) * num27;
// // // // 			result.Forward.Z = (float) ((num1 *  num29) - (num2 *  num31) + (num4 *  num33)) * num27;
// // // // 			result.Translation.Z = (float) -((num1 *  num30) - (num2 *  num32) + (num3 *  num33)) * num27;
// // // // 			float num34 = (float) ((num7 *  num12) - (num8 *  num11));
// // // // 			float num35 = (float) ((num6 *  num12) - (num8 *  num10));
// // // // 			float num36 = (float) ((num6 *  num11) - (num7 *  num10));
// // // // 			float num37 = (float) ((num5 *  num12) - (num8 *  num9));
// // // // 			float num38 = (float) ((num5 *  num11) - (num7 *  num9));
// // // // 			float num39 = (float) ((num5 *  num10) - (num6 *  num9));
// // // // 			result.Right.W = (float) -((num2 *  num34) - (num3 *  num35) + (num4 *  num36)) * num27;
// // // // 			result.Up.W = (float) ((num1 *  num34) - (num3 *  num37) + (num4 *  num38)) * num27;
// // // // 			result.Forward.W = (float) -((num1 *  num35) - (num2 *  num37) + (num4 *  num39)) * num27;
// // // // 			result.Translation.W = (float) ((num1 *  num36) - (num2 *  num38) + (num3 *  num39)) * num27;
// // // //         }
// // // //         /// <summary>
// // // //         /// Create a matrix world with all element position, target,Up
// // // //         /// </summary>
// // // //         /// <param name="result"></param>
// // // //         /// <param name="position"></param>
// // // //         /// <param name="forward"></param>
// // // //         /// <param name="up"></param>
// // // //         public static void CreateWorld( ref Matrix result,ref Vector4 position,ref  Vector4 forward, ref Vector4 up)
// // // //         {
// // // //             Vector4 z = new(forward);
// // // //             z.Normalize();
// // // //             Vector4 x = Vector4.Cross(ref forward,ref up);
// // // //             Vector4 y = Vector4.Cross(ref x,ref forward);
// // // //             x.Normalize();
// // // //             y.Normalize();

// // // //             result.Identity();
// // // //             result.Right = x;
// // // //             result.Up=y;
// // // //             result.Translation =position;
// // // //             result.Forward= z;
// // // //             result.Translation.W = 1.0f;
// // // //         }

// // // //         public bool Equals(Matrix other) => false;
// // // //     }
// // // // }








// // //         // public static void Transform(ref Vector3 result,ref Vector3 value, ref Matrix matrix)
// // //         //     => (result.X, result.Y , result.Z, result.W) =
// // //         //     ((value.X * matrix.Right.X) + (value.Y * matrix.Up.X) + (value.Z * matrix.Forward.X) + (value.W * matrix.Translation.X),
// // //         //     (value.X * matrix.Right.Y) + (value.Y * matrix.Up.Y) + (value.Z * matrix.Forward.Y) + (value.W * matrix.Translation.Y),
// // //         //     (value.X * matrix.Right.Z) + (value.Y * matrix.Up.Z) + (value.Z * matrix.Forward.Z) + (value.W * matrix.Translation.Z),
// // //         //     (value.X * matrix.Right.W) + (value.Y * matrix.Up.W) + (value.Z * matrix.Forward.W) + (value.W * matrix.Translation.W));


// // // //         /// <summary>
// // // //         /// Creates a new <see cref="Vector3"/> that contains a transformation of 3d-vector by the specified <see cref="Matrix"/>.
// // // //         /// </summary>
// // // //         /// <param name="position">Source <see cref="Vector3"/>.</param>
// // // //         /// <param name="matrix">The transformation <see cref="Matrix"/>.</param>
// // // //         /// <param name="result">Transformed <see cref="Vector3"/> as an output parameter.</param>
// // // //         // public static void Transform(ref Vector3 position, ref Matrix4 matrix, ref Vector3 result)
// // // //         // {
// // // //         //     var x = (position.X * matrix.Row1[1]) + (position.Y * matrix.Row2[1]) + (position.Z * matrix.Row3[1]) + matrix.Row4[1];
// // // //         //     var y = (position.X * matrix.Row1[2]) + (position.Y * matrix.Row2[2]) + (position.Z * matrix.Row3[2]) + matrix.Row4[2];
// // // //         //     var z = (position.X * matrix.Row1[3]) + (position.Y * matrix.Row2[3]) + (position.Z * matrix.Row3[3]) + matrix.Row4[3];
// // // //         //     result.X = x;
// // // //         //     result.Y = y;
// // // //         //     result.Z = z;
// // // //         // }
