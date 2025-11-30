using UnityEngine;

public interface Interactable
{
    // Called when the player presses the interact button
    void Interact(PlayerCharacter player);

    public bool Is_collided() { return false; }
}
