namespace RitaEngine.Resources;

public struct ResourceShaderInfo
{
    public string FilenameSPV =string.Empty;
    public string entryPointName = "main";
    public ShaderType Type;

    public int Layout =0;
    public int Uniform=0;
    public int PushConstant;

    public ResourceShaderInfo() { }
}



public enum ShaderType
{
    VertexShader,
    FragmentShader,
    ComputeShader,
    GeometryShader,
    TessControlShader,
    TessEvaluationShader,
}


