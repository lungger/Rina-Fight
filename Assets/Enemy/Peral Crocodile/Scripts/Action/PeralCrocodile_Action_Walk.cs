using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeralCrocodile_Action_Walk : ActionInterface
{
    PeralCrocodile_MainScript MasterScript; //鱷魚 main script

    public int ActionID { get; set; }
    public string ActionName { get; set; }

    string thisActionName = PeralCrocodile_MainScript.ACTION_WALK_NAME;

    GameCharatcer attackTarget;
    GameCharacterController controller;

    Quaternion Targetrotation;

    float walkSpeed = 15.0f;

    float walkTimer = 1.5f;
    float SeePlayerTimer = 1.2f;

    public PeralCrocodile_Action_Walk(GameCharatcer player, int ID, string Name)
    {
        SetState(player, ID, Name);
    }

    //設定動作(有參數)
    public void SetState(GameCharatcer player, int ID, string Name)
    {
        MasterScript = (PeralCrocodile_MainScript)player;
        ActionID = ID;
        ActionName = Name;

        attackTarget = MasterScript.AttackTarget;
        controller = MasterScript.gameCharacterController;

        walkSpeed = MasterScript.PeralCrocodileData.WalkSpeed;
        walkTimer = MasterScript.PeralCrocodileData.WalkTime;
        SeePlayerTimer = MasterScript.PeralCrocodileData.SeePlayerTimeOnWalk;
    }

    //動作必須要有實體程式
    public void ProcessAction(int currentId)
    {
        //如果不等於此動作則退出
        if (!(currentId == ActionID))
            return;

        attackTarget = MasterScript.AttackTarget;
        controller = MasterScript.gameCharacterController;

        if (walkTimer >= 0.0f)
        {
            MasterScript.gameCharacterController.transform.rotation = Quaternion.Slerp(
                MasterScript.gameCharacterController.transform.rotation,
                Targetrotation,
                Time.deltaTime * 2f);
                MasterScript.gameCharacterController.moveVector = MasterScript.gameCharacterController.transform.forward * walkSpeed;
        }
        else
        {
            if (SeePlayerTimer >= 0.0f) //先轉向Player , SeePlayerTime < 0時, CheckChange 到 Attack
            {
                SetTargetrotation(0.0f);
                MasterScript.gameCharacterController.transform.rotation = Quaternion.Slerp(
                MasterScript.gameCharacterController.transform.rotation,
                Targetrotation,
                Time.deltaTime * 3f);
            }
            SeePlayerTimer -= Time.deltaTime;
        }

        walkTimer -= Time.deltaTime;

        //檢查是否切換
        CheckChange(currentId);
    }

    //檢查是否能跳到別的動作
    public void CheckChange(int currentId)
    {
        if (ControllDriver.DistenceOf(MasterScript.gameCharacterController.CenterPosition, attackTarget.CenterPosition) <= MasterScript.PeralCrocodileData.MaxRunDistance)
        {
            MasterScript.JumpInActionByName(PeralCrocodile_MainScript.ACTION_RUN_NAME);
        }

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
        attackTarget = MasterScript.AttackTarget;
        controller = MasterScript.gameCharacterController;
        SetTargetrotation(-180);

        walkTimer = MasterScript.PeralCrocodileData.WalkTime;
        SeePlayerTimer = MasterScript.PeralCrocodileData.SeePlayerTimeOnWalk;
    }

    void SetTargetrotation(float deltaAngle)
    {
        GameObject StartReference = new GameObject();
        Vector3 TargetPosition = new Vector3(0, 0, 0);

        TargetPosition = attackTarget.CenterPosition;

        //設定理娜旋轉並得到目標角度
        StartReference.transform.position = MasterScript.transform.position;
        StartReference.transform.LookAt(TargetPosition);
        float TargetAngle =  StartReference.transform.rotation.eulerAngles.y + deltaAngle;
        ControllDriver.RefreshAngles(ref TargetAngle);
        Targetrotation = Quaternion.Euler(0,  TargetAngle, 0);

        GameObject.Destroy(StartReference);
    }
}
