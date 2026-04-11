using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed;
    [SerializeField] private bool reverseAtEnd = false;
    [SerializeField] private bool controlledByLever = false;
    [SerializeField] private bool controlledByPlayer = false;
    private int currentWaypointIndex = 0;
    private bool isLeverActivated;
    private bool isPlatformMoving;
    public Vector3 velocity;
    public Vector3 Velocity { get => velocity; set => velocity = value; }
    public UnityEvent<bool> onLockedStateChanged;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // All platforms start stationary, except uncontrolled ones
        if (controlledByLever || controlledByPlayer)
        {
            isPlatformMoving = false;
            isLeverActivated = false;
        }
        else
        {
            // Uncontrolled: starts moving immediately
            isPlatformMoving = true;
        }
    }

    void FixedUpdate()
    {
        if (!isPlatformMoving || waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        Vector3 target = transform.position + direction * speed * Time.fixedDeltaTime;
        Velocity = (target - transform.position) / Time.fixedDeltaTime;
        //transform.position += direction * speed * Time.fixedDeltaTime;
        transform.position = target;

        // Check if the platform has reached the target waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            // Are we at the end of the waypoints array?
            if (currentWaypointIndex == waypoints.Length - 1)
            {
                // Player-controlled: stop at the final waypoint
                if (controlledByPlayer)
                {
                    isPlatformMoving = false;
                }

                // Lever-controlled: stop if lever is off
                if (controlledByLever && !isLeverActivated)
                {
                    isPlatformMoving = false;
                    onLockedStateChanged.Invoke(false);
                }
                
                // Reverse the waypoints if needed
                if (reverseAtEnd)
                {
                    System.Array.Reverse(waypoints);
                }
            }

            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    public void ActivatePlatformLever(bool active)
    {
        isLeverActivated = active;
        if (isLeverActivated && !isPlatformMoving)
        {
            isPlatformMoving = true;
        }  
        else if (!isLeverActivated && isPlatformMoving)  // Lock the lever if it's not active and currently moving
        {
            onLockedStateChanged.Invoke(true);
        }
    }

    // Activate platform once, e.g. from a button press event
    public void ActivatePlatform()
    {
        isPlatformMoving = true;
    }
    /*
    public Vector3 GetVelocity()
    {
        if (waypoints.Length == 0 || !isPlatformMoving) return Vector3.zero;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.localPosition) / Time.fixedDeltaTime;
        return direction;
    }*/

    void OnTriggerEnter(Collider other)
    {
        // If the player steps on the platform start moving
        if (other.CompareTag("Player") && !controlledByLever && controlledByPlayer)
        {
            isPlatformMoving = true;
        }
    }
}

