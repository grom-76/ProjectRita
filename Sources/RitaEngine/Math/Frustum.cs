namespace RitaEngine.Math;

using static RitaEngine.Math.Helper;
/// <summary>
/// code pris le 17/05/2022 à : https://github.com/SaschaWillems/Vulkan/blob/master/base/frustum.hpp
/// 
/// </summary>
public class Frustum
{
    private Vector4[] planes;

    /// <summary> Side of plane</summary>
    public static class FrustumSide  { 
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
    public Frustum()  =>(planes) = ( new Vector4[6]);     

    /// <summary>
    /// Update fustrum culling with matrix view ( place function inside game loop update)
    /// </summary>
    /// <param name="clip">projection * view</param>
    public void Update(Matrix clip)
    {
            planes[FrustumSide.LEFT].X = clip.M14 + clip.M11;
			planes[FrustumSide.LEFT].Y = clip.M24 + clip.M21;
			planes[FrustumSide.LEFT].Z = clip.M34 + clip.M31;
			planes[FrustumSide.LEFT].W = clip.M44 + clip.M41;

			planes[FrustumSide.LEFT].X = clip.M14 - clip.M11;
			planes[FrustumSide.LEFT].Y = clip.M24 - clip.M21;
			planes[FrustumSide.LEFT].Z = clip.M34 - clip.M31;
			planes[FrustumSide.LEFT].W = clip.M44 - clip.M41;

			planes[FrustumSide.TOP].X = clip.M14 - clip.M12;
			planes[FrustumSide.TOP].Y = clip.M24 - clip.M22;
			planes[FrustumSide.TOP].Z = clip.M34 - clip.M32;
			planes[FrustumSide.TOP].W = clip.M44 - clip.M42;

			planes[FrustumSide.BOTTOM].X = clip.M14 + clip.M12;
			planes[FrustumSide.BOTTOM].Y = clip.M24 + clip.M22;
			planes[FrustumSide.BOTTOM].Z = clip.M34 + clip.M32;
			planes[FrustumSide.BOTTOM].W = clip.M44 + clip.M42;

			planes[FrustumSide.BACK].X = clip.M14 + clip.M13;
			planes[FrustumSide.BACK].Y = clip.M24 + clip.M23;
			planes[FrustumSide.BACK].Z = clip.M34 + clip.M33;
			planes[FrustumSide.BACK].W = clip.M44 + clip.M43;

			planes[FrustumSide.FRONT].X = clip.M14 - clip.M13;
			planes[FrustumSide.FRONT].Y = clip.M24 - clip.M23;
			planes[FrustumSide.FRONT].Z = clip.M34 - clip.M33;
			planes[FrustumSide.FRONT].W = clip.M44 - clip.M43;

            //normalize ? (il manque W )
            var left = Sqrt(planes[FrustumSide.LEFT].X * planes[FrustumSide.LEFT].X + planes[FrustumSide.LEFT].Y * planes[FrustumSide.LEFT].Y + planes[FrustumSide.LEFT].Z * planes[FrustumSide.LEFT].Z);
            planes[FrustumSide.LEFT] /= left;

            var right = Sqrt(planes[FrustumSide.RIGHT].X * planes[FrustumSide.RIGHT].X + planes[FrustumSide.RIGHT].Y * planes[FrustumSide.RIGHT].Y + planes[FrustumSide.RIGHT].Z * planes[FrustumSide.RIGHT].Z);
            planes[FrustumSide.RIGHT] /= right;

            var TOP = Sqrt(planes[FrustumSide.TOP].X * planes[FrustumSide.TOP].X + planes[FrustumSide.TOP].Y * planes[FrustumSide.TOP].Y + planes[FrustumSide.TOP].Z * planes[FrustumSide.TOP].Z);
            planes[FrustumSide.TOP] /= TOP;

            var BOTTOM = Sqrt(planes[FrustumSide.BOTTOM].X * planes[FrustumSide.BOTTOM].X + planes[FrustumSide.BOTTOM].Y * planes[FrustumSide.BOTTOM].Y + planes[FrustumSide.BOTTOM].Z * planes[FrustumSide.BOTTOM].Z);
            planes[FrustumSide.BOTTOM] /= BOTTOM;

            var BACK = Sqrt(planes[FrustumSide.BACK].X * planes[FrustumSide.BACK].X + planes[FrustumSide.BACK].Y * planes[FrustumSide.BACK].Y + planes[FrustumSide.BACK].Z * planes[FrustumSide.BACK].Z);
            planes[FrustumSide.BACK] /= BACK;

            var FRONT = Sqrt(planes[FrustumSide.FRONT].X * planes[FrustumSide.FRONT].X + planes[FrustumSide.FRONT].Y * planes[FrustumSide.FRONT].Y + planes[FrustumSide.FRONT].Z * planes[FrustumSide.FRONT].Z);
            planes[FrustumSide.FRONT] /= FRONT;
    }

