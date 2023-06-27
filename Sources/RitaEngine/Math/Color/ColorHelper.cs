namespace RitaEngine.Math.Color;
    
using static RitaEngine.Math.Helper;
public static class ColorHelper
{
    public static float[] PaletteToRGBA( Palette color)
	{
        float[] cc = {  
            Round((float)Get_r((uint)color)  / (float)byte.MaxValue,2),
            Round((float)Get_g((uint)color)  / (float)byte.MaxValue,2),
            Round((float)Get_b((uint)color)  / (float)byte.MaxValue,2),
            Round((float)Get_a((uint)color)  / (float)byte.MaxValue,2)
        };
        return cc;	
	}

    public static byte Get_a( uint argbcolor) =>  (byte) (argbcolor >> 24);
	public static byte Get_r( uint argbcolor) =>  (byte) (argbcolor >> 16);
	public static byte Get_g( uint argbcolor) =>  (byte) (argbcolor >> 8);
	public static byte Get_b( uint argbcolor) =>  (byte) (argbcolor & 0x000000FF );

     public static uint ToABGR(byte r, byte g, byte b, byte alpha)
		=> ((uint)alpha << 24) | ((uint)b << 16) | ((uint)g << 8) | (r);
    
    public static uint ToARGB(byte a, byte r, byte g, byte b)
    	=>  (uint) ((((uint) a) << 24) | (((uint) r) << 16)  | (((uint) g) << 8) | ((uint) b));

	
    
	public static byte[] ConvertCmykToRgb(float c, float m, float y, float k)
    {
        byte r;
        byte g;
        byte b;

        r = System.Convert.ToByte(255 * (1 - c) * (1 - k));
        g = System.Convert.ToByte(255 * (1 - m) * (1 - k));
        b = System.Convert.ToByte(255 * (1 - y) * (1 - k));

        return new byte[] {r, g, b};
    }

    public static float[] ToCmyk(int r, int g, int b)
    {
        float c;
        float m;
        float y;
        float k;
        float rf;
        float gf;
        float bf;

        rf = r / 255F;
        gf = g / 255F;
        bf = b / 255F;

        k = ClampCmyk(1 - System.Math.Max(System.Math.Max(rf, gf), bf));
        c = ClampCmyk((1 - rf - k) / (1 - k));
        m = ClampCmyk((1 - gf - k) / (1 - k));
        y = ClampCmyk((1 - bf - k) / (1 - k));

        return new float[]{c, m, y, k};
    }

    private static float ClampCmyk(float value)
    {
        if (value < 0 || float.IsNaN(value))
        {
            value = 0;
        }

        return value;
    }    

