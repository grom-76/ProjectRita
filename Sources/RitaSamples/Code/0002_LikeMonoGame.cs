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

            GraphicDevice.Render.BackColorARGB = RitaEngine.Base.Math.Color.Palette.BlanchedAlmond;
            GraphicDevice.Render.FragmentEntryPoint ="main";
            GraphicDevice.Render.VertexEntryPoint ="main";
            GraphicDevice.Render.FragmentShaderFileNameSPV = path + "fragment_base.spv";
            GraphicDevice.Render.VertexShaderFileNameSPV = path + "vertex_base.spv";
            GraphicDevice.Render.TextureName = path+"tex.bmp";
            GraphicDevice.BuildRender();
        }

        protected override void Release()
        {
            snd.Dispose();
        }

        protected override void UpdateDraw()
        {
           GraphicDevice.Draw();
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

        // UniformBufferObject ubo{};
        // ubo.model = glm::rotate(glm::mat4(1.0f), time * glm::radians(90.0f), glm::vec3(0.0f, 0.0f, 1.0f));
        // ubo.view = glm::lookAt(glm::vec3(2.0f, 2.0f, 2.0f), glm::vec3(0.0f, 0.0f, 0.0f), glm::vec3(0.0f, 0.0f, 1.0f));
        // ubo.proj = glm::perspective(glm::radians(45.0f), swapChainExtent.width / (float) swapChainExtent.height, 0.1f, 10.0f);
        // ubo.proj[1][1] *= -1;
        GraphicDevice.Render.ubo.Model = RitaEngine.Base.Math.Matrix.Identite;
        
        GraphicDevice.Update();
        }

        protected override void WarmUp()
        {
           
        }
    }
}