using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class Attack_RinaShot_0_Script : AttackObject
{

    public float MaxLifeTime = 0.1f;
    public float liveTime = 0;
    public Vector3 startPosition;
    public int HitTrigger = 0;
    public List<GameCharatcer> Hits;
    public AudioSource HitSound;
    public GameObject Sounds;
    public bool Hited = false;
    public float DestoryTime = 0;
    public float DestoryMaxTime = 1f;


    //擊中後函式(擊中東西，但未必有效)
    public override void HitTarget(GameCharatcer target)
    {
        if (target.canBeHit)
        {
            target.BeHit(this);
        }
        
    }

    //擊中觸發函式
    public override void OnTriggerEnter(Collider targetCollider)
    {
        if (!(IsTargetType(targetCollider.tag)))
            return;

        GameCharatcer target = targetCollider.gameObject.GetComponent<GameCharatcer>();
        Hits.Add(target);
    }

    //整到真正擊中的目標
    private GameCharatcer TrueHitTarget()
    {
        GameCharatcer returner = null;
        float min = 100000;
        foreach (GameCharatcer hitGameCharatcer in Hits)
        {
            float dis = ControllDriver.DistenceOf(hitGameCharatcer.transform.position, startPosition);
            if (dis < min)
            {
                min = dis;
                returner = hitGameCharatcer;
            }
        }
        return returner;
    }

    //函式迴圈
    public override void FixedUpdate()
    {
        liveTime += Time.deltaTime;
        if (liveTime >= MaxLifeTime)
        {
            Destroy(this.gameObject);
        }
        //擊中後兩次迴圈計算傷害
        if (HitTrigger > 2 && !Hited)
        {
            GameCharatcer beHiter = TrueHitTarget();
            if (beHiter != null)
            {
                HitTarget(beHiter);
            }
            Hited = true;
            HitSound.PlayOneShot(HitSound.clip);
        }
        if (Hits.Count > 0)
            HitTrigger++;
        if (Hited)
        {
            DestoryTime += Time.deltaTime;
            if (DestoryTime > DestoryMaxTime)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void Start()
    {
        Master = this.gameObject;
    }


    //初始化函式
    public void PresetAttack(GameCharatcer user, Vector3 startPosition, GameCharatcer target, List<string> attackTags)
    {
        Master = this.gameObject;
        Sounds = ChildrenFinder.FindByName(Master, "Sounds", 0);
        this.attackRigidbody = Master.GetComponent<Rigidbody>();
        this.attackCollider = Master.GetComponent<Collider>();
        this.trackTarget = target;
        this.attackTags = attackTags;
        liveTime = 0;
        Master.transform.position = startPosition;
        Master.transform.LookAt(target.transform);
        this.startPosition = startPosition;
        Hits.Clear();
        HitTrigger = 0;
        Hited = false;
        DestoryTime = 0;
        HitSound = SoundFinder.FindAudioSourceByName(Sounds, "Attack_RinaShot_0_HitSound");
        HitSound.enabled = true;
        this.gameObject.SetActive(true);
    }

    public void PresetAttack(GameCharatcer user, Vector3 startPosition, Vector3 target, List<string> attackTags)
    {
        Master = this.gameObject;
        Sounds = ChildrenFinder.FindByName(Master,"Sounds",0);
        this.attackRigidbody = Master.GetComponent<Rigidbody>();
        this.attackCollider = Master.GetComponent<Collider>();
        this.attackTags = attackTags;
        liveTime = 0;
        Master.transform.position = startPosition;
        Master.transform.LookAt(target);
        this.startPosition = startPosition;
        Hits.Clear();
        HitTrigger = 0;
        Hited = false;
        DestoryTime = 0;
        HitSound = SoundFinder.FindAudioSourceByName(Sounds, "Attack_RinaShot_0_HitSound");
        HitSound.enabled = true;
        this.gameObject.SetActive(true);
    }
}
