using UnityEngine;
using System.Collections.Generic;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance { get; private set; }

    [Header("Question Pools")]
    [SerializeField] private QuestionEntry[] easyQuestions;
    [SerializeField] private QuestionEntry[] mediumQuestions;
    [SerializeField] private QuestionEntry[] hardQuestions;

    [Header("Icons")]
    [SerializeField] private ItemIconBank iconBank;   // ScriptableObject with Sprite[] answerSprites

    private QuestionEntry[] activeQuestions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Uncomment if you want one global QuestionManager:
            // DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // -----------------------------------------------------------
    // Helpers
    // -----------------------------------------------------------

    private void EnsureActiveQuestions()
    {
        if (activeQuestions != null && activeQuestions.Length > 0)
            return;

        GameDifficulty difficulty = GameDifficulty.Easy;

        if (DifficultyManager.Instance != null)
            difficulty = DifficultyManager.Instance.CurrentDifficulty;
        else
            Debug.LogWarning("[QuestionManager] No DifficultyManager, defaulting to Easy.");

        QuestionEntry[] chosen = null;
        switch (difficulty)
        {
            case GameDifficulty.Easy:   chosen = easyQuestions;   break;
            case GameDifficulty.Medium: chosen = mediumQuestions; break;
            case GameDifficulty.Hard:   chosen = hardQuestions;   break;
        }

        // Fallback to any non-empty pool
        if (chosen == null || chosen.Length == 0)
        {
            if (easyQuestions != null && easyQuestions.Length > 0)       chosen = easyQuestions;
            else if (mediumQuestions != null && mediumQuestions.Length > 0) chosen = mediumQuestions;
            else if (hardQuestions != null && hardQuestions.Length > 0)    chosen = hardQuestions;
        }

        activeQuestions = chosen;

        if (activeQuestions == null || activeQuestions.Length == 0)
            Debug.LogError("[QuestionManager] No questions set up for ANY difficulty.");
        else
            Debug.Log("[QuestionManager] Using " + activeQuestions.Length +
                      " questions for difficulty: " + difficulty);
    }

    private List<Sprite> GetShuffledIcons()
    {
        if (iconBank == null || iconBank.answerSprites == null || iconBank.answerSprites.Length == 0)
        {
            Debug.LogError("[QuestionManager] IconBank is missing or empty.");
            return new List<Sprite>();
        }

        var list = new List<Sprite>(iconBank.answerSprites);

        // Fisher–Yates shuffle
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }

        return list;
    }

    private HashSet<Item> ApplyAnswersAndIcons(QuestionEntry q, List<Sprite> icons)
    {
        var changedItems = new HashSet<Item>();
        int iconIndex = 0;

        // Helper for safe icon fetch
        Sprite GetNextIcon()
        {
            if (icons == null || icons.Count == 0) return null;
            if (iconIndex >= icons.Count) return null;
            return icons[iconIndex++];
        }

        // Correct item
        if (q.correctItem != null)
        {
            q.correctItem.displayName = q.correctAnswerText;

            Sprite icon = GetNextIcon();
            if (icon != null)
                q.correctItem.icon = icon;

            changedItems.Add(q.correctItem);
        }

        // Wrong items
        if (q.wrongItems != null)
        {
            for (int i = 0; i < q.wrongItems.Length; i++)
            {
                Item wrong = q.wrongItems[i];
                if (wrong == null) continue;

                if (q.wrongAnswerTexts != null && i < q.wrongAnswerTexts.Length)
                    wrong.displayName = q.wrongAnswerTexts[i];

                Sprite icon = GetNextIcon();
                if (icon != null)
                    wrong.icon = icon;

                changedItems.Add(wrong);
            }
        }

        return changedItems;
    }

    private void UpdatePickupsForItems(HashSet<Item> changedItems)
    {
        foreach (var pickup in ItemPickup.ActivePickups)
        {
            if (pickup == null || pickup.itemData == null) continue;
            if (changedItems.Contains(pickup.itemData))
                pickup.RefreshSprite();
        }
    }

    // -----------------------------------------------------------
    // Public Functions Used in other classes
    // -----------------------------------------------------------

    public QuestionEntry GetRandomQuestion()
    {
        EnsureActiveQuestions();

        if (activeQuestions == null || activeQuestions.Length == 0)
        {
            Debug.LogError("[QuestionManager] activeQuestions is empty! Returning null.");
            return null;
        }

        // Choose question
        int idx = Random.Range(0, activeQuestions.Length);
        QuestionEntry q = activeQuestions[idx];

        // Prepare unique icons for this question
        List<Sprite> shuffledIcons = GetShuffledIcons();

        // Apply displayName + unique icons to items, track which changed
        HashSet<Item> changedItems = ApplyAnswersAndIcons(q, shuffledIcons);

        // Sync all active pickups using these items
        UpdatePickupsForItems(changedItems);

        Debug.Log("[QuestionManager] Assigned texts & unique icons for question: " + q.questionText);
        return q;
    }
}
