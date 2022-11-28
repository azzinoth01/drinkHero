using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudioManager : MonoBehaviour
{
    private AudioSource _sfxAudioSource;
    public AudioSource SfxAudioSource => _sfxAudioSource;

    public static GlobalAudioManager Instance;

    [Header("AudioEvents")] [SerializeField]
    private SimpleAudioEvent _playerDamage;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        _sfxAudioSource = GetComponent<AudioSource>();
    }

    public void Play(SimpleAudioEvent audioEvent)
    {
        audioEvent.Play(_sfxAudioSource);
    }
}