namespace RitaEngine.Base.Platform.Config;


[SuppressUnmanagedCodeSecurity,StructLayout(LayoutKind.Sequential), SkipLocalsInit]
public sealed class ControllerDeviceConfig : IDisposable
{
   
    //??? XInput wmm ,  MAx Controller 
    public int MaxController =  RitaEngine.Base.Platform.API.DirectX.XInput.Constants.XUSER_MAX_COUNT;
    public float DeadZone =0.24f; //Todo change dead zone ?
    public float threshold = 0.2f;

    public double AcquireMiliSec = 0.033;
    public bool IsPrintReport = false;
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}