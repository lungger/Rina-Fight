using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRM;
using UnityEngine;



//事件廣播器
public static class EventManager
{
    //教學↓
    /*
     * ※此腳本只需在編譯過程被編譯，無須掛載於任何遊戲物件上
     * 使用方式，在其他腳本的Start函式中預先註冊事件，範例↓
     * EventManager.NormalEvents += new EventManager.NormalEventHandler(Myfunction);
     * 可整理成
     * EventManager."廣播事件種類的容器" += new EventManager."廣播事件種類" ("接收到此廣播會執行的函式");
     * 
     * 呼叫方式
     * 在任何一個地方使用:EventManager.entity.CallNormalEvents("廣播名稱");
     * 即可廣播全域事件
     */

    //NormalEvent -- 適用於需要傳遞一個任何資料的時機
    #region NormalEvent
    public delegate void NormalEventHandler(string eventName, object value);
    public static event NormalEventHandler NormalEvents;
    public static void CallNormalEvents(string eventName, object value)
    {
        if (NormalEvents != null)
            NormalEvents.Invoke(eventName, value);
    }
    #endregion NormalEvent

}


//移動擴充函式
public class ControllDriver
{
    //處理IK漸變到0
    public static void SetIKPositionWeightTrunZero(Animator animator, AvatarIKGoal avatarIKGoal, ref float tickTimer, float cycle)
    {

        tickTimer += Time.deltaTime;
        if (tickTimer > cycle)
        {
            tickTimer = 0.0f;
            if (animator.GetIKPositionWeight(avatarIKGoal) > 0)
                animator.SetIKPositionWeight(avatarIKGoal, animator.GetIKPositionWeight(avatarIKGoal) - 0.1f);
            else
                animator.SetIKPositionWeight(avatarIKGoal, 0.0f);
        }
    }

    //處理重力速度
    public static void ProcessGravity(Rigidbody rigidbody)
    {
        rigidbody.velocity += GameEnvironment.entity.Gravity * 0.4f;
    }

    //處理重力速度
    public static void ProcessGravity(ref Vector3 moveVector, ref Vector3 moveSpeed, ref float gravityVelocity, bool IsGrounded, bool useGravity)
    {
        //增加重力
        if (useGravity && IsGrounded == false)
            gravityVelocity += ((Omector3)GameEnvironment.entity.Gravity).Power * 0.25f;
        else
        {
            if (gravityVelocity > 0 && moveSpeed.y > 0)
                moveSpeed.y = 0;
            gravityVelocity = 0;
        }
        if (useGravity)
            moveVector += ((Vector3)GameEnvironment.entity.GravityDirection) * gravityVelocity;
    }

    //內部更改重整角度
    public static void RefreshAngles(ref float Angle)
    {
        while (Angle > 180)
            Angle -= 360;
        while (Angle < -180)
            Angle += 360;
        float angle = Angle - 180;
        if (angle > 0)
        {
            Angle = angle - 180;
        }
        else
        {
            Angle = angle + 180;
        }
    }

    //找到搖桿角度
    public static float GetStickAngle_L(Input_Manager InputState)
    {
        //找到移動角度
        return (Mathf.Atan2(-InputState.Now.L_JoyY, InputState.Now.L_JoyX) / Mathf.PI * 180);
    }

    //找到搖桿角度
    public static float GetStickAngle_R(Input_Manager InputState)
    {
        //找到移動角度
        return (Mathf.Atan2(-InputState.Now.R_JoyY, InputState.Now.R_JoyX) / Mathf.PI * 180);
    }

    //移動函式
    public static void NormalMove(ref GameObject Reference, ref GameObject Character, float angle, float Speed)
    {
        //Reference為參考物件，可能是相機等等
        GameCharacterController characterController = Character.GetComponent<GameCharacterController>();
        PlayableCharacter playerCharacter = Character.GetComponent<PlayableCharacter>();

        //找到移動角度
        float TargetAngle = Reference.transform.rotation.eulerAngles.y + angle + 90;
        ControllDriver.RefreshAngles(ref TargetAngle);

        //設定移動向量
        Quaternion Targetrotation = Quaternion.Euler(0, TargetAngle, 0);
        characterController.transform.rotation = Quaternion.Slerp(characterController.transform.rotation, Targetrotation, 0.08f);
        float RefreshedRinaRotation_Y = characterController.transform.rotation.eulerAngles.y;
        TargetAngle = Targetrotation.eulerAngles.y;
        ControllDriver.RefreshAngles(ref TargetAngle);
        ControllDriver.RefreshAngles(ref RefreshedRinaRotation_Y);

        if (Mathf.Abs(Mathf.Abs(TargetAngle) - Mathf.Abs(RefreshedRinaRotation_Y)) < 75)
        {
            playerCharacter.gameCharacterController.moveSpeed.x = characterController.transform.forward.x * Speed;
            playerCharacter.gameCharacterController.moveSpeed.z = characterController.transform.forward.z * Speed;
        }
    }

