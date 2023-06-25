namespace RitaEngine.Base.Graphic;

using RitaEngine.Base.Math;
using RitaEngine.Base.Platform;

public struct Camera :IEquatable<Camera>
{
    Matrix World = Matrix.Identity;
    Matrix View=Matrix.Identity;
    Matrix Projection=Matrix.Identity;

    /// <summary>
    /// Called too Position
    /// </summary>
    /// <returns></returns>
    public Vector3 _position =new(0.0f,-0.12f,-2.0f);
    private Vector3 _rotation = new(0.0f, 45.0f, 00.0f);
    public Vector3 Target =new(0.00f,0.00f,0.00f);
    public Vector3 Up =new(0.0f,1.0f,0.0f);
    public float FieldOfViewInDegree = 45.0f;
    private CameraType _type = CameraType.LookAt;


    public Camera() { }

    public void AddLookAkCamera(Vector3 position,Vector3 rotation, Vector3 up , float fov, float ratio , float near , float far )
    {
        _type = CameraType.LookAt;
        World =  RitaEngine.Base.Math.Matrix.Identity;
        Matrix.CreatePerspectiveFieldOfView( Helper.ToRadians( FieldOfViewInDegree) ,(1280.0f/720.0f), 0.1f,100.0f,out Projection );
        Projection.M22 *= -1;
        UpdateViewMatrix();
    }

    public void AddFirstPersonCamera( Vector3 position,Vector3 target, Vector3 up , float fov, float ratio , float near , float far)
    {
        _type = CameraType.FirstPerson;
        World =  RitaEngine.Base.Math.Matrix.Identity;
        Matrix.CreateLookAt( ref _position ,ref Target, ref Up, out View);
        Matrix.CreatePerspectiveFieldOfView( Helper.ToRadians( FieldOfViewInDegree) ,(1280.0f/720.0f), 0.1f,100.0f,out Projection );
        Projection.M22 *= -1;
        //https://computergraphics.stackexchange.com/questions/12448/vulkan-perspective-matrix-vs-opengl-perspective-matrix
        // Matrix.MakeProjectionMatrixWithoutFlipYAxis( Helper.ToRadians( FieldOfViewInDegree) ,(1280.0f/720.0f), 0.1f,100.0f,out Projection );
    }

    /// <summary>
    ///  Update Camera per Frame 
    /// </summary>
    /// <param name="DeltaTime"></param>
    public void Transform(double DeltaTime)
    {
        //Rotation
        //Translation
        //Scale
    }


    public void Update(float deltaTime)
    {
        _ = _type switch{
            CameraType.LookAt => UpdateViewMatrix(),
            CameraType.FirstPerson => UpdateFirstPerson(),
            _=> UpdateViewMatrix()
        };
    }

    private void UpdateViewMatrix()
    {
        Matrix rotM = Matrix.Identity;
        Matrix transM;
        
        rotM = Matrix.RotationX(Math.Helper.ToRadians(_rotation.X*-1.0f)) * rotM;
        rotM = Matrix.RotationY(Math.Helper.ToRadians(_rotation.Y)) * rotM;
        rotM = Matrix.RotationZ(Math.Helper.ToRadians(_rotation.Z)) * rotM;
        // rotM = Matrix.RotationAxis(_rotation, 1.0f);

       Vector3 translation = _position;
        translation.Y *= -1.0f;
        transM = Matrix.Translation(translation);

        // View = rotM * transM;
        View =  transM * rotM;
    }

    private void UpdateFirstPerson(){}

    public void Translate(float x , float y , float z)
    {
        _position.X += x;  _position.Y += y;  _position.Z += z;
    }
    public void Translate(Vector3 delta)
    {
        _position += delta;
    UpdateViewMatrix();
    }

    // public float[] ToArray => new float[ ]
    // {
    //     World[0],World[1],World[2],World[3],World[4],World[5],World[6],World[7],World[8],World[9],World[10],World[11],World[12],World[13],World[14],World[15],
    //     View[0],View[1],View[2],View[3],View[4],View[5],View[6],View[7],View[8],View[9],View[10],View[11],View[12],View[13],View[14],View[15],
    //     Projection[0],Projection[1],Projection[2],Projection[3],Projection[4],Projection[5],Projection[6],Projection[7],Projection[8],Projection[9],Projection[10],Projection[11],Projection[12],Projection[13],Projection[14],Projection[15],
    // };

    public float[] ToArray()
    {
        float[] result = {
            World[0],World[1],World[2],World[3],World[4],World[5],World[6],World[7],World[8],World[9],World[10],World[11],World[12],World[13],World[14],World[15],
        View[0],View[1],View[2],View[3],View[4],View[5],View[6],View[7],View[8],View[9],View[10],View[11],View[12],View[13],View[14],View[15],
        Projection[0],Projection[1],Projection[2],Projection[3],Projection[4],Projection[5],Projection[6],Projection[7],Projection[8],Projection[9],Projection[10],Projection[11],Projection[12],Projection[13],Projection[14],Projection[15],
        };
        return result;
    }

    public void Release()
    {
        
    }

    #region OVERRIDE    
    public override string ToString() => string.Format($"Camera Manager? " );
    public override int GetHashCode() => HashCode.Combine(  _position, Target );
    public override bool Equals(object? obj) => obj is Camera  camera && this.Equals(camera) ;
    public bool Equals(Camera other)=>  _position.Equals(other._position) ;
    public static bool operator ==(Camera  left,Camera right) => left.Equals(right);
    public static bool operator !=(Camera  left,Camera right) => !left.Equals(right);
    #endregion
}





// // namespace MCJ.Engine.Math
// // {
// //     using System;
// //     using System.Runtime.InteropServices;
// //     using System.Runtime.CompilerServices;
// //     using System.Diagnostics.CodeAnalysis;

// //     /// <summary>
// //     /// Caméra
// //     /// Utilisation :
// //     ///     Camera cam = new(0); // 0  obligatoire sinon error
// //     ///     cam.Update() ; a chaque frame 
// //     ///  Propriete pour config :
// //     ///     _position
// //     ///     
// //     /// </summary>
// //     [StructLayout(LayoutKind.Sequential, Pack = 4)]
// //     [SkipLocalsInit]
// //     internal struct Camera : IEquatable<Camera> , IDisposable
// //     {
// // #region DEFAULT VALUE (CONSTANTE)
// //         private static readonly Vector3 POSITION_DEFAULT = new Vector3(0.0f,0.0f,5.0f);
// //         private static readonly Vector3 TARGET_DEFAULT = new Vector3(0.0f,0.0f,-1.0f);
// //         private static readonly Vector3 UP_DEFAULT = new Vector3(0.0f,1.0f,0.0f);
// //         private static readonly float YAW_DEFAULT = -90.0f;
// //         private static readonly float FOV_DEFAULT = 45.0f; // in degree
// // #endregion
// // #region ATTRIBUTS
// //         private Matrix _view;
// //         private Matrix _projection;
// //         private Vector3 _cameraPosition;
// //         private Vector3 _cameraFront;
// //         private Vector3 _cameraDirection;
// //         private Vector3 _cameraUp;
// //         private Vector3 _up;
// //         private Vector3 _cameraRight;

// //         private float _yaw   ;	// yaw is initialized to -90.0 degrees since a yaw of 0.0 results in a direction vector pointing to the right so we initially rotate a bit to the left.
// //         private float _pitch ;
// //         private float _roll;
// //         private bool _firstmovearound ;
// //         private float _lastX;
// //         private float _lastY;
// //         private float _zoom;

// // #endregion       
// //         /// <summary>
// //         /// initialisation par default de la camera
// //         /// </summary>
// //         /// <param name="numero"> numero de la camera créer ( gestion de plusieurs caméra </param>
// //         internal Camera(int numero )
// //         {   
// //             _view = new(1.0f);
// //             _projection = new(1.0f);
// //             _cameraPosition = POSITION_DEFAULT;
// //             _cameraFront = TARGET_DEFAULT;
// //             _cameraDirection =  TARGET_DEFAULT;
// //             _up = UP_DEFAULT;
// //             _cameraUp = UP_DEFAULT;
// //             _cameraRight = new(0.0f);
// //             _yaw = YAW_DEFAULT;
// //             _pitch = 0.0f;
// //             _roll=0.0f;
// //             _firstmovearound=true ;
// //             _lastX = 720.0f;
// //             _lastY = 320.0f;
// //             _zoom = FOV_DEFAULT;
// //             _lookatChange = true;
// //             _positionChange = true;
// //         }

// //         /// <summary>
// //         /// 
// //         /// </summary>
// //         /// <param name="viewport"> x, y , width, height / ou  left top right bottom </param>
// //         /// <param name="projectionType"></param>
// //         /// <param name="fovdegree"></param>
// //         /// <param name="zNear"></param>
// //         /// <param name="zFar"></param>
// //         internal void SetProjection( Vector4 viewport, int projectionType =0,float fovdegree =45.0f,float zNear=0.1f,float zFar=100.0f )
// //         {
// //             _projection = projectionType switch
// //             {
// //                 0=> Matrix.Perspective( fovdegree,viewport[2],viewport[3] ,zNear,zFar ),
// //                 1=> Matrix.PerspectiveFOV( fovdegree, viewport[2],viewport[3] ,zNear,zFar ),
// //                 3=>Matrix.Ortho(viewport[0],viewport[3],viewport[1],viewport[2],zNear,zFar),
// //                 4=>Matrix.Ortho2D(viewport[0],viewport[2],viewport[3],viewport[1]),
// //                 5=>Matrix.TweakedInfinitePerspective( Maths.DegToRad(fovdegree), viewport[2] / viewport[3], zNear ),
// //                 6=> Matrix.Frustum(viewport[0],viewport[2],viewport[3],viewport[1], zNear,zFar),
// //                 7=> Matrix.InfinitePerspective( Maths.DegToRad(fovdegree), viewport[2] / viewport[3],zNear),
// //                 _=> throw new Exception("Wrong value in Camera Setprojection")
// //             };
// //         }

// //         private bool _positionChange;
// //         private bool _lookatChange;

// //         /// <summary>
// //         /// Mise a jour du positionement de la direction de la caméra a faire à chaque frame
// //         /// </summary>
// //         internal void Update( )
// //         {
// //             if ( _positionChange )
// //                 UpdatePosition();
// //             if ( _lookatChange )
// //                 UpdateLookAt();
            
// //             _positionChange = !_positionChange;
// //             _lookatChange = !_lookatChange;
// //         }

// //         private void UpdatePosition()
// //         {
// //             // FOr yaw and pitch 
// //             _cameraFront = Vector3.Normalize( Vector3.StackAlloc(   
// //                 _cameraDirection.X * Maths.Cos( Maths.DegToRad(_yaw)) * Maths.Cos( Maths.DegToRad(_pitch) ) , 
// //                 _cameraDirection.Y * Maths.Sin( Maths.DegToRad(_pitch) ) , 
// //                 _cameraDirection.Z * Maths.Sin( Maths.DegToRad(_yaw)) *  Maths.Cos( Maths.DegToRad(_pitch))
// //             )); 
        
// //             // for ROLL
// //              _cameraRight = Vector3.Normalize(
// //                 ( Vector3.Cross( ref _cameraFront,ref _up) * Maths.Cos(_roll * Maths.PIOVER180)) 
// //                 + ( _up * Maths.Sin(_roll * Maths.PIOVER180))
// //             );
// //             // recaluculer sens camera 
// //             _cameraUp = Vector3.Normalize(  Vector3.Cross( ref _cameraRight,ref _cameraFront)   );
// //         }

// //         private void UpdateLookAt()
// //         {
// //             _view = Matrix.LookAt( _cameraPosition  ,_cameraPosition + _cameraFront, _cameraUp); 
// //         }

// //         /// <summary>
// //         /// make sure the user stays at the ground level
// //         /// POur les FPS  la caméra reste au niveau du sol 
// //         /// execute befor update
// //         /// </summary>
// //         /// <param name="groundposition">0.0f by default </param>
// //         internal void StayAtGroundXZPlane( float groundposition=0.0f)
// //         {
// //             _cameraPosition.Y = groundposition  ; // <-- this one-liner keeps the user at the ground level (xz plane)
// //         }

// // #region MOVE

// //         /// <summary>
// //         /// Regarder autour généralement a la souris ne bouge pas inclinaison de la caméra vers haut/bas ou gaauche droite
// //         /// </summary>
// //         /// <param name="xpos"></param>
// //         /// <param name="ypos"></param>
// //         /// <param name="sensitivity"></param>
// //         internal void LookAround(float xpos, float ypos , float sensitivity = 0.1f)
// //         {
// //             if (_firstmovearound)
// //             {
// //                 _lastX = xpos;
// //                 _lastY = ypos;
// //                 _firstmovearound = false;
// //             }

// //             float xoffset = xpos - _lastX;
// //             float yoffset = _lastY - ypos; // reversed since y-coordinates go from bottom to top
// //             _lastX = xpos;
// //             _lastY = ypos;

// //             // change this value to your liking
// //             xoffset *= sensitivity;
// //             yoffset *= sensitivity;

// //             _yaw += xoffset;
// //             _pitch += yoffset;
// //             Maths.Clampfref(ref _pitch, -89.0f,89.0f);
// //             _lookatChange = true;
// //             _positionChange = true;
// //         }

// //         /// <summary>
// //         /// // x movement  LEFT => negatif RIGHT => positif
// //         /// </summary>
// //         /// <param name="distance"></param>
// //         internal void Strafe(float distance)
// //         {
// //             _cameraPosition -= (_cameraRight * distance);
// //             _lookatChange = true;
// //             _positionChange = true;
// //         }
            
// //         /// <summary>
// //         /// // y movement UP DOWN ( for jump )
// //         /// </summary>
// //         /// <param name="distance"></param>
// //         internal void Ascend(float distance)
// //         {
// //             _cameraPosition += _cameraUp * distance;
// //             _lookatChange = true;
// //             _positionChange = true;
// //         }
            

// //         /// <summary>
// //         ///z movement FORWARD => positif distance BACKWARD => negatif distance
// //         /// </summary>
// //         /// <param name="distance"></param>
// //         internal void Advance(float distance)
// //         {
// //              // => camera.Position += camera.Direction * -distance;
// //             _cameraPosition += (distance * _cameraFront) ;
// //             _lookatChange = true;
// //             _positionChange = true;
// //         }
           
// //          /// <summary>
// //         /// Rotate around Y axis with angle in radians
// //         /// </summary>
// //         /// <param name="angle">angle in radians</param>
// //         internal void Yaw(float angle)
// //         {
// //             _yaw +=Maths.ClampFloat(angle,0,Maths.PI);
// //             _cameraDirection.X -= Maths.Cos( Maths.DegToRad(_yaw)) * Maths.Cos( Maths.DegToRad(_pitch) );

// //             _lookatChange = true;
// //             _positionChange = true;
// //         }

// //         /// <summary>
// //         /// Rotate around  Z Axis
// //         /// </summary>
// //         /// <param name="angle">angle in radians</param>
// //         internal void Roll( float angle)
// //         {
// //             _roll += Maths.ClampFloat(angle,0,Maths.PI);
// //             // _cameraDirection.Z -= Maths.Sin( Maths.DegToRad(_yaw)) *  Maths.Cos( Maths.DegToRad(_pitch));
// //             //    _cameraRight = Vector3.Normalize(
// //             //     ( Vector3.Cross( ref _cameraFront,ref _up) * Maths.Cos(_roll * Maths.PIOVER180)) 
// //             //     + ( _up * Maths.Sin(_roll * Maths.PIOVER180))
// //             // );
// //             _lookatChange = true;
// //             _positionChange = true;
// //         }

// //         /// <summary>
// //         /// Rotate around X axis
// //         /// </summary>
// //         /// <param name="angle">in radians</param>
// //         internal void Pitch(float angle)
// //         {
// //             _pitch += Maths.ClampFloat(angle,0,Maths.PI);
// //             _cameraDirection.Y -= Maths.Sin( Maths.DegToRad(_pitch) ) ;
// //             _lookatChange = true;
// //             _positionChange = true;
// //         }
            

