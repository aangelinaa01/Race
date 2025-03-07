using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public Slider slider;
    public AudioClip clip;
    public AudioSource audioSource;
    static AudioController instance;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            PlaySound();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        audioSource.volume = slider.value;
    }

    public static void PlaySound()
    {
        instance.audioSource.PlayOneShot(instance.clip);
    }
}
