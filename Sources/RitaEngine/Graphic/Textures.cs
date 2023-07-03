namespace RitaEngine.Graphic;

using System.IO;
using RitaEngine.API.Vulkan;
using RitaEngine.Base;
using RitaEngine.Base.Debug;
using RitaEngine.Resources.Images;

public struct Textures
{
    
}


public static class TexturesImplement
{
    public static void Load( ref TextureData textureData  )
    {

    }
    public static void Release( ref TextureData textureData  )
    {
        
    }

    private static void SetImageLayout(ref TextureData textureData )
    {

    }


#region SAMPLER NEED TO DESCRIPTOR SET
    public unsafe static void CreateTextureSampler(ref GraphicDeviceFunctions  func, ref GraphicDeviceData data)
    {
        VkSamplerCreateInfo samplerInfo = new();
        samplerInfo.sType =  VkStructureType.VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO;
        samplerInfo.magFilter = VkFilter.VK_FILTER_LINEAR;
        samplerInfo.minFilter = VkFilter.VK_FILTER_LINEAR;
        samplerInfo.addressModeU = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT;
        samplerInfo.addressModeV = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT;
        samplerInfo.addressModeW = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT;
        samplerInfo.anisotropyEnable = VK.VK_TRUE;
        samplerInfo.maxAnisotropy = data.Info.PhysicalDeviceProperties.limits.maxSamplerAnisotropy;
        samplerInfo.borderColor = VkBorderColor . VK_BORDER_COLOR_INT_OPAQUE_BLACK;
        samplerInfo.unnormalizedCoordinates = VK.VK_FALSE;
        samplerInfo.compareEnable = VK.VK_FALSE;
        samplerInfo.compareOp = VkCompareOp.VK_COMPARE_OP_ALWAYS;
        samplerInfo.mipmapMode = VkSamplerMipmapMode .VK_SAMPLER_MIPMAP_MODE_LINEAR;

        fixed(VkSampler* sampler  = &data.Info.TextureSampler)
        {
            func.vkCreateSampler(data.Handles.Device, &samplerInfo, null, sampler).Check("failed to create texture sampler!");
        }   
        Log.Info($"Create Texture sampler {data.Info.TextureSampler}");
    }

    public unsafe static void DisposeTextureSampler(in GraphicDeviceFunctions  func, ref GraphicDeviceData data)
    {
        if( !data.Info.TextureSampler.IsNull)
        {
            Log.Info($"Destroy Texture sampler {data.Info.TextureSampler}");
            func.vkDestroySampler(data.Handles.Device,data.Info.TextureSampler, null);
        }
      
    }
#endregion

 #region Texture

    public unsafe static void CreateTextureImage(ref GraphicDeviceFunctions func,ref GraphicDeviceData data )
    {
        VkFormat format = VkFormat.VK_FORMAT_UNDEFINED;
        uint texWidth=512, texHeight=512;
        uint texChannels=4;
        //TODO CreateTextureImage  File.ReadAllBytes( data.Info.TextureName) do this outside 
        var file = File.ReadAllBytes( data.Info.TextureName);
        // StbImage.stbi__vertically_flip_on_load_set = 1;
        ImageResult result = ImageResult.FromMemory(file , ColorComponents.RedGreenBlueAlpha);
        texWidth = (uint)result.Width;
        texHeight = (uint)result.Height;
        texChannels = (uint)result.Comp;

        ulong imageSize = (ulong)(texWidth * texHeight * texChannels);

        VkBuffer stagingBuffer = VkBuffer.Null;
        VkDeviceMemory stagingBufferMemory = VkDeviceMemory.Null;

        // CreateStagingBuffer(ref func , ref data , imageSize, 
        //     VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_SRC_BIT, 
        //     VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | 
        //     VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, 
        //     ref stagingBuffer, 
        //     ref stagingBufferMemory);

        byte* imgPtr = null;
        func.vkMapMemory(data.Handles.Device, stagingBufferMemory, 0, imageSize, 0, (void**)&imgPtr).Check("Impossible to map memory for texture");
       
            // void* ptr = Unsafe.AsPointer( ref  result.Data[0]);
        fixed( byte* tex2D = &result.Data[0])
        {
            Unsafe.CopyBlock(imgPtr  ,tex2D ,(uint)imageSize);
        }

          
        func.vkUnmapMemory(data.Handles.Device, stagingBufferMemory);

        if (result.Comp == ColorComponents.RedGreenBlue  )
            format = VkFormat.VK_FORMAT_R8G8B8_SRGB;
        else if (result.Comp == ColorComponents.RedGreenBlueAlpha )    
            format = VkFormat.VK_FORMAT_R8G8B8A8_SRGB;

        CreateImage(ref func , ref data, ref data.Info.TextureImage, ref data.Info.TextureImageMemory, texWidth, texHeight, 
            format,//VkFormat.VK_FORMAT_R8G8B8A8_SRGB, 
            VkImageTiling.VK_IMAGE_TILING_OPTIMAL, 
            VkImageUsageFlagBits.VK_IMAGE_USAGE_TRANSFER_DST_BIT | VkImageUsageFlagBits.VK_IMAGE_USAGE_SAMPLED_BIT, 
            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT   );

        TransitionImageLayout(ref func , ref data, format, VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);
            CopyBufferToImage(ref  func,ref  data,stagingBuffer, (texWidth), (texHeight));
        TransitionImageLayout(ref func , ref data,format, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);

        func.vkDestroyBuffer(data.Handles.Device, stagingBuffer, null);
        func.vkFreeMemory(data.Handles.Device, stagingBufferMemory, null);
        
        file = null!;
        result.Data = null!;
        
        Log.Info($"Create Texture Image {data.Info.TextureImage} ");
        Log.Info($"Create Texture Image Memory {data.Info.TextureImageMemory} ");
    }

