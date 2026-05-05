using UnityEngine;
using UnityEngine.AI; 

public class EnemyAI : MonoBehaviour
{
    [Header("Wander Settings")]
    public float wanderRadius = 10f;
    public float walkSpeed = 0.5f;

    [Header("Chase Settings")]
    [SerializeField]
    private float runSpeed = 1f; 

    private NavMeshAgent agent;
    private Animator animator;
    
    private Transform playerTarget;
    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); 
        
        agent.speed = walkSpeed;
        SetNewRandomDestination();
    }

    void Update()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            SetNewRandomDestination();
        }
        

        // 2. Tell the Animator how fast we are moving to play the walk animation
        if (animator != null)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude); 
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(playerTarget.position);
    }

    void SetNewRandomDestination()
    {
        // Get a random point within a sphere around the enemy
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        // Ensure the random point is actually on the blue walkable floor (NavMesh)
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1))
        {
            // Tell the NavMeshAgent to walk to that point
            agent.SetDestination(hit.position);
        }
    }

    public void Die()
    {
        agent.enabled = false;
        // Optionally, play a death animation here
        /*if (animator != null)
        {
            animator.SetTrigger("Die");
        }*/
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTarget = other.transform;
            agent.speed = runSpeed;
            isChasing = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTarget = null;
            agent.speed = walkSpeed;
            isChasing = false;
        }
    }
}