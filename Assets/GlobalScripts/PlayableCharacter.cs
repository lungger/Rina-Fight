using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

abstract public class PlayableCharacter : GameCharatcer
{
    #region 常數群
    //相機的最高與最低角度
    const float CAMERA_MIN_X = -45;
    const float CAMERA_MAX_X = 65;
    #endregion
    #region 自動查找或設定的元件變數
    [Header("----------------------PlayableCharacter自動查找或設定的元件變數----------------------")]
    [Tooltip("在這之中的變數不需預先設定，而是自己的物件中必須包含")]
    //角色輸入控制
    public Input_Manager InputState;
    //搖桿方向狀態
    [Header("搖桿方向狀態 -1=Null 0=前 1=右 2=後 3=左")]
    public DirectState StickDirectionState = DirectState.Null;
    //敵人清單
    public List<Enemy> enemyList = new List<Enemy>();
    //角色追蹤目標
    [Header("鎖定目標")]
    public GameCharatcer lockTarget;
    //視角模式
    [Header("視角模式")]
    public LockMode cameraMode;

    //角色追蹤目標清單(敵人清單)
    [Header("鎖定目標的清單")]
    public List<GameCharatcer> lockList;
    #endregion
    #region 可設定的變數
    [Header("----------------------PlayableCharacter需設定的變數----------------------")]
    [Tooltip("在這之中的變數通常需要先指定物件或設定好")]

    //角色相機或標的物
    [Header("角色相機或標的物")]
    public GameObject lookReference;

    //準星物件(需要從外面設定)
    [Header("準星的物件")]
    public GameObject lockViewer;

    //角色相機鏡頭旋轉速度
    [Header("鏡頭旋轉速度")]
    public float cameraLerpSpeed = 100f;

    //角色頭的Transform
    [Header("頭部位置")]
    public Transform Head;

    //角色左手的Transform
    [Header("左手位置")]
    public Transform LeftHand;

    //角色右手的Transform
    [Header("右手位置")]
    public Transform RightHand;

    // 敵人列表控制器 (功能: 鎖定 / 解除鎖定 / 切換左右敵人, 同時控制敵人列表UI)
    public EnemyListController enemyListController;
    #endregion

    //角色現在方向(Y轉軸)
    public float Direct
    {
        set
        {
            gameCharacterController.transform.rotation = Quaternion.Euler(gameCharacterController.transform.rotation.eulerAngles.x, value, gameCharacterController.transform.rotation.eulerAngles.z);
        }
        get
        {
            return gameCharacterController.transform.rotation.eulerAngles.y;
        }
    }

    //角色現在方向(Y轉軸)
    public float DirectWithCamera
    {
        get
        {
            return lookReference.transform.rotation.y - gameCharacterController.transform.rotation.eulerAngles.y;
        }
    }


    public abstract void Start();
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void LateUpdate();

    //初始化程式
    virtual protected void PlayerCharacterInitialization()
    {
        InputState = Master.GetComponent<Input_Manager>();
    }
    //必定在Update開頭先要執行的程式
    virtual protected void PlayerCharacterEarlyProcess()
    {
        InputState.Now.GetButtonStates(InputState.Controller_Mode);
        if (ControllDriver.IsAnyStickPushing_L(InputState))
            StickDirectionState = GetStickDirectionState(ControllDriver.GetStickAngle_L(InputState));
        else
            StickDirectionState = DirectState.Null;
        animator.SetInteger("StickDirectState", DirectToInt(StickDirectionState));
        animator.SetBool("DashButton", InputState.Now.Button_Dash);
    }
    //程式主體
    virtual protected void PlayerCharacterMainProcess()
    {
        UpdateEnemyList();
        LockTarget();
        SwitchTarget();
        UpdateLockEnemyHp();
    }
    //必定在Update結尾先要執行的程式
    virtual protected void PlayerCharacterLateProcess()
    {
        SetCamera();
    }

