using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitmanNewContract : StateMachineBehaviour
{
    AStarNavigation astar;
    HitmanFSM hitFSM;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        astar = animator.GetComponent<AStarNavigation>();
        hitFSM = animator.GetComponent<HitmanFSM>();
        hitFSM.NextContract();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Check if there is a new target contract
        if (hitFSM.GetContractObject() == null)
        {
            //disable hitman
            hitFSM.gameObject.SetActive(false);
        }
        else
        {
            if (animator.GetBool("IsTargetDead"))
            {
                //reset
                animator.SetBool("ReachedWayPoint",true);
                animator.SetBool("FoundTarget", false);
                //transition to Set New Destination
                animator.SetBool("IsTargetDead", false);
            }
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
