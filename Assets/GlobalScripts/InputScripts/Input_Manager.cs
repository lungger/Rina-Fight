using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Manager : MonoBehaviour
{
    [SerializeField]
    public int Controller_Mode = 2;
    public KeyState Now = new KeyState();
    public KeyState Last = new KeyState();
    public bool IsKeyDown(bool now, bool last)
    {
        return (!last) && (now);
    }
    public bool IsKeyUp(bool now, bool last)
    {
        return (last) && (!now);
    }

    // Start is called before the first frame update
    public virtual void Start()
    {

    }
    // Update is called once per frame
    public virtual void Update()
    {
        Now.GetButtonStates(Controller_Mode);
    }

    public virtual void LateUpdate()
    {
        Last.GetButtonStates(Controller_Mode);
    }


}
public class KeyState
{

    public void changeControllerMode(int theset)
    {
        ControllerMode = theset;
    }

    private int ControllerMode = 0;

    //類比搖桿
    public float L_JoyX = 0.0f;
    public float L_JoyY = 0.0f;
    public float R_JoyX = 0.0f;
    public float R_JoyY = 0.0f;

    //攻擊/取消
    public bool Button_Attack = false;
    //跳躍/確定
    public bool Button_Jump = false;
    //技能1
    public bool Button_Skill1 = false;
    //技能2
    public bool Button_Skill2 = false;
    //十字鍵左
    public bool Arrow_Left = false;
    //十字鍵右
    public bool Arrow_Right = false;
    //十字鍵上
    public bool Arrow_Up = false;
    //十字鍵下
    public bool Arrow_Down = false;
    //切換鎖定目標--右
    public bool Button_LockRight = false;
    //跑步/衝刺
    public bool Button_Dash = false;
    //切換鎖定目標--左
    public bool Button_LockLeft = false;
    //迴避//直播宣告
    public bool Button_Dodge = false;
    //選單
    public bool Button_Menu = false;
    //特殊鈕
    public bool Button_Special = false;
    //鎖定敵人
    public bool Button_Lock = false;
    //目前沒用到
    public bool Button_L3 = false;//Unuse

    private JoyInputGetter joyInputGetter = new JoyInputGetter();
    private KeyboardInputGetter keyboardInputGetter = new KeyboardInputGetter();

    public KeyState()
    {
        EventManager.NormalEvents += new EventManager.NormalEventHandler(ReloadJoystickConfig);
        EventManager.NormalEvents += new EventManager.NormalEventHandler(ReloadKeyboardConfig);
    }

    public void ReloadJoystickConfig(string message, object value)
    {
        if (message == "ReloadSetting")
        {
            joyInputGetter = new JoyInputGetter();
        }
    }

    public void ReloadKeyboardConfig(string message, object value)
    {
        if (message == "ReloadKeyboardSetting")
        {
            keyboardInputGetter = new KeyboardInputGetter();
        }
    }

    private void UpdateButton(ref bool button, KeyCode Key)
    {
        if (Input.GetKeyDown(Key))
        {
            button = true;
        }
        else if (Input.GetKeyUp(Key))
        {
            button = false;
        }
    }

    public void GetButtonStates(int SetControllerMode)
    {
        ControllerMode = SetControllerMode;
        //預設鍵盤
        if (ControllerMode == 0)
        {
            L_JoyY = Input.GetAxis("Vertical");
            L_JoyX = Input.GetAxis("Horizontal");
            R_JoyY = Input.GetAxis("ViewVertical");
            R_JoyX = Input.GetAxis("ViewHorizontal");

            //Button_Attack = joyInputGetter.Button_Circle;
            //Button_Jump = joyInputGetter.Button_Cross;
            //Button_Skill2 = joyInputGetter.Button_Square;
            //Button_Skill1 = joyInputGetter.Button_Triangle;
            //Button_Menu = joyInputGetter.Button_Start;
            //Arrow_Left = joyInputGetter.Arrow_Left;
            //Arrow_Right = joyInputGetter.Arrow_Right;
            //Arrow_Up = joyInputGetter.Arrow_Top;
            //Arrow_Down = joyInputGetter.Arrow_Down;
            //Button_Special = joyInputGetter.Button_Reset;
            //Button_LockLeft = joyInputGetter.Button_L1;
            //Button_Dodge = Input.GetKey(KeyCode.LeftControl);
            //Button_L3 = joyInputGetter.Button_L3;
            //Button_LockRight = joyInputGetter.Button_R1;
            //Button_Dash = Input.GetKey(KeyCode.C);
            //Button_Lock = joyInputGetter.Button_R3;

            keyboardInputGetter.UpdateKeyboardInput();
            UpdateKeyboardStates();
        }
        //預設搖桿
        else if (ControllerMode == 1)
        {

        }
        //使用者設定
        else if (ControllerMode == 2)
        {
            joyInputGetter.UpdateJoystickInput();
            UpdateJoystickStates();
        }
        // 搖桿+鍵盤 (有搖桿的話以搖桿為主)
        else if (ControllerMode == 3)
        {
            keyboardInputGetter.UpdateKeyboardInput();
            joyInputGetter.UpdateJoystickInput();
            UpdateInputStates();
        }
    }

