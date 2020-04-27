using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//註冊enum
public enum AttackHitType { None, Normal, HitFly };
public enum DirectState { Null, Front, Back, Right, Left };
public enum LockMode { Free, Track, Lock };
public enum ElementSet { Wind, Spark, Fire, Water, Ground };

public class GameEnvironmentScripts : MonoBehaviour
{
    static GameEnvironmentScripts entity;
    //注意!!!!!!遊戲的所有設定都在這裡!!!!!!!!
    [Header("全部聲音百分比")]
    public float MasterVolume = 1.0f;
    [Header("音量百分比")]
    public float MusicVolume = 1.0f;
    [Header("音效百分比")]
    public float SoundVolume = 1.0f;
    [Header("人聲百分比")]
    public float VocalVolume = 1.0f;

    [Header("預設重力")]
    public Vector3 Gravity = new Vector3(0, -9.81f, 0);

    public AudioMixer audioMixer;

    public Omector3 GravityDirection
    {
        get
        {
            Omector3 returner = (Omector3)Gravity;
            returner.Power = 1;
            return returner;
        }
    }

    [Header("速度比率衰減")]
    public float FixSpeed = 10f;

    private int HitdelayCurrent = 0;
    public int HitdelayCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (entity == null)
        {
            entity = this;
            DontDestroyOnLoad(this);
        }
        else if (entity != null) //切到新場景時，若已經有entity，則刪掉新場景的GameEnvironment (只留舊的)
        {
            Destroy(gameObject);
        }

        #region 讀取 音量設定
        var filePath = Application.streamingAssetsPath + "/volumeConfig";
        VolumeConfig volumeConfig = FunctionTools.ReadJsonData<VolumeConfig>(filePath);
        MusicVolume = volumeConfig.BGM;
        SoundVolume = volumeConfig.Sound;
        VocalVolume = volumeConfig.Voice;
        MasterVolume = volumeConfig.TotalVolume;
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        Physics.gravity = Gravity;
        audioMixer.SetFloat("MusicVolumn", -100 + MusicVolume * 100);
        audioMixer.SetFloat("SoundVolumn", -100 + SoundVolume * 100);
        audioMixer.SetFloat("VocalVolumn", -100 + VocalVolume * 100);
        audioMixer.SetFloat("MasterVolumn", -100 + MasterVolume * 100);
    }

    private void FixedUpdate()
    {
        if (HitdelayCount > HitdelayCurrent)
        {
            HitdelayCurrent++;
            Time.timeScale = 0.5f;
        }
        else
        {
            HitdelayCount = 0;
            HitdelayCurrent = 0;
            Time.timeScale = 1f;
        }
    }
}
