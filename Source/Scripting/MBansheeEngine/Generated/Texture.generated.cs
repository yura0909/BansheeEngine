using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace BansheeEngine
{
	/** @addtogroup Rendering
	 *  @{
	 */

	/// <summary>
	/// Abstract class representing a texture. Specific render systems have their own Texture implementations. Internally 
	/// represented as one or more surfaces with pixels in a certain number of dimensions, backed by a hardware buffer.
	/// </summary>
	public partial class Texture : Resource
	{
		private Texture(bool __dummy0) { }
		protected Texture() { }

		private Texture(PixelFormat format, uint width, uint height, uint depth, TextureType texType, TextureUsage usage, uint numSamples, bool hasMipmaps, bool gammaCorrection)
		{
			Internal_create(this, format, width, height, depth, texType, usage, numSamples, hasMipmaps, gammaCorrection);
		}

		/// <summary>Returns the pixel format for the texture surface.</summary>
		public PixelFormat PixelFormat
		{
			get { return Internal_getPixelFormat(mCachedPtr); }
		}

		/// <summary>Returns a value that signals the engine in what way is the texture expected to be used.</summary>
		public TextureUsage Usage
		{
			get { return Internal_getUsage(mCachedPtr); }
		}

		/// <summary>Gets the type of texture.</summary>
		public TextureType Type
		{
			get { return Internal_getType(mCachedPtr); }
		}

		/// <summary>Returns the width of the texture.</summary>
		public uint Width
		{
			get { return Internal_getWidth(mCachedPtr); }
		}

		/// <summary>Returns the height of the texture.</summary>
		public uint Height
		{
			get { return Internal_getHeight(mCachedPtr); }
		}

		/// <summary>Returns the depth of the texture (only applicable for 3D textures).</summary>
		public uint Depth
		{
			get { return Internal_getDepth(mCachedPtr); }
		}

		/// <summary>
		/// Determines does the texture contain gamma corrected data. If true then the GPU will automatically convert the  pixels 
		/// to linear space before reading from the texture, and convert them to gamma space when writing to the texture.
		/// </summary>
		public bool GammaSpace
		{
			get { return Internal_getGammaCorrection(mCachedPtr); }
		}

		/// <summary>Gets the number of samples used for multisampling (0 or 1 if multisampling is not used).</summary>
		public uint SampleCount
		{
			get { return Internal_getSampleCount(mCachedPtr); }
		}

		/// <summary>
		/// Gets the number of mipmaps to be used for this texture. This number excludes the top level map (which is always 
		/// assumed to be present).
		/// </summary>
		public uint MipMapCount
		{
			get { return Internal_getMipmapCount(mCachedPtr); }
		}

		/// <summary>
		/// Returns pixels for the specified mip level & face. Pixels will be read from system memory, which means the texture 
		/// has to be created with TextureUsage.CPUCached. If the texture was updated from the GPU the pixels retrieved from this 
		/// method will not reflect that, and you should use GetGPUPixels instead.
		/// </summary>
		/// <param name="mipLevel">Mip level to retrieve pixels for. Top level (0) is the highest quality.</param>
		/// <param name="face">
		/// Face to read the pixels from. Cubemap textures have six faces whose face indices are as specified in the CubeFace 
		/// enum. Array textures can have an arbitrary number of faces (if it's a cubemap array it has to be a multiple of 6).
		/// </param>
		/// <returns>A set of pixels for the specified mip level.</returns>
		public PixelData GetPixels(uint face = 0, uint mipLevel = 0)
		{
			return Internal_getPixels(mCachedPtr, face, mipLevel);
		}

		/// <summary>
		/// Reads texture pixels directly from the GPU. This is similar to GetPixels" but the texture doesn't need to be created 
		/// with TextureUsage.CPUCached, and the data will contain any updates performed by the GPU. This method can be 
		/// potentially slow as it introduces a CPU-GPU synchronization point. Additionally this method is asynchronous which 
		/// means the data is not available immediately.
		/// </summary>
		/// <param name="mipLevel">Mip level to retrieve pixels for. Top level (0) is the highest quality.</param>
		/// <param name="face">
		/// Face to read the pixels from. Cubemap textures have six faces whose face indices are as specified in the CubeFace 
		/// enum. Array textures can have an arbitrary number of faces (if it's a cubemap array it has to be a multiple of 6).
		/// </param>
		/// <returns>AsyncOp object that will contain a PixelData object when the operation completes.</returns>
		public AsyncOp GetGPUPixels(uint face = 0, uint mipLevel = 0)
		{
			return Internal_getGPUPixels(mCachedPtr, face, mipLevel);
		}

		/// <summary>Sets pixels for the specified mip level and face.</summary>
		/// <param name="data">
		/// Pixels to assign to the specified mip level. Pixel data must match the mip level size and texture pixel format.
		/// </param>
		/// <param name="mipLevel">Mip level to set pixels for. Top level (0) is the highest quality.</param>
		/// <param name="face">
		/// Face to write the pixels to. Cubemap textures have six faces whose face indices are as specified in the CubeFace 
		/// enum. Array textures can have an arbitrary number of faces (if it's a cubemap array it has to be a multiple of 6).
		/// </param>
		public void SetPixels(PixelData data, uint face = 0, uint mipLevel = 0)
		{
			Internal_setPixels(mCachedPtr, data, face, mipLevel);
		}

		/// <summary>Sets pixels for the specified mip level and face.</summary>
		/// <param name="colors">
		/// Pixels to assign to the specified mip level. Size of the array must match the mip level dimensions. Data is expected 
		/// to be laid out row by row. Pixels will be automatically converted to the valid pixel format.
		/// </param>
		/// <param name="mipLevel">Mip level to set pixels for. Top level (0) is the highest quality.</param>
		/// <param name="face">
		/// Face to write the pixels to. Cubemap textures have six faces whose face indices are as specified in the CubeFace 
		/// enum. Array textures can have an arbitrary number of faces (if it's a cubemap array it has to be a multiple of 6).
		/// </param>
		public void SetPixels(Color[] colors, uint face = 0, uint mipLevel = 0)
		{
			Internal_setPixelsArray(mCachedPtr, colors, face, mipLevel);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_create(Texture managedInstance, PixelFormat format, uint width, uint height, uint depth, TextureType texType, TextureUsage usage, uint numSamples, bool hasMipmaps, bool gammaCorrection);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern PixelFormat Internal_getPixelFormat(IntPtr thisPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern TextureUsage Internal_getUsage(IntPtr thisPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern TextureType Internal_getType(IntPtr thisPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint Internal_getWidth(IntPtr thisPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint Internal_getHeight(IntPtr thisPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint Internal_getDepth(IntPtr thisPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Internal_getGammaCorrection(IntPtr thisPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint Internal_getSampleCount(IntPtr thisPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint Internal_getMipmapCount(IntPtr thisPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern PixelData Internal_getPixels(IntPtr thisPtr, uint face, uint mipLevel);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AsyncOp Internal_getGPUPixels(IntPtr thisPtr, uint face, uint mipLevel);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_setPixels(IntPtr thisPtr, PixelData data, uint face, uint mipLevel);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_setPixelsArray(IntPtr thisPtr, Color[] colors, uint face, uint mipLevel);
	}

	/** @} */
}
