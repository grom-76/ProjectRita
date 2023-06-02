namespace RitaEngine.Base.Math;

using static RitaEngine.Base.Math.Helper;
/// <summary>
/// code pris le 17/05/2022 à : https://github.com/SaschaWillems/Vulkan/blob/master/base/frustum.hpp
/// 
/// </summary>
public class Fustrum
{
    private Vector4[] planes;

    /// <summary> Side of plane</summary>
    public static class Side  { 
        /// <summary>LEFT ( gauche ) Side of plane</summary>
        public const int LEFT = 0; 
        /// <summary> RIGHT ( droit ) Side of plane</summary>
        public const int RIGHT = 1;
        /// <summary>TOP ( haut ) Side of plane</summary>
        public const int TOP = 2;
        /// <summary> BOTTOm ( bas) Side of plane</summary>
        public const int BOTTOM = 3; 
        /// <summary> BACK ( arriere, loin , far)  Side of plane</summary>
        public const int BACK = 4;
        /// <summary>FRONT ( devant, near ) Side of plane</summary>
        public const int FRONT = 5;  
    };

    /// <summary> Init planes </summary>
    public Fustrum()  =>(planes) = ( new Vector4[6]);     

    /// <summary>
    /// Update fustrum culling with matrix view ( place function inside game loop update)
    /// </summary>
    /// <param name="matrix"></param>
    public void Update(Matrix matrix)
    {
            planes[Side.LEFT].X = matrix[0].W + matrix[0].X;
			planes[Side.LEFT].Y = matrix[1].W + matrix[1].X;
			planes[Side.LEFT].Z = matrix[2].W + matrix[2].X;
			planes[Side.LEFT].W = matrix[3].W + matrix[3].X;

			planes[Side.RIGHT].X = matrix[0].W - matrix[0].X;
			planes[Side.RIGHT].Y = matrix[1].W - matrix[1].X;
			planes[Side.RIGHT].Z = matrix[2].W - matrix[2].X;
			planes[Side.RIGHT].W = matrix[3].W - matrix[3].X;

			planes[Side.TOP].X = matrix[0].W - matrix[0].Y;
			planes[Side.TOP].Y = matrix[1].W - matrix[1].Y;
			planes[Side.TOP].Z = matrix[2].W - matrix[2].Y;
			planes[Side.TOP].W = matrix[3].W - matrix[3].Y;

			planes[Side.BOTTOM].X = matrix[0].W + matrix[0].Y;
			planes[Side.BOTTOM].Y = matrix[1].W + matrix[1].Y;
			planes[Side.BOTTOM].Z = matrix[2].W + matrix[2].Y;
			planes[Side.BOTTOM].W = matrix[3].W + matrix[3].Y;

			planes[Side.BACK].X = matrix[0].W + matrix[0].Z;
			planes[Side.BACK].Y = matrix[1].W + matrix[1].Z;
			planes[Side.BACK].Z = matrix[2].W + matrix[2].Z;
			planes[Side.BACK].W = matrix[3].W + matrix[3].Z;

			planes[Side.FRONT].X = matrix[0].W - matrix[0].Z;
			planes[Side.FRONT].Y = matrix[1].W - matrix[1].Z;
			planes[Side.FRONT].Z = matrix[2].W - matrix[2].Z;
			planes[Side.FRONT].W = matrix[3].W - matrix[3].Z;

            //normalize ? (il manque W )
            var left = Sqrt(planes[Side.LEFT].X * planes[Side.LEFT].X + planes[Side.LEFT].Y * planes[Side.LEFT].Y + planes[Side.LEFT].Z * planes[Side.LEFT].Z);
            planes[Side.LEFT] /= left;

            var right = Sqrt(planes[Side.RIGHT].X * planes[Side.RIGHT].X + planes[Side.RIGHT].Y * planes[Side.RIGHT].Y + planes[Side.RIGHT].Z * planes[Side.RIGHT].Z);
            planes[Side.RIGHT] /= right;

            var TOP = Sqrt(planes[Side.TOP].X * planes[Side.TOP].X + planes[Side.TOP].Y * planes[Side.TOP].Y + planes[Side.TOP].Z * planes[Side.TOP].Z);
            planes[Side.TOP] /= TOP;

            var BOTTOM = Sqrt(planes[Side.BOTTOM].X * planes[Side.BOTTOM].X + planes[Side.BOTTOM].Y * planes[Side.BOTTOM].Y + planes[Side.BOTTOM].Z * planes[Side.BOTTOM].Z);
            planes[Side.BOTTOM] /= BOTTOM;

            var BACK = Sqrt(planes[Side.BACK].X * planes[Side.BACK].X + planes[Side.BACK].Y * planes[Side.BACK].Y + planes[Side.BACK].Z * planes[Side.BACK].Z);
            planes[Side.BACK] /= BACK;

            var FRONT = Sqrt(planes[Side.FRONT].X * planes[Side.FRONT].X + planes[Side.FRONT].Y * planes[Side.FRONT].Y + planes[Side.FRONT].Z * planes[Side.FRONT].Z);
            planes[Side.FRONT] /= FRONT;
    }

