using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioSource musicSource;

    // ��������������� ������
    public void PlaySound(AudioClip _sound)
    {
        soundSource.PlayOneShot(_sound);
    }

    // ���������� ������
    public void SetMusic(AudioClip _music)
    {
        musicSource.clip = _music;
        musicSource.loop = true;
    }

    // ��������������� ������
    public void PlayMusic()
    {
        musicSource.Play();
    }

    // ��������� ��������������� ������
    public void StopMusic()
    {
        musicSource.Stop();
    }
}