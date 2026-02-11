using UnityEngine;

public class Resucer : MonoBehaviour
{
    [Header("Push force for the Rescuer")]
    public float pushForce = 200f;

    [Header("Maximum speed the rescuer can reach")]
    public float maxSpeed = 5f;

    [Header("The force to stop after the touch is not happened")]
    public float stopThreshold = 0.05f;

    Rigidbody2D rb;
    bool wasPushedThisStep;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Physics callbacks run in the fixed step; use collision contacts to detect pushing
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider == null) 
        { 
            return; 
        }
        if (!collision.collider.CompareTag("Player")) 
        { 
            return; 
        }

        Rigidbody2D playerRb = collision.collider.attachedRigidbody;
        if (playerRb == null)
        {
            return;
        }

        // Examine all contacts; if player's velocity has a positive component towards the rescuer, treat as pushing
        foreach (var contact in collision.contacts)
        {
            Vector2 contactNormal = contact.normal;               // points from other collider into this collider
            Vector2 pushDir = -contactNormal.normalized;          // direction from player into rescuer

            // Project player's velocity onto push direction to see if player is moving into the rescuer
            float playerSpeedAlong = Vector2.Dot(playerRb.linearVelocity, pushDir);

            const float minPushSpeed = 0.1f;                       // small deadzone to avoid tiny pushes

            if (playerSpeedAlong > minPushSpeed)
            {
                // Apply force proportional to how fast the player is moving into the rescuer
                Vector2 force = pushDir * playerSpeedAlong * pushForce * Time.fixedDeltaTime;
                rb.AddForce(force, ForceMode2D.Force);
                wasPushedThisStep = true;
            }
        }
    }

    private void FixedUpdate()
    {
        // Clamp maximum speed so it never gets out of control
        if (rb.linearVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        // If nobody pushed this step, gently damp the object (simulate friction)
        if (!wasPushedThisStep)
        {
            // simple damping toward zero
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 6f * Time.fixedDeltaTime);

            if (rb.linearVelocity.magnitude < stopThreshold)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }

        // Reset for next physics step
        wasPushedThisStep = false;
    }
}