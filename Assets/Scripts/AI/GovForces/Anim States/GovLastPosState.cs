using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GovLastPosState : StateMachineBehaviour
{
    
    AStarNavigation astar;
    GovForceFSM GovFSM;
    GameObject visibleTarget;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        astar = animator.GetComponent<AStarNavigation>();
        GovFSM = animator.GetComponent<GovForceFSM>();
        EnemyFoundCheck(animator);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        EnemyFoundCheck(animator);
    }
    /// <summary>
    /// Check if Enemy's in sight or if this object reached last known position
    /// </summary>
    /// <param name="_animator"></param>
    private void EnemyFoundCheck(Animator _animator)
    {
        visibleTarget = GovFSM.GetFirstVisibleObject(GovFSM.EnemyAspects);
        if (visibleTarget != null)
        {
            //Transition to Chasing
            //_animator.SetBool("EnemyLost", false);
            _animator.SetBool("InShootRange", true);
        }
        else if (astar.reachedGoal)
        {
            //Transition to Patrol
            _animator.SetBool("WayPointReached", true);
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
