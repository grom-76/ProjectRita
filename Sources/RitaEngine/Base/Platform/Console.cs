namespace RitaEngine.Platform;


//https://github.com/TasThief/ConsoleRenderingSystem/tree/master/FinalGame

/// <summary>
///  MAnage console
/// </summary>
public static class Consoles
{
    /// <summary>
    /// Simple wrapper ecrire du text
    /// </summary>
    /// <param name="text"></param>
    public static void DisplayRawText( string text)
        => System.Console.WriteLine(text);

    /// <summary>
    /// Modifier la taille de la fenetre de console
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public static void ChangeSize(int width, int height )
    {
        #pragma warning disable CA1416
        // System.Console.SetBufferSize(width,height) ;
        System.Console.SetWindowSize(width,height);
        #pragma warning restore CA1416
#if WINDOWS
        #pragma warning disable CA1416
        // System.Console.SetBufferSize(width,height) ;
        System.Console.SetWindowSize(width,height);
        #pragma warning restore CA1416
#elif LINUX

#endif
    }

    /// <summary>
    /// Affiche la console , ne fonctione que si hide avant
    /// </summary>
    public static void ShowConsole( )
    {
#if WINDOWS            
        var handle = Csl.GetConsoleWindow();
        Csl.ShowWindow(handle, Csl.SW_SHOW);
#elif LINUX

#endif

    }

    /// <summary>
    ///  cache la console actuelle , attention ne marche qu'avec CMD , ne fonctionne pas avec Windows Terminal
    /// </summary>
    public static void HideConsole()
    {
// #if WINDOWS    
        // send command Exit ??
        // System.Environment.Exit(0);
        // Application.Exit;

        // System.IntPtr handle = Console.GetConsoleWindow();
        // System.IntPtr handle = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
        // Win32.ShowWindow(handle, Win32.SW.HIDE);
// #elif LINUX

// #endif
    }

    /// <summary>
    /// Restore la couleur d'origine du texte dans la console
    /// </summary>
    public static void ResetColor()
        => System.Console.ResetColor();

    /// <summary>
    /// Modifie la couleur du texte dans la console
    /// </summary>
    /// <param name="c">  enum <see>ConsoleColor </see> </param>
    public static void ChangeColor(ConsoleColor c)
        => System.Console.ForegroundColor = (System.ConsoleColor)c;

    /// <summary>
    /// Modifier la couelr de la police
    /// </summary>
    /// <param name="color"></param>
    public static void ChangeColorInt(int color)
        => System.Console.ForegroundColor = (System.ConsoleColor)color;

    /// <summary>
    /// change color of foreground 
    /// </summary>
    public enum Color
    {
        ///<summary> Aucune</summary>
        None = 0,
        ///<summary> Gris </summary>
        Gray = (int)System.ConsoleColor.Gray,
        ///<summary> Rouge </summary>
        Red = (int)System.ConsoleColor.Red,
        ///<summary> Jaune</summary>
        Yellow = (int)System.ConsoleColor.Yellow,
        ///<summary> blanc </summary>
        White = (int)System.ConsoleColor.White,
        ///<summary>  Bleu</summary>
        Blue = (int)System.ConsoleColor.Blue,
    }

}
// // namespace RITAENGINE.PLATFORM;

// // using System;

// // public static class Consoles
// // {
    
// //     public static void WriteLine( string message)=> Console.WriteLine(message);
// //     public static void Write( string message)=> Console.Write(message);

// //      /// <summary>
// //     /// Modifier la taille de la fenetre de console
// //     /// </summary>
// //     /// <param name="width"></param>
// //     /// <param name="height"></param>
// //     public static void ChangeSize(int width, int height )
// //     {
// // //         #pragma warning disable CA1416
// // //         // System.Console.SetBufferSize(width,height) ;
// // //         System.Console.SetWindowSize(width,height);
// // //         #pragma warning restore CA1416
// // // #if WINDOWS
// // //         #pragma warning disable CA1416
// // //         // System.Console.SetBufferSize(width,height) ;
// // //         System.Console.SetWindowSize(width,height);
// // //         #pragma warning restore CA1416
// // // #elif LINUX

// // // #endif
// //     }

// //     /// <summary>
// //     /// Affiche la console , ne fonctione que si hide avant
// //     /// </summary>
// //     public static void ShowConsole( )
// //     {
// // // #if WINDOWS            
// // //         var handle = Csl.GetConsoleWindow();
// // //         Csl.ShowWindow(handle, Csl.SW_SHOW);
// // // #elif LINUX

// // // #endif

// //     }

// //     /// <summary>
// //     ///  cache la console actuelle , attention ne marche qu'avec CMD , ne fonctionne pas avec Windows Terminal
// //     /// </summary>
// //     public static void HideConsole()
// //     {
// // // #if WINDOWS    
// //         // send command Exit ??
// //         // System.Environment.Exit(0);
// //         // Application.Exit;

// //         // System.IntPtr handle = Console.GetConsoleWindow();
// //         // System.IntPtr handle = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
// //         // Win32.ShowWindow(handle, Win32.SW.HIDE);
// // // #elif LINUX

// // // #endif
// //     }

// //     /// <summary>
// //     /// Restore la couleur d'origine du texte dans la console
// //     /// </summary>
// //     public static void ResetColor()
// //         => System.Console.ResetColor();

// //     /// <summary>
// //     /// Modifie la couleur du texte dans la console
// //     /// </summary>
// //     /// <param name="c">  enum <see>ConsoleColor </see> </param>
// //     public static void ChangeColor(ConsoleColor c)
// //         => System.Console.ForegroundColor = (System.ConsoleColor)c;

// //     /// <summary>
// //     /// Modifier la couelr de la police
// //     /// </summary>
// //     /// <param name="color"></param>
// //     public static void ChangeColor(int color)
// //         => System.Console.ForegroundColor = (System.ConsoleColor)color;

// //     /// <summary>
// //     /// change color of foreground 
// //     /// </summary>
// //     public enum Color
// //     {
// //         ///<summary> Aucune</summary>
// //         None = 0,
// //         ///<summary> Gris </summary>
// //         Gray = (int)System.ConsoleColor.Gray,
// //         ///<summary> Rouge </summary>
// //         Red = (int)System.ConsoleColor.Red,
// //         ///<summary> Jaune</summary>
// //         Yellow = (int)System.ConsoleColor.Yellow,
// //         ///<summary> blanc </summary>
// //         White = (int)System.ConsoleColor.White,
// //         ///<summary>  Bleu</summary>
// //         Blue = (int)System.ConsoleColor.Blue,
// //     }
// // }
// /*
// https://github.com/ollelogdahl/ConsoleGameEngine

// ## Usage / Features
// Library contains two main classes, `ConsoleEngine` and `ConsoleGame`

// - Custom character screen buffer, allows clearing and blitting to console window
// - Console colors with full rgb capabilities
// - Custom & premade Palettes, used for changing console window palette
// - Accessing and setting pixels individually
// - Functions to draw basic shapes and primitives (Triangles, Rectangles, Lines etc.)
// - Writing characters to screen using plain-text and FIGlet fonts
// - Multiple game loops, including fixed framerate and deltatime settings
// - Point and Vector class, for int and float positions
// - Setting console window settings, changing window size and running console borderless
// - Input handling

// #### ConsoleEngine
// Is used to draw to the screen, replacement for the `System.Console` class *(kind of)*

// ```c#
// using ConsoleGameEngine;
// ...
// Engine = new ConsoleEngine(windowWidth, windowHeight, fontWidth, fontHeight);

// Engine.SetPixel(new Point(8, 8), ConsoleCharacter.Full, 15);

// ```

// #### ConsoleGame
// Keeps an instance of the `ConsoleEngine` and implements game loops.

// **Note** *Not neccessary, you could use the ConsoleEngine as is*

// ```c#
// using ConsoleGameEngine;
// ...

// new AppName.Construct(windowWidth, windowHeight, fontWidth, fontHeight, FramerateMode.Unlimited);
// class AppName : ConsoleGame {
// public override void Create() {
// }

// public override void Update() {
// }

// public override void Render() {
// }
// }
// ```
// */
// namespace RITAENGINE.PLATFORM.SYSTEM.CONSOLE;

// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Runtime.InteropServices;
// using System.Text;
// using System.Text.RegularExpressions;
// using System.Threading;
// using Microsoft.Win32.SafeHandles;

// /// <summary>
// /// Class for Drawing to a console window.
// /// </summary>
// public partial class ConsoleEngine {

