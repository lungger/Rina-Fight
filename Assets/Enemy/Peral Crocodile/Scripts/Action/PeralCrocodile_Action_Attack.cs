using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeralCrocodile_Action_Attack : ActionInterface
{
    PeralCrocodile_MainScript MasterScript; //鱷魚 main script

    public int ActionID { get; set; }
    public string ActionName { get; set; }

    GameCharatcer attackTarget;
    GameCharacterController controller;
    bool AttackObjectBuilded;
    float shootDelay;
    float shootDelayTimer = 0.0f;
    string thisActionName = PeralCrocodile_MainScript.ACTION_ATTACK_NAME;

    public PeralCrocodile_Action_Attack(GameCharatcer player, int ID, string Name)
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
        shootDelay = MasterScript.PeralCrocodileData.ShootDelay;
    }

    //動作必須要有實體程式
    public void ProcessAction(int currentId)
    {
        //如果不等於此動作則退出
        if (!(currentId == ActionID))
            return;

        if (!AttackObjectBuilded && shootDelayTimer >= shootDelay)
        {
            AttackObjectBuilded = true;
            GameObject peral = Object.Instantiate(MasterScript.PeralCrocodileData.Peral);
            peral.GetComponent<AttackObject_Peral>().PresetAttack(
                MasterScript,
                MasterScript.CenterPosition + MasterScript.transform.up * 0.5f + MasterScript.transform.forward * 2f,
                MasterScript.AttackTarget.CenterPosition,
                new List<string>() { "Player" });
            peral.transform.rotation = MasterScript.gameCharacterController.transform.rotation;
        }
        shootDelayTimer += Time.deltaTime;

        //檢查是否切換
        CheckChange(currentId);
    }

    //檢查是否能跳到別的動作
    public void CheckChange(int currentId)
    {
        if (MasterScript.animator.GetCurrentAnimatorStateInfo(0).IsName(thisActionName) && MasterScript.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            if (MasterScript.hpViewer == null)
            {
                MasterScript.JumpInActionByName(PeralCrocodile_MainScript.ACTION_IDLE_NAME);
            }
            else
            {
                MasterScript.JumpInActionByName(PeralCrocodile_MainScript.ACTION_RUN_NAME);
            }
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
        AttackObjectBuilded = false;
        shootDelayTimer = 0.0f;
    }
}
