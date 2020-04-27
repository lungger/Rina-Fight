using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuTongTree_Action_Block : ActionInterface
{
    // Start is called before the first frame update
    YuTongTree_MainScript MasterScript;
    YuTongTree_Data data;

    public int ActionID { get; set; }
    public string ActionName { get; set; }

    Quaternion Targetrotation;
    float blockTimer = 0f;

    bool isGoal = false;

    public YuTongTree_Action_Block(GameCharatcer player, int ID, string Name)
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

        blockTimer += Time.deltaTime;

        #region 動作實體
        Rina_Mainscript rina = (Rina_Mainscript)MasterScript.AttackTarget;

        // 幫擋子彈 (不用幫自己擋)
        if (MasterScript.HelpTarget != null && MasterScript.hpViewer == null)
        {
            Vector3 TargetPostion = GetTargetPostion(data.HelpDistance);
            // 轉向幫助怪物

            SetTargetrotation(0, TargetPostion);
            RotateAndMove(0);

            if (Vector3.Distance(MasterScript.HelpTarget.CenterPosition, TargetPostion) > 0.5f)
            {
                //Debug.Log("stop it");
                Vector3 moveVector = TargetPostion - MasterScript.gameCharacterController.transform.position;
                moveVector.y = 0.0f;
                if (moveVector.magnitude < 0.1f)
                {
                    moveVector = Vector3.zero;
                    isGoal = true;
                }
                MasterScript.gameCharacterController.moveVector = moveVector.normalized * data.MaxHelpSpeed;
            }
        }
        // 靠近玩家
        else if (MasterScript.TargetDistance > data.AttackDistance)
        {
            // 面相玩家
            SetTargetrotation(0, MasterScript.AttackTarget);
            RotateAndMove(data.MaxMoveSpeed);
        }
        // 繞玩家
        else
        {
            // 側向玩家
            SetTargetrotation(90, MasterScript.AttackTarget);
            RotateAndMove(data.MaxMoveSpeed);
        }

        #endregion

        //檢查是否切換
        CheckChange(currentId);
    }

    private Vector3 GetTargetPostion(float dis)
    {
        GameObject StartReference = new GameObject();
        Vector3 TargetPosition = new Vector3(0, 0, 0);

        TargetPosition = MasterScript.AttackTarget.CenterPosition;
        TargetPosition.y = 0.0f;

        Vector3 help = MasterScript.HelpTarget.CenterPosition;
        help.y = 0.0f;

        //設定理娜旋轉並得到目標角度
        StartReference.transform.position = MasterScript.transform.position;
        StartReference.transform.LookAt(TargetPosition);

        //StartReference.transform.position += StartReference.transform.forward * dis;
        StartReference.transform.position = (TargetPosition + help + help) / 3;
        Vector3 PostionRef = StartReference.transform.position;

        GameObject.Destroy(StartReference);

        return PostionRef;
    }

    //檢查能夠跳到那些動作
    public void CheckChange(int currentId)
    {
        if (MasterScript.AttackTarget == null || blockTimer <= data.walkTime)
            return;

        // 繼續保護
        if (MasterScript.HelpTarget != null && MasterScript.hpViewer == null)
        {
            //Debug.Log("now : " + MasterScript.HelpDistance + ", data : " + data.HelpDistance);
            //if (MasterScript.TargetDistance >= data.HelpDistance)
            if (!isGoal)
            {
                //Debug.Log("Block Continue");
                JumpIn();
            }
            else
            {
                //Debug.Log("Block to Idle");
                // 跳到待機嘲諷
                MasterScript.JumpInActionByName(YuTongTree_MainScript.ACTION_IDLE_NAME);
            }
        }
        else if (MasterScript.TargetDistance <= data.AttackDistance && blockTimer >= data.walkTime)
        {
            // 跳到攻擊
            MasterScript.JumpInActionByName(YuTongTree_MainScript.ACTION_ATTACK_NAME);
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

        blockTimer = 0f;
        isGoal = false;
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

    //毫無反應 只是多形:^)
    void SetTargetrotation(float deltaAngle, Vector3 target)
    {
        GameObject StartReference = new GameObject();
        Vector3 TargetPosition = new Vector3(0, 0, 0);

        TargetPosition = target;
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