// //         /// <summary>
// //         /// Angle de vision 
// //         /// </summary>
// //         /// <param name="fov">angle en degree de 1° a 90°  default is 45°</param>
// //         internal void Zoom( float fov )
// //         {
// //             _zoom += Maths.DegToRad( Maths.ClampFloat( fov , 1.0f, 90.0f ) ) ;
            
// //         }
// // #endregion
        
// //         /// <summary>
// //         /// https://github.com/raysan5/raylib/blob/master/src/rcore.c ligne : 
// //         ///  retrouve les coordonnée ecran x,y de la postion de l'objet 3D 
// //         /// Unproject in GLM
// //         /// </summary>
// //         /// <param name="position"></param>
// //         /// <param name="camera"></param>
// //         /// <param name="width"></param>
// //         /// <param name="height"></param>
// //         /// <returns></returns>
// //         internal static Vector2 WorldToScreen(Vector3 position, Camera camera, float width, float height)
// //         {
// //             Vector4 worldPos = Matrix.Vector4Transform( new Vector4(position.X, position.Y, position.Z, 1.0f) , camera.View);
// //             worldPos = Matrix.Vector4Transform( worldPos , camera.Projection);
// //             Vector3 ndcPos = new Vector3(worldPos.X/worldPos.W, -worldPos.Y/worldPos.W, worldPos.Z/worldPos.W);

// //             return new Vector2( (ndcPos.X + 1.0f)/2.0f* width  , (ndcPos.Y + 1.0f)/2.0f* height  );
// //         }
// //         /// <summary>
// //         /// TODO A faire
// //         /// </summary>
// //         /// <param name="windowPosition"></param>
// //         /// <param name="camera"></param>
// //         /// <param name="viewport"></param>
// //         /// <returns></returns>
// //         internal static Vector3 ScreenToWorld(Vector2 windowPosition, Camera camera,Vector4 viewport)
// //         {
// //             // Store values in a vector (normalize )
// //             Vector3 deviceCoords = new( (2.0f*windowPosition.X)/viewport[2] - 1.0f , 
// //                                     1.0f - (2.0f*windowPosition.Y)/viewport[3]  ,
// //                                     1.0f);
// //              // Unproject far/near points
// //             deviceCoords.Z =0.10f;
// //             Vector3 nearPoint = Matrix.UnProject(deviceCoords, camera.View, camera.Projection,viewport);
// //             deviceCoords.Z = 100.0f;
// //             Vector3 farPoint = Matrix.UnProject(deviceCoords, camera.View, camera.Projection,viewport);

// //             // Unproject the mouse cursor in the near plane.
// //             // We need this as the source position because orthographic projects, compared to perspect doesn't have a
// //             // convergence point, meaning that the "eye" of the camera is more like a plane than a point.
// //             deviceCoords.Z =-1.0f;
// //             Vector3 cameraPlanePointerPos = Matrix.UnProject(deviceCoords, camera.View, camera.Projection,viewport);

// //             // Calculate normalized direction vector
// //             Vector3 direction = Vector3.Normalize(farPoint- nearPoint) ;


// //             return cameraPlanePointerPos;
// //         }

// //         private static float ax =0.0f;
// //         private static float ay = 0.0f;
        
// //         /// <summary>
// //         /// 
// //         /// </summary>
// //         /// <param name="position"></param>
// //         /// <param name="target"></param>
// //         /// <param name="x"></param>
// //         /// <param name="y"></param>
// //         internal void AroundTarget( Vector3 position, Vector3 target , float x, float y)
// //         {
// //             ax += x;
// //             ay += y;
// //             float radius = Vector3.Distance(ref position,ref target);//X = radius

// //             if ( position.X == 0.0f) position.X += Maths.Epsilon;
            
// //             float phi = Maths.ATan2(position.Z/position.X, position.X);//Y = polar (azimut ) horizontal angle            
// //             float theta = Maths.ACos(position.Y / radius);// elevation vertical angle
            
// //             if (position.X < 0) phi += Maths.PI;
            
// //             phi += ax;
// //             theta += ay;
            
// //             Maths.Clampfref(ref  phi , 0.0f, Maths.PI);
// //             // calcul coordonee cartesienne avec changment d'angle 
// //             Vector3 result = new(0.0f);
// //             result.X = (radius * Maths.Sin(phi) * Maths.Cos(theta)); //;+ target.X;
// //             result.Y = (radius * Maths.Cos(phi)) ;//;+ target.Y;
// //             result.Z = (radius * Maths.Sin(phi) * Maths.Sin(theta)); //;+ target.Z         
// //             System.Console.WriteLine($"Angle phi {phi} Theta {theta} Position {result} Result{_cameraDirection}");
// //             // _cameraDirection = target - _cameraDirection ; 
            

// //         }

// // #region ACCESSOR
// //         /// <summary>
// //         /// A faire a chaque frame pour le calcul du fustrum
// //         /// réalise l'operation projection * view
// //         /// </summary>
// //         internal Matrix ClipPlanes =>  _projection * _view;

// //         /// <summary>
// //         /// projection matrice
// //         /// </summary>
// //         internal Matrix Projection => _projection;
// //         /// <summary>
// //         /// matrice vue
// //         /// </summary>
// //         internal Matrix View => _view;

// //         /// <summary> For uniform </summary>
// //         internal  float[] ToViewArray =>  _view.ToArray;
// //         /// <summary> For uniform </summary>
// //         internal float[] ToProjectionArray => _projection.ToArray;
// //         /// <summary> Position de la camera </summary>
// //         internal Vector3 Position  {get =>  _cameraPosition;  set => _cameraPosition =value;}
// //          /// <summary>  Direction de la camera matrice View  </summary>
// //         internal Vector4 CameraTranslation => _view[3];
// //         /// <summary>  Direction de la camera matrice View  </summary>
// //         internal Vector4 CameraRight => _view[0];
// //         /// <summary>  Direction de la camera matrice View  </summary>
// //         internal Vector4 CameraUp => _view[1];
// //         /// <summary>  Direction de la camera matrice View  </summary>
// //         internal Vector4 CameraForward => _view[2];// TODO voir remplacer par _cameraDirection = _cameraPosition + _cameraFront à normalizer
// //         /// <summary>  Direction   de la camera ( n'est pas le FRONT ou Target) </summary>
// //         internal Vector3 Target {get => _cameraDirection ; set => _cameraDirection = value;}
// //         /// <summary> mettre y à -1.0f pour avoir la tête en base  </summary>
// //         internal Vector3 Up {get => _up ; set => _up = value;}
// // #endregion
// // #region OVERRIDE
// //         /// <summary>
// //         /// Egalité entre deux caméra
// //         /// </summary>
// //         /// <param name="other"></param>
// //         /// <returns></returns>
// //         internal bool Equals(Camera other) => false;
// //         /// <summary>
// //         /// 
// //         /// </summary>
// //         internal void Dispose()
// //         {
// //             _view.Dispose();
// //             _projection.Dispose();
// //             _cameraPosition.Dispose();
// //             _cameraFront.Dispose();
// //             _cameraDirection.Dispose();
// //             _cameraUp.Dispose();
// //             _up.Dispose();
// //             _cameraRight.Dispose();
        
// //             #pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
// //             GC.SuppressFinalize(this);
// //             #pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
// //         }
// // #endregion    
// //     }
// // }
// // /*
// //  ██████  ██      ██████  
// // ██    ██ ██      ██   ██ 
// // ██    ██ ██      ██   ██ 
// // ██    ██ ██      ██   ██ 
// //  ██████  ███████ ██████  
                         

// // /// <summary>
// //         /// Mise a jour du positionement de la direction de la caméra a faire à chaque frame
// //         /// </summary>
// //         /// <param name="stayatgroundlevel"> POur les FPS  la caméra reste au niveau du sol </param>
// //         internal void Update(float stayatgroundlevel =0.0f , bool withlookatcalcul =true )
// //         {
// //             // / <param name="withlookatcalcul"> Pour element 2D mettre a false ( ne calcul pas me LookAt et renvoi matrice identité</param>
// //             // Vector3 front = new Vector3( 
// //             //     Maths.Cos( Maths.DegToRad(_yaw)) * Maths.Cos( Maths.DegToRad(_pitch) ) , 
// //             //     Maths.Sin( Maths.DegToRad(_pitch) ) , 
// //             //     Maths.Sin( Maths.DegToRad(_yaw)) *  Maths.Cos( Maths.DegToRad(_pitch))   );
// //             _cameraFront = Vector3.Normalize( Vector3.StackAlloc(   
// //                 Maths.Cos( Maths.DegToRad(_yaw)) * Maths.Cos( Maths.DegToRad(_pitch) ) , 
// //                 Maths.Sin( Maths.DegToRad(_pitch) ) , 
// //                 Maths.Sin( Maths.DegToRad(_yaw)) *  Maths.Cos( Maths.DegToRad(_pitch))
// //             )); 
            
// //             // _cameraRight = Vector3.Normalize ( Vector3.Cross( ref _cameraFront,ref _up));//old yaw and pitch now inside roll
// //             // _cameraUp = Vector3.Normalize(  Vector3.Cross( ref _cameraRight,ref _cameraFront)   );// after roll           
// //             //ROLL
// //              _cameraRight = Vector3.Normalize(
// //                 ( _cameraRightVector3.Cross( ref _cameraFront,ref _up) * Maths.Cos(_roll * Maths.PIOVER180)) +
// //                 (_up* Maths.Sin(_roll * Maths.PIOVER180))
// //             );
// //             _cameraUp = Vector3.Normalize(  Vector3.Cross( ref _cameraRight,ref _cameraFront)   );

// //             // make sure the user stays at the ground level
// //             _cameraPosition.Y = stayatgroundlevel == 0.0f ? 0.0f : _cameraPosition.Y ; // <-- this one-liner keeps the user at the ground level (xz plane)
            
// //             _cameraDirection = !_orbit? _cameraPosition + _cameraFront : _target;
// //             _view = Matrix.LookAt( _cameraPosition , _cameraDirection , _cameraUp); 
// //             //_orbit = false;
// //         }                         






// // */
// // // /// <summary>
// //         // /// Constructeur complet 
// //         // /// </summary>
// //         // /// <param name="position"></param>
// //         // /// <param name="target"></param>
// //         // /// <param name="up"></param>
// //         // internal Camera(Vector3 position = default , Vector3 target =default , Vector3 up = default)
// //         // {
// //         //     _view = new(1.0f);
// //         //     _projection = new(1.0f);
// //         //     _cameraPosition = position;
// //         //     _cameraFront = target;
// //         //     _cameraDirection = Vector3.Normalize( _cameraPosition - _cameraFront);
// //         //     _up = up;
// //         //     _cameraRight = Vector3.Normalize( Vector3.Cross(ref _up ,ref _cameraDirection  ));
// //         //     _cameraUp = Vector3.Cross(ref _cameraDirection, ref _cameraRight); 

// // // namespace MCJLib.Game.Core.Maths
// // // {
// // //     using System.Runtime.CompilerServices;
// // //     using System.Runtime.InteropServices;
// // //     using System;

// // //     [StructLayout(LayoutKind.Sequential, Pack = 8)]
// // //     internal struct Camera : IEquatable<Camera>
// // //     {
// // //         internal Matrix View ;
// // //         internal Matrix Projection ;

// // //         /// <summary>
// // //         /// = projection * view stored calcul in camera update
// // //         /// Pour calculer a chaque frame le clip Space
// // //         /// </summary>
// // //         internal Matrix Clip ;

// // //         /// <summary>
// // //         /// 6 plane saved stored clip space calculer a partir du clip matrix
// // //         /// voir la fonction in CLipSpace class
// // //         /// </summary>
// // //         // internal ClipPlanes Frustum;
// // //         internal Vector4 CamPosition ;//eye 
// // //         internal Vector4 CamDirection ;//direction ( target )
// // //         internal Vector4 CamRight ;// Strafe
// // //         internal Vector4 CamUp ;// head up camera  if y = -1.0f see tete en bas

// // //         internal Vector4 Up;
// // //         internal Vector4 Front;
// // //         internal Vector4 Axis;
// // //         internal Matrix ExpView;

// // //         internal bool Equals(Camera other)
// // //         {
// // //             throw new NotImplementedException();
// // //         }
// // //     }

// // //     internal static class Cameras
// // //     {
// // //         internal static void Init( ref Camera camera, Vector4 position, Vector4 target , Vector4 up)
// // //         {
// // //             camera.View.Identity();
// // //             camera.CamUp = up;
// // //             camera.CamPosition = position;
// // //             camera.CamDirection = target;
// // //             camera.CamRight = Vector4.Normalize( Vector4.Cross(ref camera.CamUp,ref camera.CamDirection));
// // //             camera.Axis = new(0.0f, -Math.PI_OVER2, 0.0f);
// // //             camera.Up = up;

// // //             camera.ExpView = new(1.0f);
// // //             Update(ref camera);
// // //             camera.ExpView = camera.View;
// // //         }

// // //         internal static void WalkAround(ref Camera camera, float speedx, float speedy , float speedz)
// // //         {
// // //             camera.CamPosition += speedz * camera.CamDirection *2.5f;
// // //             camera.CamPosition += speedy * camera.Up;
// // //             camera.CamPosition += speedx * camera.CamRight;
// // //             // camera.Up = Vector4.UnitY;
// // //             camera.CamRight = Vector4.Normalize( Vector4.Cross(ref camera.Up,ref camera.CamDirection  ));
// // //         }

// // //         internal static void LookAround( ref Camera camera, float angleX, float angleY, float angleZ)
// // //         {
// // //             camera.Axis.X += angleX;//pitch
// // //             camera.Axis.Y += angleY;//yaw
// // //             camera.Axis.Z += angleZ;//roll

// // //             camera.Front.X = Math.Cos(camera.Axis.Y  ) * Math.Cos( camera.Axis.X) ;
// // //             camera.Front.Y = Math.Sin(camera.Axis.X);
// // //             camera.Front.Z = Math.Sin(camera.Axis.Y) * Math.Cos(camera.Axis.X);//camera.CamDirection;

// // //             camera.CamDirection = Vector4.Normalize(camera.Front);
// // //             camera.CamRight = Vector4.Normalize( Vector4.Cross(ref camera.CamDirection,ref camera.Up   ));
// // //         }

// // //         internal static void ExpRotate(ref Camera camera, float x , float y , float z)
// // //         {
// // //             Matrix world = new(1.0f);
// // //             Matrix.RotationY(ref world, y );
// // //             camera.ExpView *= world;
// // //             Matrix.RotationX(ref world, x );
// // //             camera.ExpView *= world;
// // //             Matrix.RotationZ(ref world , z);
// // //             camera.ExpView *= world;
// // //         }

// // //         internal static void ExpMove(ref Camera camera, float x , float y , float z)
// // //         {
// // //             camera.ExpView.Translation.X += x;
// // //             camera.ExpView.Translation.Y += y;
// // //             camera.ExpView.Translation.Z += z;
// // //         }

// // //         private static float ax =0.0f;
// // //         private static float ay = 0.0f;
// // //         internal static void AroundTarget( ref Camera camera , ref Vector4 target , float x, float y)
// // //         {
// // //             ax += x;
// // //             ay += y;
// // //             float radius = Vector4.Distance(ref camera.CamPosition,ref target);//X = radius

// // //             if ( camera.CamPosition.X == 0.0f)
// // //                 camera.CamPosition.X = Math.EPSILON;
// // //             float phi = Math.ATan2(camera.CamPosition.Z/camera.CamPosition.X, camera.CamPosition.X);//Y = polar (azimut ) horizontal angle            
// // //             float theta = Math.ACos(camera.CamPosition.Y / radius);// elevation vertical angle

