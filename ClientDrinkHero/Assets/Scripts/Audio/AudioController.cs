using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;
    public AudioTrack[] tracks;
    
    private Hashtable _audioTable;
    private Hashtable _jobTable;

    public bool debug;

    public void PlayAudio(AudioType type, bool fade=false, float delay=0.0f)
    {
        AddJob(new AudioJob(AudioJobType.Start, type, fade, delay));
    }
    
    public void StopAudio(AudioType type, bool fade=false, float delay=0.0f)
    {
        AddJob(new AudioJob(AudioJobType.Start, type, fade, delay));
    }
    
    public void RestartAudio(AudioType type, bool fade=false, float delay=0.0f)
    {
        AddJob(new AudioJob(AudioJobType.Start, type, fade, delay));
    }

    private AudioClip GetAudioClipFromAudioTrack(AudioType type, AudioTrack track)
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
        public bool Fade;
        public float Delay;

        public AudioJob(AudioJobType jobType, AudioType audioType, bool fade, float delay)
        {
            JobType = jobType;
            Type = audioType;
            Fade = fade;
            Delay = delay;
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

        GetCurrentVolume();
    }

    private void GetCurrentVolume()
    {
        tracks[0].Source.volume = 0f;
        tracks[1].Source.volume = UIDataContainer.SfxVolume;
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
        StartCoroutine(jobRunner);
        
        Log($"Starting job on: {audioJob.Type} with operation: {audioJob.JobType}.");
    }

    private IEnumerator RunAudioJob(AudioJob audioJob)
    {
        yield return new WaitForSeconds(audioJob.Delay);
        
        AudioTrack track = (AudioTrack)_audioTable[audioJob.Type];
        track.Source.clip = GetAudioClipFromAudioTrack(audioJob.Type, track);

        switch (audioJob.JobType)
        {
            case AudioJobType.Start:
                track.Source.Play();
                break;
            
            case AudioJobType.Stop:
                if (!audioJob.Fade)
                {
                    track.Source.Stop();
                }
                break;
            
            case AudioJobType.Restart:
                track.Source.Stop();
                track.Source.Play();
                break;
        }

        if (audioJob.Fade)
        {
            // TODO: should take current volume into account
            float lastVolume = UIDataContainer.MusicVolume;
            
            float initialVolume = audioJob.JobType is AudioJobType.Start or AudioJobType.Restart
                ? 0f
                : 1f;
            //float targetVolume = initialVolume == 0 ? 1 : 0;
            float targetVolume = initialVolume == 0 ? lastVolume : 0;
            
            float fadeDuration = 5f;

            float timer = 0f;

            while (timer <= fadeDuration)
            {
                track.Source.volume = Mathf.Lerp(initialVolume, targetVolume, timer / fadeDuration);
                timer += Time.deltaTime;
                
                yield return null;
            }

            if (audioJob.JobType == AudioJobType.Stop)
            {
                track.Source.Stop();
            }
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

