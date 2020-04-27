using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets;
public class Rina_Action_Doge : ActionInterface
{
    Rina_Mainscript MasterScript;
    Rina_Data rina_Data;
    Input_Manager InputState;
    AudioSource ShotSound;

    //閃躲時間
    [HideInInspector]
    public float dogetimer = 0;

    //閃躲無敵時間
    [HideInInspector]
    public float dogeGodTime = 0.3f;

    //閃躲週期時間
    [HideInInspector]
    public float dogeCycleTime = 0.525f;

    //閃躲現時速度
    [HideInInspector]
    public float dogeCurrentSpeed = 0f;

    //閃躲時的角度
    [HideInInspector]
    public float dogeVector = 0f;

    //閃躲時的步驟
    [HideInInspector]
    public float dogeStep = 0;

    public int ActionID { get; set; }
    public string ActionName { get; set; }

    public Rina_Action_Doge(GameCharatcer player, int ID, string Name)
    {
        SetState(player, ID, Name);
        rina_Data = MasterScript.rina_Data;
        InputState = MasterScript.InputState;
        ShotSound = SoundFinder.FindAudioSourceByName(ChildrenFinder.FindByName(MasterScript.gameObject,"Sounds",0), "Rina_Doge_Sound");
    }

    //設定動作(必定先初始化)
    public void SetState(GameCharatcer player, int ID, string Name)
    {
        MasterScript = (Rina_Mainscript)player;
        ActionID = ID;
        ActionName = Name;
    }

    //動作必須要有實體程式
    public void ProcessAction(int currentId)
    {
        //如果不等於此動作則退出
        if (!(currentId == ActionID))
            return;

        if (dogetimer < dogeGodTime)
        {
            MasterScript.canBeHit = false;
        }
        else
        {
            MasterScript.canBeHit = true;
        }


        dogetimer += Time.deltaTime;
        if (dogetimer < dogeCycleTime)
        {
            if (dogetimer >= dogeCycleTime / 6)
            {
                dogeStep = 1;
                //開始減速
                if (MasterScript.GetStickDirectionState(dogeVector) == DirectState.Back)
                    ControllDriver.NormalMove(ref MasterScript.lookReference, ref MasterScript.Master, dogeVector, dogeCurrentSpeed * 1f);
                else
                    ControllDriver.NormalMove(ref MasterScript.lookReference, ref MasterScript.Master, dogeVector, dogeCurrentSpeed * 1f);
                if (dogeCurrentSpeed > rina_Data.MaxDogeSpeed / 3)
                    dogeCurrentSpeed -= 750 * Time.deltaTime;
                else
                    dogeCurrentSpeed = rina_Data.MaxDogeSpeed / 3;
            }
            else
            {
                if (MasterScript.GetStickDirectionState(dogeVector) == DirectState.Back)
                    ControllDriver.NormalMove(ref MasterScript.lookReference, ref MasterScript.Master, dogeVector, rina_Data.MaxDogeSpeed);
                else
                    ControllDriver.NormalMove(ref MasterScript.lookReference, ref MasterScript.Master, dogeVector, rina_Data.MaxDogeSpeed);
            }
        }

        //檢查是否切換
        CheckChange(currentId);
    }

    //檢查能夠跳到那些動作
    public void CheckChange(int currentId)
    {
        if (dogetimer >= dogeCycleTime)
        {

            //是否進入跑步
            if (MasterScript.StickDirectionState == DirectState.Null)
            {
                //待機   
                MasterScript.JumpInActionByName("Idle");
            }
            else if (!(MasterScript.StickDirectionState == DirectState.Null) && InputState.Now.Button_Dash == false)
            {
                //慢跑
                MasterScript.JumpInActionByName("Jog");
            }
            else if (!(MasterScript.StickDirectionState == DirectState.Null) && InputState.Now.Button_Dash == true)
            {
                //跑步
                MasterScript.JumpInActionByName("Run");
            }
        }
        if (MasterScript.IsGrounded == false)
        {
            //掉落
            MasterScript.JumpInActionByName("Jump");
            MasterScript.animator.SetBool("SetDrop", true);
            ((Rina_Action_Jump)ActionFinder.GetActionByName(MasterScript.ActionSets, "Jump")).jumptimer = 0.15f;
            ((Rina_Action_Jump)ActionFinder.GetActionByName(MasterScript.ActionSets, "Jump")).jumpstep = 0;
            ((Rina_Action_Jump)ActionFinder.GetActionByName(MasterScript.ActionSets, "Jump")).jumpVelocity = 0;
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
        ParticleSystem.EmissionModule emissionModule = MasterScript.rina_Data.walkTrail.emission;
        emissionModule.enabled = false;
        rina_Data.mtionblurVolume.enabled = false;
        MasterScript.canBeHit = true;
    }

    //進入動作的函式
    public void EnterAction(int currentId, int nextId)
    {
        if (!(nextId == ActionID && !(currentId == nextId)))
            return;
        //跑步特效關閉
        ParticleSystem.EmissionModule emissionModule = MasterScript.rina_Data.walkTrail.emission;
        emissionModule.enabled = false;
        //動態模糊
        rina_Data.mtionblurVolume.enabled = true;
        //迴避方向設定
        if (ControllDriver.IsAnyStickPushing_L(InputState))
            dogeVector = ControllDriver.GetStickAngle_L(InputState);
        //設定迴避速度
        dogeCurrentSpeed = rina_Data.MaxDogeSpeed;
        //迴避計時器歸
        dogetimer = 0;
        //迴避步驟
        dogeStep = 0;
        ShotSound.enabled = true;
        ShotSound.PlayOneShot(ShotSound.clip);
    }
}
