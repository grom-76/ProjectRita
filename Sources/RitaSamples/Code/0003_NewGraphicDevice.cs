namespace RitaSamples;

using RitaEngine.Audio;
using RitaEngine.Base;
using RitaEngine.Platform;

public static class Sample_0003
{
    /// <summary>
    /// Just mise en place test
    /// </summary>
    public static void Run()
    {   
        //ALL CODE TO CREATE INITIALIZE PLATFORM SYSTEM
        RitaEngine.Platform. PlatformConfig config = new();
        RitaEngine.Platform.Clock clock = new();
        RitaEngine.Platform.Window window = new();
        RitaEngine.Input.Inputs input = new();

        RitaEngine.API.Vulkan.VulkanFunctions func = new();
        RitaEngine.Graphic.GraphicsData data = new();
        RitaEngine.Graphic.GraphicsConfig graphicsConfig = new();
        
        
        try
        {
            config.Game_Title = "My Game";
            config.Window_Resolution =  WindowResolution.HD_720p_1920x720;
            config.AssetPath( @"C:\Users\Administrator\Documents\Repos\ProjectRita\Assets\" );
            config.Log(Log.Display.OnConsole);
            
            clock.Init(config);
            window.Init(config, clock);
            input.Init(config, window);

            graphicsConfig.Device.SetVSync(true);
            RitaEngine.Graphic.GraphicsImplement.Instance.Init(ref func,ref data,ref graphicsConfig , in window );    
            RitaEngine.Graphic.GraphicsImplement.Device.Init(ref func,ref data,ref graphicsConfig , in window );    
            RitaEngine.Graphic.GraphicsImplement.SwapChain.Init(ref func,ref data,ref graphicsConfig , in window ); 
            RitaEngine.Graphic.GraphicsImplement.Render.Init(ref func,ref data,ref graphicsConfig  ); 

            graphicsConfig.Render.BackGroundColor = RitaEngine.Math.Color.Palette.SkyBlue;
            RitaEngine.Graphic.GraphicsImplement.Pipeline.Build(ref func,ref data,ref graphicsConfig  ); 

            window.Show();
            clock.Reset();
            while(window.ShouldClose())
            {
                if ( input.IsKeyPressed( RitaEngine.Input.InputKeys.Escape ))
                {
                    window.RequestClose();
                }

                
                RitaEngine.Graphic.GraphicsImplement.Render.Draw(ref func, ref data, ref graphicsConfig);

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
            RitaEngine.Graphic.GraphicsImplement.Pipeline.Dispose(ref func, ref data);
            RitaEngine.Graphic.GraphicsImplement.Render.Dispose(ref func, ref data);
            RitaEngine.Graphic.GraphicsImplement.SwapChain.Dispose(ref func, ref data);
            RitaEngine.Graphic.GraphicsImplement.Device.Dispose(ref func, ref data);
            RitaEngine.Graphic.GraphicsImplement.Instance.Dispose(ref func, ref data);
            input.Release();
            window.Release();
            clock.Release();
            config.Dispose();
        }
        
    }

}