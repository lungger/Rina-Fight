using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyboardConfig
{
    /// <summary> 按鍵設定 - 鍵位 to 動作 (Ex: dict["Button_A" = "Attack") </summary>
    public Dictionary<KeyCode, string> keyconfig_KeyToAction = new Dictionary<KeyCode, string>();

    /// <summary> 按鍵設定 - 動作 to 鍵位 (Ex: dict["Attack" = "Button_A") </summary>
    public Dictionary<string, KeyCode> keyconfig_ActionToKey = new Dictionary<string, KeyCode>();

    public KeyboardConfig()
    {
        #region 初始化 keyconfig_ActionToKey 字典 (預設鍵)
        keyconfig_ActionToKey = new Dictionary<string, KeyCode>()
        {
            { ActionName.MOVE_FRONT, KeyCode.UpArrow },
            { ActionName.MOVE_BACK, KeyCode.DownArrow },
            { ActionName.MOVE_LEFT, KeyCode.LeftArrow },
            { ActionName.MOVE_RIGHT, KeyCode.RightArrow },
            { ActionName.MENU, KeyCode.Escape},
            { ActionName.SPECIAL_ACTION_1, KeyCode.Backspace},
            { ActionName.SPECIAL_ACTION_2, KeyCode.F12},
            { ActionName.JUMP, KeyCode.Space },
            { ActionName.Attack, KeyCode.Z},
            { ActionName.SKILL_1, KeyCode.X},
            { ActionName.SKILL_2, KeyCode.C},
            { ActionName.SWITCH_WEAPON_UP, KeyCode.W},
            { ActionName.SWITCH_WEAPON_DOWN, KeyCode.S},
            { ActionName.SWITCH_WEAPON_LEFT, KeyCode.A},
            { ActionName.SWITCH_WEAPON_RIGHT, KeyCode.D},

            { ActionName.SWITCH_LOCK_MONSTER_LEFT, KeyCode.Q},
            { ActionName.SWITCH_LOCK_MONSTER_RIGHT, KeyCode.E},
            { ActionName.RUN, KeyCode.LeftControl },
            { ActionName.AVOID, KeyCode.LeftAlt},
            { ActionName.LOCK_CAMERA, KeyCode.LeftShift },
            { ActionName.CAMERA_FRONT, KeyCode.I},
            { ActionName.CAMERA_BACK, KeyCode.K},
            { ActionName.CAMERA_LEFT, KeyCode.J},
            { ActionName.CAMERA_RIGHT, KeyCode.L}
        };
        #endregion

        #region 初始化 keyconfig_KeyToAction 字典
        //    { KeyName.Left_POV_Up, "Move Front" },
        foreach (KeyValuePair<string, KeyCode> kv in keyconfig_ActionToKey)
        {
            keyconfig_KeyToAction.Add(kv.Value, kv.Key);
        }
        #endregion
    }
}
