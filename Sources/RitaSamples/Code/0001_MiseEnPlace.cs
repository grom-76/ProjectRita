namespace RitaSamples;

using RitaEngine.Base;
using RitaEngine.Base.Platform.Config;


public static class Sample_0001
{
    /// <summary>
    /// Just mise en place test
    /// </summary>
    public static void Run()
    {   
        RitaEngine.Base.Log.Config(Log.Display.OnConsole);
        string path = @"C:\Users\Administrator\Documents\Repos\ProjectRita\Assets\";

        //ALL CODE TO CREATE INITIALIZE PLATFORM SYSTEM
        RitaEngine.Base.Platform.Clock clock = new();
        RitaEngine.Base.Platform.Window win = new();
        RitaEngine.Base.Platform.Inputs input = new();
        RitaEngine.Base.Platform.AudioDevice audio = new();
        RitaEngine.Base.Platform.GraphicDevice graphic = new();
        RitaEngine.Base.Platform.Config.GraphicDeviceConfig graphicConfig = new();
        RitaEngine.Base.Platform.Config.GraphicRenderConfig renderConfig = new();
        RitaEngine.Base.Platform.Config.WindowConfig windowConfig = new();
        
        RitaEngine.Base.Audio.PlayerSound2D snd = new( );
        try
        {
            clock.Config.FixedTimeStep = 0.033;
            clock.Config.LoopMode = RitaEngine.Base.Platform.Config.ClockLoopMode.Default;
            clock.Init();

            windowConfig.SetTitle("My Game");
            windowConfig.SetResolution( WindowResolution.HD_720p_1920x720);
            win.Init(windowConfig);

            input.Config.ShowCursor = true;
            input.Init( win);

            audio.Config.Category = AudioCategory.GameMedia;
            audio.Config.Channels = AudioChannels.stereo;
            audio.Init();

            graphicConfig.EnableDebugMode = true;
            graphic.Init( graphicConfig, win);
            // END INITIALIZE SYSTEM

            
            snd.Init( audio, path+  "demo.wav");

            renderConfig.BackColorARGB = RitaEngine.Base.Math.Color.Palette.Lavender ;
            renderConfig.FragmentEntryPoint ="main";
            renderConfig.VertexEntryPoint ="main";
            renderConfig.FragmentShaderFileNameSPV = path + "fragment_base.spv";
            renderConfig.VertexShaderFileNameSPV = path + "vertex_base.spv";
            graphic.BuildRender(renderConfig);
            // BEGIN LOOP
            win.Show();

            while(win.ShouldClose())
            {
                if ( input.IsKeyPressed( RitaEngine.Base.Platform.InputKeys.Escape ))
                {
                    win.RequestClose();
                }

                if (input.IsKeyPressed( RitaEngine.Base.Platform.InputKeys.Space ))
                {
                    snd.PlaySource();
                }

                graphic.DrawRender();

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
            // graphicConfig.Dispose();
        }
        
    }

}