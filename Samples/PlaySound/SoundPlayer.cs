namespace Samples.PlaySound;

internal class SoundPlayer
{
    private readonly SoundEngine _soundEngine;
    private readonly Sound _sound;


    public SoundPlayer(string soundPath)
    {
        _soundEngine    = new();
        _sound          = Sound.Load(soundPath, _soundEngine);
    }

    public void Play() => _sound.Play();
}