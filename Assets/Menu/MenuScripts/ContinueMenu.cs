using UnityEngine;
using UnityEngine.SceneManagement;
public class ContinueLevel : MonoBehaviour
{
    public void ContinueToNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        
        SceneManager.LoadScene(currentIndex+1);
    }

    public void Main_menu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
