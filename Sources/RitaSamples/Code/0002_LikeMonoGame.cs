namespace RitaSamples;


using RitaEngine.Base;
using RitaEngine.Base.Math;
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
        Vector3 scale =new(1.0f);
        Vector3 eye =new(0.0f,0.0f,5.0f);
        Vector3 target =new(0.0f,0.0f,-1.0f);
        Vector3 up =new(0.0f,1.0f,0.0f);
        float fovDegree = 45.0f;
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
            RenderConfig.ubo.Model =  RitaEngine.Base.Math.Matrix.Identity;
            RenderConfig.ubo.View =  Matrix.LookAtTo(eye,target,up);// Matrix.LookAt( eye,  target ,  up ); 
            RenderConfig.ubo.Projection = Matrix.PerspectiveFOV(45.0f,1280.0f,720.0f,0.1f,100.0f ) ; //Matrix.PerspectiveFov(Helper.ToRadians( fovDegree) ,1.0f,0.1f,100.0f )  ;
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

            if ( Input.IsKeyDown( RitaEngine.Base.Platform.InputKeys.Up ))
            {
                scale.X +=0.1f;scale.Y +=0.1f;
            }
            if ( Input.IsKeyDown( RitaEngine.Base.Platform.InputKeys.Down))
            {
                scale.X -=0.1f;scale.Y -=0.1f;
            }
        }

        protected override void UpdatePhysics()
        {
           
            RenderConfig.ubo.Model= RitaEngine.Base.Math.Matrix.Scaling(scale);
         
            GraphicDevice.UpdateRender(RenderConfig);
        }

        protected override void WarmUp()
        {
           
        }
    }
}