using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;
    public AudioTrack[] tracks;
    
    private Hashtable _audioTable;
    private Hashtable _jobTable;

    public bool debug;

    public void PlayAudio(AudioType type)
    {
        AddJob(new AudioJob(AudioJobType.Start, type));
    }
    
    public void StopAudio(AudioType type)
    {
        AddJob(new AudioJob(AudioJobType.Stop, type));
    }
    
    public void RestartAudio(AudioType type)
    {
        AddJob(new AudioJob(AudioJobType.Restart, type));
    }
    
    public AudioClip GetAudioClipFromAudioTrack(AudioType type, AudioTrack track)
    {
        foreach (var audioObject in track.Audio)
        {
            if (audioObject.Type == type)
            {
                return audioObject.Clip;
            }
        }

        return null;
    }

    private class AudioJob
    {
        public AudioJobType JobType;
        public AudioType Type;

        public AudioJob(AudioJobType jobType, AudioType audioType)
        {
            JobType = jobType;
            Type = audioType;
        }
    }
    
    private enum AudioJobType
    {
        Start,
        Stop,
        Restart
    }
    
    private void Awake()
    {
        if (!Instance)
        {
            Configure();
        }
    }

    private void OnDisable()
    {
        Dispose();
    }

    private void Configure()
    {
        Instance = this;
        
        _audioTable = new Hashtable();
        _jobTable = new Hashtable();
        
        GenerateAudioTable();
    }

    private void Dispose()
    {
        foreach (DictionaryEntry entry in _jobTable)
        {
            IEnumerator job = (IEnumerator)entry.Value;
            StopCoroutine(job);
        }
    }

    private void AddJob(AudioJob audioJob)
    {
        RemoveConflictingJobs(audioJob.Type);
        IEnumerator jobRunner = RunAudioJob(audioJob);
        
        _jobTable.Add(audioJob.Type, jobRunner);
        Log($"Starting job on: {audioJob.Type} with operation: {audioJob.JobType}.");
    }

    private IEnumerator RunAudioJob(AudioJob audioJob)
    {
        AudioTrack track = (AudioTrack)_audioTable[audioJob.Type];
        track.Source.clip = GetAudioClipFromAudioTrack(audioJob.Type, track);

        switch (audioJob.JobType)
        {
            case AudioJobType.Start:
                track.Source.Play();
                break;
            
            case AudioJobType.Stop:
                track.Source.Stop();
                break;
            
            case AudioJobType.Restart:
                track.Source.Stop();
                track.Source.Play();
                break;
        }
        _jobTable.Remove(audioJob.Type);
        Log($"Job Count: {_jobTable.Count}.");

        yield return null;
    }

    private void RemoveJob(AudioType type)
    {
        if (!_jobTable.ContainsKey(type))
        {
            LogWarning($"Trying to stop a job {type} that is not running.");
            return;
        }

        IEnumerator runningJob = (IEnumerator)_jobTable[type];
        StopCoroutine(runningJob);
        
        _jobTable.Remove(type);
    }

    private void RemoveConflictingJobs(AudioType type)
    {
        if (_jobTable.ContainsKey(type))
        {
            RemoveJob(type);
        }

        AudioType conflictAudio = AudioType.None;

        foreach (DictionaryEntry entry in _jobTable)
        {
            AudioType audioType = (AudioType)entry.Key;
            AudioTrack audioTrackInUse = (AudioTrack)_audioTable[audioType];
            AudioTrack audioTrackNeeded = (AudioTrack)_audioTable[type];

            if (audioTrackNeeded.Source == audioTrackInUse.Source)
            {
                conflictAudio = audioType;
            }
        }

        if (conflictAudio != AudioType.None)
        {
            RemoveJob(conflictAudio);
        }
    }

    private void GenerateAudioTable()
    {
        foreach (var track in tracks)
        {
            foreach (var audioObject in track.Audio)
            {
                if (_audioTable.ContainsKey(audioObject.Type))
                {
                    LogWarning($"{audioObject.Type} is already registered!");
                }
                else
                {
                    _audioTable.Add(audioObject.Type, track);
                    Log($"Registering [{audioObject.Type}]!");
                }
            }
        }
    }

    private void Log(string message)
    {
        if (!debug) return;
        Debug.Log($"[AudioController]: {message}");
    }
    
    private void LogWarning(string message)
    {
        if (!debug) return;
        Debug.LogWarning($"[AudioController]: {message}");
    }
}

