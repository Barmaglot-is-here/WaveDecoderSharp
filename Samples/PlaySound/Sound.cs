using Vortice.XAudio2;

public class Sound : IDisposable
{
    private readonly IXAudio2SourceVoice _source;
    private readonly AudioBuffer _buffer;

    public static Sound Load(string path, SoundEngine soundEngine) 
        => SoundLoader.Load(path, soundEngine);

    internal Sound(IXAudio2SourceVoice source, AudioBuffer buffer)
    {
        _source     = source;
        _buffer     = buffer; 

        source.SubmitSourceBuffer(buffer);
    }

    public void Play()  => _source.Start();
  
    public void Dispose()
    {
        _source.Dispose();
        _buffer.Dispose();
    }
}