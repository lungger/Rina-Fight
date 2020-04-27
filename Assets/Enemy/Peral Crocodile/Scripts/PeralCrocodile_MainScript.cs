using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeralCrocodile_MainScript : Enemy
{
    //待機
    public const int ACTION_IDLE_ID = 0;
    public const string ACTION_IDLE_NAME = "Crocodile_Idle";

    //走路
    public const int ACTION_WALK_ID = 1;
    public const string ACTION_WALK_NAME = "Crocodile_walk";

    //跑步
    public const int ACTION_RUN_ID = 2;
    public const string ACTION_RUN_NAME = "Crocodile_Run";

    //攻擊
    public const int ACTION_ATTACK_ID = 3;
    public const string ACTION_ATTACK_NAME = "Crocodile_Attack";

    //受傷
    public const int ACTION_DAMAGE_ID = 4;
    public const string ACTION_DAMAGE_NAME = "PeralCrocodile|Damage";

    //死亡
    public const int ACTION_DIE_ID = 5;
    public const string ACTION_DIE_NAME = "PeralCrocodile|Die";

    //霸體
    public bool isUnstoppable = false;

    public EffectPlayer effectPlayer = new EffectPlayer();

    //是否可移動
    public bool canMove = true;
    //是否在主選單
    public bool isInMenu = false;
    //珍珠
    public GameObject peral;

    public PeralCrocodile_Data PeralCrocodileData;

    protected void PeralCrocodileInitialization()
    {
        PeralCrocodileData = GetComponent<PeralCrocodile_Data>();

        maxHp = PeralCrocodileData.MaxHp;
        currentHp = maxHp;

        ActionSets.Clear();
        ActionSets.Add(new PeralCrocodile_Action_Idle(this, ACTION_IDLE_ID, ACTION_IDLE_NAME));
        ActionSets.Add(new PeralCrocodile_Action_Walk(this, ACTION_WALK_ID, ACTION_WALK_NAME));
        ActionSets.Add(new PeralCrocodile_Action_Run(this, ACTION_RUN_ID, ACTION_RUN_NAME));
        ActionSets.Add(new PeralCrocodile_Action_Attack(this, ACTION_ATTACK_ID, ACTION_ATTACK_NAME));
        ActionSets.Add(new PeralCrocodile_Action_Damage(this, ACTION_DAMAGE_ID, ACTION_DAMAGE_NAME));
        ActionSets.Add(new PeralCrocodile_Action_Die(this, ACTION_DIE_ID, ACTION_DIE_NAME));

        try
        {
            AttackTarget = FindTarget().GetComponent<GameCharatcer>();
        }
        catch
        {
            Debug.LogError("Peral Crocodile: This Player Are Not GameCharacter??");
        }
    }

    protected void PeralCrocodileMainProcess()
    {
        try
        {
            AttackTarget = FindTarget().GetComponent<GameCharatcer>();
        }
        catch
        {
            Debug.LogError("Peral Crocodile: This Player Are Not GameCharacter??");
        }

        if (gameObject && currentHp <= 0)
        {
            JumpInActionByName(ACTION_DIE_NAME);
        }

        if (isDie)
        {
            JumpInActionByName(ACTION_DIE_NAME);
            //Destroy(gameObject, 0.25f);
        }

        if ((!isInMenu) && canMove)
        {
            foreach (ActionInterface action in ActionSets)
                action.ProcessAction(actionIndex);
        }

        if (!isDie&&hitTimer > 0 && actionIndex != ACTION_DAMAGE_ID)
        {
            //受傷狀態
            JumpInActionByName(ACTION_DAMAGE_NAME);
        }

    }

    protected void PeralCrocodileLateProcess()
    {
        //執行每個動作的離開函式
        foreach (ActionInterface action in ActionSets)
            action.LeaveAction(actionIndex, actionNext);

        //執行每個動作的進入函式
        foreach (ActionInterface action in ActionSets)
            action.EnterAction(actionIndex, actionNext);
    }

    override public void BeHit(AttackObject attackObject)
    {
        base.BeHit(attackObject);
        currentHp -= attackObject.Power;
        GameObject TempObject = new GameObject();
        TempObject.transform.position = Master.transform.position;
        TempObject.transform.rotation = attackObject.transform.rotation;
        TempObject.transform.rotation = Quaternion.Euler(TempObject.transform.rotation.eulerAngles.x, TempObject.transform.rotation.eulerAngles.y + 180, TempObject.transform.rotation.eulerAngles.z);
        Destroy(TempObject);
    }

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

    #region CorePrograms
    public override void Start()
    {
        GameCharacterInitialization();
    }
    public override void Update()
    {
    }
    public override void FixedUpdate()
    {
        //currentHp -= 5 * Time.deltaTime;
        GameCharacterEarlyProcess();

        GameCharacterMainProcess();
        PeralCrocodileMainProcess();

        PeralCrocodileLateProcess();
        GameCharacterLateProcess();

    }
    public override void LateUpdate()
    {
        // Only dog use LateUpdate, Wolf Wolf
    }
    #endregion

    #region Override vitual
    // Initialize FireMouse
    protected override void GameCharacterInitialization()
    {
        base.GameCharacterInitialization();
        PeralCrocodileInitialization();
    }
    #endregion
}
