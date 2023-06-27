#pragma warning disable

namespace RitaEngine.Resources.Sound.MP3Sharp 
{

using System;
using RitaEngine.Resources.Sound.MP3Sharp.Decoding;
using System.IO;
using System.Runtime.Serialization;
using RitaEngine.Resources.Sound.MP3Sharp.Support;

    /// <summary>
    /// public class used to queue samples that are being obtained from an Mp3 stream. 
    /// This class handles stereo 16-bit output, and can double 16-bit mono to stereo.
    /// </summary>
    public class Buffer16BitStereo : ABuffer {
        internal bool DoubleMonoToStereo = false;

        private const int OUTPUT_CHANNELS = 2;

        // Write offset used in append_bytes
        private readonly byte[] _Buffer = new byte[OBUFFERSIZE * 2]; // all channels interleaved
        private readonly int[] _BufferChannelOffsets = new int[MAXCHANNELS]; // contains write offset for each channel.

        // end marker, one past end of array. Same as bufferp[0], but
        // without the array bounds check.
        private int _End;

        // Read offset used to read from the stream, in bytes.
        private int _Offset;

        internal Buffer16BitStereo() {
            // Initialize the buffer pointers
            ClearBuffer();
        }

        /// <summary>
        /// Gets the number of bytes remaining from the current position on the buffer.
        /// </summary>
        internal int BytesLeft => _End - _Offset;

        /// <summary>
        /// Reads a sequence of bytes from the buffer and advances the position of the 
        /// buffer by the number of bytes read.
        /// </summary>
        /// <returns>
        /// The total number of bytes read in to the buffer. This can be less than the
        /// number of bytes requested if that many bytes are not currently available, or
        /// zero if th eend of the buffer has been reached.
        /// </returns>
        internal int Read(byte[] bufferOut, int offset, int count) {
            if (bufferOut == null) {
                throw new ArgumentNullException(nameof(bufferOut));
            }
            if ((count + offset) > bufferOut.Length) {
                throw new ArgumentException("The sum of offset and count is larger than the buffer length");
            }
            int remaining = BytesLeft;
            int copySize;
            if (count > remaining) {
                copySize = remaining;
            }
            else {
                // Copy an even number of sample frames
                int remainder = count % (2 * OUTPUT_CHANNELS);
                copySize = count - remainder;
            }
            Array.Copy(_Buffer, _Offset, bufferOut, offset, copySize);
            _Offset += copySize;
            return copySize;
        }

        /// <summary>
        /// Writes a single sample value to the buffer.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="sampleValue">The sample value.</param>
        protected override void Append(int channel, short sampleValue) {
            _Buffer[_BufferChannelOffsets[channel]] = (byte)(sampleValue & 0xff);
            _Buffer[_BufferChannelOffsets[channel] + 1] = (byte)(sampleValue >> 8);
            _BufferChannelOffsets[channel] += OUTPUT_CHANNELS * 2;
        }

        /// <summary>
        /// Writes 32 samples to the buffer.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="samples">An array of sample values.</param>
        /// <remarks>
        /// The <paramref name="samples"/> parameter must have a length equal to
        /// or greater than 32.
        /// </remarks>
        internal override void AppendSamples(int channel, float[] samples) {
            if (samples == null) {
                // samples is required.
                throw new ArgumentNullException(nameof(samples));
            }
            if (samples.Length < 32) {
                throw new ArgumentException("samples must have 32 values");
            }
            if (_BufferChannelOffsets == null || channel >= _BufferChannelOffsets.Length) {
                throw new Exception("Song is closing...");
            }
            int pos = _BufferChannelOffsets[channel];
            // Always, 32 samples are appended
            for (int i = 0; i < 32; i++) {
                float fs = samples[i];
                // clamp values
                if (fs > 32767.0f) {
                    fs = 32767.0f;
                }
                else if (fs < -32767.0f) {
                    fs = -32767.0f;
                }
                int sample = (int)fs;
                _Buffer[pos] = (byte)(sample & 0xff);
                _Buffer[pos + 1] = (byte)(sample >> 8);
                if (DoubleMonoToStereo) {
                    _Buffer[pos + 2] = (byte)(sample & 0xff);
                    _Buffer[pos + 3] = (byte)(sample >> 8);
                }
                pos += OUTPUT_CHANNELS * 2;
            }
            _BufferChannelOffsets[channel] = pos;
        }

        /// <summary>
        /// This implementation does not clear the buffer.
        /// </summary>
        internal sealed override void ClearBuffer() {
            _Offset = 0;
            _End = 0;
            for (int i = 0; i < OUTPUT_CHANNELS; i++)
                _BufferChannelOffsets[i] = i * 2; // two bytes per channel
        }

        internal override void SetStopFlag() { }

        internal override void WriteBuffer(int val) {
            _Offset = 0;
            // speed optimization - save end marker, and avoid
            // array access at read time. Can you believe this saves
            // like 1-2% of the cpu on a PIII? I guess allocating
            // that temporary "new int(0)" is expensive, too.
            _End = _BufferChannelOffsets[0];
        }

        internal override void Close() { }
    }

    /// <summary>
    /// Describes sound formats that can be produced by the Mp3Stream class.
    /// </summary>
    public enum SoundFormat {
        /// <summary>
        /// PCM encoded, 16-bit Mono sound format.
        /// </summary>
        Pcm16BitMono,

        /// <summary>
        /// PCM encoded, 16-bit Stereo sound format.
        /// </summary>
        Pcm16BitStereo
    }

    /// <summary>
    /// Provides a view of the sequence of bytes that are produced during the conversion of an MP3 stream
    /// into a 16-bit PCM-encoded ("WAV" format) stream.
    /// </summary>
    public class MP3Stream : Stream {
        // Used to interface with JavaZoom code.
        private readonly Bitstream _BitStream;

        private readonly Decoder _Decoder = new Decoder(Decoder.DefaultParams);

        // local variables.
        private readonly Buffer16BitStereo _Buffer;
        private readonly Stream _SourceStream;
        private const int BACK_STREAM_BYTE_COUNT_REP = 0;
        private short _ChannelCountRep = -1;
        private readonly SoundFormat FormatRep;
        private int _FrequencyRep = -1;

        public bool IsEOF { get; protected set; }

        /// <summary>
        /// Creates a new stream instance using the provided filename, and the default chunk size of 4096 bytes.
        /// </summary>
        public MP3Stream(string fileName)
            : this(new FileStream(fileName, FileMode.Open, FileAccess.Read)) { }

        /// <summary>
        /// Creates a new stream instance using the provided filename and chunk size.
        /// </summary>
        public MP3Stream(string fileName, int chunkSize)
            : this(new FileStream(fileName, FileMode.Open, FileAccess.Read), chunkSize) { }

        /// <summary>
        /// Creates a new stream instance using the provided stream as a source, and the default chunk size of 4096 bytes.
        /// </summary>
        public MP3Stream(Stream sourceStream) : this(sourceStream, 4096) { }

        /// <summary>
        /// Creates a new stream instance using the provided stream as a source.
        /// Will also read the first frame of the MP3 into the internal buffer.
        /// </summary>
        public MP3Stream(Stream sourceStream, int chunkSize) {
            IsEOF = false;
            _SourceStream = sourceStream;
            _BitStream = new Bitstream(new PushbackStream(_SourceStream, chunkSize));
            _Buffer = new Buffer16BitStereo();
            _Decoder.OutputBuffer = _Buffer;
            // read the first frame. This will fill the initial buffer with data, and get our frequency!
            IsEOF |= !ReadFrame();
            switch (_ChannelCountRep) {
                case 1:
                    FormatRep = SoundFormat.Pcm16BitMono;
                    break;
                case 2:
                    FormatRep = SoundFormat.Pcm16BitStereo;
                    break;
                default:
                    throw new MP3SharpException($"Unhandled channel count rep: {_ChannelCountRep} (allowed values are 1-mono and 2-stereo).");
            }
            if (FormatRep == SoundFormat.Pcm16BitMono) {
                _Buffer.DoubleMonoToStereo = true;
            }
        }

        /// <summary>
        /// Gets the chunk size.
        /// </summary>
        internal int ChunkSize => BACK_STREAM_BYTE_COUNT_REP;

        /// <summary>
        /// Gets a value indicating whether the current stream supports reading.
        /// </summary>
        public override bool CanRead => _SourceStream.CanRead;

        /// <summary>
        /// Gets a value indicating whether the current stream supports seeking.
        /// </summary>
        public override bool CanSeek => _SourceStream.CanSeek;

        /// <summary>
        /// Gets a value indicating whether the current stream supports writing.
        /// </summary>
        public override bool CanWrite => _SourceStream.CanWrite;

        /// <summary>
        /// Gets the length in bytes of the stream.
        /// </summary>
        public override long Length => _SourceStream.Length;

        /// <summary>
        /// Gets or sets the position of the source stream.  This is relative to the number of bytes in the MP3 file, rather
        /// than the total number of PCM bytes (typically signicantly greater) contained in the Mp3Stream's output.
        /// </summary>
        public override long Position {
            get => _SourceStream.Position;
            set {
                if (value < 0)
                    value = 0;
                if (value > _SourceStream.Length)
                    value = _SourceStream.Length;
                _SourceStream.Position = value;
                IsEOF = false;
                IsEOF |= !ReadFrame();
            }
        }

        /// <summary>
        /// Gets the frequency of the audio being decoded. Updated every call to Read() or DecodeFrames(),
        /// to reflect the most recent header information from the MP3 Stream.
        /// </summary>
        public int Frequency => _FrequencyRep;

        /// <summary>
        /// Gets the number of channels available in the audio being decoded. Updated every call to Read() or DecodeFrames(),
        /// to reflect the most recent header information from the MP3 Stream.
        /// </summary>
        internal short ChannelCount => _ChannelCountRep;

        /// <summary>
        /// Gets the PCM output format of this stream.
        /// </summary>
        internal SoundFormat Format => FormatRep;

        /// <summary>
        /// Clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        public override void Flush() {
            _SourceStream.Flush();
        }

        /// <summary>
        /// Sets the position of the source stream.
        /// </summary>
        public override long Seek(long offset, SeekOrigin origin) {
            return _SourceStream.Seek(offset, origin);
        }

        /// <summary>
        /// This method is not valid for an Mp3Stream.
        /// </summary>
        public override void SetLength(long value) {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// This method is not valid for an Mp3Stream.
        /// </summary>
        public override void Write(byte[] buffer, int offset, int count) {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Decodes the requested number of frames from the MP3 stream and caches their PCM-encoded bytes.
        /// These can subsequently be obtained using the Read method.
        /// Returns the number of frames that were successfully decoded.
        /// </summary>
        internal int DecodeFrames(int frameCount) {
            int framesDecoded = 0;
            bool aFrameWasRead = true;
            while (framesDecoded < frameCount && aFrameWasRead) {
                aFrameWasRead = ReadFrame();
                if (aFrameWasRead) framesDecoded++;
            }
            return framesDecoded;
        }

        /// <summary>
        /// Reads the MP3 stream as PCM-encoded bytes.  Decodes a portion of the stream if necessary.
        /// Returns the number of bytes read.
        /// </summary>
        public override int Read(byte[] buffer, int offset, int count) {
            // Copy from queue buffers, reading new ones as necessary,
            // until we can't read more or we have read "count" bytes
            if (IsEOF)
                return 0;

            int bytesRead = 0;
            while (true) {
                if (_Buffer.BytesLeft <= 0) {
                    if (!ReadFrame()) // out of frames or end of stream?
                    {
                        IsEOF = true;
                        _BitStream.CloseFrame();
                        break;
                    }
                }

                // Copy as much as we can from the current buffer:
                bytesRead += _Buffer.Read(buffer,
                    offset + bytesRead,
                    count - bytesRead);

                if (bytesRead >= count)
                    break;
            }
            return bytesRead;
        }

        /// <summary>
        /// Closes the source stream and releases any associated resources.
        /// If you don't call this, you may be leaking file descriptors.
        /// </summary>
        public override void Close() {
            _BitStream.Close(); // This should close SourceStream as well.
        }

        /// <summary>
        /// Reads a frame from the MP3 stream.  Returns whether the operation was successful.  If it wasn't,
        /// the source stream is probably at its end.
        /// </summary>
        private bool ReadFrame() {
            // Read a frame from the bitstream.
            Header header = _BitStream.ReadFrame();
            if (header == null)
                return false;

            try {
                // Set the channel count and frequency values for the stream.
                if (header.Mode() == Header.SINGLE_CHANNEL)
                    _ChannelCountRep = 1;
                else
                    _ChannelCountRep = 2;
                _FrequencyRep = header.Frequency();
                // Decode the frame.
                ABuffer decoderOutput = _Decoder.DecodeFrame(header, _BitStream);
                if (decoderOutput != _Buffer) {
                    throw new ApplicationException("Output buffers are different.");
                }
            }
            finally {
                // No resource leaks please!
                _BitStream.CloseFrame();
            }
            return true;
        }
    }

    /// <summary>
    /// MP3SharpException is the base class for all API-level
    /// exceptions thrown by MP3Sharp. To facilitate conversion and
    /// common handling of exceptions from other domains, the class
    /// can delegate some functionality to a contained Throwable instance.
    /// </summary>
    // [Serializable] 
    public class MP3SharpException : Exception {
        internal MP3SharpException() { }

        internal MP3SharpException(string message) : base(message) { }

        internal MP3SharpException(string message, Exception inner) : base(message, inner) { }

        protected MP3SharpException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        internal void PrintStackTrace() {
            SupportClass.WriteStackTrace(this, Console.Error);
        }

        internal void PrintStackTrace(StreamWriter ps) {
            if (InnerException == null) {
                SupportClass.WriteStackTrace(this, ps);
            }
            else {
                SupportClass.WriteStackTrace(InnerException, Console.Error);
            }
        }
    }



}

namespace RitaEngine.Resources.Sound.MP3Sharp.Support {
    
using System;
using System.IO;

    public class SupportClass {
        internal static int URShift(int number, int bits) {
            if (number >= 0)
                return number >> bits;
            return (number >> bits) + (2 << ~bits);
        }

        internal static int URShift(int number, long bits) {
            return URShift(number, (int)bits);
        }

        internal static long URShift(long number, int bits) {
            if (number >= 0)
                return number >> bits;
            return (number >> bits) + (2L << ~bits);
        }

        internal static long URShift(long number, long bits) {
            return URShift(number, (int)bits);
        }

        internal static void WriteStackTrace(Exception throwable, TextWriter stream) {
            stream.Write(throwable.StackTrace);
            stream.Flush();
        }

        /// <summary>
        /// This method is used as a dummy method to simulate VJ++ behavior
        /// </summary>
        /// <param name="literal">The literal to return</param>
        /// <returns>The received value</returns>
        internal static long Identity(long literal) {
            return literal;
        }

        /// <summary>
        /// This method is used as a dummy method to simulate VJ++ behavior
        /// </summary>
        /// <param name="literal">The literal to return</param>
        /// <returns>The received value</returns>
        internal static ulong Identity(ulong literal) {
            return literal;
        }

        /// <summary>
        /// This method is used as a dummy method to simulate VJ++ behavior
        /// </summary>
        /// <param name="literal">The literal to return</param>
        /// <returns>The received value</returns>
        internal static float Identity(float literal) {
            return literal;
        }

        /// <summary>
        /// This method is used as a dummy method to simulate VJ++ behavior
        /// </summary>
        /// <param name="literal">The literal to return</param>
        /// <returns>The received value</returns>
        internal static double Identity(double literal) {
            return literal;
        }

        /// <summary>
        /// Reads a number of characters from the current source Stream and writes the data to the target array at the
        /// specified index.
        /// </summary>
        /// <param name="sourceStream">The source Stream to read from</param>
        /// <param name="target">Contains the array of characteres read from the source Stream.</param>
        /// <param name="start">The starting index of the target array.</param>
        /// <param name="count">The maximum number of characters to read from the source Stream.</param>
        /// <returns>
        /// The number of characters read. The number will be less than or equal to count depending on the data available
        /// in the source Stream.
        /// </returns>
        internal static int ReadInput(Stream sourceStream, ref sbyte[] target, int start, int count) {
            byte[] receiver = new byte[target.Length];
            int bytesRead = sourceStream.Read(receiver, start, count);

            for (int i = start; i < start + bytesRead; i++)
                target[i] = (sbyte)receiver[i];

            return bytesRead;
        }

        /// <summary>
        /// Converts an array of sbytes to an array of bytes
        /// </summary>
        /// <param name="sbyteArray">The array of sbytes to be converted</param>
        /// <returns>The new array of bytes</returns>
        internal static byte[] ToByteArray(sbyte[] sbyteArray) {
            byte[] byteArray = new byte[sbyteArray.Length];
            for (int index = 0; index < sbyteArray.Length; index++)
                byteArray[index] = (byte)sbyteArray[index];
            return byteArray;
        }

        /// <summary>
        /// Converts a string to an array of bytes
        /// </summary>
        /// <param name="sourceString">The string to be converted</param>
        /// <returns>The new array of bytes</returns>
        internal static byte[] ToByteArray(string sourceString) {
            byte[] byteArray = new byte[sourceString.Length];
            for (int index = 0; index < sourceString.Length; index++)
                byteArray[index] = (byte)sourceString[index];
            return byteArray;
        }

        /// <summary>
        /// Method that copies an array of sbytes from a String to a received array.
        /// </summary>
        /// <param name="sourceString">The String to get the sbytes.</param>
        /// <param name="sourceStart">Position in the String to start getting sbytes.</param>
        /// <param name="sourceEnd">Position in the String to end getting sbytes.</param>
        /// <param name="destinationArray">Array to store the bytes.</param>
        /// <param name="destinationStart">Position in the destination array to start storing the sbytes.</param>
        /// <returns>An array of sbytes</returns>
        internal static void GetSBytesFromString(string sourceString, int sourceStart, int sourceEnd,
            ref sbyte[] destinationArray, int destinationStart) {
            int sourceCounter;
            int destinationCounter;
            sourceCounter = sourceStart;
            destinationCounter = destinationStart;
            while (sourceCounter < sourceEnd) {
                destinationArray[destinationCounter] = (sbyte)sourceString[sourceCounter];
                sourceCounter++;
                destinationCounter++;
            }
        }
    }
}

namespace RitaEngine.Resources.Sound.MP3Sharp.IO {

using System;
using RitaEngine.Resources.Sound.MP3Sharp.Decoding;
using System.IO;
using System.Runtime.Serialization;
using RitaEngine.Resources.Sound.MP3Sharp.Support;
    /// <summary>
    /// public class allowing WaveFormat Access
    /// </summary>
    public class WaveFile : RiffFile {
        internal const int MAX_WAVE_CHANNELS = 2;
        private readonly int _NumSamples;
        private readonly RiffChunkHeader _PcmData;
        private readonly WaveFormatChunk _WaveFormat;
        private bool _JustWriteLengthBytes;
        private long _PcmDataOffset; // offset of 'pc_data' in output file

        /// <summary>
        /// Creates a new WaveFile instance.
        /// </summary>
        internal WaveFile() {
            _PcmData = new RiffChunkHeader(this);
            _WaveFormat = new WaveFormatChunk(this);
            _PcmData.CkId = FourCC("data");
            _PcmData.CkSize = 0;
            _NumSamples = 0;
        }

        /// <summary>
        /// Pass in either a FileName or a Stream.
        /// </summary>
        internal virtual int OpenForWrite(string filename, Stream stream, int samplingRate, short bitsPerSample,
            short numChannels) {
            // Verify parameters...
            if ((bitsPerSample != 8 && bitsPerSample != 16) || numChannels < 1 || numChannels > 2) {
                return DDC_INVALID_CALL;
            }

            _WaveFormat.Data.Config(samplingRate, bitsPerSample, numChannels);

            int retcode = DDC_SUCCESS;
            if (stream != null)
                Open(stream, RF_WRITE);
            else
                Open(filename, RF_WRITE);

            if (retcode == DDC_SUCCESS) {
                sbyte[] theWave = {
                    (sbyte)SupportClass.Identity('W'), (sbyte)SupportClass.Identity('A'),
                    (sbyte)SupportClass.Identity('V'), (sbyte)SupportClass.Identity('E')
                };
                retcode = Write(theWave, 4);

                if (retcode == DDC_SUCCESS) {
                    // Ecriture de wave_format
                    retcode = Write(_WaveFormat.Header, 8);
                    retcode = Write(_WaveFormat.Data.FormatTag, 2);
                    retcode = Write(_WaveFormat.Data.NumChannels, 2);
                    retcode = Write(_WaveFormat.Data.NumSamplesPerSec, 4);
                    retcode = Write(_WaveFormat.Data.NumAvgBytesPerSec, 4);
                    retcode = Write(_WaveFormat.Data.NumBlockAlign, 2);
                    retcode = Write(_WaveFormat.Data.NumBitsPerSample, 2);

                    if (retcode == DDC_SUCCESS) {
                        _PcmDataOffset = CurrentFilePosition();
                        retcode = Write(_PcmData, 8);
                    }
                }
            }

            return retcode;
        }

        /// <summary>
        /// Write 16-bit audio
        /// </summary>
        internal virtual int WriteData(short[] data, int numData) {
            int extraBytes = numData * 2;
            _PcmData.CkSize += extraBytes;
            return Write(data, extraBytes);
        }

        internal override int Close() {
            int rc = DDC_SUCCESS;

            if (Fmode == RF_WRITE)
                rc = Backpatch(_PcmDataOffset, _PcmData, 8);
            if (!_JustWriteLengthBytes) {
                if (rc == DDC_SUCCESS)
                    rc = base.Close();
            }
            return rc;
        }

        internal int Close(bool justWriteLengthBytes) {
            _JustWriteLengthBytes = justWriteLengthBytes;
            int ret = Close();
            _JustWriteLengthBytes = false;
            return ret;
        }

        // [Hz]
        internal virtual int SamplingRate() {
            return _WaveFormat.Data.NumSamplesPerSec;
        }

        internal virtual short BitsPerSample() {
            return _WaveFormat.Data.NumBitsPerSample;
        }

        internal virtual short NumChannels() {
            return _WaveFormat.Data.NumChannels;
        }

        internal virtual int NumSamples() {
            return _NumSamples;
        }

        /// <summary>
        /// Open for write using another wave file's parameters...
        /// </summary>
        internal virtual int OpenForWrite(string filename, WaveFile otherWave) {
            return OpenForWrite(filename, null, otherWave.SamplingRate(), otherWave.BitsPerSample(),
                otherWave.NumChannels());
        }

        internal sealed class WaveFormatChunkData {
            private WaveFile _EnclosingInstance;
            internal int NumAvgBytesPerSec;
            internal short NumBitsPerSample;
            internal short NumBlockAlign;
            internal short NumChannels; // Number of channels (mono=1, stereo=2)
            internal int NumSamplesPerSec; // Sampling rate [Hz]
            internal short FormatTag; // Format category (PCM=1)

            internal WaveFormatChunkData(WaveFile enclosingInstance) {
                InitBlock(enclosingInstance);
                FormatTag = 1; // PCM
                Config(44100, 16, 1);
            }

            internal WaveFile EnclosingInstance => _EnclosingInstance;

            private void InitBlock(WaveFile enclosingInstance) {
                _EnclosingInstance = enclosingInstance;
            }

            internal void Config(int newSamplingRate, short newBitsPerSample, short newNumChannels) {
                NumSamplesPerSec = newSamplingRate;
                NumChannels = newNumChannels;
                NumBitsPerSample = newBitsPerSample;
                NumAvgBytesPerSec = (NumChannels * NumSamplesPerSec * NumBitsPerSample) / 8;
                NumBlockAlign = (short)((NumChannels * NumBitsPerSample) / 8);
            }
        }

        public class WaveFormatChunk {
            internal WaveFormatChunkData Data;
            private WaveFile _EnclosingInstance;
            internal RiffChunkHeader Header;

            internal WaveFormatChunk(WaveFile enclosingInstance) {
                InitBlock(enclosingInstance);
                Header = new RiffChunkHeader(enclosingInstance);
                Data = new WaveFormatChunkData(enclosingInstance);
                Header.CkId = FourCC("fmt ");
                Header.CkSize = 16;
            }

            internal WaveFile EnclosingInstance => _EnclosingInstance;

            private void InitBlock(WaveFile enclosingInstance) {
                _EnclosingInstance = enclosingInstance;
            }

            internal virtual int VerifyValidity() {
                bool ret = Header.CkId == FourCC("fmt ") && (Data.NumChannels == 1 || Data.NumChannels == 2) &&
                           Data.NumAvgBytesPerSec == (Data.NumChannels * Data.NumSamplesPerSec * Data.NumBitsPerSample) / 8 &&
                           Data.NumBlockAlign == (Data.NumChannels * Data.NumBitsPerSample) / 8;
                return ret ? 1 : 0;
            }
        }

        public class WaveFileSample {
            internal short[] Chan;
            private WaveFile _EnclosingInstance;

            internal WaveFileSample(WaveFile enclosingInstance) {
                InitBlock(enclosingInstance);
                Chan = new short[MAX_WAVE_CHANNELS];
            }

            internal WaveFile EnclosingInstance => _EnclosingInstance;

            private void InitBlock(WaveFile enclosingInstance) {
                _EnclosingInstance = enclosingInstance;
            }
        }
    }

    /// <summary> Implements an Obuffer by writing the data to a file in RIFF WAVE format.</summary>
    public class WaveFileBuffer : ABuffer {
        private readonly short[] _Buffer;
        private readonly short[] _Bufferp;
        private readonly int _Channels;
        private readonly WaveFile _OutWave;

        internal WaveFileBuffer(int numberOfChannels, int freq, string fileName) {
            if (fileName == null)
                throw new NullReferenceException("FileName");

            _Buffer = new short[OBUFFERSIZE];
            _Bufferp = new short[MAXCHANNELS];
            _Channels = numberOfChannels;

            for (int i = 0; i < numberOfChannels; ++i)
                _Bufferp[i] = (short)i;

            _OutWave = new WaveFile();

            int rc = _OutWave.OpenForWrite(fileName, null, freq, 16, (short)_Channels);
        }

        internal WaveFileBuffer(int numberOfChannels, int freq, Stream stream) {
            _Buffer = new short[OBUFFERSIZE];
            _Bufferp = new short[MAXCHANNELS];
            _Channels = numberOfChannels;

            for (int i = 0; i < numberOfChannels; ++i)
                _Bufferp[i] = (short)i;

            _OutWave = new WaveFile();

            int rc = _OutWave.OpenForWrite(null, stream, freq, 16, (short)_Channels);
        }

        /// <summary>
        /// Takes a 16 Bit PCM sample.
        /// </summary>
        protected override void Append(int channel, short valueRenamed) {
            _Buffer[_Bufferp[channel]] = valueRenamed;
            _Bufferp[channel] = (short)(_Bufferp[channel] + _Channels);
        }

        internal override void WriteBuffer(int val) {
            int rc = _OutWave.WriteData(_Buffer, _Bufferp[0]);
            for (int i = 0; i < _Channels; ++i)
                _Bufferp[i] = (short)i;
        }

        internal void Close(bool justWriteLengthBytes) {
            _OutWave.Close(justWriteLengthBytes);
        }

        internal override void Close() {
            _OutWave.Close();
        }

        /// <summary>
        /// *
        /// </summary>
        internal override void ClearBuffer() { }

        /// <summary>
        /// *
        /// </summary>
        internal override void SetStopFlag() { }
    }
    /// <summary>
    /// public class to manage RIFF files
    /// </summary>
    public class RiffFile {
        protected const int DDC_SUCCESS = 0; // The operation succeded
        protected const int DDC_FAILURE = 1; // The operation failed for unspecified reasons
        protected const int DDC_OUT_OF_MEMORY = 2; // Operation failed due to running out of memory
        protected const int DDC_FILE_ERROR = 3; // Operation encountered file I/O error
        protected const int DDC_INVALID_CALL = 4; // Operation was called with invalid parameters
        protected const int DDC_USER_ABORT = 5; // Operation was aborted by the user
        protected const int DDC_INVALID_FILE = 6; // File format does not match
        protected const int RF_UNKNOWN = 0; // undefined type (can use to mean "N/A" or "not open")
        protected const int RF_WRITE = 1; // open for write
        protected const int RF_READ = 2; // open for read
        private readonly RiffChunkHeader _RiffHeader; // header for whole file
        protected int Fmode; // current file I/O mode
        private Stream _File; // I/O stream to use

        internal RiffFile() {
            _File = null;
            Fmode = RF_UNKNOWN;
            _RiffHeader = new RiffChunkHeader(this);

            _RiffHeader.CkId = FourCC("RIFF");
            _RiffHeader.CkSize = 0;
        }

        /// <summary>
        /// Return File Mode.
        /// </summary>
        internal int CurrentFileMode() => Fmode;

        /// <summary>
        /// Open a RIFF file.
        /// </summary>
        internal virtual int Open(string filename, int newMode) {
            int retcode = DDC_SUCCESS;

            if (Fmode != RF_UNKNOWN) {
                retcode = Close();
            }

            if (retcode == DDC_SUCCESS) {
                switch (newMode) {
                    case RF_WRITE:
                        try {
                            _File = RandomAccessFileStream.CreateRandomAccessFile(filename, "rw");

                            try {
                                // Write the RIFF header...
                                // We will have to come back later and patch it!
                                sbyte[] br = new sbyte[8];
                                br[0] = (sbyte)((SupportClass.URShift(_RiffHeader.CkId, 24)) & 0x000000FF);
                                br[1] = (sbyte)((SupportClass.URShift(_RiffHeader.CkId, 16)) & 0x000000FF);
                                br[2] = (sbyte)((SupportClass.URShift(_RiffHeader.CkId, 8)) & 0x000000FF);
                                br[3] = (sbyte)(_RiffHeader.CkId & 0x000000FF);

                                sbyte br4 = (sbyte)((SupportClass.URShift(_RiffHeader.CkSize, 24)) & 0x000000FF);
                                sbyte br5 = (sbyte)((SupportClass.URShift(_RiffHeader.CkSize, 16)) & 0x000000FF);
                                sbyte br6 = (sbyte)((SupportClass.URShift(_RiffHeader.CkSize, 8)) & 0x000000FF);
                                sbyte br7 = (sbyte)(_RiffHeader.CkSize & 0x000000FF);

                                br[4] = br7;
                                br[5] = br6;
                                br[6] = br5;
                                br[7] = br4;

                                _File.Write(SupportClass.ToByteArray(br), 0, 8);
                                Fmode = RF_WRITE;
                            }
                            catch {
                                _File.Close();
                                Fmode = RF_UNKNOWN;
                            }
                        }
                        catch {
                            Fmode = RF_UNKNOWN;
                            retcode = DDC_FILE_ERROR;
                        }
                        break;

                    case RF_READ:
                        try {
                            _File = RandomAccessFileStream.CreateRandomAccessFile(filename, "r");
                            try {
                                // Try to read the RIFF header...
                                sbyte[] br = new sbyte[8];
                                SupportClass.ReadInput(_File, ref br, 0, 8);
                                Fmode = RF_READ;
                                _RiffHeader.CkId = ((br[0] << 24) & (int)SupportClass.Identity(0xFF000000)) |
                                                   ((br[1] << 16) & 0x00FF0000) | ((br[2] << 8) & 0x0000FF00) |
                                                   (br[3] & 0x000000FF);
                                _RiffHeader.CkSize = ((br[4] << 24) & (int)SupportClass.Identity(0xFF000000)) |
                                                     ((br[5] << 16) & 0x00FF0000) | ((br[6] << 8) & 0x0000FF00) |
                                                     (br[7] & 0x000000FF);
                            }
                            catch {
                                _File.Close();
                                Fmode = RF_UNKNOWN;
                            }
                        }
                        catch {
                            Fmode = RF_UNKNOWN;
                            retcode = DDC_FILE_ERROR;
                        }
                        break;

                    default:
                        retcode = DDC_INVALID_CALL;
                        break;
                }
            }
            return retcode;
        }

        /// <summary>
        /// Open a RIFF STREAM.
        /// </summary>
        internal virtual int Open(Stream stream, int newMode) {
            int retcode = DDC_SUCCESS;

            if (Fmode != RF_UNKNOWN) {
                retcode = Close();
            }

            if (retcode == DDC_SUCCESS) {
                switch (newMode) {
                    case RF_WRITE:
                        try {
                            //file = SupportClass.RandomAccessFileSupport.CreateRandomAccessFile(Filename, "rw");
                            _File = stream;

                            try {
                                // Write the RIFF header...
                                // We will have to come back later and patch it!
                                sbyte[] br = new sbyte[8];
                                br[0] = (sbyte)((SupportClass.URShift(_RiffHeader.CkId, 24)) & 0x000000FF);
                                br[1] = (sbyte)((SupportClass.URShift(_RiffHeader.CkId, 16)) & 0x000000FF);
                                br[2] = (sbyte)((SupportClass.URShift(_RiffHeader.CkId, 8)) & 0x000000FF);
                                br[3] = (sbyte)(_RiffHeader.CkId & 0x000000FF);

                                sbyte br4 = (sbyte)((SupportClass.URShift(_RiffHeader.CkSize, 24)) & 0x000000FF);
                                sbyte br5 = (sbyte)((SupportClass.URShift(_RiffHeader.CkSize, 16)) & 0x000000FF);
                                sbyte br6 = (sbyte)((SupportClass.URShift(_RiffHeader.CkSize, 8)) & 0x000000FF);
                                sbyte br7 = (sbyte)(_RiffHeader.CkSize & 0x000000FF);

                                br[4] = br7;
                                br[5] = br6;
                                br[6] = br5;
                                br[7] = br4;

                                _File.Write(SupportClass.ToByteArray(br), 0, 8);
                                Fmode = RF_WRITE;
                            }
                            catch {
                                _File.Close();
                                Fmode = RF_UNKNOWN;
                            }
                        }
                        catch {
                            Fmode = RF_UNKNOWN;
                            retcode = DDC_FILE_ERROR;
                        }
                        break;

                    case RF_READ:
                        try {
                            _File = stream;
                            //file = SupportClass.RandomAccessFileSupport.CreateRandomAccessFile(Filename, "r");
                            try {
                                // Try to read the RIFF header... 
                                sbyte[] br = new sbyte[8];
                                SupportClass.ReadInput(_File, ref br, 0, 8);
                                Fmode = RF_READ;
                                _RiffHeader.CkId = ((br[0] << 24) & (int)SupportClass.Identity(0xFF000000)) |
                                                   ((br[1] << 16) & 0x00FF0000) | ((br[2] << 8) & 0x0000FF00) |
                                                   (br[3] & 0x000000FF);
                                _RiffHeader.CkSize = ((br[4] << 24) & (int)SupportClass.Identity(0xFF000000)) |
                                                     ((br[5] << 16) & 0x00FF0000) | ((br[6] << 8) & 0x0000FF00) |
                                                     (br[7] & 0x000000FF);
                            }
                            catch {
                                _File.Close();
                                Fmode = RF_UNKNOWN;
                            }
                        }
                        catch {
                            Fmode = RF_UNKNOWN;
                            retcode = DDC_FILE_ERROR;
                        }
                        break;

                    default:
                        retcode = DDC_INVALID_CALL;
                        break;
                }
            }
            return retcode;
        }

        /// <summary>
        /// Write NumBytes data.
        /// </summary>
        internal virtual int Write(sbyte[] data, int numBytes) {
            if (Fmode != RF_WRITE) {
                return DDC_INVALID_CALL;
            }
            try {
                _File.Write(SupportClass.ToByteArray(data), 0, numBytes);
                Fmode = RF_WRITE;
            }
            catch {
                return DDC_FILE_ERROR;
            }
            _RiffHeader.CkSize += numBytes;
            return DDC_SUCCESS;
        }

        /// <summary>
        /// Write NumBytes data.
        /// </summary>
        internal virtual int Write(short[] data, int numBytes) {
            sbyte[] theData = new sbyte[numBytes];
            int yc = 0;
            for (int y = 0; y < numBytes; y = y + 2) {
                theData[y] = (sbyte)(data[yc] & 0x00FF);
                theData[y + 1] = (sbyte)((SupportClass.URShift(data[yc++], 8)) & 0x00FF);
            }
            if (Fmode != RF_WRITE) {
                return DDC_INVALID_CALL;
            }
            try {
                _File.Write(SupportClass.ToByteArray(theData), 0, numBytes);
                Fmode = RF_WRITE;
            }
            catch {
                return DDC_FILE_ERROR;
            }
            _RiffHeader.CkSize += numBytes;
            return DDC_SUCCESS;
        }

        /// <summary>
        /// Write NumBytes data.
        /// </summary>
        internal virtual int Write(RiffChunkHeader riffHeader, int numBytes) {
            sbyte[] br = new sbyte[8];
            br[0] = (sbyte)((SupportClass.URShift(riffHeader.CkId, 24)) & 0x000000FF);
            br[1] = (sbyte)((SupportClass.URShift(riffHeader.CkId, 16)) & 0x000000FF);
            br[2] = (sbyte)((SupportClass.URShift(riffHeader.CkId, 8)) & 0x000000FF);
            br[3] = (sbyte)(riffHeader.CkId & 0x000000FF);

            sbyte br4 = (sbyte)((SupportClass.URShift(riffHeader.CkSize, 24)) & 0x000000FF);
            sbyte br5 = (sbyte)((SupportClass.URShift(riffHeader.CkSize, 16)) & 0x000000FF);
            sbyte br6 = (sbyte)((SupportClass.URShift(riffHeader.CkSize, 8)) & 0x000000FF);
            sbyte br7 = (sbyte)(riffHeader.CkSize & 0x000000FF);

            br[4] = br7;
            br[5] = br6;
            br[6] = br5;
            br[7] = br4;

            if (Fmode != RF_WRITE) {
                return DDC_INVALID_CALL;
            }
            try {
                _File.Write(SupportClass.ToByteArray(br), 0, numBytes);
                Fmode = RF_WRITE;
            }
            catch {
                return DDC_FILE_ERROR;
            }
            _RiffHeader.CkSize += numBytes;
            return DDC_SUCCESS;
        }

        /// <summary>
        /// Write NumBytes data.
        /// </summary>
        internal virtual int Write(short data, int numBytes) {
            short theData = data; //(short) (((SupportClass.URShift(data, 8)) & 0x00FF) | ((Data << 8) & 0xFF00));
            if (Fmode != RF_WRITE) {
                return DDC_INVALID_CALL;
            }
            try {
                BinaryWriter tempBinaryWriter = new BinaryWriter(_File);
                tempBinaryWriter.Write(theData);
                Fmode = RF_WRITE;
            }
            catch {
                return DDC_FILE_ERROR;
            }
            _RiffHeader.CkSize += numBytes;
            return DDC_SUCCESS;
        }

        /// <summary>
        /// Write NumBytes data.
        /// </summary>
        internal virtual int Write(int data, int numBytes) {
            int theData = data;
            if (Fmode != RF_WRITE) {
                return DDC_INVALID_CALL;
            }
            try {
                BinaryWriter tempBinaryWriter = new BinaryWriter(_File);
                tempBinaryWriter.Write(theData);
                Fmode = RF_WRITE;
            }
            catch {
                return DDC_FILE_ERROR;
            }
            _RiffHeader.CkSize += numBytes;
            return DDC_SUCCESS;
        }

        /// <summary>
        /// Read NumBytes data.
        /// </summary>
        internal virtual int Read(sbyte[] data, int numBytes) {
            int retcode = DDC_SUCCESS;
            try {
                SupportClass.ReadInput(_File, ref data, 0, numBytes);
            }
            catch {
                retcode = DDC_FILE_ERROR;
            }
            return retcode;
        }

        /// <summary>
        /// Expect NumBytes data.
        /// </summary>
        internal virtual int Expect(string data, int numBytes) {
            int cnt = 0;
            try {
                while ((numBytes--) != 0) {
                    sbyte target = (sbyte)_File.ReadByte();
                    if (target != data[cnt++])
                        return DDC_FILE_ERROR;
                }
            }
            catch {
                return DDC_FILE_ERROR;
            }
            return DDC_SUCCESS;
        }

        /// <summary>
        /// Close Riff File.
        /// Length is written too.
        /// </summary>
        internal virtual int Close() {
            int retcode = DDC_SUCCESS;

            switch (Fmode) {
                case RF_WRITE:
                    try {
                        _File.Seek(0, SeekOrigin.Begin);
                        try {
                            sbyte[] br = new sbyte[8];
                            br[0] = (sbyte)((SupportClass.URShift(_RiffHeader.CkId, 24)) & 0x000000FF);
                            br[1] = (sbyte)((SupportClass.URShift(_RiffHeader.CkId, 16)) & 0x000000FF);
                            br[2] = (sbyte)((SupportClass.URShift(_RiffHeader.CkId, 8)) & 0x000000FF);
                            br[3] = (sbyte)(_RiffHeader.CkId & 0x000000FF);

                            br[7] = (sbyte)((SupportClass.URShift(_RiffHeader.CkSize, 24)) & 0x000000FF);
                            br[6] = (sbyte)((SupportClass.URShift(_RiffHeader.CkSize, 16)) & 0x000000FF);
                            br[5] = (sbyte)((SupportClass.URShift(_RiffHeader.CkSize, 8)) & 0x000000FF);
                            br[4] = (sbyte)(_RiffHeader.CkSize & 0x000000FF);
                            _File.Write(SupportClass.ToByteArray(br), 0, 8);
                            _File.Close();
                        }
                        catch {
                            retcode = DDC_FILE_ERROR;
                        }
                    }
                    catch {
                        retcode = DDC_FILE_ERROR;
                    }
                    break;

                case RF_READ:
                    try {
                        _File.Close();
                    }
                    catch {
                        retcode = DDC_FILE_ERROR;
                    }
                    break;
            }
            _File = null;
            Fmode = RF_UNKNOWN;
            return retcode;
        }

        /// <summary>
        /// Return File Position.
        /// </summary>
        internal virtual long CurrentFilePosition() {
            long position;
            try {
                position = _File.Position;
            }
            catch {
                position = -1;
            }
            return position;
        }

        /// <summary>
        /// Write Data to specified offset.
        /// </summary>
        internal virtual int Backpatch(long fileOffset, RiffChunkHeader data, int numBytes) {
            if (_File == null) {
                return DDC_INVALID_CALL;
            }
            try {
                _File.Seek(fileOffset, SeekOrigin.Begin);
            }
            catch {
                return DDC_FILE_ERROR;
            }
            return Write(data, numBytes);
        }

        internal virtual int Backpatch(long fileOffset, sbyte[] data, int numBytes) {
            if (_File == null) {
                return DDC_INVALID_CALL;
            }
            try {
                _File.Seek(fileOffset, SeekOrigin.Begin);
            }
            catch {
                return DDC_FILE_ERROR;
            }
            return Write(data, numBytes);
        }

        /// <summary>
        /// Seek in the File.
        /// </summary>
        protected virtual int Seek(long offset) {
            int rc;
            try {
                _File.Seek(offset, SeekOrigin.Begin);
                rc = DDC_SUCCESS;
            }
            catch {
                rc = DDC_FILE_ERROR;
            }
            return rc;
        }

        /// <summary>
        /// Fill the header.
        /// </summary>
        internal static int FourCC(string chunkName) {
            sbyte[] p = {0x20, 0x20, 0x20, 0x20};
            SupportClass.GetSBytesFromString(chunkName, 0, 4, ref p, 0);
            int ret = (((p[0] << 24) & (int)SupportClass.Identity(0xFF000000)) | ((p[1] << 16) & 0x00FF0000) |
                       ((p[2] << 8) & 0x0000FF00) | (p[3] & 0x000000FF));
            return ret;
        }

        public class RiffChunkHeader {
            internal int CkId; // Four-character chunk ID
            internal int CkSize;

            private RiffFile _EnclosingInstance;

            // Length of data in chunk
            internal RiffChunkHeader(RiffFile enclosingInstance) {
                InitBlock(enclosingInstance);
            }

            internal RiffFile EnclosingInstance => _EnclosingInstance;

            private void InitBlock(RiffFile enclosingInstance) {
                _EnclosingInstance = enclosingInstance;
            }
        }
    }

    public class RandomAccessFileStream {
        internal static FileStream CreateRandomAccessFile(string fileName, string mode) {
            FileStream newFile;

            if (string.Compare(mode, "rw", StringComparison.Ordinal) == 0)
                newFile = new FileStream(fileName, FileMode.OpenOrCreate,
                    FileAccess.ReadWrite);
            else if (string.Compare(mode, "r", StringComparison.Ordinal) == 0)
                newFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            else
                throw new ArgumentException();

            return newFile;
        }
    }

}

namespace RitaEngine.Resources.Sound.MP3Sharp.Decoding {


using System.IO;
using RitaEngine.Resources.Sound.MP3Sharp.Support;
using System;
using System.Runtime.Serialization;
using RitaEngine.Resources.Sound.MP3Sharp.Decoding.Decoders;
using System.Text;

    /// <summary>
    /// public class for extracting information from a frame header.
    /// TODO: move strings into resources.
    /// </summary>
    public class Header {
        /// <summary>
        /// Constant for MPEG-2 LSF version
        /// </summary>
        internal const int MPEG2_LSF = 0;

        internal const int MPEG25_LSF = 2; // SZD

        /// <summary>
        /// Constant for MPEG-1 version
        /// </summary>
        internal const int MPEG1 = 1;

        internal const int STEREO = 0;
        internal const int JOINT_STEREO = 1;
        internal const int DUAL_CHANNEL = 2;
        internal const int SINGLE_CHANNEL = 3;
        internal const int FOURTYFOUR_POINT_ONE = 0;
        internal const int FOURTYEIGHT = 1;
        internal const int THIRTYTWO = 2;

        internal static readonly int[][] Frequencies = {
            new[] {22050, 24000, 16000, 1}, new[] {44100, 48000, 32000, 1},
            new[] {11025, 12000, 8000, 1}
        };

        internal static readonly int[][][] Bitrates = {
            new[] {
                new[] {
                    0, 32000, 48000, 56000, 64000, 80000, 96000, 112000, 128000, 144000, 160000, 176000, 192000, 224000,
                    256000, 0
                },
                new[] {
                    0, 8000, 16000, 24000, 32000, 40000, 48000, 56000, 64000, 80000, 96000, 112000, 128000, 144000,
                    160000,
                    0
                },
                new[] {
                    0, 8000, 16000, 24000, 32000, 40000, 48000, 56000, 64000, 80000, 96000, 112000, 128000, 144000,
                    160000,
                    0
                }
            },
            new[] {
                new[] {
                    0, 32000, 64000, 96000, 128000, 160000, 192000, 224000, 256000, 288000, 320000, 352000, 384000,
                    416000,
                    448000, 0
                },
                new[] {
                    0, 32000, 48000, 56000, 64000, 80000, 96000, 112000, 128000, 160000, 192000, 224000, 256000, 320000,
                    384000, 0
                },
                new[] {
                    0, 32000, 40000, 48000, 56000, 64000, 80000, 96000, 112000, 128000, 160000, 192000, 224000, 256000,
                    320000, 0
                }
            },
            new[] {
                new[] {
                    0, 32000, 48000, 56000, 64000, 80000, 96000, 112000, 128000, 144000, 160000, 176000, 192000, 224000,
                    256000, 0
                },
                new[] {
                    0, 8000, 16000, 24000, 32000, 40000, 48000, 56000, 64000, 80000, 96000, 112000, 128000, 144000,
                    160000,
                    0
                },
                new[] {
                    0, 8000, 16000, 24000, 32000, 40000, 48000, 56000, 64000, 80000, 96000, 112000, 128000, 144000,
                    160000,
                    0
                }
            }
        };
        
        internal static readonly string[][][] BitrateStr = {
            new[] {
                new[] {
                    "free format", "32 kbit/s", "48 kbit/s", "56 kbit/s", "64 kbit/s", "80 kbit/s", "96 kbit/s",
                    "112 kbit/s", "128 kbit/s", "144 kbit/s", "160 kbit/s", "176 kbit/s", "192 kbit/s", "224 kbit/s",
                    "256 kbit/s", "forbidden"
                },
                new[] {
                    "free format", "8 kbit/s", "16 kbit/s", "24 kbit/s", "32 kbit/s", "40 kbit/s", "48 kbit/s",
                    "56 kbit/s",
                    "64 kbit/s", "80 kbit/s", "96 kbit/s", "112 kbit/s", "128 kbit/s", "144 kbit/s", "160 kbit/s",
                    "forbidden"
                },
                new[] {
                    "free format", "8 kbit/s", "16 kbit/s", "24 kbit/s", "32 kbit/s", "40 kbit/s", "48 kbit/s",
                    "56 kbit/s",
                    "64 kbit/s", "80 kbit/s", "96 kbit/s", "112 kbit/s", "128 kbit/s", "144 kbit/s", "160 kbit/s",
                    "forbidden"
                }
            },
            new[] {
                new[] {
                    "free format", "32 kbit/s", "64 kbit/s", "96 kbit/s", "128 kbit/s", "160 kbit/s", "192 kbit/s",
                    "224 kbit/s", "256 kbit/s", "288 kbit/s", "320 kbit/s", "352 kbit/s", "384 kbit/s", "416 kbit/s",
                    "448 kbit/s", "forbidden"
                },
                new[] {
                    "free format", "32 kbit/s", "48 kbit/s", "56 kbit/s", "64 kbit/s", "80 kbit/s", "96 kbit/s",
                    "112 kbit/s", "128 kbit/s", "160 kbit/s", "192 kbit/s", "224 kbit/s", "256 kbit/s", "320 kbit/s",
                    "384 kbit/s", "forbidden"
                },
                new[] {
                    "free format", "32 kbit/s", "40 kbit/s", "48 kbit/s", "56 kbit/s", "64 kbit/s", "80 kbit/s",
                    "96 kbit/s", "112 kbit/s", "128 kbit/s", "160 kbit/s", "192 kbit/s", "224 kbit/s", "256 kbit/s",
                    "320 kbit/s", "forbidden"
                }
            },
            new[] {
                new[] {
                    "free format", "32 kbit/s", "48 kbit/s", "56 kbit/s", "64 kbit/s", "80 kbit/s", "96 kbit/s",
                    "112 kbit/s", "128 kbit/s", "144 kbit/s", "160 kbit/s", "176 kbit/s", "192 kbit/s", "224 kbit/s",
                    "256 kbit/s", "forbidden"
                },
                new[] {
                    "free format", "8 kbit/s", "16 kbit/s", "24 kbit/s", "32 kbit/s", "40 kbit/s", "48 kbit/s",
                    "56 kbit/s",
                    "64 kbit/s", "80 kbit/s", "96 kbit/s", "112 kbit/s", "128 kbit/s", "144 kbit/s", "160 kbit/s",
                    "forbidden"
                },
                new[] {
                    "free format", "8 kbit/s", "16 kbit/s", "24 kbit/s", "32 kbit/s", "40 kbit/s", "48 kbit/s",
                    "56 kbit/s",
                    "64 kbit/s", "80 kbit/s", "96 kbit/s", "112 kbit/s", "128 kbit/s", "144 kbit/s", "160 kbit/s",
                    "forbidden"
                }
            }
        };

        internal short Checksum;
        internal int NSlots;

        private Crc16 _CRC;
        internal int Framesize;
        private bool _Copyright, _Original;
        private int _Headerstring = -1;
        private int _Layer, _ProtectionBit, _BitrateIndex, _PaddingBit, _ModeExtension;
        private int _Mode;
        private int _NumberOfSubbands, _IntensityStereoBound;
        private int _SampleFrequency;
        private sbyte _Syncmode;
        private int _Version;

        internal Header() {
            InitBlock();
        }

        /// <summary>
        /// Returns synchronized header.
        /// </summary>
        internal virtual int SyncHeader => _Headerstring;

        private void InitBlock() {
            _Syncmode = Bitstream.INITIAL_SYNC;
        }

        public override string ToString() {
            StringBuilder buffer = new StringBuilder(200);
            buffer.Append("Layer ");
            buffer.Append(LayerAsString());
            buffer.Append(" frame ");
            buffer.Append(ModeAsString());
            buffer.Append(' ');
            buffer.Append(VersionAsString());
            if (!IsProtection())
                buffer.Append(" no");
            buffer.Append(" checksums");
            buffer.Append(' ');
            buffer.Append(SampleFrequencyAsString());
            buffer.Append(',');
            buffer.Append(' ');
            buffer.Append(BitrateAsString());

            string s = buffer.ToString();
            return s;
        }

        /// <summary>
        /// Read a 32-bit header from the bitstream.
        /// </summary>
        internal void read_header(Bitstream stream, Crc16[] crcp) {
            int headerstring;
            int channelBitrate;

            bool sync = false;

            do {
                headerstring = stream.SyncHeader(_Syncmode);
                _Headerstring = headerstring;

                if (_Syncmode == Bitstream.INITIAL_SYNC) {
                    _Version = SupportClass.URShift(headerstring, 19) & 1;
                    if ((SupportClass.URShift(headerstring, 20) & 1) == 0)
                        // SZD: MPEG2.5 detection
                        if (_Version == MPEG2_LSF)
                            _Version = MPEG25_LSF;
                        else
                            throw stream.NewBitstreamException(BitstreamErrors.UNKNOWN_ERROR);

                    if ((_SampleFrequency = SupportClass.URShift(headerstring, 10) & 3) == 3) {
                        throw stream.NewBitstreamException(BitstreamErrors.UNKNOWN_ERROR);
                    }
                }

                _Layer = 4 - SupportClass.URShift(headerstring, 17) & 3;
                _ProtectionBit = SupportClass.URShift(headerstring, 16) & 1;
                _BitrateIndex = SupportClass.URShift(headerstring, 12) & 0xF;
                _PaddingBit = SupportClass.URShift(headerstring, 9) & 1;
                _Mode = SupportClass.URShift(headerstring, 6) & 3;
                _ModeExtension = SupportClass.URShift(headerstring, 4) & 3;
                if (_Mode == JOINT_STEREO)
                    _IntensityStereoBound = (_ModeExtension << 2) + 4;
                else
                    _IntensityStereoBound = 0;
                // should never be used
                _Copyright |= (SupportClass.URShift(headerstring, 3) & 1) == 1;
                _Original |= (SupportClass.URShift(headerstring, 2) & 1) == 1;

                // calculate number of subbands:
                if (_Layer == 1)
                    _NumberOfSubbands = 32;
                else {
                    channelBitrate = _BitrateIndex;
                    // calculate bitrate per channel:
                    if (_Mode != SINGLE_CHANNEL)
                        if (channelBitrate == 4)
                            channelBitrate = 1;
                        else
                            channelBitrate -= 4;

                    if (channelBitrate == 1 || channelBitrate == 2)
                        if (_SampleFrequency == THIRTYTWO)
                            _NumberOfSubbands = 12;
                        else
                            _NumberOfSubbands = 8;
                    else if (_SampleFrequency == FOURTYEIGHT || channelBitrate >= 3 && channelBitrate <= 5)
                        _NumberOfSubbands = 27;
                    else
                        _NumberOfSubbands = 30;
                }
                if (_IntensityStereoBound > _NumberOfSubbands)
                    _IntensityStereoBound = _NumberOfSubbands;
                // calculate framesize and nSlots
                CalculateFrameSize();

                // read framedata:
                stream.Read_frame_data(Framesize);

                if (stream.IsSyncCurrentPosition(_Syncmode)) {
                    if (_Syncmode == Bitstream.INITIAL_SYNC) {
                        _Syncmode = Bitstream.STRICT_SYNC;
                        stream.SetSyncWord(headerstring & unchecked((int)0xFFF80CC0));
                    }
                    sync = true;
                }
                else {
                    stream.UnreadFrame();
                }
            } while (!sync);

            stream.ParseFrame();

            if (_ProtectionBit == 0) {
                // frame contains a crc checksum
                Checksum = (short)stream.GetBitsFromBuffer(16);
                if (_CRC == null)
                    _CRC = new Crc16();
                _CRC.AddBits(headerstring, 16);
                crcp[0] = _CRC;
            }
            else
                crcp[0] = null;
            if (_SampleFrequency == FOURTYFOUR_POINT_ONE) {
                /*
                if (offset == null)
                {
                int max = max_number_of_frames(stream);
                offset = new int[max];
                for(int i=0; i<max; i++) offset[i] = 0;
                }
                // Bizarre, y avait ici une acollade ouvrante
                int cf = stream.current_frame();
                int lf = stream.last_frame();
                if ((cf > 0) && (cf == lf))
                {
                offset[cf] = offset[cf-1] + h_padding_bit;
                }
                else
                {
                offset[0] = h_padding_bit;
                }
                */
            }
        }

        // Functions to query header contents:
        /// <summary>
        /// Returns version.
        /// </summary>
        internal int Version() => _Version;

        /// <summary>
        /// Returns Layer ID.
        /// </summary>
        internal int Layer() => _Layer;

        /// <summary>
        /// Returns bitrate index.
        /// </summary>
        internal int bitrate_index() => _BitrateIndex;

        /// <summary>
        /// Returns Sample Frequency.
        /// </summary>
        internal int sample_frequency() => _SampleFrequency;

        /// <summary>
        /// Returns Frequency.
        /// </summary>
        internal int Frequency() => Frequencies[_Version][_SampleFrequency];

        /// <summary>
        /// Returns Mode.
        /// </summary>
        internal int Mode() => _Mode;

        /// <summary>
        /// Returns Protection bit.
        /// </summary>
        internal bool IsProtection() {
            if (_ProtectionBit == 0)
                return true;
            return false;
        }

        /// <summary>
        /// Returns Copyright.
        /// </summary>
        internal bool IsCopyright() => _Copyright;

        /// <summary>
        /// Returns Original.
        /// </summary>
        internal bool IsOriginal() => _Original;

        /// <summary>
        /// Returns Checksum flag.
        /// Compares computed checksum with stream checksum.
        /// </summary>
        internal bool IsChecksumOK() => Checksum == _CRC.Checksum();

        // Seeking and layer III stuff
        /// <summary>
        /// Returns Layer III Padding bit.
        /// </summary>
        internal bool IsPadding() {
            if (_PaddingBit == 0)
                return false;
            return true;
        }

        /// <summary>
        /// Returns Slots.
        /// </summary>
        internal int Slots() => NSlots;

        /// <summary>
        /// Returns Mode Extension.
        /// </summary>
        internal int mode_extension() => _ModeExtension;
        
        /// <summary>
        /// Calculate Frame size.
        /// Calculates framesize in bytes excluding header size.
        /// </summary>
        internal int CalculateFrameSize() {
            if (_Layer == 1) {
                Framesize = 12 * Bitrates[_Version][0][_BitrateIndex] / Frequencies[_Version][_SampleFrequency];
                if (_PaddingBit != 0)
                    Framesize++;
                Framesize <<= 2; // one slot is 4 bytes long
                NSlots = 0;
            }
            else {
                Framesize = 144 * Bitrates[_Version][_Layer - 1][_BitrateIndex] /
                            Frequencies[_Version][_SampleFrequency];
                if (_Version == MPEG2_LSF || _Version == MPEG25_LSF)
                    Framesize >>= 1;
                // SZD
                if (_PaddingBit != 0)
                    Framesize++;
                // Layer III slots
                if (_Layer == 3) {
                    if (_Version == MPEG1) {
                        NSlots = Framesize - (_Mode == SINGLE_CHANNEL ? 17 : 32) - (_ProtectionBit != 0 ? 0 : 2) -
                                 4; // header size
                    }
                    else {
                        // MPEG-2 LSF, SZD: MPEG-2.5 LSF
                        NSlots = Framesize - (_Mode == SINGLE_CHANNEL ? 9 : 17) - (_ProtectionBit != 0 ? 0 : 2) -
                                 4; // header size
                    }
                }
                else {
                    NSlots = 0;
                }
            }
            Framesize -= 4; // subtract header size
            return Framesize;
        }

        /// <summary>
        /// Returns the maximum number of frames in the stream.
        /// </summary>
        internal int MaxNumberOfFrame(int streamsize) {
            if (Framesize + 4 - _PaddingBit == 0)
                return 0;
            return streamsize / (Framesize + 4 - _PaddingBit);
        }

        /// <summary>
        /// Returns the maximum number of frames in the stream.
        /// </summary>
        internal int min_number_of_frames(int streamsize) {
            if (Framesize + 5 - _PaddingBit == 0)
                return 0;
            return streamsize / (Framesize + 5 - _PaddingBit);
        }

        /// <summary>
        /// Returns ms/frame.
        /// </summary>
        internal float MsPerFrame() {
            float[][] msPerFrameArray = {
                new[] {8.707483f, 8.0f, 12.0f}, new[] {26.12245f, 24.0f, 36.0f},
                new[] {26.12245f, 24.0f, 36.0f}
            };
            return msPerFrameArray[_Layer - 1][_SampleFrequency];
        }

        /// <summary>
        /// Returns total ms.
        /// </summary>
        internal float TotalMS(int streamsize) => MaxNumberOfFrame(streamsize) * MsPerFrame();

        // functions which return header informations as strings:
        /// <summary>
        /// Return Layer version.
        /// </summary>
        internal string LayerAsString() {
            switch (_Layer) {
                case 1:
                    return "I";

                case 2:
                    return "II";

                case 3:
                    return "III";
            }
            return null;
        }

        /// <summary>
        /// Returns Bitrate.
        /// </summary>
        internal string BitrateAsString() => BitrateStr[_Version][_Layer - 1][_BitrateIndex];

        /// <summary>
        /// Returns Frequency
        /// </summary>
        internal string SampleFrequencyAsString() {
            switch (_SampleFrequency) {
                case THIRTYTWO:
                    if (_Version == MPEG1)
                        return "32 kHz";
                    if (_Version == MPEG2_LSF)
                        return "16 kHz";
                    return "8 kHz";
                case FOURTYFOUR_POINT_ONE:
                    if (_Version == MPEG1)
                        return "44.1 kHz";
                    if (_Version == MPEG2_LSF)
                        return "22.05 kHz";
                    return "11.025 kHz";
                case FOURTYEIGHT:
                    if (_Version == MPEG1)
                        return "48 kHz";
                    if (_Version == MPEG2_LSF)
                        return "24 kHz";
                    return "12 kHz";
            }
            return null;
        }

        /// <summary>
        /// Returns Mode.
        /// </summary>
        internal string ModeAsString() {
            switch (_Mode) {
                case STEREO:
                    return "Stereo";
                case JOINT_STEREO:
                    return "Joint stereo";
                case DUAL_CHANNEL:
                    return "Dual channel";
                case SINGLE_CHANNEL:
                    return "Single channel";
            }
            return null;
        }

        /// <summary>
        /// Returns Version.
        /// </summary>
        internal string VersionAsString() {
            switch (_Version) {
                case MPEG1:
                    return "MPEG-1";

                case MPEG2_LSF:
                    return "MPEG-2 LSF";

                case MPEG25_LSF:
                    return "MPEG-2.5 LSF";
            }
            return null;
        }

        /// <summary>
        /// Returns the number of subbands in the current frame.
        /// </summary>
        internal int NumberSubbands() => _NumberOfSubbands;

        /// <summary>
        /// Returns Intensity Stereo.
        /// Layer II joint stereo only).
        /// Returns the number of subbands which are in stereo mode,
        /// subbands above that limit are in intensity stereo mode.
        /// </summary>
        internal int IntensityStereoBound() => _IntensityStereoBound;
    }

    /// <summary>
    /// Encapsulates the details of decoding an MPEG audio frame.
    /// </summary>
    public class Decoder {
        private static readonly Params DecoderDefaultParams = new Params();
        private Equalizer _Equalizer;

        private SynthesisFilter _LeftChannelFilter;
        private SynthesisFilter _RightChannelFilter;

        private bool _IsInitialized;
        private LayerIDecoder _L1Decoder;
        private LayerIIDecoder _L2Decoder;
        private LayerIIIDecoder _L3Decoder;

        private ABuffer _Output;

        private int _OutputChannels;
        private int _OutputFrequency;

        /// <summary>
        /// Creates a new Decoder instance with default parameters.
        /// </summary>
        internal Decoder() : this(null) {
            InitBlock();
        }

        /// <summary>
        /// Creates a new Decoder instance with custom parameters.
        /// </summary>
        internal Decoder(Params params0) {
            InitBlock();
            if (params0 == null) {
                params0 = DecoderDefaultParams;
            }
            Equalizer eq = params0.InitialEqualizerSettings;
            if (eq != null) {
                _Equalizer.FromEqualizer = eq;
            }
        }

        internal static Params DefaultParams => (Params)DecoderDefaultParams.Clone();

        internal virtual Equalizer Equalizer {
            set {
                if (value == null) {
                    value = Equalizer.PassThruEq;
                }
                _Equalizer.FromEqualizer = value;
                float[] factors = _Equalizer.BandFactors;
                if (_LeftChannelFilter != null)
                    _LeftChannelFilter.Eq = factors;

                if (_RightChannelFilter != null)
                    _RightChannelFilter.Eq = factors;
            }
        }

        /// <summary>
        /// Changes the output buffer. This will take effect the next time
        /// decodeFrame() is called.
        /// </summary>
        internal virtual ABuffer OutputBuffer {
            set => _Output = value;
        }

        /// <summary>
        /// Retrieves the sample frequency of the PCM samples output
        /// by this decoder. This typically corresponds to the sample
        /// rate encoded in the MPEG audio stream.
        /// </summary>
        internal virtual int OutputFrequency => _OutputFrequency;

        /// <summary>
        /// Retrieves the number of channels of PCM samples output by
        /// this decoder. This usually corresponds to the number of
        /// channels in the MPEG audio stream.
        /// </summary>
        internal virtual int OutputChannels => _OutputChannels;

        /// <summary>
        /// Retrieves the maximum number of samples that will be written to
        /// the output buffer when one frame is decoded. This can be used to
        /// help calculate the size of other buffers whose size is based upon
        /// the number of samples written to the output buffer. NB: this is
        /// an upper bound and fewer samples may actually be written, depending
        /// upon the sample rate and number of channels.
        /// </summary>
        internal virtual int OutputBlockSize => ABuffer.OBUFFERSIZE;

        private void InitBlock() {
            _Equalizer = new Equalizer();
        }

        /// <summary>
        /// Decodes one frame from an MPEG audio bitstream.
        /// </summary>
        /// <param name="header">
        /// Header describing the frame to decode.
        /// </param>
        /// <param name="stream">
        /// Bistream that provides the bits for the body of the frame.
        /// </param>
        /// <returns>
        /// A SampleBuffer containing the decoded samples.
        /// </returns>
        internal virtual ABuffer DecodeFrame(Header header, Bitstream stream) {
            if (!_IsInitialized) {
                Initialize(header);
            }
            int layer = header.Layer();
            _Output.ClearBuffer();
            IFrameDecoder decoder = RetrieveDecoder(header, stream, layer);
            decoder.DecodeFrame();
            _Output.WriteBuffer(1);
            return _Output;
        }

        protected virtual DecoderException NewDecoderException(int errorcode) => new DecoderException(errorcode, null);

        protected virtual DecoderException NewDecoderException(int errorcode, Exception throwable) => new DecoderException(errorcode, throwable);

        protected virtual IFrameDecoder RetrieveDecoder(Header header, Bitstream stream, int layer) {
            IFrameDecoder decoder = null;

            // REVIEW: allow channel output selection type
            // (LEFT, RIGHT, BOTH, DOWNMIX)
            switch (layer) {
                case 3:
                    if (_L3Decoder == null) {
                        _L3Decoder = new LayerIIIDecoder(stream, header, _LeftChannelFilter, _RightChannelFilter, _Output,
                            (int)OutputChannelsEnum.BothChannels);
                    }

                    decoder = _L3Decoder;
                    break;

                case 2:
                    if (_L2Decoder == null) {
                        _L2Decoder = new LayerIIDecoder();
                        _L2Decoder.Create(stream, header, _LeftChannelFilter, _RightChannelFilter, _Output,
                            (int)OutputChannelsEnum.BothChannels);
                    }
                    decoder = _L2Decoder;
                    break;

                case 1:
                    if (_L1Decoder == null) {
                        _L1Decoder = new LayerIDecoder();
                        _L1Decoder.Create(stream, header, _LeftChannelFilter, _RightChannelFilter, _Output,
                            (int)OutputChannelsEnum.BothChannels);
                    }
                    decoder = _L1Decoder;
                    break;
            }

            if (decoder == null) {
                throw NewDecoderException(DecoderErrors.UNSUPPORTED_LAYER, null);
            }

            return decoder;
        }

        private void Initialize(Header header) {
            // REVIEW: allow customizable scale factor
            const float scalefactor = 32700.0f;
            int channels = header.Mode() == Header.SINGLE_CHANNEL ? 1 : 2;

            // set up output buffer if not set up by client.
            if (_Output == null)
                _Output = new SampleBuffer(header.Frequency(), channels);

            float[] factors = _Equalizer.BandFactors;
            _LeftChannelFilter = new SynthesisFilter(0, scalefactor, factors);
            if (channels == 2)
                _RightChannelFilter = new SynthesisFilter(1, scalefactor, factors);

            _OutputChannels = channels;
            _OutputFrequency = header.Frequency();

            _IsInitialized = true;
        }

        /// <summary>
        /// The Params class presents the customizable
        /// aspects of the decoder. Instances of this class are not thread safe.
        /// </summary>
        public class Params : ICloneable {
            private OutputChannels _OutputChannels;

            internal virtual OutputChannels OutputChannels {
                get => _OutputChannels;

                set => _OutputChannels = value ?? throw new NullReferenceException("out");
            }

            /// <summary>
            /// Retrieves the equalizer settings that the decoder's equalizer
            /// will be initialized from.
            /// The Equalizer instance returned
            /// cannot be changed in real time to affect the
            /// decoder output as it is used only to initialize the decoders
            /// EQ settings. To affect the decoder's output in realtime,
            /// use the Equalizer returned from the getEqualizer() method on
            /// the decoder.
            /// </summary>
            /// <returns>
            /// The Equalizer used to initialize the
            /// EQ settings of the decoder.
            /// </returns>
            private readonly Equalizer _Equalizer = null;

            internal virtual Equalizer InitialEqualizerSettings => _Equalizer;

            public object Clone() {
                try {
                    return MemberwiseClone();
                }
                catch (Exception ex) {
                    throw new ApplicationException(this + ": " + ex);
                }
            }
        }
    }

    /// <summary>
    /// This interface provides constants describing the error
    /// codes used by the Decoder to indicate errors.
    /// </summary>
    internal struct DecoderErrors {
        internal const int UNKNOWN_ERROR = BitstreamErrors.DECODER_ERROR + 0;
        internal const int UNSUPPORTED_LAYER = BitstreamErrors.DECODER_ERROR + 1;
    }
    /// <summary>
    /// The DecoderException represents the class of
    /// errors that can occur when decoding MPEG audio.
    /// </summary>
    [Serializable]
    public class DecoderException : MP3SharpException {
        private int _ErrorCode;

        internal DecoderException(string message, Exception inner) : base(message, inner) {
            InitBlock();
        }

        internal DecoderException(int errorcode, Exception inner) : this(GetErrorString(errorcode), inner) {
            InitBlock();
            _ErrorCode = errorcode;
        }

        protected DecoderException(SerializationInfo info, StreamingContext context) : base(info, context) {
            _ErrorCode = info.GetInt32("ErrorCode");
        }

        internal virtual int ErrorCode => _ErrorCode;

        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            if (info == null) {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue("ErrorCode", _ErrorCode);
            base.GetObjectData(info, context);
        }

        private void InitBlock() {
            _ErrorCode = DecoderErrors.UNKNOWN_ERROR;
        }

        internal static string GetErrorString(int errorcode) =>
            // REVIEW: use resource file to map error codes
            // to locale-sensitive strings. 
            "Decoder errorcode " + Convert.ToString(errorcode, 16);
    }

    /// <summary>
    /// The Equalizer class can be used to specify
    /// equalization settings for the MPEG audio decoder.
    /// The equalizer consists of 32 band-pass filters.
    /// Each band of the equalizer can take on a fractional value between
    /// -1.0 and +1.0.
    /// At -1.0, the input signal is attenuated by 6dB, at +1.0 the signal is
    /// amplified by 6dB.
    /// </summary>
    public class Equalizer {
        private const int BANDS = 32;

        /// <summary>
        /// Equalizer setting to denote that a given band will not be
        /// present in the output signal.
        /// </summary>
        internal const float BAND_NOT_PRESENT = float.NegativeInfinity;

        internal static readonly Equalizer PassThruEq = new Equalizer();
        private float[] _Settings;

        /// <summary>
        /// Creates a new Equalizer instance.
        /// </summary>
        internal Equalizer() {
            InitBlock();
        }

        //    private Equalizer(float b1, float b2, float b3, float b4, float b5,
        //                     float b6, float b7, float b8, float b9, float b10, float b11,
        //                     float b12, float b13, float b14, float b15, float b16,
        //                     float b17, float b18, float b19, float b20);

        internal Equalizer(float[] settings) {
            InitBlock();
            FromFloatArray = settings;
        }

        internal Equalizer(EQFunction eq) {
            InitBlock();
            FromEQFunction = eq;
        }

        internal float[] FromFloatArray {
            set {
                Reset();
                int max = value.Length > BANDS ? BANDS : value.Length;

                for (int i = 0; i < max; i++) {
                    _Settings[i] = Limit(value[i]);
                }
            }
        }

        //UPGRADE_TODO: Method 'setFrom' was converted to a set modifier. This name conflicts with another property. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1137"'
        /// <summary>
        /// Sets the bands of this equalizer to the value the bands of
        /// another equalizer. Bands that are not present in both equalizers are ignored.
        /// </summary>
        internal virtual Equalizer FromEqualizer {
            set {
                if (value != this) {
                    FromFloatArray = value._Settings;
                }
            }
        }

        //UPGRADE_TODO: Method 'setFrom' was converted to a set modifier. This name conflicts with another property. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1137"'
        internal EQFunction FromEQFunction {
            set {
                Reset();
                int max = BANDS;

                for (int i = 0; i < max; i++) {
                    _Settings[i] = Limit(value.GetBand(i));
                }
            }
        }

        /// <summary>
        /// Retrieves the number of bands present in this equalizer.
        /// </summary>
        internal virtual int BandCount => _Settings.Length;

        /// <summary>
        /// Retrieves an array of floats whose values represent a
        /// scaling factor that can be applied to linear samples
        /// in each band to provide the equalization represented by
        /// this instance.
        /// </summary>
        /// <returns>
        /// an array of factors that can be applied to the
        /// subbands.
        /// </returns>
        internal virtual float[] BandFactors {
            get {
                float[] factors = new float[BANDS];
                for (int i = 0; i < BANDS; i++) {
                    factors[i] = GetBandFactor(_Settings[i]);
                }

                return factors;
            }
        }

        private void InitBlock() {
            _Settings = new float[BANDS];
        }

        /// <summary>
        /// Sets all bands to 0.0
        /// </summary>
        internal void Reset() {
            for (int i = 0; i < BANDS; i++) {
                _Settings[i] = 0.0f;
            }
        }

        internal float SetBand(int band, float neweq) {
            float eq = 0.0f;

            if (band >= 0 && band < BANDS) {
                eq = _Settings[band];
                _Settings[band] = Limit(neweq);
            }

            return eq;
        }

        /// <summary>
        /// Retrieves the eq setting for a given band.
        /// </summary>
        internal float GetBand(int band) {
            float eq = 0.0f;

            if (band >= 0 && band < BANDS) {
                eq = _Settings[band];
            }

            return eq;
        }

        private float Limit(float eq) {
            if (eq == BAND_NOT_PRESENT)
                return eq;
            if (eq > 1.0f)
                return 1.0f;
            if (eq < -1.0f)
                return -1.0f;

            return eq;
        }

        /// <summary>
        /// Converts an equalizer band setting to a sample factor.
        /// The factor is determined by the function f = 2^n where
        /// n is the equalizer band setting in the range [-1.0,1.0].
        /// </summary>
        internal float GetBandFactor(float eq) {
            if (eq == BAND_NOT_PRESENT)
                return 0.0f;

            float f = (float)Math.Pow(2.0, eq);
            return f;
        }

        internal abstract class EQFunction {
            /// <summary>
            /// Returns the setting of a band in the equalizer.
            /// </summary>
            /// <param name="band">
            /// The index of the band to retrieve the setting for.
            /// </param>
            /// <returns>
            /// the setting of the specified band. This is a value between
            /// -1 and +1.
            /// </returns>
            internal virtual float GetBand(int band) => 0.0f;
        }
    }

    /// <summary>
    /// A class for the synthesis filter bank.
    /// This class does a fast downsampling from 32, 44.1 or 48 kHz to 8 kHz, if ULAW is defined.
    /// Frequencies above 4 kHz are removed by ignoring higher subbands.
    /// </summary>
    public class SynthesisFilter {
        private const double MY_PI = 3.14159265358979323846;

        // Note: These values are not in the same order
        // as in Annex 3-B.3 of the ISO/IEC DIS 11172-3 
        private static readonly float Cos164 = (float)(1.0 / (2.0 * Math.Cos(MY_PI / 64.0)));
        private static readonly float Cos364 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 3.0 / 64.0)));
        private static readonly float Cos564 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 5.0 / 64.0)));
        private static readonly float Cos764 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 7.0 / 64.0)));
        private static readonly float Cos964 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 9.0 / 64.0)));
        private static readonly float Cos1164 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 11.0 / 64.0)));
        private static readonly float Cos1364 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 13.0 / 64.0)));
        private static readonly float Cos1564 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 15.0 / 64.0)));
        private static readonly float Cos1764 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 17.0 / 64.0)));
        private static readonly float Cos1964 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 19.0 / 64.0)));
        private static readonly float Cos2164 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 21.0 / 64.0)));
        private static readonly float Cos2364 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 23.0 / 64.0)));
        private static readonly float Cos2564 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 25.0 / 64.0)));
        private static readonly float Cos2764 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 27.0 / 64.0)));
        private static readonly float Cos2964 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 29.0 / 64.0)));
        private static readonly float Cos3164 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 31.0 / 64.0)));
        private static readonly float Cos132 = (float)(1.0 / (2.0 * Math.Cos(MY_PI / 32.0)));
        private static readonly float Cos332 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 3.0 / 32.0)));
        private static readonly float Cos532 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 5.0 / 32.0)));
        private static readonly float Cos732 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 7.0 / 32.0)));
        private static readonly float Cos932 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 9.0 / 32.0)));
        private static readonly float Cos1132 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 11.0 / 32.0)));
        private static readonly float Cos1332 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 13.0 / 32.0)));
        private static readonly float Cos1532 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 15.0 / 32.0)));
        private static readonly float Cos116 = (float)(1.0 / (2.0 * Math.Cos(MY_PI / 16.0)));
        private static readonly float Cos316 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 3.0 / 16.0)));
        private static readonly float Cos516 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 5.0 / 16.0)));
        private static readonly float Cos716 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 7.0 / 16.0)));
        private static readonly float Cos18 = (float)(1.0 / (2.0 * Math.Cos(MY_PI / 8.0)));
        private static readonly float Cos38 = (float)(1.0 / (2.0 * Math.Cos(MY_PI * 3.0 / 8.0)));
        private static readonly float Cos14 = (float)(1.0 / (2.0 * Math.Cos(MY_PI / 4.0)));

        private static float[] _d;

        /// d[] split into subarrays of length 16. This provides for
        /// more faster access by allowing a block of 16 to be addressed
        /// with constant offset.
        private static float[][] _d16;

        // The original data for d[]. This data (was) loaded from a file
        // to reduce the overall package size and to improve performance. 
        private static readonly float[] DData = {
            0.000000000f, -0.000442505f, 0.003250122f, -0.007003784f,
            0.031082153f, -0.078628540f, 0.100311279f, -0.572036743f,
            1.144989014f, 0.572036743f, 0.100311279f, 0.078628540f,
            0.031082153f, 0.007003784f, 0.003250122f, 0.000442505f,
            -0.000015259f, -0.000473022f, 0.003326416f, -0.007919312f,
            0.030517578f, -0.084182739f, 0.090927124f, -0.600219727f,
            1.144287109f, 0.543823242f, 0.108856201f, 0.073059082f,
            0.031478882f, 0.006118774f, 0.003173828f, 0.000396729f,
            -0.000015259f, -0.000534058f, 0.003387451f, -0.008865356f,
            0.029785156f, -0.089706421f, 0.080688477f, -0.628295898f,
            1.142211914f, 0.515609741f, 0.116577148f, 0.067520142f,
            0.031738281f, 0.005294800f, 0.003082275f, 0.000366211f,
            -0.000015259f, -0.000579834f, 0.003433228f, -0.009841919f,
            0.028884888f, -0.095169067f, 0.069595337f, -0.656219482f,
            1.138763428f, 0.487472534f, 0.123474121f, 0.061996460f,
            0.031845093f, 0.004486084f, 0.002990723f, 0.000320435f,
            -0.000015259f, -0.000625610f, 0.003463745f, -0.010848999f,
            0.027801514f, -0.100540161f, 0.057617188f, -0.683914185f,
            1.133926392f, 0.459472656f, 0.129577637f, 0.056533813f,
            0.031814575f, 0.003723145f, 0.002899170f, 0.000289917f,
            -0.000015259f, -0.000686646f, 0.003479004f, -0.011886597f,
            0.026535034f, -0.105819702f, 0.044784546f, -0.711318970f,
            1.127746582f, 0.431655884f, 0.134887695f, 0.051132202f,
            0.031661987f, 0.003005981f, 0.002792358f, 0.000259399f,
            -0.000015259f, -0.000747681f, 0.003479004f, -0.012939453f,
            0.025085449f, -0.110946655f, 0.031082153f, -0.738372803f,
            1.120223999f, 0.404083252f, 0.139450073f, 0.045837402f,
            0.031387329f, 0.002334595f, 0.002685547f, 0.000244141f,
            -0.000030518f, -0.000808716f, 0.003463745f, -0.014022827f,
            0.023422241f, -0.115921021f, 0.016510010f, -0.765029907f,
            1.111373901f, 0.376800537f, 0.143264771f, 0.040634155f,
            0.031005859f, 0.001693726f, 0.002578735f, 0.000213623f,
            -0.000030518f, -0.000885010f, 0.003417969f, -0.015121460f,
            0.021575928f, -0.120697021f, 0.001068115f, -0.791213989f,
            1.101211548f, 0.349868774f, 0.146362305f, 0.035552979f,
            0.030532837f, 0.001098633f, 0.002456665f, 0.000198364f,
            -0.000030518f, -0.000961304f, 0.003372192f, -0.016235352f,
            0.019531250f, -0.125259399f, -0.015228271f, -0.816864014f,
            1.089782715f, 0.323318481f, 0.148773193f, 0.030609131f,
            0.029937744f, 0.000549316f, 0.002349854f, 0.000167847f,
            -0.000030518f, -0.001037598f, 0.003280640f, -0.017349243f,
            0.017257690f, -0.129562378f, -0.032379150f, -0.841949463f,
            1.077117920f, 0.297210693f, 0.150497437f, 0.025817871f,
            0.029281616f, 0.000030518f, 0.002243042f, 0.000152588f,
            -0.000045776f, -0.001113892f, 0.003173828f, -0.018463135f,
            0.014801025f, -0.133590698f, -0.050354004f, -0.866363525f,
            1.063217163f, 0.271591187f, 0.151596069f, 0.021179199f,
            0.028533936f, -0.000442505f, 0.002120972f, 0.000137329f,
            -0.000045776f, -0.001205444f, 0.003051758f, -0.019577026f,
            0.012115479f, -0.137298584f, -0.069168091f, -0.890090942f,
            1.048156738f, 0.246505737f, 0.152069092f, 0.016708374f,
            0.027725220f, -0.000869751f, 0.002014160f, 0.000122070f,
            -0.000061035f, -0.001296997f, 0.002883911f, -0.020690918f,
            0.009231567f, -0.140670776f, -0.088775635f, -0.913055420f,
            1.031936646f, 0.221984863f, 0.151962280f, 0.012420654f,
            0.026840210f, -0.001266479f, 0.001907349f, 0.000106812f,
            -0.000061035f, -0.001388550f, 0.002700806f, -0.021789551f,
            0.006134033f, -0.143676758f, -0.109161377f, -0.935195923f,
            1.014617920f, 0.198059082f, 0.151306152f, 0.008316040f,
            0.025909424f, -0.001617432f, 0.001785278f, 0.000106812f,
            -0.000076294f, -0.001480103f, 0.002487183f, -0.022857666f,
            0.002822876f, -0.146255493f, -0.130310059f, -0.956481934f,
            0.996246338f, 0.174789429f, 0.150115967f, 0.004394531f,
            0.024932861f, -0.001937866f, 0.001693726f, 0.000091553f,
            -0.000076294f, -0.001586914f, 0.002227783f, -0.023910522f,
            -0.000686646f, -0.148422241f, -0.152206421f, -0.976852417f,
            0.976852417f, 0.152206421f, 0.148422241f, 0.000686646f,
            0.023910522f, -0.002227783f, 0.001586914f, 0.000076294f,
            -0.000091553f, -0.001693726f, 0.001937866f, -0.024932861f,
            -0.004394531f, -0.150115967f, -0.174789429f, -0.996246338f,
            0.956481934f, 0.130310059f, 0.146255493f, -0.002822876f,
            0.022857666f, -0.002487183f, 0.001480103f, 0.000076294f,
            -0.000106812f, -0.001785278f, 0.001617432f, -0.025909424f,
            -0.008316040f, -0.151306152f, -0.198059082f, -1.014617920f,
            0.935195923f, 0.109161377f, 0.143676758f, -0.006134033f,
            0.021789551f, -0.002700806f, 0.001388550f, 0.000061035f,
            -0.000106812f, -0.001907349f, 0.001266479f, -0.026840210f,
            -0.012420654f, -0.151962280f, -0.221984863f, -1.031936646f,
            0.913055420f, 0.088775635f, 0.140670776f, -0.009231567f,
            0.020690918f, -0.002883911f, 0.001296997f, 0.000061035f,
            -0.000122070f, -0.002014160f, 0.000869751f, -0.027725220f,
            -0.016708374f, -0.152069092f, -0.246505737f, -1.048156738f,
            0.890090942f, 0.069168091f, 0.137298584f, -0.012115479f,
            0.019577026f, -0.003051758f, 0.001205444f, 0.000045776f,
            -0.000137329f, -0.002120972f, 0.000442505f, -0.028533936f,
            -0.021179199f, -0.151596069f, -0.271591187f, -1.063217163f,
            0.866363525f, 0.050354004f, 0.133590698f, -0.014801025f,
            0.018463135f, -0.003173828f, 0.001113892f, 0.000045776f,
            -0.000152588f, -0.002243042f, -0.000030518f, -0.029281616f,
            -0.025817871f, -0.150497437f, -0.297210693f, -1.077117920f,
            0.841949463f, 0.032379150f, 0.129562378f, -0.017257690f,
            0.017349243f, -0.003280640f, 0.001037598f, 0.000030518f,
            -0.000167847f, -0.002349854f, -0.000549316f, -0.029937744f,
            -0.030609131f, -0.148773193f, -0.323318481f, -1.089782715f,
            0.816864014f, 0.015228271f, 0.125259399f, -0.019531250f,
            0.016235352f, -0.003372192f, 0.000961304f, 0.000030518f,
            -0.000198364f, -0.002456665f, -0.001098633f, -0.030532837f,
            -0.035552979f, -0.146362305f, -0.349868774f, -1.101211548f,
            0.791213989f, -0.001068115f, 0.120697021f, -0.021575928f,
            0.015121460f, -0.003417969f, 0.000885010f, 0.000030518f,
            -0.000213623f, -0.002578735f, -0.001693726f, -0.031005859f,
            -0.040634155f, -0.143264771f, -0.376800537f, -1.111373901f,
            0.765029907f, -0.016510010f, 0.115921021f, -0.023422241f,
            0.014022827f, -0.003463745f, 0.000808716f, 0.000030518f,
            -0.000244141f, -0.002685547f, -0.002334595f, -0.031387329f,
            -0.045837402f, -0.139450073f, -0.404083252f, -1.120223999f,
            0.738372803f, -0.031082153f, 0.110946655f, -0.025085449f,
            0.012939453f, -0.003479004f, 0.000747681f, 0.000015259f,
            -0.000259399f, -0.002792358f, -0.003005981f, -0.031661987f,
            -0.051132202f, -0.134887695f, -0.431655884f, -1.127746582f,
            0.711318970f, -0.044784546f, 0.105819702f, -0.026535034f,
            0.011886597f, -0.003479004f, 0.000686646f, 0.000015259f,
            -0.000289917f, -0.002899170f, -0.003723145f, -0.031814575f,
            -0.056533813f, -0.129577637f, -0.459472656f, -1.133926392f,
            0.683914185f, -0.057617188f, 0.100540161f, -0.027801514f,
            0.010848999f, -0.003463745f, 0.000625610f, 0.000015259f,
            -0.000320435f, -0.002990723f, -0.004486084f, -0.031845093f,
            -0.061996460f, -0.123474121f, -0.487472534f, -1.138763428f,
            0.656219482f, -0.069595337f, 0.095169067f, -0.028884888f,
            0.009841919f, -0.003433228f, 0.000579834f, 0.000015259f,
            -0.000366211f, -0.003082275f, -0.005294800f, -0.031738281f,
            -0.067520142f, -0.116577148f, -0.515609741f, -1.142211914f,
            0.628295898f, -0.080688477f, 0.089706421f, -0.029785156f,
            0.008865356f, -0.003387451f, 0.000534058f, 0.000015259f,
            -0.000396729f, -0.003173828f, -0.006118774f, -0.031478882f,
            -0.073059082f, -0.108856201f, -0.543823242f, -1.144287109f,
            0.600219727f, -0.090927124f, 0.084182739f, -0.030517578f,
            0.007919312f, -0.003326416f, 0.000473022f, 0.000015259f
        };

        private readonly int _Channel;
        private readonly float[] _Samples; // 32 new subband samples
        private readonly float _Scalefactor;
        private readonly float[] _V1;
        private readonly float[] _V2;

        /// <summary>
        /// Compute PCM Samples.
        /// </summary>
        private float[] _TmpOut;

        private float[] _ActualV; // v1 or v2
        private int _ActualWritePos; // 0-15
        private float[] _Eq;

        /// <summary>
        /// Quality value for controlling CPU usage/quality tradeoff.
        /// </summary>
        /// <summary>
        /// Contructor.
        /// The scalefactor scales the calculated float pcm samples to short values
        /// (raw pcm samples are in [-1.0, 1.0], if no violations occur).
        /// </summary>
        internal SynthesisFilter(int channelnumber, float factor, float[] eq0) {
            InitBlock();
            if (_d == null) {
                _d = DData; // load_d();
                _d16 = SplitArray(_d, 16);
            }

            _V1 = new float[512];
            _V2 = new float[512];
            _Samples = new float[32];
            _Channel = channelnumber;
            _Scalefactor = factor;
            Eq = _Eq;

            Reset();
        }

        internal float[] Eq {
            set {
                _Eq = value;

                if (_Eq == null) {
                    _Eq = new float[32];
                    for (int i = 0; i < 32; i++)
                        _Eq[i] = 1.0f;
                }
                if (_Eq.Length < 32) {
                    throw new ArgumentException("eq0");
                }
            }
        }

        private void InitBlock() {
            _TmpOut = new float[32];
        }

        /// <summary>
        /// Reset the synthesis filter.
        /// </summary>
        internal void Reset() {
            // initialize v1[] and v2[]:
            for (int p = 0; p < 512; p++)
                _V1[p] = _V2[p] = 0.0f;

            // initialize samples[]:
            for (int p2 = 0; p2 < 32; p2++)
                _Samples[p2] = 0.0f;

            _ActualV = _V1;
            _ActualWritePos = 15;
        }

        internal void AddSample(float sample, int subbandnumber) {
            _Samples[subbandnumber] = _Eq[subbandnumber] * sample;
        }

        internal void AddSamples(float[] s) {
            for (int i = 31; i >= 0; i--) {
                _Samples[i] = s[i] * _Eq[i];
            }
        }

        /// <summary>
        /// Compute new values via a fast cosine transform.
        /// </summary>
        private void ComputeNewValues() {
            float newV0, newV1, newV2, newV3, newV4, newV5, newV6, newV7, newV8, newV9;
            float newV10, newV11, newV12, newV13, newV14, newV15, newV16, newV17, newV18, newV19;
            float newV20, newV21, newV22, newV23, newV24, newV25, newV26, newV27, newV28, newV29;
            float newV30, newV31;

            float[] s = _Samples;

            float s0 = s[0];
            float s1 = s[1];
            float s2 = s[2];
            float s3 = s[3];
            float s4 = s[4];
            float s5 = s[5];
            float s6 = s[6];
            float s7 = s[7];
            float s8 = s[8];
            float s9 = s[9];
            float s10 = s[10];
            float s11 = s[11];
            float s12 = s[12];
            float s13 = s[13];
            float s14 = s[14];
            float s15 = s[15];
            float s16 = s[16];
            float s17 = s[17];
            float s18 = s[18];
            float s19 = s[19];
            float s20 = s[20];
            float s21 = s[21];
            float s22 = s[22];
            float s23 = s[23];
            float s24 = s[24];
            float s25 = s[25];
            float s26 = s[26];
            float s27 = s[27];
            float s28 = s[28];
            float s29 = s[29];
            float s30 = s[30];
            float s31 = s[31];

            float p0 = s0 + s31;
            float p1 = s1 + s30;
            float p2 = s2 + s29;
            float p3 = s3 + s28;
            float p4 = s4 + s27;
            float p5 = s5 + s26;
            float p6 = s6 + s25;
            float p7 = s7 + s24;
            float p8 = s8 + s23;
            float p9 = s9 + s22;
            float p10 = s10 + s21;
            float p11 = s11 + s20;
            float p12 = s12 + s19;
            float p13 = s13 + s18;
            float p14 = s14 + s17;
            float p15 = s15 + s16;

            float pp0 = p0 + p15;
            float pp1 = p1 + p14;
            float pp2 = p2 + p13;
            float pp3 = p3 + p12;
            float pp4 = p4 + p11;
            float pp5 = p5 + p10;
            float pp6 = p6 + p9;
            float pp7 = p7 + p8;
            float pp8 = (p0 - p15) * Cos132;
            float pp9 = (p1 - p14) * Cos332;
            float pp10 = (p2 - p13) * Cos532;
            float pp11 = (p3 - p12) * Cos732;
            float pp12 = (p4 - p11) * Cos932;
            float pp13 = (p5 - p10) * Cos1132;
            float pp14 = (p6 - p9) * Cos1332;
            float pp15 = (p7 - p8) * Cos1532;

            p0 = pp0 + pp7;
            p1 = pp1 + pp6;
            p2 = pp2 + pp5;
            p3 = pp3 + pp4;
            p4 = (pp0 - pp7) * Cos116;
            p5 = (pp1 - pp6) * Cos316;
            p6 = (pp2 - pp5) * Cos516;
            p7 = (pp3 - pp4) * Cos716;
            p8 = pp8 + pp15;
            p9 = pp9 + pp14;
            p10 = pp10 + pp13;
            p11 = pp11 + pp12;
            p12 = (pp8 - pp15) * Cos116;
            p13 = (pp9 - pp14) * Cos316;
            p14 = (pp10 - pp13) * Cos516;
            p15 = (pp11 - pp12) * Cos716;

            pp0 = p0 + p3;
            pp1 = p1 + p2;
            pp2 = (p0 - p3) * Cos18;
            pp3 = (p1 - p2) * Cos38;
            pp4 = p4 + p7;
            pp5 = p5 + p6;
            pp6 = (p4 - p7) * Cos18;
            pp7 = (p5 - p6) * Cos38;
            pp8 = p8 + p11;
            pp9 = p9 + p10;
            pp10 = (p8 - p11) * Cos18;
            pp11 = (p9 - p10) * Cos38;
            pp12 = p12 + p15;
            pp13 = p13 + p14;
            pp14 = (p12 - p15) * Cos18;
            pp15 = (p13 - p14) * Cos38;

            p0 = pp0 + pp1;
            p1 = (pp0 - pp1) * Cos14;
            p2 = pp2 + pp3;
            p3 = (pp2 - pp3) * Cos14;
            p4 = pp4 + pp5;
            p5 = (pp4 - pp5) * Cos14;
            p6 = pp6 + pp7;
            p7 = (pp6 - pp7) * Cos14;
            p8 = pp8 + pp9;
            p9 = (pp8 - pp9) * Cos14;
            p10 = pp10 + pp11;
            p11 = (pp10 - pp11) * Cos14;
            p12 = pp12 + pp13;
            p13 = (pp12 - pp13) * Cos14;
            p14 = pp14 + pp15;
            p15 = (pp14 - pp15) * Cos14;

            // this is pretty insane coding
            float tmp1;
            newV19 = -(newV4 = (newV12 = p7) + p5) - p6;
            newV27 = -p6 - p7 - p4;
            newV6 = (newV10 = (newV14 = p15) + p11) + p13;
            newV17 = -(newV2 = p15 + p13 + p9) - p14;
            newV21 = (tmp1 = -p14 - p15 - p10 - p11) - p13;
            newV29 = -p14 - p15 - p12 - p8;
            newV25 = tmp1 - p12;
            newV31 = -p0;
            newV0 = p1;
            newV23 = -(newV8 = p3) - p2;

            p0 = (s0 - s31) * Cos164;
            p1 = (s1 - s30) * Cos364;
            p2 = (s2 - s29) * Cos564;
            p3 = (s3 - s28) * Cos764;
            p4 = (s4 - s27) * Cos964;
            p5 = (s5 - s26) * Cos1164;
            p6 = (s6 - s25) * Cos1364;
            p7 = (s7 - s24) * Cos1564;
            p8 = (s8 - s23) * Cos1764;
            p9 = (s9 - s22) * Cos1964;
            p10 = (s10 - s21) * Cos2164;
            p11 = (s11 - s20) * Cos2364;
            p12 = (s12 - s19) * Cos2564;
            p13 = (s13 - s18) * Cos2764;
            p14 = (s14 - s17) * Cos2964;
            p15 = (s15 - s16) * Cos3164;

            pp0 = p0 + p15;
            pp1 = p1 + p14;
            pp2 = p2 + p13;
            pp3 = p3 + p12;
            pp4 = p4 + p11;
            pp5 = p5 + p10;
            pp6 = p6 + p9;
            pp7 = p7 + p8;
            pp8 = (p0 - p15) * Cos132;
            pp9 = (p1 - p14) * Cos332;
            pp10 = (p2 - p13) * Cos532;
            pp11 = (p3 - p12) * Cos732;
            pp12 = (p4 - p11) * Cos932;
            pp13 = (p5 - p10) * Cos1132;
            pp14 = (p6 - p9) * Cos1332;
            pp15 = (p7 - p8) * Cos1532;

            p0 = pp0 + pp7;
            p1 = pp1 + pp6;
            p2 = pp2 + pp5;
            p3 = pp3 + pp4;
            p4 = (pp0 - pp7) * Cos116;
            p5 = (pp1 - pp6) * Cos316;
            p6 = (pp2 - pp5) * Cos516;
            p7 = (pp3 - pp4) * Cos716;
            p8 = pp8 + pp15;
            p9 = pp9 + pp14;
            p10 = pp10 + pp13;
            p11 = pp11 + pp12;
            p12 = (pp8 - pp15) * Cos116;
            p13 = (pp9 - pp14) * Cos316;
            p14 = (pp10 - pp13) * Cos516;
            p15 = (pp11 - pp12) * Cos716;

            pp0 = p0 + p3;
            pp1 = p1 + p2;
            pp2 = (p0 - p3) * Cos18;
            pp3 = (p1 - p2) * Cos38;
            pp4 = p4 + p7;
            pp5 = p5 + p6;
            pp6 = (p4 - p7) * Cos18;
            pp7 = (p5 - p6) * Cos38;
            pp8 = p8 + p11;
            pp9 = p9 + p10;
            pp10 = (p8 - p11) * Cos18;
            pp11 = (p9 - p10) * Cos38;
            pp12 = p12 + p15;
            pp13 = p13 + p14;
            pp14 = (p12 - p15) * Cos18;
            pp15 = (p13 - p14) * Cos38;

            p0 = pp0 + pp1;
            p1 = (pp0 - pp1) * Cos14;
            p2 = pp2 + pp3;
            p3 = (pp2 - pp3) * Cos14;
            p4 = pp4 + pp5;
            p5 = (pp4 - pp5) * Cos14;
            p6 = pp6 + pp7;
            p7 = (pp6 - pp7) * Cos14;
            p8 = pp8 + pp9;
            p9 = (pp8 - pp9) * Cos14;
            p10 = pp10 + pp11;
            p11 = (pp10 - pp11) * Cos14;
            p12 = pp12 + pp13;
            p13 = (pp12 - pp13) * Cos14;
            p14 = pp14 + pp15;
            p15 = (pp14 - pp15) * Cos14;

            // manually doing something that a compiler should handle sucks
            // coding like this is hard to read
            float tmp2;
            newV5 = (newV11 = (newV13 = (newV15 = p15) + p7) + p11) + p5 + p13;
            newV7 = (newV9 = p15 + p11 + p3) + p13;
            newV16 = -(newV1 = (tmp1 = p13 + p15 + p9) + p1) - p14;
            newV18 = -(newV3 = tmp1 + p5 + p7) - p6 - p14;

            newV22 = (tmp1 = -p10 - p11 - p14 - p15) - p13 - p2 - p3;
            newV20 = tmp1 - p13 - p5 - p6 - p7;
            newV24 = tmp1 - p12 - p2 - p3;
            newV26 = tmp1 - p12 - (tmp2 = p4 + p6 + p7);
            newV30 = (tmp1 = -p8 - p12 - p14 - p15) - p0;
            newV28 = tmp1 - tmp2;

            // insert V[0-15] (== new_v[0-15]) into actual v:
            // float[] x2 = actual_v + actual_write_pos;
            float[] dest = _ActualV;

            int pos = _ActualWritePos;

            dest[0 + pos] = newV0;
            dest[16 + pos] = newV1;
            dest[32 + pos] = newV2;
            dest[48 + pos] = newV3;
            dest[64 + pos] = newV4;
            dest[80 + pos] = newV5;
            dest[96 + pos] = newV6;
            dest[112 + pos] = newV7;
            dest[128 + pos] = newV8;
            dest[144 + pos] = newV9;
            dest[160 + pos] = newV10;
            dest[176 + pos] = newV11;
            dest[192 + pos] = newV12;
            dest[208 + pos] = newV13;
            dest[224 + pos] = newV14;
            dest[240 + pos] = newV15;

            // V[16] is always 0.0:
            dest[256 + pos] = 0.0f;

            // insert V[17-31] (== -new_v[15-1]) into actual v:
            dest[272 + pos] = -newV15;
            dest[288 + pos] = -newV14;
            dest[304 + pos] = -newV13;
            dest[320 + pos] = -newV12;
            dest[336 + pos] = -newV11;
            dest[352 + pos] = -newV10;
            dest[368 + pos] = -newV9;
            dest[384 + pos] = -newV8;
            dest[400 + pos] = -newV7;
            dest[416 + pos] = -newV6;
            dest[432 + pos] = -newV5;
            dest[448 + pos] = -newV4;
            dest[464 + pos] = -newV3;
            dest[480 + pos] = -newV2;
            dest[496 + pos] = -newV1;

            // insert V[32] (== -new_v[0]) into other v:
            dest = _ActualV == _V1 ? _V2 : _V1;

            dest[0 + pos] = -newV0;
            // insert V[33-48] (== new_v[16-31]) into other v:
            dest[16 + pos] = newV16;
            dest[32 + pos] = newV17;
            dest[48 + pos] = newV18;
            dest[64 + pos] = newV19;
            dest[80 + pos] = newV20;
            dest[96 + pos] = newV21;
            dest[112 + pos] = newV22;
            dest[128 + pos] = newV23;
            dest[144 + pos] = newV24;
            dest[160 + pos] = newV25;
            dest[176 + pos] = newV26;
            dest[192 + pos] = newV27;
            dest[208 + pos] = newV28;
            dest[224 + pos] = newV29;
            dest[240 + pos] = newV30;
            dest[256 + pos] = newV31;

            // insert V[49-63] (== new_v[30-16]) into other v:
            dest[272 + pos] = newV30;
            dest[288 + pos] = newV29;
            dest[304 + pos] = newV28;
            dest[320 + pos] = newV27;
            dest[336 + pos] = newV26;
            dest[352 + pos] = newV25;
            dest[368 + pos] = newV24;
            dest[384 + pos] = newV23;
            dest[400 + pos] = newV22;
            dest[416 + pos] = newV21;
            dest[432 + pos] = newV20;
            dest[448 + pos] = newV19;
            dest[464 + pos] = newV18;
            dest[480 + pos] = newV17;
            dest[496 + pos] = newV16;
        }

        private void compute_pc_samples0(ABuffer buffer) {
            float[] vp = _ActualV;
            //int inc = v_inc;
            float[] tmpOut = _TmpOut;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++) {
                float pcSample;
                float[] dp = _d16[i];
                pcSample =
                    (vp[0 + dvp] * dp[0] + vp[15 + dvp] * dp[1] + vp[14 + dvp] * dp[2] + vp[13 + dvp] * dp[3] +
                     vp[12 + dvp] * dp[4] + vp[11 + dvp] * dp[5] + vp[10 + dvp] * dp[6] + vp[9 + dvp] * dp[7] +
                     vp[8 + dvp] * dp[8] + vp[7 + dvp] * dp[9] + vp[6 + dvp] * dp[10] + vp[5 + dvp] * dp[11] +
                     vp[4 + dvp] * dp[12] + vp[3 + dvp] * dp[13] + vp[2 + dvp] * dp[14] + vp[1 + dvp] * dp[15]) *
                    _Scalefactor;

                tmpOut[i] = pcSample;

                dvp += 16;
            }
            // for
        }

        private void compute_pc_samples1(ABuffer buffer) {
            float[] vp = _ActualV;
            //int inc = v_inc;
            float[] tmpOut = _TmpOut;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++) {
                float[] dp = _d16[i];
                float pcSample;

                pcSample =
                    (vp[1 + dvp] * dp[0] + vp[0 + dvp] * dp[1] + vp[15 + dvp] * dp[2] + vp[14 + dvp] * dp[3] +
                     vp[13 + dvp] * dp[4] + vp[12 + dvp] * dp[5] + vp[11 + dvp] * dp[6] + vp[10 + dvp] * dp[7] +
                     vp[9 + dvp] * dp[8] + vp[8 + dvp] * dp[9] + vp[7 + dvp] * dp[10] + vp[6 + dvp] * dp[11] +
                     vp[5 + dvp] * dp[12] + vp[4 + dvp] * dp[13] + vp[3 + dvp] * dp[14] + vp[2 + dvp] * dp[15]) *
                    _Scalefactor;

                tmpOut[i] = pcSample;

                dvp += 16;
            }
            // for
        }

        private void compute_pc_samples2(ABuffer buffer) {
            float[] vp = _ActualV;

            //int inc = v_inc;
            float[] tmpOut = _TmpOut;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++) {
                float[] dp = _d16[i];
                float pcSample;

                pcSample =
                    (vp[2 + dvp] * dp[0] + vp[1 + dvp] * dp[1] + vp[0 + dvp] * dp[2] + vp[15 + dvp] * dp[3] +
                     vp[14 + dvp] * dp[4] + vp[13 + dvp] * dp[5] + vp[12 + dvp] * dp[6] + vp[11 + dvp] * dp[7] +
                     vp[10 + dvp] * dp[8] + vp[9 + dvp] * dp[9] + vp[8 + dvp] * dp[10] + vp[7 + dvp] * dp[11] +
                     vp[6 + dvp] * dp[12] + vp[5 + dvp] * dp[13] + vp[4 + dvp] * dp[14] + vp[3 + dvp] * dp[15]) *
                    _Scalefactor;

                tmpOut[i] = pcSample;

                dvp += 16;
            }
            // for
        }

        private void compute_pc_samples3(ABuffer buffer) {
            float[] vp = _ActualV;

            float[] tmpOut = _TmpOut;
            int dvp = 0;

            for (int i = 0; i < 32; i++) {
                float[] dp = _d16[i];
                float pcSample = (vp[3 + dvp] * dp[0] + vp[2 + dvp] * dp[1] + vp[1 + dvp] * dp[2] + vp[0 + dvp] * dp[3] +
                                   vp[15 + dvp] * dp[4] + vp[14 + dvp] * dp[5] + vp[13 + dvp] * dp[6] + vp[12 + dvp] * dp[7] +
                                   vp[11 + dvp] * dp[8] + vp[10 + dvp] * dp[9] + vp[9 + dvp] * dp[10] + vp[8 + dvp] * dp[11] +
                                   vp[7 + dvp] * dp[12] + vp[6 + dvp] * dp[13] + vp[5 + dvp] * dp[14] + vp[4 + dvp] * dp[15]) *
                                  _Scalefactor;

                tmpOut[i] = pcSample;

                dvp += 16;
            }
        }

        private void compute_pc_samples4(ABuffer buffer) {
            float[] vp = _ActualV;

            float[] tmpOut = _TmpOut;
            int dvp = 0;

            for (int i = 0; i < 32; i++) {
                float[] dp = _d16[i];
                float pcSample = (vp[4 + dvp] * dp[0] + vp[3 + dvp] * dp[1] + vp[2 + dvp] * dp[2] + vp[1 + dvp] * dp[3] +
                                   vp[0 + dvp] * dp[4] + vp[15 + dvp] * dp[5] + vp[14 + dvp] * dp[6] + vp[13 + dvp] * dp[7] +
                                   vp[12 + dvp] * dp[8] + vp[11 + dvp] * dp[9] + vp[10 + dvp] * dp[10] + vp[9 + dvp] * dp[11] +
                                   vp[8 + dvp] * dp[12] + vp[7 + dvp] * dp[13] + vp[6 + dvp] * dp[14] + vp[5 + dvp] * dp[15]) *
                                  _Scalefactor;

                tmpOut[i] = pcSample;

                dvp += 16;
            }
            // for
        }

        private void compute_pc_samples5(ABuffer buffer) {
            float[] vp = _ActualV;

            float[] tmpOut = _TmpOut;
            int dvp = 0;

            for (int i = 0; i < 32; i++) {
                float[] dp = _d16[i];
                float pcSample = (vp[5 + dvp] * dp[0] + vp[4 + dvp] * dp[1] + vp[3 + dvp] * dp[2] + vp[2 + dvp] * dp[3] +
                                   vp[1 + dvp] * dp[4] + vp[0 + dvp] * dp[5] + vp[15 + dvp] * dp[6] + vp[14 + dvp] * dp[7] +
                                   vp[13 + dvp] * dp[8] + vp[12 + dvp] * dp[9] + vp[11 + dvp] * dp[10] + vp[10 + dvp] * dp[11] +
                                   vp[9 + dvp] * dp[12] + vp[8 + dvp] * dp[13] + vp[7 + dvp] * dp[14] + vp[6 + dvp] * dp[15]) *
                                  _Scalefactor;

                tmpOut[i] = pcSample;

                dvp += 16;
            }
            // for
        }

        private void compute_pc_samples6(ABuffer buffer) {
            float[] vp = _ActualV;
            float[] tmpOut = _TmpOut;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++) {
                float[] dp = _d16[i];
                float pcSample = (vp[6 + dvp] * dp[0] + vp[5 + dvp] * dp[1] + vp[4 + dvp] * dp[2] + vp[3 + dvp] * dp[3] +
                                   vp[2 + dvp] * dp[4] + vp[1 + dvp] * dp[5] + vp[0 + dvp] * dp[6] + vp[15 + dvp] * dp[7] +
                                   vp[14 + dvp] * dp[8] + vp[13 + dvp] * dp[9] + vp[12 + dvp] * dp[10] + vp[11 + dvp] * dp[11] +
                                   vp[10 + dvp] * dp[12] + vp[9 + dvp] * dp[13] + vp[8 + dvp] * dp[14] + vp[7 + dvp] * dp[15]) *
                                  _Scalefactor;

                tmpOut[i] = pcSample;

                dvp += 16;
            }
            // for
        }

        private void compute_pc_samples7(ABuffer buffer) {
            float[] vp = _ActualV;

            //int inc = v_inc;
            float[] tmpOut = _TmpOut;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++) {
                float[] dp = _d16[i];
                float pcSample;

                pcSample =
                    (vp[7 + dvp] * dp[0] + vp[6 + dvp] * dp[1] + vp[5 + dvp] * dp[2] + vp[4 + dvp] * dp[3] +
                     vp[3 + dvp] * dp[4] + vp[2 + dvp] * dp[5] + vp[1 + dvp] * dp[6] + vp[0 + dvp] * dp[7] +
                     vp[15 + dvp] * dp[8] + vp[14 + dvp] * dp[9] + vp[13 + dvp] * dp[10] + vp[12 + dvp] * dp[11] +
                     vp[11 + dvp] * dp[12] + vp[10 + dvp] * dp[13] + vp[9 + dvp] * dp[14] + vp[8 + dvp] * dp[15]) *
                    _Scalefactor;

                tmpOut[i] = pcSample;

                dvp += 16;
            }
            // for
        }

        private void compute_pc_samples8(ABuffer buffer) {
            float[] vp = _ActualV;

            //int inc = v_inc;
            float[] tmpOut = _TmpOut;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++) {
                float[] dp = _d16[i];
                float pcSample;

                pcSample =
                    (vp[8 + dvp] * dp[0] + vp[7 + dvp] * dp[1] + vp[6 + dvp] * dp[2] + vp[5 + dvp] * dp[3] +
                     vp[4 + dvp] * dp[4] + vp[3 + dvp] * dp[5] + vp[2 + dvp] * dp[6] + vp[1 + dvp] * dp[7] +
                     vp[0 + dvp] * dp[8] + vp[15 + dvp] * dp[9] + vp[14 + dvp] * dp[10] + vp[13 + dvp] * dp[11] +
                     vp[12 + dvp] * dp[12] + vp[11 + dvp] * dp[13] + vp[10 + dvp] * dp[14] + vp[9 + dvp] * dp[15]) *
                    _Scalefactor;

                tmpOut[i] = pcSample;

                dvp += 16;
            }
            // for
        }

        private void compute_pc_samples9(ABuffer buffer) {
            float[] vp = _ActualV;

            //int inc = v_inc;
            float[] tmpOut = _TmpOut;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++) {
                float[] dp = _d16[i];
                float pcSample;

                pcSample =
                    (vp[9 + dvp] * dp[0] + vp[8 + dvp] * dp[1] + vp[7 + dvp] * dp[2] + vp[6 + dvp] * dp[3] +
                     vp[5 + dvp] * dp[4] + vp[4 + dvp] * dp[5] + vp[3 + dvp] * dp[6] + vp[2 + dvp] * dp[7] +
                     vp[1 + dvp] * dp[8] + vp[0 + dvp] * dp[9] + vp[15 + dvp] * dp[10] + vp[14 + dvp] * dp[11] +
                     vp[13 + dvp] * dp[12] + vp[12 + dvp] * dp[13] + vp[11 + dvp] * dp[14] + vp[10 + dvp] * dp[15]) *
                    _Scalefactor;

                tmpOut[i] = pcSample;

                dvp += 16;
            }
            // for
        }

        private void compute_pc_samples10(ABuffer buffer) {
            float[] vp = _ActualV;
            //int inc = v_inc;
            float[] tmpOut = _TmpOut;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++) {
                float[] dp = _d16[i];
                float pcSample;

                pcSample =
                    (vp[10 + dvp] * dp[0] + vp[9 + dvp] * dp[1] + vp[8 + dvp] * dp[2] + vp[7 + dvp] * dp[3] +
                     vp[6 + dvp] * dp[4] + vp[5 + dvp] * dp[5] + vp[4 + dvp] * dp[6] + vp[3 + dvp] * dp[7] +
                     vp[2 + dvp] * dp[8] + vp[1 + dvp] * dp[9] + vp[0 + dvp] * dp[10] + vp[15 + dvp] * dp[11] +
                     vp[14 + dvp] * dp[12] + vp[13 + dvp] * dp[13] + vp[12 + dvp] * dp[14] + vp[11 + dvp] * dp[15]) *
                    _Scalefactor;

                tmpOut[i] = pcSample;

                dvp += 16;
            }
            // for
        }

        private void compute_pc_samples11(ABuffer buffer) {
            float[] vp = _ActualV;

            //int inc = v_inc;
            float[] tmpOut = _TmpOut;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++) {
                float[] dp = _d16[i];
                float pcSample;

                pcSample =
                    (vp[11 + dvp] * dp[0] + vp[10 + dvp] * dp[1] + vp[9 + dvp] * dp[2] + vp[8 + dvp] * dp[3] +
                     vp[7 + dvp] * dp[4] + vp[6 + dvp] * dp[5] + vp[5 + dvp] * dp[6] + vp[4 + dvp] * dp[7] +
                     vp[3 + dvp] * dp[8] + vp[2 + dvp] * dp[9] + vp[1 + dvp] * dp[10] + vp[0 + dvp] * dp[11] +
                     vp[15 + dvp] * dp[12] + vp[14 + dvp] * dp[13] + vp[13 + dvp] * dp[14] + vp[12 + dvp] * dp[15]) *
                    _Scalefactor;

                tmpOut[i] = pcSample;

                dvp += 16;
            }
            // for
        }

        private void compute_pc_samples12(ABuffer buffer) {
            float[] vp = _ActualV;
            //int inc = v_inc;
            float[] tmpOut = _TmpOut;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++) {
                float[] dp = _d16[i];
                float pcSample;

                pcSample =
                    (vp[12 + dvp] * dp[0] + vp[11 + dvp] * dp[1] + vp[10 + dvp] * dp[2] + vp[9 + dvp] * dp[3] +
                     vp[8 + dvp] * dp[4] + vp[7 + dvp] * dp[5] + vp[6 + dvp] * dp[6] + vp[5 + dvp] * dp[7] +
                     vp[4 + dvp] * dp[8] + vp[3 + dvp] * dp[9] + vp[2 + dvp] * dp[10] + vp[1 + dvp] * dp[11] +
                     vp[0 + dvp] * dp[12] + vp[15 + dvp] * dp[13] + vp[14 + dvp] * dp[14] + vp[13 + dvp] * dp[15]) *
                    _Scalefactor;

                tmpOut[i] = pcSample;

                dvp += 16;
            }
            // for
        }

        private void compute_pc_samples13(ABuffer buffer) {
            float[] vp = _ActualV;

            //int inc = v_inc;
            float[] tmpOut = _TmpOut;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++) {
                float[] dp = _d16[i];
                float pcSample;

                pcSample =
                    (vp[13 + dvp] * dp[0] + vp[12 + dvp] * dp[1] + vp[11 + dvp] * dp[2] + vp[10 + dvp] * dp[3] +
                     vp[9 + dvp] * dp[4] + vp[8 + dvp] * dp[5] + vp[7 + dvp] * dp[6] + vp[6 + dvp] * dp[7] +
                     vp[5 + dvp] * dp[8] + vp[4 + dvp] * dp[9] + vp[3 + dvp] * dp[10] + vp[2 + dvp] * dp[11] +
                     vp[1 + dvp] * dp[12] + vp[0 + dvp] * dp[13] + vp[15 + dvp] * dp[14] + vp[14 + dvp] * dp[15]) *
                    _Scalefactor;

                tmpOut[i] = pcSample;

                dvp += 16;
            }
            // for
        }

        private void compute_pc_samples14(ABuffer buffer) {
            float[] vp = _ActualV;

            //int inc = v_inc;
            float[] tmpOut = _TmpOut;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++) {
                float[] dp = _d16[i];
                float pcSample;

                pcSample =
                    (vp[14 + dvp] * dp[0] + vp[13 + dvp] * dp[1] + vp[12 + dvp] * dp[2] + vp[11 + dvp] * dp[3] +
                     vp[10 + dvp] * dp[4] + vp[9 + dvp] * dp[5] + vp[8 + dvp] * dp[6] + vp[7 + dvp] * dp[7] +
                     vp[6 + dvp] * dp[8] + vp[5 + dvp] * dp[9] + vp[4 + dvp] * dp[10] + vp[3 + dvp] * dp[11] +
                     vp[2 + dvp] * dp[12] + vp[1 + dvp] * dp[13] + vp[0 + dvp] * dp[14] + vp[15 + dvp] * dp[15]) *
                    _Scalefactor;

                tmpOut[i] = pcSample;

                dvp += 16;
            }
            // for
        }

        private void Compute_pc_samples15(ABuffer buffer) {
            float[] vp = _ActualV;

            //int inc = v_inc;
            float[] tmpOut = _TmpOut;
            int dvp = 0;

            // fat chance of having this loop unroll
            for (int i = 0; i < 32; i++) {
                float[] dp = _d16[i];
                float pcSample = (vp[15 + dvp] * dp[0] + vp[14 + dvp] * dp[1] + vp[13 + dvp] * dp[2] + vp[12 + dvp] * dp[3] +
                                  vp[11 + dvp] * dp[4] + vp[10 + dvp] * dp[5] + vp[9 + dvp] * dp[6] + vp[8 + dvp] * dp[7] +
                                  vp[7 + dvp] * dp[8] + vp[6 + dvp] * dp[9] + vp[5 + dvp] * dp[10] + vp[4 + dvp] * dp[11] +
                                  vp[3 + dvp] * dp[12] + vp[2 + dvp] * dp[13] + vp[1 + dvp] * dp[14] + vp[0 + dvp] * dp[15]) *
                                 _Scalefactor;

                tmpOut[i] = pcSample;
                dvp += 16;
            }
            // for
        }

        private void compute_pc_samples(ABuffer buffer) {
            switch (_ActualWritePos) {
                case 0:
                    compute_pc_samples0(buffer);
                    break;

                case 1:
                    compute_pc_samples1(buffer);
                    break;

                case 2:
                    compute_pc_samples2(buffer);
                    break;

                case 3:
                    compute_pc_samples3(buffer);
                    break;

                case 4:
                    compute_pc_samples4(buffer);
                    break;

                case 5:
                    compute_pc_samples5(buffer);
                    break;

                case 6:
                    compute_pc_samples6(buffer);
                    break;

                case 7:
                    compute_pc_samples7(buffer);
                    break;

                case 8:
                    compute_pc_samples8(buffer);
                    break;

                case 9:
                    compute_pc_samples9(buffer);
                    break;

                case 10:
                    compute_pc_samples10(buffer);
                    break;

                case 11:
                    compute_pc_samples11(buffer);
                    break;

                case 12:
                    compute_pc_samples12(buffer);
                    break;

                case 13:
                    compute_pc_samples13(buffer);
                    break;

                case 14:
                    compute_pc_samples14(buffer);
                    break;

                case 15:
                    Compute_pc_samples15(buffer);
                    break;
            }

            buffer?.AppendSamples(_Channel, _TmpOut);
        }

        /// <summary>
        /// Calculate 32 PCM samples and put the into the Obuffer-object.
        /// </summary>
        internal void calculate_pc_samples(ABuffer buffer) {
            ComputeNewValues();
            compute_pc_samples(buffer);

            _ActualWritePos = (_ActualWritePos + 1) & 0xf;
            _ActualV = _ActualV == _V1 ? _V2 : _V1;

            // initialize samples[]:
            //for (register float *floatp = samples + 32; floatp > samples; )
            // *--floatp = 0.0f;  

            // MDM: this may not be necessary. The Layer III decoder always
            // outputs 32 subband samples, but I haven't checked layer I & II.
            for (int p = 0; p < 32; p++)
                _Samples[p] = 0.0f;
        }

        /// <summary>
        /// Converts a 1D array into a number of smaller arrays. This is used
        /// to achieve offset + constant indexing into an array. Each sub-array
        /// represents a block of values of the original array.
        /// </summary>
        /// <param name="array">
        /// The array to split up into blocks.
        /// </param>
        /// <param name="blockSize">
        /// The size of the blocks to split the array
        /// into. This must be an exact divisor of
        /// the length of the array, or some data
        /// will be lost from the main array.
        /// </param>
        /// <returns>
        /// An array of arrays in which each element in the returned
        /// array will be of length blockSize.
        /// </returns>
        private static float[][] SplitArray(float[] array, int blockSize) {
            int size = array.Length / blockSize;
            float[][] split = new float[size][];
            for (int i = 0; i < size; i++) {
                split[i] = SubArray(array, i * blockSize, blockSize);
            }
            return split;
        }

        private static float[] SubArray(float[] array, int offs, int len) {
            if (offs + len > array.Length) {
                len = array.Length - offs;
            }

            if (len < 0)
                len = 0;

            float[] subarray = new float[len];
            for (int i = 0; i < len; i++) {
                subarray[i] = array[offs + i];
            }

            return subarray;
        }
    }

    /// <summary>
    /// Base Class for audio output.
    /// </summary>
    public abstract class ABuffer {
        internal const int OBUFFERSIZE = 2 * 1152; // max. 2 * 1152 samples per frame
        internal const int MAXCHANNELS = 2; // max. number of channels

        /// <summary>
        /// Takes a 16 Bit PCM sample.
        /// </summary>
        protected abstract void Append(int channel, short sampleValue);

        /// <summary>
        /// Accepts 32 new PCM samples.
        /// </summary>
        internal virtual void AppendSamples(int channel, float[] samples) {
            for (int i = 0; i < 32; i++) {
                Append(channel, Clip(samples[i]));
            }
        }

        /// <summary>
        /// Clip Sample to 16 Bits
        /// </summary>
        private static short Clip(float sample) => sample > 32767.0f ? (short)32767 : sample < -32768.0f ? (short)-32768 : (short)sample;

        /// <summary>
        /// Write the samples to the file or directly to the audio hardware.
        /// </summary>
        internal abstract void WriteBuffer(int val);

        internal abstract void Close();

        /// <summary>
        /// Clears all data in the buffer (for seeking).
        /// </summary>
        internal abstract void ClearBuffer();

        /// <summary>
        /// Notify the buffer that the user has stopped the stream.
        /// </summary>
        internal abstract void SetStopFlag();
    }

    /// <summary>
    /// The SampleBuffer class implements an output buffer
    /// that provides storage for a fixed size block of samples.
    /// </summary>
    public class SampleBuffer : ABuffer {
        private readonly short[] _Buffer;
        private readonly int[] _Bufferp;
        private readonly int _Channels;
        private readonly int _Frequency;

        internal SampleBuffer(int sampleFrequency, int numberOfChannels) {
            _Buffer = new short[OBUFFERSIZE];
            _Bufferp = new int[MAXCHANNELS];
            _Channels = numberOfChannels;
            _Frequency = sampleFrequency;

            for (int i = 0; i < numberOfChannels; ++i)
                _Bufferp[i] = (short)i;
        }

        internal virtual int ChannelCount => _Channels;

        internal virtual int SampleFrequency => _Frequency;

        internal virtual short[] Buffer => _Buffer;

        internal virtual int BufferLength => _Bufferp[0];

        /// <summary>
        /// Takes a 16 Bit PCM sample.
        /// </summary>
        protected override void Append(int channel, short valueRenamed) {
            _Buffer[_Bufferp[channel]] = valueRenamed;
            _Bufferp[channel] += _Channels;
        }

        internal override void AppendSamples(int channel, float[] samples) {
            int pos = _Bufferp[channel];

            short s;
            float fs;
            for (int i = 0; i < 32;) {
                fs = samples[i++];
                fs = fs > 32767.0f ? 32767.0f : fs < -32767.0f ? -32767.0f : fs;

                //UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
                s = (short)fs;
                _Buffer[pos] = s;
                pos += _Channels;
            }

            _Bufferp[channel] = pos;
        }

        /// <summary>
        /// Write the samples to the file (Random Acces).
        /// </summary>
        internal override void WriteBuffer(int val) {
            // for (int i = 0; i < channels; ++i) 
            // bufferp[i] = (short)i;
        }

        internal override void Close() { }

        /// <summary>
        /// *
        /// </summary>
        internal override void ClearBuffer() {
            for (int i = 0; i < _Channels; ++i)
                _Bufferp[i] = (short)i;
        }

        /// <summary>
        /// *
        /// </summary>
        internal override void SetStopFlag() { }
    }


    /// <summary>
    /// The Bistream class is responsible for parsing an MPEG audio bitstream.
    /// REVIEW: much of the parsing currently occurs in the various decoders.
    /// This should be moved into this class and associated inner classes.
    /// </summary>
    public sealed class Bitstream {
        /// <summary>
        /// Maximum size of the frame buffer:
        /// 1730 bytes per frame: 144 * 384kbit/s / 32000 Hz + 2 Bytes CRC
        /// </summary>
        private const int BUFFER_INT_SIZE = 433;

        /// <summary>
        /// Synchronization control constant for the initial
        /// synchronization to the start of a frame.
        /// </summary>
        internal const sbyte INITIAL_SYNC = 0;

        /// <summary>
        /// Synchronization control constant for non-inital frame
        /// synchronizations.
        /// </summary>
        internal const sbyte STRICT_SYNC = 1;

        private readonly int[] _Bitmask = {
            0x00000000, 0x00000001, 0x00000003, 0x00000007, 0x0000000F, 0x0000001F, 0x0000003F, 0x0000007F,
            0x000000FF, 0x000001FF, 0x000003FF, 0x000007FF, 0x00000FFF, 0x00001FFF, 0x00003FFF, 0x00007FFF,
            0x0000FFFF, 0x0001FFFF
        };

        private readonly PushbackStream _SourceStream;

        /// <summary>
        /// Number (0-31, from MSB to LSB) of next bit for get_bits()
        /// </summary>
        private int _BitIndex;

        private Crc16[] _CRC;

        /// <summary>
        /// The bytes read from the stream.
        /// </summary>
        private sbyte[] _FrameBytes;

        /// <summary>
        /// The frame buffer that holds the data for the current frame.
        /// </summary>
        private int[] _FrameBuffer;

        /// <summary>
        /// Number of valid bytes in the frame buffer.
        /// </summary>
        private int _FrameSize;

        private Header _Header;

        private bool _SingleChMode;

        private sbyte[] _SyncBuffer;

        /// <summary>
        /// The current specified syncword
        /// </summary>
        private int _SyncWord;

        /// <summary>
        /// Index into framebuffer where the next bits are retrieved.
        /// </summary>
        private int _WordPointer;

        /// <summary>
        /// Create a IBitstream that reads data from a given InputStream.
        /// </summary>
        internal Bitstream(PushbackStream stream) {
            InitBlock();
            _SourceStream = stream ?? throw new NullReferenceException("in stream is null");
            CloseFrame();
        }

        private void InitBlock() {
            _CRC = new Crc16[1];
            _SyncBuffer = new sbyte[4];
            _FrameBytes = new sbyte[BUFFER_INT_SIZE * 4];
            _FrameBuffer = new int[BUFFER_INT_SIZE];
            _Header = new Header();
        }

        internal void Close() {
            try {
                _SourceStream.Close();
            }
            catch (IOException ex) {
                throw NewBitstreamException(BitstreamErrors.STREA_ERROR, ex);
            }
        }

        /// <summary>
        /// Reads and parses the next frame from the input source.
        /// </summary>
        /// <returns>
        /// The Header describing details of the frame read,
        /// or null if the end of the stream has been reached.
        /// </returns>
        internal Header ReadFrame() {
            Header result = null;
            try {
                result = ReadNextFrame();
            }
            catch (BitstreamException ex) {
                if (ex.ErrorCode != BitstreamErrors.STREA_EOF) {
                    // wrap original exception so stack trace is maintained.
                    throw NewBitstreamException(ex.ErrorCode, ex);
                }
            }
            return result;
        }

        private Header ReadNextFrame() {
            if (_FrameSize == -1) {
                // entire frame is read by the header class.
                _Header.read_header(this, _CRC);
            }

            return _Header;
        }

        /// <summary>
        /// Unreads the bytes read from the frame.
        /// Throws BitstreamException.
        /// REVIEW: add new error codes for this.
        /// </summary>
        internal void UnreadFrame() {
            if (_WordPointer == -1 && _BitIndex == -1 && _FrameSize > 0) {
                try {
                    _SourceStream.UnRead(_FrameSize);
                }
                catch {
                    throw NewBitstreamException(BitstreamErrors.STREA_ERROR);
                }
            }
        }

        internal void CloseFrame() {
            _FrameSize = -1;
            _WordPointer = -1;
            _BitIndex = -1;
        }

        /// <summary>
        /// Determines if the next 4 bytes of the stream represent a frame header.
        /// </summary>
        internal bool IsSyncCurrentPosition(int syncmode) {
            int read = ReadBytes(_SyncBuffer, 0, 4);
            int headerstring = ((_SyncBuffer[0] << 24) & (int)SupportClass.Identity(0xFF000000)) |
                               ((_SyncBuffer[1] << 16) & 0x00FF0000) | ((_SyncBuffer[2] << 8) & 0x0000FF00) |
                               ((_SyncBuffer[3] << 0) & 0x000000FF);

            try {
                _SourceStream.UnRead(read);
            }
            catch (Exception e) {
                throw new MP3SharpException("Could not restore file after reading frame header.", e);
            }

            bool sync = false;
            switch (read) {
                case 0:
                    sync = true;
                    break;

                case 4:
                    sync = IsSyncMark(headerstring, syncmode, _SyncWord);
                    break;
            }

            return sync;
        }

        // REVIEW: this class should provide inner classes to
        // parse the frame contents. Eventually, readBits will
        // be removed.
        internal int ReadBits(int n) => GetBitsFromBuffer(n);

        // REVIEW: implement CRC check.
        internal int ReadCheckedBits(int n) => GetBitsFromBuffer(n);

        internal BitstreamException NewBitstreamException(int errorcode) => new BitstreamException(errorcode, null);

        internal BitstreamException NewBitstreamException(int errorcode, Exception throwable) => new BitstreamException(errorcode, throwable);

        /// <summary>
        /// Get next 32 bits from bitstream.
        /// They are stored in the headerstring.
        /// syncmod allows Synchro flag ID
        /// The returned value is False at the end of stream.
        /// </summary>
        internal int SyncHeader(sbyte syncmode) {
            bool sync = false;
            // read additional 2 bytes
            int bytesRead = ReadBytes(_SyncBuffer, 0, 3);
            if (bytesRead != 3) {
                throw NewBitstreamException(BitstreamErrors.STREA_EOF, null);
            }

            int headerstring = ((_SyncBuffer[0] << 16) & 0x00FF0000) | ((_SyncBuffer[1] << 8) & 0x0000FF00) |
                           ((_SyncBuffer[2] << 0) & 0x000000FF);

            do {
                headerstring <<= 8;
                if (ReadBytes(_SyncBuffer, 3, 1) != 1) {
                    throw NewBitstreamException(BitstreamErrors.STREA_EOF, null);
                }
                headerstring |= _SyncBuffer[3] & 0x000000FF;
                if (CheckAndSkipId3Tag(headerstring)) {
                    bytesRead = ReadBytes(_SyncBuffer, 0, 3);
                    if (bytesRead != 3) {
                        throw NewBitstreamException(BitstreamErrors.STREA_EOF, null);
                    }
                    headerstring = ((_SyncBuffer[0] << 16) & 0x00FF0000) | ((_SyncBuffer[1] << 8) & 0x0000FF00) |
                                   ((_SyncBuffer[2] << 0) & 0x000000FF);
                    continue;
                }
                sync = IsSyncMark(headerstring, syncmode, _SyncWord);
            } while (!sync);

            return headerstring;
        }
        /// <summary>
        /// check and skip the id3v2 tag.
        /// mp3 frame sync inside id3 tag may led false decodeing.
        /// id3 tag do have a flag for "unsynchronisation", indicate there are no
        /// frame sync inside tags, scence decoder don't care about tags, we just
        /// skip all tags.
        /// </summary>
        internal bool CheckAndSkipId3Tag(int headerstring) {
            bool id3 = (headerstring & 0xFFFFFF00) == 0x49443300;

            if (id3) {
                sbyte[] id3_header = new sbyte[6];

                if (ReadBytes(id3_header, 0, 6) != 6)
                    throw NewBitstreamException(BitstreamErrors.STREA_EOF, null);

                // id3 header uses 4 bytes to store the size of all tags,
                // but only the low 7 bits of each byte is used, to avoid
                // mp3 frame sync.
                int id3_tag_size = 0;
                id3_tag_size |= id3_header[2] & 0x0000007F; id3_tag_size <<= 7;
                id3_tag_size |= id3_header[3] & 0x0000007F; id3_tag_size <<= 7;
                id3_tag_size |= id3_header[4] & 0x0000007F; id3_tag_size <<= 7;
                id3_tag_size |= id3_header[5] & 0x0000007F;

                sbyte[] id3_tag = new sbyte[id3_tag_size];

                if (ReadBytes(id3_tag, 0, id3_tag_size) != id3_tag_size)
                    throw NewBitstreamException(BitstreamErrors.STREA_EOF, null);
            }

            return id3;
        }

        internal bool IsSyncMark(int headerstring, int syncmode, int word) {
            bool sync;
            if (syncmode == INITIAL_SYNC) {
                //sync =  ((headerstring & 0xFFF00000) == 0xFFF00000);
                sync = (headerstring & 0xFFE00000) == 0xFFE00000; // SZD: MPEG 2.5
            }
            else {
                //sync = ((headerstring & 0xFFF80C00) == word) 
                sync = (headerstring & 0xFFE00000) == 0xFFE00000 // ROB -- THIS IS PROBABLY WRONG. A WEAKER CHECK.
                       && (headerstring & 0x000000C0) == 0x000000C0 == _SingleChMode;
            }

            // filter out invalid sample rate
            if (sync) {
                sync = (SupportClass.URShift(headerstring, 10) & 3) != 3;
                // if (!sync) Trace.WriteLine("INVALID SAMPLE RATE DETECTED", "Bitstream");
            }
            // filter out invalid layer
            if (sync) {
                sync = (SupportClass.URShift(headerstring, 17) & 3) != 0;
                // if (!sync) Trace.WriteLine("INVALID LAYER DETECTED", "Bitstream");
            }
            // filter out invalid version
            if (sync) {
                sync = (SupportClass.URShift(headerstring, 19) & 3) != 1;
                if (!sync) Console.WriteLine("INVALID VERSION DETECTED");
            }
            return sync;
        }

        /// <summary>
        /// Reads the data for the next frame. The frame is not parsed
        /// until parse frame is called.
        /// </summary>
        internal void Read_frame_data(int bytesize) {
            ReadFully(_FrameBytes, 0, bytesize);
            _FrameSize = bytesize;
            _WordPointer = -1;
            _BitIndex = -1;
        }

        /// <summary>
        /// Parses the data previously read with read_frame_data().
        /// </summary>
        internal void ParseFrame() {
            // Convert Bytes read to int
            int b = 0;
            sbyte[] byteread = _FrameBytes;
            int bytesize = _FrameSize;

            for (int k = 0; k < bytesize; k = k + 4) {
                sbyte b0 = byteread[k];
                sbyte b1 = 0;
                sbyte b2 = 0;
                sbyte b3 = 0;
                if (k + 1 < bytesize)
                    b1 = byteread[k + 1];
                if (k + 2 < bytesize)
                    b2 = byteread[k + 2];
                if (k + 3 < bytesize)
                    b3 = byteread[k + 3];
                _FrameBuffer[b++] = ((b0 << 24) & (int)SupportClass.Identity(0xFF000000)) | ((b1 << 16) & 0x00FF0000) |
                                    ((b2 << 8) & 0x0000FF00) | (b3 & 0x000000FF);
            }

            _WordPointer = 0;
            _BitIndex = 0;
        }

        /// <summary>
        /// Read bits from buffer into the lower bits of an unsigned int.
        /// The LSB contains the latest read bit of the stream.
        /// (between 1 and 16, inclusive).
        /// </summary>
        internal int GetBitsFromBuffer(int countBits) {
            int returnvalue;
            int sum = _BitIndex + countBits;
            if (_WordPointer < 0) {
                _WordPointer = 0;
            }
            if (sum <= 32) {
                // all bits contained in *wordpointer
                returnvalue = SupportClass.URShift(_FrameBuffer[_WordPointer], 32 - sum) & _Bitmask[countBits];
                if ((_BitIndex += countBits) == 32) {
                    _BitIndex = 0;
                    _WordPointer++;
                }
                return returnvalue;
            }
            int right = _FrameBuffer[_WordPointer] & 0x0000FFFF;
            _WordPointer++;
            int left = _FrameBuffer[_WordPointer] & (int)SupportClass.Identity(0xFFFF0000);
            returnvalue = ((right << 16) & (int)SupportClass.Identity(0xFFFF0000)) | (SupportClass.URShift(left, 16) & 0x0000FFFF);
            returnvalue = SupportClass.URShift(returnvalue, 48 - sum);
            returnvalue &= _Bitmask[countBits];
            _BitIndex = sum - 32;
            return returnvalue;
        }

        /// <summary>
        /// Set the word we want to sync the header to.
        /// In Big-Endian byte order
        /// </summary>
        internal void SetSyncWord(int syncword0) {
            _SyncWord = syncword0 & unchecked((int)0xFFFFFF3F);
            _SingleChMode = (syncword0 & 0x000000C0) == 0x000000C0;
        }

        /// <summary>
        /// Reads the exact number of bytes from the source input stream into a byte array.
        /// </summary>
        private void ReadFully(sbyte[] b, int offs, int len) {
            try {
                while (len > 0) {
                    int bytesread = _SourceStream.Read(b, offs, len);
                    if (bytesread == -1 || bytesread == 0) // t/DD -- .NET returns 0 at end-of-stream!
                    {
                        // t/DD: this really SHOULD throw an exception here...
                        // Trace.WriteLine("readFully -- returning success at EOF? (" + bytesread + ")", "Bitstream");
                        while (len-- > 0) {
                            b[offs++] = 0;
                        }
                        break;
                        //throw newBitstreamException(UNEXPECTED_EOF, new EOFException());
                    }

                    offs += bytesread;
                    len -= bytesread;
                }
            }
            catch (IOException ex) {
                throw NewBitstreamException(BitstreamErrors.STREA_ERROR, ex);
            }
        }

        /// <summary>
        /// Simlar to readFully, but doesn't throw exception when EOF is reached.
        /// </summary>
        private int ReadBytes(sbyte[] b, int offs, int len) {
            int totalBytesRead = 0;
            try {
                while (len > 0) {
                    int bytesread = _SourceStream.Read(b, offs, len);
                    // for (int i = 0; i < len; i++) b[i] = (sbyte)Temp[i];
                    if (bytesread == -1 || bytesread == 0) {
                        break;
                    }
                    totalBytesRead += bytesread;
                    offs += bytesread;
                    len -= bytesread;
                }
            }
            catch (IOException ex) {
                throw NewBitstreamException(BitstreamErrors.STREA_ERROR, ex);
            }
            return totalBytesRead;
        }
    }

    /// <summary>
    /// Implementation of Bit Reservoir for Layer III.
    /// The implementation stores single bits as a word in the buffer. If
    /// a bit is set, the corresponding word in the buffer will be non-zero.
    /// If a bit is clear, the corresponding word is zero. Although this
    /// may seem waseful, this can be a factor of two quicker than
    /// packing 8 bits to a byte and extracting.
    /// </summary>

    // REVIEW: there is no range checking, so buffer underflow or overflow
    // can silently occur.
    internal sealed class BitReserve {
        /// <summary>
        /// Size of the internal buffer to store the reserved bits.
        /// Must be a power of 2. And x8, as each bit is stored as a single
        /// entry.
        /// </summary>
        private const int BUFSIZE = 4096 * 8;

        /// <summary>
        /// Mask that can be used to quickly implement the
        /// modulus operation on BUFSIZE.
        /// </summary>
        private const int BUFSIZE_MASK = BUFSIZE - 1;

        private int[] _Buffer;
        private int _Offset, _Totbit, _BufByteIdx;

        internal BitReserve() {
            InitBlock();

            _Offset = 0;
            _Totbit = 0;
            _BufByteIdx = 0;
        }

        private void InitBlock() {
            _Buffer = new int[BUFSIZE];
        }

        /// <summary>
        /// Return totbit Field.
        /// </summary>
        internal int HssTell() => _Totbit;

        /// <summary>
        /// Read a number bits from the bit stream.
        /// </summary>
        internal int ReadBits(int n) {
            _Totbit += n;

            int val = 0;

            int pos = _BufByteIdx;
            if (pos + n < BUFSIZE) {
                while (n-- > 0) {
                    val <<= 1;
                    val |= _Buffer[pos++] != 0 ? 1 : 0;
                }
            }
            else {
                while (n-- > 0) {
                    val <<= 1;
                    val |= _Buffer[pos] != 0 ? 1 : 0;
                    pos = (pos + 1) & BUFSIZE_MASK;
                }
            }
            _BufByteIdx = pos;
            return val;
        }

        /// <summary>
        /// Read 1 bit from the bit stream.
        /// </summary>
        internal int ReadOneBit() {
            _Totbit++;
            int val = _Buffer[_BufByteIdx];
            _BufByteIdx = (_BufByteIdx + 1) & BUFSIZE_MASK;
            return val;
        }

        /// <summary>
        /// Write 8 bits into the bit stream.
        /// </summary>
        internal void PutBuffer(int val) {
            int ofs = _Offset;
            _Buffer[ofs++] = val & 0x80;
            _Buffer[ofs++] = val & 0x40;
            _Buffer[ofs++] = val & 0x20;
            _Buffer[ofs++] = val & 0x10;
            _Buffer[ofs++] = val & 0x08;
            _Buffer[ofs++] = val & 0x04;
            _Buffer[ofs++] = val & 0x02;
            _Buffer[ofs++] = val & 0x01;
            if (ofs == BUFSIZE)
                _Offset = 0;
            else
                _Offset = ofs;
        }

        /// <summary>
        /// Rewind n bits in Stream.
        /// </summary>
        internal void RewindStreamBits(int bitCount) {
            _Totbit -= bitCount;
            _BufByteIdx -= bitCount;
            if (_BufByteIdx < 0)
                _BufByteIdx += BUFSIZE;
        }

        /// <summary>
        /// Rewind n bytes in Stream.
        /// </summary>
        internal void RewindStreamBytes(int byteCount) {
            int bits = byteCount << 3;
            _Totbit -= bits;
            _BufByteIdx -= bits;
            if (_BufByteIdx < 0)
                _BufByteIdx += BUFSIZE;
        }
    }

    /// <summary>
    /// This struct describes all error codes that can be thrown
    /// in BistreamExceptions.
    /// </summary>
    internal struct BitstreamErrors {
        internal const int UNKNOWN_ERROR = BITSTREAM_ERROR + 0;
        internal const int UNKNOWN_SAMPLE_RATE = BITSTREAM_ERROR + 1;
        internal const int STREA_ERROR = BITSTREAM_ERROR + 2;
        internal const int UNEXPECTED_EOF = BITSTREAM_ERROR + 3;
        internal const int STREA_EOF = BITSTREAM_ERROR + 4;
        internal const int BITSTREA_LAST = 0x1ff;

        internal const int BITSTREAM_ERROR = 0x100;
        internal const int DECODER_ERROR = 0x200;
    }

    /// <summary>
    /// Instances of BitstreamException are thrown
    /// when operations on a Bitstream fail.
    /// <p>
    /// The exception provides details of the exception condition
    /// in two ways:
    /// <ol>
    ///     <li>
    ///         as an error-code describing the nature of the error
    ///     </li>
    ///     <br></br>
    ///     <li>
    ///         as the Throwable instance, if any, that was thrown
    ///         indicating that an exceptional condition has occurred.
    ///     </li>
    /// </ol>
    /// </p>
    /// </summary>
    // [Serializable]
    public class BitstreamException : MP3SharpException {
        private int _ErrorCode;

        internal BitstreamException(string message, Exception inner) : base(message, inner) {
            InitBlock();
        }

        internal BitstreamException(int errorcode, Exception inner) : this(GetErrorString(errorcode), inner) {
            InitBlock();
            _ErrorCode = errorcode;
        }

        protected BitstreamException(SerializationInfo info, StreamingContext context) : base(info, context) {
            _ErrorCode = info.GetInt32("ErrorCode");
        }

        internal virtual int ErrorCode => _ErrorCode;

        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            if (info == null) {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue("ErrorCode", _ErrorCode);
            base.GetObjectData(info, context);
        }

        private void InitBlock() {
            _ErrorCode = BitstreamErrors.UNKNOWN_ERROR;
        }

        internal static string GetErrorString(int errorcode) => "Bitstream errorcode " + Convert.ToString(errorcode, 16);
    }

    [Serializable]
    internal class CircularByteBuffer {
        private byte[] _DataArray;
        private int _Index;
        private int _Length;
        private int _NumValid;

        internal CircularByteBuffer(int size) {
            _DataArray = new byte[size];
            _Length = size;
        }

        /// <summary>
        /// Initialize by copying the CircularByteBuffer passed in
        /// </summary>
        internal CircularByteBuffer(CircularByteBuffer cdb) {
            lock (cdb) {
                _Length = cdb._Length;
                _NumValid = cdb._NumValid;
                _Index = cdb._Index;
                _DataArray = new byte[_Length];
                for (int c = 0; c < _Length; c++) {
                    _DataArray[c] = cdb._DataArray[c];
                }
            }
        }

        /// <summary>
        /// The physical size of the Buffer (read/write)
        /// </summary>
        internal int BufferSize {
            get => _Length;
            set {
                byte[] newDataArray = new byte[value];

                int minLength = _Length > value ? value : _Length;
                for (int i = 0; i < minLength; i++) {
                    newDataArray[i] = InternalGet(i - _Length + 1);
                }
                _DataArray = newDataArray;
                _Index = minLength - 1;
                _Length = value;
            }
        }

        /// <summary>
        /// e.g. Offset[0] is the current value
        /// </summary>
        internal byte this[int index] {
            get => InternalGet(-1 - index);
            set => InternalSet(-1 - index, value);
        }

        /// <summary>
        /// How far back it is safe to look (read/write).  Write only to reduce NumValid.
        /// </summary>
        internal int NumValid {
            get => _NumValid;
            set {
                if (value > _NumValid)
                    throw new Exception("Can't set NumValid to " + value +
                                        " which is greater than the current numValid value of " + _NumValid);
                _NumValid = value;
            }
        }

        internal CircularByteBuffer Copy() => new CircularByteBuffer(this);

        internal void Reset() {
            _Index = 0;
            _NumValid = 0;
        }

        /// <summary>
        /// Push a byte into the buffer.  Returns the value of whatever comes off.
        /// </summary>
        internal byte Push(byte newValue) {
            byte ret;
            lock (this) {
                ret = InternalGet(_Length);
                _DataArray[_Index] = newValue;
                _NumValid++;
                if (_NumValid > _Length) _NumValid = _Length;
                _Index++;
                _Index %= _Length;
            }
            return ret;
        }

        /// <summary>
        /// Pop an integer off the start of the buffer. Throws an exception if the buffer is empty (NumValid == 0)
        /// </summary>
        internal byte Pop() {
            lock (this) {
                if (_NumValid == 0) throw new Exception("Can't pop off an empty CircularByteBuffer");
                _NumValid--;
                return this[_NumValid];
            }
        }

        /// <summary>
        /// Returns what would fall out of the buffer on a Push.  NOT the same as what you'd get with a Pop().
        /// </summary>
        internal byte Peek() {
            lock (this) {
                return InternalGet(_Length);
            }
        }

        private byte InternalGet(int offset) {
            int ind = _Index + offset;

            // Do thin modulo (should just drop through)
            for (; ind >= _Length; ind -= _Length) { }
            for (; ind < 0; ind += _Length) { }
            // Set value
            return _DataArray[ind];
        }

        private void InternalSet(int offset, byte valueToSet) {
            int ind = _Index + offset;

            // Do thin modulo (should just drop through)
            for (; ind > _Length; ind -= _Length) { }

            for (; ind < 0; ind += _Length) { }
            // Set value
            _DataArray[ind] = valueToSet;
        }

        /// <summary>
        /// Returns a range (in terms of Offsets) in an int array in chronological (oldest-to-newest) order. e.g. (3, 0)
        /// returns the last four ints pushed, with result[3] being the most recent.
        /// </summary>
        internal byte[] GetRange(int str, int stp) {
            byte[] outByte = new byte[str - stp + 1];

            for (int i = str, j = 0; i >= stp; i--, j++) {
                outByte[j] = this[i];
            }

            return outByte;
        }

        public override string ToString() {
            string ret = "";
            for (int i = 0; i < _DataArray.Length; i++) {
                ret += _DataArray[i] + " ";
            }
            ret += "\n index = " + _Index + " numValid = " + NumValid;
            return ret;
        }
    }

    /// <summary>
    /// 16-Bit CRC checksum
    /// </summary>
    public sealed class Crc16 {
        private static readonly short Polynomial;
        private short _CRC;

        static Crc16() {
            Polynomial = (short)SupportClass.Identity(0x8005);
        }

        internal Crc16() {
            _CRC = (short)SupportClass.Identity(0xFFFF);
        }

        /// <summary>
        /// Feed a bitstring to the crc calculation (length between 0 and 32, not inclusive).
        /// </summary>
        internal void AddBits(int bitstring, int length) {
            int bitmask = 1 << (length - 1);
            do
                if (((_CRC & 0x8000) == 0) ^ ((bitstring & bitmask) == 0)) {
                    _CRC <<= 1;
                    _CRC ^= Polynomial;
                }
                else
                    _CRC <<= 1;
            while ((bitmask = SupportClass.URShift(bitmask, 1)) != 0);
        }

        /// <summary>
        /// Return the calculated checksum.
        /// Erase it for next calls to add_bits().
        /// </summary>
        internal short Checksum() {
            short sum = _CRC;
            _CRC = (short)SupportClass.Identity(0xFFFF);
            return sum;
        }
    }

    /// <summary>
    /// A Type-safe representation of the the supported output channel
    /// constants. This class is immutable and, hence, is thread safe.
    /// </summary>
    /// <author>
    /// Mat McGowan
    /// </author>
    public class OutputChannels {
        /// <summary>
        /// Flag to indicate output should include both channels.
        /// </summary>
        internal const int BOTH_CHANNELS = 0;

        /// <summary>
        /// Flag to indicate output should include the left channel only.
        /// </summary>
        internal const int LEFT_CHANNEL = 1;

        /// <summary>
        /// Flag to indicate output should include the right channel only.
        /// </summary>
        internal const int RIGHT_CHANNEL = 2;

        /// <summary>
        /// Flag to indicate output is mono.
        /// </summary>
        internal const int DOWNMIX_CHANNELS = 3;

        internal static readonly OutputChannels Left = new OutputChannels(LEFT_CHANNEL);
        internal static readonly OutputChannels Right = new OutputChannels(RIGHT_CHANNEL);
        internal static readonly OutputChannels Both = new OutputChannels(BOTH_CHANNELS);
        internal static readonly OutputChannels DownMix = new OutputChannels(DOWNMIX_CHANNELS);
        private readonly int _OutputChannels;

        private OutputChannels(int channels) {
            _OutputChannels = channels;

            if (channels < 0 || channels > 3) {
                throw new ArgumentException("channels");
            }
        }

        /// <summary>
        /// Retrieves the code representing the desired output channels.
        /// Will be one of LEFT_CHANNEL, RIGHT_CHANNEL, BOTH_CHANNELS
        /// or DOWNMIX_CHANNELS.
        /// </summary>
        /// <returns>
        /// the channel code represented by this instance.
        /// </returns>
        internal virtual int ChannelsOutputCode => _OutputChannels;

        /// <summary>
        /// Retrieves the number of output channels represented
        /// by this channel output type.
        /// </summary>
        /// <returns>
        /// The number of output channels for this channel output
        /// type. This will be 2 for BOTH_CHANNELS only, and 1
        /// for all other types.
        /// </returns>
        internal virtual int ChannelCount {
            get {
                int count = _OutputChannels == BOTH_CHANNELS ? 2 : 1;
                return count;
            }
        }

        /// <summary>
        /// Creates an OutputChannels instance
        /// corresponding to the given channel code.
        /// </summary>
        /// <param name="code">
        /// one of the OutputChannels channel code constants.
        /// @throws IllegalArgumentException if code is not a valid
        /// channel code.
        /// </param>
        internal static OutputChannels FromInt(int code) {
            switch (code) {
                case (int)OutputChannelsEnum.LeftChannel:
                    return Left;

                case (int)OutputChannelsEnum.RightChannel:
                    return Right;

                case (int)OutputChannelsEnum.BothChannels:
                    return Both;

                case (int)OutputChannelsEnum.DownmixChannels:
                    return DownMix;

                default:
                    throw new ArgumentException("Invalid channel code: " + code);
            }
        }

        public override bool Equals(object obj) {
            bool equals = false;

            if (obj is OutputChannels oc) {
                equals = oc._OutputChannels == _OutputChannels;
            }

            return equals;
        }

        public override int GetHashCode() => _OutputChannels;
    }

    internal enum OutputChannelsEnum {
        BothChannels = 0,
        LeftChannel = 1,
        RightChannel = 2,
        DownmixChannels = 3
    }

    /// <summary>
    /// A PushbackStream is a stream that can "push back" or "unread" data. This is useful in situations where it is convenient for a
    /// fragment of code to read an indefinite number of data bytes that are delimited by a particular byte value; after reading the
    /// terminating byte, the code fragment can "unread" it, so that the next read operation on the input stream will reread the byte
    /// that was pushed back.
    /// </summary>
    public class PushbackStream {
        private readonly int _BackBufferSize;
        private readonly CircularByteBuffer _CircularByteBuffer;
        private readonly Stream _Stream;
        private readonly byte[] _TemporaryBuffer;
        private int _NumForwardBytesInBuffer;

        internal PushbackStream(Stream s, int backBufferSize) {
            _Stream = s;
            _BackBufferSize = backBufferSize;
            _TemporaryBuffer = new byte[_BackBufferSize];
            _CircularByteBuffer = new CircularByteBuffer(_BackBufferSize);
        }

        internal int Read(sbyte[] toRead, int offset, int length) {
            // Read 
            int currentByte = 0;
            bool canReadStream = true;
            while (currentByte < length && canReadStream) {
                if (_NumForwardBytesInBuffer > 0) {
                    // from mem
                    _NumForwardBytesInBuffer--;
                    toRead[offset + currentByte] = (sbyte)_CircularByteBuffer[_NumForwardBytesInBuffer];
                    currentByte++;
                }
                else {
                    // from stream
                    int newBytes = length - currentByte;
                    int numRead = _Stream.Read(_TemporaryBuffer, 0, newBytes);
                    canReadStream = numRead >= newBytes;
                    for (int i = 0; i < numRead; i++) {
                        _CircularByteBuffer.Push(_TemporaryBuffer[i]);
                        toRead[offset + currentByte + i] = (sbyte)_TemporaryBuffer[i];
                    }
                    currentByte += numRead;
                }
            }
            return currentByte;
        }

        internal void UnRead(int length) {
            _NumForwardBytesInBuffer += length;
            if (_NumForwardBytesInBuffer > _BackBufferSize) {
                throw new Exception("The backstream cannot unread the requested number of bytes.");
            }
        }

        internal void Close() {
            _Stream.Close();
        }
    }


    /// <summary>
    /// Implements a Huffman decoder.
    /// </summary>
    internal sealed class Huffman {
        private const int MXOFF = 250;
        private const int HTN = 34;
        private static readonly int[][] ValTab0 = {new[] {0, 0}};

        private static readonly int[][] ValTab1 = {
            new[] {2, 1}, new[] {0, 0}, new[] {2, 1},
            new[] {0, 16}, new[] {2, 1}, new[] {0, 1}, new[] {0, 17}
        };

        private static readonly int[][] ValTab2 = {
            new[] {2, 1}, new[] {0, 0}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 16}, new[] {0, 1}, new[] {2, 1}, new[] {0, 17}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 32}, new[] {0, 33}, new[] {2, 1}, new[] {0, 18},
            new[] {2, 1}, new[] {0, 2}, new[] {0, 34}
        };

        private static readonly int[][] ValTab3 = {
            new[] {4, 1}, new[] {2, 1}, new[] {0, 0},
            new[] {0, 1}, new[] {2, 1}, new[] {0, 17}, new[] {2, 1}, new[] {0, 16}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 32}, new[] {0, 33}, new[] {2, 1}, new[] {0, 18},
            new[] {2, 1}, new[] {0, 2}, new[] {0, 34}
        };

        private static readonly int[][] ValTab4 = {new[] {0, 0}}; // dummy

        private static readonly int[][] ValTab5 = {
            new[] {2, 1}, new[] {0, 0}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 16}, new[] {0, 1}, new[] {2, 1}, new[] {0, 17}, new[] {8, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 32}, new[] {0, 2}, new[] {2, 1}, new[] {0, 33},
            new[] {0, 18}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 34},
            new[] {0, 48}, new[] {2, 1}, new[] {0, 3}, new[] {0, 19}, new[] {2, 1},
            new[] {0, 49}, new[] {2, 1}, new[] {0, 50}, new[] {2, 1}, new[] {0, 35},
            new[] {0, 51}
        };

        private static readonly int[][] ValTab6 = {
            new[] {6, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 0}, new[] {0, 16}, new[] {0, 17}, new[] {6, 1}, new[] {2, 1}, new[] {0, 1},
            new[] {2, 1}, new[] {0, 32}, new[] {0, 33}, new[] {6, 1}, new[] {2, 1},
            new[] {0, 18}, new[] {2, 1}, new[] {0, 2}, new[] {0, 34}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 49}, new[] {0, 19}, new[] {4, 1}, new[] {2, 1}, new[] {0, 48},
            new[] {0, 50}, new[] {2, 1}, new[] {0, 35}, new[] {2, 1}, new[] {0, 3},
            new[] {0, 51}
        };

        private static readonly int[][] ValTab7 = {
            new[] {2, 1}, new[] {0, 0}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 16}, new[] {0, 1}, new[] {8, 1}, new[] {2, 1}, new[] {0, 17},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 32}, new[] {0, 2}, new[] {0, 33},
            new[] {18, 1}, new[] {6, 1}, new[] {2, 1}, new[] {0, 18}, new[] {2, 1},
            new[] {0, 34}, new[] {0, 48}, new[] {4, 1}, new[] {2, 1}, new[] {0, 49},
            new[] {0, 19}, new[] {4, 1}, new[] {2, 1}, new[] {0, 3}, new[] {0, 50}, new[] {2, 1},
            new[] {0, 35}, new[] {0, 4}, new[] {10, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 64}, new[] {0, 65}, new[] {2, 1}, new[] {0, 20}, new[] {2, 1},
            new[] {0, 66}, new[] {0, 36}, new[] {12, 1}, new[] {6, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 51}, new[] {0, 67}, new[] {0, 80}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 52}, new[] {0, 5}, new[] {0, 81}, new[] {6, 1}, new[] {2, 1},
            new[] {0, 21}, new[] {2, 1}, new[] {0, 82}, new[] {0, 37}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 68}, new[] {0, 53}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 83}, new[] {0, 84}, new[] {2, 1}, new[] {0, 69}, new[] {0, 85}
        };

        private static readonly int[][] ValTab8 = {
            new[] {6, 1}, new[] {2, 1}, new[] {0, 0},
            new[] {2, 1}, new[] {0, 16}, new[] {0, 1}, new[] {2, 1}, new[] {0, 17}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 33}, new[] {0, 18}, new[] {14, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 32}, new[] {0, 2}, new[] {2, 1}, new[] {0, 34}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 48}, new[] {0, 3}, new[] {2, 1}, new[] {0, 49},
            new[] {0, 19}, new[] {14, 1}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 50}, new[] {0, 35}, new[] {2, 1}, new[] {0, 64}, new[] {0, 4},
            new[] {2, 1}, new[] {0, 65}, new[] {2, 1}, new[] {0, 20}, new[] {0, 66},
            new[] {12, 1}, new[] {6, 1}, new[] {2, 1}, new[] {0, 36}, new[] {2, 1},
            new[] {0, 51}, new[] {0, 80}, new[] {4, 1}, new[] {2, 1}, new[] {0, 67},
            new[] {0, 52}, new[] {0, 81}, new[] {6, 1}, new[] {2, 1}, new[] {0, 21},
            new[] {2, 1}, new[] {0, 5}, new[] {0, 82}, new[] {6, 1}, new[] {2, 1}, new[] {0, 37},
            new[] {2, 1}, new[] {0, 68}, new[] {0, 53}, new[] {2, 1}, new[] {0, 83},
            new[] {2, 1}, new[] {0, 69}, new[] {2, 1}, new[] {0, 84}, new[] {0, 85}
        };

        private static readonly int[][] ValTab9 = {
            new[] {8, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 0}, new[] {0, 16}, new[] {2, 1}, new[] {0, 1}, new[] {0, 17},
            new[] {10, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 32}, new[] {0, 33},
            new[] {2, 1}, new[] {0, 18}, new[] {2, 1}, new[] {0, 2}, new[] {0, 34},
            new[] {12, 1}, new[] {6, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 48}, new[] {0, 3},
            new[] {0, 49}, new[] {2, 1}, new[] {0, 19}, new[] {2, 1}, new[] {0, 50},
            new[] {0, 35}, new[] {12, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 65},
            new[] {0, 20}, new[] {4, 1}, new[] {2, 1}, new[] {0, 64}, new[] {0, 51},
            new[] {2, 1}, new[] {0, 66}, new[] {0, 36}, new[] {10, 1}, new[] {6, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 4}, new[] {0, 80}, new[] {0, 67}, new[] {2, 1},
            new[] {0, 52}, new[] {0, 81}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 21}, new[] {0, 82}, new[] {2, 1}, new[] {0, 37}, new[] {0, 68},
            new[] {6, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 5}, new[] {0, 84}, new[] {0, 83},
            new[] {2, 1}, new[] {0, 53}, new[] {2, 1}, new[] {0, 69}, new[] {0, 85}
        };

        private static readonly int[][] ValTab10 = {
            new[] {2, 1}, new[] {0, 0}, new[] {4, 1}, new[] {2, 1}, new[] {0, 16}, new[] {0, 1},
            new[] {10, 1}, new[] {2, 1}, new[] {0, 17}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 32}, new[] {0, 2}, new[] {2, 1}, new[] {0, 33}, new[] {0, 18},
            new[] {28, 1}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 34},
            new[] {0, 48}, new[] {2, 1}, new[] {0, 49}, new[] {0, 19}, new[] {8, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 3}, new[] {0, 50}, new[] {2, 1}, new[] {0, 35},
            new[] {0, 64}, new[] {4, 1}, new[] {2, 1}, new[] {0, 65}, new[] {0, 20},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 4}, new[] {0, 51}, new[] {2, 1}, new[] {0, 66},
            new[] {0, 36}, new[] {28, 1}, new[] {10, 1}, new[] {6, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 80}, new[] {0, 5}, new[] {0, 96}, new[] {2, 1},
            new[] {0, 97}, new[] {0, 22}, new[] {12, 1}, new[] {6, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 67}, new[] {0, 52}, new[] {0, 81}, new[] {2, 1},
            new[] {0, 21}, new[] {2, 1}, new[] {0, 82}, new[] {0, 37}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 38}, new[] {0, 54}, new[] {0, 113}, new[] {20, 1},
            new[] {8, 1}, new[] {2, 1}, new[] {0, 23}, new[] {4, 1}, new[] {2, 1}, new[] {0, 68},
            new[] {0, 83}, new[] {0, 6}, new[] {6, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 53},
            new[] {0, 69}, new[] {0, 98}, new[] {2, 1}, new[] {0, 112}, new[] {2, 1},
            new[] {0, 7}, new[] {0, 100}, new[] {14, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 114}, new[] {0, 39}, new[] {6, 1}, new[] {2, 1}, new[] {0, 99},
            new[] {2, 1}, new[] {0, 84}, new[] {0, 85}, new[] {2, 1}, new[] {0, 70},
            new[] {0, 115}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 55},
            new[] {0, 101}, new[] {2, 1}, new[] {0, 86}, new[] {0, 116},
            new[] {6, 1}, new[] {2, 1}, new[] {0, 71}, new[] {2, 1}, new[] {0, 102},
            new[] {0, 117}, new[] {4, 1}, new[] {2, 1}, new[] {0, 87}, new[] {0, 118},
            new[] {2, 1}, new[] {0, 103}, new[] {0, 119}
        };

        private static readonly int[][] ValTab11 = {
            new[] {6, 1}, new[] {2, 1}, new[] {0, 0}, new[] {2, 1}, new[] {0, 16}, new[] {0, 1},
            new[] {8, 1}, new[] {2, 1}, new[] {0, 17}, new[] {4, 1}, new[] {2, 1}, new[] {0, 32},
            new[] {0, 2}, new[] {0, 18}, new[] {24, 1}, new[] {8, 1}, new[] {2, 1},
            new[] {0, 33}, new[] {2, 1}, new[] {0, 34}, new[] {2, 1}, new[] {0, 48},
            new[] {0, 3}, new[] {4, 1}, new[] {2, 1}, new[] {0, 49}, new[] {0, 19}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 50}, new[] {0, 35}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 64}, new[] {0, 4}, new[] {2, 1}, new[] {0, 65}, new[] {0, 20},
            new[] {30, 1}, new[] {16, 1}, new[] {10, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 66}, new[] {0, 36}, new[] {4, 1}, new[] {2, 1}, new[] {0, 51},
            new[] {0, 67}, new[] {0, 80}, new[] {4, 1}, new[] {2, 1}, new[] {0, 52},
            new[] {0, 81}, new[] {0, 97}, new[] {6, 1}, new[] {2, 1}, new[] {0, 22},
            new[] {2, 1}, new[] {0, 6}, new[] {0, 38}, new[] {2, 1}, new[] {0, 98}, new[] {2, 1},
            new[] {0, 21}, new[] {2, 1}, new[] {0, 5}, new[] {0, 82}, new[] {16, 1},
            new[] {10, 1}, new[] {6, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 37},
            new[] {0, 68}, new[] {0, 96}, new[] {2, 1}, new[] {0, 99}, new[] {0, 54},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 112}, new[] {0, 23}, new[] {0, 113},
            new[] {16, 1}, new[] {6, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 7},
            new[] {0, 100}, new[] {0, 114}, new[] {2, 1}, new[] {0, 39}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 83}, new[] {0, 53}, new[] {2, 1}, new[] {0, 84},
            new[] {0, 69}, new[] {10, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 70},
            new[] {0, 115}, new[] {2, 1}, new[] {0, 55}, new[] {2, 1}, new[] {0, 101},
            new[] {0, 86}, new[] {10, 1}, new[] {6, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 85}, new[] {0, 87}, new[] {0, 116}, new[] {2, 1}, new[] {0, 71},
            new[] {0, 102}, new[] {4, 1}, new[] {2, 1}, new[] {0, 117}, new[] {0, 118},
            new[] {2, 1}, new[] {0, 103}, new[] {0, 119}
        };

        private static readonly int[][] ValTab12 = {
            new[] {12, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 16}, new[] {0, 1}, new[] {2, 1},
            new[] {0, 17}, new[] {2, 1}, new[] {0, 0}, new[] {2, 1}, new[] {0, 32}, new[] {0, 2},
            new[] {16, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 33}, new[] {0, 18},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 34}, new[] {0, 49}, new[] {2, 1},
            new[] {0, 19}, new[] {2, 1}, new[] {0, 48}, new[] {2, 1}, new[] {0, 3},
            new[] {0, 64}, new[] {26, 1}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 50}, new[] {0, 35}, new[] {2, 1}, new[] {0, 65}, new[] {0, 51},
            new[] {10, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 20}, new[] {0, 66},
            new[] {2, 1}, new[] {0, 36}, new[] {2, 1}, new[] {0, 4}, new[] {0, 80}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 67}, new[] {0, 52}, new[] {2, 1}, new[] {0, 81},
            new[] {0, 21}, new[] {28, 1}, new[] {14, 1}, new[] {8, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 82}, new[] {0, 37}, new[] {2, 1}, new[] {0, 83},
            new[] {0, 53}, new[] {4, 1}, new[] {2, 1}, new[] {0, 96}, new[] {0, 22},
            new[] {0, 97}, new[] {4, 1}, new[] {2, 1}, new[] {0, 98}, new[] {0, 38},
            new[] {6, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 5}, new[] {0, 6}, new[] {0, 68},
            new[] {2, 1}, new[] {0, 84}, new[] {0, 69}, new[] {18, 1}, new[] {10, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 99}, new[] {0, 54}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 112}, new[] {0, 7}, new[] {0, 113}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 23}, new[] {0, 100}, new[] {2, 1}, new[] {0, 70}, new[] {0, 114},
            new[] {10, 1}, new[] {6, 1}, new[] {2, 1}, new[] {0, 39}, new[] {2, 1},
            new[] {0, 85}, new[] {0, 115}, new[] {2, 1}, new[] {0, 55}, new[] {0, 86},
            new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 101},
            new[] {0, 116}, new[] {2, 1}, new[] {0, 71}, new[] {0, 102}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 117}, new[] {0, 87}, new[] {2, 1}, new[] {0, 118},
            new[] {2, 1}, new[] {0, 103}, new[] {0, 119}
        };

        private static readonly int[][] ValTab13 = {
            new[] {2, 1}, new[] {0, 0}, new[] {6, 1}, new[] {2, 1}, new[] {0, 16}, new[] {2, 1},
            new[] {0, 1}, new[] {0, 17}, new[] {28, 1}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 32}, new[] {0, 2}, new[] {2, 1}, new[] {0, 33}, new[] {0, 18},
            new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 34}, new[] {0, 48}, new[] {2, 1},
            new[] {0, 3}, new[] {0, 49}, new[] {6, 1}, new[] {2, 1}, new[] {0, 19}, new[] {2, 1},
            new[] {0, 50}, new[] {0, 35}, new[] {4, 1}, new[] {2, 1}, new[] {0, 64},
            new[] {0, 4}, new[] {0, 65}, new[] {70, 1}, new[] {28, 1}, new[] {14, 1},
            new[] {6, 1}, new[] {2, 1}, new[] {0, 20}, new[] {2, 1}, new[] {0, 51},
            new[] {0, 66}, new[] {4, 1}, new[] {2, 1}, new[] {0, 36}, new[] {0, 80},
            new[] {2, 1}, new[] {0, 67}, new[] {0, 52}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 81}, new[] {0, 21}, new[] {4, 1}, new[] {2, 1}, new[] {0, 5},
            new[] {0, 82}, new[] {2, 1}, new[] {0, 37}, new[] {2, 1}, new[] {0, 68},
            new[] {0, 83}, new[] {14, 1}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 96}, new[] {0, 6}, new[] {2, 1}, new[] {0, 97}, new[] {0, 22},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 128}, new[] {0, 8}, new[] {0, 129},
            new[] {16, 1}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 53},
            new[] {0, 98}, new[] {2, 1}, new[] {0, 38}, new[] {0, 84}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 69}, new[] {0, 99}, new[] {2, 1}, new[] {0, 54},
            new[] {0, 112}, new[] {6, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 7},
            new[] {0, 85}, new[] {0, 113}, new[] {2, 1}, new[] {0, 23}, new[] {2, 1},
            new[] {0, 39}, new[] {0, 55}, new[] {72, 1}, new[] {24, 1}, new[] {12, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 24}, new[] {0, 130}, new[] {2, 1},
            new[] {0, 40}, new[] {4, 1}, new[] {2, 1}, new[] {0, 100}, new[] {0, 70},
            new[] {0, 114}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 132},
            new[] {0, 72}, new[] {2, 1}, new[] {0, 144}, new[] {0, 9}, new[] {2, 1},
            new[] {0, 145}, new[] {0, 25}, new[] {24, 1}, new[] {14, 1}, new[] {8, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 115}, new[] {0, 101}, new[] {2, 1},
            new[] {0, 86}, new[] {0, 116}, new[] {4, 1}, new[] {2, 1}, new[] {0, 71},
            new[] {0, 102}, new[] {0, 131}, new[] {6, 1}, new[] {2, 1}, new[] {0, 56},
            new[] {2, 1}, new[] {0, 117}, new[] {0, 87}, new[] {2, 1}, new[] {0, 146},
            new[] {0, 41}, new[] {14, 1}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 103}, new[] {0, 133}, new[] {2, 1}, new[] {0, 88}, new[] {0, 57},
            new[] {2, 1}, new[] {0, 147}, new[] {2, 1}, new[] {0, 73}, new[] {0, 134},
            new[] {6, 1}, new[] {2, 1}, new[] {0, 160}, new[] {2, 1}, new[] {0, 104},
            new[] {0, 10}, new[] {2, 1}, new[] {0, 161}, new[] {0, 26}, new[] {68, 1},
            new[] {24, 1}, new[] {12, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 162},
            new[] {0, 42}, new[] {4, 1}, new[] {2, 1}, new[] {0, 149}, new[] {0, 89},
            new[] {2, 1}, new[] {0, 163}, new[] {0, 58}, new[] {8, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 74}, new[] {0, 150}, new[] {2, 1}, new[] {0, 176},
            new[] {0, 11}, new[] {2, 1}, new[] {0, 177}, new[] {0, 27}, new[] {20, 1},
            new[] {8, 1}, new[] {2, 1}, new[] {0, 178}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 118}, new[] {0, 119}, new[] {0, 148}, new[] {6, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 135}, new[] {0, 120}, new[] {0, 164}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 105}, new[] {0, 165}, new[] {0, 43}, new[] {12, 1},
            new[] {6, 1}, new[] {4, 1}, new[] {2, 1}, new[] {
                0,
                90
            },
            new[] {0, 136}, new[] {0, 179}, new[] {2, 1}, new[] {0, 59}, new[] {2, 1},
            new[] {0, 121}, new[] {0, 166}, new[] {6, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 106}, new[] {0, 180}, new[] {0, 192}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 12}, new[] {0, 152}, new[] {0, 193}, new[] {60, 1}, new[] {22, 1},
            new[] {10, 1}, new[] {6, 1}, new[] {2, 1}, new[] {0, 28}, new[] {2, 1},
            new[] {0, 137}, new[] {0, 181}, new[] {2, 1}, new[] {0, 91}, new[] {0, 194},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 44}, new[] {0, 60}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 182}, new[] {0, 107}, new[] {2, 1}, new[] {0, 196}, new[] {0, 76},
            new[] {16, 1}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 168},
            new[] {0, 138}, new[] {2, 1}, new[] {0, 208}, new[] {0, 13}, new[] {2, 1},
            new[] {0, 209}, new[] {2, 1}, new[] {0, 75}, new[] {2, 1}, new[] {0, 151},
            new[] {0, 167}, new[] {12, 1}, new[] {6, 1}, new[] {2, 1}, new[] {0, 195},
            new[] {2, 1}, new[] {0, 122}, new[] {0, 153}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 197}, new[] {0, 92}, new[] {0, 183}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 29}, new[] {0, 210}, new[] {2, 1}, new[] {0, 45}, new[] {2, 1},
            new[] {0, 123}, new[] {0, 211}, new[] {52, 1}, new[] {28, 1}, new[] {12, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 61}, new[] {0, 198}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 108}, new[] {0, 169}, new[] {2, 1}, new[] {0, 154},
            new[] {0, 212}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 184},
            new[] {0, 139}, new[] {2, 1}, new[] {0, 77}, new[] {0, 199}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 124}, new[] {0, 213}, new[] {2, 1}, new[] {0, 93},
            new[] {0, 224}, new[] {10, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 225},
            new[] {0, 30}, new[] {4, 1}, new[] {2, 1}, new[] {0, 14}, new[] {0, 46}, new[] {0, 226}, new[] {8, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 227}, new[] {0, 109}, new[] {2, 1},
            new[] {0, 140}, new[] {0, 228}, new[] {4, 1}, new[] {2, 1}, new[] {0, 229},
            new[] {0, 186}, new[] {0, 240}, new[] {38, 1}, new[] {16, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 241}, new[] {0, 31}, new[] {6, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 170}, new[] {0, 155}, new[] {0, 185}, new[] {2, 1},
            new[] {0, 62}, new[] {2, 1}, new[] {0, 214}, new[] {0, 200}, new[] {12, 1},
            new[] {6, 1}, new[] {2, 1}, new[] {0, 78}, new[] {2, 1}, new[] {0, 215},
            new[] {0, 125}, new[] {2, 1}, new[] {0, 171}, new[] {2, 1}, new[] {0, 94},
            new[] {0, 201}, new[] {6, 1}, new[] {2, 1}, new[] {0, 15}, new[] {2, 1},
            new[] {0, 156}, new[] {0, 110}, new[] {2, 1}, new[] {0, 242}, new[] {0, 47},
            new[] {32, 1}, new[] {16, 1}, new[] {6, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 216}, new[] {0, 141}, new[] {0, 63}, new[] {6, 1}, new[] {2, 1},
            new[] {0, 243}, new[] {2, 1}, new[] {0, 230}, new[] {0, 202}, new[] {2, 1},
            new[] {0, 244}, new[] {0, 79}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 187}, new[] {0, 172}, new[] {2, 1}, new[] {0, 231}, new[] {0, 245},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 217}, new[] {0, 157}, new[] {2, 1},
            new[] {0, 95}, new[] {0, 232}, new[] {30, 1}, new[] {12, 1}, new[] {6, 1},
            new[] {2, 1}, new[] {0, 111}, new[] {2, 1}, new[] {0, 246}, new[] {0, 203},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 188}, new[] {0, 173}, new[] {0, 218},
            new[] {8, 1}, new[] {2, 1}, new[] {0, 247}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 126}, new[] {0, 127}, new[] {0, 142}, new[] {6, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 158}, new[] {0, 174}, new[] {0, 204}, new[] {2, 1}, new[] {0, 248}, new[] {0, 143}, new[] {18, 1},
            new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 219}, new[] {0, 189},
            new[] {2, 1}, new[] {0, 234}, new[] {0, 249}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 159}, new[] {0, 235}, new[] {2, 1}, new[] {0, 190}, new[] {2, 1},
            new[] {0, 205}, new[] {0, 250}, new[] {14, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 221}, new[] {0, 236}, new[] {6, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 233}, new[] {0, 175}, new[] {0, 220}, new[] {2, 1}, new[] {0, 206},
            new[] {0, 251}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 191},
            new[] {0, 222}, new[] {2, 1}, new[] {0, 207}, new[] {0, 238}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 223}, new[] {0, 239}, new[] {2, 1}, new[] {0, 255},
            new[] {2, 1}, new[] {0, 237}, new[] {2, 1}, new[] {0, 253}, new[] {2, 1},
            new[] {0, 252}, new[] {0, 254}
        };

        private static readonly int[][] ValTab14 = {new[] {0, 0}};

        private static readonly int[][] ValTab15 = {
            new[] {16, 1}, new[] {6, 1}, new[] {2, 1}, new[] {0, 0}, new[] {2, 1}, new[] {0, 16},
            new[] {0, 1}, new[] {2, 1}, new[] {0, 17}, new[] {4, 1}, new[] {2, 1}, new[] {0, 32},
            new[] {0, 2}, new[] {2, 1}, new[] {0, 33}, new[] {0, 18}, new[] {50, 1},
            new[] {16, 1}, new[] {6, 1}, new[] {2, 1}, new[] {0, 34}, new[] {2, 1},
            new[] {0, 48}, new[] {0, 49}, new[] {6, 1}, new[] {2, 1}, new[] {0, 19},
            new[] {2, 1}, new[] {0, 3}, new[] {0, 64}, new[] {2, 1}, new[] {0, 50},
            new[] {0, 35}, new[] {14, 1}, new[] {6, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 4},
            new[] {0, 20}, new[] {0, 65}, new[] {4, 1}, new[] {2, 1}, new[] {0, 51},
            new[] {0, 66}, new[] {2, 1}, new[] {0, 36}, new[] {0, 67}, new[] {10, 1},
            new[] {6, 1}, new[] {2, 1}, new[] {0, 52}, new[] {2, 1}, new[] {0, 80}, new[] {0, 5},
            new[] {2, 1}, new[] {0, 81}, new[] {0, 21}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 82}, new[] {0, 37}, new[] {4, 1}, new[] {2, 1}, new[] {0, 68},
            new[] {0, 83}, new[] {0, 97}, new[] {90, 1}, new[] {36, 1}, new[] {18, 1},
            new[] {10, 1}, new[] {6, 1}, new[] {2, 1}, new[] {0, 53}, new[] {2, 1},
            new[] {0, 96}, new[] {0, 6}, new[] {2, 1}, new[] {0, 22}, new[] {0, 98},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 38}, new[] {0, 84}, new[] {2, 1},
            new[] {0, 69}, new[] {0, 99}, new[] {10, 1}, new[] {6, 1}, new[] {2, 1},
            new[] {0, 54}, new[] {2, 1}, new[] {0, 112}, new[] {0, 7}, new[] {2, 1},
            new[] {0, 113}, new[] {0, 85}, new[] {4, 1}, new[] {2, 1}, new[] {0, 23},
            new[] {0, 100}, new[] {2, 1}, new[] {0, 114}, new[] {0, 39}, new[] {24, 1},
            new[] {16, 1}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 70},
            new[] {0, 115}, new[] {2, 1}, new[] {0, 55}, new[] {0, 101}, new[] {4, 1}, new[] {2, 1}, new[] {0, 86}, new[] {0, 128}, new[] {2, 1}, new[] {0, 8},
            new[] {0, 116}, new[] {4, 1}, new[] {2, 1}, new[] {0, 129}, new[] {0, 24},
            new[] {2, 1}, new[] {0, 130}, new[] {0, 40}, new[] {16, 1}, new[] {8, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 71}, new[] {0, 102}, new[] {2, 1},
            new[] {0, 131}, new[] {0, 56}, new[] {4, 1}, new[] {2, 1}, new[] {0, 117},
            new[] {0, 87}, new[] {2, 1}, new[] {0, 132}, new[] {0, 72}, new[] {6, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 144}, new[] {0, 25}, new[] {0, 145},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 146}, new[] {0, 118}, new[] {2, 1},
            new[] {0, 103}, new[] {0, 41}, new[] {92, 1}, new[] {36, 1}, new[] {18, 1},
            new[] {10, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 133}, new[] {0, 88},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 9}, new[] {0, 119}, new[] {0, 147},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 57}, new[] {0, 148}, new[] {2, 1},
            new[] {0, 73}, new[] {0, 134}, new[] {10, 1}, new[] {6, 1}, new[] {2, 1},
            new[] {0, 104}, new[] {2, 1}, new[] {0, 160}, new[] {0, 10}, new[] {2, 1},
            new[] {0, 161}, new[] {0, 26}, new[] {4, 1}, new[] {2, 1}, new[] {0, 162},
            new[] {0, 42}, new[] {2, 1}, new[] {0, 149}, new[] {0, 89}, new[] {26, 1},
            new[] {14, 1}, new[] {6, 1}, new[] {2, 1}, new[] {0, 163}, new[] {2, 1},
            new[] {0, 58}, new[] {0, 135}, new[] {4, 1}, new[] {2, 1}, new[] {0, 120},
            new[] {0, 164}, new[] {2, 1}, new[] {0, 74}, new[] {0, 150}, new[] {6, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 105}, new[] {0, 176}, new[] {0, 177},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 27}, new[] {0, 165}, new[] {0, 178},
            new[] {14, 1}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 90},
            new[] {0, 43}, new[] {2, 1}, new[] {0, 136}, new[] {
                0, 151
            },
            new[] {2, 1}, new[] {0, 179}, new[] {2, 1}, new[] {0, 121}, new[] {0, 59},
            new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 106}, new[] {0, 180},
            new[] {2, 1}, new[] {0, 75}, new[] {0, 193}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 152}, new[] {0, 137}, new[] {2, 1}, new[] {0, 28}, new[] {0, 181},
            new[] {80, 1}, new[] {34, 1}, new[] {16, 1}, new[] {6, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 91}, new[] {0, 44}, new[] {0, 194}, new[] {6, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 11}, new[] {0, 192}, new[] {0, 166},
            new[] {2, 1}, new[] {0, 167}, new[] {0, 122}, new[] {10, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 195}, new[] {0, 60}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 12}, new[] {0, 153}, new[] {0, 182}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 107}, new[] {0, 196}, new[] {2, 1}, new[] {0, 76}, new[] {0, 168},
            new[] {20, 1}, new[] {10, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 138},
            new[] {0, 197}, new[] {4, 1}, new[] {2, 1}, new[] {0, 208}, new[] {0, 92},
            new[] {0, 209}, new[] {4, 1}, new[] {2, 1}, new[] {0, 183}, new[] {0, 123},
            new[] {2, 1}, new[] {0, 29}, new[] {2, 1}, new[] {0, 13}, new[] {0, 45},
            new[] {12, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 210}, new[] {0, 211},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 61}, new[] {0, 198}, new[] {2, 1},
            new[] {0, 108}, new[] {0, 169}, new[] {6, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 154}, new[] {0, 184}, new[] {0, 212}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 139}, new[] {0, 77}, new[] {2, 1}, new[] {0, 199}, new[] {0, 124},
            new[] {68, 1}, new[] {34, 1}, new[] {18, 1}, new[] {10, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 213}, new[] {0, 93}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 224}, new[] {0, 14}, new[] {
                0,
                225
            },
            new[] {4, 1}, new[] {2, 1}, new[] {0, 30}, new[] {0, 226}, new[] {2, 1},
            new[] {0, 170}, new[] {0, 46}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 185}, new[] {0, 155}, new[] {2, 1}, new[] {0, 227}, new[] {0, 214},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 109}, new[] {0, 62}, new[] {2, 1},
            new[] {0, 200}, new[] {0, 140}, new[] {16, 1}, new[] {8, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 228}, new[] {0, 78}, new[] {2, 1}, new[] {0, 215},
            new[] {0, 125}, new[] {4, 1}, new[] {2, 1}, new[] {0, 229}, new[] {0, 186},
            new[] {2, 1}, new[] {0, 171}, new[] {0, 94}, new[] {8, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 201}, new[] {0, 156}, new[] {2, 1}, new[] {0, 241},
            new[] {0, 31}, new[] {6, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 240},
            new[] {0, 110}, new[] {0, 242}, new[] {2, 1}, new[] {0, 47}, new[] {0, 230},
            new[] {38, 1}, new[] {18, 1}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 216}, new[] {0, 243}, new[] {2, 1}, new[] {0, 63}, new[] {0, 244},
            new[] {6, 1}, new[] {2, 1}, new[] {0, 79}, new[] {2, 1}, new[] {0, 141},
            new[] {0, 217}, new[] {2, 1}, new[] {0, 187}, new[] {0, 202}, new[] {8, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 172}, new[] {0, 231}, new[] {2, 1},
            new[] {0, 126}, new[] {0, 245}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 157}, new[] {0, 95}, new[] {2, 1}, new[] {0, 232}, new[] {0, 142},
            new[] {2, 1}, new[] {0, 246}, new[] {0, 203}, new[] {34, 1}, new[] {18, 1},
            new[] {10, 1}, new[] {6, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 15},
            new[] {0, 174}, new[] {0, 111}, new[] {2, 1}, new[] {0, 188}, new[] {0, 218},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 173}, new[] {0, 247}, new[] {2, 1},
            new[] {0, 127}, new[] {0, 233}, new[] {
                8, 1
            },
            new[] {4, 1}, new[] {2, 1}, new[] {0, 158}, new[] {0, 204}, new[] {2, 1},
            new[] {0, 248}, new[] {0, 143}, new[] {4, 1}, new[] {2, 1}, new[] {0, 219},
            new[] {0, 189}, new[] {2, 1}, new[] {0, 234}, new[] {0, 249}, new[] {16, 1},
            new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 159}, new[] {0, 220},
            new[] {2, 1}, new[] {0, 205}, new[] {0, 235}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 190}, new[] {0, 250}, new[] {2, 1}, new[] {0, 175}, new[] {0, 221},
            new[] {14, 1}, new[] {6, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 236},
            new[] {0, 206}, new[] {0, 251}, new[] {4, 1}, new[] {2, 1}, new[] {0, 191},
            new[] {0, 237}, new[] {2, 1}, new[] {0, 222}, new[] {0, 252}, new[] {6, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 207}, new[] {0, 253}, new[] {0, 238},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 223}, new[] {0, 254}, new[] {2, 1},
            new[] {0, 239}, new[] {0, 255}
        };

        private static readonly int[][] ValTab16 = {
            new[] {2, 1}, new[] {0, 0}, new[] {6, 1}, new[] {2, 1}, new[] {0, 16}, new[] {2, 1},
            new[] {0, 1}, new[] {0, 17}, new[] {42, 1}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 32}, new[] {0, 2}, new[] {2, 1}, new[] {0, 33}, new[] {0, 18},
            new[] {10, 1}, new[] {6, 1}, new[] {2, 1}, new[] {0, 34}, new[] {2, 1},
            new[] {0, 48}, new[] {0, 3}, new[] {2, 1}, new[] {0, 49}, new[] {0, 19},
            new[] {10, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 50}, new[] {0, 35},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 64}, new[] {0, 4}, new[] {0, 65}, new[] {6, 1},
            new[] {2, 1}, new[] {0, 20}, new[] {2, 1}, new[] {0, 51}, new[] {0, 66},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 36}, new[] {0, 80}, new[] {2, 1},
            new[] {0, 67}, new[] {0, 52}, new[] {138, 1}, new[] {40, 1}, new[] {16, 1},
            new[] {6, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 5}, new[] {0, 21}, new[] {0, 81},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 82}, new[] {0, 37}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 68}, new[] {0, 53}, new[] {0, 83}, new[] {10, 1}, new[] {6, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 96}, new[] {0, 6}, new[] {0, 97}, new[] {2, 1},
            new[] {0, 22}, new[] {0, 98}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 38}, new[] {0, 84}, new[] {2, 1}, new[] {0, 69}, new[] {0, 99},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 54}, new[] {0, 112}, new[] {0, 113},
            new[] {40, 1}, new[] {18, 1}, new[] {8, 1}, new[] {2, 1}, new[] {0, 23},
            new[] {2, 1}, new[] {0, 7}, new[] {2, 1}, new[] {0, 85}, new[] {0, 100},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 114}, new[] {0, 39}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 70}, new[] {0, 101}, new[] {0, 115}, new[] {10, 1},
            new[] {6, 1}, new[] {2, 1}, new[] {0, 55}, new[] {2, 1}, new[] {0, 86}, new[] {0, 8}, new[] {2, 1}, new[] {0, 128},
            new[] {0, 129}, new[] {6, 1}, new[] {2, 1}, new[] {0, 24}, new[] {2, 1},
            new[] {0, 116}, new[] {0, 71}, new[] {2, 1}, new[] {0, 130}, new[] {2, 1},
            new[] {0, 40}, new[] {0, 102}, new[] {24, 1}, new[] {14, 1}, new[] {8, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 131}, new[] {0, 56}, new[] {2, 1},
            new[] {0, 117}, new[] {0, 132}, new[] {4, 1}, new[] {2, 1}, new[] {0, 72},
            new[] {0, 144}, new[] {0, 145}, new[] {6, 1}, new[] {2, 1}, new[] {0, 25},
            new[] {2, 1}, new[] {0, 9}, new[] {0, 118}, new[] {2, 1}, new[] {0, 146},
            new[] {0, 41}, new[] {14, 1}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 133}, new[] {0, 88}, new[] {2, 1}, new[] {0, 147}, new[] {0, 57},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 160}, new[] {0, 10}, new[] {0, 26},
            new[] {8, 1}, new[] {2, 1}, new[] {0, 162}, new[] {2, 1}, new[] {0, 103},
            new[] {2, 1}, new[] {0, 87}, new[] {0, 73}, new[] {6, 1}, new[] {2, 1},
            new[] {0, 148}, new[] {2, 1}, new[] {0, 119}, new[] {0, 134}, new[] {2, 1},
            new[] {0, 161}, new[] {2, 1}, new[] {0, 104}, new[] {0, 149}, new[] {220, 1},
            new[] {126, 1}, new[] {50, 1}, new[] {26, 1}, new[] {12, 1}, new[] {6, 1},
            new[] {2, 1}, new[] {0, 42}, new[] {2, 1}, new[] {0, 89}, new[] {0, 58},
            new[] {2, 1}, new[] {0, 163}, new[] {2, 1}, new[] {0, 135}, new[] {0, 120},
            new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 164}, new[] {0, 74},
            new[] {2, 1}, new[] {0, 150}, new[] {0, 105}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 176}, new[] {0, 11}, new[] {0, 177}, new[] {10, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 27}, new[] {0, 178}, new[] {2, 1}, new[] {0, 43},
            new[] {2, 1}, new[] {0, 165}, new[] {0, 90}, new[]
                {6, 1},
            new[] {2, 1}, new[] {0, 179}, new[] {2, 1}, new[] {0, 166}, new[] {0, 106},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 180}, new[] {0, 75}, new[] {2, 1},
            new[] {0, 12}, new[] {0, 193}, new[] {30, 1}, new[] {14, 1}, new[] {6, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 181}, new[] {0, 194}, new[] {0, 44},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 167}, new[] {0, 195}, new[] {2, 1},
            new[] {0, 107}, new[] {0, 196}, new[] {8, 1}, new[] {2, 1}, new[] {0, 29},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 136}, new[] {0, 151}, new[] {0, 59},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 209}, new[] {0, 210}, new[] {2, 1},
            new[] {0, 45}, new[] {0, 211}, new[] {18, 1}, new[] {6, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 30}, new[] {0, 46}, new[] {0, 226}, new[] {6, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 121}, new[] {0, 152}, new[] {0, 192},
            new[] {2, 1}, new[] {0, 28}, new[] {2, 1}, new[] {0, 137}, new[] {0, 91},
            new[] {14, 1}, new[] {6, 1}, new[] {2, 1}, new[] {0, 60}, new[] {2, 1},
            new[] {0, 122}, new[] {0, 182}, new[] {4, 1}, new[] {2, 1}, new[] {0, 76},
            new[] {0, 153}, new[] {2, 1}, new[] {0, 168}, new[] {0, 138}, new[] {6, 1},
            new[] {2, 1}, new[] {0, 13}, new[] {2, 1}, new[] {0, 197}, new[] {0, 92},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 61}, new[] {0, 198}, new[] {2, 1},
            new[] {0, 108}, new[] {0, 154}, new[] {88, 1}, new[] {86, 1}, new[] {36, 1},
            new[] {16, 1}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 139},
            new[] {0, 77}, new[] {2, 1}, new[] {0, 199}, new[] {0, 124}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 213}, new[] {0, 93}, new[] {2, 1}, new[] {0, 224},
            new[] {0, 14}, new[] {8, 1}, new[] {2, 1}, new[] {0, 227}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 208}, new[] {0, 183},
            new[] {0, 123}, new[] {6, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 169},
            new[] {0, 184}, new[] {0, 212}, new[] {2, 1}, new[] {0, 225}, new[] {2, 1},
            new[] {0, 170}, new[] {0, 185}, new[] {24, 1}, new[] {10, 1}, new[] {6, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 155}, new[] {0, 214}, new[] {0, 109},
            new[] {2, 1}, new[] {0, 62}, new[] {0, 200}, new[] {6, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 140}, new[] {0, 228}, new[] {0, 78}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 215}, new[] {0, 229}, new[] {2, 1}, new[] {0, 186},
            new[] {0, 171}, new[] {12, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 156},
            new[] {0, 230}, new[] {4, 1}, new[] {2, 1}, new[] {0, 110}, new[] {0, 216},
            new[] {2, 1}, new[] {0, 141}, new[] {0, 187}, new[] {8, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 231}, new[] {0, 157}, new[] {2, 1}, new[] {0, 232},
            new[] {0, 142}, new[] {4, 1}, new[] {2, 1}, new[] {0, 203}, new[] {0, 188},
            new[] {0, 158}, new[] {0, 241}, new[] {2, 1}, new[] {0, 31}, new[] {2, 1},
            new[] {0, 15}, new[] {0, 47}, new[] {66, 1}, new[] {56, 1}, new[] {2, 1},
            new[] {0, 242}, new[] {52, 1}, new[] {50, 1}, new[] {20, 1}, new[] {8, 1},
            new[] {2, 1}, new[] {0, 189}, new[] {2, 1}, new[] {0, 94}, new[] {2, 1},
            new[] {0, 125}, new[] {0, 201}, new[] {6, 1}, new[] {2, 1}, new[] {0, 202},
            new[] {2, 1}, new[] {0, 172}, new[] {0, 126}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 218}, new[] {0, 173}, new[] {0, 204}, new[] {10, 1}, new[] {6, 1},
            new[] {2, 1}, new[] {0, 174}, new[] {2, 1}, new[] {0, 219}, new[] {0, 220},
            new[] {2, 1}, new[] {0, 205}, new[] {0, 190}, new[] {6, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 235}, new[] {0, 237}, new[] {0, 238}, new[] {6, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {
                0,
                217
            },
            new[] {0, 234}, new[] {0, 233}, new[] {2, 1}, new[] {0, 222}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 221}, new[] {0, 236}, new[] {0, 206}, new[] {0, 63},
            new[] {0, 240}, new[] {4, 1}, new[] {2, 1}, new[] {0, 243}, new[] {0, 244},
            new[] {2, 1}, new[] {0, 79}, new[] {2, 1}, new[] {0, 245}, new[] {0, 95},
            new[] {10, 1}, new[] {2, 1}, new[] {0, 255}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 246}, new[] {0, 111}, new[] {2, 1}, new[] {0, 247}, new[] {0, 127},
            new[] {12, 1}, new[] {6, 1}, new[] {2, 1}, new[] {0, 143}, new[] {2, 1},
            new[] {0, 248}, new[] {0, 249}, new[] {4, 1}, new[] {2, 1}, new[] {0, 159},
            new[] {0, 250}, new[] {0, 175}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 251}, new[] {0, 191}, new[] {2, 1}, new[] {0, 252}, new[] {0, 207},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 253}, new[] {0, 223}, new[] {2, 1},
            new[] {0, 254}, new[] {0, 239}
        };

        private static readonly int[][] ValTab24 = {
            new[] {60, 1}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 0}, new[] {0, 16},
            new[] {2, 1}, new[] {0, 1}, new[] {0, 17}, new[] {14, 1}, new[] {6, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 32}, new[] {0, 2}, new[] {0, 33}, new[] {2, 1},
            new[] {0, 18}, new[] {2, 1}, new[] {0, 34}, new[] {2, 1}, new[] {0, 48},
            new[] {0, 3}, new[] {14, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 49},
            new[] {0, 19}, new[] {4, 1}, new[] {2, 1}, new[] {0, 50}, new[] {0, 35},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 64}, new[] {0, 4}, new[] {0, 65}, new[] {8, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 20}, new[] {0, 51}, new[] {2, 1},
            new[] {0, 66}, new[] {0, 36}, new[] {6, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 67}, new[] {0, 52}, new[] {0, 81}, new[] {6, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 80}, new[] {0, 5}, new[] {0, 21}, new[] {2, 1},
            new[] {0, 82}, new[] {0, 37}, new[] {250, 1}, new[] {98, 1}, new[] {34, 1},
            new[] {18, 1}, new[] {10, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 68},
            new[] {0, 83}, new[] {2, 1}, new[] {0, 53}, new[] {2, 1}, new[] {0, 96},
            new[] {0, 6}, new[] {4, 1}, new[] {2, 1}, new[] {0, 97}, new[] {0, 22}, new[] {2, 1},
            new[] {0, 98}, new[] {0, 38}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 84}, new[] {0, 69}, new[] {2, 1}, new[] {0, 99}, new[] {0, 54},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 113}, new[] {0, 85}, new[] {2, 1},
            new[] {0, 100}, new[] {0, 70}, new[] {32, 1}, new[] {14, 1}, new[] {6, 1},
            new[] {2, 1}, new[] {0, 114}, new[] {2, 1}, new[] {0, 39}, new[] {0, 55},
            new[] {2, 1}, new[] {0, 115}, new[] {4, 1}, new[] {2, 1}, new[] {0, 112},
            new[] {0, 7}, new[] {0, 23}, new[] {10, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 101}, new[] {0, 86}, new[] {4, 1}, new[] {2, 1}, new[] {0, 128},
            new[] {0, 8}, new[] {0, 129}, new[] {4, 1}, new[] {2, 1}, new[] {0, 116},
            new[] {0, 71}, new[] {2, 1}, new[] {0, 24}, new[] {0, 130}, new[] {16, 1},
            new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 40}, new[] {0, 102},
            new[] {2, 1}, new[] {0, 131}, new[] {0, 56}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 117}, new[] {0, 87}, new[] {2, 1}, new[] {0, 132}, new[] {0, 72},
            new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 145}, new[] {0, 25},
            new[] {2, 1}, new[] {0, 146}, new[] {0, 118}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 103}, new[] {0, 41}, new[] {2, 1}, new[] {0, 133}, new[] {0, 88},
            new[] {92, 1}, new[] {34, 1}, new[] {16, 1}, new[] {8, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 147}, new[] {0, 57}, new[] {2, 1}, new[] {0, 148},
            new[] {0, 73}, new[] {4, 1}, new[] {2, 1}, new[] {0, 119}, new[] {0, 134},
            new[] {2, 1}, new[] {0, 104}, new[] {0, 161}, new[] {8, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 162}, new[] {0, 42}, new[] {2, 1}, new[] {0, 149},
            new[] {0, 89}, new[] {4, 1}, new[] {2, 1}, new[] {0, 163}, new[] {0, 58},
            new[] {2, 1}, new[] {0, 135}, new[] {2, 1}, new[] {0, 120}, new[] {0, 74},
            new[] {22, 1}, new[] {12, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 164},
            new[] {0, 150}, new[] {4, 1}, new[] {2, 1}, new[] {0, 105}, new[] {0, 177},
            new[] {2, 1}, new[] {0, 27}, new[] {0, 165}, new[] {6, 1}, new[] {2, 1},
            new[] {0, 178}, new[] {2, 1}, new[] {0, 90}, new[] {0, 43}, new[] {2, 1},
            new[] {0, 136}, new[] {0, 179}, new[] {16, 1}, new[] {10, 1}, new[] {6, 1},
            new[] {2, 1}, new[] {0, 144}, new[] {2, 1}, new[] {0, 9}, new[] {0, 160},
            new[] {2, 1}, new[] {0, 151}, new[] {0, 121}, new[]
                {4, 1},
            new[] {2, 1}, new[] {0, 166}, new[] {0, 106}, new[] {0, 180}, new[] {12, 1},
            new[] {6, 1}, new[] {2, 1}, new[] {0, 26}, new[] {2, 1}, new[] {0, 10},
            new[] {0, 176}, new[] {2, 1}, new[] {0, 59}, new[] {2, 1}, new[] {0, 11},
            new[] {0, 192}, new[] {4, 1}, new[] {2, 1}, new[] {0, 75}, new[] {0, 193},
            new[] {2, 1}, new[] {0, 152}, new[] {0, 137}, new[] {67, 1}, new[] {34, 1},
            new[] {16, 1}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 28},
            new[] {0, 181}, new[] {2, 1}, new[] {0, 91}, new[] {0, 194}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 44}, new[] {0, 167}, new[] {2, 1}, new[] {0, 122},
            new[] {0, 195}, new[] {10, 1}, new[] {6, 1}, new[] {2, 1}, new[] {0, 60},
            new[] {2, 1}, new[] {0, 12}, new[] {0, 208}, new[] {2, 1}, new[] {0, 182},
            new[] {0, 107}, new[] {4, 1}, new[] {2, 1}, new[] {0, 196}, new[] {0, 76},
            new[] {2, 1}, new[] {0, 153}, new[] {0, 168}, new[] {16, 1}, new[] {8, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 138}, new[] {0, 197}, new[] {2, 1},
            new[] {0, 92}, new[] {0, 209}, new[] {4, 1}, new[] {2, 1}, new[] {0, 183},
            new[] {0, 123}, new[] {2, 1}, new[] {0, 29}, new[] {0, 210}, new[] {9, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 45}, new[] {0, 211}, new[] {2, 1},
            new[] {0, 61}, new[] {0, 198}, new[] {85, 250}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 108}, new[] {0, 169}, new[] {2, 1}, new[] {0, 154}, new[] {0, 212},
            new[] {32, 1}, new[] {16, 1}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 184}, new[] {0, 139}, new[] {2, 1}, new[] {0, 77}, new[] {0, 199},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 124}, new[] {0, 213}, new[] {2, 1},
            new[] {0, 93}, new[] {0, 225}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 30}, new[] {0, 226}, new[] {
                2, 1
            },
            new[] {0, 170}, new[] {0, 185}, new[] {4, 1}, new[] {2, 1}, new[] {0, 155},
            new[] {0, 227}, new[] {2, 1}, new[] {0, 214}, new[] {0, 109}, new[] {20, 1},
            new[] {10, 1}, new[] {6, 1}, new[] {2, 1}, new[] {0, 62}, new[] {2, 1},
            new[] {0, 46}, new[] {0, 78}, new[] {2, 1}, new[] {0, 200}, new[] {0, 140},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 228}, new[] {0, 215}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 125}, new[] {0, 171}, new[] {0, 229}, new[] {10, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 186}, new[] {0, 94}, new[] {2, 1},
            new[] {0, 201}, new[] {2, 1}, new[] {0, 156}, new[] {0, 110}, new[] {8, 1},
            new[] {2, 1}, new[] {0, 230}, new[] {2, 1}, new[] {0, 13}, new[] {2, 1},
            new[] {0, 224}, new[] {0, 14}, new[] {4, 1}, new[] {2, 1}, new[] {0, 216},
            new[] {0, 141}, new[] {2, 1}, new[] {0, 187}, new[] {0, 202}, new[] {74, 1},
            new[] {2, 1}, new[] {0, 255}, new[] {64, 1}, new[] {58, 1}, new[] {32, 1},
            new[] {16, 1}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 172},
            new[] {0, 231}, new[] {2, 1}, new[] {0, 126}, new[] {0, 217}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 157}, new[] {0, 232}, new[] {2, 1}, new[] {0, 142},
            new[] {0, 203}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 188},
            new[] {0, 218}, new[] {2, 1}, new[] {0, 173}, new[] {0, 233}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 158}, new[] {0, 204}, new[] {2, 1}, new[] {0, 219},
            new[] {0, 189}, new[] {16, 1}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 234}, new[] {0, 174}, new[] {2, 1}, new[] {0, 220}, new[] {0, 205},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 235}, new[] {0, 190}, new[] {2, 1},
            new[] {0, 221}, new[] {0, 236}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 206}, new[] {0, 237}, new[]
                {2, 1},
            new[] {0, 222}, new[] {0, 238}, new[] {0, 15}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 240}, new[] {0, 31}, new[] {0, 241}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 242}, new[] {0, 47}, new[] {2, 1}, new[] {0, 243}, new[] {0, 63},
            new[] {18, 1}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 244},
            new[] {0, 79}, new[] {2, 1}, new[] {0, 245}, new[] {0, 95}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 246}, new[] {0, 111}, new[] {2, 1}, new[] {0, 247},
            new[] {2, 1}, new[] {0, 127}, new[] {0, 143}, new[] {10, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 248}, new[] {0, 249}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 159}, new[] {0, 175}, new[] {0, 250}, new[] {8, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 251}, new[] {0, 191}, new[] {2, 1}, new[] {0, 252},
            new[] {0, 207}, new[] {4, 1}, new[] {2, 1}, new[] {0, 253}, new[] {0, 223},
            new[] {2, 1}, new[] {0, 254}, new[] {0, 239}
        };

        private static readonly int[][] ValTab32 = {
            new[] {2, 1}, new[] {0, 0}, new[] {8, 1},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 8}, new[] {0, 4}, new[] {2, 1}, new[] {0, 1},
            new[] {0, 2}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 12}, new[] {0, 10},
            new[] {2, 1}, new[] {0, 3}, new[] {0, 6}, new[] {6, 1}, new[] {2, 1}, new[] {0, 9},
            new[] {2, 1}, new[] {0, 5}, new[] {0, 7}, new[] {4, 1}, new[] {2, 1}, new[] {0, 14},
            new[] {0, 13}, new[] {2, 1}, new[] {0, 15}, new[] {0, 11}
        };

        private static readonly int[][] ValTab33 = {
            new[] {16, 1}, new[] {8, 1}, new[] {4, 1},
            new[] {2, 1}, new[] {0, 0}, new[] {0, 1}, new[] {2, 1}, new[] {0, 2}, new[] {0, 3},
            new[] {4, 1}, new[] {2, 1}, new[] {0, 4}, new[] {0, 5}, new[] {2, 1}, new[] {0, 6},
            new[] {0, 7}, new[] {8, 1}, new[] {4, 1}, new[] {2, 1}, new[] {0, 8}, new[] {0, 9},
            new[] {2, 1}, new[] {0, 10}, new[] {0, 11}, new[] {4, 1}, new[] {2, 1},
            new[] {0, 12}, new[] {0, 13}, new[] {2, 1}, new[] {0, 14}, new[] {0, 15}
        };

        internal static Huffman[] HuffmanTable; //Simulate extern struct
        private readonly int _Linbits; //number of linbits
        private readonly char _Tablename0; //string, containing table_description
        private readonly char _Tablename1; //string, containing table_description
        private readonly int _Treelen; //length of decoder tree
        private readonly int[][] _Val; //decoder tree
        private readonly int _Xlen; //max. x-index+
        private readonly int _Ylen; //max. y-index+
        private readonly int[] _Hlen; //pointer to array[xlen][ylen]
        private readonly int _Linmax; //max number to be stored in linbits
        private readonly int _RefRenamed; //a positive value indicates a reference
        private readonly int[] _Table; //pointer to array[xlen][ylen]
        private readonly char _Tablename2; //string, containing table_description

        static Huffman() { }

        /// <summary>
        /// Computes all Huffman Tables.
        /// </summary>
        private Huffman(string s, int xlen, int ylen, int linbits, int linmax, int @ref, int[] table, int[] hlen,
            int[][] val, int treelen) {
            _Tablename0 = s[0];
            _Tablename1 = s[1];
            _Tablename2 = s[2];
            _Xlen = xlen;
            _Ylen = ylen;
            _Linbits = linbits;
            _Linmax = linmax;
            _RefRenamed = @ref;
            _Table = table;
            _Hlen = hlen;
            _Val = val;
            _Treelen = treelen;
        }

        /// <summary>
        /// Do the huffman-decoding.
        /// NOTE: for counta, countb -the 4 bit value is returned in y, discard x.
        /// </summary>
        internal static int Decode(Huffman h, int[] x, int[] y, int[] v, int[] w, BitReserve br) {
            // array of all huffcodtable headers
            // 0..31 Huffman code table 0..31
            // 32,33 count1-tables

            int dmask = 1 << (4 * 8 - 1);
            int point = 0;
            int error = 1;
            int level = dmask;

            if (h._Val == null)
                return 2;

            /* table 0 needs no bits */
            if (h._Treelen == 0) {
                x[0] = y[0] = 0;
                return 0;
            }

            /* Lookup in Huffman table. */

            /*int bitsAvailable = 0;     
            int bitIndex = 0;
            
            int bits[] = bitbuf;*/
            do {
                if (h._Val[point][0] == 0) {
                    /*end of tree*/
                    x[0] = SupportClass.URShift(h._Val[point][1], 4);
                    y[0] = h._Val[point][1] & 0xf;
                    error = 0;
                    break;
                }

                // hget1bit() is called thousands of times, and so needs to be
                // ultra fast. 
                /*
                if (bitIndex==bitsAvailable)
                {
                bitsAvailable = br.readBits(bits, 32);            
                bitIndex = 0;
                }
                */
                //if (bits[bitIndex++]!=0)
                if (br.ReadOneBit() != 0) {
                    while (h._Val[point][1] >= MXOFF)
                        point += h._Val[point][1];
                    point += h._Val[point][1];
                }
                else {
                    while (h._Val[point][0] >= MXOFF)
                        point += h._Val[point][0];
                    point += h._Val[point][0];
                }
                level = SupportClass.URShift(level, 1);
                // MDM: ht[0] is always 0;
            } while (level != 0 || point < 0);

            // put back any bits not consumed
            /*    
            int unread = (bitsAvailable-bitIndex);
            if (unread>0)
            br.rewindNbits(unread);
            */
            /* Process sign encodings for quadruples tables. */
            // System.out.println(h.tablename);
            if (h._Tablename0 == '3' && (h._Tablename1 == '2' || h._Tablename1 == '3')) {
                v[0] = (y[0] >> 3) & 1;
                w[0] = (y[0] >> 2) & 1;
                x[0] = (y[0] >> 1) & 1;
                y[0] = y[0] & 1;

                /* v, w, x and y are reversed in the bitstream.
                switch them around to make test bistream work. */

                if (v[0] != 0)
                    if (br.ReadOneBit() != 0)
                        v[0] = -v[0];
                if (w[0] != 0)
                    if (br.ReadOneBit() != 0)
                        w[0] = -w[0];
                if (x[0] != 0)
                    if (br.ReadOneBit() != 0)
                        x[0] = -x[0];
                if (y[0] != 0)
                    if (br.ReadOneBit() != 0)
                        y[0] = -y[0];
            }
            else {
                // Process sign and escape encodings for dual tables.
                // x and y are reversed in the test bitstream.
                // Reverse x and y here to make test bitstream work.

                if (h._Linbits != 0)
                    if (h._Xlen - 1 == x[0])
                        x[0] += br.ReadBits(h._Linbits);
                if (x[0] != 0)
                    if (br.ReadOneBit() != 0)
                        x[0] = -x[0];
                if (h._Linbits != 0)
                    if (h._Ylen - 1 == y[0])
                        y[0] += br.ReadBits(h._Linbits);
                if (y[0] != 0)
                    if (br.ReadOneBit() != 0)
                        y[0] = -y[0];
            }
            return error;
        }

        internal static void Initialize() {
            if (HuffmanTable != null)
                return;

            HuffmanTable = new Huffman[HTN];
            HuffmanTable[0] = new Huffman("0  ", 0, 0, 0, 0, -1, null, null, ValTab0, 0);
            HuffmanTable[1] = new Huffman("1  ", 2, 2, 0, 0, -1, null, null, ValTab1, 7);
            HuffmanTable[2] = new Huffman("2  ", 3, 3, 0, 0, -1, null, null, ValTab2, 17);
            HuffmanTable[3] = new Huffman("3  ", 3, 3, 0, 0, -1, null, null, ValTab3, 17);
            HuffmanTable[4] = new Huffman("4  ", 0, 0, 0, 0, -1, null, null, ValTab4, 0);
            HuffmanTable[5] = new Huffman("5  ", 4, 4, 0, 0, -1, null, null, ValTab5, 31);
            HuffmanTable[6] = new Huffman("6  ", 4, 4, 0, 0, -1, null, null, ValTab6, 31);
            HuffmanTable[7] = new Huffman("7  ", 6, 6, 0, 0, -1, null, null, ValTab7, 71);
            HuffmanTable[8] = new Huffman("8  ", 6, 6, 0, 0, -1, null, null, ValTab8, 71);
            HuffmanTable[9] = new Huffman("9  ", 6, 6, 0, 0, -1, null, null, ValTab9, 71);
            HuffmanTable[10] = new Huffman("10 ", 8, 8, 0, 0, -1, null, null, ValTab10, 127);
            HuffmanTable[11] = new Huffman("11 ", 8, 8, 0, 0, -1, null, null, ValTab11, 127);
            HuffmanTable[12] = new Huffman("12 ", 8, 8, 0, 0, -1, null, null, ValTab12, 127);
            HuffmanTable[13] = new Huffman("13 ", 16, 16, 0, 0, -1, null, null, ValTab13, 511);
            HuffmanTable[14] = new Huffman("14 ", 0, 0, 0, 0, -1, null, null, ValTab14, 0);
            HuffmanTable[15] = new Huffman("15 ", 16, 16, 0, 0, -1, null, null, ValTab15, 511);
            HuffmanTable[16] = new Huffman("16 ", 16, 16, 1, 1, -1, null, null, ValTab16, 511);
            HuffmanTable[17] = new Huffman("17 ", 16, 16, 2, 3, 16, null, null, ValTab16, 511);
            HuffmanTable[18] = new Huffman("18 ", 16, 16, 3, 7, 16, null, null, ValTab16, 511);
            HuffmanTable[19] = new Huffman("19 ", 16, 16, 4, 15, 16, null, null, ValTab16, 511);
            HuffmanTable[20] = new Huffman("20 ", 16, 16, 6, 63, 16, null, null, ValTab16, 511);
            HuffmanTable[21] = new Huffman("21 ", 16, 16, 8, 255, 16, null, null, ValTab16, 511);
            HuffmanTable[22] = new Huffman("22 ", 16, 16, 10, 1023, 16, null, null, ValTab16, 511);
            HuffmanTable[23] = new Huffman("23 ", 16, 16, 13, 8191, 16, null, null, ValTab16, 511);
            HuffmanTable[24] = new Huffman("24 ", 16, 16, 4, 15, -1, null, null, ValTab24, 512);
            HuffmanTable[25] = new Huffman("25 ", 16, 16, 5, 31, 24, null, null, ValTab24, 512);
            HuffmanTable[26] = new Huffman("26 ", 16, 16, 6, 63, 24, null, null, ValTab24, 512);
            HuffmanTable[27] = new Huffman("27 ", 16, 16, 7, 127, 24, null, null, ValTab24, 512);
            HuffmanTable[28] = new Huffman("28 ", 16, 16, 8, 255, 24, null, null, ValTab24, 512);
            HuffmanTable[29] = new Huffman("29 ", 16, 16, 9, 511, 24, null, null, ValTab24, 512);
            HuffmanTable[30] = new Huffman("30 ", 16, 16, 11, 2047, 24, null, null, ValTab24, 512);
            HuffmanTable[31] = new Huffman("31 ", 16, 16, 13, 8191, 24, null, null, ValTab24, 512);
            HuffmanTable[32] = new Huffman("32 ", 1, 16, 0, 0, -1, null, null, ValTab32, 31);
            HuffmanTable[33] = new Huffman("33 ", 1, 16, 0, 0, -1, null, null, ValTab33, 31);
        }
    }

}

namespace RitaEngine.Resources.Sound.MP3Sharp.Decoding.Decoders {

using RitaEngine.Resources.Sound.MP3Sharp.Decoding.Decoders.LayerI;
using RitaEngine.Resources.Sound.MP3Sharp.Decoding.Decoders.LayerII;
using System;
using RitaEngine.Resources.Sound.MP3Sharp.Decoding.Decoders.LayerIII;
using RitaEngine.Resources.Sound.MP3Sharp.Support;

    /// <summary>
    /// Abstract base class for subband classes of layer I and II
    /// </summary>
    public abstract class ASubband {
        /*
        *  Changes from version 1.1 to 1.2:
        *    - array size increased by one, although a scalefactor with index 63
        *      is illegal (to prevent segmentation faults)
        */
        // Scalefactors for layer I and II, Annex 3-B.1 in ISO/IEC DIS 11172:
        internal static readonly float[] ScaleFactors = {
            2.00000000000000f, 1.58740105196820f, 1.25992104989487f, 1.00000000000000f, 0.79370052598410f,
            0.62996052494744f, 0.50000000000000f, 0.39685026299205f, 0.31498026247372f, 0.25000000000000f,
            0.19842513149602f, 0.15749013123686f, 0.12500000000000f, 0.09921256574801f, 0.07874506561843f,
            0.06250000000000f, 0.04960628287401f, 0.03937253280921f, 0.03125000000000f, 0.02480314143700f,
            0.01968626640461f, 0.01562500000000f, 0.01240157071850f, 0.00984313320230f, 0.00781250000000f,
            0.00620078535925f, 0.00492156660115f, 0.00390625000000f, 0.00310039267963f, 0.00246078330058f,
            0.00195312500000f, 0.00155019633981f, 0.00123039165029f, 0.00097656250000f, 0.00077509816991f,
            0.00061519582514f, 0.00048828125000f, 0.00038754908495f, 0.00030759791257f, 0.00024414062500f,
            0.00019377454248f, 0.00015379895629f, 0.00012207031250f, 0.00009688727124f, 0.00007689947814f,
            0.00006103515625f, 0.00004844363562f, 0.00003844973907f, 0.00003051757813f, 0.00002422181781f,
            0.00001922486954f, 0.00001525878906f, 0.00001211090890f, 0.00000961243477f, 0.00000762939453f,
            0.00000605545445f, 0.00000480621738f, 0.00000381469727f, 0.00000302772723f, 0.00000240310869f,
            0.00000190734863f, 0.00000151386361f, 0.00000120155435f, 0.00000000000000f
        };

        internal abstract void ReadAllocation(Bitstream stream, Header header, Crc16 crc);

        internal abstract void ReadScaleFactor(Bitstream stream, Header header);

        internal abstract bool ReadSampleData(Bitstream stream);

        internal abstract bool PutNextSample(int channels, SynthesisFilter filter1, SynthesisFilter filter2);
    }

    /// <summary>
    /// Implementations of FrameDecoder are responsible for decoding
    /// an MPEG audio frame.
    /// </summary>
    //REVIEW: the interface currently is too thin. There should be
    // methods to specify the output buffer, the synthesis filters and
    // possibly other objects used by the decoder. 
    public interface IFrameDecoder {
        /// <summary>
        /// Decodes one frame of MPEG audio.
        /// </summary>
        void DecodeFrame();
    }

    /// <summary>
    /// Implements decoding of MPEG Audio Layer I frames.
    /// </summary>
    public class LayerIDecoder : IFrameDecoder {
        protected ABuffer Buffer;
        protected readonly Crc16 CRC;
        protected SynthesisFilter Filter1, Filter2;
        protected Header Header;
        protected int Mode;
        protected int NuSubbands;
        protected Bitstream Stream;
        protected ASubband[] Subbands;

        protected int WhichChannels;
        // new Crc16[1] to enable CRC checking.

        internal LayerIDecoder() {
            CRC = new Crc16();
        }

        public virtual void DecodeFrame() {
            NuSubbands = Header.NumberSubbands();
            Subbands = new ASubband[32];
            Mode = Header.Mode();

            CreateSubbands();

            ReadAllocation();
            ReadScaleFactorSelection();

            if (CRC != null || Header.IsChecksumOK()) {
                ReadScaleFactors();

                ReadSampleData();
            }
        }

        internal virtual void Create(Bitstream stream0, Header header0, SynthesisFilter filtera, SynthesisFilter filterb,
            ABuffer buffer0, int whichCh0) {
            Stream = stream0;
            Header = header0;
            Filter1 = filtera;
            Filter2 = filterb;
            Buffer = buffer0;
            WhichChannels = whichCh0;
        }

        protected virtual void CreateSubbands() {
            int i;
            if (Mode == Header.SINGLE_CHANNEL)
                for (i = 0; i < NuSubbands; ++i)
                    Subbands[i] = new SubbandLayer1(i);
            else if (Mode == Header.JOINT_STEREO) {
                for (i = 0; i < Header.IntensityStereoBound(); ++i)
                    Subbands[i] = new SubbandLayer1Stereo(i);
                for (; i < NuSubbands; ++i)
                    Subbands[i] = new SubbandLayer1IntensityStereo(i);
            }
            else {
                for (i = 0; i < NuSubbands; ++i)
                    Subbands[i] = new SubbandLayer1Stereo(i);
            }
        }

        protected virtual void ReadAllocation() {
            // start to read audio data:
            for (int i = 0; i < NuSubbands; ++i)
                Subbands[i].ReadAllocation(Stream, Header, CRC);
        }

        protected virtual void ReadScaleFactorSelection() {
            // scale factor selection not present for layer I. 
        }

        protected virtual void ReadScaleFactors() {
            for (int i = 0; i < NuSubbands; ++i)
                Subbands[i].ReadScaleFactor(Stream, Header);
        }

        protected virtual void ReadSampleData() {
            bool readReady = false;
            bool writeReady = false;
            int hdrMode = Header.Mode();
            do {
                int i;
                for (i = 0; i < NuSubbands; ++i)
                    readReady = Subbands[i].ReadSampleData(Stream);
                do {
                    for (i = 0; i < NuSubbands; ++i)
                        writeReady = Subbands[i].PutNextSample(WhichChannels, Filter1, Filter2);

                    Filter1.calculate_pc_samples(Buffer);
                    if (WhichChannels == OutputChannels.BOTH_CHANNELS && hdrMode != Header.SINGLE_CHANNEL)
                        Filter2.calculate_pc_samples(Buffer);
                } while (!writeReady);
            } while (!readReady);
        }
    }

    /// <summary>
    /// Implements decoding of MPEG Audio Layer II frames.
    /// </summary>
    public class LayerIIDecoder : LayerIDecoder {
        protected override void CreateSubbands() {
            int i;
            switch (Mode) {
                case Header.SINGLE_CHANNEL: {
                    for (i = 0; i < NuSubbands; ++i)
                        Subbands[i] = new SubbandLayer2(i);
                    break;
                }
                case Header.JOINT_STEREO: {
                    for (i = 0; i < Header.IntensityStereoBound(); ++i)
                        Subbands[i] = new SubbandLayer2Stereo(i);
                    for (; i < NuSubbands; ++i)
                        Subbands[i] = new SubbandLayer2IntensityStereo(i);
                    break;
                }
                default: {
                    for (i = 0; i < NuSubbands; ++i)
                        Subbands[i] = new SubbandLayer2Stereo(i);
                    break;
                }
            }
        }

        protected override void ReadScaleFactorSelection() {
            for (int i = 0; i < NuSubbands; ++i)
                ((SubbandLayer2)Subbands[i]).ReadScaleFactorSelection(Stream, CRC);
        }
    }

    /// <summary>
    /// Implements decoding of MPEG Audio Layer 3 frames.
    /// </summary>
    internal sealed class LayerIIIDecoder : IFrameDecoder {
        private const int SSLIMIT = 18;
        private const int SBLIMIT = 32;

        private static readonly int[][] Slen = {
            new[] {0, 0, 0, 0, 3, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4},
            new[] {0, 1, 2, 3, 0, 1, 2, 3, 1, 2, 3, 1, 2, 3, 2, 3}
        };

        internal static readonly int[] Pretab = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 3, 3, 3, 2, 0};

        internal static readonly float[] TwoToNegativeHalfPow = {
            1.0000000000e+00f, 7.0710678119e-01f, 5.0000000000e-01f, 3.5355339059e-01f, 2.5000000000e-01f,
            1.7677669530e-01f, 1.2500000000e-01f, 8.8388347648e-02f, 6.2500000000e-02f, 4.4194173824e-02f,
            3.1250000000e-02f, 2.2097086912e-02f, 1.5625000000e-02f, 1.1048543456e-02f, 7.8125000000e-03f,
            5.5242717280e-03f, 3.9062500000e-03f, 2.7621358640e-03f, 1.9531250000e-03f, 1.3810679320e-03f,
            9.7656250000e-04f, 6.9053396600e-04f, 4.8828125000e-04f, 3.4526698300e-04f, 2.4414062500e-04f,
            1.7263349150e-04f, 1.2207031250e-04f, 8.6316745750e-05f, 6.1035156250e-05f, 4.3158372875e-05f,
            3.0517578125e-05f, 2.1579186438e-05f, 1.5258789062e-05f, 1.0789593219e-05f, 7.6293945312e-06f,
            5.3947966094e-06f, 3.8146972656e-06f, 2.6973983047e-06f, 1.9073486328e-06f, 1.3486991523e-06f,
            9.5367431641e-07f, 6.7434957617e-07f, 4.7683715820e-07f, 3.3717478809e-07f, 2.3841857910e-07f,
            1.6858739404e-07f, 1.1920928955e-07f, 8.4293697022e-08f, 5.9604644775e-08f, 4.2146848511e-08f,
            2.9802322388e-08f, 2.1073424255e-08f, 1.4901161194e-08f, 1.0536712128e-08f, 7.4505805969e-09f,
            5.2683560639e-09f, 3.7252902985e-09f, 2.6341780319e-09f, 1.8626451492e-09f, 1.3170890160e-09f,
            9.3132257462e-10f, 6.5854450798e-10f, 4.6566128731e-10f, 3.2927225399e-10f
        };

        internal static readonly float[] PowerTable;

        internal static readonly float[][] Io = {
            new[] {
                1.0000000000e+00f, 8.4089641526e-01f, 7.0710678119e-01f, 5.9460355751e-01f, 5.0000000001e-01f,
                4.2044820763e-01f, 3.5355339060e-01f, 2.9730177876e-01f, 2.5000000001e-01f, 2.1022410382e-01f,
                1.7677669530e-01f, 1.4865088938e-01f, 1.2500000000e-01f, 1.0511205191e-01f, 8.8388347652e-02f,
                7.4325444691e-02f, 6.2500000003e-02f, 5.2556025956e-02f, 4.4194173826e-02f, 3.7162722346e-02f,
                3.1250000002e-02f, 2.6278012978e-02f, 2.2097086913e-02f, 1.8581361173e-02f, 1.5625000001e-02f,
                1.3139006489e-02f, 1.1048543457e-02f, 9.2906805866e-03f, 7.8125000006e-03f, 6.5695032447e-03f,
                5.5242717285e-03f, 4.6453402934e-03f
            },
            new[] {
                1.0000000000e+00f, 7.0710678119e-01f, 5.0000000000e-01f, 3.5355339060e-01f, 2.5000000000e-01f,
                1.7677669530e-01f, 1.2500000000e-01f, 8.8388347650e-02f, 6.2500000001e-02f, 4.4194173825e-02f,
                3.1250000001e-02f, 2.2097086913e-02f, 1.5625000000e-02f, 1.1048543456e-02f, 7.8125000002e-03f,
                5.5242717282e-03f, 3.9062500001e-03f, 2.7621358641e-03f, 1.9531250001e-03f, 1.3810679321e-03f,
                9.7656250004e-04f, 6.9053396603e-04f, 4.8828125002e-04f, 3.4526698302e-04f, 2.4414062501e-04f,
                1.7263349151e-04f, 1.2207031251e-04f, 8.6316745755e-05f, 6.1035156254e-05f, 4.3158372878e-05f,
                3.0517578127e-05f, 2.1579186439e-05f
            }
        };

        internal static readonly float[] Tan12 = {
            0.0f, 0.26794919f, 0.57735027f, 1.0f, 1.73205081f, 3.73205081f, 9.9999999e10f, -3.73205081f, -1.73205081f,
            -1.0f, -0.57735027f, -0.26794919f, 0.0f, 0.26794919f, 0.57735027f, 1.0f
        };

        private static int[][] _reorderTable; // Generated on demand

        private static readonly float[] Cs = {
            0.857492925712f, 0.881741997318f, 0.949628649103f, 0.983314592492f, 0.995517816065f, 0.999160558175f,
            0.999899195243f, 0.999993155067f
        };

        private static readonly float[] Ca = {
            -0.5144957554270f, -0.4717319685650f, -0.3133774542040f, -0.1819131996110f, -0.0945741925262f,
            -0.0409655828852f, -0.0141985685725f, -0.00369997467375f
        };

        internal static readonly float[][] Win = {
            new[] {
                -1.6141214951e-02f, -5.3603178919e-02f, -1.0070713296e-01f, -1.6280817573e-01f, -4.9999999679e-01f,
                -3.8388735032e-01f, -6.2061144372e-01f, -1.1659756083e+00f, -3.8720752656e+00f, -4.2256286556e+00f,
                -1.5195289984e+00f, -9.7416483388e-01f, -7.3744074053e-01f, -1.2071067773e+00f, -5.1636156596e-01f,
                -4.5426052317e-01f, -4.0715656898e-01f, -3.6969460527e-01f, -3.3876269197e-01f, -3.1242222492e-01f,
                -2.8939587111e-01f, -2.6880081906e-01f, -5.0000000266e-01f, -2.3251417468e-01f, -2.1596714708e-01f,
                -2.0004979098e-01f, -1.8449493497e-01f, -1.6905846094e-01f, -1.5350360518e-01f, -1.3758624925e-01f,
                -1.2103922149e-01f, -2.0710679058e-01f, -8.4752577594e-02f, -6.4157525656e-02f, -4.1131172614e-02f,
                -1.4790705759e-02f
            },
            new[] {
                -1.6141214951e-02f, -5.3603178919e-02f, -1.0070713296e-01f, -1.6280817573e-01f, -4.9999999679e-01f,
                -3.8388735032e-01f, -6.2061144372e-01f, -1.1659756083e+00f, -3.8720752656e+00f, -4.2256286556e+00f,
                -1.5195289984e+00f, -9.7416483388e-01f, -7.3744074053e-01f, -1.2071067773e+00f, -5.1636156596e-01f,
                -4.5426052317e-01f, -4.0715656898e-01f, -3.6969460527e-01f, -3.3908542600e-01f, -3.1511810350e-01f,
                -2.9642226150e-01f, -2.8184548650e-01f, -5.4119610000e-01f, -2.6213228100e-01f, -2.5387916537e-01f,
                -2.3296291359e-01f, -1.9852728987e-01f, -1.5233534808e-01f, -9.6496400054e-02f, -3.3423828516e-02f,
                0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f,
                0.0000000000e+00f
            },
            new[] {
                -4.8300800645e-02f, -1.5715656932e-01f, -2.8325045177e-01f, -4.2953747763e-01f, -1.2071067795e+00f,
                -8.2426483178e-01f, -1.1451749106e+00f, -1.7695290101e+00f, -4.5470225061e+00f, -3.4890531002e+00f,
                -7.3296292804e-01f, -1.5076514758e-01f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f,
                0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f,
                0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f,
                0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f,
                0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f,
                0.0000000000e+00f
            },
            new[] {
                0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f, 0.0000000000e+00f,
                0.0000000000e+00f, -1.5076513660e-01f, -7.3296291107e-01f, -3.4890530566e+00f, -4.5470224727e+00f,
                -1.7695290031e+00f, -1.1451749092e+00f, -8.3137738100e-01f, -1.3065629650e+00f, -5.4142014250e-01f,
                -4.6528974900e-01f, -4.1066990750e-01f, -3.7004680800e-01f, -3.3876269197e-01f, -3.1242222492e-01f,
                -2.8939587111e-01f, -2.6880081906e-01f, -5.0000000266e-01f, -2.3251417468e-01f, -2.1596714708e-01f,
                -2.0004979098e-01f,
                -1.8449493497e-01f, -1.6905846094e-01f, -1.5350360518e-01f, -1.3758624925e-01f, -1.2103922149e-01f,
                -2.0710679058e-01f, -8.4752577594e-02f, -6.4157525656e-02f, -4.1131172614e-02f, -1.4790705759e-02f
            }
        };

        internal static readonly int[][][] NrOfSfbBlock = {
            new[] {new[] {6, 5, 5, 5}, new[] {9, 9, 9, 9}, new[] {6, 9, 9, 9}},
            new[] {new[] {6, 5, 7, 3}, new[] {9, 9, 12, 6}, new[] {6, 9, 12, 6}},
            new[] {new[] {11, 10, 0, 0}, new[] {18, 18, 0, 0}, new[] {15, 18, 0, 0}},
            new[] {new[] {7, 7, 7, 0}, new[] {12, 12, 12, 0}, new[] {6, 15, 12, 0}},
            new[] {new[] {6, 6, 6, 3}, new[] {12, 9, 9, 6}, new[] {6, 12, 9, 6}},
            new[] {new[] {8, 8, 5, 0}, new[] {15, 12, 9, 0}, new[] {6, 18, 9, 0}}
        };

        private readonly ABuffer _Buffer;
        private readonly int _Channels;
        private readonly SynthesisFilter _Filter1;
        private readonly SynthesisFilter _Filter2;
        private readonly int _FirstChannel;
        private readonly Header _Header;
        private readonly int[] _Is1D;
        private readonly float[][] _K;
        private readonly int _LastChannel;
        private readonly float[][][] _Lr;

        private readonly int _MaxGr;
        private readonly int[] _Nonzero;
        private readonly float[] _Out1D;
        private readonly float[][] _Prevblck;
        private readonly float[][][] _Ro;
        private readonly ScaleFactorData[] _Scalefac;
        private readonly SBI[] _SfBandIndex; // Init in the ctor.
        private readonly int _Sfreq;
        private readonly Layer3SideInfo _SideInfo;
        private readonly Bitstream _Stream;
        private readonly int _WhichChannels;
        private BitReserve _BitReserve;

        private int _CheckSumHuff;
        private int _FrameStart;

        internal int[] IsPos;

        internal float[] IsRatio;

        // MDM: new_slen is fully initialized before use, no need
        // to reallocate array.
        private int[] _NewSlen;

        private int _Part2Start;
        internal float[] Rawout;

        // subband samples are buffered and passed to the
        // SynthesisFilter in one go.
        private float[] _Samples1;

        private float[] _Samples2;
        internal int[] ScalefacBuffer;
        internal ScaleFactorTable Sftable;

        // MDM: tsOutCopy and rawout do not need initializing, so the arrays
        // can be reused.
        internal float[] TsOutCopy;

        internal int[] V = {0};
        internal int[] W = {0};
        internal int[] X = {0};
        internal int[] Y = {0};

        static LayerIIIDecoder() {
            PowerTable = CreatePowerTable();
        }

        /// <summary>
        /// REVIEW: these ctor arguments should be moved to the decodeFrame() method.
        /// </summary>
        internal LayerIIIDecoder(Bitstream stream, Header header, SynthesisFilter filtera, SynthesisFilter filterb, ABuffer buffer, int whichCh) {
            Huffman.Initialize();

            InitBlock();
            _Is1D = new int[SBLIMIT * SSLIMIT + 4];
            _Ro = new float[2][][];
            for (int i = 0; i < 2; i++) {
                _Ro[i] = new float[SBLIMIT][];
                for (int i2 = 0; i2 < SBLIMIT; i2++) {
                    _Ro[i][i2] = new float[SSLIMIT];
                }
            }
            _Lr = new float[2][][];
            for (int i3 = 0; i3 < 2; i3++) {
                _Lr[i3] = new float[SBLIMIT][];
                for (int i4 = 0; i4 < SBLIMIT; i4++) {
                    _Lr[i3][i4] = new float[SSLIMIT];
                }
            }
            _Out1D = new float[SBLIMIT * SSLIMIT];
            _Prevblck = new float[2][];
            for (int i5 = 0; i5 < 2; i5++) {
                _Prevblck[i5] = new float[SBLIMIT * SSLIMIT];
            }
            _K = new float[2][];
            for (int i6 = 0; i6 < 2; i6++) {
                _K[i6] = new float[SBLIMIT * SSLIMIT];
            }
            _Nonzero = new int[2];

            //III_scalefact_t
            ScaleFactorData[] iiiScalefacT = new ScaleFactorData[2];
            iiiScalefacT[0] = new ScaleFactorData();
            iiiScalefacT[1] = new ScaleFactorData();
            _Scalefac = iiiScalefacT;
            // L3TABLE INIT

            _SfBandIndex = new SBI[9]; // SZD: MPEG2.5 +3 indices
            int[] l0 = {
                0, 6, 12, 18, 24, 30, 36, 44, 54, 66, 80, 96, 116, 140, 168, 200, 238, 284, 336, 396, 464, 522, 576
            };
            int[] s0 = {0, 4, 8, 12, 18, 24, 32, 42, 56, 74, 100, 132, 174, 192};
            int[] l1 = {
                0, 6, 12, 18, 24, 30, 36, 44, 54, 66, 80, 96, 114, 136, 162, 194, 232, 278, 330, 394, 464, 540, 576
            };
            int[] s1 = {0, 4, 8, 12, 18, 26, 36, 48, 62, 80, 104, 136, 180, 192};
            int[] l2 = {
                0, 6, 12, 18, 24, 30, 36, 44, 54, 66, 80, 96, 116, 140, 168, 200, 238, 284, 336, 396, 464, 522, 576
            };
            int[] s2 = {0, 4, 8, 12, 18, 26, 36, 48, 62, 80, 104, 134, 174, 192};

            int[] l3 = {
                0, 4, 8, 12, 16, 20, 24, 30, 36, 44, 52, 62, 74, 90, 110, 134, 162, 196, 238, 288, 342, 418, 576
            };
            int[] s3 = {0, 4, 8, 12, 16, 22, 30, 40, 52, 66, 84, 106, 136, 192};
            int[] l4 = {
                0, 4, 8, 12, 16, 20, 24, 30, 36, 42, 50, 60, 72, 88, 106, 128, 156, 190, 230, 276, 330, 384, 576
            };
            int[] s4 = {0, 4, 8, 12, 16, 22, 28, 38, 50, 64, 80, 100, 126, 192};
            int[] l5 = {
                0, 4, 8, 12, 16, 20, 24, 30, 36, 44, 54, 66, 82, 102, 126, 156, 194, 240, 296, 364, 448, 550,
                576
            };
            int[] s5 = {0, 4, 8, 12, 16, 22, 30, 42, 58, 78, 104, 138, 180, 192};
            // SZD: MPEG2.5
            int[] l6 = {
                0, 6, 12, 18, 24, 30, 36, 44, 54, 66, 80, 96, 116, 140, 168, 200, 238, 284, 336, 396, 464, 522,
                576
            };
            int[] s6 = {0, 4, 8, 12, 18, 26, 36, 48, 62, 80, 104, 134, 174, 192};
            int[] l7 = {
                0, 6, 12, 18, 24, 30, 36, 44, 54, 66, 80, 96, 116, 140, 168, 200, 238, 284, 336, 396, 464, 522,
                576
            };
            int[] s7 = {0, 4, 8, 12, 18, 26, 36, 48, 62, 80, 104, 134, 174, 192};
            int[] l8 = {
                0, 12, 24, 36, 48, 60, 72, 88, 108, 132, 160, 192, 232, 280, 336, 400, 476, 566, 568, 570, 572,
                574, 576
            };
            int[] s8 = {0, 8, 16, 24, 36, 52, 72, 96, 124, 160, 162, 164, 166, 192};

            _SfBandIndex[0] = new SBI(l0, s0);
            _SfBandIndex[1] = new SBI(l1, s1);
            _SfBandIndex[2] = new SBI(l2, s2);

            _SfBandIndex[3] = new SBI(l3, s3);
            _SfBandIndex[4] = new SBI(l4, s4);
            _SfBandIndex[5] = new SBI(l5, s5);
            //SZD: MPEG2.5
            _SfBandIndex[6] = new SBI(l6, s6);
            _SfBandIndex[7] = new SBI(l7, s7);
            _SfBandIndex[8] = new SBI(l8, s8);
            // END OF L3TABLE INIT

            if (_reorderTable == null) {
                // SZD: generate LUT
                _reorderTable = new int[9][];
                for (int i = 0; i < 9; i++)
                    _reorderTable[i] = Reorder(_SfBandIndex[i].S);
            }

            // Sftable
            int[] ll0 = {0, 6, 11, 16, 21};
            int[] ss0 = {0, 6, 12};
            Sftable = new ScaleFactorTable(this, ll0, ss0);
            // END OF Sftable

            // scalefac_buffer
            ScalefacBuffer = new int[54];
            // END OF scalefac_buffer

            _Stream = stream;
            _Header = header;
            _Filter1 = filtera;
            _Filter2 = filterb;
            _Buffer = buffer;
            _WhichChannels = whichCh;

            _FrameStart = 0;
            _Channels = _Header.Mode() == Header.SINGLE_CHANNEL ? 1 : 2;
            _MaxGr = _Header.Version() == Header.MPEG1 ? 2 : 1;

            _Sfreq = _Header.sample_frequency() +
                    (_Header.Version() == Header.MPEG1 ? 3 : _Header.Version() == Header.MPEG25_LSF ? 6 : 0); // SZD

            if (_Channels == 2) {
                switch (_WhichChannels) {
                    case (int)OutputChannelsEnum.LeftChannel:
                    case (int)OutputChannelsEnum.DownmixChannels:
                        _FirstChannel = _LastChannel = 0;
                        break;

                    case (int)OutputChannelsEnum.RightChannel:
                        _FirstChannel = _LastChannel = 1;
                        break;

                    default: // OutputChannelsEnum.BothChannels:
                        _FirstChannel = 0;
                        _LastChannel = 1;
                        break;
                }
            }
            else {
                _FirstChannel = _LastChannel = 0;
            }

            for (int ch = 0; ch < 2; ch++)
                for (int j = 0; j < 576; j++)
                    _Prevblck[ch][j] = 0.0f;

            _Nonzero[0] = _Nonzero[1] = 576;

            _BitReserve = new BitReserve();
            _SideInfo = new Layer3SideInfo();
        }

        public void DecodeFrame() {
            Decode();
        }

        private void InitBlock() {
            Rawout = new float[36];
            TsOutCopy = new float[18];
            IsRatio = new float[576];
            IsPos = new int[576];
            _NewSlen = new int[4];
            _Samples2 = new float[32];
            _Samples1 = new float[32];
        }

        /// <summary>
        /// Notify decoder that a seek is being made.
        /// </summary>
        internal void SeekNotify() {
            _FrameStart = 0;
            for (int ch = 0; ch < 2; ch++)
                for (int j = 0; j < 576; j++)
                    _Prevblck[ch][j] = 0.0f;
            _BitReserve = new BitReserve();
        }

        internal void Decode() {
            int nSlots = _Header.Slots();
            int flushMain;
            int gr, ch, ss, sb, sb18;
            int mainDataEnd;
            int bytesToDiscard;
            int i;

            ReadSideInfo();

            for (i = 0; i < nSlots; i++)
                _BitReserve.PutBuffer(_Stream.GetBitsFromBuffer(8));

            mainDataEnd = SupportClass.URShift(_BitReserve.HssTell(), 3); // of previous frame

            if ((flushMain = _BitReserve.HssTell() & 7) != 0) {
                _BitReserve.ReadBits(8 - flushMain);
                mainDataEnd++;
            }

            bytesToDiscard = _FrameStart - mainDataEnd - _SideInfo.MainDataBegin;

            _FrameStart += nSlots;

            if (bytesToDiscard < 0)
                return;

            if (mainDataEnd > 4096) {
                _FrameStart -= 4096;
                _BitReserve.RewindStreamBytes(4096);
            }

            for (; bytesToDiscard > 0; bytesToDiscard--)
                _BitReserve.ReadBits(8);

            for (gr = 0; gr < _MaxGr; gr++) {
                for (ch = 0; ch < _Channels; ch++) {
                    _Part2Start = _BitReserve.HssTell();

                    if (_Header.Version() == Header.MPEG1)
                        ReadScaleFactors(ch, gr);
                    // MPEG-2 LSF, SZD: MPEG-2.5 LSF
                    else
                        GLSFScaleFactors(ch, gr);

                    HuffmanDecode(ch, gr);
                    // System.out.println("CheckSum HuffMan = " + CheckSumHuff);
                    DequantizeSample(_Ro[ch], ch, gr);
                }

                Stereo(gr);

                if (_WhichChannels == OutputChannels.DOWNMIX_CHANNELS && _Channels > 1)
                    DoDownMix();

                for (ch = _FirstChannel; ch <= _LastChannel; ch++) {
                    Reorder(_Lr[ch], ch, gr);
                    Antialias(ch, gr);
                    //for (int hb = 0;hb<576;hb++) CheckSumOut1d = CheckSumOut1d + out_1d[hb];
                    //System.out.println("CheckSumOut1d = "+CheckSumOut1d);

                    Hybrid(ch, gr);

                    //for (int hb = 0;hb<576;hb++) CheckSumOut1d = CheckSumOut1d + out_1d[hb];
                    //System.out.println("CheckSumOut1d = "+CheckSumOut1d);

                    for (sb18 = 18; sb18 < 576; sb18 += 36)
                        // Frequency inversion
                        for (ss = 1; ss < SSLIMIT; ss += 2)
                            _Out1D[sb18 + ss] = -_Out1D[sb18 + ss];

                    if (ch == 0 || _WhichChannels == OutputChannels.RIGHT_CHANNEL) {
                        for (ss = 0; ss < SSLIMIT; ss++) {
                            // Polyphase synthesis
                            sb = 0;
                            for (sb18 = 0; sb18 < 576; sb18 += 18) {
                                _Samples1[sb] = _Out1D[sb18 + ss];
                                //filter1.input_sample(out_1d[sb18+ss], sb);
                                sb++;
                            }
                            //buffer.appendSamples(0, samples1);
                            //Console.WriteLine("Adding samples right into output buffer");
                            _Filter1.AddSamples(_Samples1);
                            _Filter1.calculate_pc_samples(_Buffer);
                        }
                    }
                    else {
                        for (ss = 0; ss < SSLIMIT; ss++) {
                            // Polyphase synthesis
                            sb = 0;
                            for (sb18 = 0; sb18 < 576; sb18 += 18) {
                                _Samples2[sb] = _Out1D[sb18 + ss];
                                //filter2.input_sample(out_1d[sb18+ss], sb);
                                sb++;
                            }
                            //buffer.appendSamples(1, samples2);
                            //Console.WriteLine("Adding samples right into output buffer");
                            _Filter2.AddSamples(_Samples2);
                            _Filter2.calculate_pc_samples(_Buffer);
                        }
                    }
                }
                // channels
            }
            // granule
            _Buffer.WriteBuffer(1);
        }

        /// <summary>
        /// Reads the side info from the stream, assuming the entire.
        /// frame has been read already.
        /// Mono   : 136 bits (= 17 bytes)
        /// Stereo : 256 bits (= 32 bytes)
        /// </summary>
        private bool ReadSideInfo() {
            int ch, gr;
            if (_Header.Version() == Header.MPEG1) {
                _SideInfo.MainDataBegin = _Stream.GetBitsFromBuffer(9);
                if (_Channels == 1)
                    _SideInfo.PrivateBits = _Stream.GetBitsFromBuffer(5);
                else
                    _SideInfo.PrivateBits = _Stream.GetBitsFromBuffer(3);

                for (ch = 0; ch < _Channels; ch++) {
                    _SideInfo.Channels[ch].ScaleFactorBits[0] = _Stream.GetBitsFromBuffer(1);
                    _SideInfo.Channels[ch].ScaleFactorBits[1] = _Stream.GetBitsFromBuffer(1);
                    _SideInfo.Channels[ch].ScaleFactorBits[2] = _Stream.GetBitsFromBuffer(1);
                    _SideInfo.Channels[ch].ScaleFactorBits[3] = _Stream.GetBitsFromBuffer(1);
                }

                for (gr = 0; gr < 2; gr++) {
                    for (ch = 0; ch < _Channels; ch++) {
                        _SideInfo.Channels[ch].Granules[gr].Part23Length = _Stream.GetBitsFromBuffer(12);
                        _SideInfo.Channels[ch].Granules[gr].BigValues = _Stream.GetBitsFromBuffer(9);
                        _SideInfo.Channels[ch].Granules[gr].GlobalGain = _Stream.GetBitsFromBuffer(8);
                        _SideInfo.Channels[ch].Granules[gr].ScaleFacCompress = _Stream.GetBitsFromBuffer(4);
                        _SideInfo.Channels[ch].Granules[gr].WindowSwitchingFlag = _Stream.GetBitsFromBuffer(1);
                        if (_SideInfo.Channels[ch].Granules[gr].WindowSwitchingFlag != 0) {
                            _SideInfo.Channels[ch].Granules[gr].BlockType = _Stream.GetBitsFromBuffer(2);
                            _SideInfo.Channels[ch].Granules[gr].MixedBlockFlag = _Stream.GetBitsFromBuffer(1);

                            _SideInfo.Channels[ch].Granules[gr].TableSelect[0] = _Stream.GetBitsFromBuffer(5);
                            _SideInfo.Channels[ch].Granules[gr].TableSelect[1] = _Stream.GetBitsFromBuffer(5);

                            _SideInfo.Channels[ch].Granules[gr].SubblockGain[0] = _Stream.GetBitsFromBuffer(3);
                            _SideInfo.Channels[ch].Granules[gr].SubblockGain[1] = _Stream.GetBitsFromBuffer(3);
                            _SideInfo.Channels[ch].Granules[gr].SubblockGain[2] = _Stream.GetBitsFromBuffer(3);

                            // Set region_count parameters since they are implicit in this case.

                            if (_SideInfo.Channels[ch].Granules[gr].BlockType == 0) {
                                // Side info bad: block_type == 0 in split block
                                return false;
                            }
                            if (_SideInfo.Channels[ch].Granules[gr].BlockType == 2 && _SideInfo.Channels[ch].Granules[gr].MixedBlockFlag == 0) {
                                _SideInfo.Channels[ch].Granules[gr].Region0Count = 8;
                            }
                            else {
                                _SideInfo.Channels[ch].Granules[gr].Region0Count = 7;
                            }
                            _SideInfo.Channels[ch].Granules[gr].Region1Count = 20 - _SideInfo.Channels[ch].Granules[gr].Region0Count;
                        }
                        else {
                            _SideInfo.Channels[ch].Granules[gr].TableSelect[0] = _Stream.GetBitsFromBuffer(5);
                            _SideInfo.Channels[ch].Granules[gr].TableSelect[1] = _Stream.GetBitsFromBuffer(5);
                            _SideInfo.Channels[ch].Granules[gr].TableSelect[2] = _Stream.GetBitsFromBuffer(5);
                            _SideInfo.Channels[ch].Granules[gr].Region0Count = _Stream.GetBitsFromBuffer(4);
                            _SideInfo.Channels[ch].Granules[gr].Region1Count = _Stream.GetBitsFromBuffer(3);
                            _SideInfo.Channels[ch].Granules[gr].BlockType = 0;
                        }
                        _SideInfo.Channels[ch].Granules[gr].Preflag = _Stream.GetBitsFromBuffer(1);
                        _SideInfo.Channels[ch].Granules[gr].ScaleFacScale = _Stream.GetBitsFromBuffer(1);
                        _SideInfo.Channels[ch].Granules[gr].Count1TableSelect = _Stream.GetBitsFromBuffer(1);
                    }
                }
            }
            else {
                // MPEG-2 LSF, SZD: MPEG-2.5 LSF

                _SideInfo.MainDataBegin = _Stream.GetBitsFromBuffer(8);
                if (_Channels == 1)
                    _SideInfo.PrivateBits = _Stream.GetBitsFromBuffer(1);
                else
                    _SideInfo.PrivateBits = _Stream.GetBitsFromBuffer(2);

                for (ch = 0; ch < _Channels; ch++) {
                    _SideInfo.Channels[ch].Granules[0].Part23Length = _Stream.GetBitsFromBuffer(12);
                    _SideInfo.Channels[ch].Granules[0].BigValues = _Stream.GetBitsFromBuffer(9);
                    _SideInfo.Channels[ch].Granules[0].GlobalGain = _Stream.GetBitsFromBuffer(8);
                    _SideInfo.Channels[ch].Granules[0].ScaleFacCompress = _Stream.GetBitsFromBuffer(9);
                    _SideInfo.Channels[ch].Granules[0].WindowSwitchingFlag = _Stream.GetBitsFromBuffer(1);

                    if (_SideInfo.Channels[ch].Granules[0].WindowSwitchingFlag != 0) {
                        _SideInfo.Channels[ch].Granules[0].BlockType = _Stream.GetBitsFromBuffer(2);
                        _SideInfo.Channels[ch].Granules[0].MixedBlockFlag = _Stream.GetBitsFromBuffer(1);
                        _SideInfo.Channels[ch].Granules[0].TableSelect[0] = _Stream.GetBitsFromBuffer(5);
                        _SideInfo.Channels[ch].Granules[0].TableSelect[1] = _Stream.GetBitsFromBuffer(5);

                        _SideInfo.Channels[ch].Granules[0].SubblockGain[0] = _Stream.GetBitsFromBuffer(3);
                        _SideInfo.Channels[ch].Granules[0].SubblockGain[1] = _Stream.GetBitsFromBuffer(3);
                        _SideInfo.Channels[ch].Granules[0].SubblockGain[2] = _Stream.GetBitsFromBuffer(3);

                        // Set region_count parameters since they are implicit in this case.

                        if (_SideInfo.Channels[ch].Granules[0].BlockType == 0) {
                            // Side info bad: block_type == 0 in split block
                            return false;
                        }
                        if (_SideInfo.Channels[ch].Granules[0].BlockType == 2 && _SideInfo.Channels[ch].Granules[0].MixedBlockFlag == 0) {
                            _SideInfo.Channels[ch].Granules[0].Region0Count = 8;
                        }
                        else {
                            _SideInfo.Channels[ch].Granules[0].Region0Count = 7;
                            _SideInfo.Channels[ch].Granules[0].Region1Count = 20 - _SideInfo.Channels[ch].Granules[0].Region0Count;
                        }
                    }
                    else {
                        _SideInfo.Channels[ch].Granules[0].TableSelect[0] = _Stream.GetBitsFromBuffer(5);
                        _SideInfo.Channels[ch].Granules[0].TableSelect[1] = _Stream.GetBitsFromBuffer(5);
                        _SideInfo.Channels[ch].Granules[0].TableSelect[2] = _Stream.GetBitsFromBuffer(5);
                        _SideInfo.Channels[ch].Granules[0].Region0Count = _Stream.GetBitsFromBuffer(4);
                        _SideInfo.Channels[ch].Granules[0].Region1Count = _Stream.GetBitsFromBuffer(3);
                        _SideInfo.Channels[ch].Granules[0].BlockType = 0;
                    }

                    _SideInfo.Channels[ch].Granules[0].ScaleFacScale = _Stream.GetBitsFromBuffer(1);
                    _SideInfo.Channels[ch].Granules[0].Count1TableSelect = _Stream.GetBitsFromBuffer(1);
                }
                // for(ch=0; ch<channels; ch++)
            }
            // if (header.version() == MPEG1)
            return true;
        }

        /// <summary>
        /// *
        /// </summary>
        private void ReadScaleFactors(int ch, int gr) {
            int sfb, window;
            GranuleInfo grInfo = _SideInfo.Channels[ch].Granules[gr];
            int scaleComp = grInfo.ScaleFacCompress;
            int length0 = Slen[0][scaleComp];
            int length1 = Slen[1][scaleComp];

            if (grInfo.WindowSwitchingFlag != 0 && grInfo.BlockType == 2) {
                if (grInfo.MixedBlockFlag != 0) {
                    // MIXED
                    for (sfb = 0; sfb < 8; sfb++)
                        _Scalefac[ch].L[sfb] = _BitReserve.ReadBits(Slen[0][grInfo.ScaleFacCompress]);
                    for (sfb = 3; sfb < 6; sfb++)
                        for (window = 0; window < 3; window++)
                            _Scalefac[ch].S[window][sfb] = _BitReserve.ReadBits(Slen[0][grInfo.ScaleFacCompress]);
                    for (sfb = 6; sfb < 12; sfb++)
                        for (window = 0; window < 3; window++)
                            _Scalefac[ch].S[window][sfb] = _BitReserve.ReadBits(Slen[1][grInfo.ScaleFacCompress]);
                    for (sfb = 12, window = 0; window < 3; window++)
                        _Scalefac[ch].S[window][sfb] = 0;
                }
                else {
                    // SHORT

                    _Scalefac[ch].S[0][0] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].S[1][0] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].S[2][0] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].S[0][1] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].S[1][1] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].S[2][1] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].S[0][2] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].S[1][2] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].S[2][2] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].S[0][3] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].S[1][3] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].S[2][3] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].S[0][4] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].S[1][4] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].S[2][4] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].S[0][5] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].S[1][5] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].S[2][5] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].S[0][6] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].S[1][6] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].S[2][6] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].S[0][7] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].S[1][7] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].S[2][7] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].S[0][8] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].S[1][8] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].S[2][8] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].S[0][9] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].S[1][9] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].S[2][9] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].S[0][10] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].S[1][10] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].S[2][10] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].S[0][11] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].S[1][11] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].S[2][11] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].S[0][12] = 0;
                    _Scalefac[ch].S[1][12] = 0;
                    _Scalefac[ch].S[2][12] = 0;
                }
                // SHORT
            }
            else {
                // LONG types 0,1,3

                if (_SideInfo.Channels[ch].ScaleFactorBits[0] == 0 || gr == 0) {
                    _Scalefac[ch].L[0] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].L[1] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].L[2] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].L[3] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].L[4] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].L[5] = _BitReserve.ReadBits(length0);
                }
                if (_SideInfo.Channels[ch].ScaleFactorBits[1] == 0 || gr == 0) {
                    _Scalefac[ch].L[6] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].L[7] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].L[8] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].L[9] = _BitReserve.ReadBits(length0);
                    _Scalefac[ch].L[10] = _BitReserve.ReadBits(length0);
                }
                if (_SideInfo.Channels[ch].ScaleFactorBits[2] == 0 || gr == 0) {
                    _Scalefac[ch].L[11] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].L[12] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].L[13] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].L[14] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].L[15] = _BitReserve.ReadBits(length1);
                }
                if (_SideInfo.Channels[ch].ScaleFactorBits[3] == 0 || gr == 0) {
                    _Scalefac[ch].L[16] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].L[17] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].L[18] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].L[19] = _BitReserve.ReadBits(length1);
                    _Scalefac[ch].L[20] = _BitReserve.ReadBits(length1);
                }

                _Scalefac[ch].L[21] = 0;
                _Scalefac[ch].L[22] = 0;
            }
        }

        private void GetLSFScaleData(int ch, int gr) {
            int scalefacComp, intScalefacComp;
            int modeExt = _Header.mode_extension();
            int m;
            int blocktypenumber;
            int blocknumber = 0;

            GranuleInfo grInfo = _SideInfo.Channels[ch].Granules[gr];

            scalefacComp = grInfo.ScaleFacCompress;

            if (grInfo.BlockType == 2) {
                if (grInfo.MixedBlockFlag == 0)
                    blocktypenumber = 1;
                else if (grInfo.MixedBlockFlag == 1)
                    blocktypenumber = 2;
                else
                    blocktypenumber = 0;
            }
            else {
                blocktypenumber = 0;
            }

            if (!((modeExt == 1 || modeExt == 3) && ch == 1)) {
                if (scalefacComp < 400) {
                    _NewSlen[0] = SupportClass.URShift(scalefacComp, 4) / 5;
                    _NewSlen[1] = SupportClass.URShift(scalefacComp, 4) % 5;
                    _NewSlen[2] = SupportClass.URShift(scalefacComp & 0xF, 2);
                    _NewSlen[3] = scalefacComp & 3;
                    _SideInfo.Channels[ch].Granules[gr].Preflag = 0;
                    blocknumber = 0;
                }
                else if (scalefacComp < 500) {
                    _NewSlen[0] = SupportClass.URShift(scalefacComp - 400, 2) / 5;
                    _NewSlen[1] = SupportClass.URShift(scalefacComp - 400, 2) % 5;
                    _NewSlen[2] = (scalefacComp - 400) & 3;
                    _NewSlen[3] = 0;
                    _SideInfo.Channels[ch].Granules[gr].Preflag = 0;
                    blocknumber = 1;
                }
                else if (scalefacComp < 512) {
                    _NewSlen[0] = (scalefacComp - 500) / 3;
                    _NewSlen[1] = (scalefacComp - 500) % 3;
                    _NewSlen[2] = 0;
                    _NewSlen[3] = 0;
                    _SideInfo.Channels[ch].Granules[gr].Preflag = 1;
                    blocknumber = 2;
                }
            }

            if ((modeExt == 1 || modeExt == 3) && ch == 1) {
                intScalefacComp = SupportClass.URShift(scalefacComp, 1);

                if (intScalefacComp < 180) {
                    _NewSlen[0] = intScalefacComp / 36;
                    _NewSlen[1] = intScalefacComp % 36 / 6;
                    _NewSlen[2] = intScalefacComp % 36 % 6;
                    _NewSlen[3] = 0;
                    _SideInfo.Channels[ch].Granules[gr].Preflag = 0;
                    blocknumber = 3;
                }
                else if (intScalefacComp < 244) {
                    _NewSlen[0] = SupportClass.URShift((intScalefacComp - 180) & 0x3F, 4);
                    _NewSlen[1] = SupportClass.URShift((intScalefacComp - 180) & 0xF, 2);
                    _NewSlen[2] = (intScalefacComp - 180) & 3;
                    _NewSlen[3] = 0;
                    _SideInfo.Channels[ch].Granules[gr].Preflag = 0;
                    blocknumber = 4;
                }
                else if (intScalefacComp < 255) {
                    _NewSlen[0] = (intScalefacComp - 244) / 3;
                    _NewSlen[1] = (intScalefacComp - 244) % 3;
                    _NewSlen[2] = 0;
                    _NewSlen[3] = 0;
                    _SideInfo.Channels[ch].Granules[gr].Preflag = 0;
                    blocknumber = 5;
                }
            }

            for (int x = 0; x < 45; x++)
                // why 45, not 54?
                ScalefacBuffer[x] = 0;

            m = 0;
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < NrOfSfbBlock[blocknumber][blocktypenumber][i]; j++) {
                    ScalefacBuffer[m] = _NewSlen[i] == 0 ? 0 : _BitReserve.ReadBits(_NewSlen[i]);
                    m++;
                }
                // for (unint32 j ...
            }
            // for (uint32 i ...
        }

        /// <summary>
        /// *
        /// </summary>
        private void GLSFScaleFactors(int ch, int gr) {
            int m = 0;
            int sfb;
            GranuleInfo grInfo = _SideInfo.Channels[ch].Granules[gr];

            GetLSFScaleData(ch, gr);

            if (grInfo.WindowSwitchingFlag != 0 && grInfo.BlockType == 2) {
                int window;
                if (grInfo.MixedBlockFlag != 0) {
                    // MIXED
                    for (sfb = 0; sfb < 8; sfb++) {
                        _Scalefac[ch].L[sfb] = ScalefacBuffer[m];
                        m++;
                    }
                    for (sfb = 3; sfb < 12; sfb++) {
                        for (window = 0; window < 3; window++) {
                            _Scalefac[ch].S[window][sfb] = ScalefacBuffer[m];
                            m++;
                        }
                    }
                    for (window = 0; window < 3; window++)
                        _Scalefac[ch].S[window][12] = 0;
                }
                else {
                    // SHORT

                    for (sfb = 0; sfb < 12; sfb++) {
                        for (window = 0; window < 3; window++) {
                            _Scalefac[ch].S[window][sfb] = ScalefacBuffer[m];
                            m++;
                        }
                    }

                    for (window = 0; window < 3; window++)
                        _Scalefac[ch].S[window][12] = 0;
                }
            }
            else {
                // LONG types 0,1,3

                for (sfb = 0; sfb < 21; sfb++) {
                    _Scalefac[ch].L[sfb] = ScalefacBuffer[m];
                    m++;
                }
                _Scalefac[ch].L[21] = 0; // Jeff
                _Scalefac[ch].L[22] = 0;
            }
        }

        private void HuffmanDecode(int ch, int gr) {
            X[0] = 0;
            Y[0] = 0;
            V[0] = 0;
            W[0] = 0;
            int part23End = _Part2Start + _SideInfo.Channels[ch].Granules[gr].Part23Length;
            int nuBits;
            int region1Start;
            int region2Start;
            int index;

            int buf, buf1;

            Huffman h;

            // Find region boundary for short block case

            if (_SideInfo.Channels[ch].Granules[gr].WindowSwitchingFlag != 0 && _SideInfo.Channels[ch].Granules[gr].BlockType == 2) {
                // Region2.
                //MS: Extrahandling for 8KHZ
                region1Start = _Sfreq == 8 ? 72 : 36; // sfb[9/3]*3=36 or in case 8KHZ = 72
                region2Start = 576; // No Region2 for short block case
            }
            else {
                // Find region boundary for long block case

                buf = _SideInfo.Channels[ch].Granules[gr].Region0Count + 1;
                buf1 = buf + _SideInfo.Channels[ch].Granules[gr].Region1Count + 1;

                if (buf1 > _SfBandIndex[_Sfreq].L.Length - 1)
                    buf1 = _SfBandIndex[_Sfreq].L.Length - 1;

                region1Start = _SfBandIndex[_Sfreq].L[buf];
                region2Start = _SfBandIndex[_Sfreq].L[buf1]; /* MI */
            }

            index = 0;
            // Read bigvalues area
            for (int i = 0; i < _SideInfo.Channels[ch].Granules[gr].BigValues << 1; i += 2) {
                if (i < region1Start)
                    h = Huffman.HuffmanTable[_SideInfo.Channels[ch].Granules[gr].TableSelect[0]];
                else if (i < region2Start)
                    h = Huffman.HuffmanTable[_SideInfo.Channels[ch].Granules[gr].TableSelect[1]];
                else
                    h = Huffman.HuffmanTable[_SideInfo.Channels[ch].Granules[gr].TableSelect[2]];

                Huffman.Decode(h, X, Y, V, W, _BitReserve);

                _Is1D[index++] = X[0];
                _Is1D[index++] = Y[0];
                _CheckSumHuff = _CheckSumHuff + X[0] + Y[0];
                // System.out.println("x = "+x[0]+" y = "+y[0]);
            }

            // Read count1 area
            h = Huffman.HuffmanTable[_SideInfo.Channels[ch].Granules[gr].Count1TableSelect + 32];
            nuBits = _BitReserve.HssTell();

            while (nuBits < part23End && index < 576) {
                Huffman.Decode(h, X, Y, V, W, _BitReserve);

                _Is1D[index++] = V[0];
                _Is1D[index++] = W[0];
                _Is1D[index++] = X[0];
                _Is1D[index++] = Y[0];
                _CheckSumHuff = _CheckSumHuff + V[0] + W[0] + X[0] + Y[0];
                // System.out.println("v = "+v[0]+" w = "+w[0]);
                // System.out.println("x = "+x[0]+" y = "+y[0]);
                nuBits = _BitReserve.HssTell();
            }

            if (nuBits > part23End) {
                _BitReserve.RewindStreamBits(nuBits - part23End);
                index -= 4;
            }

            nuBits = _BitReserve.HssTell();

            // Dismiss stuffing bits
            if (nuBits < part23End)
                _BitReserve.ReadBits(part23End - nuBits);

            // Zero out rest

            if (index < 576)
                _Nonzero[ch] = index;
            else
                _Nonzero[ch] = 576;

            if (index < 0)
                index = 0;

            // may not be necessary
            for (; index < 576; index++)
                _Is1D[index] = 0;
        }

        /// <summary>
        /// *
        /// </summary>
        private void GetKStereoValues(int isPos, int ioType, int i) {
            if (isPos == 0) {
                _K[0][i] = 1.0f;
                _K[1][i] = 1.0f;
            }
            else if ((isPos & 1) != 0) {
                _K[0][i] = Io[ioType][SupportClass.URShift(isPos + 1, 1)];
                _K[1][i] = 1.0f;
            }
            else {
                _K[0][i] = 1.0f;
                _K[1][i] = Io[ioType][SupportClass.URShift(isPos, 1)];
            }
        }

        /// <summary>
        /// *
        /// </summary>
        private void DequantizeSample(float[][] xr, int ch, int gr) {
            GranuleInfo grInfo = _SideInfo.Channels[ch].Granules[gr];
            int cb = 0;
            int nextCbBoundary;
            int cbBegin = 0;
            int cbWidth = 0;
            int index = 0;
            int j;
            float[][] xr1D = xr;

            // choose correct scalefactor band per block type, initalize boundary

            if (grInfo.WindowSwitchingFlag != 0 && grInfo.BlockType == 2) {
                if (grInfo.MixedBlockFlag != 0)
                    nextCbBoundary = _SfBandIndex[_Sfreq].L[1];
                // LONG blocks: 0,1,3
                else {
                    cbWidth = _SfBandIndex[_Sfreq].S[1];
                    nextCbBoundary = (cbWidth << 2) - cbWidth;
                    cbBegin = 0;
                }
            }
            else {
                nextCbBoundary = _SfBandIndex[_Sfreq].L[1]; // LONG blocks: 0,1,3
            }

            // Compute overall (global) scaling.

            float gGain = (float)Math.Pow(2.0, 0.25 * (grInfo.GlobalGain - 210.0));

            for (j = 0; j < _Nonzero[ch]; j++) {
                int reste = j % SSLIMIT;
                int quotien = (j - reste) / SSLIMIT;
                if (_Is1D[j] == 0)
                    xr1D[quotien][reste] = 0.0f;
                else {
                    int abv = _Is1D[j];
                    // Begin Patch
                    // This was taken from a patch to the original java file. Ported by DamianMehers
                    // Original:
                    // if (is_1d[j] > 0)
                    //     xr_1d[quotien][reste] = g_gain * t_43[abv];
                    // else
                    //     xr_1d[quotien][reste] = -g_gain * t_43[-abv];
                    const double d43 = 4.0 / 3.0;
                    if (abv < PowerTable.Length) {
                        if (_Is1D[j] > 0) {
                            xr1D[quotien][reste] = gGain * PowerTable[abv];
                        }
                        else if (-abv < PowerTable.Length) {
                            xr1D[quotien][reste] = -gGain * PowerTable[-abv];
                        }
                        else {
                            xr1D[quotien][reste] = -gGain * (float)Math.Pow(-abv, d43);
                        }
                    }
                    else if (_Is1D[j] > 0) {
                        xr1D[quotien][reste] = gGain * (float)Math.Pow(abv, d43);
                    }
                    else {
                        xr1D[quotien][reste] = -gGain * (float)Math.Pow(-abv, d43);
                    }
                    // End Patch
                }
            }

            // apply formula per block type
            for (j = 0; j < _Nonzero[ch]; j++) {
                int reste = j % SSLIMIT;
                int quotien = (j - reste) / SSLIMIT;
                if (index == nextCbBoundary) {
                    /* Adjust critical band boundary */
                    if (grInfo.WindowSwitchingFlag != 0 && grInfo.BlockType == 2) {
                        if (grInfo.MixedBlockFlag != 0) {
                            if (index == _SfBandIndex[_Sfreq].L[8]) {
                                nextCbBoundary = _SfBandIndex[_Sfreq].S[4];
                                nextCbBoundary = (nextCbBoundary << 2) - nextCbBoundary;
                                cb = 3;
                                cbWidth = _SfBandIndex[_Sfreq].S[4] - _SfBandIndex[_Sfreq].S[3];

                                cbBegin = _SfBandIndex[_Sfreq].S[3];
                                cbBegin = (cbBegin << 2) - cbBegin;
                            }
                            else if (index < _SfBandIndex[_Sfreq].L[8]) {
                                nextCbBoundary = _SfBandIndex[_Sfreq].L[++cb + 1];
                            }
                            else {
                                nextCbBoundary = _SfBandIndex[_Sfreq].S[++cb + 1];
                                nextCbBoundary = (nextCbBoundary << 2) - nextCbBoundary;

                                cbBegin = _SfBandIndex[_Sfreq].S[cb];
                                cbWidth = _SfBandIndex[_Sfreq].S[cb + 1] - cbBegin;
                                cbBegin = (cbBegin << 2) - cbBegin;
                            }
                        }
                        else {
                            nextCbBoundary = _SfBandIndex[_Sfreq].S[++cb + 1];
                            nextCbBoundary = (nextCbBoundary << 2) - nextCbBoundary;

                            cbBegin = _SfBandIndex[_Sfreq].S[cb];
                            cbWidth = _SfBandIndex[_Sfreq].S[cb + 1] - cbBegin;
                            cbBegin = (cbBegin << 2) - cbBegin;
                        }
                    }
                    else {
                        // long blocks

                        nextCbBoundary = _SfBandIndex[_Sfreq].L[++cb + 1];
                    }
                }

                // Do long/short dependent scaling operations

                if (grInfo.WindowSwitchingFlag != 0 &&
                    (grInfo.BlockType == 2 && grInfo.MixedBlockFlag == 0 ||
                     grInfo.BlockType == 2 && grInfo.MixedBlockFlag != 0 && j >= 36)) {
                    int tIndex = (index - cbBegin) / cbWidth;
                    /*            xr[sb][ss] *= pow(2.0, ((-2.0 * gr_info.subblock_gain[t_index])
                    -(0.5 * (1.0 + gr_info.scalefac_scale)
                    * scalefac[ch].s[t_index][cb]))); */
                    int idx = _Scalefac[ch].S[tIndex][cb] << grInfo.ScaleFacScale;
                    idx += grInfo.SubblockGain[tIndex] << 2;

                    xr1D[quotien][reste] *= TwoToNegativeHalfPow[idx];
                }
                else {
                    // LONG block types 0,1,3 & 1st 2 subbands of switched blocks
                    /* xr[sb][ss] *= pow(2.0, -0.5 * (1.0+gr_info.scalefac_scale)
                    * (scalefac[ch].l[cb]
                    + gr_info.preflag * pretab[cb])); */
                    int idx = _Scalefac[ch].L[cb];

                    if (grInfo.Preflag != 0)
                        idx += Pretab[cb];

                    idx = idx << grInfo.ScaleFacScale;
                    xr1D[quotien][reste] *= TwoToNegativeHalfPow[idx];
                }
                index++;
            }

            for (j = _Nonzero[ch]; j < 576; j++) {
                int reste = j % SSLIMIT;
                int quotien = (j - reste) / SSLIMIT;
                if (reste < 0)
                    reste = 0;
                if (quotien < 0)
                    quotien = 0;
                xr1D[quotien][reste] = 0.0f;
            }
        }

        /// <summary>
        /// *
        /// </summary>
        private void Reorder(float[][] xr, int ch, int gr) {
            GranuleInfo grInfo = _SideInfo.Channels[ch].Granules[gr];
            int freq, freq3;
            int index;
            int sfb, sfbStart, sfbLines;
            int srcLine, desLine;
            float[][] xr1D = xr;

            if (grInfo.WindowSwitchingFlag != 0 && grInfo.BlockType == 2) {
                for (index = 0; index < 576; index++)
                    _Out1D[index] = 0.0f;

                if (grInfo.MixedBlockFlag != 0) {
                    // NO REORDER FOR LOW 2 SUBBANDS
                    for (index = 0; index < 36; index++) {
                        int reste = index % SSLIMIT;
                        int quotien = (index - reste) / SSLIMIT;
                        _Out1D[index] = xr1D[quotien][reste];
                    }
                    // REORDERING FOR REST SWITCHED SHORT
                    for (sfb = 3, sfbStart = _SfBandIndex[_Sfreq].S[3], sfbLines = _SfBandIndex[_Sfreq].S[4] - sfbStart;
                        sfb < 13;
                        sfb++, sfbStart = _SfBandIndex[_Sfreq].S[sfb],
                        sfbLines = _SfBandIndex[_Sfreq].S[sfb + 1] - sfbStart) {
                        int sfbStart3 = (sfbStart << 2) - sfbStart;

                        for (freq = 0, freq3 = 0; freq < sfbLines; freq++, freq3 += 3) {
                            srcLine = sfbStart3 + freq;
                            desLine = sfbStart3 + freq3;
                            int reste = srcLine % SSLIMIT;
                            int quotien = (srcLine - reste) / SSLIMIT;

                            _Out1D[desLine] = xr1D[quotien][reste];
                            srcLine += sfbLines;
                            desLine++;

                            reste = srcLine % SSLIMIT;
                            quotien = (srcLine - reste) / SSLIMIT;

                            _Out1D[desLine] = xr1D[quotien][reste];
                            srcLine += sfbLines;
                            desLine++;

                            reste = srcLine % SSLIMIT;
                            quotien = (srcLine - reste) / SSLIMIT;

                            _Out1D[desLine] = xr1D[quotien][reste];
                        }
                    }
                }
                else {
                    // pure short
                    for (index = 0; index < 576; index++) {
                        int j = _reorderTable[_Sfreq][index];
                        int reste = j % SSLIMIT;
                        int quotien = (j - reste) / SSLIMIT;
                        _Out1D[index] = xr1D[quotien][reste];
                    }
                }
            }
            else {
                // long blocks
                for (index = 0; index < 576; index++) {
                    int reste = index % SSLIMIT;
                    int quotien = (index - reste) / SSLIMIT;
                    _Out1D[index] = xr1D[quotien][reste];
                }
            }
        }

        private void Stereo(int gr) {
            int sb, ss;

            if (_Channels == 1) {
                // mono , bypass xr[0][][] to lr[0][][]

                for (sb = 0; sb < SBLIMIT; sb++)
                    for (ss = 0; ss < SSLIMIT; ss += 3) {
                        _Lr[0][sb][ss] = _Ro[0][sb][ss];
                        _Lr[0][sb][ss + 1] = _Ro[0][sb][ss + 1];
                        _Lr[0][sb][ss + 2] = _Ro[0][sb][ss + 2];
                    }
            }
            else {
                GranuleInfo grInfo = _SideInfo.Channels[0].Granules[gr];
                int modeExt = _Header.mode_extension();
                int sfb;
                int i;
                int lines, temp, temp2;

                bool msStereo = _Header.Mode() == Header.JOINT_STEREO && (modeExt & 0x2) != 0;
                bool iStereo = _Header.Mode() == Header.JOINT_STEREO && (modeExt & 0x1) != 0;
                bool lsf = _Header.Version() == Header.MPEG2_LSF || _Header.Version() == Header.MPEG25_LSF; // SZD

                int ioType = grInfo.ScaleFacCompress & 1;

                // initialization

                for (i = 0; i < 576; i++) {
                    IsPos[i] = 7;

                    IsRatio[i] = 0.0f;
                }

                if (iStereo) {
                    if (grInfo.WindowSwitchingFlag != 0 && grInfo.BlockType == 2) {
                        if (grInfo.MixedBlockFlag != 0) {
                            int maxSfb = 0;

                            for (int j = 0; j < 3; j++) {
                                int sfbcnt;
                                sfbcnt = 2;
                                for (sfb = 12; sfb >= 3; sfb--) {
                                    i = _SfBandIndex[_Sfreq].S[sfb];
                                    lines = _SfBandIndex[_Sfreq].S[sfb + 1] - i;
                                    i = (i << 2) - i + (j + 1) * lines - 1;

                                    while (lines > 0) {
                                        if (_Ro[1][i / 18][i % 18] != 0.0f) {
                                            // MDM: in java, array access is very slow.
                                            // Is quicker to compute div and mod values.
                                            //if (ro[1][ss_div[i]][ss_mod[i]] != 0.0f) {
                                            sfbcnt = sfb;
                                            sfb = -10;
                                            lines = -10;
                                        }

                                        lines--;
                                        i--;
                                    } // while (lines > 0)
                                }
                                // for (sfb=12 ...
                                sfb = sfbcnt + 1;

                                if (sfb > maxSfb)
                                    maxSfb = sfb;

                                while (sfb < 12) {
                                    temp = _SfBandIndex[_Sfreq].S[sfb];
                                    sb = _SfBandIndex[_Sfreq].S[sfb + 1] - temp;
                                    i = (temp << 2) - temp + j * sb;

                                    for (; sb > 0; sb--) {
                                        IsPos[i] = _Scalefac[1].S[j][sfb];
                                        if (IsPos[i] != 7)
                                            if (lsf)
                                                GetKStereoValues(IsPos[i], ioType, i);
                                            else
                                                IsRatio[i] = Tan12[IsPos[i]];

                                        i++;
                                    }
                                    // for (; sb>0...
                                    sfb++;
                                } // while (sfb < 12)
                                sfb = _SfBandIndex[_Sfreq].S[10];
                                sb = _SfBandIndex[_Sfreq].S[11] - sfb;
                                sfb = (sfb << 2) - sfb + j * sb;
                                temp = _SfBandIndex[_Sfreq].S[11];
                                sb = _SfBandIndex[_Sfreq].S[12] - temp;
                                i = (temp << 2) - temp + j * sb;

                                for (; sb > 0; sb--) {
                                    IsPos[i] = IsPos[sfb];

                                    if (lsf) {
                                        _K[0][i] = _K[0][sfb];
                                        _K[1][i] = _K[1][sfb];
                                    }
                                    else {
                                        IsRatio[i] = IsRatio[sfb];
                                    }
                                    i++;
                                }
                                // for (; sb > 0 ...
                            }
                            if (maxSfb <= 3) {
                                i = 2;
                                ss = 17;
                                sb = -1;
                                while (i >= 0) {
                                    if (_Ro[1][i][ss] != 0.0f) {
                                        sb = (i << 4) + (i << 1) + ss;
                                        i = -1;
                                    }
                                    else {
                                        ss--;
                                        if (ss < 0) {
                                            i--;
                                            ss = 17;
                                        }
                                    }
                                    // if (ro ...
                                } // while (i>=0)
                                i = 0;
                                while (_SfBandIndex[_Sfreq].L[i] <= sb)
                                    i++;
                                sfb = i;
                                i = _SfBandIndex[_Sfreq].L[i];
                                for (; sfb < 8; sfb++) {
                                    sb = _SfBandIndex[_Sfreq].L[sfb + 1] - _SfBandIndex[_Sfreq].L[sfb];
                                    for (; sb > 0; sb--) {
                                        IsPos[i] = _Scalefac[1].L[sfb];
                                        if (IsPos[i] != 7)
                                            if (lsf)
                                                GetKStereoValues(IsPos[i], ioType, i);
                                            else
                                                IsRatio[i] = Tan12[IsPos[i]];
                                        i++;
                                    }
                                    // for (; sb>0 ...
                                }
                                // for (; sfb<8 ...
                            }
                            // for (j=0 ...
                        }
                        else {
                            // if (gr_info.mixed_block_flag)
                            for (int j = 0; j < 3; j++) {
                                int sfbcnt;
                                sfbcnt = -1;
                                for (sfb = 12; sfb >= 0; sfb--) {
                                    temp = _SfBandIndex[_Sfreq].S[sfb];
                                    lines = _SfBandIndex[_Sfreq].S[sfb + 1] - temp;
                                    i = (temp << 2) - temp + (j + 1) * lines - 1;

                                    while (lines > 0) {
                                        if (_Ro[1][i / 18][i % 18] != 0.0f) {
                                            // MDM: in java, array access is very slow.
                                            // Is quicker to compute div and mod values.
                                            //if (ro[1][ss_div[i]][ss_mod[i]] != 0.0f) {
                                            sfbcnt = sfb;
                                            sfb = -10;
                                            lines = -10;
                                        }
                                        lines--;
                                        i--;
                                    } // while (lines > 0) */
                                }
                                // for (sfb=12 ...
                                sfb = sfbcnt + 1;
                                while (sfb < 12) {
                                    temp = _SfBandIndex[_Sfreq].S[sfb];
                                    sb = _SfBandIndex[_Sfreq].S[sfb + 1] - temp;
                                    i = (temp << 2) - temp + j * sb;
                                    for (; sb > 0; sb--) {
                                        IsPos[i] = _Scalefac[1].S[j][sfb];
                                        if (IsPos[i] != 7)
                                            if (lsf)
                                                GetKStereoValues(IsPos[i], ioType, i);
                                            else
                                                IsRatio[i] = Tan12[IsPos[i]];
                                        i++;
                                    }
                                    // for (; sb>0 ...
                                    sfb++;
                                } // while (sfb<12)

                                temp = _SfBandIndex[_Sfreq].S[10];
                                temp2 = _SfBandIndex[_Sfreq].S[11];
                                sb = temp2 - temp;
                                sfb = (temp << 2) - temp + j * sb;
                                sb = _SfBandIndex[_Sfreq].S[12] - temp2;
                                i = (temp2 << 2) - temp2 + j * sb;

                                for (; sb > 0; sb--) {
                                    IsPos[i] = IsPos[sfb];

                                    if (lsf) {
                                        _K[0][i] = _K[0][sfb];
                                        _K[1][i] = _K[1][sfb];
                                    }
                                    else {
                                        IsRatio[i] = IsRatio[sfb];
                                    }
                                    i++;
                                }
                                // for (; sb>0 ...
                            }
                            // for (sfb=12
                        }
                        // for (j=0 ...
                    }
                    else {
                        // if (gr_info.window_switching_flag ...
                        i = 31;
                        ss = 17;
                        sb = 0;
                        while (i >= 0) {
                            if (_Ro[1][i][ss] != 0.0f) {
                                sb = (i << 4) + (i << 1) + ss;
                                i = -1;
                            }
                            else {
                                ss--;
                                if (ss < 0) {
                                    i--;
                                    ss = 17;
                                }
                            }
                        }
                        i = 0;
                        while (_SfBandIndex[_Sfreq].L[i] <= sb)
                            i++;

                        sfb = i;
                        i = _SfBandIndex[_Sfreq].L[i];
                        for (; sfb < 21; sfb++) {
                            sb = _SfBandIndex[_Sfreq].L[sfb + 1] - _SfBandIndex[_Sfreq].L[sfb];
                            for (; sb > 0; sb--) {
                                IsPos[i] = _Scalefac[1].L[sfb];
                                if (IsPos[i] != 7)
                                    if (lsf)
                                        GetKStereoValues(IsPos[i], ioType, i);
                                    else
                                        IsRatio[i] = Tan12[IsPos[i]];
                                i++;
                            }
                        }
                        sfb = _SfBandIndex[_Sfreq].L[20];
                        for (sb = 576 - _SfBandIndex[_Sfreq].L[21]; sb > 0 && i < 576; sb--) {
                            IsPos[i] = IsPos[sfb]; // error here : i >=576

                            if (lsf) {
                                _K[0][i] = _K[0][sfb];
                                _K[1][i] = _K[1][sfb];
                            }
                            else {
                                IsRatio[i] = IsRatio[sfb];
                            }
                            i++;
                        }
                        // if (gr_info.mixed_block_flag)
                    }
                    // if (gr_info.window_switching_flag ...
                }
                // if (i_stereo)

                i = 0;
                for (sb = 0; sb < SBLIMIT; sb++)
                    for (ss = 0; ss < SSLIMIT; ss++) {
                        if (IsPos[i] == 7) {
                            if (msStereo) {
                                _Lr[0][sb][ss] = (_Ro[0][sb][ss] + _Ro[1][sb][ss]) * 0.707106781f;
                                _Lr[1][sb][ss] = (_Ro[0][sb][ss] - _Ro[1][sb][ss]) * 0.707106781f;
                            }
                            else {
                                _Lr[0][sb][ss] = _Ro[0][sb][ss];
                                _Lr[1][sb][ss] = _Ro[1][sb][ss];
                            }
                        }
                        else if (iStereo) {
                            if (lsf) {
                                _Lr[0][sb][ss] = _Ro[0][sb][ss] * _K[0][i];
                                _Lr[1][sb][ss] = _Ro[0][sb][ss] * _K[1][i];
                            }
                            else {
                                _Lr[1][sb][ss] = _Ro[0][sb][ss] / (1 + IsRatio[i]);
                                _Lr[0][sb][ss] = _Lr[1][sb][ss] * IsRatio[i];
                            }
                        }
                        /* else {
                        System.out.println("Error in stereo processing\n");
                        } */
                        i++;
                    }
            }
            // channels == 2
        }

        /// <summary>
        /// *
        /// </summary>
        private void Antialias(int ch, int gr) {
            int sb18, ss, sb18Lim;
            GranuleInfo grInfo = _SideInfo.Channels[ch].Granules[gr];
            // 31 alias-reduction operations between each pair of sub-bands
            // with 8 butterflies between each pair

            if (grInfo.WindowSwitchingFlag != 0 && grInfo.BlockType == 2 && !(grInfo.MixedBlockFlag != 0))
                return;

            if (grInfo.WindowSwitchingFlag != 0 && grInfo.MixedBlockFlag != 0 && grInfo.BlockType == 2) {
                sb18Lim = 18;
            }
            else {
                sb18Lim = 558;
            }

            for (sb18 = 0; sb18 < sb18Lim; sb18 += 18) {
                for (ss = 0; ss < 8; ss++) {
                    int srcIdx1 = sb18 + 17 - ss;
                    int srcIdx2 = sb18 + 18 + ss;
                    float bu = _Out1D[srcIdx1];
                    float bd = _Out1D[srcIdx2];
                    _Out1D[srcIdx1] = bu * Cs[ss] - bd * Ca[ss];
                    _Out1D[srcIdx2] = bd * Cs[ss] + bu * Ca[ss];
                }
            }
        }

        private void Hybrid(int ch, int gr) {
            int bt;
            int sb18;
            GranuleInfo grInfo = _SideInfo.Channels[ch].Granules[gr];
            float[] tsOut;

            float[][] prvblk;

            for (sb18 = 0; sb18 < 576; sb18 += 18) {
                bt = grInfo.WindowSwitchingFlag != 0 && grInfo.MixedBlockFlag != 0 && sb18 < 36 ? 0 : grInfo.BlockType;

                tsOut = _Out1D;
                for (int cc = 0; cc < 18; cc++)
                    TsOutCopy[cc] = tsOut[cc + sb18];
                InverseMdct(TsOutCopy, Rawout, bt);
                for (int cc = 0; cc < 18; cc++)
                    tsOut[cc + sb18] = TsOutCopy[cc];

                // overlap addition
                prvblk = _Prevblck;

                tsOut[0 + sb18] = Rawout[0] + prvblk[ch][sb18 + 0];
                prvblk[ch][sb18 + 0] = Rawout[18];
                tsOut[1 + sb18] = Rawout[1] + prvblk[ch][sb18 + 1];
                prvblk[ch][sb18 + 1] = Rawout[19];
                tsOut[2 + sb18] = Rawout[2] + prvblk[ch][sb18 + 2];
                prvblk[ch][sb18 + 2] = Rawout[20];
                tsOut[3 + sb18] = Rawout[3] + prvblk[ch][sb18 + 3];
                prvblk[ch][sb18 + 3] = Rawout[21];
                tsOut[4 + sb18] = Rawout[4] + prvblk[ch][sb18 + 4];
                prvblk[ch][sb18 + 4] = Rawout[22];
                tsOut[5 + sb18] = Rawout[5] + prvblk[ch][sb18 + 5];
                prvblk[ch][sb18 + 5] = Rawout[23];
                tsOut[6 + sb18] = Rawout[6] + prvblk[ch][sb18 + 6];
                prvblk[ch][sb18 + 6] = Rawout[24];
                tsOut[7 + sb18] = Rawout[7] + prvblk[ch][sb18 + 7];
                prvblk[ch][sb18 + 7] = Rawout[25];
                tsOut[8 + sb18] = Rawout[8] + prvblk[ch][sb18 + 8];
                prvblk[ch][sb18 + 8] = Rawout[26];
                tsOut[9 + sb18] = Rawout[9] + prvblk[ch][sb18 + 9];
                prvblk[ch][sb18 + 9] = Rawout[27];
                tsOut[10 + sb18] = Rawout[10] + prvblk[ch][sb18 + 10];
                prvblk[ch][sb18 + 10] = Rawout[28];
                tsOut[11 + sb18] = Rawout[11] + prvblk[ch][sb18 + 11];
                prvblk[ch][sb18 + 11] = Rawout[29];
                tsOut[12 + sb18] = Rawout[12] + prvblk[ch][sb18 + 12];
                prvblk[ch][sb18 + 12] = Rawout[30];
                tsOut[13 + sb18] = Rawout[13] + prvblk[ch][sb18 + 13];
                prvblk[ch][sb18 + 13] = Rawout[31];
                tsOut[14 + sb18] = Rawout[14] + prvblk[ch][sb18 + 14];
                prvblk[ch][sb18 + 14] = Rawout[32];
                tsOut[15 + sb18] = Rawout[15] + prvblk[ch][sb18 + 15];
                prvblk[ch][sb18 + 15] = Rawout[33];
                tsOut[16 + sb18] = Rawout[16] + prvblk[ch][sb18 + 16];
                prvblk[ch][sb18 + 16] = Rawout[34];
                tsOut[17 + sb18] = Rawout[17] + prvblk[ch][sb18 + 17];
                prvblk[ch][sb18 + 17] = Rawout[35];
            }
        }

        /// <summary>
        /// *
        /// </summary>
        private void DoDownMix() {
            for (int sb = 0; sb < SSLIMIT; sb++) {
                for (int ss = 0; ss < SSLIMIT; ss += 3) {
                    _Lr[0][sb][ss] = (_Lr[0][sb][ss] + _Lr[1][sb][ss]) * 0.5f;
                    _Lr[0][sb][ss + 1] = (_Lr[0][sb][ss + 1] + _Lr[1][sb][ss + 1]) * 0.5f;
                    _Lr[0][sb][ss + 2] = (_Lr[0][sb][ss + 2] + _Lr[1][sb][ss + 2]) * 0.5f;
                }
            }
        }

        /// <summary>
        /// Fast Inverse Modified discrete cosine transform.
        /// </summary>
        internal void InverseMdct(float[] inValues, float[] outValues, int blockType) {
            float tmpf0, tmpf1, tmpf2, tmpf3, tmpf4, tmpf5, tmpf6, tmpf7, tmpf8, tmpf9;
            float tmpf10, tmpf11, tmpf12, tmpf13, tmpf14, tmpf15, tmpf16, tmpf17;

            if (blockType == 2) {
                /*
                *
                * Under MicrosoftVM 2922, This causes a GPF, or
                * At best, an ArrayIndexOutOfBoundsExceptin.
                for(int p=0;p<36;p+=9)
                {
                out[p]   = out[p+1] = out[p+2] = out[p+3] =
                out[p+4] = out[p+5] = out[p+6] = out[p+7] =
                out[p+8] = 0.0f;
                }
                */
                outValues[0] = 0.0f;
                outValues[1] = 0.0f;
                outValues[2] = 0.0f;
                outValues[3] = 0.0f;
                outValues[4] = 0.0f;
                outValues[5] = 0.0f;
                outValues[6] = 0.0f;
                outValues[7] = 0.0f;
                outValues[8] = 0.0f;
                outValues[9] = 0.0f;
                outValues[10] = 0.0f;
                outValues[11] = 0.0f;
                outValues[12] = 0.0f;
                outValues[13] = 0.0f;
                outValues[14] = 0.0f;
                outValues[15] = 0.0f;
                outValues[16] = 0.0f;
                outValues[17] = 0.0f;
                outValues[18] = 0.0f;
                outValues[19] = 0.0f;
                outValues[20] = 0.0f;
                outValues[21] = 0.0f;
                outValues[22] = 0.0f;
                outValues[23] = 0.0f;
                outValues[24] = 0.0f;
                outValues[25] = 0.0f;
                outValues[26] = 0.0f;
                outValues[27] = 0.0f;
                outValues[28] = 0.0f;
                outValues[29] = 0.0f;
                outValues[30] = 0.0f;
                outValues[31] = 0.0f;
                outValues[32] = 0.0f;
                outValues[33] = 0.0f;
                outValues[34] = 0.0f;
                outValues[35] = 0.0f;

                int sixI = 0;

                int i;
                for (i = 0; i < 3; i++) {
                    // 12 point IMDCT
                    // Begin 12 point IDCT
                    // Input aliasing for 12 pt IDCT
                    inValues[15 + i] += inValues[12 + i];
                    inValues[12 + i] += inValues[9 + i];
                    inValues[9 + i] += inValues[6 + i];
                    inValues[6 + i] += inValues[3 + i];
                    inValues[3 + i] += inValues[0 + i];

                    // Input aliasing on odd indices (for 6 point IDCT)
                    inValues[15 + i] += inValues[9 + i];
                    inValues[9 + i] += inValues[3 + i];

                    // 3 point IDCT on even indices
                    float pp1, pp2, sum;
                    pp2 = inValues[12 + i] * 0.500000000f;
                    pp1 = inValues[6 + i] * 0.866025403f;
                    sum = inValues[0 + i] + pp2;
                    tmpf1 = inValues[0 + i] - inValues[12 + i];
                    tmpf0 = sum + pp1;
                    tmpf2 = sum - pp1;

                    // End 3 point IDCT on even indices
                    // 3 point IDCT on odd indices (for 6 point IDCT)
                    pp2 = inValues[15 + i] * 0.500000000f;
                    pp1 = inValues[9 + i] * 0.866025403f;
                    sum = inValues[3 + i] + pp2;
                    tmpf4 = inValues[3 + i] - inValues[15 + i];
                    tmpf5 = sum + pp1;
                    tmpf3 = sum - pp1;
                    // End 3 point IDCT on odd indices
                    // Twiddle factors on odd indices (for 6 point IDCT)

                    tmpf3 *= 1.931851653f;
                    tmpf4 *= 0.707106781f;
                    tmpf5 *= 0.517638090f;

                    // Output butterflies on 2 3 point IDCT's (for 6 point IDCT)
                    float save = tmpf0;
                    tmpf0 += tmpf5;
                    tmpf5 = save - tmpf5;
                    save = tmpf1;
                    tmpf1 += tmpf4;
                    tmpf4 = save - tmpf4;
                    save = tmpf2;
                    tmpf2 += tmpf3;
                    tmpf3 = save - tmpf3;

                    // End 6 point IDCT
                    // Twiddle factors on indices (for 12 point IDCT)

                    tmpf0 *= 0.504314480f;
                    tmpf1 *= 0.541196100f;
                    tmpf2 *= 0.630236207f;
                    tmpf3 *= 0.821339815f;
                    tmpf4 *= 1.306562965f;
                    tmpf5 *= 3.830648788f;

                    // End 12 point IDCT

                    // Shift to 12 point modified IDCT, multiply by window type 2
                    tmpf8 = -tmpf0 * 0.793353340f;
                    tmpf9 = -tmpf0 * 0.608761429f;
                    tmpf7 = -tmpf1 * 0.923879532f;
                    tmpf10 = -tmpf1 * 0.382683432f;
                    tmpf6 = -tmpf2 * 0.991444861f;
                    tmpf11 = -tmpf2 * 0.130526192f;

                    tmpf0 = tmpf3;
                    tmpf1 = tmpf4 * 0.382683432f;
                    tmpf2 = tmpf5 * 0.608761429f;

                    tmpf3 = -tmpf5 * 0.793353340f;
                    tmpf4 = -tmpf4 * 0.923879532f;
                    tmpf5 = -tmpf0 * 0.991444861f;

                    tmpf0 *= 0.130526192f;

                    outValues[sixI + 6] += tmpf0;
                    outValues[sixI + 7] += tmpf1;
                    outValues[sixI + 8] += tmpf2;
                    outValues[sixI + 9] += tmpf3;
                    outValues[sixI + 10] += tmpf4;
                    outValues[sixI + 11] += tmpf5;
                    outValues[sixI + 12] += tmpf6;
                    outValues[sixI + 13] += tmpf7;
                    outValues[sixI + 14] += tmpf8;
                    outValues[sixI + 15] += tmpf9;
                    outValues[sixI + 16] += tmpf10;
                    outValues[sixI + 17] += tmpf11;

                    sixI += 6;
                }
            }
            else {
                // 36 point IDCT
                // input aliasing for 36 point IDCT
                inValues[17] += inValues[16];
                inValues[16] += inValues[15];
                inValues[15] += inValues[14];
                inValues[14] += inValues[13];
                inValues[13] += inValues[12];
                inValues[12] += inValues[11];
                inValues[11] += inValues[10];
                inValues[10] += inValues[9];
                inValues[9] += inValues[8];
                inValues[8] += inValues[7];
                inValues[7] += inValues[6];
                inValues[6] += inValues[5];
                inValues[5] += inValues[4];
                inValues[4] += inValues[3];
                inValues[3] += inValues[2];
                inValues[2] += inValues[1];
                inValues[1] += inValues[0];

                // 18 point IDCT for odd indices
                // input aliasing for 18 point IDCT
                inValues[17] += inValues[15];
                inValues[15] += inValues[13];
                inValues[13] += inValues[11];
                inValues[11] += inValues[9];
                inValues[9] += inValues[7];
                inValues[7] += inValues[5];
                inValues[5] += inValues[3];
                inValues[3] += inValues[1];

                float tmp0, tmp1, tmp2, tmp3, tmp4, tmp0X, tmp1X, tmp2X, tmp3X;
                float tmp0O, tmp1O, tmp2O, tmp3O, tmp4O, tmp0Xo, tmp1Xo, tmp2Xo, tmp3Xo;

                // Fast 9 Point Inverse Discrete Cosine Transform
                //
                // By  Francois-Raymond Boyer
                //         mailto:boyerf@iro.umontreal.ca
                //         http://www.iro.umontreal.ca/~boyerf
                //
                // The code has been optimized for Intel processors
                //  (takes a lot of time to convert float to and from iternal FPU representation)
                //
                // It is a simple "factorization" of the IDCT matrix.

                // 9 point IDCT on even indices

                // 5 points on odd indices (not realy an IDCT)
                float i00 = inValues[0] + inValues[0];
                float iip12 = i00 + inValues[12];

                tmp0 = iip12 + inValues[4] * 1.8793852415718f + inValues[8] * 1.532088886238f +
                       inValues[16] * 0.34729635533386f;
                tmp1 = i00 + inValues[4] - inValues[8] - inValues[12] - inValues[12] - inValues[16];
                tmp2 = iip12 - inValues[4] * 0.34729635533386f - inValues[8] * 1.8793852415718f +
                       inValues[16] * 1.532088886238f;
                tmp3 = iip12 - inValues[4] * 1.532088886238f + inValues[8] * 0.34729635533386f -
                       inValues[16] * 1.8793852415718f;
                tmp4 = inValues[0] - inValues[4] + inValues[8] - inValues[12] + inValues[16];

                // 4 points on even indices
                float i66 = inValues[6] * 1.732050808f; // Sqrt[3]

                tmp0X = inValues[2] * 1.9696155060244f + i66 + inValues[10] * 1.2855752193731f +
                        inValues[14] * 0.68404028665134f;
                tmp1X = (inValues[2] - inValues[10] - inValues[14]) * 1.732050808f;
                tmp2X = inValues[2] * 1.2855752193731f - i66 - inValues[10] * 0.68404028665134f +
                        inValues[14] * 1.9696155060244f;
                tmp3X = inValues[2] * 0.68404028665134f - i66 + inValues[10] * 1.9696155060244f -
                        inValues[14] * 1.2855752193731f;

                // 9 point IDCT on odd indices
                // 5 points on odd indices (not realy an IDCT)
                float i0 = inValues[0 + 1] + inValues[0 + 1];
                float i0P12 = i0 + inValues[12 + 1];

                tmp0O = i0P12 + inValues[4 + 1] * 1.8793852415718f + inValues[8 + 1] * 1.532088886238f +
                        inValues[16 + 1] * 0.34729635533386f;
                tmp1O = i0 + inValues[4 + 1] - inValues[8 + 1] - inValues[12 + 1] - inValues[12 + 1] -
                        inValues[16 + 1];
                tmp2O = i0P12 - inValues[4 + 1] * 0.34729635533386f - inValues[8 + 1] * 1.8793852415718f +
                        inValues[16 + 1] * 1.532088886238f;
                tmp3O = i0P12 - inValues[4 + 1] * 1.532088886238f + inValues[8 + 1] * 0.34729635533386f -
                        inValues[16 + 1] * 1.8793852415718f;
                tmp4O = (inValues[0 + 1] - inValues[4 + 1] + inValues[8 + 1] - inValues[12 + 1] +
                         inValues[16 + 1]) * 0.707106781f; // Twiddled

                // 4 points on even indices
                float i6 = inValues[6 + 1] * 1.732050808f; // Sqrt[3]

                tmp0Xo = inValues[2 + 1] * 1.9696155060244f + i6 + inValues[10 + 1] * 1.2855752193731f +
                         inValues[14 + 1] * 0.68404028665134f;
                tmp1Xo = (inValues[2 + 1] - inValues[10 + 1] - inValues[14 + 1]) * 1.732050808f;
                tmp2Xo = inValues[2 + 1] * 1.2855752193731f - i6 - inValues[10 + 1] * 0.68404028665134f +
                         inValues[14 + 1] * 1.9696155060244f;
                tmp3Xo = inValues[2 + 1] * 0.68404028665134f - i6 + inValues[10 + 1] * 1.9696155060244f -
                         inValues[14 + 1] * 1.2855752193731f;

                // Twiddle factors on odd indices
                // and
                // Butterflies on 9 point IDCT's
                // and
                // twiddle factors for 36 point IDCT

                float e, o;
                e = tmp0 + tmp0X;
                o = (tmp0O + tmp0Xo) * 0.501909918f;
                tmpf0 = e + o;
                tmpf17 = e - o;
                e = tmp1 + tmp1X;
                o = (tmp1O + tmp1Xo) * 0.517638090f;
                tmpf1 = e + o;
                tmpf16 = e - o;
                e = tmp2 + tmp2X;
                o = (tmp2O + tmp2Xo) * 0.551688959f;
                tmpf2 = e + o;
                tmpf15 = e - o;
                e = tmp3 + tmp3X;
                o = (tmp3O + tmp3Xo) * 0.610387294f;
                tmpf3 = e + o;
                tmpf14 = e - o;
                tmpf4 = tmp4 + tmp4O;
                tmpf13 = tmp4 - tmp4O;
                e = tmp3 - tmp3X;
                o = (tmp3O - tmp3Xo) * 0.871723397f;
                tmpf5 = e + o;
                tmpf12 = e - o;
                e = tmp2 - tmp2X;
                o = (tmp2O - tmp2Xo) * 1.183100792f;
                tmpf6 = e + o;
                tmpf11 = e - o;
                e = tmp1 - tmp1X;
                o = (tmp1O - tmp1Xo) * 1.931851653f;
                tmpf7 = e + o;
                tmpf10 = e - o;
                e = tmp0 - tmp0X;
                o = (tmp0O - tmp0Xo) * 5.736856623f;
                tmpf8 = e + o;
                tmpf9 = e - o;

                // end 36 point IDCT */
                // shift to modified IDCT
                float[] winBt = Win[blockType];

                outValues[0] = -tmpf9 * winBt[0];
                outValues[1] = -tmpf10 * winBt[1];
                outValues[2] = -tmpf11 * winBt[2];
                outValues[3] = -tmpf12 * winBt[3];
                outValues[4] = -tmpf13 * winBt[4];
                outValues[5] = -tmpf14 * winBt[5];
                outValues[6] = -tmpf15 * winBt[6];
                outValues[7] = -tmpf16 * winBt[7];
                outValues[8] = -tmpf17 * winBt[8];
                outValues[9] = tmpf17 * winBt[9];
                outValues[10] = tmpf16 * winBt[10];
                outValues[11] = tmpf15 * winBt[11];
                outValues[12] = tmpf14 * winBt[12];
                outValues[13] = tmpf13 * winBt[13];
                outValues[14] = tmpf12 * winBt[14];
                outValues[15] = tmpf11 * winBt[15];
                outValues[16] = tmpf10 * winBt[16];
                outValues[17] = tmpf9 * winBt[17];
                outValues[18] = tmpf8 * winBt[18];
                outValues[19] = tmpf7 * winBt[19];
                outValues[20] = tmpf6 * winBt[20];
                outValues[21] = tmpf5 * winBt[21];
                outValues[22] = tmpf4 * winBt[22];
                outValues[23] = tmpf3 * winBt[23];
                outValues[24] = tmpf2 * winBt[24];
                outValues[25] = tmpf1 * winBt[25];
                outValues[26] = tmpf0 * winBt[26];
                outValues[27] = tmpf0 * winBt[27];
                outValues[28] = tmpf1 * winBt[28];
                outValues[29] = tmpf2 * winBt[29];
                outValues[30] = tmpf3 * winBt[30];
                outValues[31] = tmpf4 * winBt[31];
                outValues[32] = tmpf5 * winBt[32];
                outValues[33] = tmpf6 * winBt[33];
                outValues[34] = tmpf7 * winBt[34];
                outValues[35] = tmpf8 * winBt[35];
            }
        }

        private static float[] CreatePowerTable() {
            float[] powerTable = new float[8192];
            double d43 = 4.0 / 3.0;
            for (int i = 0; i < 8192; i++) {
                powerTable[i] = (float)Math.Pow(i, d43);
            }
            return powerTable;
        }

        internal static int[] Reorder(int[] scalefacBand) {
            // SZD: converted from LAME
            int j = 0;
            int[] ix = new int[576];
            for (int sfb = 0; sfb < 13; sfb++) {
                int start = scalefacBand[sfb];
                int end = scalefacBand[sfb + 1];
                for (int window = 0; window < 3; window++)
                    for (int i = start; i < end; i++)
                        ix[3 * i + window] = j++;
            }
            return ix;
        }
    }

}

namespace RitaEngine.Resources.Sound.MP3Sharp.Decoding.Decoders.LayerI {
    /// <summary>
    /// public class for layer I subbands in single channel mode.
    /// Used for single channel mode
    /// and in derived class for intensity stereo mode
    /// </summary>
    public class SubbandLayer1 : ASubband {
        // Factors and offsets for sample requantization
        internal static readonly float[] TableFactor = {
            0.0f, 1.0f / 2.0f * (4.0f / 3.0f), 1.0f / 4.0f * (8.0f / 7.0f), 1.0f / 8.0f * (16.0f / 15.0f),
            1.0f / 16.0f * (32.0f / 31.0f), 1.0f / 32.0f * (64.0f / 63.0f), 1.0f / 64.0f * (128.0f / 127.0f),
            1.0f / 128.0f * (256.0f / 255.0f), 1.0f / 256.0f * (512.0f / 511.0f), 1.0f / 512.0f * (1024.0f / 1023.0f),
            1.0f / 1024.0f * (2048.0f / 2047.0f), 1.0f / 2048.0f * (4096.0f / 4095.0f), 1.0f / 4096.0f * (8192.0f / 8191.0f),
            1.0f / 8192.0f * (16384.0f / 16383.0f), 1.0f / 16384.0f * (32768.0f / 32767.0f)
        };

        internal static readonly float[] TableOffset = {
            0.0f, (1.0f / 2.0f - 1.0f) * (4.0f / 3.0f), (1.0f / 4.0f - 1.0f) * (8.0f / 7.0f),
            (1.0f / 8.0f - 1.0f) * (16.0f / 15.0f), (1.0f / 16.0f - 1.0f) * (32.0f / 31.0f),
            (1.0f / 32.0f - 1.0f) * (64.0f / 63.0f), (1.0f / 64.0f - 1.0f) * (128.0f / 127.0f),
            (1.0f / 128.0f - 1.0f) * (256.0f / 255.0f), (1.0f / 256.0f - 1.0f) * (512.0f / 511.0f),
            (1.0f / 512.0f - 1.0f) * (1024.0f / 1023.0f), (1.0f / 1024.0f - 1.0f) * (2048.0f / 2047.0f),
            (1.0f / 2048.0f - 1.0f) * (4096.0f / 4095.0f), (1.0f / 4096.0f - 1.0f) * (8192.0f / 8191.0f),
            (1.0f / 8192.0f - 1.0f) * (16384.0f / 16383.0f), (1.0f / 16384.0f - 1.0f) * (32768.0f / 32767.0f)
        };

        protected int Allocation;
        protected float Factor, Offset;
        protected float Sample;
        protected int Samplelength;
        protected int Samplenumber;
        protected float Scalefactor;
        protected readonly int Subbandnumber;

        /// <summary>
        /// Construtor.
        /// </summary>
        internal SubbandLayer1(int subbandnumber) {
            Subbandnumber = subbandnumber;
            Samplenumber = 0;
        }

        /// <summary>
        /// *
        /// </summary>
        internal override void ReadAllocation(Bitstream stream, Header header, Crc16 crc) {
            if ((Allocation = stream.GetBitsFromBuffer(4)) == 15) { }
            // cerr << "WARNING: stream contains an illegal allocation!\n";
            // MPEG-stream is corrupted!
            crc?.AddBits(Allocation, 4);
            if (Allocation != 0) {
                Samplelength = Allocation + 1;
                Factor = TableFactor[Allocation];
                Offset = TableOffset[Allocation];
            }
        }

        /// <summary>
        /// *
        /// </summary>
        internal override void ReadScaleFactor(Bitstream stream, Header header) {
            if (Allocation != 0)
                Scalefactor = ScaleFactors[stream.GetBitsFromBuffer(6)];
        }

        /// <summary>
        /// *
        /// </summary>
        internal override bool ReadSampleData(Bitstream stream) {
            if (Allocation != 0) {
                Sample = stream.GetBitsFromBuffer(Samplelength);
            }
            if (++Samplenumber == 12) {
                Samplenumber = 0;
                return true;
            }
            return false;
        }

        /// <summary>
        /// *
        /// </summary>
        internal override bool PutNextSample(int channels, SynthesisFilter filter1, SynthesisFilter filter2) {
            if (Allocation != 0 && channels != OutputChannels.RIGHT_CHANNEL) {
                float scaledSample = (Sample * Factor + Offset) * Scalefactor;
                filter1.AddSample(scaledSample, Subbandnumber);
            }
            return true;
        }
    }

    /// <summary>
    /// public class for layer I subbands in joint stereo mode.
    /// </summary>
    public class SubbandLayer1IntensityStereo : SubbandLayer1 {
        protected float Channel2Scalefactor;

        internal SubbandLayer1IntensityStereo(int subbandnumber)
            : base(subbandnumber) { }

        /// <summary>
        /// *
        /// </summary>
        internal override void ReadScaleFactor(Bitstream stream, Header header) {
            if (Allocation != 0) {
                Scalefactor = ScaleFactors[stream.GetBitsFromBuffer(6)];
                Channel2Scalefactor = ScaleFactors[stream.GetBitsFromBuffer(6)];
            }
        }

        /// <summary>
        /// *
        /// </summary>
        internal override bool PutNextSample(int channels, SynthesisFilter filter1, SynthesisFilter filter2) {
            if (Allocation != 0) {
                Sample = Sample * Factor + Offset; // requantization
                if (channels == OutputChannels.BOTH_CHANNELS) {
                    float sample1 = Sample * Scalefactor, sample2 = Sample * Channel2Scalefactor;
                    filter1.AddSample(sample1, Subbandnumber);
                    filter2.AddSample(sample2, Subbandnumber);
                }
                else if (channels == OutputChannels.LEFT_CHANNEL) {
                    float sample1 = Sample * Scalefactor;
                    filter1.AddSample(sample1, Subbandnumber);
                }
                else {
                    float sample2 = Sample * Channel2Scalefactor;
                    filter1.AddSample(sample2, Subbandnumber);
                }
            }
            return true;
        }
    }

    /// <summary>
    /// public class for layer I subbands in stereo mode.
    /// </summary>
    public class SubbandLayer1Stereo : SubbandLayer1 {
        protected int Channel2Allocation;
        protected float Channel2Factor, Channel2Offset;
        protected float Channel2Sample;
        protected int Channel2Samplelength;
        protected float Channel2Scalefactor;

        internal SubbandLayer1Stereo(int subbandnumber)
            : base(subbandnumber) { }

        /// <summary>
        /// *
        /// </summary>
        internal override void ReadAllocation(Bitstream stream, Header header, Crc16 crc) {
            Allocation = stream.GetBitsFromBuffer(4);
            if (Allocation > 14) {
                return;
            }
            Channel2Allocation = stream.GetBitsFromBuffer(4);
            if (crc != null) {
                crc.AddBits(Allocation, 4);
                crc.AddBits(Channel2Allocation, 4);
            }
            if (Allocation != 0) {
                Samplelength = Allocation + 1;
                Factor = TableFactor[Allocation];
                Offset = TableOffset[Allocation];
            }
            if (Channel2Allocation != 0) {
                Channel2Samplelength = Channel2Allocation + 1;
                Channel2Factor = TableFactor[Channel2Allocation];
                Channel2Offset = TableOffset[Channel2Allocation];
            }
        }

        /// <summary>
        /// *
        /// </summary>
        internal override void ReadScaleFactor(Bitstream stream, Header header) {
            if (Allocation != 0)
                Scalefactor = ScaleFactors[stream.GetBitsFromBuffer(6)];
            if (Channel2Allocation != 0)
                Channel2Scalefactor = ScaleFactors[stream.GetBitsFromBuffer(6)];
        }

        /// <summary>
        /// *
        /// </summary>
        internal override bool ReadSampleData(Bitstream stream) {
            bool returnvalue = base.ReadSampleData(stream);
            if (Channel2Allocation != 0) {
                Channel2Sample = stream.GetBitsFromBuffer(Channel2Samplelength);
            }
            return returnvalue;
        }

        /// <summary>
        /// *
        /// </summary>
        internal override bool PutNextSample(int channels, SynthesisFilter filter1, SynthesisFilter filter2) {
            base.PutNextSample(channels, filter1, filter2);
            if (Channel2Allocation != 0 && channels != OutputChannels.LEFT_CHANNEL) {
                float sample2 = (Channel2Sample * Channel2Factor + Channel2Offset) * Channel2Scalefactor;
                if (channels == OutputChannels.BOTH_CHANNELS)
                    filter2.AddSample(sample2, Subbandnumber);
                else
                    filter1.AddSample(sample2, Subbandnumber);
            }
            return true;
        }
    }


}

namespace RitaEngine.Resources.Sound.MP3Sharp.Decoding.Decoders.LayerII {
    /// <summary>
    /// public class for layer II subbands in single channel mode.
    /// </summary>
    public class SubbandLayer2 : ASubband {
        // this table contains 3 requantized samples for each legal codeword
        // when grouped in 5 bits, i.e. 3 quantization steps per sample
        internal static readonly float[] Grouping5Bits = {
            -2.0f / 3.0f, -2.0f / 3.0f, -2.0f / 3.0f, 0.0f, -2.0f / 3.0f, -2.0f / 3.0f, 2.0f / 3.0f, -2.0f / 3.0f, -2.0f / 3.0f,
            -2.0f / 3.0f, 0.0f, -2.0f / 3.0f, 0.0f, 0.0f, -2.0f / 3.0f, 2.0f / 3.0f, 0.0f, -2.0f / 3.0f, -2.0f / 3.0f, 2.0f / 3.0f,
            -2.0f / 3.0f, 0.0f, 2.0f / 3.0f, -2.0f / 3.0f, 2.0f / 3.0f, 2.0f / 3.0f, -2.0f / 3.0f, -2.0f / 3.0f, -2.0f / 3.0f, 0.0f,
            0.0f, -2.0f / 3.0f, 0.0f, 2.0f / 3.0f, -2.0f / 3.0f, 0.0f, -2.0f / 3.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 2.0f / 3.0f,
            0.0f, 0.0f, -2.0f / 3.0f, 2.0f / 3.0f, 0.0f, 0.0f, 2.0f / 3.0f, 0.0f, 2.0f / 3.0f, 2.0f / 3.0f, 0.0f, -2.0f / 3.0f,
            -2.0f / 3.0f, 2.0f / 3.0f, 0.0f, -2.0f / 3.0f, 2.0f / 3.0f, 2.0f / 3.0f, -2.0f / 3.0f, 2.0f / 3.0f, -2.0f / 3.0f, 0.0f,
            2.0f / 3.0f, 0.0f, 0.0f, 2.0f / 3.0f, 2.0f / 3.0f, 0.0f, 2.0f / 3.0f, -2.0f / 3.0f, 2.0f / 3.0f, 2.0f / 3.0f, 0.0f,
            2.0f / 3.0f, 2.0f / 3.0f, 2.0f / 3.0f, 2.0f / 3.0f, 2.0f / 3.0f
        };

        // this table contains 3 requantized samples for each legal codeword
        // when grouped in 7 bits, i.e. 5 quantizationsteps per sample
        internal static readonly float[] Grouping7Bits = {
            -0.8f, -0.8f, -0.8f, -0.4f, -0.8f, -0.8f, 0.0f, -0.8f, -0.8f, 0.4f, -0.8f, -0.8f, 0.8f, -0.8f, -0.8f,
            -0.8f, -0.4f, -0.8f, -0.4f, -0.4f, -0.8f, 0.0f, -0.4f, -0.8f, 0.4f, -0.4f, -0.8f, 0.8f, -0.4f, -0.8f,
            -0.8f, 0.0f, -0.8f, -0.4f, 0.0f, -0.8f, 0.0f, 0.0f, -0.8f, 0.4f, 0.0f, -0.8f, 0.8f, 0.0f, -0.8f, -0.8f,
            0.4f, -0.8f, -0.4f, 0.4f, -0.8f, 0.0f, 0.4f, -0.8f, 0.4f, 0.4f, -0.8f, 0.8f, 0.4f, -0.8f, -0.8f, 0.8f,
            -0.8f, -0.4f, 0.8f, -0.8f, 0.0f, 0.8f, -0.8f, 0.4f, 0.8f, -0.8f, 0.8f, 0.8f, -0.8f, -0.8f, -0.8f, -0.4f,
            -0.4f, -0.8f, -0.4f, 0.0f, -0.8f, -0.4f, 0.4f, -0.8f, -0.4f, 0.8f, -0.8f, -0.4f, -0.8f, -0.4f, -0.4f,
            -0.4f, -0.4f, -0.4f, 0.0f, -0.4f, -0.4f, 0.4f, -0.4f, -0.4f, 0.8f, -0.4f, -0.4f, -0.8f, 0.0f, -0.4f,
            -0.4f, 0.0f, -0.4f, 0.0f, 0.0f, -0.4f, 0.4f, 0.0f, -0.4f, 0.8f, 0.0f, -0.4f, -0.8f, 0.4f, -0.4f, -0.4f,
            0.4f, -0.4f, 0.0f, 0.4f, -0.4f, 0.4f, 0.4f, -0.4f, 0.8f, 0.4f, -0.4f, -0.8f, 0.8f, -0.4f, -0.4f, 0.8f,
            -0.4f, 0.0f, 0.8f, -0.4f, 0.4f, 0.8f, -0.4f, 0.8f, 0.8f, -0.4f, -0.8f, -0.8f, 0.0f, -0.4f, -0.8f, 0.0f,
            0.0f, -0.8f, 0.0f, 0.4f, -0.8f, 0.0f, 0.8f, -0.8f, 0.0f, -0.8f, -0.4f, 0.0f, -0.4f, -0.4f, 0.0f, 0.0f,
            -0.4f, 0.0f, 0.4f, -0.4f, 0.0f, 0.8f, -0.4f, 0.0f, -0.8f, 0.0f, 0.0f, -0.4f, 0.0f, 0.0f, 0.0f, 0.0f,
            0.0f, 0.4f, 0.0f, 0.0f, 0.8f, 0.0f, 0.0f, -0.8f, 0.4f, 0.0f, -0.4f, 0.4f, 0.0f, 0.0f, 0.4f, 0.0f, 0.4f,
            0.4f, 0.0f, 0.8f, 0.4f, 0.0f, -0.8f, 0.8f, 0.0f, -0.4f, 0.8f, 0.0f, 0.0f, 0.8f, 0.0f, 0.4f, 0.8f, 0.0f,
            0.8f, 0.8f, 0.0f, -0.8f, -0.8f, 0.4f, -0.4f, -0.8f, 0.4f, 0.0f, -0.8f, 0.4f, 0.4f, -0.8f, 0.4f, 0.8f,
            -0.8f, 0.4f, -0.8f, -0.4f, 0.4f, -0.4f, -0.4f, 0.4f, 0.0f, -0.4f, 0.4f, 0.4f, -0.4f, 0.4f, 0.8f, -0.4f,
            0.4f, -0.8f, 0.0f, 0.4f, -0.4f, 0.0f, 0.4f, 0.0f, 0.0f, 0.4f, 0.4f, 0.0f, 0.4f, 0.8f, 0.0f, 0.4f, -0.8f,
            0.4f, 0.4f, -0.4f, 0.4f, 0.4f, 0.0f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.8f, 0.4f, 0.4f,
            -0.8f, 0.8f, 0.4f, -0.4f, 0.8f, 0.4f, 0.0f, 0.8f, 0.4f, 0.4f, 0.8f, 0.4f, 0.8f, 0.8f, 0.4f, -0.8f, -0.8f,
            0.8f, -0.4f, -0.8f, 0.8f, 0.0f, -0.8f, 0.8f, 0.4f, -0.8f, 0.8f, 0.8f, -0.8f, 0.8f, -0.8f, -0.4f, 0.8f,
            -0.4f, -0.4f, 0.8f, 0.0f, -0.4f, 0.8f, 0.4f, -0.4f, 0.8f, 0.8f, -0.4f, 0.8f, -0.8f, 0.0f, 0.8f, -0.4f,
            0.0f, 0.8f, 0.0f, 0.0f, 0.8f, 0.4f, 0.0f, 0.8f, 0.8f, 0.0f, 0.8f, -0.8f, 0.4f, 0.8f, -0.4f, 0.4f, 0.8f,
            0.0f, 0.4f, 0.8f, 0.4f, 0.4f, 0.8f, 0.8f, 0.4f, 0.8f, -0.8f, 0.8f, 0.8f, -0.4f, 0.8f, 0.8f, 0.0f, 0.8f,
            0.8f, 0.4f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f
        };

        // this table contains 3 requantized samples for each legal codeword
        // when grouped in 10 bits, i.e. 9 quantizationsteps per sample
        internal static readonly float[] Grouping10Bits = {
            -8.0f / 9.0f, -8.0f / 9.0f, -8.0f / 9.0f, -6.0f / 9.0f, -8.0f / 9.0f, -8.0f / 9.0f, -4.0f / 9.0f, -8.0f / 9.0f,
            -8.0f / 9.0f, -2.0f / 9.0f, -8.0f / 9.0f, -8.0f / 9.0f, 0.0f, -8.0f / 9.0f, -8.0f / 9.0f, 2.0f / 9.0f, -8.0f / 9.0f,
            -8.0f / 9.0f, 4.0f / 9.0f, -8.0f / 9.0f, -8.0f / 9.0f, 6.0f / 9.0f, -8.0f / 9.0f, -8.0f / 9.0f, 8.0f / 9.0f, -8.0f / 9.0f,
            -8.0f / 9.0f, -8.0f / 9.0f, -6.0f / 9.0f, -8.0f / 9.0f, -6.0f / 9.0f, -6.0f / 9.0f, -8.0f / 9.0f, -4.0f / 9.0f,
            -6.0f / 9.0f, -8.0f / 9.0f, -2.0f / 9.0f, -6.0f / 9.0f, -8.0f / 9.0f, 0.0f, -6.0f / 9.0f, -8.0f / 9.0f, 2.0f / 9.0f,
            -6.0f / 9.0f, -8.0f / 9.0f, 4.0f / 9.0f, -6.0f / 9.0f, -8.0f / 9.0f, 6.0f / 9.0f, -6.0f / 9.0f, -8.0f / 9.0f, 8.0f / 9.0f,
            -6.0f / 9.0f, -8.0f / 9.0f, -8.0f / 9.0f, -4.0f / 9.0f, -8.0f / 9.0f, -6.0f / 9.0f, -4.0f / 9.0f, -8.0f / 9.0f,
            -4.0f / 9.0f, -4.0f / 9.0f, -8.0f / 9.0f, -2.0f / 9.0f, -4.0f / 9.0f, -8.0f / 9.0f, 0.0f, -4.0f / 9.0f, -8.0f / 9.0f,
            2.0f / 9.0f, -4.0f / 9.0f, -8.0f / 9.0f, 4.0f / 9.0f, -4.0f / 9.0f, -8.0f / 9.0f, 6.0f / 9.0f, -4.0f / 9.0f, -8.0f / 9.0f,
            8.0f / 9.0f, -4.0f / 9.0f, -8.0f / 9.0f, -8.0f / 9.0f, -2.0f / 9.0f, -8.0f / 9.0f, -6.0f / 9.0f, -2.0f / 9.0f,
            -8.0f / 9.0f, -4.0f / 9.0f, -2.0f / 9.0f, -8.0f / 9.0f, -2.0f / 9.0f, -2.0f / 9.0f, -8.0f / 9.0f, 0.0f, -2.0f / 9.0f,
            -8.0f / 9.0f, 2.0f / 9.0f, -2.0f / 9.0f, -8.0f / 9.0f, 4.0f / 9.0f, -2.0f / 9.0f, -8.0f / 9.0f, 6.0f / 9.0f, -2.0f / 9.0f,
            -8.0f / 9.0f, 8.0f / 9.0f, -2.0f / 9.0f, -8.0f / 9.0f, -8.0f / 9.0f, 0.0f, -8.0f / 9.0f, -6.0f / 9.0f, 0.0f,
            -8.0f / 9.0f, -4.0f / 9.0f, 0.0f, -8.0f / 9.0f, -2.0f / 9.0f, 0.0f, -8.0f / 9.0f, 0.0f, 0.0f, -8.0f / 9.0f,
            2.0f / 9.0f, 0.0f, -8.0f / 9.0f, 4.0f / 9.0f, 0.0f, -8.0f / 9.0f, 6.0f / 9.0f, 0.0f, -8.0f / 9.0f, 8.0f / 9.0f, 0.0f,
            -8.0f / 9.0f, -8.0f / 9.0f, 2.0f / 9.0f, -8.0f / 9.0f, -6.0f / 9.0f, 2.0f / 9.0f, -8.0f / 9.0f, -4.0f / 9.0f, 2.0f / 9.0f,
            -8.0f / 9.0f, -2.0f / 9.0f, 2.0f / 9.0f, -8.0f / 9.0f, 0.0f, 2.0f / 9.0f, -8.0f / 9.0f, 2.0f / 9.0f, 2.0f / 9.0f,
            -8.0f / 9.0f, 4.0f / 9.0f, 2.0f / 9.0f, -8.0f / 9.0f, 6.0f / 9.0f, 2.0f / 9.0f, -8.0f / 9.0f, 8.0f / 9.0f, 2.0f / 9.0f,
            -8.0f / 9.0f, -8.0f / 9.0f, 4.0f / 9.0f, -8.0f / 9.0f, -6.0f / 9.0f, 4.0f / 9.0f, -8.0f / 9.0f, -4.0f / 9.0f, 4.0f / 9.0f,
            -8.0f / 9.0f, -2.0f / 9.0f, 4.0f / 9.0f, -8.0f / 9.0f, 0.0f, 4.0f / 9.0f, -8.0f / 9.0f, 2.0f / 9.0f, 4.0f / 9.0f,
            -8.0f / 9.0f, 4.0f / 9.0f, 4.0f / 9.0f, -8.0f / 9.0f, 6.0f / 9.0f, 4.0f / 9.0f, -8.0f / 9.0f, 8.0f / 9.0f, 4.0f / 9.0f,
            -8.0f / 9.0f, -8.0f / 9.0f, 6.0f / 9.0f, -8.0f / 9.0f, -6.0f / 9.0f, 6.0f / 9.0f, -8.0f / 9.0f, -4.0f / 9.0f, 6.0f / 9.0f,
            -8.0f / 9.0f, -2.0f / 9.0f, 6.0f / 9.0f, -8.0f / 9.0f, 0.0f, 6.0f / 9.0f, -8.0f / 9.0f, 2.0f / 9.0f, 6.0f / 9.0f,
            -8.0f / 9.0f, 4.0f / 9.0f, 6.0f / 9.0f, -8.0f / 9.0f, 6.0f / 9.0f, 6.0f / 9.0f, -8.0f / 9.0f, 8.0f / 9.0f, 6.0f / 9.0f,
            -8.0f / 9.0f, -8.0f / 9.0f, 8.0f / 9.0f, -8.0f / 9.0f, -6.0f / 9.0f, 8.0f / 9.0f, -8.0f / 9.0f, -4.0f / 9.0f, 8.0f / 9.0f,
            -8.0f / 9.0f, -2.0f / 9.0f, 8.0f / 9.0f, -8.0f / 9.0f, 0.0f, 8.0f / 9.0f, -8.0f / 9.0f, 2.0f / 9.0f, 8.0f / 9.0f,
            -8.0f / 9.0f, 4.0f / 9.0f, 8.0f / 9.0f, -8.0f / 9.0f, 6.0f / 9.0f, 8.0f / 9.0f, -8.0f / 9.0f, 8.0f / 9.0f, 8.0f / 9.0f,
            -8.0f / 9.0f, -8.0f / 9.0f, -8.0f / 9.0f, -6.0f / 9.0f, -6.0f / 9.0f, -8.0f / 9.0f, -6.0f / 9.0f, -4.0f / 9.0f,
            -8.0f / 9.0f, -6.0f / 9.0f, -2.0f / 9.0f, -8.0f / 9.0f, -6.0f / 9.0f, 0.0f, -8.0f / 9.0f, -6.0f / 9.0f, 2.0f / 9.0f,
            -8.0f / 9.0f, -6.0f / 9.0f, 4.0f / 9.0f, -8.0f / 9.0f, -6.0f / 9.0f, 6.0f / 9.0f, -8.0f / 9.0f, -6.0f / 9.0f, 8.0f / 9.0f,
            -8.0f / 9.0f, -6.0f / 9.0f, -8.0f / 9.0f, -6.0f / 9.0f, -6.0f / 9.0f, -6.0f / 9.0f, -6.0f / 9.0f, -6.0f / 9.0f,
            -4.0f / 9.0f, -6.0f / 9.0f, -6.0f / 9.0f, -2.0f / 9.0f, -6.0f / 9.0f, -6.0f / 9.0f, 0.0f, -6.0f / 9.0f, -6.0f / 9.0f,
            2.0f / 9.0f, -6.0f / 9.0f, -6.0f / 9.0f,
            4.0f / 9.0f, -6.0f / 9.0f, -6.0f / 9.0f, 6.0f / 9.0f, -6.0f / 9.0f, -6.0f / 9.0f, 8.0f / 9.0f, -6.0f / 9.0f, -6.0f / 9.0f,
            -8.0f / 9.0f, -4.0f / 9.0f, -6.0f / 9.0f, -6.0f / 9.0f, -4.0f / 9.0f, -6.0f / 9.0f, -4.0f / 9.0f, -4.0f / 9.0f,
            -6.0f / 9.0f, -2.0f / 9.0f, -4.0f / 9.0f, -6.0f / 9.0f, 0.0f, -4.0f / 9.0f, -6.0f / 9.0f, 2.0f / 9.0f, -4.0f / 9.0f,
            -6.0f / 9.0f, 4.0f / 9.0f, -4.0f / 9.0f, -6.0f / 9.0f, 6.0f / 9.0f, -4.0f / 9.0f, -6.0f / 9.0f, 8.0f / 9.0f, -4.0f / 9.0f,
            -6.0f / 9.0f, -8.0f / 9.0f, -2.0f / 9.0f, -6.0f / 9.0f, -6.0f / 9.0f, -2.0f / 9.0f, -6.0f / 9.0f, -4.0f / 9.0f,
            -2.0f / 9.0f, -6.0f / 9.0f, -2.0f / 9.0f, -2.0f / 9.0f, -6.0f / 9.0f, 0.0f, -2.0f / 9.0f, -6.0f / 9.0f, 2.0f / 9.0f,
            -2.0f / 9.0f, -6.0f / 9.0f, 4.0f / 9.0f, -2.0f / 9.0f, -6.0f / 9.0f, 6.0f / 9.0f, -2.0f / 9.0f, -6.0f / 9.0f, 8.0f / 9.0f,
            -2.0f / 9.0f, -6.0f / 9.0f, -8.0f / 9.0f, 0.0f, -6.0f / 9.0f, -6.0f / 9.0f, 0.0f, -6.0f / 9.0f, -4.0f / 9.0f, 0.0f,
            -6.0f / 9.0f, -2.0f / 9.0f, 0.0f, -6.0f / 9.0f, 0.0f, 0.0f, -6.0f / 9.0f, 2.0f / 9.0f, 0.0f, -6.0f / 9.0f, 4.0f / 9.0f,
            0.0f, -6.0f / 9.0f, 6.0f / 9.0f, 0.0f, -6.0f / 9.0f, 8.0f / 9.0f, 0.0f, -6.0f / 9.0f, -8.0f / 9.0f, 2.0f / 9.0f,
            -6.0f / 9.0f, -6.0f / 9.0f, 2.0f / 9.0f, -6.0f / 9.0f, -4.0f / 9.0f, 2.0f / 9.0f, -6.0f / 9.0f, -2.0f / 9.0f, 2.0f / 9.0f,
            -6.0f / 9.0f, 0.0f, 2.0f / 9.0f, -6.0f / 9.0f, 2.0f / 9.0f, 2.0f / 9.0f, -6.0f / 9.0f, 4.0f / 9.0f, 2.0f / 9.0f,
            -6.0f / 9.0f, 6.0f / 9.0f, 2.0f / 9.0f, -6.0f / 9.0f, 8.0f / 9.0f, 2.0f / 9.0f, -6.0f / 9.0f, -8.0f / 9.0f, 4.0f / 9.0f,
            -6.0f / 9.0f, -6.0f / 9.0f, 4.0f / 9.0f, -6.0f / 9.0f, -4.0f / 9.0f, 4.0f / 9.0f, -6.0f / 9.0f, -2.0f / 9.0f, 4.0f / 9.0f,
            -6.0f / 9.0f, 0.0f, 4.0f / 9.0f, -6.0f / 9.0f, 2.0f / 9.0f, 4.0f / 9.0f, -6.0f / 9.0f, 4.0f / 9.0f, 4.0f / 9.0f,
            -6.0f / 9.0f, 6.0f / 9.0f, 4.0f / 9.0f, -6.0f / 9.0f, 8.0f / 9.0f, 4.0f / 9.0f, -6.0f / 9.0f, -8.0f /
                                                                                                          9.0f,
            6.0f / 9.0f, -6.0f / 9.0f, -6.0f / 9.0f, 6.0f / 9.0f, -6.0f / 9.0f, -4.0f / 9.0f, 6.0f / 9.0f, -6.0f / 9.0f, -2.0f / 9.0f, 6.0f / 9.0f, -6.0f / 9.0f, 0.0f, 6.0f / 9.0f, -6.0f / 9.0f,
            2.0f / 9.0f, 6.0f / 9.0f, -6.0f / 9.0f, 4.0f / 9.0f, 6.0f / 9.0f, -6.0f / 9.0f, 6.0f / 9.0f, 6.0f / 9.0f, -6.0f / 9.0f,
            8.0f / 9.0f, 6.0f / 9.0f, -6.0f / 9.0f, -8.0f / 9.0f, 8.0f / 9.0f, -6.0f / 9.0f, -6.0f / 9.0f, 8.0f / 9.0f, -6.0f / 9.0f,
            -4.0f / 9.0f, 8.0f / 9.0f, -6.0f / 9.0f, -2.0f / 9.0f, 8.0f / 9.0f, -6.0f / 9.0f, 0.0f, 8.0f / 9.0f, -6.0f / 9.0f,
            2.0f / 9.0f, 8.0f / 9.0f, -6.0f / 9.0f, 4.0f / 9.0f, 8.0f / 9.0f, -6.0f / 9.0f, 6.0f / 9.0f, 8.0f / 9.0f, -6.0f / 9.0f,
            8.0f / 9.0f, 8.0f / 9.0f, -6.0f / 9.0f, -8.0f / 9.0f, -8.0f / 9.0f, -4.0f / 9.0f, -6.0f / 9.0f, -8.0f / 9.0f, -4.0f / 9.0f,
            -4.0f / 9.0f, -8.0f / 9.0f, -4.0f / 9.0f, -2.0f / 9.0f, -8.0f / 9.0f, -4.0f / 9.0f, 0.0f, -8.0f / 9.0f, -4.0f / 9.0f,
            2.0f / 9.0f, -8.0f / 9.0f, -4.0f / 9.0f, 4.0f / 9.0f, -8.0f / 9.0f, -4.0f / 9.0f, 6.0f / 9.0f, -8.0f / 9.0f, -4.0f / 9.0f,
            8.0f / 9.0f, -8.0f / 9.0f, -4.0f / 9.0f, -8.0f / 9.0f, -6.0f / 9.0f, -4.0f / 9.0f, -6.0f / 9.0f, -6.0f / 9.0f,
            -4.0f / 9.0f, -4.0f / 9.0f, -6.0f / 9.0f, -4.0f / 9.0f, -2.0f / 9.0f, -6.0f / 9.0f, -4.0f / 9.0f, 0.0f, -6.0f / 9.0f,
            -4.0f / 9.0f, 2.0f / 9.0f, -6.0f / 9.0f, -4.0f / 9.0f, 4.0f / 9.0f, -6.0f / 9.0f, -4.0f / 9.0f, 6.0f / 9.0f, -6.0f / 9.0f,
            -4.0f / 9.0f, 8.0f / 9.0f, -6.0f / 9.0f, -4.0f / 9.0f, -8.0f / 9.0f, -4.0f / 9.0f, -4.0f / 9.0f, -6.0f / 9.0f,
            -4.0f / 9.0f, -4.0f / 9.0f, -4.0f / 9.0f, -4.0f / 9.0f, -4.0f / 9.0f, -2.0f / 9.0f, -4.0f / 9.0f, -4.0f / 9.0f, 0.0f,
            -4.0f / 9.0f, -4.0f / 9.0f, 2.0f / 9.0f, -4.0f / 9.0f, -4.0f / 9.0f, 4.0f / 9.0f, -4.0f / 9.0f, -4.0f / 9.0f, 6.0f / 9.0f,
            -4.0f / 9.0f, -4.0f / 9.0f, 8.0f / 9.0f, -4.0f / 9.0f, -4.0f / 9.0f, -8.0f / 9.0f, -2.0f / 9.0f, -4.0f / 9.0f,
            -6.0f / 9.0f, -2.0f / 9.0f, -4.0f / 9.0f, -4.0f / 9.0f, -2.0f / 9.0f, -4.0f / 9.0f, -2.0f / 9.0f, -2.0f / 9.0f,
            -4.0f / 9.0f, 0.0f, -2.0f / 9.0f, -4.0f / 9.0f,
            2.0f / 9.0f, -2.0f / 9.0f, -4.0f / 9.0f, 4.0f / 9.0f, -2.0f / 9.0f, -4.0f / 9.0f, 6.0f / 9.0f, -2.0f / 9.0f, -4.0f / 9.0f,
            8.0f / 9.0f, -2.0f / 9.0f, -4.0f / 9.0f, -8.0f / 9.0f, 0.0f, -4.0f / 9.0f, -6.0f / 9.0f, 0.0f, -4.0f / 9.0f,
            -4.0f / 9.0f, 0.0f, -4.0f / 9.0f, -2.0f / 9.0f, 0.0f, -4.0f / 9.0f, 0.0f, 0.0f, -4.0f / 9.0f, 2.0f / 9.0f, 0.0f,
            -4.0f / 9.0f, 4.0f / 9.0f, 0.0f, -4.0f / 9.0f, 6.0f / 9.0f, 0.0f, -4.0f / 9.0f, 8.0f / 9.0f, 0.0f, -4.0f / 9.0f,
            -8.0f / 9.0f, 2.0f / 9.0f, -4.0f / 9.0f, -6.0f / 9.0f, 2.0f / 9.0f, -4.0f / 9.0f, -4.0f / 9.0f, 2.0f / 9.0f, -4.0f / 9.0f,
            -2.0f / 9.0f, 2.0f / 9.0f, -4.0f / 9.0f, 0.0f, 2.0f / 9.0f, -4.0f / 9.0f, 2.0f / 9.0f, 2.0f / 9.0f, -4.0f / 9.0f,
            4.0f / 9.0f, 2.0f / 9.0f, -4.0f / 9.0f, 6.0f / 9.0f, 2.0f / 9.0f, -4.0f / 9.0f, 8.0f / 9.0f, 2.0f / 9.0f, -4.0f / 9.0f,
            -8.0f / 9.0f, 4.0f / 9.0f, -4.0f / 9.0f, -6.0f / 9.0f, 4.0f / 9.0f, -4.0f / 9.0f, -4.0f / 9.0f, 4.0f / 9.0f, -4.0f / 9.0f,
            -2.0f / 9.0f, 4.0f / 9.0f, -4.0f / 9.0f, 0.0f, 4.0f / 9.0f, -4.0f / 9.0f, 2.0f / 9.0f, 4.0f / 9.0f, -4.0f / 9.0f,
            4.0f / 9.0f, 4.0f / 9.0f, -4.0f / 9.0f, 6.0f / 9.0f, 4.0f / 9.0f, -4.0f / 9.0f, 8.0f / 9.0f, 4.0f / 9.0f, -4.0f / 9.0f,
            -8.0f / 9.0f, 6.0f / 9.0f, -4.0f / 9.0f, -6.0f / 9.0f, 6.0f / 9.0f, -4.0f / 9.0f, -4.0f / 9.0f, 6.0f / 9.0f, -4.0f / 9.0f,
            -2.0f / 9.0f, 6.0f / 9.0f, -4.0f / 9.0f, 0.0f, 6.0f / 9.0f, -4.0f / 9.0f, 2.0f / 9.0f, 6.0f / 9.0f, -4.0f / 9.0f,
            4.0f / 9.0f, 6.0f / 9.0f, -4.0f / 9.0f, 6.0f / 9.0f, 6.0f / 9.0f, -4.0f / 9.0f, 8.0f / 9.0f, 6.0f / 9.0f, -4.0f / 9.0f,
            -8.0f / 9.0f, 8.0f / 9.0f, -4.0f / 9.0f, -6.0f / 9.0f, 8.0f / 9.0f, -4.0f / 9.0f, -4.0f / 9.0f, 8.0f / 9.0f, -4.0f / 9.0f,
            -2.0f / 9.0f, 8.0f / 9.0f, -4.0f / 9.0f, 0.0f, 8.0f / 9.0f, -4.0f / 9.0f, 2.0f / 9.0f, 8.0f / 9.0f, -4.0f / 9.0f,
            4.0f / 9.0f, 8.0f / 9.0f, -4.0f / 9.0f, 6.0f / 9.0f, 8.0f / 9.0f, -4.0f / 9.0f, 8.0f / 9.0f, 8.0f / 9.0f, -4.0f / 9.0f,
            -8.0f / 9.0f, -8.0f / 9.0f, -2.0f / 9.0f, -6.0f / 9.0f,
            -8.0f / 9.0f, -2.0f / 9.0f, -4.0f / 9.0f, -8.0f / 9.0f, -2.0f / 9.0f, -2.0f / 9.0f, -8.0f / 9.0f, -2.0f / 9.0f, 0.0f,
            -8.0f / 9.0f, -2.0f / 9.0f, 2.0f / 9.0f, -8.0f / 9.0f, -2.0f / 9.0f, 4.0f / 9.0f, -8.0f / 9.0f, -2.0f / 9.0f, 6.0f / 9.0f,
            -8.0f / 9.0f, -2.0f / 9.0f, 8.0f / 9.0f, -8.0f / 9.0f, -2.0f / 9.0f, -8.0f / 9.0f, -6.0f / 9.0f, -2.0f / 9.0f,
            -6.0f / 9.0f, -6.0f / 9.0f, -2.0f / 9.0f, -4.0f / 9.0f, -6.0f / 9.0f, -2.0f / 9.0f, -2.0f / 9.0f, -6.0f / 9.0f,
            -2.0f / 9.0f, 0.0f, -6.0f / 9.0f, -2.0f / 9.0f, 2.0f / 9.0f, -6.0f / 9.0f, -2.0f / 9.0f, 4.0f / 9.0f, -6.0f / 9.0f,
            -2.0f / 9.0f, 6.0f / 9.0f, -6.0f / 9.0f, -2.0f / 9.0f, 8.0f / 9.0f, -6.0f / 9.0f, -2.0f / 9.0f, -8.0f / 9.0f, -4.0f / 9.0f,
            -2.0f / 9.0f, -6.0f / 9.0f, -4.0f / 9.0f, -2.0f / 9.0f, -4.0f / 9.0f, -4.0f / 9.0f, -2.0f / 9.0f, -2.0f / 9.0f,
            -4.0f / 9.0f, -2.0f / 9.0f, 0.0f, -4.0f / 9.0f, -2.0f / 9.0f, 2.0f / 9.0f, -4.0f / 9.0f, -2.0f / 9.0f, 4.0f / 9.0f,
            -4.0f / 9.0f, -2.0f / 9.0f, 6.0f / 9.0f, -4.0f / 9.0f, -2.0f / 9.0f, 8.0f / 9.0f, -4.0f / 9.0f, -2.0f / 9.0f, -8.0f / 9.0f,
            -2.0f / 9.0f, -2.0f / 9.0f, -6.0f / 9.0f, -2.0f / 9.0f, -2.0f / 9.0f, -4.0f / 9.0f, -2.0f / 9.0f, -2.0f / 9.0f,
            -2.0f / 9.0f, -2.0f / 9.0f, -2.0f / 9.0f, 0.0f, -2.0f / 9.0f, -2.0f / 9.0f, 2.0f / 9.0f, -2.0f / 9.0f, -2.0f / 9.0f,
            4.0f / 9.0f, -2.0f / 9.0f, -2.0f / 9.0f, 6.0f / 9.0f, -2.0f / 9.0f, -2.0f / 9.0f, 8.0f / 9.0f, -2.0f / 9.0f, -2.0f / 9.0f,
            -8.0f / 9.0f, 0.0f, -2.0f / 9.0f, -6.0f / 9.0f, 0.0f, -2.0f / 9.0f, -4.0f / 9.0f, 0.0f, -2.0f / 9.0f, -2.0f / 9.0f,
            0.0f, -2.0f / 9.0f, 0.0f, 0.0f, -2.0f / 9.0f, 2.0f / 9.0f, 0.0f, -2.0f / 9.0f, 4.0f / 9.0f, 0.0f, -2.0f / 9.0f,
            6.0f / 9.0f, 0.0f, -2.0f / 9.0f, 8.0f / 9.0f, 0.0f, -2.0f / 9.0f, -8.0f / 9.0f, 2.0f / 9.0f, -2.0f / 9.0f, -6.0f / 9.0f,
            2.0f / 9.0f, -2.0f / 9.0f, -4.0f / 9.0f, 2.0f / 9.0f, -2.0f / 9.0f, -2.0f / 9.0f, 2.0f / 9.0f, -2.0f / 9.0f, 0.0f,
            2.0f / 9.0f, -2.0f / 9.0f, 2.0f / 9.0f, 2.0f
                                                    / 9.0f,
            -2.0f / 9.0f, 4.0f / 9.0f, 2.0f / 9.0f, -2.0f / 9.0f, 6.0f / 9.0f, 2.0f / 9.0f, -2.0f / 9.0f, 8.0f / 9.0f, 2.0f / 9.0f,
            -2.0f / 9.0f, -8.0f / 9.0f, 4.0f / 9.0f, -2.0f / 9.0f, -6.0f / 9.0f, 4.0f / 9.0f, -2.0f / 9.0f, -4.0f / 9.0f, 4.0f / 9.0f,
            -2.0f / 9.0f, -2.0f / 9.0f, 4.0f / 9.0f, -2.0f / 9.0f, 0.0f, 4.0f / 9.0f, -2.0f / 9.0f, 2.0f / 9.0f, 4.0f / 9.0f,
            -2.0f / 9.0f, 4.0f / 9.0f, 4.0f / 9.0f, -2.0f / 9.0f, 6.0f / 9.0f, 4.0f / 9.0f, -2.0f / 9.0f, 8.0f / 9.0f, 4.0f / 9.0f,
            -2.0f / 9.0f, -8.0f / 9.0f, 6.0f / 9.0f, -2.0f / 9.0f, -6.0f / 9.0f, 6.0f / 9.0f, -2.0f / 9.0f, -4.0f / 9.0f, 6.0f / 9.0f,
            -2.0f / 9.0f, -2.0f / 9.0f, 6.0f / 9.0f, -2.0f / 9.0f, 0.0f, 6.0f / 9.0f, -2.0f / 9.0f, 2.0f / 9.0f, 6.0f / 9.0f,
            -2.0f / 9.0f, 4.0f / 9.0f, 6.0f / 9.0f, -2.0f / 9.0f, 6.0f / 9.0f, 6.0f / 9.0f, -2.0f / 9.0f, 8.0f / 9.0f, 6.0f / 9.0f,
            -2.0f / 9.0f, -8.0f / 9.0f, 8.0f / 9.0f, -2.0f / 9.0f, -6.0f / 9.0f, 8.0f / 9.0f, -2.0f / 9.0f, -4.0f / 9.0f, 8.0f / 9.0f,
            -2.0f / 9.0f, -2.0f / 9.0f, 8.0f / 9.0f, -2.0f / 9.0f, 0.0f, 8.0f / 9.0f, -2.0f / 9.0f, 2.0f / 9.0f, 8.0f / 9.0f,
            -2.0f / 9.0f, 4.0f / 9.0f, 8.0f / 9.0f, -2.0f / 9.0f, 6.0f / 9.0f, 8.0f / 9.0f, -2.0f / 9.0f, 8.0f / 9.0f, 8.0f / 9.0f,
            -2.0f / 9.0f, -8.0f / 9.0f, -8.0f / 9.0f, 0.0f, -6.0f / 9.0f, -8.0f / 9.0f, 0.0f, -4.0f / 9.0f, -8.0f / 9.0f, 0.0f,
            -2.0f / 9.0f, -8.0f / 9.0f, 0.0f, 0.0f, -8.0f / 9.0f, 0.0f, 2.0f / 9.0f, -8.0f / 9.0f, 0.0f, 4.0f / 9.0f, -8.0f / 9.0f,
            0.0f, 6.0f / 9.0f, -8.0f / 9.0f, 0.0f, 8.0f / 9.0f, -8.0f / 9.0f, 0.0f, -8.0f / 9.0f, -6.0f / 9.0f, 0.0f, -6.0f / 9.0f,
            -6.0f / 9.0f, 0.0f, -4.0f / 9.0f, -6.0f / 9.0f, 0.0f, -2.0f / 9.0f, -6.0f / 9.0f, 0.0f, 0.0f, -6.0f / 9.0f, 0.0f,
            2.0f / 9.0f, -6.0f / 9.0f, 0.0f, 4.0f / 9.0f, -6.0f / 9.0f, 0.0f, 6.0f / 9.0f, -6.0f / 9.0f, 0.0f, 8.0f / 9.0f,
            -6.0f / 9.0f, 0.0f, -8.0f / 9.0f, -4.0f / 9.0f, 0.0f, -6.0f / 9.0f, -4.0f / 9.0f, 0.0f, -4.0f / 9.0f, -4.0f / 9.0f,
            0.0f, -2.0f / 9.0f, -4.0f / 9.0f, 0.0f, 0.0f,
            -4.0f / 9.0f, 0.0f,
            2.0f / 9.0f, -4.0f / 9.0f, 0.0f, 4.0f / 9.0f, -4.0f / 9.0f, 0.0f, 6.0f / 9.0f, -4.0f / 9.0f, 0.0f, 8.0f / 9.0f,
            -4.0f / 9.0f, 0.0f, -8.0f / 9.0f, -2.0f / 9.0f, 0.0f, -6.0f / 9.0f, -2.0f / 9.0f, 0.0f, -4.0f / 9.0f, -2.0f / 9.0f,
            0.0f, -2.0f / 9.0f, -2.0f / 9.0f, 0.0f, 0.0f, -2.0f / 9.0f, 0.0f, 2.0f / 9.0f, -2.0f / 9.0f, 0.0f, 4.0f / 9.0f,
            -2.0f / 9.0f, 0.0f, 6.0f / 9.0f, -2.0f / 9.0f, 0.0f, 8.0f / 9.0f, -2.0f / 9.0f, 0.0f, -8.0f / 9.0f, 0.0f, 0.0f,
            -6.0f / 9.0f, 0.0f, 0.0f, -4.0f / 9.0f, 0.0f, 0.0f, -2.0f / 9.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 2.0f / 9.0f,
            0.0f, 0.0f, 4.0f / 9.0f, 0.0f, 0.0f, 6.0f / 9.0f, 0.0f, 0.0f, 8.0f / 9.0f, 0.0f, 0.0f, -8.0f / 9.0f, 2.0f / 9.0f,
            0.0f, -6.0f / 9.0f, 2.0f / 9.0f, 0.0f, -4.0f / 9.0f, 2.0f / 9.0f, 0.0f, -2.0f / 9.0f, 2.0f / 9.0f, 0.0f, 0.0f,
            2.0f / 9.0f, 0.0f, 2.0f / 9.0f, 2.0f / 9.0f, 0.0f, 4.0f / 9.0f, 2.0f / 9.0f, 0.0f, 6.0f / 9.0f, 2.0f / 9.0f, 0.0f,
            8.0f / 9.0f, 2.0f / 9.0f, 0.0f, -8.0f / 9.0f, 4.0f / 9.0f, 0.0f, -6.0f / 9.0f, 4.0f / 9.0f, 0.0f, -4.0f / 9.0f,
            4.0f / 9.0f, 0.0f, -2.0f / 9.0f, 4.0f / 9.0f, 0.0f, 0.0f, 4.0f / 9.0f, 0.0f, 2.0f / 9.0f, 4.0f / 9.0f, 0.0f,
            4.0f / 9.0f, 4.0f / 9.0f, 0.0f, 6.0f / 9.0f, 4.0f / 9.0f, 0.0f, 8.0f / 9.0f, 4.0f / 9.0f, 0.0f, -8.0f / 9.0f,
            6.0f / 9.0f, 0.0f, -6.0f / 9.0f, 6.0f / 9.0f, 0.0f, -4.0f / 9.0f, 6.0f / 9.0f, 0.0f, -2.0f / 9.0f, 6.0f / 9.0f, 0.0f,
            0.0f, 6.0f / 9.0f, 0.0f, 2.0f / 9.0f, 6.0f / 9.0f, 0.0f, 4.0f / 9.0f, 6.0f / 9.0f, 0.0f, 6.0f / 9.0f, 6.0f / 9.0f,
            0.0f, 8.0f / 9.0f, 6.0f / 9.0f, 0.0f, -8.0f / 9.0f, 8.0f / 9.0f, 0.0f, -6.0f / 9.0f, 8.0f / 9.0f, 0.0f, -4.0f / 9.0f,
            8.0f / 9.0f, 0.0f, -2.0f / 9.0f, 8.0f / 9.0f, 0.0f, 0.0f, 8.0f / 9.0f, 0.0f, 2.0f / 9.0f, 8.0f / 9.0f, 0.0f,
            4.0f / 9.0f, 8.0f / 9.0f, 0.0f, 6.0f / 9.0f, 8.0f / 9.0f, 0.0f, 8.0f / 9.0f, 8.0f / 9.0f, 0.0f, -8.0f / 9.0f,
            -8.0f / 9.0f, 2.0f / 9.0f, -6.0f / 9.0f, -8.0f / 9.0f, 2.0f / 9.0f, -4.0f / 9.0f, -8.0f / 9.0f, 2.0f / 9.0f, -2.0f / 9.0f,
            -8.0f / 9.0f, 2.0f / 9.0f, 0.0f, -8.0f
                                             / 9.0f,
            2.0f / 9.0f, 2.0f / 9.0f, -8.0f / 9.0f, 2.0f / 9.0f, 4.0f / 9.0f, -8.0f / 9.0f, 2.0f / 9.0f, 6.0f / 9.0f, -8.0f / 9.0f,
            2.0f / 9.0f, 8.0f / 9.0f, -8.0f / 9.0f, 2.0f / 9.0f, -8.0f / 9.0f, -6.0f / 9.0f, 2.0f / 9.0f, -6.0f / 9.0f, -6.0f / 9.0f,
            2.0f / 9.0f, -4.0f / 9.0f, -6.0f / 9.0f, 2.0f / 9.0f, -2.0f / 9.0f, -6.0f / 9.0f, 2.0f / 9.0f, 0.0f, -6.0f / 9.0f,
            2.0f / 9.0f, 2.0f / 9.0f, -6.0f / 9.0f, 2.0f / 9.0f, 4.0f / 9.0f, -6.0f / 9.0f, 2.0f / 9.0f, 6.0f / 9.0f, -6.0f / 9.0f,
            2.0f / 9.0f, 8.0f / 9.0f, -6.0f / 9.0f, 2.0f / 9.0f, -8.0f / 9.0f, -4.0f / 9.0f, 2.0f / 9.0f, -6.0f / 9.0f, -4.0f / 9.0f,
            2.0f / 9.0f, -4.0f / 9.0f, -4.0f / 9.0f, 2.0f / 9.0f, -2.0f / 9.0f, -4.0f / 9.0f, 2.0f / 9.0f, 0.0f, -4.0f / 9.0f,
            2.0f / 9.0f, 2.0f / 9.0f, -4.0f / 9.0f, 2.0f / 9.0f, 4.0f / 9.0f, -4.0f / 9.0f, 2.0f / 9.0f, 6.0f / 9.0f, -4.0f / 9.0f,
            2.0f / 9.0f, 8.0f / 9.0f, -4.0f / 9.0f, 2.0f / 9.0f, -8.0f / 9.0f, -2.0f / 9.0f, 2.0f / 9.0f, -6.0f / 9.0f, -2.0f / 9.0f,
            2.0f / 9.0f, -4.0f / 9.0f, -2.0f / 9.0f, 2.0f / 9.0f, -2.0f / 9.0f, -2.0f / 9.0f, 2.0f / 9.0f, 0.0f, -2.0f / 9.0f,
            2.0f / 9.0f, 2.0f / 9.0f, -2.0f / 9.0f, 2.0f / 9.0f, 4.0f / 9.0f, -2.0f / 9.0f, 2.0f / 9.0f, 6.0f / 9.0f, -2.0f / 9.0f,
            2.0f / 9.0f, 8.0f / 9.0f, -2.0f / 9.0f, 2.0f / 9.0f, -8.0f / 9.0f, 0.0f, 2.0f / 9.0f, -6.0f / 9.0f, 0.0f, 2.0f / 9.0f,
            -4.0f / 9.0f, 0.0f, 2.0f / 9.0f, -2.0f / 9.0f, 0.0f, 2.0f / 9.0f, 0.0f, 0.0f, 2.0f / 9.0f, 2.0f / 9.0f, 0.0f,
            2.0f / 9.0f, 4.0f / 9.0f, 0.0f, 2.0f / 9.0f, 6.0f / 9.0f, 0.0f, 2.0f / 9.0f, 8.0f / 9.0f, 0.0f, 2.0f / 9.0f,
            -8.0f / 9.0f, 2.0f / 9.0f, 2.0f / 9.0f, -6.0f / 9.0f, 2.0f / 9.0f, 2.0f / 9.0f, -4.0f / 9.0f, 2.0f / 9.0f, 2.0f / 9.0f,
            -2.0f / 9.0f, 2.0f / 9.0f, 2.0f / 9.0f, 0.0f, 2.0f / 9.0f, 2.0f / 9.0f, 2.0f / 9.0f, 2.0f / 9.0f, 2.0f / 9.0f, 4.0f / 9.0f,
            2.0f / 9.0f, 2.0f / 9.0f, 6.0f / 9.0f, 2.0f / 9.0f, 2.0f / 9.0f, 8.0f / 9.0f, 2.0f / 9.0f, 2.0f / 9.0f, -8.0f / 9.0f,
            4.0f / 9.0f, 2.0f / 9.0f, -6.0f / 9.0f, 4.0f / 9.0f, 2.0f / 9.0f, -4.0f
                                                                              / 9.0f,
            4.0f / 9.0f, 2.0f / 9.0f, -2.0f / 9.0f, 4.0f / 9.0f, 2.0f / 9.0f, 0.0f, 4.0f / 9.0f, 2.0f / 9.0f, 2.0f / 9.0f, 4.0f / 9.0f,
            2.0f / 9.0f, 4.0f / 9.0f, 4.0f / 9.0f, 2.0f / 9.0f, 6.0f / 9.0f, 4.0f / 9.0f, 2.0f / 9.0f, 8.0f / 9.0f, 4.0f / 9.0f,
            2.0f / 9.0f, -8.0f / 9.0f, 6.0f / 9.0f, 2.0f / 9.0f, -6.0f / 9.0f, 6.0f / 9.0f, 2.0f / 9.0f, -4.0f / 9.0f, 6.0f / 9.0f,
            2.0f / 9.0f, -2.0f / 9.0f, 6.0f / 9.0f, 2.0f / 9.0f, 0.0f, 6.0f / 9.0f, 2.0f / 9.0f, 2.0f / 9.0f, 6.0f / 9.0f, 2.0f / 9.0f,
            4.0f / 9.0f, 6.0f / 9.0f, 2.0f / 9.0f, 6.0f / 9.0f, 6.0f / 9.0f, 2.0f / 9.0f, 8.0f / 9.0f, 6.0f / 9.0f, 2.0f / 9.0f,
            -8.0f / 9.0f, 8.0f / 9.0f, 2.0f / 9.0f, -6.0f / 9.0f, 8.0f / 9.0f, 2.0f / 9.0f, -4.0f / 9.0f, 8.0f / 9.0f, 2.0f / 9.0f,
            -2.0f / 9.0f, 8.0f / 9.0f, 2.0f / 9.0f, 0.0f, 8.0f / 9.0f, 2.0f / 9.0f, 2.0f / 9.0f, 8.0f / 9.0f, 2.0f / 9.0f, 4.0f / 9.0f,
            8.0f / 9.0f, 2.0f / 9.0f, 6.0f / 9.0f, 8.0f / 9.0f, 2.0f / 9.0f, 8.0f / 9.0f, 8.0f / 9.0f, 2.0f / 9.0f, -8.0f / 9.0f,
            -8.0f / 9.0f, 4.0f / 9.0f, -6.0f / 9.0f, -8.0f / 9.0f, 4.0f / 9.0f, -4.0f / 9.0f, -8.0f / 9.0f, 4.0f / 9.0f, -2.0f / 9.0f,
            -8.0f / 9.0f, 4.0f / 9.0f, 0.0f, -8.0f / 9.0f, 4.0f / 9.0f, 2.0f / 9.0f, -8.0f / 9.0f, 4.0f / 9.0f, 4.0f / 9.0f,
            -8.0f / 9.0f, 4.0f / 9.0f, 6.0f / 9.0f, -8.0f / 9.0f, 4.0f / 9.0f, 8.0f / 9.0f, -8.0f / 9.0f, 4.0f / 9.0f, -8.0f / 9.0f,
            -6.0f / 9.0f, 4.0f / 9.0f, -6.0f / 9.0f, -6.0f / 9.0f, 4.0f / 9.0f, -4.0f / 9.0f, -6.0f / 9.0f, 4.0f / 9.0f, -2.0f / 9.0f,
            -6.0f / 9.0f, 4.0f / 9.0f, 0.0f, -6.0f / 9.0f, 4.0f / 9.0f, 2.0f / 9.0f, -6.0f / 9.0f, 4.0f / 9.0f, 4.0f / 9.0f,
            -6.0f / 9.0f, 4.0f / 9.0f, 6.0f / 9.0f, -6.0f / 9.0f, 4.0f / 9.0f, 8.0f / 9.0f, -6.0f / 9.0f, 4.0f / 9.0f, -8.0f / 9.0f,
            -4.0f / 9.0f, 4.0f / 9.0f, -6.0f / 9.0f, -4.0f / 9.0f, 4.0f / 9.0f, -4.0f / 9.0f, -4.0f / 9.0f, 4.0f / 9.0f, -2.0f / 9.0f,
            -4.0f / 9.0f, 4.0f / 9.0f, 0.0f, -4.0f / 9.0f, 4.0f / 9.0f, 2.0f / 9.0f, -4.0f / 9.0f, 4.0f / 9.0f, 4.0f / 9.0f,
            -4.0f / 9.0f, 4.0f / 9.0f, 6.0f / 9.0f, -4.0f / 9.0f,
            4.0f / 9.0f, 8.0f / 9.0f, -4.0f / 9.0f, 4.0f / 9.0f, -8.0f / 9.0f, -2.0f / 9.0f, 4.0f / 9.0f, -6.0f / 9.0f, -2.0f / 9.0f,
            4.0f / 9.0f, -4.0f / 9.0f, -2.0f / 9.0f, 4.0f / 9.0f, -2.0f / 9.0f, -2.0f / 9.0f, 4.0f / 9.0f, 0.0f, -2.0f / 9.0f,
            4.0f / 9.0f, 2.0f / 9.0f, -2.0f / 9.0f, 4.0f / 9.0f, 4.0f / 9.0f, -2.0f / 9.0f, 4.0f / 9.0f, 6.0f / 9.0f, -2.0f / 9.0f,
            4.0f / 9.0f, 8.0f / 9.0f, -2.0f / 9.0f, 4.0f / 9.0f, -8.0f / 9.0f, 0.0f, 4.0f / 9.0f, -6.0f / 9.0f, 0.0f, 4.0f / 9.0f,
            -4.0f / 9.0f, 0.0f, 4.0f / 9.0f, -2.0f / 9.0f, 0.0f, 4.0f / 9.0f, 0.0f, 0.0f, 4.0f / 9.0f, 2.0f / 9.0f, 0.0f,
            4.0f / 9.0f, 4.0f / 9.0f, 0.0f, 4.0f / 9.0f, 6.0f / 9.0f, 0.0f, 4.0f / 9.0f, 8.0f / 9.0f, 0.0f, 4.0f / 9.0f,
            -8.0f / 9.0f, 2.0f / 9.0f, 4.0f / 9.0f, -6.0f / 9.0f, 2.0f / 9.0f, 4.0f / 9.0f, -4.0f / 9.0f, 2.0f / 9.0f, 4.0f / 9.0f,
            -2.0f / 9.0f, 2.0f / 9.0f, 4.0f / 9.0f, 0.0f, 2.0f / 9.0f, 4.0f / 9.0f, 2.0f / 9.0f, 2.0f / 9.0f, 4.0f / 9.0f, 4.0f / 9.0f,
            2.0f / 9.0f, 4.0f / 9.0f, 6.0f / 9.0f, 2.0f / 9.0f, 4.0f / 9.0f, 8.0f / 9.0f, 2.0f / 9.0f, 4.0f / 9.0f, -8.0f / 9.0f,
            4.0f / 9.0f, 4.0f / 9.0f, -6.0f / 9.0f, 4.0f / 9.0f, 4.0f / 9.0f, -4.0f / 9.0f, 4.0f / 9.0f, 4.0f / 9.0f, -2.0f / 9.0f,
            4.0f / 9.0f, 4.0f / 9.0f, 0.0f, 4.0f / 9.0f, 4.0f / 9.0f, 2.0f / 9.0f, 4.0f / 9.0f, 4.0f / 9.0f, 4.0f / 9.0f, 4.0f / 9.0f,
            4.0f / 9.0f, 6.0f / 9.0f, 4.0f / 9.0f, 4.0f / 9.0f, 8.0f / 9.0f, 4.0f / 9.0f, 4.0f / 9.0f, -8.0f / 9.0f, 6.0f / 9.0f,
            4.0f / 9.0f, -6.0f / 9.0f, 6.0f / 9.0f, 4.0f / 9.0f, -4.0f / 9.0f, 6.0f / 9.0f, 4.0f / 9.0f, -2.0f / 9.0f, 6.0f / 9.0f,
            4.0f / 9.0f, 0.0f, 6.0f / 9.0f, 4.0f / 9.0f, 2.0f / 9.0f, 6.0f / 9.0f, 4.0f / 9.0f, 4.0f / 9.0f, 6.0f / 9.0f, 4.0f / 9.0f,
            6.0f / 9.0f, 6.0f / 9.0f, 4.0f / 9.0f, 8.0f / 9.0f, 6.0f / 9.0f, 4.0f / 9.0f, -8.0f / 9.0f, 8.0f / 9.0f, 4.0f / 9.0f,
            -6.0f / 9.0f, 8.0f / 9.0f, 4.0f / 9.0f, -4.0f / 9.0f, 8.0f / 9.0f, 4.0f / 9.0f, -2.0f / 9.0f, 8.0f / 9.0f, 4.0f / 9.0f,
            0.0f, 8.0f / 9.0f, 4.0f / 9.0f, 2.0f / 9.0f, 8.0f / 9.0f, 4.0f / 9.0f, 4.0f / 9.0f, 8.0f /
                                                                                                9.0f,
            4.0f / 9.0f, 6.0f / 9.0f, 8.0f / 9.0f, 4.0f / 9.0f, 8.0f / 9.0f, 8.0f / 9.0f, 4.0f / 9.0f, -8.0f / 9.0f, -8.0f / 9.0f,
            6.0f / 9.0f, -6.0f / 9.0f, -8.0f / 9.0f, 6.0f / 9.0f, -4.0f / 9.0f, -8.0f / 9.0f, 6.0f / 9.0f, -2.0f / 9.0f, -8.0f / 9.0f,
            6.0f / 9.0f, 0.0f, -8.0f / 9.0f, 6.0f / 9.0f, 2.0f / 9.0f, -8.0f / 9.0f, 6.0f / 9.0f, 4.0f / 9.0f, -8.0f / 9.0f,
            6.0f / 9.0f, 6.0f / 9.0f, -8.0f / 9.0f, 6.0f / 9.0f, 8.0f / 9.0f, -8.0f / 9.0f, 6.0f / 9.0f, -8.0f / 9.0f, -6.0f / 9.0f,
            6.0f / 9.0f, -6.0f / 9.0f, -6.0f / 9.0f, 6.0f / 9.0f, -4.0f / 9.0f, -6.0f / 9.0f, 6.0f / 9.0f, -2.0f / 9.0f, -6.0f / 9.0f,
            6.0f / 9.0f, 0.0f, -6.0f / 9.0f, 6.0f / 9.0f, 2.0f / 9.0f, -6.0f / 9.0f, 6.0f / 9.0f, 4.0f / 9.0f, -6.0f / 9.0f,
            6.0f / 9.0f, 6.0f / 9.0f, -6.0f / 9.0f, 6.0f / 9.0f, 8.0f / 9.0f, -6.0f / 9.0f, 6.0f / 9.0f, -8.0f / 9.0f, -4.0f / 9.0f,
            6.0f / 9.0f, -6.0f / 9.0f, -4.0f / 9.0f, 6.0f / 9.0f, -4.0f / 9.0f, -4.0f / 9.0f, 6.0f / 9.0f, -2.0f / 9.0f, -4.0f / 9.0f,
            6.0f / 9.0f, 0.0f, -4.0f / 9.0f, 6.0f / 9.0f, 2.0f / 9.0f, -4.0f / 9.0f, 6.0f / 9.0f, 4.0f / 9.0f, -4.0f / 9.0f,
            6.0f / 9.0f, 6.0f / 9.0f, -4.0f / 9.0f, 6.0f / 9.0f, 8.0f / 9.0f, -4.0f / 9.0f, 6.0f / 9.0f, -8.0f / 9.0f, -2.0f / 9.0f,
            6.0f / 9.0f, -6.0f / 9.0f, -2.0f / 9.0f, 6.0f / 9.0f, -4.0f / 9.0f, -2.0f / 9.0f, 6.0f / 9.0f, -2.0f / 9.0f, -2.0f / 9.0f,
            6.0f / 9.0f, 0.0f, -2.0f / 9.0f, 6.0f / 9.0f, 2.0f / 9.0f, -2.0f / 9.0f, 6.0f / 9.0f, 4.0f / 9.0f, -2.0f / 9.0f,
            6.0f / 9.0f, 6.0f / 9.0f, -2.0f / 9.0f, 6.0f / 9.0f, 8.0f / 9.0f, -2.0f / 9.0f, 6.0f / 9.0f, -8.0f / 9.0f, 0.0f,
            6.0f / 9.0f, -6.0f / 9.0f, 0.0f, 6.0f / 9.0f, -4.0f / 9.0f, 0.0f, 6.0f / 9.0f, -2.0f / 9.0f, 0.0f, 6.0f / 9.0f, 0.0f,
            0.0f, 6.0f / 9.0f, 2.0f / 9.0f, 0.0f, 6.0f / 9.0f, 4.0f / 9.0f, 0.0f, 6.0f / 9.0f, 6.0f / 9.0f, 0.0f, 6.0f / 9.0f,
            8.0f / 9.0f, 0.0f, 6.0f / 9.0f, -8.0f / 9.0f, 2.0f / 9.0f, 6.0f / 9.0f, -6.0f / 9.0f, 2.0f / 9.0f, 6.0f / 9.0f,
            -4.0f / 9.0f, 2.0f / 9.0f, 6.0f / 9.0f, -2.0f / 9.0f, 2.0f / 9.0f, 6.0f /
                                                                               9.0f,
            0.0f, 2.0f / 9.0f, 6.0f / 9.0f, 2.0f / 9.0f, 2.0f / 9.0f, 6.0f / 9.0f, 4.0f / 9.0f, 2.0f / 9.0f, 6.0f / 9.0f, 6.0f / 9.0f,
            2.0f / 9.0f, 6.0f / 9.0f, 8.0f / 9.0f, 2.0f / 9.0f, 6.0f / 9.0f, -8.0f / 9.0f, 4.0f / 9.0f, 6.0f / 9.0f, -6.0f / 9.0f,
            4.0f / 9.0f, 6.0f / 9.0f, -4.0f / 9.0f, 4.0f / 9.0f, 6.0f / 9.0f, -2.0f / 9.0f, 4.0f / 9.0f, 6.0f / 9.0f, 0.0f,
            4.0f / 9.0f, 6.0f / 9.0f, 2.0f / 9.0f, 4.0f / 9.0f, 6.0f / 9.0f, 4.0f / 9.0f, 4.0f / 9.0f, 6.0f / 9.0f, 6.0f / 9.0f,
            4.0f / 9.0f, 6.0f / 9.0f, 8.0f / 9.0f, 4.0f / 9.0f, 6.0f / 9.0f, -8.0f / 9.0f, 6.0f / 9.0f, 6.0f / 9.0f, -6.0f / 9.0f,
            6.0f / 9.0f, 6.0f / 9.0f, -4.0f / 9.0f, 6.0f / 9.0f, 6.0f / 9.0f, -2.0f / 9.0f, 6.0f / 9.0f, 6.0f / 9.0f, 0.0f,
            6.0f / 9.0f, 6.0f / 9.0f, 2.0f / 9.0f, 6.0f / 9.0f, 6.0f / 9.0f, 4.0f / 9.0f, 6.0f / 9.0f, 6.0f / 9.0f, 6.0f / 9.0f,
            6.0f / 9.0f, 6.0f / 9.0f, 8.0f / 9.0f, 6.0f / 9.0f, 6.0f / 9.0f, -8.0f / 9.0f, 8.0f / 9.0f, 6.0f / 9.0f, -6.0f / 9.0f,
            8.0f / 9.0f, 6.0f / 9.0f, -4.0f / 9.0f, 8.0f / 9.0f, 6.0f / 9.0f, -2.0f / 9.0f, 8.0f / 9.0f, 6.0f / 9.0f, 0.0f,
            8.0f / 9.0f, 6.0f / 9.0f, 2.0f / 9.0f, 8.0f / 9.0f, 6.0f / 9.0f, 4.0f / 9.0f, 8.0f / 9.0f, 6.0f / 9.0f, 6.0f / 9.0f,
            8.0f / 9.0f, 6.0f / 9.0f, 8.0f / 9.0f, 8.0f / 9.0f, 6.0f / 9.0f, -8.0f / 9.0f, -8.0f / 9.0f, 8.0f / 9.0f, -6.0f / 9.0f,
            -8.0f / 9.0f, 8.0f / 9.0f, -4.0f / 9.0f, -8.0f / 9.0f, 8.0f / 9.0f, -2.0f / 9.0f, -8.0f / 9.0f, 8.0f / 9.0f, 0.0f,
            -8.0f / 9.0f, 8.0f / 9.0f, 2.0f / 9.0f, -8.0f / 9.0f, 8.0f / 9.0f, 4.0f / 9.0f, -8.0f / 9.0f, 8.0f / 9.0f, 6.0f / 9.0f,
            -8.0f / 9.0f, 8.0f / 9.0f, 8.0f / 9.0f, -8.0f / 9.0f, 8.0f / 9.0f, -8.0f / 9.0f, -6.0f / 9.0f, 8.0f / 9.0f, -6.0f / 9.0f,
            -6.0f / 9.0f, 8.0f / 9.0f, -4.0f / 9.0f, -6.0f / 9.0f, 8.0f / 9.0f, -2.0f / 9.0f, -6.0f / 9.0f, 8.0f / 9.0f, 0.0f,
            -6.0f / 9.0f, 8.0f / 9.0f, 2.0f / 9.0f, -6.0f / 9.0f, 8.0f / 9.0f, 4.0f / 9.0f, -6.0f / 9.0f, 8.0f / 9.0f, 6.0f / 9.0f,
            -6.0f / 9.0f, 8.0f / 9.0f, 8.0f / 9.0f, -6.0f / 9.0f, 8.0f / 9.0f, -8.0f / 9.0f, -4.0f / 9.0f, 8.0f
                                                                                                           / 9.0f,
            -6.0f / 9.0f, -4.0f / 9.0f, 8.0f / 9.0f, -4.0f / 9.0f, -4.0f / 9.0f, 8.0f / 9.0f, -2.0f / 9.0f, -4.0f / 9.0f, 8.0f / 9.0f,
            0.0f, -4.0f / 9.0f, 8.0f / 9.0f, 2.0f / 9.0f, -4.0f / 9.0f, 8.0f / 9.0f, 4.0f / 9.0f, -4.0f / 9.0f, 8.0f / 9.0f,
            6.0f / 9.0f, -4.0f / 9.0f, 8.0f / 9.0f, 8.0f / 9.0f, -4.0f / 9.0f, 8.0f / 9.0f, -8.0f / 9.0f, -2.0f / 9.0f, 8.0f / 9.0f,
            -6.0f / 9.0f, -2.0f / 9.0f, 8.0f / 9.0f, -4.0f / 9.0f, -2.0f / 9.0f, 8.0f / 9.0f, -2.0f / 9.0f, -2.0f / 9.0f, 8.0f / 9.0f,
            0.0f, -2.0f / 9.0f, 8.0f / 9.0f, 2.0f / 9.0f, -2.0f / 9.0f, 8.0f / 9.0f, 4.0f / 9.0f, -2.0f / 9.0f, 8.0f / 9.0f,
            6.0f / 9.0f, -2.0f / 9.0f, 8.0f / 9.0f, 8.0f / 9.0f, -2.0f / 9.0f, 8.0f / 9.0f, -8.0f / 9.0f, 0.0f, 8.0f / 9.0f,
            -6.0f / 9.0f, 0.0f, 8.0f / 9.0f, -4.0f / 9.0f, 0.0f, 8.0f / 9.0f, -2.0f / 9.0f, 0.0f, 8.0f / 9.0f, 0.0f, 0.0f,
            8.0f / 9.0f, 2.0f / 9.0f, 0.0f, 8.0f / 9.0f, 4.0f / 9.0f, 0.0f, 8.0f / 9.0f, 6.0f / 9.0f, 0.0f, 8.0f / 9.0f, 8.0f / 9.0f,
            0.0f, 8.0f / 9.0f, -8.0f / 9.0f, 2.0f / 9.0f, 8.0f / 9.0f, -6.0f / 9.0f, 2.0f / 9.0f, 8.0f / 9.0f, -4.0f / 9.0f,
            2.0f / 9.0f, 8.0f / 9.0f, -2.0f / 9.0f, 2.0f / 9.0f, 8.0f / 9.0f, 0.0f, 2.0f / 9.0f, 8.0f / 9.0f, 2.0f / 9.0f, 2.0f / 9.0f,
            8.0f / 9.0f, 4.0f / 9.0f, 2.0f / 9.0f, 8.0f / 9.0f, 6.0f / 9.0f, 2.0f / 9.0f, 8.0f / 9.0f, 8.0f / 9.0f, 2.0f / 9.0f,
            8.0f / 9.0f, -8.0f / 9.0f, 4.0f / 9.0f, 8.0f / 9.0f, -6.0f / 9.0f, 4.0f / 9.0f, 8.0f / 9.0f, -4.0f / 9.0f, 4.0f / 9.0f,
            8.0f / 9.0f, -2.0f / 9.0f, 4.0f / 9.0f, 8.0f / 9.0f, 0.0f, 4.0f / 9.0f, 8.0f / 9.0f, 2.0f / 9.0f, 4.0f / 9.0f, 8.0f / 9.0f,
            4.0f / 9.0f, 4.0f / 9.0f, 8.0f / 9.0f, 6.0f / 9.0f, 4.0f / 9.0f, 8.0f / 9.0f, 8.0f / 9.0f, 4.0f / 9.0f, 8.0f / 9.0f,
            -8.0f / 9.0f, 6.0f / 9.0f, 8.0f / 9.0f, -6.0f / 9.0f, 6.0f / 9.0f, 8.0f / 9.0f, -4.0f / 9.0f, 6.0f / 9.0f, 8.0f / 9.0f,
            -2.0f / 9.0f, 6.0f / 9.0f, 8.0f / 9.0f, 0.0f, 6.0f / 9.0f, 8.0f / 9.0f, 2.0f / 9.0f, 6.0f / 9.0f, 8.0f / 9.0f, 4.0f / 9.0f,
            6.0f / 9.0f, 8.0f / 9.0f, 6.0f / 9.0f, 6.0f / 9.0f, 8.0f / 9.0f, 8.0f / 9.0f,
            6.0f / 9.0f, 8.0f / 9.0f, -8.0f / 9.0f, 8.0f / 9.0f, 8.0f / 9.0f, -6.0f / 9.0f, 8.0f / 9.0f, 8.0f / 9.0f, -4.0f / 9.0f,
            8.0f / 9.0f, 8.0f / 9.0f, -2.0f / 9.0f, 8.0f / 9.0f, 8.0f / 9.0f, 0.0f, 8.0f / 9.0f, 8.0f / 9.0f, 2.0f / 9.0f, 8.0f / 9.0f,
            8.0f / 9.0f, 4.0f / 9.0f, 8.0f / 9.0f, 8.0f / 9.0f, 6.0f / 9.0f, 8.0f / 9.0f, 8.0f / 9.0f, 8.0f / 9.0f, 8.0f / 9.0f,
            8.0f / 9.0f
        };

        // data taken from ISO/IEC DIS 11172, Annexes 3-B.2[abcd] and 3-B.4:

        // subbands 0-2 in tables 3-B.2a and 2b: (index is allocation)
        internal static readonly int[] TableAb1Codelength = {0, 5, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16};

        internal static readonly float[][] TableAb1Groupingtables = {
            null, Grouping5Bits, null, null, null, null,
            null, null, null, null, null, null, null, null, null, null
        };

        internal static readonly float[] TableAb1Factor = {
            0.0f, 1.0f / 2.0f, 1.0f / 4.0f, 1.0f / 8.0f, 1.0f / 16.0f, 1.0f / 32.0f, 1.0f / 64.0f, 1.0f / 128.0f, 1.0f / 256.0f,
            1.0f / 512.0f, 1.0f / 1024.0f, 1.0f / 2048.0f, 1.0f / 4096.0f, 1.0f / 8192.0f, 1.0f / 16384.0f, 1.0f / 32768.0f
        };

        internal static readonly float[] TableAb1C = {
            0.0f, 1.33333333333f, 1.14285714286f, 1.06666666666f, 1.03225806452f, 1.01587301587f, 1.00787401575f,
            1.00392156863f, 1.00195694716f, 1.00097751711f, 1.00048851979f, 1.00024420024f, 1.00012208522f,
            1.00006103888f, 1.00003051851f, 1.00001525902f
        };

        internal static readonly float[] TableAb1D = {
            0.0f, 0.50000000000f, 0.25000000000f, 0.12500000000f, 0.06250000000f, 0.03125000000f, 0.01562500000f,
            0.00781250000f, 0.00390625000f, 0.00195312500f, 0.00097656250f, 0.00048828125f, 0.00024414063f,
            0.00012207031f, 0.00006103516f, 0.00003051758f
        };

        // subbands 3-... tables 3-B.2a and 2b:
        internal static readonly float[][] TableAb234Groupingtables = {
            null, Grouping5Bits, Grouping7Bits, null,
            Grouping10Bits, null, null, null, null, null, null, null, null, null, null, null
        };

        // subbands 3-10 in tables 3-B.2a and 2b:
        internal static readonly int[] TableAb2Codelength = {0, 5, 7, 3, 10, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 16};

        internal static readonly float[] TableAb2Factor = {
            0.0f, 1.0f / 2.0f, 1.0f / 4.0f, 1.0f / 4.0f, 1.0f / 8.0f, 1.0f / 8.0f, 1.0f / 16.0f, 1.0f / 32.0f, 1.0f / 64.0f,
            1.0f / 128.0f, 1.0f / 256.0f, 1.0f / 512.0f, 1.0f / 1024.0f, 1.0f / 2048.0f, 1.0f / 4096.0f, 1.0f / 32768.0f
        };

        internal static readonly float[] TableAb2C = {
            0.0f, 1.33333333333f, 1.60000000000f, 1.14285714286f, 1.77777777777f, 1.06666666666f, 1.03225806452f,
            1.01587301587f, 1.00787401575f, 1.00392156863f, 1.00195694716f, 1.00097751711f, 1.00048851979f,
            1.00024420024f, 1.00012208522f, 1.00001525902f
        };

        internal static readonly float[] TableAb2D = {
            0.0f, 0.50000000000f, 0.50000000000f, 0.25000000000f, 0.50000000000f, 0.12500000000f, 0.06250000000f,
            0.03125000000f, 0.01562500000f, 0.00781250000f, 0.00390625000f, 0.00195312500f, 0.00097656250f,
            0.00048828125f, 0.00024414063f, 0.00003051758f
        };

        // subbands 11-22 in tables 3-B.2a and 2b:
        internal static readonly int[] TableAb3Codelength = {0, 5, 7, 3, 10, 4, 5, 16};

        internal static readonly float[] TableAb3Factor = {
            0.0f, 1.0f / 2.0f, 1.0f / 4.0f, 1.0f / 4.0f, 1.0f / 8.0f,
            1.0f / 8.0f, 1.0f / 16.0f, 1.0f / 32768.0f
        };

        internal static readonly float[] TableAb3C = {
            0.0f, 1.33333333333f, 1.60000000000f, 1.14285714286f, 1.77777777777f, 1.06666666666f, 1.03225806452f,
            1.00001525902f
        };

        internal static readonly float[] TableAb3D = {
            0.0f, 0.50000000000f, 0.50000000000f, 0.25000000000f, 0.50000000000f, 0.12500000000f, 0.06250000000f,
            0.00003051758f
        };

        // subbands 23-... in tables 3-B.2a and 2b:
        internal static readonly int[] TableAb4Codelength = {0, 5, 7, 16};
        internal static readonly float[] TableAb4Factor = {0.0f, 1.0f / 2.0f, 1.0f / 4.0f, 1.0f / 32768.0f};
        internal static readonly float[] TableAb4C = {0.0f, 1.33333333333f, 1.60000000000f, 1.00001525902f};

        internal static readonly float[] TableAb4D = {0.0f, 0.50000000000f, 0.50000000000f, 0.00003051758f};

        // subbands in tables 3-B.2c and 2d:
        internal static readonly int[] TableCdCodelength = {0, 5, 7, 10, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15};

        internal static readonly float[][] TableCdGroupingtables = {
            null, Grouping5Bits, Grouping7Bits,
            Grouping10Bits, null, null, null, null, null, null, null, null, null, null, null, null
        };

        internal static readonly float[] TableCdFactor = {
            0.0f, 1.0f / 2.0f, 1.0f / 4.0f, 1.0f / 8.0f, 1.0f / 8.0f, 1.0f / 16.0f, 1.0f / 32.0f, 1.0f / 64.0f, 1.0f / 128.0f,
            1.0f / 256.0f, 1.0f / 512.0f, 1.0f / 1024.0f, 1.0f / 2048.0f, 1.0f / 4096.0f, 1.0f / 8192.0f, 1.0f / 16384.0f
        };

        internal static readonly float[] TableCdC = {
            0.0f, 1.33333333333f, 1.60000000000f, 1.77777777777f, 1.06666666666f, 1.03225806452f, 1.01587301587f,
            1.00787401575f, 1.00392156863f, 1.00195694716f, 1.00097751711f, 1.00048851979f, 1.00024420024f,
            1.00012208522f, 1.00006103888f, 1.00003051851f
        };

        internal static readonly float[] TableCdD = {
            0.0f, 0.50000000000f, 0.50000000000f, 0.50000000000f, 0.12500000000f, 0.06250000000f, 0.03125000000f,
            0.01562500000f, 0.00781250000f, 0.00390625000f, 0.00195312500f, 0.00097656250f, 0.00048828125f,
            0.00024414063f, 0.00012207031f, 0.00006103516f
        };

        protected int Allocation;
        protected readonly float[] CFactor = {0};
        protected readonly int[] Codelength = {0};

        protected float[] D = {0};

        //protected float[][] groupingtable = {{0},{0}} ;
        protected readonly float[] Factor = {0.0f};
        protected float[][] Groupingtable;
        protected int Groupnumber;
        protected int Samplenumber;
        protected float[] Samples;
        protected float Scalefactor1, Scalefactor2, Scalefactor3;
        protected int Scfsi;
        protected readonly int Subbandnumber;

        internal SubbandLayer2(int subbandnumber) {
            Subbandnumber = subbandnumber;
            Groupnumber = Samplenumber = 0;
        }

        private void InitBlock() {
            Samples = new float[3];
            Groupingtable = new float[2][];
        }

        /// <summary>
        /// *
        /// </summary>
        protected virtual int GetAllocationLength(Header header) {
            if (header.Version() == Header.MPEG1) {
                int channelBitrate = header.bitrate_index();

                // calculate bitrate per channel:
                if (header.Mode() != Header.SINGLE_CHANNEL)
                    if (channelBitrate == 4)
                        channelBitrate = 1;
                    else
                        channelBitrate -= 4;

                if (channelBitrate == 1 || channelBitrate == 2)
                    // table 3-B.2c or 3-B.2d
                    if (Subbandnumber <= 1)
                        return 4;
                    else
                        return 3;
                // tables 3-B.2a or 3-B.2b
                if (Subbandnumber <= 10)
                    return 4;
                if (Subbandnumber <= 22)
                    return 3;
                return 2;
            }
            // MPEG-2 LSF -- Jeff

            // table B.1 of ISO/IEC 13818-3
            if (Subbandnumber <= 3)
                return 4;
            if (Subbandnumber <= 10)
                return 3;
            return 2;
        }

        /// <summary>
        /// *
        /// </summary>
        protected virtual void PrepareForSampleRead(Header header, int allocation, int channel,
            float[] factor, int[] codelength, float[] c, float[] d) {
            int channelBitrate = header.bitrate_index();
            // calculate bitrate per channel:
            if (header.Mode() != Header.SINGLE_CHANNEL)
                if (channelBitrate == 4)
                    channelBitrate = 1;
                else
                    channelBitrate -= 4;

            if (channelBitrate == 1 || channelBitrate == 2) {
                // table 3-B.2c or 3-B.2d
                Groupingtable[channel] = TableCdGroupingtables[allocation];
                factor[0] = TableCdFactor[allocation];
                codelength[0] = TableCdCodelength[allocation];
                c[0] = TableCdC[allocation];
                d[0] = TableCdD[allocation];
            }
            else {
                // tables 3-B.2a or 3-B.2b
                if (Subbandnumber <= 2) {
                    Groupingtable[channel] = TableAb1Groupingtables[allocation];
                    factor[0] = TableAb1Factor[allocation];
                    codelength[0] = TableAb1Codelength[allocation];
                    c[0] = TableAb1C[allocation];
                    d[0] = TableAb1D[allocation];
                }
                else {
                    Groupingtable[channel] = TableAb234Groupingtables[allocation];
                    if (Subbandnumber <= 10) {
                        factor[0] = TableAb2Factor[allocation];
                        codelength[0] = TableAb2Codelength[allocation];
                        c[0] = TableAb2C[allocation];
                        d[0] = TableAb2D[allocation];
                    }
                    else if (Subbandnumber <= 22) {
                        factor[0] = TableAb3Factor[allocation];
                        codelength[0] = TableAb3Codelength[allocation];
                        c[0] = TableAb3C[allocation];
                        d[0] = TableAb3D[allocation];
                    }
                    else {
                        factor[0] = TableAb4Factor[allocation];
                        codelength[0] = TableAb4Codelength[allocation];
                        c[0] = TableAb4C[allocation];
                        d[0] = TableAb4D[allocation];
                    }
                }
            }
        }

        /// <summary>
        /// *
        /// </summary>
        internal override void ReadAllocation(Bitstream stream, Header header, Crc16 crc) {
            int length = GetAllocationLength(header);
            Allocation = stream.GetBitsFromBuffer(length);
            crc?.AddBits(Allocation, length);
        }

        /// <summary>
        /// *
        /// </summary>
        internal virtual void ReadScaleFactorSelection(Bitstream stream, Crc16 crc) {
            if (Allocation != 0) {
                Scfsi = stream.GetBitsFromBuffer(2);
                crc?.AddBits(Scfsi, 2);
            }
        }

        /// <summary>
        /// *
        /// </summary>
        internal override void ReadScaleFactor(Bitstream stream, Header header) {
            if (Allocation != 0) {
                switch (Scfsi) {
                    case 0:
                        Scalefactor1 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        Scalefactor2 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        Scalefactor3 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        break;

                    case 1:
                        Scalefactor1 = Scalefactor2 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        Scalefactor3 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        break;

                    case 2:
                        Scalefactor1 = Scalefactor2 = Scalefactor3 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        break;

                    case 3:
                        Scalefactor1 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        Scalefactor2 = Scalefactor3 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        break;
                }
                PrepareForSampleRead(header, Allocation, 0, Factor, Codelength, CFactor, D);
            }
        }

        /// <summary>
        /// *
        /// </summary>
        internal override bool ReadSampleData(Bitstream stream) {
            if (Allocation != 0)
                if (Groupingtable[0] != null) {
                    int samplecode = stream.GetBitsFromBuffer(Codelength[0]);
                    // create requantized samples:
                    samplecode += samplecode << 1;
                    float[] target = Samples;
                    float[] source = Groupingtable[0];
                    /*
                    int tmp = 0;
                    int temp = 0;
                    target[tmp++] = source[samplecode + temp];
                    temp++;
                    target[tmp++] = source[samplecode + temp];
                    temp++;
                    target[tmp] = source[samplecode + temp];
                    */
                    //Bugfix:
                    int tmp = 0;
                    int temp = samplecode;

                    if (temp > source.Length - 3)
                        temp = source.Length - 3;

                    target[tmp] = source[temp];
                    temp++;
                    tmp++;
                    target[tmp] = source[temp];
                    temp++;
                    tmp++;
                    target[tmp] = source[temp];

                    // memcpy (samples, groupingtable + samplecode, 3 * sizeof (real));
                }
                else {
                    Samples[0] = (float)(stream.GetBitsFromBuffer(Codelength[0]) * Factor[0] - 1.0);
                    Samples[1] = (float)(stream.GetBitsFromBuffer(Codelength[0]) * Factor[0] - 1.0);
                    Samples[2] = (float)(stream.GetBitsFromBuffer(Codelength[0]) * Factor[0] - 1.0);
                }

            Samplenumber = 0;
            if (++Groupnumber == 12)
                return true;
            return false;
        }

        /// <summary>
        /// *
        /// </summary>
        internal override bool PutNextSample(int channels, SynthesisFilter filter1, SynthesisFilter filter2) {
            if (Allocation != 0 && channels != OutputChannels.RIGHT_CHANNEL) {
                float sample = Samples[Samplenumber];
                if (Groupingtable[0] == null)
                    sample = (sample + D[0]) * CFactor[0];
                if (Groupnumber <= 4)
                    sample *= Scalefactor1;
                else if (Groupnumber <= 8)
                    sample *= Scalefactor2;
                else
                    sample *= Scalefactor3;
                filter1.AddSample(sample, Subbandnumber);
            }
            if (++Samplenumber == 3) {
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// public class for layer II subbands in joint stereo mode.
    /// </summary>
    public class SubbandLayer2IntensityStereo : SubbandLayer2 {
        protected float Channel2Scalefactor1, Channel2Scalefactor2, Channel2Scalefactor3;
        protected int Channel2Scfsi;

        internal SubbandLayer2IntensityStereo(int subbandnumber)
            : base(subbandnumber) { }

        /// <summary>
        /// *
        /// </summary>
        internal override void ReadScaleFactorSelection(Bitstream stream, Crc16 crc) {
            if (Allocation != 0) {
                Scfsi = stream.GetBitsFromBuffer(2);
                Channel2Scfsi = stream.GetBitsFromBuffer(2);
                if (crc != null) {
                    crc.AddBits(Scfsi, 2);
                    crc.AddBits(Channel2Scfsi, 2);
                }
            }
        }

        /// <summary>
        /// *
        /// </summary>
        internal override void ReadScaleFactor(Bitstream stream, Header header) {
            if (Allocation != 0) {
                base.ReadScaleFactor(stream, header);
                switch (Channel2Scfsi) {
                    case 0:
                        Channel2Scalefactor1 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        Channel2Scalefactor2 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        Channel2Scalefactor3 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        break;

                    case 1:
                        Channel2Scalefactor1 = Channel2Scalefactor2 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        Channel2Scalefactor3 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        break;

                    case 2:
                        Channel2Scalefactor1 =
                            Channel2Scalefactor2 = Channel2Scalefactor3 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        break;

                    case 3:
                        Channel2Scalefactor1 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        Channel2Scalefactor2 = Channel2Scalefactor3 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        break;
                }
            }
        }

        /// <summary>
        /// *
        /// </summary>
        internal override bool PutNextSample(int channels, SynthesisFilter filter1, SynthesisFilter filter2) {
            if (Allocation != 0) {
                float sample = Samples[Samplenumber];

                if (Groupingtable[0] == null)
                    sample = (sample + D[0]) * CFactor[0];
                if (channels == OutputChannels.BOTH_CHANNELS) {
                    float sample2 = sample;
                    if (Groupnumber <= 4) {
                        sample *= Scalefactor1;
                        sample2 *= Channel2Scalefactor1;
                    }
                    else if (Groupnumber <= 8) {
                        sample *= Scalefactor2;
                        sample2 *= Channel2Scalefactor2;
                    }
                    else {
                        sample *= Scalefactor3;
                        sample2 *= Channel2Scalefactor3;
                    }
                    filter1.AddSample(sample, Subbandnumber);
                    filter2.AddSample(sample2, Subbandnumber);
                }
                else if (channels == OutputChannels.LEFT_CHANNEL) {
                    if (Groupnumber <= 4)
                        sample *= Scalefactor1;
                    else if (Groupnumber <= 8)
                        sample *= Scalefactor2;
                    else
                        sample *= Scalefactor3;
                    filter1.AddSample(sample, Subbandnumber);
                }
                else {
                    if (Groupnumber <= 4)
                        sample *= Channel2Scalefactor1;
                    else if (Groupnumber <= 8)
                        sample *= Channel2Scalefactor2;
                    else
                        sample *= Channel2Scalefactor3;
                    filter1.AddSample(sample, Subbandnumber);
                }
            }

            if (++Samplenumber == 3)
                return true;
            return false;
        }
    }

    /// <summary>
    /// public class for layer II subbands in stereo mode.
    /// </summary>
    public class SubbandLayer2Stereo : SubbandLayer2 {
        protected int Channel2Allocation;
        protected readonly float[] Channel2C = {0};
        protected readonly int[] Channel2Codelength = {0};
        protected readonly float[] Channel2D = {0};
        protected readonly float[] Channel2Factor = {0};
        protected readonly float[] Channel2Samples;
        protected float Channel2Scalefactor1, Channel2Scalefactor2, Channel2Scalefactor3;
        protected int Channel2Scfsi;

        internal SubbandLayer2Stereo(int subbandnumber)
            : base(subbandnumber) {
            Channel2Samples = new float[3];
        }

        /// <summary>
        /// *
        /// </summary>
        internal override void ReadAllocation(Bitstream stream, Header header, Crc16 crc) {
            int length = GetAllocationLength(header);
            Allocation = stream.GetBitsFromBuffer(length);
            Channel2Allocation = stream.GetBitsFromBuffer(length);
            if (crc != null) {
                crc.AddBits(Allocation, length);
                crc.AddBits(Channel2Allocation, length);
            }
        }

        /// <summary>
        /// *
        /// </summary>
        internal override void ReadScaleFactorSelection(Bitstream stream, Crc16 crc) {
            if (Allocation != 0) {
                Scfsi = stream.GetBitsFromBuffer(2);
                crc?.AddBits(Scfsi, 2);
            }
            if (Channel2Allocation != 0) {
                Channel2Scfsi = stream.GetBitsFromBuffer(2);
                crc?.AddBits(Channel2Scfsi, 2);
            }
        }

        /// <summary>
        /// *
        /// </summary>
        internal override void ReadScaleFactor(Bitstream stream, Header header) {
            base.ReadScaleFactor(stream, header);
            if (Channel2Allocation != 0) {
                switch (Channel2Scfsi) {
                    case 0:
                        Channel2Scalefactor1 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        Channel2Scalefactor2 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        Channel2Scalefactor3 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        break;

                    case 1:
                        Channel2Scalefactor1 = Channel2Scalefactor2 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        Channel2Scalefactor3 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        break;

                    case 2:
                        Channel2Scalefactor1 =
                            Channel2Scalefactor2 = Channel2Scalefactor3 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        break;

                    case 3:
                        Channel2Scalefactor1 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        Channel2Scalefactor2 = Channel2Scalefactor3 = ScaleFactors[stream.GetBitsFromBuffer(6)];
                        break;
                }
                PrepareForSampleRead(header, Channel2Allocation, 1, Channel2Factor, Channel2Codelength,
                    Channel2C, Channel2D);
            }
        }

        /// <summary>
        /// *
        /// </summary>
        internal override bool ReadSampleData(Bitstream stream) {
            bool returnvalue = base.ReadSampleData(stream);

            if (Channel2Allocation != 0)
                if (Groupingtable[1] != null) {
                    int samplecode = stream.GetBitsFromBuffer(Channel2Codelength[0]);
                    // create requantized samples:
                    samplecode += samplecode << 1;
                    /*
                    float[] target = channel2_samples;
                    float[] source = channel2_groupingtable[0];
                    int tmp = 0;
                    int temp = 0;
                    target[tmp++] = source[samplecode + temp];
                    temp++;
                    target[tmp++] = source[samplecode + temp];
                    temp++;
                    target[tmp] = source[samplecode + temp];
                    // memcpy (channel2_samples, channel2_groupingtable + samplecode, 3 * sizeof (real));
                    */
                    float[] target = Channel2Samples;
                    float[] source = Groupingtable[1];
                    int tmp = 0;
                    int temp = samplecode;
                    target[tmp] = source[temp];
                    temp++;
                    tmp++;
                    target[tmp] = source[temp];
                    temp++;
                    tmp++;
                    target[tmp] = source[temp];
                }
                else {
                    Channel2Samples[0] =
                        (float)(stream.GetBitsFromBuffer(Channel2Codelength[0]) * Channel2Factor[0] - 1.0);
                    Channel2Samples[1] =
                        (float)(stream.GetBitsFromBuffer(Channel2Codelength[0]) * Channel2Factor[0] - 1.0);
                    Channel2Samples[2] =
                        (float)(stream.GetBitsFromBuffer(Channel2Codelength[0]) * Channel2Factor[0] - 1.0);
                }
            return returnvalue;
        }

        /// <summary>
        /// *
        /// </summary>
        internal override bool PutNextSample(int channels, SynthesisFilter filter1, SynthesisFilter filter2) {
            bool returnvalue = base.PutNextSample(channels, filter1, filter2);
            if (Channel2Allocation != 0 && channels != OutputChannels.LEFT_CHANNEL) {
                float sample = Channel2Samples[Samplenumber - 1];

                if (Groupingtable[1] == null)
                    sample = (sample + Channel2D[0]) * Channel2C[0];

                if (Groupnumber <= 4)
                    sample *= Channel2Scalefactor1;
                else if (Groupnumber <= 8)
                    sample *= Channel2Scalefactor2;
                else
                    sample *= Channel2Scalefactor3;
                if (channels == OutputChannels.BOTH_CHANNELS)
                    filter2.AddSample(sample, Subbandnumber);
                else
                    filter1.AddSample(sample, Subbandnumber);
            }
            return returnvalue;
        }
    }

}

namespace RitaEngine.Resources.Sound.MP3Sharp.Decoding.Decoders.LayerIII 
{
    public class ScaleFactorTable {
        internal int[] L;
        internal int[] S;

        private LayerIIIDecoder _EnclosingInstance;

        internal ScaleFactorTable(LayerIIIDecoder enclosingInstance) {
            InitBlock(enclosingInstance);
            L = new int[5];
            S = new int[3];
        }

        internal ScaleFactorTable(LayerIIIDecoder enclosingInstance, int[] thel, int[] thes) {
            InitBlock(enclosingInstance);
            L = thel;
            S = thes;
        }

        internal LayerIIIDecoder EnclosingInstance => _EnclosingInstance;

        private void InitBlock(LayerIIIDecoder enclosingInstance) {
            _EnclosingInstance = enclosingInstance;
        }
    }

    public class ScaleFactorData {
        internal int[] L; /* [cb] */
        internal int[][] S; /* [window][cb] */

        internal ScaleFactorData() {
            L = new int[23];
            S = new int[3][];
            for (int i = 0; i < 3; i++) {
                S[i] = new int[13];
            }
        }
    }

    public class GranuleInfo {
        internal int BigValues;
        internal int BlockType;
        internal int Count1TableSelect;
        internal int GlobalGain;
        internal int MixedBlockFlag;
        internal int Part23Length;
        internal int Preflag;
        internal int Region0Count;
        internal int Region1Count;
        internal int ScaleFacCompress;
        internal int ScaleFacScale;
        internal int[] SubblockGain;
        internal int[] TableSelect;
        internal int WindowSwitchingFlag;

        internal GranuleInfo() {
            TableSelect = new int[3];
            SubblockGain = new int[3];
        }
    }

    public class ChannelData {
        internal GranuleInfo[] Granules;
        internal int[] ScaleFactorBits;

        internal ChannelData() {
            ScaleFactorBits = new int[4];
            Granules = new GranuleInfo[2];
            Granules[0] = new GranuleInfo();
            Granules[1] = new GranuleInfo();
        }
    }

    public class Layer3SideInfo {
        internal ChannelData[] Channels;
        internal int MainDataBegin;
        internal int PrivateBits;

        internal Layer3SideInfo() {
            Channels = new ChannelData[2];
            Channels[0] = new ChannelData();
            Channels[1] = new ChannelData();
        }
    }

    public class SBI {
        internal int[] L;
        internal int[] S;

        internal SBI() {
            L = new int[23];
            S = new int[14];
        }

        internal SBI(int[] thel, int[] thes) {
            L = thel;
            S = thes;
        }
    }

}

#pragma warning restore