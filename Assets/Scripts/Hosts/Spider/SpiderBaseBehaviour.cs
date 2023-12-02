using System;
using Unity.VisualScripting;
// using UnityEditor.Animations;
using UnityEngine;

public class SpiderBaseBehaviour : StateMachineBehaviour
{
    public static int crawlHash = Animator.StringToHash("BaseLayer.Crawl");
    public static int idleHash = Animator.StringToHash("BaseLayer.Idle");

    [SerializeField]
    Animator animator;
    [SerializeField]
    SpiderComponent spider;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;
        spider = animator.GetComponent<SpiderComponent>();
        HostBehaviour.SubscribeToDelegate(ref spider.web.Touched, spider.MoveToWeb);
        HostBehaviour.SubscribeToDelegate(ref spider.Moving, UpdateDir);
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        spider.Moving -= UpdateDir;
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    void UpdateDir(Vector2 pos)
    {
        float degrees = Vector2.SignedAngle(Vector2.down, pos);
        if (spider.transform.eulerAngles.z == degrees)
        {
            return;
        }
        var rotation = Quaternion.Euler(0, 0, degrees);
        spider.transform.rotation = rotation;
    }
}
