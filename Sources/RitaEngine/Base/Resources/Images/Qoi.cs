
namespace RitaEngine.Base.Resources.Images;

using System;


public static unsafe class QOI
{
    // <summary>
    /// QOI image. Definition
    /// </summary>
    public class QoiImage
    {
        /// <summary>
        /// Raw pixel data.
        /// </summary>
        public byte[] Data { get; }
        
        /// <summary>
        /// Image width.
        /// </summary>
        public int Width { get; }
        
        /// <summary>
        /// Image height
        /// </summary>
        public int Height { get; }
        
        /// <summary>
        /// Channels.
        /// </summary>
        public Channels Channels { get; }
        
        /// <summary>
        /// Color space.
        /// </summary>
        public ColorSpace ColorSpace { get; }
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        public QoiImage(byte[] data, int width, int height, Channels channels, ColorSpace colorSpace = ColorSpace.SRgb)
        {
            Data = data;
            Width = width;
            Height = height;
            Channels = channels;
            ColorSpace = colorSpace;
        }
    }

    public enum Channels : byte
    {
        /// <summary>
        /// 3-channel format containing data for Red, Green, Blue.
        /// </summary>
        Rgb = 3,
        
        /// <summary>
        /// 4-channel format containing data for Red, Green, Blue, and Alpha.
        /// </summary>
        RgbWithAlpha = 4
    }

    public enum ColorSpace : byte
    {
        /// <summary>
        /// Gamma scaled RGB channels and a linear alpha channel.
        /// </summary>
        SRgb = 0,
        
        /// <summary>
        /// All channels are linear.
        /// </summary>
        Linear = 1
    }

    public static class QoiCodec
    {
        public const byte Index = 0x00;
        public const byte Diff = 0x40;
        public const byte Luma = 0x80;
        public const byte Run = 0xC0;
        public const byte Rgb = 0xFE;
        public const byte Rgba = 0xFF;
        public const byte Mask2 = 0xC0;

        /// <summary>
        /// 2GB is the max file size that this implementation can safely handle. We guard
        /// against anything larger than that, assuming the worst case with 5 bytes per 
        /// pixel, rounded down to a nice clean value. 400 million pixels ought to be 
        /// enough for anybody.
        /// </summary>
        public static int MaxPixels = 400_000_000;
        
        public const int HashTableSize = 64; 
        
        public const byte HeaderSize = 14;
        public const string MagicString = "qoif";
        
        public static readonly int Magic = CalculateMagic(MagicString.AsSpan());
        public static readonly byte[] Padding = {0, 0, 0, 0, 0, 0, 0, 1};

        public static int CalculateHashTableIndex(int r, int g, int b, int a) =>
            ((r & 0xFF) * 3 + (g & 0xFF) * 5 + (b & 0xFF) * 7 + (a & 0xFF) * 11) % HashTableSize * 4;

        public static bool IsValidMagic(byte[] magic) => CalculateMagic(magic) == Magic;
        
        private static int CalculateMagic(ReadOnlySpan<char> chars) => chars[0] << 24 | chars[1] << 16 | chars[2] << 8 | chars[3];
        private static int CalculateMagic(ReadOnlySpan<byte> data) => data[0] << 24 | data[1] << 16 | data[2] << 8 | data[3];
    }

