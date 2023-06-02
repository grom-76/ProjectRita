

namespace RitaEngine.Base.Math;
    
[Flags]public enum EasingCurvetype : int
{
    Linear = 0,
    Quadratic = 10,
    Cubic =20 ,
    Quartic=30,
    Quintic=40,
    Sine =50,
    Circular =60,
    Expo=70,
    Elastic=90,
    Back=100,
    Bounce=110,
}
[Flags] public enum EasingMode : int
{ 
    In = 0,
    Out = 1,
    InOut =2
}
    

/* source : https://github.com/warrenm/AHEasing
    The following types of easing functions are supported: see enum EasingCureveType
    https://github.com/acron0/Easings
*/
/// <summary>
/// Les courbes d'accélérations (easing functions) décrivent la vitesse à laquelle un paramètre change en fonction du temps.
/// Dans la vie, les objets ne s'arrêtent pas instantanément, et ne bougent presque jamais à vitesse constante. Lorsque nous ouvrons un tiroir,
/// nous le déplaçons d'abord rapidement puis de plus en plus lentement jusqu'à ce qu'il sorte.
///  Faites tomber quelque chose au sol, il va premièrement accélérer vers le sol puis rebondir.
/// Cette page vous aide à choisir les bonnes courbes d'accélérations.
/// https://easings.net/fr
///  inspired by raylib
/// <example>
/// *   int currentTime = 0;
/// *   int duration = 100;
/// *   float startPositionX = 0.0f;
/// *   float finalPositionX = 30.0f;
/// *   float currentPositionX = startPositionX;
/// *
/// *   while (currentPositionX inferieur finalPositionX)
/// *   {
/// *       currentPositionX = EaseLinearNone(currentTime, startPositionX, finalPositionX - startPositionX, duration);
/// *       currentTime++;
/// *   }   
/// A port of Robert Penner's easing equations to C (http://robertpenner.com/easing/)
/// </example>
/// </summary>
[SkipLocalsInit] 
public static class Easing
{
    /// <summary>
    /// Use Easing function 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="mode"></param>
    /// <param name="scalar_start"></param>
    /// <param name="scalar_end"></param>
    /// <param name="duration">in milisecond</param>
    /// <param name="elapsed"></param>
    /// <returns></returns>
    public static float Tween(EasingCurvetype type, EasingMode mode , float scalar_start , float scalar_end , float duration, float elapsed  )
    {
        start_time =  elapsed / duration;
        int easing =  (int)type + (int)mode ;
        return easing switch 
        {
            (int)EasingCurvetype.Linear +  (int)EasingMode.In => Linear.In(start_time,scalar_start,scalar_end,duration),
            _ => 0.0f  
        };
    }
    private static float start_time = 0.0f;

    private const MethodImplOptions options = MethodImplOptions.AggressiveOptimization;
    private const float PI = 3.14159265358979323846264338327950288f;
    private static float Cos(float x) => System.MathF.Cos(x);
    private static float Sin(float x) => System.MathF.Sin(x);
    private static float Sqrt(float x) => (float)System.Math.Sqrt(x);
    private static float Pow(float x , float y ) => (float)System.Math.Pow(x,y);
    /// <summary>
    /// Linear
    /// </summary>
    [SkipLocalsInit] public static class Linear
    {
        /// <summary>
        /// Linear Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float None(float t, float b, float c, float d)
            => (c * t / d) + b;

        /// <summary>
        /// Linear Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float In(float t, float b, float c, float d)
            =>(c * t / d) + b;

        /// <summary>
        /// Linear Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float Out(float t, float b, float c, float d)
            =>(c * t / d )+ b;

        /// <summary>
        /// Linear Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float InOut(float t, float b, float c, float d)
            => (c * t / d) + b;
    }

