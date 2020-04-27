using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeralCrocodile_Action_Idle : ActionInterface
{
    PeralCrocodile_MainScript MasterScript; //鱷魚 main script

    public int ActionID { get; set; }
    public string ActionName { get; set; }

    public string thisActionName = PeralCrocodile_MainScript.ACTION_IDLE_NAME;

    Quaternion Targetrotation;

    float idleTime;
    float idleTimer = 0.0f;

    public PeralCrocodile_Action_Idle(GameCharatcer player, int ID, string Name)
    {
        SetState(player, ID, Name);
    }

    //設定動作(有參數)
    public void SetState(GameCharatcer player, int ID, string Name)
    {
        MasterScript = (PeralCrocodile_MainScript)player;
        ActionID = ID;
        ActionName = Name;

        idleTime = MasterScript.PeralCrocodileData.Idle_Time;
    }

    //動作必須要有實體程式
    public void ProcessAction(int currentId)
    {
        //如果不等於此動作則退出
        if (!(currentId == ActionID))
            return;
        
        if (MasterScript.AttackTarget != null)
        {
            SetTargetrotation(0);
            MasterScript.gameCharacterController.transform.rotation = Quaternion.Slerp(
                MasterScript.gameCharacterController.transform.rotation,
                Targetrotation,
                Time.deltaTime * 2f);
        }
        

        idleTimer += Time.deltaTime;

        //檢查是否切換
        CheckChange(currentId);
    }

    //檢查是否能跳到別的動作
    public void CheckChange(int currentId)
    {
        if (MasterScript.AttackTarget == null)
            return;
        
        if (ControllDriver.DistenceOf(MasterScript.gameCharacterController.CenterPosition, MasterScript.AttackTarget.CenterPosition) <= MasterScript.PeralCrocodileData.MaxRunDistance)
        {
            MasterScript.JumpInActionByName(PeralCrocodile_MainScript.ACTION_RUN_NAME);
        }

        //else if (MasterScript.hpViewer != null) //鎖定中
        //{
        //    MasterScript.JumpInActionByName(PeralCrocodile_MainScript.ACTION_WALK_NAME);
        //}
        else if (idleTimer >= idleTime)
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

        idleTimer = 0.0f;
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