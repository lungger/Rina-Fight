using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets;

public class Rina_Action_NormalAttack : ActionInterface
{
    Rina_Mainscript MasterScript;
    Rina_Data rina_Data;
    Input_Manager InputState;
    EffectLibrary.Effect shotEffect;
    AudioSource ShotSound;
    Animator animator;
    //AudioSource AttackSound;

    public const float attackMaxDelay = 0.5f;
    public bool attacked = false;
    public float attackDelayTimer = 0;
    public List<float> attackOutDelay = new List<float>() { 0.1f, 0.15f, 0.2f };
    public bool attackStep = false;
    private bool combo = false;
    private bool ToKickUp = false;
    private bool shoted = false;
    public int attackCount = 0;
    public int ActionID { get; set; }
    public string ActionName { get; set; }

    public Rina_Action_NormalAttack(GameCharatcer player, int ID, string Name)
    {
        SetState(player, ID, Name);
        rina_Data = MasterScript.rina_Data;
        InputState = MasterScript.InputState;
        ShotSound = SoundFinder.FindAudioSourceByName(MasterScript.Sounds, "Rina_Shot_Sound");
    }

    //得到所有角度
    private void ProcessAngleSetting(ref GameObject StartReference, ref GameObject TargetReference, ref Vector3 TargetPosition, ref Quaternion Targetrotation, ref float TargetAngle, ref float RefreshedRinaRotation_Y)
    {
        //鎖定模式的話
        if (MasterScript.cameraMode == LockMode.Lock)
        {
            TargetPosition = MasterScript.lockTarget.CenterPosition;
        }
        else
        {
            TargetPosition = MasterScript.CenterPosition;
            TargetPosition += MasterScript.gameCharacterController.transform.forward * 20;
            TargetPosition.y = MasterScript.CenterPosition.y;
        }
        //設定理娜旋轉並得到目標角度
        StartReference.transform.position = MasterScript.transform.position;
        StartReference.transform.LookAt(TargetPosition);
        TargetAngle = StartReference.transform.rotation.eulerAngles.y;
        ControllDriver.RefreshAngles(ref TargetAngle);
        Targetrotation = Quaternion.Euler(0, TargetAngle, 0);

        //設定持續旋轉直到角度小於10
        MasterScript.gameCharacterController.transform.rotation = Quaternion.Slerp(MasterScript.gameCharacterController.transform.rotation, Targetrotation, Time.deltaTime * 100f);
        RefreshedRinaRotation_Y = MasterScript.gameCharacterController.transform.rotation.eulerAngles.y;
        TargetAngle = Targetrotation.eulerAngles.y;
        ControllDriver.RefreshAngles(ref TargetAngle);
        ControllDriver.RefreshAngles(ref RefreshedRinaRotation_Y);
    }

    //第一次攻擊的程式碼
    private void Attack_1(ref GameObject StartReference, ref GameObject TargetReference, ref Vector3 TargetPosition, ref Quaternion Targetrotation, ref float TargetAngle, ref float RefreshedRinaRotation_Y)
    {
        //開始發射
        attacked = true;
        GameObject rinaNormalAttack_0 = Object.Instantiate(rina_Data.rinaNormalAttack_0);
        rinaNormalAttack_0.GetComponent<Attack_RinaNormalAttack_0_Script>().PresetAttack(MasterScript, MasterScript.LeftHand.position, TargetPosition, new List<string>() { "Enemy" });
    }

    //第二次攻擊的程式碼
    private void Attack_2(ref GameObject StartReference, ref GameObject TargetReference, ref Vector3 TargetPosition, ref Quaternion Targetrotation, ref float TargetAngle, ref float RefreshedRinaRotation_Y)
    {
        attacked = true;
        GameObject rinaNormalAttack_0 = Object.Instantiate(rina_Data.rinaNormalAttack_0);
        rinaNormalAttack_0.GetComponent<Attack_RinaNormalAttack_0_Script>().PresetAttack(MasterScript, MasterScript.RightHand.position, TargetPosition, new List<string>() { "Enemy" });
    }

