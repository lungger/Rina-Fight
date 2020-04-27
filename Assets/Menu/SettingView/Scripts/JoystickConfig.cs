using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary> 裝常數的類別 - 搖桿上的按鍵名稱 </summary>
static class JoystickKeyName
{
    public const string A = "A";
    public const string B = "B";
    public const string X = "X";
    public const string Y = "Y";
    public const string START = "Start";
    public const string RESET = "Reset";

    public const string ARROW_UP = "Up";
    public const string ARROW_DOWN = "Down";
    public const string ARROW_LEFT = "Left";
    public const string ARROW_RIGHT = "Right";

    public const string LEFT_POV_UP = "Left-POV-Up";
    public const string LEFT_POV_DOWN = "Left-POV-Down";
    public const string LEFT_POV_LEFT = "Left-POV-Left";
    public const string LEFT_POV_RIGHT = "Left-POV-Right";

    public const string RIGHT_POV_UP = "Right-POV-Up";
    public const string RIGHT_POV_DOWN = "Right-POV-Down";
    public const string RIGHT_POV_LEFT = "Right-POV-Left";
    public const string RIGHT_POV_RIGHT = "Right-POV-Right";

    public const string R1 = "R1";
    public const string L1 = "L1";
    public const string RT = "RT"; //R2
    public const string LT = "LT"; //L2

    public const string CLICK_RPOV = "Click-Right-POV"; //R3
    public const string CLICK_LPOV = "Click-Left-POV"; //L3
}

/// <summary> 裝常數的類別 - 操作名稱 </summary>
static class ActionName
{
    public const string MOVE_FRONT = "Move Front";
    public const string MOVE_BACK = "Move Back";
    public const string MOVE_LEFT = "Move Left";
    public const string MOVE_RIGHT = "Move Right";

    public const string CAMERA_FRONT = "Camera Front";
    public const string CAMERA_BACK = "Camera Back";
    public const string CAMERA_LEFT = "Camera Left";
    public const string CAMERA_RIGHT = "Camera Right";

    public const string MENU = "Menu";
    public const string JUMP = "Jump";
    public const string Attack = "LightAttack";

    public const string SPECIAL_ACTION_1 = "SpecialAction1";
    public const string SPECIAL_ACTION_2 = "SpecialAction2";

    public const string SKILL_1 = "Skill1";
    public const string SKILL_2 = "Skill2";

    public const string SWITCH_WEAPON_UP = "SwitchWeaponU";
    public const string SWITCH_WEAPON_DOWN = "SwitchWeaponD";
    public const string SWITCH_WEAPON_LEFT = "SwitchWeaponL";
    public const string SWITCH_WEAPON_RIGHT = "SwitchWeaponR";

    public const string RUN = "Run";
    public const string AVOID = "Avoid";
    public const string LOCK_CAMERA = "LockCamera";

    public const string SWITCH_LOCK_MONSTER_LEFT = "SwitchLockedMonsterL"; //向左切換鎖定敵人
    public const string SWITCH_LOCK_MONSTER_RIGHT = "SwitchLockedMonsterR"; //向右切換鎖定敵人


}

[System.Serializable]
public class JoystickConfig
{
    #region static 
    /// <summary> 搖桿使用的InputManager的Button名稱 </summary>
    Dictionary<string, string> _joystickInputButton = new Dictionary<string, string>()
    {
        { JoystickKeyName.A, "Joystick_A"},
        { JoystickKeyName.B, "Joystick_B"},
        { JoystickKeyName.X, "Joystick_X" },
        { JoystickKeyName.Y, "Joystick_Y" },
        { JoystickKeyName.START, "Joystick_Start" },
        { JoystickKeyName.RESET, "Joystick_Reset" },
        { JoystickKeyName.R1, "Joystick_R1" },
        { JoystickKeyName.L1, "Joystick_L1" },
        { JoystickKeyName.CLICK_LPOV, "Joystick_ClickLPOV" },
        { JoystickKeyName.CLICK_RPOV, "Joystick_ClickRPOV" }
    };

