using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class Rina_Action_ShotBlendChanger : StateMachineBehaviour
{
    private const float IK_TICK_CYCLE = 0.075f;
    private bool enter = false;
    private float IKTickTimer = 0.0f;
    private float shotActionTimer = 0.0f;
    private float IK = 0.0f;
    private Vector3 TargetPosition = new Vector3(0, 0, 0);
    private PlayableCharacter Master;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Master = animator.gameObject.GetComponent<PlayableCharacter>();
        animator.SetInteger("ActionIndex", animator.gameObject.GetComponent<PlayableCharacter>().actionIndex);
        animator.SetInteger("ActionTrigger", -1);
        enter = true;
        shotActionTimer = 0;
        if (Master.lockTarget != null)
        {
            TargetPosition = Master.lockTarget.CenterPosition;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enter = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        shotActionTimer += Time.deltaTime;
        //持續讓手部對準目標
        if (animator.GetBool("LockTarget"))
        {
            if (shotActionTimer < 0.4)
            {
                IK = 0.7f;
            }
            else
            {
                IKTickTimer += Time.deltaTime;
                if (IKTickTimer > IK_TICK_CYCLE)
                {
                    IKTickTimer = 0;
                    if (IK > 0.0f)
                        IK -= 0.1f;
                    else
                        IK = 0;
                }
            }
        }
        else
        {
            IKTickTimer += Time.deltaTime;
            if (IKTickTimer > IK_TICK_CYCLE)
            {
                IKTickTimer = 0;
                if (IK > 0.0f)
                    IK -= 0.1f;
                else
                    IK = 0;
            }
        }
        if (Master.lockTarget != null)
        {
            animator.SetIKPosition(AvatarIKGoal.LeftHand, animator.gameObject.GetComponent<PlayableCharacter>().lockTarget.CenterPosition);
            animator.SetLookAtPosition(animator.gameObject.GetComponent<PlayableCharacter>().lockTarget.transform.position);
        }
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, IK);
        animator.SetLookAtWeight(IK);
    }
}
