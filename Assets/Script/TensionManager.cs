using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Sahne geçişi için gerekli

public class TensionManager : MonoBehaviour
{
    public static TensionManager Instance;

    [Header("Gerilim Ayarları")]
    public float currentTension = 0f;
    public float maxTension = 100f;
    public string gameOverSceneName = "GameOver"; // Buraya Unity'deki sahne adını yaz

    [Header("UI Referansları")]
    public Slider tensionSlider; // Gerilim barı (Slider)
    public Image screenVignette; // Ekranın kenarlarındaki kırmızı görsel

    private void Awake()
    {
        // Singleton yapısı: Diğer scriptlerin bu scripti bulmasını sağlar
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Başlangıçta barı ve ekranı sıfırla
        UpdateUI();
    }

    // Yalan söylendiğinde veya QTE başarısız olduğunda bu fonksiyon çağrılır
    public void IncreaseTension(float amount)
    {
        currentTension += amount;
        currentTension = Mathf.Clamp(currentTension, 0, maxTension); // 0-100 arasında tut
        
        UpdateUI();

        // KRİTİK NOKTA: Bar dolduğunda Game Over sahnesine git
        if (currentTension >= maxTension)
        {
            TriggerGameOver();
        }
    }

    private void UpdateUI()
    {
        // Slider'ı güncelle (0 ile 1 arası değer alır)
        if (tensionSlider != null)
            tensionSlider.value = currentTension / maxTension;

        // Ekranın kırmızılığını güncelle
        if (screenVignette != null)
        {
            Color c = screenVignette.color;
            c.a = currentTension / maxTension; // Gerilim arttıkça Alpha (görünürlük) artar
            screenVignette.color = c;
        }
    }

    private void TriggerGameOver()
    {
        Debug.Log("Bar Doldu! Game Over sahnesi yükleniyor...");
        
        // Eğer sahne ismi boş değilse o sahneye git
        if (!string.IsNullOrEmpty(gameOverSceneName))
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
        else
        {
            Debug.LogError("HATA: TensionManager'da 'Game Over Scene Name' kısmına bir isim yazmadın!");
        }
    }
}