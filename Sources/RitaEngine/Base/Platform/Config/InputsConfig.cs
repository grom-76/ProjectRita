namespace RitaEngine.Base.Platform.Config;


[StructLayout(LayoutKind.Sequential, Pack = 4),SkipLocalsInit]
public sealed class InputsConfig  : IDisposable// TODO rename to config parameters
{
    public string ModulesUser32 = "User32.dll";
    // public string AdditionnalSearchPathDLL =string.Empty;
    public bool MappingKeysToAzerty = true;
    public string Langue ="FRA";

    public bool ShowCursor = false;

    //Realse Builder 
    public void ChangeMappingToAzerty()
    {
        MappingKeysToAzerty = true;
    }

    // public void AddSearchPathDLL( string path)
    // {
    //     AdditionnalSearchPathDLL = path;
    // }

    public void ChangeDLLName( string dll )
    {
        ModulesUser32  = dll;
    }

    public InputsConfig() { }

    public InputsConfig GetConfig() { return this;} 

    public void Dispose()
    {
        // ModulesUser32 = null!; always reclame by garbage collector
        GC.SuppressFinalize(this);
    }
}

