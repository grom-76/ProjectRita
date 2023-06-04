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
        string path = @"C:\Users\Administrator\Documents\ProjectRita\Assets\";

        //ALL CODE TO CREATE INITIALIZE PLATFORM SYSTEM
        RitaEngine.Base.Platform.Clock clock = new();
        RitaEngine.Base.Platform.Window win = new();
        RitaEngine.Base.Platform.Inputs input = new();
        RitaEngine.Base.Platform.AudioDevice audio = new();
        RitaEngine.Base.Platform.GraphicDevice graphic = new();
        
        RitaEngine.Base.Audio.PlayerSound2D snd = new( );
        try
        {
            clock.Config.FixedTimeStep = 0.033;
            clock.Config.LoopMode = RitaEngine.Base.Platform.Config.ClockLoopMode.Default;
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

            
            snd.Init( audio, path+  "demo.wav");

            graphic.Render.BackColorARGB = RitaEngine.Base.Math.Color.Palette.Lavender ;
            graphic.Render.FragmentEntryPoint ="main";
            graphic.Render.VertexEntryPoint ="main";
            graphic.Render.FragmentShaderFileNameSPV = path + "fragment_base.spv";
            graphic.Render.VertexShaderFileNameSPV = path + "vertex_base.spv";
            graphic.BuildRender();
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
        
    }

}