namespace RitaEngine.Base.Platform ;// OS SYSTEMS// LAUNCHER CONTEXT DEVICE MACHINE , User Interface , Output

using RitaEngine.Base.Platform.Config;
using RitaEngine.Base.Platform.API.DirectX.XInput;
using RitaEngine.Base.Platform.Structures;


public struct ControllerDevice
{
     

    private const int FALSE =0 ;
    private const int TRUE = 1 ;

    public unsafe static void InitControllersXInput(ref  ControllerDeviceData  data  ,ref ControllerDeviceFunction func, in ControllerDeviceConfig config)
    {

        func.XInputEnable(TRUE );
        if ( config.MaxController <= 0 && config.MaxController > 8 )throw new Exception("Wrong max controller ");

        data.AcquireMiliSec = config.AcquireMiliSec;
        data.States = new ControllerState[config.MaxController];
       
        data.Max_Count = config.MaxController;
        
        for (uint i=0; i< config.MaxController; i++ )
        {
           data.States[i] = new();

        //    GetCapabilitieXInput(ref infos , i);
        //    data.States[i].LeftMotorSpeedMax = infos.Capabilities[i].LeftMotor ;
        //    data.States[i].RightMotorSpeedMax = infos.Capabilities[i].RightMotor ;
        }

    }
    public unsafe static void ReleaseControllersXInput(ref  ControllerDeviceData  data  ,ref ControllerDeviceFunction func)
    {

        for(uint i=0 ; i< data.Max_Count ; i++)
        {
            // StopFeedbackXInput(i);
        }
        func.XInputEnable(FALSE );

    }

    //TODO : value int  OR value float -1.0f to 1.0f OR Percent 0% to 100%

    public static void GetCapabilitiesXInput(ref  ControllerDeviceData  data  ,ref ControllerDeviceFunction func )
    {
        #if WIN
        for (uint i=0; i< infos.Max_Count; i++ )
        {
           GetCapabilitieXInput( ref infos, i);
        }
        #endif
    }

    public unsafe static void GetCapabilitieXInput(ref  ControllerDeviceData  data  ,ref ControllerDeviceFunction func, uint id)
    {
        #if WIN
        XINPUT_CAPABILITIES pCapabilities = default;
        uint error = XInputGetCapabilities( id,XINPUT_FLAG_GAMEPAD, &pCapabilities );
        infos.Capabilities[id].IsConnected =false;
        if ( error == 0)
        {
            infos.Capabilities[id].IsConnected =true;
            infos.Capabilities[id].Features = (CapabilitieFeatures)pCapabilities.Flags;
            infos.Capabilities[id].Type = (CapabilitiesDevType)pCapabilities.Type; 
            infos.Capabilities[id].SubType = (CapabilitiesDevSubType)pCapabilities.SubType; 

            XINPUT_VIBRATION vibration = pCapabilities.Vibration;
            if ( vibration.wLeftMotorSpeed >0 || vibration.wRightMotorSpeed > 0 )
            {
                infos.Capabilities[id].HaveVibration = true;
                infos.Capabilities[id].LeftMotor = vibration.wLeftMotorSpeed;
                infos.Capabilities[id].RightMotor = vibration.wRightMotorSpeed;
            }
            
            GetBatteryInformationXInput(ref infos , id);
        }
        #endif
    }

    public unsafe static void GetBatteryInformationXInput(ref  ControllerDeviceData  data  ,ref ControllerDeviceFunction func, uint id )
    {
        #if WIN
        XINPUT_BATTERY_INFORMATION batterie =default;
        _ = XInputGetBatteryInformation( id,BATTERY_DEVTYPE_GAMEPAD,&batterie);
        infos.Capabilities[id].BatteryLevel = (BatteryLevelInformation)batterie.BatteryLevel;
        infos.Capabilities[id].BatteryType = (BatteryTypeInformation)batterie.BatteryType;
        #endif
    }

    public unsafe  static void GetStateXInput(ref  ControllerDeviceData  data  ,ref ControllerDeviceFunction func, uint id )
    {
        #if WIN 
        XINPUT_STATE state = default ;
        uint dwResult = XInputGetState( id , &state );

        data.States[id].IsConntected = false;
        if( dwResult == 0 )
        {
            data.States[id].PreviousButtons = data.States[id].Buttons ;
            data.States[id].Buttons  = state.Gamepad.wButtons ;
            data.States[id].IsConntected = true;

            data.States[id].Left_Trigger = state.Gamepad.bLeftTrigger ;
            data.States[id].Right_Trigger = state.Gamepad.bRightTrigger ;
            
            data.States[id].Left_X = state.Gamepad.sThumbLX ;
            data.States[id].Left_Y = state.Gamepad.sThumbLY ;
            data.States[id].Right_X = state.Gamepad.sThumbRX ;
            data.States[id].Right_Y = state.Gamepad.sThumbRY ;
        }
        #endif
    }

    public unsafe  static void GeKeysStateXInput(ref  ControllerDeviceData  data  ,ref ControllerDeviceFunction func, uint id )
    { 

        XINPUT_KEYSTROKE keystroke= default;
        _ = func.XInputGetKeystroke( id, 0,&keystroke);
        // data.States[id].keystroke = (VirtualKey)keystroke.VirtualKey;
        // data.States[id].KeystrokState = (KeystrokeFlags)keystroke.Flags;
        
        // WCHAR Unicode;always 0
        // BYTE  UserIndex;
        // BYTE  HidCode;

    }
    
    public static unsafe void UpdateXInput(ref  ControllerDeviceData  data  ,ref ControllerDeviceFunction func, double elapsed ) // to do in update ...
    {
        data.Accumulate += elapsed;

        if ( data.Accumulate < data.AcquireMiliSec ) return;

        data.Accumulate =0.0;
        for (uint i=0; i< data.Max_Count; i++ )
        {
           GetStateXInput( ref data,ref func, i);
           GeKeysStateXInput( ref data,ref func,i);
        }
    }

    private static ushort Clamp(ushort value, ushort min, ushort max)=> value < min ? min : value > max ? max : value;
    
    /// <summary>
    /// Set vibration effect ( clamp value between 0 to 65535)
    /// </summary>
    /// <param name="i">Controller ID </param>
    /// <param name="leftMotor">0 to 65535  => reference to Data.LEftMotorSpeedMAx</param>
    /// <param name="rightMotor">0 to 65535 </param>
    public static unsafe void VibrationEffectXInput(ref ControllerDeviceFunction func, uint id, ushort leftMotor , ushort rightMotor , ushort leftMotorMax  , ushort rightMotorMax  )
    {

        XINPUT_VIBRATION vibration = default;
        vibration.wLeftMotorSpeed = Clamp( leftMotor, 0 ,rightMotorMax); // use any value between 0-65535 here
        vibration.wRightMotorSpeed = Clamp( rightMotor, 0 , leftMotorMax ); // use any value between 0-65535 here
        func.XInputSetState( id, &vibration );

    }

    private static unsafe void StopFeedbackXInput(ref ControllerDeviceFunction func,uint ID )
    {
        VibrationEffectXInput(ref func,ID,0,0,0,0);
    }

    public static float DeadZone(float x ,float deadzone = 0.24f,  float max = 1.0f )
    {
        var value = x < -max ? -max : x > max ? max : x;
        return  x > -deadzone && x < deadzone ? 0 : x;      
    }


// /// https://github.com/turanszkij/WickedEngine/blob/master/WickedEngine/wiXInput.cpp

}
   