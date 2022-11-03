using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField, Range(0.1f, 5f)] private float fadeDuration = 0.25f;
    [SerializeField, Range(0.1f, 1f)] private float fadeVolume = 1f;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnDisable()
    {
    }

    private void FadeInTrack(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
        _audioSource.DOFade(fadeVolume, fadeDuration);
    }
    
    private void FadeOutTrack()
    {
        _audioSource.DOFade(0, fadeDuration);
    }
    
}
