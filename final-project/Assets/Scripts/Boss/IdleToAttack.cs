using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleToAttack : StateMachineBehaviour
{
    Transform player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //in base alla distanza con il giocatore...
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance <= 5f)
        {
            animator.SetBool("isAttacking", true);
            animator.SetBool("isChasing", false);
        }
        else if (distance >= 5f && distance <= 15f)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isChasing", true);
        }
        else
        {
            if (Boss.currentHP <= 250 && distance >= 15f) //...e alla vita del boss...
                animator.SetBool("isCasting", true);
            else if (Boss.currentHP <= 250 && distance <= 15f)
                animator.SetBool("isCasting", false);
            //...passa da uno stato all'altro dell'animator.
        }
    }
}