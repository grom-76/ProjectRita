namespace RitaEngine.Base.Math.Color;
  
public struct Color
{

} //generic color  need Format ? 

/// <summary>
/// Quadrichromie
/// </summary>
public struct CMYK
{
    public float Cyan;
    public float Magenta;
    public float Yellow;
    public float Key;
}

public struct HSL
{
    public float Hue ;
    public float Saturation;// in Percent
    public float Lightness;// in percent
}

public struct HeXColor
{
    //same value as palette 
    public uint Value ;
}

public struct RGB
{
    public byte R;
    public byte G;
    public byte B;
}

public struct RGBA
{
    public byte R;
    public byte G;
    public byte B;
    public byte A;
}
public enum ColorFormat
{

} //see VulkanFormat R8G5B8 ..... binding vulkan ?