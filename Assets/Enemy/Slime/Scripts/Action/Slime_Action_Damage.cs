using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class Slime_Action_Damage : ActionInterface
{
    // Start is called before the first frame update
    Slime_MainScript MasterScript;
    Slime_Data slime_data;
    GameCharatcer attackTarget;

    public int ActionID { get; set; }
    public string ActionName { get; set; }


    public Slime_Action_Damage(GameCharatcer player, int ID, string Name)
    {
        SetState(player, ID, Name);
    }

    //設定動作(必定先初始化)
    public void SetState(GameCharatcer slimeScript, int ID, string Name)
    {
        MasterScript = (Slime_MainScript)slimeScript;
        slime_data = MasterScript.slime_data;
        attackTarget = MasterScript.AttackTarget;
        ActionID = ID;
        ActionName = Name;
    }

    //動作必須要有實體程式
    public void ProcessAction(int currentId)
    {
        //如果不等於此動作則退出
        if (!(currentId == ActionID))
            return;
        if (MasterScript.hitTimer > 0)
            MasterScript.hitTimer -= Time.deltaTime;
        else
            MasterScript.hitTimer = 0;


        //檢查是否切換
        CheckChange(currentId);
    }

    //檢查能夠跳到那些動作
    public void CheckChange(int currentId)
    {
        if (MasterScript.hitTimer <= 0)
        {
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
