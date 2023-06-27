namespace RitaEngine.Resources ;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RitaEngine.Resources.Fonts;


/// <summary>
/// Class wrapper for use Stb truetype  used in resources too
/// </summary>
public sealed class FontsHelper
{
    private const int FontBitmapWidth = 8192;
    private const int FontBitmapHeight = 8192;

    /// <summary>
    /// 
    /// </summary>
    public static void LoadFont()
    {
        var fontBaker = new FontBaker();

        fontBaker.Begin(FontBitmapWidth, FontBitmapHeight);
        fontBaker.Add(File.ReadAllBytes("Fonts/DroidSans.ttf"), 32, new[]
        {
            CharacterRange.BasicLatin,
            CharacterRange.Latin1Supplement,
            CharacterRange.LatinExtendedA,
            CharacterRange.Cyrillic,
            CharacterRange.Greek,
        });

        fontBaker.Add(File.ReadAllBytes("Fonts/DroidSansJapanese.ttf"), 32, new[]
        {
            CharacterRange.Hiragana,
            CharacterRange.Katakana,
        });

        fontBaker.Add(File.ReadAllBytes("Fonts/ZCOOLXiaoWei-Regular.ttf"), 32, new[]
        {
            CharacterRange.CjkSymbolsAndPunctuation,
            CharacterRange.CjkUnifiedIdeographs
        });

        fontBaker.Add(File.ReadAllBytes("Fonts/KoPubBatang-Regular.ttf"), 32, new[]
        {
            CharacterRange.HangulCompatibilityJamo,
            CharacterRange.HangulSyllables
        });

        var _charData = fontBaker.End();

        // Offset by minimal offset
        int minimumOffsetY = 10000;
        foreach (var pair in _charData.Glyphs)
        {
            if (pair.Value.YOffset < minimumOffsetY)
            {
                minimumOffsetY = pair.Value.YOffset;
            }
        }

        var keys = _charData.Glyphs.Keys.ToArray();
        foreach (var key in keys)
        {
            var pc = _charData.Glyphs[key];
            pc.YOffset -= minimumOffsetY;
            _charData.Glyphs[key] = pc;
        }

        // var rgb = new Color[FontBitmapWidth * FontBitmapHeight];
        // for (var i = 0; i < _charData.Bitmap.Length; ++i)
        // {
        //     var b = _charData.Bitmap[i];
        //     rgb[i].R = b;
        //     rgb[i].G = b;
        //     rgb[i].B = b;

        //     rgb[i].A = b;
        // }

        // var fontTexture = new Texture2D(GraphicsDevice, FontBitmapWidth, FontBitmapHeight);
        // fontTexture.SetData(rgb);

        // var glyphBounds = new List<Rectangle>();
        // var cropping = new List<Rectangle>();
        // var chars = new List<char>();
        // var kerning = new List<Vector3>();

        // var orderedKeys = _charData.Glyphs.Keys.OrderBy(a => a);
        // foreach (var key in orderedKeys)
        // {
        //     var character = _charData.Glyphs[key];

        //     var bounds = new Rectangle(character.X, character.Y,
        //                             character.Width,
        //                             character.Height);

        //     glyphBounds.Add(bounds);
        //     cropping.Add(new Rectangle(character.XOffset, character.YOffset, bounds.Width, bounds.Height));

        //     chars.Add((char)key);

        //     kerning.Add(new Vector3(0, bounds.Width, character.XAdvance - bounds.Width));
        // }

        // var constructorInfo = typeof(SpriteFont).GetTypeInfo().DeclaredConstructors.First();
        // _font = (SpriteFont)constructorInfo.Invoke(new object[]
        // {
        //     fontTexture, glyphBounds, cropping,
        //     chars, 20, 0, kerning, ' '
        // });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public float GetWidth(string text) 
    { 
        float length = 0f; 
        for (int i = 0; i < text.Length; i++) 
        { 
            // IntBuffer advancewidth = BufferUtils.createIntBuffer(1); 
            // IntBuffer leftsidebearing = BufferUtils.createIntBuffer(1); 
            // StbTrueType.stbtt_GetCodepointHMetrics(info, (int)text.charAt(i), advancewidth, leftsidebearing); 
            // length += advancewidth; 
        } 
        return length ; 
    } 

    //GETGlyphsList
}


/// <summary>
/// 
/// </summary>
public struct CharacterRange
{
    /// <summary> . </summary>
    public static readonly CharacterRange BasicLatin = new CharacterRange(0x0020, 0x007F);
    /// <summary> . </summary>
    public static readonly CharacterRange Latin1Supplement = new CharacterRange(0x00A0, 0x00FF);
    /// <summary> . </summary>
    public static readonly CharacterRange LatinExtendedA = new CharacterRange(0x0100, 0x017F);
    /// <summary> . </summary>
    public static readonly CharacterRange LatinExtendedB = new CharacterRange(0x0180, 0x024F);
    /// <summary> . </summary>
    public static readonly CharacterRange Cyrillic = new CharacterRange(0x0400, 0x04FF);
    /// <summary> . </summary>
    public static readonly CharacterRange CyrillicSupplement = new CharacterRange(0x0500, 0x052F);
    /// <summary> . </summary>
    public static readonly CharacterRange Hiragana = new CharacterRange(0x3040, 0x309F);
    /// <summary> . </summary>
    public static readonly CharacterRange Katakana = new CharacterRange(0x30A0, 0x30FF);
    /// <summary> . </summary>
    public static readonly CharacterRange Greek = new CharacterRange(0x0370, 0x03FF);
    /// <summary> . </summary>
    public static readonly CharacterRange CjkSymbolsAndPunctuation = new CharacterRange(0x3000, 0x303F);
    /// <summary> . </summary>
    public static readonly CharacterRange CjkUnifiedIdeographs = new CharacterRange(0x4e00, 0x9fff);
    /// <summary> . </summary>
    public static readonly CharacterRange HangulCompatibilityJamo = new CharacterRange(0x3130, 0x318f);
    /// <summary> . </summary>
    public static readonly CharacterRange HangulSyllables = new CharacterRange(0xac00, 0xd7af);
    
    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public int Start    {
        get;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public int End
    {
        get;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public int Size
    {
        get
        {
            return End - Start + 1;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public CharacterRange(int start, int end)
    {
        Start = start;
        End = end;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="single"></param>
    /// <returns></returns>
    public CharacterRange(int single) : this(single, single)
    {
    }
}

/// <summary>
/// 
/// </summary>
public struct GlyphInfo
{
    /// <summary> . </summary>
    public int X;
    /// <summary> . </summary>
    public int  Y;
    /// <summary> . </summary>
    public int  Width;
    /// <summary> . </summary>
    public int  Height;
    /// <summary> . </summary>
    public int XOffset;
    /// <summary> . </summary>
    public int  YOffset;
    /// <summary> . </summary>
    public int XAdvance;
    
}
/// <summary>
/// 
/// </summary>
public class FontBakerResult
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="glyphs"></param>
    /// <param name="bitmap"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public FontBakerResult(Dictionary<int, GlyphInfo> glyphs, byte[] bitmap, int width, int height)
    {
        if (glyphs == null)
            throw new ArgumentNullException(nameof(glyphs));

        if (bitmap == null)
            throw new ArgumentNullException(nameof(bitmap));

        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(width));

        if (height <= 0)
            throw new ArgumentOutOfRangeException(nameof(height));

        if (bitmap.Length < width * height)
            throw new ArgumentException("pixels.Length should be higher than width * height");

        Glyphs = glyphs;
        Bitmap = bitmap;
        Width = width;
        Height = height;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public Dictionary<int, GlyphInfo> Glyphs
    {
        get;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public byte[] Bitmap
    {
        get;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public int Width
    {
        get;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public int Height
    {
        get;
    }
}

/// <summary>
/// 
/// </summary>
public unsafe class FontBaker
{
    /// <summary> . </summary>
    private byte[]? _bitmap;
    /// <summary> . </summary>
    private StbTrueType.stbtt_pack_context? _context;
    private Dictionary<int, GlyphInfo>? _glyphs;
    private int bitmapWidth, bitmapHeight;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void Begin(int width, int height)
    {
        bitmapWidth = width;
        bitmapHeight = height;
        _bitmap = new byte[width * height];
        _context = new StbTrueType.stbtt_pack_context();

        fixed (byte* pixelsPtr = _bitmap)
        {
            StbTrueType.stbtt_PackBegin(_context, pixelsPtr, width, height, width, 1, null);
        }

        _glyphs = new Dictionary<int, GlyphInfo>();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ttf"></param>
    /// <param name="fontPixelHeight"></param>
    /// <param name="characterRanges"></param>
    public void Add(byte[] ttf, float fontPixelHeight, IEnumerable<CharacterRange> characterRanges)
    {
        if (ttf == null || ttf.Length == 0)
            throw new ArgumentNullException(nameof(ttf));

        if (fontPixelHeight <= 0)
            throw new ArgumentOutOfRangeException(nameof(fontPixelHeight));

        if (characterRanges == null)
            throw new ArgumentNullException(nameof(characterRanges));

        if (!characterRanges.Any())
            throw new ArgumentException("characterRanges must have a least one value.");

        var fontInfo = StbTrueType.CreateFont(ttf, 0);
        if (fontInfo == null)
            throw new Exception("Failed to init font.");

        var scaleFactor = StbTrueType.stbtt_ScaleForPixelHeight(fontInfo, fontPixelHeight);

        int ascent, descent, lineGap;
        StbTrueType.stbtt_GetFontVMetrics(fontInfo, &ascent, &descent, &lineGap);

        foreach (var range in characterRanges)
        {
            if (range.Start > range.End)
                continue;

            var cd = new StbTrueType.stbtt_packedchar[range.End - range.Start + 1];
            fixed (StbTrueType.stbtt_packedchar* chardataPtr = cd)
            {
                StbTrueType.stbtt_PackFontRange(_context!, fontInfo.data, 0, fontPixelHeight,
                    range.Start,
                    range.End - range.Start + 1,
                    chardataPtr);
            }

            for (var i = 0; i < cd.Length; ++i)
            {
                var yOff = cd[i].yoff;
                yOff += ascent * scaleFactor;

                var glyphInfo = new GlyphInfo
                {
                    X = cd[i].x0,
                    Y = cd[i].y0,
                    Width = cd[i].x1 - cd[i].x0,
                    Height = cd[i].y1 - cd[i].y0,
                    XOffset = (int)cd[i].xoff,
                    YOffset = (int)Math.Round(yOff),
                    XAdvance = (int)Math.Round(cd[i].xadvance)
                };

                _glyphs![i + range.Start] = glyphInfo;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public FontBakerResult End()
    {
        return new FontBakerResult(_glyphs!, _bitmap!, bitmapWidth, bitmapHeight);
    }
}

/*
FontStashSharp
https://github.com/FontStashSharp/FontStashSharp/tree/main/src/FontStashSharp

*/

// namespace RITAENGINE.PLATFORM.FONT;

// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Runtime.InteropServices;
// using System.Threading.Tasks;

// using RITAENGINE.MATH;

// public interface IFontStashRenderer
// {
//     // GraphicsDevice GraphicsDevice { get; }

//     // void Draw(Texture2D texture, Vector2 pos, Rectangle? src, Color color, float rotation, Vector2 scale, float depth);
// }

// [StructLayout(LayoutKind.Sequential, Pack = 1)]
// public struct VertexPositionColorTexture
// {
//     /// <summary>
//     /// Position
//     /// </summary>
//     public Vector3 Position;

//     // /// <summary>
//     // /// Color
//     // /// </summary>
//     // public FSColor Color;

//     /// <summary>
//     /// Texture Coordinate
//     /// </summary>
//     public Vector2 TextureCoordinate;
// }


// public interface IFontStashRenderer2
// {
//     // GraphicsDevice GraphicsDevice { get; }

//     // void DrawQuad(Texture2D texture, ref VertexPositionColorTexture topLeft, ref VertexPositionColorTexture topRight, ref VertexPositionColorTexture bottomLeft, ref VertexPositionColorTexture bottomRight);
// }

// /// <summary>
// /// Texture Creation Service
// /// </summary>
// public interface ITexture2DManager
// {
//     /// <summary>
//     /// Creates a texture of the specified size
//     /// </summary>
//     /// <param name="width"></param>
//     /// <param name="height"></param>
//     /// <returns></returns>
//     object CreateTexture(int width, int height);

//     // /// <summary>
//     // /// Returns size of the specified texture
//     // /// </summary>
//     // /// <param name="texture"></param>
//     // /// <returns></returns>
//     // Point GetTextureSize(object texture);

//     // /// <summary>
//     // /// Sets RGBA data at the specified bounds
//     // /// </summary>
//     // /// <param name="bounds"></param>
//     // /// <param name="data"></param>
//     // void SetTextureData(object texture, Rectangle bounds, byte[] data);
// }

// //RICH TEXT

// public abstract class BaseChunk
// {
//     public abstract Point Size { get; }

//     public int LineIndex { get; internal set; }
//     public int ChunkIndex { get; internal set; }
//     public int VerticalOffset { get; internal set; }
//     public Color? Color { get; set; }

//     protected BaseChunk()
//     {
//     }

//     public abstract void Draw(FSRenderContext context, Vector2 position, Color color);
// }

// internal enum ChunkInfoType
// 	{
// 		Text,
// 		Space,
// 		Image
// 	}

// 	internal struct ChunkInfo
// 	{
// 		public ChunkInfoType Type;
// 		public int X;
// 		public int Y;
// 		public bool LineEnd;
// 		public int StartIndex, EndIndex;
// 		public IRenderable Renderable;

// 		public int Width
// 		{
// 			get
// 			{
// 				if (Type == ChunkInfoType.Image)
// 				{
// 					return Renderable.Size.X;
// 				}

// 				return X;
// 			}
// 		}

// 		public int Height
// 		{
// 			get
// 			{
// 				if (Type == ChunkInfoType.Image)
// 				{
// 					return Renderable.Size.Y;
// 				}

// 				return Y;
// 			}
// 		}
// 	}


//     	public static class ColorStorage
// 	{
// 		public class ColorInfo
// 		{
// 			public Color Color { get; set; }
// 			public string Name { get; set; }
// 		}

// 		public static readonly Dictionary<string, ColorInfo> Colors = new Dictionary<string, ColorInfo>();

// 		static ColorStorage()
// 		{
// 			var type = typeof(Color);

// 			var colors = type.GetRuntimeFields();
// 			foreach (var c in colors)
// 			{
// 				if (c.FieldType != typeof(Color))
// 				{
// 					continue;
// 				}

// 				var value = (Color)c.GetValue(null);
// 				Colors[c.Name.ToLower()] = new ColorInfo
// 				{
// 					Color = value,
// 					Name = c.Name
// 				};
// 			}

// 		}

// 		public static string ToHexString(this Color c)
// 		{
// 			return string.Format("#{0}{1}{2}{3}",
// 				c.R.ToString("X2"),
// 				c.G.ToString("X2"),
// 				c.B.ToString("X2"),
// 				c.A.ToString("X2"));
// 		}

// 		public static string GetColorName(this Color color)
// 		{
// 			foreach (var c in Colors)
// 			{
// 				if (c.Value.Color == color)
// 				{
// 					return c.Value.Name;
// 				}
// 			}

// 			return null;
// 		}

// 		public static Color? FromName(string name)
// 		{
// 			if (name.StartsWith("#"))
// 			{
// 				name = name.Substring(1);
// 				uint u;
// 				if (uint.TryParse(name, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out u))
// 				{
// 					// Parsed value contains color in RGBA form
// 					// Extract color components

// 					byte r = 0, g = 0, b = 0, a = 0;

// 					unchecked
// 					{
// 						if (name.Length == 6)
// 						{
// 							r = (byte)(u >> 16);
// 							g = (byte)(u >> 8);
// 							b = (byte)u;
// 							a = 255;
// 						}
// 						else if (name.Length == 8)
// 						{
// 							r = (byte)(u >> 24);
// 							g = (byte)(u >> 16);
// 							b = (byte)(u >> 8);
// 							a = (byte)u;
// 						}
// 					}

// 					return new Color(r, g, b, a);
// 				}
// 			}
// 			else
// 			{
// 				ColorInfo result;
// 				if (Colors.TryGetValue(name.ToLower(), out result))
// 				{
// 					return result.Color;
// 				}
// 			}

// 			return null;
// 		}

// 		public static Color CreateColor(int r, int g, int b, int a = 255)
// 		{
// 			return new Color((byte)r, (byte)g, (byte)b, (byte)a);
// 		}
// 	}

