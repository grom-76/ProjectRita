namespace RitaEngine.Base.Platform.Structures;


using RitaEngine.Base.Platform.API.DirectX.XInput;

[StructLayout(LayoutKind.Sequential), SkipLocalsInit]
public unsafe struct ControllerDeviceData : IEquatable<ControllerDeviceData>
{
    public ControllerState[] States = null!;
    public int Max_Count =4;
    public double AcquireMiliSec = 0.33;
    public double Accumulate =0.0;
    public ControllerDeviceData() { }
    public void Release()
    {
        States = null!;

    }

        #region OVERRIDE    
    public override string ToString()  
        => string.Format($"DataModule" );
    public override int GetHashCode() => (int)0;
    public override bool Equals(object? obj) => obj is ControllerDeviceData data && this.Equals(data) ;
    public bool Equals(ControllerDeviceData other)=>  false;
    public static bool operator ==(ControllerDeviceData left, ControllerDeviceData right) => left.Equals(right);
    public static bool operator !=(ControllerDeviceData left, ControllerDeviceData  right) => !left.Equals(right);
    #endregion


}

[StructLayout(LayoutKind.Sequential), SkipLocalsInit]
public struct ControllerState
{

    public bool IsConntected = false;
    public uint Buttons = 0;
    public uint PreviousButtons =0;
    public VirtualKey keystroke =0;
    public KeystrokeFlags KeystrokState =0;
    public short Left_X =0;
    public short Left_Y =0;
    public short Right_X =0;
    public short Right_Y =0;
    public short threshold = 10;
    public ushort LeftMotorSpeedMax = 0;
    public ushort RightMotorSpeedMax = 0;
    public byte Left_Trigger =0;
    public byte Right_Trigger = 0;

    public bool IsConnected = false;
    public bool HaveVibration = false;
    public BatteryLevelInformation BatteryLevel = 0;//BATTERY_LEVEL_EMPTY
    public BatteryTypeInformation BatteryType =0;// BATTERY_TYPE_WIRED
    public CapabilitiesDevType Type = CapabilitiesDevType.XINPUT_DEVTYPE_GAMEPAD;//XINPUT_DEVTYPE_GAMEPAD
    public CapabilitiesDevSubType SubType =CapabilitiesDevSubType.XINPUT_DEVSUBTYPE_UNKNOWN; //XINPUT_DEVSUBTYPE_GAMEPAD
    public  CapabilitieFeatures Features =0; // XINPUT_CAPS_FFB_SUPPORTED 

    public ushort LeftMotor =0;
    public ushort RightMotor =0;

    public ControllerState()        {    }
}
