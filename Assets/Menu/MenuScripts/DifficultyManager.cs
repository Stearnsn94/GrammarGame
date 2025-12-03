using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    public GameDifficulty CurrentDifficulty { get; private set; } = GameDifficulty.Easy;

    private void Awake()
    {
        // Singleton + persist across scene loads
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetDifficulty(GameDifficulty difficulty)
    {
        CurrentDifficulty = difficulty;
        Debug.Log("[DifficultyManager] Difficulty set to: " + difficulty);
    }

    // Overload for UI buttons with int parameter
    public void SetDifficultyByIndex(int index)
    {
        CurrentDifficulty = (GameDifficulty)index;
        Debug.Log("[DifficultyManager] Difficulty set to (index): " + CurrentDifficulty);
    }
}
