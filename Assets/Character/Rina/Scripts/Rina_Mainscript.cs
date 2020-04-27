using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets;

public class Rina_Mainscript : PlayableCharacter
{
    #region 動作編號常數
    //待機
    public const int ACTION_IDLE_ID = 0;
    public const string ACTION_IDLE_NAME = "Idle";
    //慢跑
    public const int ACTION_JOG_ID = 1;
    public const string ACTION_JOG_NAME = "Jog";
    //跑步
    public const int ACTION_RUN_ID = 2;
    public const string ACTION_RUN_NAME = "Run";
    //迴避
    public const int ACTION_DOGE_ID = 3;
    public const string ACTION_DOGE_NAME = "Doge";
    //跳躍
    public const int ACTION_JUMP_ID = 4;
    public const string ACTION_JUMP_NAME = "Jump";
    //射擊
    public const int ACTION_SHOT_ID = 5;
    public const string ACTION_SHOT_NAME = "Shot";
    //射擊空中
    public const int ACTION_SHOT_AIR_ID = 6;
    public const string ACTION_SHOT_AIR_NAME = "Shot_Air";
    //攻擊
    public const int ACTION_NORMAL_ATTACK_ID = 7;
    public const string ACTION_NORMAL_ATTACK_NAME = "NormalAttack";
    //受傷
    public const int ACTION_DAMAGE_ID = 8;
    public const string ACTION_DAMAGE_NAME = "Damage";
    //飛踢
    public const int ACTION_FLYKICK_ID = 9;
    public const string ACTION_FLYKICK_NAME = "FlyKick";
    //上踢
    public const int ACTION_KICKUP_ID = 10;
    public const string ACTION_KICKUP_NAME = "KickUp";
    //空中攻擊
    public const int ACTION_AIRATTACK_ID = 11;
    public const string ACTION_AIRATTACK_NAME = "AirNormalAttack";
    #endregion

    #region 自動查找或設定的元件變數
    [Header("----------------------Rina自動查找或設定的元件變數----------------------")]
    [Tooltip("在這之中的變數不需預先設定，而是自己的物件中必須包含")]
    //人物屬性設定
    public Rina_Data rina_Data;
    //聲音庫子物件
    public GameObject Sounds;
    //現時跑速
    public float RunSpeed = 0f;
    //最後畫面更新時的臉部動作編號
    public int FaceIndex = 1;
    //程式流程中的臉部暫存動作編號
    public int FaceTemp = 0;
    //特效庫
    public EffectPlayer effectPlayer = new EffectPlayer();
    //是否可移動
    public bool canMove = true;
    //是否在主選單
    public bool isInMenu = false;
    //Rina的武器屬性
    public ElementSet Element = ElementSet.Fire;
    #endregion

    

    //角色初始化
    protected void RinaInitialization()
    {
        //得到Data以及相關資料
        rina_Data = GetComponent<Rina_Data>();
        rina_Data.mtionblurVolume = ChildrenFinder.FindByName((ChildrenFinder.FindByTag(Master, "PostProcessingVolumes", 0)), "RunningMotionBlur", 0).GetComponent<Volume>();

        maxHp = rina_Data.MaxHP;
        currentHp = rina_Data.MaxHP;

        //讀取音訊物件
        Sounds = ChildrenFinder.FindByName(Master, "Sounds", 0);

        //讀取特效
        effectPlayer = GameObject.Find("EffectPlayer").GetComponent<EffectPlayer>();

        //設定所有動作
        ActionSets.Clear();
        ActionSets.Add(new Rina_Action_Idle(this, ACTION_IDLE_ID, ACTION_IDLE_NAME));
        ActionSets.Add(new Rina_Action_Jog(this, ACTION_JOG_ID, ACTION_JOG_NAME));
        ActionSets.Add(new Rina_Action_Run(this, ACTION_RUN_ID, ACTION_RUN_NAME));
        ActionSets.Add(new Rina_Action_Jump(this, ACTION_JUMP_ID, ACTION_JUMP_NAME));
        ActionSets.Add(new Rina_Action_Doge(this, ACTION_DOGE_ID, ACTION_DOGE_NAME));
        ActionSets.Add(new Rina_Action_Shot(this, ACTION_SHOT_ID, ACTION_SHOT_NAME));
        ActionSets.Add(new Rina_Action_Shot_Air(this, ACTION_SHOT_AIR_ID, ACTION_SHOT_AIR_NAME));
        ActionSets.Add(new Rina_Action_NormalAttack(this, ACTION_NORMAL_ATTACK_ID, ACTION_NORMAL_ATTACK_NAME));
        ActionSets.Add(new Rina_Action_Damage(this, ACTION_DAMAGE_ID, ACTION_DAMAGE_NAME));
        ActionSets.Add(new Rina_Action_FlyKick(this, ACTION_FLYKICK_ID, ACTION_FLYKICK_NAME));
        ActionSets.Add(new Rina_Action_KickUp(this, ACTION_KICKUP_ID, ACTION_KICKUP_NAME));
        ActionSets.Add(new Rina_Action_AirNormalAttack(this, ACTION_AIRATTACK_ID, ACTION_AIRATTACK_NAME));
    }

