namespace RitaEngine.Graphic;

public static class ShaderHelper{}

public static class ShaderImplement
{
    /*
        Creation d'un shader a partir d'un texte    

        Include clasic Main : =>  void main(){ gl_posiion = MVP * ( layout0 ); }

        Iclude MVP at index = 0   : => layout(binding = 0(index) ) uniform UniformBufferObject {  mat4 model;  mat4 view;  mat4 proj; } ubo;
        ------------------------------------------------------------------------------------------------------------------------------------
        a partir d'un fichier => 
        // RitaEngine.Resources.Shaders. CreateSPIRV(path, "shader_depth.vert","shader_depth_vert.spv", RitaEngine.Resources.Shaders.ShaderType.VertexShader , "main");
        // RitaEngine.Resources.Shaders. CreateSPIRV(path, "shader_depth.frag","shader_depth_frag.spv", RitaEngine.Resources.Shaders.ShaderType.FragmentShader , "main");

        ------------------------------------------------------------------------------------------------------------------------------------
        a partir d'une resource 
        
        ResourceShaderInfo shader  =  CreateShaderWithResource( file, nameOfShader , options );

    
    */
}