    //移動函式(倒退專用)
    public static void MoveBack(ref GameObject Reference, ref GameObject Character, float angle, float Speed)
    {
        //Reference為參考物件，可能是相機等等
        GameCharacterController characterController = Character.GetComponent<GameCharacterController>();
        PlayableCharacter playerCharacter = Character.GetComponent<PlayableCharacter>();

        //找到移動角度
        float TargetAngle = Reference.transform.rotation.eulerAngles.y + angle + 90 + 180;
        ControllDriver.RefreshAngles(ref TargetAngle);

        //設定移動向量
        Quaternion Targetrotation = Quaternion.Euler(0, TargetAngle, 0);
        characterController.transform.rotation = Quaternion.Slerp(characterController.transform.rotation, Targetrotation, 0.08f);
        float RefreshedRinaRotation_Y = characterController.transform.rotation.eulerAngles.y;
        TargetAngle = Targetrotation.eulerAngles.y;
        ControllDriver.RefreshAngles(ref TargetAngle);
        ControllDriver.RefreshAngles(ref RefreshedRinaRotation_Y);

        if (Mathf.Abs(Mathf.Abs(TargetAngle) - Mathf.Abs(RefreshedRinaRotation_Y)) < 75)
        {
            playerCharacter.gameCharacterController.moveSpeed.x = characterController.transform.forward.x * -1 * Speed;
            playerCharacter.gameCharacterController.moveSpeed.z = characterController.transform.forward.z * -1 * Speed;
        }
    }

    //處理視窗角度高度限制
    public static void HandleXAngle(ref GameObject lookReference, float CAMERA_MIN_X, float CAMERA_MAX_X)
    {
        if (FunctionDriver.GetRefreshAngles(lookReference.transform.rotation.eulerAngles.x) < CAMERA_MIN_X)
            lookReference.transform.transform.rotation = Quaternion.Euler(CAMERA_MIN_X, lookReference.transform.rotation.eulerAngles.y, lookReference.transform.rotation.eulerAngles.z);
        if (FunctionDriver.GetRefreshAngles(lookReference.transform.rotation.eulerAngles.x) > CAMERA_MAX_X)
            lookReference.transform.transform.rotation = Quaternion.Euler(CAMERA_MAX_X, lookReference.transform.rotation.eulerAngles.y, lookReference.transform.rotation.eulerAngles.z);
    }

    //類比值是否有所輸入
    public static bool IsAnyStickPushing_L(Input_Manager InputState)
    {
        return (!(Mathf.Abs(InputState.Now.L_JoyY) < 0.3f)) || (!(Mathf.Abs(InputState.Now.L_JoyX) < 0.3f));
    }

    //類比值是否有所輸入
    public static bool IsAnyStickPushing_R(Input_Manager InputState)
    {
        return (!(Mathf.Abs(InputState.Now.R_JoyY) < 0.3f)) || (!(Mathf.Abs(InputState.Now.R_JoyX) < 0.3f));
    }

    //得到兩點之間的距離
    public static float DistenceOf(Vector3 A, Vector3 B)
    {
        Vector3 offset = A - B;
        //得到距离
        float distance = offset.magnitude;
        return distance;
    }
}

//方便的子物件查找
public class ChildrenFinder
{
    public static List<GameObject> FindByTag(GameObject parent, string tag)
    {
        List<GameObject> children = new List<GameObject>();
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).gameObject.tag == tag)
                children.Add(parent.transform.GetChild(i).gameObject);
        }
        return children;
    }
    public static GameObject FindByTag(GameObject parent, string tag, int index)
    {
        if (index < 0)
            return null;
        List<GameObject> children = new List<GameObject>();
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).gameObject.tag == tag)
                children.Add(parent.transform.GetChild(i).gameObject);
        }
        try
        {
            return children[index];
        }
        catch
        {
            return null;
        }
    }
    public static List<GameObject> FindByName(GameObject parent, string name)
    {
        List<GameObject> children = new List<GameObject>();
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).gameObject.name == name)
                children.Add(parent.transform.GetChild(i).gameObject);
        }
        return children;
    }
    public static GameObject FindByName(GameObject parent, string name, int index)
    {
        if (index < 0)
            return null;
        List<GameObject> children = new List<GameObject>();
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).gameObject.name == name)
                children.Add(parent.transform.GetChild(i).gameObject);
        }
        try
        {
            return children[index];
        }
        catch
        {
            return null;
        }
    }
}

//以名稱尋找Action
public static class ActionFinder
{
    //以名稱尋找Action
    public static ActionInterface GetActionByName(List<ActionInterface> characterActions, string name)
    {
        foreach (ActionInterface characterAction in characterActions)
        {
            if (name == characterAction.ActionName)
                return characterAction;
        }
        return null;
    }

    //以編號尋找Action
    public static ActionInterface GetActionByID(List<ActionInterface> characterActions, int ID)
    {
        foreach (ActionInterface characterAction in characterActions)
        {
            if (ID == characterAction.ActionID)
                return characterAction;
        }
        return null;
    }
}

//方便的查詢聲音
public class SoundFinder
{
    //聲音必須是唯一的名字!!
    public static AudioClip FindAudioByName(List<AudioClip> source, string name)
    {
        for (int i = 0; i < source.Count; i++)
        {
            if (source[i].name == name)
                return source[i];
        }
        return null;
    }

    //聲音必須是唯一的名字!!
    public static AudioSource FindAudioSourceByName(GameObject source, string name)
    {
        AudioSource[] audioSources = source.GetComponents<AudioSource>();
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i].clip.name == name)
                return audioSources[i];
        }
        return null;
    }
}

//遊戲環境驅動器
public class GameEnvironment
{
    public static GameEnvironmentScripts entity
    {
        get
        {
            return GameObject.Find("GameEnvironment").GetComponent<GameEnvironmentScripts>();
        }
    }
}

public class AnimatorDriver
{
    public static bool IsCurrentAnimation(Animator animator, int layerIndex ,string animationName)
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(layerIndex);
        string LayerName = animator.GetLayerName(layerIndex);
       // Debug.Log(LayerName);
        return (currentState.nameHash == Animator.StringToHash(LayerName + "."+animationName));
    }
}