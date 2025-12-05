using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverMenu : MonoBehaviour
{
    public void restart()
    {
        SceneManager.LoadSceneAsync("level 1");
    }
    public void Main_menu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
