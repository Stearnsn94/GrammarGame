using UnityEngine;
using System.Collections.Generic;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance { get; private set; }

    [Header("Question Pools")]
    [SerializeField] private QuestionEntry[] easyQuestions;
    [SerializeField] private QuestionEntry[] mediumQuestions;
    [SerializeField] private QuestionEntry[] hardQuestions;

    [SerializeField] private ItemIconBank iconBank;

    private QuestionEntry[] activeQuestions;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Pick which list to use, if we haven't already
    private void EnsureActiveQuestions()
    {
        if (activeQuestions != null && activeQuestions.Length > 0)
            return;

        GameDifficulty difficulty = GameDifficulty.Easy;

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
            Debug.Log("[QuestionManager] Using " + activeQuestions.Length + " questions for difficulty: " + difficulty);
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
            // We can still return a question, just without icons
        }

        int idx = Random.Range(0, activeQuestions.Length);
        QuestionEntry q = activeQuestions[idx];

        // --- Correct answer: set text (icon randomized if you’re still using that logic) ---
        if (q.correctItem != null)
        {
            q.correctItem.displayName = q.correctAnswerText;
            // if you want random icons, do it here:
            if (iconBank != null && iconBank.answerSprites.Length > 0)
                 q.correctItem.icon = iconBank.answerSprites[Random.Range(0, iconBank.answerSprites.Length)];
        }

        // --- Wrong answers: set text (and optionally icons) ---
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

                // if using random icons:
                if (iconBank != null && iconBank.answerSprites.Length > 0)
                     wrongItem.icon = iconBank.answerSprites[Random.Range(0, iconBank.answerSprites.Length)];
            }
        }

        Debug.Log("[QuestionManager] Selected question: " + q.questionText);
        return q;
    }
}