    /// <summary>
    /// If point inside sphere
    /// </summary>
    /// <param name="pos">center of sphere </param>
    /// <param name="radius"> rayon ode la sphere</param>
    /// <returns></returns>
    
    public bool CheckSphere(Vector3 pos, float radius)
    {
        if ((planes[Side.LEFT].X * pos.X) + (planes[Side.LEFT].Y * pos.Y) + (planes[Side.LEFT].Z * pos.Z) + planes[Side.LEFT].W <= -radius)return false;
        if ((planes[Side.RIGHT].X * pos.X) + (planes[Side.RIGHT].Y * pos.Y) + (planes[Side.RIGHT].Z * pos.Z) + planes[Side.RIGHT].W <= -radius)return false;
        if ((planes[Side.TOP].X * pos.X) + (planes[Side.TOP].Y * pos.Y) + (planes[Side.TOP].Z * pos.Z) + planes[Side.TOP].W <= -radius)return false;
        if ((planes[Side.BOTTOM].X * pos.X) + (planes[Side.BOTTOM].Y * pos.Y) + (planes[Side.BOTTOM].Z * pos.Z) + planes[Side.BOTTOM].W <= -radius)return false;
        if ((planes[Side.BACK].X * pos.X) + (planes[Side.BACK].Y * pos.Y) + (planes[Side.BACK].Z * pos.Z) + planes[Side.BACK].W <= -radius)return false;
        if ((planes[Side.FRONT].X * pos.X) + (planes[Side.FRONT].Y * pos.Y) + (planes[Side.FRONT].Z * pos.Z) + planes[Side.FRONT].W <= -radius)return false;
        return true;
    }

    /// <summary>
    /// If point inside sphere
    /// </summary>
    /// <param name="pos">center of sphere </param>
    /// <param name="radius"> rayon ode la sphere</param>
    /// <returns></returns>
    /// [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public bool CheckSphereNoIf(Vector3 pos, float radius)
        =>  ((planes[Side.LEFT].X * pos.X) + (planes[Side.LEFT].Y * pos.Y) + (planes[Side.LEFT].Z * pos.Z) + planes[Side.LEFT].W <= -radius)
        && ((planes[Side.RIGHT].X * pos.X) + (planes[Side.RIGHT].Y * pos.Y) + (planes[Side.RIGHT].Z * pos.Z) + planes[Side.RIGHT].W <= -radius)
        && ((planes[Side.TOP].X * pos.X) + (planes[Side.TOP].Y * pos.Y) + (planes[Side.TOP].Z * pos.Z) + planes[Side.TOP].W <= -radius)
        && ((planes[Side.BOTTOM].X * pos.X) + (planes[Side.BOTTOM].Y * pos.Y) + (planes[Side.BOTTOM].Z * pos.Z) + planes[Side.BOTTOM].W <= -radius)
        && ((planes[Side.BACK].X * pos.X) + (planes[Side.BACK].Y * pos.Y) + (planes[Side.BACK].Z * pos.Z) + planes[Side.BACK].W <= -radius)
        && ((planes[Side.FRONT].X * pos.X) + (planes[Side.FRONT].Y * pos.Y) + (planes[Side.FRONT].Z * pos.Z) + planes[Side.FRONT].W <= -radius);

}

