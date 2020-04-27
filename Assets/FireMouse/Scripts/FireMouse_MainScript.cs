using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class FireMouse_MainScript : Enemy
{
    //待機
    public const int ACTION_IDLE_ID = 0;
    public const string ACTION_IDLE_NAME = "Idle";

    //跑步
    public const int ACTION_RUNNING_ID = 1;
    public const string ACTION_RUNNING_NAME = "Running";

    //被攻擊
    public const int ACTION_BEHIT_ID = 2;
    public const string ACTION_BEHIT_NAME = "BeHit";

    //風火輪攻擊
    public const int ACTION_ROLLATTACK_ID = 3;
    public const string ACTION_ROLLATTACK_NAME = "RollAttack";

    //死亡
    public const int ACTION_DIE_ID = 4;
    public const string ACTION_DIE_NAME = "Die";

    //死亡爆炸
    public const int ACTION_DIEEXPLOSION_ID = 5;
    public const string ACTION_DIEEXPLOSION_NAME = "DieExplosion";

    //霸體
    public bool isUnstoppable = false;

    //死透
    public bool isDead = false;

    public EffectPlayer effectPlayer = new EffectPlayer();

    public FireMouse_Data Data;

    //是否可移動
    public bool canMove = true;
    //是否在主選單
    public bool isInMenu = false;

    protected void FireMouseInitialization()
    {
        ActionSets.Clear();
        effectPlayer = GameObject.Find("EffectPlayer").GetComponent<EffectPlayer>();
        ActionSets.Add(new FireMouse_Action_Idle(this, ACTION_IDLE_ID, ACTION_IDLE_NAME));
        ActionSets.Add(new FireMouse_Action_BeHit(this, ACTION_BEHIT_ID, ACTION_BEHIT_NAME));
        ActionSets.Add(new FireMouse_Action_Running(this, ACTION_RUNNING_ID, ACTION_RUNNING_NAME));
        ActionSets.Add(new FireMouse_Action_RollAttack(this, ACTION_ROLLATTACK_ID, ACTION_ROLLATTACK_NAME));
        ActionSets.Add(new FireMouse_Action_Die(this, ACTION_DIE_ID, ACTION_DIE_NAME));
        ActionSets.Add(new FireMouse_Action_DieExplosion(this, ACTION_DIEEXPLOSION_ID, ACTION_DIEEXPLOSION_NAME));
    }

    protected void FireMouseMainProcess()
    {
        try
        {
            AttackTarget = FindTarget().GetComponent<GameCharatcer>();
        }
        catch
        {

        }

        if (gameObject && currentHp <= 0 && !isDead)
        {
            JumpInActionByName("Die");
        }

        if (isDead)
        {
            Destroy(gameObject, 0f);
        }

        if ((!isInMenu) && canMove)
        {
            foreach (ActionInterface action in ActionSets)
                action.ProcessAction(actionIndex);
        }

    }

    protected void FireMouseLateProcess()
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
        currentHp -= attackObject.Power;
        if (currentHp > 0 && !isUnstoppable)
        {
            JumpInActionByName("BeHit");
        }
        GameObject TempObject = new GameObject();
        TempObject.transform.position = Master.transform.position;
        TempObject.transform.rotation = attackObject.transform.rotation;
        TempObject.transform.rotation = Quaternion.Euler(TempObject.transform.rotation.eulerAngles.x, TempObject.transform.rotation.eulerAngles.y + 180f, TempObject.transform.rotation.eulerAngles.z);
        gameCharacterController.moveSpeed += TempObject.transform.forward * -1 * attackObject.FlyPower_Horizontal;
        gameCharacterController.moveSpeed.y += attackObject.FlyPower_Vertical;
        Destroy(TempObject);
    }

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

    //跑步的時候撞到牆就轉方向
    void OnCollisionEnter(Collision collision)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Running") && string.Compare(collision.collider.name, "Terrain") != 0)
        {
            foreach (ActionInterface action in ActionSets)
            {
                if(string.Compare(action.ActionName, "Running") == 0)
                {
                    if(string.Compare(((FireMouse_Action_Running)action).RunMode, "PushToTarget")== 0)
                    {
                        ((FireMouse_Action_Running)action).AdjustRunDirection(Random.Range(-90f, 90f), 5f, 10f);
                    }
                    else
                    {
                        ((FireMouse_Action_Running)action).AdjustRunDirection(Random.Range(90f, 270f), 20f, 25f);
                    }
                    
                }
            }
        }
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
        FireMouseMainProcess();
        FireMouseLateProcess();
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
        FireMouseInitialization();

    }
    #endregion
}