//     // pekare för ConsoleHelper-anrop
//     private readonly IntPtr stdInputHandle = NativeMethods.GetStdHandle(-10);
//     private readonly IntPtr stdOutputHandle = NativeMethods.GetStdHandle(-11);
//     private readonly IntPtr stdErrorHandle = NativeMethods.GetStdHandle(-12);
//     private readonly IntPtr consoleHandle = NativeMethods.GetConsoleWindow();

//     /// <summary> The active color palette. </summary> <see cref="Color"/>
//     public Color[] Palette { get; private set; }=null!;

//     /// <summary> The current size of the font. </summary> <see cref="Point"/>
//     public Point FontSize { get; private set; }

//     /// <summary> The dimensions of the window in characters. </summary> <see cref="Point"/>
//     public Point WindowSize { get; private set; }

//     /*private char[,] CharBuffer { get; set; }
//     private int[,] ColorBuffer { get; set; }
//     private int[,] BackgroundBuffer { get; set; }*/
//     private Glyph[,] GlyphBuffer { get; set; }  
//     private int Background { get; set; }
//     private ConsoleBuffer ConsoleBuffer { get; set; }
//     private bool IsBorderless { get; set; }

//     /// <summary> Creates a new ConsoleEngine. </summary>
//     /// <param name="width">Target window width.</param>
//     /// <param name="height">Target window height.</param>
//     /// <param name="fontW">Target font width.</param>
//     /// <param name="fontH">Target font height.</param>
//     public ConsoleEngine(int width, int height, int fontW, int fontH) {
//         if (width < 1 || height < 1) throw new ArgumentOutOfRangeException();
//         if (fontW < 1 || fontH < 1) throw new ArgumentOutOfRangeException();

//         Console.Title = "Untitled console application";
//         Console.CursorVisible = false;
// #pragma warning disable
//         //sets console location to 0,0 to try and avoid the error where the console is to big
//         Console.SetWindowPosition(0, 0);

//         // sätter fönstret och bufferns storlek
//         // buffern måste sättas efter fönsret, eftersom den aldrig får vara mindre än skärmen
//         Console.SetWindowSize(width, height);
//         Console.SetBufferSize(width, height);
// #pragma warning restore
//         ConsoleBuffer = new ConsoleBuffer(width, height);

//         WindowSize = new Point(width, height);
//         FontSize = new Point(fontW, fontH);

//         /*CharBuffer = new char[width, height];
//         ColorBuffer = new int[width, height];
//         BackgroundBuffer = new int[width, height];*/

//         GlyphBuffer = new Glyph[width, height];
        

//         SetBackground(0);
//         SetPalette(Palettes.Default);

//         // Stänger av alla standard ConsoleInput metoder (Quick-edit etc)
//         NativeMethods.SetConsoleMode(stdInputHandle, 0x0080);

//         // Sätter fontstorlek och tvingar Raster (Terminal) / Consolas
//         // Detta måste göras efter SetBufferSize, annars ger den en IOException
//         ConsoleFont.SetFont(stdOutputHandle, (short)fontW, (short)fontH);
//     }

//     // Rita
//     public void SetPixel(Point selectedPoint, int color, char character)
//     {
//         SetPixel(selectedPoint,color,Background,character);
//     }

//     //new Draw method, which supports background
//     public void SetPixel(Point selectedPoint, int fgColor, int bgColor, char character) {
//         if (selectedPoint.X >= GlyphBuffer.GetLength(0) || selectedPoint.Y >= GlyphBuffer.GetLength(1)
//             || selectedPoint.X < 0 || selectedPoint.Y < 0) return;

//         /*CharBuffer[selectedPoint.X, selectedPoint.Y] = character;
//         ColorBuffer[selectedPoint.X, selectedPoint.Y] = fgColor;
//         BackgroundBuffer[selectedPoint.X, selectedPoint.Y] = bgColor;*/
//         GlyphBuffer[selectedPoint.X, selectedPoint.Y].set(character, fgColor, bgColor);
//     }

//     /// <summary>
//     /// returns gylfh at point given
//     /// </summary>
//     /// <param name="selectedPoint"></param>
//     /// <returns></returns>
//     public Glyph PixelAt(Point selectedPoint)
//     {
//         if(selectedPoint.X > 0 && selectedPoint.X < GlyphBuffer.GetLength(0) && selectedPoint.Y > 0 && selectedPoint.Y < GlyphBuffer.GetLength(1))
//             return GlyphBuffer[selectedPoint.X, selectedPoint.Y];
//         else return default;
//     }


//     /// <summary> Sets the console's color palette </summary>
//     /// <param name="colors"></param>
//     /// <exception cref="ArgumentException"/> <exception cref="ArgumentNullException"/>
//     public void SetPalette(Color[] colors) {
//         if (colors.Length > 16) throw new ArgumentException("Windows command prompt only support 16 colors.");
//         Palette = colors ?? throw new ArgumentNullException();

//         for (int i = 0; i < colors.Length; i++) {
//             ConsolePalette.SetColor(i, colors[i]);
//         }
//     }

//     /// <summary> Sets the console's background color to one in the active palette. </summary>
//     /// <param name="color">Index of background color in palette.</param>
//     public void SetBackground(int color = 0) {
//         if (color > 16 || color < 0) throw new IndexOutOfRangeException();
//         Background = color;
//     }

//     /// <summary>Gets Background</summary>
//     /// <returns>Returns the background</returns>
//     public int GetBackground()
//     {
//         return Background;
//     }

//     /// <summary> Clears the screenbuffer. </summary>
//     public void ClearBuffer() {
//         /*Array.Clear(CharBuffer, 0, CharBuffer.Length);
//         Array.Clear(ColorBuffer, 0, ColorBuffer.Length);
//         Array.Clear(BackgroundBuffer, 0, BackgroundBuffer.Length);*/
//         Array.Clear(GlyphBuffer, 0, GlyphBuffer.Length);
//     }

//     /// <summary> Blits the screenbuffer to the Console window. </summary>
//     public void DisplayBuffer() {
//         ConsoleBuffer.SetBuffer(GlyphBuffer, Background);
//         ConsoleBuffer.Blit();
//     }

//     /// <summary> Sets the window to borderless mode. </summary>
//     public void Borderless() {
//         IsBorderless = true;

//         int GWL_STYLE = -16;                // hex konstant för stil-förändring
//         int WS_BORDERLESS = 0x00080000;     // helt borderless

//         NativeMethods.Rect rect = new NativeMethods.Rect();
//         NativeMethods.Rect desktopRect = new NativeMethods.Rect();

//         NativeMethods.GetWindowRect(consoleHandle, ref rect);
//         IntPtr desktopHandle = NativeMethods.GetDesktopWindow();
//         NativeMethods.MapWindowPoints(desktopHandle, consoleHandle, ref rect, 2);
//         NativeMethods.GetWindowRect(desktopHandle, ref desktopRect);

//         Point wPos = new Point(
//             (desktopRect.Right / 2) - ((WindowSize.X * FontSize.X) / 2),
//             (desktopRect.Bottom / 2) - ((WindowSize.Y * FontSize.Y) / 2));

//         NativeMethods.SetWindowLong(consoleHandle, GWL_STYLE, WS_BORDERLESS);
//         NativeMethods.SetWindowPos(consoleHandle, -2, wPos.X, wPos.Y, rect.Right - 8, rect.Bottom - 8, 0x0040);

//         NativeMethods.DrawMenuBar(consoleHandle);
//     }

//     #region Primitives

//     /// <summary> Draws a single pixel to the screenbuffer. calls new method with Background as the bgColor </summary>
//     /// <param name="v">The Point that should be drawn to.</param>
//     /// <param name="color">The color index.</param>
//     /// <param name="c">The character that should be drawn with.</param>
//     public void SetPixel(Point v, int color, ConsoleCharacter c = ConsoleCharacter.Full) {
//         SetPixel(v, color, Background, (char)c);
//     }

//     /// <summary> Overloaded Method Draws a single pixel to the screenbuffer with custom bgColor. </summary>
//     /// <param name="v">The Point that should be drawn to.</param>
//     /// <param name="fgColor">The foreground color index.</param>
//     /// <param name="bgColor">The background color index.</param>
//     /// <param name="c">The character that should be drawn with.</param>
//     public void SetPixel(Point v, int fgColor, int bgColor, ConsoleCharacter c = ConsoleCharacter.Full)
//     {
//         SetPixel(v, fgColor, bgColor, (char)c);
//     }

