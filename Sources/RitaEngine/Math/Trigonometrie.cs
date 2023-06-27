

namespace RitaEngine.Math;

using RitaEngine.Base;

[ StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static class Transform
{
    
}
//     //  code take on github : https://github.com/FlaxEngine/FlaxAPI/blob/master/FlaxEngine/Math/Viewport.cs
//     // /// <summary>
//     // /// Projects a 3D vector from object space into screen space.
//     // /// </summary>
//     // /// <param name="source">The vector to project.</param>
//     // /// <param name="projection">The projection matrix.</param>
//     // /// <param name="view">The view matrix.</param>
//     // /// <param name="world">The world matrix.</param>
//     // /// <returns>The projected vector.</returns>
//     // internal Vector3 Project(Vector3 source, Matrix projection, Matrix view, Matrix world)
//     // {
//     //     // Matrix matrix;
//     //     // Matrix.Multiply(ref world, ref view, out matrix);
//     //     // Matrix.Multiply(ref matrix, ref projection, out matrix);

//     //     Vector3 vector;
//     //     Project(ref source, ref matrix, out vector);
//     //     return vector;
//     // }

//     // /// <summary>
//     // /// Projects a 3D vector from object space into screen space.
//     // /// </summary>
//     // /// <param name="source">The vector to project.</param>
//     // /// <param name="matrix">A combined WorldViewProjection matrix.</param>
//     // /// <param name="vector">The projected vector.</param>
//     // internal void Project(ref Vector3 source, ref Matrix matrix, out Vector3 vector)
//     // {
//     //     // Vector3.Transform(ref source, ref matrix, out vector);
//     //     // float w = source.X * matrix.M14 + source.Y * matrix.M24 + source.Z * matrix.M34 + matrix.M44;

//     //     // if (!Mathf.IsZero(w))
//     //     // {
//     //     //     vector /= w;
//     //     // }

//     //     // vector.X = (vector.X + 1f) * 0.5f * Width + X;
//     //     // vector.Y = (-vector.Y + 1f) * 0.5f * Height + Y;
//     //     // vector.Z = vector.Z * (MaxDepth - MinDepth) + MinDepth;
//     // }

//     // /// <summary>
//     // /// Converts a screen space point into a corresponding point in world space.
//     // /// </summary>
//     // /// <param name="source">The vector to project.</param>
//     // /// <param name="projection">The projection matrix.</param>
//     // /// <param name="view">The view matrix.</param>
//     // /// <param name="world">The world matrix.</param>
//     // /// <returns>The unprojected Vector.</returns>
//     // internal Vector3 Unproject(Vector3 source, Matrix projection, Matrix view, Matrix world)
//     // {
//     //     Matrix matrix;
//     //     // Matrix.Multiply(ref world, ref view, out matrix);
//     //     // Matrix.Multiply(ref matrix, ref projection, out matrix);
//     //     // Matrix.Invert(ref matrix, out matrix);

//     //     Vector3 vector;
//     //     Unproject(ref source, ref matrix, out vector);
//     //     return vector;
//     // }

//     // /// <summary>
//     // /// Converts a screen space point into a corresponding point in world space.
//     // /// </summary>
//     // /// <param name="source">The vector to project.</param>
//     // /// <param name="matrix">An inverted combined WorldViewProjection matrix.</param>
//     // /// <param name="vector">The unprojected vector.</param>
//     // internal void Unproject(ref Vector3 source, ref Matrix matrix, out Vector3 vector)
//     // {
//     //     // vector.X = (source.X - X) / Width * 2f - 1f;
//     //     // vector.Y = -((source.Y - Y) / Height * 2f - 1f);
//     //     // vector.Z = (source.Z - MinDepth) / (MaxDepth - MinDepth);

//     //     // float w = vector.X * matrix.M14 + vector.Y * matrix.M24 + vector.Z * matrix.M34 + matrix.M44;
//     //     // Vector3.Transform(ref vector, ref matrix, out vector);

//     //     // if (!Mathf.IsZero(w))
//     //     // {
//     //     //     vector /= w;
//     //     // }
//     // }
// }


// // namespace MCJ.Engine.Math
// // {
// //     /// <summary>
// //     /// This is transform like GLM 
// //     /// 
// //     /// </summary>
// //     internal static class Transform
// //     {
// //         /// <summary>
// //         /// 
// //         /// </summary>
// //         /// <param name="m"></param>
// //         /// <param name="v"></param>
// //         /// <returns></returns>
// // 		internal static Matrix4 Translate(Matrix4 m, Vector3 v) 
// // 		{	
// // 			Matrix4 result = new(m);
// // 			result.Translation = m.Right * v[0] +  m.Up * v[1] + m.Forward * v[2] + m.Translation;
// // 			return result;
// // 		}

// // 		/// <summary>
// // 		/// 
// // 		/// </summary>
// // 		/// <param name="m"></param>
// // 		/// <param name="v"></param>
// //         internal static void TranslateRef(ref Matrix4 m ,in Vector3 v)
// //             =>  m.Translation = m.Right * v[0] +  m.Up * v[1] + m.Forward * v[2] + m.Translation;

// // 		/// <summary>
// // 		/// 
// // 		/// </summary>
// // 		/// <param name="m"></param>
// // 		/// <param name="v"></param>
// // 		/// <returns></returns>
// //         internal static Matrix4 Scale(Matrix4 m , Vector3 v)
// //         {
// //             Matrix4 Result = new Matrix4(m);
// //             // Result[0] = m[0] * v[0];
// // 			Result.Right = m.Right * v[0];
// //             // Result[1] = m[1] * v[1];
// // 			Result.Up = m.Up * v[1];
// //             // Result[2] = m[2] * v[2];
// // 			Result.Forward = m.Forward * v[2];
// //             // Result[3] = m[3];
// // 			Result.Translation = m.Translation;
// //             return Result;        
// //         }

// // 		/// <summary>
// // 		/// 
// // 		/// </summary>
// // 		/// <param name="mat"></param>
// // 		/// <param name="v"></param>
// // 		internal static void ScaleRef(ref Matrix4 mat,in  Vector3 v)
// // 			=>(mat.Right,mat.Up , mat.Forward )=( mat.Right * v[0], mat.Up *= v[1], mat.Forward *= v[2]);

// // 		/// <summary>
// // 		/// Rotate around arbitrary axis
// //         /// </summary>
// //         /// <param name="m"></param>
// //         /// <param name="angle">in radians 0 to 2PI </param>
// //         /// <param name="v"></param>
// //         internal static Matrix4 Rotate(Matrix4 m,in float angle,Vector3 v)
// //         {
// //             var c = Maths.Cos(  angle  );
// //             var s = Maths.Sin(  angle  );

// //             Vector3 axis = new(v);
// // 			axis.Normalize();
// //             Vector3 temp = new ((1 - c) * axis);

// //             Matrix4 Rotate = Matrix4.Identite;
// // 			Rotate[0,0] = c + temp[0] * axis[0];
// // 			Rotate[0,1] = temp[0] * axis[1] + s * axis[2];
// // 			Rotate[0,2] = temp[0] * axis[2] - s * axis[1];

// // 			Rotate[1,0] = temp[1] * axis[0] - s * axis[2];
// // 			Rotate[1,1] = c + temp[1] * axis[1];
// // 			Rotate[1,2] = temp[1] * axis[2] + s * axis[0];

// // 			Rotate[2,0] = temp[2] * axis[0] + s * axis[1];
// // 			Rotate[2,1] = temp[2] * axis[1] - s * axis[0];
// // 			Rotate[2,2] = c + temp[2] * axis[2];

// // 			Matrix4 Result = Matrix4.Identite;
// // 			// Result[0] = m[0] * Rotate[0,0] + m[1] * Rotate[0,1] + m[2] * Rotate[0,2];
// // 			Result.Right = m.Right * Rotate[0,0] + m.Up * Rotate[0,1] + m.Forward * Rotate[0,2];
// // 			// Result[1] = m[0] * Rotate[1,0] + m[1] * Rotate[1,1] + m[2] * Rotate[1,2];
// // 			Result.Up = m.Right * Rotate[1,0] + m.Up * Rotate[1,1] + m.Forward * Rotate[1,2];
// // 			// Result[2] = m[0] * Rotate[2,0] + m[1] * Rotate[2,1] + m[2] * Rotate[2,2];
// // 			Result.Forward = m.Right * Rotate[2,0] + m.Up * Rotate[2,1] + m.Forward * Rotate[2,2];
// // 			// Result[3] = m[3];
// // 			Result.Translation = m.Translation ;
// // 			return Result;
// //         }


// // //	// 		Matririx PlayerAbsoluteTransform;https://gamedev.stackexchange.com/questions/27966/rotating-a-model-and-translating-it-forward-in-xna
// // // void UpdatePlayer( Vector3 EnemyPosition, float PlayerVelocity, float Seconds)
// // // {
// // //      Vector3 PlayerPosition = PlayerAbsoluteTransform.Translation;
// // //      Vector3 Forward = EnemyPosition - PlayerPosition;
// // //      Forward.Normalize();

// // //      // This moves your player towars the enemy
// // //      PlayerPosition += Forward * PlayerVelocity * Seconds; 

// // //      // This create the transform matrix for your model, 
// // //      // note that maybe you have to rotate the model before to face right
// // //      PlayerAbsoluteTransform = Matrix.CreateWorld(PlayerPosition, Forward, PlayerUp);
// // // }
	

		

// // #region Clip_Space

// // 		// internal static Matrix4 Perspective(float fovydegree, float aspect, float zNear, float zFar , bool LH =false , bool NO = false)
// // 		// {
// // 		// 	float tanHalfFovy =Functions.Tan( Functions.DegreesToRadians(fovydegree)  /2 );

// // 		// 	Matrix4x4 Result = new Matrix4x4(0.0f);
// // 		// 	Result[0][0] = 1.0f / (aspect * tanHalfFovy);
// // 		// 	Result[1][1] = 1.0f / (tanHalfFovy);
// // 		// 	Result[2][2] =NO ?  - (zFar + zNear) / (zFar - zNear) :   zFar / (zNear - zFar);
// // 		// 	Result[2][3] =LH? 1.0f : - 1.0f;
// // 		// 	Result[3][2] =NO ?  - (2.0f * zFar * zNear) / (zFar - zNear) : -(zFar * zNear) / (zFar - zNear);
// // 		// 	return Result;
// // 		// }

// // 	// 	internal static void PerspectiveTest(ref Matrix4x4 pers,  float fovydegree, float aspect, float zNear, float zFar , bool LH =false , bool NO = false)
// // 	// 	{
// // 	// 		float tanHalfFovy =Functions.Tan( Functions.DegreesToRadians(fovydegree)  /2 );

// // 	// 		//Matrix4x4 Result = new Matrix4x4(0.0f);
// // 	// 		pers[0][0] = 1.0f / (aspect * tanHalfFovy);
// // 	// 		pers[1][1] = 1.0f / (tanHalfFovy);
// // 	// 		pers[2][2] =NO ?  - (zFar + zNear) / (zFar - zNear) :   zFar / (zNear - zFar);
// // 	// 		pers[2][3] =LH? 1.0f : - 1.0f;
// // 	// 		pers[3][2] =NO ?  - (2.0f * zFar * zNear) / (zFar - zNear) : -(zFar * zNear) / (zFar - zNear);
// // 	// 		//return Result;
// // 	// 	}

// // 	// 	internal static void PerspectiveTestR( Matrix4x4 pers,  float fovydegree, float aspect, float zNear, float zFar , bool LH =false , bool NO = false)
// // 	// 	{
// // 	// 		float tanHalfFovy =Functions.Tan( Functions.DegreesToRadians(fovydegree)  /2 );

// // 	// 		//Matrix4x4 Result = new Matrix4x4(0.0f);
// // 	// 		pers[0][0] = 1.0f / (aspect * tanHalfFovy);
// // 	// 		pers[1][1] = 1.0f / (tanHalfFovy);
// // 	// 		pers[2][2] =NO ?  - (zFar + zNear) / (zFar - zNear) :   zFar / (zNear - zFar);
// // 	// 		pers[2][3] =LH? 1.0f : - 1.0f;
// // 	// 		pers[3][2] =NO ?  - (2.0f * zFar * zNear) / (zFar - zNear) : -(zFar * zNear) / (zFar - zNear);
// // 	// 		//return Result;
// // 	// 	}


// //     //     internal static Matrix4x4 frustum( float left , float right , float bottom , float top , float nearVal , float farVal , bool NO = true , bool LH = true)
// //     //     {
// //     //             //TODO différence en tre NO et ZO;
// //     //         float lh = LH ? 1.0f : -1.0f ; 
// //     //         // FOR LH 

// //     //         Matrix4x4 Result = new Matrix4x4(1.0f);
// //     //         Result[0][0] = ( 2.0f * nearVal) / (right - left);
// //     //         Result[1][1] = ( 2.0f * nearVal) / (top - bottom);
// //     //         Result[2][0] = (right + left) / (right - left);
// //     //         Result[2][1] = (top + bottom) / (top - bottom);
// //     //         Result[2][2] =NO ? lh * (farVal + nearVal) / (farVal - nearVal)  : farVal / (farVal - nearVal)  ;
// //     //         Result[3][2] =NO ?  - ( 2.0f * farVal * nearVal) / (farVal - nearVal) : -(farVal * nearVal) / (farVal - nearVal)  ;
// //     //         Result[2][3] =lh;            
// //     //         return Result;
// //     //     }

// //     // internal static Matrix4x4 Ortho( float left , float right , float bottom , float top , float zNear , float zFar , bool NO = true , bool LH = true)
// //     // {
// //     //     Matrix4x4 Result = new Matrix4x4(1.0f);

// //     //     Result[0][0] =  2.0f / (right - left);
// //     //     Result[1][1] =  2.0f / (top - bottom);
// //     //     Result[2][2] =NO ? 2.0f / (zFar - zNear) :   1.0f / (zFar - zNear);
// //     //     Result[3][0] = - (right + left) / (right - left);
// //     //     Result[3][1] = - (top + bottom) / (top - bottom);
// //     //     Result[3][2] =NO ?  - (zFar + zNear) / (zFar - zNear) : - zNear / (zFar - zNear);
// //     //     return Result;
// //     // }

// // 	// internal static Matrix4x4  ortho2D(float left, float right, float bottom, float top)
// // 	// {
// // 	// 	Matrix4x4 Result = new Matrix4x4(1.0f);
// // 	// 	Result[0][0] = 2.0f / (right - left);
// // 	// 	Result[1][1] = 2.0f / (top - bottom);
// // 	// 	Result[2][2] = - 1.0f;
// // 	// 	Result[3][0] = - (right + left) / (right - left);
// // 	// 	Result[3][1] = - (top + bottom) / (top - bottom);
// // 	// 	return Result;
// // 	// }

// // /*




// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> perspectiveFovRH_ZO(T fov, T width, T height, T zNear, T zFar)
// // 	{
// // 		assert(width > static_cast<T>(0));
// // 		assert(height > static_cast<T>(0));
// // 		assert(fov > static_cast<T>(0));

// // 		T const rad = fov;
// // 		T const h = glm::cos(static_cast<T>(0.5) * rad) / glm::sin(static_cast<T>(0.5) * rad);
// // 		T const w = h * height / width; ///todo max(width , Height) / min(width , Height)?

// // 		mat<4, 4, T, defaultp> Result(static_cast<T>(0));
// // 		Result[0][0] = w;
// // 		Result[1][1] = h;
// // 		Result[2][2] = zFar / (zNear - zFar);
// // 		Result[2][3] = - static_cast<T>(1);
// // 		Result[3][2] = -(zFar * zNear) / (zFar - zNear);
// // 		return Result;
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> perspectiveFovRH_NO(T fov, T width, T height, T zNear, T zFar)
// // 	{
// // 		assert(width > static_cast<T>(0));
// // 		assert(height > static_cast<T>(0));
// // 		assert(fov > static_cast<T>(0));

// // 		T const rad = fov;
// // 		T const h = glm::cos(static_cast<T>(0.5) * rad) / glm::sin(static_cast<T>(0.5) * rad);
// // 		T const w = h * height / width; ///todo max(width , Height) / min(width , Height)?

// // 		mat<4, 4, T, defaultp> Result(static_cast<T>(0));
// // 		Result[0][0] = w;
// // 		Result[1][1] = h;
// // 		Result[2][2] = - (zFar + zNear) / (zFar - zNear);
// // 		Result[2][3] = - static_cast<T>(1);
// // 		Result[3][2] = - (static_cast<T>(2) * zFar * zNear) / (zFar - zNear);
// // 		return Result;
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> perspectiveFovLH_ZO(T fov, T width, T height, T zNear, T zFar)
// // 	{
// // 		assert(width > static_cast<T>(0));
// // 		assert(height > static_cast<T>(0));
// // 		assert(fov > static_cast<T>(0));

// // 		T const rad = fov;
// // 		T const h = glm::cos(static_cast<T>(0.5) * rad) / glm::sin(static_cast<T>(0.5) * rad);
// // 		T const w = h * height / width; ///todo max(width , Height) / min(width , Height)?

// // 		mat<4, 4, T, defaultp> Result(static_cast<T>(0));
// // 		Result[0][0] = w;
// // 		Result[1][1] = h;
// // 		Result[2][2] = zFar / (zFar - zNear);
// // 		Result[2][3] = static_cast<T>(1);
// // 		Result[3][2] = -(zFar * zNear) / (zFar - zNear);
// // 		return Result;
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> perspectiveFovLH_NO(T fov, T width, T height, T zNear, T zFar)
// // 	{
// // 		assert(width > static_cast<T>(0));
// // 		assert(height > static_cast<T>(0));
// // 		assert(fov > static_cast<T>(0));

// // 		T const rad = fov;
// // 		T const h = glm::cos(static_cast<T>(0.5) * rad) / glm::sin(static_cast<T>(0.5) * rad);
// // 		T const w = h * height / width; ///todo max(width , Height) / min(width , Height)?

// // 		mat<4, 4, T, defaultp> Result(static_cast<T>(0));
// // 		Result[0][0] = w;
// // 		Result[1][1] = h;
// // 		Result[2][2] = (zFar + zNear) / (zFar - zNear);
// // 		Result[2][3] = static_cast<T>(1);
// // 		Result[3][2] = - (static_cast<T>(2) * zFar * zNear) / (zFar - zNear);
// // 		return Result;
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> perspectiveFovZO(T fov, T width, T height, T zNear, T zFar)
// // 	{
// // #		if GLM_CONFIG_CLIP_CONTROL & GLM_CLIP_CONTROL_LH_BIT
// // 			return perspectiveFovLH_ZO(fov, width, height, zNear, zFar);
// // #		else
// // 			return perspectiveFovRH_ZO(fov, width, height, zNear, zFar);
// // #		endif
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> perspectiveFovNO(T fov, T width, T height, T zNear, T zFar)
// // 	{
// // #		if GLM_CONFIG_CLIP_CONTROL & GLM_CLIP_CONTROL_LH_BIT
// // 			return perspectiveFovLH_NO(fov, width, height, zNear, zFar);
// // #		else
// // 			return perspectiveFovRH_NO(fov, width, height, zNear, zFar);
// // #		endif
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> perspectiveFovLH(T fov, T width, T height, T zNear, T zFar)
// // 	{
// // #		if GLM_CONFIG_CLIP_CONTROL & GLM_CLIP_CONTROL_ZO_BIT
// // 			return perspectiveFovLH_ZO(fov, width, height, zNear, zFar);
// // #		else
// // 			return perspectiveFovLH_NO(fov, width, height, zNear, zFar);
// // #		endif
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> perspectiveFovRH(T fov, T width, T height, T zNear, T zFar)
// // 	{
// // #		if GLM_CONFIG_CLIP_CONTROL & GLM_CLIP_CONTROL_ZO_BIT
// // 			return perspectiveFovRH_ZO(fov, width, height, zNear, zFar);
// // #		else
// // 			return perspectiveFovRH_NO(fov, width, height, zNear, zFar);
// // #		endif
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> perspectiveFov(T fov, T width, T height, T zNear, T zFar)
// // 	{
// // #		if GLM_CONFIG_CLIP_CONTROL == GLM_CLIP_CONTROL_LH_ZO
// // 			return perspectiveFovLH_ZO(fov, width, height, zNear, zFar);
// // #		elif GLM_CONFIG_CLIP_CONTROL == GLM_CLIP_CONTROL_LH_NO
// // 			return perspectiveFovLH_NO(fov, width, height, zNear, zFar);
// // #		elif GLM_CONFIG_CLIP_CONTROL == GLM_CLIP_CONTROL_RH_ZO
// // 			return perspectiveFovRH_ZO(fov, width, height, zNear, zFar);
// // #		elif GLM_CONFIG_CLIP_CONTROL == GLM_CLIP_CONTROL_RH_NO
// // 			return perspectiveFovRH_NO(fov, width, height, zNear, zFar);
// // #		endif
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> infinitePerspectiveRH(T fovy, T aspect, T zNear)
// // 	{
// // 		T const range = tan(fovy / static_cast<T>(2)) * zNear;
// // 		T const left = -range * aspect;
// // 		T const right = range * aspect;
// // 		T const bottom = -range;
// // 		T const top = range;

// // 		mat<4, 4, T, defaultp> Result(static_cast<T>(0));
// // 		Result[0][0] = (static_cast<T>(2) * zNear) / (right - left);
// // 		Result[1][1] = (static_cast<T>(2) * zNear) / (top - bottom);
// // 		Result[2][2] = - static_cast<T>(1);
// // 		Result[2][3] = - static_cast<T>(1);
// // 		Result[3][2] = - static_cast<T>(2) * zNear;
// // 		return Result;
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> infinitePerspectiveLH(T fovy, T aspect, T zNear)
// // 	{
// // 		T const range = tan(fovy / static_cast<T>(2)) * zNear;
// // 		T const left = -range * aspect;
// // 		T const right = range * aspect;
// // 		T const bottom = -range;
// // 		T const top = range;

// // 		mat<4, 4, T, defaultp> Result(T(0));
// // 		Result[0][0] = (static_cast<T>(2) * zNear) / (right - left);
// // 		Result[1][1] = (static_cast<T>(2) * zNear) / (top - bottom);
// // 		Result[2][2] = static_cast<T>(1);
// // 		Result[2][3] = static_cast<T>(1);
// // 		Result[3][2] = - static_cast<T>(2) * zNear;
// // 		return Result;
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> infinitePerspective(T fovy, T aspect, T zNear)
// // 	{
// // #		if GLM_CONFIG_CLIP_CONTROL & GLM_CLIP_CONTROL_LH_BIT
// // 			return infinitePerspectiveLH(fovy, aspect, zNear);
// // #		else
// // 			return infinitePerspectiveRH(fovy, aspect, zNear);
// // #		endif
// // 	}

// // 	// Infinite projection matrix: http://www.terathon.com/gdc07_lengyel.pdf
// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> tweakedInfinitePerspective(T fovy, T aspect, T zNear, T ep)
// // 	{
// // 		T const range = tan(fovy / static_cast<T>(2)) * zNear;
// // 		T const left = -range * aspect;
// // 		T const right = range * aspect;
// // 		T const bottom = -range;
// // 		T const top = range;

// // 		mat<4, 4, T, defaultp> Result(static_cast<T>(0));
// // 		Result[0][0] = (static_cast<T>(2) * zNear) / (right - left);
// // 		Result[1][1] = (static_cast<T>(2) * zNear) / (top - bottom);
// // 		Result[2][2] = ep - static_cast<T>(1);
// // 		Result[2][3] = static_cast<T>(-1);
// // 		Result[3][2] = (ep - static_cast<T>(2)) * zNear;
// // 		return Result;
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> tweakedInfinitePerspective(T fovy, T aspect, T zNear)
// // 	{
// // 		return tweakedInfinitePerspective(fovy, aspect, zNear, epsilon<T>());
// // 	}

// // */

// // #endregion


// //         // internal static void LookAtLH(ref Matrix4x4 result,Vector3 position, Vector3 target, Vector3 up)
// //         // {   
// //         //     direction = Normalize(position - target);
// //         //     right = Normalize(Cross(up, direction));
// //         //     camUp = Normalize(Cross(direction, right));

            
// // 		// 	result.Identity();

// // 		// 	result[0][0] = right.X;
// // 		// 	result[1][0] = right.Y;
// // 		// 	result[2][0] = right.Z;
// // 		// 	result[0][1] = camUp.X;
// // 		// 	result[1][1] = camUp.Y;
// // 		// 	result[2][1] = camUp.Z;
// // 		// 	result[0][2] = direction.X;
// // 		// 	result[1][2] = direction.Y;
// // 		// 	result[2][2] = direction.Z;
// // 		// 	result[3][0] = -Dot(right, position);
// // 		// 	result[3][1] = -Dot(camUp, position);
// // 		// 	result[3][2] = -Dot(direction, position);
// //         // }

// //         // private static Vector3 direction = new Vector3(0.0f,0.0f,0.0f);
// //         // private static Vector3 right = new Vector3(0.0f,0.0f,0.0f);
// //         // private static Vector3 camUp = new Vector3(0.0f,0.0f,0.0f);


// //         // /// <summary>
// //         // /// Opengl
// //         // /// </summary>
// //         // /// <param name="result"></param>
// //         // /// <param name="position"></param>
// //         // /// <param name="target"></param>
// //         // /// <param name="up"></param>
// //         // internal static void LookAtRH(ref Matrix4x4 result,Vector3 position, Vector3 target, Vector3 up)
// //         // {   
// //         //     direction = Normalize( target - position);
// //         //     right = Normalize( Cross(direction,up ) );//camera right
// //         //     camUp = Cross(right,direction); //up
            
// // 		// 	result.Identity();

// // 		// 	result[0][0] = right.X;
// // 		// 	result[1][0] = right.Y;
// // 		// 	result[2][0] = right.Z;
// // 		// 	result[0][1] = camUp.X;
// // 		// 	result[1][1] = camUp.Y;
// // 		// 	result[2][1] = camUp.Z;
// // 		// 	result[0][2] = -direction.X;
// // 		// 	result[1][2] = -direction.Y;
// // 		// 	result[2][2] = -direction.Z;
// // 		// 	result[3][0] = -Dot(right, position);
// // 		// 	result[3][1] = -Dot(camUp, position);
// // 		// 	result[3][2] = Dot(direction, position);
// //         // }

// //         // internal static float Dot(Vector2  v1, Vector2  v2) {
// // 		// 	return  v1.X * v2.X + v1.Y * v2.Y;
// // 		// }

// //         // internal static float Dot(Vector3  v1, Vector3  v2) {
// // 		// 	return  v1.X * v2.X + v1.Y * v2.Y+ v1.Z * v2.Z;
// // 		// }

// //         // internal static float Dot(Vector4  v1, Vector4  v2) {
// // 		// 	return  v1.x * v2.x + v1.y * v2.y + v1.z * v2.z + v1.w*v2.w;
// // 		// }
		
		
// // 		// internal static Vector3 Cross(Vector3 v1, Vector3 v2) 
// //         // {
// //         //      return new Vector3(v1.Y * v2.Z - v1.Z * v2.Y, 
// //         //         v1.Z * v2.X - v1.X * v2.Z,
// //         //         v1.X * v2.Y - v1.Y * v2.X);
// // 		// }

// //         // internal static void CrossR(ref Vector3 result, Vector3 v1, Vector3 v2) 
// //         // {
// //         //     result.X = v1.Y * v2.Z - v1.Z * v2.Y;
// //         //     result.Y = v1.Z * v2.X - v1.X * v2.Z ;
// //         //     result.Z = v1.X * v2.Y - v1.Y * v2.X;
// // 		// }

// // 		// internal static Vector4 Cross(Vector4 x, Vector4 y) {
// // 		// 	return new Vector4(
// // 		// 		x.y * y.z - x.z * y.y,
// // 		// 		x.z * y.x - x.x * y.z,
// // 		// 		x.x * y.y - x.y * y.x,
// // 		// 		x.w * y.z - x.z);
// // 		// }

// // 		// internal static Vector3 Normalize(Vector3 x) {
// //         //     float length =1/ x.Length();
// //         //     return new Vector3(x.X*length,x.Y*length,x.Z*length);
// // 		// }



// //         // internal static Vector4 Normalize(Vector4 x) {
// // 		// 	float length = 1/ x.Length;
// //         //     return new Vector4( x.x*length, x.y*length, x.z*length , x.w*length); 
// // 		// }
// //     }
// // }


// // // #region Transform Object in space   
// // //     internal static void Translate(ref Matrix4x4 m ,in Vector3 v)
// // //         => m[3] += (m[0] * v[0] + m[1] * v[1] + m[2] * v[2]  );

// // //     internal static void Scale(ref Matrix4x4 mat, Vector3 v)
// // //     {
// // //         mat[0] *= v[0];
// // //         mat[1] *= v[1];
// // //         mat[2] *= v[2];
// // //     }

// // //     /// <summary>
// // //     /// 
// // //     /// </summary>
// // //     /// <param name="m"></param>
// // //     /// <param name="angle">in degree 0 to 360° </param>
// // //     /// <param name="axis"></param>
// // //     internal static void Rotate(ref  Matrix4x4 m,in float angle,in Vector3 axis) 
// // //     {
// // //         var c = Maths.Cos( Maths.DegToRad( angle ) );
// // //         var s = Maths.Sin( Maths.DegToRad( angle ) );

// // //         axis.Normalize();
// // //         Vector3 temp = new Vector3((1 - c) * axis);

// // //         Matrix4x4 Rotate = new Matrix4x4(1.0f);
// // //         Rotate[0,0] = c + temp[0] * axis[0];
// // //         Rotate[0,1] = temp[0] * axis[1] + s * axis[2];
// // //         Rotate[0,2] = temp[0] * axis[2] - s * axis[1];

// // //         Rotate[1,0] = temp[1] * axis[0] - s * axis[2];
// // //         Rotate[1,1] = c + temp[1] * axis[1];
// // //         Rotate[1,2] = temp[1] * axis[2] + s * axis[0];

// // //         Rotate[2,0] = temp[2] * axis[0] + s * axis[1];
// // //         Rotate[2,1] = temp[2] * axis[1] - s * axis[0];
// // //         Rotate[2,2] = c + temp[2] * axis[2];

// // //         m[0] = new Vector4(m[0] * Rotate[0][0] + m[1] * Rotate[0][1] + m[2] * Rotate[0][2]); // row 1
// // //         m[1] = new Vector4(m[0] * Rotate[1][0] + m[1] * Rotate[1][1] + m[2] * Rotate[1][2]); // row 2
// // //         m[2] = new Vector4(m[0] * Rotate[2][0] + m[1] * Rotate[2][1] + m[2] * Rotate[2][2]); // row 3
// // //     }

    
// // //     /// <summary>
// // //     /// 
// // //     /// </summary>
// // //     /// <param name="result"></param>
// // //     /// <param name="angle">in degree 0 to 360 </param>
// // //     internal static void RotationX(ref Matrix4x4 result , in float angle)
// // //     {
// // //         float cosx = Maths.Cos( Maths.DegToRad( angle ) );
// // //         float sinx = Maths.Sin(Maths.DegToRad( angle ));

// // //         result.Identity();
// // //         result[1][1] = cosx;
// // //         result[1][2] = sinx;
// // //         result[2][1] = -sinx;
// // //         result[2][2] = cosx;
// // //     }
// // //     internal static void RotationY(ref Matrix4x4 result,in float angle )
// // //     {
// // //         float cosy = Maths.Cos(Maths.DegToRad( angle ));
// // //         float siny = Maths.Sin(Maths.DegToRad( angle ));

// // //         result.Identity();
// // //         result[0][0] = cosy;
// // //         result[0][2] = -siny;
// // //         result[2][0] = siny;
// // //         result[2][2] = cosy;
// // //     }
    
// // //     internal static void RotationZ(ref Matrix4x4 result ,in float angle )
// // //     {
// // //         float cosz = Maths.Cos(Maths.DegToRad( angle ));
// // //         float sinz = Maths.Sin(Maths.DegToRad( angle ));

// // //         result.Identity();
// // //         result[0][0] = cosz;
// // //         result[0][1] = sinz;
// // //         result[1][0] = -sinz;
// // //         result[1][1] = cosz;
// // //     }

// // //     internal static void RotationXYZ(ref Matrix4x4 result , in float angleX, in float angleY, in float angleZ)
// // //     {
// // //         //result.Identity();
// // //         float cosx = Maths.Cos( Maths.DegToRad( angleX ) );
// // //         float sinx = Maths.Sin(Maths.DegToRad( angleX ));

// // //         result[1][1] = cosx;
// // //         result[1][2] = sinx;
// // //         result[2][1] = -sinx;
// // //         result[2][2] = cosx;

// // //         float cosy = Maths.Cos(Maths.DegToRad( angleY ));
// // //         float siny = Maths.Sin(Maths.DegToRad( angleY ));

// // //         // result.Identity();
// // //         result[0][0] = cosy;
// // //         result[0][2] = -siny;
// // //         result[2][0] = siny;
// // //         result[2][2] *= cosy;

// // //         float cosz = Maths.Cos(Maths.DegToRad( angleZ ));
// // //         float sinz = Maths.Sin(Maths.DegToRad( angleZ ));

// // //         //result.Identity();
// // //         result[0][0] *= cosz;
// // //         result[0][1] = sinz;
// // //         result[1][0] = -sinz;
// // //         result[1][1] *= cosz;
// // //     }

// // //     /// <summary>
// // //     /// Realise Translate , scale and rotate of an object in world
// // //     /// result is in model
// // //     /// </summary>
// // //     /// <param name="model"></param>
// // //     /// <param name="translate"></param>
// // //     /// <param name="rotate"></param>
// // //     /// <param name="angle"></param>
// // //     /// <param name="scale"></param>
// // //     internal static void MoveObject(ref Matrix4x4 model, Vector3 translate, Vector3 axisRotate, float angle, Vector3 scale )
// // //     {
// // //         model.Identity();
// // //         Translate(ref model,translate);
// // //         Rotate(ref model,angle, axisRotate);
// // //         Scale(ref model, scale);
// // //     }

// // // #endregion

// // // #region Project unproject pickMatrix
// // //     internal static Vector4 MultiplyMat4ByVec4(in Matrix4x4 matrix ,in  Vector4 value )
// // //     {
// // //         Vector4 result = new Vector4();
// // //         result.X = value.X * matrix[0][0] + value.Y* matrix[0][1] + value.Z*matrix[0][2] + value.W * matrix[0][3] ;
// // //         result.Y = value.X * matrix[1][0] + value.Y* matrix[1][1] + value.Z*matrix[1][2] + value.W * matrix[1][3] ;
// // //         result.Z = value.X * matrix[2][0] + value.Y* matrix[2][1] + value.Z*matrix[2][2] + value.W * matrix[2][3] ;
// // //         result.W = value.X * matrix[3][0] + value.Y* matrix[3][1] + value.Z*matrix[3][2] + value.W * matrix[3][3] ;
// // //         return result ;
// // //     }

// // //     internal static void MultiplyMat4ByVec4(ref Vector4 result,in Matrix4x4 matrix ,in Vector4 value )
// // //     {
// // //         result.X = value.X * matrix[0][0] + value.Y* matrix[0][1] + value.Z*matrix[0][2] + value.W * matrix[0][3] ;
// // //         result.Y = (value.X * matrix[1][0]) + value.Y* matrix[1][1] + value.Z*matrix[1][2] + value.W * matrix[1][3] ;
// // //         result.Z = value.X * matrix[2][0] + value.Y* matrix[2][1] + value.Z*matrix[2][2] + value.W * matrix[2][3] ;
// // //         result.W = value.X * matrix[3][0] + value.Y* matrix[3][1] + value.Z*matrix[3][2] + value.W * matrix[3][3] ;
// // //     }

// // //     internal static Vector4 MultiplyVec4ByMat4(in Vector4 value,in  Matrix4x4 matrix)
// // //         => new Vector4
// // //         {
// // //             X = value.X * matrix[0][0] + value.Y * matrix[1][0] + value.Z * matrix[2][0] + value.W * matrix[3][0],
// // //             Y = value.X * matrix[0][1] + value.Y * matrix[1][1] + value.Z * matrix[2][1] + value.W * matrix[3][1],
// // //             Z = value.X * matrix[0][2] + value.Y * matrix[1][2] + value.Z * matrix[2][2] + value.W * matrix[3][2],
// // //             W = value.X * matrix[0][3] + value.Y * matrix[1][3] + value.Z * matrix[2][3] + value.W * matrix[3][3]
// // //         };
    
// // //     internal static Matrix4x4 MultiplyMat4byMat4(in Matrix4x4 m1 ,in Matrix4x4 m2)
// // //     {
// // //         Matrix4x4 result = new Matrix4x4(1.0f);
// // //         result[0] = m1[0] * m2[0][0] + m1[1] * m2[0][1] + m1[2] * m2[0][2] + m1[3] * m2[0][3];
// // //         result[1] = m1[0] * m2[1][0] + m1[1] * m2[1][1] + m1[2] * m2[1][2] + m1[3] * m2[1][3];
// // //         result[2] = m1[0] * m2[2][0] + m1[1] * m2[2][1] + m1[2] * m2[2][2] + m1[3] * m2[2][3];
// // //         result[3] = m1[0] * m2[3][0] + m1[1] * m2[3][1] + m1[2] * m2[3][2] + m1[3] * m2[3][3];
// // //         return result ;
// // //     }

// // //     internal static void MultiplyMat4byMat4(ref Matrix4x4 result ,in  Matrix4x4 m1 ,in Matrix4x4 m2)
// // //     {
// // //         result[0] = m1[0] * m2[0][0] + m1[1] * m2[0][1] + m1[2] * m2[0][2] + m1[3] * m2[0][3];
// // //         result[1] = m1[0] * m2[1][0] + m1[1] * m2[1][1] + m1[2] * m2[1][2] + m1[3] * m2[1][3];
// // //         result[2] = m1[0] * m2[2][0] + m1[1] * m2[2][1] + m1[2] * m2[2][2] + m1[3] * m2[2][3];
// // //         result[3] = m1[0] * m2[3][0] + m1[1] * m2[3][1] + m1[2] * m2[3][2] + m1[3] * m2[3][3];
// // //     }

// // //     /// Map the specified object coordinates (obj.x, obj.y, obj.z) into window coordinates.
// // // 	/// The near and far clip planes correspond to z normalized device coordinates of -1 and +1 respectively. (OpenGL clip volume definition)
// // // 	///
// // // 	/// @param obj Specify the object coordinates.
// // // 	/// @param model Specifies the current modelview matrix
// // // 	/// @param proj Specifies the current projection matrix
// // // 	/// @param viewport Specifies the current viewport
// // // 	/// @return Return the computed window coordinates.
// // // 	/// @tparam T Native type used for the computation. Currently supported: half (not recommended), float or double.
// // // 	/// @tparam U Currently supported: Floating-point types and integer types.
// // // 	///
// // // 	/// @see <a href="https://www.khronos.org/registry/OpenGL-Refpages/gl2.1/xhtml/gluProject.xml">gluProject man page</a>
// // //     internal static Vector4 ProjectNO(in Vector3 objPosition,in Matrix4x4 modelview,in Matrix4x4 projection ,in Vector4 viewport)
// // //     {
// // //         Vector4 tmp = new Vector4(objPosition.X, objPosition.Y, objPosition.Z,1.0f);
// // //         tmp = MultiplyMat4ByVec4( modelview ,tmp); //tmp = model * tmp;
// // //         tmp = MultiplyMat4ByVec4( projection, tmp);//tmp = projection * tmp; 

// // //         tmp /= tmp.W;
// // //         tmp = tmp * 0.5f  + 0.5f;
// // //         // tmp.Y = tmp.Y * 0.5f +0.5f;
// // //         // tmp.X = tmp.X * 0.5f +0.5f;
// // //         tmp[0] = tmp[0] *  viewport[2] + viewport[0];
// // //         tmp[1] = tmp[1] *  viewport[3] + viewport[1];
// // //         return tmp;
// // //     }

// // //     internal static void Invert(in Matrix4x4 value, ref Matrix4x4 result)
// // //     {
// // //         float b0 = (value[2][0] * value[3][1]) - (value[2][1] * value[3][0]);
// // //         float b1 = (value[2][0] * value[3][2]) - (value[2][2] * value[3][0]);
// // //         float b2 = (value[2][3] * value[3][0]) - (value[2][0] * value[3][3]);
// // //         float b3 = (value[2][1] * value[3][2]) - (value[2][2] * value[3][1]);
// // //         float b4 = (value[2][3] * value[3][1]) - (value[2][1] * value[3][3]);
// // //         float b5 = (value[2][2] * value[3][3]) - (value[2][3] * value[3][2]);

// // //         float d11 = value[1][1] *  b5 + value[1][2] *  b4 + value[1][3] * b3;
// // //         float d12 = value[1][0] *  b5 + value[1][2] *  b2 + value[1][3] * b1;
// // //         float d13 = value[1][0] * -b4 + value[1][1] *  b2 + value[1][3] * b0;
// // //         float d14 = value[1][0] *  b3 + value[1][1] * -b1 + value[1][2] * b0;

// // //         float det = value[0][0] * d11 - value[0][1] * d12 + value[0][2] * d13 - value[0][3] * d14;
            
// // //         // if (Math.Abs(det) == 0.0f)
// // //         // {
// // //         //   result = Matrix.Zero;
// // //         //   return;
// // //         // }

// // //         det = 1f / det;

// // //         float a0 = (value[0][0] * value[1][1]) - (value[0][1] * value[1][0]);
// // //         float a1 = (value[0][0] * value[1][2]) - (value[0][2] * value[1][0]);
// // //         float a2 = (value[0][3] * value[1][0]) - (value[0][0] * value[1][3]);
// // //         float a3 = (value[0][1] * value[1][2]) - (value[0][2] * value[1][1]);
// // //         float a4 = (value[0][3] * value[1][1]) - (value[0][1] * value[1][3]);
// // //         float a5 = (value[0][2] * value[1][3]) - (value[0][3] * value[1][2]);

// // //         float d21 = value[0][1] *  b5 + value[0][2] *  b4 + value[0][3] * b3;
// // //         float d22 = value[0][0] *  b5 + value[0][2] *  b2 + value[0][3] * b1;
// // //         float d23 = value[0][0] * -b4 + value[0][1] *  b2 + value[0][3] * b0;
// // //         float d24 = value[0][0] *  b3 + value[0][1] * -b1 + value[0][2] * b0;

// // //         float d31 = value[3][1] *  a5 + value[3][2] *  a4 + value[3][3] * a3;
// // //         float d32 = value[3][0] *  a5 + value[3][2] *  a2 + value[3][3] * a1;
// // //         float d33 = value[3][0] * -a4 + value[3][1] *  a2 + value[3][3] * a0;
// // //         float d34 = value[3][0] *  a3 + value[3][1] * -a1 + value[3][2] * a0;

// // //         float d41 = value[2][1] *  a5 + value[2][2] *  a4 + value[2][3] * a3;
// // //         float d42 = value[2][0] *  a5 + value[2][2] *  a2 + value[2][3] * a1;
// // //         float d43 = value[2][0] * -a4 + value[2][1] *  a2 + value[2][3] * a0;
// // //         float d44 = value[2][0] *  a3 + value[2][1] * -a1 + value[2][2] * a0;

// // //         result[0][0] = +d11 * det; result[0][1] = -d21 * det; result[0][2] = +d31 * det; result[0][3] = -d41 * det;
// // //         result[1][0] = -d12 * det; result[1][1] = +d22 * det; result[1][2] = -d32 * det; result[1][3] = +d42 * det;
// // //         result[2][0] = +d13 * det; result[2][1] = -d23 * det; result[2][2] = +d33 * det; result[2][3] = -d43 * det;
// // //         result[3][0] = -d14 * det; result[3][1] = +d24 * det; result[3][2] = -d34 * det; result[3][3] = +d44 * det;
// // //     }    

// // //     /// Map the specified window coordinates (win.x, win.y, win.z) into object coordinates.
// // // 	/// The near and far clip planes correspond to z normalized device coordinates of -1 and +1 respectively. (OpenGL clip volume definition)
// // // 	///
// // // 	/// @param win Specify the window coordinates to be mapped.
// // // 	/// @param model Specifies the modelview matrix
// // // 	/// @param proj Specifies the projection matrix
// // // 	/// @param viewport Specifies the viewport
// // // 	/// @return Returns the computed object coordinates.
// // // 	/// @tparam T Native type used for the computation. Currently supported: half (not recommended), float or double.
// // // 	/// @tparam U Currently supported: Floating-point types and integer types.
// // // 	///
// // // 	/// @see <a href="https://www.khronos.org/registry/OpenGL-Refpages/gl2.1/xhtml/gluUnProject.xml">gluUnProject man page</a>
// // //     internal static Vector4 UnProjectNO(in Vector3 window,in Matrix4x4 Clip,in Vector4 viewport)
// // //     {
// // //         Matrix4x4 inverse = new Matrix4x4(0.0f);
// // //         Invert( Clip, ref inverse);
        
// // //         Vector4 tmp = new Vector4(window.X,window.Y,window.Z, 1.0f);
        
// // //         tmp.X = (tmp.X - viewport[0]) / viewport[2];
// // // 		tmp.Y = (tmp.Y - viewport[1]) / viewport[3];
// // // 		tmp = (tmp*2) - 1.0f ;
// // // // https://stackoverflow.com/questions/7692988/opengl-math-projecting-screen-space-to-world-space-coords
// // // //https://stackoverflow.com/questions/29997209/opengl-c-mouse-ray-picking-glmunproject
// // //           Vector4 obj = MultiplyMat4ByVec4(  inverse , tmp);
// // //         //  var obj = MultiplyVec4ByMat4(tmp , inverse);
// // //         // obj /= obj.W;
// // //         // return obj;
// // //         return  Normalize(obj);
// // //     }
















// /// <summary>
// /// https://cloudapps.herokuapp.com/imagetoascii/
// /// EASE IN
// ///                                       #      
// ///                                     #      
// ///                                   #          
// ///                                #            
// ///                            ##                
// ///                          ##                  
// ///                       ##                     
// ///                   ##                         
// ///              ####                            
// ///    # #######                                                              
// /// </summary>
// internal static class Easing
// {
    
// }

// namespace MCJGame.Engine.Math;


// // 
// //     collide
// // Finish Easing
// // Color
// // Convert
// // Culling
// //     FustrumCulling
// // Clip Space

// // Space Subdivision
// //     Octree
// //     Quadtree
// //     BSP

// // OPtimise Object/Entity / Model
// //     LOD
// //     Optimise Mesh

// // Physics
// //     Formula
// // Random

// // Hash 

// // Transform

// // Pojection 

// // Quaternion =>> si obligatoir sinon oublier 

// // Trigonometrie
// //     
// */










        






// // namespace MCJ.Engine.Math
// // {
// //     using System;
// //     /// <summary>
// //     /// 
// //     /// </summary>
// //     internal static class Projection
// //     {
// //         /// <summary>
// //         /// RH right hand opengl, NO negative one to one  
// //         /// </summary>
// //         /// <param name="mat"></param>
// //         /// <param name="fovdegree"></param>
// //         /// <param name="width"></param>
// //         /// <param name="height"></param>
// //         /// <param name="zNear"></param>
// //         /// <param name="zFar"></param>
// //         internal static void PerspectiveFOV_RHNO(ref Matrix4 mat,in float fovdegree,in float width,in float height,in float zNear,in float zFar )
// // 		{
// // 			float rad = Maths.DegToRad(fovdegree);
// // 			float h = Maths.Cos( 0.5f * rad) / Maths.Sin( 0.5f * rad );
// // 			float w =  h * height / width;

// // 			mat[0,0] = w;
// // 			mat[1,1] = h;
// // 			mat[2,2] = - (zFar + zNear) / (zFar - zNear);
// // 			mat[2,3] = - 1.0f;
// // 			mat[3,2] = - (2.0f * zFar * zNear) / (zFar - zNear);
// // 		}

// //         /// <summary>
// //         /// 
// //         /// </summary>
// //         /// <param name="fovdegree"></param>
// //         /// <param name="aspect"></param>
// //         /// <param name="zNear"></param>
// //         /// <param name="zFar"></param>
// //         /// <returns></returns>
// //         internal static Matrix4 Perspective_RHNO(float fovdegree,float aspect,float zNear,float zFar )
// // 		{
// //             float tanHalfFovy = Maths.Tan( Maths.DegToRad(fovdegree) / 2.0f);

// //             Matrix4 Result = new(0);
// //             Result[0,0] = 1.0f / (aspect * tanHalfFovy);
// //             Result[1,1] = 1.0f / (tanHalfFovy);
// //             Result[2,2] = - (zFar + zNear) / (zFar - zNear);
// //             Result[2,3] = - 1.0f;
// //             Result[3,2] = - (2.0f * zFar * zNear) / (zFar - zNear);
// //             return Result;
// //             //  var negFarRange = float.IsPositiveInfinity(farPlaneDistance) ? -1.0f : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

// //             // result.M11 = (2.0f * nearPlaneDistance) / width;
            
// //             // result.M22 = (2.0f * nearPlaneDistance) / height;
            
// //             // result.M33 = negFarRange;
            
// //             // result.M34 = -1.0f;
            
// //             // result.M43 = nearPlaneDistance * negFarRange;
// // 		}
// //         /// <summary>
// //         /// 
// //         /// </summary>
// //         /// <param name="fieldOfView"></param>
// //         /// <param name="aspectRatio"></param>
// //         /// <param name="nearPlaneDistance"></param>
// //         /// <param name="farPlaneDistance"></param>
// //         /// <param name="result"></param>
// //         internal static void CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance, out Matrix4 result)
// //         {
// //             if ((fieldOfView <= 0f) || (fieldOfView >= 3.141593f))
// // 		    {
// // 		        throw new ArgumentException("fieldOfView <= 0 or >= PI");
// // 		    }
// // 		    if (nearPlaneDistance <= 0f)
// // 		    {
// // 		        throw new ArgumentException("nearPlaneDistance <= 0");
// // 		    }
// // 		    if (farPlaneDistance <= 0f)
// // 		    {
// // 		        throw new ArgumentException("farPlaneDistance <= 0");
// // 		    }
// // 		    if (nearPlaneDistance >= farPlaneDistance)
// // 		    {
// // 		        throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
// // 		    }

// //             var yScale = 1.0f / (float)Math.Tan((double)fieldOfView * 0.5f);
// //             var xScale = yScale / aspectRatio;
// //             var negFarRange = float.IsPositiveInfinity(farPlaneDistance) ? -1.0f : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

// //             result.M11 = xScale;
// //             result.M12 = result.M13 = result.M14 = 0.0f;
// //             result.M22 = yScale;
// //             result.M21 = result.M23 = result.M24 = 0.0f;
// //             result.M31 = result.M32 = 0.0f;            
// //             result.M33 = negFarRange;
// //             result.M34 = -1.0f;
// //             result.M41 = result.M42 = result.M44 = 0.0f;
// //             result.M43 = nearPlaneDistance * negFarRange;
// //         }
// //         /// <summary>
// //         /// 
// //         /// </summary>
// //         /// <param name="width"></param>
// //         /// <param name="height"></param>
// //         /// <param name="nearPlaneDistance"></param>
// //         /// <param name="farPlaneDistance"></param>
// //         /// <param name="result"></param>
// //         internal static void CreatePerspective(float width, float height, float nearPlaneDistance, float farPlaneDistance, out Matrix4 result)
// //         {
// //             if (nearPlaneDistance <= 0f)
// // 		    {
// // 		        throw new ArgumentException("nearPlaneDistance <= 0");
// // 		    }
// // 		    if (farPlaneDistance <= 0f)
// // 		    {
// // 		        throw new ArgumentException("farPlaneDistance <= 0");
// // 		    }
// // 		    if (nearPlaneDistance >= farPlaneDistance)
// // 		    {
// // 		        throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
// // 		    }

// //             var negFarRange = float.IsPositiveInfinity(farPlaneDistance) ? -1.0f : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

// //             result.M11 = (2.0f * nearPlaneDistance) / width;
// //             result.M12 = result.M13 = result.M14 = 0.0f;
// //             result.M22 = (2.0f * nearPlaneDistance) / height;
// //             result.M21 = result.M23 = result.M24 = 0.0f;            
// //             result.M33 = negFarRange;
// //             result.M31 = result.M32 = 0.0f;
// //             result.M34 = -1.0f;
// //             result.M41 = result.M42 = result.M44 = 0.0f;
// //             result.M43 = nearPlaneDistance * negFarRange;
// //         }

// //         /// <summary>
// //         /// 
// //         /// </summary>
// //         /// <param name="fovdegree"></param>
// //         /// <param name="aspect"></param>
// //         /// <param name="zNear"></param>
// //         /// <param name="zFar"></param>
// //         /// <returns></returns>
// //         internal static Matrix4 Perspective_RHZO(float fovdegree,float aspect,float zNear, float zFar )
// // 		{
// //             float tanHalfFovy = Maths.Tan( Maths.DegToRad(fovdegree) / 2.0f);

// //             Matrix4 Result = new(0);
// //             Result[0,0] = 1.0f / (aspect * tanHalfFovy);
// //             Result[1,1] = 1.0f / (tanHalfFovy);
// //             Result[2,2] = zFar / (zNear - zFar);
// //             Result[2,3] = - 1.0f;
// //             Result[3,2] = -(zFar * zNear) / (zFar - zNear);
// //             return Result;
// // 		}

// //         /// <summary>
// //         /// 
// //         /// </summary>
// //         /// <param name="fovdegree"></param>
// //         /// <param name="aspect"></param>
// //         /// <param name="zNear"></param>
// //         /// <param name="zFar"></param>
// //         /// <returns></returns>
// //         internal static Matrix4 Perspective_LHZO(float fovdegree,float aspect,float zNear, float zFar )
// // 		{
// //             float tanHalfFovy = Maths.Tan( Maths.DegToRad(fovdegree) / 2.0f);

// //             Matrix4 Result = new(0);
// //             Result[0,0] = 1.0f / (aspect * tanHalfFovy);
// //             Result[1,1] = 1.0f / (tanHalfFovy);
// //             Result[2,2] = zFar / (zFar - zNear);
// //             Result[2,3] = 1.0f;
// //             Result[3,2] = -(zFar * zNear) / (zFar - zNear);
// //             return Result;
// // 		}
// //         /// <summary>
// //         /// 
// //         /// </summary>
// //         /// <param name="fovdegree"></param>
// //         /// <param name="aspect"></param>
// //         /// <param name="zNear"></param>
// //         /// <param name="zFar"></param>
// //         /// <returns></returns>
// //         internal static Matrix4 Perspective_LHNO(float fovdegree,float aspect,float zNear, float zFar )
// // 		{
// //             float tanHalfFovy = Maths.Tan( Maths.DegToRad(fovdegree) / 2.0f);

// //             Matrix4 Result = new(0);
// //             Result[0,0] = 1.0f / (aspect * tanHalfFovy);
// //             Result[1,1] = 1.0f / (tanHalfFovy);
// //             Result[2,2] = (zFar + zNear) / (zFar - zNear);
// //             Result[2,3] = 1.0f;
// //             Result[3,2] = - (2.0f * zFar * zNear) / (zFar - zNear);
// //             return Result;
// // 		}
// //     }
// // }








// // // ///////////////////////////////////////////////////////////////////////////////////////
// // // //https://www.howtobuildsoftware.com/index.php/how-do/7RO/c-opengl-opengl-c-mouse-ray-picking-glmunproject
// // // // // Values you might be interested:
// // // // glm::vec3 cameraPosition; // some camera position, this is supplied by you
// // // // glm::vec3 rayDirection = CFreeCamera::CreateRay();
// // // // glm::vec3 rayStartPositon = cameraPosition;
// // // // glm::vec3 rayEndPosition = rayStartPosition + rayDirection * someDistance;

// // //     internal static Vector4 CreateRay( in Vector3 position , in Matrix4x4 clip , in Vector4 viewport )
// // //     {
// // //             Matrix4x4 inv = Compute_Inverse(clip);
// // //             Vector4 near = new Vector4( (position.X - viewport.Z)/viewport.Z ,-1 *( (position.Y - viewport.Z)/ viewport.Z) , -1.0f,1.0f);
// // //             Vector4 far =  new Vector4( (position.X - viewport.Z)/viewport.Z ,-1 *( (position.Y - viewport.Z)/ viewport.Z) , 1.0f,1.0f);

// // //             Vector4 nearResult = inv * near ;
// // //             Vector4 farResult = inv * far;
// // //             nearResult /= nearResult.W ;
// // //             farResult /= farResult.W;
// // //             // Vector4 dir = new Vector4( farResult - nearResult);
// // //             // return Normalize(dir);
// // //             return Normalize(farResult - nearResult);
// // //     }        

// // //     internal static Matrix4x4 Transpose(in Matrix4x4 m)
// // //     {
// // //         Matrix4x4 Result = new Matrix4x4(1.0f);
// // //             Result[0][0] = m[0][0];
// // // 			Result[0][1] = m[1][0];
// // // 			Result[0][2] = m[2][0];
// // // 			Result[0][3] = m[3][0];

// // // 			Result[1][0] = m[0][1];
// // // 			Result[1][1] = m[1][1];
// // // 			Result[1][2] = m[2][1];
// // // 			Result[1][3] = m[3][1];

// // // 			Result[2][0] = m[0][2];
// // // 			Result[2][1] = m[1][2];
// // // 			Result[2][2] = m[2][2];
// // // 			Result[2][3] = m[3][2];

// // // 			Result[3][0] = m[0][3];
// // // 			Result[3][1] = m[1][3];
// // // 			Result[3][2] = m[2][3];
// // // 			Result[3][3] = m[3][3];

// // //         return Result;
// // //     }

// // //     internal static float Determinant(in Matrix4x4 m)
// // //     {
// // //         Matrix4x4 Result = new Matrix4x4(1.0f);
// // //             var SubFactor00 = m[2][2] * m[3][3] - m[3][2] * m[2][3];
// // // 			var SubFactor01 = m[2][1] * m[3][3] - m[3][1] * m[2][3];
// // // 			var SubFactor02 = m[2][1] * m[3][2] - m[3][1] * m[2][2];
// // // 			var SubFactor03 = m[2][0] * m[3][3] - m[3][0] * m[2][3];
// // // 			var SubFactor04 = m[2][0] * m[3][2] - m[3][0] * m[2][2];
// // // 			var SubFactor05 = m[2][0] * m[3][1] - m[3][0] * m[2][1];

// // // 			Vector4 DetCof = new Vector4(
// // // 				+ (m[1][1] * SubFactor00 - m[1][2] * SubFactor01 + m[1][3] * SubFactor02),
// // // 				- (m[1][0] * SubFactor00 - m[1][2] * SubFactor03 + m[1][3] * SubFactor04),
// // // 				+ (m[1][0] * SubFactor01 - m[1][1] * SubFactor03 + m[1][3] * SubFactor05),
// // // 				- (m[1][0] * SubFactor02 - m[1][1] * SubFactor04 + m[1][2] * SubFactor05));

// // // 			return
// // // 				m[0][0] * DetCof[0] + m[0][1] * DetCof[1] +
// // // 				m[0][2] * DetCof[2] + m[0][3] * DetCof[3];
// // //         // return Result;
// // //     }   

// // //     internal static Matrix4x4 Compute_Inverse(in Matrix4x4 m) 
// // //     {
// // //         // Matrix4x4 Result = new Matrix4x4(1.0f);

// // //         var Coef00 = m[2][2] * m[3][3] - m[3][2] * m[2][3];
// // //         var Coef02 = m[1][2] * m[3][3] - m[3][2] * m[1][3];
// // //         var Coef03 = m[1][2] * m[2][3] - m[2][2] * m[1][3];

// // //         var Coef04 = m[2][1] * m[3][3] - m[3][1] * m[2][3];
// // //         var Coef06 = m[1][1] * m[3][3] - m[3][1] * m[1][3];
// // //         var Coef07 = m[1][1] * m[2][3] - m[2][1] * m[1][3];

// // //         var Coef08 = m[2][1] * m[3][2] - m[3][1] * m[2][2];
// // //         var Coef10 = m[1][1] * m[3][2] - m[3][1] * m[1][2];
// // //         var Coef11 = m[1][1] * m[2][2] - m[2][1] * m[1][2];

// // //         var Coef12 = m[2][0] * m[3][3] - m[3][0] * m[2][3];
// // //         var Coef14 = m[1][0] * m[3][3] - m[3][0] * m[1][3];
// // //         var Coef15 = m[1][0] * m[2][3] - m[2][0] * m[1][3];

// // //         var Coef16 = m[2][0] * m[3][2] - m[3][0] * m[2][2];
// // //         var Coef18 = m[1][0] * m[3][2] - m[3][0] * m[1][2];
// // //         var Coef19 = m[1][0] * m[2][2] - m[2][0] * m[1][2];

// // //         var Coef20 = m[2][0] * m[3][1] - m[3][0] * m[2][1];
// // //         var Coef22 = m[1][0] * m[3][1] - m[3][0] * m[1][1];
// // //         var Coef23 = m[1][0] * m[2][1] - m[2][0] * m[1][1];

// // //         Vector4 Fac0 = new Vector4(Coef00, Coef00, Coef02, Coef03);
// // //         Vector4 Fac1 = new Vector4(Coef04, Coef04, Coef06, Coef07);
// // //         Vector4 Fac2 = new Vector4(Coef08, Coef08, Coef10, Coef11);
// // //         Vector4 Fac3 = new Vector4(Coef12, Coef12, Coef14, Coef15);
// // //         Vector4 Fac4 = new Vector4(Coef16, Coef16, Coef18, Coef19);
// // //         Vector4 Fac5 = new Vector4(Coef20, Coef20, Coef22, Coef23);

// // //         Vector4 Vec0 = new Vector4(m[1][0], m[0][0], m[0][0], m[0][0]);
// // //         Vector4 Vec1 = new Vector4(m[1][1], m[0][1], m[0][1], m[0][1]);
// // //         Vector4 Vec2 = new Vector4(m[1][2], m[0][2], m[0][2], m[0][2]);
// // //         Vector4 Vec3 = new Vector4(m[1][3], m[0][3], m[0][3], m[0][3]);

// // //         Vector4 Inv0 = new Vector4(Vec1 * Fac0 - Vec2 * Fac1 + Vec3 * Fac2);
// // //         Vector4 Inv1 = new Vector4(Vec0 * Fac0 - Vec2 * Fac3 + Vec3 * Fac4);
// // //         Vector4 Inv2 = new Vector4(Vec0 * Fac1 - Vec1 * Fac3 + Vec3 * Fac5);
// // //         Vector4 Inv3 = new Vector4(Vec0 * Fac2 - Vec1 * Fac4 + Vec2 * Fac5);

// // //         Vector4  SignA = new Vector4(+1, -1, +1, -1);
// // //         Vector4  SignB = new Vector4(-1, +1, -1, +1);
// // //         Matrix4x4 Inverse = new Matrix4x4(Inv0 * SignA, Inv1 * SignB, Inv2 * SignA, Inv3 * SignB);

// // //         Vector4 Row0 = new Vector4(Inverse[0][0], Inverse[1][0], Inverse[2][0], Inverse[3][0]);

// // //         Vector4 Dot0 = new Vector4(m[0] * Row0);
// // //         var Dot1 = (Dot0.X + Dot0.Y) + (Dot0.Z + Dot0.W);

// // //         var OneOverDeterminant = 1 / Dot1;

// // //         return Inverse * OneOverDeterminant;
// // //     }

    
// // //     internal static Matrix4x4 PickMatrix(in Vector2 center ,in Vector2 delta ,in Vector4 viewport)
// // //     {
// // //         Matrix4x4 result = new Matrix4x4(1.0f);

// // //         if ( delta.X < 0 && delta.Y > 0 ) return result;

// // //         Vector3 tmp = new Vector3(   
// // //             ( viewport[2] - 2.0f * (center.X - viewport[0]) )/ delta.X ,
// // //             ( viewport[3] - 2.0f * (center.Y - viewport[1]) )/ delta.Y  ,
// // //             0.0f
// // //         );

// // //         Translate( ref result , tmp);
// // //         Scale(ref result , new Vector3 ( viewport[2]/delta.X, viewport[3]/delta.Y,1.0f    ));
// // //         return result;
// // //     }

// // // #endregion

// // // #region Camera Projection    

// // //     /// <summary>
// // //     /// Opengl RH (right hand ) and Negative one to one NO 
// // //     /// 
// // //     /// </summary>
// // //     /// <param name="pers"></param>
// // //     /// <param name="fovydegree"></param>
// // //     /// <param name="aspect"></param>
// // //     /// <param name="zNear"></param>
// // //     /// <param name="zFar"></param>
// // //     internal static void Perspective_RHNO( Matrix4x4 pers,in float fovydegree,in float aspect,in float zNear,in float zFar )
// // //     {
// // //         float tanHalfFovy =Maths.Tan( Maths.DegToRad(fovydegree)  /2 );

// // //         pers[0][0] = 1.0f / (aspect * tanHalfFovy);
// // //         pers[1][1] = 1.0f / (tanHalfFovy);
// // //         pers[2][2] =  - (zFar + zNear) / (zFar - zNear) ;
// // //         pers[2][3] = - 1.0f;
// // //         pers[3][2] =  - (2.0f * zFar * zNear) / (zFar - zNear) ;
// // //     }

// // //     internal static void PerspectiveFOV_RHNO( Matrix4x4 result,in float fovydegree,in float width,in float height,in float zNear,in float zFar )
// // //     {		
// // //         float rad = Maths.DegToRad(fovydegree);
// // //         float h = Maths.Cos( 0.5f * rad) / Maths.Sin( 0.5f * rad );
// // //         float w =  h * height / width;

// // //         result[0][0] = w;
// // //         result[1][1] = h;
// // //         result[2][2] = - (zFar + zNear) / (zFar - zNear);
// // //         result[2][3] = - 1.0f;
// // //         result[3][2] = - (2.0f * zFar * zNear) / (zFar - zNear);
// // //     }


// // //     internal static void Frustum_RHNO(ref Matrix4x4 result ,in float left ,in float right ,in float bottom ,in float top ,in float nearVal ,in float farVal )
// // //     {
// // //         result.Zero();
// // //         result[0][0] = ( 2.0f * nearVal) / (right - left);
// // //         result[1][1] = ( 2.0f * nearVal) / (top - bottom);
// // //         result[2][0] = (right + left) / (right - left);
// // //         result[2][1] = (top + bottom) / (top - bottom);
// // //         result[2][2] = - (farVal + nearVal) / (farVal - nearVal) ;
// // //         result[3][2] = - ( 2.0f * farVal * nearVal) / (farVal - nearVal)   ;
// // //         result[2][3] =-1.0f;
// // //     }

// // //     internal static void Ortho(ref Matrix4x4 result ,in float left ,in float right ,in float bottom ,in float top ,in float zNear ,in float zFar)
// // //     {
// // //         result.Identity();

// // //         result[0][0] = 2.0f / (right - left);
// // //         result[1][1] = 2.0f / (top - bottom);
// // //         result[2][2] = - 2.0f / (zFar - zNear) ;
// // //         result[3][0] = - (right + left) / (right - left);
// // //         result[3][1] = - (top + bottom) / (top - bottom);
// // //         result[3][2] = - (zFar + zNear) / (zFar - zNear) ;
// // //     }

// // //     internal static Matrix4x4  ortho2D(in float left,in float right,in float bottom,in float top)
// // //     {
// // //         Matrix4x4 Result = new Matrix4x4(1.0f);
// // //         Result[0][0] = 2.0f / (right - left);
// // //         Result[1][1] = 2.0f / (top - bottom);
// // //         Result[2][2] = - 1.0f;
// // //         Result[3][0] = - (right + left) / (right - left);
// // //         Result[3][1] = - (top + bottom) / (top - bottom);
// // //         return Result;
// // //     }

// // //     /// <summary>
// // //     /// Opengl
// // //     /// </summary>
// // //     /// <param name="result"></param>
// // //     /// <param name="position"></param>
// // //     /// <param name="target"></param>
// // //     /// <param name="up"></param>
// // //     internal static void LookAtRH(ref Matrix4x4 result, in Vector3 position,in Vector3 target,in Vector3 up)
// // //     {   
// // //         // var direction = Normalize( target - position);
// // //         var direction = Normalize( target );
// // //         var right = Normalize( Cross(direction,up ) );//camera right
// // //         var camUp = Cross(right,direction); //up
        
// // //         result.Identity();

// // //         result[0][0] = right.X;
// // //         result[1][0] = right.Y;
// // //         result[2][0] = right.Z;
// // //         result[0][1] = camUp.X;
// // //         result[1][1] = camUp.Y;
// // //         result[2][1] = camUp.Z;
// // //         result[0][2] = -direction.X;
// // //         result[1][2] = -direction.Y;
// // //         result[2][2] = -direction.Z;
// // //         result[3][0] = -Dot(right, position);
// // //         result[3][1] = -Dot(camUp, position);
// // //         result[3][2] = Dot(direction, position);
// // //     }

// // // #endregion 

// // // #region Dot

// // //     internal static float Dot(Vector2  v1, Vector2  v2) 
// // //         => v1.X * v2.X + v1.Y * v2.Y;

// // //     internal static float Dot(Vector3  v1, Vector3  v2) 
// // //         => v1.X * v2.X + v1.Y * v2.Y+ v1.Z * v2.Z;

// // //     internal static float Dot(Vector4  v1, Vector4  v2) 
// // //         => v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z + v1.W*v2.W;

// // // #endregion

// // // #region Cross
// // //     internal static Vector3 Cross(Vector3 v1, Vector3 v2) 
// // //         => new Vector3(v1.Y * v2.Z - v1.Z * v2.Y, 
// // //             v1.Z * v2.X - v1.X * v2.Z,
// // //             v1.X * v2.Y - v1.Y * v2.X);

// // //     internal static void CrossR(ref Vector3 result, Vector3 v1, Vector3 v2) 
// // //         => (result.X,result.Y,result.Z) = (v1.Y * v2.Z - v1.Z * v2.Y,v1.Z * v2.X - v1.X * v2.Z , v1.X * v2.Y - v1.Y * v2.X    );
  
// // //     internal static Vector4 Cross(Vector4 x, Vector4 y) {
// // //         return new Vector4(
// // //             x.Y * y.Z - x.Z * y.Y,
// // //             x.Z * y.X - x.X * y.Z,
// // //             x.X * y.Y - x.Y * y.X,
// // //             x.W * y.Z - x.Z);
// // //     }
// // // #endregion

// // // #region Normalize
// // //     internal static Vector3 Normalize(Vector3 x) {
// // //         float length =1/ x.Length;
// // //         return new Vector3(x.X*length,x.Y*length,x.Z*length);
// // //     }

// // //     internal static Vector4 Normalize(in Vector4 x) {
// // //         float length = 1/ x.Length;
// // //         return new Vector4( x.X*length, x.Y*length, x.Z*length , x.W*length); 
// // //     }

// // //     internal static Plane Normalize(in Plane x) {
// // //         float length = 1/ x.Length;
// // //         return new Plane( x.X*length, x.Y*length, x.Z*length , x.D*length); 
// // //     }

// // //     internal static float Distance(Vector3 v1, Vector3 v2)
// // //         => Maths.Sqrt(  ((v2.X - v1.X)*(v2.X - v1.X))  +   ((v2.Y - v1.Y)*(v2.Y - v1.Y))  + ((v2.Z - v1.Z)*(v2.Z - v1.Z))  ); 

// // // #endregion

// // // #region Frustum
// // //     internal static void ViewFustrum ( Plane[] frustum, Matrix4x4 clip )
// // //     {
// // //         //https://www.gamedevs.org/uploads/fast-extraction-viewing-frustum-planes-from-world-view-projection-matrix.pdf
// // //         //Matrix4x4 clip = projection  * modelview ;
        
// // //         // Extract the numbers for the RIGHT plane  ( col3 - col0 )
// // //         frustum[0].X = clip[0][3] - clip[0][0];// frustum[0].X = clip[3] - clip[ 0];
// // //         frustum[0].Y = clip[1][3] - clip[1][0];// frustum[0].Y = clip[7] - clip[ 4];
// // //         frustum[0].Z = clip[2][3] - clip[2][0];// frustum[0].Z = clip[11] - clip[ 8];
// // //         frustum[0].D = clip[3][3] - clip[3][0];// frustum[0].W = clip[15] - clip[12];        

// // //         // Extract the numbers for the LEFT plane (col3 + col0 )
// // //         frustum[1].X = clip[0][3] + clip[0][0];// frustum[1].X = clip[ 3] + clip[ 0];
// // //         frustum[1].Y = clip[1][3] + clip[1][0];// frustum[1].Y = clip[ 7] + clip[ 4];
// // //         frustum[1].Z = clip[2][3] + clip[2][0];// frustum[1].Z = clip[11] + clip[ 8];
// // //         frustum[1].D = clip[3][3] + clip[3][0];// frustum[1].W = clip[15] + clip[12];         

// // //         // Extract the BOTTOM plane     (  col3 + col1 )
// // //         frustum[2].X = clip[0][3] + clip[0][1];// frustum[2].X = clip[ 3] + clip[ 1];
// // //         frustum[2].Y = clip[1][3] + clip[1][1];// frustum[2].Y = clip[ 7] + clip[ 5];
// // //         frustum[2].Z = clip[2][3] + clip[2][1];// frustum[2].Z = clip[11] + clip[ 9];
// // //         frustum[2].D = clip[3][3] + clip[3][1];// frustum[2].W = clip[15] + clip[13];

// // //         // Extract the TOP plane      (  col3  - col1 )
// // //         frustum[3].X = clip[0][3] - clip[0][1];// frustum[2].X = clip[ 3] - clip[ 1];
// // //         frustum[3].Y = clip[1][3] - clip[1][1];// frustum[2].Y = clip[ 7] - clip[ 5];
// // //         frustum[3].Z = clip[2][3] - clip[2][1];// frustum[2].Z = clip[11] - clip[ 9];
// // //         frustum[3].D = clip[3][3] - clip[3][1];// frustum[2].W = clip[15] - clip[13];

// // //         // Extract the FAR plane   (   col3 -  col2  )
// // //         frustum[4].X = clip[0][3] - clip[0][2];// frustum[4].X = clip[ 3] - clip[ 2];
// // //         frustum[4].Y = clip[1][3] - clip[1][2];// frustum[4].Y = clip[ 7] - clip[ 6];
// // //         frustum[4].Z = clip[2][3] - clip[2][2];// frustum[4].Z = clip[11] - clip[10];
// // //         frustum[4].D = clip[3][3] - clip[3][2];// frustum[4].W = clip[15] - clip[14];

// // //         // Extract the NEAR plane ( col3 - col2 )
// // //         frustum[5].X = clip[0][3] + clip[0][2];// frustum[5].X = clip[ 3] + clip[ 2];
// // //         frustum[5].Y = clip[1][3] + clip[1][2];// frustum[5].Y = clip[ 7] + clip[ 6];
// // //         frustum[5].Z = clip[2][3] + clip[2][2];// frustum[5].Z = clip[11] + clip[10];
// // //         frustum[5].D = clip[3][3] + clip[3][2];// frustum[5].W = clip[15] + clip[14];

// // //         Normalize( frustum[0]);
// // //         Normalize( frustum[1]);
// // //         Normalize( frustum[2]);
// // //         Normalize( frustum[3]);
// // //         Normalize( frustum[4]);
// // //         Normalize( frustum[5]);
// // //     }

// // //     /// <summary>
// // //     /// 
// // //     /// </summary>
// // //     /// <param name="p"></param>
// // //     /// <param name="radius"></param>
// // //     /// <returns></returns>
// // //     internal static bool CullingFustrum(System.ReadOnlySpan<Plane> frustum, Vector3 p, float radius )
// // //     {
// // //         for (int i = 0; i < 6  ;++i  )
// // //         {
// // //             if( frustum[i].X * p.X + frustum[i].Y * p.Y + frustum[i].Z * p.Z + frustum[i].D - radius <= 0   )
// // //         }
// // //         return true ;
// // //     }
// // // #endregion

// // // } //END TRANSFORM
// // // } // END NAMESPACE




//     //  code take on github : https://github.com/FlaxEngine/FlaxAPI/blob/master/FlaxEngine/Math/Viewport.cs
//     // /// <summary>
//     // /// Projects a 3D vector from object space into screen space.
//     // /// </summary>
//     // /// <param name="source">The vector to project.</param>
//     // /// <param name="projection">The projection matrix.</param>
//     // /// <param name="view">The view matrix.</param>
//     // /// <param name="world">The world matrix.</param>
//     // /// <returns>The projected vector.</returns>
//     // internal Vector3 Project(Vector3 source, Matrix projection, Matrix view, Matrix world)
//     // {
//     //     // Matrix matrix;
//     //     // Matrix.Multiply(ref world, ref view, out matrix);
//     //     // Matrix.Multiply(ref matrix, ref projection, out matrix);

//     //     Vector3 vector;
//     //     Project(ref source, ref matrix, out vector);
//     //     return vector;
//     // }

//     // /// <summary>
//     // /// Projects a 3D vector from object space into screen space.
//     // /// </summary>
//     // /// <param name="source">The vector to project.</param>
//     // /// <param name="matrix">A combined WorldViewProjection matrix.</param>
//     // /// <param name="vector">The projected vector.</param>
//     // internal void Project(ref Vector3 source, ref Matrix matrix, out Vector3 vector)
//     // {
//     //     // Vector3.Transform(ref source, ref matrix, out vector);
//     //     // float w = source.X * matrix.M14 + source.Y * matrix.M24 + source.Z * matrix.M34 + matrix.M44;

//     //     // if (!Mathf.IsZero(w))
//     //     // {
//     //     //     vector /= w;
//     //     // }

//     //     // vector.X = (vector.X + 1f) * 0.5f * Width + X;
//     //     // vector.Y = (-vector.Y + 1f) * 0.5f * Height + Y;
//     //     // vector.Z = vector.Z * (MaxDepth - MinDepth) + MinDepth;
//     // }

//     // /// <summary>
//     // /// Converts a screen space point into a corresponding point in world space.
//     // /// </summary>
//     // /// <param name="source">The vector to project.</param>
//     // /// <param name="projection">The projection matrix.</param>
//     // /// <param name="view">The view matrix.</param>
//     // /// <param name="world">The world matrix.</param>
//     // /// <returns>The unprojected Vector.</returns>
//     // internal Vector3 Unproject(Vector3 source, Matrix projection, Matrix view, Matrix world)
//     // {
//     //     Matrix matrix;
//     //     // Matrix.Multiply(ref world, ref view, out matrix);
//     //     // Matrix.Multiply(ref matrix, ref projection, out matrix);
//     //     // Matrix.Invert(ref matrix, out matrix);

//     //     Vector3 vector;
//     //     Unproject(ref source, ref matrix, out vector);
//     //     return vector;
//     // }

//     // /// <summary>
//     // /// Converts a screen space point into a corresponding point in world space.
//     // /// </summary>
//     // /// <param name="source">The vector to project.</param>
//     // /// <param name="matrix">An inverted combined WorldViewProjection matrix.</param>
//     // /// <param name="vector">The unprojected vector.</param>
//     // internal void Unproject(ref Vector3 source, ref Matrix matrix, out Vector3 vector)
//     // {
//     //     // vector.X = (source.X - X) / Width * 2f - 1f;
//     //     // vector.Y = -((source.Y - Y) / Height * 2f - 1f);
//     //     // vector.Z = (source.Z - MinDepth) / (MaxDepth - MinDepth);

//     //     // float w = vector.X * matrix.M14 + vector.Y * matrix.M24 + vector.Z * matrix.M34 + matrix.M44;
//     //     // Vector3.Transform(ref vector, ref matrix, out vector);

//     //     // if (!Mathf.IsZero(w))
//     //     // {
//     //     //     vector /= w;
//     //     // }
//     // }
// }



























// // namespace MCJ.Engine.Math
// // {
// //     /// <summary>
// //     /// This is transform like GLM 
// //     /// 
// //     /// </summary>
// //     internal static class Transform
// //     {
// //         /// <summary>
// //         /// 
// //         /// </summary>
// //         /// <param name="m"></param>
// //         /// <param name="v"></param>
// //         /// <returns></returns>
// // 		internal static Matrix4 Translate(Matrix4 m, Vector3 v) 
// // 		{	
// // 			Matrix4 result = new(m);
// // 			result.Translation = m.Right * v[0] +  m.Up * v[1] + m.Forward * v[2] + m.Translation;
// // 			return result;
// // 		}

// // 		/// <summary>
// // 		/// 
// // 		/// </summary>
// // 		/// <param name="m"></param>
// // 		/// <param name="v"></param>
// //         internal static void TranslateRef(ref Matrix4 m ,in Vector3 v)
// //             =>  m.Translation = m.Right * v[0] +  m.Up * v[1] + m.Forward * v[2] + m.Translation;

// // 		/// <summary>
// // 		/// 
// // 		/// </summary>
// // 		/// <param name="m"></param>
// // 		/// <param name="v"></param>
// // 		/// <returns></returns>
// //         internal static Matrix4 Scale(Matrix4 m , Vector3 v)
// //         {
// //             Matrix4 Result = new Matrix4(m);
// //             // Result[0] = m[0] * v[0];
// // 			Result.Right = m.Right * v[0];
// //             // Result[1] = m[1] * v[1];
// // 			Result.Up = m.Up * v[1];
// //             // Result[2] = m[2] * v[2];
// // 			Result.Forward = m.Forward * v[2];
// //             // Result[3] = m[3];
// // 			Result.Translation = m.Translation;
// //             return Result;        
// //         }

// // 		/// <summary>
// // 		/// 
// // 		/// </summary>
// // 		/// <param name="mat"></param>
// // 		/// <param name="v"></param>
// // 		internal static void ScaleRef(ref Matrix4 mat,in  Vector3 v)
// // 			=>(mat.Right,mat.Up , mat.Forward )=( mat.Right * v[0], mat.Up *= v[1], mat.Forward *= v[2]);

// // 		/// <summary>
// // 		/// Rotate around arbitrary axis
// //         /// </summary>
// //         /// <param name="m"></param>
// //         /// <param name="angle">in radians 0 to 2PI </param>
// //         /// <param name="v"></param>
// //         internal static Matrix4 Rotate(Matrix4 m,in float angle,Vector3 v)
// //         {
// //             var c = Maths.Cos(  angle  );
// //             var s = Maths.Sin(  angle  );

// //             Vector3 axis = new(v);
// // 			axis.Normalize();
// //             Vector3 temp = new ((1 - c) * axis);

// //             Matrix4 Rotate = Matrix4.Identite;
// // 			Rotate[0,0] = c + temp[0] * axis[0];
// // 			Rotate[0,1] = temp[0] * axis[1] + s * axis[2];
// // 			Rotate[0,2] = temp[0] * axis[2] - s * axis[1];

// // 			Rotate[1,0] = temp[1] * axis[0] - s * axis[2];
// // 			Rotate[1,1] = c + temp[1] * axis[1];
// // 			Rotate[1,2] = temp[1] * axis[2] + s * axis[0];

// // 			Rotate[2,0] = temp[2] * axis[0] + s * axis[1];
// // 			Rotate[2,1] = temp[2] * axis[1] - s * axis[0];
// // 			Rotate[2,2] = c + temp[2] * axis[2];

// // 			Matrix4 Result = Matrix4.Identite;
// // 			// Result[0] = m[0] * Rotate[0,0] + m[1] * Rotate[0,1] + m[2] * Rotate[0,2];
// // 			Result.Right = m.Right * Rotate[0,0] + m.Up * Rotate[0,1] + m.Forward * Rotate[0,2];
// // 			// Result[1] = m[0] * Rotate[1,0] + m[1] * Rotate[1,1] + m[2] * Rotate[1,2];
// // 			Result.Up = m.Right * Rotate[1,0] + m.Up * Rotate[1,1] + m.Forward * Rotate[1,2];
// // 			// Result[2] = m[0] * Rotate[2,0] + m[1] * Rotate[2,1] + m[2] * Rotate[2,2];
// // 			Result.Forward = m.Right * Rotate[2,0] + m.Up * Rotate[2,1] + m.Forward * Rotate[2,2];
// // 			// Result[3] = m[3];
// // 			Result.Translation = m.Translation ;
// // 			return Result;
// //         }


// // //	// 		Matririx PlayerAbsoluteTransform;https://gamedev.stackexchange.com/questions/27966/rotating-a-model-and-translating-it-forward-in-xna
// // // void UpdatePlayer( Vector3 EnemyPosition, float PlayerVelocity, float Seconds)
// // // {
// // //      Vector3 PlayerPosition = PlayerAbsoluteTransform.Translation;
// // //      Vector3 Forward = EnemyPosition - PlayerPosition;
// // //      Forward.Normalize();

// // //      // This moves your player towars the enemy
// // //      PlayerPosition += Forward * PlayerVelocity * Seconds; 

// // //      // This create the transform matrix for your model, 
// // //      // note that maybe you have to rotate the model before to face right
// // //      PlayerAbsoluteTransform = Matrix.CreateWorld(PlayerPosition, Forward, PlayerUp);
// // // }
	

		

// // #region Clip_Space

// // 		// internal static Matrix4 Perspective(float fovydegree, float aspect, float zNear, float zFar , bool LH =false , bool NO = false)
// // 		// {
// // 		// 	float tanHalfFovy =Functions.Tan( Functions.DegreesToRadians(fovydegree)  /2 );

// // 		// 	Matrix4x4 Result = new Matrix4x4(0.0f);
// // 		// 	Result[0][0] = 1.0f / (aspect * tanHalfFovy);
// // 		// 	Result[1][1] = 1.0f / (tanHalfFovy);
// // 		// 	Result[2][2] =NO ?  - (zFar + zNear) / (zFar - zNear) :   zFar / (zNear - zFar);
// // 		// 	Result[2][3] =LH? 1.0f : - 1.0f;
// // 		// 	Result[3][2] =NO ?  - (2.0f * zFar * zNear) / (zFar - zNear) : -(zFar * zNear) / (zFar - zNear);
// // 		// 	return Result;
// // 		// }

// // 	// 	internal static void PerspectiveTest(ref Matrix4x4 pers,  float fovydegree, float aspect, float zNear, float zFar , bool LH =false , bool NO = false)
// // 	// 	{
// // 	// 		float tanHalfFovy =Functions.Tan( Functions.DegreesToRadians(fovydegree)  /2 );

// // 	// 		//Matrix4x4 Result = new Matrix4x4(0.0f);
// // 	// 		pers[0][0] = 1.0f / (aspect * tanHalfFovy);
// // 	// 		pers[1][1] = 1.0f / (tanHalfFovy);
// // 	// 		pers[2][2] =NO ?  - (zFar + zNear) / (zFar - zNear) :   zFar / (zNear - zFar);
// // 	// 		pers[2][3] =LH? 1.0f : - 1.0f;
// // 	// 		pers[3][2] =NO ?  - (2.0f * zFar * zNear) / (zFar - zNear) : -(zFar * zNear) / (zFar - zNear);
// // 	// 		//return Result;
// // 	// 	}

// // 	// 	internal static void PerspectiveTestR( Matrix4x4 pers,  float fovydegree, float aspect, float zNear, float zFar , bool LH =false , bool NO = false)
// // 	// 	{
// // 	// 		float tanHalfFovy =Functions.Tan( Functions.DegreesToRadians(fovydegree)  /2 );

// // 	// 		//Matrix4x4 Result = new Matrix4x4(0.0f);
// // 	// 		pers[0][0] = 1.0f / (aspect * tanHalfFovy);
// // 	// 		pers[1][1] = 1.0f / (tanHalfFovy);
// // 	// 		pers[2][2] =NO ?  - (zFar + zNear) / (zFar - zNear) :   zFar / (zNear - zFar);
// // 	// 		pers[2][3] =LH? 1.0f : - 1.0f;
// // 	// 		pers[3][2] =NO ?  - (2.0f * zFar * zNear) / (zFar - zNear) : -(zFar * zNear) / (zFar - zNear);
// // 	// 		//return Result;
// // 	// 	}


// //     //     internal static Matrix4x4 frustum( float left , float right , float bottom , float top , float nearVal , float farVal , bool NO = true , bool LH = true)
// //     //     {
// //     //             //TODO différence en tre NO et ZO;
// //     //         float lh = LH ? 1.0f : -1.0f ; 
// //     //         // FOR LH 

// //     //         Matrix4x4 Result = new Matrix4x4(1.0f);
// //     //         Result[0][0] = ( 2.0f * nearVal) / (right - left);
// //     //         Result[1][1] = ( 2.0f * nearVal) / (top - bottom);
// //     //         Result[2][0] = (right + left) / (right - left);
// //     //         Result[2][1] = (top + bottom) / (top - bottom);
// //     //         Result[2][2] =NO ? lh * (farVal + nearVal) / (farVal - nearVal)  : farVal / (farVal - nearVal)  ;
// //     //         Result[3][2] =NO ?  - ( 2.0f * farVal * nearVal) / (farVal - nearVal) : -(farVal * nearVal) / (farVal - nearVal)  ;
// //     //         Result[2][3] =lh;            
// //     //         return Result;
// //     //     }

// //     // internal static Matrix4x4 Ortho( float left , float right , float bottom , float top , float zNear , float zFar , bool NO = true , bool LH = true)
// //     // {
// //     //     Matrix4x4 Result = new Matrix4x4(1.0f);

// //     //     Result[0][0] =  2.0f / (right - left);
// //     //     Result[1][1] =  2.0f / (top - bottom);
// //     //     Result[2][2] =NO ? 2.0f / (zFar - zNear) :   1.0f / (zFar - zNear);
// //     //     Result[3][0] = - (right + left) / (right - left);
// //     //     Result[3][1] = - (top + bottom) / (top - bottom);
// //     //     Result[3][2] =NO ?  - (zFar + zNear) / (zFar - zNear) : - zNear / (zFar - zNear);
// //     //     return Result;
// //     // }

// // 	// internal static Matrix4x4  ortho2D(float left, float right, float bottom, float top)
// // 	// {
// // 	// 	Matrix4x4 Result = new Matrix4x4(1.0f);
// // 	// 	Result[0][0] = 2.0f / (right - left);
// // 	// 	Result[1][1] = 2.0f / (top - bottom);
// // 	// 	Result[2][2] = - 1.0f;
// // 	// 	Result[3][0] = - (right + left) / (right - left);
// // 	// 	Result[3][1] = - (top + bottom) / (top - bottom);
// // 	// 	return Result;
// // 	// }

// // /*




// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> perspectiveFovRH_ZO(T fov, T width, T height, T zNear, T zFar)
// // 	{
// // 		assert(width > static_cast<T>(0));
// // 		assert(height > static_cast<T>(0));
// // 		assert(fov > static_cast<T>(0));

// // 		T const rad = fov;
// // 		T const h = glm::cos(static_cast<T>(0.5) * rad) / glm::sin(static_cast<T>(0.5) * rad);
// // 		T const w = h * height / width; ///todo max(width , Height) / min(width , Height)?

// // 		mat<4, 4, T, defaultp> Result(static_cast<T>(0));
// // 		Result[0][0] = w;
// // 		Result[1][1] = h;
// // 		Result[2][2] = zFar / (zNear - zFar);
// // 		Result[2][3] = - static_cast<T>(1);
// // 		Result[3][2] = -(zFar * zNear) / (zFar - zNear);
// // 		return Result;
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> perspectiveFovRH_NO(T fov, T width, T height, T zNear, T zFar)
// // 	{
// // 		assert(width > static_cast<T>(0));
// // 		assert(height > static_cast<T>(0));
// // 		assert(fov > static_cast<T>(0));

// // 		T const rad = fov;
// // 		T const h = glm::cos(static_cast<T>(0.5) * rad) / glm::sin(static_cast<T>(0.5) * rad);
// // 		T const w = h * height / width; ///todo max(width , Height) / min(width , Height)?

// // 		mat<4, 4, T, defaultp> Result(static_cast<T>(0));
// // 		Result[0][0] = w;
// // 		Result[1][1] = h;
// // 		Result[2][2] = - (zFar + zNear) / (zFar - zNear);
// // 		Result[2][3] = - static_cast<T>(1);
// // 		Result[3][2] = - (static_cast<T>(2) * zFar * zNear) / (zFar - zNear);
// // 		return Result;
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> perspectiveFovLH_ZO(T fov, T width, T height, T zNear, T zFar)
// // 	{
// // 		assert(width > static_cast<T>(0));
// // 		assert(height > static_cast<T>(0));
// // 		assert(fov > static_cast<T>(0));

// // 		T const rad = fov;
// // 		T const h = glm::cos(static_cast<T>(0.5) * rad) / glm::sin(static_cast<T>(0.5) * rad);
// // 		T const w = h * height / width; ///todo max(width , Height) / min(width , Height)?

// // 		mat<4, 4, T, defaultp> Result(static_cast<T>(0));
// // 		Result[0][0] = w;
// // 		Result[1][1] = h;
// // 		Result[2][2] = zFar / (zFar - zNear);
// // 		Result[2][3] = static_cast<T>(1);
// // 		Result[3][2] = -(zFar * zNear) / (zFar - zNear);
// // 		return Result;
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> perspectiveFovLH_NO(T fov, T width, T height, T zNear, T zFar)
// // 	{
// // 		assert(width > static_cast<T>(0));
// // 		assert(height > static_cast<T>(0));
// // 		assert(fov > static_cast<T>(0));

// // 		T const rad = fov;
// // 		T const h = glm::cos(static_cast<T>(0.5) * rad) / glm::sin(static_cast<T>(0.5) * rad);
// // 		T const w = h * height / width; ///todo max(width , Height) / min(width , Height)?

// // 		mat<4, 4, T, defaultp> Result(static_cast<T>(0));
// // 		Result[0][0] = w;
// // 		Result[1][1] = h;
// // 		Result[2][2] = (zFar + zNear) / (zFar - zNear);
// // 		Result[2][3] = static_cast<T>(1);
// // 		Result[3][2] = - (static_cast<T>(2) * zFar * zNear) / (zFar - zNear);
// // 		return Result;
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> perspectiveFovZO(T fov, T width, T height, T zNear, T zFar)
// // 	{
// // #		if GLM_CONFIG_CLIP_CONTROL & GLM_CLIP_CONTROL_LH_BIT
// // 			return perspectiveFovLH_ZO(fov, width, height, zNear, zFar);
// // #		else
// // 			return perspectiveFovRH_ZO(fov, width, height, zNear, zFar);
// // #		endif
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> perspectiveFovNO(T fov, T width, T height, T zNear, T zFar)
// // 	{
// // #		if GLM_CONFIG_CLIP_CONTROL & GLM_CLIP_CONTROL_LH_BIT
// // 			return perspectiveFovLH_NO(fov, width, height, zNear, zFar);
// // #		else
// // 			return perspectiveFovRH_NO(fov, width, height, zNear, zFar);
// // #		endif
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> perspectiveFovLH(T fov, T width, T height, T zNear, T zFar)
// // 	{
// // #		if GLM_CONFIG_CLIP_CONTROL & GLM_CLIP_CONTROL_ZO_BIT
// // 			return perspectiveFovLH_ZO(fov, width, height, zNear, zFar);
// // #		else
// // 			return perspectiveFovLH_NO(fov, width, height, zNear, zFar);
// // #		endif
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> perspectiveFovRH(T fov, T width, T height, T zNear, T zFar)
// // 	{
// // #		if GLM_CONFIG_CLIP_CONTROL & GLM_CLIP_CONTROL_ZO_BIT
// // 			return perspectiveFovRH_ZO(fov, width, height, zNear, zFar);
// // #		else
// // 			return perspectiveFovRH_NO(fov, width, height, zNear, zFar);
// // #		endif
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> perspectiveFov(T fov, T width, T height, T zNear, T zFar)
// // 	{
// // #		if GLM_CONFIG_CLIP_CONTROL == GLM_CLIP_CONTROL_LH_ZO
// // 			return perspectiveFovLH_ZO(fov, width, height, zNear, zFar);
// // #		elif GLM_CONFIG_CLIP_CONTROL == GLM_CLIP_CONTROL_LH_NO
// // 			return perspectiveFovLH_NO(fov, width, height, zNear, zFar);
// // #		elif GLM_CONFIG_CLIP_CONTROL == GLM_CLIP_CONTROL_RH_ZO
// // 			return perspectiveFovRH_ZO(fov, width, height, zNear, zFar);
// // #		elif GLM_CONFIG_CLIP_CONTROL == GLM_CLIP_CONTROL_RH_NO
// // 			return perspectiveFovRH_NO(fov, width, height, zNear, zFar);
// // #		endif
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> infinitePerspectiveRH(T fovy, T aspect, T zNear)
// // 	{
// // 		T const range = tan(fovy / static_cast<T>(2)) * zNear;
// // 		T const left = -range * aspect;
// // 		T const right = range * aspect;
// // 		T const bottom = -range;
// // 		T const top = range;

// // 		mat<4, 4, T, defaultp> Result(static_cast<T>(0));
// // 		Result[0][0] = (static_cast<T>(2) * zNear) / (right - left);
// // 		Result[1][1] = (static_cast<T>(2) * zNear) / (top - bottom);
// // 		Result[2][2] = - static_cast<T>(1);
// // 		Result[2][3] = - static_cast<T>(1);
// // 		Result[3][2] = - static_cast<T>(2) * zNear;
// // 		return Result;
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> infinitePerspectiveLH(T fovy, T aspect, T zNear)
// // 	{
// // 		T const range = tan(fovy / static_cast<T>(2)) * zNear;
// // 		T const left = -range * aspect;
// // 		T const right = range * aspect;
// // 		T const bottom = -range;
// // 		T const top = range;

// // 		mat<4, 4, T, defaultp> Result(T(0));
// // 		Result[0][0] = (static_cast<T>(2) * zNear) / (right - left);
// // 		Result[1][1] = (static_cast<T>(2) * zNear) / (top - bottom);
// // 		Result[2][2] = static_cast<T>(1);
// // 		Result[2][3] = static_cast<T>(1);
// // 		Result[3][2] = - static_cast<T>(2) * zNear;
// // 		return Result;
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> infinitePerspective(T fovy, T aspect, T zNear)
// // 	{
// // #		if GLM_CONFIG_CLIP_CONTROL & GLM_CLIP_CONTROL_LH_BIT
// // 			return infinitePerspectiveLH(fovy, aspect, zNear);
// // #		else
// // 			return infinitePerspectiveRH(fovy, aspect, zNear);
// // #		endif
// // 	}

// // 	// Infinite projection matrix: http://www.terathon.com/gdc07_lengyel.pdf
// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> tweakedInfinitePerspective(T fovy, T aspect, T zNear, T ep)
// // 	{
// // 		T const range = tan(fovy / static_cast<T>(2)) * zNear;
// // 		T const left = -range * aspect;
// // 		T const right = range * aspect;
// // 		T const bottom = -range;
// // 		T const top = range;

// // 		mat<4, 4, T, defaultp> Result(static_cast<T>(0));
// // 		Result[0][0] = (static_cast<T>(2) * zNear) / (right - left);
// // 		Result[1][1] = (static_cast<T>(2) * zNear) / (top - bottom);
// // 		Result[2][2] = ep - static_cast<T>(1);
// // 		Result[2][3] = static_cast<T>(-1);
// // 		Result[3][2] = (ep - static_cast<T>(2)) * zNear;
// // 		return Result;
// // 	}

// // 	template<typename T>
// // 	GLM_FUNC_QUALIFIER mat<4, 4, T, defaultp> tweakedInfinitePerspective(T fovy, T aspect, T zNear)
// // 	{
// // 		return tweakedInfinitePerspective(fovy, aspect, zNear, epsilon<T>());
// // 	}

// // */

// // #endregion


// //         // internal static void LookAtLH(ref Matrix4x4 result,Vector3 position, Vector3 target, Vector3 up)
// //         // {   
// //         //     direction = Normalize(position - target);
// //         //     right = Normalize(Cross(up, direction));
// //         //     camUp = Normalize(Cross(direction, right));

            
// // 		// 	result.Identity();

// // 		// 	result[0][0] = right.X;
// // 		// 	result[1][0] = right.Y;
// // 		// 	result[2][0] = right.Z;
// // 		// 	result[0][1] = camUp.X;
// // 		// 	result[1][1] = camUp.Y;
// // 		// 	result[2][1] = camUp.Z;
// // 		// 	result[0][2] = direction.X;
// // 		// 	result[1][2] = direction.Y;
// // 		// 	result[2][2] = direction.Z;
// // 		// 	result[3][0] = -Dot(right, position);
// // 		// 	result[3][1] = -Dot(camUp, position);
// // 		// 	result[3][2] = -Dot(direction, position);
// //         // }

// //         // private static Vector3 direction = new Vector3(0.0f,0.0f,0.0f);
// //         // private static Vector3 right = new Vector3(0.0f,0.0f,0.0f);
// //         // private static Vector3 camUp = new Vector3(0.0f,0.0f,0.0f);


// //         // /// <summary>
// //         // /// Opengl
// //         // /// </summary>
// //         // /// <param name="result"></param>
// //         // /// <param name="position"></param>
// //         // /// <param name="target"></param>
// //         // /// <param name="up"></param>
// //         // internal static void LookAtRH(ref Matrix4x4 result,Vector3 position, Vector3 target, Vector3 up)
// //         // {   
// //         //     direction = Normalize( target - position);
// //         //     right = Normalize( Cross(direction,up ) );//camera right
// //         //     camUp = Cross(right,direction); //up
            
// // 		// 	result.Identity();

// // 		// 	result[0][0] = right.X;
// // 		// 	result[1][0] = right.Y;
// // 		// 	result[2][0] = right.Z;
// // 		// 	result[0][1] = camUp.X;
// // 		// 	result[1][1] = camUp.Y;
// // 		// 	result[2][1] = camUp.Z;
// // 		// 	result[0][2] = -direction.X;
// // 		// 	result[1][2] = -direction.Y;
// // 		// 	result[2][2] = -direction.Z;
// // 		// 	result[3][0] = -Dot(right, position);
// // 		// 	result[3][1] = -Dot(camUp, position);
// // 		// 	result[3][2] = Dot(direction, position);
// //         // }

// //         // internal static float Dot(Vector2  v1, Vector2  v2) {
// // 		// 	return  v1.X * v2.X + v1.Y * v2.Y;
// // 		// }

// //         // internal static float Dot(Vector3  v1, Vector3  v2) {
// // 		// 	return  v1.X * v2.X + v1.Y * v2.Y+ v1.Z * v2.Z;
// // 		// }

// //         // internal static float Dot(Vector4  v1, Vector4  v2) {
// // 		// 	return  v1.x * v2.x + v1.y * v2.y + v1.z * v2.z + v1.w*v2.w;
// // 		// }
		
		
// // 		// internal static Vector3 Cross(Vector3 v1, Vector3 v2) 
// //         // {
// //         //      return new Vector3(v1.Y * v2.Z - v1.Z * v2.Y, 
// //         //         v1.Z * v2.X - v1.X * v2.Z,
// //         //         v1.X * v2.Y - v1.Y * v2.X);
// // 		// }

// //         // internal static void CrossR(ref Vector3 result, Vector3 v1, Vector3 v2) 
// //         // {
// //         //     result.X = v1.Y * v2.Z - v1.Z * v2.Y;
// //         //     result.Y = v1.Z * v2.X - v1.X * v2.Z ;
// //         //     result.Z = v1.X * v2.Y - v1.Y * v2.X;
// // 		// }

// // 		// internal static Vector4 Cross(Vector4 x, Vector4 y) {
// // 		// 	return new Vector4(
// // 		// 		x.y * y.z - x.z * y.y,
// // 		// 		x.z * y.x - x.x * y.z,
// // 		// 		x.x * y.y - x.y * y.x,
// // 		// 		x.w * y.z - x.z);
// // 		// }

// // 		// internal static Vector3 Normalize(Vector3 x) {
// //         //     float length =1/ x.Length();
// //         //     return new Vector3(x.X*length,x.Y*length,x.Z*length);
// // 		// }



// //         // internal static Vector4 Normalize(Vector4 x) {
// // 		// 	float length = 1/ x.Length;
// //         //     return new Vector4( x.x*length, x.y*length, x.z*length , x.w*length); 
// // 		// }
// //     }
// // }


// // // #region Transform Object in space   
// // //     internal static void Translate(ref Matrix4x4 m ,in Vector3 v)
// // //         => m[3] += (m[0] * v[0] + m[1] * v[1] + m[2] * v[2]  );

// // //     internal static void Scale(ref Matrix4x4 mat, Vector3 v)
// // //     {
// // //         mat[0] *= v[0];
// // //         mat[1] *= v[1];
// // //         mat[2] *= v[2];
// // //     }

// // //     /// <summary>
// // //     /// 
// // //     /// </summary>
// // //     /// <param name="m"></param>
// // //     /// <param name="angle">in degree 0 to 360° </param>
// // //     /// <param name="axis"></param>
// // //     internal static void Rotate(ref  Matrix4x4 m,in float angle,in Vector3 axis) 
// // //     {
// // //         var c = Maths.Cos( Maths.DegToRad( angle ) );
// // //         var s = Maths.Sin( Maths.DegToRad( angle ) );

// // //         axis.Normalize();
// // //         Vector3 temp = new Vector3((1 - c) * axis);

// // //         Matrix4x4 Rotate = new Matrix4x4(1.0f);
// // //         Rotate[0,0] = c + temp[0] * axis[0];
// // //         Rotate[0,1] = temp[0] * axis[1] + s * axis[2];
// // //         Rotate[0,2] = temp[0] * axis[2] - s * axis[1];

// // //         Rotate[1,0] = temp[1] * axis[0] - s * axis[2];
// // //         Rotate[1,1] = c + temp[1] * axis[1];
// // //         Rotate[1,2] = temp[1] * axis[2] + s * axis[0];

// // //         Rotate[2,0] = temp[2] * axis[0] + s * axis[1];
// // //         Rotate[2,1] = temp[2] * axis[1] - s * axis[0];
// // //         Rotate[2,2] = c + temp[2] * axis[2];

// // //         m[0] = new Vector4(m[0] * Rotate[0][0] + m[1] * Rotate[0][1] + m[2] * Rotate[0][2]); // row 1
// // //         m[1] = new Vector4(m[0] * Rotate[1][0] + m[1] * Rotate[1][1] + m[2] * Rotate[1][2]); // row 2
// // //         m[2] = new Vector4(m[0] * Rotate[2][0] + m[1] * Rotate[2][1] + m[2] * Rotate[2][2]); // row 3
// // //     }

    
// // //     /// <summary>
// // //     /// 
// // //     /// </summary>
// // //     /// <param name="result"></param>
// // //     /// <param name="angle">in degree 0 to 360 </param>
// // //     internal static void RotationX(ref Matrix4x4 result , in float angle)
// // //     {
// // //         float cosx = Maths.Cos( Maths.DegToRad( angle ) );
// // //         float sinx = Maths.Sin(Maths.DegToRad( angle ));

// // //         result.Identity();
// // //         result[1][1] = cosx;
// // //         result[1][2] = sinx;
// // //         result[2][1] = -sinx;
// // //         result[2][2] = cosx;
// // //     }
// // //     internal static void RotationY(ref Matrix4x4 result,in float angle )
// // //     {
// // //         float cosy = Maths.Cos(Maths.DegToRad( angle ));
// // //         float siny = Maths.Sin(Maths.DegToRad( angle ));

// // //         result.Identity();
// // //         result[0][0] = cosy;
// // //         result[0][2] = -siny;
// // //         result[2][0] = siny;
// // //         result[2][2] = cosy;
// // //     }
    
// // //     internal static void RotationZ(ref Matrix4x4 result ,in float angle )
// // //     {
// // //         float cosz = Maths.Cos(Maths.DegToRad( angle ));
// // //         float sinz = Maths.Sin(Maths.DegToRad( angle ));

// // //         result.Identity();
// // //         result[0][0] = cosz;
// // //         result[0][1] = sinz;
// // //         result[1][0] = -sinz;
// // //         result[1][1] = cosz;
// // //     }

// // //     internal static void RotationXYZ(ref Matrix4x4 result , in float angleX, in float angleY, in float angleZ)
// // //     {
// // //         //result.Identity();
// // //         float cosx = Maths.Cos( Maths.DegToRad( angleX ) );
// // //         float sinx = Maths.Sin(Maths.DegToRad( angleX ));

// // //         result[1][1] = cosx;
// // //         result[1][2] = sinx;
// // //         result[2][1] = -sinx;
// // //         result[2][2] = cosx;

// // //         float cosy = Maths.Cos(Maths.DegToRad( angleY ));
// // //         float siny = Maths.Sin(Maths.DegToRad( angleY ));

// // //         // result.Identity();
// // //         result[0][0] = cosy;
// // //         result[0][2] = -siny;
// // //         result[2][0] = siny;
// // //         result[2][2] *= cosy;

// // //         float cosz = Maths.Cos(Maths.DegToRad( angleZ ));
// // //         float sinz = Maths.Sin(Maths.DegToRad( angleZ ));

// // //         //result.Identity();
// // //         result[0][0] *= cosz;
// // //         result[0][1] = sinz;
// // //         result[1][0] = -sinz;
// // //         result[1][1] *= cosz;
// // //     }

// // //     /// <summary>
// // //     /// Realise Translate , scale and rotate of an object in world
// // //     /// result is in model
// // //     /// </summary>
// // //     /// <param name="model"></param>
// // //     /// <param name="translate"></param>
// // //     /// <param name="rotate"></param>
// // //     /// <param name="angle"></param>
// // //     /// <param name="scale"></param>
// // //     internal static void MoveObject(ref Matrix4x4 model, Vector3 translate, Vector3 axisRotate, float angle, Vector3 scale )
// // //     {
// // //         model.Identity();
// // //         Translate(ref model,translate);
// // //         Rotate(ref model,angle, axisRotate);
// // //         Scale(ref model, scale);
// // //     }

// // // #endregion

// // // #region Project unproject pickMatrix
// // //     internal static Vector4 MultiplyMat4ByVec4(in Matrix4x4 matrix ,in  Vector4 value )
// // //     {
// // //         Vector4 result = new Vector4();
// // //         result.X = value.X * matrix[0][0] + value.Y* matrix[0][1] + value.Z*matrix[0][2] + value.W * matrix[0][3] ;
// // //         result.Y = value.X * matrix[1][0] + value.Y* matrix[1][1] + value.Z*matrix[1][2] + value.W * matrix[1][3] ;
// // //         result.Z = value.X * matrix[2][0] + value.Y* matrix[2][1] + value.Z*matrix[2][2] + value.W * matrix[2][3] ;
// // //         result.W = value.X * matrix[3][0] + value.Y* matrix[3][1] + value.Z*matrix[3][2] + value.W * matrix[3][3] ;
// // //         return result ;
// // //     }

// // //     internal static void MultiplyMat4ByVec4(ref Vector4 result,in Matrix4x4 matrix ,in Vector4 value )
// // //     {
// // //         result.X = value.X * matrix[0][0] + value.Y* matrix[0][1] + value.Z*matrix[0][2] + value.W * matrix[0][3] ;
// // //         result.Y = (value.X * matrix[1][0]) + value.Y* matrix[1][1] + value.Z*matrix[1][2] + value.W * matrix[1][3] ;
// // //         result.Z = value.X * matrix[2][0] + value.Y* matrix[2][1] + value.Z*matrix[2][2] + value.W * matrix[2][3] ;
// // //         result.W = value.X * matrix[3][0] + value.Y* matrix[3][1] + value.Z*matrix[3][2] + value.W * matrix[3][3] ;
// // //     }

// // //     internal static Vector4 MultiplyVec4ByMat4(in Vector4 value,in  Matrix4x4 matrix)
// // //         => new Vector4
// // //         {
// // //             X = value.X * matrix[0][0] + value.Y * matrix[1][0] + value.Z * matrix[2][0] + value.W * matrix[3][0],
// // //             Y = value.X * matrix[0][1] + value.Y * matrix[1][1] + value.Z * matrix[2][1] + value.W * matrix[3][1],
// // //             Z = value.X * matrix[0][2] + value.Y * matrix[1][2] + value.Z * matrix[2][2] + value.W * matrix[3][2],
// // //             W = value.X * matrix[0][3] + value.Y * matrix[1][3] + value.Z * matrix[2][3] + value.W * matrix[3][3]
// // //         };
    
// // //     internal static Matrix4x4 MultiplyMat4byMat4(in Matrix4x4 m1 ,in Matrix4x4 m2)
// // //     {
// // //         Matrix4x4 result = new Matrix4x4(1.0f);
// // //         result[0] = m1[0] * m2[0][0] + m1[1] * m2[0][1] + m1[2] * m2[0][2] + m1[3] * m2[0][3];
// // //         result[1] = m1[0] * m2[1][0] + m1[1] * m2[1][1] + m1[2] * m2[1][2] + m1[3] * m2[1][3];
// // //         result[2] = m1[0] * m2[2][0] + m1[1] * m2[2][1] + m1[2] * m2[2][2] + m1[3] * m2[2][3];
// // //         result[3] = m1[0] * m2[3][0] + m1[1] * m2[3][1] + m1[2] * m2[3][2] + m1[3] * m2[3][3];
// // //         return result ;
// // //     }

// // //     internal static void MultiplyMat4byMat4(ref Matrix4x4 result ,in  Matrix4x4 m1 ,in Matrix4x4 m2)
// // //     {
// // //         result[0] = m1[0] * m2[0][0] + m1[1] * m2[0][1] + m1[2] * m2[0][2] + m1[3] * m2[0][3];
// // //         result[1] = m1[0] * m2[1][0] + m1[1] * m2[1][1] + m1[2] * m2[1][2] + m1[3] * m2[1][3];
// // //         result[2] = m1[0] * m2[2][0] + m1[1] * m2[2][1] + m1[2] * m2[2][2] + m1[3] * m2[2][3];
// // //         result[3] = m1[0] * m2[3][0] + m1[1] * m2[3][1] + m1[2] * m2[3][2] + m1[3] * m2[3][3];
// // //     }

// // //     /// Map the specified object coordinates (obj.x, obj.y, obj.z) into window coordinates.
// // // 	/// The near and far clip planes correspond to z normalized device coordinates of -1 and +1 respectively. (OpenGL clip volume definition)
// // // 	///
// // // 	/// @param obj Specify the object coordinates.
// // // 	/// @param model Specifies the current modelview matrix
// // // 	/// @param proj Specifies the current projection matrix
// // // 	/// @param viewport Specifies the current viewport
// // // 	/// @return Return the computed window coordinates.
// // // 	/// @tparam T Native type used for the computation. Currently supported: half (not recommended), float or double.
// // // 	/// @tparam U Currently supported: Floating-point types and integer types.
// // // 	///
// // // 	/// @see <a href="https://www.khronos.org/registry/OpenGL-Refpages/gl2.1/xhtml/gluProject.xml">gluProject man page</a>
// // //     internal static Vector4 ProjectNO(in Vector3 objPosition,in Matrix4x4 modelview,in Matrix4x4 projection ,in Vector4 viewport)
// // //     {
// // //         Vector4 tmp = new Vector4(objPosition.X, objPosition.Y, objPosition.Z,1.0f);
// // //         tmp = MultiplyMat4ByVec4( modelview ,tmp); //tmp = model * tmp;
// // //         tmp = MultiplyMat4ByVec4( projection, tmp);//tmp = projection * tmp; 

// // //         tmp /= tmp.W;
// // //         tmp = tmp * 0.5f  + 0.5f;
// // //         // tmp.Y = tmp.Y * 0.5f +0.5f;
// // //         // tmp.X = tmp.X * 0.5f +0.5f;
// // //         tmp[0] = tmp[0] *  viewport[2] + viewport[0];
// // //         tmp[1] = tmp[1] *  viewport[3] + viewport[1];
// // //         return tmp;
// // //     }

// // //     internal static void Invert(in Matrix4x4 value, ref Matrix4x4 result)
// // //     {
// // //         float b0 = (value[2][0] * value[3][1]) - (value[2][1] * value[3][0]);
// // //         float b1 = (value[2][0] * value[3][2]) - (value[2][2] * value[3][0]);
// // //         float b2 = (value[2][3] * value[3][0]) - (value[2][0] * value[3][3]);
// // //         float b3 = (value[2][1] * value[3][2]) - (value[2][2] * value[3][1]);
// // //         float b4 = (value[2][3] * value[3][1]) - (value[2][1] * value[3][3]);
// // //         float b5 = (value[2][2] * value[3][3]) - (value[2][3] * value[3][2]);

// // //         float d11 = value[1][1] *  b5 + value[1][2] *  b4 + value[1][3] * b3;
// // //         float d12 = value[1][0] *  b5 + value[1][2] *  b2 + value[1][3] * b1;
// // //         float d13 = value[1][0] * -b4 + value[1][1] *  b2 + value[1][3] * b0;
// // //         float d14 = value[1][0] *  b3 + value[1][1] * -b1 + value[1][2] * b0;

// // //         float det = value[0][0] * d11 - value[0][1] * d12 + value[0][2] * d13 - value[0][3] * d14;
            
// // //         // if (Math.Abs(det) == 0.0f)
// // //         // {
// // //         //   result = Matrix.Zero;
// // //         //   return;
// // //         // }

// // //         det = 1f / det;

// // //         float a0 = (value[0][0] * value[1][1]) - (value[0][1] * value[1][0]);
// // //         float a1 = (value[0][0] * value[1][2]) - (value[0][2] * value[1][0]);
// // //         float a2 = (value[0][3] * value[1][0]) - (value[0][0] * value[1][3]);
// // //         float a3 = (value[0][1] * value[1][2]) - (value[0][2] * value[1][1]);
// // //         float a4 = (value[0][3] * value[1][1]) - (value[0][1] * value[1][3]);
// // //         float a5 = (value[0][2] * value[1][3]) - (value[0][3] * value[1][2]);

// // //         float d21 = value[0][1] *  b5 + value[0][2] *  b4 + value[0][3] * b3;
// // //         float d22 = value[0][0] *  b5 + value[0][2] *  b2 + value[0][3] * b1;
// // //         float d23 = value[0][0] * -b4 + value[0][1] *  b2 + value[0][3] * b0;
// // //         float d24 = value[0][0] *  b3 + value[0][1] * -b1 + value[0][2] * b0;

// // //         float d31 = value[3][1] *  a5 + value[3][2] *  a4 + value[3][3] * a3;
// // //         float d32 = value[3][0] *  a5 + value[3][2] *  a2 + value[3][3] * a1;
// // //         float d33 = value[3][0] * -a4 + value[3][1] *  a2 + value[3][3] * a0;
// // //         float d34 = value[3][0] *  a3 + value[3][1] * -a1 + value[3][2] * a0;

// // //         float d41 = value[2][1] *  a5 + value[2][2] *  a4 + value[2][3] * a3;
// // //         float d42 = value[2][0] *  a5 + value[2][2] *  a2 + value[2][3] * a1;
// // //         float d43 = value[2][0] * -a4 + value[2][1] *  a2 + value[2][3] * a0;
// // //         float d44 = value[2][0] *  a3 + value[2][1] * -a1 + value[2][2] * a0;

// // //         result[0][0] = +d11 * det; result[0][1] = -d21 * det; result[0][2] = +d31 * det; result[0][3] = -d41 * det;
// // //         result[1][0] = -d12 * det; result[1][1] = +d22 * det; result[1][2] = -d32 * det; result[1][3] = +d42 * det;
// // //         result[2][0] = +d13 * det; result[2][1] = -d23 * det; result[2][2] = +d33 * det; result[2][3] = -d43 * det;
// // //         result[3][0] = -d14 * det; result[3][1] = +d24 * det; result[3][2] = -d34 * det; result[3][3] = +d44 * det;
// // //     }    

// // //     /// Map the specified window coordinates (win.x, win.y, win.z) into object coordinates.
// // // 	/// The near and far clip planes correspond to z normalized device coordinates of -1 and +1 respectively. (OpenGL clip volume definition)
// // // 	///
// // // 	/// @param win Specify the window coordinates to be mapped.
// // // 	/// @param model Specifies the modelview matrix
// // // 	/// @param proj Specifies the projection matrix
// // // 	/// @param viewport Specifies the viewport
// // // 	/// @return Returns the computed object coordinates.
// // // 	/// @tparam T Native type used for the computation. Currently supported: half (not recommended), float or double.
// // // 	/// @tparam U Currently supported: Floating-point types and integer types.
// // // 	///
// // // 	/// @see <a href="https://www.khronos.org/registry/OpenGL-Refpages/gl2.1/xhtml/gluUnProject.xml">gluUnProject man page</a>
// // //     internal static Vector4 UnProjectNO(in Vector3 window,in Matrix4x4 Clip,in Vector4 viewport)
// // //     {
// // //         Matrix4x4 inverse = new Matrix4x4(0.0f);
// // //         Invert( Clip, ref inverse);
        
// // //         Vector4 tmp = new Vector4(window.X,window.Y,window.Z, 1.0f);
        
// // //         tmp.X = (tmp.X - viewport[0]) / viewport[2];
// // // 		tmp.Y = (tmp.Y - viewport[1]) / viewport[3];
// // // 		tmp = (tmp*2) - 1.0f ;
// // // // https://stackoverflow.com/questions/7692988/opengl-math-projecting-screen-space-to-world-space-coords
// // // //https://stackoverflow.com/questions/29997209/opengl-c-mouse-ray-picking-glmunproject
// // //           Vector4 obj = MultiplyMat4ByVec4(  inverse , tmp);
// // //         //  var obj = MultiplyVec4ByMat4(tmp , inverse);
// // //         // obj /= obj.W;
// // //         // return obj;
// // //         return  Normalize(obj);
// // //     }
















// /// <summary>
// /// https://cloudapps.herokuapp.com/imagetoascii/
// /// EASE IN
// ///                                       #      
// ///                                     #      
// ///                                   #          
// ///                                #            
// ///                            ##                
// ///                          ##                  
// ///                       ##                     
// ///                   ##                         
// ///              ####                            
// ///    # #######                                                              
// /// </summary>
// internal static class Easing
// {
    
// }

// namespace MCJGame.Engine.Math;


// // 
// //     collide
// // Finish Easing
// // Color
// // Convert
// // Culling
// //     FustrumCulling
// // Clip Space

// // Space Subdivision
// //     Octree
// //     Quadtree
// //     BSP

// // OPtimise Object/Entity / Model
// //     LOD
// //     Optimise Mesh

// // Physics
// //     Formula
// // Random

// // Hash 

// // Transform

// // Pojection 

// // Quaternion =>> si obligatoir sinon oublier 

// // Trigonometrie
// //     
// */










        






// // namespace MCJ.Engine.Math
// // {
// //     using System;
// //     /// <summary>
// //     /// 
// //     /// </summary>
// //     internal static class Projection
// //     {
// //         /// <summary>
// //         /// RH right hand opengl, NO negative one to one  
// //         /// </summary>
// //         /// <param name="mat"></param>
// //         /// <param name="fovdegree"></param>
// //         /// <param name="width"></param>
// //         /// <param name="height"></param>
// //         /// <param name="zNear"></param>
// //         /// <param name="zFar"></param>
// //         internal static void PerspectiveFOV_RHNO(ref Matrix4 mat,in float fovdegree,in float width,in float height,in float zNear,in float zFar )
// // 		{
// // 			float rad = Maths.DegToRad(fovdegree);
// // 			float h = Maths.Cos( 0.5f * rad) / Maths.Sin( 0.5f * rad );
// // 			float w =  h * height / width;

// // 			mat[0,0] = w;
// // 			mat[1,1] = h;
// // 			mat[2,2] = - (zFar + zNear) / (zFar - zNear);
// // 			mat[2,3] = - 1.0f;
// // 			mat[3,2] = - (2.0f * zFar * zNear) / (zFar - zNear);
// // 		}

// //         /// <summary>
// //         /// 
// //         /// </summary>
// //         /// <param name="fovdegree"></param>
// //         /// <param name="aspect"></param>
// //         /// <param name="zNear"></param>
// //         /// <param name="zFar"></param>
// //         /// <returns></returns>
// //         internal static Matrix4 Perspective_RHNO(float fovdegree,float aspect,float zNear,float zFar )
// // 		{
// //             float tanHalfFovy = Maths.Tan( Maths.DegToRad(fovdegree) / 2.0f);

// //             Matrix4 Result = new(0);
// //             Result[0,0] = 1.0f / (aspect * tanHalfFovy);
// //             Result[1,1] = 1.0f / (tanHalfFovy);
// //             Result[2,2] = - (zFar + zNear) / (zFar - zNear);
// //             Result[2,3] = - 1.0f;
// //             Result[3,2] = - (2.0f * zFar * zNear) / (zFar - zNear);
// //             return Result;
// //             //  var negFarRange = float.IsPositiveInfinity(farPlaneDistance) ? -1.0f : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

// //             // result.M11 = (2.0f * nearPlaneDistance) / width;
            
// //             // result.M22 = (2.0f * nearPlaneDistance) / height;
            
// //             // result.M33 = negFarRange;
            
// //             // result.M34 = -1.0f;
            
// //             // result.M43 = nearPlaneDistance * negFarRange;
// // 		}
// //         /// <summary>
// //         /// 
// //         /// </summary>
// //         /// <param name="fieldOfView"></param>
// //         /// <param name="aspectRatio"></param>
// //         /// <param name="nearPlaneDistance"></param>
// //         /// <param name="farPlaneDistance"></param>
// //         /// <param name="result"></param>
// //         internal static void CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance, out Matrix4 result)
// //         {
// //             if ((fieldOfView <= 0f) || (fieldOfView >= 3.141593f))
// // 		    {
// // 		        throw new ArgumentException("fieldOfView <= 0 or >= PI");
// // 		    }
// // 		    if (nearPlaneDistance <= 0f)
// // 		    {
// // 		        throw new ArgumentException("nearPlaneDistance <= 0");
// // 		    }
// // 		    if (farPlaneDistance <= 0f)
// // 		    {
// // 		        throw new ArgumentException("farPlaneDistance <= 0");
// // 		    }
// // 		    if (nearPlaneDistance >= farPlaneDistance)
// // 		    {
// // 		        throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
// // 		    }

// //             var yScale = 1.0f / (float)Math.Tan((double)fieldOfView * 0.5f);
// //             var xScale = yScale / aspectRatio;
// //             var negFarRange = float.IsPositiveInfinity(farPlaneDistance) ? -1.0f : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

// //             result.M11 = xScale;
// //             result.M12 = result.M13 = result.M14 = 0.0f;
// //             result.M22 = yScale;
// //             result.M21 = result.M23 = result.M24 = 0.0f;
// //             result.M31 = result.M32 = 0.0f;            
// //             result.M33 = negFarRange;
// //             result.M34 = -1.0f;
// //             result.M41 = result.M42 = result.M44 = 0.0f;
// //             result.M43 = nearPlaneDistance * negFarRange;
// //         }
// //         /// <summary>
// //         /// 
// //         /// </summary>
// //         /// <param name="width"></param>
// //         /// <param name="height"></param>
// //         /// <param name="nearPlaneDistance"></param>
// //         /// <param name="farPlaneDistance"></param>
// //         /// <param name="result"></param>
// //         internal static void CreatePerspective(float width, float height, float nearPlaneDistance, float farPlaneDistance, out Matrix4 result)
// //         {
// //             if (nearPlaneDistance <= 0f)
// // 		    {
// // 		        throw new ArgumentException("nearPlaneDistance <= 0");
// // 		    }
// // 		    if (farPlaneDistance <= 0f)
// // 		    {
// // 		        throw new ArgumentException("farPlaneDistance <= 0");
// // 		    }
// // 		    if (nearPlaneDistance >= farPlaneDistance)
// // 		    {
// // 		        throw new ArgumentException("nearPlaneDistance >= farPlaneDistance");
// // 		    }

// //             var negFarRange = float.IsPositiveInfinity(farPlaneDistance) ? -1.0f : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);

// //             result.M11 = (2.0f * nearPlaneDistance) / width;
// //             result.M12 = result.M13 = result.M14 = 0.0f;
// //             result.M22 = (2.0f * nearPlaneDistance) / height;
// //             result.M21 = result.M23 = result.M24 = 0.0f;            
// //             result.M33 = negFarRange;
// //             result.M31 = result.M32 = 0.0f;
// //             result.M34 = -1.0f;
// //             result.M41 = result.M42 = result.M44 = 0.0f;
// //             result.M43 = nearPlaneDistance * negFarRange;
// //         }

// //         /// <summary>
// //         /// 
// //         /// </summary>
// //         /// <param name="fovdegree"></param>
// //         /// <param name="aspect"></param>
// //         /// <param name="zNear"></param>
// //         /// <param name="zFar"></param>
// //         /// <returns></returns>
// //         internal static Matrix4 Perspective_RHZO(float fovdegree,float aspect,float zNear, float zFar )
// // 		{
// //             float tanHalfFovy = Maths.Tan( Maths.DegToRad(fovdegree) / 2.0f);

// //             Matrix4 Result = new(0);
// //             Result[0,0] = 1.0f / (aspect * tanHalfFovy);
// //             Result[1,1] = 1.0f / (tanHalfFovy);
// //             Result[2,2] = zFar / (zNear - zFar);
// //             Result[2,3] = - 1.0f;
// //             Result[3,2] = -(zFar * zNear) / (zFar - zNear);
// //             return Result;
// // 		}

// //         /// <summary>
// //         /// 
// //         /// </summary>
// //         /// <param name="fovdegree"></param>
// //         /// <param name="aspect"></param>
// //         /// <param name="zNear"></param>
// //         /// <param name="zFar"></param>
// //         /// <returns></returns>
// //         internal static Matrix4 Perspective_LHZO(float fovdegree,float aspect,float zNear, float zFar )
// // 		{
// //             float tanHalfFovy = Maths.Tan( Maths.DegToRad(fovdegree) / 2.0f);

// //             Matrix4 Result = new(0);
// //             Result[0,0] = 1.0f / (aspect * tanHalfFovy);
// //             Result[1,1] = 1.0f / (tanHalfFovy);
// //             Result[2,2] = zFar / (zFar - zNear);
// //             Result[2,3] = 1.0f;
// //             Result[3,2] = -(zFar * zNear) / (zFar - zNear);
// //             return Result;
// // 		}
// //         /// <summary>
// //         /// 
// //         /// </summary>
// //         /// <param name="fovdegree"></param>
// //         /// <param name="aspect"></param>
// //         /// <param name="zNear"></param>
// //         /// <param name="zFar"></param>
// //         /// <returns></returns>
// //         internal static Matrix4 Perspective_LHNO(float fovdegree,float aspect,float zNear, float zFar )
// // 		{
// //             float tanHalfFovy = Maths.Tan( Maths.DegToRad(fovdegree) / 2.0f);

// //             Matrix4 Result = new(0);
// //             Result[0,0] = 1.0f / (aspect * tanHalfFovy);
// //             Result[1,1] = 1.0f / (tanHalfFovy);
// //             Result[2,2] = (zFar + zNear) / (zFar - zNear);
// //             Result[2,3] = 1.0f;
// //             Result[3,2] = - (2.0f * zFar * zNear) / (zFar - zNear);
// //             return Result;
// // 		}
// //     }
// // }








// // // ///////////////////////////////////////////////////////////////////////////////////////
// // // //https://www.howtobuildsoftware.com/index.php/how-do/7RO/c-opengl-opengl-c-mouse-ray-picking-glmunproject
// // // // // Values you might be interested:
// // // // glm::vec3 cameraPosition; // some camera position, this is supplied by you
// // // // glm::vec3 rayDirection = CFreeCamera::CreateRay();
// // // // glm::vec3 rayStartPositon = cameraPosition;
// // // // glm::vec3 rayEndPosition = rayStartPosition + rayDirection * someDistance;

// // //     internal static Vector4 CreateRay( in Vector3 position , in Matrix4x4 clip , in Vector4 viewport )
// // //     {
// // //             Matrix4x4 inv = Compute_Inverse(clip);
// // //             Vector4 near = new Vector4( (position.X - viewport.Z)/viewport.Z ,-1 *( (position.Y - viewport.Z)/ viewport.Z) , -1.0f,1.0f);
// // //             Vector4 far =  new Vector4( (position.X - viewport.Z)/viewport.Z ,-1 *( (position.Y - viewport.Z)/ viewport.Z) , 1.0f,1.0f);

// // //             Vector4 nearResult = inv * near ;
// // //             Vector4 farResult = inv * far;
// // //             nearResult /= nearResult.W ;
// // //             farResult /= farResult.W;
// // //             // Vector4 dir = new Vector4( farResult - nearResult);
// // //             // return Normalize(dir);
// // //             return Normalize(farResult - nearResult);
// // //     }        

// // //     internal static Matrix4x4 Transpose(in Matrix4x4 m)
// // //     {
// // //         Matrix4x4 Result = new Matrix4x4(1.0f);
// // //             Result[0][0] = m[0][0];
// // // 			Result[0][1] = m[1][0];
// // // 			Result[0][2] = m[2][0];
// // // 			Result[0][3] = m[3][0];

// // // 			Result[1][0] = m[0][1];
// // // 			Result[1][1] = m[1][1];
// // // 			Result[1][2] = m[2][1];
// // // 			Result[1][3] = m[3][1];

// // // 			Result[2][0] = m[0][2];
// // // 			Result[2][1] = m[1][2];
// // // 			Result[2][2] = m[2][2];
// // // 			Result[2][3] = m[3][2];

// // // 			Result[3][0] = m[0][3];
// // // 			Result[3][1] = m[1][3];
// // // 			Result[3][2] = m[2][3];
// // // 			Result[3][3] = m[3][3];

// // //         return Result;
// // //     }

// // //     internal static float Determinant(in Matrix4x4 m)
// // //     {
// // //         Matrix4x4 Result = new Matrix4x4(1.0f);
// // //             var SubFactor00 = m[2][2] * m[3][3] - m[3][2] * m[2][3];
// // // 			var SubFactor01 = m[2][1] * m[3][3] - m[3][1] * m[2][3];
// // // 			var SubFactor02 = m[2][1] * m[3][2] - m[3][1] * m[2][2];
// // // 			var SubFactor03 = m[2][0] * m[3][3] - m[3][0] * m[2][3];
// // // 			var SubFactor04 = m[2][0] * m[3][2] - m[3][0] * m[2][2];
// // // 			var SubFactor05 = m[2][0] * m[3][1] - m[3][0] * m[2][1];

// // // 			Vector4 DetCof = new Vector4(
// // // 				+ (m[1][1] * SubFactor00 - m[1][2] * SubFactor01 + m[1][3] * SubFactor02),
// // // 				- (m[1][0] * SubFactor00 - m[1][2] * SubFactor03 + m[1][3] * SubFactor04),
// // // 				+ (m[1][0] * SubFactor01 - m[1][1] * SubFactor03 + m[1][3] * SubFactor05),
// // // 				- (m[1][0] * SubFactor02 - m[1][1] * SubFactor04 + m[1][2] * SubFactor05));

// // // 			return
// // // 				m[0][0] * DetCof[0] + m[0][1] * DetCof[1] +
// // // 				m[0][2] * DetCof[2] + m[0][3] * DetCof[3];
// // //         // return Result;
// // //     }   


    
// // //     internal static Matrix4x4 PickMatrix(in Vector2 center ,in Vector2 delta ,in Vector4 viewport)
// // //     {
// // //         Matrix4x4 result = new Matrix4x4(1.0f);

// // //         if ( delta.X < 0 && delta.Y > 0 ) return result;

// // //         Vector3 tmp = new Vector3(   
// // //             ( viewport[2] - 2.0f * (center.X - viewport[0]) )/ delta.X ,
// // //             ( viewport[3] - 2.0f * (center.Y - viewport[1]) )/ delta.Y  ,
// // //             0.0f
// // //         );

// // //         Translate( ref result , tmp);
// // //         Scale(ref result , new Vector3 ( viewport[2]/delta.X, viewport[3]/delta.Y,1.0f    ));
// // //         return result;
// // //     }

// // // #endregion

// // // #region Camera Projection    

// // //     /// <summary>
// // //     /// Opengl RH (right hand ) and Negative one to one NO 
// // //     /// 
// // //     /// </summary>
// // //     /// <param name="pers"></param>
// // //     /// <param name="fovydegree"></param>
// // //     /// <param name="aspect"></param>
// // //     /// <param name="zNear"></param>
// // //     /// <param name="zFar"></param>
// // //     internal static void Perspective_RHNO( Matrix4x4 pers,in float fovydegree,in float aspect,in float zNear,in float zFar )
// // //     {
// // //         float tanHalfFovy =Maths.Tan( Maths.DegToRad(fovydegree)  /2 );

// // //         pers[0][0] = 1.0f / (aspect * tanHalfFovy);
// // //         pers[1][1] = 1.0f / (tanHalfFovy);
// // //         pers[2][2] =  - (zFar + zNear) / (zFar - zNear) ;
// // //         pers[2][3] = - 1.0f;
// // //         pers[3][2] =  - (2.0f * zFar * zNear) / (zFar - zNear) ;
// // //     }

// // //     internal static void PerspectiveFOV_RHNO( Matrix4x4 result,in float fovydegree,in float width,in float height,in float zNear,in float zFar )
// // //     {		
// // //         float rad = Maths.DegToRad(fovydegree);
// // //         float h = Maths.Cos( 0.5f * rad) / Maths.Sin( 0.5f * rad );
// // //         float w =  h * height / width;

// // //         result[0][0] = w;
// // //         result[1][1] = h;
// // //         result[2][2] = - (zFar + zNear) / (zFar - zNear);
// // //         result[2][3] = - 1.0f;
// // //         result[3][2] = - (2.0f * zFar * zNear) / (zFar - zNear);
// // //     }


// // //     internal static void Frustum_RHNO(ref Matrix4x4 result ,in float left ,in float right ,in float bottom ,in float top ,in float nearVal ,in float farVal )
// // //     {
// // //         result.Zero();
// // //         result[0][0] = ( 2.0f * nearVal) / (right - left);
// // //         result[1][1] = ( 2.0f * nearVal) / (top - bottom);
// // //         result[2][0] = (right + left) / (right - left);
// // //         result[2][1] = (top + bottom) / (top - bottom);
// // //         result[2][2] = - (farVal + nearVal) / (farVal - nearVal) ;
// // //         result[3][2] = - ( 2.0f * farVal * nearVal) / (farVal - nearVal)   ;
// // //         result[2][3] =-1.0f;
// // //     }

// // //     internal static void Ortho(ref Matrix4x4 result ,in float left ,in float right ,in float bottom ,in float top ,in float zNear ,in float zFar)
// // //     {
// // //         result.Identity();

// // //         result[0][0] = 2.0f / (right - left);
// // //         result[1][1] = 2.0f / (top - bottom);
// // //         result[2][2] = - 2.0f / (zFar - zNear) ;
// // //         result[3][0] = - (right + left) / (right - left);
// // //         result[3][1] = - (top + bottom) / (top - bottom);
// // //         result[3][2] = - (zFar + zNear) / (zFar - zNear) ;
// // //     }

// // //     internal static Matrix4x4  ortho2D(in float left,in float right,in float bottom,in float top)
// // //     {
// // //         Matrix4x4 Result = new Matrix4x4(1.0f);
// // //         Result[0][0] = 2.0f / (right - left);
// // //         Result[1][1] = 2.0f / (top - bottom);
// // //         Result[2][2] = - 1.0f;
// // //         Result[3][0] = - (right + left) / (right - left);
// // //         Result[3][1] = - (top + bottom) / (top - bottom);
// // //         return Result;
// // //     }

// // //     /// <summary>
// // //     /// Opengl
// // //     /// </summary>
// // //     /// <param name="result"></param>
// // //     /// <param name="position"></param>
// // //     /// <param name="target"></param>
// // //     /// <param name="up"></param>
// // //     internal static void LookAtRH(ref Matrix4x4 result, in Vector3 position,in Vector3 target,in Vector3 up)
// // //     {   
// // //         // var direction = Normalize( target - position);
// // //         var direction = Normalize( target );
// // //         var right = Normalize( Cross(direction,up ) );//camera right
// // //         var camUp = Cross(right,direction); //up
        
// // //         result.Identity();

// // //         result[0][0] = right.X;
// // //         result[1][0] = right.Y;
// // //         result[2][0] = right.Z;
// // //         result[0][1] = camUp.X;
// // //         result[1][1] = camUp.Y;
// // //         result[2][1] = camUp.Z;
// // //         result[0][2] = -direction.X;
// // //         result[1][2] = -direction.Y;
// // //         result[2][2] = -direction.Z;
// // //         result[3][0] = -Dot(right, position);
// // //         result[3][1] = -Dot(camUp, position);
// // //         result[3][2] = Dot(direction, position);
// // //     }

// 




