    public unsafe static void CreateTextureImageView(ref GraphicDeviceFunctions  func, ref GraphicDeviceData data)
    {
        VkImageViewCreateInfo viewInfo = new();
        viewInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO;
        viewInfo.image = data.Info.TextureImage;
        viewInfo.viewType = VkImageViewType. VK_IMAGE_VIEW_TYPE_2D;
        viewInfo.format = VkFormat. VK_FORMAT_R8G8B8A8_SRGB;
        viewInfo.subresourceRange.aspectMask =  (uint)VkImageAspectFlagBits. VK_IMAGE_ASPECT_COLOR_BIT;
        viewInfo.subresourceRange.baseMipLevel = 0;
        viewInfo.subresourceRange.levelCount = 1;
        viewInfo.subresourceRange.baseArrayLayer = 0;
        viewInfo.subresourceRange.layerCount = 1;

        fixed (VkImageView* imageView = &data.Info.TextureImageView )
        {
            func.vkCreateImageView(data.Handles.Device, &viewInfo, null, imageView).Check("failed to create image view!");
        }
        Log.Info($"Create Texture Image View {data.Info.TextureImageView}");
    }

    public unsafe static void DisposeTextureImage(in GraphicDeviceFunctions  func, ref GraphicDeviceData data)
    {
        if( !data.Info.TextureImage.IsNull)
        {
            Log.Info($"Create Texture Image {data.Info.TextureImage} ");
            func.vkDestroyImage(data.Handles.Device, data.Info.TextureImage, null);
        }
        if( !data.Info.TextureImageMemory.IsNull)
        {
            Log.Info($"Create Texture Image Memory {data.Info.TextureImageMemory} ");
            func.vkFreeMemory(data.Handles.Device, data.Info.TextureImageMemory, null);
        }
        if( !data.Info.TextureImageView.IsNull)
        {
            Log.Info($"Destroy Texture Image View {data.Info.TextureImageView}");
            func.vkDestroyImageView(data.Handles.Device, data.Info.TextureImageView, null);
        }
    }
    
    private unsafe static void CreateImage(ref GraphicDeviceFunctions func,ref GraphicDeviceData data,
        ref VkImage image,ref VkDeviceMemory imageMemory,  uint width, uint height, VkFormat format, VkImageTiling tiling,
        VkImageUsageFlagBits usage, VkMemoryPropertyFlagBits properties) 
    {
        VkImageCreateInfo imageInfo = new();
        imageInfo.sType =VkStructureType.VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO;
        imageInfo.imageType =VkImageType. VK_IMAGE_TYPE_2D;
        
        imageInfo.extent.width = width;
        imageInfo.extent.height = height;
        imageInfo.extent.depth = 1;
        imageInfo.mipLevels = 1;
        imageInfo.arrayLayers = 1;
        imageInfo.format = format;
        imageInfo.tiling = tiling;
        imageInfo.initialLayout =VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
        imageInfo.usage = (uint)usage;
        imageInfo.samples =VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;
        imageInfo.sharingMode = VkSharingMode. VK_SHARING_MODE_EXCLUSIVE;
        imageInfo.pNext = null;

        fixed( VkImage* img = &image)
        {
            func.vkCreateImage(data.Handles.Device, &imageInfo, null,img).Check("failed to create image!");
        }
        
        VkMemoryRequirements memRequirements;
        func.vkGetImageMemoryRequirements(data.Handles.Device, image, &memRequirements);

        VkMemoryAllocateInfo allocInfo = new();
        allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO;
        allocInfo.allocationSize = memRequirements.size;
        // allocInfo.memoryTypeIndex = FindMemoryType(ref func , ref data, memRequirements.memoryTypeBits, properties);

        fixed (VkDeviceMemory* imgMem = &imageMemory  )
        {
            func.vkAllocateMemory(data.Handles.Device, &allocInfo, null, imgMem).Check("failed to allocate image memory!");
        }

        func.vkBindImageMemory( data.Handles.Device, image,imageMemory, 0).Check("Bind Image Memory");
    }

    private unsafe static void TransitionImageLayout(ref GraphicDeviceFunctions  func,ref GraphicDeviceData data, VkFormat format, VkImageLayout oldLayout, VkImageLayout newLayout) 
    {
        VkCommandBuffer commandBuffer = BeginSingleTimeCommands(ref func , ref data);

        VkImageMemoryBarrier barrier = new();
        barrier.sType =VkStructureType. VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER;
        barrier.oldLayout = oldLayout;
        barrier.newLayout = newLayout;
        barrier.srcQueueFamilyIndex = VK.VK_QUEUE_FAMILY_IGNORED;
        barrier.dstQueueFamilyIndex = VK.VK_QUEUE_FAMILY_IGNORED;
        barrier.image = data.Info.TextureImage;
        barrier.subresourceRange.aspectMask =(uint) VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT;
        barrier.subresourceRange.baseMipLevel = 0;
        barrier.subresourceRange.levelCount = 1;
        barrier.subresourceRange.baseArrayLayer = 0;
        barrier.subresourceRange.layerCount = 1;

        VkPipelineStageFlagBits sourceStage=VkPipelineStageFlagBits. VK_PIPELINE_STAGE_TOP_OF_PIPE_BIT;
        VkPipelineStageFlagBits destinationStage = VkPipelineStageFlagBits .VK_PIPELINE_STAGE_TRANSFER_BIT;

        if (oldLayout == VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED && newLayout == VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL)
        {
            barrier.srcAccessMask = 0;
            barrier.dstAccessMask = (uint)VkAccessFlagBits.VK_ACCESS_TRANSFER_WRITE_BIT;

            sourceStage = VkPipelineStageFlagBits. VK_PIPELINE_STAGE_TOP_OF_PIPE_BIT;
            destinationStage = VkPipelineStageFlagBits .VK_PIPELINE_STAGE_TRANSFER_BIT;
        } 
        else if (oldLayout == VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL && newLayout ==VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL) 
        {
            barrier.srcAccessMask = (uint)VkAccessFlagBits.VK_ACCESS_TRANSFER_WRITE_BIT;
            barrier.dstAccessMask = (uint)VkAccessFlagBits.VK_ACCESS_SHADER_READ_BIT;

            sourceStage = VkPipelineStageFlagBits .VK_PIPELINE_STAGE_TRANSFER_BIT;
            destinationStage = VkPipelineStageFlagBits .VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT;
        } else 
        {
            Guard.ThrowWhenConditionIsTrue(true,"unsupported layout transition!");
        }

        func.vkCmdPipelineBarrier( commandBuffer,
            (uint)sourceStage, (uint)destinationStage,
            0, 0, null, 0, null, 1, &barrier   );

        EndSingleTimeCommands(ref func , ref data ,commandBuffer);
    }

