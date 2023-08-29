using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : StateMachineBehaviour
{

    float timer;
    Transform player;
    float chaseRange;
    List<Transform> wayPoints = new List<Transform>();  //aggiungo la lista di waypoints
    NavMeshAgent agent;

    public bool isTurtle = false;
    public bool isSlime = false;
    public bool isScarab = false;

    private void Awake()
    {
        if (isTurtle)
        {
            chaseRange = 15;
        }
        else
        { 
            chaseRange = 8;
        }
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isTurtle)
        {
            timer = 0f;
        }

        player = GameObject.FindWithTag("Player").transform;  //inizializzo l'oggetto player
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = 1.5f;
        GameObject go = GameObject.FindGameObjectWithTag("Waypoint");   //trovo gli oggetti col tag Waypoints
        foreach (Transform t in go.transform)             //trovo tutti i figli dell'Empty object "Waypoints"
            wayPoints.Add(t);

        agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);   //cambia la posizione del nemico in uno dei waypoints
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);   //controlla quando un waypoint è stato raggiunto per passare al successivo
        
        float distance = Vector3.Distance(player.position, animator.transform.position);

        if (isTurtle) 
        {
            timer += Time.deltaTime;
            if (timer > 7)
                animator.SetBool("isPatrolling", false);
            if (distance < chaseRange)
                animator.SetBool("isChasing", true);
        }
        else if (isSlime) 
        {
            animator.SetBool("isPatrolling", false);
            if (distance < chaseRange)
                animator.SetBool("isChasing", true);
        }
        else if (isScarab)
        {
            animator.SetBool("isPatrolling", true);
            if (distance < chaseRange)
            {
                animator.SetBool("isPatrolling", false);
                animator.SetBool("isChasing", true);
            }
            else
            {
                animator.SetBool("isPatrolling", true);
                animator.SetBool("isChasing", false);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);   //ferma il nemico quando esce dallo stato Patrol
    }

}
