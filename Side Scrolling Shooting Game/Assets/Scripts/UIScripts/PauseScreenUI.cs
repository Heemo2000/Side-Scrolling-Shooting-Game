using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseScreenUI : MonoBehaviour
{
    
    public void BackToMain()
    {
        SceneManager.LoadScene(StringHolder.MainMenuSceneName);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
