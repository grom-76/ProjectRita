namespace RitaEngine.Base.Platform.Structures;

using RitaEngine.Base.Platform.API.DirectX.XInput;


[StructLayout(LayoutKind.Sequential, Pack =4),SkipLocalsInit]
public readonly struct ControllerDeviceFunction
{
    public unsafe readonly delegate* unmanaged<uint,uint, XINPUT_KEYSTROKE*,uint> XInputGetKeystroke = null;
    public unsafe readonly delegate* unmanaged<uint,uint,XINPUT_BATTERY_INFORMATION*,uint> XInputGetBatteryInformation = null;
    public unsafe readonly delegate* unmanaged<uint,ushort*,uint*,ushort*,uint*,uint> XInputGetAudioDeviceIds = null;
    public unsafe readonly delegate* unmanaged<int,void> XInputEnable = null;
    public unsafe readonly delegate* unmanaged<uint,uint, XINPUT_CAPABILITIES*,uint> XInputGetCapabilities = null;
    public unsafe readonly delegate* unmanaged<uint , XINPUT_VIBRATION*,uint> XInputSetState = null;
    public unsafe readonly delegate* unmanaged<uint, XINPUT_STATE*,uint> XInputGetState = null;

    public unsafe ControllerDeviceFunction(PFN_GetSymbolPointer load, IntPtr module)
    {
        XInputGetKeystroke = (delegate* unmanaged<uint,uint, XINPUT_KEYSTROKE*,uint>) load( module, "XInputGetKeystroke" );
        XInputGetBatteryInformation =(delegate* unmanaged<uint,uint,XINPUT_BATTERY_INFORMATION*,uint> ) load( module, "XInputGetBatteryInformation" );
        XInputGetAudioDeviceIds =(delegate* unmanaged<uint,ushort*,uint*,ushort*,uint*,uint>  )load( module, "XInputGetAudioDeviceIds" );
        XInputEnable =(delegate* unmanaged<int , void> ) load( module, "XInputEnable" );
        XInputGetCapabilities =(delegate* unmanaged<uint,uint, XINPUT_CAPABILITIES*,uint> ) load( module, "XInputGetCapabilities" );
        XInputSetState =(delegate* unmanaged<uint,XINPUT_VIBRATION*,uint>) load( module, "XInputSetState" );
        XInputGetState =( delegate* unmanaged<uint, XINPUT_STATE*,uint>) load( module, "XInputGetState" );
    }

     public unsafe nint AddressOfPtrThis( ){fixed (void* pointer = &this)  { return((nint) pointer ) ; }  }
    #region OVERRIDE
    public override string ToString() => string.Format($"Vector" );
    public unsafe override int GetHashCode() => HashCode.Combine( ((nint)XInputGetState).ToInt32()  ,  ((nint)XInputSetState).ToInt32(),  ((nint)XInputGetState).ToInt32(), ((nint) XInputGetCapabilities).ToInt32() ) ;
    public override bool Equals(object? obj) => obj is ControllerDeviceFunction context && this.Equals(context) ;
    public unsafe bool Equals(ControllerDeviceFunction? other)=> other is ControllerDeviceFunction input && (((nint)XInputGetState).ToInt64()).Equals(((nint)input.XInputGetState).ToInt64() );
    public static bool operator ==(ControllerDeviceFunction  left, ControllerDeviceFunction right) => left.Equals(right);
    public static bool operator !=(ControllerDeviceFunction  left, ControllerDeviceFunction  right) => !left.Equals(right);
    public void Dispose() {  }
    #endregion


}
