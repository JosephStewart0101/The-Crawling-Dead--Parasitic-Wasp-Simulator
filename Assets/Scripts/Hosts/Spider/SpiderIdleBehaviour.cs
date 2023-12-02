using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpiderIdleBehaviour : StateMachineBehaviour
{
    Animator animator;
    SpiderComponent spider;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;
        spider = animator.GetComponent<SpiderComponent>();
        HostBehaviour.SubscribeToDelegate(ref spider.Moving, PlayCrawl);
        HostBehaviour.SubscribeToDelegate(ref spider.web.Touched, PlayCrawl);

        if (spider.curCoroutine != null)
        {
            spider.StopCoroutine(spider.curCoroutine);
        }
        spider.curCoroutine = spider.Think();
        spider.StartCoroutine(spider.curCoroutine);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (spider.curCoroutine != null)
        {
            spider.StopCoroutine(spider.curCoroutine);
            spider.curCoroutine = null;
        }
        spider.Moving -= PlayCrawl;
        spider.web.Touched -= PlayCrawl;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    void PlayCrawl(Vector2 dst)
    {
        animator.Play(SpiderBaseBehaviour.crawlHash);
    }
}