    //第三次攻擊的程式碼
    private void Attack_3(ref GameObject StartReference, ref GameObject TargetReference, ref Vector3 TargetPosition, ref Quaternion Targetrotation, ref float TargetAngle, ref float RefreshedRinaRotation_Y)
    {
        attacked = true;
        attacked = true;
        GameObject rinaNormalAttack_1 = Object.Instantiate(rina_Data.rinaNormalAttack_1);
        rinaNormalAttack_1.GetComponent<Attack_RinaNormalAttack_0_Script>().PresetAttack(MasterScript, MasterScript.CenterPosition +MasterScript.transform.forward*0.5f, TargetPosition, new List<string>() { "Enemy" });
    }

    //第一次攻擊附帶射擊的程式碼
    private void Shot_1(ref GameObject StartReference, ref GameObject TargetReference, ref Vector3 TargetPosition, ref Quaternion Targetrotation, ref float TargetAngle, ref float RefreshedRinaRotation_Y)
    {
        ShotSound.PlayOneShot(ShotSound.clip);
        shotEffect = new EffectLibrary.Gunshot();
        ((EffectLibrary.Gunshot)shotEffect).SetTargetPosition(TargetPosition);
        MasterScript.effectPlayer.PlayEffect(ref shotEffect, null, MasterScript.LeftHand.position, MasterScript.transform.rotation.eulerAngles, 1f, 1f);
        //發射本體
        GameObject rinaShotAttack = Object.Instantiate(rina_Data.rinashotPrefabs);
        rinaShotAttack.GetComponent<Attack_RinaShot_0_Script>().PresetAttack(MasterScript, MasterScript.LeftHand.position, TargetPosition, new List<string>() { "Enemy" });
        shoted = true;
    }

    //第二次攻擊附帶射擊的程式碼
    private void Shot_2(ref GameObject StartReference, ref GameObject TargetReference, ref Vector3 TargetPosition, ref Quaternion Targetrotation, ref float TargetAngle, ref float RefreshedRinaRotation_Y)
    {
        ShotSound.PlayOneShot(ShotSound.clip);
        shotEffect = new EffectLibrary.Gunshot();
        ((EffectLibrary.Gunshot)shotEffect).SetTargetPosition(TargetPosition);
        MasterScript.effectPlayer.PlayEffect(ref shotEffect, null, MasterScript.RightHand.position, MasterScript.transform.rotation.eulerAngles, 1f, 1f);
        //發射本體
        GameObject rinaShotAttack = Object.Instantiate(rina_Data.rinashotPrefabs);
        rinaShotAttack.GetComponent<Attack_RinaShot_0_Script>().PresetAttack(MasterScript, MasterScript.RightHand.position, TargetPosition, new List<string>() { "Enemy" });
        shoted = true;
    }

