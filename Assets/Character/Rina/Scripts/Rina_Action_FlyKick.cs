using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets;

public class Rina_Action_FlyKick : ActionInterface
{
    Rina_Mainscript MasterScript;
    Rina_Data rina_Data;
    Input_Manager InputState;
    EffectLibrary.Effect shotEffect;
    AudioSource ShotSound;
    Animator animator;
    //AudioSource AttackSound;

    public const float attackMaxDelay = 0.6f;
    public bool attacked = false;
    public float attackDelayTimer = 0;
    public float attackOutDelay  = 0.275f;
    public bool attackStep = false;
    private bool combo = false;
    private bool shoted = false;
    public int attackCount = 0;
    public int ActionID { get; set; }
    public string ActionName { get; set; }

    public Rina_Action_FlyKick(GameCharatcer player, int ID, string Name)
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
        GameObject rinaNormalAttack_0 = Object.Instantiate(rina_Data.rinaFlyAttack_0);
        rinaNormalAttack_0.transform.SetParent(MasterScript.gameObject.transform);
        rinaNormalAttack_0.GetComponent<Attack_RinaNormalAttack_0_Script>().PresetAttack(MasterScript, MasterScript.transform.position+Vector3.up*0.8f+ MasterScript.transform.forward*0.75f, TargetPosition, new List<string>() { "Enemy" });
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
        if (Mathf.Abs(Mathf.Abs(TargetAngle) - Mathf.Abs(RefreshedRinaRotation_Y)) < 30 && attackDelayTimer > attackOutDelay)
        {
            if (!attacked)
            {
                MasterScript.gameCharacterController.moveSpeed += MasterScript.transform.forward * 250;
                Attack_1(ref StartReference, ref TargetReference, ref TargetPosition, ref Targetrotation, ref TargetAngle, ref RefreshedRinaRotation_Y);
            }
        }

        GameObject.Destroy(StartReference);
        GameObject.Destroy(TargetReference);
        //檢查切換
        CheckChange(currentId);
    }

    //檢查能夠跳到那些動作
    public void CheckChange(int currentId)
    {
        if (attacked == true && attackDelayTimer > attackMaxDelay)
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
        attackCount = 0;
        attackDelayTimer = 0;
    }
}
