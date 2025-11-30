using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;


public class PlayerCharacter : MonoBehaviour
{
    // Simple inventory list
    public List<Item> inventory = new List<Item>();

    [Header("Movement (optional)")]
    public float moveSpeed = 4f;
    private Rigidbody2D rb;

    // The interactable currently in range
    private Interactable currentInteractable;

    //players health
    [Header("players healht")]
    private int player_health = 100; // health should be increments of 3

    //to get the players position
    public Vector2 get_players_location() { return rb.position; }

    private Interactable currentInteractable2;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Used AI for this because I dont know Unity/C# logging convention
        Debug.Log($"Player's HEALTH: ({player_health})");
        Debug.Log($"Player's current location: ({get_players_location().x}, {get_players_location().y})");
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

    public int drain_health(int drain_amount)
    {
        return player_health - drain_amount;
    }
}