// // //             // if (camera.CamPosition.X < 0) phi += Math.PI;
// // //             phi += ax;
// // //             theta += ay;
// // //             // Math.Clampfr(ref theta, Math.EPSILON, Math.PI);
// // //             // Math.Clampfr(ref phi, Math.EPSILON, Math.TWOPI);

// // //             var sinustheta = radius * Math.Sin(theta);
// // //             // calcul coordonee cartesienne avec changment d'angle 
// // //             Vector4 position = new(0.0f);
// // //             position.Z = ( sinustheta * Math.Cos(phi)) ;//+ target.Z;
// // //             position.X = ( sinustheta * Math.Sin(phi)) ;//+ target.X;
// // //             position.Y = (radius * Math.Cos(theta)) ;//+ target.Y;
// // //             camera.CamPosition = position + target ;
// // //             camera.CamDirection =position- target;
// // //             camera.CamDirection.Negative();
// // //             camera.CamRight = Vector4.Normalize( Vector4.Cross(ref camera.CamDirection,ref camera.Up   ));
// // //         }
// // //         internal static void SetProjection( ref Camera camera,in float width, in float height, in float near = 0.1f, in float far = 1000.0f, in float FOV = 45.0f)
// // //         {
// // //             camera.Projection.Identity();
// // //             Maths.Projection.PerspectiveFOV_RHNO(ref camera.Projection, FOV, width, height, near, far);
// // //         }

// // //         /// <summary>
// // //         /// Place code in loop
// // //         /// Need to call each frame to calculate fustrum
// // //         /// </summary>
// // //         internal static void Update(ref Camera camera )
// // //         {
// // //             Maths.Projection.LookAtRH(ref camera.View,
// // //                 camera.CamPosition,// camera is here
// // //                 camera.CamPosition + camera.CamDirection, //// and looks here : at the same position, plus "direction"
// // //                 camera.CamUp);// Head is up (set to 0,-1,0 to look upside-down)

// // //             Matrix.MultiplyMat4byMat4(ref camera.Clip,ref camera.Projection,ref camera.View);
// // //             // ClipPlanes.ViewFustrum( ref camera.Frustum , ref camera.Clip);
// // //         }

// // //         internal static void ExpUpdate( ref Camera camera)
// // //         {
// // //             Matrix.MultiplyMat4byMat4(ref camera.Clip,ref camera.Projection,ref camera.ExpView);
// // //         }
// // //     }
// // // }

// // // /*
// // // OLD CAMERA OBSOLETE
// // //         internal static void LoadCamera( ref Camera camera, Vector4 position, Vector4 target , Vector4 Up)
// // //         {
// // //             camera.View.Identity();
// // //             camera.Frustum = new();
// // //             camera.Up = Up;
// // //             camera.Position = position;
// // //             camera.Direction = target;
// // //             camera.Right = Vector4.Normalize( Vector4.Cross(ref camera.Up,ref camera.Direction));

// // //             // camera.World =new(1.0f);
// // //             // Matrix.CreateWorld(ref camera.World, ref camera.Position, ref camera.Direction, ref camera.Up);
// // //             // camera.Velocity = new(0.0f);
// // //             // camera.Axis = new(0.0f);
// // //         }

// // //         internal static void SetProjection( ref Camera camera,in float width, in float height, in float near = 0.1f, in float far = 1000.0f, in float FOV = 45.0f)
// // //         {
// // //             camera.Projection.Identity();
// // //             Maths.Projection.PerspectiveFOV_RHNO(ref camera.Projection, FOV, width, height, near, far);
// // //             Update(ref camera);
// // //         }

        
// // //         internal static void Update(ref Camera camera )
// // //         {
// // //             // Matrix orientation= new(1.0f);


// // //             // camera.World.Identity();
// // //             // camera.Position += camera.Velocity;
// // //             // Matrix.Translate(ref camera.World, ref camera.Position);
// // //             // Matrix.RotationY(ref orientation, camera.Axis.Y );
// // //             // camera.World *=  orientation;
// // //             //             Matrix.Translate(ref orientation,ref camera.Position);
// // //             // Matrix.RotationZ(ref orientation, camera.Axis.Z );
// // //             // camera.World *=  orientation;
// // //             // Matrix.RotationX(ref orientation, camera.Axis.X );
// // //             // camera.World *=  orientation;
            
// // //             // camera.Axis =Vector4.Zero;
// // //             // camera.Velocity = Vector4.Zero;

// // //             // Vector4.Transform(ref camera.Position, ref camera.Position , ref camera.World);
// // //             // Vector4.Transform(ref camera.Direction, ref camera.Direction , ref camera.World);

// // //             Maths.Projection.LookAtRH(ref camera.View,
// // //                 camera.Position,// camera is here
// // //                 camera.Position + camera.Direction + direction , //// and looks here : at the same position, plus "direction"
// // //                 camera.Up);// Head is up (set to 0,-1,0 to look upside-down)

// // //             Matrix.MultiplyMat4byMat4(ref camera.Clip,ref camera.Projection,ref camera.View);
// // //             ClipPlanes.ViewFustrum( ref camera.Frustum , ref camera.Clip);
// // //         }

// // //         /// <summary>
// // //         /// Rotate around  Z Axis
// // //         /// </summary>
// // //         /// <param name="angle"></param>
// // //         internal static void Roll(ref Camera camera,in float angle)
// // //         {
// // //             // camera.roll += angle ;//RotateZ
// // //             camera.Right = Vector4.Normalize(
// // //                 (camera.Right * Math.Cos(angle * Math.PIOVER180)) +
// // //                 (camera.Up * Math.Sin(angle * Math.PIOVER180))
// // //             );

// // //             camera.Up = Vector4.Cross(ref camera.Direction,ref camera.Right);
// // //             camera.Up *= -1;
// // //         }

// // //         /// <summary>
// // //         /// Rotate around X axis
// // //         /// </summary>
// // //         /// <param name="angle"></param>
// // //         internal static void Pitch(ref Camera camera,in float angle)
// // //         {
// // //             // camera.pitch += angle  ;// rotateX
// // //             camera.Direction = Vector4.Normalize(
// // //                 (camera.Direction * Math.Cos(Math.DegToRad(angle))) +
// // //                 (camera.Up * Math.Sin(Math.DegToRad(angle)))
// // //             );

// // //             camera.Up = Vector4.Cross(ref camera.Direction,ref camera.Right);
// // //             camera.Up *= -1;
// // //         }

// // //         /// <summary>
// // //         /// Rotate around Y axis with angle in radians
// // //         /// </summary>
// // //         /// <param name="angle"></param>
// // //         internal static void Yaw(ref Camera camera,in float angle)
// // //         {
// // //             // angle = Math.Clampf(angle,0,Math.PI);
// // //             // camera.yaw += angle  ;//rotateY
// // //             // re-calculate the new forward vector
// // //             camera.Direction = Vector4.Normalize(
// // //                 (camera.Direction * Math.Cos(angle)) -
// // //                 (camera.Right * Math.Sin(angle))
// // //             );
// // //             // re-calculate the new right vector
// // //             camera.Right = Vector4.Cross(ref camera.Direction, ref camera.Up);
// // //         }

// // //         /// <summary>
// // //         /// // x movement  LEFT RIGHT
// // //         /// </summary>
// // //         /// <param name="distance"></param>
// // //         internal static void Strafe(ref Camera camera,in float distance)
// // //             => camera.Position += camera.Right * distance;

// // //         /// <summary>
// // //         /// // y movement UP DOWN ( for jump )
// // //         /// </summary>
// // //         /// <param name="distance"></param>
// // //         internal static void Ascend(ref Camera camera,in float distance)
// // //             => camera.Position += camera.Up * distance;

// // //         /// <summary>
// // //         ///z movement FORWARD BACKWARD
// // //         /// </summary>
// // //         /// <param name="distance"></param>
// // //         internal static void Advance(ref Camera camera,in float distance)
// // //             => camera.Position += camera.Direction * -distance;

// // //         /// <summary>
// // //         /// deplacement de n pixel pour chaque axe
// // //         /// x => strafe left right
// // //         /// y=> ascend jump up down
// // //         /// z => advance   ( forward, backward )
// // //         /// </summary>
// // //         /// <param name="velocity"></param>
// // //         internal static void Translate(ref Camera camera, Vector4 velocity)
// // //             => camera.Position += (camera.Direction * velocity.Z) + (camera.Up * velocity.Y) + (camera.Right * velocity.X);

// // //         internal static void RotateAroundTarget(ref Camera camera,ref Vector4 target, float angleX)
// // //         {
// // //             Matrix orientation = new(1.0f);
// // //             camera.World.Identity();
// // //             Matrix.Translate(ref orientation, ref target);
// // //             Matrix.RotationY(ref orientation, angleX);
// // //             orientation.Translation = Vector4.UnitW; //translate to origine of target .????
// // //             camera.View *= orientation;
// // //             // https://wiki.unity3d.com/index.php/MouseOrbitImproved
// // //             System.Console.WriteLine("World" + camera.View);
// // //             camera.Clip = camera.Projection * camera.View ;
// // //        }

// // //         internal static void Orbit(ref Camera camera, Vector4 target, float angleX , float angleY )
// // //         {
// // //             angle += angleX;
// // //             angley += angleY;
// // //             //https://nerdhut.de/2020/05/09/unity-arcball-camera-spherical-coordinates/
// // //             float radius = Vector4.Distance(ref camera.Position,ref target);//X = radius

// // //             float phi = Math.ATan2(camera.Position.Z/camera.Position.X, camera.Position.X);//Y = polar (azimut ) horizontal angle            
// // //             float theta = Math.ACos(camera.Position.Y / radius);// elevation vertical angle

// // //             phi += angle;
// // //             if (camera.Position.X < 0) phi += Math.PI;
// // //             theta += angley;

// // //             var sinustheta = radius * Math.Sin(theta);
// // //             // calcul coordonee cartesienne avec changment d'angle 
// // //             Vector4 position = new(0.0f);
// // //             position.Z = ( sinustheta * Math.Cos(phi)) ;//+ target.Z;
// // //             position.X = ( sinustheta * Math.Sin(phi)) ;//+ target.X;
// // //             position.Y = (radius * Math.Cos(theta)) ;//+ target.Y;
// // //             Vector4 direction = new(0.0f);
// // //             direction.Z =  sinustheta * Math.Cos(phi + Math.PI) ;
// // //             direction.X =  sinustheta * Math.Sin(phi + Math.PI) ;
// // //             direction.Y = (radius * Math.Cos(theta)) ;
// // //             camera.Position = position + target;
// // //             camera.Direction= direction ;
// // //             camera.Right = Vector4.Cross(ref camera.Direction,ref camera.Up);
// // //         }

// // //         private static float angle=0.0f;
// // //         private static float angley=0.0f;
// // //         internal static void Orbital(ref Camera camera, Vector4 target, float angleX )
// // //         {
// // //             angle += angleX;
// // //             //https://roy-t.nl/2010/02/21/xna-simple-arcballcamera.html
// // //             //Calculate the relative position of the camera                        
// // //             // position = Vector3.Transform(Vector3.Backward, Matrix.CreateFromYawPitchRoll(yaw, pitch, 0));
// // //             // //Convert the relative position to the absolute position
// // //             // position *= zoom;
// // //             // position += lookAt;
// // //             Matrix.CreateWorld(ref camera.World, ref camera.Position, ref camera.Direction, ref camera.Up);

// // //             // camera.Position = Vector3.Transform(cameraPosition - cameraTarget, Matrix.CreateFromAxisAngle(axis, angle)) + cameraTarget;
// // //             // view = CreateLookAt(cameraPosition, cameraTarget, cameraUp);
// // //             Vector4 newposition = camera.Position - target;

// // //             Matrix.Translate(ref camera.World, ref newposition);
// // //             Vector4 axis =  new(0.0f,1.0f,0.0f,1.0f);
// // //             Matrix.Rotate(ref camera.World, angle,ref axis);
// // //             Vector4.Transform(ref newposition , ref camera.Position, ref camera.World);
// // //             camera.Position = newposition;
// // //             camera.Direction = target;
// // //         }

// // //         /// <summary>
// // //         /// Center a camera to target object and backward of distance
// // //         /// https://stackoverflow.com/questions/10752435/how-do-i-make-a-camera-follow-an-object-in-unity3d-c
// // //         /// </summary>
// // //         /// <param name="camera">used Camera contains data of position</param>
// // //         /// <param name="target"></param>
// // //         internal static void CenterCameraToObject(ref Camera camera , ref Vector4 target, float radius=0.0f)
// // //         {
// // //             radius = radius==0.0f ?  Vector4.Distance(ref camera.Position, ref target): radius;
// // //             camera.Position = target - (radius * camera.Direction );
// // //         }

// // //         internal static void CreateMatrixWorld(ref Matrix result, ref Vector4 position, ref Vector4 forward, ref Vector4 up)
// // //         {
// // //             Vector4 x, y, z;
// // //             z = forward;
// // //             z.Normalize();
// // //             // z.Negative();
// // //             x = Vector4.Cross(ref forward,ref  up);
// // //             y = Vector4.Cross(ref x,ref forward);
// // //             x.Normalize();
// // //             y.Normalize();

// // //             result.Right = x;
// // //             result.Up = y;
// // //             result.Forward = z;
// // //             result.Translation = position;
// // //             result.Translation.W = 1f;
// // //         }

// // //         // internal static float SmoothDamp (float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
// // //         // {
// // //         //     // smoothTime = Mathf.Max (0.0001f, smoothTime);
// // //         //     float num = 2f / smoothTime;
// // //         //     float num2 = num * deltaTime;
// // //         //     float num3 = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
// // //         //     float num4 = current - target;
// // //         //     float num5 = target;
// // //         //     float num6 = maxSpeed * smoothTime;
// // //         //     num4 = Mathf.Clamp (num4, -num6, num6);
// // //         //     target = current - num4;
// // //         //     float num7 = (currentVelocity + num * num4) * deltaTime;
// // //         //     currentVelocity = (currentVelocity - num * num7) * num3;
// // //         //     float num8 = target + (num4 + num7) * num3;
// // //         //     if (num5 - current > 0f == num8 > num5)
// // //         //     {
// // //         //         num8 = num5;
// // //         //         currentVelocity = (num8 - num5) / deltaTime;
// // //         //     }
// // //         //     return num8;
// // //         // }

// // // */



// // // {
// // //     using System.Runtime.CompilerServices;
// // //     using System.Runtime.InteropServices;

// // //     [StructLayout(LayoutKind.Sequential, Pack = 4)]
// // //     internal struct Camera
// // //     {
// // //         internal Matrix View ;
// // //         internal Matrix Projection ;

// // //         /// <summary>
// // //         /// = projection * view stored calcul in camera update
// // //         /// Pour calculer a chaque frame le clip Space
// // //         /// </summary>
// // //         internal Matrix Clip ;

// // //         /// <summary>
// // //         /// 6 plane saved stored clip space calculer a partir du clip matrix
// // //         /// voir la fonction in CLipSpace class
// // //         /// </summary>
// // //         // internal ClipPlanes Frustum;
// // //         internal Vector4 CamPosition ;//eye 
// // //         internal Vector4 CamDirection ;//direction ( target )
// // //         internal Vector4 CamRight ;// Strafe
// // //         internal Vector4 CamUp ;// head up camera  if y = -1.0f see tete en bas

// // //         internal Vector4 Up;
// // //         internal Vector4 Front;
// // //         internal Vector4 Axis;

// // //         internal Matrix ExpView;
// // //     }

// // //     internal static class Cameras
// // //     {
// // //         internal static void Init( ref Camera camera, Vector4 position, Vector4 target , Vector4 up)
// // //         {
// // //             camera.View.Identity();
// // //             camera.CamUp = up;
// // //             camera.CamPosition = position;
// // //             camera.CamDirection = target;
// // //             camera.CamRight = Vector4.Normalize( Vector4.Cross(ref camera.CamUp,ref camera.CamDirection));
// // //             camera.Axis = new(0.0f, -Math.PI_OVER2, 0.0f);
// // //             camera.Up = up;

