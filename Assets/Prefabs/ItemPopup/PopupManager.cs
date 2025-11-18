using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    [SerializeField] private Camera cam;
    [SerializeField] private GameObject popup_prefab;

    void Awake()
    {
        // Simple singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Call this from ItemPickup
    public void ShowPopup(Vector3 worldPosition, string text)
    {
        GameObject popup = Instantiate(popup_prefab, worldPosition, Quaternion.identity);
        popup.GetComponent<Popup>().text_value = text;
    }

    void Update()
    {
        // TURN OFF MOUSE POPUP CREATION
        // So popups ONLY appear on interact
    }
}
