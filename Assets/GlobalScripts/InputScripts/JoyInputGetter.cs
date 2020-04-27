using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JoyInputGetter
{
    JoystickConfig joystickConfig = new JoystickConfig();
    
    /// <summary> 搖桿使用的InputManager的Button名稱 </summary>
    Dictionary<string, string> joystickInputButton = new Dictionary<string, string>();

    /// <summary> 搖桿使用的InputManager的Axis名稱 </summary>
    Dictionary<string, string> joystickInputAxis = new Dictionary<string, string>();

    /// <summary> 操作的輸入值 </summary>
    Dictionary<string, float> joystick_ActionValue = new Dictionary<string, float>();

    #region 變數區
    // 從 joystick_ActionValue 轉換過來
    public float L_JoyX = 0.0f;
    public float L_JoyY = 0.0f;
    public float R_JoyX = 0.0f;
    public float R_JoyY = 0.0f;

    public bool Button_Circle = false;
    public bool Button_Cross = false;
    public bool Button_Square = false;
    public bool Button_Triangle = false;

    public bool Arrow_Left = false;
    public bool Arrow_Right = false;
    public bool Arrow_Top = false;
    public bool Arrow_Down = false;

    public bool Button_R1 = false;
    public bool Button_R2 = false;
    public bool Button_R3 = false;

    public bool Button_L1 = false;
    public bool Button_L2 = false;
    public bool Button_L3 = false;

    public bool Button_Start = false;
    public bool Button_Reset = false;
    #endregion

    public JoyInputGetter()
    {
        var filePath = Application.streamingAssetsPath + "/joystickConfig";
        try
        {
            joystickConfig = FunctionTools.ReadJsonData<JoystickConfig>(filePath);
            string ok = "success";
            FunctionTools.WriteJsonData<string>(Application.streamingAssetsPath + "/Edit", ok);
        }
        catch (System.Exception e)
        {
            string ok = "fail";
            FunctionTools.WriteJsonData<string>(Application.streamingAssetsPath + "/Edit", ok);
            FunctionTools.WriteJsonData<string>(Application.streamingAssetsPath + "/errorCode2", e.Message);
        }
        joystickInputButton = joystickConfig.GetJoyInputButton();
        joystickInputAxis = joystickConfig.GetJoyInputAxis();

        foreach (string key in joystickConfig.keyconfig_ActionToKey.Keys)
        {
            joystick_ActionValue.Add(key, 0.0f);
        }

        #region 初始化變數
        L_JoyX = 0.0f;
        L_JoyY = 0.0f;
        R_JoyX = 0.0f;
        R_JoyY = 0.0f;

        Button_Circle = false;
        Button_Cross = false;
        Button_Square = false;
        Button_Triangle = false;

        Arrow_Left = false;
        Arrow_Right = false;
        Arrow_Top = false;
        Arrow_Down = false;

        Button_R1 = false;
        Button_R2 = false;
        Button_R3 = false;

        Button_L1 = false;
        Button_L2 = false;
        Button_L3 = false;

        Button_Start = false;
        Button_Reset = false;
        #endregion
    }

    public void UpdateJoystickInput()
    {
        foreach (string action in joystickConfig.keyconfig_ActionToKey.Keys)
        {
            joystick_ActionValue[action] = GetJoystickInputValue(joystickConfig.keyconfig_ActionToKey[action]);
            //Debug.Log(action + ": " + joystick_ActionValue[action]);
        }
        
        L_JoyX = 0.0f;
        L_JoyY = 0.0f;
        R_JoyX = 0.0f;
        R_JoyY = 0.0f;

        L_JoyX = GetActionInput(L_JoyX, ActionName.MOVE_RIGHT, 1);
        L_JoyX = GetActionInput(L_JoyX, ActionName.MOVE_LEFT, -1);
        L_JoyY = GetActionInput(L_JoyY, ActionName.MOVE_FRONT, 1);
        L_JoyY = GetActionInput(L_JoyY, ActionName.MOVE_BACK, -1);

        R_JoyX = GetActionInput(R_JoyX, ActionName.CAMERA_RIGHT, 1);
        R_JoyX = GetActionInput(R_JoyX, ActionName.CAMERA_LEFT, -1);
        R_JoyY = GetActionInput(R_JoyY, ActionName.CAMERA_FRONT, 1);
        R_JoyY = GetActionInput(R_JoyY, ActionName.CAMERA_BACK, -1);

        Button_Circle = GetActionInput(Button_Circle, ActionName.Attack);
        Button_Cross = GetActionInput(Button_Cross, ActionName.JUMP);
        Button_Square = GetActionInput(Button_Square, ActionName.SKILL_2);
        Button_Triangle = GetActionInput(Button_Triangle, ActionName.SKILL_1);

        Button_Start = GetActionInput(Button_Start, ActionName.MENU);
        Button_Reset = GetActionInput(Button_Reset, ActionName.SPECIAL_ACTION_1);

        Arrow_Top = GetActionInput(Arrow_Top, ActionName.SWITCH_WEAPON_UP);
        Arrow_Down = GetActionInput(Arrow_Down, ActionName.SWITCH_WEAPON_DOWN);
        Arrow_Left = GetActionInput(Arrow_Left, ActionName.SWITCH_WEAPON_LEFT);
        Arrow_Right = GetActionInput(Arrow_Right, ActionName.SWITCH_WEAPON_RIGHT);

        Button_L1 = GetActionInput(Button_L1, ActionName.SWITCH_LOCK_MONSTER_LEFT);
        Button_L2 = GetActionInput(Button_L2, ActionName.AVOID);
        Button_L3 = GetActionInput(Button_L3, ActionName.SPECIAL_ACTION_2);

        Button_R1 = GetActionInput(Button_R1, ActionName.SWITCH_LOCK_MONSTER_RIGHT);
        Button_R2 = GetActionInput(Button_R2, ActionName.RUN);
        Button_R3 = GetActionInput(Button_R3, ActionName.LOCK_CAMERA);

        //IsValueTrue(L_JoyX, "Test L_JoyX");
        //IsValueTrue(L_JoyY, "Test L_JoyY");

        //IsValueTrue(Button_Circle, "LightAttack");
        //IsValueTrue(Button_Cross, "Jump");
    }

    //Test Function
    void IsValueTrue(float kkk, string name)
    {
        if (kkk != 0.00f)
        {
            Debug.Log(name + ": " + kkk.ToString());
        }
    }

    //Test Function
    void IsValueTrue(bool kkk, string name)
    {
        if (kkk)
        {
            Debug.Log(name + ": " + kkk.ToString());
        }
    }
    
    //將輸入的值塞進變數裡
    public float GetActionInput(float value, string action, int delta)
    {
        if (joystick_ActionValue[action] != 0.00f)
        {
            return joystick_ActionValue[action] * delta;
        }
        else
        {
            if (value != 0.0f)
                return value;
            return 0.0f;
        }
    }

    //將輸入的值塞進變數裡
    public bool GetActionInput(bool value, string action)
    {
        return joystick_ActionValue[action] != 0.0f;
    }
    
    //這裡是哪裡我是誰 
    //實際得到輸入的值
    float GetJoystickInputValue(string keyName)
    {
        if (joystickInputAxis.ContainsKey(keyName))
        {
            if (keyName == JoystickKeyName.ARROW_UP || keyName == JoystickKeyName.ARROW_RIGHT ||
                   keyName == JoystickKeyName.LEFT_POV_UP || keyName == JoystickKeyName.LEFT_POV_RIGHT ||
                   keyName == JoystickKeyName.RIGHT_POV_UP || keyName == JoystickKeyName.RIGHT_POV_RIGHT ||
                   keyName == JoystickKeyName.RT)
            {
                return Input.GetAxis(joystickInputAxis[keyName]) > 0.0f ? Input.GetAxis(joystickInputAxis[keyName]) : 0.0f;
            }
            else
            {
                return Input.GetAxis(joystickInputAxis[keyName]) < 0.0f ? Input.GetAxis(joystickInputAxis[keyName]) * (-1) : 0.0f;
            }
        }
        else if (joystickInputButton.ContainsKey(keyName))
        {
            return Input.GetButton(joystickInputButton[keyName]) ? 1.0f : 0.0f;
        }
        return 0.0f;
    }
}