//     /// <summary> Draws a frame using boxdrawing symbols, calls new method with Background as the bgColor. </summary>
//     /// <param name="pos">Top Left corner of box.</param>
//     /// <param name="end">Bottom Right corner of box.</param>
//     /// <param name="color">The specified color index.</param>
//     public void Frame(Point pos, Point end, int color) {
//         Frame(pos,end,color,Background);
//     }

//     /// <summary> Draws a frame using boxdrawing symbols. </summary>
//     /// <param name="pos">Top Left corner of box.</param>
//     /// <param name="end">Bottom Right corner of box.</param>
//     /// <param name="fgColor">The specified color index.</param>
//     /// <param name="bgColor">The specified background color index.</param>
//     public void Frame(Point pos, Point end, int fgColor, int bgColor)
//     {
//         for(int i = 1; i < end.X - pos.X; i++) {
//             SetPixel(new Point(pos.X + i, pos.Y), fgColor, bgColor, ConsoleCharacter.BoxDrawingL_H);
//             SetPixel(new Point(pos.X + i, end.Y), fgColor, bgColor, ConsoleCharacter.BoxDrawingL_H);
//         }

//         for(int i = 1; i < end.Y - pos.Y; i++) {
//             SetPixel(new Point(pos.X, pos.Y + i), fgColor, bgColor, ConsoleCharacter.BoxDrawingL_V);
//             SetPixel(new Point(end.X, pos.Y + i), fgColor, bgColor, ConsoleCharacter.BoxDrawingL_V);
//         }

//         SetPixel(new Point(pos.X, pos.Y), fgColor, bgColor, ConsoleCharacter.BoxDrawingL_DR);
//         SetPixel(new Point(end.X, pos.Y), fgColor, bgColor, ConsoleCharacter.BoxDrawingL_DL);
//         SetPixel(new Point(pos.X, end.Y), fgColor, bgColor, ConsoleCharacter.BoxDrawingL_UR);
//         SetPixel(new Point(end.X, end.Y), fgColor, bgColor, ConsoleCharacter.BoxDrawingL_UL);
//     }

//     /// <summary> Writes plain text to the buffer, calls new method with Background as the bgColor. </summary>
//     /// <param name="pos">The position to write to.</param>
//     /// <param name="text">String to write.</param>
//     /// <param name="color">Specified color index to write with.</param>
//     public void WriteText(Point pos, string text, int color) {
//         WriteText(pos,text,color,Background);
//     }

//     /// <summary> Writes plain text to the buffer. </summary>
//     /// <param name="pos">The position to write to.</param>
//     /// <param name="text">String to write.</param>
//     /// <param name="fgColor">Specified color index to write with.</param>
//     /// <param name="bgColor">Specified background color index to write with.</param>
//     public void WriteText(Point pos, string text, int fgColor, int bgColor)
//     {
//         for(int i = 0; i < text.Length; i++) {
//             SetPixel(new Point(pos.X + i, pos.Y), fgColor,bgColor, text[i]);
//         }
//     }

//     /// <summary>  Writes text to the buffer in a FIGlet font, calls new method with Background as the bgColor. </summary>
//     /// <param name="pos">The Top left corner of the text.</param>
//     /// <param name="text">String to write.</param>
//     /// <param name="font">FIGLET font to write with.</param>
//     /// <param name="color">Specified color index to write with.</param>
//     /// <see cref="FigletFont"/>
//     public void WriteFiglet(Point pos, string text, FigletFont font, int color) {
//         WriteFiglet(pos, text, font, color, Background);
//     }

//     /// <summary>  Writes text to the buffer in a FIGlet font. </summary>
//     /// <param name="pos">The Top left corner of the text.</param>
//     /// <param name="text">String to write.</param>
//     /// <param name="font">FIGLET font to write with.</param>
//     /// <param name="fgColor">Specified color index to write with.</param>
//     /// <param name="bgColor">Specified background color index to write with.</param>
//     /// <see cref="FigletFont"/>
//     public void WriteFiglet(Point pos, string text, FigletFont font, int fgColor, int bgColor)
//     {
//         if(text == null) throw new ArgumentNullException(nameof(text));
//         if(Encoding.UTF8.GetByteCount(text) != text.Length) throw new ArgumentException("String contains non-ascii characters");

//         int sWidth = FigletFont.GetStringWidth(font, text);

//         for(int line = 1; line <= font.Height; line++) {
//             int runningWidthTotal = 0;

//             for(int c = 0; c < text.Length; c++) {
//                 char character = text[c];
//                 string fragment = FigletFont.GetCharacter(font, character, line);
//                 for(int f = 0; f < fragment.Length; f++) {
//                     if(fragment[f] != ' ') {
//                         SetPixel(new Point(pos.X + runningWidthTotal + f, pos.Y + line - 1), fgColor,bgColor, fragment[f]);
//                     }
//                 }
//                 runningWidthTotal += fragment.Length;
//             }
//         }
//     }

//     /// <summary> Draws an Arc, calls new method with Background as the bgColor. </summary>
//     /// <param name="pos">Center of Arc.</param>
//     /// <param name="radius">Radius of Arc.</param>
//     /// <param name="color">Specified color index.</param>
//     /// <param name="arc">angle in degrees, 360 if not specified.</param>
//     /// <param name="c">Character to use.</param>
//     public void Arc(Point pos, int radius, int color, int arc = 360, ConsoleCharacter c = ConsoleCharacter.Full) {
//         Arc(pos, radius, color, Background, arc, c);
//     }

//     /// <summary> Draws an Arc. </summary>
//     /// <param name="pos">Center of Arc.</param>
//     /// <param name="radius">Radius of Arc.</param>
//     /// <param name="fgColor">Specified color index.</param>
//     /// <param name="bgColor">Specified background color index.</param>
//     /// <param name="arc">angle in degrees, 360 if not specified.</param>
//     /// <param name="c">Character to use.</param>
//     public void Arc(Point pos, int radius, int fgColor, int bgColor, int arc = 360, ConsoleCharacter c = ConsoleCharacter.Full)
//     {
//         for(int a = 0; a < arc; a++) {
//             int x = (int)(radius * Math.Cos((float)a / 57.29577f));
//             int y = (int)(radius * Math.Sin((float)a / 57.29577f));

//             Point v = new Point(pos.X + x, pos.Y + y);
//             SetPixel(v, fgColor,bgColor, ConsoleCharacter.Full);
//         }
//     }

//     /// <summary> Draws a filled Arc, calls new method with Background as the bgColor </summary>
//     /// <param name="pos">Center of Arc.</param>
//     /// <param name="radius">Radius of Arc.</param>
//     /// <param name="start">Start angle in degrees.</param>
//     /// <param name="arc">End angle in degrees.</param>
//     /// <param name="color">Specified color index.</param>
//     /// <param name="c">Character to use.</param>
//     public void SemiCircle(Point pos, int radius, int start, int arc, int color, ConsoleCharacter c = ConsoleCharacter.Full) {
//         SemiCircle(pos, radius, start, arc, color, Background, c);
//     }

//     /// <summary> Draws a filled Arc. </summary>
//     /// <param name="pos">Center of Arc.</param>
//     /// <param name="radius">Radius of Arc.</param>
//     /// <param name="start">Start angle in degrees.</param>
//     /// <param name="arc">End angle in degrees.</param>
//     /// <param name="fgColor">Specified color index.</param>
//     /// <param name="bgColor">Specified background color index.</param>
//     /// <param name="c">Character to use.</param>
//     public void SemiCircle(Point pos, int radius, int start, int arc, int fgColor,int bgColor, ConsoleCharacter c = ConsoleCharacter.Full)
//     {
//         for(int a = start; a > -arc + start; a--) {
//             for(int r = 0; r < radius + 1; r++) {
//                 int x = (int)(r * Math.Cos((float)a / 57.29577f));
//                 int y = (int)(r * Math.Sin((float)a / 57.29577f));

//                 Point v = new Point(pos.X + x, pos.Y + y);
//                 SetPixel(v, fgColor,bgColor, c);
//             }
//         }
//     }

//     // Bresenhams Line Algorithm
//     // https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
//     /// <summary> Draws a line from start to end. (Bresenhams Line), calls overloaded method with background as bgColor </summary>
//     /// <param name="start">Point to draw line from.</param>
//     /// <param name="end">Point to end line at.</param>
//     /// <param name="color">Color to draw with.</param>
//     /// <param name="c">Character to use.</param>
//     public void Line(Point start, Point end, int color, ConsoleCharacter c = ConsoleCharacter.Full) {
//         Line(start, end, color, Background, c);
//     }