// // //             camera.ExpView = new(1.0f);
// // //             Update(ref camera);
// // //             camera.ExpView = camera.View;
// // //         }

// // //         internal static void WalkAround(ref Camera camera, float speedx, float speedy , float speedz)
// // //         {
// // //             camera.CamPosition += speedz * camera.CamDirection *2.5f;
// // //             camera.CamPosition += speedy * camera.Up;
// // //             camera.CamPosition += speedx * camera.CamRight;
// // //             // camera.Up = Vector4.UnitY;
// // //             camera.CamRight = Vector4.Normalize( Vector4.Cross(ref camera.Up,ref camera.CamDirection  ));
// // //         }

// // //         internal static void LookAround( ref Camera camera, float angleX, float angleY, float angleZ)
// // //         {
// // //             camera.Axis.X += angleX;//pitch
// // //             camera.Axis.Y += angleY;//yaw
// // //             camera.Axis.Z += angleZ;//roll

// // //             camera.Front.X = Math.Cos(camera.Axis.Y  ) * Math.Cos( camera.Axis.X) ;
// // //             camera.Front.Y = Math.Sin(camera.Axis.X);
// // //             camera.Front.Z = Math.Sin(camera.Axis.Y) * Math.Cos(camera.Axis.X);//camera.CamDirection;

// // //             camera.CamDirection = Vector4.Normalize(camera.Front);
// // //             camera.CamRight = Vector4.Normalize( Vector4.Cross(ref camera.CamDirection,ref camera.Up   ));
// // //         }

// // //         internal static void ExpRotate(ref Camera camera, float x , float y , float z)
// // //         {
// // //             Matrix world = new(1.0f);
// // //             Matrix.RotationY(ref world, y );
// // //             camera.ExpView *= world;
// // //             Matrix.RotationX(ref world, x );
// // //             camera.ExpView *= world;
// // //             Matrix.RotationZ(ref world , z);
// // //             camera.ExpView *= world;
// // //         }

// // //         internal static void ExpMove(ref Camera camera, float x , float y , float z)
// // //         {
// // //             camera.ExpView.Translation.X += x;
// // //             camera.ExpView.Translation.Y += y;
// // //             camera.ExpView.Translation.Z += z;
// // //         }

// // //         private static float ax =0.0f;
// // //         private static float ay = 0.0f;
// // //         internal static void AroundTarget( ref Camera camera , ref Vector4 target , float x, float y)
// // //         {
// // //             ax += x;
// // //             ay += y;
// // //             float radius = Vector4.Distance(ref camera.CamPosition,ref target);//X = radius

// // //             if ( camera.CamPosition.X == 0.0f)
// // //                 camera.CamPosition.X = Math.EPSILON;
// // //             float phi = Math.ATan2(camera.CamPosition.Z/camera.CamPosition.X, camera.CamPosition.X);//Y = polar (azimut ) horizontal angle            
// // //             float theta = Math.ACos(camera.CamPosition.Y / radius);// elevation vertical angle

// // //             // if (camera.CamPosition.X < 0) phi += Math.PI;
// // //             phi += ax;
// // //             theta += ay;
// // //             // Math.Clampfr(ref theta, Math.EPSILON, Math.PI);
// // //             // Math.Clampfr(ref phi, Math.EPSILON, Math.TWOPI);

// // //             var sinustheta = radius * Math.Sin(theta);
// // //             // calcul coordonee cartesienne avec changment d'angle 
// // //             Vector4 position = new(0.0f);
// // //             position.Z = ( sinustheta * Math.Cos(phi)) ;//+ target.Z;
// // //             position.X = ( sinustheta * Math.Sin(phi)) ;//+ target.X;
// // //             position.Y = (radius * Math.Cos(theta)) ;//+ target.Y;
// // //             camera.CamPosition = position + target ;
// // //             camera.CamDirection =position- target;
// // //             camera.CamDirection.Negative();
// // //             camera.CamRight = Vector4.Normalize( Vector4.Cross(ref camera.CamDirection,ref camera.Up   ));
// // //         }
// // //         internal static void SetProjection( ref Camera camera,in float width, in float height, in float near = 0.1f, in float far = 1000.0f, in float FOV = 45.0f)
// // //         {
// // //             camera.Projection.Identity();
// // //             Maths.Projection.PerspectiveFOV_RHNO(ref camera.Projection, FOV, width, height, near, far);
// // //         }

// // //         /// <summary>
// // //         /// Place code in loop
// // //         /// Need to call each frame to calculate fustrum
// // //         /// </summary>
// // //         internal static void Update(ref Camera camera )
// // //         {
// // //             Maths.Projection.LookAtRH(ref camera.View,
// // //                 camera.CamPosition,// camera is here
// // //                 camera.CamPosition + camera.CamDirection, //// and looks here : at the same position, plus "direction"
// // //                 camera.CamUp);// Head is up (set to 0,-1,0 to look upside-down)

// // //             Matrix.MultiplyMat4byMat4(ref camera.Clip,ref camera.Projection,ref camera.View);
// // //             // ClipPlanes.ViewFustrum( ref camera.Frustum , ref camera.Clip);
// // //         }

// // //         internal static void ExpUpdate( ref Camera camera)
// // //         {
// // //             Matrix.MultiplyMat4byMat4(ref camera.Clip,ref camera.Projection,ref camera.ExpView);
// // //         }
// // //     }
// // // }

// // //         internal static void RotateAroundTarget(ref Camera camera,ref Vector4 target, float angleX)
// // //         {
// // //             Matrix orientation = new(1.0f);
// // //             camera.World.Identity();
// // //             Matrix.Translate(ref orientation, ref target);
// // //             Matrix.RotationY(ref orientation, angleX);
// // //             orientation.Translation = Vector4.UnitW; //translate to origine of target .????
// // //             camera.View *= orientation;
// // //             // https://wiki.unity3d.com/index.php/MouseOrbitImproved
// // //             System.Console.WriteLine("World" + camera.View);
// // //             camera.Clip = camera.Projection * camera.View ;
// // //        }

// // //         internal static void Orbit(ref Camera camera, Vector4 target, float angleX , float angleY )
// // //         {
// // //             angle += angleX;
// // //             angley += angleY;
// // //             //https://nerdhut.de/2020/05/09/unity-arcball-camera-spherical-coordinates/
// // //             float radius = Vector4.Distance(ref camera.Position,ref target);//X = radius

// // //             float phi = Math.ATan2(camera.Position.Z/camera.Position.X, camera.Position.X);//Y = polar (azimut ) horizontal angle            
// // //             float theta = Math.ACos(camera.Position.Y / radius);// elevation vertical angle

// // //             phi += angle;
// // //             if (camera.Position.X < 0) phi += Math.PI;
// // //             theta += angley;

// // //             var sinustheta = radius * Math.Sin(theta);
// // //             // calcul coordonee cartesienne avec changment d'angle 
// // //             Vector4 position = new(0.0f);
// // //             position.Z = ( sinustheta * Math.Cos(phi)) ;//+ target.Z;
// // //             position.X = ( sinustheta * Math.Sin(phi)) ;//+ target.X;
// // //             position.Y = (radius * Math.Cos(theta)) ;//+ target.Y;
// // //             Vector4 direction = new(0.0f);
// // //             direction.Z =  sinustheta * Math.Cos(phi + Math.PI) ;
// // //             direction.X =  sinustheta * Math.Sin(phi + Math.PI) ;
// // //             direction.Y = (radius * Math.Cos(theta)) ;
// // //             camera.Position = position + target;
// // //             camera.Direction= direction ;
// // //             camera.Right = Vector4.Cross(ref camera.Direction,ref camera.Up);
// // //         }

// // //         private static float angle=0.0f;
// // //         private static float angley=0.0f;
// // //         internal static void Orbital(ref Camera camera, Vector4 target, float angleX )
// // //         {
// // //             angle += angleX;
// // //             //https://roy-t.nl/2010/02/21/xna-simple-arcballcamera.html
// // //             //Calculate the relative position of the camera                        
// // //             // position = Vector3.Transform(Vector3.Backward, Matrix.CreateFromYawPitchRoll(yaw, pitch, 0));
// // //             // //Convert the relative position to the absolute position
// // //             // position *= zoom;
// // //             // position += lookAt;
// // //             Matrix.CreateWorld(ref camera.World, ref camera.Position, ref camera.Direction, ref camera.Up);

// // //             // camera.Position = Vector3.Transform(cameraPosition - cameraTarget, Matrix.CreateFromAxisAngle(axis, angle)) + cameraTarget;
// // //             // view = CreateLookAt(cameraPosition, cameraTarget, cameraUp);
// // //             Vector4 newposition = camera.Position - target;

// // //             Matrix.Translate(ref camera.World, ref newposition);
// // //             Vector4 axis =  new(0.0f,1.0f,0.0f,1.0f);
// // //             Matrix.Rotate(ref camera.World, angle,ref axis);
// // //             Vector4.Transform(ref newposition , ref camera.Position, ref camera.World);
// // //             camera.Position = newposition;
// // //             camera.Direction = target;
// // //         }

// // //         /// <summary>
// // //         /// Center a camera to target object and backward of distance
// // //         /// https://stackoverflow.com/questions/10752435/how-do-i-make-a-camera-follow-an-object-in-unity3d-c
// // //         /// </summary>
// // //         /// <param name="camera">used Camera contains data of position</param>
// // //         /// <param name="target"></param>
// // //         internal static void CenterCameraToObject(ref Camera camera , ref Vector4 target, float radius=0.0f)
// // //         {
// // //             radius = radius==0.0f ?  Vector4.Distance(ref camera.Position, ref target): radius;
// // //             camera.Position = target - (radius * camera.Direction );
// // //         }

// // //         internal static void CreateMatrixWorld(ref Matrix result, ref Vector4 position, ref Vector4 forward, ref Vector4 up)
// // //         {
// // //             Vector4 x, y, z;
// // //             z = forward;
// // //             z.Normalize();
// // //             // z.Negative();
// // //             x = Vector4.Cross(ref forward,ref  up);
// // //             y = Vector4.Cross(ref x,ref forward);
// // //             x.Normalize();
// // //             y.Normalize();

// // //             result.Right = x;
// // //             result.Up = y;
// // //             result.Forward = z;
// // //             result.Translation = position;
// // //             result.Translation.W = 1f;
// // //         }

// // //         // internal static float SmoothDamp (float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
// // //         // {
// // //         //     // smoothTime = Mathf.Max (0.0001f, smoothTime);
// // //         //     float num = 2f / smoothTime;
// // //         //     float num2 = num * deltaTime;
// // //         //     float num3 = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
// // //         //     float num4 = current - target;
// // //         //     float num5 = target;
// // //         //     float num6 = maxSpeed * smoothTime;
// // //         //     num4 = Mathf.Clamp (num4, -num6, num6);
// // //         //     target = current - num4;
// // //         //     float num7 = (currentVelocity + num * num4) * deltaTime;
// // //         //     currentVelocity = (currentVelocity - num * num7) * num3;
// // //         //     float num8 = target + (num4 + num7) * num3;
// // //         //     if (num5 - current > 0f == num8 > num5)
// // //         //     {
// // //         //         num8 = num5;
// // //         //         currentVelocity = (num8 - num5) / deltaTime;
// // //         //     }
// // //         //     return num8;
// // //         // }

// // // */




























// // <!-- using MCJ.Game.Core.Maths;

// // using MCJEngine.Maths;
// // using MCJEngine.OS;
// // using System;
// // using Global;

// // namespace MCJ.Game.Core.Graphics
// // {
// //     internal static class Camera
// //     {
// //         internal static void SetPosition( ref CameraData camera, Vector3 position, Vector3 target , Vector3 Up)
// //         {
// //             camera.Up = Up;
// //             camera.Position = position;
// //             camera.Direction = target;
// //             camera.Right = Vector3.Normalize( Vector3.Cross(camera.Up, camera.Direction));
// //             camera.World.Identity();
// //         }
// //         internal static void SetProjection( ref CameraData camera, CameraType typecam, in int width, in int height, in float near = 0.1f, in float far = 100.0f, in float FOV = 45.0f)
// //         {
// //             camera.Projection.Identity();
// //             switch (typecam)
// //             {
// //                 case CameraType.OrthoParallel:
// //                     Projection.PerspectiveFOV_RHNO(ref camera.Projection, FOV, width, height, near, far);
// //                     break;
// //                 case CameraType.Prespective:
// //                     Projection.Perspective_RHNO(ref camera.Projection, FOV, (float)width / (float)height, near, far);
// //                     break;
// //                 case CameraType.Ortho2D:
// //                     Projection.Ortho(ref camera.Projection, 0, width, 0, height, near, far);
// //                     break;
// //             }
// //             Update(ref camera);
// //         }

// //         /// <summary>
// //         /// Place code in loop
// //         /// Need to call each frame to calculate fustrum
// //         /// </summary>
// //         internal static void Update(ref CameraData camera )
// //         {
// //             Projection.LookAtRH(ref camera.View,
// //                 camera.Position,// camera is here
// //                 camera.Position + camera.Direction /*+ direction */, //// and looks here : at the same position, plus "direction"
// //                 camera.Up);// Head is up (set to 0,-1,0 to look upside-down)

// //             //calcul Frustum no need to do multi time
// //             // camera.Clip = camera.Projection *  camera.View;// evite le new ...
// //             Matrix4.MultiplyMat4byMat4(ref camera.Clip, camera.Projection, camera.View);
// //         }

// //                 /// <summary>
// //         /// Rotate around  Z Axis
// //         /// </summary>
// //         /// <param name="angle"></param>
// //         internal static void Roll(ref CameraData camera,in float angle)
// //         {
// //             // camera.roll += angle ;//RotateZ
// //             camera.Right = Vector3.Normalize(
// //                 (camera.Right * Math.Cos(angle * Math.PIOVER180)) +
// //                 (camera.Up * Math.Sin(angle * Math.PIOVER180))
// //             );

// //             camera.Up = Vector3.Cross(camera.Direction, camera.Right);
// //             camera.Up *= -1;
// //         }

// //         /// <summary>
// //         /// Rotate around X axis
// //         /// </summary>
// //         /// <param name="angle"></param>
// //         internal static void Pitch(ref CameraData camera,in float angle)
// //         {
// //             // camera.pitch += angle  ;// rotateX
// //             camera.Direction = Vector3.Normalize(
// //                 (camera.Direction * Math.Cos(Math.DegToRad(angle))) +
// //                 (camera.Up * Math.Sin(Math.DegToRad(angle)))
// //             );

// //             camera.Up = Vector3.Cross(camera.Direction, camera.Right);
// //             camera.Up *= -1;
// //         }

// //         /// <summary>
// //         /// Rotate around Y axis with angle in degree
// //         /// </summary>
// //         /// <param name="angle"></param>
// //         internal static void Yaw(ref CameraData camera,in float angle)
// //         {
// //             // camera.yaw += angle  ;//rotateY
// //             // re-calculate the new forward vector
// //             camera.Direction = Vector3.Normalize(
// //                 (camera.Direction * Math.Cos(Math.DegToRad(angle))) -
// //                 (camera.Right * Math.Sin(Math.DegToRad(angle)))
// //             );
// //             // re-calculate the new right vector
// //             camera.Right = Vector3.Cross(camera.Direction, camera.Up);
// //         }

// //         /// <summary>
// //         /// // x movement  LEFT RIGHT
// //         /// </summary>
// //         /// <param name="distance"></param>
// //         internal static void Strafe(ref CameraData camera,in float distance)
// //             => camera.Position += camera.Right * distance;

// //         /// <summary>
// //         /// // y movement UP DOWN ( for jump )
// //         /// </summary>
// //         /// <param name="distance"></param>
// //         internal static void Ascend(ref CameraData camera,in float distance)
// //             => camera.Position += camera.Up * distance;

