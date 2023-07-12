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

            graphicsConfig.Device.SetVerticalSynchro(true);
            RitaEngine.Graphic.GraphicsImplement.Device.Init(ref func,ref data,ref graphicsConfig , in window );    
            RitaEngine.Graphic.GraphicsImplement.Render.Init(ref func,ref data,ref graphicsConfig  ); 

            graphicsConfig.Render.BackGroundColor = RitaEngine.Math.Color.Palette.SkyBlue;
            graphicsConfig.Pipeline.FaceCullMode =  RitaEngine.Graphic.GraphicsConfig.FaceCullMode.None ;
            graphicsConfig.Pipeline.FragmentEntryPoint ="main";
            graphicsConfig.Pipeline.VertexEntryPoint ="main";
            graphicsConfig.Pipeline.FragmentShaderFileNameSPV =  "shader_depth_frag.spv";
            graphicsConfig.Pipeline.VertexShaderFileNameSPV =  "shader_depth_vert.spv";
            graphicsConfig.SceneLoad.TextureName = "grid.png";
            graphicsConfig.SceneLoad.Camera.AddCamera(new(0.0f,2.0f,-4.0f), new(0.0f,0.0f,0.0f),new(0.0f,1.0f,0.0f) );
            graphicsConfig.SceneLoad.Camera.AddProjection( RitaEngine.Graphic.CameraProjectionType.PerspectiveFOV,45.0f,(float)1280.0f/720.0f, 0.1f,100.0f);
            graphicsConfig.SceneLoad.Primitive = RitaEngine.Math.GeometricPrimitive.CreateBox(1.0f,1.0f,1.0f);

            RitaEngine.Graphic.GraphicsImplement.Pipelines.Load(ref func,ref data,ref graphicsConfig ); 

            RitaEngine.Graphic.GraphicsImplement.Pipelines.Build(ref func,ref data,ref graphicsConfig  /*, Selected pipeline*/); 
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
            RitaEngine.Graphic.GraphicsImplement.Pipelines.Dispose(ref func, ref data);
            RitaEngine.Graphic.GraphicsImplement.Render.Dispose(ref func, ref data);
            RitaEngine.Graphic.GraphicsImplement.Device.Dispose(ref func, ref data);

            input.Release();
            window.Release();
            clock.Release();
            config.Dispose();
        }
        
    }

}