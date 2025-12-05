using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, Interactable
{
    [Header("Lock State")]
    public bool isLocked = true;

    [Header("Door Components")]
    [SerializeField] private Collider2D doorCollider;

    [SerializeField] private QuestionManager questionManager;

    [SerializeField] private SpriteRenderer doorSprite;

    // The item that is the correct answer for THIS interaction
    private Item requiredItem;

    // The question currently active on this door interaction
    private QuestionEntry currentQuestion;

    private void Awake()
    {
        currentQuestion = questionManager.GetRandomQuestion(); 
    }

    private void Reset()
    {
        doorCollider = GetComponent<Collider2D>();
        doorSprite = GetComponent<SpriteRenderer>();
    }

    public void Interact(PlayerCharacter player)
    {
        if (!isLocked)
        {
            Debug.Log("[Door] Door already open.");
            return;
        }

        if (questionManager == null)
        {
            Debug.LogError("[Door] No QuestionManager.Instance in scene!");
            return;
        }

        // 1) Get a random question
        
        if (currentQuestion == null)
        {
            Debug.LogError("[Door] QuestionManager returned null question.");
            return;
        }

        // This is the item that counts as the correct answer
        requiredItem = currentQuestion.correctItem;

        // if player has no items, don't pause / don't ask
        if (player.inventory.Count == 0)
        {
            if (InventoryUI.Instance != null)
            {
                InventoryUI.Instance.ShowQuestion("You have no items to try on this door.");
            }
            return;
        }

        // Rebuild inventory so button labels use UPDATED item.displayName
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.Refresh();
            InventoryUI.Instance.ShowQuestion(currentQuestion.questionText);
            InventoryUI.Instance.BeginDoorSelection(this);
        }

        // 4) Pause the game while the player chooses an answer
        Time.timeScale = 0f;
    }

    public string GetPromptText()
    {
        return isLocked ? "Try Door" : "Enter Door";
    }

    // Called by InventoryUI when the player clicks an item as their answer
    public bool TryUseItem(Item item, PlayerCharacter player)
    {
        if (!isLocked)
            return true;

        if (item == requiredItem)
        {
            // CORRECT ANSWER
            isLocked = false;
            OpenDoor();

            // Optional: consume the item
            if (player.inventory.Contains(item))
            {
                player.inventory.Remove(item);
                InventoryUI.Instance.Refresh();
            }

            // Unpause
            Time.timeScale = 1f;

            return true;
        }

        // WRONG
        return false;
    }

    private void OpenDoor()
    {
        if (doorCollider != null)
            doorCollider.enabled = false;

        if (doorSprite != null)
            doorSprite.color = Color.gray;

        Debug.Log("[Door] Door opened.");
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(currentIndex+1);
    }
}
