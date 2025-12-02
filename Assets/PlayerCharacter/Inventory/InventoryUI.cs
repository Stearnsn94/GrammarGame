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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (feedbackText != null)
            feedbackText.text = "";

        Refresh();
    }

    // ----------------- Feedback helpers -----------------
    private IEnumerator HideFeedbackAfterDelay(float delay)
    {
        // Realtime so it still works when Time.timeScale == 0
        yield return new WaitForSecondsRealtime(delay);

        if (feedbackText != null)
            feedbackText.text = "";
    }

    private void ShowFeedbackTemporary(string message, float duration = 2f)
    {
        if (feedbackText == null)
            return;

        feedbackText.text = message;

        if (hideRoutine != null)
            StopCoroutine(hideRoutine);

        hideRoutine = StartCoroutine(HideFeedbackAfterDelay(duration));
    }

    // ----------------- Inventory rebuild -----------------
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

        // Create a button for each item
        foreach (Item item in player.inventory)
        {
            GameObject buttonObj = Instantiate(itemButtonPrefab, itemListRoot);

            // 1) ICON: use the Image on the ItemButton itself (root)
            Image iconImage = buttonObj.GetComponent<Image>();
            if (iconImage != null)
            {
                // This will override the temporary sprite set on the prefab
                iconImage.sprite = item.icon;
                iconImage.enabled = (item.icon != null);

                Debug.Log($"[InventoryUI] Setting icon for {item.displayName} to {(item.icon != null ? item.icon.name : "NULL")}");
            }
            else
            {
                Debug.LogWarning("[InventoryUI] ItemButton prefab has no Image on the root object.");
            }

            // 2) LABEL: find a TMP_Text in children and set the name
            TMP_Text label = buttonObj.GetComponentInChildren<TMP_Text>();
            if (label != null)
            {
                label.text = item.displayName;
            }
            else
            {
                Debug.LogWarning("InventoryUI.Refresh: No TMP_Text found on itemButtonPrefab.");
            }

            // 3) CLICK HANDLER
            Button btn = buttonObj.GetComponent<Button>();
            Item capturedItem = item;
            if (btn != null)
            {
                btn.onClick.AddListener(() => TrySelectItem(capturedItem));
            }
        }
    }

    // Door question flow
    public void BeginDoorSelection(Door door)
    {
        currentDoor = door;
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

        // Unpause after answer (right or wrong)
        Time.timeScale = 1f;

        // Update inventory (e.g., if the key is removed)
        Refresh();
    }

    public void ShowQuestion(string questionText)
    {
        Debug.Log("[InventoryUI] ShowQuestion: " + questionText);
        if (feedbackText != null)
            feedbackText.text = questionText;
    }
}
