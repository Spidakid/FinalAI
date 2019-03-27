using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GovShootState : StateMachineBehaviour
{
    GovForceFSM GovFSM;
    AStarNavigation astar;
    GameObject projectile, visibleTarget;
    private float currentTime,MaxTime;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //set enemy not lost
        animator.SetBool("EnemyLost", false);
        GovFSM = animator.GetComponent<GovForceFSM>();
        astar = animator.GetComponent<AStarNavigation>();
        astar.isStopped = true;//activate astar
        ResetShootTimer(GovFSM);
        visibleTarget = GovFSM.GetFirstVisibleObject(GovFSM.EnemyAspects);
        if (visibleTarget != null)
        {
            TurnToTarget(animator,visibleTarget,GovFSM);
            FireProjectile(animator);
        }
        else
        {
            //Transition to LastKnownPosition
            animator.SetBool("InShootRange",false);
            astar.isStopped = false;//deactivate astar
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        visibleTarget = GovFSM.GetFirstVisibleObject(GovFSM.EnemyAspects);
        if (visibleTarget != null)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= MaxTime)
            {
                currentTime = 0;
                FireProjectile(animator);
                Debug.Log("Fire!");
            }
            TurnToTarget(animator, visibleTarget,GovFSM);
        }
        else
        {

            //Transition to LastKnownPosition
            animator.SetBool("InShootRange", false);
            animator.SetBool("EnemyLost", true);
            astar.isStopped = false;//deactivate astar
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
    private void TurnToTarget(Animator _animator,GameObject _target,GovForceFSM _govfsm)
    {
        Quaternion lookRot = Quaternion.LookRotation(_target.transform.position - _animator.transform.position);
        _animator.transform.rotation = Quaternion.Slerp(_animator.transform.rotation,lookRot,_govfsm.turnSpeed * Time.deltaTime);
    }
    private void ResetShootTimer(GovForceFSM _govfsm)
    {
        MaxTime = _govfsm.shootInterval;
        currentTime = 0;
    }
    private void FireProjectile(Animator _anim)
    {
        projectile = ObjectPooling.Instance.getFalseObject(_anim.transform.position + _anim.transform.forward / 1.5f, _anim.transform.rotation);
        projectile.SetActive(true);
    }
}
