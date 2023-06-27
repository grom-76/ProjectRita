namespace RitaEngine.Resources;

using System.IO;
using RitaEngine.Resources.Sound.MP3Sharp;
using RitaEngine.Resources.Sound;
public static class SoundsHelper
{
    public static void ReadMp3(string filename, int chunksize)
    {
        // open the mp3 file.
        MP3Stream stream = new MP3Stream(filename,chunksize);

        
        // Create the buffer.
        byte[] buffer = new byte[chunksize];
        // read the entire mp3 file.
        int bytesReturned = 1;
        int totalBytesRead = 0;
        while (bytesReturned > 0)
        {
            bytesReturned = stream.Read(buffer, 0, buffer.Length);
            totalBytesRead += bytesReturned;
        }
        // close the stream after we're done with it.
        stream.Close();
    }

    public static void ReadMp3Stream(string filename, int chunksize=4096)
    {
        // open the mp3 file.
        MP3Stream stream = new MP3Stream(filename,chunksize);

        
        // Create the buffer.
        byte[] buffer = new byte[chunksize];
        // read the entire mp3 file.
        int bytesReturned = 1;
        int totalBytesRead = 0;
        while (bytesReturned > 0)
        {
            bytesReturned = stream.Read(buffer, 0, buffer.Length);
            totalBytesRead += bytesReturned;
        }
        // close the stream after we're done with it.
        stream.Close();
    }

    public static void ReadMp3StreamBuffer( ref byte[] buffer, MP3Stream stream ,int chunksize )
    {
        int bytesReturned = stream.Read(buffer, 0,buffer.Length);
            if (bytesReturned != chunksize) {
                // if (_Repeat) {
                //     _Stream.Position = 0;
                //     _Stream.Read(_WaveBuffer, bytesReturned, _WaveBuffer.Length - bytesReturned);
                // }
                // else {
                //     if (bytesReturned == 0) {
                //         Stop();
                //     }
                // }
            }
    }




    public static void ReadOgg(string filename)
    {
        // var path = Path.GetDirectoryName(path: Assembly.GetEntryAssembly()?.Location);
        // path = Path.Combine(path!, "music.ogg");
        var buffer = File.ReadAllBytes(filename);

        int sampleRate, channels;

        var audioShort = StbVorbis.decode_vorbis_from_memory(buffer, out sampleRate, out channels);

        byte[] audioData = new byte[audioShort.Length * 2];
        for (var i = 0; i < audioShort.Length; ++i)
        {
            if (i * 2 >= audioData.Length)
            {
                break;
            }

            var b1 = (byte)(audioShort[i] >> 8);
            var b2 = (byte)(audioShort[i] & 256);

            audioData[i * 2 + 0] = b2;
            audioData[i * 2 + 1] = b1;
        }

        // soundEffect = new SoundEffect(audioData, sampleRate, (AudioChannels)channels);
        // soundEffect.Play();
    }

    public static void ReadOggBuffer(Vorbis _vorbis, string filename)
    {
            // var path = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            // path = Path.Combine(path!, "music.ogg");
            var buffer = File.ReadAllBytes(filename);

            _vorbis = Vorbis.FromMemory(buffer);

            // soundEffect _effect = new DynamicSoundEffectInstance(_vorbis.SampleRate, (AudioChannels)_vorbis.Channels)
            // soundEffect = new SoundEffect(audioData, sampleRate, (AudioChannels)channels);
            // soundEffect.Play();

            //ReadNexBuffer(); => _effect.BufferNeeded += (s, a) => SubmitBuffer();
    }

    public static void ReadOggNexBuffer(Vorbis _vorbis/*, Source source _effect*/)
    {
        _vorbis.SubmitBuffer();

        if (_vorbis.Decoded == 0)
        {
            // Restart
            _vorbis.Restart();
            _vorbis.SubmitBuffer();
        }

        var audioShort = _vorbis.SongBuffer;
        byte[] audioData = new byte[_vorbis.Decoded * _vorbis.Channels * 2];
        for (var i = 0; i < _vorbis.Decoded * _vorbis.Channels; ++i)
        {
            if (i * 2 >= audioData.Length)
            {
                break;
            }

            var b1 = (byte)(audioShort[i] >> 8);
            var b2 = (byte)(audioShort[i] & 256);

            audioData[i * 2 + 0] = b2;
            audioData[i * 2 + 1] = b1;
        }

        // _effect.SubmitBuffer(audioData);
    }


}
