using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class VolumeViewController : MonoBehaviour
{
    [SerializeField]
    Input_Manager _input;

    [SerializeField]
    SelectOptions _options;

    [SerializeField]
    GridVolumeController[] _gridVolumeController;

    int gridVolumeOptionIndex = 0;
    GridVolumeController gridVolumePointer = null;

    VolumeConfig _volumeConfig = new VolumeConfig();

    bool _isPressKey = false; //是否持續按壓鍵
    float _maxTime = 0.16f;
    float _time = 0.0f;
    const float SPACE_TIME = 0.016f;

    #region 函數實作
    //讀取音量設定
    void ReadVolumeSetting()
    {
        var filePath = Application.streamingAssetsPath + "/volumeConfig";
        _volumeConfig = FunctionTools.ReadJsonData<VolumeConfig>(filePath);
        GameEnvironment.entity.MasterVolume = _volumeConfig.TotalVolume;
        GameEnvironment.entity.MusicVolume = _volumeConfig.BGM;
        GameEnvironment.entity.SoundVolume = _volumeConfig.Sound;
        GameEnvironment.entity.VocalVolume = _volumeConfig.Voice;

        _gridVolumeController[0].VolumeIndex = Mathf.RoundToInt(GameEnvironment.entity.MasterVolume * 10.0f);
        _gridVolumeController[1].VolumeIndex = Mathf.RoundToInt(GameEnvironment.entity.MusicVolume * 10.0f);
        _gridVolumeController[2].VolumeIndex = Mathf.RoundToInt(GameEnvironment.entity.SoundVolume * 10.0f);
        _gridVolumeController[3].VolumeIndex = Mathf.RoundToInt(GameEnvironment.entity.VocalVolume * 10.0f);
    }

    //儲存音量設定
    void SaveVolumeSetting()
    {
        float totalVolume = (float)_gridVolumeController[0].VolumeIndex / 10.0f;
        float BGM = (float)_gridVolumeController[1].VolumeIndex / 10.0f;
        float sound = (float)_gridVolumeController[2].VolumeIndex / 10.0f;
        float voice = (float)_gridVolumeController[3].VolumeIndex / 10.0f;

        _volumeConfig.SetVolume(totalVolume, BGM, sound, voice);
        var savePath = Application.streamingAssetsPath + "/volumeConfig";
        FunctionTools.WriteJsonData(savePath, _volumeConfig);

        GameEnvironment.entity.MasterVolume = totalVolume;
        GameEnvironment.entity.MusicVolume = BGM;
        GameEnvironment.entity.SoundVolume = sound;
        GameEnvironment.entity.VocalVolume = voice;
    }

    //設定音量
    public void SetVolume()
    {
        float totalVolume = (float)_gridVolumeController[0].VolumeIndex / 10.0f;
        float BGM = (float)_gridVolumeController[1].VolumeIndex / 10.0f;
        float sound = (float)_gridVolumeController[2].VolumeIndex / 10.0f;
        float voice = (float)_gridVolumeController[3].VolumeIndex / 10.0f;

        GameEnvironment.entity.MasterVolume = totalVolume;
        GameEnvironment.entity.MusicVolume = BGM;
        GameEnvironment.entity.SoundVolume = sound;
        GameEnvironment.entity.VocalVolume = voice;
    }

    //初始化
    void Initialize()
    {
        gridVolumeOptionIndex = 0;
        gridVolumePointer = _gridVolumeController[0];
        _options.SetNowSelectIndex(0);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        ReadVolumeSetting(); //ReadSetting on Start
        for(int i = 0; i < _gridVolumeController.Length; i++)
        {
            _gridVolumeController[i].Initialize();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gridVolumePointer != null)
        {
            if (gridVolumeOptionIndex != _options.NowSelectIndex) //更新Option Index
            {
                gridVolumeOptionIndex = _options.NowSelectIndex;
                gridVolumePointer = _gridVolumeController[gridVolumeOptionIndex];
            }

            if (!_input.Now.Arrow_Left && !_input.Now.Arrow_Right)
            {
                _time = _maxTime;
                _isPressKey = false;
                StopAllCoroutines();
            }

            if (_input.Now.Arrow_Left && _time >= _maxTime) //減少音量
            {
                gridVolumePointer.DecreaseVolume();
                _time = 0.0f;
                StartCoroutine("SetTime");
            }

            if (_input.Now.Arrow_Right && _time >= _maxTime) // 增加音量
            {
                gridVolumePointer.AddVolume();
                _time = 0.0f;
                StartCoroutine("SetTime");
            }
            //_time += SPACE_TIME;
        }
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable() //離開頁面時
    {
        SaveVolumeSetting();
        gridVolumePointer = null;
    }

    IEnumerator SetTime()
    {
        _isPressKey = true;
        yield return new WaitForSecondsRealtime(_maxTime);
        if (_time <= _maxTime)
        {
            _time = _maxTime;
        }
    }
}
