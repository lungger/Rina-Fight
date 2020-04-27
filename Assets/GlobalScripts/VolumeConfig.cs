using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VolumeConfig
{
    public float TotalVolume;

    public float BGM;

    public float Sound;

    public float Voice; //語音


    public VolumeConfig()
    {
        TotalVolume = 0.5f;
        BGM = 0.5f;
        Sound = 0.5f;
        Voice = 0.5f;
    }

    public void SetVolume(float totalVolume, float bgm, float sound, float voice)
    {
        TotalVolume = totalVolume;
        BGM = bgm;
        Sound = sound;
        Voice = voice;
    }
}
