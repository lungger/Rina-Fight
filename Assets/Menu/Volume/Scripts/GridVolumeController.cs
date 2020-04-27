using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GridVolumeController : MonoBehaviour
{
    //size = 11, 自己本身 + 10子物件
    Transform[] _gridVolume;

    int _volumeIndex = 5;
    
    public int VolumeIndex
    {
        get
        {
            return _volumeIndex;
        }
        set
        {
            _volumeIndex = value;
            if (_volumeIndex < 0)
                _volumeIndex = 0;
            else if (_volumeIndex > 10)
                _volumeIndex = 10;
        }
    }

    #region 增加 / 減少音量
    public void AddVolume()
    {
        if (VolumeIndex < 10)
        {
            VolumeIndex++;
            _gridVolume[VolumeIndex].gameObject.SetActive(true);
            SetVolume();
        }
    }

    public void DecreaseVolume() //減少
    {
        if (VolumeIndex > 0)
        {
            _gridVolume[VolumeIndex].gameObject.SetActive(false);
            VolumeIndex--;
            SetVolume();
        }
    }

    //從 VolumeViewController 向 遊戲環境 設定音量
    public void SetVolume()
    {
        gameObject.transform.parent.parent.parent.GetComponent<VolumeViewController>().SetVolume();
    }

    public void ButtonClickTest()
    {
        Debug.Log("Click");
    }
    #endregion

    private void Awake()
    {
        _gridVolume = gameObject.GetComponentsInChildren<Transform>();
    }

    #region Initialize
    public void Initialize()
    {
        for (int i = 1; i < _gridVolume.Length; i++)
        {
            _gridVolume[i].gameObject.SetActive(i <= _volumeIndex);
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
