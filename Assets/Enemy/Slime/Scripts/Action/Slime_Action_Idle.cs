﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class Slime_Action_Idle : ActionInterface
{
    // Start is called before the first frame update
    Slime_MainScript MasterScript;
    Slime_Data slime_data;

    public int ActionID { get; set; }
    public string ActionName { get; set; }

    public Slime_Action_Idle(GameCharatcer player, int ID, string Name)
    {
        SetState(player, ID, Name);
    }

    //設定動作(必定先初始化)
    public void SetState(GameCharatcer slimeScript, int ID, string Name)
    {
        MasterScript = (Slime_MainScript)slimeScript;
        slime_data = MasterScript.slime_data;
        ActionID = ID;
        ActionName = Name;
    }

    //動作必須要有實體程式
    public void ProcessAction(int currentId)
    {
        //如果不等於此動作則退出
        if (!(currentId == ActionID))
            return;

        
        //檢查是否切換
        CheckChange(currentId);
    }

    //檢查能夠跳到那些動作
    public void CheckChange(int currentId)
    {
        if (MasterScript.AttackTarget == null)
            return;
        if (MasterScript.TargetDistance <= slime_data.TraceDistance)
        {
            // 跳到追擊
            //Debug.Log("IdletoTrace");
            MasterScript.JumpInActionByName("Trace");
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