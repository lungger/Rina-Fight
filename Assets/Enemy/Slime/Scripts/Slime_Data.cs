using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets;

public  class Slime_Data : MonoBehaviour
{

    [Header("血量最大值")]
    public float MaxHP = 100f;

    [Header("追擊速度")]
    public float MaxTraceSpeed = 25f;
    [Header("追擊時跳躍速度")]
    public float MaxJumpSpeed = 600f;
    [Header("開始追擊距離")]
    public float TraceDistance = 10f;

    [Header("攻擊移動速度")]
    public float MaxAttackSpeed = 50f;
    [Header("攻擊範圍")]
    public float AttackDistance = 1.5f;

    [Header("攻擊冷卻時間")]
    public float AttackCoolDownTime = 4f;
    [Header("攻擊延遲時間")]
    public float AttackDelayTime = 1.5f;

    public GameObject SlimeNormalAttack;
}