// //         /// <summary>
// //         ///z movement FORWARD BACKWARD
// //         /// </summary>
// //         /// <param name="distance"></param>
// //         internal static void Advance(ref CameraData camera,in float distance)
// //             => camera.Position += camera.Direction * -distance;

// //         /// <summary>
// //         /// deplacement de n pixel pour chaque axe
// //         /// x => strafe left right
// //         /// y=> ascend jump up down
// //         /// z => advance   ( forward, backward )
// //         /// </summary>
// //         /// <param name="velocity"></param>
// //         internal static void Translate(ref CameraData camera, Vector3 velocity)
// //             => camera.Position += (camera.Direction * velocity.Z) + (camera.Up * velocity.Y) + (camera.Right * velocity.X);

// //         internal static void RotateAroundTarget(ref CameraData camera, Vector3 target, float angleAxeZ)
// //         {
// // //             //wikipedia calcul coordonee spherique  https://fr.wikipedia.org/wiki/Coordonn%C3%A9es_sph%C3%A9riques
// //             float radius = Vector3.Distance(camera.Position, target);//X = radius
// //             float phi = Math.ATan2(camera.Position.Z/camera.Position.X, camera.Position.X);//Y = polar (azimut ) horizontal angle            
// //             float theta = Math.ACos(camera.Position.Y / radius);// elevation vertical angle

// //             //if (camera.Position.X < 0) phi += Math.PI;

// //             phi += angleAxeZ;
// //             // theta += verticalAngeDegree;

// //             // phi = Math.Clampf( phi,0,Math.PI );

// //             var sinustheta = radius * Math.Sin(theta);
// //             // calcul coordonee cartesienne avec changment d'angle 
// //             camera.Position.Z = ( sinustheta * Math.Cos(phi)) + target.Z;
// //             camera.Position.X = ( sinustheta * Math.Sin(phi)) + target.X;
// //             camera.Position.Y = (radius * Math.Cos(theta)) + target.Y;

// //             camera.Direction.Z =  sinustheta * Math.Cos(phi + Math.PI) ;
// //             camera.Direction.X =  sinustheta * Math.Sin(phi + Math.PI) ;
// //             camera.Direction.Y = radius * Math.Cos(theta ) ;
// //             camera.Right = Vector3.Cross(camera.Direction, camera.Up);
// //         }

// //         internal static void OrbitLeftRight(ref CameraData camera, Vector3 target,float YangleDegree)
// //         {
// //             //ref : https://community.monogame.net/t/rotate-camera-around-target/12749/2
// //             // var rotAmnt = Matrix.CreateRotationY(angle);
// //             //         World *= rotAmnt;
// //             //         World.Translation = Vector3.Normalize(World.Translation - targetPosition) * distance;
// //             //         World = Matrix.CreateWorld(World.Translation, targetPosition - World.Translation, World.Up);
// //             // float radius = Vector3.Distance(camera.Position, target);//X = radius
// //             // Matrix4 rot = new(1.0f);
// //             // Matrix4.RotationY( ref rot, YangleDegree );
// //             // camera.View *= rot;
// //             // camera.View.Row4 = Vector4.Normalize(camera.View.Row4 - new Vector4(target))*radius;
// //             // Vector4 targetPos = new(target);
// //             // CreateWorld(ref camera.View, ref camera.View.Row4  , targetPos- camera.View.Row4,ref camera.View.Row2);
// //             // camera.Direction = target - camera.View.Translation;
// //             // camera.Position = camera.View.Translation;
// //             // camera.Right = Vector3.Cross(camera.Direction, camera.Up);
// //             // camera.Direction=target ;
// //             // Vector3 pos = camera.Position - camera.Direction;
// //             // Matrix4 mat = new(1.0f);
// //             // Matrix4.CreateFromAxisAngle(1.0f,0.0f,0.0f, YangleDegree,ref mat);
// //             // Vector3.Transform( ref pos, ref mat, ref camera.Position);
// //             // camera.Position +=  camera.Direction;
// //             // camera.Right = Vector3.Normalize( Vector3.Cross(camera.Up, camera.Direction));
// //             // https://stackoverflow.com/questions/5906907/xna-rotate-camera-around-its-createlookat-target
// //             // cameraPosition = Vector3.Transform(cameraPosition - cameraTarget, Matrix.CreateFromAxisAngle(axis, angle)) + cameraTarget;
// //             // view = CreateLookAt(cameraPosition, cameraTarget, cameraUp);
// //             float distance = Vector3.Distance(camera.Position, target);//X = radius
// //             System.Console.WriteLine("distance : before" + distance);

// //             camera.Position = new( target.X + (Math.Cos(Math.DegToRad(YangleDegree))* distance), target.Y, target.Y + (Math.Sin(Math.DegToRad(YangleDegree))*distance)   );
// //             camera.Direction =new( target.X + (Math.Cos( Math.DegToRad(YangleDegree)+ Math.PI )* distance) , target.Y, target.Z + (Math.Sin(Math.DegToRad(YangleDegree)+Math.PI)*distance)   );
// //             camera.Right = Vector3.Cross(camera.Direction, camera.Up);
// //             System.Console.WriteLine("distance : after" + distance);
// //         }

// //         internal static void CreateWorld( ref Matrix4 result,ref Vector4 position,  Vector4 forward, ref Vector4 up)
// //         {
// //             Vector4 z = new(forward);
// //             z.Normalize();
// //             Vector4 x = Vector4.Cross(forward, up);
// //             Vector4 y = Vector4.Cross(x, forward);
// //             x.Normalize();
// //             y.Normalize();

// //             //result.Identity();
// //             result.Row1 = x;
// //             result.Row2=y;
// //             result.Row4 =position;
// //             result.Row3= z;
// //             result.Row4.W = 1.0f;
// //         }
// //         internal static void CreateLookAt(ref Matrix4 result,ref Vector3 cameraPosition, ref Vector3 cameraTarget, ref Vector3 cameraUpVector )
// //         {
// //             var vector = Vector3.Normalize(cameraPosition - cameraTarget);
// //             var vector2 = Vector3.Normalize(Vector3.Cross(cameraUpVector, vector));
// //             var vector3 = Vector3.Cross(vector, vector2);
// // 		    result.Row1.X = vector2.X;
// // 		    result.Row1.Y = vector3.X;
// // 		    result.Row1.Z = vector.X;
// // 		    result.Row1.W = 0f;
// // 		    result.Row2.X = vector2.Y;
// // 		    result.Row2.Y = vector3.Y;
// // 		    result.Row2.Z = vector.Y;
// // 		    result.Row2.W = 0f;
// // 		    result.Row3.X = vector2.Z;
// // 		    result.Row3.Y = vector3.Z;
// // 		    result.Row3.Z = vector.Z;
// // 		    result.Row3.W = 0f;
// // 		    result.Row4.X = -Vector3.Dot(vector2, cameraPosition);
// // 		    result.Row4.Y = -Vector3.Dot(vector3, cameraPosition);
// // 		    result.Row4.Z = -Vector3.Dot(vector, cameraPosition);
// // 		    result.Row4.W = 1f;
// //         }

// //         internal static void CreatePerspectiveFieldOfView( ref Matrix4 result,float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance)
// //         {
// //             var yScale = 1.0f / (float)Math.Tan(fieldOfView * 0.5f);
// //             var xScale = yScale / aspectRatio;
// //             var negFarRange = float.IsPositiveInfinity(farPlaneDistance) ? -1.0f : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

// //             result.Row1.X = xScale;
// //             result.Row1.Y = result.Row1[3] = result.Row1[4] = 0.0f;
// //             result.Row2.Y = yScale;
// //             result.Row2.X = result.Row2[3] = result.Row2[4] = 0.0f;
// //             result.Row3.X = result.Row3[2] = 0.0f;
// //             result.Row3.Z = negFarRange;
// //             result.Row3.W = -1.0f;
// //             result.Row4.X = result.Row4[2] = result.Row4[4] = 0.0f;
// //             result.Row4.Z = nearPlaneDistance * negFarRange;
// //         }
// //         internal static void CreateTranslation(ref Vector3 position, ref Matrix4 result)
// //         {
// //             result.Row1[1] = 1;
// //             result.Row1[2] = 0;
// //             result.Row1[3] = 0;
// //             result.Row1[4] = 0;
// //             result.Row2[1] = 0;
// //             result.Row2[2] = 1;
// //             result.Row2[3] = 0;
// //             result.Row2[4] = 0;
// //             result.Row3[1] = 0;
// //             result.Row3[2] = 0;
// //             result.Row3[3] = 1;
// //             result.Row3[4] = 0;
// //             result.Row4[1] = position.X;
// //             result.Row4[2] = position.Y;
// //             result.Row4[3] = position.Z;
// //             result.Row4[4] = 1;
// //         }
// //     }
// // }

// // namespace MCJEngine.Graphics
// // {

// //     internal class CameraType
// //     {
// //         internal const byte Ortho2D = 1;

// //         internal const byte OrthoParallel = 2;

// //         internal const byte Prespective =3;

// //     }



// //     internal class Camera
// //     {
// //         private readonly Profiler log = Profilers.GetProfiler(typeof(Camera).FullName);
// //         private readonly WinData data;

// //         private readonly Shader shader;


// //         private Matrix4x4 view = new Maths.Matrix4x4(0.0f);
// //         private Matrix4x4 projection = new Matrix4x4(0.0f);


// //         // private Vector3 WorldUp = new Vector3(0.0f, 1.0f, 0.0f);
// //         private Vector3 Position = new Vector3(0.0f, 0.0f, 3.0f);

// //         /// <summary>
// //         /// Eye direction
// //         /// </summary>
// //         /// <returns></returns>
// //         private Vector3 Direction = new Vector3(0.0f, 0.0f, -1.0f);
// //         private Vector3 Right = new Vector3(-1.0f, 0.0f, 0.0f);
        
// //         /// <summary>
// //         /// head up camera  if y = -1.0f see tete en bas
// //         /// </summary>
// //         /// <returns></returns>
// //         private Vector3 Up = new Vector3(0.0f, 1.0f, 0.0f);




// //         internal Camera(WinData data, Shader cameraShader)
// //         {
// //             this.shader = cameraShader;
// //             this.data = data;
// //         }

// //         internal void Init(Vector3 position )
// //         {
// //             log.Notice("Camera Init");
// //             this.Position = position;
            
// //         }

// //         /// <summary>
// //         /// Set perspective Right Hand 
// //         /// Shader need to be actived before
// //         /// </summary>
// //         /// <param name="name"></param>
// //         /// <param name="near"></param>
// //         /// <param name="far"></param>
// //         /// <param name="FOV"></param>
// //         internal void SetProjection(string name, float near, float far, float FOV=45.0f)
// //         {
// //             projection.Identity();
// //             projection = Transform.Perspective(FOV, (float)data.Width / (float)data.Height, 0.1f, 100.0f);

// //             shader.UniformWithMat4(name, projection.ToArray() ,true);
// //         }    

// //         //TODO SetOrtho    
        

// //         /// <summary>
// //         /// Need to call each frame to calculate fustrum 
// //         /// </summary>
// //         internal void Update()
// //         {
// //             Maths.Transform.LookAtRH(ref view,
// //                 Position,// camera is here
// //                 Position + Direction, //// and looks here : at the same position, plus "direction"
// //                 Up);// Head is up (set to 0,-1,0 to look upside-down)
// //         }

// //         internal void Release()
// //         {
// //             // view.Release();
// //         }

// //         internal float[] ViewMatrix { get { return view.ToArray(); } }
// //         internal float[] ProjectionMatrix { get { return projection.ToArray(); }}

// // //http://tuttlem.github.io/2013/12/30/a-camera-implementation-in-c.html for yaw pitch roll

// // private float roll = 90.0f;

// //         /// <summary>
// //         /// Rotate around  Z Axis
// //         /// </summary>
// //         /// <param name="angle"></param>
// //         internal void Roll(float angle) 
// //         {
// //             this.roll += angle ;//RotateZ

// //             this.Right = Transform.Normalize(   
// //                     this.Right* Functions.Cos( angle * Functions.PI / 180 ) +
// //                     this.Up * Functions.Sin( angle * Functions.PI /180 )

// //             );
// //             this.Up = Transform.Cross(  Direction , Right);
// //             this.Up *= -1;
// //         }

// // private float pitch = 0.0f;

// //         /// <summary>
// //         /// Rotate around X axis
// //         /// </summary>
// //         /// <param name="angle"></param>
// //         internal void Pitch(float angle) 
// //         {
// //             this.pitch += angle  ;// rotateX

// //             this.Direction =Transform.Normalize(  
// //                 this.Direction * Functions.Cos( Functions.DegreesToRadians(angle) ) +
// //                 this.Up *  Functions.Sin( Functions.DegreesToRadians(angle))
// //             );

// //             this.Up = Transform.Cross(  Direction , Right);
// //             this.Up *= -1;
// //         }

// // private float yaw = 90.0f;

// //         /// <summary>
// //         /// Rotate around Y axis
// //         /// </summary>
// //         /// <param name="angle"></param>
// //         internal void Yaw(float angle) 
// //         {
// //             this.yaw += angle  ;//rotateY

// //             // re-calculate the new forward vector
// //             this.Direction =  Transform.Normalize(
// //                 this.Direction * Functions.Cos(  Functions.DegreesToRadians(angle)  ) -
// //                 this.Right * Functions.Sin(  Functions.DegreesToRadians(angle) )
// //             );

// //             // re-calculate the new right vector
// //             this.Right= Transform.Cross(this.Direction, this.Up);

// //         }


// // /*
// // reddo mousemoveFps( x ,y )

// // yaw(y)
// // pitch(x)


// // SEE Web  for orbit  and trackball
// // https://www.songho.ca/opengl/gl_camera.html#example1
// // */
        
// //         /// <summary>
// //         /// // x movement  LEFT RIGHT
// //         /// </summary>
// //         /// <param name="distance"></param>
// //         internal void Strafe(float distance) => this.Position += (this.Right * distance);
        
// //         /// <summary>
// //         /// // y movement UP DOWN ( for jump ) 
// //         /// </summary>
// //         /// <param name="distance"></param>
// //         internal void Ascend(float distance) =>  this.Position += (this.Up * distance);
        
// //         /// <summary>
// //         ///z movement FORWARD BACKWARD 
// //         /// </summary>
// //         /// <param name="distance"></param>
// //         internal void Advance(float distance)  =>this.Position +=  ( this.Direction * - distance );

// //         //for update camera vectors
// //         // private Vector3 front = new Vector3(0.0f, 0.0f, 0.0f);

// //         // /// <summary>
// //         // /// Horizontal angle in degree
// //         // /// </summary>
// //         // internal float YAW = -90.0f;

// //         // /// <summary>
// //         // /// Vertical angle in degree
// //         // /// </summary>
// //         // internal float PITCH = 0.0f;

// //         // internal void MoveForward(float speed, float deltaTime) => this.Position += this.Direction * speed * deltaTime;
// //         // internal void MoveBackward(float speed, float deltaTime) => this.Position -= this.Direction * speed * deltaTime;
// //         // internal void StrafeRight(float speed, float deltaTime) => this.Position += this.Right * speed * deltaTime;
// //         // internal void StrafeLeft(float speed, float deltaTime) => this.Position -= this.Right * speed * deltaTime;
        
// //         // //ascend
// //         // internal void MoveUp(float speed, float deltaTime   )=> this.Position += this.Up *speed * deltaTime;
// //         // //ascend
// //         // internal void MoveDown(float speed, float deltaTime   )=> this.Position -= this.Up *speed * deltaTime;

// //         // internal void RotateThroughAxeX( float speed , float deltaTime )
// //         // {
// //         //     this.YAW += deltaTime * speed ;
// //         //     updateCameraVectors();
// //         // }