    // <summary>
    /// Encodes raw pixel data into QOI.
    /// </summary>
    /// <param name="image">QOI image.</param>
    /// <returns>Encoded image.</returns>
    /// <exception cref="QoiEncodingException">Thrown when image information is invalid.</exception>
    public static byte[] Encode(QoiImage image)
    {
        if (image.Width == 0)
        {
            throw new Exception($"Invalid width: {image.Width}");
        }

        if (image.Height == 0 || image.Height >= QoiCodec.MaxPixels / image.Width)
        {
            throw new Exception($"Invalid height: {image.Height}. Maximum for this image is {QoiCodec.MaxPixels / image.Width - 1}");
        }

        int width = image.Width;
        int height = image.Height;
        byte channels = (byte)image.Channels;
        byte colorSpace = (byte)image.ColorSpace;
        byte[] pixels = image.Data;

        byte[] bytes = new byte[QoiCodec.HeaderSize + QoiCodec.Padding.Length + (width * height * channels)];

        bytes[0] = (byte)(QoiCodec.Magic >> 24);
        bytes[1] = (byte)(QoiCodec.Magic >> 16);
        bytes[2] = (byte)(QoiCodec.Magic >> 8);
        bytes[3] = (byte)QoiCodec.Magic;

        bytes[4] = (byte)(width >> 24);
        bytes[5] = (byte)(width >> 16);
        bytes[6] = (byte)(width >> 8);
        bytes[7] = (byte)width;

        bytes[8] = (byte)(height >> 24);
        bytes[9] = (byte)(height >> 16);
        bytes[10] = (byte)(height >> 8);
        bytes[11] = (byte)height;

        bytes[12] = channels;
        bytes[13] = colorSpace;

        byte[] index = new byte[QoiCodec.HashTableSize * 4];

        byte prevR = 0;
        byte prevG = 0;
        byte prevB = 0;
        byte prevA = 255;

        byte r = 0;
        byte g = 0;
        byte b = 0;
        byte a = 255;

        int run = 0;
        int p = QoiCodec.HeaderSize;
        bool hasAlpha = channels == 4;

        int pixelsLength = width * height * channels;
        int pixelsEnd = pixelsLength - channels;
        int counter = 0;

        for (int pxPos = 0; pxPos < pixelsLength; pxPos += channels)
        {
            r = pixels[pxPos];
            g = pixels[pxPos + 1];
            b = pixels[pxPos + 2];
            if (hasAlpha)
            {
                a = pixels[pxPos + 3];
            }

            if (RgbaEquals(prevR, prevG, prevB, prevA, r, g, b, a))
            {
                run++;
                if (run == 62 || pxPos == pixelsEnd)
                {
                    bytes[p++] = (byte)(QoiCodec.Run | (run - 1));
                    run = 0;
                }
            }
            else
            {
                if (run > 0)
                {
                    bytes[p++] = (byte)(QoiCodec.Run | (run - 1));
                    run = 0;
                }

                int indexPos = QoiCodec.CalculateHashTableIndex(r, g, b, a);

                if (RgbaEquals(r, g, b, a, index[indexPos], index[indexPos + 1], index[indexPos + 2], index[indexPos + 3]))
                {
                    bytes[p++] = (byte)(QoiCodec.Index | (indexPos / 4));
                }
                else
                {
                    index[indexPos] = r;
                    index[indexPos + 1] = g;
                    index[indexPos + 2] = b;
                    index[indexPos + 3] = a;

                    if (a == prevA)
                    {
                        int vr = r - prevR;
                        int vg = g - prevG;
                        int vb = b - prevB;

                        int vgr = vr - vg;
                        int vgb = vb - vg;

                        if (vr is > -3 and < 2 &&
                            vg is > -3 and < 2 &&
                            vb is > -3 and < 2)
                        {
                            counter++;
                            bytes[p++] = (byte)(QoiCodec.Diff | (vr + 2) << 4 | (vg + 2) << 2 | (vb + 2));
                        }
                        else if (vgr is > -9 and < 8 &&
                                 vg is > -33 and < 32 &&
                                 vgb is > -9 and < 8
                                )
                        {
                            bytes[p++] = (byte)(QoiCodec.Luma | (vg + 32));
                            bytes[p++] = (byte)((vgr + 8) << 4 | (vgb + 8));
                        }
                        else
                        {
                            bytes[p++] = QoiCodec.Rgb;
                            bytes[p++] = r;
                            bytes[p++] = g;
                            bytes[p++] = b;
                        }
                    }
                    else
                    {
                        bytes[p++] = QoiCodec.Rgba;
                        bytes[p++] = r;
                        bytes[p++] = g;
                        bytes[p++] = b;
                        bytes[p++] = a;
                    }
                }
            }

            prevR = r;
            prevG = g;
            prevB = b;
            prevA = a;
        }

        for (int padIdx = 0; padIdx < QoiCodec.Padding.Length; padIdx++)
        {
            bytes[p + padIdx] = QoiCodec.Padding[padIdx];
        }

        p += QoiCodec.Padding.Length;

        return bytes[..p];
    }

    private static bool RgbaEquals(byte r1, byte g1, byte b1, byte a1, byte r2, byte g2, byte b2, byte a2) =>
        r1 == r2 &&
        g1 == g2 &&
        b1 == b2 &&
        a1 == a2;

