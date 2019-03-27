using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GovPatrolState : StateMachineBehaviour
{
    AStarNavigation astar;
    GovForceFSM GovFSM;
    GameObject visibleTarget;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GovFSM = animator.GetComponent<GovForceFSM>();
        astar = animator.GetComponent<AStarNavigation>();
        astar.stopRadius = astar.initialstopRadius;//set to the original stopRadius
        visibleTarget = GovFSM.GetFirstVisibleObject(GovFSM.EnemyAspects);
        if (visibleTarget != null)
        {
            //Transition to Chase
            animator.SetBool("EnemyLost",false);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        visibleTarget = GovFSM.GetFirstVisibleObject(GovFSM.EnemyAspects);
        if (visibleTarget != null)
        {
            //Transition to chase
            animator.SetBool("EnemyLost", false);
        }
        else if (astar.reachedGoal)
        {
            animator.SetBool("WayPointReached",true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