//     // Bresenhams Line Algorithm
//     // https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
//     /// <summary> Draws a line from start to end. (Bresenhams Line) </summary>
//     /// <param name="start">Point to draw line from.</param>
//     /// <param name="end">Point to end line at.</param>
//     /// <param name="fgColor">Color to draw with.</param>
//     /// <param name="bgColor">Color to draw the background with.</param>
//     /// <param name="c">Character to use.</param>
//     public void Line(Point start, Point end, int fgColor,int bgColor, ConsoleCharacter c = ConsoleCharacter.Full)
//     {
//         Point delta = end - start;
//         Point da = Point.Zero, db = Point.Zero;
//         if(delta.X < 0) da.X = -1; else if(delta.X > 0) da.X = 1;
//         if(delta.Y < 0) da.Y = -1; else if(delta.Y > 0) da.Y = 1;
//         if(delta.X < 0) db.X = -1; else if(delta.X > 0) db.X = 1;
//         int longest = Math.Abs(delta.X);
//         int shortest = Math.Abs(delta.Y);

//         if(!(longest > shortest)) {
//             longest = Math.Abs(delta.Y);
//             shortest = Math.Abs(delta.X);
//             if(delta.Y < 0) db.Y = -1; else if(delta.Y > 0) db.Y = 1;
//             db.X = 0;
//         }

//         int numerator = longest >> 1;
//         Point p = new Point(start.X, start.Y);
//         for(int i = 0; i <= longest; i++) {
//             SetPixel(p, fgColor,bgColor, c);
//             numerator += shortest;
//             if(!(numerator < longest)) {
//                 numerator -= longest;
//                 p += da;
//             }
//             else {
//                 p += db;
//             }
//         }
//     }

//     /// <summary> Draws a Rectangle, calls overloaded method with background as bgColor  </summary>
//     /// <param name="pos">Top Left corner of rectangle.</param>
//     /// <param name="end">Bottom Right corner of rectangle.</param>
//     /// <param name="color">Color to draw with.</param>
//     /// <param name="c">Character to use.</param>
//     public void Rectangle(Point pos, Point end, int color, ConsoleCharacter c = ConsoleCharacter.Full) {
//         Rectangle(pos, end, color, Background, c);
//     }

//     /// <summary> Draws a Rectangle. </summary>
//     /// <param name="pos">Top Left corner of rectangle.</param>
//     /// <param name="end">Bottom Right corner of rectangle.</param>
//     /// <param name="fgColor">Color to draw with.</param>
//     /// <param name="bgColor">Color to draw to the background with.</param>
//     /// <param name="c">Character to use.</param>
//     public void Rectangle(Point pos, Point end, int fgColor,int bgColor, ConsoleCharacter c = ConsoleCharacter.Full)
//     {
//         for(int i = 0; i < end.X - pos.X; i++) {
//             SetPixel(new Point(pos.X + i, pos.Y), fgColor,bgColor, c);
//             SetPixel(new Point(pos.X + i, end.Y), fgColor, bgColor, c);
//         }

//         for(int i = 0; i < end.Y - pos.Y + 1; i++) {
//             SetPixel(new Point(pos.X, pos.Y + i), fgColor, bgColor, c);
//             SetPixel(new Point(end.X, pos.Y + i), fgColor, bgColor, c);
//         }
//     }

//     /// <summary> Draws a Rectangle and fills it, calls overloaded method with background as bgColor </summary>
//     /// <param name="a">Top Left corner of rectangle.</param>
//     /// <param name="b">Bottom Right corner of rectangle.</param>
//     /// <param name="color">Color to draw with.</param>
//     /// <param name="c">Character to use.</param>
//     public void Fill(Point a, Point b, int color, ConsoleCharacter c = ConsoleCharacter.Full) {
//         Fill(a, b, color, Background, c);
//     }

//     /// <summary> Draws a Rectangle and fills it. </summary>
//     /// <param name="a">Top Left corner of rectangle.</param>
//     /// <param name="b">Bottom Right corner of rectangle.</param>
//     /// <param name="fgColor">Color to draw with.</param>
//     /// <param name="bgColor">Color to draw the background with.</param>
//     /// <param name="c">Character to use.</param>
//     public void Fill(Point a, Point b, int fgColor,int bgColor, ConsoleCharacter c = ConsoleCharacter.Full)
//     {
//         for(int y = a.Y; y < b.Y; y++) {
//             for(int x = a.X; x < b.X; x++) {
//                 SetPixel(new Point(x, y), fgColor,bgColor, c);
//             }
//         }
//     }

//     /// <summary> Draws a grid, calls overloaded method with background as bgColor </summary>
//     /// <param name="a">Top Left corner of grid.</param>
//     /// <param name="b">Bottom Right corner of grid.</param>
//     /// <param name="spacing">the spacing until next line</param>
//     /// <param name="color">Color to draw with.</param>
//     /// <param name="c">Character to use.</param>
//     public void Grid(Point a, Point b, int spacing, int color, ConsoleCharacter c = ConsoleCharacter.Full) {
//         Grid(a, b, spacing, color, Background, c);
//     }

//     /// <summary> Draws a grid. </summary>
//     /// <param name="a">Top Left corner of grid.</param>
//     /// <param name="b">Bottom Right corner of grid.</param>
//     /// <param name="spacing">the spacing until next line</param>
//     /// <param name="fgColor">Color to draw with.</param>
//     /// <param name="bgColor">Color to draw the background with.</param>
//     /// <param name="c">Character to use.</param>
//     public void Grid(Point a, Point b, int spacing, int fgColor,int bgColor, ConsoleCharacter c = ConsoleCharacter.Full)
//     {
//         for(int y = a.Y; y < b.Y / spacing; y++) {
//             Line(new Point(a.X, y * spacing), new Point(b.X, y * spacing), fgColor,bgColor, c);
//         }
//         for(int x = a.X; x < b.X / spacing; x++) {
//             Line(new Point(x * spacing, a.Y), new Point(x * spacing, b.Y), fgColor, bgColor, c);
//         }
//     }

//     /// <summary> Draws a Triangle, calls overloaded method with background as bgColor </summary>
//     /// <param name="a">Point A.</param>
//     /// <param name="b">Point B.</param>
//     /// <param name="c">Point C.</param>
//     /// <param name="color">Color to draw with.</param>
//     /// <param name="character">Character to use.</param>
//     public void Triangle(Point a, Point b, Point c, int color, ConsoleCharacter character = ConsoleCharacter.Full) {
//         Triangle(a,b,c,color,Background,character);
//     }

//     /// <summary> Draws a Triangle. </summary>
//     /// <param name="a">Point A.</param>
//     /// <param name="b">Point B.</param>
//     /// <param name="c">Point C.</param>
//     /// <param name="fgColor">Color to draw with.</param>
//     /// <param name="bgColor">Color to draw to the background with.</param>
//     /// <param name="character">Character to use.</param>
//     public void Triangle(Point a, Point b, Point c, int fgColor,int bgColor, ConsoleCharacter character = ConsoleCharacter.Full)
//     {
//         Line(a, b, fgColor,bgColor, character);
//         Line(b, c, fgColor, bgColor, character);
//         Line(c, a, fgColor, bgColor, character);
//     }

//     // Bresenhams Triangle Algorithm

//     /// <summary> Draws a Triangle and fills it, calls overloaded method with background as bgColor </summary>
//     /// <param name="a">Point A.</param>
//     /// <param name="b">Point B.</param>
//     /// <param name="c">Point C.</param>
//     /// <param name="color">Color to draw with.</param>
//     /// <param name="character">Character to use.</param>
//     public void FillTriangle(Point a, Point b, Point c, int color, ConsoleCharacter character = ConsoleCharacter.Full) {
//         FillTriangle(a, b, c, color, Background, character);
//     }

//     /// <summary> Draws a Triangle and fills it. </summary>
//     /// <param name="a">Point A.</param>
//     /// <param name="b">Point B.</param>
//     /// <param name="c">Point C.</param>
//     /// <param name="fgColor">Color to draw with.</param>
//     /// <param name="bgColor">Color to draw to the background with.</param>
//     /// <param name="character">Character to use.</param>
//     public void FillTriangle(Point a, Point b, Point c, int fgColor, int bgColor, ConsoleCharacter character = ConsoleCharacter.Full)
//     {
//         Point min = new Point(Math.Min(Math.Min(a.X, b.X), c.X), Math.Min(Math.Min(a.Y, b.Y), c.Y));
//         Point max = new Point(Math.Max(Math.Max(a.X, b.X), c.X), Math.Max(Math.Max(a.Y, b.Y), c.Y));