     /// <summary>
    /// Decodes QOI data into raw pixel data.
    /// </summary>
    /// <param name="data">QOI data</param>
    /// <returns>Decoding result.</returns>
    /// <exception cref="Exception">Thrown when data is invalid.</exception>
    public static QoiImage Decode(byte[] data)
    {
        if (data.Length < QoiCodec.HeaderSize + QoiCodec.Padding.Length)
        {
            throw new Exception("File too short");
        }
        
        if (!QoiCodec.IsValidMagic(data[..4]))
        {
            throw new Exception("Invalid file magic"); // TODO: add magic value
        }

        int width = data[4] << 24 | data[5] << 16 | data[6] << 8 | data[7];
        int height = data[8] << 24 | data[9] << 16 | data[10] << 8 | data[11];
        byte channels = data[12]; 
        var colorSpace = (ColorSpace)data[13];

        if (width == 0)
        {
            throw new Exception($"Invalid width: {width}");
        }
        if (height == 0 || height >= QoiCodec.MaxPixels / width)
        {
            throw new Exception($"Invalid height: {height}. Maximum for this image is {QoiCodec.MaxPixels / width - 1}");
        }
        if (channels is not 3 and not 4)
        {
            throw new Exception($"Invalid number of channels: {channels}");
        }
        
        byte[] index = new byte[QoiCodec.HashTableSize * 4];
        if (channels == 3) // TODO: delete
        {
            for (int indexPos = 3; indexPos < index.Length; indexPos += 4)
            {
                index[indexPos] = 255;
            }
        }

        byte[] pixels = new byte[width * height * channels];
        
        byte r = 0;
        byte g = 0;
        byte b = 0;
        byte a = 255;
        
        int run = 0;
        int p = QoiCodec.HeaderSize;

        for (int pxPos = 0; pxPos < pixels.Length; pxPos += channels)
        {
            if (run > 0)
            {
                run--;
            }
            else
            {
                byte b1 = data[p++];

                if (b1 == QoiCodec.Rgb)
                {
                    r = data[p++];
                    g = data[p++];
                    b = data[p++];
                }
                else if (b1 == QoiCodec.Rgba)
                {
                    r = data[p++];
                    g = data[p++];
                    b = data[p++];
                    a = data[p++];
                }
                else if ((b1 & QoiCodec.Mask2) == QoiCodec.Index)
                {
                    int indexPos = (b1 & ~QoiCodec.Mask2) * 4;
                    r = index[indexPos];
                    g = index[indexPos + 1];
                    b = index[indexPos + 2];
                    a = index[indexPos + 3];
                }
                else if ((b1 & QoiCodec.Mask2) == QoiCodec.Diff)
                {
                    r += (byte)(((b1 >> 4) & 0x03) - 2);
                    g += (byte)(((b1 >> 2) & 0x03) - 2);
                    b += (byte)((b1 & 0x03) - 2);
                }
                else if ((b1 & QoiCodec.Mask2) == QoiCodec.Luma) 
                {
                    int b2 = data[p++];
                    int vg = (b1 & 0x3F) - 32;
                    r += (byte)(vg - 8 + ((b2 >> 4) & 0x0F));
                    g += (byte)vg;
                    b += (byte)(vg - 8 + (b2 & 0x0F));
                }
                else if ((b1 & QoiCodec.Mask2) == QoiCodec.Run) 
                {
                    run = b1 & 0x3F;
                }
                
                int indexPos2 = QoiCodec.CalculateHashTableIndex(r, g, b, a);
                index[indexPos2] = r;
                index[indexPos2 + 1] = g;
                index[indexPos2 + 2] = b;
                index[indexPos2 + 3] = a;
            }

            pixels[pxPos] = r;
            pixels[pxPos + 1] = g;
            pixels[pxPos + 2] = b;
            if (channels == 4)
            {
                pixels[pxPos + 3] = a;
            }
        }
        
        int pixelsEnd = data.Length - QoiCodec.Padding.Length;
        for (int padIdx = 0; padIdx < QoiCodec.Padding.Length; padIdx++) 
        {
            if (data[pixelsEnd + padIdx] != QoiCodec.Padding[padIdx]) 
            {
                throw new InvalidOperationException("Invalid padding");
            }
        }

        return new QoiImage(pixels, width, height, (Channels)channels, colorSpace);
    }

}
// /*

// Copyright (c) 2021, Dominic Szablewski - https://phoboslab.org
// SPDX-License-Identifier: MIT


// QOI - The "Quite OK Image" format for fast, lossless image compression

// -- About

// QOI encodes and decodes images in a lossless format. Compared to stb_image and
// stb_image_write QOI offers 20x-50x faster encoding, 3x-4x faster decoding and
// 20% better compression.


// -- Synopsis

// // Define `QOI_IMPLEMENTATION` in *one* C/C++ file before including this
// // library to create the implementation.

// #define QOI_IMPLEMENTATION
// #include "qoi.h"

// // Encode and store an RGBA buffer to the file system. The qoi_desc describes
// // the input pixel data.
// qoi_write("image_new.qoi", rgba_pixels, &(qoi_desc){
// 	.width = 1920,
// 	.height = 1080,
// 	.channels = 4,
// 	.colorspace = QOI_SRGB
// });

// // Load and decode a QOI image from the file system into a 32bbp RGBA buffer.
// // The qoi_desc struct will be filled with the width, height, number of channels
// // and colorspace read from the file header.
// qoi_desc desc;
// void *rgba_pixels = qoi_read("image.qoi", &desc, 4);



// -- Documentation

// This library provides the following functions;
// - qoi_read    -- read and decode a QOI file
// - qoi_decode  -- decode the raw bytes of a QOI image from memory
// - qoi_write   -- encode and write a QOI file
// - qoi_encode  -- encode an rgba buffer into a QOI image in memory

