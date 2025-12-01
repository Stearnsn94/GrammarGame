using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class main_menu : MonoBehaviour
{
    private diffculty_manager diff_mang;

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void ExitGame() 
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
