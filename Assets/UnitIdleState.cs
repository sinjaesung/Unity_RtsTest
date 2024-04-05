using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIdleState : StateMachineBehaviour
{
    //wow
    //wow
    //wow ->여러줄 한줄주석으로 동시에 하고싶으면 ctrl + k + / -> 풀고싶을땐 ctrl + k + u
    AttackController attackController;
   
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
     override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
     {
        attackController = animator.transform.GetComponent<AttackController>();
     }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Check if there is an available target
        if (attackController.targetToAttack != null)
        {
            // --- Transition to Follow State -- 
            animator.SetBool("isFollowing", true);
        }
    }
}
