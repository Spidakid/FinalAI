using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormPreyState : StateMachineBehaviour
{
    AStarNavigation astar;
    WormFSM wormFSM;
    bool finishEating;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        finishEating = false;
        astar = animator.GetComponent<AStarNavigation>();
        wormFSM = animator.GetComponent<WormFSM>();
        astar.ChangeGoalPosition(wormFSM.foodAgent.transform.position);
        astar.Speed = wormFSM.chaseSpeed;//Increase speed 
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (astar.goalPos.position != wormFSM.foodAgent.transform.position)
        {
            astar.goalPos.position = wormFSM.foodAgent.transform.position;
        }
        if (astar.reachedGoal)
        {
            if (!finishEating)
            {
                finishEating = true;
                astar.Speed = wormFSM.initialSpeed; 
                //transition to Grow State
                animator.SetTrigger("Grow");
                animator.SetBool("SmellsPoo", false);
                
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