// See the function declaration below for the signature and more information.

// If you don't want/need the qoi_read and qoi_write functions, you can define
// QOI_NO_STDIO before including this library.

// This library uses malloc() and free(). To supply your own malloc implementation
// you can define QOI_MALLOC and QOI_FREE before including this library.

// This library uses memset() to zero-initialize the index. To supply your own
// implementation you can define QOI_ZEROARR before including this library.


// -- Data Format

// A QOI file has a 14 byte header, followed by any number of data "chunks" and an
// 8-byte end marker.

// struct qoi_header_t {
// 	char     magic[4];   // magic bytes "qoif"
// 	uint32_t width;      // image width in pixels (BE)
// 	uint32_t height;     // image height in pixels (BE)
// 	uint8_t  channels;   // 3 = RGB, 4 = RGBA
// 	uint8_t  colorspace; // 0 = sRGB with linear alpha, 1 = all channels linear
// };

// Images are encoded row by row, left to right, top to bottom. The decoder and
// encoder start with {r: 0, g: 0, b: 0, a: 255} as the previous pixel value. An
// image is complete when all pixels specified by width * height have been covered.

// Pixels are encoded as
//  - a run of the previous pixel
//  - an index into an array of previously seen pixels
//  - a difference to the previous pixel value in r,g,b
//  - full r,g,b or r,g,b,a values

// The color channels are assumed to not be premultiplied with the alpha channel
// ("un-premultiplied alpha").

// A running array[64] (zero-initialized) of previously seen pixel values is
// maintained by the encoder and decoder. Each pixel that is seen by the encoder
// and decoder is put into this array at the position formed by a hash function of
// the color value. In the encoder, if the pixel value at the index matches the
// current pixel, this index position is written to the stream as QOI_OP_INDEX.
// The hash function for the index is:

// 	index_position = (r * 3 + g * 5 + b * 7 + a * 11) % 64

// Each chunk starts with a 2- or 8-bit tag, followed by a number of data bits. The
// bit length of chunks is divisible by 8 - i.e. all chunks are byte aligned. All
// values encoded in these data bits have the most significant bit on the left.

// The 8-bit tags have precedence over the 2-bit tags. A decoder must check for the
// presence of an 8-bit tag first.

// The byte stream's end is marked with 7 0x00 bytes followed a single 0x01 byte.


// The possible chunks are:


// .- QOI_OP_INDEX ----------.
// |         Byte[0]         |
// |  7  6  5  4  3  2  1  0 |
// |-------+-----------------|
// |  0  0 |     index       |
// `-------------------------`
// 2-bit tag b00
// 6-bit index into the color index array: 0..63

// A valid encoder must not issue 2 or more consecutive QOI_OP_INDEX chunks to the
// same index. QOI_OP_RUN should be used instead.


// .- QOI_OP_DIFF -----------.
// |         Byte[0]         |
// |  7  6  5  4  3  2  1  0 |
// |-------+-----+-----+-----|
// |  0  1 |  dr |  dg |  db |
// `-------------------------`
// 2-bit tag b01
// 2-bit   red channel difference from the previous pixel between -2..1
// 2-bit green channel difference from the previous pixel between -2..1
// 2-bit  blue channel difference from the previous pixel between -2..1

// The difference to the current channel values are using a wraparound operation,
// so "1 - 2" will result in 255, while "255 + 1" will result in 0.

// Values are stored as uintegers with a bias of 2. E.g. -2 is stored as
// 0 (b00). 1 is stored as 3 (b11).

// The alpha value remains unchanged from the previous pixel.


// .- QOI_OP_LUMA -------------------------------------.
// |         Byte[0]         |         Byte[1]         |
// |  7  6  5  4  3  2  1  0 |  7  6  5  4  3  2  1  0 |
// |-------+-----------------+-------------+-----------|
// |  1  0 |  green diff     |   dr - dg   |  db - dg  |
// `---------------------------------------------------`
// 2-bit tag b10
// 6-bit green channel difference from the previous pixel -32..31
// 4-bit   red channel difference minus green channel difference -8..7
// 4-bit  blue channel difference minus green channel difference -8..7

// The green channel is used to indicate the general direction of change and is
// encoded in 6 bits. The red and blue channels (dr and db) base their diffs off
// of the green channel difference and are encoded in 4 bits. I.e.:
// 	dr_dg = (cur_px.r - prev_px.r) - (cur_px.g - prev_px.g)
// 	db_dg = (cur_px.b - prev_px.b) - (cur_px.g - prev_px.g)

// The difference to the current channel values are using a wraparound operation,
// so "10 - 13" will result in 253, while "250 + 7" will result in 1.

// Values are stored as uintegers with a bias of 32 for the green channel
// and a bias of 8 for the red and blue channel.

// The alpha value remains unchanged from the previous pixel.


