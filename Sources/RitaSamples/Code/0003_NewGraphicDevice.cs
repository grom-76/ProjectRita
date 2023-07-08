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
        RitaEngine.Graphic.GraphicDeviceDatas data = new();
        RitaEngine.Graphic.GraphicsConfig gconfig = new();
        
        
        try
        {
            config.Game_Title = "My Game";
            config.Window_Resolution =  WindowResolution.HD_720p_1920x720;
            config.AssetPath( @"C:\Users\Administrator\Documents\Repos\ProjectRita\Assets\" );
            config.Log(Log.Display.OnConsole);
            
            clock.Init(config);
            window.Init(config, clock);
            input.Init(config, window);

            RitaEngine.Graphic.GraphicDeviceImplement.App.Init(ref func,ref data,ref gconfig , in window );    
            
            window.Show();
            clock.Reset();
            while(window.ShouldClose())
            {
                if ( input.IsKeyPressed( RitaEngine.Input.InputKeys.Escape ))
                {
                    window.RequestClose();
                }

                
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
            
            input.Release();
            window.Release();
            clock.Release();
            config.Dispose();
        }
        
    }

}