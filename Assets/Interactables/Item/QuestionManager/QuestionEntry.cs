using UnityEngine;

[System.Serializable]
public class QuestionEntry
{
    [TextArea]
    public string questionText;

    [Header("Correct")]
    public Item correctItem;
    public string correctAnswerText;

    [Header("Wrong")]
    public Item[] wrongItems;        // you can have 1 or many wrongs
    public string[] wrongAnswerTexts; // must match wrongItems length
}
