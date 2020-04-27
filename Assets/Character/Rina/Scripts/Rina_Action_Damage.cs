using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets;

public class Rina_Action_Damage : ActionInterface
{
    Rina_Mainscript MasterScript;
    Rina_Data rina_Data;
    Input_Manager InputState;
    Animator animator;

    public int ActionID { get; set; }
    public string ActionName { get; set; }

    public Rina_Action_Damage(GameCharatcer player, int ID, string Name)
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
        animator = MasterScript.animator;
        animator.SetBool("HitFly",(MasterScript.beHittingType == AttackHitType.HitFly));
        MasterScript.hitTimer -= Time.deltaTime;


        //檢查是否切換
        CheckChange(currentId);
    }

    //檢查能夠跳到那些動作
    public void CheckChange(int currentId)
    {
        if (MasterScript.hitTimer <= 0)
        {
            //跳到跑步
            MasterScript.JumpInActionByName("Idle");
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
    }

    //進入動作的函式
    public void EnterAction(int currentId, int nextId)
    {
        if (!(nextId == ActionID && !(currentId == nextId)))
            return;
    }
}
