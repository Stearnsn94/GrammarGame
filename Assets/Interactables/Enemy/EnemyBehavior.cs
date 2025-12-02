using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class EnemyKnockback : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private float damageCooldown = 1f; // seconds between hits

    [Header("Knockback Settings")]
    [SerializeField] private float knockbackForce = 5f;

    [Header("Patrol Settings")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private Transform[] patrolPoints;

    [Header("Chase Settings")]
    [SerializeField] private float chaseSpeed = 3.5f;

    [Header("Vision (Cone Detector)")]
    [SerializeField] private float viewDistance = 5f;      // how far enemy can see
    [SerializeField] private float viewAngle = 60f;        // cone angle in degrees
    [SerializeField] private LayerMask obstacleMask;       // optional: walls, etc.

    [Header("Sprites (Cardinal Directions)")]
    [SerializeField] private Sprite spriteUp;
    [SerializeField] private Sprite spriteDown;
    [SerializeField] private Sprite spriteLeft;
    [SerializeField] private Sprite spriteRight;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float nextDamageTime = 0f;
    private int currentPatrolIndex = 0;
    private PlayerCharacter player;

    private enum State { Patrolling, Chasing }
    private State currentState = State.Patrolling;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true; // trigger is used for "too close" damage

        // SpriteRenderer may be on this object or a child
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        Debug.Log("[ENEMY] Awake. Knockback + patrol + vision enemy on: " + gameObject.name);
    }

    private void Start()
    {
        // Find the player once
        player = FindObjectOfType<PlayerCharacter>();
        if (player == null)
        {
            Debug.LogWarning("[ENEMY] No PlayerCharacter found in scene.");
        }
    }

    private void Update()
    {
        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        bool canSeePlayer = CanSeePlayer();

        // State switching
        if (canSeePlayer)
        {
            currentState = State.Chasing;
        }
        else if (currentState == State.Chasing && !canSeePlayer)
        {
            // Lost sight of the player → go back to patrol
            currentState = State.Patrolling;
        }

        switch (currentState)
        {
            case State.Patrolling:
                HandlePatrol();
                break;
            case State.Chasing:
                HandleChase();
                break;
        }
    }

    private void HandlePatrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            rb.linearVelocity = Vector2.zero;
            UpdateSpriteFacing(rb.linearVelocity);
            return;
        }

        Transform target = patrolPoints[currentPatrolIndex];
        if (target == null)
        {
            rb.linearVelocity = Vector2.zero;
            UpdateSpriteFacing(rb.linearVelocity);
            return;
        }

        Vector2 toTarget = (Vector2)target.position - rb.position;
        Vector2 dir = toTarget.normalized;

        rb.linearVelocity = dir * patrolSpeed;

        // Update sprite based on movement direction
        UpdateSpriteFacing(rb.linearVelocity);

        // If close enough to this point, advance to the next
        if (toTarget.magnitude < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    private void HandleChase()
    {
        Vector2 toPlayer = (Vector2)player.transform.position - rb.position;
        Vector2 dir = toPlayer.normalized;

        rb.linearVelocity = dir * chaseSpeed;

        // Update sprite based on chase direction
        UpdateSpriteFacing(rb.linearVelocity);
    }

    // Set sprite based on cardinal movement direction
    private void UpdateSpriteFacing(Vector2 velocity)
    {
        if (spriteRenderer == null)
            return;

        // Don't change sprite when barely moving
        if (velocity.sqrMagnitude < 0.01f)
            return;

        // Horizontal movement dominates?
        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
        {
            if (velocity.x > 0f && spriteRight != null)
                spriteRenderer.sprite = spriteRight;
            else if (velocity.x < 0f && spriteLeft != null)
                spriteRenderer.sprite = spriteLeft;
        }
        else // Vertical movement dominates
        {
            if (velocity.y > 0f && spriteUp != null)
                spriteRenderer.sprite = spriteUp;
            else if (velocity.y < 0f && spriteDown != null)
                spriteRenderer.sprite = spriteDown;
        }
    }

    // Cone-based vision, using movement direction as "forward"
    private bool CanSeePlayer()
    {
        Vector2 toPlayer = (Vector2)player.transform.position - rb.position;
        float distanceToPlayer = toPlayer.magnitude;

        // Too far
        if (distanceToPlayer > viewDistance)
            return false;

        // Determine forward direction: prefer movement, fallback to +X
        Vector2 forward = rb.linearVelocity.normalized;
        if (forward.sqrMagnitude < 0.01f)
        {
            forward = Vector2.right; // default facing if not moving
        }

        float angleToPlayer = Vector2.Angle(forward, toPlayer.normalized);
        if (angleToPlayer > viewAngle * 0.5f)
            return false;

        // Optional: raycast to check if something blocks view
        if (obstacleMask.value != 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(rb.position, toPlayer.normalized, distanceToPlayer, obstacleMask);
            if (hit.collider != null)
            {
                // A wall/obstacle blocked line of sight
                return false;
            }
        }

        return true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // This trigger is for "too close" damage/knockback, NOT for vision
        PlayerCharacter hitPlayer = other.GetComponent<PlayerCharacter>();
        if (hitPlayer == null)
            hitPlayer = other.GetComponentInParent<PlayerCharacter>();

        if (hitPlayer == null) return;

        // Cooldown between hits
        if (Time.time < nextDamageTime) return;

        Debug.Log("[ENEMY] Damaging player now.");
        hitPlayer.DrainHealth(damageAmount);

        // Knock enemy away from the player
        Vector2 directionAway = (rb.position - (Vector2)hitPlayer.transform.position).normalized;
        rb.AddForce(directionAway * knockbackForce, ForceMode2D.Impulse);

        nextDamageTime = Time.time + damageCooldown;
    }

#if UNITY_EDITOR
    // Draw vision cone in the editor for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 origin = transform.position;

        // Use current velocity as forward; if none, default to +X
        Vector2 vel = Vector2.zero;
        Rigidbody2D rbEditor = GetComponent<Rigidbody2D>();
        if (rbEditor != null)
        {
            vel = rbEditor.linearVelocity;
        }

        Vector2 forward2D = vel.normalized;
        if (forward2D.sqrMagnitude < 0.01f)
        {
            forward2D = Vector2.right;
        }

        Vector3 forward = new Vector3(forward2D.x, forward2D.y, 0f);

        float halfAngle = viewAngle * 0.5f;
        Quaternion leftRot = Quaternion.AngleAxis(-halfAngle, Vector3.forward);
        Quaternion rightRot = Quaternion.AngleAxis(halfAngle, Vector3.forward);

        Vector3 leftDir = leftRot * forward * viewDistance;
        Vector3 rightDir = rightRot * forward * viewDistance;

        Gizmos.DrawLine(origin, origin + leftDir);
        Gizmos.DrawLine(origin, origin + rightDir);
    }
#endif
}