    //必須放在程式迴圈一開始執行的程序
    protected void RinaEarlyProcess()
    {
        //更新Input
        InputState.Now.GetButtonStates(InputState.Controller_Mode);

        //更新臉部ID
        FaceTemp = FaceIndex;

        //更新血量
        UpdateHpViewer();

        //Debugs
        //Debug.Log(AnimatorDriver.IsCurrentAnimation(animator,0,"Idle_0"));
    
    }

    //迴圈主體
    protected void RinaMainProcess()
    {
        //執行每套動作
        if ((!isInMenu) && canMove)
        {
            foreach (ActionInterface action in ActionSets)
                action.ProcessAction(actionIndex);
            //切換屬性
            SwitchElement();
        }
        if (hitTimer > 0 && actionIndex != ACTION_DAMAGE_ID)
        {
            //受傷狀態
            JumpInActionByName(ACTION_DAMAGE_NAME);
        }
    }

    //必須放在程式迴圈最後執行的程序
    protected void RinaLateProcess()
    {
        //執行每個動作的離開函式
        foreach (ActionInterface action in ActionSets)
            action.LeaveAction(actionIndex, actionNext);

        //執行每個動作的進入函式
        foreach (ActionInterface action in ActionSets)
            action.EnterAction(actionIndex, actionNext);

        //檢查死亡
        if (currentHp <= 0&&!isDie)
        {
            animator.SetBool("IsDie", true);
            isDie = true;
            canMove = false;
        }


        //更新InputState.Last為下次迴圈的參考
        InputState.Last.GetButtonStates(InputState.Controller_Mode);
    }

    //更新血量
    override public void UpdateHpViewer()
    {
        if (hpViewer != null)
        {
            maxHp = rina_Data.MaxHP;
            hpViewer.MaxHp = maxHp;
            hpViewer.NowHp = currentHp;
        }
    }

    // 切換屬性
    void SwitchElement()
    {
        // 根據十字軸按的方向切換屬性
        if (InputState.IsKeyDown(InputState.Now.Arrow_Up, InputState.Last.Arrow_Up))
        {
            Element = hpViewer.gameObject.transform.parent.GetComponent<SwitchElement>().SwitchElementType(Element, 1);
        }
        else if (InputState.IsKeyDown(InputState.Now.Arrow_Down, InputState.Last.Arrow_Down))
        {
            Element = hpViewer.gameObject.transform.parent.GetComponent<SwitchElement>().SwitchElementType(Element, 2);
        }
        else if (InputState.IsKeyDown(InputState.Now.Arrow_Left, InputState.Last.Arrow_Left))
        {
            Element = hpViewer.gameObject.transform.parent.GetComponent<SwitchElement>().SwitchElementType(Element, 3);
        }
        else if (InputState.IsKeyDown(InputState.Now.Arrow_Right, InputState.Last.Arrow_Right))
        {
            Element = hpViewer.gameObject.transform.parent.GetComponent<SwitchElement>().SwitchElementType(Element, 4);
        }
    }

    #region CorePrograms
    public override void Start()
    {
        GameCharacterInitialization();
        PlayerCharacterInitialization();
        RinaInitialization();
    }
    public override void FixedUpdate()
    {
        GameCharacterEarlyProcess();
        PlayerCharacterEarlyProcess();
        RinaEarlyProcess();

        GameCharacterMainProcess();
        PlayerCharacterMainProcess();
        RinaMainProcess();

        RinaLateProcess();
        PlayerCharacterLateProcess();
        GameCharacterLateProcess();
    }
    public override void Update()
    {
    }
    public override void LateUpdate()
    {
    }
    #endregion


}


