using WaveDecoderSharp;

namespace Samples;

internal class SoundWatcher
{
    private readonly WaveFormat _format;

    public SoundWatcher(string soundPath)
    {
        using WaveDecoder loader = new(soundPath);

        _format = loader.ReadFormat(out int dataSize);
    }

    public void ShowInfo()
    {
        string info =
            $"Tag: {_format.Tag}\n" +
            $"Channels: {_format.Channels}\n" +
            $"SamplesPerSec: {_format.SamplesPerSec}\n" +
            $"AvgBytesPerSecond: {_format.AvgBytesPerSecond}\n" +
            $"BlockAlign: {_format.BlockAlign}\n" +
            $"BitsPerSample: {_format.BitsPerSample}\n" +
            $"Extended: {_format.Extended}";

        Console.WriteLine();
        Console.WriteLine("---INFO---");
        Console.WriteLine(info);
        Console.WriteLine("----------");
    }
}