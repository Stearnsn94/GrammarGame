using UnityEngine;

public class ItemPickup : MonoBehaviour, Interactable
{
    public Item itemData;

    public void Interact(PlayerCharacter player)
    {
        // Show popup above item BEFORE destroying it
        PopupManager.Instance.ShowPopup(transform.position, itemData.displayName);
        player.Collect(itemData);
        Destroy(gameObject);
    }
}
