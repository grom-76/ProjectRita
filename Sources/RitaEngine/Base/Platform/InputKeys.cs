namespace RitaEngine.Base.Platform;


[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack =1),SkipLocalsInit]
public static class InputKeys 
{
	public static byte Back = 0x8;
	public static byte Tab = 0x9;
	public static byte Enter = 0xd;
	public static byte Pause = 0x13;
	public static byte CapsLock = 0x14;
	public static byte Kana = 0x15;
	public static byte ImeOn = 0x16;
	public static byte Kanji = 0x19;
	public static byte ImeOff = 0x1a;
	public static byte Escape = 0x1b;
	public static byte ImeConvert = 0x1c;
	public static byte ImeNoConvert = 0x1d;
	public static byte Space = 0x20;
	public static byte PageUp = 0x21;
	public static byte PageDown = 0x22;
	public static byte End = 0x23;
	public static byte Home = 0x24;
	public static byte Left = 0x25;
	public static byte Up = 0x26;
	public static byte Right = 0x27;
	public static byte Down = 0x28;
	public static byte Select = 0x29;
	public static byte Print = 0x2a;
	public static byte Execute = 0x2b;
	public static byte PrintScreen = 0x2c;
	public static byte Insert = 0x2d;
	public static byte Delete = 0x2e;
	public static byte Help = 0x2f;
	public static byte D0 = 0x30;
	public static byte D1 = 0x31;
	public static byte D2 = 0x32;
	public static byte D3 = 0x33;
	public static byte D4 = 0x34;
	public static byte D5 = 0x35;
	public static byte D6 = 0x36;
	public static byte D7 = 0x37;
	public static byte D8 = 0x38;
	public static byte D9 = 0x39;
	public static byte A = 0x41;
	public static byte B = 0x42;
	public static byte C = 0x43;
	public static byte D = 0x44;
	public static byte E = 0x45;
	public static byte F = 0x46;
	public static byte G = 0x47;
	public static byte H = 0x48;
	public static byte I = 0x49;
	public static byte J = 0x4a;
	public static byte K = 0x4b;
	public static byte L = 0x4c;
	public static byte M = 0x4d;
	public static byte N = 0x4e;
	public static byte O = 0x4f;
	public static byte P = 0x50;
	public static byte Q = 0x51;
	public static byte R = 0x52;
	public static byte S = 0x53;
	public static byte T = 0x54;
	public static byte U = 0x55;
	public static byte V = 0x56;
	public static byte W = 0x57;
	public static byte X = 0x58;
	public static byte Y = 0x59;
	public static byte Z = 0x5a;
	public static byte LeftWindows = 0x5b;
	public static byte RightWindows = 0x5c;
	public static byte Apps = 0x5d;
	public static byte Sleep = 0x5f;
	public static byte NumPad0 = 0x60;
	public static byte NumPad1 = 0x61;
	public static byte NumPad2 = 0x62;
	public static byte NumPad3 = 0x63;
	public static byte NumPad4 = 0x64;
	public static byte NumPad5 = 0x65;
	public static byte NumPad6 = 0x66;
	public static byte NumPad7 = 0x67;
	public static byte NumPad8 = 0x68;
	public static byte NumPad9 = 0x69;
	public static byte Multiply = 0x6a;
	public static byte Add = 0x6b;
	public static byte Separator = 0x6c;
	public static byte Subtract = 0x6d;
	public static byte Decimal = 0x6e;
	public static byte Divide = 0x6f;
	public static byte F1 = 0x70;
	public static byte F2 = 0x71;
	public static byte F3 = 0x72;
	public static byte F4 = 0x73;
	public static byte F5 = 0x74;
	public static byte F6 = 0x75;
	public static byte F7 = 0x76;
	public static byte F8 = 0x77;
	public static byte F9 = 0x78;
	public static byte F10 = 0x79;
	public static byte F11 = 0x7a;
	public static byte F12 = 0x7b;
	public static byte F13 = 0x7c;
	public static byte F14 = 0x7d;
	public static byte F15 = 0x7e;
	public static byte F16 = 0x7f;
	public static byte F17 = 0x80;
	public static byte F18 = 0x81;
	public static byte F19 = 0x82;
	public static byte F20 = 0x83;
	public static byte F21 = 0x84;
	public static byte F22 = 0x85;
	public static byte F23 = 0x86;
	public static byte F24 = 0x87;
	public static byte NumLock = 0x90;
	public static byte Scroll = 0x91;
	public static byte LeftShift = 0xa0;
	public static byte RightShift = 0xa1;
	public static byte LeftControl = 0xa2;
	public static byte RightControl = 0xa3;
	public static byte LeftAlt = 0xa4;
	public static byte RightAlt = 0xa5;
	public static byte BrowserBack = 0xa6;
	public static byte BrowserForward = 0xa7;
	public static byte BrowserRefresh = 0xa8;
	public static byte BrowserStop = 0xa9;
	public static byte BrowserSearch = 0xaa;
	public static byte BrowserFavorites = 0xab;
	public static byte BrowserHome = 0xac;
	public static byte VolumeMute = 0xad;
	public static byte VolumeDown = 0xae;
	public static byte VolumeUp = 0xaf;
	public static byte MediaNextTrack = 0xb0;
	public static byte MediaPreviousTrack = 0xb1;
	public static byte MediaStop = 0xb2;
	public static byte MediaPlayPause = 0xb3;
	public static byte LaunchMail = 0xb4;
	public static byte SelectMedia = 0xb5;
	public static byte LaunchApplication1 = 0xb6;
	public static byte LaunchApplication2 = 0xb7;
	public static byte OemSemicolon = 0xba;
	public static byte OemPlus = 0xbb;
	public static byte OemComma = 0xbc;
	public static byte OemMinus = 0xbd;
	public static byte OemPeriod = 0xbe;
	public static byte OemQuestion = 0xbf;
	public static byte OemTilde = 0xc0;
	public static byte OemOpenBrackets = 0xdb;
	public static byte OemPipe = 0xdc;
	public static byte OemCloseBrackets = 0xdd;
	public static byte OemQuotes = 0xde;
	public static byte Oem8 = 0xdf;
	public static byte OemBackslash = 0xe2;
	public static byte ProcessKey = 0xe5;
	public static byte OemCopy = 0xf2;
	public static byte OemAuto = 0xf3;
	public static byte OemEnlW = 0xf4;
	public static byte Attn = 0xf6;
	public static byte Crsel = 0xf7;
	public static byte Exsel = 0xf8;
	public static byte EraseEof = 0xf9;
	public static byte Play = 0xfa;
	public static byte Zoom = 0xfb;
	public static byte Pa1 = 0xfd;
	public static byte OemClear = 0xfe;

}
