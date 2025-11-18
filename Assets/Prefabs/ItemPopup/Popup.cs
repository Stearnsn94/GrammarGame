using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public string text_value;
    void Start()
    {
        text.text = text_value;
        Destroy(gameObject, 1.5f);
    }

}
