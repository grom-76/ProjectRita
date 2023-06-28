using RitaEngine.Base;

namespace RitaEngine;


[SkipLocalsInit, StructLayout(LayoutKind.Sequential)]
public abstract class RitaEngineMain : IDisposable, IEquatable<RitaEngineMain>
{
    #region PUBLIC ACCESS 

    /// <summary> Ctor Empty for settings game use each instance </summary>
    public RitaEngineMain()
    {     
        //GC latency
    }
    
    /// <summary>  Just Run  with try catch  </summary>
    public void Run()
    {
        try
        {
            InternalInit();

            InternalLoad(); 

            InternalWarmUp();

            InternalLoop();

        }
        catch (System.Exception ex)
        {
            Log.Critical(ex.Message);
        }
    }

    ///<inherit />
    public void Dispose()
    {
        InternalRelease();
        GC.SuppressFinalize(this);

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }
    
    #endregion
    
    #region Private Internal

    private unsafe void InternalInit()
    {    
        Init();
        clock.Init(Config);
        window.Init(Config,clock);
        graphic.Init(Config, window );
        input.Init( Config, window);
        audio.Init(Config);  
    }


    private void InternalLoad()
    {
        Load();   
    }

    private void InternalWarmUp()
    {
        WarmUp();
        window.Show();
        clock.Reset();
    }

    private void InternalLoop()
    {

        while(window.ShouldClose())
        {
          
            window.DispatchPending();
            clock.Update();
            
            #if !DEBUG
            if (window.IsLostFocus)
            {
                continue;
            }
            #endif
        
            input.Update();
            UpdateInputs();

            UpdatePhysics();

            UpdateDraw();
            graphic.DrawRender(RenderConfig);
        }
    }

    private void InternalRelease()
    {
        if ( _disposed)return ;
        Log.Info("Dispose RitaEngineMain");
        Release();

        graphic.Release();
        audio.Release();
        input.Release();
        window.Release();
        clock.Release();
        Config.Dispose();
        Log.Release();

    }

    ~RitaEngineMain(){ InternalRelease();}
    #endregion
    
    #region Attributs
    RitaEngine.Platform.Clock clock = new();
    RitaEngine.Platform.Window window = new();
    RitaEngine.Input.Inputs input = new();
    RitaEngine.Audio.AudioDevice audio = new();
    RitaEngine.Graphic.GraphicDevice graphic =new();
    private bool _disposed = false ;

    #endregion
    public RitaEngine.Platform.PlatformConfig Config = new();
    public RitaEngine.Graphic.GraphicRenderConfig RenderConfig = new();

    public ref readonly RitaEngine.Platform.Clock Clock => ref clock;
    public ref RitaEngine.Platform.Window Window => ref window;
    public ref readonly RitaEngine.Input.Inputs Input => ref input;
    public ref readonly RitaEngine.Audio.AudioDevice AudioDevice => ref  audio;
    public ref RitaEngine.Graphic.GraphicDevice GraphicDevice => ref graphic;

    #region [ Abstract to override ]

    /// <summary>
    /// Creation et positionnment des models (sprites)
    /// autre donées du jeu, Ici le context graphic et audio et activé
    /// </summary>
    protected abstract void Load();

    /// <summary>
    /// Charge tous les fichiers du jeux ou le nom de toutes les ressources
    /// Avant le init ( pas de context activé  )
    /// </summary>
    protected abstract void Init();

    /// <summary>
    /// Dans la boucle de jeu
    /// couche pour traiter les evenements
    /// Action de l'utilisateur / Reception de données d'un server / Action systeme /...
    /// </summary>
    protected abstract void UpdateInputs();

    /// <summary>
    /// Dans la boucle de jeu
    /// Pour le dessiner tous les objets créer
    /// </summary>
    protected abstract void UpdateDraw();

    /// <summary>
    /// Quitter proprement le programme
    /// tous ce que vous avez crée doit être détruis
    /// </summary>
    protected abstract void Release();

    /// <summary>
    /// Dans la boucle de jeu
    /// Mettre a jour les positions des objets, calculer les colisions
    /// ou la physiques ( ne pas inclure les inputs car gestion speciale du temps)
    /// </summary>
    protected abstract void UpdatePhysics();

    /// <summary>
    /// Realisé avant de faire le loop ( tous les device sont actif clock mis à jour ...)
    /// </summary>
    protected abstract void WarmUp();
    
    #endregion

    #region [ internal OVERRIDE ]
    /// <inheritdoc />
    public bool Equals(RitaEngineMain? other) => false;
                                    // public bool Equals(RitaEngineMain? other) => _graphicSurface.Settings.Name.Equals(other!._graphicSurface.Settings.Name )
                                    // && _graphicSurface.Settings.Handle.Equals(other!._graphicSurface.Settings.Handle);
    /// <inheritdoc />
    public static bool operator ==(RitaEngineMain left , RitaEngineMain right)=> left.Equals(right);
    /// <inheritdoc />
    public static bool operator !=( RitaEngineMain left , RitaEngineMain right)=> !left.Equals(right);
    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is RitaEngineMain _base && _base.Equals(this);
    /// <inheritdoc />
    public override string ToString() => $"Abstract class RitaEngineMain made with {RitaEngine.Base.BaseHelper.ENGINE_NAME} ver {RitaEngine.Base.BaseHelper.ENGINE_VERSION}";
    /// <inheritdoc />
    public override int GetHashCode() => ToString().GetHashCode();    
    #endregion
}
