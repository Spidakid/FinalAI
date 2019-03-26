using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GovShootState : StateMachineBehaviour
{
    GameObject projectile;
    private float currentTime,MaxTime;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetShootTimer(animator);
        FireProjectile(animator);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime += Time.deltaTime;
        if (currentTime >= MaxTime)
        {
            currentTime = 0;
            FireProjectile(animator);
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
    private void ResetShootTimer(Animator _anim)
    {
        MaxTime = _anim.GetComponent<GovForceFSM>().shootInterval;
        currentTime = 0;
    }
    private void FireProjectile(Animator _anim)
    {
        projectile = ObjectPooling.Instance.getFalseObject(_anim.transform.position + _anim.transform.forward / 2, _anim.transform.rotation);
        projectile.SetActive(true);
    }
}
