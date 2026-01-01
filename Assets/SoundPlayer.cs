using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public SoundAsset[] audios;
    public int channelIndex;

    public void Play(int audioIndex)
    {
        Sound s = audios[audioIndex].sound;


        AudioManager.Play(s, channelIndex);
    }
}
