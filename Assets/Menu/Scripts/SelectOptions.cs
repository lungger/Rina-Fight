using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SelectOptions : MonoBehaviour
{
    [SerializeField]
    int Options = 4;
    
    [SerializeField]
    Input_Manager input;

    [SerializeField]
    EnterOptions _enterOptionController;

    [SerializeField]
    ExitPage _exitPageController;

    [SerializeField]
    GameObject _titleImage;

    public bool CanSelect = true;

    int _nowSelectIndex = 0;

    public int NowSelectIndex
    {
        get { return _nowSelectIndex; }
    }

    const float MAX_SPACE_TIME = 0.5f;
    float _maxTime = 0.5f;
    float _time = 0.0f;
    const float SPACE_TIME = 0.016f;

    bool _isPressKey = false; //是否持續按壓鍵

    OptionsPanelUI[] _optionsPanel;
    OptionsPanelData[] _options;

    #region 函數實作
    public void Initialize()
    {
        SetNowSelectIndex(0);
        if (_optionsPanel.Length > 0)
            _optionsPanel[0].EnterOption();
        _isPressKey = false;
    }

    public void SetNowSelectIndex(int newIndex)
    {
        int maxOptionIndex = Options;
        int originOptionIndex = _nowSelectIndex;

        if (newIndex < 0)
        {
            _nowSelectIndex = maxOptionIndex - 1;
        }
        else if(newIndex >= maxOptionIndex)
        {
            _nowSelectIndex = 0;
        }
        else
        {
            _nowSelectIndex = newIndex;
        }

        if (originOptionIndex != _nowSelectIndex)
        {
            _optionsPanel[originOptionIndex].ExitOption(); //原本的Panel 取消選取
            _optionsPanel[_nowSelectIndex].EnterOption(); //後來的Panel 新增選取
        }
    }

    bool IsSelectUp() //是否向上選擇
    {
        return input.Now.Arrow_Up || input.Now.L_JoyY > 0.00f;
    }

    bool IsSelectDown() //是否向下選擇
    {
        return input.Now.Arrow_Down || input.Now.L_JoyY < 0.00f;
    }

    bool IsNoSelect()
    {
        return !(input.Now.Arrow_Down) && !(input.Now.Arrow_Up) && input.Now.L_JoyY == 0.00f;
    }

    // 確定選擇該選項
    public bool IsEnterOption()
    {
        return input.IsKeyDown(input.Now.Button_Jump, input.Last.Button_Jump);
    }

    public bool IsExitOption()
    {
        return input.IsKeyDown(input.Now.Button_Special, input.Last.Button_Special);
    }
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        _optionsPanel = gameObject.GetComponentsInChildren<OptionsPanelUI>();
        _options = gameObject.GetComponentsInChildren<OptionsPanelData>();


        if (_exitPageController == null)
        {
            _exitPageController = gameObject.GetComponent<ExitPage>();
        }
    }

    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanSelect)
        {
            if (_titleImage != null)
                _titleImage.SetActive(true);
            if (IsNoSelect())
            {
                _isPressKey = false;
                _maxTime = MAX_SPACE_TIME;
                _time = _maxTime;
                StopAllCoroutines();
            }
            if (_time >= _maxTime)
            {
                if (IsSelectUp())
                {
                    SetNowSelectIndex(_nowSelectIndex - 1);
                    _time = 0.0f;
                    StartCoroutine("SetTime");
                }
                if (IsSelectDown())
                {
                    SetNowSelectIndex(_nowSelectIndex + 1);
                    _time = 0.0f;
                    StartCoroutine("SetTime");
                }
            }

            if (_isPressKey)
            {
                _maxTime = MAX_SPACE_TIME / 3;
            }

            //進入選項
            if (_enterOptionController != null && IsEnterOption())
            {
                if (_nowSelectIndex < _options.Length)
                {
                    if(_titleImage != null)
                        _titleImage.SetActive(false);
                    _enterOptionController.EnterOption(_options[_nowSelectIndex], gameObject);
                }
            }

            //離開選項
            if (_exitPageController != null && IsExitOption())
            {
                SetNowSelectIndex(0);
                _exitPageController.Exit();
            }
        }
        //_time += SPACE_TIME;
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
