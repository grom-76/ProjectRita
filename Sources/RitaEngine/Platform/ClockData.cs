
namespace RitaEngine.Platform;

using RitaEngine.Base;


[ StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public struct ClockData : IEquatable<ClockData>
{
    public delegate void PFN_Update(ref ClockData data ,ref ClockFunctions func );
    public ulong PreviousTick =0L;
    public ulong BaseTime =0L;
    public ulong StopTime =0L;
    public ulong PausedTime =0L;
    public double FixedTimeStep =0.0;
    /// <summary> Period or second per count/cycles </summary>
    public double SecondPerCycle =0L;
    /// <summary>  Delta time in miliseecone </summary>
    public double ElapsedInMiliSec =0L;
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
