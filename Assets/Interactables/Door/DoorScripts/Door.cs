using UnityEngine;

public class Door : MonoBehaviour, Interactable
{
    public bool isLocked = true;
    public Item requiredItem;
    public Collider2D doorCollider;
    public SpriteRenderer doorSprite;

    private void Reset()
    {
        doorCollider = GetComponent<Collider2D>();
        doorSprite = GetComponent<SpriteRenderer>();
    }

    public void Interact(PlayerCharacter player)
    {
        if (!isLocked)
        {
            Debug.Log("Door already open.");
            return;
        }

        InventoryUI.Instance.BeginDoorSelection(this);
    }

    public string GetPromptText()
    {
        return isLocked ? "Try Door" : "Enter Door";
    }

    public bool TryUseItem(Item item, PlayerCharacter player)
    {
        if (item == requiredItem)
        {
            isLocked = false;
            OpenDoor();

            player.inventory.Remove(item);
            InventoryUI.Instance.Refresh();

            return true;
        }

        return false;
    }

    private void OpenDoor()
    {
        if (doorCollider != null)
            doorCollider.enabled = false;

        if (doorSprite != null)
            doorSprite.color = Color.gray; // optional visual change
    }
}
