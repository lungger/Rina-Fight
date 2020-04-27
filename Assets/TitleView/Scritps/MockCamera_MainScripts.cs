using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets;

public class MockCamera_MainScripts : PlayableCharacter
{

    #region CorePrograms
    public override void Start()
    {
        GameCharacterInitialization();
        PlayerCharacterInitialization();
    }
    public override void FixedUpdate()
    {
        GameCharacterEarlyProcess();
        PlayerCharacterEarlyProcess();

        GameCharacterMainProcess();
        //PlayerCharacterMainProcess();

        PlayerCharacterLateProcess();
        GameCharacterLateProcess();
    }
    public override void Update()
    {
        
    }
    public override void LateUpdate()
    {
    }
    #endregion


}


