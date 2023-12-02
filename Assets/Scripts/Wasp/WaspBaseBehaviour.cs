using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspBaseBehaviour : StateMachineBehaviour
{
    public static int groundedHash = Animator.StringToHash("Grounded");
    public static int horizontalHash = Animator.StringToHash("Horizontal");

    public static int crawlStateHash = Animator.StringToHash("BaseLayer.Crawl");
    public static int fallStateHash = Animator.StringToHash("BaseLayer.Fall");
    public static int flyStateHash = Animator.StringToHash("BaseLayer.Fly");
    public static int locomotionStateHash = Animator.StringToHash("BaseLayer.LocomotionTree");
    public static int stingStateHash = Animator.StringToHash("BaseLayer.Sting");

    public Animator animator;
    public PlayerMovement pm;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;
        pm = animator.GetComponent<PlayerMovement>();

        HostBehaviour.SubscribeToDelegate(ref pm.move, UpdateMovementVec);
        HostBehaviour.SubscribeToDelegate(ref pm.updateGrounded, UpdateGrounded);
        HostBehaviour.SubscribeToDelegate(ref pm.parasitize, PlaySting);
        if (pm.isFlying)
        {
            pm.flying -= PlayFly;
        }
        else
        {
            HostBehaviour.SubscribeToDelegate(ref pm.flying, PlayFly);
        }

    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    void PlayFly()
    {
        animator.Play(flyStateHash);
    }

    void PlaySting()
    {
        animator.Play(stingStateHash);
    }

    void UpdateGrounded(bool isGrounded)
    {
        animator.SetBool(groundedHash, isGrounded);
    }

    void UpdateMovementVec(Vector2 dir)
    {
        animator.SetFloat(horizontalHash, Mathf.Abs(dir.x));
    }
}