    //設定最後鏡頭位置(必須放在FixedUpdate)
    protected void SetCamera()
    {
        //預先指定視角
        if (cameraMode == LockMode.Free)        //自由視角
        {
            animator.SetBool("LockTarget", false);
        }
        else if (cameraMode == LockMode.Track)        //跟隨視角
        {
            //建立標的物目標位置
            Vector3 TargetPosition;

            //設定視角跟隨
            animator.SetBool("LockTarget", false);

            ////無須準心
            //lockViewer.GetComponent<TargetFollowV2>().LockTarget(null);

            //可供視角旋轉
            if (ControllDriver.IsAnyStickPushing_R(InputState))
                lookReference.transform.rotation = Quaternion.Euler(lookReference.transform.rotation.eulerAngles.x + (-InputState.Now.R_JoyY * cameraLerpSpeed * Time.deltaTime), lookReference.transform.rotation.eulerAngles.y + (InputState.Now.R_JoyX * cameraLerpSpeed * Time.deltaTime), 0);
            TargetPosition = gameCharacterController.transform.position + (1.5f * Master.GetComponent<GameCharacterController>().transform.up) + (-2f * lookReference.transform.transform.forward);
            lookReference.transform.transform.position = Vector3.Lerp(lookReference.GetComponent<Rigidbody>().transform.position, TargetPosition, 20.0f * Time.deltaTime);

            //鎖定角度不能太高
            ControllDriver.HandleXAngle(ref lookReference, CAMERA_MIN_X, CAMERA_MAX_X);
        }
        else if (cameraMode == LockMode.Lock && lockTarget != null)         //鎖定視角
        {
            //建立暫存物件
            GameObject CameraTempObject = new GameObject();
            Vector3 TargetPosition;

            //設定視角鎖定
            animator.SetBool("LockTarget", true);

            ////啟動鎖定
            //lockViewer.GetComponent<TargetFollowV2>().LockTarget(lockTarget);

            /*
            lockTarget.hpViewer = ChildrenFinder.FindByName(lockViewer, "ImageHp", 0).GetComponent<HpUpdater>();
            */

            //標的物的目標位置
            TargetPosition = Master.transform.position + (1.5f * Master.GetComponent<GameCharacterController>().transform.up);

            //標的物與目標的轉換過程
            CameraTempObject.transform.position = TargetPosition;
            CameraTempObject.transform.LookAt(lockTarget.CenterPosition);
            ControllDriver.HandleXAngle(ref CameraTempObject, CAMERA_MIN_X, CAMERA_MAX_X);
            lookReference.transform.rotation = Quaternion.Lerp(lookReference.GetComponent<Rigidbody>().rotation, CameraTempObject.transform.rotation, cameraLerpSpeed * Time.deltaTime * 0.0175f);
            TargetPosition += (-2f * lookReference.transform.transform.forward);
            lookReference.transform.transform.position = Vector3.Lerp(lookReference.GetComponent<Rigidbody>().transform.position, TargetPosition, cameraLerpSpeed * Time.deltaTime * 0.125f);

            //鎖定角度不能太高
            ControllDriver.HandleXAngle(ref lookReference, CAMERA_MIN_X, CAMERA_MAX_X);

            //刪除暫存物件
            Destroy(CameraTempObject);
        }
    }

    //設定搖桿方向狀態
    public DirectState GetStickDirectionState(float StickAngle)
    {
        if ((StickAngle >= 45f && StickAngle <= 135f))
        {
            //後方
            return DirectState.Back;
        }
        else if ((StickAngle >= 135f && StickAngle <= 180f) || (StickAngle <= -135f && StickAngle >= -180f))
        {
            //左方
            return DirectState.Left;
        }
        else if ((StickAngle <= 45f && StickAngle >= 0f) || (StickAngle >= -45f && StickAngle <= -0f))
        {
            //右方
            return DirectState.Right;
        }
        else if ((StickAngle <= -45f && StickAngle >= -135f))
        {
            //前方
            return DirectState.Front;
        }
        else
        {
            return DirectState.Null;
        }
    }

    //搖桿方向狀態轉成Int型態(--1=Null 0=前 1=右 2=後 3=左)
    public int DirectToInt(DirectState directState)
    {
        if (directState == DirectState.Null)
            return -1;
        else if (directState == DirectState.Front)
            return 0;
        else if (directState == DirectState.Right)
            return 1;
        else if (directState == DirectState.Back)
            return 2;
        else if (directState == DirectState.Left)
            return 3;
        else
            return -1;

    }

