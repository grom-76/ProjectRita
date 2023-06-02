namespace RitaEngine.Base.Platform.Config ;


[StructLayout(LayoutKind.Sequential, Pack = 4),SkipLocalsInit]
public sealed class ClockConfig : IDisposable
{
    public ClockLoopMode LoopMode = ClockLoopMode.Default;

    public double FixedTimeStep =0.033;

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}


public enum ClockLoopMode
{
    Interpolation,
    Accurate,
    FixedTimeStep,
    Default ,
    SequencingPattern,
}

