using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(EnemyController))]
[DefaultExecutionOrder(2)]
public class EnemyMovement : MovementScript
{
    private List<Transform> waypoints;
    [SerializeField] private float sightRange, attackRange;
    [SerializeField] private LayerMask playerMask;
    private int currentWalkPoint;
    private bool playerIsSightRange => Physics.CheckSphere(transform.position, sightRange, playerMask);
    private bool playerIsAttackRange => Physics.CheckSphere(transform.position, sightRange, playerMask);
    private NavMeshAgent agent;
    private Controller enemyController;
    private Transform playerTransform;
    protected override void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyController = GetComponent<Controller>();
    }
    private void Start()
    {
        waypoints = WayPointManager.Instance.Waypoints;
        playerTransform = UserController.Instance.transform;
    }
    protected override void Update()
    {
        if(!playerIsSightRange && !playerIsAttackRange) { patrol(); }
        else if(playerIsSightRange && !playerIsAttackRange) { chasePlayer(); }
        else if(playerIsAttackRange&& playerIsSightRange) { attackPlayer(); }
    }

    private void attackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(playerTransform.position);
        enemyController.Shoot();

    }

    private void chasePlayer()
    {
        agent.SetDestination(playerTransform.position);
        run();
    }

    private void patrol()
    {
        if (currentWalkPoint>0)//move to position
        {
            agent.SetDestination(waypoints[currentWalkPoint].position);
            walk();
            if ((transform.position-waypoints[currentWalkPoint].position).magnitude<1f)
            {
                currentWalkPoint= -1;
            }
        }
        else//find position to go to
        {
            searchWalkPoint();
        }
    }
    private void searchWalkPoint()
    {
        currentWalkPoint=Random.Range(0,waypoints.Count);
    }
}
