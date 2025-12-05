using UnityEngine;
using UnityEngine.SceneManagement;

public class main_menu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("level 1");
    }

    public void StartGameWithDifficulty(int difficultyIndex)
    {
        if (DifficultyManager.Instance != null)
        {
            DifficultyManager.Instance.SetDifficultyByIndex(difficultyIndex);
        }
    }

}
