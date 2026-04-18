using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles parenting the player to moving platforms.
/// Optimized to ONLY parent when the player is on TOP of the platform.
/// Uses Delta Movement Synchronization for absolute stability.
/// </summary>
public class PlayerChilder : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 lastPosition;
    private List<Rigidbody2D> childRigidbodies = new List<Rigidbody2D>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            lastPosition = rb.position;
            // Force kinematic for platform stability
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.simulated = true;
            rb.useFullKinematicContacts = false;
        }
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        // Calculate platform movement delta
        Vector2 currentPosition = rb.position;
        Vector2 movementDelta = currentPosition - lastPosition;
        lastPosition = currentPosition;

        // Sync child positions
        if (movementDelta.sqrMagnitude > 0)
        {
            for (int i = childRigidbodies.Count - 1; i >= 0; i--)
            {
                var childRb = childRigidbodies[i];
                if (childRb != null)
                {
                    childRb.MovePosition(childRb.position + movementDelta);
                }
                else
                {
                    childRigidbodies.RemoveAt(i);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // IMPORTANT: Only parent if landing on the TOP of the platform
            // This prevents "sticking" to sides or bottom.
            if (collision.contacts.Length > 0 && collision.contacts[0].normal.y < -0.5f)
            {
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null && !childRigidbodies.Contains(playerRb))
                {
                    childRigidbodies.Add(playerRb);
                    collision.transform.SetParent(transform);
                    Debug.Log($"[PlayerChilder] Player landed on TOP of {name}. Parenting active.");
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                childRigidbodies.Remove(playerRb);
                if (collision.transform.parent == transform)
                {
                    collision.transform.SetParent(null);
                    Debug.Log($"[PlayerChilder] Player left platform {name}. Parenting released.");
                }
            }
        }
    }

    // Trigger support for non-solid platforms (e.g., semi-solid ones)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // For triggers, we do a simple height check to ensure player is above center
            if (collision.transform.position.y > transform.position.y)
            {
                Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
                if (playerRb != null && !childRigidbodies.Contains(playerRb))
                {
                    childRigidbodies.Add(playerRb);
                    collision.transform.SetParent(transform);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                childRigidbodies.Remove(playerRb);
                if (collision.transform.parent == transform)
                {
                    collision.transform.SetParent(null);
                }
            }
        }
    }
}
