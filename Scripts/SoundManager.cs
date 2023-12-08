using Sirenix.OdinInspector;
using UnityEngine;

public class SoundManager : SingletonClass.Singleton<SoundManager>
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSourcePitch;


    [Title("Common Sounds")] 
    [SerializeField] private AudioClip audioClip;

    public bool PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("Audio clip is empty..");
            return false;
        }

        audioSource.PlayOneShot(clip);
        return true;
    }

    public void PlaySoundWithPitch(AudioClip clip, float pitchRate)
    {
        if (clip == null)
        {
            Debug.LogWarning("Audio clip is empty..");
            return;
        }

        audioSourcePitch.pitch = pitchRate;
        audioSourcePitch.PlayOneShot(clip, pitchRate);
    }

    public void PlayPurchaseSound()
    {
        PlaySound(audioClip);
    }
}