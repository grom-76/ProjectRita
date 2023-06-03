namespace RitaEngine.Base.Platform.Structures;



[ StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public struct ClockData : IEquatable<ClockData>
{
    public delegate void PFN_Update(ref ClockData data ,ref ClockFunctions func );
    public UInt64 PreviousTick =0;
    
    public double OneOverFrequency=0.0 ;
    public double Elapsed_ms=0.0;
    public double FixedTimeStep =0.0;

    public nint Kernel32 = nint.Zero;
    public PFN_Update LoopMethod = null!;
    private nint _address = nint.Zero;
    public bool IsPaused = false;

    public ClockData() {   _address = AddressOfPtrThis( ) ; }
    public unsafe nint AddressOfPtrThis( ) { 
            #pragma warning disable CS8500
        fixed (void* pointer = &this )  { return((nint) pointer ) ; }  
        #pragma warning restore
    }
    #region OVERRIDE    
    public override string ToString() => string.Format($"Data Input " );
    public unsafe override int GetHashCode() => HashCode.Combine( 0);
    public override bool Equals(object? obj) => obj is ClockData window && this.Equals(window) ;
    public unsafe bool Equals(ClockData other)=>  false;
    public static bool operator ==(ClockData  left, ClockData right) => left.Equals(right);
    public static bool operator !=(ClockData  left, ClockData right) => !left.Equals(right);
    #endregion
    public unsafe void Dispose()  { }

}
