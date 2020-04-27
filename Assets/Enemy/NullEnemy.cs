using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class NullEnemy : Enemy
{
    //test
    void NullEmenyMainProcess()
    {
        if (gameObject && currentHp <= 0)
        {
            Destroy(gameObject, 0.5f);
        }
    }

    #region CorePrograms
    public override void Start()
    {
        GameCharacterInitialization();
    }
    public override void Update()
    {
    }
    public override void FixedUpdate()
    {
        //currentHp -= 5 * Time.deltaTime;
        GameCharacterEarlyProcess();
        GameCharacterMainProcess();
        NullEmenyMainProcess();
        GameCharacterLateProcess();
    }
    public override void LateUpdate()
    {
        // Only dog use LateUpdate, Wolf Wolf
    }
    #endregion

}
