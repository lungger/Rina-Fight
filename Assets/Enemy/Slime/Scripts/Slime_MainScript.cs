using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_MainScript : Enemy
{
    #region define const
    // die & explode slime prefab name
    const string SLIME_DIE = "Slime_die";

    // define action
    // 待機
    public const int ACTION_IDLE_ID = 0;
    public const string ACTION_IDLE_NAME = "Idle";
    // 追擊
    public const int ACTION_TRACE_ID = 1;
    public const string ACTION_TRACE_NAME = "Trace";
    // 準備攻擊
    public const int ACTION_PREATTACK_ID = 2;
    public const string ACTION_PREATTACK_NAME = "PreAttack";
    // 準備攻擊
    public const int ACTION_ATTACK_ID = 4;
    public const string ACTION_ATTACK_NAME = "Attack";
    // 受到傷害
    public const int ACTION_DAMAGE_ID = 3;
    public const string ACTION_DAMAGE_NAME = "Damage";
    #endregion

    #region 自動查找或設定的元件變數
    [Header("----------------------薄荷方塊自動查找或設定的元件變數----------------------")]
    [Tooltip("在這之中的變數不需預先設定，而是自己的物件中必須包含")]

    // Sound bank
    public GameObject Sounds;
    // Effect database
    public EffectPlayer effectPlayer = new EffectPlayer();
    // Slime Data
    public Slime_Data slime_data;
    // explosion prefab
    public GameObject exploder;
    // can Slime move
    public bool canMove = true;
    // is in the menu
    public bool isInMenu = false;
    // die delay
    public float dieDelay = 0.1f;
    //
    public float dieTimer = 0;
    // 是否撞到玩家
    public bool isHit = false;
    // 是否被攻擊到
    public bool isDamage = false;
    #endregion


    #region Slime Progress
    // Initialize Slime
    protected void SlimeInitialization()
    {
        slime_data = GetComponent<Slime_Data>();
        maxHp = slime_data.MaxHP;
        currentHp = maxHp;


        // Find and Initialize the exposion prefab after die
        exploder = ChildrenFinder.FindByName(gameObject, SLIME_DIE)[0];
        exploder.GetComponent<Slime_die>().Destroydelay = dieDelay;

        // 將動作加入動作List
        ActionSets.Clear();
        ActionSets.Add(new Slime_Action_Idle(this, ACTION_IDLE_ID, ACTION_IDLE_NAME));
        ActionSets.Add(new Slime_Action_Trace(this, ACTION_TRACE_ID, ACTION_TRACE_NAME));
        ActionSets.Add(new Slime_Action_PreAttack(this, ACTION_PREATTACK_ID, ACTION_PREATTACK_NAME));
        ActionSets.Add(new Slime_Action_Damage(this, ACTION_DAMAGE_ID, ACTION_DAMAGE_NAME));
        ActionSets.Add(new Slime_Action_Attack(this, ACTION_ATTACK_ID, ACTION_ATTACK_NAME));

        animator.SetInteger("ActionTrigger", 0);
    }

    // process at loop begin
    protected void SlimeEarlyProcess()
    {
        if (gameObject && isDie)
        {
            dieTimer += Time.deltaTime;
            if (dieTimer > dieDelay)
                SlimeDieProcess();
            return;
        }
    }


    private void SlimeDieProcess()
    {
        Visible = false;
        // make explosion slime visable
        exploder.SetActive(true);

        // destroy self
        Destroy(gameObject, dieDelay);
    }

    //迴圈主體
    protected void SlimeMainProcess()
    {
        //反覆更新進入範圍的敵人
        try
        {
            AttackTarget = FindTarget().GetComponent<GameCharatcer>();
        }
        catch
        {
            Debug.LogError("Slime: This Player Are Not GameCharacter??");
        }
        //執行每套動作
        if ((!isInMenu) && canMove)
        {
            foreach (ActionInterface action in ActionSets)
                action.ProcessAction(actionIndex);
        }
        if (hitTimer > 0 && actionIndex != ACTION_DAMAGE_ID)
        {
            //受傷狀態
            JumpInActionByName(ACTION_DAMAGE_NAME);
        }
    }

    //必須放在程式迴圈最後執行的程序
    protected void SlimeLateProcess()
    {
        //執行每個動作的離開函式
        foreach (ActionInterface action in ActionSets)
            action.LeaveAction(actionIndex, actionNext);

        //執行每個動作的進入函式
        foreach (ActionInterface action in ActionSets)
            action.EnterAction(actionIndex, actionNext);

    }

    #endregion

    #region Action API
    public float TargetDistance
    {
        get
        {
            if (AttackTarget != null)
                return Vector3.Distance(gameObject.transform.position, AttackTarget.transform.position);
            else
                return 0;
        }
    }
    #endregion

    #region CorePrograms
    public override void Start()
    {
        GameCharacterInitialization();
        SlimeInitialization();
    }
    public override void Update()
    {
    }
    public override void FixedUpdate()
    {
        GameCharacterEarlyProcess();
        SlimeEarlyProcess();

        GameCharacterMainProcess();
        SlimeMainProcess();

        SlimeLateProcess();
        GameCharacterLateProcess();
    }
    public override void LateUpdate()
    {
        // Only dog use LateUpdate, Wolf Wolf
    }
    #endregion


    //尋找目標
    public GameObject FindTarget()
    {
        List<GameObject> allPlayers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        float minDistance = float.MaxValue;
        GameObject FinalTarget = null;
        foreach (GameObject gameObject in allPlayers)
        {
            float dis = Vector3.Distance(gameObject.transform.position, gameCharacterController.CenterPosition);
            if (dis < minDistance)
            {
                FinalTarget = gameObject;
                minDistance = dis;
            }
        }
        return FinalTarget;
    }
    override public void UpdateHpViewer()
    {
        if (hpViewer != null)
        {
            maxHp = slime_data.MaxHP;
            hpViewer.MaxHp = maxHp;
            hpViewer.NowHp = currentHp;
        }
    }

}
