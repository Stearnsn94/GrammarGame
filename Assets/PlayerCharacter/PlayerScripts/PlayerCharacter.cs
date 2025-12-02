using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    // Simple inventory list
    public List<Item> inventory = new List<Item>();

    [Header("Movement (optional)")]
    public float moveSpeed = 4f;
    private Rigidbody2D rb;

    // The interactable currently in range
    private Interactable currentInteractable;
    // The spriteRenderer to update which direction it looks
    private SpriteRenderer spriteRenderer;

    [Header("Health")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int currentHealth;
    [Header("UI")]
    [SerializeField] private Slider healthSlider;
    [Header("Sprites (Cardinal Directions)")]
    private Animator animator;

    private Vector2 moveInput;
    private Vector2 lastMoveDirection = Vector2.down;  // default facing down

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        if(healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        HandleMovement();
        HandleAnimation();
        HandleInteractInput();
    }

    // Movement (optional)
    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 input = new Vector2(x, y).normalized;
        moveInput = input;
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

    private void HandleAnimation()
    {
        Vector2 animDir = GetAnimDirection(moveInput);
        bool isMoving = animDir.sqrMagnitude > 0.01f;

        if (isMoving)
        {
            lastMoveDirection = animDir;
        }

        animator.SetBool("IsMoving", isMoving);
        // Current movement direction (cardinal)
        animator.SetFloat("MoveX", animDir.x);
        animator.SetFloat("MoveY", animDir.y);
        // Last facing direction for idle
        animator.SetFloat("LastMoveX", lastMoveDirection.x);
        animator.SetFloat("LastMoveY", lastMoveDirection.y);
    }

    private Vector2 GetAnimDirection(Vector2 input)
    {
        if (input.sqrMagnitude < 0.01f)
            return Vector2.zero;

        // Decide whether horizontal or vertical dominates
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            // Horizontal
            return new Vector2(Mathf.Sign(input.x), 0f);
        }
        else
        {
            // Vertical
            return new Vector2(0f, Mathf.Sign(input.y));
        }
    }


    // Interact (E key)
    private void HandleInteractInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact(this);

            if (InteractionPromptUI.Instance != null)
            {
                InteractionPromptUI.Instance.Hide();
            }
        }
    }

    // Called by ItemPickup when we collect an item
    public void Collect(Item item)
    {
        if (item == null) return;

        if (!inventory.Contains(item))
        {
            inventory.Add(item);
        }

        Debug.Log("Collected item: " + item.displayName);

        // Update the inventory panel if it exists
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.Refresh();
        }
    }

    // --- Health / Damage ---

    public void DrainHealth(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        Debug.Log("Player took damage. Current health: " + currentHealth);

        // Update the slider UI
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // TODO: you can add respawn or game-over behavior here if desired.
        // For now, we just log it.
        if (currentHealth == 0)
        {
            Debug.Log("Player died (no respawn implemented yet).");
        }
    }

    // Detect interactables via trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        var interactObj = other.GetComponent<Interactable>();
        if (interactObj != null)
        {
            currentInteractable = interactObj;

            if (InteractionPromptUI.Instance != null)
            {
                InteractionPromptUI.Instance.Show("Press E to interact");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var interactObj = other.GetComponent<Interactable>();
        if (interactObj != null && interactObj == currentInteractable)
        {
            currentInteractable = null;

            if (InteractionPromptUI.Instance != null)
            {
                InteractionPromptUI.Instance.Hide();
            }
        }
    }
}