    private unsafe static void CopyBufferToImage(ref GraphicDeviceFunctions  func,ref GraphicDeviceData data,VkBuffer buffer,  uint width, uint height) 
    {
        VkCommandBuffer commandBuffer = BeginSingleTimeCommands(ref func , ref data);

        VkBufferImageCopy region = new();
        region.bufferOffset = 0;
        region.bufferRowLength = 0;
        region.bufferImageHeight = 0;
        region.imageSubresource.aspectMask = (uint)VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT;
        region.imageSubresource.mipLevel = 0;
        region.imageSubresource.baseArrayLayer = 0;
        region.imageSubresource.layerCount = 1;
        
        VkOffset3D offset3D = new(); offset3D.x=0;offset3D.y=0;offset3D.z =0;
        region.imageOffset = offset3D;
        VkExtent3D extent3D = new(); extent3D.width = width; extent3D.height = height; extent3D.depth =1;
        region.imageExtent = extent3D;

        func.vkCmdCopyBufferToImage(commandBuffer, buffer, data.Info.TextureImage, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &region);

        EndSingleTimeCommands(ref func , ref data, commandBuffer);
    }

    private unsafe static VkCommandBuffer BeginSingleTimeCommands(ref GraphicDeviceFunctions  func,ref GraphicDeviceData data)
    {
        
        VkCommandBufferAllocateInfo allocInfo = new();
        allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO;
        allocInfo.level = VkCommandBufferLevel.VK_COMMAND_BUFFER_LEVEL_PRIMARY;
        allocInfo.commandPool = data.Handles.CommandPool;
        allocInfo.commandBufferCount = 1;

        VkCommandBuffer commandBuffer = VkCommandBuffer.Null;

        func.vkAllocateCommandBuffers(data.Handles.Device, &allocInfo, &commandBuffer);

        VkCommandBufferBeginInfo beginInfo = new();
        beginInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO;
        beginInfo.flags =(uint) VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_ONE_TIME_SUBMIT_BIT;

        func.vkBeginCommandBuffer(commandBuffer, &beginInfo);

        return commandBuffer;
    }

    private unsafe static void EndSingleTimeCommands(ref GraphicDeviceFunctions  func,ref GraphicDeviceData data, VkCommandBuffer commandBuffer) 
    {
        func.vkEndCommandBuffer(commandBuffer);
       
        VkSubmitInfo submitInfo = new();
        submitInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SUBMIT_INFO;
        submitInfo.commandBufferCount = 1;
        submitInfo.pCommandBuffers =&commandBuffer;
        
        func.vkQueueSubmit(data.Handles.GraphicQueue, 1, &submitInfo, VkFence.Null);
        func.vkQueueWaitIdle(data.Handles.GraphicQueue);

        func.vkFreeCommandBuffers(data.Handles.Device, data.Handles.CommandPool, 1, &commandBuffer);
    }

    #endregion

}


public struct TextureData
{
    public VkSampler sampler;
    public VkImage image;
    public VkImageLayout imageLayout;
    public VkDeviceMemory DeviceMemory;
    public VkImageView view;
    public uint width, height;
    public uint mipLevels;

    public float lodBias; // distance between object and camera  Level of detail
}
#region Texture Mapping
//Inspired by : https://github.com/mellinoe/vk/blob/master/src/samples/texture/TextureMappingExample.cs#L895
// /*
//  public void Dispose()
//         {
//             // Clean up used Vulkan resources 
//             // Note : Inherited destructor cleans up resources stored in base class

//             destroyTextureImage(texture);

//             vkDestroyPipeline(device, pipelines_solid, null);

//             vkDestroyPipelineLayout(device, pipelineLayout, null);
//             vkDestroyDescriptorSetLayout(device, descriptorSetLayout, null);

//             vertexBuffer.destroy();
//             indexBuffer.destroy();
//             uniformBufferVS.destroy();
//         }

//         // Create an image memory barrier for changing the layout of
//         // an image and put it into an active command buffer
//         void setImageLayout(
//             VkCommandBuffer cmdBuffer,
//             VkImage image,
//             VkImageAspectFlags aspectMask,
//             VkImageLayout oldImageLayout,
//             VkImageLayout newImageLayout,
//             VkImageSubresourceRange subresourceRange)
//         {
//             // Create an image barrier object
//             VkImageMemoryBarrier imageMemoryBarrier = Initializers.imageMemoryBarrier(); ;
//             imageMemoryBarrier.oldLayout = oldImageLayout;
//             imageMemoryBarrier.newLayout = newImageLayout;
//             imageMemoryBarrier.image = image;
//             imageMemoryBarrier.subresourceRange = subresourceRange;

//             // Only sets masks for layouts used in this example
//             // For a more complete version that can be used with other layouts see vks::tools::setImageLayout

//             // Source layouts (old)
//             switch (oldImageLayout)
//             {
//                 case VkImageLayout.Undefined:
//                     // Only valid as initial layout, memory contents are not preserved
//                     // Can be accessed directly, no source dependency required
//                     imageMemoryBarrier.srcAccessMask = 0;
//                     break;
//                 case VkImageLayout.Preinitialized:
//                     // Only valid as initial layout for linear images, preserves memory contents
//                     // Make sure host writes to the image have been finished
//                     imageMemoryBarrier.srcAccessMask = VkAccessFlags.HostWrite;
//                     break;
//                 case VkImageLayout.TransferDstOptimal:
//                     // Old layout is transfer destination
//                     // Make sure any writes to the image have been finished
//                     imageMemoryBarrier.srcAccessMask = VkAccessFlags.TransferWrite;
//                     break;
//             }

//             // Target layouts (new)
//             switch (newImageLayout)
//             {
//                 case VkImageLayout.TransferSrcOptimal:
//                     // Transfer source (copy, blit)
//                     // Make sure any reads from the image have been finished
//                     imageMemoryBarrier.dstAccessMask = VkAccessFlags.TransferRead;
//                     break;
//                 case VkImageLayout.TransferDstOptimal:
//                     // Transfer destination (copy, blit)
//                     // Make sure any writes to the image have been finished
//                     imageMemoryBarrier.dstAccessMask = VkAccessFlags.TransferWrite;
//                     break;
//                 case VkImageLayout.ShaderReadOnlyOptimal:
//                     // Shader read (sampler, input attachment)
//                     imageMemoryBarrier.dstAccessMask = VkAccessFlags.ShaderRead;
//                     break;
//             }