// //         // internal void RotateThroughAxeY( float speed , float deltaTime )
// //         // {
// //         //     this.PITCH += deltaTime * speed;
// //         //     updateCameraVectors();
// //         // }   

// //         // //rool


        

// //         // internal void MoveMouse(float xoffset, float yoffset, float speed)
// //         // {
// //         //     this.YAW += xoffset * speed;
// //         //     this.PITCH += yoffset * speed;
// //         //     this.PITCH = PITCH > 89.0f ? 89.0f : PITCH < -89.0f ? -89.0f : this.PITCH;

// //         //     // updateCameraVectors();

// //         //     // update Direction, Right and Up Vectors using the updated Euler angles
// //         //     front.X = Maths.Functions.Cos(Maths.Functions.DegreesToRadians(YAW)) * 
// //         //               Maths.Functions.Cos(Maths.Functions.DegreesToRadians(PITCH));
            
// //         //     front.Y = Maths.Functions.Sin(Maths.Functions.DegreesToRadians(PITCH));
            
// //         //     front.Z = Maths.Functions.Sin(Maths.Functions.DegreesToRadians(YAW)) * 
// //         //               Maths.Functions.Cos(Maths.Functions.DegreesToRadians(PITCH));

// //         //     Direction = Transform.Normalize(front);
// //         //     // also re-calculate the Right and Up vector         
            
// //         //     // Right = Transform.Normalize(Transform.Cross(Direction, WorldUp));  // normalize the vectors, because their length gets closer to 0 the more you look up or down which results in slower movement.
// //         //     // Up = Transform.Normalize(Transform.Cross(Right, Direction));// Up vector : perpendicular to both direction and right

// //         // }

// //         // private void updateCameraVectors()
// //         // {
// //         //     // // update Direction, Right and Up Vectors using the updated Euler angles
// //         //     // front.X = Maths.Functions.Cos(Maths.Functions.DegreesToRadians(YAW)) * 
// //         //     //           Maths.Functions.Cos(Maths.Functions.DegreesToRadians(PITCH));
            
// //         //     // front.Y = Maths.Functions.Sin(Maths.Functions.DegreesToRadians(PITCH));
            
// //         //     // front.Z = Maths.Functions.Sin(Maths.Functions.DegreesToRadians(YAW)) * 
// //         //     //           Maths.Functions.Cos(Maths.Functions.DegreesToRadians(PITCH));

// //         //     // Direction = Transform.Normalize(front);
// //         //     // // also re-calculate the Right and Up vector
            
// //         //     // Right = Transform.Normalize(Transform.Cross(Direction, WorldUp));  // normalize the vectors, because their length gets closer to 0 the more you look up or down which results in slower movement.
// //         //     // Up = Transform.Normalize(Transform.Cross(Right, Direction));// Up vector : perpendicular to both direction and right
// //         // }


// //     }
// // }
// // //namespace MCJEngine.Maths
// // // {
// // //     using Core;
    
// // //     internal class Cameras : System.IDisposable
// // //     {
// // //         readonly Core.Profiler log = Core.Profilers.GetProfiler(typeof(Cameras).FullName);
// // //         readonly MainGameData data;

// // //         internal Cameras(MainGameData data) =>  this.data = data;
// // //         internal void Dispose()=> Release();
        
// // //         internal void Release()    {}

// // //         internal float[] GetCameraProjectionView(int cameraId)=> this.data.Camera_Clip[cameraId].ToArray();

// // //         // #region Gestion Cameras
// // //         internal void Load( int NummberofCameras = 8)
// // //         {
// // //             this.data.Camera_View = new List<Matrix4x4>(NummberofCameras);
// // //             this.data.Camera_Projection = new List<Matrix4x4>(NummberofCameras);
// // //             this.data.Camera_Clip = new List<Matrix4x4>(NummberofCameras);
// // //             this.data.Camera_Position = new List<Vector3>(NummberofCameras);
// // //             this.data.Camera_Direction = new List<Vector3>(NummberofCameras);//Eye direction ( target ?)
// // //             this.data.Camera_Right = new List<Vector3>(NummberofCameras);// Strafe
// // //             this.data.Camera_Up = new List<Vector3>(NummberofCameras);/// head up camera  if y = -1.0f see tete en bas
// // //             this.data.Camera_Near = new List<float>(NummberofCameras);
// // //             this.data.Camera_Far = new List<float>(NummberofCameras);
// // //             this.data.Camera_Fov = new List<float>(NummberofCameras);
// // //             this.data.Camera_Active =new List<int>(NummberofCameras); 
// // //             this.data.Camera_Width = new List<float>(NummberofCameras);
// // //             this.data.Camera_Height= new List<float>(NummberofCameras);
// // //             this.data.Camera_Type =  new List<CameraType>(NummberofCameras);        
// // //             this.data.Camera_IsActive = new List<bool>(NummberofCameras);
// // //             this.data.Camera_Velocity = new List<Vector3>(NummberofCameras);
// // //         }

// // //         internal int AddCamera( CameraType type,float width, float height, float fov, float near, float far , bool activateIt = true)
// // //         {
// // //             var cameraId = this.data.Camera_Active.Count;

// // //             this.data.Camera_View.Add(new Matrix4x4(1.0f));
// // //             this.data.Camera_Projection.Add(new Matrix4x4(1.0f));
// // //             this.data.Camera_Clip.Add(new Matrix4x4(1.0f));
// // //             this.data.Camera_Position.Add(new Vector3(0.0f,0.0f,3.0f) );
// // //             this.data.Camera_Direction.Add(new Vector3(0.0f,0.0f,-4.0f) );
// // //             this.data.Camera_Right.Add(new Vector3(0.0f,0.0f,0.0f) );
// // //             this.data.Camera_Up.Add(new Vector3(0.0f,1.0f,0.0f));
// // //             this.data.Camera_Near.Add(near);
// // //             this.data.Camera_Far.Add(far);
// // //             this.data.Camera_Fov.Add(fov);
// // //             this.data.Camera_Active.Add(cameraId);
// // //             this.data.Camera_Width.Add(width);
// // //             this.data.Camera_Height.Add(height);
// // //             this.data.Camera_Type.Add(type);
// // //             this.data.Camera_Velocity.Add(new Vector3(0.0f));
// // //             this.data.Camera_IsActive.Add(activateIt);
// // //             SetCameraProjection(cameraId , type, width,  height, fov,  near, far  );
// // //             return cameraId;
// // //         }

// // //         internal void ChangeActiveCamera( int cameraId, bool inactiveOldCameraId = true)
// // //         {
// // //             //check if id > 0 inferieur Camera.Length 
// // //             this.data.Camera_IsActive.Fill(false);
// // //             this.data.Camera_IsActive[cameraId] = true ;
// // //             // this.data.Camera_CurrentActiveIndex = id ;
// // //         }

// // //         internal void SetCameraTransform(int cameraId , Vector3 position ,Vector3 direction, Vector3 velocity )
// // //         {
// // //             data.Camera_Velocity[cameraId] = velocity;
// // //             data.Camera_Position[cameraId] = position;
// // //             data.Camera_Direction[cameraId] = direction;
// // //             Update(cameraId);
// // //         }

// // //         internal void SetCameraProjection(int cameraId ,CameraType type, float width, float height, float fov, float near, float far  )
// // //         {
// // //             //projection .....  
// // //             data.Camera_Projection[cameraId].Identity();
// // //             switch (type)
// // //             {
// // //                 case CameraType.OrthoParallel:
// // //                     Project3D.PerspectiveFOV_RHNO(data.Camera_Projection[cameraId], fov, width, height, near, far);
// // //                     break;
// // //                 case CameraType.Prespective:
// // //                     Project3D.Perspective_RHNO(data.Camera_Projection[cameraId], fov, (float)width / (float)height, near, far);
// // //                     break;
// // //                 case CameraType.Ortho2D:
// // //                     // Transform.Ortho(ref data.Camera.Projections[id], 0, width, 0, height, near, far);
// // //                     break;

// // //             }  
// // //             Update(cameraId);
// // //         }

// // //         internal void UpdateAll()
// // //         {
// // //             for(int i = 0 ; i < data.Camera_Active.Count ; i++ )
// // //                 Update(i);   
// // //         }

// // //         internal void Update(int cameraId)
// // //         {
// // //             // Matrix4x4 m = data.Camera_View[cameraId] ;
// // //             Project3D.LookAtRH(data.Camera_View[cameraId],
// // //             data.Camera_Position[cameraId],// camera is here
// // //             data.Camera_Direction[cameraId] /*+ direction */, //// and looks here : at the same position, plus "direction"
// // //             data.Camera_Up[cameraId]);// Head is up (set to 0,-1,0 to look upside-down)

// // //             data.Camera_Clip[cameraId] =data.Camera_Projection[cameraId] * data.Camera_View[cameraId];
// // //         }

// // //         /// <summary>
// // //         /// Rotate around  Z Axis
// // //         /// </summary>
// // //         /// <param name="angle"></param>
// // //         internal void Roll(in int cameraId, in float angle)
// // //         {
// // //             // this.roll += angle ;//RotateZ
// // //             this.data.Camera_Right[cameraId] = Vector3.Normalize(
// // //                 this.data.Camera_Right[cameraId] * Maths.Cos(angle * Maths.PIOVER180) +
// // //                 this.data.Camera_Up[cameraId] * Maths.Sin(angle * Maths.PIOVER180)
// // //             );

// // //             this.data.Camera_Up[cameraId] = Vector3.Cross(this.data.Camera_Direction[cameraId], this.data.Camera_Right[cameraId]);
// // //             this.data.Camera_Up[cameraId] *= -1;
// // //         }

// // //         /// <summary>
// // //         /// Rotate around X axis
// // //         /// </summary>
// // //         /// <param name="angle"></param>
// // //         internal void Pitch(in int cameraId,in float angle)
// // //         {
// // //             // this.pitch += angle  ;// rotateX
// // //             this.data.Camera_Direction[cameraId] = Vector3.Normalize(
// // //                 this.data.Camera_Direction[cameraId] * Maths.Cos(Maths.DegToRad(angle)) +
// // //                 this.data.Camera_Up[cameraId] * Maths.Sin(Maths.DegToRad(angle))
// // //             );

// // //             this.data.Camera_Up[cameraId] = Vector3.Cross(this.data.Camera_Direction[cameraId], this.data.Camera_Right[cameraId]);
// // //             this.data.Camera_Up[cameraId] *= -1;
// // //         }

// // //         /// <summary>
// // //         /// Rotate around Y axis with angle in degree
// // //         /// </summary>
// // //         /// <param name="angle"></param>
// // //         internal void Yaw(in int cameraId,in float angle)
// // //         {
// // //             // this.yaw += angle  ;//rotateY
// // //             // re-calculate the new forward vector
// // //             this.data.Camera_Direction[cameraId] = Vector3.Normalize(
// // //                 this.data.Camera_Direction[cameraId] * Maths.Cos(Maths.DegToRad(angle)) -
// // //                 this.data.Camera_Right[cameraId] * Maths.Sin(Maths.DegToRad(angle))
// // //             );
// // //             // re-calculate the new right vector
// // //             this.data.Camera_Right[cameraId] = Vector3.Cross(this.data.Camera_Direction[cameraId], this.data.Camera_Up[cameraId]);
// // //         }

// // //         /// <summary>
// // //         /// // x movement  LEFT RIGHT
// // //         /// </summary>
// // //         /// <param name="distance"></param>
// // //         internal void Strafe(in int cameraId, in float distance)
// // //             => this.data.Camera_Position[cameraId] += (this.data.Camera_Right[cameraId] * distance);

// // //         /// <summary>
// // //         /// // y movement UP DOWN ( for jump ) 
// // //         /// </summary>
// // //         /// <param name="distance"></param>
// // //         internal void Ascend(in int cameraId, in float distance)
// // //             => this.data.Camera_Position[cameraId] += (this.data.Camera_Up[cameraId] * distance);

// // //         /// <summary>
// // //         ///z movement FORWARD BACKWARD 
// // //         /// </summary>
// // //         /// <param name="distance"></param>
// // //         internal void Advance(in int cameraId, in float distance)
// // //             => this.data.Camera_Position[cameraId] += (this.data.Camera_Direction[cameraId] * -distance);

// // //         /// <summary>
// // //         /// deplacement de n pixel pour chaque axe 
// // //         /// x => strafe left right 
// // //         /// y=> ascend jump up down 
// // //         /// z => advance   ( forward, backward )
// // //         /// </summary>
// // //         /// <param name="distance"></param>
// // //         internal void Translate(in int cameraId, in Vector3 distance)
// // //             => this.data.Camera_Position[cameraId] += (this.data.Camera_Direction[cameraId] * distance.Z) + (this.data.Camera_Up[cameraId] * distance.Y) + (this.data.Camera_Right[cameraId] * distance.X);

// // //         internal void Translate(in int cameraId, in float x = 0.0f, in float y = 0.0f, in float z = 0.0f)
// // //             => this.data.Camera_Position[cameraId] += (this.data.Camera_Direction[cameraId] * z) + (this.data.Camera_Up[cameraId] * y) + (this.data.Camera_Right[cameraId] * x);

// // //         internal void RotateAroundTarget(in int cameraId, in Vector3 target, in float horizontalAngledegre, in float verticalAngeDegree)
// // //         {
// // //             //orbit += horizontalAngledegre <= -360 ? 0 : horizontalAngledegre >= 360 ? 0 : horizontalAngledegre;
            
// // //             if (this.data.Camera_Position[cameraId].X == 0) this.data.Camera_Position[cameraId].X = 0.01f;// Mathf.Epsilon;
// // //             if (this.data.Camera_Position[cameraId].Y == 0) this.data.Camera_Position[cameraId].Z = 0.01f;// Mathf.Epsilon;
// // //             if (this.data.Camera_Position[cameraId].Z == 0) this.data.Camera_Position[cameraId].Y = 0.01f;// Mathf.Epsilon;

// // //             //wikipedia calcul coordonee spherique  https://fr.wikipedia.org/wiki/Coordonn%C3%A9es_sph%C3%A9riques
           
// // //             float radius = Vector3.Distance(this.data.Camera_Position[cameraId], target);//X = radius
// // //             float phi = Maths.ATan2(this.data.Camera_Position[cameraId].X, this.data.Camera_Position[cameraId].Z);//Y = polar (azimut ) horizontal angle            
// // //             float theta = Maths.ACos(this.data.Camera_Position[cameraId].Y / radius);// elevation vertical angle

// // //             if (this.data.Camera_Position[cameraId].X < 0) phi += Maths.PI;
           
// // //             phi += horizontalAngledegre;
// // //             theta += verticalAngeDegree;

// // //             // phi = Maths.Clampf( phi, -1.5f,1.5f );
// // //             // theta = Maths.Clampf(theta , -3.14f , 3.14f);
            
// // //             var sinustheta = radius * Maths.Sin(theta);
            
// // //             this.data.Camera_Position[cameraId].Z = ( sinustheta * Maths.Cos(phi)) + target.Z;
// // //             this.data.Camera_Position[cameraId].X = ( sinustheta * Maths.Sin(phi)) + target.X;
// // //             this.data.Camera_Position[cameraId].Y =(theta < 0.0f ? 1 : -1 )* (radius * Maths.Cos(theta)) + target.Y;
            
// // //             this.data.Camera_Direction[cameraId].Z =  sinustheta * Maths.Cos(phi + Maths.PI) ;
// // //             this.data.Camera_Direction[cameraId].X =  sinustheta * Maths.Sin(phi + Maths.PI) ;
// // //             this.data.Camera_Direction[cameraId].Y = (theta < 0.0f ? -1 : 1 )* radius * Maths.Cos(theta ) ;
// // //             this.data.Camera_Direction[cameraId].Normalize();
            
// // //             this.data.Camera_Right[cameraId] = Vector3.Cross(data.Camera_Direction[cameraId],data.Camera_Up[cameraId]);
           
