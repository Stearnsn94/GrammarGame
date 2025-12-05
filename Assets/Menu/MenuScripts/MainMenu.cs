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

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
