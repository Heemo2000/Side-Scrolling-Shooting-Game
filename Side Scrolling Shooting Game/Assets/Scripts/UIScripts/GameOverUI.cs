using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameOverUI : MonoBehaviour
{
    [SerializeField]private TMP_Text gameOverScoreText;
    
    private void Start() {
        ShowScoreAfterGameOver();
    }
    private void ShowScoreAfterGameOver()
    {
        gameOverScoreText.text = ScoreManager.Instance.CurrentScore.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(StringHolder.GameplaySceneName);
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