//         Point p = new Point();
//         for(p.Y = min.Y; p.Y < max.Y; p.Y++) {
//             for(p.X = min.X; p.X < max.X; p.X++) {
//                 int w0 = Orient(b, c, p);
//                 int w1 = Orient(c, a, p);
//                 int w2 = Orient(a, b, p);

//                 if(w0 >= 0 && w1 >= 0 && w2 >= 0) SetPixel(p, fgColor,bgColor, character);
//             }
//         }
//     }

//     private int Orient(Point a, Point b, Point c) {
//         return ((b.X - a.X) * (c.Y - a.Y)) - ((b.Y - a.Y) * (c.X - a.X));
//     }

//     #endregion Primitives

//     // Input

//     /// <summary>Checks to see if the console is in focus </summary>
//     /// <returns>True if Console is in focus</returns>
//     private bool ConsoleFocused()
//     {
//         return NativeMethods.GetConsoleWindow() == NativeMethods.GetForegroundWindow();
//     }

//     /// <summary> Checks if specified key is pressed. </summary>
//     /// <param name="key">The key to check.</param>
//     /// <returns>True if key is pressed</returns>
//     public bool GetKey(ConsoleKey key) {
//         short s = NativeMethods.GetAsyncKeyState((int)key);
//         return (s & 0x8000) > 0 && ConsoleFocused();
//     }

//     /// <summary> Checks if specified keyCode is pressed. </summary>
//     /// <param name="virtualkeyCode">keycode to check</param>
//     /// <returns>True if key is pressed</returns>
//     public bool GetKey(int virtualkeyCode)
//     {
//         short s = NativeMethods.GetAsyncKeyState(virtualkeyCode);
//         return (s & 0x8000) > 0 && ConsoleFocused();
//     }

//     /// <summary> Checks if specified key is pressed down. </summary>
//     /// <param name="key">The key to check.</param>
//     /// <returns>True if key is down</returns>
//     public bool GetKeyDown(ConsoleKey key) {
//         int s = Convert.ToInt32(NativeMethods.GetAsyncKeyState((int)key));
//         return (s == -32767) && ConsoleFocused();
//     }

//     /// <summary> Checks if specified keyCode is pressed down. </summary>
//     /// <param name="virtualkeyCode">keycode to check</param>
//     /// <returns>True if key is down</returns>
//     public bool GetKeyDown(int virtualkeyCode)
//     {
//         int s = Convert.ToInt32(NativeMethods.GetAsyncKeyState(virtualkeyCode));
//         return (s == -32767) && ConsoleFocused();
//     }

//     /// <summary> Checks if left mouse button is pressed down. </summary>
//     /// <returns>True if left mouse button is down</returns>
//     public bool GetMouseLeft() {
//         short s = NativeMethods.GetAsyncKeyState(0x01);
//         return (s & 0x8000) > 0 && ConsoleFocused();
//     }

//     /// <summary> Checks if right mouse button is pressed down. </summary>
//     /// <returns>True if right mouse button is down</returns>
//     public bool GetMouseRight()
//     {
//         short s = NativeMethods.GetAsyncKeyState(0x02);
//         return (s & 0x8000) > 0 && ConsoleFocused();
//     }

//     /// <summary> Checks if middle mouse button is pressed down. </summary>
//     /// <returns>True if middle mouse button is down</returns>
//     public bool GetMouseMiddle()
//     {
//         short s = NativeMethods.GetAsyncKeyState(0x04);
//         return (s & 0x8000) > 0 && ConsoleFocused();
//     }

//     /// <summary> Gets the mouse position. </summary>
//     /// <returns>The mouse's position in character-space.</returns>
//     /// <exception cref="Exception"/>
//     public Point GetMousePos() {
//         NativeMethods.Rect r = new NativeMethods.Rect();
//         NativeMethods.GetWindowRect(consoleHandle, ref r);

//         if (NativeMethods.GetCursorPos(out NativeMethods.POINT p)) {
//             Point point = new Point();
//             if (!IsBorderless) {
//                 p.Y -= 29;
//                 point = new Point(
//                     (int)Math.Floor(((p.X - r.Left) / (float)FontSize.X) - 0.5f),
//                     (int)Math.Floor(((p.Y - r.Top) / (float)FontSize.Y))
//                 );
//             } else {
//                 point = new Point(
//                     (int)Math.Floor(((p.X - r.Left) / (float)FontSize.X)),
//                     (int)Math.Floor(((p.Y - r.Top) / (float)FontSize.Y))
//                 );
//             }
//             return new Point(Utility.Clamp(point.X, 0, WindowSize.X - 1), Utility.Clamp(point.Y, 0, WindowSize.Y - 1));
//         }
//         throw new Exception();
//     }
// }

// /// <summary> Represents an RGB color. </summary>
// public class Color {
// 	/// <summary> Red component. </summary>
// 	public uint R { get; set; }
// 	/// <summary> Green component. </summary>
// 	public uint G { get; set; }
// 	/// <summary> Bkue component. </summary>
// 	public uint B { get; set; }

// 	/// <summary> Creates a new Color from rgb. </summary>
// 	public Color(int r, int g, int b) {
// 		this.R = (uint)r;
// 		this.G = (uint)g;
// 		this.B = (uint)b;
// 	}
// }

// class ConsoleBuffer {
// 	private NativeMethods.CharInfo[] CharInfoBuffer { get; set; } = null!;
// 	SafeFileHandle h;

// 	readonly int width, height;

// 	public ConsoleBuffer(int w, int he) {
// 		width = w;
// 		height = he;

// 		h = NativeMethods.CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

// 		if (!h.IsInvalid) {
// 			CharInfoBuffer = new NativeMethods.CharInfo[width * height];
// 		}
// 	}
// 	/// <summary>
// 	/// Sets the buffer to values
// 	/// </summary>
// 	/// <param name="GlyphBuffer"></param>
// 	/// <param name="charBuffer"> array of chars which get added to the buffer</param>
// 	/// <param name="colorBuffer"> array of foreground(front)colors which get added to the buffer</param>
// 	/// <param name="background"> array of background colors which get added to the buffer</param>
// 	/// <param name="defualtBackground"> default color(may reduce fps?), this is the background color
// 	///									null chars will get set to this default background</param>
// 	public void SetBuffer(Glyph[,] GlyphBuffer, int defualtBackground) {
// 		for (int y = 0; y < height; y++) {
// 			for (int x = 0; x < width; x++) {
// 				int i = (y * width) + x;

// 				if(GlyphBuffer[x, y].c == 0)
// 					GlyphBuffer[x, y].bg = defualtBackground;

// 				CharInfoBuffer[i].Attributes = (short)(GlyphBuffer[x, y].fg |(GlyphBuffer[x,y].bg << 4) );
// 				CharInfoBuffer[i].UnicodeChar = GlyphBuffer[x, y].c;
// 			}
// 		}
// 	}

// 	public bool Blit() {
// 		NativeMethods.SmallRect rect = new NativeMethods.SmallRect() { Left = 0, Top = 0, Right = (short)width, Bottom = (short)height };

// 		return NativeMethods.WriteConsoleOutputW(h, CharInfoBuffer,
// 			new NativeMethods.Coord() { X = (short)width, Y = (short)height },
// 			new NativeMethods.Coord() { X = 0, Y = 0 }, ref rect);
// 	}
// }

// /// <summary> Enum for basic Unicodes. </summary>
// public enum ConsoleCharacter {
// 	Null = 0x0000,

// 	Full = 0x2588,
// 	Dark = 0x2593,
// 	Medium = 0x2592,
// 	Light = 0x2591,

// 	// box drawing syboler
// 	// ┌───────┐
// 	// │       │
// 	// │       │
// 	// └───────┘
// 	BoxDrawingL_H = 0x2500,
// 	BoxDrawingL_V = 0x2502,
// 	BoxDrawingL_DR = 0x250C,
// 	BoxDrawingL_DL = 0x2510,
// 	BoxDrawingL_UL = 0x2518,
// 	BoxDrawingL_UR = 0x2514,
// }

// /// <summary> Enum for Different Gameloop modes. </summary>
// public enum FramerateMode {
// 	/// <summary>Run at max speed, but no higher than TargetFramerate.</summary>
// 	MaxFps,
// 	/// <summary>Run at max speed.</summary>
// 	Unlimited
// }

