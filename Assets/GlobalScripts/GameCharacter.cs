using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;
using Unity.Collections;

public class GameCharatcer : MonoBehaviour
{

    #region 自動查找或設定的元件變數
    [Header("----------------------GameCharacter自動查找或設定的元件變數----------------------")]
    [Tooltip("在這之中的變數不需預先設定，而是自己的物件中必須包含")]
    //代表角色本體物件(無論在哪呼叫都應該是這個)
    public GameObject Master;
    //動畫器
    public Animator animator;
    //角色控制器
    public GameCharacterController gameCharacterController;
    //現在的動作編號
    public int actionIndex = 0;
    //下一瞬間的動作編號
    public int actionNext = 0;
    //現在血量
    public float maxHp = 100;//預設
    //現在血量
    public float currentHp = 100;//預設
    //可被攻擊
    public bool canBeHit = true;
    //受傷計時器
    public float hitTimer = 0;
    //是否死亡
    public bool isDie = false;
    //動作集合
    public List<ActionInterface> ActionSets = new List<ActionInterface>();
    //受到的傷害狀態
    public AttackHitType beHittingType = AttackHitType.None;
    #endregion

    #region 可設定的變數
    [Header("----------------------GameCharacter需設定的變數----------------------")]
    [Tooltip("在這之中的變數通常需要先指定物件或設定好")]
    //綁定的HP顯示View(必須從外部實現)
    [Header("HP顯示器")]
    public HpUpdater hpViewer;
    #endregion

    #region 屬性
    //是否可見(請使用屬性設定)
    private bool _visible = true;
    public bool Visible
    {
        set
        {
            SetVisible(value);
        }
        get
        {
            return _visible;
        }
    }

    //
    public bool IsGrounded
    {
        get
        {
            return gameCharacterController.IsGrounded;
        }
        set
        {
            gameCharacterController.IsGrounded = true;
        }
    }
    //中心位置
    public Vector3 CenterPosition
    {
        get
        {
            return gameCharacterController.CenterPosition;
        }
    }

    #endregion

    #region 副程式群

    //受傷函式(通常需要Override!!)
    virtual public void BeHit(AttackObject attackObject)
    {
        currentHp -= attackObject.Power;
        GameObject TempObject = new GameObject();
        TempObject.transform.position = Master.transform.position;
        TempObject.transform.rotation = attackObject.transform.rotation;
        TempObject.transform.rotation = Quaternion.Euler(TempObject.transform.rotation.eulerAngles.x, TempObject.transform.rotation.eulerAngles.y + 180, TempObject.transform.rotation.eulerAngles.z);
        gameCharacterController.moveSpeed += TempObject.transform.forward * -1 * attackObject.FlyPower_Horizontal;
        if (gameCharacterController.moveSpeed.y < 0)
        {
            gameCharacterController.moveSpeed.y = 0;
        }
            gameCharacterController.moveSpeed.y += attackObject.FlyPower_Vertical;
        GameEnvironment.entity.HitdelayCount = attackObject.HitDelayCount;
        if (!(beHittingType == AttackHitType.HitFly))
        {
            beHittingType = attackObject.attackHitType;
        }
        if (attackObject.attackHitType == AttackHitType.HitFly)
            hitTimer = attackObject.HitContinueTime;
        else
            hitTimer += attackObject.HitContinueTime;
        gameCharacterController.gravityVelocity = 0;
        Destroy(TempObject);
    }

    //更新血量Viewer
    virtual public void UpdateHpViewer()
    {
        if (hpViewer != null)
        {
            hpViewer.MaxHp = maxHp;
            hpViewer.NowHp = currentHp;
        }
    }

    //設定是否可見
    private void SetVisible(bool boolIn)
    {
        if (boolIn && !_visible)
        {
            _visible = true;
            //Debug.Log("dsds");
            for (int i = 0; i < Master.transform.childCount; i++)
            {
                if (Master.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>() != null)
                    Master.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>().enabled = true;
            }
        }
        else if (!boolIn && _visible)
        {
            _visible = false;
            for (int i = 0; i < Master.transform.childCount; i++)
            {
                if (Master.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>() != null)
                    Master.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>().enabled = false;
            }
        }
    }


    //轉換動作機
    public void JumpInActionByName(string nameIn)
    {
        ActionFinder.GetActionByName(ActionSets, nameIn).JumpIn();
    }
    //轉換動作機
    public void JumpInActionByID(int ID)
    {
        ActionFinder.GetActionByID(ActionSets, ID).JumpIn();
    }

    #endregion

    #region 核心程式
    //初始化程式
    virtual protected void GameCharacterInitialization()
    {
        Master = this.transform.gameObject;

        gameCharacterController = Master.GetComponent<GameCharacterController>();

        animator = Master.GetComponent<Animator>();
        
    }

    //必定在Update開頭先要執行的程式
    virtual protected void GameCharacterEarlyProcess()
    {
        //如果要依照腳色情況覆寫請在角色自己的函式重新覆蓋
        UpdateHpViewer();
    }

    //程式主體
    virtual protected void GameCharacterMainProcess()
    {
        // Debug.Log(characterController.isGrounded);
    }

    //必定在Update結尾先要執行的程式
    virtual protected void GameCharacterLateProcess()
    {
        //可選
        /*
        if (hitTimer >= 0)
        {
            //受傷狀態
            JumpInActionByName("Damage");
        }
        */

        //更新死亡狀態
        isDie = currentHp <= 0;

        //更新動作Trigger
        if (actionNext != actionIndex)
            animator.SetInteger("ActionTrigger", actionNext);
        //更新動作Index
        actionIndex = actionNext;

        //最終移動目標
        gameCharacterController.Move(GameEnvironment.entity.Gravity, GameEnvironment.entity.FixSpeed);
    }
    #endregion
}
