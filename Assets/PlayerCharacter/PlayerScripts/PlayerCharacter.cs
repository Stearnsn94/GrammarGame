using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Movement (optional)")]
    public float moveSpeed = 4f;
    private Rigidbody2D rb;

    // The interactable currently in range
    private Interactable currentInteractable;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractInput();
    }

    // --- Movement (optional) ---
    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 input = new Vector2(x, y).normalized;
        if (rb != null)
        {
            rb.linearVelocity = input * moveSpeed;
        }
        else
        {
            // Fallback: move transform directly if no Rigidbody2D
            transform.position += (Vector3)(input * moveSpeed * Time.deltaTime);
        }
    }

    // --- Interact (E key) ---
    private void HandleInteractInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact(this);
        }
    }

    // Called by ItemPickup when we collect an item
    public void Collect(Item item)
    {
        if (item == null) return;

        Debug.Log("Collected item: " + item.displayName);
        // later: add to inventory list
    }

    // Detect interactables via trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        var interactObj = other.GetComponent<Interactable>();
        if (interactObj != null)
        {
            currentInteractable = interactObj;
            // later: show "Press E" UI
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var interactObj = other.GetComponent<Interactable>();
        if (interactObj != null && interactObj == currentInteractable)
        {
            currentInteractable = null;
            // later: hide "Press E" UI
        }
    }
}
