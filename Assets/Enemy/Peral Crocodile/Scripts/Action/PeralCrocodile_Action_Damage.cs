using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeralCrocodile_Action_Damage : ActionInterface
{
    PeralCrocodile_MainScript MasterScript; //鱷魚 main script

    public int ActionID { get; set; }
    public string ActionName { get; set; }

    string thisActionName = PeralCrocodile_MainScript.ACTION_DAMAGE_NAME;

    public PeralCrocodile_Action_Damage(GameCharatcer player, int ID, string Name)
    {
        SetState(player, ID, Name);
    }

    //設定動作(有參數)
    public void SetState(GameCharatcer player, int ID, string Name)
    {
        MasterScript = (PeralCrocodile_MainScript)player;
        ActionID = ID;
        ActionName = Name;
    }

    //動作必須要有實體程式
    public void ProcessAction(int currentId)
    {
        //如果不等於此動作則退出
        if (!(currentId == ActionID))
            return;

        if (MasterScript.hitTimer > 0)
            MasterScript.hitTimer -= Time.deltaTime;
        else
            MasterScript.hitTimer = 0;

        //檢查是否切換
        CheckChange(currentId);
    }

    //檢查是否能跳到別的動作
    public void CheckChange(int currentId)
    {
        if (MasterScript.isDie)
        {
            MasterScript.JumpInActionByName(PeralCrocodile_MainScript.ACTION_DIE_NAME);
        }

        if (MasterScript.hitTimer <= 0.0f)
        {
            if (ControllDriver.DistenceOf(MasterScript.gameCharacterController.CenterPosition, MasterScript.AttackTarget.CenterPosition) <= MasterScript.PeralCrocodileData.MaxRunDistance)
            {
                MasterScript.JumpInActionByName(PeralCrocodile_MainScript.ACTION_RUN_NAME);
            }
            else
            {
                MasterScript.JumpInActionByName(PeralCrocodile_MainScript.ACTION_IDLE_NAME);
            }
            //else if (MasterScript.hpViewer == null)
            //{
            //    MasterScript.JumpInActionByName(PeralCrocodile_MainScript.ACTION_IDLE_NAME);
            //}
            //else
            //{
            //    MasterScript.JumpInActionByName(PeralCrocodile_MainScript.ACTION_WALK_NAME);
            //}
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