// /// <summary> Represents prebuilt palettes. </summary>
// public static class Palettes {
// 	/// <summary> Pico8 palette. </summary>
// 	public static Color[] Pico8 { get; set; } = new Color[16] {
// 		new Color(0,    0,     0),
// 		new Color(29,   43,    83),
// 		new Color(126,  37,    83),
// 		new Color(0,    135,   81),
// 		new Color(171,  82,    54),
// 		new Color(95,   87,    79),
// 		new Color(194,  195,   199),
// 		new Color(255,  241,   232),
// 		new Color(255,  0,     77),
// 		new Color(255,  163,   0),
// 		new Color(255,  236,   39),
// 		new Color(0,    228,   54),
// 		new Color(41,   173,   255),
// 		new Color(131,  118,   156),
// 		new Color(255,  119,   168),
// 		new Color(255,  204,   170),
// 	};

// 	/// <summary> default windows console palette. </summary>
// 	public static Color[] Default { get; set; } = new Color[16] {
// 		new Color(12,   12,     12),			// Black
// 		new Color(0,    55,     218),			// DarkBlue
// 		new Color(19,   161,    14),			// DarkGreen
// 		new Color(58,   150,    221),			// DarkCyan
// 		new Color(197,  15,     31),			// DarkRed
// 		new Color(136,  23,     152),			// DarkMagenta
// 		new Color(193,  156,    0),				// DarkYellow
// 		new Color(204,  204,    204),			// Gray
// 		new Color(118,  118,    118),			// DarkGray
// 		new Color(59,   120,    255),			// Blue
// 		new Color(22,   192,    12),			// Green
// 		new Color(97,   214,    214),			// Cyan
// 		new Color(231,  72,     86),			// Red
// 		new Color(180,  0,      158),			// Magenta
// 		new Color(249,  241,    165),			// Yellow
// 		new Color(242,  242,    242),			// White
// 	};


// 	/// <summary> Color constants for ease of use ex: Palettes.BlUE</summary>
// 	public static readonly int NULL = -1;
// 	public static readonly int BLACK = 0;
// 	public static readonly int DARK_BLUE = 1;
// 	public static readonly int DARK_GREEN = 2;
// 	public static readonly int DARK_CYAN = 3;
// 	public static readonly int DARK_RED = 4;
// 	public static readonly int DARK_MAGENTA = 5;
// 	public static readonly int DARK_YELLOW = 6;
// 	public static readonly int GRAY = 7;
// 	public static readonly int DARK_GRAY = 8;
// 	public static readonly int BLUE = 9;
// 	public static readonly int GREEN = 10;
// 	public static readonly int CYAN = 11;
// 	public static readonly int RED = 12;
// 	public static readonly int MAGENTA = 13;
// 	public static readonly int YELLOW = 14;
// 	public static readonly int WHITE = 15;

// 	private static String[] COLOR_NAME = new String[]
// 	{
// 		"Black","Dark Blue","Dark Green","Dark Cyan","Dark Red","Dark Magenta","Dark Yellow","Gray","Dark Gray","Blue","Green","Cyan","Red","Magenta","Yellow","White"
// 	};

// 	/// <summary>toString function, which returns the string name of the color</summary>
// 	/// <param name="colorPosition">position in array</param>
// 	/// <returns>the name of the color in the palette array</returns>
// 	public static String ColorName(int colorPosition)
// 	{
// 		if (colorPosition < 0 || colorPosition > COLOR_NAME.Length)
// 			return "null";
// 		return COLOR_NAME[colorPosition];
// 	}


// }


// class ConsoleFont {



//     internal static int SetFont(IntPtr h, short sizeX, short sizeY) {
//         if (h == new IntPtr(-1)) {
//             return Marshal.GetLastWin32Error();
//         }

//         NativeMethods.CONSOLE_FONT_INFO_EX cfi = new NativeMethods.CONSOLE_FONT_INFO_EX();
//         cfi.cbSize = (uint)Marshal.SizeOf(cfi);
//         cfi.nFont = 0;

//         cfi.dwFontSize.X = sizeX;
//         cfi.dwFontSize.Y = sizeY;

//         // sätter font till Terminal (Raster)
//         if (sizeX < 4 || sizeY < 4) cfi.FaceName = "Consolas";
//         else cfi.FaceName = "Terminal";

//         NativeMethods.SetCurrentConsoleFontEx(h, false, ref cfi);
//         return 0;
//     }
// }

// /// Abstract class to aid in Gamemaking.
// /// Implements an instance of the ConsoleEngine and has Looping methods.
// /// </summary>
// public abstract class ConsoleGame {
// 	/// <summary> Instance of a ConsoleEngine. </summary>
// 	public ConsoleEngine Engine { get; private set; }= null!;

// 	/// <summary> A counter representing the current unique frame we're at. </summary>
// 	public int FrameCounter { get; set; }

// 	/// <summary> A counter representing the total frames since launch</summary>
// 	public int FrameTotal { get; private set; }
// 	/// <summary> Factor for generating framerate-independent physics. time between last frame and current. </summary>
// 	public float DeltaTime { get; set; }

// 	/// <summary>The time the program started in DateTime, set after Create()</summary>
// 	public DateTime StartTime { get; private set; }

// 	/// <summary> The framerate the engine is trying to run at. </summary>
// 	public int TargetFramerate { get; set; }

// 	private bool Running { get; set; }
// 	private Thread gameThread= null!;

// 	private double[] framerateSamples=null!;

// 	/// <summary> Initializes the ConsoleGame. Creates the instance of a ConsoleEngine and starts the game loop. </summary>
// 	/// <param name="width">Width of the window.</param>
// 	/// <param name="height">Height of the window.</param>
// 	/// <param name="fontW">Width of the font.</param>
// 	/// <param name="fontH">´Height of the font.</param>
// 	/// <param name="m">Framerate mode to run at.</param>
// 	/// <see cref="FramerateMode"/> <see cref="ConsoleEngine"/>
// 	public void Construct(int width, int height, int fontW, int fontH, FramerateMode m) {
// 		TargetFramerate = 30;

// 		Engine = new ConsoleEngine(width, height, fontW, fontH);
// 		Create();
// 		StartTime = DateTime.Now;

// 		if (m == FramerateMode.Unlimited) gameThread = new Thread(new ThreadStart(GameLoopUnlimited));
// 		if (m == FramerateMode.MaxFps) gameThread = new Thread(new ThreadStart(GameLoopLocked));
// 		Running = true;
// 		gameThread.Start();

// 		// gör special checks som ska gå utanför spelloopen
// 		// om spel-loopen hänger sig ska man fortfarande kunna avsluta
// 		while (Running) {
// 			CheckForExit();
// 		}
// 	}

// 	private void GameLoopLocked() {
// 		int sampleCount = TargetFramerate;
// 		framerateSamples = new double[sampleCount];

// 		DateTime lastTime;
// 		float uncorrectedSleepDuration = 1000 / TargetFramerate;
// 		while (Running) {
// 			lastTime = DateTime.UtcNow;

// 			FrameCounter++;
// 			FrameCounter = FrameCounter % sampleCount;

// 			// kör main programmet
// 			Update();
// 			Render();

// 			float computingDuration = (float)(DateTime.UtcNow - lastTime).TotalMilliseconds;
// 			int sleepDuration = (int)(uncorrectedSleepDuration - computingDuration);
// 			if (sleepDuration > 0) {
// 				// programmet ligger före maxFps, sänker det
// 				Thread.Sleep(sleepDuration);
// 			}

// 			//increases total frames
// 			FrameTotal++;

// 			// beräknar framerate
// 			TimeSpan diff = DateTime.UtcNow - lastTime;
// 			DeltaTime = (float)(1 / (TargetFramerate * diff.TotalSeconds));

// 			framerateSamples[FrameCounter] = (double)diff.TotalSeconds;
// 		}
// 	}

// 	private void GameLoopUnlimited() {
// 		int sampleCount = TargetFramerate;
// 		framerateSamples = new double[sampleCount];

// 		DateTime lastTime;
// 		while (Running) {
// 			lastTime = DateTime.UtcNow;

// 			FrameCounter++;
// 			FrameCounter = FrameCounter % sampleCount;

// 			Update();
// 			Render();

// 			//increases total frames
// 			FrameTotal++;

// 			// beräknar framerate
// 			TimeSpan diff = DateTime.UtcNow - lastTime;
// 			DeltaTime = (float)diff.TotalSeconds;

// 			framerateSamples[FrameCounter] = diff.TotalSeconds;

