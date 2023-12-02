using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBaseBehaviour : StateMachineBehaviour
{
    public static int crawlHash = Animator.StringToHash("BaseLayer.Crawl");
    public static int idleHash = Animator.StringToHash("BaseLayer.Idle");

    [SerializeField] CaterpillarComponent cat;
    [SerializeField] SpriteRenderer sr;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cat = animator.GetComponent<CaterpillarComponent>();
        sr = animator.GetComponent<SpriteRenderer>();
        HostBehaviour.SubscribeToDelegate(ref cat.Moving, UpdateDir);
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cat.Moving -= UpdateDir;
    }

    // Make sprite face movement direction
    public void UpdateDir(Vector2 dir)
    {
        if (dir.x < 0 && !sr.flipX)
        {
            sr.flipX = true;
        }
        else if (dir.x > 0 && sr.flipX)
        {
            sr.flipX = false;
        }
    }
}
