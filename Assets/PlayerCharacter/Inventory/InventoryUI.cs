using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [Header("References")]
    [SerializeField] private PlayerCharacter player;
    [SerializeField] private Transform itemListRoot;
    [SerializeField] private GameObject itemButtonPrefab;
    [SerializeField] private TMP_Text feedbackText;

    private Door currentDoor;

    private Coroutine hideRoutine;
    private IEnumerator HideFeedbackAfterDelay(float delay)
    {
        // Wait
        yield return new WaitForSecondsRealtime(delay);  // Realtime so it works even when game is paused

        // Clear text
        feedbackText.text = "";
    }

    private void ShowFeedbackTemporary(string message, float duration = 2f)
    {
        feedbackText.text = message;

        // Cancel old coroutine if text was already on screen
        if (hideRoutine != null)
            StopCoroutine(hideRoutine);

        // Start new hide timer
        hideRoutine = StartCoroutine(HideFeedbackAfterDelay(duration));
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        feedbackText.text = "";
        Refresh();
    }

    public void Refresh()
    {
        if (player == null || itemListRoot == null)
        {
            Debug.LogWarning("InventoryUI.Refresh: player or itemListRoot is null");
            return;
        }

        Debug.Log("InventoryUI.Refresh: inventory count = " + player.inventory.Count);

        // Clear old buttons
        foreach (Transform child in itemListRoot)
        {
            Destroy(child.gameObject);
        }

        // Add a button for every item in inventory
        foreach (Item item in player.inventory)
        {
            GameObject buttonObj = Instantiate(itemButtonPrefab, itemListRoot);

            TMP_Text label = buttonObj.GetComponentInChildren<TMP_Text>();
            if (label != null)
            {
                label.text = item.displayName;   // show the item name in the inventory
            }
            else
            {
                Debug.LogWarning("InventoryUI.Refresh: No TMP_Text found on itemButtonPrefab.");
            }

            // Hook up click event
            Button btn = buttonObj.GetComponent<Button>();
            Item capturedItem = item; // capture for closure
            if (btn != null)
            {
                btn.onClick.AddListener(() => TrySelectItem(capturedItem));
            }
        }
    }

    public void BeginDoorSelection(Door door)
{
    currentDoor = door;

    // If the player has NO items → don’t pause
    if (player.inventory.Count == 0)
    {
            ShowFeedbackTemporary("You have no items to try on this door.");
        return;
    }

    // Player HAS items → pause and show question
    feedbackText.text = "The subject in which you find out the solution to a problem is ____.";
    Time.timeScale = 0f;
}


    private void TrySelectItem(Item item)
    {
        if (item == null) return;

        if (currentDoor == null)
        {
            ShowFeedbackTemporary("Selected: " + item.displayName);
            return;
        }

        bool success = currentDoor.TryUseItem(item, player);

        if (success)
        {
            ShowFeedbackTemporary("Correct item! The door opens.");
            currentDoor = null;
        }
        else
        {
            ShowFeedbackTemporary("Wrong item.");
        }

        // Unpause the game after making a choice (right OR wrong)
        Time.timeScale = 1f;

        // Rebuild the buttons in case you removed the key from inventory
        Refresh();
    }
}
