using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance { get; private set; }

    [SerializeField] private QuestionEntry[] questions;

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

        int idx = Random.Range(0, questions.Length);
        QuestionEntry q = questions[idx];

        // Set correct item text
        if (q.correctItem != null)
            Debug.Log("Correct item has been selected");
            q.correctItem.displayName = q.correctAnswerText;

        // Set wrong item texts
        if (q.wrongItems != null && q.wrongAnswerTexts != null)
        {
            for (int i = 0; i < q.wrongItems.Length && i < q.wrongAnswerTexts.Length; i++)
            {
                if (q.wrongItems[i] != null)
                    q.wrongItems[i].displayName = q.wrongAnswerTexts[i];
            }
        }

        Debug.Log("[QuestionManager] Assigned answer texts to items for question: " + q.questionText);
        return q;
    }
}
