namespace RitaEngine.Base.Platform.Native.Vulkan;

using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System;
using System.Security;


#region HANDLES 
#region VK_VERSION_1_0
[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkBuffer : IEquatable<VkBuffer>
{
	public VkBuffer(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkBuffer Null => new (nuint.Zero);
	public unsafe static implicit operator VkBuffer(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkBuffer left, VkBuffer right) => left.Handle == right.Handle;
	public static bool operator !=(VkBuffer left, VkBuffer right) => left.Handle != right.Handle;
	public static bool operator ==(VkBuffer left, nuint right) => left.Handle == right;
	public static bool operator !=(VkBuffer left, nuint right) => left.Handle != right;
	public bool Equals(VkBuffer other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkBuffer handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkBuffer [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkImage : IEquatable<VkImage>
{
	public VkImage(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkImage Null => new (nuint.Zero);
	public unsafe static implicit operator VkImage(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkImage left, VkImage right) => left.Handle == right.Handle;
	public static bool operator !=(VkImage left, VkImage right) => left.Handle != right.Handle;
	public static bool operator ==(VkImage left, nuint right) => left.Handle == right;
	public static bool operator !=(VkImage left, nuint right) => left.Handle != right;
	public bool Equals(VkImage other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkImage handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkImage [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkInstance : IEquatable<VkInstance>
{
	public VkInstance(nint handle) { Handle = handle; }
	public nint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nint.Zero;
	public static VkInstance Null => new (nint.Zero);
	public unsafe static implicit operator VkInstance(void* handle) => new (new nint(handle) );
	public static bool operator ==(VkInstance left, VkInstance right) => left.Handle == right.Handle;
	public static bool operator !=(VkInstance left, VkInstance right) => left.Handle != right.Handle;
	public static bool operator ==(VkInstance left, nint right) => left.Handle == right;
	public static bool operator !=(VkInstance left, nint right) => left.Handle != right;
	public bool Equals(VkInstance other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkInstance handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkInstance [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkPhysicalDevice : IEquatable<VkPhysicalDevice>
{
	public VkPhysicalDevice(nint handle) { Handle = handle; }
	public nint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nint.Zero;
	public static VkPhysicalDevice Null => new (nint.Zero);
	public unsafe static implicit operator VkPhysicalDevice(void* handle) => new (new nint(handle) );
	public static bool operator ==(VkPhysicalDevice left, VkPhysicalDevice right) => left.Handle == right.Handle;
	public static bool operator !=(VkPhysicalDevice left, VkPhysicalDevice right) => left.Handle != right.Handle;
	public static bool operator ==(VkPhysicalDevice left, nint right) => left.Handle == right;
	public static bool operator !=(VkPhysicalDevice left, nint right) => left.Handle != right;
	public bool Equals(VkPhysicalDevice other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkPhysicalDevice handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkPhysicalDevice [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkDevice : IEquatable<VkDevice>
{
	public VkDevice(nint handle) { Handle = handle; }
	public nint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nint.Zero;
	public static VkDevice Null => new (nint.Zero);
	public unsafe static implicit operator VkDevice(void* handle) => new (new nint(handle) );
	public static bool operator ==(VkDevice left, VkDevice right) => left.Handle == right.Handle;
	public static bool operator !=(VkDevice left, VkDevice right) => left.Handle != right.Handle;
	public static bool operator ==(VkDevice left, nint right) => left.Handle == right;
	public static bool operator !=(VkDevice left, nint right) => left.Handle != right;
	public bool Equals(VkDevice other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkDevice handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkDevice [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkQueue : IEquatable<VkQueue>
{
	public VkQueue(nint handle) { Handle = handle; }
	public nint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nint.Zero;
	public static VkQueue Null => new (nint.Zero);
	public unsafe static implicit operator VkQueue(void* handle) => new (new nint(handle) );
	public static bool operator ==(VkQueue left, VkQueue right) => left.Handle == right.Handle;
	public static bool operator !=(VkQueue left, VkQueue right) => left.Handle != right.Handle;
	public static bool operator ==(VkQueue left, nint right) => left.Handle == right;
	public static bool operator !=(VkQueue left, nint right) => left.Handle != right;
	public bool Equals(VkQueue other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkQueue handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkQueue [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkSemaphore : IEquatable<VkSemaphore>
{
	public VkSemaphore(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkSemaphore Null => new (nuint.Zero);
	public unsafe static implicit operator VkSemaphore(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkSemaphore left, VkSemaphore right) => left.Handle == right.Handle;
	public static bool operator !=(VkSemaphore left, VkSemaphore right) => left.Handle != right.Handle;
	public static bool operator ==(VkSemaphore left, nuint right) => left.Handle == right;
	public static bool operator !=(VkSemaphore left, nuint right) => left.Handle != right;
	public bool Equals(VkSemaphore other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkSemaphore handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkSemaphore [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkCommandBuffer : IEquatable<VkCommandBuffer>
{
	public VkCommandBuffer(nint handle) { Handle = handle; }
	public nint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nint.Zero;
	public static VkCommandBuffer Null => new (nint.Zero);
	public unsafe static implicit operator VkCommandBuffer(void* handle) => new (new nint(handle) );
	public static bool operator ==(VkCommandBuffer left, VkCommandBuffer right) => left.Handle == right.Handle;
	public static bool operator !=(VkCommandBuffer left, VkCommandBuffer right) => left.Handle != right.Handle;
	public static bool operator ==(VkCommandBuffer left, nint right) => left.Handle == right;
	public static bool operator !=(VkCommandBuffer left, nint right) => left.Handle != right;
	public bool Equals(VkCommandBuffer other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkCommandBuffer handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkCommandBuffer [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkFence : IEquatable<VkFence>
{
	public VkFence(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkFence Null => new (nuint.Zero);
	public unsafe static implicit operator VkFence(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkFence left, VkFence right) => left.Handle == right.Handle;
	public static bool operator !=(VkFence left, VkFence right) => left.Handle != right.Handle;
	public static bool operator ==(VkFence left, nuint right) => left.Handle == right;
	public static bool operator !=(VkFence left, nuint right) => left.Handle != right;
	public bool Equals(VkFence other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkFence handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkFence [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkDeviceMemory : IEquatable<VkDeviceMemory>
{
	public VkDeviceMemory(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkDeviceMemory Null => new (nuint.Zero);
	public unsafe static implicit operator VkDeviceMemory(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkDeviceMemory left, VkDeviceMemory right) => left.Handle == right.Handle;
	public static bool operator !=(VkDeviceMemory left, VkDeviceMemory right) => left.Handle != right.Handle;
	public static bool operator ==(VkDeviceMemory left, nuint right) => left.Handle == right;
	public static bool operator !=(VkDeviceMemory left, nuint right) => left.Handle != right;
	public bool Equals(VkDeviceMemory other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkDeviceMemory handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkDeviceMemory [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkEvent : IEquatable<VkEvent>
{
	public VkEvent(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkEvent Null => new (nuint.Zero);
	public unsafe static implicit operator VkEvent(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkEvent left, VkEvent right) => left.Handle == right.Handle;
	public static bool operator !=(VkEvent left, VkEvent right) => left.Handle != right.Handle;
	public static bool operator ==(VkEvent left, nuint right) => left.Handle == right;
	public static bool operator !=(VkEvent left, nuint right) => left.Handle != right;
	public bool Equals(VkEvent other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkEvent handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkEvent [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkQueryPool : IEquatable<VkQueryPool>
{
	public VkQueryPool(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkQueryPool Null => new (nuint.Zero);
	public unsafe static implicit operator VkQueryPool(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkQueryPool left, VkQueryPool right) => left.Handle == right.Handle;
	public static bool operator !=(VkQueryPool left, VkQueryPool right) => left.Handle != right.Handle;
	public static bool operator ==(VkQueryPool left, nuint right) => left.Handle == right;
	public static bool operator !=(VkQueryPool left, nuint right) => left.Handle != right;
	public bool Equals(VkQueryPool other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkQueryPool handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkQueryPool [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkBufferView : IEquatable<VkBufferView>
{
	public VkBufferView(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkBufferView Null => new (nuint.Zero);
	public unsafe static implicit operator VkBufferView(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkBufferView left, VkBufferView right) => left.Handle == right.Handle;
	public static bool operator !=(VkBufferView left, VkBufferView right) => left.Handle != right.Handle;
	public static bool operator ==(VkBufferView left, nuint right) => left.Handle == right;
	public static bool operator !=(VkBufferView left, nuint right) => left.Handle != right;
	public bool Equals(VkBufferView other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkBufferView handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkBufferView [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkImageView : IEquatable<VkImageView>
{
	public VkImageView(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkImageView Null => new (nuint.Zero);
	public unsafe static implicit operator VkImageView(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkImageView left, VkImageView right) => left.Handle == right.Handle;
	public static bool operator !=(VkImageView left, VkImageView right) => left.Handle != right.Handle;
	public static bool operator ==(VkImageView left, nuint right) => left.Handle == right;
	public static bool operator !=(VkImageView left, nuint right) => left.Handle != right;
	public bool Equals(VkImageView other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkImageView handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkImageView [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkShaderModule : IEquatable<VkShaderModule>
{
	public VkShaderModule(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkShaderModule Null => new (nuint.Zero);
	public unsafe static implicit operator VkShaderModule(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkShaderModule left, VkShaderModule right) => left.Handle == right.Handle;
	public static bool operator !=(VkShaderModule left, VkShaderModule right) => left.Handle != right.Handle;
	public static bool operator ==(VkShaderModule left, nuint right) => left.Handle == right;
	public static bool operator !=(VkShaderModule left, nuint right) => left.Handle != right;
	public bool Equals(VkShaderModule other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkShaderModule handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkShaderModule [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkPipelineCache : IEquatable<VkPipelineCache>
{
	public VkPipelineCache(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkPipelineCache Null => new (nuint.Zero);
	public unsafe static implicit operator VkPipelineCache(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkPipelineCache left, VkPipelineCache right) => left.Handle == right.Handle;
	public static bool operator !=(VkPipelineCache left, VkPipelineCache right) => left.Handle != right.Handle;
	public static bool operator ==(VkPipelineCache left, nuint right) => left.Handle == right;
	public static bool operator !=(VkPipelineCache left, nuint right) => left.Handle != right;
	public bool Equals(VkPipelineCache other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkPipelineCache handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkPipelineCache [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkPipelineLayout : IEquatable<VkPipelineLayout>
{
	public VkPipelineLayout(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkPipelineLayout Null => new (nuint.Zero);
	public unsafe static implicit operator VkPipelineLayout(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkPipelineLayout left, VkPipelineLayout right) => left.Handle == right.Handle;
	public static bool operator !=(VkPipelineLayout left, VkPipelineLayout right) => left.Handle != right.Handle;
	public static bool operator ==(VkPipelineLayout left, nuint right) => left.Handle == right;
	public static bool operator !=(VkPipelineLayout left, nuint right) => left.Handle != right;
	public bool Equals(VkPipelineLayout other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkPipelineLayout handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkPipelineLayout [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkPipeline : IEquatable<VkPipeline>
{
	public VkPipeline(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkPipeline Null => new (nuint.Zero);
	public unsafe static implicit operator VkPipeline(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkPipeline left, VkPipeline right) => left.Handle == right.Handle;
	public static bool operator !=(VkPipeline left, VkPipeline right) => left.Handle != right.Handle;
	public static bool operator ==(VkPipeline left, nuint right) => left.Handle == right;
	public static bool operator !=(VkPipeline left, nuint right) => left.Handle != right;
	public bool Equals(VkPipeline other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkPipeline handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkPipeline [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkRenderPass : IEquatable<VkRenderPass>
{
	public VkRenderPass(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkRenderPass Null => new (nuint.Zero);
	public unsafe static implicit operator VkRenderPass(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkRenderPass left, VkRenderPass right) => left.Handle == right.Handle;
	public static bool operator !=(VkRenderPass left, VkRenderPass right) => left.Handle != right.Handle;
	public static bool operator ==(VkRenderPass left, nuint right) => left.Handle == right;
	public static bool operator !=(VkRenderPass left, nuint right) => left.Handle != right;
	public bool Equals(VkRenderPass other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkRenderPass handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkRenderPass [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkDescriptorSetLayout : IEquatable<VkDescriptorSetLayout>
{
	public VkDescriptorSetLayout(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkDescriptorSetLayout Null => new (nuint.Zero);
	public unsafe static implicit operator VkDescriptorSetLayout(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkDescriptorSetLayout left, VkDescriptorSetLayout right) => left.Handle == right.Handle;
	public static bool operator !=(VkDescriptorSetLayout left, VkDescriptorSetLayout right) => left.Handle != right.Handle;
	public static bool operator ==(VkDescriptorSetLayout left, nuint right) => left.Handle == right;
	public static bool operator !=(VkDescriptorSetLayout left, nuint right) => left.Handle != right;
	public bool Equals(VkDescriptorSetLayout other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkDescriptorSetLayout handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkDescriptorSetLayout [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkSampler : IEquatable<VkSampler>
{
	public VkSampler(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkSampler Null => new (nuint.Zero);
	public unsafe static implicit operator VkSampler(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkSampler left, VkSampler right) => left.Handle == right.Handle;
	public static bool operator !=(VkSampler left, VkSampler right) => left.Handle != right.Handle;
	public static bool operator ==(VkSampler left, nuint right) => left.Handle == right;
	public static bool operator !=(VkSampler left, nuint right) => left.Handle != right;
	public bool Equals(VkSampler other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkSampler handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkSampler [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkDescriptorSet : IEquatable<VkDescriptorSet>
{
	public VkDescriptorSet(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkDescriptorSet Null => new (nuint.Zero);
	public unsafe static implicit operator VkDescriptorSet(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkDescriptorSet left, VkDescriptorSet right) => left.Handle == right.Handle;
	public static bool operator !=(VkDescriptorSet left, VkDescriptorSet right) => left.Handle != right.Handle;
	public static bool operator ==(VkDescriptorSet left, nuint right) => left.Handle == right;
	public static bool operator !=(VkDescriptorSet left, nuint right) => left.Handle != right;
	public bool Equals(VkDescriptorSet other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkDescriptorSet handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkDescriptorSet [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkDescriptorPool : IEquatable<VkDescriptorPool>
{
	public VkDescriptorPool(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkDescriptorPool Null => new (nuint.Zero);
	public unsafe static implicit operator VkDescriptorPool(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkDescriptorPool left, VkDescriptorPool right) => left.Handle == right.Handle;
	public static bool operator !=(VkDescriptorPool left, VkDescriptorPool right) => left.Handle != right.Handle;
	public static bool operator ==(VkDescriptorPool left, nuint right) => left.Handle == right;
	public static bool operator !=(VkDescriptorPool left, nuint right) => left.Handle != right;
	public bool Equals(VkDescriptorPool other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkDescriptorPool handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkDescriptorPool [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkFramebuffer : IEquatable<VkFramebuffer>
{
	public VkFramebuffer(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkFramebuffer Null => new (nuint.Zero);
	public unsafe static implicit operator VkFramebuffer(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkFramebuffer left, VkFramebuffer right) => left.Handle == right.Handle;
	public static bool operator !=(VkFramebuffer left, VkFramebuffer right) => left.Handle != right.Handle;
	public static bool operator ==(VkFramebuffer left, nuint right) => left.Handle == right;
	public static bool operator !=(VkFramebuffer left, nuint right) => left.Handle != right;
	public bool Equals(VkFramebuffer other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkFramebuffer handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkFramebuffer [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkCommandPool : IEquatable<VkCommandPool>
{
	public VkCommandPool(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkCommandPool Null => new (nuint.Zero);
	public unsafe static implicit operator VkCommandPool(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkCommandPool left, VkCommandPool right) => left.Handle == right.Handle;
	public static bool operator !=(VkCommandPool left, VkCommandPool right) => left.Handle != right.Handle;
	public static bool operator ==(VkCommandPool left, nuint right) => left.Handle == right;
	public static bool operator !=(VkCommandPool left, nuint right) => left.Handle != right;
	public bool Equals(VkCommandPool other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkCommandPool handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkCommandPool [0x{0}]", Handle.ToString("X"));
}

#endregion
#region VK_VERSION_1_1
[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkSamplerYcbcrConversion : IEquatable<VkSamplerYcbcrConversion>
{
	public VkSamplerYcbcrConversion(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkSamplerYcbcrConversion Null => new (nuint.Zero);
	public unsafe static implicit operator VkSamplerYcbcrConversion(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkSamplerYcbcrConversion left, VkSamplerYcbcrConversion right) => left.Handle == right.Handle;
	public static bool operator !=(VkSamplerYcbcrConversion left, VkSamplerYcbcrConversion right) => left.Handle != right.Handle;
	public static bool operator ==(VkSamplerYcbcrConversion left, nuint right) => left.Handle == right;
	public static bool operator !=(VkSamplerYcbcrConversion left, nuint right) => left.Handle != right;
	public bool Equals(VkSamplerYcbcrConversion other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkSamplerYcbcrConversion handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkSamplerYcbcrConversion [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkDescriptorUpdateTemplate : IEquatable<VkDescriptorUpdateTemplate>
{
	public VkDescriptorUpdateTemplate(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkDescriptorUpdateTemplate Null => new (nuint.Zero);
	public unsafe static implicit operator VkDescriptorUpdateTemplate(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkDescriptorUpdateTemplate left, VkDescriptorUpdateTemplate right) => left.Handle == right.Handle;
	public static bool operator !=(VkDescriptorUpdateTemplate left, VkDescriptorUpdateTemplate right) => left.Handle != right.Handle;
	public static bool operator ==(VkDescriptorUpdateTemplate left, nuint right) => left.Handle == right;
	public static bool operator !=(VkDescriptorUpdateTemplate left, nuint right) => left.Handle != right;
	public bool Equals(VkDescriptorUpdateTemplate other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkDescriptorUpdateTemplate handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkDescriptorUpdateTemplate [0x{0}]", Handle.ToString("X"));
}

#endregion
#region VK_VERSION_1_3
[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkPrivateDataSlot : IEquatable<VkPrivateDataSlot>
{
	public VkPrivateDataSlot(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkPrivateDataSlot Null => new (nuint.Zero);
	public unsafe static implicit operator VkPrivateDataSlot(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkPrivateDataSlot left, VkPrivateDataSlot right) => left.Handle == right.Handle;
	public static bool operator !=(VkPrivateDataSlot left, VkPrivateDataSlot right) => left.Handle != right.Handle;
	public static bool operator ==(VkPrivateDataSlot left, nuint right) => left.Handle == right;
	public static bool operator !=(VkPrivateDataSlot left, nuint right) => left.Handle != right;
	public bool Equals(VkPrivateDataSlot other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkPrivateDataSlot handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkPrivateDataSlot [0x{0}]", Handle.ToString("X"));
}

#endregion
#region VK_KHR_surface
[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkSurfaceKHR : IEquatable<VkSurfaceKHR>
{
	public VkSurfaceKHR(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkSurfaceKHR Null => new (nuint.Zero);
	public unsafe static implicit operator VkSurfaceKHR(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkSurfaceKHR left, VkSurfaceKHR right) => left.Handle == right.Handle;
	public static bool operator !=(VkSurfaceKHR left, VkSurfaceKHR right) => left.Handle != right.Handle;
	public static bool operator ==(VkSurfaceKHR left, nuint right) => left.Handle == right;
	public static bool operator !=(VkSurfaceKHR left, nuint right) => left.Handle != right;
	public bool Equals(VkSurfaceKHR other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkSurfaceKHR handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkSurfaceKHR [0x{0}]", Handle.ToString("X"));
}

#endregion
#region VK_KHR_swapchain
[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkSwapchainKHR : IEquatable<VkSwapchainKHR>
{
	public VkSwapchainKHR(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkSwapchainKHR Null => new (nuint.Zero);
	public unsafe static implicit operator VkSwapchainKHR(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkSwapchainKHR left, VkSwapchainKHR right) => left.Handle == right.Handle;
	public static bool operator !=(VkSwapchainKHR left, VkSwapchainKHR right) => left.Handle != right.Handle;
	public static bool operator ==(VkSwapchainKHR left, nuint right) => left.Handle == right;
	public static bool operator !=(VkSwapchainKHR left, nuint right) => left.Handle != right;
	public bool Equals(VkSwapchainKHR other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkSwapchainKHR handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkSwapchainKHR [0x{0}]", Handle.ToString("X"));
}

#endregion
#region VK_KHR_display
[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkDisplayKHR : IEquatable<VkDisplayKHR>
{
	public VkDisplayKHR(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkDisplayKHR Null => new (nuint.Zero);
	public unsafe static implicit operator VkDisplayKHR(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkDisplayKHR left, VkDisplayKHR right) => left.Handle == right.Handle;
	public static bool operator !=(VkDisplayKHR left, VkDisplayKHR right) => left.Handle != right.Handle;
	public static bool operator ==(VkDisplayKHR left, nuint right) => left.Handle == right;
	public static bool operator !=(VkDisplayKHR left, nuint right) => left.Handle != right;
	public bool Equals(VkDisplayKHR other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkDisplayKHR handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkDisplayKHR [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkDisplayModeKHR : IEquatable<VkDisplayModeKHR>
{
	public VkDisplayModeKHR(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkDisplayModeKHR Null => new (nuint.Zero);
	public unsafe static implicit operator VkDisplayModeKHR(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkDisplayModeKHR left, VkDisplayModeKHR right) => left.Handle == right.Handle;
	public static bool operator !=(VkDisplayModeKHR left, VkDisplayModeKHR right) => left.Handle != right.Handle;
	public static bool operator ==(VkDisplayModeKHR left, nuint right) => left.Handle == right;
	public static bool operator !=(VkDisplayModeKHR left, nuint right) => left.Handle != right;
	public bool Equals(VkDisplayModeKHR other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkDisplayModeKHR handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkDisplayModeKHR [0x{0}]", Handle.ToString("X"));
}

#endregion
#region VK_KHR_deferred_host_operations
[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkDeferredOperationKHR : IEquatable<VkDeferredOperationKHR>
{
	public VkDeferredOperationKHR(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkDeferredOperationKHR Null => new (nuint.Zero);
	public unsafe static implicit operator VkDeferredOperationKHR(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkDeferredOperationKHR left, VkDeferredOperationKHR right) => left.Handle == right.Handle;
	public static bool operator !=(VkDeferredOperationKHR left, VkDeferredOperationKHR right) => left.Handle != right.Handle;
	public static bool operator ==(VkDeferredOperationKHR left, nuint right) => left.Handle == right;
	public static bool operator !=(VkDeferredOperationKHR left, nuint right) => left.Handle != right;
	public bool Equals(VkDeferredOperationKHR other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkDeferredOperationKHR handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkDeferredOperationKHR [0x{0}]", Handle.ToString("X"));
}

#endregion
#region VK_EXT_debug_report
[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkDebugReportCallbackEXT : IEquatable<VkDebugReportCallbackEXT>
{
	public VkDebugReportCallbackEXT(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkDebugReportCallbackEXT Null => new (nuint.Zero);
	public unsafe static implicit operator VkDebugReportCallbackEXT(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkDebugReportCallbackEXT left, VkDebugReportCallbackEXT right) => left.Handle == right.Handle;
	public static bool operator !=(VkDebugReportCallbackEXT left, VkDebugReportCallbackEXT right) => left.Handle != right.Handle;
	public static bool operator ==(VkDebugReportCallbackEXT left, nuint right) => left.Handle == right;
	public static bool operator !=(VkDebugReportCallbackEXT left, nuint right) => left.Handle != right;
	public bool Equals(VkDebugReportCallbackEXT other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkDebugReportCallbackEXT handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkDebugReportCallbackEXT [0x{0}]", Handle.ToString("X"));
}

#endregion
#region VK_NVX_binary_import
[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkCuModuleNVX : IEquatable<VkCuModuleNVX>
{
	public VkCuModuleNVX(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkCuModuleNVX Null => new (nuint.Zero);
	public unsafe static implicit operator VkCuModuleNVX(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkCuModuleNVX left, VkCuModuleNVX right) => left.Handle == right.Handle;
	public static bool operator !=(VkCuModuleNVX left, VkCuModuleNVX right) => left.Handle != right.Handle;
	public static bool operator ==(VkCuModuleNVX left, nuint right) => left.Handle == right;
	public static bool operator !=(VkCuModuleNVX left, nuint right) => left.Handle != right;
	public bool Equals(VkCuModuleNVX other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkCuModuleNVX handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkCuModuleNVX [0x{0}]", Handle.ToString("X"));
}

[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkCuFunctionNVX : IEquatable<VkCuFunctionNVX>
{
	public VkCuFunctionNVX(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkCuFunctionNVX Null => new (nuint.Zero);
	public unsafe static implicit operator VkCuFunctionNVX(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkCuFunctionNVX left, VkCuFunctionNVX right) => left.Handle == right.Handle;
	public static bool operator !=(VkCuFunctionNVX left, VkCuFunctionNVX right) => left.Handle != right.Handle;
	public static bool operator ==(VkCuFunctionNVX left, nuint right) => left.Handle == right;
	public static bool operator !=(VkCuFunctionNVX left, nuint right) => left.Handle != right;
	public bool Equals(VkCuFunctionNVX other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkCuFunctionNVX handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkCuFunctionNVX [0x{0}]", Handle.ToString("X"));
}

#endregion
#region VK_EXT_debug_utils
[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkDebugUtilsMessengerEXT : IEquatable<VkDebugUtilsMessengerEXT>
{
	public VkDebugUtilsMessengerEXT(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkDebugUtilsMessengerEXT Null => new (nuint.Zero);
	public unsafe static implicit operator VkDebugUtilsMessengerEXT(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkDebugUtilsMessengerEXT left, VkDebugUtilsMessengerEXT right) => left.Handle == right.Handle;
	public static bool operator !=(VkDebugUtilsMessengerEXT left, VkDebugUtilsMessengerEXT right) => left.Handle != right.Handle;
	public static bool operator ==(VkDebugUtilsMessengerEXT left, nuint right) => left.Handle == right;
	public static bool operator !=(VkDebugUtilsMessengerEXT left, nuint right) => left.Handle != right;
	public bool Equals(VkDebugUtilsMessengerEXT other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkDebugUtilsMessengerEXT handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkDebugUtilsMessengerEXT [0x{0}]", Handle.ToString("X"));
}

#endregion
#region VK_EXT_validation_cache
[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkValidationCacheEXT : IEquatable<VkValidationCacheEXT>
{
	public VkValidationCacheEXT(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkValidationCacheEXT Null => new (nuint.Zero);
	public unsafe static implicit operator VkValidationCacheEXT(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkValidationCacheEXT left, VkValidationCacheEXT right) => left.Handle == right.Handle;
	public static bool operator !=(VkValidationCacheEXT left, VkValidationCacheEXT right) => left.Handle != right.Handle;
	public static bool operator ==(VkValidationCacheEXT left, nuint right) => left.Handle == right;
	public static bool operator !=(VkValidationCacheEXT left, nuint right) => left.Handle != right;
	public bool Equals(VkValidationCacheEXT other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkValidationCacheEXT handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkValidationCacheEXT [0x{0}]", Handle.ToString("X"));
}

#endregion
#region VK_NV_ray_tracing
[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkAccelerationStructureNV : IEquatable<VkAccelerationStructureNV>
{
	public VkAccelerationStructureNV(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkAccelerationStructureNV Null => new (nuint.Zero);
	public unsafe static implicit operator VkAccelerationStructureNV(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkAccelerationStructureNV left, VkAccelerationStructureNV right) => left.Handle == right.Handle;
	public static bool operator !=(VkAccelerationStructureNV left, VkAccelerationStructureNV right) => left.Handle != right.Handle;
	public static bool operator ==(VkAccelerationStructureNV left, nuint right) => left.Handle == right;
	public static bool operator !=(VkAccelerationStructureNV left, nuint right) => left.Handle != right;
	public bool Equals(VkAccelerationStructureNV other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkAccelerationStructureNV handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkAccelerationStructureNV [0x{0}]", Handle.ToString("X"));
}

#endregion
#region VK_INTEL_performance_query
[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkPerformanceConfigurationINTEL : IEquatable<VkPerformanceConfigurationINTEL>
{
	public VkPerformanceConfigurationINTEL(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkPerformanceConfigurationINTEL Null => new (nuint.Zero);
	public unsafe static implicit operator VkPerformanceConfigurationINTEL(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkPerformanceConfigurationINTEL left, VkPerformanceConfigurationINTEL right) => left.Handle == right.Handle;
	public static bool operator !=(VkPerformanceConfigurationINTEL left, VkPerformanceConfigurationINTEL right) => left.Handle != right.Handle;
	public static bool operator ==(VkPerformanceConfigurationINTEL left, nuint right) => left.Handle == right;
	public static bool operator !=(VkPerformanceConfigurationINTEL left, nuint right) => left.Handle != right;
	public bool Equals(VkPerformanceConfigurationINTEL other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkPerformanceConfigurationINTEL handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkPerformanceConfigurationINTEL [0x{0}]", Handle.ToString("X"));
}

#endregion
#region VK_NV_device_generated_commands
[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkIndirectCommandsLayoutNV : IEquatable<VkIndirectCommandsLayoutNV>
{
	public VkIndirectCommandsLayoutNV(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkIndirectCommandsLayoutNV Null => new (nuint.Zero);
	public unsafe static implicit operator VkIndirectCommandsLayoutNV(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkIndirectCommandsLayoutNV left, VkIndirectCommandsLayoutNV right) => left.Handle == right.Handle;
	public static bool operator !=(VkIndirectCommandsLayoutNV left, VkIndirectCommandsLayoutNV right) => left.Handle != right.Handle;
	public static bool operator ==(VkIndirectCommandsLayoutNV left, nuint right) => left.Handle == right;
	public static bool operator !=(VkIndirectCommandsLayoutNV left, nuint right) => left.Handle != right;
	public bool Equals(VkIndirectCommandsLayoutNV other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkIndirectCommandsLayoutNV handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkIndirectCommandsLayoutNV [0x{0}]", Handle.ToString("X"));
}

#endregion
#region VK_EXT_opacity_micromap
[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkMicromapEXT : IEquatable<VkMicromapEXT>
{
	public VkMicromapEXT(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkMicromapEXT Null => new (nuint.Zero);
	public unsafe static implicit operator VkMicromapEXT(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkMicromapEXT left, VkMicromapEXT right) => left.Handle == right.Handle;
	public static bool operator !=(VkMicromapEXT left, VkMicromapEXT right) => left.Handle != right.Handle;
	public static bool operator ==(VkMicromapEXT left, nuint right) => left.Handle == right;
	public static bool operator !=(VkMicromapEXT left, nuint right) => left.Handle != right;
	public bool Equals(VkMicromapEXT other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkMicromapEXT handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkMicromapEXT [0x{0}]", Handle.ToString("X"));
}

#endregion
#region VK_NV_optical_flow
[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkOpticalFlowSessionNV : IEquatable<VkOpticalFlowSessionNV>
{
	public VkOpticalFlowSessionNV(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkOpticalFlowSessionNV Null => new (nuint.Zero);
	public unsafe static implicit operator VkOpticalFlowSessionNV(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkOpticalFlowSessionNV left, VkOpticalFlowSessionNV right) => left.Handle == right.Handle;
	public static bool operator !=(VkOpticalFlowSessionNV left, VkOpticalFlowSessionNV right) => left.Handle != right.Handle;
	public static bool operator ==(VkOpticalFlowSessionNV left, nuint right) => left.Handle == right;
	public static bool operator !=(VkOpticalFlowSessionNV left, nuint right) => left.Handle != right;
	public bool Equals(VkOpticalFlowSessionNV other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkOpticalFlowSessionNV handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkOpticalFlowSessionNV [0x{0}]", Handle.ToString("X"));
}

#endregion
#region VK_KHR_acceleration_structure
[StructLayout(LayoutKind.Sequential)]
public unsafe readonly struct VkAccelerationStructureKHR : IEquatable<VkAccelerationStructureKHR>
{
	public VkAccelerationStructureKHR(nuint handle) { Handle = handle; }
	public nuint Handle { get; }
	public void* Ptr => Handle.ToPointer();
	public bool IsNull => Handle == nuint.Zero;
	public static VkAccelerationStructureKHR Null => new (nuint.Zero);
	public unsafe static implicit operator VkAccelerationStructureKHR(void* handle) => new (new nuint(handle) );
	public static bool operator ==(VkAccelerationStructureKHR left, VkAccelerationStructureKHR right) => left.Handle == right.Handle;
	public static bool operator !=(VkAccelerationStructureKHR left, VkAccelerationStructureKHR right) => left.Handle != right.Handle;
	public static bool operator ==(VkAccelerationStructureKHR left, nuint right) => left.Handle == right;
	public static bool operator !=(VkAccelerationStructureKHR left, nuint right) => left.Handle != right;
	public bool Equals(VkAccelerationStructureKHR other) => Handle == other.Handle;
	public override bool Equals(object? obj) => obj is VkAccelerationStructureKHR handle && Equals(handle);
	public override int GetHashCode() => Handle.GetHashCode();
	public override string ToString() => string.Format("VkAccelerationStructureKHR [0x{0}]", Handle.ToString("X"));
}

#endregion
#endregion
