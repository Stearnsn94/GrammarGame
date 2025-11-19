using UnityEngine;
using System.Collections.Generic;


public class PlayerCharacter : MonoBehaviour
{
    // Simple inventory list
    public List<Item> inventory = new List<Item>();

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

    //Movement (optional)
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

    //Interact (E key)
    private void HandleInteractInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact(this);
            InteractionPromptUI.Instance.Hide();
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


    // Detect interactables via trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        var interactObj = other.GetComponent<Interactable>();
        if (interactObj != null)
        {
            currentInteractable = interactObj;
            InteractionPromptUI.Instance.Show("Press E to interact");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var interactObj = other.GetComponent<Interactable>();
        if (interactObj != null && interactObj == currentInteractable)
        {
            currentInteractable = null;
            InteractionPromptUI.Instance.Hide();
        }
    }
}
