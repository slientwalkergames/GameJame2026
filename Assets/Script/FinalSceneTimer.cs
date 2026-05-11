using UnityEngine;
using UnityEngine.SceneManagement; // Sahne yönetimi için şart

public class FinalSceneTimer : MonoBehaviour
{
    [Header("Ayarlar")]
    public float waitTime = 5f;          // Kaç saniye beklesin? (Örn: 5 saniye)
    public string targetSceneName = "GmeOver"; // Gidilecek sahnenin adı

    void Start()
    {
        // Belirlenen süre sonunda sahne geçiş fonksiyonunu çağır
        Invoke("ChangeScene", waitTime);
    }

    void ChangeScene()
    {
        // Sahne ismi boş değilse geçiş yap
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("HATA: Gidilecek sahne adı yazılmamış!");
        }
    }
}