    /// <summary> 搖桿使用的InputManager的Axis名稱 </summary>
    Dictionary<string, string> _joystickInputAxis = new Dictionary<string, string>()
    {
        { JoystickKeyName.ARROW_UP, "Joystick_CrossVertical" },
        { JoystickKeyName.ARROW_DOWN, "Joystick_CrossVertical" },
        { JoystickKeyName.ARROW_LEFT, "Joystick_CrossHorizontal" },
        { JoystickKeyName.ARROW_RIGHT, "Joystick_CrossHorizontal" },

        { JoystickKeyName.LEFT_POV_UP, "Joystick_LPOVVertical" },
        { JoystickKeyName.LEFT_POV_DOWN, "Joystick_LPOVVertical" },
        { JoystickKeyName.LEFT_POV_LEFT, "Joystick_LPOVHorizontal" },
        { JoystickKeyName.LEFT_POV_RIGHT, "Joystick_LPOVHorizontal" },

        { JoystickKeyName.RIGHT_POV_UP, "Joystick_RPOVVertical" },
        { JoystickKeyName.RIGHT_POV_DOWN, "Joystick_RPOVVertical" },
        { JoystickKeyName.RIGHT_POV_LEFT, "Joystick_RPOVHorizontal" },
        { JoystickKeyName.RIGHT_POV_RIGHT, "Joystick_RPOVHorizontal" },

        { JoystickKeyName.RT, "Joystick_LTRT" },
        { JoystickKeyName.LT, "Joystick_LTRT" }
    };
    #endregion

    /// <summary> 按鍵設定 - 鍵位 to 動作 (Ex: dict["Button_A" = "Attack") </summary>
    public Dictionary<string, string> keyconfig_KeyToAction = new Dictionary<string, string>();

    /// <summary> 按鍵設定 - 動作 to 鍵位 (Ex: dict["Attack" = "Button_A") </summary>
    public Dictionary<string, string> keyconfig_ActionToKey = new Dictionary<string, string>();

    public JoystickConfig()
    {
        #region 初始化 keyconfig_ActionToKey 字典
        keyconfig_ActionToKey = new Dictionary<string, string>()
        {
            { ActionName.MOVE_FRONT, JoystickKeyName.LEFT_POV_UP },
            { ActionName.MOVE_BACK, JoystickKeyName.LEFT_POV_DOWN },
            { ActionName.MOVE_LEFT, JoystickKeyName.LEFT_POV_LEFT },
            { ActionName.MOVE_RIGHT, JoystickKeyName.LEFT_POV_RIGHT },
            { ActionName.MENU, JoystickKeyName.START},
            { ActionName.SPECIAL_ACTION_1, JoystickKeyName.RESET},
            { ActionName.SPECIAL_ACTION_2, JoystickKeyName.CLICK_LPOV},
            { ActionName.JUMP, JoystickKeyName.A },
            { ActionName.Attack, JoystickKeyName.B},
            { ActionName.SKILL_1, JoystickKeyName.Y},
            { ActionName.SKILL_2, JoystickKeyName.X},
            { ActionName.SWITCH_WEAPON_UP, JoystickKeyName.ARROW_UP},
            { ActionName.SWITCH_WEAPON_DOWN, JoystickKeyName.ARROW_DOWN},
            { ActionName.SWITCH_WEAPON_LEFT, JoystickKeyName.ARROW_LEFT},
            { ActionName.SWITCH_WEAPON_RIGHT, JoystickKeyName.ARROW_RIGHT},
            { ActionName.SWITCH_LOCK_MONSTER_LEFT, JoystickKeyName.LT},
            { ActionName.SWITCH_LOCK_MONSTER_RIGHT, JoystickKeyName.RT},
            { ActionName.RUN, JoystickKeyName.R1 },
            { ActionName.AVOID, JoystickKeyName.L1},
            { ActionName.LOCK_CAMERA, JoystickKeyName.CLICK_RPOV },
            { ActionName.CAMERA_FRONT, JoystickKeyName.RIGHT_POV_UP},
            { ActionName.CAMERA_BACK, JoystickKeyName.RIGHT_POV_DOWN},
            { ActionName.CAMERA_LEFT, JoystickKeyName.RIGHT_POV_LEFT},
            { ActionName.CAMERA_RIGHT, JoystickKeyName.RIGHT_POV_RIGHT}
        };
        #endregion

        #region 初始化 keyconfig_KeyToAction 字典
        //    { KeyName.Left_POV_Up, "Move Front" },
        foreach (KeyValuePair<string, string> kv in keyconfig_ActionToKey)
        {
            keyconfig_KeyToAction.Add(kv.Value, kv.Key);
        }
        #endregion
    }

    public Dictionary<string, string> GetJoyInputButton()
    {
        return _joystickInputButton;
    }

    public Dictionary<string, string> GetJoyInputAxis()
    {
        return _joystickInputAxis;
    }
}
