using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAudioManager : MonoBehaviour
{
    public static AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
}
