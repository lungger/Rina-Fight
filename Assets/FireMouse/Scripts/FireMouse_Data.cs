using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets;

public  class FireMouse_Data : MonoBehaviour
{
    [Header("跑步速度")]
    public float RunningSpeed = 100f;
    [Header("風火輪集氣時間")]
    public float RollAttackReadyTime = 1.5f;
    [Header("風火輪發動最遠距離")]
    public float MaxRollAttackDistance = 20f;
    [Header("風火輪發動最短距離")]
    public float MinRollAttackDistance = 8f;
    [Header("風火輪多滾多少距離(滾到目標點後繼續滾)")]
    public float RollAttackExtraDistance = 5f;
    [Header("風火輪車速")]
    public float RollAttackSpeed = 300f;
    [Header("自爆集氣時間")]
    public float DieExplosionReadyTime = 3.0f;
    [Header("被擊中後多久開始逃跑")]
    public float RunDelay = 0.25f;

    public GameObject Attack_FireMouseRollAttackPrefab;
    public GameObject Attack_FireMouseDieExplosionPrefab;
}
