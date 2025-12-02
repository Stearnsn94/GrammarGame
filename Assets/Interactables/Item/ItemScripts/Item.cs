using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item")]
public class Item : ScriptableObject
{
    public string itemId;
    public string displayName; // This is dynamic and will be changed at runtime
}
