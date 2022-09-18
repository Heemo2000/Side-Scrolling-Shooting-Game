using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuScreenUI : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(StringHolder.GameplaySceneName);   
    }

    public void Exit()
    {
        Application.Quit();
    }
}