//             // Put barrier on top of pipeline
//             VkPipelineStageFlags srcStageFlags = VkPipelineStageFlags.TopOfPipe;
//             VkPipelineStageFlags destStageFlags = VkPipelineStageFlags.TopOfPipe;

//             // Put barrier inside setup command buffer
//             vkCmdPipelineBarrier(
//                 cmdBuffer,
//                 srcStageFlags,
//                 destStageFlags,
//                 VkDependencyFlags.None,
//                 0, null,
//                 0, null,
//                 1, &imageMemoryBarrier);
//         }

//         void loadTexture(string fileName, VkFormat format, bool forceLinearTiling)
//         {
//             KtxFile tex2D;
//             using (var fs = File.OpenRead(fileName))
//             {
//                 tex2D = KtxFile.Load(fs, false);
//             }

//             VkFormatProperties formatProperties;

//             texture.width = tex2D.Header.PixelWidth;
//             texture.height = tex2D.Header.PixelHeight;
//             texture.mipLevels = tex2D.Header.NumberOfMipmapLevels;

//             // Get Device properites for the requested texture format
//             vkGetPhysicalDeviceFormatProperties(physicalDevice, format, &formatProperties);

//             // Only use linear tiling if requested (and supported by the Device)
//             // Support for linear tiling is mostly limited, so prefer to use
//             // optimal tiling instead
//             // On most implementations linear tiling will only support a very
//             // limited amount of formats and features (mip maps, cubemaps, arrays, etc.)
//             uint useStaging = 1;

//             // Only use linear tiling if forced
//             if (forceLinearTiling)
//             {
//                 // Don't use linear if format is not supported for (linear) shader sampling
//                 useStaging = ((formatProperties.linearTilingFeatures & VkFormatFeatureFlags.SampledImage) != VkFormatFeatureFlags.SampledImage) ? 1u : 0u;
//             }

//             VkMemoryAllocateInfo memAllocInfo = Initializers.memoryAllocateInfo();
//             VkMemoryRequirements memReqs = new VkMemoryRequirements();

//             if (useStaging == 1)
//             {
//                 // Create a host-visible staging buffer that contains the raw image data
//                 VkBuffer stagingBuffer;
//                 VkDeviceMemory stagingMemory;

//                 VkBufferCreateInfo bufferCreateInfo = Initializers.bufferCreateInfo();
//                 bufferCreateInfo.size = tex2D.GetTotalSize();
//                 // This buffer is used as a transfer source for the buffer copy
//                 bufferCreateInfo.usage = VkBufferUsageFlags.TransferSrc;
//                 bufferCreateInfo.sharingMode = VkSharingMode.Exclusive;

//                 Util.CheckResult(vkCreateBuffer(device, &bufferCreateInfo, null, &stagingBuffer));

//                 // Get memory requirements for the staging buffer (alignment, memory type bits)
//                 vkGetBufferMemoryRequirements(device, stagingBuffer, &memReqs);

//                 memAllocInfo.allocationSize = memReqs.size;
//                 // Get memory type index for a host visible buffer
//                 memAllocInfo.memoryTypeIndex = vulkanDevice.getMemoryType(memReqs.memoryTypeBits, VkMemoryPropertyFlags.HostVisible | VkMemoryPropertyFlags.HostCoherent);

//                 Util.CheckResult(vkAllocateMemory(device, &memAllocInfo, null, &stagingMemory));
//                 Util.CheckResult(vkBindBufferMemory(device, stagingBuffer, stagingMemory, 0));

//                 // Copy texture data into staging buffer
//                 byte* data;
//                 Util.CheckResult(vkMapMemory(device, stagingMemory, 0, memReqs.size, 0, (void**)&data));
//                 byte[] allData = tex2D.GetAllTextureData();
//                 fixed (byte* tex2DDataPtr = &allData[0])
//                 {
//                     Unsafe.CopyBlock(data, tex2DDataPtr, (uint)allData.Length);
//                 }
//                 vkUnmapMemory(device, stagingMemory);

//                 // Setup buffer copy regions for each mip level
//                 NativeList<VkBufferImageCopy> bufferCopyRegions = new NativeList<VkBufferImageCopy>();
//                 uint offset = 0;

//                 for (uint i = 0; i < texture.mipLevels; i++)
//                 {
//                     VkBufferImageCopy bufferCopyRegion = new VkBufferImageCopy();
//                     bufferCopyRegion.imageSubresource.aspectMask = VkImageAspectFlags.Color;
//                     bufferCopyRegion.imageSubresource.mipLevel = i;
//                     bufferCopyRegion.imageSubresource.baseArrayLayer = 0;
//                     bufferCopyRegion.imageSubresource.layerCount = 1;
//                     bufferCopyRegion.imageExtent.width = tex2D.Faces[0].Mipmaps[i].Width;
//                     bufferCopyRegion.imageExtent.height = tex2D.Faces[0].Mipmaps[i].Height;
//                     bufferCopyRegion.imageExtent.depth = 1;
//                     bufferCopyRegion.bufferOffset = offset;

//                     bufferCopyRegions.Add(bufferCopyRegion);

//                     offset += tex2D.Faces[0].Mipmaps[i].SizeInBytes;
//                 }

//                 // Create optimal tiled target image
//                 VkImageCreateInfo imageCreateInfo = Initializers.imageCreateInfo();
//                 imageCreateInfo.imageType = VkImageType.Image2D;
//                 imageCreateInfo.format = format;
//                 imageCreateInfo.mipLevels = texture.mipLevels;
//                 imageCreateInfo.arrayLayers = 1;
//                 imageCreateInfo.samples = VkSampleCountFlags.Count1;
//                 imageCreateInfo.tiling = VkImageTiling.Optimal;
//                 imageCreateInfo.sharingMode = VkSharingMode.Exclusive;
//                 // Set initial layout of the image to undefined
//                 imageCreateInfo.initialLayout = VkImageLayout.Undefined;
//                 imageCreateInfo.extent = new VkExtent3D { width = texture.width, height = texture.height, depth = 1 };
//                 imageCreateInfo.usage = VkImageUsageFlags.TransferDst | VkImageUsageFlags.Sampled;

