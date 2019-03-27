using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GovSetGoalState : StateMachineBehaviour
{
    AStarNavigation astar;
    GovForceFSM GovFSM;
    GameObject visibleTarget;
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        astar = animator.GetComponent<AStarNavigation>();
        GovFSM = animator.GetComponent<GovForceFSM>();
        visibleTarget = GovFSM.GetFirstVisibleObject(GovFSM.EnemyAspects);
        
        astar.stopRadius = astar.initialstopRadius;//set to the original stopRadius
        if (visibleTarget != null)
        {
            //Transition to Chase Enemy
            animator.SetBool("EnemyLost", false);
        }
        
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        visibleTarget = GovFSM.GetFirstVisibleObject(GovFSM.EnemyAspects);
        if (visibleTarget != null)
        {
            //Transition to Chase Enemy
            animator.SetBool("EnemyLost", false);
        }
        else if (animator.GetBool("WayPointReached"))
        {
            astar.ChangeGoalPosition(astar.goalPos.GetComponent<Waypoint>().GetRandomDestination());
            //Transition to Patrol
            animator.SetBool("WayPointReached", false);
        }
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

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
