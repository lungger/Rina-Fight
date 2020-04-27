using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class Slime_Action_PreAttack : ActionInterface
{
    //這個動作會逐漸累積攻擊值

    // Start is called before the first frame update
    Slime_MainScript MasterScript;
    Slime_Data slime_data;
    GameCharatcer attackTarget;
    GameCharacterController controller;
    
    public int ActionID { get; set; }
    public string ActionName { get; set; }

    // 參數定義
    public float attackCoolDownTimer = 0f;
    public bool attacked = false;

    public Slime_Action_PreAttack(GameCharatcer player, int ID, string Name)
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

        attackCoolDownTimer += Time.deltaTime;
        GameObject StartReference = new GameObject();
        GameObject TargetReference = new GameObject();
        Vector3 TargetPosition = new Vector3(0, 0, 0);
        attackTarget = MasterScript.AttackTarget;
        TargetPosition = attackTarget.CenterPosition;

        //設定理娜旋轉並得到目標角度
        StartReference.transform.position = MasterScript.transform.position;
        StartReference.transform.LookAt(TargetPosition);
        float TargetAngle = StartReference.transform.rotation.eulerAngles.y;
        Quaternion Targetrotation = Quaternion.Euler(0, TargetAngle, 0);

        //設定旋轉
        MasterScript.gameCharacterController.transform.rotation = Quaternion.Slerp(MasterScript.gameCharacterController.transform.rotation, Targetrotation, Time.deltaTime * 2f);
        GameObject.Destroy(StartReference);
        GameObject.Destroy(TargetReference);
        //檢查是否切換
        CheckChange(currentId);
    }

    //檢查能夠跳到那些動作
    public void CheckChange(int currentId)
    {
        if (MasterScript.TargetDistance >= slime_data.AttackDistance)
        {
            // 回到追擊
            //Debug.Log("AttacktoTrace");
            MasterScript.JumpInActionByName("Trace");
        }
        else if (MasterScript.TargetDistance < slime_data.AttackDistance && attackCoolDownTimer>=slime_data.AttackCoolDownTime)
        {
            //到攻擊，有成功攻擊的話就把CoolDown歸0
            //Debug.Log("AttacktoTrace");
            MasterScript.JumpInActionByName("Attack");
            attackCoolDownTimer = 0;
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