// .- QOI_OP_RUN ------------.
// |         Byte[0]         |
// |  7  6  5  4  3  2  1  0 |
// |-------+-----------------|
// |  1  1 |       run       |
// `-------------------------`
// 2-bit tag b11
// 6-bit run-length repeating the previous pixel: 1..62

// The run-length is stored with a bias of -1. Note that the run-lengths 63 and 64
// (b111110 and b111111) are illegal as they are occupied by the QOI_OP_RGB and
// QOI_OP_RGBA tags.


// .- QOI_OP_RGB ------------------------------------------.
// |         Byte[0]         | Byte[1] | Byte[2] | Byte[3] |
// |  7  6  5  4  3  2  1  0 | 7 .. 0  | 7 .. 0  | 7 .. 0  |
// |-------------------------+---------+---------+---------|
// |  1  1  1  1  1  1  1  0 |   red   |  green  |  blue   |
// `-------------------------------------------------------`
// 8-bit tag b11111110
// 8-bit   red channel value
// 8-bit green channel value
// 8-bit  blue channel value

// The alpha value remains unchanged from the previous pixel.


// .- QOI_OP_RGBA ---------------------------------------------------.
// |         Byte[0]         | Byte[1] | Byte[2] | Byte[3] | Byte[4] |
// |  7  6  5  4  3  2  1  0 | 7 .. 0  | 7 .. 0  | 7 .. 0  | 7 .. 0  |
// |-------------------------+---------+---------+---------+---------|
// |  1  1  1  1  1  1  1  1 |   red   |  green  |  blue   |  alpha  |
// `-----------------------------------------------------------------`
// 8-bit tag b11111111
// 8-bit   red channel value
// 8-bit green channel value
// 8-bit  blue channel value
// 8-bit alpha channel value

// */


// /* -----------------------------------------------------------------------------
// Header - Public functions */
// using System;
// using System.Runtime.InteropServices;

// public unsafe  static class QOI{
// // #ifndef QOI_H
// // #define QOI_H

// // #ifdef __cplusplus
// // extern "C" {
// // #endif

// /* A pointer to a qoi_desc struct has to be supplied to all of qoi's functions.
// It describes either the input format (for qoi_write and qoi_encode), or is
// filled with the description read from the file header (for qoi_read and
// qoi_decode).

// The colorspace in this qoi_desc is an enum where
// 	0 = sRGB, i.e. gamma scaled RGB channels and a linear alpha channel
// 	1 = all channels are linear
// You may use the constants QOI_SRGB or QOI_LINEAR. The colorspace is purely
// informative. It will be saved to the file header, but does not affect
// how chunks are en-/decoded. */

// public const int QOI_SRGB   =0;
// public const int QOI_LINEAR =1;

// [StructLayout(LayoutKind.Sequential)]
// public unsafe struct qoi_desc{
// 	public uint width;
// 	public uint height;
// 	public byte channels;
// 	public byte colorspace;
// } 

// // #ifndef QOI_NO_STDIO

// /* Encode raw RGB or RGBA pixels into a QOI image and write it to the file
// system. The qoi_desc struct must be filled with the image width, height,
// number of channels (3 = RGB, 4 = RGBA) and the colorspace.

// The function returns 0 on failure (invalid parameters, or fopen or malloc
// failed) or the number of bytes written on success. */

// // int qoi_write(const char *filename, const void *data, const qoi_desc *desc);


// /* Read and decode a QOI image from the file system. If channels is 0, the
// number of channels from the file header is used. If channels is 3 or 4 the
// output format will be forced into this number of channels.

// The function either returns null on failure (invalid data, or malloc or fopen
// failed) or a pointer to the decoded pixels. On success, the qoi_desc struct
// will be filled with the description from the file header.

// The returned pixel data should be free()d after use. */

// // void *qoi_read(const char *filename, qoi_desc *desc, int channels);

// // #endif /* QOI_NO_STDIO */


// /* Encode raw RGB or RGBA pixels into a QOI image in memory.

// The function either returns null on failure (invalid parameters or malloc
// failed) or a pointer to the encoded data on success. On success the out_len
// is set to the size in bytes of the encoded data.

// The returned qoi data should be free()d after use. */

// // void *qoi_encode(const void *data, const qoi_desc *desc, int *out_len);


// /* Decode a QOI image from memory.

// The function either returns null on failure (invalid parameters or malloc
// failed) or a pointer to the decoded pixels. On success, the qoi_desc struct
// is filled with the description from the file header.

// The returned pixel data should be free()d after use. */

// // void *qoi_decode(const void *data, int size, qoi_desc *desc, int channels);


// // #ifdef __cplusplus
// // }
// // #endif
// // #endif /* QOI_H */


// /* -----------------------------------------------------------------------------
// Implementation */

