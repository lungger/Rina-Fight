using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets;
public class FireMouse_Action_RollAttack : ActionInterface
{
    FireMouse_MainScript MasterScript;

    public float StartTime;
    public float ReadyTime;
    public float RollTime;
    public bool ColiderBuilded;
    public bool RollTimeDecided;
    public GameObject FireMouseRollAttack;
    public int ActionID { get; set; }
    public string ActionName { get; set; }

    public FireMouse_Action_RollAttack(GameCharatcer player, int ID, string Name)
    {
        SetState(player, ID, Name);
    }

    //設定動作(必定先初始化)
    public void SetState(GameCharatcer player, int ID, string Name)
    {
        MasterScript = (FireMouse_MainScript)player;
        ActionID = ID;
        ActionName = Name;
    }

    //動作必須要有實體程式
    public void ProcessAction(int currentId)
    {
        //如果不等於此動作則退出
        if (!(currentId == ActionID))
            return;

        if (!ColiderBuilded)
        {
            ColiderBuilded = true;
            FireMouseRollAttack = Object.Instantiate(MasterScript.Data.Attack_FireMouseRollAttackPrefab, MasterScript.transform);
            FireMouseRollAttack.GetComponent<Attack_FireMouseRollAttack>().PresetAttack(MasterScript, new Vector3(0f, 0.3553333f, -0.1599986f), MasterScript.AttackTarget.CenterPosition, new List<string>() { "Player" });
        }

        if(Time.time - StartTime < ReadyTime)
        {
            MasterScript.transform.LookAt(MasterScript.AttackTarget.transform);
            MasterScript.transform.eulerAngles = new Vector3(0f, MasterScript.transform.eulerAngles.y, 0f);
        }
        else
        {
            if (!RollTimeDecided)
            {
                RollTimeDecided = true;
                float DistanceToAttackTarget = (MasterScript.transform.position - MasterScript.AttackTarget.transform.position).magnitude;
                if(DistanceToAttackTarget > MasterScript.Data.MaxRollAttackDistance)
                {
                    DistanceToAttackTarget = MasterScript.Data.MaxRollAttackDistance;
                }
                RollTime = (DistanceToAttackTarget + MasterScript.Data.RollAttackExtraDistance) / (MasterScript.Data.RollAttackSpeed / 10f);
                if (ColiderBuilded && FireMouseRollAttack != null)
                {
                    FireMouseRollAttack.GetComponent<Attack_FireMouseRollAttack>().maxLifeTime = ReadyTime + RollTime;
                }
            }
            MasterScript.isUnstoppable = true;
            Vector3 tempMoveVector = MasterScript.transform.forward * MasterScript.Data.RollAttackSpeed;
            tempMoveVector.y = 0f;
            MasterScript.gameCharacterController.moveVector = tempMoveVector;
        }
        


        //檢查是否切換
        CheckChange(currentId);
    }

    //檢查能夠跳到那些動作
    public void CheckChange(int currentId)
    {
        if(Time.time - StartTime >= ReadyTime + RollTime)
        {
            MasterScript.isUnstoppable = false;
            MasterScript.JumpInActionByName("Idle");
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

        if (ColiderBuilded && FireMouseRollAttack != null)
        {
            FireMouseRollAttack.GetComponent<Attack_FireMouseRollAttack>().hittedTimer = FireMouseRollAttack.GetComponent<Attack_FireMouseRollAttack>().hittedDeleteTime;
            FireMouseRollAttack.GetComponent<Attack_FireMouseRollAttack>().liveTime = FireMouseRollAttack.GetComponent<Attack_FireMouseRollAttack>().maxLifeTime;
        }
    }

    //進入動作的函式
    public void EnterAction(int currentId, int nextId)
    {
        if (!(nextId == ActionID && !(currentId == nextId)))
            return;

        StartTime = Time.time;
        ReadyTime = MasterScript.Data.RollAttackReadyTime;
        RollTime = 0.5f;
        RollTimeDecided = false;
        ColiderBuilded = false;

    }
}
