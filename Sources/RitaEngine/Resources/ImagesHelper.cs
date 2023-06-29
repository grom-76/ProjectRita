namespace RitaEngine.Resources;


using System;
using System.IO;
using RitaEngine.Resources.Images;


public static class ImagesHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileIn"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static byte[] ReadImage( string fileIn, out int width , out int height, int color )
    {
        try
        {
            if (  string.IsNullOrEmpty(fileIn  )  )
                throw new Exception("filename in not be null ");
            if ( !System.IO.File.Exists(fileIn))
                throw new Exception("filename in not exist ");
            
            var filedata = System.IO.File.ReadAllBytes( fileIn );
            
            ImageResult result = ImageResult.FromMemory(filedata );
            filedata = null;

            width = result.Width;
            height = result.Height;
            color = (int)result.SourceComp;
            
            return result.Data!;
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Read image : " + ex.Message);
            throw;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="output"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="color"></param>
    public static void WritePNGImage(ref byte[] data,string output , int width, int height, int color)
    {
        using Stream stream = File.OpenWrite(output);

        ImageWriter writer = new ImageWriter();
        writer.WritePng(data, width, height, ColorComponents.RedGreenBlueAlpha, stream);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="comp"></param>
    /// <param name="newWidth"></param>
    /// <param name="newHeight"></param>
    /// <returns></returns>
    public static byte[] Resize(byte[] data , int width, int height, int comp ,int newWidth, int newHeight  )
    {
        // Retrieve amount of channels in the image(from 1 to 4)
        int channels = comp;//(int)image.Comp;

        byte[] newImageData = new byte[newWidth * newHeight * channels];
        StbImageResize.stbir_resize_uint8(data, width, height, width * channels,
            newImageData, newWidth, newHeight, newWidth * channels, channels);
        
        return newImageData;
    }
}

public static class ImagesFormatEncoderDecoder
{
    //CONVERTER 
//internal methods qoi bmp ... revisité STB ??????????
} 
   