#define LocalKeyboardTest123
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScrollViewComponentCreator : MonoBehaviour
{
    [SerializeField]
    GameObject ButtonPrefab;

    [SerializeField]
    GameObject LabelPrefab;

    [SerializeField]
    GameObject PanelRowPrefab;

    [SerializeField]
    ScrollRect scrollCtrl;

    [SerializeField]
    GameObject ParentPage; //上一頁(主選單的選項)

    [SerializeField]
    describeController describeText;

    //掛載於 ScrollView - Context
    /// <summary> 顯示在螢幕上的label的文字 </summary>
    Dictionary<string, string> lbl_actionName = new Dictionary<string, string>();

    /// <summary> 顯示在螢幕上的按鈕的文字 </summary>
    Dictionary<string, string> btn_text = new Dictionary<string, string>();

    /// <summary> 搖桿使用的InputManager的Button名稱 </summary>
    Dictionary<string, string> joystickInputButton = new Dictionary<string, string>();

    /// <summary> 搖桿使用的InputManager的Axis名稱 </summary>
    Dictionary<string, string> joystickInputAxis = new Dictionary<string, string>();

    /// <summary> 儲存搖桿的設定 </summary>
    JoystickConfig joystickConfig = new JoystickConfig();

    /// <summary> 被點擊的button </summary>
    Button ClickedButton = null;

    /// <summary> 94按鈕List </summary>
    List<Button> buttonList = new List<Button>();
    
    public float scrollMoveValue = 0.0933f; //每次移動卷軸的值

    [SerializeField]
    GameObject player;

    int nowMarkButtonIndex = 0; //現在選擇到的Action的Index

    const float SPACE_TIME = 0.016f;
    const float MAX_SPACE_TIME = 0.34f;
    float _maxTime = 0.34f;
    float _time = 0.0f;
    bool isLockSelect = false;
    bool isKeyDown = false;
    bool isPressingKey = false; //持續Press 
    bool firstEnter = true; //第一次進入

    OptionsPanelUI[] optionsPanelUI;

    // Start is called before the first frame update
    void Start()
    {
        #region Create lbl_actionName 
        lbl_actionName.Add(ActionName.JUMP, "確定/跳躍");
        lbl_actionName.Add(ActionName.Attack, "取消/攻擊");
        lbl_actionName.Add(ActionName.SKILL_1, "技能1");
        lbl_actionName.Add(ActionName.SKILL_2, "技能2");
        lbl_actionName.Add(ActionName.SWITCH_WEAPON_UP, "選擇/切換武器--上");
        lbl_actionName.Add(ActionName.SWITCH_WEAPON_DOWN, "選擇/切換武器--下");
        lbl_actionName.Add(ActionName.SWITCH_WEAPON_LEFT, "選擇/切換武器--左");
        lbl_actionName.Add(ActionName.SWITCH_WEAPON_RIGHT, "選擇/切換武器--右");
        lbl_actionName.Add(ActionName.MOVE_FRONT, "移動--前");
        lbl_actionName.Add(ActionName.MOVE_BACK, "移動--後");
        lbl_actionName.Add(ActionName.MOVE_LEFT, "移動--左");
        lbl_actionName.Add(ActionName.MOVE_RIGHT, "移動--右");
        lbl_actionName.Add(ActionName.CAMERA_FRONT, "旋轉鏡頭--上");
        lbl_actionName.Add(ActionName.CAMERA_BACK, "旋轉鏡頭--下");
        lbl_actionName.Add(ActionName.CAMERA_LEFT, "旋轉鏡頭--左");
        lbl_actionName.Add(ActionName.CAMERA_RIGHT, "旋轉鏡頭--右");
        lbl_actionName.Add(ActionName.MENU, "選單");
        lbl_actionName.Add(ActionName.SPECIAL_ACTION_1, "特殊動作1");
        lbl_actionName.Add(ActionName.SWITCH_LOCK_MONSTER_LEFT, "切換敵人--左");
        lbl_actionName.Add(ActionName.SWITCH_LOCK_MONSTER_RIGHT, "切換敵人--右");
        lbl_actionName.Add(ActionName.RUN, "跑步");
        lbl_actionName.Add(ActionName.AVOID, "防禦/閃避");
        lbl_actionName.Add(ActionName.LOCK_CAMERA, "切換鏡頭");
        lbl_actionName.Add(ActionName.SPECIAL_ACTION_2, "特殊動作2");
        #endregion

        #region Create btn_text (English to chinese)
        btn_text.Add(JoystickKeyName.A, "A");
        btn_text.Add(JoystickKeyName.B, "B");
        btn_text.Add(JoystickKeyName.X, "X");
        btn_text.Add(JoystickKeyName.Y, "Y");
        btn_text.Add(JoystickKeyName.START, "Start");
        btn_text.Add(JoystickKeyName.RESET, "重設鍵");
        btn_text.Add(JoystickKeyName.R1, "R1");
        btn_text.Add(JoystickKeyName.L1, "L1");
        btn_text.Add(JoystickKeyName.ARROW_UP, "十字軸上");
        btn_text.Add(JoystickKeyName.ARROW_DOWN, "十字軸下");
        btn_text.Add(JoystickKeyName.ARROW_LEFT, "十字軸左");
        btn_text.Add(JoystickKeyName.ARROW_RIGHT, "十字軸右");
        btn_text.Add(JoystickKeyName.LEFT_POV_UP, "左蘑菇頭上");
        btn_text.Add(JoystickKeyName.LEFT_POV_DOWN, "左蘑菇頭下");
        btn_text.Add(JoystickKeyName.LEFT_POV_LEFT, "左蘑菇頭左");
        btn_text.Add(JoystickKeyName.LEFT_POV_RIGHT, "左蘑菇頭右");

        btn_text.Add(JoystickKeyName.RIGHT_POV_UP, "右蘑菇頭上");
        btn_text.Add(JoystickKeyName.RIGHT_POV_DOWN, "右蘑菇頭下");
        btn_text.Add(JoystickKeyName.RIGHT_POV_LEFT, "右蘑菇頭左");
        btn_text.Add(JoystickKeyName.RIGHT_POV_RIGHT, "右蘑菇頭右");

        btn_text.Add(JoystickKeyName.LT, "L2");
        btn_text.Add(JoystickKeyName.RT, "R2");

        btn_text.Add(JoystickKeyName.CLICK_LPOV, "點擊左蘑菇頭");
        btn_text.Add(JoystickKeyName.CLICK_RPOV, "點擊右蘑菇頭");
        #endregion

        #region Bulid Dictionary of Input button
        joystickInputButton = joystickConfig.GetJoyInputButton();
        #endregion

        #region Bulid Dictionary of Input Axis
        joystickInputAxis = joystickConfig.GetJoyInputAxis();
        #endregion

        Initialize();

        CreateComponent();
        optionsPanelUI = gameObject.GetComponentsInChildren<OptionsPanelUI>();
        
        optionsPanelUI[0].EnterOption();
        describeText.SetDescribeText(0);
        firstEnter = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsJoyStickNoKeyPress())
        {
            _maxTime = MAX_SPACE_TIME;
            _time = _maxTime;
            isKeyDown = false;
            isPressingKey = false;
            StopAllCoroutines();
        }

        if (isPressingKey)
        {
            _maxTime = MAX_SPACE_TIME / 2;
        }

        if (ClickedButton != null && isLockSelect && isKeyDown == false)
        {
            string keyname = GetInputKey(); //A, B, RIGHT-POV-RIGHT
            if (keyname != "None")
            {
                bool haveReaptText = false; //有重複設定

                #region 刪除重複設定
                foreach (Button btn in buttonList)
                {
                    if (GetButtonText(btn) == btn_text[keyname])
                    {
                        string a = GetButtonText(btn), b = GetButtonText(ClickedButton);
                        ChangeButtonText(btn, b);
                        ChangeButtonText(ClickedButton, a);
                        haveReaptText = true;

                        ChangeSettingByButton(btn);
                        ChangeSettingByButton(ClickedButton);
                    }
                }
                #endregion

                if (!haveReaptText) //沒有重複 正常換
                {
                    ChangeButtonText(ClickedButton, btn_text[keyname]); //改變按鈕文字

                    ChangeSettingByButton(ClickedButton);
                }
                isKeyDown = true;
                ClickedButton.GetComponent<ButtonOutline>().CancelSelectButton();
                isLockSelect = false;
                ClickedButton = null;
            }
        }
        else
        {
            if (IsResetConfig())
            {
                ResetPlayerConfig();
            }
            else if (IsExit())
            {
                ExitOption();
            }
            else if (!isLockSelect && IsJoyStickKeyPressDown() && _time >= _maxTime && !isKeyDown) //往下選擇
            {
                SetNowMarkButtonIndex(ref nowMarkButtonIndex, 1);
                ResizeScrollValue_DownSelect();
                _time = 0.0f;
                StartCoroutine("SetTime");
            }
            else if (!isLockSelect && IsJoyStickKeyPressUp() && _time >= _maxTime && !isKeyDown) //往上選擇
            {
                SetNowMarkButtonIndex(ref nowMarkButtonIndex, -1);
                ResizeScrollValue_UpSelect();
                _time = 0.0f;
                StartCoroutine("SetTime");
            }
            else if (IsJoyStickKeyPressConfirm() && !isKeyDown)
            {
                isKeyDown = true;
                buttonList[nowMarkButtonIndex].GetComponent<ButtonOutline>().SelectButton();
                isLockSelect = true;

                ClickButtonToSetKey(buttonList[nowMarkButtonIndex]);
            }
        }
        //_time += SPACE_TIME;
    }

    private void OnEnable() //SetActive = true時執行
    {
        Initialize();
    }

    void ChangeSettingByButton(Button button)
    {
        string action = button.name.Replace("btn_", "");
        string jkey = GetDictKeyByValue(GetButtonText(button));
        joystickConfig.keyconfig_KeyToAction[jkey] = action;
        joystickConfig.keyconfig_ActionToKey[action] = jkey;
    }

    void SetNowMarkButtonIndex(ref int index, int delta)
    {
        int maxSize = lbl_actionName.Count;
        int originIndex = index;

        index += delta;
        if (index < 0)
            index = maxSize - 1;
        else if (index >= maxSize)
            index = 0;

        optionsPanelUI[originIndex].ExitOption();
        optionsPanelUI[index].EnterOption();
        describeText.SetDescribeText(index);
    }

    void CreateComponent()
    {
        foreach(string keyname in lbl_actionName.Keys)
        {
            #region panel
            GameObject panelRow = Instantiate(PanelRowPrefab, transform);
            panelRow.name = "Panel_Row" + keyname;
            #endregion

            #region 創建 label
            GameObject label = Instantiate(LabelPrefab, panelRow.transform);
            label.name = "lbl_" + keyname;
            label.GetComponent<Text>().text += lbl_actionName[keyname];
            #endregion

            #region 創建 button
            GameObject gObject_button = Instantiate(ButtonPrefab, panelRow.transform);
            Button button = gObject_button.GetComponent<Button>();
            gObject_button.name = "btn_" + keyname;
            ChangeButtonText(button, btn_text[joystickConfig.keyconfig_ActionToKey[keyname]]);
            button.onClick.AddListener(delegate () { ClickButtonToSetKey(button); }); //新增按鈕點擊事件
            buttonList.Add(button);
            #endregion
        }
    }

    string GetInputKey()
    {
        foreach(KeyValuePair<string, string> input in joystickInputButton)
        {
            if (Input.GetButtonDown(input.Value))
                return input.Key;
        }

        //這裡是哪裡我是誰
        foreach(KeyValuePair<string, string> input in joystickInputAxis)
        {
            if (Input.GetAxis(input.Value) != 0.00f)
            {
                if (input.Key == JoystickKeyName.ARROW_UP || input.Key == JoystickKeyName.ARROW_RIGHT ||
                   input.Key == JoystickKeyName.LEFT_POV_UP || input.Key == JoystickKeyName.LEFT_POV_RIGHT ||
                   input.Key == JoystickKeyName.RIGHT_POV_UP || input.Key == JoystickKeyName.RIGHT_POV_RIGHT ||
                   input.Key == JoystickKeyName.RT)
                {
                    if (Input.GetAxis(input.Value) > 0.00f)
                    {
                        return input.Key;
                    }
                }
                else
                {
                    if (Input.GetAxis(input.Value) < 0.00f)
                    {
                        return input.Key;
                    }
                }
            }
        }

        return "None";
    }

    void ClickButtonToSetKey(Button btn)
    {
        if (ClickedButton == null)
        {
            ClickedButton = btn;
        }
    }

    #region position
    void ResizeScrollValue_UpSelect()
    {
        if (nowMarkButtonIndex <= lbl_actionName.Count - 12 && scrollCtrl.verticalNormalizedPosition != 1.0f)
        {
            //scrollCtrl.verticalNormalizedPosition = (1.0f) - (float)(nowMarkButtonIndex) / (float)lbl_actionName.Count;
            //scrollCtrl.verticalNormalizedPosition = 1.0f;
            scrollCtrl.verticalNormalizedPosition = Mathf.Min(scrollCtrl.verticalNormalizedPosition + scrollMoveValue, 1.0f);
        }
        else if (nowMarkButtonIndex == lbl_actionName.Count - 1)
        {
            scrollCtrl.verticalNormalizedPosition = 0.0f;
        }
    }

    void ResizeScrollValue_DownSelect()
    {
        if (nowMarkButtonIndex >= 12 && scrollCtrl.verticalNormalizedPosition != 0.0f)
        {
            //scrollCtrl.verticalNormalizedPosition = (1.0f) - (float)(nowMarkButtonIndex) / (float)lbl_actionName.Count;
            //scrollCtrl.verticalNormalizedPosition = 0.0f;
            scrollCtrl.verticalNormalizedPosition = Mathf.Max(scrollCtrl.verticalNormalizedPosition - scrollMoveValue, 0.0f);
        }
        else if (nowMarkButtonIndex == 0)
        {
            scrollCtrl.verticalNormalizedPosition = 1.0f;
        }
    }
    #endregion

    #region save
    void SaveConfig()
    {
        #region 建立儲存成Json的物件
        foreach (string action in lbl_actionName.Keys) //遍歷所有 搖桿按鍵
        {
            string jkey = GetDictKeyByValue(GetButtonText(GameObject.Find("btn_" + action).GetComponent<Button>()));
            joystickConfig.keyconfig_KeyToAction[jkey] = action;
            joystickConfig.keyconfig_ActionToKey[action] = jkey;
        }
        #endregion
        var savePath = Application.streamingAssetsPath + "/joystickConfig";
        FunctionTools.WriteJsonData(savePath, joystickConfig);
    }

    string GetDictKeyByValue(string value)
    {
        foreach(KeyValuePair<string, string> pair in btn_text)
        {
            if (pair.Value == value)
                return pair.Key;
        }
        return JoystickKeyName.A;
    }
    #endregion

    #region 離開場景
    void ExitOption()
    {
        SaveConfig();
        optionsPanelUI[nowMarkButtonIndex].ExitOption();
        ParentPage.SetActive(true);
        scrollCtrl.gameObject.transform.parent.gameObject.SetActive(false);
        ReloadPlayerConfig();
    }
    #endregion

    #region button text
    void ChangeButtonText(Button btn, string newBtnContext)
    {
        btn.transform.Find("Text").GetComponent<Text>().text = newBtnContext;
    }

    string GetButtonText(Button btn)
    {
        return btn.transform.Find("Text").GetComponent<Text>().text;
    }
    #endregion

    #region key down
    //搖桿向下事件
    bool IsJoyStickKeyPressDown()
    {
        #if LocalKeyboardTest
        if (Input.GetAxis("Vertical") < 0.00f)
            return true;
        #endif
        return Input.GetAxis(joystickInputAxis[JoystickKeyName.ARROW_DOWN]) < 0.00f;
        //return IsPressKey(ActionName.SWITCH_WEAPON_DOWN);
    }

    bool IsJoyStickKeyPressUp()
    {
        #if LocalKeyboardTest
        if (Input.GetAxis("Vertical") > 0.00f)
            return true;
        #endif
        return Input.GetAxis(joystickInputAxis[JoystickKeyName.ARROW_UP]) > 0.00f;
        //return IsPressKey(ActionName.SWITCH_WEAPON_UP);
    }

    bool IsJoyStickNoKeyPress()
    {
        #if LocalKeyboardTest
        if (Input.GetAxis("Vertical") == 0.00f)
            return true;
        //return false;
        #endif
        //return !IsPressKey(ActionName.SWITCH_WEAPON_DOWN) && !IsPressKey(ActionName.SWITCH_WEAPON_UP) && !IsPressKey(ActionName.JUMP);
        return Input.GetAxis(joystickInputAxis[JoystickKeyName.ARROW_UP]) == 0.00f && Input.GetAxis(joystickInputAxis[JoystickKeyName.LEFT_POV_UP]) == 0.00f;
    }

    bool IsJoyStickKeyPressConfirm()
    {
        if (isLockSelect)
            return false;
        #if LocalKeyboardTest
        if (Input.GetKeyDown(KeyCode.Return))
            return true;
        #endif
        return Input.GetButtonDown(joystickInputButton[JoystickKeyName.A]);
        //return Input.GetButtonDown(joystickInputButton[confirmButton]);
        //return IsPressKey(ActionName.JUMP);
    }

    bool IsJoyStickKeyPressCancel()
    {
        if (isLockSelect == false)
            return false;
        #if LocalKeyboardTest
        if (Input.GetKeyDown(KeyCode.Return))
            return true;
        #endif
        return Input.GetButtonDown(joystickInputButton[JoystickKeyName.X]);
        //return Input.GetButtonDown(joystickInputButton[cancelButton]);
    }

    bool IsExit()
    {
        #if LocalKeyboardTest
        if (Input.GetKeyDown(KeyCode.Escape))
            return true;
        #endif
        return Input.GetButtonDown(joystickInputButton[JoystickKeyName.RESET]) || Input.GetKeyDown(KeyCode.Escape);
        //return IsPressKey(ActionName.SPECIAL_ACTION_2);
    }

    //post ActionName.JUMP
    bool IsPressKey(string action)
    {
        string keyName = joystickConfig.keyconfig_ActionToKey[action];
        if (joystickInputAxis.ContainsKey(keyName))
        {
            if (keyName == JoystickKeyName.ARROW_UP || keyName == JoystickKeyName.ARROW_RIGHT ||
                   keyName == JoystickKeyName.LEFT_POV_UP || keyName == JoystickKeyName.LEFT_POV_RIGHT ||
                   keyName == JoystickKeyName.RIGHT_POV_UP || keyName == JoystickKeyName.RIGHT_POV_RIGHT ||
                   keyName == JoystickKeyName.RT)
            {
                return Input.GetAxis(joystickInputAxis[keyName]) > 0.0f ? true : false;
            }
            else
            {
                return Input.GetAxis(joystickInputAxis[keyName]) < 0.0f ? true : false;
            }
        }
        else if (joystickInputButton.ContainsKey(keyName))
        {
            return Input.GetButton(joystickInputButton[keyName]) ? true : false;
        }
        return false;
    }

    bool IsResetConfig()
    {
        return Input.GetButtonDown(joystickInputButton[JoystickKeyName.X]);
    }
    #endregion

    // 重新載入角色的操控設定
    void ReloadPlayerConfig()
    {
        EventManager.CallNormalEvents("ReloadSetting", null);
    }

    //將角色設定回歸初始
    void ResetPlayerConfig()
    {
        joystickConfig = new JoystickConfig();

        #region Reset button text
        foreach (string keyname in lbl_actionName.Keys)
        {
            Button button = GameObject.Find("btn_" + keyname).GetComponent<Button>();
            ChangeButtonText(button, btn_text[joystickConfig.keyconfig_ActionToKey[keyname]]);
        }
        #endregion
    }

    public void Initialize()
    {
        var savePath = Application.streamingAssetsPath + "/joystickConfig";
        joystickConfig = FunctionTools.ReadJsonData<JoystickConfig>(savePath);
        nowMarkButtonIndex = 0;
        isLockSelect = false;
        _time = 0.0f;
        _maxTime = MAX_SPACE_TIME;
        isPressingKey = false;
        scrollCtrl.verticalNormalizedPosition = 1.0f;
        isKeyDown = true;
        if (!firstEnter)
        {
            optionsPanelUI[0].EnterOption();
            describeText.SetDescribeText(0);
        }
    }

    IEnumerator SetTime()
    {
        isPressingKey = true;
        yield return new WaitForSecondsRealtime(_maxTime);
        if (_time <= _maxTime)
        {
            _time = _maxTime;
        }
    }
}
