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
    [SerializeField] private ItemIconBank iconBank;   // holds Sprite[] answerSprites
// hello
    private QuestionEntry[] activeQuestions;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Pick the proper pool for the current difficulty
    private void EnsureActiveQuestions()
    {

        GameDifficulty difficulty = GameDifficulty.Easy;
        if (activeQuestions != null && activeQuestions.Length > 0)
            return;

        

        if (DifficultyManager.Instance != null)
        {
            difficulty = DifficultyManager.Instance.CurrentDifficulty;
        }

        switch (difficulty)
        {
            case GameDifficulty.Easy:
                activeQuestions = easyQuestions;
                break;
            case GameDifficulty.Medium:
                activeQuestions = mediumQuestions;
                break;
            case GameDifficulty.Hard:
                activeQuestions = hardQuestions;
                break;
        }

        if (activeQuestions == null || activeQuestions.Length == 0)
        {
            Debug.LogError("[QuestionManager] No questions set up for difficulty: " + difficulty);
        }
        else
        {
            Debug.Log("[QuestionManager] Using " + activeQuestions.Length +
                      " questions for difficulty: " + difficulty);
        }
    }

    public QuestionEntry GetRandomQuestion()
    {
        EnsureActiveQuestions();

        if (activeQuestions == null || activeQuestions.Length == 0)
        {
            Debug.LogError("[QuestionManager] activeQuestions is empty! Returning null.");
            return null;
        }

        if (iconBank == null || iconBank.answerSprites == null || iconBank.answerSprites.Length == 0)
        {
            Debug.LogError("[QuestionManager] IconBank is missing or empty!");
            // we can still return a question, but icons won't randomize
        }

        int idx = Random.Range(0, activeQuestions.Length);
        QuestionEntry q = activeQuestions[idx];

        // Track which Items had their icons/text changed
        HashSet<Item> changedItems = new HashSet<Item>();

        // Correct answer
        if (q.correctItem != null)
        {
            q.correctItem.displayName = q.correctAnswerText;

            if (iconBank != null && iconBank.answerSprites.Length > 0)
            {
                q.correctItem.icon = GetRandomIcon();
            }

            changedItems.Add(q.correctItem);
        }

        // Wrong answers
        if (q.wrongItems != null)
        {
            for (int i = 0; i < q.wrongItems.Length; i++)
            {
                Item wrongItem = q.wrongItems[i];
                if (wrongItem == null) continue;

                if (q.wrongAnswerTexts != null && i < q.wrongAnswerTexts.Length)
                {
                    wrongItem.displayName = q.wrongAnswerTexts[i];
                }

                if (iconBank != null && iconBank.answerSprites.Length > 0)
                {
                    wrongItem.icon = GetRandomIcon();
                }

                changedItems.Add(wrongItem);
            }
        }

        // Update ALL active pickups using these Items so the world matches
        foreach (var pickup in ItemPickup.ActivePickups)
        {
            if (pickup != null && pickup.itemData != null && changedItems.Contains(pickup.itemData))
            {
                pickup.RefreshSprite();
            }
        }

        Debug.Log("[QuestionManager] Assigned texts & icons for question: " + q.questionText);
        return q;
    }

    private Sprite GetRandomIcon()
    {
        int index = Random.Range(0, iconBank.answerSprites.Length);
        return iconBank.answerSprites[index];
    }
}
