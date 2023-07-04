
namespace RitaEngine.Resources;

using System.IO;
using RitaEngine.Resources.Shader.ShaderCompiler;
using RitaEngine.Resources.Shader.SpirvCross;

public static  class Shaders
{
    public static void CreateSPIRV( string input, string output, ShaderType shadertype, string entrypoint)
    {
       
        string shaderSourceFile = Path.Combine(RitaEngine.Platform.PlatformHelper.AssetsPath, input );
        if( !File.Exists(shaderSourceFile))
        {
            throw new Exception("Error file not found : " + shaderSourceFile);
        }

        string shaderSource = File.ReadAllText(shaderSourceFile);

        using RitaEngine.Resources.Shader.ShaderCompiler.Compiler compiler = new (RitaEngine.Platform.PlatformHelper.AssetsPath);

        using var result = compiler.Compile(shaderSource, shaderSourceFile, (ShaderKind)shadertype, entrypoint);
                
        if( result.Status !=CompilationStatus.Success)
        {
            throw new Exception("Error compila shader : "+ result.ErrorMessage);
        }

        var shaderCode = result.GetBytecode().ToArray();

        File.WriteAllBytes(Path.Combine(RitaEngine.Platform.PlatformHelper.AssetsPath,output), shaderCode) ;
       
    }

    
    /*Exemple vertex buffer outside shader  => https://github.com/Overv/VulkanTutorial/blob/main/code/18_shader_vertexbuffer.vert*/



     public  static byte[] GetBytecode(string path, string name)
    {
        return File.ReadAllBytes(Path.Combine(path, $"{name}.spv"));
    }
    
    public static void ShaderCrossCompiler(string path,string filename,  Backend format)
    {
        //TODO fiare list CompilerOptions OPtions { option, value } => code : foreachOptions Setuint/SetBool 
        try
        {
            byte[] vertexBytecode = GetBytecode(path,filename);
            using RitaEngine.Resources.Shader.SpirvCross.Context context = new();
            context.ParseSpirv(vertexBytecode, out SpvcParsedIr parsedIr).CheckResult();
            RitaEngine.Resources.Shader.SpirvCross.Compiler compiler = context.CreateCompiler(format, parsedIr, CaptureMode.TakeOwnership);
            // compiler.Options.SetUInt(CompilerOption.GLSL_Version, 330);
            // compiler.Options.SetBool(CompilerOption.GLSL_ES, false);
            // compiler.Options.SetUInt(CompilerOption.HLSLShaderModel, 50);
           
            compiler.Apply();

            string hlsl = compiler.Compile();
        // Assert.IsNotEmpty(hlsl);
        // Assert.IsEmpty(context.GetLastErrorString());
        }
        catch (System.Exception)
        {
            
            throw;
        }
        finally
        {

        }
    }





    public static string shader_vertexbuf_vert = @"
        #version 450

        layout(location = 0) in vec3 fragColor;

        layout(location = 0) out vec4 outColor;

        void main() {
            outColor = vec4(fragColor, 1.0);
        }
    ";
    public static string shader_vertexbuf_frag =@"
        #version 450

        layout(location = 0) in vec2 inPosition;
        layout(location = 1) in vec3 inColor;

        layout(location = 0) out vec3 fragColor;

        void main() {
            gl_Position = vec4(inPosition, 0.0, 1.0);
            fragColor = inColor;
        }
    ";
}


