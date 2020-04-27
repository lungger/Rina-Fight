using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rina_Action_JogBlendChanger : StateMachineBehaviour
{
    private const int ACTION_INDEX = 1;
    private const string BLEND_NAME_X = "Blend_Jog_0";
    private const string BLEND_NAME_Y = "Blend_Jog_1";
    private float IdleChangeTimmer = 0;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!(animator.GetInteger("ActionTrigger") == -1))
        {
            if (animator.GetBool("LockTarget") == true)
            {
                Input_Manager InputState = animator.gameObject.GetComponent<Rina_Mainscript>().InputState;
                float X = InputState.Now.L_JoyX;
                float Y = InputState.Now.L_JoyY;
                animator.SetFloat(BLEND_NAME_X, X);
                animator.SetFloat(BLEND_NAME_Y, Y);
            }
        }
        animator.SetInteger("ActionTrigger", -1);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("LockTarget") == true)
        {
            Input_Manager InputState = animator.gameObject.GetComponent<Rina_Mainscript>().InputState;
            float X = InputState.Now.L_JoyX;
            float Y = InputState.Now.L_JoyY;
            animator.SetFloat(BLEND_NAME_X, X);
            animator.SetFloat(BLEND_NAME_Y, Y);
        }
        else 
        {
            float X = 0;
            float Y = 0;
            animator.SetFloat(BLEND_NAME_X, 0);
            animator.SetFloat(BLEND_NAME_Y, 0);
        }

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