    //第三次攻擊附帶射擊的程式碼
    private void Shot_3(ref GameObject StartReference, ref GameObject TargetReference, ref Vector3 TargetPosition, ref Quaternion Targetrotation, ref float TargetAngle, ref float RefreshedRinaRotation_Y)
    {
        ShotSound.PlayOneShot(ShotSound.clip);
        shotEffect = new EffectLibrary.Gunshot();
        ((EffectLibrary.Gunshot)shotEffect).SetTargetPosition(TargetPosition);
        MasterScript.effectPlayer.PlayEffect(ref shotEffect, null, MasterScript.Head.position+MasterScript.transform.forward*0.75f, MasterScript.transform.rotation.eulerAngles, 1f, 1f);
        //發射本體
        GameObject rinaShotAttack = Object.Instantiate(rina_Data.rinashotPrefabs);
        rinaShotAttack.GetComponent<Attack_RinaShot_0_Script>().PresetAttack(MasterScript, MasterScript.Head.position + MasterScript.transform.forward * 0.75f, TargetPosition, new List<string>() { "Enemy" });
        shoted = true;
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
        animator = MasterScript.animator;
        bool AttackButton_Now = InputState.Now.Button_Attack;
        bool AttackButton_Last = InputState.Last.Button_Attack;
        bool Skill2Button_Now = InputState.Now.Button_Skill2;
        bool Skill2Button_Last = InputState.Last.Button_Skill2;

        GameObject StartReference = new GameObject();
        GameObject TargetReference = new GameObject();
        Vector3 TargetPosition = new Vector3(0, 0, 0);
        Quaternion Targetrotation = new Quaternion(0, 0, 0, 0);
        float TargetAngle = 0;
        float RefreshedRinaRotation_Y = 0;

        attackDelayTimer += Time.deltaTime;
        //得到所有角度
        ProcessAngleSetting(ref StartReference, ref TargetReference, ref TargetPosition, ref Targetrotation, ref TargetAngle, ref RefreshedRinaRotation_Y);

        //如果角度小於10並且發動時間大於0.1秒
        if (Mathf.Abs(Mathf.Abs(TargetAngle) - Mathf.Abs(RefreshedRinaRotation_Y)) < 30 && attackDelayTimer > attackOutDelay[attackCount])
        {
            if (!attacked)
            {
                if (attackCount == 0)
                    Attack_1(ref StartReference, ref TargetReference, ref TargetPosition, ref Targetrotation, ref TargetAngle, ref RefreshedRinaRotation_Y);
                else if(attackCount == 1)
                    Attack_2(ref StartReference, ref TargetReference, ref TargetPosition, ref Targetrotation, ref TargetAngle, ref RefreshedRinaRotation_Y);
                else if (attackCount == 2)
                    Attack_3(ref StartReference, ref TargetReference, ref TargetPosition, ref Targetrotation, ref TargetAngle, ref RefreshedRinaRotation_Y);
            }
        }
        if (InputState.IsKeyDown(Skill2Button_Now, Skill2Button_Last) && shoted == false)
        {
            if (attackCount == 0)
                Shot_1(ref StartReference, ref TargetReference, ref TargetPosition, ref Targetrotation, ref TargetAngle, ref RefreshedRinaRotation_Y);
            else if (attackCount == 1)
                Shot_2(ref StartReference, ref TargetReference, ref TargetPosition, ref Targetrotation, ref TargetAngle, ref RefreshedRinaRotation_Y);
            else if (attackCount == 2)
                Shot_3(ref StartReference, ref TargetReference, ref TargetPosition, ref Targetrotation, ref TargetAngle, ref RefreshedRinaRotation_Y);
        }
        if (InputState.IsKeyDown(AttackButton_Now, AttackButton_Last))
            combo = true;
        if (InputState.IsKeyDown(InputState.Now.Button_Skill1, InputState.Last.Button_Skill1))
            ToKickUp = true;

        GameObject.Destroy(StartReference);
        GameObject.Destroy(TargetReference);
        //檢查切換
        CheckChange(currentId);
    }

    //檢查能夠跳到那些動作
    public void CheckChange(int currentId)
    {
        Rina_Action_Jump JumpScript = ((Rina_Action_Jump)ActionFinder.GetActionByName(MasterScript.ActionSets, "Jump"));
        //有連發就中斷
        if (attacked == true && combo == true && attackCount < 2 && attackDelayTimer > attackMaxDelay*0.75f)
        {
            JumpIn();
            attacked = false;
            combo = false;
            shoted = false;
            ToKickUp = false;
            attackDelayTimer = 0;
            attackCount++;
            animator.SetFloat("Blend_NormalAttack", attackCount * 0.5f);
            animator.SetInteger("ActionTrigger", ActionID);
            MasterScript.gameCharacterController.moveSpeed += MasterScript.transform.forward * 20;
            return;
        }
        if (attacked == true && attackDelayTimer < attackMaxDelay && InputState.Now.Button_Jump)
        {
            MasterScript.JumpInActionByName("Jump");
            return;
        }
        if (attacked == true&&ToKickUp && attackDelayTimer < attackMaxDelay )
        {
            MasterScript.JumpInActionByName("KickUp");
            return;
        }
        else if (attacked == true && attackDelayTimer > attackMaxDelay)
        {
            MasterScript.JumpInActionByName("Idle");
            return;
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
    }

    //進入動作的函式
    public void EnterAction(int currentId, int nextId)
    {
        if (!(nextId == ActionID && !(currentId == nextId)))
            return;
        //跑步特效關閉
        ParticleSystem.EmissionModule emissionModule = MasterScript.rina_Data.walkTrail.emission;
        emissionModule.enabled = false;
        animator = MasterScript.animator;
        attacked = false;
        combo = false;
        shoted = false;
        ToKickUp = false;
        attackCount = 0;
        attackDelayTimer = 0;
        animator.SetFloat("Blend_NormalAttack", attackCount * 0.5f);
        MasterScript.gameCharacterController.moveSpeed += MasterScript.transform.forward * 20;
    }
}