    /// <summary>
    /// Sinus
    /// </summary>
    [SkipLocalsInit] public static class Sine
    {

        /// <summary>
        /// Sine Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float In(float t, float b, float c, float d)
            => (-c * Cos(t / d * (PI / 2))) + c + b;

        /// <summary>
        /// Sine Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float Out(float t, float b, float c, float d)
            => (c * Sin(t / d * (PI / 2))) + b;

        /// <summary>
        /// Sine Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float InOut(float t, float b, float c, float d)
            => (-c / 2 * ((float)Cos(PI * t / d) - 1)) + b;
    }

    /// <summary>
    /// Circular
    /// </summary>
    [SkipLocalsInit] public static class Circular
    {
        /// <summary>
        /// Circular Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float In(float t, float b, float c, float d)
            => (-c * ((float)Sqrt(1 - ((t /= d) * t)) - 1)) + b;

        /// <summary>
        /// Circular Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float Out(float t, float b, float c, float d)
            => (c * (float)Sqrt(1 - ((t = (t / d) - 1) * t))) + b;

        /// <summary>
        /// Circular Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float InOut(float t, float b, float c, float d)
            =>  ((t /= d / 2) < 1) ?
                ((-c / 2 * ((float)Sqrt(1 - (t * t)) - 1)) + b):
                ((c / 2 * ((float)Sqrt(1 - (t * (t -= 2))) + 1)) + b);
    }

    /// <summary>
    /// Cubic
    /// </summary>
    [SkipLocalsInit] public static class Cubic
    {
        /// <summary>
        /// Cubic Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float In(float t, float b, float c, float d)
            => (c * (t /= d) * t * t) + b;

        /// <summary>
        /// Cubic Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float Out(float t, float b, float c, float d)
            => (c * (((t = (t / d) - 1) * t * t) + 1)) + b;

        /// <summary>
        /// Cubic Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float InOut(float t, float b, float c, float d)
            => ((t /= d / 2) < 1) ?
                ((c / 2 * t * t * t) + b)
                :((c / 2 * (((t -= 2) * t * t) + 2)) + b);
    }

    /// <summary>
    /// Quadratic
    /// </summary>
    [SkipLocalsInit] public static class Quadratic
    {
        /// <summary>
        /// Quadratic Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float In(float t, float b, float c, float d)
            => (c * (t /= d) * t) + b;

        /// <summary>
        /// Quadratic Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float Out(float t, float b, float c, float d)
            => (-c * (t /= d) * (t - 2)) + b;

        /// <summary>
        /// Quadratic Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float InOut(float t, float b, float c, float d)
            => ((t /= d / 2) < 1) ?
                (((c / 2) * (t * t)) + b)
                : ((-c / 2 * (((t - 2) * (--t)) - 1)) + b);
    }

    /// <summary>
    /// Exponentielle
    /// </summary>
    [SkipLocalsInit] public static class Exponential
    {
        /// <summary>
        /// Exponential Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float In(float t, float b, float c, float d)
            => (t == 0) ? b : ((c * Pow(2, 10 * ((t / d) - 1))) + b);

        /// <summary>
        /// Exponential Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float Out(float t, float b, float c, float d)
            => (t == d) ? (b + c) : ((c * (- Pow(2, -10 * t / d) + 1)) + b);

        /// <summary>
        /// Exponential Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float InOut(float t, float b, float c, float d)
        {
            if (t == 0) return b;
            if (t == d) return b + c;

            if ((t /= d / 2) < 1)
                return (c / 2 * Pow(2, 10 * (t - 1))) + b;

            return (c / 2 * (-Pow(2, -10 * --t) + 2)) + b;
        }
    }

