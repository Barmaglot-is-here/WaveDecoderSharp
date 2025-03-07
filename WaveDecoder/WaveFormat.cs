namespace WaveDecoderSharp;

public struct WaveFormat
{
    public short Tag { get; set; }
    public short Channels { get; set; }
    public int SamplesPerSec { get; set; }
    public int AvgBytesPerSecond { get; set; }
    public short BlockAlign { get; set; }
    public short BitsPerSample { get; set; }

    public bool Extended { get; set; }

    public short ValidBitsPerSample { get; set; }
    public int ChannelMask { get; set; }
    public byte[] SubFormat { get; set; }
}