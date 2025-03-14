using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSound : MonoBehaviour
{
    public string audioName;
    public int index;

    public void Play()
    {
        AudioManager.Play(audioName,index);
    }
}