//                 Util.CheckResult(vkCreateImage(device, &imageCreateInfo, null, out texture.image));

//                 vkGetImageMemoryRequirements(device, texture.image, &memReqs);

//                 memAllocInfo.allocationSize = memReqs.size;
//                 memAllocInfo.memoryTypeIndex = vulkanDevice.getMemoryType(memReqs.memoryTypeBits, VkMemoryPropertyFlags.DeviceLocal);

//                 Util.CheckResult(vkAllocateMemory(device, &memAllocInfo, null, out texture.DeviceMemory));
//                 Util.CheckResult(vkBindImageMemory(device, texture.image, texture.DeviceMemory, 0));

//                 VkCommandBuffer copyCmd = base.createCommandBuffer(VkCommandBufferLevel.Primary, true);

//                 // Image barrier for optimal image

//                 // The sub resource range describes the regions of the image we will be transition
//                 VkImageSubresourceRange subresourceRange = new VkImageSubresourceRange();
//                 // Image only contains color data
//                 subresourceRange.aspectMask = VkImageAspectFlags.Color;
//                 // Start at first mip level
//                 subresourceRange.baseMipLevel = 0;
//                 // We will transition on all mip levels
//                 subresourceRange.levelCount = texture.mipLevels;
//                 // The 2D texture only has one layer
//                 subresourceRange.layerCount = 1;

//                 // Optimal image will be used as destination for the copy, so we must transfer from our
//                 // initial undefined image layout to the transfer destination layout
//                 setImageLayout(
//                     copyCmd,
//                     texture.image,
//                      VkImageAspectFlags.Color,
//                      VkImageLayout.Undefined,
//                      VkImageLayout.TransferDstOptimal,
//                     subresourceRange);

//                 // Copy mip levels from staging buffer
//                 vkCmdCopyBufferToImage(
//                     copyCmd,
//                     stagingBuffer,
//                     texture.image,
//                      VkImageLayout.TransferDstOptimal,
//                     bufferCopyRegions.Count,
//                     bufferCopyRegions.Data);

//                 // Change texture image layout to shader read after all mip levels have been copied
//                 texture.imageLayout = VkImageLayout.ShaderReadOnlyOptimal;
//                 setImageLayout(
//                     copyCmd,
//                     texture.image,
//                     VkImageAspectFlags.Color,
//                     VkImageLayout.TransferDstOptimal,
//                     texture.imageLayout,
//                     subresourceRange);

//                 flushCommandBuffer(copyCmd, queue, true);

//                 // Clean up staging resources
//                 vkFreeMemory(device, stagingMemory, null);
//                 vkDestroyBuffer(device, stagingBuffer, null);
//             }
//             else
//             {
//                 throw new NotImplementedException();

//                 /*
//                 // Prefer using optimal tiling, as linear tiling 
//                 // may support only a small set of features 
//                 // depending on implementation (e.g. no mip maps, only one layer, etc.)

//                 VkImage mappableImage;
//                 VkDeviceMemory mappableMemory;

//                 // Load mip map level 0 to linear tiling image
//                 VkImageCreateInfo imageCreateInfo = Initializers.imageCreateInfo();
//                 imageCreateInfo.imageType = VkImageType._2d;
//                 imageCreateInfo.format = format;
//                 imageCreateInfo.mipLevels = 1;
//                 imageCreateInfo.arrayLayers = 1;
//                 imageCreateInfo.samples = VkSampleCountFlags._1;
//                 imageCreateInfo.tiling = VkImageTiling.Linear;
//                 imageCreateInfo.usage = VkImageUsageFlags.Sampled;
//                 imageCreateInfo.sharingMode = VkSharingMode.Exclusive;
//                 imageCreateInfo.initialLayout = VkImageLayout.Preinitialized;
//                 imageCreateInfo.extent = new VkExtent3D { width = texture.width, height = texture.height, depth = 1 };
//                 Util.CheckResult(vkCreateImage(Device, &imageCreateInfo, null, &mappableImage));

//                 // Get memory requirements for this image 
//                 // like size and alignment
//                 vkGetImageMemoryRequirements(Device, mappableImage, &memReqs);
//                 // Set memory allocation size to required memory size
//                 memAllocInfo.allocationSize = memReqs.size;

//                 // Get memory type that can be mapped to host memory
//                 memAllocInfo.memoryTypeIndex = VulkanDevice.GetMemoryType(memReqs.memoryTypeBits,  VkMemoryPropertyFlags.HostVisible |  VkMemoryPropertyFlags.HostCoherent);

//                 // Allocate host memory
//                 Util.CheckResult(vkAllocateMemory(Device, &memAllocInfo, null, &mappableMemory));

//                 // Bind allocated image for use
//                 Util.CheckResult(vkBindImageMemory(Device, mappableImage, mappableMemory, 0));

//                 // Get sub resource layout
//                 // Mip map count, array layer, etc.
//                 VkImageSubresource subRes = new VkImageSubresource();
//                 subRes.aspectMask =  VkImageAspectFlags.Color;

//                 VkSubresourceLayout subResLayout;
//                 void* data;

//                 // Get sub resources layout 
//                 // Includes row pitch, size offsets, etc.
//                 vkGetImageSubresourceLayout(Device, mappableImage, &subRes, &subResLayout);

//                 // Map image memory
//                 Util.CheckResult(vkMapMemory(Device, mappableMemory, 0, memReqs.size, 0, &data));

//                 // Copy image data into memory
//                 memcpy(data, tex2D[subRes.mipLevel].data(), tex2D[subRes.mipLevel].size());

//                 vkUnmapMemory(Device, mappableMemory);

//                 // Linear tiled images don't need to be staged
//                 // and can be directly used as textures
//                 texture.image = mappableImage;
//                 texture.DeviceMemory = mappableMemory;
//                 texture.imageLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;

//                 VkCommandBuffer copyCmd = VulkanExampleBase::createCommandBuffer(VK_COMMAND_BUFFER_LEVEL_PRIMARY, true);

//                 // Setup image memory barrier transfer image to shader read layout

