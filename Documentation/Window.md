# WINDOW

## Installation Window

### PREREQUIS

    Window 8 10 11
    Driver avec vulkan installé
    [ SDK .Net7.0 ](https://dotnet.microsoft.com/en-us/download)
    Visual studio code
    git
    VS code extension DOC : omnisharp / C#Extension

### Utilisation

    Copie le repertoire Build\Window\*.* dans votre répertoire créer pour votre jeu
    dans le csproj ajouter la ligne suivante : 

### Get started  like monogame

    ```c#

    program.cs
        using game = g new();
        g.Run();

    game.cs
    
    public class Game : MCJEngine.GameBase
    {
        public Init()
        {

        }

        public Load()
        {

        }

        public Loop()
        {

        }

        public Shutdown()
        {

        }
    }
    ```

### get started like Viking:  c++ as sdl / raylib / glfw / sfml /

    ```c#
// program.cs
    RitaEngine.Base.Log.Config(Log.Display.OnConsole);
        string path = @"C:\Users\Administrator\Documents\ProjectRita\Assets\";

        //ALL CODE TO CREATE INITIALIZE PLATFORM SYSTEM
        RitaEngine.Platform.Clock clock = new();
        RitaEngine.Platform.Window win = new();
        RitaEngine.Platform.Inputs input = new();
        RitaEngine.Platform.AudioDevice audio = new();
        RitaEngine.Platform.GraphicDevice graphic = new();

        try
        {
            clock.Config.FixedTimeStep = 0.033;
            clock.Config.LoopMode = RitaEngine.Platform.ClockLoopMode.Default;
            clock.Init();

            win.Config.SetTitle("My Game");
            win.Config.SetResolution( WindowResolution.HD_720p_1920x720);
            win.Init();

            input.Config.ShowCursor = true;
            input.Init( win);

            audio.Config.Category = AudioCategory.GameMedia;
            audio.Config.Channels = AudioChannels.stereo;
            audio.Init();

            graphic.Config.EnableDebugMode = true;
            graphic.Init( win);
            // END INITIALIZE SYSTEM

            RitaEngine.Audio.PlayerSound2D snd = new( audio, path+  "demo.wav");

            graphic.Render.BackColorARGB = RitaEngine.Math.Color.Palette.Lavender ;
            graphic.Render.FragmentEntryPoint ="main";
            graphic.Render.VertexEntryPoint ="main";
            graphic.Render.FragmentShaderFileNameSPV = path + "fragment_base.spv";
            graphic.Render.VertexShaderFileNameSPV = path + "vertex_base.spv";
            graphic.BuildRender();
            // BEGIN LOOP
            win.Show();

            while(win.ShouldClose())
            {
                if ( input.IsKeyPressed( RitaEngine.Platform.InputKeys.Escape ))
                {
                    win.RequestClose();
                }

                if (input.IsKeyPressed( RitaEngine.Platform.InputKeys.Space ))
                {
                    snd.PlaySource();
                }

                graphic.Draw();

                win.DispatchPending();
                input.Update();
                clock.Update();
            }

        }
        catch( Exception ex)
        {
            Log.Critical(ex.Message);
        }
        finally
        {
            graphic.Release();
            audio.Release();
            input.Release();
            win.Release();
            clock.Release();
            Log.Release();
        }
    ```

### Compilation / Execution

    dotnet build et dotnet run  : thats all
