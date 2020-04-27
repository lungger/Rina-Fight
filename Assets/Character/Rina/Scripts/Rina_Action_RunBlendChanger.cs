using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rina_Action_RunBlendChanger : StateMachineBehaviour
{
    private const int ACTION_INDEX = 2;
    private const string BLEND_NAME = "Blend_Run";
    private const string MULTIPLIER_NAME = "Multiplier_Run";
    private const float RUN_CHANGE_CYCLETIME = 0.15f;
    private float RunChangeTimmer = 0;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!(animator.GetInteger("ActionTrigger") == -1))
        {
            animator.SetFloat(BLEND_NAME, 0.0f);
            animator.SetFloat(MULTIPLIER_NAME, 0.0f);
            RunChangeTimmer = 0;
        }
        animator.SetInteger("ActionTrigger", -1);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("ActionIndex", ACTION_INDEX);
        if (RunChangeTimmer < RUN_CHANGE_CYCLETIME)
        {
            RunChangeTimmer += Time.deltaTime * 1;
            animator.SetFloat(MULTIPLIER_NAME, 2.5f);
        }
        else
        {
            RunChangeTimmer = RUN_CHANGE_CYCLETIME;
            if (animator.GetFloat(BLEND_NAME) < 1)
                animator.SetFloat(BLEND_NAME, animator.GetFloat(BLEND_NAME) + 3f * Time.deltaTime);
            if (animator.GetFloat(BLEND_NAME) > 1)
            {
                animator.SetFloat(MULTIPLIER_NAME, 4f);
                animator.SetFloat(BLEND_NAME, 1.0f);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat(BLEND_NAME, 0.0f);
        animator.SetFloat(MULTIPLIER_NAME, 0.0f);
        RunChangeTimmer = 0;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
