#define LocalKeyboardTest123
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KeyboardSettingView : MonoBehaviour
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

    const string FILE_NAME = "/keyboardConfig";

    //掛載於 ScrollView - Context
    /// <summary> 顯示在螢幕上的label的文字 </summary>
    Dictionary<string, string> lbl_actionName = new Dictionary<string, string>();

    /// <summary> 顯示在螢幕上的按鈕的文字 </summary>
    //Dictionary<KeyCode, string> btn_text = new Dictionary<KeyCode, string>();

    /// <summary> 儲存鍵盤的設定 </summary>
    KeyboardConfig keyboardConfig = new KeyboardConfig();

    /// <summary> 被點擊的button </summary>
    Button ClickedButton = null;

    /// <summary> 94按鈕List </summary>
    List<Button> buttonList = new List<Button>();

    public float scrollMoveValue = 0.0933f; //每次移動卷軸的值

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
        if (IsPressNoSelect())
        {
            _maxTime = MAX_SPACE_TIME;
            isPressingKey = false;
            _time = _maxTime;
            isKeyDown = false;
            StopAllCoroutines();
        }

        if (isPressingKey)
        {
            //_maxTime = MAX_SPACE_TIME - 0.1f;
            _maxTime = MAX_SPACE_TIME / 2;
        }

        if (ClickedButton != null && isLockSelect && isKeyDown == false)
        {
            KeyCode keyname = GetInputKey(); //A, B, RIGHT-POV-RIGHT
            if (keyname != KeyCode.None)
            {
                bool haveReaptText = false; //有重複設定

                #region 刪除重複設定
                foreach (Button btn in buttonList)
                {
                    if (GetButtonText(btn) == keyname.ToString())
                    {
                        KeyCode btnKey = keyboardConfig.keyconfig_ActionToKey[GetAction(btn)];
                        KeyCode clickBtnKey = keyboardConfig.keyconfig_ActionToKey[GetAction(ClickedButton)];
                        ChangeButtonText(btn, clickBtnKey.ToString());
                        ChangeButtonText(ClickedButton, btnKey.ToString());
                        haveReaptText = true;

                        ChangeSettingByButton(btn, clickBtnKey);
                        ChangeSettingByButton(ClickedButton, btnKey);
                    }
                }
                #endregion

                if (!haveReaptText) //沒有重複 正常換
                {
                    ChangeButtonText(ClickedButton, keyname.ToString()); //改變按鈕文字

                    ChangeSettingByButton(ClickedButton, keyname);
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
            else if (!isLockSelect && IsPressSelectDown() && _time >= _maxTime && !isKeyDown) //往下選擇
            {
                SetNowMarkButtonIndex(ref nowMarkButtonIndex, 1);
                ResizeScrollValue_DownSelect();
                _time = 0.0f;
                StartCoroutine("SetTime");
            }
            else if (!isLockSelect && IsPressSelectUp() && _time >= _maxTime && !isKeyDown) //往上選擇
            {
                SetNowMarkButtonIndex(ref nowMarkButtonIndex, -1);
                ResizeScrollValue_UpSelect();
                _time = 0.0f;
                StartCoroutine("SetTime");
            }
            else if (IsPressConfirm() && !isKeyDown)
            {
                ClickButtonToSetKey(buttonList[nowMarkButtonIndex]);
            }
        }
        //_time += SPACE_TIME;
    }

    private void OnEnable() //SetActive = true時執行
    {
        Initialize();
    }

    void ChangeSettingByButton(Button button, KeyCode key)
    {
        string action = button.name.Replace("btn_", "");
        keyboardConfig.keyconfig_ActionToKey[action] = key;
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

    void SetNowMarkButtonIndex(int setIndex)
    {
        int maxSize = lbl_actionName.Count;
        int originIndex = nowMarkButtonIndex;

        nowMarkButtonIndex = setIndex;

        optionsPanelUI[originIndex].ExitOption();
        optionsPanelUI[nowMarkButtonIndex].EnterOption();
        describeText.SetDescribeText(nowMarkButtonIndex);
    }

    void CreateComponent()
    {
        foreach (string action in lbl_actionName.Keys)
        {
            #region panel
            GameObject panelRow = Instantiate(PanelRowPrefab, transform);
            panelRow.name = "Panel_Row" + action;
            #endregion

            #region 創建 label
            GameObject label = Instantiate(LabelPrefab, panelRow.transform);
            label.name = "lbl_" + action;
            label.GetComponent<Text>().text += lbl_actionName[action];
            #endregion

            #region 創建 button
            GameObject gObject_button = Instantiate(ButtonPrefab, panelRow.transform);
            Button button = gObject_button.GetComponent<Button>();
            gObject_button.name = "btn_" + action;
            ChangeButtonText(button, keyboardConfig.keyconfig_ActionToKey[action].ToString());
            button.onClick.AddListener(delegate () { ClickButtonToSetKey(button); }); //新增按鈕點擊事件
            buttonList.Add(button);
            #endregion
        }
    }

    //to be construct
    KeyCode GetInputKey()
    {
        return FunctionTools.GetKeyboardKeyDown();
    }

    void ClickButtonToSetKey(Button btn)
    {
        if (ClickedButton == null)
        {
            isKeyDown = true;
            isLockSelect = true;
            ClickedButton = btn;
            SetNowMarkButtonIndex(buttonList.FindIndex(a => (a.transform.name == btn.transform.name)));
            buttonList[nowMarkButtonIndex].GetComponent<ButtonOutline>().SelectButton();
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
        keyboardConfig.keyconfig_KeyToAction.Clear();
        foreach (KeyValuePair<string, KeyCode> kv in keyboardConfig.keyconfig_ActionToKey)
        {
            keyboardConfig.keyconfig_KeyToAction.Add(kv.Value, kv.Key);
        }

        var savePath = Application.streamingAssetsPath + FILE_NAME;
        FunctionTools.WriteJsonData(savePath, keyboardConfig);
    }

    KeyCode GetDictKeyByValue(string value)
    {
        foreach (KeyValuePair<string, KeyCode> pair in keyboardConfig.keyconfig_ActionToKey)
        {
            if (pair.Value.ToString() == value)
                return pair.Value;
        }
        return KeyCode.A;
    }

    string GetAction(Button button)
    {
        string action = button.name.Replace("btn_", "");
        return action;
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
    //向下事件
    bool IsPressSelectDown()
    {
        return Input.GetKey(KeyCode.DownArrow);
    }

    bool IsPressSelectUp()
    {
        return Input.GetKey(KeyCode.UpArrow);
    }

    bool IsPressNoSelect()
    {
        return !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow);
    }

    bool IsPressConfirm()
    {
        if (isLockSelect)
            return false;
        return Input.GetKeyDown(KeyCode.Return);
    }

    bool IsExit()
    {
        return Input.GetKeyDown(KeyCode.Escape);
    }

    bool IsResetConfig()
    {
        return Input.GetKeyDown(KeyCode.F5);
    }
    #endregion

    // 重新載入角色的操控設定
    void ReloadPlayerConfig()
    {
        EventManager.CallNormalEvents("ReloadKeyboardSetting", null);
    }

    //將角色設定回歸初始
    void ResetPlayerConfig()
    {
        keyboardConfig = new KeyboardConfig();

        #region Reset button text
        foreach (string action in lbl_actionName.Keys)
        {
            Button button = GameObject.Find("btn_" + action).GetComponent<Button>();
            ChangeButtonText(button, keyboardConfig.keyconfig_ActionToKey[action].ToString());
        }
        #endregion
    }

    public void Initialize()
    {
        var savePath = Application.streamingAssetsPath + FILE_NAME;
        keyboardConfig = FunctionTools.ReadJsonData<KeyboardConfig>(savePath);
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
