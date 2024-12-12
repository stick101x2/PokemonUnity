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
    public const string SHOOT = "shoot";
    public const string PLRHIT = "player_damaged";
    public const string PLRDEAD = "player_death";
    public const string PLRJUMP = "player_jump";
    public const string PLRLAND = "player_land";
    public const string PLRDASH = "player_dash";
    public const string PLRCHARGE = "player_charging";

    public const int SOURCE_MUSIC = 0;
    public const int SOURCE_MISC1 = 1;
    public const int SOURCE_MISC2 = 2;
    public const int SOURCE_MISC3 = 3;
    public const int SOURCE_MISC4 = 4;
    public const int SOURCE_MISC5 = 5;
    public const int SOURCE_UI1 = 6;
    public const int SOURCE_UI2 = 7;
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
    //   string par = Directory.GetParent(Application.dataPath).FullName + "/Music/MMX_INTRO_STAGE.ogg";

    void ReadMetaData(string filepath)
    {
        DirectoryInfo dir = Directory.GetParent(Application.dataPath);
        string par = Directory.GetParent(Application.dataPath).FullName + "/Music/" + filepath + ".ogg";


        /*
        Debug.Log(par);
        Debug.Log(System.IO.File.Exists(par));

        List<string> fileLines = System.IO.File.ReadAllLines(partext).ToList();
        */

        var tfile = TagLib.File.Create(par);
        var custom = (TagLib.Ogg.XiphComment)tfile.GetTag(TagTypes.Xiph);

        string[] loopStart = custom.GetField("LOOPSTART");
        string[] loopEnd = custom.GetField("LOOPEND");


        double lstart = double.Parse(loopStart[0]);
        double lend = double.Parse(loopEnd[0]);

        /*
        double lstart = double.Parse(fileLines[0]);
        Debug.Log(fileLines[0]);
        double lend = double.Parse(fileLines[1]);
        Debug.Log(fileLines[1]);
        */
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

                Sound s = new Sound();
                s.clip = myClip;
                s.loopStart = start;
                s.loopEnd = end;
                s.group = musicGroup;
                Play(s, SOURCE_MUSIC);
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
    
    public SoundSource Play(string soundName, int index)
    {
        SoundAsset s = Array.Find(soundeffects, sound => sound.sound.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Unable to play " + (soundName) + " on SoundSource " + soundSources[index]);
            return null;
        }
        SoundSource source = soundSources[index];
        source.Play(s.sound);
        return source;
    }
    public SoundSource Play(Sound s, int index)
    {
        SoundSource sound = soundSources[index];
        sound.Play(s);
        return sound;
    }
}
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0, 1)]
    public float volume = 1f;
    public double loopStart = -1;
    public double loopEnd = -1;
    public AudioMixerGroup group;
}
public class SoundSource : MonoBehaviour
{
    AudioSource source;
    Sound sound;
    int loopStartSamples;
    int loopEndSamples;
    int loopLengthSamples;
    int timeSamples;
    public void Setup()
    {
        source = gameObject.AddComponent<AudioSource>();
        enabled = false;
    }

    public void Update()
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

        source.volume = sound.volume;
        source.clip = sound.clip;
        source.outputAudioMixerGroup = sound.group;

        enabled = true;
        source.Play();

    }
}
