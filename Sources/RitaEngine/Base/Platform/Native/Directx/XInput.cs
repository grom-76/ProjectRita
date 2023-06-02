namespace RitaEngine.Base.Platform.Native.DirectX.XInput;



#region XINPUT

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential),SkipLocalsInit]
public unsafe static class Constants
{
    // public const string XInputDllName ="Xinput1_4.dll";

    public const int XUSER_INDEX_ANY = 0x000000FF;
    public const int XUSER_MAX_COUNT = 4;

    public const string XINPUT_DLL = "xinput1_4.dll";

    public const int XINPUT_GAMEPAD_DPAD_UP = 0x0001;
    public const int XINPUT_GAMEPAD_DPAD_DOWN = 0x0002;
    public const int XINPUT_GAMEPAD_DPAD_LEFT = 0x0004;
    public const int XINPUT_GAMEPAD_DPAD_RIGHT = 0x0008;
    public const int XINPUT_GAMEPAD_START = 0x0010;
    public const int XINPUT_GAMEPAD_BACK = 0x0020;
    public const int XINPUT_GAMEPAD_LEFT_THUMB = 0x0040;
    public const int XINPUT_GAMEPAD_RIGHT_THUMB = 0x0080;
    public const int XINPUT_GAMEPAD_LEFT_SHOULDER = 0x0100;
    public const int XINPUT_GAMEPAD_RIGHT_SHOULDER = 0x0200;
    public const int XINPUT_GAMEPAD_A = 0x1000;
    public const int XINPUT_GAMEPAD_B = 0x2000;
    public const int XINPUT_GAMEPAD_X = 0x4000;
    public const int XINPUT_GAMEPAD_Y = 0x8000;
    public const int XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE = 7849;
    public const int XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE = 8689;
    public const int XINPUT_GAMEPAD_TRIGGER_THRESHOLD = 30;
    public const int XINPUT_FLAG_GAMEPAD = 0x00000001;

    public const int BATTERY_DEVTYPE_GAMEPAD = 0x00;
    public const int BATTERY_DEVTYPE_HEADSET = 0x01;

}

[StructLayout(LayoutKind.Sequential, Pack =1),SkipLocalsInit]
public partial struct XINPUT_BATTERY_INFORMATION
{
    public byte BatteryType;
    public byte BatteryLevel;
}

[StructLayout(LayoutKind.Sequential, Pack =1),SkipLocalsInit]
public struct XINPUT_CAPABILITIES
{
    public byte Type;
    public byte SubType;          
    public ushort Flags;
    public XINPUT_GAMEPAD Gamepad;
    public XINPUT_VIBRATION Vibration;
}

[StructLayout(LayoutKind.Sequential, Pack =1),SkipLocalsInit]
public struct XINPUT_GAMEPAD
{
    public ushort wButtons;
    public byte bLeftTrigger;
    public byte bRightTrigger;
    public short sThumbLX;
    public short sThumbLY;
    public short sThumbRX;
    public short sThumbRY;
}

[StructLayout(LayoutKind.Sequential, Pack =1),SkipLocalsInit]
public struct XINPUT_KEYSTROKE
{
    public ushort VirtualKey;           
    public ushort Unicode;
    public ushort Flags;
    public byte UserIndex;
    public byte HidCode;
}

[StructLayout(LayoutKind.Sequential, Pack =1),SkipLocalsInit]
public struct XINPUT_STATE
{
    public uint dwPacketNumber;
    public XINPUT_GAMEPAD Gamepad;
}

[StructLayout(LayoutKind.Sequential, Pack =1),SkipLocalsInit]
public struct XINPUT_VIBRATION
{
    public ushort wLeftMotorSpeed;
    public ushort wRightMotorSpeed;
}


public enum CapabilitiesDevType
{
    XINPUT_DEVTYPE_GAMEPAD = 0x01
}
    

