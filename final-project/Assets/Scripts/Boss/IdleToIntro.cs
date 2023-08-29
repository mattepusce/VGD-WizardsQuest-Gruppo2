using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.AI;

//Gestisce il passaggio dallo stato iniziale di Idle a una animazione di "Aggro"
public class IdleToIntro : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent agent;
    float distance;
    public float AggroRange = 30f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        distance = Vector3.Distance(player.position, animator.transform.position);  //calcola distanza boss player
        if (distance <= AggroRange)
        {
            animator.SetBool("isAggroing", true);   //passa allo stato successivo
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isAggroing", false);
    }
}