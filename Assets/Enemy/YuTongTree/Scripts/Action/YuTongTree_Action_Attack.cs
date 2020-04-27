using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuTongTree_Action_Attack : ActionInterface
{
    // Start is called before the first frame update
    YuTongTree_MainScript MasterScript;
    YuTongTree_Data data;

    public int ActionID { get; set; }
    public string ActionName { get; set; }

    float attackCoolDownTimer = 0f;
    bool attacked = false;

    public YuTongTree_Action_Attack(GameCharatcer player, int ID, string Name)
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

        // 攻擊冷卻時間
        attackCoolDownTimer += Time.deltaTime;
        // 轉向玩家
        MasterScript.gameCharacterController.transform.rotation = Quaternion.Slerp(
                MasterScript.gameCharacterController.transform.rotation,
                GetTargetrotation(0),
                Time.deltaTime * 4f);
        //MasterScript.gameCharacterController.moveVector = MasterScript.gameCharacterController.transform.forward * data.MaxMoveSpeed;

        #region 動作實體
        if (!attacked)
        {
            data.attackScripts.Attack();
            attacked = true;
        }
        #endregion

        //檢查是否切換
        CheckChange(currentId);
    }

    //檢查能夠跳到那些動作
    public void CheckChange(int currentId)
    {
        if (MasterScript.AttackTarget == null)
            return;

        // 撥完攻擊動畫就切回 Block
        if (MasterScript.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && MasterScript.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            MasterScript.JumpInActionByName(YuTongTree_MainScript.ACTION_BLOCK_NAME);
        }
        //if (MasterScript.TargetDistance >= data.AttackDistance)
        //{
        //    // 回到行走
        //    MasterScript.JumpInActionByName(YuTongTree_MainScript.ACTION_BLOCK_NAME);
        //}
        //else if (MasterScript.TargetDistance < data.AttackDistance && attackCoolDownTimer >= data.AttackCoolDownTime)
        //{
        //    //到攻擊，有成功攻擊的話就把CoolDown歸0
        //    attackCoolDownTimer = 0;
        //    MasterScript.JumpInActionByName(YuTongTree_MainScript.ACTION_IDLE_NAME);
        //}
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
        //Debug.Log("Enter attack");
        attacked = false;
        attackCoolDownTimer = 0f;
    }

    // 面向player，並加上指定的篇移角度
    Quaternion GetTargetrotation(float deltaAngle)
    {
        GameObject StartReference = new GameObject();
        Vector3 TargetPosition = new Vector3(0, 0, 0);

        TargetPosition = MasterScript.AttackTarget.CenterPosition;

        //設定理娜旋轉並得到目標角度
        StartReference.transform.position = MasterScript.transform.position;
        StartReference.transform.LookAt(TargetPosition);
        float TargetAngle = StartReference.transform.rotation.eulerAngles.y + deltaAngle;
        ControllDriver.RefreshAngles(ref TargetAngle);
        GameObject.Destroy(StartReference);
        return Quaternion.Euler(0, TargetAngle, 0);
    }
}
