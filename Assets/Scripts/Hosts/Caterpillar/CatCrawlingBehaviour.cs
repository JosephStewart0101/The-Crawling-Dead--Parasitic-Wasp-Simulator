using UnityEngine;

public class CatCrawlBehaviour : StateMachineBehaviour
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
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Distance from destination
        if (Mathf.Abs(cat.curDests[0].x - cat.rb.position.x) < .05f)
        {
            PlayIdle();
            cat.DestReached?.Invoke();
            return;
        }
        cat.MoveToDest(cat.curDests[0]);
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

    void PlayIdle()
    {
        animator.Play(CatBaseBehaviour.idleHash);
    }
}
