namespace WaveDecoderSharp;

public class WaveDecoder : IDisposable
{
    private const int BASE_DATA_CHUNK_SIZE  = 16;
    private const int EXTESION_SIZE         = 22;
    private const int SUB_FORMAT_SIZE       = 16;

    private const string RIFF_CHUNK_NAME    = "RIFF";
    private const string WAVE_CHUNK_NAME    = "WAVE";
    private const string FMT_CHUNK_NAME     = "fmt ";
    private const string DATA_CHUNK_NAME    = "data";

    private readonly Stream _fs;

    public bool EndOfFile   => _fs.Position >= _fs.Length;
    public Stream Stream    => _fs;

    public WaveDecoder(string path)     => _fs = File.OpenRead(path);
    public WaveDecoder(Stream stream)   => _fs = stream;

    public WaveFormat ReadFormat(out int dataSize)
    {
        if (_fs.Position != 0)
            throw new WaveLoadingException("Data already loaded");

        if (!IsRIFFFile())
            throw new WaveLoadingException("Isn't RIFF file");

        SkipRIFFChunkSize();

        if (!IsWAVEFormat())
            throw new WaveLoadingException("Isn't WAVE format");

        JumpToFmtChunk();

        int fmtChunkSize = ReadInt32(_fs);

        ValidateFmtSize(fmtChunkSize, out bool isExtended);

        WaveFormat format = new();

        format.Tag                  = ReadInt16(_fs);
        format.Channels             = ReadInt16(_fs);
        format.SamplesPerSec        = ReadInt32(_fs);
        format.AvgBytesPerSecond    = ReadInt32(_fs);
        format.BlockAlign           = ReadInt16(_fs);
        format.BitsPerSample        = ReadInt16(_fs);

        if (isExtended)
        {
            short extesionSize = ReadInt16(_fs);

            if (extesionSize == EXTESION_SIZE)
            {
                format.Extended = true;

                format.ValidBitsPerSample = ReadInt16(_fs);
                format.ChannelMask        = ReadInt32(_fs);
                format.SubFormat          = ReadBytes(_fs, SUB_FORMAT_SIZE);
            }
            else if (extesionSize != 0)
                throw new WaveLoadingException("Wrong extension size");
        }

        JumpToDataChunk();

        dataSize = ReadInt32(_fs);

        return format;
    }

    public byte[] ReadBuffer(int size)
    {
        if (CanRead(size))
            return ReadBytes(_fs, size);

        throw new ArgumentOutOfRangeException();
    }

    private bool CanRead(int bytesCount) => _fs.Position + bytesCount <= _fs.Length;

    private  bool IsRIFFFile()      => IsSameChunk(RIFF_CHUNK_NAME);
    private  bool IsWAVEFormat()    => IsSameChunk(WAVE_CHUNK_NAME);

    private void SkipRIFFChunkSize() => SkipBytes(_fs, 4);

    private  void JumpToFmtChunk()  => JumpToChunk(FMT_CHUNK_NAME);
    private  void JumpToDataChunk() => JumpToChunk(DATA_CHUNK_NAME);

    private  void JumpToChunk(string name)
    {
        while (!EndOfFile)
        {
            if (IsSameChunk(name))
                return;

            int chunkSize = ReadInt32(_fs);

            SkipBytes(_fs, chunkSize);
        }

        throw new WaveLoadingException($"Can't find chunk: {name}");
    }

    //Размер имени чанка всегда занимает 4 байта
    //Мы просто считываем их последовательно, сравнивая с искомым именем
    private  bool IsSameChunk(string name)
        => _fs.ReadByte() == name[0] &
           _fs.ReadByte() == name[1] &
           _fs.ReadByte() == name[2] &
           _fs.ReadByte() == name[3];

    private void ValidateFmtSize(int size, out bool isExtended)
    {
        if (size == BASE_DATA_CHUNK_SIZE)
            isExtended = false;
        else if (size >= BASE_DATA_CHUNK_SIZE + 2) //BASE_DATA_CHUNK_SIZE + 2 extension size bytes
            isExtended = true;
        else
            throw new WaveLoadingException("Wrong fmt chunk size");
    }

    private byte[] ReadBytes(Stream fs, int count)
    {
        byte[] bytes = new byte[count];

        fs.Read(bytes, 0, count);

        return bytes;
    }

    private void SkipBytes(Stream fs, int count) => fs.Position += count;

    private int ReadInt32(Stream fs)
    {
        byte[] int32 = ReadBytes(fs, 4);

        return BitConverter.ToInt32(int32);
    }

    private short ReadInt16(Stream fs)
    {
        byte[] bytes = ReadBytes(fs, 2);

        return BitConverter.ToInt16(bytes);
    }

    public void Dispose() => _fs.Dispose();
}