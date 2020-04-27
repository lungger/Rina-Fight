using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets;

public  class Rina_Data : MonoBehaviour
{

    //特效變數
    [Header("動態模糊特效")]
    public Volume mtionblurVolume;
    [Header("跑步速度")]
    public float MaxRunSpeed = 1200f;
    [Header("慢跑速度")]
    public float MaxJogSpeed = 600f;
    [Header("跳躍力道")]
    public float MaxJumpSpeed = 600f;
    [Header("閃躲速度")]
    public float MaxDogeSpeed = 2000f;
    [Header("血量最大值")]
    public float MaxHP = 100f;
    [Header("跑步特效")]
    public ParticleSystem walkTrail;
    [Header("RinaShot")]
    public GameObject rinashotPrefabs;
    [Header("RinaNormalAttack_0")]
    public GameObject rinaNormalAttack_0;
    [Header("RinaNormalAttack_1")]
    public GameObject rinaNormalAttack_1;
    [Header("RinaFlyAttack")]
    public GameObject rinaFlyAttack_0;
    [Header("RinaKickUpAttack")]
    public GameObject rinaKickUpAttack_0;
    [Header("RinaAirAttack_0")]
    public GameObject rinaAirNormalAttack_0;
    [Header("RinaAirAttack_1")]
    public GameObject rinaAirNormalAttack_1;
}
