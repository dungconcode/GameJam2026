using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--- Audio Source ---")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("--- Audio Clips ---")]
    public AudioClip background;

    public void PlayMusicBG()
    {
        Debug.Log("Dung ngu");
        musicSource.clip = background;
        musicSource.Play();
    }
}
