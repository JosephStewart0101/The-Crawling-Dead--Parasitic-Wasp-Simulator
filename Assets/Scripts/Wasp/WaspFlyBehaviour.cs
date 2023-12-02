using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspFlyBehaviour : StateMachineBehaviour
{
    Animator animator;
    PlayerMovement pm;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;
        pm = animator.GetComponent<PlayerMovement>();
        HostBehaviour.SubscribeToDelegate(ref pm.notFlying, PlayNext);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pm.notFlying -= PlayNext;
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

    void PlayNext()
    {
        if (animator.GetBool(WaspBaseBehaviour.groundedHash))
        {
            animator.Play(WaspBaseBehaviour.locomotionStateHash);
        }
        else
        {
            animator.Play(WaspBaseBehaviour.fallStateHash);
        }
    }

}
