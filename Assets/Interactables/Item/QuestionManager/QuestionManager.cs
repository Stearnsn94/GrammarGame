using UnityEngine;
using System.Collections.Generic;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance { get; private set; }

    [SerializeField] private QuestionEntry[] questions;
    [SerializeField] private ItemIconBank iconBank;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public QuestionEntry GetRandomQuestion()
    {
        if (questions == null || questions.Length == 0)
        {
            Debug.LogError("[QuestionManager] No questions configured!");
            return null;
        }

        if (iconBank == null || iconBank.answerSprites == null || iconBank.answerSprites.Length == 0)
        {
            Debug.LogError("[QuestionManager] IconBank is missing or empty!");
            return null;
        }

        int idx = Random.Range(0, questions.Length);
        QuestionEntry q = questions[idx];

        // Track which items had their icons changed
        HashSet<Item> changedItems = new HashSet<Item>();

        // --- Correct answer: set text + random icon ---
        if (q.correctItem != null)
        {
            q.correctItem.displayName = q.correctAnswerText;
            q.correctItem.icon = GetRandomIcon();
            changedItems.Add(q.correctItem);
        }

        // --- Wrong answers: set text + random icons ---
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

                wrongItem.icon = GetRandomIcon();
                changedItems.Add(wrongItem);
            }
        }

        // --- Update all active pickups for changed items ---
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
        int iconIndex = Random.Range(0, iconBank.answerSprites.Length);
        return iconBank.answerSprites[iconIndex];
    }
}
