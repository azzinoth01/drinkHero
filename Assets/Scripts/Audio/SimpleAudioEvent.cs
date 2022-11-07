using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Audio Events/Simple")]
public class SimpleAudioEvent : AudioEvent
{
    public AudioClip[] clips;
    [MinMaxRange(0, 2)] public RangedFloat pitchRange;

    private AudioSource _source;
    
    private void Awake()
    {
    }

    public override void Play(AudioSource source)
    {
        if (clips.Length == 0) return;
        
        source.pitch = Random.Range(pitchRange.minValue, pitchRange.maxValue);
        source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
}