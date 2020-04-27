using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets;
public class FireMouse_Action_Running : ActionInterface
{
    FireMouse_MainScript MasterScript;

    public Vector3 OriginPosition;
    public float RunDirection;
    public float FireMouseRotation;
    public Vector3 RunVector;
    public float RunDistance;
    public float RunStartTime;
    public float RunDelay;
    public string RunMode = "RunAway";

    public Vector3 StuckDetectionPosition;
    public float StuckDetectionStartTime;
    public float StuckDetectionFrequency;
    public float StuckDetectionDirection;

    public int ActionID { get; set; }
    public string ActionName { get; set; }

    public FireMouse_Action_Running(GameCharatcer player, int ID, string Name)
    {
        SetState(player, ID, Name);
    }

    //設定動作(必定先初始化)
    public void SetState(GameCharatcer player, int ID, string Name)
    {
        MasterScript = (FireMouse_MainScript)player;
        ActionID = ID;
        ActionName = Name;
    }

    //動作必須要有實體程式
    public void ProcessAction(int currentId)
    {
        //如果不等於此動作則退出
        if (!(currentId == ActionID))
            return;

        if(Time.time - StuckDetectionStartTime >= StuckDetectionFrequency)
        {
            if((MasterScript.transform.position - StuckDetectionPosition).magnitude < MasterScript.Data.RunningSpeed / 10f / 4f)
            {
                if(string.Compare(RunMode, "PushToTarget") == 0)
                {
                    AdjustRunDirection(Random.Range(15f * StuckDetectionDirection, 30f * StuckDetectionDirection), 5f, 10f);
                }
                else
                {
                    AdjustRunDirection(Random.Range(15f * StuckDetectionDirection, 30f * StuckDetectionDirection), 20f, 25f);
                }
            }
            StuckDetectionPosition = MasterScript.transform.position;
            StuckDetectionStartTime = Time.time;
        }

        if (string.Compare(RunMode, "PushToTarget") == 0)
        {
            MasterScript.gameCharacterController.moveVector = RunVector;
        }
        else
        {
            if (Time.time - RunStartTime >= RunDelay)
            {
                MasterScript.gameCharacterController.moveVector = RunVector;
            }
        }

        MasterScript.transform.eulerAngles = Vector3.Lerp(MasterScript.transform.eulerAngles, new Vector3(0f, FireMouseRotation,0f), 0.1f);
        
        //檢查是否切換
        CheckChange(currentId);
    }

    

    //檢查能夠跳到那些動作
    public void CheckChange(int currentId)
    {
        if ((MasterScript.transform.position - OriginPosition).magnitude > RunDistance)
        {
            MasterScript.transform.LookAt(MasterScript.AttackTarget.transform);

            MasterScript.JumpInActionByName("Idle");
        }
    }

    //從其他動作跳到這裡
    public void JumpIn()
    {
        MasterScript.actionNext = ActionID;
    }

    //動作離開時必定執行的程式
    public void LeaveAction(int currentId, int nextId)
    {
        if (!(currentId == ActionID && !(currentId == nextId)))
            return;

        RunMode = "RunAway";
    }

    //進入動作的函式
    public void EnterAction(int currentId, int nextId)
    {
        if (!(nextId == ActionID && !(currentId == nextId)))
            return;
        RunStartTime = Time.time;
        RunDelay = MasterScript.Data.RunDelay;
        StuckDetectionPosition = MasterScript.transform.position;
        StuckDetectionStartTime = Time.time + RunDelay;
        StuckDetectionFrequency = 1.0f;
        float temp = Random.Range(0f, 2f);
        if(temp <= 1f)
        {
            StuckDetectionDirection = 1f;
        }
        else
        {
            StuckDetectionDirection = -1f;
        }
        float DistanceToAttackTarget = (MasterScript.transform.position - MasterScript.AttackTarget.transform.position).magnitude;
        if(string.Compare(RunMode, "PushToTarget") == 0)
        {
            RunSetup(-30f, 30f);
        }
        else
        {
            RunSetup(120f, 240f);
        }
        
    }

    //逃跑時候的方向，180度就是向後跑
    public void RunSetup(float rotateAngleRangeLeft, float rotateAngleRangeRight)
    {
        if(rotateAngleRangeLeft > rotateAngleRangeRight)
        {
            float temp = rotateAngleRangeLeft;
            rotateAngleRangeLeft = rotateAngleRangeRight;
            rotateAngleRangeRight = temp;
        }
        Vector2 toTargetVector = new Vector2(MasterScript.AttackTarget.transform.position.x - MasterScript.transform.position.x, MasterScript.AttackTarget.transform.position.z - MasterScript.transform.position.z);
        float angleToTarget = Vector2.SignedAngle(MasterScript.transform.up, toTargetVector);
        if (angleToTarget < 0f)
        {
            angleToTarget = 360f - Mathf.Abs(angleToTarget);
        }
        RunDirection = Random.Range(360f - angleToTarget + rotateAngleRangeLeft, 360f - angleToTarget + rotateAngleRangeRight);
        RunDirection = ClampAngle(RunDirection, 0f, 360f);
        RunVector = Quaternion.Euler(0f, RunDirection - 90f, 0f) * new Vector3(MasterScript.Data.RunningSpeed, 0f, 0f);
        RunDistance = Random.Range(10f, 15f);
        MasterScript.transform.eulerAngles = new Vector3(0f, ClampAngle(MasterScript.transform.eulerAngles.y, 0f, 360f), 0f);
        FireMouseRotation = RunDirection;
        OriginPosition = MasterScript.transform.position;
    }

    //調整逃跑時候的方向和距離
    public void AdjustRunDirection(float AdjustAngle, float RunDistanceRangeLeft, float RunDistanceRangeRight)
    {
        RunDirection += AdjustAngle;
        RunDirection = ClampAngle(RunDirection, 0f, 360f);
        RunVector = Quaternion.Euler(0f, RunDirection - 90f, 0f) * new Vector3(MasterScript.Data.RunningSpeed, 0f, 0f);
        RunDistance = Random.Range(RunDistanceRangeLeft, RunDistanceRangeRight);
        MasterScript.transform.eulerAngles = new Vector3(0f, ClampAngle(MasterScript.transform.eulerAngles.y, 0f, 360f), 0f);
        FireMouseRotation = ClampAngle(RunDirection, 0f, 360f);
        OriginPosition = MasterScript.transform.position;
    }

    float ClampAngle(float angle, float min, float max)
    {
        while (angle < min || angle > max)
        {
            if (angle > max)
            {
                angle -= 360f;
            }

            if (angle < min)
            {
                angle += 360f;
            }
        }

        return angle;
    }

    
}
