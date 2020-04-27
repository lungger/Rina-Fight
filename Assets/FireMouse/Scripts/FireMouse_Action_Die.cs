using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets;
public class FireMouse_Action_Die : ActionInterface
{
    FireMouse_MainScript MasterScript;


    public int ActionID { get; set; }
    public string ActionName { get; set; }

    public FireMouse_Action_Die(GameCharatcer player, int ID, string Name)
    {
        SetState(player, ID, Name);
    }

    //設定動作(必定先初始化)
    public void SetState(GameCharatcer player, int ID, string Name)
    {
        MasterScript = (FireMouse_MainScript)player;
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
        if (MasterScript.animator.GetCurrentAnimatorStateInfo(0).IsName("Die") && MasterScript.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 2.0f && !MasterScript.isDead)
        {
            EffectLibrary.Effect newEffect = new EffectLibrary.MobDeathSmoke();
            MasterScript.effectPlayer.PlayEffect(ref newEffect, null, MasterScript.transform.Find("骨架/root/body").position + new Vector3(0f, 0.1f, 0f), new Vector3(0f, 0f, 0f), 1f, 1f);
            MasterScript.isDead = true;
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
