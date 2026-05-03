using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverTrigger : MonoBehaviour 
{
    [Header("Sahne Ayarları")]
    public string gameOverSceneName = "GmeOver"; // Inspector'dan sahne adını yaz

    public void GoToGameOver() 
    {
        // Game Over sahnesine geçiş yap
        SceneManager.LoadScene(gameOverSceneName);
    }
}