// // //         }
// // //     }

// // //     // https://catlikecoding.com/unity/tutorials/movement/orbit-camera/
// // //     internal class Camera
// // //     {
// // //         internal struct Cam
// // //         {
// // //             Matrix4x4 view ;
// // //             Matrix4x4 projection ;
// // //             Matrix4x4 clip;
// // //             Vector3 position ;
// // //             Vector3 direction;//Eye direction ( target ?)
// // //             Vector3 right ;// Strafe
// // //             Vector3 up ;/// head up camera  if y = -1.0f see tete en bas
   
// // //         }

// // //         private readonly Cam _cam ;
// // //         internal ref readonly  Cam SCamera => ref _cam;
        
// // //         private readonly Core.Profiler log = Core.Profilers.GetProfiler(typeof(Camera).FullName);
// // //         private Matrix4x4 view = new Matrix4x4(0.0f);
// // //         private Matrix4x4 projection = new Matrix4x4(0.0f);
// // //         private Matrix4x4 clip = new Matrix4x4(1.0f);
// // //         private Vector3 position = new Vector3(0.0f, 0.0f, 3.0f);
// // //         private Vector3 direction = new Vector3(0.0f, 0.0f, -4.0f);//Eye direction ( target ?)
// // //         private Vector3 right = new Vector3(0.0f, 0.0f, 0.0f);// Strafe
// // //         private Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);/// head up camera  if y = -1.0f see tete en bas

// // //         internal ref readonly Vector3 Position => ref position;
// // //         internal ref readonly Vector3 Direction => ref direction;
// // //         internal ref readonly Vector3 Right => ref right;
// // //         internal ref readonly Vector3 Up => ref up;
// // //         internal ref readonly Matrix4x4 Clip => ref clip;
// // //         internal ref readonly Matrix4x4 ViewMatrix => ref view;
        
// // //         internal float[] View => view.ToArray();
// // //         internal float[] Projection => projection.ToArray();
// // //         internal float[] ProjectionView => clip.ToArray();

// // //         internal Camera()  {   }

// // //         internal void SetPosition(float positionX, float positionY, float positionZ, float directionX, float directionY, float directionZ)
// // //             => (position.X, position.Y, position.Z, direction.X, direction.Y, direction.Z) =
// // //                 (positionX, positionY, positionZ, directionX, directionY, directionZ);

// // //         /// <summary>
// // //         /// Set perspective right Hand 
// // //         /// Shader need to be actived before
// // //         /// </summary>
// // //         /// <param name="name"></param>
// // //         /// <param name="near"></param>
// // //         /// <param name="far"></param>
// // //         /// <param name="FOV"></param>
// // //         internal void SetProjection(CameraType typecam, in int width, in int height, in float near = 0.1f, in float far = 100.0f, in float FOV = 45.0f)
// // //         {
// // //             projection.Identity();
// // //             switch (typecam)
// // //             {
// // //                 case CameraType.OrthoParallel:
// // //                     Project3D.PerspectiveFOV_RHNO(projection, FOV, width, height, near, far);
// // //                     break;
// // //                 case CameraType.Prespective:
// // //                     Project3D.Perspective_RHNO(projection, FOV, (float)width / (float)height, near, far);
// // //                     break;
// // //                 case CameraType.Ortho2D:
// // //                     Project3D.Ortho(ref projection, 0, width, 0, height, near, far);
// // //                     break;

// // //             }
// // //         }

// // //         /// <summary>
// // //         /// Need to call each frame to calculate fustrum 
// // //         /// </summary>
// // //         internal void Update()
// // //         {
// // //             Project3D.LookAtRH( view,
// // //                 position,// camera is here
// // //                 direction /*+ direction */, //// and looks here : at the same position, plus "direction"
// // //                 up);// Head is up (set to 0,-1,0 to look upside-down)

// // //             //Pour le calcul du Frustum
// // //             clip = projection * view;
            
// // //         }
        
// // //         internal void RotateAroundTarget(in Vector3 target, in float horizontalAngledegre, in float verticalAngeDegree)
// // //         {
// // //             //orbit += horizontalAngledegre <= -360 ? 0 : horizontalAngledegre >= 360 ? 0 : horizontalAngledegre;
            
// // //             if (this.position.X == 0) this.position.X = 0.01f;// Mathf.Epsilon;
// // //             if (this.position.Z == 0) this.position.Z = 0.01f;// Mathf.Epsilon;
// // //             if (this.position.Y == 0) this.position.Y = 0.01f;// Mathf.Epsilon;

// // //             //wikipedia calcul coordonee spherique  https://fr.wikipedia.org/wiki/Coordonn%C3%A9es_sph%C3%A9riques
           
// // //             float radius = Vector3.Distance(this.position, target);//X = radius
// // //             float phi = Maths.ATan2(this.position.X, this.position.Z);//Y = polar (azimut ) horizontal angle            
// // //             float theta = Maths.ACos(this.position.Y / radius);// elevation vertical angle

// // //             if (this.position.X < 0) phi += Maths.PI;
           
// // //             phi += horizontalAngledegre;
// // //             theta += verticalAngeDegree;

// // //             phi = Maths.Clampf( phi, -1.5f,1.5f );
            
// // //             var sinustheta = radius * Maths.Sin(theta);
// // //             // calcul coordonee cartesienne avec changment d'angle 
// // //             position.Z = ( sinustheta * Maths.Cos(phi)) + target.Z;
// // //             position.X = ( sinustheta * Maths.Sin(phi)) + target.X;
// // //             position.Y =(theta < 0.0f ? 1 : -1 )* (radius * Maths.Cos(theta)) + target.Y;
// // //             //
// // //             direction.Z =  sinustheta * Maths.Cos(phi + Maths.PI) ;
// // //             direction.X =  sinustheta * Maths.Sin(phi + Maths.PI) ;
// // //             direction.Y = radius * Maths.Cos(theta ) ;
            
// // //             // pense a renormalise Right ???    
// // //             this.right = Vector3.Cross(this.direction, this.up);
            
// // //            // log.Display("ORBIT", log.Args(phi.ArgOf("PHI"), theta.ArgOf("theta"), radius.ArgOf("R"),  direction.ArgOf("Dir"), position.ArgOf("pos")));
// // //         }
// // //         // camera 3D refaire orbt 
        
// // //         // found new position in 3D with spherical 
// // //         // newposition = move( position + new position in X in Y  in Z )
// // //         // Diretion = target ; 
// // //         // (reclarcul Right and Up )
// // //         // look AT ( position , target , up );


// // //         /// <summary>
// // //         /// Rotate around  Z Axis
// // //         /// </summary>
// // //         /// <param name="angle"></param>
// // //         internal void Roll(in float angle)
// // //         {
// // //             // this.roll += angle ;//RotateZ
// // //             this.right = Vector3.Normalize(
// // //                 this.right * Maths.Cos(angle * Maths.PIOVER180) +
// // //                 this.up * Maths.Sin(angle * Maths.PIOVER180)
// // //             );

// // //             this.up = Vector3.Cross(direction, right);
// // //             this.up *= -1;
// // //         }

// // //         /// <summary>
// // //         /// Rotate around X axis
// // //         /// </summary>
// // //         /// <param name="angle"></param>
// // //         internal void Pitch(in float angle)
// // //         {
// // //             // this.pitch += angle  ;// rotateX
// // //             this.direction = Vector3.Normalize(
// // //                 this.direction * Maths.Cos(Maths.DegToRad(angle)) +
// // //                 this.up * Maths.Sin(Maths.DegToRad(angle))
// // //             );

// // //             this.up = Vector3.Cross(direction, right);
// // //             this.up *= -1;
// // //         }

// // //         /// <summary>
// // //         /// Rotate around Y axis with angle in degree
// // //         /// </summary>
// // //         /// <param name="angle"></param>
// // //         internal void Yaw(in float angle)
// // //         {
// // //             // this.yaw += angle  ;//rotateY
// // //             // re-calculate the new forward vector
// // //             this.direction = Vector3.Normalize(
// // //                 this.direction * Maths.Cos(Maths.DegToRad(angle)) -
// // //                 this.right * Maths.Sin(Maths.DegToRad(angle))
// // //             );
// // //             // re-calculate the new right vector
// // //             this.right = Vector3.Cross(this.direction, this.up);
// // //         }

// // //         /// <summary>
// // //         /// // x movement  LEFT RIGHT
// // //         /// </summary>
// // //         /// <param name="distance"></param>
// // //         internal void Strafe(in float distance)
// // //             => this.position += (this.right * distance);

// // //         /// <summary>
// // //         /// // y movement UP DOWN ( for jump ) 
// // //         /// </summary>
// // //         /// <param name="distance"></param>
// // //         internal void Ascend(in float distance)
// // //             => this.position += (this.up * distance);

// // //         /// <summary>
// // //         ///z movement FORWARD BACKWARD 
// // //         /// </summary>
// // //         /// <param name="distance"></param>
// // //         internal void Advance(in float distance)
// // //             => this.position += (this.direction * -distance);

// // //         /// <summary>
// // //         /// deplacement de n pixel pour chaque axe 
// // //         /// x => strafe left right 
// // //         /// y=> ascend jump up down 
// // //         /// z => advance   ( forward, backward )
// // //         /// </summary>
// // //         /// <param name="distance"></param>
// // //         internal void Translate(in Vector3 distance)
// // //             => this.position += (this.direction * distance.Z) + (this.up * distance.Y) + (this.right * distance.X);

// // //         internal void Translate(in float x = 0.0f, in float y = 0.0f, in float z = 0.0f)
// // //             => this.position += (this.direction * z) + (this.up * y) + (this.right * x);

// // //         /// <summary>
// // //         /// axis in degree 0 to 360 (  actuel direction + 0^ ) 180 srr behind actal position  
// // //         /// </summary>
// // //         /// <param name="axis"></param>
// // //         internal void Rotate(in Vector3 axis)
// // //         {
// // //             //Yaw( rotate Y)
// // //             this.direction = Vector3.Normalize(
// // //                 this.direction * Maths.Cos(Maths.DegToRad(axis.Y)) -
// // //                 this.right * Maths.Sin(Maths.DegToRad(axis.Y))
// // //             );

// // //             // Pitch ( rotate X )
// // //             this.direction = Vector3.Normalize(
// // //                 this.direction * Maths.Cos(Maths.DegToRad(axis.X)) +
// // //                 this.up * Maths.Sin(Maths.DegToRad(axis.X))
// // //             );

// // //             this.right = Vector3.Cross(this.direction, this.up);

// // //             // roll ( rotate axe Z)
// // //             this.right = Vector3.Normalize(
// // //                 this.right * Maths.Cos(Maths.DegToRad(axis.Z)) +
// // //                 this.up * Maths.Sin(Maths.DegToRad(axis.Z))
// // //             );

// // //             this.up = Vector3.Cross(direction, right);
// // //             this.up *= -1;
// // //         }

// // //         /// <summary>
// // //         /// axis in degree 0 to 360 (  actuel direction + 0^ ) 180 srr behind actal position  
// // //         /// </summary>
// // //         /// <param name="axis"></param>
// // //         internal void Rotate(in float x = 0.0f, in float y = 0.0f, in float z = 0.0f)
// // //         {
// // //             //Yaw( rotate Y)
// // //             this.direction = Vector3.Normalize(
// // //                 this.direction * Maths.Cos(Maths.DegToRad(y)) -
// // //                 this.right * Maths.Sin(Maths.DegToRad(y))
// // //             );

// // //             // Pitch ( rotate X )
// // //             this.direction = Vector3.Normalize(
// // //                 this.direction * Maths.Cos(Maths.DegToRad(x)) +
// // //                 this.up * Maths.Sin(Maths.DegToRad(x))
// // //             );

// // //             this.right = Vector3.Cross(this.direction, this.up);

// // //             // roll ( rotate axe Z)
// // //             this.right = Vector3.Normalize(
// // //                 this.right * Maths.Cos(Maths.DegToRad(z)) +
// // //                 this.up * Maths.Sin(Maths.DegToRad(z))
// // //             );

// // //             this.up = Vector3.Cross(direction, right);
// // //             this.up *= -1;
// // //         }

// // //         internal void Move(in Vector3 translate, in Vector3 rotate)
// // //         {
// // //             this.position += (this.direction * translate.Z) + (this.up * translate.Y) + (this.right * translate.X);

// // //             //Yaw( rotate Y)
// // //             this.direction = Vector3.Normalize(
// // //                 this.direction * Maths.Cos(Maths.DegToRad(rotate.Y)) -
// // //                 this.right * Maths.Sin(Maths.DegToRad(rotate.Y))
// // //             );

// // //             // Pitch ( rotate X )
// // //             this.direction = Vector3.Normalize(
// // //                 this.direction * Maths.Cos(Maths.DegToRad(rotate.X)) +
// // //                 this.up * Maths.Sin(Maths.DegToRad(rotate.X))
// // //             );

// // //             this.right = Vector3.Cross(this.direction, this.up);

// // //             // roll ( rotate axe Z)
// // //             this.right = Vector3.Normalize(
// // //                 this.right * Maths.Cos(Maths.DegToRad(rotate.Z)) +
// // //                 this.up * Maths.Sin(Maths.DegToRad(rotate.Z))
// // //             );

// // //             this.up = Vector3.Cross(direction, right);
// // //             this.up *= -1;
// // //         }

// // //         internal void Release()
// // //         {

// // //         }

        
// // //         internal override string ToString()
// // //             => "Camera :" + position.ToString();
// // //         internal override int GetHashCode()
// // //             => Core.Utils.Hash;
// // //         internal override bool Equals(object obj)
// // //             => false;

// // //     }


// // // }

// // // // namespace MCJ.Maths
// // // // {

// // // //     using MCJ.Core;

// // // //     internal enum CameraType
// // // //     {
// // // //         Ortho2D = 1,
// // // //         OrthoParallel = 2,
// // // //         Prespective = 3,
// // // //     }

// // // //     internal class Cameras : System.IDisposable
// // // //     {
// // // //         readonly Profiler log = Profilers.GetProfiler(typeof(Cameras).FullName);
// // // //         readonly GameData data;

// // // //         internal Cameras(GameData data)
// // // //         {
// // // //             this.data = data;
// // // //         }

// // // //         #region Gestion Cameras
// // // //         internal void Load( int NummberofCamera = 8)
// // // //         {
// // // //             this.data.Camera_Capacite=NummberofCamera;
// // // //             this.data.Camera_CreatedNumber =0 ; // pas de camera encore creer;
// // // //             this.data.Camera_CurrentActiveIndex =0;
// // // //             this.data.Camera_EndActivesIndex =1;
// // // //             this.data.Camera_StartActivesIndex=0;
// // // //             this.data.Camera.Fars = new RefList<float>(NummberofCamera);
// // // //             this.data.Camera.IsActives = new RefList<bool>(NummberofCamera);
// // // //         }

// // // //         internal int AddCamera( CameraType type,float width, float height, float fov, float near, float far )
// // // //         {
// // // //             this.data.Camera.Fars.Add( far);
            
// // // //             this.data.Camera_CurrentActiveIndex = this.data.Camera.IsActives.Count ;   
// // // //             return this.data.Camera_CurrentActiveIndex;
// // // //         }

// // // //         internal void ChangeActiveCamera( int id, bool inactiveOldCameraId = true)
// // // //         {
// // // //             //check if id > 0 inferieur Camera.Length 
// // // //             this.data.Camera.IsActives[this.data.Camera_CurrentActiveIndex] = !inactiveOldCameraId;
// // // //             this.data.Camera.IsActives[id] = true ;
// // // //             this.data.Camera_CurrentActiveIndex = id ;
// // // //         }

