using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuTongTree_MainScript : Enemy
{
    #region define const

    // define action
    // 待機
    public const int ACTION_IDLE_ID = 0;
    public const string ACTION_IDLE_NAME = "Idle";
    // 追擊
    public const int ACTION_BLOCK_ID = 1;
    public const string ACTION_BLOCK_NAME = "Block";
    // 準備攻擊
    public const int ACTION_ATTACK_ID = 2;
    public const string ACTION_ATTACK_NAME = "Attack";
    // 受到傷害
    public const int ACTION_DAMAGE_ID = 3;
    public const string ACTION_DAMAGE_NAME = "Damage";
    // 死去
    public const int ACTION_DIE_ID = 4;
    public const string ACTION_DIE_NAME = "Die";
    #endregion

    #region 自動查找或設定的元件變數
    [Header("----------------------機掰樹自動查找或設定的元件變數----------------------")]
    [Tooltip("在這之中的變數不需預先設定，而是自己的物件中必須包含")]

    // Sound bank
    public GameObject Sounds;
    // Effect database
    public EffectPlayer effectPlayer = new EffectPlayer();
    // Slime Data
    public YuTongTree_Data data;
    // 幫助目標
    public GameCharatcer HelpTarget;
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
    // 是否死透了
    public bool isDead = false;
    #endregion


    #region YuTongTree Progress
    // Initialize Slime
    protected void TreeInitialization()
    {
        data = GetComponent<YuTongTree_Data>();
        maxHp = data.MaxHP;
        data.attackScripts = GetComponent<YuTongTree_Attack>();
        maxHp = data.MaxHP;
        currentHp = maxHp;
        // 將動作加入動作List
        ActionSets.Clear();
        ActionSets.Add(new YuTongTree_Action_Idle(this, ACTION_IDLE_ID, ACTION_IDLE_NAME));
        ActionSets.Add(new YuTongTree_Action_Block(this, ACTION_BLOCK_ID, ACTION_BLOCK_NAME));
        ActionSets.Add(new YuTongTree_Action_Damage(this, ACTION_DAMAGE_ID, ACTION_DAMAGE_NAME));
        ActionSets.Add(new YuTongTree_Action_Attack(this, ACTION_ATTACK_ID, ACTION_ATTACK_NAME));
        ActionSets.Add(new YuTongTree_Action_Die(this, ACTION_DIE_ID, ACTION_DIE_NAME));

        animator.SetInteger("ActionTrigger", 0);
    }

    // process at loop begin
    protected void TreeEarlyProcess()
    {
        if (gameObject && currentHp <= 0)
        {
            JumpInActionByName(ACTION_DIE_NAME);
        }

        if (isDead)
        {
            Destroy(gameObject, 0.25f);
        }
    }

    //迴圈主體
    protected void TreeMainProcess()
    {
        //反覆更新進入範圍的敵人
        try
        {
            AttackTarget = FindTarget("Player").GetComponent<GameCharatcer>();
        }
        catch
        {
            Debug.LogError("YuTongTree: This Player Are Not GameCharacter??");
        }
        // 更新最近的被鎖定敵人
        HelpTarget = FindHelpTarget();

        // 更新在幫助範圍內的友軍
        //HelpTarget = FindTarget("Enemy").GetComponent<GameCharatcer>();

        //執行每套動作
        if ((!isInMenu) && canMove)
        {
            foreach (ActionInterface action in ActionSets)
                action.ProcessAction(actionIndex);
        }

        if (!isDie && hitTimer > 0 && actionIndex != ACTION_DAMAGE_ID)
        {
            //受傷狀態
            JumpInActionByName(ACTION_DAMAGE_NAME);
        }

    }

    //必須放在程式迴圈最後執行的程序
    protected void TreeLateProcess()
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
    // 與玩家的距離
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
    // 與最近友軍的距離
    public float HelpDistance
    {
        get
        {
            if (HelpTarget != null)
                return Vector3.Distance(gameObject.transform.position, HelpTarget.transform.position);
            else
                return -1;
        }
    }

    //尋找目標
    public GameObject FindTarget(string targetTag)
    {
        List<GameObject> allPlayers = new List<GameObject>(GameObject.FindGameObjectsWithTag(targetTag));
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

    // 獲取最近的玩家，其鎖定之怪物
    public GameCharatcer FindHelpTarget()
    {
        GameObject player = FindTarget("Player");
        if (player != null)
        {
            PlayableCharacter character = player.GetComponent<PlayableCharacter>();
            if (character.lockTarget != null && hpViewer == null)
                return character.lockTarget;
            else
                return null;
        }
        else
        {
            Debug.LogError("欸欸欸欸欸");
            return null;
        }
    }

    
    #endregion

    #region CorePrograms
    public override void Start()
    {
        GameCharacterInitialization();
        TreeInitialization();
    }
    public override void Update()
    {
    }
    public override void FixedUpdate()
    {
        GameCharacterEarlyProcess();
        TreeEarlyProcess();

        GameCharacterMainProcess();
        TreeMainProcess();

        TreeLateProcess();
        GameCharacterLateProcess();
    }
    public override void LateUpdate()
    {
        // Only dog use LateUpdate, Wolf Wolf
    }
    #endregion

    override public void UpdateHpViewer()
    {
        if (hpViewer != null)
        {
            maxHp = data.MaxHP;
            hpViewer.MaxHp = maxHp;
            hpViewer.NowHp = currentHp;
        }
    }

}
