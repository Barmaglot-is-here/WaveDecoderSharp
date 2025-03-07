using Vortice.Multimedia;
using Vortice.XAudio2;
using WaveDecoderSharp;

internal static class SoundLoader
{
    public static Sound Load(string path, SoundEngine soundEngine)
    {
        using WaveDecoder loader = new(path);

        WaveDecoderSharp.WaveFormat waveData    = loader.ReadFormat(out int dataSize);
        byte[] buffer                           = loader.ReadBuffer(dataSize);

        AudioBuffer audioBuffer = new(buffer, BufferFlags.EndOfStream);

        Vortice.Multimedia.WaveFormat waveFormat = Vortice.Multimedia.WaveFormat.CreateCustomFormat
        (
            (WaveFormatEncoding)waveData.Tag,
            waveData.SamplesPerSec,
            waveData.Channels,
            waveData.AvgBytesPerSecond,
            waveData.BlockAlign,
            waveData.BitsPerSample
        );

        IXAudio2SourceVoice source;

        if (waveData.Extended)
        {
            WaveFormatExtensible waveFormatExtensible = (WaveFormatExtensible)waveFormat;

            waveFormatExtensible.ChannelMask    = waveData.ChannelMask;
            waveFormatExtensible.GuidSubFormat  = new(waveData.SubFormat);

            source = soundEngine.CreateSource(waveFormatExtensible);
        }
        else
            source = soundEngine.CreateSource(waveFormat);

        return new(source, audioBuffer);
    }
}