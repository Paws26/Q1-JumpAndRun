using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed;
    [SerializeField] private bool reverseAtEnd = false;
    private int currentWaypointIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position += direction * speed * Time.fixedDeltaTime;

        // Check if the platform has reached the target waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            // If reverseAtEnd is true and we are at the last waypoint, reverse the waypoints array
            if (currentWaypointIndex == waypoints.Length - 1 && reverseAtEnd)
            {
                System.Array.Reverse(waypoints);
            }

            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

    }

    public Vector3 GetVelocity()
    {
        if (waypoints.Length == 0) return Vector3.zero;
        
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        return direction * speed;
    }
}

