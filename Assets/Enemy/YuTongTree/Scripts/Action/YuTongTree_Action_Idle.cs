using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuTongTree_Action_Idle : ActionInterface
{
    // Start is called before the first frame update
    YuTongTree_MainScript MasterScript;
    YuTongTree_Data data;
    Quaternion Targetrotation;

    public int ActionID { get; set; }
    public string ActionName { get; set; }

    public YuTongTree_Action_Idle(GameCharatcer player, int ID, string Name)
    {
        SetState(player, ID, Name);
    }

    float idleTimer = 0f;

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

        idleTimer += Time.deltaTime;

        SetTargetrotation(0, MasterScript.AttackTarget);
        RotateAndMove(0);

        //檢查是否切換
        CheckChange(currentId);
    }

    //檢查能夠跳到那些動作
    public void CheckChange(int currentId)
    {
        if (MasterScript.AttackTarget == null)
            return;

        Rina_Mainscript rina = (Rina_Mainscript)MasterScript.AttackTarget;
        
        if (/*idleTimer >= data.idleTime &&*/ MasterScript.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && MasterScript.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            // 跳到追擊
            MasterScript.JumpInActionByName(YuTongTree_MainScript.ACTION_BLOCK_NAME);
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

    // 面向player，並加上指定的篇移角度
    void SetTargetrotation(float deltaAngle, GameCharatcer target)
    {
        GameObject StartReference = new GameObject();
        Vector3 TargetPosition = new Vector3(0, 0, 0);

        TargetPosition = target.CenterPosition;
        //TargetPosition = MasterScript.AttackTarget.CenterPosition;

        //設定理娜旋轉並得到目標角度
        StartReference.transform.position = MasterScript.transform.position;
        StartReference.transform.LookAt(TargetPosition);
        float TargetAngle = StartReference.transform.rotation.eulerAngles.y + deltaAngle;
        ControllDriver.RefreshAngles(ref TargetAngle);
        Targetrotation = Quaternion.Euler(0, TargetAngle, 0);

        GameObject.Destroy(StartReference);
    }

    // 轉向並移動
    void RotateAndMove(float speed)
    {
        MasterScript.gameCharacterController.transform.rotation = Quaternion.Slerp(
               MasterScript.gameCharacterController.transform.rotation,
               Targetrotation,
               Time.deltaTime * 4f);
        MasterScript.gameCharacterController.moveVector = MasterScript.gameCharacterController.transform.forward * speed;
    }
}