//                 // The sub resource range describes the regions of the image we will be transition
//                 VkImageSubresourceRange subresourceRange = { };
//                 // Image only contains color data
//                 subresourceRange.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
//                 // Start at first mip level
//                 subresourceRange.baseMipLevel = 0;
//                 // Only one mip level, most implementations won't support more for linear tiled images
//                 subresourceRange.levelCount = 1;
//                 // The 2D texture only has one layer
//                 subresourceRange.layerCount = 1;

//                 setImageLayout(
//                     copyCmd,
//                     texture.image,
//                     VK_IMAGE_ASPECT_COLOR_BIT,
//                     VK_IMAGE_LAYOUT_PREINITIALIZED,
//                     texture.imageLayout,
//                     subresourceRange);

//                 VulkanExampleBase::flushCommandBuffer(copyCmd, queue, true);
//                 */
//             }

//             // Create sampler
//             // In Vulkan textures are accessed by samplers
//             // This separates all the sampling information from the 
//             // texture data
//             // This means you could have multiple sampler objects
//             // for the same texture with different settings
//             // Similar to the samplers available with OpenGL 3.3
//             VkSamplerCreateInfo sampler = Initializers.samplerCreateInfo();
//             sampler.magFilter = VkFilter.Linear;
//             sampler.minFilter = VkFilter.Linear;
//             sampler.mipmapMode = VkSamplerMipmapMode.Linear;
//             sampler.addressModeU = VkSamplerAddressMode.Repeat;
//             sampler.addressModeV = VkSamplerAddressMode.Repeat;
//             sampler.addressModeW = VkSamplerAddressMode.Repeat;
//             sampler.mipLodBias = 0.0f;
//             sampler.compareOp = VkCompareOp.Never;
//             sampler.minLod = 0.0f;
//             // Set max level-of-detail to mip level count of the texture
//             sampler.maxLod = (useStaging == 1) ? (float)texture.mipLevels : 0.0f;
//             // Enable anisotropic filtering
//             // This feature is optional, so we must check if it's supported on the Device
//             if (vulkanDevice.features.samplerAnisotropy == 1)
//             {
//                 // Use max. level of anisotropy for this example
//                 sampler.maxAnisotropy = vulkanDevice.properties.limits.maxSamplerAnisotropy;
//                 sampler.anisotropyEnable = True;
//             }
//             else
//             {
//                 // The Device does not support anisotropic filtering
//                 sampler.maxAnisotropy = 1.0f;
//                 sampler.anisotropyEnable = False;
//             }
//             sampler.borderColor = VkBorderColor.FloatOpaqueWhite;
//             Util.CheckResult(vkCreateSampler(device, ref sampler, null, out texture.sampler));

//             // Create image view
//             // Textures are not directly accessed by the shaders and
//             // are abstracted by image views containing additional
//             // information and sub resource ranges
//             VkImageViewCreateInfo view = Initializers.imageViewCreateInfo();
//             view.viewType = VkImageViewType.Image2D;
//             view.format = format;
//             view.components = new VkComponentMapping { r = VkComponentSwizzle.R, g = VkComponentSwizzle.G, b = VkComponentSwizzle.B, a = VkComponentSwizzle.A };
//             // The subresource range describes the set of mip levels (and array layers) that can be accessed through this image view
//             // It's possible to create multiple image views for a single image referring to different (and/or overlapping) ranges of the image
//             view.subresourceRange.aspectMask = VkImageAspectFlags.Color;
//             view.subresourceRange.baseMipLevel = 0;
//             view.subresourceRange.baseArrayLayer = 0;
//             view.subresourceRange.layerCount = 1;
//             // Linear tiling usually won't support mip maps
//             // Only set mip map count if optimal tiling is used
//             view.subresourceRange.levelCount = (useStaging == 1) ? texture.mipLevels : 1;
//             // The view will be based on the texture's image
//             view.image = texture.image;
//             Util.CheckResult(vkCreateImageView(device, &view, null, out texture.view));
//         }

//         // Free all Vulkan resources used a texture object
//         void destroyTextureImage(Texture texture)
//         {
//             vkDestroyImageView(device, texture.view, null);
//             vkDestroyImage(device, texture.image, null);
//             vkDestroySampler(device, texture.sampler, null);
//             vkFreeMemory(device, texture.DeviceMemory, null);
//         }

//         void loadTextures()
//         {
//             // Vulkan core supports three different compressed texture formats
//             // As the support differs between implemementations we need to check Device features and select a proper format and file
//             string filename;
//             VkFormat format;
//             if (DeviceFeatures.textureCompressionBC == 1)
//             {
//                 filename = "metalplate01_bc2_unorm.ktx";
//                 format = VkFormat.Bc2UnormBlock;
//             }
//             else if (DeviceFeatures.textureCompressionASTC_LDR == 1)
//             {
//                 filename = "metalplate01_astc_8x8_unorm.ktx";
//                 format = VkFormat.Astc8x8UnormBlock;
//             }
//             else if (DeviceFeatures.textureCompressionETC2 == 1)
//             {
//                 filename = "metalplate01_etc2_unorm.ktx";
//                 format = VkFormat.Etc2R8g8b8a8UnormBlock;
//             }
//             else
//             {
//                 throw new InvalidOperationException("Device does not support any compressed texture format!");
//             }

//             loadTexture(getAssetPath() + "textures/" + filename, format, false);
//         }

// */
#endregion
#region Texture Array
//    public void Dispose()
//         {
//             // Clean up used Vulkan resources 
//             // Note : Inherited destructor cleans up resources stored in base class

//             // Clean up texture resources
//             vkDestroyImageView(device, textureArray.view, null);
//             vkDestroyImage(device, textureArray.image, null);
//             vkDestroySampler(device, textureArray.sampler, null);
//             vkFreeMemory(device, textureArray.deviceMemory, null);

//             vkDestroyPipeline(device, pipeline, null);

//             vkDestroyPipelineLayout(device, pipelineLayout, null);
//             vkDestroyDescriptorSetLayout(device, descriptorSetLayout, null);

//             vertexBuffer.destroy();
//             indexBuffer.destroy();

//             uniformBufferVS.destroy();

//             // delete[] uboVS.instance;
//         }

