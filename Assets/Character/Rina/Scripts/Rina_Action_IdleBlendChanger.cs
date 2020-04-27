using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class Rina_Action_IdleBlendChanger : StateMachineBehaviour
{
    private const int ACTION_INDEX = 0;
    private const string BLEND_NAME = "Blend_Idle";
    private const float IDLE_CHANGE_CYCLETIME = 3;
    private float IdleChangeTimmer = 0;
    private float IKTickTimer = 0.0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!(animator.GetInteger("ActionTrigger") == -1))
        {
            IdleChangeTimmer = 0;
            animator.SetFloat(BLEND_NAME, 0.0f);
        }
        animator.SetInteger("ActionTrigger", -1);
        IKTickTimer = 0.0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("ActionIndex", ACTION_INDEX);
        if (IdleChangeTimmer < IDLE_CHANGE_CYCLETIME)
            IdleChangeTimmer += Time.deltaTime;
        else
        {
            if (animator.GetFloat(BLEND_NAME) < 1)
                animator.SetFloat(BLEND_NAME, animator.GetFloat(BLEND_NAME) + 2 * Time.deltaTime);
            if (animator.GetFloat(BLEND_NAME) > 1)
                animator.SetFloat(BLEND_NAME, 1.0f);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        IdleChangeTimmer = 0;
        animator.SetFloat(BLEND_NAME, 0.0f);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
