using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets;

public class Rina_Action_Jog : ActionInterface
{
    Rina_Mainscript MasterScript;
    Rina_Data rina_Data;
    Input_Manager InputState;

    public int ActionID { get; set; }
    public string ActionName { get; set; }
    float moveAngle = 0;
    public Rina_Action_Jog(GameCharatcer player, int ID, string Name)
    {
        SetState(player, ID, Name);
        rina_Data = MasterScript.rina_Data;
        InputState = MasterScript.InputState;
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

        if ((MasterScript.StickDirectionState == DirectState.Back) && MasterScript.cameraMode == LockMode.Lock)
            ControllDriver.NormalMove(ref MasterScript.lookReference, ref MasterScript.Master, moveAngle, MasterScript.RunSpeed);
        else
            ControllDriver.NormalMove(ref MasterScript.lookReference, ref MasterScript.Master, moveAngle, MasterScript.RunSpeed);
        //檢查是否維持慢跑
        if (ControllDriver.IsAnyStickPushing_L(InputState))
        {
            moveAngle = ControllDriver.GetStickAngle_L(InputState);
            if (MasterScript.RunSpeed < rina_Data.MaxJogSpeed)
                MasterScript.RunSpeed += 200 * Time.deltaTime;
            else
                MasterScript.RunSpeed = rina_Data.MaxJogSpeed;
        }
        else
        {
            if (MasterScript.RunSpeed > rina_Data.MaxRunSpeed / 6)
                MasterScript.RunSpeed -= 200 * Time.deltaTime;
        }
        //檢查是否切換
        CheckChange(currentId);
    }

    //檢查能夠跳到那些動作
    public void CheckChange(int currentId)
    {
        if (ControllDriver.IsAnyStickPushing_L(InputState))
        {
            if (InputState.Now.Button_Dash == true)
            {
                MasterScript.RunSpeed = MasterScript.rina_Data.MaxRunSpeed / 2;
                //跳到跑步
                MasterScript.JumpInActionByName("Run");
            }
        }
        else
        {
            if (!(MasterScript.RunSpeed > rina_Data.MaxRunSpeed / 6))
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
        else if (InputState.IsKeyDown(InputState.Now.Button_Skill2, InputState.Last.Button_Skill2))
        {
            //跳到射擊
            MasterScript.JumpInActionByName("Shot");
        }
        else if ( InputState.IsKeyDown(InputState.Now.Button_Skill1, InputState.Last.Button_Skill1))
        {
            //跳到攻擊
            MasterScript.JumpInActionByName("KickUp");
        }
        else if (InputState.IsKeyDown(InputState.Now.Button_Attack, InputState.Last.Button_Attack))
        {
            //跳到攻擊
            MasterScript.JumpInActionByName("NormalAttack");
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
        ParticleSystem.EmissionModule emissionModule = MasterScript.rina_Data.walkTrail.emission;
        emissionModule.enabled = false;
    }

    //進入動作的函式
    public void EnterAction(int currentId, int nextId)
    {
        if (!(nextId == ActionID && !(currentId == nextId)))
            return;
        ParticleSystem.EmissionModule emissionModule = MasterScript.rina_Data.walkTrail.emission;
        emissionModule.enabled = true;
    }
}
