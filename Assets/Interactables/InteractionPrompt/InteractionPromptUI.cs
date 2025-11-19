using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    public static InteractionPromptUI Instance { get; private set; }

    [SerializeField] private GameObject rootObject;
    [SerializeField] private TMP_Text promptText;

    private void Awake()
    {
        Instance = this;
        rootObject.SetActive(false); // hide on start
    }

    public void Show(string text)
    {
        promptText.text = text;
        rootObject.SetActive(true);
    }

    public void Hide()
    {
        rootObject.SetActive(false);
    }
}
