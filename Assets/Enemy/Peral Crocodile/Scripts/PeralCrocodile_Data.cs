using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeralCrocodile_Data : MonoBehaviour
{
    [Header("待機時間")]
    public float Idle_Time;

    [Header("最大HP")]
    public float MaxHp;

    [Header("走路速度")]
    public float WalkSpeed;
    [Header("走路時間")]
    public float WalkTime;
    [Header("走完路轉向Player的時間")]
    public float SeePlayerTimeOnWalk;

    [Header("距離Player多近時開始跑步")]
    public float MaxRunDistance;
    [Header("跑多久")]
    public float RunTime;
    [Header("跑步速度")]
    public float RunSpeed;
    [Header("跑完步轉向Player的時間")]
    public float SeePlayerTimeOnRun;

    [Header("進入攻擊動畫後，幾秒後開始攻擊")]
    public float ShootDelay;

    [Header("珍珠")]
    public GameObject Peral;
}
