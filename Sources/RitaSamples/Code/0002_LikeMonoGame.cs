namespace RitaSamples;


using RitaEngine.Base;
using RitaEngine.Base.Math;
using RitaEngine.Base.Platform;
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
   
        
        public MyGame()
        {
            Config.Log( Log.Display.OnConsole);
            Config.AssetPath( @"C:\Users\Administrator\Documents\Repos\ProjectRita\Assets\" );
            Config.Clock_FixedTimeStep = 0.033;
            Config.Clock_LoopMode = RitaEngine.Base.Platform.Config.ClockLoopMode.Default;
            Config.Game_Title ="My First game";
            Config.Window_Resolution = WindowResolution.HD_720p_1920x720;
            Config.Input_ShowCursor = true;
            Config.Audio_Category = AudioCategory.GameMedia;
            Config.Audio_Channels = AudioChannels.stereo;
            Config.GraphicDevice_EnableDebugMode = true;
            // RitaEngine.Base.Resources.Shaders. CreateSPIRV(path, "shader_depth.vert","shader_depth_vert.spv", RitaEngine.Base.Resources.Shaders.ShaderType.VertexShader , "main");
            // RitaEngine.Base.Resources.Shaders. CreateSPIRV(path, "shader_depth.frag","shader_depth_frag.spv", RitaEngine.Base.Resources.Shaders.ShaderType.FragmentShader , "main");
            Window.Events.OnKillFocus = onkillFocus;
            Window.Events.OnSetFocus = onSetFocus;
        }

        private void onSetFocus(nuint wParam, nint lParam)
        {
            Log.Info("SETTTT FOCUUSSSS");
        }

        private void onkillFocus(nuint wParam, nint lParam)
        {
            Log.Info("KILLL FOCUUSSSS");
        }

        protected override void Init()
        {
          
        }

        protected override void Load()
        {
            snd.Init( AudioDevice ,   "demo.wav" );

            RenderConfig.BackColorARGB = RitaEngine.Base.Math.Color.Palette.Lavender;
            RenderConfig.FragmentEntryPoint ="main";
            RenderConfig.VertexEntryPoint ="main";
            RenderConfig.FragmentShaderFileNameSPV =  "shader_depth_frag.spv";
            RenderConfig.VertexShaderFileNameSPV =  "shader_depth_vert.spv";
            RenderConfig.TextureName = "grid.png";
            RenderConfig.Camera.AddLookAkCamera(new(0.0f,-0.10f,-4.0f), new(0.0f,15.0f, 00.0f),new(0.0f,1.0f,0.0f),45.0f,(float)1280.0f/720.0f, 0.1f,100.0f );
            // RenderConfig.Camera.AddFirstPersonCamera(new(0.0f,-0.1f,-4.0f), new(0.0f,0.0f,0.0f),new(0.0f,1.0f,0.0f),45.0f,(float)1280.0f/720.0f, 0.1f,100.0f );
            RenderConfig.Primitive = RitaEngine.Base.Math.GeometricPrimitive.CreateCube(1.0f,1.0f,2.0f);

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
            if ( Input.IsKeyPressed(InputKeys.Escape ))
            {
                Window.RequestClose();
            }

            if (Input.IsKeyPressed(InputKeys.Space ))
            {
                snd.PlaySource();
            }

            if ( Input.IsKeyPressed( InputKeys.Left ))
            {
                RenderConfig.Camera.TranslateLookAt( 0.1f,0.0f,0.0f);
            }
            if ( Input.IsKeyPressed( InputKeys.Right))
            {
                RenderConfig.Camera.TranslateLookAt( -0.1f ,0.0f,0.0f);
            }

            if ( Input.IsKeyPressed( InputKeys.P))
            {
                RenderConfig.Camera.PoorZoom( +1.0f);
            }
            if ( Input.IsKeyPressed( InputKeys.O))
            {   
                RenderConfig.Camera.PoorZoom( -1.0f);
            }

            if ( Input.IsKeyPressed( InputKeys.Q))
            {
                RenderConfig.Camera.Strafe( 0.1f);
            }
            if ( Input.IsKeyPressed( InputKeys.D))
            {
                RenderConfig.Camera.Strafe( -0.1f);
            }

            
        }

        protected override void UpdatePhysics()
        {

            RenderConfig.Camera.Update(Clock.DeltaTime);
            GraphicDevice.UpdateRender(RenderConfig);
        }

        protected override void WarmUp()
        {
           
        }
    }
}