namespace RitaSamples;

using RitaEngine.Audio;
using RitaEngine.Base;
using RitaEngine.Input;
using RitaEngine.Platform;


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
    

    public class MyGame : RitaEngine.RitaEngineMain
    {

        RitaEngine.Audio.PlayerSound2D snd = new();
   
        
        public MyGame()
        {
            Config.Log( Log.Display.OnConsole);
            Config.AssetPath( @"C:\Users\Administrator\Documents\Repos\ProjectRita\Assets\" );
            Config.Clock_FixedTimeStep = 0.033;
            Config.Clock_LoopMode = RitaEngine.Platform.ClockLoopMode.Default;
            Config.Game_Title ="My First game";
            Config.Window_Resolution = WindowResolution.HD_720p_1920x720;
            Config.Input_ShowCursor = true;
            Config.Audio_Category = AudioCategory.GameMedia;
            Config.Audio_Channels = AudioChannels.stereo;
            Config.GraphicDevice_EnableDebugMode = true;
            // RitaEngine.Resources.Shaders. CreateSPIRV(path, "shader_depth.vert","shader_depth_vert.spv", RitaEngine.Resources.Shaders.ShaderType.VertexShader , "main");
            // RitaEngine.Resources.Shaders. CreateSPIRV(path, "shader_depth.frag","shader_depth_frag.spv", RitaEngine.Resources.Shaders.ShaderType.FragmentShader , "main");
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

            RenderConfig.BackColorARGB = RitaEngine.Math.Color.Palette.Lavender;
            RenderConfig.FragmentEntryPoint ="main";
            RenderConfig.VertexEntryPoint ="main";
            RenderConfig.FragmentShaderFileNameSPV =  "shader_depth_frag.spv";
            RenderConfig.VertexShaderFileNameSPV =  "shader_depth_vert.spv";
            RenderConfig.TextureName = "grid.png";
            // RenderConfig.Camera.AddLookAkCamera(new(0.0f,2f,-4.0f), new(-27.0f,0.0f, 0.0f),new(0.0f,1.0f,0.0f),45.0f,(float)1280.0f/720.0f, 0.1f,100.0f );
            // RenderConfig.Camera.AddFirstPersonCamera(new(0.0f,2f,-4.0f), new(0.0f,0.0f,0.0f),new(0.0f,1.0f,0.0f),45.0f,(float)1280.0f/720.0f, 0.1f,100.0f );
            RenderConfig.Camera.AddCamera(new(0.0f,2f,-4.0f), new(0.0f,0.0f,0.0f),new(0.0f,1.0f,0.0f),45.0f,(float)1280.0f/720.0f, 0.1f,100.0f );
            RenderConfig.Primitive = RitaEngine.Math.GeometricPrimitive.CreateBox(1.0f,1.0f,2.0f);

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

            if ( Input.IsKeyDown( InputKeys.Left )&& Input.IsKeyUp( InputKeys.LeftControl)) {
                RenderConfig.Camera.RotateLookAt( .0f,1.0f,0.0f);
            }
            if ( Input.IsKeyDown( InputKeys.Right)&& Input.IsKeyUp( InputKeys.LeftControl)) {
                RenderConfig.Camera.RotateLookAt( 0.0f ,-1.0f,0.0f);
            }
            if ( Input.IsKeyDown( InputKeys.Up )&& Input.IsKeyUp( InputKeys.LeftControl)) {
                RenderConfig.Camera.RotateLookAt( .0f,0.0f,1.0f);
            }
            if ( Input.IsKeyDown( InputKeys.Down)&& Input.IsKeyUp( InputKeys.LeftControl)) {
                RenderConfig.Camera.RotateLookAt( 0.0f ,0.0f,-1.0f);
            }
            if ( Input.IsKeyDown( InputKeys.P)&& Input.IsKeyUp( InputKeys.LeftControl)) {
                RenderConfig.Camera.RotateLookAt( 1.0f ,0.0f,0.0f);
            }
            if ( Input.IsKeyDown( InputKeys.O) && Input.IsKeyUp( InputKeys.LeftControl)) {   
                RenderConfig.Camera.RotateLookAt( -1.0f ,0.0f,0.0f);
            }

            if ( Input.IsKeyDown( InputKeys.Left) && Input.IsKeyDown( InputKeys.LeftControl)) {
                RenderConfig.Camera.TranslateLookAt( 0.10f,0.0f,0.0f);
            }
            if ( Input.IsKeyDown( InputKeys.Right) && Input.IsKeyDown( InputKeys.LeftControl)) {
                RenderConfig.Camera.TranslateLookAt( -0.10f,0.0f,0.0f);
            }
            if ( Input.IsKeyDown( InputKeys.Up) && Input.IsKeyDown( InputKeys.LeftControl)) {
                RenderConfig.Camera.TranslateLookAt( 0.0f,0.10f,0.0f);
            }
            if ( Input.IsKeyDown( InputKeys.Down) && Input.IsKeyDown( InputKeys.LeftControl)) {
                RenderConfig.Camera.TranslateLookAt( 0.0f,-0.10f,0.0f);
            }
            if ( Input.IsKeyDown( InputKeys.P) && Input.IsKeyDown( InputKeys.LeftControl)) {
                RenderConfig.Camera.TranslateLookAt( 0.0f,0.0f,0.10f);
            }
              if ( Input.IsKeyDown( InputKeys.O) && Input.IsKeyDown( InputKeys.LeftControl)) {
                RenderConfig.Camera.TranslateLookAt( 0.0f,0.0f,-0.10f);
            }


            if ( Input.IsKeyDown( InputKeys.Q)&& Input.IsKeyUp( InputKeys.LeftControl)) {
                RenderConfig.Camera.Pitch( 1.0f);
            }
            if ( Input.IsKeyDown( InputKeys.D)&& Input.IsKeyUp( InputKeys.LeftControl)) {
                RenderConfig.Camera.Pitch( -1.0f);
            }
            if ( Input.IsKeyDown( InputKeys.Z)&& Input.IsKeyUp( InputKeys.LeftControl)) {
                RenderConfig.Camera.Yaw( 1.0f);
            }
            if ( Input.IsKeyDown( InputKeys.S)&& Input.IsKeyUp( InputKeys.LeftControl)) {
                RenderConfig.Camera.Yaw( -1.0f);
            }
            if ( Input.IsKeyDown( InputKeys.R)&& Input.IsKeyUp( InputKeys.LeftControl)) {
                RenderConfig.Camera.Roll( 1.0f);
            }
            if ( Input.IsKeyDown( InputKeys.F)&& Input.IsKeyUp( InputKeys.LeftControl)) {
                RenderConfig.Camera.Roll( -1.0f);
            }
            
            if ( Input.IsKeyDown( InputKeys.Q)&& Input.IsKeyDown( InputKeys.LeftControl)) {
                RenderConfig.Camera.Strafe( -0.10f);
            }
            if ( Input.IsKeyDown( InputKeys.D)&& Input.IsKeyDown( InputKeys.LeftControl)) {
                RenderConfig.Camera.Strafe( 0.10f);
            }
            if ( Input.IsKeyDown( InputKeys.Z)&& Input.IsKeyDown( InputKeys.LeftControl)) {
                RenderConfig.Camera.Ascend( 0.10f);
            }
            if ( Input.IsKeyDown( InputKeys.S)&& Input.IsKeyDown( InputKeys.LeftControl)) {
                RenderConfig.Camera.Ascend( -0.10f);
            }
            if ( Input.IsKeyDown( InputKeys.R)&& Input.IsKeyDown( InputKeys.LeftControl)) {
                RenderConfig.Camera.Advance( 0.10f);
            }
            if ( Input.IsKeyDown( InputKeys.F)&& Input.IsKeyDown( InputKeys.LeftControl)) {
                RenderConfig.Camera.Advance( -0.1f);
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