// // // //         internal void SetCamera(int id ,CameraType type, float width, float height, float fov, float near, float far  )
// // // //         {
// // // //             //projection .....  
// // // //             data.Camera.Projections[id].Identity();
// // // //             switch (type)
// // // //             {
// // // //                 case CameraType.OrthoParallel:
// // // //                     Transform.PerspectiveFOV_RHNO(data.Camera.Projections[id], fov, width, height, near, far);
// // // //                     break;
// // // //                 case CameraType.Prespective:
// // // //                     Transform.Perspective_RHNO(data.Camera.Projections[id], fov, (float)width / (float)height, near, far);
// // // //                     break;
// // // //                 case CameraType.Ortho2D:
// // // //                     // Transform.Ortho(ref data.Camera.Projections[id], 0, width, 0, height, near, far);
// // // //                     break;

// // // //             }  
// // // //             UpdateAll();
// // // //         }

// // // //         internal void UpdateAll()
// // // //         {
// // // //             for(int i = data.Camera_StartActivesIndex ; i < data.Camera_EndActivesIndex ; i++ )
// // // //                 Update(i);
            
// // // //         }

// // // //         internal void Update(int id)
// // // //         {
// // // //             Matrix4x4 m = data.Camera.Views[id] ;
// // // //             Transform.LookAtRH(ref m,
// // // //             data.Camera.Positions[id],// camera is here
// // // //             data.Camera.Targets[id] /*+ direction */, //// and looks here : at the same position, plus "direction"
// // // //             data.Camera.Ups[id]);// Head is up (set to 0,-1,0 to look upside-down)

// // // //             data.Camera.ProjectionViews[id] =data.Camera.Projections[id] * data.Camera.Views[id];
// // // //         }


// // // //         internal void Release()
// // // //         {
// // // //             this.data.Camera.Fars.Dispose();
// // // //             this.data.Camera.Fars = null;
// // // //         }

// // // //         internal void Dispose()
// // // //         {
// // // //            Release();
// // // //         }

// // // //         #endregion

// // // //         #region utilisation

// // // //         internal void Move( int idCamera, Vector3 velocite )
// // // //         {
// // // //             //data.Camera.Positions[idCamera] ;
// // // //         }

// // // //         #endregion

// // // //     }


// // // //     internal class Camera
// // // //     {
// // // //         readonly Profiler log = Profilers.GetProfiler(typeof(Camera).FullName);

// // // //         Matrix4x4 view = new Matrix4x4(0.0f);
// // // //         Matrix4x4 projection = new Matrix4x4(0.0f);
// // // //         Matrix4x4 clip = new Matrix4x4(1.0f);
// // // //         Vector3 position = new Vector3(0.0f, 0.0f, 3.0f);
// // // //         Vector3 direction = new Vector3(0.0f, 0.0f, -4.0f);//Eye direction ( target ?)
// // // //         Vector3 right = new Vector3(0.0f, 0.0f, 0.0f);// Strafe
// // // //         Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);/// head up camera  if y = -1.0f see tete en bas

// // // //         internal ref readonly Vector3 Position => ref position;
// // // //         internal ref readonly Vector3 Direction => ref direction;
// // // //         internal ref readonly Vector3 Right => ref right;
// // // //         internal ref readonly Vector3 Up => ref up;
// // // //         internal ref readonly Matrix4x4 Clip => ref clip;

// // // //         internal ref readonly Matrix4x4 ViewMatrix => ref view;
// // // //         internal float[] View => view.ToArray();
// // // //         internal float[] Projection => projection.ToArray();

// // // //         internal float[] ProjectionView => clip.ToArray();

// // // //         internal Camera()
// // // //         {

// // // //         }

// // // //         internal void SetPosition(float positionX, float positionY, float positionZ, float directionX, float directionY, float directionZ)
// // // //             => (position.X, position.Y, position.Z, direction.X, direction.Y, direction.Z) =
// // // //                 (positionX, positionY, positionZ, directionX, directionY, directionZ);

// // // //         /// <summary>
// // // //         /// Set perspective right Hand 
// // // //         /// Shader need to be actived before
// // // //         /// </summary>
// // // //         /// <param name="name"></param>
// // // //         /// <param name="near"></param>
// // // //         /// <param name="far"></param>
// // // //         /// <param name="FOV"></param>
// // // //         internal void SetProjection(CameraType typecam, in int width, in int height, in float near = 0.1f, in float far = 100.0f, in float FOV = 45.0f)
// // // //         {
// // // //             projection.Identity();
// // // //             switch (typecam)
// // // //             {
// // // //                 case CameraType.OrthoParallel:
// // // //                     Transform.PerspectiveFOV_RHNO(projection, FOV, width, height, near, far);
// // // //                     break;
// // // //                 case CameraType.Prespective:
// // // //                     Transform.Perspective_RHNO(projection, FOV, (float)width / (float)height, near, far);
// // // //                     break;
// // // //                 case CameraType.Ortho2D:
// // // //                     Transform.Ortho(ref projection, 0, width, 0, height, near, far);
// // // //                     break;

// // // //             }
// // // //         }


// // // //         /// <summary>
// // // //         /// Need to call each frame to calculate fustrum 
// // // //         /// </summary>
// // // //         internal void Update()
// // // //         {
// // // //             Transform.LookAtRH(ref view,
// // // //                 position,// camera is here
// // // //                 direction /*+ direction */, //// and looks here : at the same position, plus "direction"
// // // //                 up);// Head is up (set to 0,-1,0 to look upside-down)

// // // //             clip = projection * view;
// // // //             //calcul Frustum
// // // //             //Transform.ViewFustrum(data.Frustum, clip);
// // // //         }
        
// // // //         internal void RotateAroundTarget(in Vector3 target, in float horizontalAngledegre, in float verticalAngeDegree)
// // // //         {
// // // //             //orbit += horizontalAngledegre <= -360 ? 0 : horizontalAngledegre >= 360 ? 0 : horizontalAngledegre;
            
// // // //             if (this.position.X == 0) this.position.X = 0.01f;// Mathf.Epsilon;
// // // //             if (this.position.Z == 0) this.position.Z = 0.01f;// Mathf.Epsilon;
// // // //             if (this.position.Y == 0) this.position.Y = 0.01f;// Mathf.Epsilon;

// // // //             //wikipedia calcul coordonee spherique  https://fr.wikipedia.org/wiki/Coordonn%C3%A9es_sph%C3%A9riques
           
// // // //             float radius = Transform.Distance(this.position, target);//X = radius
// // // //             float phi = Maths.ATan2(this.position.X, this.position.Z);//Y = polar (azimut ) horizontal angle            
// // // //             float theta = Maths.ACos(this.position.Y / radius);// elevation vertical angle

// // // //             if (this.position.X < 0) phi += Maths.PI;
           
// // // //             phi += horizontalAngledegre;
// // // //             theta += verticalAngeDegree;

// // // //             phi = Maths.Clampf( phi, -1.5f,1.5f );
            
// // // //             var sinustheta = radius * Maths.Sin(theta);
// // // //             // calcul coordonee cartesienne avec changment d'angle 
// // // //             position.Z = ( sinustheta * Maths.Cos(phi)) + target.Z;
// // // //             position.X = ( sinustheta * Maths.Sin(phi)) + target.X;
// // // //             position.Y =(theta < 0.0f ? 1 : -1 )* (radius * Maths.Cos(theta)) + target.Y;
// // // //             //
// // // //             direction.Z =  sinustheta * Maths.Cos(phi + Maths.PI) ;
// // // //             direction.X =  sinustheta * Maths.Sin(phi + Maths.PI) ;
// // // //             direction.Y = radius * Maths.Cos(theta ) ;
            
// // // //             // pense a renormalise Right ???    
// // // //             this.right = Transform.Cross(this.direction, this.up);
            
// // // //            // log.Display("ORBIT", log.Args(phi.ArgOf("PHI"), theta.ArgOf("theta"), radius.ArgOf("R"),  direction.ArgOf("Dir"), position.ArgOf("pos")));
// // // //         }

// // // //         /// <summary>
// // // //         /// Rotate around  Z Axis
// // // //         /// </summary>
// // // //         /// <param name="angle"></param>
// // // //         internal void Roll(in float angle)
// // // //         {
// // // //             // this.roll += angle ;//RotateZ
// // // //             this.right = Transform.Normalize(
// // // //                 this.right * Maths.Cos(angle * Maths.PIOVER180) +
// // // //                 this.up * Maths.Sin(angle * Maths.PIOVER180)
// // // //             );

// // // //             this.up = Transform.Cross(direction, right);
// // // //             this.up *= -1;
// // // //         }

// // // //         /// <summary>
// // // //         /// Rotate around X axis
// // // //         /// </summary>
// // // //         /// <param name="angle"></param>
// // // //         internal void Pitch(in float angle)
// // // //         {
// // // //             // this.pitch += angle  ;// rotateX
// // // //             this.direction = Transform.Normalize(
// // // //                 this.direction * Maths.Cos(Maths.DegToRad(angle)) +
// // // //                 this.up * Maths.Sin(Maths.DegToRad(angle))
// // // //             );

// // // //             this.up = Transform.Cross(direction, right);
// // // //             this.up *= -1;
// // // //         }

// // // //         /// <summary>
// // // //         /// Rotate around Y axis with angle in degree
// // // //         /// </summary>
// // // //         /// <param name="angle"></param>
// // // //         internal void Yaw(in float angle)
// // // //         {
// // // //             // this.yaw += angle  ;//rotateY
// // // //             // re-calculate the new forward vector
// // // //             this.direction = Transform.Normalize(
// // // //                 this.direction * Maths.Cos(Maths.DegToRad(angle)) -
// // // //                 this.right * Maths.Sin(Maths.DegToRad(angle))
// // // //             );
// // // //             // re-calculate the new right vector
// // // //             this.right = Transform.Cross(this.direction, this.up);
// // // //         }

// // // //         /// <summary>
// // // //         /// // x movement  LEFT RIGHT
// // // //         /// </summary>
// // // //         /// <param name="distance"></param>
// // // //         internal void Strafe(in float distance)
// // // //             => this.position += (this.right * distance);

// // // //         /// <summary>
// // // //         /// // y movement UP DOWN ( for jump ) 
// // // //         /// </summary>
// // // //         /// <param name="distance"></param>
// // // //         internal void Ascend(in float distance)
// // // //             => this.position += (this.up * distance);

// // // //         /// <summary>
// // // //         ///z movement FORWARD BACKWARD 
// // // //         /// </summary>
// // // //         /// <param name="distance"></param>
// // // //         internal void Advance(in float distance)
// // // //             => this.position += (this.direction * -distance);

// // // //         /// <summary>
// // // //         /// deplacement de n pixel pour chaque axe 
// // // //         /// x => strafe left right 
// // // //         /// y=> ascend jump up down 
// // // //         /// z => advance   ( forward, backward )
// // // //         /// </summary>
// // // //         /// <param name="distance"></param>
// // // //         internal void Translate(in Vector3 distance)
// // // //             => this.position += (this.direction * distance.Z) + (this.up * distance.Y) + (this.right * distance.X);

// // // //         internal void Translate(in float x = 0.0f, in float y = 0.0f, in float z = 0.0f)
// // // //             => this.position += (this.direction * z) + (this.up * y) + (this.right * x);

// // // //         /// <summary>
// // // //         /// axis in degree 0 to 360 (  actuel direction + 0^ ) 180 srr behind actal position  
// // // //         /// </summary>
// // // //         /// <param name="axis"></param>
// // // //         internal void Rotate(in Vector3 axis)
// // // //         {
// // // //             //Yaw( rotate Y)
// // // //             this.direction = Transform.Normalize(
// // // //                 this.direction * Maths.Cos(Maths.DegToRad(axis.Y)) -
// // // //                 this.right * Maths.Sin(Maths.DegToRad(axis.Y))
// // // //             );

// // // //             // Pitch ( rotate X )
// // // //             this.direction = Transform.Normalize(
// // // //                 this.direction * Maths.Cos(Maths.DegToRad(axis.X)) +
// // // //                 this.up * Maths.Sin(Maths.DegToRad(axis.X))
// // // //             );

// // // //             this.right = Transform.Cross(this.direction, this.up);

// // // //             // roll ( rotate axe Z)
// // // //             this.right = Transform.Normalize(
// // // //                 this.right * Maths.Cos(Maths.DegToRad(axis.Z)) +
// // // //                 this.up * Maths.Sin(Maths.DegToRad(axis.Z))
// // // //             );

// // // //             this.up = Transform.Cross(direction, right);
// // // //             this.up *= -1;
// // // //         }

// // // //         /// <summary>
// // // //         /// axis in degree 0 to 360 (  actuel direction + 0^ ) 180 srr behind actal position  
// // // //         /// </summary>
// // // //         /// <param name="axis"></param>
// // // //         internal void Rotate(in float x = 0.0f, in float y = 0.0f, in float z = 0.0f)
// // // //         {
// // // //             //Yaw( rotate Y)
// // // //             this.direction = Transform.Normalize(
// // // //                 this.direction * Maths.Cos(Maths.DegToRad(y)) -
// // // //                 this.right * Maths.Sin(Maths.DegToRad(y))
// // // //             );

// // // //             // Pitch ( rotate X )
// // // //             this.direction = Transform.Normalize(
// // // //                 this.direction * Maths.Cos(Maths.DegToRad(x)) +
// // // //                 this.up * Maths.Sin(Maths.DegToRad(x))
// // // //             );

// // // //             this.right = Transform.Cross(this.direction, this.up);

// // // //             // roll ( rotate axe Z)
// // // //             this.right = Transform.Normalize(
// // // //                 this.right * Maths.Cos(Maths.DegToRad(z)) +
// // // //                 this.up * Maths.Sin(Maths.DegToRad(z))
// // // //             );

// // // //             this.up = Transform.Cross(direction, right);
// // // //             this.up *= -1;
// // // //         }

// // // //         internal void Move(in Vector3 translate, in Vector3 rotate)
// // // //         {
// // // //             this.position += (this.direction * translate.Z) + (this.up * translate.Y) + (this.right * translate.X);

// // // //             //Yaw( rotate Y)
// // // //             this.direction = Transform.Normalize(
// // // //                 this.direction * Maths.Cos(Maths.DegToRad(rotate.Y)) -
// // // //                 this.right * Maths.Sin(Maths.DegToRad(rotate.Y))
// // // //             );

// // // //             // Pitch ( rotate X )
// // // //             this.direction = Transform.Normalize(
// // // //                 this.direction * Maths.Cos(Maths.DegToRad(rotate.X)) +
// // // //                 this.up * Maths.Sin(Maths.DegToRad(rotate.X))
// // // //             );

// // // //             this.right = Transform.Cross(this.direction, this.up);

// // // //             // roll ( rotate axe Z)
// // // //             this.right = Transform.Normalize(
// // // //                 this.right * Maths.Cos(Maths.DegToRad(rotate.Z)) +
// // // //                 this.up * Maths.Sin(Maths.DegToRad(rotate.Z))
// // // //             );

// // // //             this.up = Transform.Cross(direction, right);
// // // //             this.up *= -1;
// // // //         }

// // // //         internal void Release()
// // // //         {

// // // //         }

        
// // // //         internal override string ToString()
// // // //             => "Camera :" + position.ToString();
// // // //         internal override int GetHashCode()
// // // //             => Common.Hash;
// // // //         internal override bool Equals(object obj)
// // // //             => false;

// // // //     }
// // // // }


// // // // /*
// // // // SEE Web  for orbit  and trackball
// // // // https://www.songho.ca/opengl/gl_camera.html#example1
// // // // http://tuttlem.github.io/2013/12/30/a-camera-implementation-in-c.html for yaw pitch roll

// // // // https://community.khronos.org/t/implementing-an-orbit-camera/75208
// // // // https://community.khronos.org/t/orbit-around-object/66465/2
// // // // https://nerdhut.de/2020/05/09/unity-arcball-camera-spherical-coordiynates/
// // // // https://forum.unity.com/threads/how-to-move-on-the-surface-of-a-sphere-using-spherical-math-without-rigidbodies-or-raycasts.470041/

// // // // */ -->

