using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitmanKillState : StateMachineBehaviour
{
    AStarNavigation astar;
    HitmanFSM hitFSM;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        astar = animator.GetComponent<AStarNavigation>();
        hitFSM = animator.GetComponent<HitmanFSM>();
        //Set destination to Contract Target
        astar.ChangeGoalPosition(hitFSM.GetContractObject().transform.position);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Check if destination is set to Contract Target
        if (astar.goalPos.position != hitFSM.GetContractObject().transform.position)
        {
            astar.goalPos.position = hitFSM.GetContractObject().transform.position;
        }
        //Check if chasing target & being chased & have smoke grenades available
        if (astar.goalPos.position == hitFSM.GetContractObject().transform.position  && hitFSM.isBeingChased)
        {
            hitFSM.CreateSmokeBubble();
        }
        if (!hitFSM.GetContractObject().activeSelf)
        {
            //transition to Set New Destination
            animator.SetBool("IsTargetDead", true);
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
