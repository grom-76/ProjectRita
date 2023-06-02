namespace RitaEngine.Base;

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack =BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static class Log
{
public enum Display {
    None,
    OnConsole,
    WithSystemDebug,
    InFile
}
private static string _file="log.txt";
private delegate void PFN_Display(int color,string header, int line , string file, string method,string message);
private static PFN_Display _display = ToNull;

private static void Release()
{
    _file = null!;
    _display = null!;
}

[MethodImpl(MethodImplOptions.AggressiveInlining)]
private unsafe static string FormatMessage(  int line ,string file, string method,string message)
{
    return new StringBuilder()
        #if WIN64
        .Append("|PID:" ).Append( Win64.GetCurrentProcessId() )
        .Append("|THD:" ).Append(  Win64.GetCurrentThreadId() )
        #endif
        .Append("|").Append( System.DateTime.Now.ToString("hh:mm:ss:fff") )
        .Append("| " ).Append(file).Append(".").Append(method)
        .Append("(").Append(line).Append(") > ")
        .Append(message).ToString();
}

private static void ToConsole(int color,string header, int line ,string file, string method,string message)
{
    System.Console.ForegroundColor = (System.ConsoleColor)color;
    System.Console.Write(header);
    System.Console.ResetColor();
    System.Console.WriteLine( FormatMessage(   line , file,  method, message) );
}

private static void ToDebug(int color,string header, int line ,string file,string method, string message)
{
    System.Diagnostics.Debug.Write(header);
    System.Diagnostics.Debug.WriteLine( FormatMessage(   line , file,  method, message) );
}

private static void ToNull(int color,string header, int line ,string file,string method,  string message)  {  }  

private static void ToFile(int color,string header, int line ,string file, string method, string message)
{
    System.IO.File.AppendAllText(_file, FormatMessage(   line , file,  method, message)  );
}

//TODO : add level all only critical only error verbose ( capabilities  debug)  and add show : Only Yours OR both=  Yours and system 
private static void Config(Display output,string file="")
{
    if( output == Display.OnConsole)
        _display = ToConsole;
    else if ( output == Display.None)
        _display =ToNull;
    else if ( output == Display.WithSystemDebug)
        _display = ToDebug;
    else if ( output == Display.InFile)
    {
        _display = ToFile;         
        _file = file?? "Log_"+ System.DateTime.Now.ToShortTimeString()+".txt" ;    
    }
    else
    _display =ToNull; 
}

public static void Error(string message,
    [CallerFilePath] string file="",
    [CallerMemberName] string method="",
    [CallerLineNumber] int line=0 )
    => _display((int)System.ConsoleColor.DarkMagenta ,"[ERROR]",line,System.IO.Path.GetFileNameWithoutExtension(file),method,message );

public static void Critical(string message,
    [CallerFilePath] string file="",
    [CallerMemberName] string method="",
    [CallerLineNumber] int line=0 )
    =>  _display((int)System.ConsoleColor.Red ,"[CRITIC]",line,System.IO.Path.GetFileNameWithoutExtension(file),method,message );

public static void WarnWhenConditionIsFalse( bool condition,
    string message,

    [CallerFilePath] string file="",
    [CallerMemberName] string method="",
    [CallerLineNumber] int line=0 )
    =>  _display( condition ? (int)System.ConsoleColor.Green:(int)System.ConsoleColor.Yellow  , 
                    condition ? "[INFO]":"[WARN]" ,line,System.IO.Path.GetFileNameWithoutExtension(file),method,message );

public static void Info(string message,
    [CallerFilePath] string file="",
    [CallerMemberName] string method="",
    [CallerLineNumber] int line=0 )
    => _display!((int)System.ConsoleColor.Green ,"[INFO]",line,System.IO.Path.GetFileNameWithoutExtension(file),method,message );

public static void Warning(string message, 
    [CallerFilePath] string file="",
    [CallerMemberName] string method="", 
    [CallerLineNumber] int line=0 )
    => _display!((int)System.ConsoleColor.Yellow ,"[WARN]",line,System.IO.Path.GetFileNameWithoutExtension(file),method,message );

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack =BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static class Win64
{
    [DllImport("kernel32.dll", SetLastError = false)] [SuppressGCTransition][SuppressUnmanagedCodeSecurity]
    public static extern uint GetCurrentProcessId();

    [DllImport("kernel32.dll", SetLastError = false)] [SuppressGCTransition][SuppressUnmanagedCodeSecurity]
    public static extern uint GetCurrentThreadId();
}


}

