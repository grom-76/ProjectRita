namespace RitaEngine.Base.Platform.Structures;



[ StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public struct ClockData : IEquatable<ClockData>
{
    public delegate void PFN_Update(ref ClockData data ,ref ClockFunctions func );
    public UInt64 PreviousTick =0;
    
    /// <summary> Period or second per count/cycles </summary>
    public double SecondPerCycle=0.0 ;
    public double ElapsedInMiliSec=0.0;
    public double FixedTimeStep =0.0;
    public PFN_Update LoopMethod = null!;
    public bool IsInPause = false;

    public ClockData() { }

    #region OVERRIDE    
    public override string ToString() => string.Format($"Data Input " );
    public unsafe override int GetHashCode() => HashCode.Combine( 0);
    public override bool Equals(object? obj) => obj is ClockData clock && this.Equals(clock) ;
    public unsafe bool Equals(ClockData other)=>  false;
    public static bool operator ==(ClockData  left, ClockData right) => left.Equals(right);
    public static bool operator !=(ClockData  left, ClockData right) => !left.Equals(right);
    #endregion 
}
