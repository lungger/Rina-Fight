using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ActionInterface
{
    //動作機順序
    //Start => SetState
    //ProcessAction -> 
    //CheckChange(如果將會改變NextIndex) ->
    //JumpIn(在這裡改變Next) ->
    //LeaveAction(執行離開函式) ->
    //EnterAction(正式切換動作)

    int ActionID { get; set; }
    string ActionName { get; set; }

    //設定動作(有參數)
    void SetState(GameCharatcer player, int ID, string Name);

    //動作必須要有實體程式
    void ProcessAction(int currentId);

    //檢查是否能跳到別的動作
    void CheckChange(int currentId);

    //從其他動作跳到這裡
    void JumpIn();

    //動作離開時必定執行的程式
    void LeaveAction(int currentId, int nextId);

    //進入動作的函式
    void EnterAction(int currentId, int nextId);
}

