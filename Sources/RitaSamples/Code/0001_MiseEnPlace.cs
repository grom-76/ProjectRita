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
        //ALL CODE TO CREATE INITIALIZE PLATFORM SYSTEM
        RitaEngine.Base.PlatformConfig config = new();
        RitaEngine.Base.Platform.GraphicRenderConfig renderConfig = new();
        
        RitaEngine.Base.Platform.Clock clock = new();
        RitaEngine.Base.Platform.Window window = new();
        RitaEngine.Base.Platform.Inputs input = new();
        RitaEngine.Base.Platform.AudioDevice audio = new();
        RitaEngine.Base.Platform.GraphicDevice graphic = new();

        RitaEngine.Base.Audio.PlayerSound2D snd = new( );
        
        try
        {
            config.Game_Title = "My Game";
            config.Window_Resolution =  WindowResolution.HD_720p_1920x720;
            config.AssetPath( @"C:\Users\Administrator\Documents\Repos\ProjectRita\Assets\" );
            config.LogConfig(Log.Display.OnConsole);
            config.Clock_FixedTimeStep = 0.033;
            config.Clock_LoopMode = RitaEngine.Base.Platform.Config.ClockLoopMode.Default;
            config.Input_ShowCursor = true;
            config.Audio_Category = AudioCategory.GameMedia;
            config.Audio_Channels = AudioChannels.stereo;
            config.GraphicDevice_EnableDebugMode = true;

            clock.Init(config);
            window.Init(config, clock);
            input.Init(config, window);
            audio.Init(config);
            graphic.Init( config, window);

            // END INITIALIZE SYSTEM
            
            snd.Init( audio,  "demo.wav");

            renderConfig.BackColorARGB = RitaEngine.Base.Math.Color.Palette.AliceBlue;
            renderConfig.FragmentEntryPoint ="main";
            renderConfig.VertexEntryPoint ="main";
            renderConfig.FragmentShaderFileNameSPV =  "fragment_base.spv";
            renderConfig.VertexShaderFileNameSPV = "vertex_base.spv";
            renderConfig.Primitive = RitaEngine.Base.Math.GeometricPrimitive.CreateQuad(1.0f,1.0f);
            graphic.BuildRender(renderConfig);
            // BEGIN LOOP
            window.Show();
            clock.Reset();
            while(window.ShouldClose())
            {
                if ( input.IsKeyPressed( RitaEngine.Base.Platform.InputKeys.Escape ))
                {
                    window.RequestClose();
                }

                if (input.IsKeyPressed( RitaEngine.Base.Platform.InputKeys.Space ))
                {
                    snd.PlaySource();
                }

                graphic.DrawRender();

                window.DispatchPending();
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
            window.Release();
            clock.Release();
            config.Dispose();
        }
        
    }

}