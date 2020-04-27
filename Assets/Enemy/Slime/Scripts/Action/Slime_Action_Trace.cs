using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class Slime_Action_Trace : ActionInterface
{
    // Start is called before the first frame update
    Slime_MainScript MasterScript;
    Slime_Data slime_data;
    GameCharatcer attackTarget;

    public int ActionID { get; set; }
    public string ActionName { get; set; }

    public Slime_Action_Trace(GameCharatcer player, int ID, string Name)
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
        attackTarget = MasterScript.AttackTarget;
        GameObject StartReference = new GameObject();
        GameObject TargetReference = new GameObject();
        Vector3 TargetPosition = new Vector3(0, 0, 0);

        TargetPosition = attackTarget.CenterPosition;

        //設定理娜旋轉並得到目標角度
        StartReference.transform.position = MasterScript.transform.position;
        StartReference.transform.LookAt(TargetPosition);
        float TargetAngle = StartReference.transform.rotation.eulerAngles.y;
        Quaternion Targetrotation = Quaternion.Euler(0, TargetAngle, 0);

        //設定旋轉
        MasterScript.gameCharacterController.transform.rotation = Quaternion.Slerp(MasterScript.gameCharacterController.transform.rotation, Targetrotation, Time.deltaTime * 2f);
        MasterScript.gameCharacterController.moveVector = MasterScript.gameCharacterController.transform.forward * slime_data.MaxTraceSpeed;

        GameObject.Destroy(StartReference);
        GameObject.Destroy(TargetReference);


        //檢查是否切換
        CheckChange(currentId);
    }

    //檢查能夠跳到那些動作
    public void CheckChange(int currentId)
    {
        if (MasterScript.TargetDistance >= slime_data.TraceDistance)
        {
            // 跳到待機
            //Debug.Log("TracetoIdle");
            MasterScript.JumpInActionByName("Idle");
        }
        else if (MasterScript.TargetDistance <= slime_data.AttackDistance)
        {
            // 跳到追擊
            //Debug.Log("TracetoPreAttack");
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
    }

    //進入動作的函式
    public void EnterAction(int currentId, int nextId)
    {
        if (!(nextId == ActionID && !(currentId == nextId)))
            return;
    }
}