     /// <summary>
    /// 
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    /// <param name="a"></param>
    /// <returns></returns>
    public static byte[] ToBytes(float r, float g, float b,float a)
    {
        byte[] colors = {
            (byte) Max(0 , Min( 255, (int) Floor(r * 256.0f))),
            (byte) Max(0 , Min( 255, (int) Floor(g * 256.0f))),
            (byte) Max(0 , Min( 255, (int) Floor(b * 256.0f))),
            (byte) Max(0 , Min( 255, (int) Floor(a * 256.0f)))
        };
        return colors;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    /// <param name="a"></param>
    /// <returns></returns>
    public static float[] ToFloats( in byte r, in byte g, in byte b ,in byte a)
    {
        float[] cc = {  
            (float)r  / byte.MaxValue,
            (float)g  / byte.MaxValue,
            (float)b  / byte.MaxValue,
            (float)a  / byte.MaxValue
        };
        return cc;
    }

    public static float[] ToFloats( byte[] rgba)
    {
        float[] cc = {  
            (float)rgba[0]  / byte.MaxValue,
            (float)rgba[1]  / byte.MaxValue,
            (float)rgba[2]  / byte.MaxValue,
            (float)rgba[3]  / byte.MaxValue
        };
        return cc;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float[] ToHSL(float r, float g , float b )
    {
        float h = 0.0f;
        float min = System.MathF.Min( System.MathF.Min(r, g), b);
        float v = System.MathF.Max(System.MathF.Max(r, g), b);
        float delta = v - min;
        float s = (v ==0.0f )? 0.0f: delta /v ;
        
        if (s == 0.0f)
            h = 0.0f;
        else
        {
            if (r == v)
                h = (g - b) / delta;
            else if (g == v)
                h = 2 + (b - r) / delta;
            else if (b == v)
                h = 4 + (r - g) / delta;

            h *= 60;

            if (h < 0.0)
                h = h + 360;
        }

        return new float[] { h, s, (v / 255)};
    }

        /// <summary>
    /// Convert HSV to RGB
    /// h is from 0-360
    /// s,v values are 0-1
    /// r,g,b values are 0-255
    /// Based upon http://ilab.usc.edu/wiki/index.php/HSV_And_H2SV_Color_Space#HSV_Transformation_C_.2F_C.2B.2B_Code_2
    /// </summary>
    public static byte[] ToRGB(float H , float S, float L)
    {
        if (S == 0)
            return new byte[]{ (byte)(L*byte.MaxValue),(byte)( L *byte.MaxValue),(byte)( L *byte.MaxValue)  } ;

        float h = (H == 360) ? 0 : H / 60;

        int i = (int)System.MathF.Truncate(h);

        float f = h - i;
        float p = L * (1.0f - S);
        float q = L * (1.0f - (S * f));
        float t = L * (1.0f - (S * (1.0f - f)));

        return  i switch
        {
            0 =>new byte[] { (byte)( L *byte.MaxValue),(byte)( t *byte.MaxValue),(byte)( p *byte.MaxValue)  },
            1 =>new byte[] { (byte)( q *byte.MaxValue),(byte)( L *byte.MaxValue),(byte)( p *byte.MaxValue)  },
            2 =>new byte[] { (byte)( p *byte.MaxValue),(byte)( L *byte.MaxValue),(byte)( t *byte.MaxValue)  },
            3 =>new byte[] { (byte)( p *byte.MaxValue),(byte)( q *byte.MaxValue),(byte)( L *byte.MaxValue)  },
            4 =>new byte[] { (byte)( t *byte.MaxValue),(byte)( p *byte.MaxValue),(byte)( L *byte.MaxValue)  },
            _ => new byte[] { (byte)( L *byte.MaxValue),(byte)( p *byte.MaxValue),(byte)( q *byte.MaxValue)  }
        };
    }
}


// 	public static byte[] ConvertCmykToRgb(float c, float m, float y, float k)
//     {
//         byte r;
//         byte g;
//         byte b;

//         r = Convert.ToByte(255 * (1 - c) * (1 - k));
//         g = Convert.ToByte(255 * (1 - m) * (1 - k));
//         b = Convert.ToByte(255 * (1 - y) * (1 - k));

//         return new byte[] {r, g, b};
//     }

//     public static float[] ToCmyk(int r, int g, int b)
//     {
//         float c;
//         float m;
//         float y;
//         float k;
//         float rf;
//         float gf;
//         float bf;

//         rf = r / 255F;
//         gf = g / 255F;
//         bf = b / 255F;

//         k = ClampCmyk(1 - System.Math.Max(System.Math.Max(rf, gf), bf));
//         c = ClampCmyk((1 - rf - k) / (1 - k));
//         m = ClampCmyk((1 - gf - k) / (1 - k));
//         y = ClampCmyk((1 - bf - k) / (1 - k));

//         return new float[]{c, m, y, k};
//     }

//     private static float ClampCmyk(float value)
//     {
//         if (value < 0 || float.IsNaN(value))
//         {
//             value = 0;
//         }

//         return value;
//     }

//     public static byte[] ToBytes(float r, float g, float b,float a)
//     {
//         byte[] colors = {
//             (byte) MaxI(0 , MinI( 255, (int) Floor(r * 256.0f))),
//             (byte) MaxI(0 , MinI( 255, (int) Floor(g * 256.0f))),
//             (byte) MaxI(0 , MinI( 255, (int) Floor(b * 256.0f))),
//             (byte) MaxI(0 , MinI( 255, (int) Floor(a * 256.0f)))
//         };
//         return colors;
//     }
//     /// <summary>
//     /// .
//     /// </summary>
//     /// <param name="r"></param>
//     /// <param name="g"></param>
//     /// <param name="b"></param>
//     /// <param name="a"></param>
//     /// <returns></returns>
//     public static float[] ToFloats( in byte r, in byte g, in byte b ,in byte a)
//     {
//         float[] cc = {
//             (float)r  / byte.MaxValue,
//             (float)g  / byte.MaxValue,
//             (float)b  / byte.MaxValue,
//             (float)a  / byte.MaxValue
//         };
//         return cc;
//     }

//     public static float[] ToFloats( byte[] rgba)
//     {
//         float[] cc = {
//             (float)rgba[0]  / byte.MaxValue,
//             (float)rgba[1]  / byte.MaxValue,
//             (float)rgba[2]  / byte.MaxValue,
//             (float)rgba[3]  / byte.MaxValue
//         };
//         return cc;
//     }

//     public static float[] ToHSL(float r, float g , float b )
//     {
//         float h = 0.0f;
//         float min = System.MathF.Min( System.MathF.Min(r, g), b);
//         float v = System.MathF.Max(System.MathF.Max(r, g), b);
//         float delta = v - min;
//         float s = (v ==0.0f )? 0.0f: delta /v ;

//         if (s == 0.0f)
//         {
//             h = 0.0f;
//         }
//         else
//         {
//             if (r == v)
//                 h = (g - b) / delta;
//             else if (g == v)
//                 h = 2 + ((b - r) / delta);
//             else if (b == v)
//                 h = 4 + ((r - g) / delta);

//             h *= 60;

//             if (h < 0.0)
//                 h += 360;
//         }

//         return new float[] { h, s, v / 255 };
//     }

//     /// <summary> Gets the value of the hue channel in degrees. </summary>
//     public float H;
//     /// <summary>  Gets the value of the saturation channel. </summary>
//     public float S;
//     /// <summary> Gets the value of the lightness channel. </summary>
//     public float L;

//     private static float NormalizeHue(float h)
//     {
//         if (h < 0) return h + (360 *((int) (h/360) + 1));
//         return h%360;
//     }

//     /// <summary>
//     ///     Initializes a new instance of the <see cref="ColorHSL" /> structure.
//     /// </summary>
//     /// <param name="h">The value of the hue channel.</param>
//     /// <param name="s">The value of the saturation channel.</param>
//     /// <param name="l">The value of the lightness channel.</param>
//     public Color(float h, float s, float l)
//     {
//         // normalize the hue
//         H = NormalizeHue(h);
//         S = Rita.Engine.Math.Helper.ClampFloat(s, 0f, 1f);
//         L = Rita.Engine.Math.Helper.ClampFloat(l, 0f, 1f);
//     }

//     /// <summary>
//     ///     Copies the individual channels of the color to the specified memory location.
//     /// </summary>
//     /// <param name="destination">The memory location to copy the axis to.</param>
//     // public void CopyTo(out HSL destination)
//     // {
//     //     destination = new HSL(H, S, L);
//     // }

//     /// <summary>
//     ///     Destructures the color, exposing the individual channels.
//     /// </summary>
//     public void Destructure(out float h, out float s, out float l)
//     {
//         h = H;
//         s = S;
//         l = L;
//     }
// //     /// <inheritdoc />
// //     public int CompareTo(ColorHSL other)
// //     {
// //         // ReSharper disable ImpureMethodCallOnReadonlyValueField
// //         return H.CompareTo(other.H)*100 + S.CompareTo(other.S)*10 + L.CompareTo(L);
// //         // ReSharper restore ImpureMethodCallOnReadonlyValueField
// //     }

// //     /// <summary>
// //     ///     Returns a <see cref="System.String" /> that represents this instance.
// //     /// </summary>
// //     /// <returns>
// //     ///     A <see cref="System.String" /> that represents this instance.
// //     /// </returns>
// //     public override string ToString()
// //     {
// //         return string.Format(CultureInfo.InvariantCulture, "H:{0:N1}° S:{1:N1} L:{2:N1}",
// //             H, 100*S, 100*L);
// //     }

// //     /// <summary>
// //     /// PArser string to HSL ?
// //     /// </summary>
// //     /// <param name="s"></param>
// //     /// <returns></returns>
// //     public static ColorHSL Parse(string s)
// //     {
// //         var hsl = s.Split(',');
// //         var hue = float.Parse(hsl[0].TrimEnd('°'), CultureInfo.InvariantCulture.NumberFormat);
// //         var sat = float.Parse(hsl[1], CultureInfo.InvariantCulture.NumberFormat);
// //         var lig = float.Parse(hsl[2], CultureInfo.InvariantCulture.NumberFormat);

// //         return new ColorHSL(hue, sat, lig);
// //     }

// //     /// <summary>
// //     ///     Implements the operator ==.
// //     /// </summary>
// //     /// <param name="x">The lvalue.</param>
// //     /// <param name="y">The rvalue.</param>
// //     /// <returns>
// //     ///     <c>true</c> if the lvalue <see cref="ColorHSL" /> is equal to the rvalue; otherwise, <c>false</c>.
// //     /// </returns>
// //     public static bool operator ==(ColorHSL x, ColorHSL y)
// //     {
// //         return x.Equals(y);
// //     }

// //     /// <summary>
// //     ///     Implements the operator !=.
// //     /// </summary>
// //     /// <param name="x">The lvalue.</param>
// //     /// <param name="y">The rvalue.</param>
// //     /// <returns>
// //     ///     <c>true</c> if the lvalue <see cref="ColorHSL" /> is not equal to the rvalue; otherwise, <c>false</c>.
// //     /// </returns>
// //     public static bool operator !=(ColorHSL x, ColorHSL y)
// //     {
// //         return !x.Equals(y);
// //     }
// //     /// <summary>
// //     /// 
// //     /// </summary>
// //     /// <param name="a"></param>
// //     /// <param name="b"></param>
// //     /// <returns></returns>
// //     public static ColorHSL operator -(ColorHSL a, ColorHSL b)
// //     {
// //         return new ColorHSL(a.H - b.H, a.S - b.S, a.L - b.L);
// //     }
// //     /// <summary>
// //     /// 
// //     /// </summary>
// //     /// <param name="c1"></param>
// //     /// <param name="c2"></param>
// //     /// <param name="t"></param>
// //     /// <returns></returns>
// //     public static ColorHSL Lerp(ColorHSL c1, ColorHSL c2, float t)
// //     {
// //         // loop around if c2.H < c1.H
// //         var h2 = c2.H >= c1.H ? c2.H : c2.H + 360;
// //         return new ColorHSL(
// //             c1.H + t*(h2 - c1.H),
// //             c1.S + t*(c2.S - c1.S),
// //             c1.L + t*(c2.L - c2.L));
// //     }

//     // public static Color FromRgbToHsl(Color color)
//     // {
//     //     // derived from http://www.geekymonkey.com/Programming/CSharp/RGB2HSL_HSL2RGB.htm
//     //     var r = color.R / 255f;
//     //     var g = color.G / 255f;
//     //     var b = color.B / 255f;
//     //     var h = 0f; // default to black
//     //     var s = 0f;
//     //     var l = 0f;
//     //     var v = Max(r, g);
//     //     v = Max(v, b);

//     //     var m = Min(r, g);
//     //     m = Min(m, b);
//     //     l = (m + v) / 2.0f;

//     //     if (l <= 0.0)
//     //         return new ColorHSL(h, s, l);

//     //     var vm = v - m;
//     //     s = vm;

//     //     if (s > 0.0)
//     //         s /= l <= 0.5f ? v + m : 2.0f - v - m;
//     //     else
//     //         return new ColorHSL(h, s, l);

//     //     var r2 = (v - r) / vm;
//     //     var g2 = (v - g) / vm;
//     //     var b2 = (v - b) / vm;

//     //     if (Abs(r - v) < float.Epsilon)
//     //         h = Abs(g - m) < float.Epsilon ? 5.0f + b2 : 1.0f - g2;
//     //     else if (Abs(g - v) < float.Epsilon)
//     //         h = Abs(b - m) < float.Epsilon ? 1.0f + r2 : 3.0f - b2;
//     //     else
//     //         h = Abs(r - m) < float.Epsilon ? 3.0f + g2 : 5.0f - r2;

//     //     h *= 60;
//     //     h = NormalizeHue(h);

//     //     return new ColorHSL(h, s, l);
//     // }
// // }

//     /// <summary>
//     /// Convert HSV to RGB
//     /// h is from 0-360
//     /// s,v values are 0-1
//     /// r,g,b values are 0-255
//     /// Based upon http://ilab.usc.edu/wiki/index.php/HSV_And_H2SV_Color_Space#HSV_Transformation_C_.2F_C.2B.2B_Code_2
//     /// </summary>
//     public static byte[] ToRGB(float H , float S, float L)
//     {
//         if (S == 0)
//             return new byte[]{ (byte)(L*byte.MaxValue),(byte)( L *byte.MaxValue),(byte)( L *byte.MaxValue)  } ;

//         float h = (H == 360) ? 0 : H / 60;

//         int i = (int)System.MathF.Truncate(h);

//         float f = h - i;
//         float p = L * (1.0f - S);
//         float q = L * (1.0f - (S * f));
//         float t = L * (1.0f - (S * (1.0f - f)));

//         return  i switch
//         {
//             0 =>new byte[] { (byte)( L *byte.MaxValue),(byte)( t *byte.MaxValue),(byte)( p *byte.MaxValue)  },
//             1 =>new byte[] { (byte)( q *byte.MaxValue),(byte)( L *byte.MaxValue),(byte)( p *byte.MaxValue)  },
//             2 =>new byte[] { (byte)( p *byte.MaxValue),(byte)( L *byte.MaxValue),(byte)( t *byte.MaxValue)  },
//             3 =>new byte[] { (byte)( p *byte.MaxValue),(byte)( q *byte.MaxValue),(byte)( L *byte.MaxValue)  },
//             4 =>new byte[] { (byte)( t *byte.MaxValue),(byte)( p *byte.MaxValue),(byte)( L *byte.MaxValue)  },
//             _ => new byte[] { (byte)( L *byte.MaxValue),(byte)( p *byte.MaxValue),(byte)( q *byte.MaxValue)  }
//         };
//     }

// 	public override string ToString()
//     {
//         Palette p = (Palette)ARGB;

//         return $"Color : {p}";
//     }
