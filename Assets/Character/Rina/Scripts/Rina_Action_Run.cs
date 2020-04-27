using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets;

public class Rina_Action_Run : ActionInterface
{
    Rina_Mainscript MasterScript;
    Rina_Data rina_Data;
    Input_Manager InputState;
    AudioSource RunningSound;

    public int ActionID { get; set; }
    public string ActionName { get; set; }

    public Rina_Action_Run(GameCharatcer player, int ID, string Name)
    {
        SetState(player, ID, Name);
        rina_Data = MasterScript.rina_Data;
        InputState = MasterScript.InputState;
        //讀取跑步聲音
        RunningSound = SoundFinder.FindAudioSourceByName(MasterScript.Sounds, "Rina_Run_Sound_0");
    }

    //設定動作(必定先初始化)
    public void SetState(GameCharatcer player, int ID, string Name)
    {
        MasterScript = (Rina_Mainscript)player;
        ActionID = ID;
        ActionName = Name;
    }

    //動作必須要有實體程式
    public void ProcessAction(int currentId)
    {
        //如果不等於此動作則退出
        if (!(currentId == ActionID))
            return;
        //播放跑步音效
        if (!RunningSound.isPlaying)
            RunningSound.Play();

        //檢查是否維持跑步
        if ((ControllDriver.IsAnyStickPushing_L(InputState) || MasterScript.RunSpeed > 750) && InputState.Now.Button_Dash == true)
        {
            float moveAngle = ControllDriver.GetStickAngle_L(InputState);
            if (MasterScript.RunSpeed < rina_Data.MaxRunSpeed)
            {
                MasterScript.RunSpeed += 150 * Time.deltaTime;
            }
            if (MasterScript.RunSpeed >= rina_Data.MaxRunSpeed && MasterScript.RunSpeed <= rina_Data.MaxRunSpeed + 15 && InputState.IsKeyDown(InputState.Now.Button_Skill2, InputState.Last.Button_Skill2))
            {
                MasterScript.RunSpeed += 200;
            }
            if (MasterScript.RunSpeed > rina_Data.MaxRunSpeed + 5)
                MasterScript.RunSpeed -= 400 * Time.deltaTime;
            else if (MasterScript.RunSpeed < rina_Data.MaxRunSpeed - 2)
                MasterScript.RunSpeed = rina_Data.MaxRunSpeed;


            ControllDriver.NormalMove(ref MasterScript.lookReference, ref MasterScript.Master, moveAngle, MasterScript.RunSpeed);
        }
        //非按緊時的摩擦力
        if (!ControllDriver.IsAnyStickPushing_L(InputState) && InputState.Now.Button_Dash == true)
        {
            if (MasterScript.RunSpeed > 0)
                MasterScript.RunSpeed -= 200 * Time.deltaTime;
            else
                MasterScript.RunSpeed = 0;
        }
        //檢查是否切換
        CheckChange(currentId);
    }

    //檢查能夠跳到那些動作
    public void CheckChange(int currentId)
    {
        if ((ControllDriver.IsAnyStickPushing_L(InputState)) && InputState.Now.Button_Dash == false)
        {
            //跳到慢跑
            MasterScript.JumpInActionByName("Jog");
        }
        if ((ControllDriver.IsAnyStickPushing_L(InputState)) && InputState.Now.Button_Dash == true&&InputState.IsKeyDown(InputState.Now.Button_Attack, InputState.Last.Button_Attack))
        {
            //跳到飛踢
            MasterScript.JumpInActionByName("FlyKick");
        }
            if ((!ControllDriver.IsAnyStickPushing_L(InputState) && MasterScript.RunSpeed < 300))
        {
            //跳到待機
            MasterScript.JumpInActionByName("Idle");
        }
        if (ControllDriver.IsAnyStickPushing_L(InputState) && InputState.IsKeyDown(InputState.Now.Button_Dodge, InputState.Last.Button_Dodge))
        {
            //跳到迴避
            MasterScript.JumpInActionByName("Doge");
        }
        if (InputState.IsKeyDown(InputState.Now.Button_Jump, InputState.Last.Button_Jump))
        {
            //跳到跳躍
            MasterScript.JumpInActionByName("Jump");
        }
        if (MasterScript.IsGrounded == false)
        {
            //掉落
            MasterScript.JumpInActionByName("Jump");
            MasterScript.animator.SetBool("SetDrop", true);
            ((Rina_Action_Jump)ActionFinder.GetActionByName(MasterScript.ActionSets, "Jump")).jumptimer = 0.15f;
            ((Rina_Action_Jump)ActionFinder.GetActionByName(MasterScript.ActionSets, "Jump")).jumpstep = 0;
            ((Rina_Action_Jump)ActionFinder.GetActionByName(MasterScript.ActionSets, "Jump")).jumpVelocity = 0;
        }
    }

    //從其他動作跳到這裡
    public void JumpIn()
    {
        MasterScript.actionNext = ActionID;
    }

    //動作離開時必定執行的程式
    public void LeaveAction(int currentId, int nextId)
    {
        if (!(currentId == ActionID && !(currentId == nextId)))
            return;
        ParticleSystem emissionModule = MasterScript.rina_Data.walkTrail;
        emissionModule.gameObject.SetActive(false);
        RunningSound.enabled = false;
        rina_Data.mtionblurVolume.enabled = false;
    }

    //進入動作的函式
    public void EnterAction(int currentId, int nextId)
    {
        if (!(nextId == ActionID && !(currentId == nextId)))
            return;
        ParticleSystem emissionModule = MasterScript.rina_Data.walkTrail;
        emissionModule.gameObject.SetActive(true);
        RunningSound.enabled = true;
        rina_Data.mtionblurVolume.enabled = true;
    }
}