    /// <summary>
    /// If point inside sphere
    /// </summary>
    /// <param name="pos">center of sphere </param>
    /// <param name="radius"> rayon ode la sphere</param>
    /// <returns></returns>
    
    public bool CheckSphere(Vector3 pos, float radius)
    {
        if ((planes[FrustumSide.LEFT].X * pos.X) + (planes[FrustumSide.LEFT].Y * pos.Y) + (planes[FrustumSide.LEFT].Z * pos.Z) + planes[FrustumSide.LEFT].W <= -radius)return false;
        if ((planes[FrustumSide.RIGHT].X * pos.X) + (planes[FrustumSide.RIGHT].Y * pos.Y) + (planes[FrustumSide.RIGHT].Z * pos.Z) + planes[FrustumSide.RIGHT].W <= -radius)return false;
        if ((planes[FrustumSide.TOP].X * pos.X) + (planes[FrustumSide.TOP].Y * pos.Y) + (planes[FrustumSide.TOP].Z * pos.Z) + planes[FrustumSide.TOP].W <= -radius)return false;
        if ((planes[FrustumSide.BOTTOM].X * pos.X) + (planes[FrustumSide.BOTTOM].Y * pos.Y) + (planes[FrustumSide.BOTTOM].Z * pos.Z) + planes[FrustumSide.BOTTOM].W <= -radius)return false;
        if ((planes[FrustumSide.BACK].X * pos.X) + (planes[FrustumSide.BACK].Y * pos.Y) + (planes[FrustumSide.BACK].Z * pos.Z) + planes[FrustumSide.BACK].W <= -radius)return false;
        if ((planes[FrustumSide.FRONT].X * pos.X) + (planes[FrustumSide.FRONT].Y * pos.Y) + (planes[FrustumSide.FRONT].Z * pos.Z) + planes[FrustumSide.FRONT].W <= -radius)return false;
        return true;
    }

    // /// <summary>
    // /// If point inside sphere
    // /// </summary>
    // /// <param name="pos">center of sphere </param>
    // /// <param name="radius"> rayon ode la sphere</param>
    // /// <returns></returns>
    // /// [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    // public bool CheckSphereNoIf(Vector3 pos, float radius)
    //     =>  ((planes[Side.LEFT].X * pos.X) + (planes[Side.LEFT].Y * pos.Y) + (planes[Side.LEFT].Z * pos.Z) + planes[Side.LEFT].W <= -radius)
    //     && ((planes[Side.RIGHT].X * pos.X) + (planes[Side.RIGHT].Y * pos.Y) + (planes[Side.RIGHT].Z * pos.Z) + planes[Side.RIGHT].W <= -radius)
    //     && ((planes[Side.TOP].X * pos.X) + (planes[Side.TOP].Y * pos.Y) + (planes[Side.TOP].Z * pos.Z) + planes[Side.TOP].W <= -radius)
    //     && ((planes[Side.BOTTOM].X * pos.X) + (planes[Side.BOTTOM].Y * pos.Y) + (planes[Side.BOTTOM].Z * pos.Z) + planes[Side.BOTTOM].W <= -radius)
    //     && ((planes[Side.BACK].X * pos.X) + (planes[Side.BACK].Y * pos.Y) + (planes[Side.BACK].Z * pos.Z) + planes[Side.BACK].W <= -radius)
    //     && ((planes[Side.FRONT].X * pos.X) + (planes[Side.FRONT].Y * pos.Y) + (planes[Side.FRONT].Z * pos.Z) + planes[Side.FRONT].W <= -radius);

}

