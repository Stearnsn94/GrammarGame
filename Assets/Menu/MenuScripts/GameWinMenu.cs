using UnityEngine;
using UnityEngine.SceneManagement;
public class GameWinMenu : MonoBehaviour
{
    public void Main_menu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
