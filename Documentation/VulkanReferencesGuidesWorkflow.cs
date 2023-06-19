

public unsafe partial class VulkanReferencesGuidesWorkflow
{
    public class Instances
    {
        public void Create()
        {
            //cmd getproc addr enumerate instace
        }

        public void Destroy()
        {

        }
    }

    public class DevicesQueues
    {
        public class PhysycalDevices
        {
            //Enumerate
            //get
            //Get physical queues
        }

        public class Devices
        {
            public void Enumeration(){}
            public void  DeviceCreation() {}

            public void DeviceDestroy(){}
        }

        public class Queues
        {
            //get device queues
            //get device queues 2
            //devicequeues info

        }
        



    }

    public class CommandBuffers
    {
        public class CommandPool{
            //Create
            //Trim
            //Reset
            //Destroy
        }

        public class CommandBufferLifetime {
            //Allocate (struct createinfo)
            //Reset
            //free
        }

        public class CommandBufferRecording{
            //Begin
            //End
        }

        public class CommandBufferSubmssion{
            //queueSubmit
        }

        public class SecondCommandBufferExecution{
            //Executecommands
        }

        public class CommandBufferDeviceMask{
            //SetDeviceMask
        }
    }

    public class SynchronozationAndCAcheControl
    {
        public class Fences{
            //Create
            //Destroy
            //Get
            //Reset
            //WaitForfence
        }

        public class Semaphore{
            //create
            //Destroy
        }

        public class Events{
            //Create
            //DEstroy
            //GetStatus
            //Set
            //Reset
            //Command Set 
            //Comand Reset
            //CommandWait
        }
    
        public class PipelineBarriers{
            //Command => memory barriers
        }

        public class WaitIdleOperations{
            //QueueWait
            //deviceWait
        }

    }

    public class RenderPass
    {
        public class RenderPassCreation{
            //create
                //Attachement Desc
                //Attachement reference
                //subpass Desc
                //sub pass Dependency
            //Destroy
        }

        public class FrameBuffers{
            //Create
                //CreateInfo
            //Destroy
        }

        public class RenderPassCommand{
            //Begin
            // GetRenderAreaGranularity
            // CmdNextSubpass 
            //End
        }
    }

    public class pipelines
    {
        public class ComputePipeline{
            //create => layout
            //
        }

        public class GraphicsPipeline{
            //create 
                //Info 
                //VertexINputState
                //VertexInputBiniding
                //VertexInputAttribut
                //InputAssemblyState
                //Tesslation
                //Multisample
                //DepthStencil
                //StencilOP
                //ViewPort
                //Resterisation
                //ColorBlend
                //ColorBlendAttachement
                //DynamicState
            //Destroy
        }

        public class PipelineCache
        {
            //Create
            //Merge
            //Get
            //DEstroy
        }

        public class PipeleineBinding{
            //bind
        }

    }

    public class MemoryAllocation
    {
        public class DeviceMemory{
            //GetPhysicalMemory
            //GetPhysicalMemory2
            //Allocation
            //Free
        }

        public class HostAccessToDEviceMemoryObject
        {
            //Map Memory
            //FlushMappedMemoryRanges
            //InvalidateMappedMemoryRanges
                //mappedMemoryRange
            //UnMap

        }

        public class LazilyAllocatedMemory{
            //GetDeviceMemory
        }

        public class PeerMemoryFeatures{
            //vkGetDeviceGroupPeerMemoryFeatures
        }

    }

    public class ResourceCreation
    {
        public class Buffers{
            //Create
            //Destroy
        }

        public class BufferViews{
            //create
            //destroy
        }

        public class Images{
            //create
                //swapchainCreateinfo
            //GetImageSubresourceLayout
                //imagesubresource
                //Subresourcelayout
            //Destroy
        }

        public class ImageViews
        {
            //Create
                //componenetMapping
            //Destroy
        }

        public class ResourceMemoryAssociation  {
            //GetBufferMemory
            //GetimageMemory
            //BindBufferMemory
            //BindImageMemory
        }

    }

    public class Samplers{
        //createe
        //destroy
        //Create ybr
        //DestroyYBR
    }

    public class ResourceDescriptors
    {
        public class DescriptorSetLayout{
            //Create
            //get
            //destroy
        }

        public class PipelineLayout{
            //create
            //destroy
        }

        public class AllocationDescriptor{
            //create Pool
                //desciptor poool size
            //destroy pool
            //reset Pool

            //Allocate DescriptorSet
            //Free DescriptorSet
        }

        public class DescriptorSet {
            //Update
                //write
                //copy
                //Imageinfo
                //bufferInfo
        }

        public class UpdateWithtemplates{
            //create
            //destroy
            //update
        }
        public class SetBinding{
            //CmdBind
        }
        public class PushConstante{
            //CmdPush
        }
    }

    public class Queries
    {
        public class QueryPool{
            //create
            //destroy
        }
        public class QueryOperation{
            //cmdReset
            //cmdBegin
            //cmdEnd
            //Get
            //cmdCopy
        }
        public class TimeStampQueries{
            //write
        }

    }

    public class ClearCommands
    {
        public class OutsideRenderPass
        {
            //cmdclearcolorimage
            //cmdcleardepthstencil
        }

        public class InsideRenderPass
        {
            //vkCmdClearAttachments
            //cmdcleardepthstencil
        }

        public void FillingBuffers(){}

        public void UpdatingBuffers(){}
    }

    public class DrawingCommand
    {
        //vkCmdBindIndexBuffe
        //vkCmdDraw
        //vkCmdDrawIndexed
        //CmdDrawIndirect
        //CmdDrawIndexedIndirect
    }

    public class FixedFunctionVertexPostProcessing{
        //cmdSetviewport
    }
}


