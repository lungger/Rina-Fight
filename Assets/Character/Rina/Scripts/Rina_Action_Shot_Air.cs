using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets;

public class Rina_Action_Shot_Air : ActionInterface
{
    Rina_Mainscript MasterScript;
    Rina_Data rina_Data;
    Input_Manager InputState;
    EffectLibrary.Effect shotEffect;
    AudioSource ShotSound;


    public const float shotMaxDelay = 0.3f;
    public bool shoted = false;
    public float shotDelay = 0;
    private bool combo = false;
    float moveAngle = 0;
    public int ActionID { get; set; }
    public string ActionName { get; set; }

    public Rina_Action_Shot_Air(GameCharatcer player, int ID, string Name)
    {
        SetState(player, ID, Name);
        rina_Data = MasterScript.rina_Data;
        InputState = MasterScript.InputState;
        ShotSound = SoundFinder.FindAudioSourceByName(MasterScript.Sounds, "Rina_Shot_Sound");
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

        bool SkillButton_Now = InputState.Now.Button_Skill2;
        bool SkillButton_Last = InputState.Last.Button_Skill2;
        ((Rina_Action_Jump)ActionFinder.GetActionByName(MasterScript.ActionSets, "Jump")).jumpVelocity =0;
        MasterScript.gameCharacterController.moveSpeed.y = MasterScript.gameCharacterController.gravityVelocity *0.975f;

        shotDelay += Time.deltaTime;
        GameObject StartReference = new GameObject();
        GameObject TargetReference = new GameObject();
        Vector3 TargetPosition = new Vector3(0, 0, 0);
        //鎖定模式的話
        if (MasterScript.cameraMode == LockMode.Lock)
        {
            TargetPosition = MasterScript.lockTarget.CenterPosition;
        }
        else
        {
            if (ControllDriver.IsAnyStickPushing_L(InputState))
                moveAngle = ControllDriver.GetStickAngle_L(InputState);
            ControllDriver.NormalMove(ref MasterScript.lookReference, ref MasterScript.Master, moveAngle, 0);
            TargetPosition = MasterScript.CenterPosition;
            TargetPosition += MasterScript.gameCharacterController.transform.forward * 20;
            TargetPosition.y = MasterScript.CenterPosition.y;
        }

        //設定理娜旋轉並得到目標角度
        StartReference.transform.position = MasterScript.transform.position;
        StartReference.transform.LookAt(TargetPosition);
        float TargetAngle = StartReference.transform.rotation.eulerAngles.y;
        ControllDriver.RefreshAngles(ref TargetAngle);
        Quaternion Targetrotation = Quaternion.Euler(0, TargetAngle, 0);

        //設定持續旋轉直到角度小於10
        MasterScript.gameCharacterController.transform.rotation = Quaternion.Slerp(MasterScript.gameCharacterController.transform.rotation, Targetrotation, Time.deltaTime * 10f);
        float RefreshedRinaRotation_Y = MasterScript.gameCharacterController.transform.rotation.eulerAngles.y;
        TargetAngle = Targetrotation.eulerAngles.y;
        ControllDriver.RefreshAngles(ref TargetAngle);
        ControllDriver.RefreshAngles(ref RefreshedRinaRotation_Y);

        //如果角度小於10並且發動時間大於0.1秒
        if (Mathf.Abs(Mathf.Abs(TargetAngle) - Mathf.Abs(RefreshedRinaRotation_Y)) < 10 && shotDelay > 0.2f)
        {
            if (!shoted)
            {
                //開始發射
                shoted = true;
                ShotSound.PlayOneShot(ShotSound.clip);
                shotEffect = new EffectLibrary.Gunshot();
                ((EffectLibrary.Gunshot)shotEffect).SetTargetPosition(TargetPosition);
                MasterScript.effectPlayer.PlayEffect(ref shotEffect, null, MasterScript.LeftHand.position, MasterScript.transform.rotation.eulerAngles, 1f, 1f);

                //發射本體
                GameObject rinaShotAttack = Object.Instantiate(rina_Data.rinashotPrefabs);
                rinaShotAttack.GetComponent<Attack_RinaShot_0_Script>().PresetAttack(MasterScript, MasterScript.LeftHand.position, TargetPosition, new List<string>() { "Enemy" });
            }
        }
        GameObject.Destroy(StartReference);
        GameObject.Destroy(TargetReference);
        if (InputState.IsKeyDown(SkillButton_Now, SkillButton_Last))
            combo = true;

        //檢查切換
        CheckChange(currentId);
    }

    //檢查能夠跳到那些動作
    public void CheckChange(int currentId)
    {
        Rina_Action_Jump JumpScript = ((Rina_Action_Jump)ActionFinder.GetActionByName(MasterScript.ActionSets, "Jump"));
        //有連發就中斷
        if (shoted == true && combo == true && !InputState.Now.Button_Jump)
        {
            JumpIn();
            shoted = false;
            combo = false;
            shotDelay = 0;
            return;
        }
        if (shoted == true && shotDelay > shotMaxDelay && InputState.Now.Button_Jump && JumpScript.jumpstep == 0)
        {
            MasterScript.JumpInActionByName("Jump");
            MasterScript.animator.SetFloat("Blend_Jump", 1.0f);
            MasterScript.gameCharacterController.moveSpeed.y = 0;
            MasterScript.gameCharacterController.moveVector.y = 0;
            MasterScript.gameCharacterController.gravityVelocity = 0.0f;
            JumpScript.jumptimer = 0;
            JumpScript.jumpstep += 1;
            JumpScript.jumpVelocity = 0;
            JumpScript.JumpSound.enabled = true;
            JumpScript.JumpSound.PlayOneShot(JumpScript.JumpSound.clip);
            JumpScript.doubleJumpEffect = new EffectLibrary.DoubleJump();
            JumpScript.effectPlayer.PlayEffect(ref ((Rina_Action_Jump)ActionFinder.GetActionByName(MasterScript.ActionSets, "Jump")).doubleJumpEffect, MasterScript.transform, MasterScript.transform.position, new Vector3(0f, 0f, 0f), 0.1f, 0.75f);
        }
        if (shoted == true && shotDelay > shotMaxDelay && MasterScript.IsGrounded && !ControllDriver.IsAnyStickPushing_L(InputState))
            MasterScript.JumpInActionByName("Idle");
        else if (shoted == true && shotDelay > shotMaxDelay && !MasterScript.IsGrounded)
        {
            MasterScript.JumpInActionByName("Jump");
            JumpScript.jumptimer = 0.2f;
        }
        else if (shoted == true && shotDelay > shotMaxDelay && MasterScript.IsGrounded && ControllDriver.IsAnyStickPushing_L(InputState) && !InputState.Now.Button_Dash)
            MasterScript.JumpInActionByName("Jog");
        else if (shoted == true && shotDelay > shotMaxDelay && MasterScript.IsGrounded && ControllDriver.IsAnyStickPushing_L(InputState) && InputState.Now.Button_Dash)
            MasterScript.JumpInActionByName("Run");
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
    }

    //進入動作的函式
    public void EnterAction(int currentId, int nextId)
    {
        if (!(nextId == ActionID && !(currentId == nextId)))
            return;
        //跑步特效關閉
        ParticleSystem.EmissionModule emissionModule = MasterScript.rina_Data.walkTrail.emission;
        emissionModule.enabled = false;
        shoted = false;
        combo = false;
        ShotSound.enabled = true;
        shotDelay = 0;
    }
}
