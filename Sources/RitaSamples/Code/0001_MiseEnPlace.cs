namespace RitaSamples;

using RitaEngine.Base;
using RitaEngine.Platform.Config;


public static class Sample_0001
{
    /// <summary>
    /// Just mise en place test
    /// </summary>
    public static void Run()
    {   
        //ALL CODE TO CREATE INITIALIZE PLATFORM SYSTEM
        RitaEngine.Platform. PlatformConfig config = new();
        RitaEngine.Graphic. GraphicRenderConfig renderConfig = new();
        
        RitaEngine.Platform.Clock clock = new();
        RitaEngine.Platform.Window window = new();
        RitaEngine.Platform.Inputs input = new();
        RitaEngine.Platform.AudioDevice audio = new();
        RitaEngine.Platform.GraphicDevice graphic = new();

        RitaEngine.Audio.PlayerSound2D snd = new( );
        
        try
        {
            config.Game_Title = "My Game";
            config.Window_Resolution =  WindowResolution.HD_720p_1920x720;
            config.AssetPath( @"C:\Users\Administrator\Documents\Repos\ProjectRita\Assets\" );
            config.Log(Log.Display.OnConsole);
            // config.Garbage(GarbageCollectionPriority.Interactive, 1024*1024*60);
            config.Clock_FixedTimeStep = 0.033;
            config.Clock_LoopMode = RitaEngine.Platform.Config.ClockLoopMode.Default;
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

            renderConfig.BackColorARGB = RitaEngine.Math.Color.Palette.AliceBlue;
            renderConfig.FragmentEntryPoint ="main";
            renderConfig.VertexEntryPoint ="main";
            renderConfig.FragmentShaderFileNameSPV =  "fragment_base.spv";
            renderConfig.VertexShaderFileNameSPV = "vertex_base.spv";
            renderConfig.Primitive = RitaEngine.Math.GeometricPrimitive.CreateQuad(1.0f,1.0f);
            graphic.BuildRender(renderConfig);
            // BEGIN LOOP
            window.Show();
            clock.Reset();
            while(window.ShouldClose())
            {
                if ( input.IsKeyPressed( RitaEngine.Platform.InputKeys.Escape ))
                {
                    window.RequestClose();
                }

                if (input.IsKeyPressed( RitaEngine.Platform.InputKeys.Space ))
                {
                    snd.PlaySource();
                }

                graphic.DrawRender(renderConfig);

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