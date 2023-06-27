namespace RitaEngine.Platform.Structures;

using RitaEngine.Base;

[ StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public struct Viewport : IEquatable<Viewport>
{
    private nint _address = nint.Zero;
    public int Width = 0;
    public int Height = 0;
    public int Left=0;
    public int Top=0;
    public uint Style = 0;
    public uint ExStyle = 0;

    public Viewport() {   _address = AddressOfPtrThis( ) ; }
    public unsafe nint AddressOfPtrThis( ) { 
            #pragma warning disable CS8500
        fixed (void* pointer = &this )  { return((nint) pointer ) ; }  
        #pragma warning restore
    }
    #region OVERRIDE    
    public override string ToString() => string.Format($"Data ViewPOrt " );
    public unsafe override int GetHashCode() => HashCode.Combine(Width,Height);
    public override bool Equals(object? obj) => obj is Viewport  window && this.Equals(window) ;
    public unsafe bool Equals(Viewport other)=>  false;
    public static bool operator ==(Viewport  left,Viewport right) => left.Equals(right);
    public static bool operator !=(Viewport  left,Viewport right) => !left.Equals(right);
    #endregion

    /*
    monitorInfo ( handle max size dpi ,... , resolutionCurrent) 
    widthSurface
    heightSurface
    leftsurface
    topSurface
    styleSurface
    handleSurface
    CurrentGraphicArea
    GraphicArea [ ]
    mindepth maxdepth 
    left top 
    width height
    offset left offset top
    */
}