// 			// kollar om spelaren vill sluta
// 			CheckForExit();
// 		}
// 	}

// 	/// <summary> Gets the current framerate the application is running at. </summary>
// 	/// <returns> Application Framerate. </returns>
// 	public double GetFramerate() {
// 		return 1 / (framerateSamples.Sum() / (TargetFramerate));
// 	}

// 	private void CheckForExit() {
// 		if (Engine.GetKeyDown(ConsoleKey.Delete)) {
// 			Running = false;
// 		}
// 	}

// 	/// <summary> Run once on Creating, import Resources here. </summary>
// 	public abstract void Create();
// 	/// <summary> Run every frame before rendering. Do math here. </summary>
// 	public abstract void Update();
// 	/// <summary> Run every frame after updating. Do drawing here. </summary>
// 	public abstract void Render();
// }

// // En helper klass för att wrappa User32 och Kernel32 dll
// class NativeMethods {
//     #region Signatures

//     [DllImport("user32.dll", SetLastError = true)]
//     public static extern short GetAsyncKeyState(Int32 vKey);
//     [DllImport("user32.dll", SetLastError = true)]
//     public static extern bool GetCursorPos(out POINT vKey);
//     [DllImport("user32.dll", SetLastError = true)]
//     public static extern IntPtr GetForegroundWindow();


//     [DllImport("user32.dll", SetLastError = true)]
//     public static extern bool GetWindowRect(IntPtr hWnd, ref Rect lpRect);
//     [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
//     public static extern IntPtr GetDesktopWindow();

//     [DllImport("user32.dll", SetLastError = true)]
//     public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
//     [DllImport("user32.dll", SetLastError = true)]
//     public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
//     [DllImport("user32.dll", SetLastError = true)]
//     public static extern bool DrawMenuBar(IntPtr hWnd);
//     [DllImport("user32.dll", SetLastError = true)]
//     public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In, Out] ref Rect rect, [MarshalAs(UnmanagedType.U4)] int cPoints);



//     [DllImport("kernel32.dll", SetLastError = true)]
//     public static extern IntPtr GetStdHandle(int nStdHandle);
//     [DllImport("kernel32.dll", SetLastError = true)]
//     public static extern IntPtr GetConsoleWindow();


//     [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
//     public static extern SafeFileHandle CreateFile(
//         string fileName,
//         [MarshalAs(UnmanagedType.U4)] uint fileAccess,
//         [MarshalAs(UnmanagedType.U4)] uint fileShare,
//         IntPtr securityAttributes,
//         [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
//         [MarshalAs(UnmanagedType.U4)] int flags,
//     IntPtr template);

//     [DllImport("kernel32.dll", SetLastError = true)]
//     public static extern bool WriteConsoleOutputW(
//         SafeFileHandle hConsoleOutput,
//         CharInfo[] lpBuffer,
//         Coord dwBufferSize,
//         Coord dwBufferCoord,
//     ref SmallRect lpWriteRegion);

//     [DllImport("kernel32.dll", SetLastError = true)]
//     public static extern bool GetConsoleScreenBufferInfoEx( IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO_EX csbe );

//     [DllImport("kernel32.dll", SetLastError = true)]
//     public static extern bool SetConsoleScreenBufferInfoEx( IntPtr ConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO_EX csbe );

//     [DllImport("kernel32.dll", SetLastError = true)]
//     public static extern Int32 SetCurrentConsoleFontEx(
//     IntPtr ConsoleOutput,
//     bool MaximumWindow,
//     ref CONSOLE_FONT_INFO_EX ConsoleCurrentFontEx);

//     [DllImport("kernel32.dll", SetLastError = true)]
//     public static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

//     #endregion

//     #region Structs

//     // Basic
//     [StructLayout(LayoutKind.Sequential)]
//     public struct POINT {
//         public int X;
//         public int Y;
//     }

//     [StructLayout(LayoutKind.Sequential)]
//     public struct Coord {
//         public short X;
//         public short Y;

//         public Coord(short X, short Y) {
//             this.X = X;
//             this.Y = Y;
//         }
//     };
//     [StructLayout(LayoutKind.Sequential)]
//     public struct Rect {
//         public int Left;
//         public int Top;
//         public int Right;
//         public int Bottom;
//     }
//     [StructLayout(LayoutKind.Sequential)]
//     public struct SmallRect {
//         public short Left;
//         public short Top;
//         public short Right;
//         public short Bottom;
//     }

//     // Tecken, används av buffern
//     [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
//     public struct CharInfo {
//         [FieldOffset(0)] public char UnicodeChar;
//         [FieldOffset(0)] public byte AsciiChar;
//         [FieldOffset(2)] public short Attributes;
//     }

//     // Används for att ändra ColorRef, custom palette :)
//     [StructLayout(LayoutKind.Sequential)]
//     public struct ColorRef {
//         internal uint ColorDWORD;

//         internal ColorRef(Color color) {
//             ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
//         }

//         internal ColorRef(uint r, uint g, uint b) {
//             ColorDWORD = r + (g << 8) + (b << 16);
//         }

//         internal Color GetColor() {
//             return new Color((int)(0x000000FFU & ColorDWORD),
//                 (int)(0x0000FF00U & ColorDWORD) >> 8, (int)(0x00FF0000U & ColorDWORD) >> 16);
//         }

//         internal void SetColor(Color color) {
//             ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
//         }
//     }

//     [StructLayout(LayoutKind.Sequential)]
//     public struct CONSOLE_SCREEN_BUFFER_INFO_EX {
//         public int cbSize;
//         public Coord dwSize;
//         public Coord dwCursorPosition;
//         public short wAttributes;
//         public SmallRect srWindow;
//         public Coord dwMaximumWindowSize;

//         public ushort wPopupAttributes;
//         public bool bFullscreenSupported;

//         internal ColorRef black;
//         internal ColorRef darkBlue;
//         internal ColorRef darkGreen;
//         internal ColorRef darkCyan;
//         internal ColorRef darkRed;
//         internal ColorRef darkMagenta;
//         internal ColorRef darkYellow;
//         internal ColorRef gray;
//         internal ColorRef darkGray;
//         internal ColorRef blue;
//         internal ColorRef green;
//         internal ColorRef cyan;
//         internal ColorRef red;
//         internal ColorRef magenta;
//         internal ColorRef yellow;
//         internal ColorRef white;
//     }

//     [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
//     public struct CONSOLE_FONT_INFO_EX {
//         public uint cbSize;
//         public uint nFont;
//         public Coord dwFontSize;
//         public int FontFamily;
//         public int FontWeight;

//         [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] // Edit sizeconst if the font name is too big
//         public string FaceName;
//     }

//     #endregion
// }

// class ConsolePalette {

//     public static int SetColor(int consoleColor, Color targetColor) {
//         return SetColor(consoleColor, targetColor.R, targetColor.G, targetColor.B);
//     }

//     private static int SetColor(int color, uint r, uint g, uint b) {
//         NativeMethods.CONSOLE_SCREEN_BUFFER_INFO_EX csbe = new NativeMethods.CONSOLE_SCREEN_BUFFER_INFO_EX();
//         csbe.cbSize = Marshal.SizeOf(csbe);
//         IntPtr hConsoleOutput = NativeMethods.GetStdHandle(-11);
//         if (hConsoleOutput == new IntPtr(-1)) {
//             return Marshal.GetLastWin32Error();
//         }
//         bool brc = NativeMethods.GetConsoleScreenBufferInfoEx(hConsoleOutput, ref csbe);
//         if (!brc) {
//             return Marshal.GetLastWin32Error();
//         }

//         switch (color) {
//             case 0:
//                 csbe.black = new NativeMethods.ColorRef(r, g, b);
//                 break;
//             case 1:
//                 csbe.darkBlue = new NativeMethods.ColorRef(r, g, b);
//                 break;
//             case 2:
//                 csbe.darkGreen = new NativeMethods.ColorRef(r, g, b);
//                 break;
//             case 3:
//                 csbe.darkCyan = new NativeMethods.ColorRef(r, g, b);
//                 break;
//             case 4:
//                 csbe.darkRed = new NativeMethods.ColorRef(r, g, b);
//                 break;
//             case 5:
//                 csbe.darkMagenta = new NativeMethods.ColorRef(r, g, b);
//                 break;
//             case 6:
//                 csbe.darkYellow = new NativeMethods.ColorRef(r, g, b);
//                 break;
//             case 7:
//                 csbe.gray = new NativeMethods.ColorRef(r, g, b);
//                 break;
//             case 8:
//                 csbe.darkGray = new NativeMethods.ColorRef(r, g, b);
//                 break;
//             case 9:
//                 csbe.blue = new NativeMethods.ColorRef(r, g, b);
//                 break;
//             case 10:
//                 csbe.green = new NativeMethods.ColorRef(r, g, b);
//                 break;
//             case 11:
//                 csbe.cyan = new NativeMethods.ColorRef(r, g, b);
//                 break;
//             case 12:
//                 csbe.red = new NativeMethods.ColorRef(r, g, b);
//                 break;
//             case 13:
//                 csbe.magenta = new NativeMethods.ColorRef(r, g, b);
//                 break;
//             case 14:
//                 csbe.yellow = new NativeMethods.ColorRef(r, g, b);
//                 break;
//             case 15:
//                 csbe.white = new NativeMethods.ColorRef(r, g, b);
//                 break;
//         }

