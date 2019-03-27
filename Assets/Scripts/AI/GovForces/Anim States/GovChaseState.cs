using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GovChaseState : StateMachineBehaviour
{
    AStarNavigation astar;
    GovForceFSM GovFSM;
    GameObject visibleTarget;
   
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("EnemyLost", false);
        astar = animator.GetComponent<AStarNavigation>();
        GovFSM = animator.GetComponent<GovForceFSM>();

        //set stopping distance to shooting stop distance
        astar.stopRadius = GovFSM.startShootingRange;
        visibleTarget = GovFSM.GetFirstVisibleObject(GovFSM.EnemyAspects);
        //Check if there is a visible object with this aspect & proceed to chase
        if (visibleTarget != null)
        {
            astar.ChangeGoalPosition(visibleTarget.transform.position);
        }
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        visibleTarget = GovFSM.GetFirstVisibleObject(GovFSM.EnemyAspects);
        //Check if there is a visible object with this aspect & proceed to chase
        if (visibleTarget != null)
        {
            
            //Set destination to visible target
            if (astar.goalPos.position != visibleTarget.transform.position)
            {
                astar.ChangeGoalPosition(visibleTarget.transform.position);
            }
            //Target visible & shoot range reached
            if (astar.reachedGoal)
            {
                //Transition to Shoot State
                animator.SetBool("InShootRange",true);
            }
        }
        else if (visibleTarget == null && !astar.reachedGoal)
        {
            //Transition to Last Known Position State
            animator.SetBool("EnemyLost",true);
        }
        else
        {
            //Transition to SetWayPoint(Patrol)
            animator.SetBool("WayPointReached",true);
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
