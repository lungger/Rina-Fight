using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeralCrocodile_Action_Run : ActionInterface
{
    PeralCrocodile_MainScript MasterScript; //鱷魚 main script

    public int ActionID { get; set; }
    public string ActionName { get; set; }

    string thisActionName = PeralCrocodile_MainScript.ACTION_RUN_NAME;

    float runTimer;
    float SeePlayerTimer;
    float runSpeed;

    Quaternion Targetrotation;

    public PeralCrocodile_Action_Run(GameCharatcer player, int ID, string Name)
    {
        SetState(player, ID, Name);
    }

    //設定動作(有參數)
    public void SetState(GameCharatcer player, int ID, string Name)
    {
        MasterScript = (PeralCrocodile_MainScript)player;
        ActionID = ID;
        ActionName = Name;

        runTimer = MasterScript.PeralCrocodileData.RunTime;
        runSpeed = MasterScript.PeralCrocodileData.RunSpeed;
        SeePlayerTimer = MasterScript.PeralCrocodileData.SeePlayerTimeOnRun;
    }

    //動作必須要有實體程式
    public void ProcessAction(int currentId)
    {
        //如果不等於此動作則退出
        if (!(currentId == ActionID))
            return;

        if (runTimer >= 0.0f) //跑步中
        {
            SetTargetrotation(-180); //時時背對Player跑步
            MasterScript.gameCharacterController.transform.rotation = Quaternion.Slerp(
                MasterScript.gameCharacterController.transform.rotation,
                Targetrotation,
                Time.deltaTime * 4f);
            MasterScript.gameCharacterController.moveVector = MasterScript.gameCharacterController.transform.forward * runSpeed;
            runTimer -= Time.deltaTime;
        }
        else
        {
            if (SeePlayerTimer >= 0.0f) //跑步結束 先轉向 在攻擊
            {
                SetTargetrotation(0.0f); //面對Player
                MasterScript.gameCharacterController.transform.rotation = Quaternion.Slerp(
                MasterScript.gameCharacterController.transform.rotation,
                Targetrotation,
                Time.deltaTime * 4f);
            }
            SeePlayerTimer -= Time.deltaTime;
        }

        //檢查是否切換
        CheckChange(currentId);
    }

    //檢查是否能跳到別的動作
    public void CheckChange(int currentId)
    {
        if (SeePlayerTimer < 0.0f)
        {
            MasterScript.JumpInActionByName(PeralCrocodile_MainScript.ACTION_ATTACK_NAME);
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

        
        runTimer = MasterScript.PeralCrocodileData.RunTime;
        SeePlayerTimer = MasterScript.PeralCrocodileData.SeePlayerTimeOnRun;
    }

    void SetTargetrotation(float deltaAngle)
    {
        GameObject StartReference = new GameObject();
        Vector3 TargetPosition = new Vector3(0, 0, 0);

        TargetPosition = MasterScript.AttackTarget.CenterPosition;

        //設定理娜旋轉並得到目標角度
        StartReference.transform.position = MasterScript.transform.position;
        StartReference.transform.LookAt(TargetPosition);
        float TargetAngle = StartReference.transform.rotation.eulerAngles.y + deltaAngle;
        ControllDriver.RefreshAngles(ref TargetAngle);
        Targetrotation = Quaternion.Euler(0, TargetAngle, 0);

        GameObject.Destroy(StartReference);
    }
}
