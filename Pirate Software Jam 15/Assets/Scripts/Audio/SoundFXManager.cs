using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource sfxObject;
    [SerializeField] private AudioSource musicObject;

    public AudioClip bgSfx;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        AudioSource audioSource = Instantiate(musicObject, transform.position, Quaternion.identity);
        audioSource.clip = bgSfx;
        audioSource.volume = 1f;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlaySoundClip (AudioClip clip, Transform spawnpoint, float volume)
    {
        // make audio obj
        AudioSource audioSource = Instantiate(sfxObject, spawnpoint.position, Quaternion.identity);

        // set clip of audio
        audioSource.clip = clip;

        // set volume
        audioSource.volume = volume;

        // Play sound
        audioSource.Play();

        // get audio clip length
        float length = audioSource.clip.length;

        // destroy audio obj after clip is done playing
        Destroy(audioSource.gameObject, length);
    }

    public void PlayRandomSoundClip (AudioClip[] clips, Transform spawnpoint, float volume)
    {
        // get random clip
        int rand = Random.Range(0, clips.Length);

        // make audio obj
        AudioSource audioSource = Instantiate(sfxObject, spawnpoint.position, Quaternion.identity);

        // set clip of audio
        audioSource.clip = clips[rand];

        // set volume
        audioSource.volume = volume;

        // Play sound
        audioSource.Play();

        // get audio clip length
        float length = audioSource.clip.length;

        // destroy audio obj after clip is done playing
        Destroy(audioSource.gameObject, length);
    }
}
