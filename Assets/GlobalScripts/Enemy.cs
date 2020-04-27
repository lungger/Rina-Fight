using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets;

abstract public class Enemy : GameCharatcer
{
    //攻擊目標(可以隨設定)
    public GameCharatcer AttackTarget;
    //2D顯示小圖片(需要在外面設定)
    public Sprite iconSprite;


    public abstract void Start();
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void LateUpdate();

    //初始化程式
    virtual protected void EnemyInitialization()
    {
       
    }
    //必定在Update開頭先要執行的程式
    virtual protected void EnemyEarlyProcess()
    {
       
    }
    //程式主體
    virtual protected void EnemyMainProcess()
    {

    }
    //必定在Update結尾先要執行的程式
    virtual protected void EnemyLateProcess()
    {

    }

}
