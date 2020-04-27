using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class Slime_Action_Attack : ActionInterface
{
    //這個動作會逐漸累積攻擊值

    // Start is called before the first frame update
    Slime_MainScript MasterScript;
    Slime_Data slime_data;
    GameCharatcer attackTarget;
    GameCharacterController controller;

    public int ActionID { get; set; }
    public string ActionName { get; set; }
    public bool Hitting = false;
    public bool AttackObjectBuilded = false;
    public bool Hitted = false;
    // 參數定義


    public Slime_Action_Attack(GameCharatcer player, int ID, string Name)
    {
        SetState(player, ID, Name);
    }

    //設定動作(必定先初始化)
    public void SetState(GameCharatcer slimeScript, int ID, string Name)
    {
        MasterScript = (Slime_MainScript)slimeScript;
        slime_data = MasterScript.slime_data;
        attackTarget = MasterScript.AttackTarget;
        controller = MasterScript.gameCharacterController;
        ActionID = ID;
        ActionName = Name;
    }

    //動作必須要有實體程式
    public void ProcessAction(int currentId)
    {
        //如果不等於此動作則退出
        if (!(currentId == ActionID))
            return;
        Hitting = MasterScript.animator.GetBool("Hitting");
        Hitted = MasterScript.animator.GetBool("Hitted");
        if (!Hitting)
            return;


        if (!AttackObjectBuilded)
        {
            AttackObjectBuilded = true;
            //這裡新增攻擊物件
            GameObject slimeNormalAttack_0 = Object.Instantiate(slime_data.SlimeNormalAttack);
            slimeNormalAttack_0.GetComponent<Attack_SlimeNormalAttack_0_Script>().PresetAttack(MasterScript, MasterScript.CenterPosition+ MasterScript .transform.up*0.5f+ MasterScript.transform.forward * 1f, MasterScript.AttackTarget.CenterPosition, new List<string>() { "Player" });
        }


        //檢查是否切換
        CheckChange(currentId);
    }

    //檢查能夠跳到那些動作
    public void CheckChange(int currentId)
    {
        if (Hitted)
        {
            // 回到預備攻擊
           // Debug.Log("AttacktoTrace");
            MasterScript.JumpInActionByName("PreAttack");
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
        Hitting = false;
        Hitted = false;
        MasterScript.animator.SetBool("Hitting",false);
        MasterScript.animator.SetBool("Hitted",false);
    }

    //進入動作的函式
    public void EnterAction(int currentId, int nextId)
    {
        if (!(nextId == ActionID && !(currentId == nextId)))
            return;
        Hitting = false;
        Hitted = false;
        AttackObjectBuilded = false;
    }
}