//         ++csbe.srWindow.Bottom;
//         ++csbe.srWindow.Right;

//         brc = NativeMethods.SetConsoleScreenBufferInfoEx(hConsoleOutput, ref csbe);
//         if (!brc) {
//             return Marshal.GetLastWin32Error();
//         }
//         return 0;
//     }
// }

// /// <summary> A FIGlet font. </summary>
// public class FigletFont {

//     public int CommentLines { get; private set; }
//     public string HardBlank { get; private set; }=null!;
//     public int Height { get; private set; }
//     public int Kerning { get; private set; }
//     public string[] Lines { get; private set; }=null!;
//     public int MaxLength { get; private set; }
//     public string Signature { get; private set; }=null!;

//     public static FigletFont Load(string filePath) {
//         if (filePath == null) throw new ArgumentNullException(nameof(filePath));
//         IEnumerable<string> fontLines = File.ReadLines(filePath);

//         FigletFont font = new FigletFont() {
//             Lines = fontLines.ToArray()
//         };
//         var cs = font.Lines.First();
//         var configs = cs.Split(' ');
//         font.Signature = configs.First().Remove(configs.First().Length - 1);

//         if(font.Signature == "flf2a") {
//             font.HardBlank = configs.First().Last().ToString();
//             font.Height = ParseInt(configs, 1);
//             font.MaxLength = ParseInt(configs, 3);
//             font.CommentLines = ParseInt(configs, 5);
//         }


//         return font;
//     }

//     private static int ParseInt(string[] values, int index) {
//         var i = 0;
//         if(values.Length > index) {
//             int.TryParse(values[index], out i);
//         }

//         return i;
//     }



//     // ----
//     internal static int GetStringWidth(FigletFont font, string value) {
//         List<int> charWidths = new List<int>();
//         foreach (var character in value) {
//             int charWidth = 0;
//             for (int line = 1; line <= font.Height; line++) {
//                 string figletCharacter = GetCharacter(font, character, line);

//                 charWidth = figletCharacter.Length > charWidth ? figletCharacter.Length : charWidth;
//             }
//             charWidths.Add(charWidth);
//         }

//         return charWidths.Sum();
//     }

//     internal static string GetCharacter(FigletFont font, char character, int line) {
//         var start = font.CommentLines + ((Convert.ToInt32(character) - 32) * font.Height);
//         var result = font.Lines[start + line];
//         var lineEnding = result[result.Length - 1];
//         result = Regex.Replace(result, @"\" + lineEnding + "{1,2}$", string.Empty);

//         if (font.Kerning > 0) {
//             result += new string(' ', font.Kerning);
//         }

//         return result.Replace(font.HardBlank, " ");
//     }
// }

// public struct Glyph
// {
//     public char c;
//     public int fg;
//     public int bg;

//     public void set(char c_, int fg_, int bg_) { c = c_; fg = fg_;bg = bg_; }

//     public void clear() { c = (char)0;fg = 0;bg = 0; }
// }

// /// <summary> A Vector containing two ints. </summary>
// public struct Point {
//     public int X { get; set; }
//     public int Y { get; set; }

//     public const float Rad2Deg = 180f / (float)Math.PI;
//     public const float Deg2Rad = (float)Math.PI / 180f;

//     /// <summary> new Point(0, 0); </summary>
//     public static Point Zero { get; private set; } = new Point(0, 0);

//     public Point(int x, int y) {
//         this.X = x;
//         this.Y = y;
//     }

//     public Vector ToVector() => new Vector((float)X, (float)Y);
//     public override string ToString() => String.Format("({0}, {1})", X, Y);

//     public static Point operator +(Point a, Point b) {
//         return new Point(a.X + b.X, a.Y + b.Y);
//     }
//     public static Point operator -(Point a, Point b) {
//         return new Point(a.X - b.X, a.Y - b.Y);
//     }
//     public static Point operator +(Point a, int b)
//     {
//         return new Point(a.X + b, a.Y + b);
//     }
//     public static Point operator -(Point a, int b)
//     {
//         return new Point(a.X - b, a.Y - b);
//     }

//     public static Point operator /(Point a, float b) {
//         return new Point((int)(a.X / b), (int)(a.Y / b));
//     }
//     public static Point operator *(Point a, float b) {
//         return new Point((int)(a.X * b), (int)(a.Y * b));
//     }
//     public static bool operator ==(Point a, Point b)
//     {
//         return a.X == b.X && a.Y == b.Y;
//     }
//     public static bool operator !=(Point a, Point b)
//     {
//         return a.X != b.X || a.Y != b.Y;
//     }
//     public static bool operator <=(Point a, Point b)
//     {
//         return a.X <= b.X && a.Y <= b.Y;
//     }
//     public static bool operator >=(Point a, Point b)
//     {
//         return a.X >= b.X && a.Y >= b.Y;
//     }
//     public static bool operator <(Point a, Point b)
//     {
//         return a.X < b.X && a.Y < b.Y;
//     }
//     public static bool operator >(Point a, Point b)
//     {
//         return a.X > b.X && a.Y > b.Y;
//     }
//     public override bool Equals(object? obj) {
//         return Equals(obj);
//     }
//     public override int GetHashCode() {
//         return GetHashCode();
//     }

//     /// <summary> Calculates distance between two points. </summary>
//     /// <param name="a">Point A</param>
//     /// <param name="b">Point B</param>
//     /// <returns>Distance between A and B</returns>
//     public static float Distance(Point a, Point b) {
//         Point dV = b - a;
//         float d = (float)Math.Sqrt(Math.Pow(dV.X, 2) + Math.Pow(dV.Y, 2));
//         return d;
//     }

//     public void Clamp(Point min, Point max) {
//         X = (X > max.X) ? max.X : X;
//         X = (X < min.X) ? min.X : X;

//         Y = (Y > max.Y) ? max.Y : Y;
//         Y = (Y < min.Y) ? min.Y : Y;
//     }
// }

// /// <summary> Utility class. </summary>
// public class Utility {
//     static public int Clamp(int a, int min, int max) {
//         a = (a > max) ? max : a;
//         a = (a < min) ? min : a;

//         return a;
//     }
// }

// /// <summary> Vector of two floats. </summary>
// public struct Vector {
//     public float X { get; set; }
//     public float Y { get; set; }


//     public static Vector Zero { get; private set; } = new Vector(0, 0);
//     public Vector(float x, float y) {
//         this.X = x;
//         this.Y = y;
//     }

//     public Point ToPoint => new Point((int)Math.Round(X, 0), (int)Math.Round(Y, 0));

//     public void Rotate(float a) {
//         Vector n = Vector.Zero;

//         n.X = (float)(X * Math.Cos(a / 57.3f) - Y * Math.Sin(a / 57.3f));
//         n.Y = (float)(X * Math.Sin(a / 57.3f) + Y * Math.Cos(a / 57.3f));

//         X = n.X;
//         Y = n.Y;
//     }

//     public static Vector operator + (Vector a, Vector b) {
//         return new Vector(a.X + b.X, a.Y + b.Y);
//     }
//     public static Vector operator - (Vector a, Vector b) {
//         return new Vector(a.X - b.X, a.Y - b.Y);
//     }

//     public static Vector operator / (Vector a, float b) {
//         return new Vector((a.X / b), (a.Y / b));
//     }
//     public static Vector operator * (Vector a, float b) {
//         return new Vector((a.X * b), (a.Y * b));
//     }

//     public static float Distance(Vector a, Vector b) {
//         Vector dV = b - a;
//         float d = (float)Math.Sqrt(Math.Pow(dV.X, 2) + Math.Pow(dV.Y, 2));
//         return d;
//     }
// }