public enum CapabilitiesDevSubType{
     XINPUT_DEVSUBTYPE_GAMEPAD = 0x01,
    XINPUT_DEVSUBTYPE_UNKNOWN = 0x00,
    XINPUT_DEVSUBTYPE_WHEEL = 0x02,
    XINPUT_DEVSUBTYPE_ARCADE_STICK = 0x03,
    XINPUT_DEVSUBTYPE_FLIGHT_STICK = 0x04,
    XINPUT_DEVSUBTYPE_DANCE_PAD = 0x05,
    XINPUT_DEVSUBTYPE_GUITAR = 0x06,
    XINPUT_DEVSUBTYPE_GUITAR_ALTERNATE = 0x07,
    XINPUT_DEVSUBTYPE_DRUM_KIT = 0x08,
    XINPUT_DEVSUBTYPE_GUITAR_BASS = 0x0B,
    XINPUT_DEVSUBTYPE_ARCADE_PAD = 0x13,
}   
   

[Flags]public enum CapabilitieFeatures : uint
{
    XINPUT_CAPS_VOICE_SUPPORTED = 0x0004,
    XINPUT_CAPS_FFB_SUPPORTED = 0x0001,
    XINPUT_CAPS_WIRELESS = 0x0002,
    XINPUT_CAPS_PMD_SUPPORTED = 0x0008,
    XINPUT_CAPS_NO_NAVIGATION = 0x0010,
}


public enum KeystrokeFlags
{
    XINPUT_KEYSTROKE_KEYDOWN = 0x0001,
    XINPUT_KEYSTROKE_KEYUP = 0x0002,
    XINPUT_KEYSTROKE_REPEAT = 0x0004
}
    
[Flags]public enum VirtualKey : uint{
    VK_PAD_A = 0x5800,
    VK_PAD_B = 0x5801,
    VK_PAD_X = 0x5802,
    VK_PAD_Y = 0x5803,
    VK_PAD_RSHOULDER = 0x5804,
    VK_PAD_LSHOULDER = 0x5805,
    VK_PAD_LTRIGGER = 0x5806,
    VK_PAD_RTRIGGER = 0x5807,
    VK_PAD_DPAD_UP = 0x5810,
    VK_PAD_DPAD_DOWN = 0x5811,
    VK_PAD_DPAD_LEFT = 0x5812,
    VK_PAD_DPAD_RIGHT = 0x5813,
    VK_PAD_START = 0x5814,
    VK_PAD_BACK = 0x5815,
    VK_PAD_LTHUMB_PRESS = 0x5816,
    VK_PAD_RTHUMB_PRESS = 0x5817,
    VK_PAD_LTHUMB_UP = 0x5820,
    VK_PAD_LTHUMB_DOWN = 0x5821,
    VK_PAD_LTHUMB_RIGHT = 0x5822,
    VK_PAD_LTHUMB_LEFT = 0x5823,
    VK_PAD_LTHUMB_UPLEFT = 0x5824,
    VK_PAD_LTHUMB_UPRIGHT = 0x5825,
    VK_PAD_LTHUMB_DOWNRIGHT = 0x5826,
    VK_PAD_LTHUMB_DOWNLEFT = 0x5827,
    VK_PAD_RTHUMB_UP = 0x5830,
    VK_PAD_RTHUMB_DOWN = 0x5831,
    VK_PAD_RTHUMB_RIGHT = 0x5832,
    VK_PAD_RTHUMB_LEFT = 0x5833,
    VK_PAD_RTHUMB_UPLEFT = 0x5834,
    VK_PAD_RTHUMB_UPRIGHT = 0x5835,
    VK_PAD_RTHUMB_DOWNRIGHT = 0x5836,
    VK_PAD_RTHUMB_DOWNLEFT = 0x5837,
}
  

public enum BatteryTypeInformation : uint{
   
    BATTERY_TYPE_DISCONNECTED = 0x00,
    BATTERY_TYPE_WIRED = 0x01,
    BATTERY_TYPE_ALKALINE = 0x02,
    BATTERY_TYPE_NIMH = 0x03,
    BATTERY_TYPE_UNKNOWN = 0xFF,
    
    }

public enum BatteryLevelInformation : uint
{
    BATTERY_LEVEL_EMPTY = 0x00,
    BATTERY_LEVEL_LOW = 0x01,
    BATTERY_LEVEL_MEDIUM = 0x02,
    BATTERY_LEVEL_FULL = 0x03,
}

        

#endregion
