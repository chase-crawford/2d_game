using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    public static SoundMixerManager instance;

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private GameObject menuUI;

    public void SetMasterVolume(float vol)
    {
        mixer.SetFloat("masterVolume", Mathf.Log10(vol) * 20);
    }

    public void SetSFXVolume(float vol)
    {
        mixer.SetFloat("sfxVolume", Mathf.Log10(vol) * 20);
    }

    public void SetMusicVolume(float vol)
    {
        mixer.SetFloat("musicVolume", Mathf.Log10(vol) * 20);
    }

    void Awake()
    {
        menuUI.SetActive(false);

        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!(GameManager.instance.inMenu && !menuUI.activeSelf))
                SetMenuVisibility(!menuUI.activeSelf);
        }
    }

    public void SetMenuVisibility(bool visible)
    {
        menuUI.SetActive(visible);

        GameManager.instance.inMenu = menuUI.activeSelf;

        if (visible)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
