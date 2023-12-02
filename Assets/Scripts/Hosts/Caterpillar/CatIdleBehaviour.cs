using UnityEngine;

public class CatIdleBehaviour : StateMachineBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    CaterpillarComponent cat;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;
        cat = animator.GetComponent<CaterpillarComponent>();
        HostBehaviour.SubscribeToDelegate(ref cat.Moving, PlayCrawl);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (cat.isParasitizable)
        {
            cat.Scan();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cat.Moving -= PlayCrawl;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    void PlayCrawl(Vector2 dst)
    {
        animator.Play(CatBaseBehaviour.crawlHash);
    }
}