// // #ifdef QOI_IMPLEMENTATION
// // #include <stdlib.h>
// // #include <string.h>

// // #ifndef QOI_MALLOC
// internal unsafe static void* QOI_MALLOC(long size)
// {
//     var ptr = Marshal.AllocHGlobal((int)size);
//     return ptr.ToPointer();
// }

// 	// #define QOI_FREE(p)    free(p)
//     internal static void QOI_FREE(void* a)
// 	{
// 		if (a == null)
// 			return;

// 		var ptr = new IntPtr(a);
// 		Marshal.FreeHGlobal(ptr);
// 		// AtomicStats.Freed();
// 	}
// // #endif
// // #ifndef QOI_ZEROARR
// 	// #define QOI_ZEROARR(a) memset((a),0,sizeof(a))
// internal static void memset(void* ptr, int value, long size)
// {
//     var bptr = (byte*)ptr;
//     var bval = (byte)value;
//     for (long i = 0; i < size; ++i)
//         *bptr++ = bval;
// }

// internal static void  QOI_ZEROARR(void* ptr, long size)
//     =>memset(ptr, 0, (long)size);
// // #endif

// public const byte QOI_OP_INDEX = 0x00; /* 00xxxxxx */
// public const byte QOI_OP_DIFF  = 0x40; /* 01xxxxxx */
// public const byte QOI_OP_LUMA  = 0x80; /* 10xxxxxx */
// public const byte QOI_OP_RUN   = 0xc0; /* 11xxxxxx */
// public const byte QOI_OP_RGB   = 0xfe; /* 11111110 */
// public const byte QOI_OP_RGBA  = 0xff; /* 11111111 */

// public const byte QOI_MASK_2   = 0xc0 ;/* 11000000 */

// // #define QOI_COLOR_HASH(C) (C.rgba.r*3 + C.rgba.g*5 + C.rgba.b*7 + C.rgba.a*11)
// public static uint QOI_COLOR_HASH(qoi_rgba_t C)
// {
//     return (uint)(C.r*3 + C.g*5 + C.b*7 + C.a*11 );
// }
// public static uint QOI_MAGIC => 	(((uint)'q') << 24 | ((uint)'o') << 16 |  ((uint)'i') <<  8 | ((uint)'f'));
// public const int QOI_HEADER_SIZE =14;

// /* 2GB is the max file size that this implementation can safely handle. We guard
// against anything larger than that, assuming the worst case with 5 bytes per
// pixel, rounded down to a nice clean value. 400 million pixels ought to be
// enough for anybody. */
// public  const uint QOI_PIXELS_MAX =((uint)400000000);


// [StructLayout(LayoutKind.Explicit)]
// public unsafe  struct qoi_rgba_t {
// 	[FieldOffset(0)]public fixed byte  rgba[4];
// 	[FieldOffset(0)]public byte  r;
//     [FieldOffset(1)]public byte  g;
//     [FieldOffset(2)]public byte  b;
//     [FieldOffset(3)]public byte  a;
// 	[FieldOffset(0)]public UInt32  v;
// }
// // typedef union {
// // 	struct { byte r, g, b, a; } rgba;
// // 	uint v;
// // } qoi_rgba_t;

// public static byte[] qoi_padding = new byte[8]{0,0,0,0,0,0,0,1};

// public unsafe static void qoi_write_32(byte* bytes, int* p, uint v) {
// 	bytes[(*p)++] = (byte)((0xff000000 & v) >> 24);
// 	bytes[(*p)++] = (byte)((0x00ff0000 & v) >> 16);
// 	bytes[(*p)++] = (byte)((0x0000ff00 & v) >> 8);
// 	bytes[(*p)++] = (byte)((0x000000ff & v));
// }

// public static uint qoi_read_32(byte *bytes, int *p) {
// 	uint a = bytes[(*p)++];
// 	uint b = bytes[(*p)++];
// 	uint c = bytes[(*p)++];
// 	uint d = bytes[(*p)++];
// 	return a << 24 | b << 16 | c << 8 | d;
// }

// // public static void *qoi_encode(void *data, qoi_desc *desc, int *out_len) {
// // 	int i, max_size, p, run;
// // 	int px_len, px_end, px_pos, channels;
// // 	byte *bytes;
// // 	byte *pixels;
// // 	qoi_rgba_t[] index = new qoi_rgba_t[64];
// // 	qoi_rgba_t px, px_prev;

// // 	if (
// // 		data == null || out_len == null || desc == null ||
// // 		desc->width == 0 || desc->height == 0 ||
// // 		desc->channels < 3 || desc->channels > 4 ||
// // 		desc->colorspace > 1 ||
// // 		desc->height >= QOI_PIXELS_MAX / desc->width
// // 	) {
// // 		return null;
// // 	}