//         void loadTextureArray(string filename, VkFormat format)
//         {
//             KtxFile tex2DArray;
//             using (var fs = File.OpenRead(filename))
//             {
//                 tex2DArray = KtxFile.Load(fs, false);
//             }

//             textureArray.width = tex2DArray.Header.PixelWidth;
//             textureArray.height = tex2DArray.Header.PixelHeight;
//             layerCount = tex2DArray.Header.NumberOfArrayElements;

//             VkMemoryAllocateInfo memAllocInfo = Initializers.memoryAllocateInfo();
//             VkMemoryRequirements memReqs;

//             // Create a host-visible staging buffer that contains the raw image data
//             VkBuffer stagingBuffer;
//             VkDeviceMemory stagingMemory;

//             VkBufferCreateInfo bufferCreateInfo = Initializers.bufferCreateInfo();
//             bufferCreateInfo.size = tex2DArray.GetTotalSize();
//             // This buffer is used as a transfer source for the buffer copy
//             bufferCreateInfo.usage = VK_BUFFER_USAGE_TRANSFER_SRC_BIT;
//             bufferCreateInfo.sharingMode = VK_SHARING_MODE_EXCLUSIVE;

//             Util.CheckResult(vkCreateBuffer(device, &bufferCreateInfo, null, &stagingBuffer));

//             // Get memory requirements for the staging buffer (alignment, memory type bits)
//             vkGetBufferMemoryRequirements(device, stagingBuffer, &memReqs);

//             memAllocInfo.allocationSize = memReqs.size;
//             // Get memory type index for a host visible buffer
//             memAllocInfo.memoryTypeIndex = vulkanDevice.getMemoryType(memReqs.memoryTypeBits, VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VK_MEMORY_PROPERTY_HOST_COHERENT_BIT);

//             Util.CheckResult(vkAllocateMemory(device, &memAllocInfo, null, &stagingMemory));
//             Util.CheckResult(vkBindBufferMemory(device, stagingBuffer, stagingMemory, 0));

//             // Copy texture data into staging buffer
//             byte* data;
//             Util.CheckResult(vkMapMemory(device, stagingMemory, 0, memReqs.size, 0, (void**)&data));
//             byte[] allTextureData = tex2DArray.GetAllTextureData();
//             fixed (byte* texPtr = allTextureData)
//             {
//                 Unsafe.CopyBlock(data, texPtr, (uint)allTextureData.Length);
//             }
//             vkUnmapMemory(device, stagingMemory);

//             // Setup buffer copy regions for array layers
//             NativeList<VkBufferImageCopy> bufferCopyRegions;
//             IntPtr offset = IntPtr.Zero;

//             for (uint layer = 0; layer < layerCount; layer++)
//             {
//                 VkBufferImageCopy bufferCopyRegion = new VkBufferImageCopy();
//                 bufferCopyRegion.imageSubresource.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
//                 bufferCopyRegion.imageSubresource.mipLevel = 0;
//                 bufferCopyRegion.imageSubresource.baseArrayLayer = layer;
//                 bufferCopyRegion.imageSubresource.layerCount = 1;
//                 bufferCopyRegion.imageExtent.width = (uint)(tex2DArray[layer][0].extent().x);
//                 bufferCopyRegion.imageExtent.height = (uint)(tex2DArray[layer][0].extent().y);
//                 bufferCopyRegion.imageExtent.depth = 1;
//                 bufferCopyRegion.bufferOffset = offset;

//                 bufferCopyRegions.push_back(bufferCopyRegion);

//                 // Increase offset into staging buffer for next level / face
//                 offset += tex2DArray[layer][0].Count;
//             }

//             // Create optimal tiled target image
//             VkImageCreateInfo imageCreateInfo = Initializers.imageCreateInfo();
//             imageCreateInfo.imageType = VK_IMAGE_TYPE_2D;
//             imageCreateInfo.format = format;
//             imageCreateInfo.mipLevels = 1;
//             imageCreateInfo.samples = VK_SAMPLE_COUNT_1_BIT;
//             imageCreateInfo.tiling = VK_IMAGE_TILING_OPTIMAL;
//             imageCreateInfo.usage = VK_IMAGE_USAGE_SAMPLED_BIT;
//             imageCreateInfo.sharingMode = VK_SHARING_MODE_EXCLUSIVE;
//             imageCreateInfo.initialLayout = VK_IMAGE_LAYOUT_UNDEFINED;
//             imageCreateInfo.extent = new  { textureArray.width, textureArray.height, 1 };
//             imageCreateInfo.usage = VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_SAMPLED_BIT;
//             imageCreateInfo.arrayLayers = layerCount;

//             Util.CheckResult(vkCreateImage(device, &imageCreateInfo, null, &textureArray.image));

//             vkGetImageMemoryRequirements(device, textureArray.image, &memReqs);

//             memAllocInfo.allocationSize = memReqs.size;
//             memAllocInfo.memoryTypeIndex = vulkanDevice->getMemoryType(memReqs.memoryTypeBits, VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT);

//             Util.CheckResult(vkAllocateMemory(device, &memAllocInfo, null, &textureArray.deviceMemory));
//             Util.CheckResult(vkBindImageMemory(device, textureArray.image, textureArray.deviceMemory, 0));

//             VkCommandBuffer copyCmd = VulkanExampleBase::createCommandBuffer(VK_COMMAND_BUFFER_LEVEL_PRIMARY, true);

//             // Image barrier for optimal image (target)
//             // Set initial layout for all array layers (faces) of the optimal (target) tiled texture
//             VkImageSubresourceRange subresourceRange = { };
//             subresourceRange.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
//             subresourceRange.baseMipLevel = 0;
//             subresourceRange.levelCount = 1;
//             subresourceRange.layerCount = layerCount;

//             vkstools::setImageLayout(
//                 copyCmd,
//                 textureArray.image,
//                 VK_IMAGE_ASPECT_COLOR_BIT,
//                 VK_IMAGE_LAYOUT_UNDEFINED,
//                 VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL,
//                 subresourceRange);

//             // Copy the cube map faces from the staging buffer to the optimal tiled image
//             vkCmdCopyBufferToImage(
//                 copyCmd,
//                 stagingBuffer,
//                 textureArray.image,
//                 VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL,
//                 bufferCopyRegions.Count,
//                 bufferCopyRegions.Data
//                 );

