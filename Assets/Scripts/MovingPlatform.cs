using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

/// <summary>
/// A moving platform that can follow a series of waypoints. 
/// It can be configured to reverse at the end of the path, be controlled by a lever, or start moving when the player steps on it. 
/// It also supports easing in and out at the start and end of the path for smoother movement.
/// 
/// Waypoints should be set up in the Unity Editor as child objects of the platforms parent, and assigned to the "waypoints" array.
/// This was a pain in the ass to get right!
/// </summary>

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed;
    [SerializeField] private bool reverseAtEnd = false;
    [SerializeField] private bool controlledByLever = false;
    [SerializeField] private bool controlledByPlayer = false;
    [SerializeField] private bool easeInOut = false;
    private float easeDistance = 1f;
    private List<Vector3> waypointPositions;
    private int fromIndex = 0;
    private int toIndex = 1;
    private bool isLeverActivated;
    private bool isPlatformMoving;
    public Vector3 velocity;
    public Vector3 Velocity { get => velocity; set => velocity = value; }
    public UnityEvent<bool> onLockedStateChanged;

    void Start()
    {
        // Build the full waypoint path: [StartPosition, Waypoint1, Waypoint2, ...]
        waypointPositions = new List<Vector3>
        {
            transform.position // Prepend start position
        };
        foreach (Transform wp in waypoints)
        {
            waypointPositions.Add(wp.position);
        }

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

    private Vector3 GetCurrentFromPosition()
    {
        return waypointPositions[fromIndex];
    }

    private Vector3 GetCurrentToPosition()
    {
        return waypointPositions[toIndex];
    }

    void FixedUpdate()
    {
        if (!isPlatformMoving || waypointPositions.Count == 0) return;

        Vector3 fromPos = GetCurrentFromPosition();
        Vector3 toPos = GetCurrentToPosition();
        Vector3 direction = (toPos - transform.position).normalized;
        float currentSpeed = speed;

        // Calculate speed for easing in and out if enabled
        if (easeInOut)
        {
            // Calculate easeDistance
            easeDistance = Mathf.Min(easeDistance, Vector3.Distance(fromPos, toPos) / 4f);
            float distanceFromStart = Vector3.Distance(transform.position, fromPos);
            float distanceToEnd = Vector3.Distance(transform.position, toPos);

            // Ease in when leaving the start position (fromIndex == 0)
            if (fromIndex == 0 && distanceFromStart < easeDistance)
            {
                currentSpeed = Mathf.Lerp(0.1f, speed, distanceFromStart / easeDistance);
            }
            // Ease out when approaching the last waypoint (toIndex == last)
            else if (toIndex == waypointPositions.Count - 1 && distanceToEnd < easeDistance)
            {
                currentSpeed = Mathf.Lerp(0.1f, speed, distanceToEnd / easeDistance);
            }
        }

        float distanceToTarget = Vector3.Distance(transform.position, toPos); // Distance to the target waypoint
        float moveDistance = currentSpeed * Time.fixedDeltaTime; // Distance to move this frame

        if (moveDistance >= distanceToTarget) 
        {
            // Snap to the waypoint
            transform.position = toPos;
            Velocity = Vector3.zero;
            Debug.Log($"Platform Velocity: {Velocity}");

            // Check if we're about to reach the end of the path
            if (toIndex == waypointPositions.Count - 1)
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

                // Reverse the path if enabled 
                // Otherwise will travel back to the start on next activation (only really makes sense for lever-controlled or uncontrolled platforms)
                if (reverseAtEnd)
                {
                    waypointPositions.Reverse();
                    fromIndex = 0;
                    toIndex = 1;
                }
            }
            else
            {
                // Move to the next waypoint segment
                fromIndex = toIndex;
                toIndex++;
            }
        }
        else
        {
            // Move towards the target waypoint
            transform.position += direction * moveDistance;
            Velocity = direction * currentSpeed;
            Debug.Log($"Platform Velocity: {Velocity}");
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

    void OnTriggerEnter(Collider other)
    {
        // If the player steps on the platform start moving
        if (other.CompareTag("Player") && !controlledByLever && controlledByPlayer)
        {
            isPlatformMoving = true;
        }
    }
}

