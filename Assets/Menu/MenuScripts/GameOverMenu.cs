using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverMenu : MonoBehaviour
{
    public void restart()
    {
        int lastLevelIndex = PlayerPrefs.GetInt("LastLevelIndex", 1);
        
        SceneManager.LoadSceneAsync(lastLevelIndex);
    }
    public void Main_menu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