    /// <summary> 更新搖桿按鍵的狀態 </summary>
    public void UpdateJoystickStates()
    {
        L_JoyX = joyInputGetter.L_JoyX;
        L_JoyY = joyInputGetter.L_JoyY;
        R_JoyX = joyInputGetter.R_JoyX;
        R_JoyY = joyInputGetter.R_JoyY;
        Button_Attack = joyInputGetter.Button_Circle;
        Button_Jump = joyInputGetter.Button_Cross;
        Button_Skill2 = joyInputGetter.Button_Square;
        Button_Skill1 = joyInputGetter.Button_Triangle;
        Button_Menu = joyInputGetter.Button_Start;
        Arrow_Left = joyInputGetter.Arrow_Left;
        Arrow_Right = joyInputGetter.Arrow_Right;
        Arrow_Up = joyInputGetter.Arrow_Top;
        Arrow_Down = joyInputGetter.Arrow_Down;
        Button_Special = joyInputGetter.Button_Reset;
        Button_LockLeft = joyInputGetter.Button_L1;
        Button_Dodge = joyInputGetter.Button_L2;
        Button_L3 = joyInputGetter.Button_L3;
        Button_LockRight = joyInputGetter.Button_R1;
        Button_Dash = joyInputGetter.Button_R2;
        Button_Lock = joyInputGetter.Button_R3;
    }
    //更新鍵盤狀態
    public void UpdateKeyboardStates()
    {
        L_JoyX = keyboardInputGetter.L_JoyX;
        L_JoyY = keyboardInputGetter.L_JoyY;
        R_JoyX = keyboardInputGetter.R_JoyX;
        R_JoyY = keyboardInputGetter.R_JoyY;
        Button_Attack = keyboardInputGetter.Button_Circle;
        Button_Jump = keyboardInputGetter.Button_Cross;
        Button_Skill2 = keyboardInputGetter.Button_Square;
        Button_Skill1 = keyboardInputGetter.Button_Triangle;
        Button_Menu = keyboardInputGetter.Button_Start;
        Arrow_Left = keyboardInputGetter.Arrow_Left;
        Arrow_Right = keyboardInputGetter.Arrow_Right;
        Arrow_Up = keyboardInputGetter.Arrow_Top;
        Arrow_Down = keyboardInputGetter.Arrow_Down;
        Button_Special = keyboardInputGetter.Button_Reset;
        Button_LockLeft = keyboardInputGetter.Button_L1;
        Button_Dodge = keyboardInputGetter.Button_L2;
        Button_L3 = keyboardInputGetter.Button_L3;
        Button_LockRight = keyboardInputGetter.Button_R1;
        Button_Dash = keyboardInputGetter.Button_R2;
        Button_Lock = keyboardInputGetter.Button_R3;
    }

    //從搖桿或鍵盤中獲取輸入
    public void UpdateInputStates()
    {
        L_JoyX = joyInputGetter.L_JoyX != 0.00f ? joyInputGetter.L_JoyX : keyboardInputGetter.L_JoyX;
        L_JoyY = joyInputGetter.L_JoyY != 0.00f ? joyInputGetter.L_JoyY : keyboardInputGetter.L_JoyY;
        R_JoyX = joyInputGetter.R_JoyX != 0.00f ? joyInputGetter.R_JoyX : keyboardInputGetter.R_JoyX;
        R_JoyY = joyInputGetter.R_JoyY != 0.00f ? joyInputGetter.R_JoyY : keyboardInputGetter.R_JoyY;

        Button_Attack = joyInputGetter.Button_Circle | keyboardInputGetter.Button_Circle;
        Button_Jump = joyInputGetter.Button_Cross | keyboardInputGetter.Button_Cross;
        Button_Skill2 = joyInputGetter.Button_Square | keyboardInputGetter.Button_Square;
        Button_Skill1 = joyInputGetter.Button_Triangle | keyboardInputGetter.Button_Triangle;
        Button_Menu = joyInputGetter.Button_Start | keyboardInputGetter.Button_Start;
        Arrow_Left = joyInputGetter.Arrow_Left | keyboardInputGetter.Arrow_Left;
        Arrow_Right = joyInputGetter.Arrow_Right | keyboardInputGetter.Arrow_Right;
        Arrow_Up = joyInputGetter.Arrow_Top | keyboardInputGetter.Arrow_Top;
        Arrow_Down = joyInputGetter.Arrow_Down | keyboardInputGetter.Arrow_Down;
        Button_Special = joyInputGetter.Button_Reset | keyboardInputGetter.Button_Reset;
        Button_LockLeft = joyInputGetter.Button_L1 | keyboardInputGetter.Button_L1;
        Button_Dodge = joyInputGetter.Button_L2 | keyboardInputGetter.Button_L2;
        Button_L3 = joyInputGetter.Button_L3 | keyboardInputGetter.Button_L3;
        Button_LockRight = joyInputGetter.Button_R1 | keyboardInputGetter.Button_R1;
        Button_Dash = joyInputGetter.Button_R2 | keyboardInputGetter.Button_R2;
        Button_Lock = joyInputGetter.Button_R3 | keyboardInputGetter.Button_R3;
    }
}
