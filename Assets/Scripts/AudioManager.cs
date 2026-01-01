using System;
using UnityEngine;
using TagLib;
using System.Linq;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.Audio;




public class AudioManager : MonoBehaviour
{
   
    public AudioMixerGroup musicGroup;
    public SoundAsset[] soundeffects;
    SoundSource[] soundSources;
    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        soundSources = new SoundSource[8];

        for (int i = 0; i < soundSources.Length; i++)
        {
            GameObject obj = new GameObject("SoundSource " + i);
            soundSources[i] = obj.AddComponent<SoundSource>();
            soundSources[i].Setup();
            obj.transform.parent = transform;
        }

        soundeffects = Resources.LoadAll<SoundAsset>("Sounds/SFX");
    }
    public void PlayExternal(string filename)
    {
        ReadMetaData(filename);
    }

    void ReadMetaData(string filepath)
    {
        DirectoryInfo dir = Directory.GetParent(Application.dataPath);
        string par = Directory.GetParent(Application.dataPath).FullName + "/Music/" + filepath + ".ogg";

        var tfile = TagLib.File.Create(par);
        var custom = (TagLib.Ogg.XiphComment)tfile.GetTag(TagTypes.Xiph);

        string[] loopStart = custom.GetField("LOOPSTART");
        string[] loopEnd = custom.GetField("LOOPEND");


        double lstart = double.Parse(loopStart[0]);
        double lend = double.Parse(loopEnd[0]);

        StartCoroutine(LoadAudioFromFile(par, lstart, lend));
    }

    public IEnumerator LoadAudioFromFile(string path, double start, double end)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.OGGVORBIS))
        {
            // ((DownloadHandlerAudioClip)www.downloadHandler).streamAudio = true;

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);

                Sound s = new()
                {
                    clip = myClip,
                    loopStart = start,
                    loopEnd = end,
                    group = musicGroup
                };
                Play(s, Constants.MUSIC);
            }
        }
    }
    public void StopAllSources()
    {
        for (int i = 0; i < soundSources.Length; i++)
        {
            soundSources[i].Stop();
        }
    }
    public void Stop(int sourceIndex)
    {
        soundSources[sourceIndex].Stop();
    }
    public void Stop(SoundSource source)
    {
        source.Stop();
    }
    
    public static SoundSource Play(string soundName, int index)
    {
        SoundAsset s = Array.Find(instance.soundeffects, sound => sound.sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Unable to play " + (soundName) + " on SoundSource " + instance.soundSources[index]);
            return null;
        }
        SoundSource source = instance.soundSources[index];
        source.Play(s.sound);
        return source;
    }
    public static SoundSource Play(string soundName, int index,float pitch)
    {
        SoundAsset s = Array.Find(instance.soundeffects, sound => sound.sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Unable to play " + (soundName) + " on SoundSource " + instance.soundSources[index]);
            return null;
        }
        SoundSource source = instance.soundSources[index];
        Sound fs = s.sound;
        fs.pitch = pitch;
        source.Play(fs);
        return source;
    }
    public static SoundSource Play(Sound s, int index, float pitch)
    {
        SoundSource source = instance.soundSources[index];
        Sound fs = new(s)
        {
            pitch = pitch
        };
        source.Play(fs);
        return source;
    }
    public static SoundSource Play(Sound s, int index)
    {
        SoundSource source = instance.soundSources[index];

        if(index == Constants.POKE)
        {
            instance.soundSources[0].SetVolume(0.4f,1f,s.clip.length+0.25f);
        }

        source.Play(s);
        return source;
    }
}
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0, 1)]
    public float volume = 1f;
    [Range(-1, 3)]
    public float pitch  = 1f;
    public double loopStart = -1;
    public double loopEnd = -1;
    public AudioMixerGroup group;
    public Sound() { }
    public Sound(Sound s)
    {
        name = s.name;
        clip = s.clip;
        volume = s.volume;
        pitch = s.pitch;
        loopEnd = s.loopEnd;
        loopStart = s.loopStart;
        group = s.group;
    }
}
public class SoundSource : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] Sound sound;
    [SerializeField] int loopStartSamples;
    [SerializeField] int loopEndSamples;
    [SerializeField] int loopLengthSamples;
    [SerializeField] int timeSamples;

    [SerializeField] float volume;
    [SerializeField] float volumeChangeSpeed;
    [SerializeField] float duration = -1000;
    public void Setup()
    {
        source = gameObject.AddComponent<AudioSource>();
        enabled = false;
    }

    public void Update()
    {

        if(duration > 0)
        {
            duration -= Time.deltaTime;
            if(duration < 0 )
            {
                volume = sound.volume;
            }
        }

        VolumeChange();

        Looping();
    }

    public void SetVolume(float setVolume,float setChangeSpeed = 1f,float setDuration = -1000)
    {
        volumeChangeSpeed = setChangeSpeed;
        duration = setDuration; 
        volume = setVolume;
    }

    void VolumeChange()
    {
        if (volume == source.volume)
            return;

       

        float dir = volume > source.volume ? 1f : -1f;

        source.volume += volumeChangeSpeed * dir * Time.deltaTime;

        if(dir > 0 && (source.volume > volume || source.volume == volume))
        {
            source.volume = volume;
        }
        else if (dir < 0 && (source.volume < volume || source.volume == volume))
        {
            source.volume = volume;
        }
    }
    void Looping()
    {
        if (sound.loopStart < 0 || sound.loopEnd < 0)
            return;

        if (!source.isPlaying)
            return;
        timeSamples = source.timeSamples;
        if (source.timeSamples >= loopEndSamples)
            source.timeSamples -= loopLengthSamples;
    }
    public void ResetSource()
    {
        enabled = false;
        loopStartSamples = 0;
        loopEndSamples = 0;
        loopLengthSamples = 0;
        timeSamples = 0;
        sound = null;

    }
    public void Stop()
    {
        source.Stop();
        ResetSource();
    }
    public void Play(Sound s)
    {
        source.Stop();
        sound = s;

        loopStartSamples = (int)(s.loopStart * s.clip.frequency);
        loopEndSamples = (int)(s.loopEnd * s.clip.frequency);
        loopLengthSamples = loopEndSamples - loopStartSamples;

        volume = sound.volume;

        source.volume = sound.volume;
        source.pitch = sound.pitch;
        source.clip = sound.clip;
        source.outputAudioMixerGroup = sound.group;

        enabled = true;
        source.Play();

    }
}
