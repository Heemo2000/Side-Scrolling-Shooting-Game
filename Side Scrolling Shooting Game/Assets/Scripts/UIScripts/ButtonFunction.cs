using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunction : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(StringHolder.GameplaySceneName);   
    }
    public void RestartGame()
    {
        Play();
    }
    public void BackToMain()
    {
        SceneManager.LoadScene(StringHolder.MainMenuSceneName);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
