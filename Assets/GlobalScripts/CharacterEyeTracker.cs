using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;
using VRM;
using System;

public class CharacterEyeTracker : MonoBehaviour
{
    [SerializeField]
    [Header("本校正的上下左右全部都是面向角色的上下左右")]
    public string m_comment;

    [Header("啟用")]
    [SerializeField]
    public bool Enable;

    [Header("物件設定")]
    [SerializeField]
    public GameObject Master;
    public Transform Target;
    public Transform Head;
    public Transform Eye;

    [Header("VRM模型定位點")]
    [SerializeField]
    public Vector3 VRM_Top;
    public Vector3 VRM_Bottom;
    public Vector3 VRM_Left;
    public Vector3 VRM_Right;

    [Header("FBX模型定位點")]
    [SerializeField]
    public Vector3 FBX_Top;
    public Vector3 FBX_Bottom;
    public Vector3 FBX_Left;
    public Vector3 FBX_Right;

    [Header("邊界範圍(面向)")]
    [SerializeField]
    public float Top_Max;
    public float Bottom_Max;
    public float Left_Max;
    public float Right_Max;


    [Header("Expert(高級設置盡量不要更動)")]
    [SerializeField]
    public float Amp = 25;
    public float Boost = 20;

    //變動量
    Vector3 Delta_Top;
    Vector3 Delta_Bottom;
    Vector3 Delta_Right;
    Vector3 Delta_Left;

    public void UpdateEyePosition(Vector3 Targetposition)
    {
        GameObject TrueRotationItem = new GameObject();
        TrueRotationItem.name = "EyeTracker";
        TrueRotationItem.transform.parent = transform;

        //先得到變動轉量
        Eye.transform.localRotation = Quaternion.Euler(0, 0, 0);
        TrueRotationItem.transform.parent = Master.transform;
        TrueRotationItem.transform.position = Head.position;
        TrueRotationItem.transform.Translate(0, 0.04f, 0);
        //float XFixer = TrueRotationItem.transform.localRotation.eulerAngles.x;
        TrueRotationItem.transform.LookAt(Targetposition);

        float Reset_X, Reset_Y, Reset_Z;
        Reset_X = TrueRotationItem.transform.localRotation.eulerAngles.x;
        ControllDriver.RefreshAngles(ref Reset_X);
        Reset_X = FunctionDriver.GetRefreshAngles(Reset_X);
        Reset_Y = TrueRotationItem.transform.localRotation.eulerAngles.y;
        ControllDriver.RefreshAngles(ref Reset_Y);
        Reset_Y = FunctionDriver.GetRefreshAngles(Reset_Y);
        Reset_Z = FunctionDriver.GetRefreshAngles(TrueRotationItem.transform.localRotation.eulerAngles.z);

        TrueRotationItem.transform.localRotation = Quaternion.Euler(Reset_X, Reset_Y, Reset_Z);


        float step = 0;
        //計算每步偏移值
        //左
        step = Math.Abs(VRM_Left.y);
        Delta_Left = new Vector3(FBX_Left.x / step, FBX_Left.y / step, FBX_Left.z / step);
        //右
        step = Math.Abs(VRM_Right.y);
        Delta_Right = new Vector3(FBX_Right.x / step, FBX_Right.y / step, FBX_Right.z / step);
        //上
        step = Math.Abs(VRM_Top.x);
        Delta_Top = new Vector3(FBX_Top.x / step, FBX_Top.y / step, FBX_Top.z / step);
        //下
        step = Math.Abs(VRM_Bottom.x);
        Delta_Bottom = new Vector3(FBX_Bottom.x / step, FBX_Bottom.y / step, FBX_Bottom.z / step);

        if (Reset_X > 0)
        {
            step = Reset_X;
            if (step > Bottom_Max / Amp)
                step = Bottom_Max / Amp;
            MoveEye(Delta_Bottom, step);
        }
        else if (Reset_X < 0)
        {
            step = Reset_X;
            if (step < Top_Max / Amp)
                step = Top_Max / Amp;
            MoveEye(Delta_Top, step);
        }

        if (Reset_Y > 0)
        {
            step = Reset_Y;
            if (step > Left_Max / Amp)
                step = Left_Max / Amp;
            MoveEye(Delta_Left, step);
        }
        else if (Reset_Y < 0)
        {
            step = Reset_Y;
            if (step < Right_Max / Amp)
                step = Right_Max / Amp;
            MoveEye(Delta_Right, step);
        }
        Destroy(TrueRotationItem);
    }

    public void ResetEyePosition()
    {
        Eye.localRotation = new Quaternion(0, 0, 0, 0);
    }


    void MoveEye(Vector3 Delta, float Variation)
    {
        Variation = Math.Abs(Variation * Boost);
        Delta = new Vector3(Delta.x * Variation, Delta.y * Variation, Delta.z * Variation);
        Quaternion ResultVector = Quaternion.Euler(Eye.localRotation.eulerAngles.x + Delta.x, Eye.localRotation.eulerAngles.y + Delta.y, Eye.localRotation.eulerAngles.z + Delta.z);
        Eye.localRotation = ResultVector;
    }

    // Start is called before the first frame update
    void Start()
    {
    }
    //確保數字不得為0

    // Update is called once per frame
    void Update()
    {
        if (Enable && Target != null)//啟用視線持續追蹤
        {
            UpdateEyePosition(Target.transform.position);
        }
    }
}
