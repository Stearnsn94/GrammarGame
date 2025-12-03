using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class ItemPickup : MonoBehaviour, Interactable
{
    // All active pickups in the scene
    public static readonly List<ItemPickup> ActivePickups = new List<ItemPickup>();

    public Item itemData;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        ActivePickups.Add(this);
        RefreshSprite();   // ensure it matches current itemData.icon
    }

    private void OnDisable()
    {
        ActivePickups.Remove(this);
    }

    // Called when icons change on the Item
    public void RefreshSprite()
    {
        if (spriteRenderer != null && itemData != null)
        {
            spriteRenderer.sprite = itemData.icon;
        }
    }

    public void Interact(PlayerCharacter player)
    {
        if (itemData == null) return;

        // Optional popup
        if (PopupManager.Instance != null)
        {
            PopupManager.Instance.ShowPopup(transform.position, itemData.displayName);
        }

        player.Collect(itemData);

        Destroy(gameObject);
    }
}
