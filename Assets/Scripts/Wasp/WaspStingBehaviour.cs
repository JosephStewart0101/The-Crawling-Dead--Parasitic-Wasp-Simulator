using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspStingBehaviour : StateMachineBehaviour
{
    PlayerMovement pm;
    Rigidbody2D rb;
    float gs;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pm = animator.GetComponent<PlayerMovement>();
        rb = animator.GetComponent<Rigidbody2D>();
        pm.enabled = false;
        rb.velocity = Vector2.zero;
        gs = rb.gravityScale;
        rb.gravityScale = 0;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pm.enabled = true;
        rb.gravityScale = gs;
        if (pm.currentHost != null)
        {
            pm.currentHost.StartMinigame();
            pm.currentHost = null;
        }
    }
}
