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

        RitaEngine.Base.Audio.PlayerSound2D snd = new();
        Vector3 scale =new(1.0f);
     
        
        public MyGame()
        {
            Config.LogConfig( Log.Display.OnConsole);
            // RitaEngine.Base.Resources.Shaders. CreateSPIRV(path, "shader_depth.vert","shader_depth_vert.spv", RitaEngine.Base.Resources.Shaders.ShaderType.VertexShader , "main");
            // RitaEngine.Base.Resources.Shaders. CreateSPIRV(path, "shader_depth.frag","shader_depth_frag.spv", RitaEngine.Base.Resources.Shaders.ShaderType.FragmentShader , "main");
        }

        protected override void Init()
        {
            Config.AssetPath( @"C:\Users\Administrator\Documents\Repos\ProjectRita\Assets\" );
            Config.Clock_FixedTimeStep = 0.033;
            Config.Clock_LoopMode = RitaEngine.Base.Platform.Config.ClockLoopMode.Default;
            Config.Game_Title ="My First game";
            Config.Window_Resolution = WindowResolution.HD_720p_1920x720;
            Config.Input_ShowCursor = true;
            Config.Audio_Category = AudioCategory.GameMedia;
            Config.Audio_Channels = AudioChannels.stereo;
            Config.GraphicDevice_EnableDebugMode = true;
        }

        protected override void Load()
        {
            snd.Init( AudioDevice ,   "demo.wav" );

            RenderConfig.BackColorARGB = RitaEngine.Base.Math.Color.Palette.BlanchedAlmond;
            RenderConfig.FragmentEntryPoint ="main";
            RenderConfig.VertexEntryPoint ="main";
            RenderConfig.FragmentShaderFileNameSPV =  "shader_depth_frag.spv";
            RenderConfig.VertexShaderFileNameSPV =  "shader_depth_vert.spv";
            RenderConfig.TextureName = "grid.png";
            RenderConfig.Camera.FieldOfViewInDegree = 45.0f;
            RenderConfig.Camera.Eye = new(2.0f,2.0f,2.0f);
            RenderConfig.Primitive = RitaEngine.Base.Math.GeometricPrimitive.CreateCube(1.0f,1.0f,1.0f);

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
                scale.X +=0.1f;scale.Y +=0.1f;scale.Z+=0.1f;
            }
            if ( Input.IsKeyDown( RitaEngine.Base.Platform.InputKeys.Down))
            {
                scale.X -=0.1f;scale.Y -=0.1f;scale.Z-=0.1f;
            }
        }

        protected override void UpdatePhysics()
        {
           
            RenderConfig.Camera.ScalingWorld( scale);
         
            GraphicDevice.UpdateRender(RenderConfig);
        }

        protected override void WarmUp()
        {
           
        }
    }
}