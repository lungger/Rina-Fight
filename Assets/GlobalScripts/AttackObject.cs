using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Assets;

abstract public class AttackObject : MonoBehaviour
{
    //攻擊物件本體
    [Header("本體")]
    public GameObject Master;

    //鋼體
    [Header("鋼體")]
    public Rigidbody attackRigidbody;

    //碰撞範圍
    [Header("碰撞範圍")]
    public Collider attackCollider;

    //攻擊追蹤目標
    [Header("攻擊追蹤目標--可有可無")]
    public GameCharatcer trackTarget;

    //攻擊目標種類--Tag
    [Header("攻擊目標Tag")]
    public List<string> attackTags;

    //傷害
    public float Power = 0;

    //擊中延遲幀數
    public int HitDelayCount = 0;

    //擊中持續時間
    public float HitContinueTime = 0;

    //擊飛力量_水平
    public float FlyPower_Horizontal = 0;
    //擊飛力量_垂直
    public float FlyPower_Vertical = 0;

    //硬質狀態
    public AttackHitType attackHitType;


    //擊中後函式
    public abstract void HitTarget(GameCharatcer target);
    //擊中處發函式
    public abstract void OnTriggerEnter(Collider targetCollider);
    //函式迴圈
    public abstract void FixedUpdate();
    //初始化函式
    virtual public void PresetAttack()
    {
    }

    //是否為合法攻擊目標
    protected bool IsTargetType(string tag)
    {
        foreach (string targetTag in attackTags)
        {
            if (tag == targetTag)
                return true;
        }
        return false;
    }
}
