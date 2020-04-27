using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleView_CameraController : MonoBehaviour
{
    [Header("4選項")]
    public TitleView_OpenOptions options;

    [Header("輸入，需外部設定")]
    [SerializeField]
    Input_Manager InputState;

    //角色相機或標的物
    [Header("角色相機或標的物")]
    public GameObject lookReference;

    //角色相機鏡頭旋轉速度
    [Header("鏡頭旋轉速度")]
    public float cameraLerpSpeed = 100f;

    //搖桿方向狀態
    [Header("搖桿方向狀態 -1=Null 0=前 1=右 2=後 3=左")]
    public DirectState StickDirectionState = DirectState.Null;

    [Header("移動速度")]
    public float moveSpeed = 5.0f;

    #region copy cat
    //角色現在方向(Y轉軸)
    public float Direct
    {
        set
        {
            gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, value, gameObject.transform.rotation.eulerAngles.z);
        }
        get
        {
            return gameObject.transform.rotation.eulerAngles.y;
        }
    }

    //角色現在方向(Y轉軸)
    public float DirectWithCamera
    {
        get
        {
            return lookReference.transform.rotation.y - gameObject.transform.rotation.eulerAngles.y;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (options == null)
            options = GameObject.Find("UICanvas").GetComponent<TitleView_OpenOptions>();
        if (lookReference == null)
            lookReference = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (options.isOpenMenu)
            return;
        Vector3 TargetPosition;

        #region 一棟
        Vector3 dir = Vector3.zero;

        if (InputState.Now.L_JoyX != 0.0f)
        {
            dir += transform.right * InputState.Now.L_JoyX * Time.deltaTime * moveSpeed;
        }
        if (InputState.Now.L_JoyY != 0.0f)
        {
            dir += transform.forward * InputState.Now.L_JoyY * Time.deltaTime * moveSpeed;
        }

        gameObject.transform.position += dir;

        #endregion

        #region 旋轉
        if (ControllDriver.IsAnyStickPushing_R(InputState))
            lookReference.transform.rotation = Quaternion.Euler(lookReference.transform.rotation.eulerAngles.x + (-InputState.Now.R_JoyY * cameraLerpSpeed * Time.deltaTime), lookReference.transform.rotation.eulerAngles.y + (InputState.Now.R_JoyX * cameraLerpSpeed * Time.deltaTime), 0);

        TargetPosition = gameObject.transform.position;
        lookReference.transform.transform.position = Vector3.Lerp(lookReference.GetComponent<Rigidbody>().transform.position, TargetPosition, 20.0f * Time.deltaTime);
        #endregion
    }

    #region copy cat
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
    #endregion
}
