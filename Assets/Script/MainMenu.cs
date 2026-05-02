using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Ayarlar")]
    public string gameSceneName; // Buraya Unity'deki sahne adını yazacaksın

    public void StartGame()
    {
        // Eğer sahne adı boş değilse o sahneye git
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogError("HATA: Game Scene Name kısmına sahne adını yazmadın!");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Oyundan çıkıldı.");
        Application.Quit();
    }
}