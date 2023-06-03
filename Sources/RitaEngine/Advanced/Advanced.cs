





//---------------------------------------------------------------------------------------------------------------------------------------------

namespace RitaEngine.Advanced
{
    namespace IA
    {
        public static class PathFinding{}
        public static class FinitStateMachine{}
    }
    
    namespace Physic
    {
        public class Physics{} //see :https://github.com/bepu/bepuphysics2
    }
    namespace Scene
    {
        public struct Scene{} //Manage block
        
        public struct Containers{} // node for godot = Components ECS .... 
        public struct Block{} //Entities 
        
        
        public class Animation{}
       
        public class Particules
        {
            //FROM CPU
            //FROM GPU
        }
        public class Terrain{}
        public class Sprites{ } //equivelent spritesbatch 
        public class SkyBoxes{} //
        public class BAckGrounds{}
        public class Fonts{} // https://github.com/myblindy/MB.Pengu.Engine/blob/master/Pengu.Engine/Renderer/VulkanContext.Font.cs
        public class GUI{}
        public class HID{}//controller human interface device
       
        public class Media {} //play video   ,  enregistre sequence video 

        public class Camera{
            //see: https://github.com/RedpointGames/Protogame/tree/master/Protogame/Camera
        } // rotote aound ,  follow , fixed , move , 3D-2D ,projection
        public enum CameraType{}


        public static class Layers{} // see : https://github.com/Syncaidius/MoltenEngine/tree/master/Molten.Engine/Scene
    }

    namespace Events
    {
        public class Scripting{}
        public class Timers{} //gestion des evenements timer ( delay , wait , ...)
        public class Triggers{} //ECS  see : 
    }
   
    
    // public class RitaEngineSystem or COntext {} // like raylib
   
    namespace Multiplayer
    {
        public class Online{}//multiplayer
        public class Server{}
        public class Client {}
    }

    namespace AudioRender
    {
        public struct DSP{}
        public struct AudioEngine{} //DSP manag listener sources synchro with game
        public struct EffectsManager{}// Echo rever delay ...
    }

    namespace GraphicRender
    {
        namespace Light
        {
            public struct Light{}
            public struct Reflection{}
            public struct ColorGradient{}
            public struct LightMap{}
            public struct LightPass{}
            public struct Illuminations{} //Global Dynamic
        }
        public struct DepthOfField{}
        public struct BillBoard{}
        public struct Blur{}
        public struct Shadow{}
        public struct PostRendering{}
        public struct PreRendering{}
        public struct AmbientOcclusion{}
        public struct Volumetric{}
        public struct Atmosphere{}

        public struct DeferredRendering{}
        public struct DeferredShadowing{}

        public struct AntiAliasing_FXAA{} //https://github.com/FlaxEngine/FlaxEngine/tree/master/Source/Engine/Renderer/AntiAliasing
        public struct AntiAliasing_SMAA{}
        public struct AntiAliasing_TAA{}

        public struct RenderPrimitive{}
        public struct RenderMesh{}

        // SOURCE : https://github.com/turanszkij/WickedEngine/blob/master/features.txt
        // Vulkan renderer
        // Image rendering
        // Font rendering (True Type)
        // Networking (UDP)
        // 3D mesh rendering
        // Skeletal animation
        // Morph target animation (with sparse accessor)
        // Physically based rendering
        // Animated texturing
        // Normal mapping
        // Displacement mapping
        // Parallax occlusion mapping
        // Real time planar reflections
        // Cube map reflections (static and real time)
        // Refractions (screen space, blurred)
        // Interactive Water
        // Bloom
        // Edge outline
        // Motion Blur
        // Lens Flare
        // Light shafts
        // Bokeh Depth of Field
        // Chromatic aberration
        // Multithreaded rendering
        // Tessellation (silhouette smoothing, displacement mapping)
        // GPU-based particles (emit from point, mesh, animated mesh)
        // Soft particles
        // Hair particle systems (grass/vegetation)
        // Instanced rendering
        // MSAA
        // FXAA
        // TAA (Temporal Antialiasing)
        // Supersampling
        // Directional lights + cascaded shadow maps
        // Spotlights + shadow maps
        // Point lights + shadow cubemaps
        // Soft shadows (PCF)
        // BULLET Physics: rigid body, soft body
        // 3D Audio (Xaudio2)
        // Input: keyboard, mouse, controller (rawinput, xinput), touch
        // Controller feedback (vibration, LED)
        // Backlog: log,input,scripting
        // Gamma correct, HDR rendering
        // Resource Manager
        // Screen Space Ambient Occlusion (SSAO, HBAO, MSAO)
        // Stochastic Screen Space Reflections
        // Color Grading
        // Sharpen filter
        // Eye adaption
        // Lua Scripting
        // Dynamic environment mapping
        // Impostor system
        // Tiled forward (Forward+) rendering (+2.5D culling)
        // Occlusion culling with gpu queries
        // Texture atlas packing
        // Tiled decals
        // Frame Profiler
        // Voxel Global Illumination
        // Reversed Z-buffer
        // Force Fields GPU simulation
        // Particle - Depth Buffer collisions
        // Ocean simulation (FFT)
        // Translucent colored shadows
        // Refraction caustics
        // Local parallax-corrected environment maps
        // Volumetric light scattering
        // Smooth Particle Hydrodynamics (SPH) Fluid Simulation
        // Ray tracing, path tracing (on GPU)
        // Entity-Component System (Data oriented design)
        // Lightmap baking (with GPU path tracing)
        // Job system
        // Inverse Kinematics
        // Springs, Colliders
        // Variable Rate Shading
        // Real time ray tracing: ambient occlusion, shadows, reflections (DXR and Vulkan raytracing)
        // Screen Space Contact Shadows
        // Stochastic alphatest transparency
        // Surfel GI
        // HDR display output
        // Dynamic Diffuse Global Illumination (DDGI)
        // Procedural terrain generator
        // Expressions
        // Humanoid rig
        // Animation retargeting
    }
}
