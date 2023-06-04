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
        //   RitaEngine.Base.Audio.PlayerSound2D snd = new( audio, path+  "demo.wav");
        public MyGame()
        {
            RitaEngine.Base.Log.Config(Log.Display.OnConsole);
        
        }

        protected override void Init()
        {
            Clock.Config.FixedTimeStep = 0.033;
            Clock.Config.LoopMode = RitaEngine.Base.Platform.Config.ClockLoopMode.Default;
            Window.Config.SetTitle("My Game");
            Window.Config.SetResolution( WindowResolution.HD_720p_1920x720);
            Input.Config.ShowCursor = true;
            AudioDevice.Config.Category = AudioCategory.GameMedia;
            AudioDevice.Config.Channels = AudioChannels.stereo;
            GraphicDevice.Config.EnableDebugMode = true;
        }

        protected override void Load()
        {
            snd.Init( AudioDevice ,  path+  "demo.wav" );

            GraphicDevice.Render.BackColorARGB = RitaEngine.Base.Math.Color.Palette.Lavender ;
            GraphicDevice.Render.FragmentEntryPoint ="main";
            GraphicDevice.Render.VertexEntryPoint ="main";
            GraphicDevice.Render.FragmentShaderFileNameSPV = path + "fragment_base.spv";
            GraphicDevice.Render.VertexShaderFileNameSPV = path + "vertex_base.spv";

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
           
        }

        protected override void WarmUp()
        {
           
        }
    }
}