// // 	max_size =
// // 		desc->width * desc->height * (desc->channels + 1) +
// // 		QOI_HEADER_SIZE + sizeof(qoi_padding);

// // 	p = 0;
// // 	bytes = (byte *) QOI_MALLOC(max_size);
// // 	if (bytes != null) {
// // 		return null;
// // 	}

// // 	qoi_write_32(bytes, &p, QOI_MAGIC);
// // 	qoi_write_32(bytes, &p, desc->width);
// // 	qoi_write_32(bytes, &p, desc->height);
// // 	bytes[p++] = desc->channels;
// // 	bytes[p++] = desc->colorspace;


// // 	pixels = ( byte *)data;

// // 	QOI_ZEROARR(index, index.Length);

// // 	run = 0;
// // 	px_prev.r = 0;
// // 	px_prev.g = 0;
// // 	px_prev.b = 0;
// // 	px_prev.a = 255;
// // 	px = px_prev;

// // 	px_len = desc->width * desc->height * desc->channels;
// // 	px_end = px_len - desc->channels;
// // 	channels = desc->channels;

// // 	for (px_pos = 0; px_pos < px_len; px_pos += channels) {
// // 		px.rgba.r = pixels[px_pos + 0];
// // 		px.rgba.g = pixels[px_pos + 1];
// // 		px.rgba.b = pixels[px_pos + 2];

// // 		if (channels == 4) {
// // 			px.rgba.a = pixels[px_pos + 3];
// // 		}

// // 		if (px.v == px_prev.v) {
// // 			run++;
// // 			if (run == 62 || px_pos == px_end) {
// // 				bytes[p++] = QOI_OP_RUN | (run - 1);
// // 				run = 0;
// // 			}
// // 		}
// // 		else {
// // 			int index_pos;

// // 			if (run > 0) {
// // 				bytes[p++] = QOI_OP_RUN | (run - 1);
// // 				run = 0;
// // 			}

// // 			index_pos = (int)QOI_COLOR_HASH(px) % 64;

// // 			if (index[index_pos].v == px.v) {
// // 				bytes[p++] = QOI_OP_INDEX | index_pos;
// // 			}
// // 			else {
// // 				index[index_pos] = px;

// // 				if (px.a == px_prev.a) {
// // 					sbyte vr = px.r - px_prev.r;
// // 					sbyte vg = px.g - px_prev.g;
// // 					sbyte vb = px.b - px_prev.b;

// // 					sbyte vg_r = vr - vg;
// // 					sbyte vg_b = vb - vg;

// // 					if (
// // 						vr > -3 && vr < 2 &&
// // 						vg > -3 && vg < 2 &&
// // 						vb > -3 && vb < 2
// // 					) {
// // 						bytes[p++] = QOI_OP_DIFF | (vr + 2) << 4 | (vg + 2) << 2 | (vb + 2);
// // 					}
// // 					else if (
// // 						vg_r >  -9 && vg_r <  8 &&
// // 						vg   > -33 && vg   < 32 &&
// // 						vg_b >  -9 && vg_b <  8
// // 					) {
// // 						bytes[p++] = QOI_OP_LUMA     | (vg   + 32);
// // 						bytes[p++] = (vg_r + 8) << 4 | (vg_b +  8);
// // 					}
// // 					else {
// // 						bytes[p++] = QOI_OP_RGB;
// // 						bytes[p++] = px.r;
// // 						bytes[p++] = px.g;
// // 						bytes[p++] = px.b;
// // 					}
// // 				}
// // 				else {
// // 					bytes[p++] = QOI_OP_RGBA;
// // 					bytes[p++] = px.r;
// // 					bytes[p++] = px.g;
// // 					bytes[p++] = px.b;
// // 					bytes[p++] = px.a;
// // 				}
// // 			}
// // 		}
// // 		px_prev = px;
// // 	}

// // 	for (i = 0; i < (int)sizeof(qoi_padding); i++) {
// // 		bytes[p++] = qoi_padding[i];
// // 	}

// // 	*out_len = p;
// // 	return bytes;
// // }

// // public static void *qoi_decode( void *data, int size, qoi_desc *desc, int channels) {
// // 	byte *bytes;
// // 	uint header_magic;
// // 	byte *pixels;
// // 	qoi_rgba_t[] index = new qoi_rgba_t[64];
// // 	qoi_rgba_t px;
// // 	int px_len, chunks_len, px_pos;
// // 	int p = 0, run = 0;

// // 	if (
// // 		data == null || desc == null ||
// // 		(channels != 0 && channels != 3 && channels != 4) ||
// // 		size < QOI_HEADER_SIZE + (int)sizeof(qoi_padding)
// // 	) {
// // 		return null;
// // 	}

// // 	bytes = ( byte *)data;

