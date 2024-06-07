using UnityEngine;

public class IngameSound : MonoBehaviour
{
    [Header("------------- Audio Source -------------")]
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("------------- Audio Clip -------------")]
    [SerializeField] public AudioClip Ingame_Background;
    [SerializeField] public AudioClip MouseClick;
    [SerializeField] public AudioClip LosingGame;
    [SerializeField] public AudioClip CarCrash;
    [SerializeField] public AudioClip RoadNoise;

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
        SFXSource.loop = false;
    }
}
