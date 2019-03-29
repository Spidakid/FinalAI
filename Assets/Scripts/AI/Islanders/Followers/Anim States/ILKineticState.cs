using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ILKineticState : StateMachineBehaviour
{
    AStarBoid astarBoid;
    Boid boid;
    BoidFeeling boidFeel;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        astarBoid = animator.GetComponent<AStarBoid>();
        astarBoid.isStopped = true;//turn off astar
        boid = animator.GetComponent<Boid>();
        animator.transform.rotation = Quaternion.Euler(Vector3.zero); //reset rotation to face Z-Axis
        boid.enabled = true;//turn on kinetic
        boidFeel = animator.GetComponentInChildren<BoidFeeling>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boidFeel.obstacleDetected)
        {
            //transition to Astar mode
           animator.SetBool("IsObstacleDetected", true);
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