// // 	header_magic = qoi_read_32(bytes, &p);
// // 	desc->width = qoi_read_32(bytes, &p);
// // 	desc->height = qoi_read_32(bytes, &p);
// // 	desc->channels = bytes[p++];
// // 	desc->colorspace = bytes[p++];

// // 	if (
// // 		desc->width == 0 || desc->height == 0 ||
// // 		desc->channels < 3 || desc->channels > 4 ||
// // 		desc->colorspace > 1 ||
// // 		header_magic != QOI_MAGIC ||
// // 		desc->height >= QOI_PIXELS_MAX / desc->width
// // 	) {
// // 		return null;
// // 	}

// // 	if (channels == 0) {
// // 		channels = desc->channels;
// // 	}

// // 	px_len = desc->width * desc->height * channels;
// // 	pixels = (byte *) QOI_MALLOC(px_len);
// // 	if (pixels == null) {
// // 		return null;
// // 	}

// // 	QOI_ZEROARR(index, index.Length);
// // 	px.r = 0;
// // 	px.g = 0;
// // 	px.b = 0;
// // 	px.a = 255;

// // 	chunks_len = size - (int)sizeof(qoi_padding);
// // 	for (px_pos = 0; px_pos < px_len; px_pos += channels) {
// // 		if (run > 0) {
// // 			run--;
// // 		}
// // 		else if (p < chunks_len) {
// // 			int b1 = bytes[p++];

// // 			if (b1 == QOI_OP_RGB) {
// // 				px.r = bytes[p++];
// // 				px.g = bytes[p++];
// // 				px.b = bytes[p++];
// // 			}
// // 			else if (b1 == QOI_OP_RGBA) {
// // 				px.r = bytes[p++];
// // 				px.g = bytes[p++];
// // 				px.b = bytes[p++];
// // 				px.a = bytes[p++];
// // 			}
// // 			else if ((b1 & QOI_MASK_2) == QOI_OP_INDEX) {
// // 				px = index[b1];
// // 			}
// // 			else if ((b1 & QOI_MASK_2) == QOI_OP_DIFF) {
// // 				px.r += ((b1 >> 4) & 0x03) - 2;
// // 				px.g += ((b1 >> 2) & 0x03) - 2;
// // 				px.b += ( b1       & 0x03) - 2;
// // 			}
// // 			else if ((b1 & QOI_MASK_2) == QOI_OP_LUMA) {
// // 				int b2 = bytes[p++];
// // 				int vg = (b1 & 0x3f) - 32;
// // 				px.r += vg - 8 + ((b2 >> 4) & 0x0f);
// // 				px.g += vg;
// // 				px.b += vg - 8 +  (b2       & 0x0f);
// // 			}
// // 			else if ((b1 & QOI_MASK_2) == QOI_OP_RUN) {
// // 				run = (b1 & 0x3f);
// // 			}

// // 			index[QOI_COLOR_HASH(px) % 64] = px;
// // 		}

// // 		pixels[px_pos + 0] = px.r;
// // 		pixels[px_pos + 1] = px.g;
// // 		pixels[px_pos + 2] = px.b;
		
// // 		if (channels == 4) {
// // 			pixels[px_pos + 3] = px.a;
// // 		}
// // 	}

// // 	return pixels;
// // }

// // #ifndef QOI_NO_STDIO
// // #include <stdio.h>

// // int qoi_write(const char *filename, const void *data, const qoi_desc *desc) {
// // 	FILE *f = fopen(filename, "wb");
// // 	int size;
// // 	void *encoded;

// // 	if (!f) {
// // 		return 0;
// // 	}

// // 	encoded = qoi_encode(data, desc, &size);
// // 	if (!encoded) {
// // 		fclose(f);
// // 		return 0;
// // 	}

// // 	fwrite(encoded, 1, size, f);
// // 	fclose(f);

// // 	QOI_FREE(encoded);
// // 	return size;
// // }

// // void *qoi_read(const char *filename, qoi_desc *desc, int channels) {
// // 	FILE *f = fopen(filename, "rb");
// // 	int size, bytes_read;
// // 	void *pixels, *data;

// // 	if (!f) {
// // 		return null;
// // 	}

// // 	fseek(f, 0, SEEK_END);
// // 	size = ftell(f);
// // 	if (size <= 0) {
// // 		fclose(f);
// // 		return null;
// // 	}
// // 	fseek(f, 0, SEEK_SET);

// // 	data = QOI_MALLOC(size);
// // 	if (!data) {
// // 		fclose(f);
// // 		return null;
// // 	}

// // 	bytes_read = fread(data, 1, size, f);
// // 	fclose(f);

// // 	pixels = qoi_decode(data, bytes_read, desc, channels);
// // 	QOI_FREE(data);
// // 	return pixels;
// // }

// // #endif /* QOI_NO_STDIO */
// // #endif /* QOI_IMPLEMENTATION */


// }//END STATIC CLASS QOI