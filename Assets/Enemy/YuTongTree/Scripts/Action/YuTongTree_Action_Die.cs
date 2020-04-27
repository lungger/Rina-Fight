using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuTongTree_Action_Die : ActionInterface
{
    // Start is called before the first frame update
    YuTongTree_MainScript MasterScript;
    YuTongTree_Data data;

    public int ActionID { get; set; }
    public string ActionName { get; set; }

    const string thisActionName = "Die";

    public YuTongTree_Action_Die(GameCharatcer player, int ID, string Name)
    {
        SetState(player, ID, Name);
    }

    //設定動作(必定先初始化)
    public void SetState(GameCharatcer enemyScript, int ID, string Name)
    {
        MasterScript = (YuTongTree_MainScript)enemyScript;
        data = MasterScript.data;
        ActionID = ID;
        ActionName = Name;
    }

    //動作必須要有實體程式
    public void ProcessAction(int currentId)
    {
        //如果不等於此動作則退出
        if (!(currentId == ActionID))
            return;

        if (MasterScript.animator.GetCurrentAnimatorStateInfo(0).IsName(thisActionName) && MasterScript.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            Debug.Log("Die");
            MasterScript.isDead = true;
        }

        //檢查是否切換
        CheckChange(currentId);
    }

    //檢查能夠跳到那些動作
    public void CheckChange(int currentId)
    {
        if (MasterScript.AttackTarget == null)
            return;
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
        MasterScript.animator.SetBool("IsDie",true);
    }
}
