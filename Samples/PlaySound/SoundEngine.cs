using Vortice.Multimedia;
using Vortice.XAudio2;

public class SoundEngine : IDisposable
{
    private readonly IXAudio2 _xAudio;
    private readonly IXAudio2MasteringVoice _masteringVoice;

    public SoundEngine()
    {
        _xAudio         = XAudio2.XAudio2Create();
        _masteringVoice = _xAudio.CreateMasteringVoice();

        _xAudio.StartEngine();
    }

    internal IXAudio2SourceVoice CreateSource(WaveFormat waveFormat)
    {
        var voice = _xAudio.CreateSourceVoice(waveFormat, false);

        return voice;
    }

    public void Dispose()
    {
        _xAudio.Dispose();
        _masteringVoice.Dispose();
    }
}