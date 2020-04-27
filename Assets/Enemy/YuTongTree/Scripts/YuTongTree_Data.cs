using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets;

public  class YuTongTree_Data : MonoBehaviour
{

    [Header("血量最大值")]
    public float MaxHP = 100f;

    [Header("移動速度")]
    public float MaxMoveSpeed = 20f;
    [Header("幫助時的速度")]
    public float MaxHelpSpeed = 30f;

    [Header("援助友軍保持的距離")]
    public float HelpDistance = 3f;

    [Header("攻擊範圍")]
    public float AttackDistance = 6f;

    [Header("攻擊冷卻時間")]
    public float AttackCoolDownTime = 2f;
    [Header("閒置時間")]
    public float idleTime = 6f;
    [Header("行走時間")]
    public float walkTime = 4f;

    [Header("攻擊腳本")]
    public YuTongTree_Attack attackScripts;
}
