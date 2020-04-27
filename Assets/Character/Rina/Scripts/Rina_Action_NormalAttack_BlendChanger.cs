using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rina_Action_NormalAttack_BlendChanger : StateMachineBehaviour
{
    Animator Animator;
    private const int ACTION_INDEX = 7;
    private const string BLEND_NAME = "Blend_NormalAttack";
    private int attackCount = 0;
    private float Blend
    {
        get
        {
            return Animator.GetFloat(BLEND_NAME);
        }
        set
        {
            Animator.SetFloat(BLEND_NAME,value);
        }
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Animator = animator;
        animator.SetInteger("ActionTrigger", -1);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Animator = animator;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