    /// <summary>
    /// BAck
    /// </summary>
    [SkipLocalsInit] public static class Back
    {
        /// <summary>
        /// Back Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float In(float t, float b, float c, float d)
        {
            const float s = 1.70158f;
            float postFix = t /= d;
            return (c * postFix * t * (((s + 1) * t) - s)) + b;
        }

        /// <summary>
        /// Back Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float Out(float t, float b, float c, float d)
        {
            const float s = 1.70158f;
            return (c * (((t = (t / d) - 1) * t * (((s + 1) * t) + s)) + 1)) + b;
        }

        /// <summary>
        /// Back Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float InOut(float t, float b, float c, float d)
        {
            float s = 1.70158f;
            float postFix = t -= 2;

            return ((t /= d / 2) < 1) ?
                ((c / 2 * (t * t * ((((s *= 1.525f) + 1) * t) - s))) + b) :
                ((c / 2 * ((postFix * t * ((((s *= 1.525f) + 1) * t) + s)) + 2)) + b);
        }
    }

    /// <summary>
    /// Bounce ( pour faire simili rebond)
    /// </summary>
    [SkipLocalsInit] public static class Bounce
    {
        /// <summary>
        /// Bounce Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        public static float Out(float t, float b=0.0f, float c=1.0f, float d =2.75f)
        {
            if ((t /= d) < (1 / 2.75f))
            {
                return (c * (7.5625f * t * t)) + b;
            }
            else if (t < (2 / 2.75f))
            {
                float postFix = t -= (1.5f / 2.75f);
                return (c * ((7.5625f * postFix * t) + 0.75f)) + b;
            }
            else if (t < (2.5 / 2.75))
            {
                float postFix = t -= (2.25f / 2.75f);
                return (c * ((7.5625f * postFix * t) + 0.9375f)) + b;
            }
            else
            {
                float postFix = t -= (2.625f / 2.75f);
                return (c * ((7.5625f * postFix * t) + 0.984375f)) + b;
            }
        }

        /// <summary>
        /// Bounce Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float In(float t, float b, float c, float d)
            => c - Out(d - t, 0, c, d) + b;

        /// <summary>
        /// Bounce Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        [MethodImpl(options)]
        public static float InOut(float t, float b, float c, float d)
            => (t < d / 2) ?
                ((In(t * 2, 0, c, d) * 0.5f) + b)
                :((Out((t * 2) - d, 0, c, d) * 0.5f) + (c * 0.5f) + b);
    }

    /// <summary>
    /// Elestic
    /// </summary>
    [SkipLocalsInit] public static class Elastic
    {
        /// <summary>
        /// Elastic Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        public static float In(float t, float b, float c, float d)
        {
            if (t == 0) return b;

            if ((t /= d) == 1) return b + c;

            float p = d * 0.3f;
            float a = c;
            float s = p / 4;
            float postFix = a *  Pow(2, 10 * t--);

            return -(postFix * Sin(((t * d) - s) * (2 * PI) / p)) + b;
        }

        /// <summary>
        /// Elastic Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        public static float Out(float t, float b, float c, float d)
        {
            if (t == 0) return b;

            if ((t /= d) == 1) return b + c;

            float p = d * 0.3f;
            float a = c;
            float s = p / 4;

            return (a * Pow(2, -10 * t) * Sin(((t * d) - s) * (2 * PI) / p)) + c + b;
        }

        /// <summary>
        /// Elastic Easing functions
        /// </summary>
        /// <param name="t">current time (in any unit measure, but same unit as duration)</param>
        /// <param name="b">starting value to interpolate</param>
        /// <param name="c">the total change in value of b that needs to occur</param>
        /// <param name="d">total time it should take to complete (duration)</param>
        /// <returns></returns>
        public static float InOut(float t, float b, float c, float d)
        {
            if (t == 0) return b;

            if ((t /= d / 2) == 2) return b + c;

            float p = d * (0.3f * 1.5f);
            float a = c;
            float s = p / 4;

            float postFix;
            if (t < 1)
            {
                postFix = a * Pow(2, 10 * (--t));
                return (-0.5f * (postFix * Sin(((t * d) - s) * (2 * PI) / p))) + b;
            }

            postFix = a * Pow(2, -10 * (--t));

            return (postFix * Sin(((t * d) - s) * (2 * PI) / p) * 0.5f) + c + b;
        }
    }
}
