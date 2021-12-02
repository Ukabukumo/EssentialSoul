using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioSource musicSource;

    // Воспроизведение звуков
    public void PlaySound(AudioClip _sound)
    {
        soundSource.PlayOneShot(_sound);
    }

    // Назначение музыки
    public void SetMusic(AudioClip _music)
    {
        musicSource.clip = _music;
        musicSource.loop = true;
    }

    // Воспроизведение музыки
    public void PlayMusic()
    {
        musicSource.Play();
    }

    // Останавка воспроизведения музыки
    public void StopMusic()
    {
        musicSource.Stop();
    }
}