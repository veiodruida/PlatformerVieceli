using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles movement along waypoints using MovePosition for perfect linear precision.
/// Automatically cleans up conflicting movement scripts.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class WaypointMover : MonoBehaviour
{
    [Header("Settings")]
    public List<Transform> waypoints = new List<Transform>();
    public float moveSpeed = 4f;
    public float waitTime = 1f;

    [Header("State")]
    public bool stopped = false;
    public Vector3 travelDirection;

    private Rigidbody2D rb;
    private int currentWaypointIndex = 0;
    private Vector2 targetPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.simulated = true;
            rb.gravityScale = 0;
            rb.freezeRotation = true;
            rb.useFullKinematicContacts = false; // Stay false for waypoint precision
        }

        DisableConflicts();

        if (waypoints.Count > 0)
        {
            targetPos = waypoints[currentWaypointIndex].position;
        }
    }

    private void DisableConflicts()
    {
        var walking = GetComponent<WalkingEnemy>();
        if (walking != null) walking.enabled = false;
        
        var flying = GetComponent<FlyingEnemy>();
        if (flying != null) flying.enabled = false;
    }

    void FixedUpdate()
    {
        if (stopped || waypoints.Count == 0) return;

        Vector2 currentPos = rb.position;
        targetPos = waypoints[currentWaypointIndex].position;

        // Visual direction for animators
        travelDirection = ((Vector3)targetPos - (Vector3)currentPos).normalized;

        // Precise linear movement
        Vector2 nextPos = Vector2.MoveTowards(currentPos, targetPos, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(nextPos);

        // Reach check
        if (Vector2.Distance(nextPos, targetPos) < 0.01f)
        {
            BeginWait();
        }
    }

    void BeginWait()
    {
        stopped = true;
        Invoke("EndWait", waitTime);
    }

    void EndWait()
    {
        stopped = false;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
    }
}