    //鎖定目標
    void LockTarget()
    {
        if (InputState.IsKeyDown(InputState.Now.Button_Lock, InputState.Last.Button_Lock))
        {
            if (lockTarget == null && enemyList.Count > 0) //未鎖定
            {
                //進行鎖定 鎖定策略: 重新依照距離遠近 抓取敵人清單
                enemyListController.LockTarget();
                cameraMode = LockMode.Lock;
                lockTarget = enemyListController.GetTargetEnemy();

                //啟動鎖定
                lockViewer.GetComponent<TargetFollowV2>().LockTarget(lockTarget);
                lockViewer.GetComponent<TargetFollowV2>().HpView.IsSetHp = true;
                lockTarget.hpViewer = lockViewer.GetComponent<TargetFollowV2>().HpView;
            }
            else
            {
                if (lockTarget != null)
                    lockTarget.hpViewer = null;
                enemyListController.UnLockTarget();
                cameraMode = LockMode.Track;
                lockTarget = null;
                //無須準心
                lockViewer.GetComponent<TargetFollowV2>().LockTarget(null);
            }
        }
    }

    // 切換目標
    void SwitchTarget()
    {
        if (lockTarget != null)
        {
            if (InputState.IsKeyDown(InputState.Now.Button_LockLeft, InputState.Last.Button_LockLeft))
            {
                lockTarget.hpViewer = null;
                enemyListController.SwitchLeftEnemy();
                lockTarget = enemyListController.GetTargetEnemy();
                lockViewer.GetComponent<TargetFollowV2>().LockTarget(lockTarget);
                lockViewer.GetComponent<TargetFollowV2>().HpView.IsSetHp = true;
                lockTarget.hpViewer = lockViewer.GetComponent<TargetFollowV2>().HpView;
            }
            else if (InputState.IsKeyDown(InputState.Now.Button_LockRight, InputState.Last.Button_LockRight))
            {
                lockTarget.hpViewer = null;
                enemyListController.SwitchRightEnemy();
                lockTarget = enemyListController.GetTargetEnemy();
                lockViewer.GetComponent<TargetFollowV2>().LockTarget(lockTarget);
                lockViewer.GetComponent<TargetFollowV2>().HpView.IsSetHp = true;
                lockTarget.hpViewer = lockViewer.GetComponent<TargetFollowV2>().HpView;
            }
        }
    }

    //非鎖定狀態下時時抓
    void UpdateEnemyList()
    {
        if (lockTarget == null) //UnLock
        {
            enemyListController.UpdateEnemyList();
            enemyList = enemyListController._enemyList;
        }
    }

    //刷新鎖定怪物的血量 (將血量給View)
    void UpdateLockEnemyHp()
    {
        if (lockTarget)
        {
            if (lockTarget.isDie) //die
            {
                lockTarget.hpViewer = null;
                lockTarget = enemyListController.OnLockEnemyDie(); //add
                lockViewer.GetComponent<TargetFollowV2>().LockTarget(lockTarget);
                if (lockTarget) //add
                {
                    lockViewer.GetComponent<TargetFollowV2>().HpView.IsSetHp = true;
                    lockTarget.hpViewer = lockViewer.GetComponent<TargetFollowV2>().HpView;
                    cameraMode = LockMode.Lock;
                }
                else
                {
                    cameraMode = LockMode.Track;
                }
            }
            else if (enemyListController.IsExitDistance(lockTarget)) //add if, 離開鎖定範圍時
            {
                lockTarget.hpViewer = null;
                lockTarget = enemyListController.OnLockEnemyExitDistance(); //add
                lockViewer.GetComponent<TargetFollowV2>().LockTarget(lockTarget);
                if (lockTarget) //add
                {
                    lockViewer.GetComponent<TargetFollowV2>().HpView.IsSetHp = true;
                    lockTarget.hpViewer = lockViewer.GetComponent<TargetFollowV2>().HpView;
                    cameraMode = LockMode.Lock;
                }
                else
                {
                    cameraMode = LockMode.Track;
                }
            }

        }

    }

}


