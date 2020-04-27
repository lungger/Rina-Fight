using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets;
public class FireMouse_Action_DieExplosion : ActionInterface
{
    FireMouse_MainScript MasterScript;
    public EffectPlayer effectPlayer;
    public bool ChargeStarted;
    public int DieExplosionChargeEffectID;
    public float DieExplosionStartTime;
    public float DieExplosionChargeTime;
    public int ActionID { get; set; }
    public string ActionName { get; set; }

    public FireMouse_Action_DieExplosion(GameCharatcer player, int ID, string Name)
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

        if (!ChargeStarted && MasterScript.animator.GetCurrentAnimatorStateInfo(0).IsName("DieExplosion") && MasterScript.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            ChargeStarted = true;
            MasterScript.isUnstoppable = true;
            MasterScript.canBeHit = false;
            EffectLibrary.Effect newEffect = new EffectLibrary.FireMouseDieExplosionCharge();
            DieExplosionChargeEffectID = MasterScript.effectPlayer.PlayEffect(ref newEffect, MasterScript.transform, MasterScript.transform.position, new Vector3(0f, 0f, 0f), 1f, 1f);
        }
        
        //檢查是否切換
        CheckChange(currentId);
    }

    //檢查能夠跳到那些動作
    public void CheckChange(int currentId)
    {
        if (MasterScript.gameObject && MasterScript.currentHp <= 0 && !ChargeStarted)
        {
            MasterScript.JumpInActionByName("Die");
        }

        if (Time.time - DieExplosionStartTime > DieExplosionChargeTime && !MasterScript.isDead)
        {
            MasterScript.effectPlayer.StopEffect(DieExplosionChargeEffectID);
            MasterScript.isDead = true;
            MasterScript.currentHp = 0;
            EffectLibrary.Effect newEffect = new EffectLibrary.FireMouseDieExplosion();
            MasterScript.effectPlayer.PlayEffect(ref newEffect, null, MasterScript.transform.position, new Vector3(0f, 0f, 0f), 1f, 1f);
            GameObject FireMouseDieExplosion = Object.Instantiate(MasterScript.Data.Attack_FireMouseDieExplosionPrefab);
            FireMouseDieExplosion.GetComponent<Attack_FireMouseDieExplosion>().PresetAttack(MasterScript, new Vector3(MasterScript.transform.position.x, MasterScript.transform.position.y + 0.3553333f, MasterScript.transform.position.z - 0.1599986f), MasterScript.AttackTarget.CenterPosition, new List<string>() { "Player" });
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
        DieExplosionStartTime = Time.time;
        DieExplosionChargeTime = MasterScript.Data.DieExplosionReadyTime;
        ChargeStarted = false;
    }
}
