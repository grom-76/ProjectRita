namespace RitaSamples;

using RitaEngine.Base;
using RitaEngine.Base.Platform.Config;


public static class Sample_0002
{
    /// <summary>
    /// Just mise en place test
    /// </summary>
    public static void Run()
    {   
       using MyGame game = new();
       game.Run();
   
    }

    public class MyGame : RitaEngine.Advanced.RitaEngineMain
    {
        string path = @"C:\Users\Administrator\Documents\ProjectRita\Assets\";
        RitaEngine.Base.Audio.PlayerSound2D snd = new();
        
        public MyGame()
        {
            RitaEngine.Base.Log.Config(Log.Display.OnConsole);
            RitaEngine.Base.Resources.Shaders. CreateSPIRV(path, "shader_depth.vert","shader_depth_vert.spv", RitaEngine.Base.Resources.Shaders.ShaderType.VertexShader , "main");
            RitaEngine.Base.Resources.Shaders. CreateSPIRV(path, "shader_depth.frag","shader_depth_frag.spv", RitaEngine.Base.Resources.Shaders.ShaderType.FragmentShader , "main");
        }

        protected override void Init()
        {
            Clock.Config.FixedTimeStep = 0.033;
            Clock.Config.LoopMode = RitaEngine.Base.Platform.Config.ClockLoopMode.Default;
            WinConfig.SetTitle("My Game");
            WinConfig.SetResolution( WindowResolution.HD_720p_1920x720);
            Input.Config.ShowCursor = true;
            AudioDevice.Config.Category = AudioCategory.GameMedia;
            AudioDevice.Config.Channels = AudioChannels.stereo;
            GraphicConfig.EnableDebugMode = true;
        }

        protected override void Load()
        {
            snd.Init( AudioDevice ,  path+  "demo.wav" );

            RenderConfig.BackColorARGB = RitaEngine.Base.Math.Color.Palette.BlanchedAlmond;
            RenderConfig.FragmentEntryPoint ="main";
            RenderConfig.VertexEntryPoint ="main";
            RenderConfig.FragmentShaderFileNameSPV = path + "shader_depth_frag.spv";
            RenderConfig.VertexShaderFileNameSPV = path   + "shader_depth_vert.spv";
            RenderConfig.TextureName = path+"wood.png";
            RenderConfig.ubo.Model = RitaEngine.Base.Math.Matrix.Identite;
            RenderConfig.ubo.View =  RitaEngine.Base.Math.Matrix.Identite;//RitaEngine.Base.Math.Matrix.LookAt( new(2.0f,2.0f,2.0f), new(0.0f,0.0f,0.0f), new(0.0f,0.0f,1.0f));
            RenderConfig.ubo.Projection =RitaEngine.Base.Math.Matrix.Identite; //RitaEngine.Base.Math.Matrix.Perspective( 45.0f,1280,720,0.1f,100.0f);
            // RenderConfig.ubo.Projection[1,1] *= -1;
            
        
           GraphicDevice.BuildRender( RenderConfig);
        }

        protected override void Release()
        {
            snd.Dispose();
        }

        protected override void UpdateDraw()
        {
          
        }

        protected override void UpdateInputs()
        {
            if ( Input.IsKeyPressed( RitaEngine.Base.Platform.InputKeys.Escape ))
            {
                Window.RequestClose();
            }

            if (Input.IsKeyPressed( RitaEngine.Base.Platform.InputKeys.Space ))
            {
                snd.PlaySource();
            }
 
        }

        protected override void UpdatePhysics()
        {
           // static auto startTime = std::chrono::high_resolution_clock::now();

        // auto currentTime = std::chrono::high_resolution_clock::now();
        // float time = std::chrono::duration<float, std::chrono::seconds::period>(currentTime - startTime).count();

         
            // GraphicDevice.UpdateRender(RenderConfig);
        }

        protected override void WarmUp()
        {
           
        }
    }
}