//             // Change texture image layout to shader read after all faces have been copied
//             textureArray.imageLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
//             vkstools::setImageLayout(
//                 copyCmd,
//                 textureArray.image,
//                 VK_IMAGE_ASPECT_COLOR_BIT,
//                 VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL,
//                 textureArray.imageLayout,
//                 subresourceRange);

//             VulkanExampleBase::flushCommandBuffer(copyCmd, queue, true);

//             // Create sampler
//             VkSamplerCreateInfo sampler = Initializers.samplerCreateInfo();
//             sampler.magFilter = VK_FILTER_LINEAR;
//             sampler.minFilter = VK_FILTER_LINEAR;
//             sampler.mipmapMode = VK_SAMPLER_MIPMAP_MODE_LINEAR;
//             sampler.addressModeU = VK_SAMPLER_ADDRESS_MODE_CLAMP_TO_EDGE;
//             sampler.addressModeV = sampler.addressModeU;
//             sampler.addressModeW = sampler.addressModeU;
//             sampler.mipLodBias = 0.0f;
//             sampler.maxAnisotropy = 8;
//             sampler.compareOp = VK_COMPARE_OP_NEVER;
//             sampler.minLod = 0.0f;
//             sampler.maxLod = 0.0f;
//             sampler.borderColor = VK_BORDER_COLOR_FLOAT_OPAQUE_WHITE;
//             Util.CheckResult(vkCreateSampler(device, &sampler, null, &textureArray.sampler));

//             // Create image view
//             VkImageViewCreateInfo view = Initializers.imageViewCreateInfo();
//             view.viewType = VK_IMAGE_VIEW_TYPE_2D_ARRAY;
//             view.format = format;
//             view.components = { VK_COMPONENT_SWIZZLE_R, VK_COMPONENT_SWIZZLE_G, VK_COMPONENT_SWIZZLE_B, VK_COMPONENT_SWIZZLE_A };
//             view.subresourceRange = { VK_IMAGE_ASPECT_COLOR_BIT, 0, 1, 0, 1 };
//             view.subresourceRange.layerCount = layerCount;
//             view.subresourceRange.levelCount = 1;
//             view.image = textureArray.image;
//             Util.CheckResult(vkCreateImageView(device, &view, null, &textureArray.view));

//             // Clean up staging resources
//             vkFreeMemory(device, stagingMemory, null);
//             vkDestroyBuffer(device, stagingBuffer, null);
//         }

//         void loadTextures()
//         {
//             // Vulkan core supports three different compressed texture formats
//             // As the support differs between implemementations we need to check device features and select a proper format and file
//             std::string filename;
//             VkFormat format;
//             if (deviceFeatures.textureCompressionBC)
//             {
//                 filename = "texturearray_bc3_unorm.ktx";
//                 format = VK_FORMAT_BC3_UNORM_BLOCK;
//             }
//             else if (deviceFeatures.textureCompressionASTC_LDR)
//             {
//                 filename = "texturearray_astc_8x8_unorm.ktx";
//                 format = VK_FORMAT_ASTC_8x8_UNORM_BLOCK;
//             }
//             else if (deviceFeatures.textureCompressionETC2)
//             {
//                 filename = "texturearray_etc2_unorm.ktx";
//                 format = VK_FORMAT_ETC2_R8G8B8_UNORM_BLOCK;
//             }
//             else
//             {
//                 vkstools::exitFatal("Device does not support any compressed texture format!", "Error");
//             }
//             loadTextureArray(getAssetPath() + "textures/" + filename, format);
//         }

//         void buildCommandBuffers()
//         {
//             VkCommandBufferBeginInfo cmdBufInfo = Initializers.commandBufferBeginInfo();

//             VkClearValue clearValues[2];
//             clearValues[0].color = defaultClearColor;
//             clearValues[1].depthStencil = { 1.0f, 0 };

//             VkRenderPassBeginInfo renderPassBeginInfo = Initializers.renderPassBeginInfo();
//             renderPassBeginInfo.renderPass = renderPass;
//             renderPassBeginInfo.renderArea.offset.x = 0;
//             renderPassBeginInfo.renderArea.offset.y = 0;
//             renderPassBeginInfo.renderArea.extent.width = width;
//             renderPassBeginInfo.renderArea.extent.height = height;
//             renderPassBeginInfo.clearValueCount = 2;
//             renderPassBeginInfo.pClearValues = clearValues;

//             for (int32_t i = 0; i < drawCmdBuffers.Count; ++i)
//             {
//                 // Set target frame buffer
//                 renderPassBeginInfo.framebuffer = frameBuffers[i];

//                 Util.CheckResult(vkBeginCommandBuffer(drawCmdBuffers[i], &cmdBufInfo));

//                 vkCmdBeginRenderPass(drawCmdBuffers[i], &renderPassBeginInfo, VK_SUBPASS_CONTENTS_INLINE);

//                 VkViewport viewport = Initializers.viewport((float)width, (float)height, 0.0f, 1.0f);
//                 vkCmdSetViewport(drawCmdBuffers[i], 0, 1, &viewport);

//                 VkRect2D scissor = Initializers.rect2D(width, height, 0, 0);
//                 vkCmdSetScissor(drawCmdBuffers[i], 0, 1, &scissor);

//                 vkCmdBindDescriptorSets(drawCmdBuffers[i], VK_PIPELINE_BIND_POINT_GRAPHICS, pipelineLayout, 0, 1, &descriptorSet, 0, NULL);
//                 vkCmdBindPipeline(drawCmdBuffers[i], VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline);

//                 VkDeviceSize offsets[1] = { 0 };
//                 vkCmdBindVertexBuffers(drawCmdBuffers[i], VERTEX_BUFFER_BIND_ID, 1, &vertexBuffer.buffer, offsets);
//                 vkCmdBindIndexBuffer(drawCmdBuffers[i], indexBuffer.buffer, 0, VK_INDEX_TYPE_UINT32);

//                 vkCmdDrawIndexed(drawCmdBuffers[i], indexCount, layerCount, 0, 0, 0);

//                 vkCmdEndRenderPass(drawCmdBuffers[i]);

//                 Util.CheckResult(vkEndCommandBuffer(drawCmdBuffers[i]));
//             }
//         }
#endregion