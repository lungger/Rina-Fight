using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets;

public class Rina_Action_Jump : ActionInterface
{
    //Jump包含掉落(也就是正常的浮空狀態)
    public Rina_Mainscript MasterScript;
    public Rina_Data rina_Data;
    public GameCharacterController characterController;
    public Input_Manager InputState;
    public AudioSource JumpSound;
    public AudioSource DropSound;
    public EffectPlayer effectPlayer;
    public EffectLibrary.Effect jumpEffect = new EffectLibrary.Jump();
    public EffectLibrary.Effect doubleJumpEffect = new EffectLibrary.DoubleJump();



    public int ActionID { get; set; }
    public string ActionName { get; set; }

    public float jumpDelay = 0.1f;
    //跳躍時間
    public float jumptimer = 0;
    //跳躍動作狀態
    public int jumpstep = 0;
    //跳躍速度
    public float jumpVelocity = 0;
    //雙重跳旗標
    public bool fakeGround = false;
    //已經起跳
    public bool jumped = false;

    public Rina_Action_Jump(GameCharatcer player, int ID, string Name)
    {
        SetState(player, ID, Name);
        rina_Data = MasterScript.rina_Data;
        InputState = MasterScript.InputState;
        characterController = MasterScript.gameCharacterController;
        effectPlayer = MasterScript.effectPlayer;
    }

    //設定動作(必定先初始化)
    public void SetState(GameCharatcer player, int ID, string Name)
    {
        MasterScript = (Rina_Mainscript)player;
        ActionID = ID;
        ActionName = Name;

        //讀取跳躍聲音
        DropSound = SoundFinder.FindAudioSourceByName(MasterScript.Sounds, "Rina_Drop_0");
        JumpSound = SoundFinder.FindAudioSourceByName(MasterScript.Sounds, "Rina_Jump_Sound_0");
    }

    //動作必須要有實體程式
    public void ProcessAction(int currentId)
    {
        //如果不等於此動作則退出
        if (!(currentId == ActionID))
            return;

        jumptimer += Time.deltaTime;
        // jumpVelocity
        //跳整二段跳姿勢
        if (MasterScript.animator.GetFloat("Blend_Jump") < 1.0f && jumpstep >= 1)
        {
            MasterScript.animator.SetFloat("Blend_Jump", MasterScript.animator.GetFloat("Blend_Jump") + 20 * Time.deltaTime);
        }
        //起跳前
        if (jumptimer <= jumpDelay)
        {
            jumpVelocity = rina_Data.MaxJumpSpeed;
        }
        //起跳後才可以影響跳躍速度
        if (jumptimer >= jumpDelay && jumpstep == 0 && !jumped)
        {
            MasterScript.gameCharacterController.moveSpeed -= ((Vector3)(GameEnvironment.entity.GravityDirection))*jumpVelocity;
            jumped = true;
        }
        else if (jumptimer >= jumpDelay && jumpstep >= 1 && !jumped)
        {
            MasterScript.gameCharacterController.moveSpeed -= ((Vector3)(GameEnvironment.entity.GravityDirection)) * (jumpVelocity*1.1f);
            jumped = true;
        }
        //空中移動
        if (ControllDriver.IsAnyStickPushing_L(InputState) && jumptimer >= jumpDelay)
        {
            float moveAngle = ControllDriver.GetStickAngle_L(InputState);
            ControllDriver.NormalMove(ref MasterScript.lookReference, ref MasterScript.Master, moveAngle, 15+ MasterScript.RunSpeed / 1.25f);
        }

        //檢查是否切換
        CheckChange(currentId);
    }

    //檢查能夠跳到那些動作
    public void CheckChange(int currentId)
    {
        bool SkillButton_Now = InputState.Now.Button_Skill2;
        bool SkillButton_Last = InputState.Last.Button_Skill2;
        if (jumptimer >= 0.3f && MasterScript.gameCharacterController._IsGrounded)
        {
            jumpVelocity = 0f;
            MasterScript.JumpInActionByName("Idle");
            DropSound.PlayOneShot(DropSound.clip);
        }
        else if (jumpstep == 0 && jumptimer >= jumpDelay && MasterScript.gameCharacterController._IsGrounded == false && InputState.IsKeyDown(InputState.Now.Button_Jump, InputState.Last.Button_Jump))
        {
            //二段跳
            if (ControllDriver.IsAnyStickPushing_L(InputState) && MasterScript.RunSpeed < rina_Data.MaxJogSpeed)
                MasterScript.RunSpeed = rina_Data.MaxJogSpeed;
            //重製重力
            MasterScript.gameCharacterController.gravityVelocity = 0.0f;
            doubleJumpEffect = new EffectLibrary.DoubleJump();
            effectPlayer.PlayEffect(ref doubleJumpEffect, MasterScript.transform, MasterScript.transform.position, new Vector3(0f, 0f, 0f), 0.1f, 0.75f);
            jumptimer = 0;
            jumpstep += 1;
            jumpVelocity = 0f;
            MasterScript.animator.SetFloat("Blend_Jump", 0.0f);
            MasterScript.animator.SetInteger("ActionTrigger", 4);
            JumpSound.PlayOneShot(JumpSound.clip);
            jumped = false;
        }
        else if (InputState.IsKeyDown(SkillButton_Now,SkillButton_Last))
        {
            MasterScript.JumpInActionByName("Shot_Air");
        }
        else if (InputState.IsKeyDown(InputState.Now.Button_Attack, InputState.Last.Button_Attack))
        {
            MasterScript.JumpInActionByName("AirNormalAttack");
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
        JumpSound.enabled = false;
    }

    //進入動作的函式
    public void EnterAction(int currentId, int nextId)
    {
        if (!(nextId == ActionID && !(currentId == nextId)))
            return;
        DropSound.enabled = true;
        JumpSound.enabled = true;
        fakeGround = false;
        jumpEffect = new EffectLibrary.Jump();
        jumped = false;

        //確定有跳起來
        if (MasterScript.IsGrounded)
        {
            effectPlayer.PlayEffect(ref jumpEffect, null, MasterScript.transform.position, new Vector3(0f, 0f, 0f), 1f, 1f);
            MasterScript.animator.SetFloat("Blend_Jump", 0.0f);
            jumptimer = 0;
            jumpstep = 0;
            jumpVelocity = 0;
            JumpSound.PlayOneShot(JumpSound.clip);
        }
    }
}
