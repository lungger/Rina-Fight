using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObject_Peral : AttackObject
{
    //自然消失最大時間
    public float maxLifeTime = 3.0f;
    //自然消失計時器
    public float liveTime = 0;
    //擊中後時間
    public float hittedTimer = 0;
    //擊中消失延遲
    public float hittedDeleteTime = 0.1f;

    //開始位置
    public Vector3 startPosition;

    //擊中位置
    public Vector3 hitPosition;

    //擊中音效
    public AudioSource hitSound;

    //特效播放器
    public EffectPlayer effectPlayer;

    //音效群
    public GameObject Sounds;

    //以擊中清單
    public List<GameCharatcer> hittedTargets = new List<GameCharatcer>();

    //飛行速度
    public float flySpeed = 3.0f;

    //擊中後函式(擊中東西，但未必有效)
    public override void HitTarget(GameCharatcer target)
    {
        //hitSound.PlayOneShot(hitSound.clip);
        if (target.canBeHit)
            target.BeHit(this);
        EffectLibrary.Effect hitEffect = new EffectLibrary.RinaNormalAttackHit();
        effectPlayer.PlayEffect(ref hitEffect, null, hitPosition, new Vector3(0, 0, 0), 1.0f, 1.0f);
    }

    //擊中觸發函式
    public override void OnTriggerEnter(Collider targetCollider)
    {
        if (!(IsTargetType(targetCollider.tag)))
            return;

        GameCharatcer target = targetCollider.gameObject.GetComponent<GameCharatcer>();
        if (hittedTargets.Contains(target))
            return;

        hitPosition = targetCollider.ClosestPoint(this.gameObject.transform.position);
        HitTarget(target);
        hittedTargets.Add(target);
    }

    //函式迴圈
    public override void FixedUpdate()
    {
        if (hittedTargets.Count == 0)
        {
            Master.transform.position += Master.transform.forward * flySpeed * Time.deltaTime;
            liveTime += Time.deltaTime;
            if (liveTime >= maxLifeTime)
                Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Start()
    {
        //得到數值
        Master = this.gameObject;
        this.attackRigidbody = Master.GetComponent<Rigidbody>();
        this.attackCollider = Master.GetComponent<Collider>();
        Master.transform.position = startPosition;
        //優化技巧，預讀
        EffectLibrary.Effect hitEffect = new EffectLibrary.RinaNormalAttackHit();
    }

    //初始化函式(無鎖定)
    public void PresetAttack(GameCharatcer user, Vector3 startPosition, Vector3 target, List<string> attackTags)
    {
        //得到數值
        Master = this.gameObject;
        this.attackRigidbody = Master.GetComponent<Rigidbody>();
        this.attackCollider = Master.GetComponent<Collider>();
        this.attackTags = attackTags;
        Master.transform.position = startPosition;
        Master.transform.LookAt(target);
        this.startPosition = startPosition;

        //重置旗標變數們
        liveTime = 0;
        hittedTimer = 0;
        hitPosition = new Vector3(0, 0, 0);
        //hitSound = SoundFinder.FindAudioSourceByName(Sounds, "Rina_NormalAttack_HitSound_0");
        effectPlayer = GameObject.Find("EffectPlayer").GetComponent<EffectPlayer>();
        hittedTargets.Clear();


        //創建物件
        this.gameObject.SetActive(true);
    }
}
