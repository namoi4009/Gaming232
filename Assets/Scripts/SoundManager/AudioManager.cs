using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("------------- Audio Source -------------")]
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("------------- Audio Clip -------------")]
    [SerializeField] public AudioClip Menu_Background;
    [SerializeField] public AudioClip MouseClick;

    public void PlaySFX_OneTime(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlaySFX_Loop(AudioClip clip)
    {
        SFXSource.clip = clip;
        SFXSource.loop = true;
        SFXSource.Play();
    }

    public void StopSFX()
    {
        SFXSource.Stop();
        SFXSource.loop= false;
    }
}