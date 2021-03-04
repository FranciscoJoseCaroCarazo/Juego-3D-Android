using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenú : MonoBehaviour
{

    public AudioMixer audioMixer;

    public void setVolume(float volumen)
    {
        audioMixer.SetFloat("volume", volumen);
    }
}
