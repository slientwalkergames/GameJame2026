using UnityEngine;
using UnityEngine.UI; // UI için gerekli

public class TensionManager : MonoBehaviour
{
    public static TensionManager Instance; // Her yerden kolayca ulaşabilmek için Singleton yapıyoruz

    [Header("Tension Settings")]
    public float currentTension = 0f;
    public float maxTension = 100f;

    [Header("UI References")]
    public Slider tensionSlider; // Gerilim barı UI
    public Image screenVignette; // Yalan söyleyince ekranın kızarması (Noir efekt)

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateUI();
    }

    // Yalan söylendiğinde çağrılacak fonksiyon
    public void IncreaseTension(float amount)
    {
        currentTension += amount;
        currentTension = Mathf.Clamp(currentTension, 0, maxTension);
        UpdateUI();

        // Eğer bar dolarsa Kırılma (Breakdown) yaşanacak
        if (currentTension >= maxTension)
        {
            TriggerBreakdown();
        }
    }

    private void UpdateUI()
    {
        if (tensionSlider != null)
            tensionSlider.value = currentTension / maxTension;

        // Vinyet (Kenar kızarması) efekti - Gerilim arttıkça kırmızılaşır
        if (screenVignette != null)
        {
            Color c = screenVignette.color;
            c.a = currentTension / maxTension; // Şeffaflığı gerilime göre artar
            screenVignette.color = c;
        }
    }

    private void TriggerBreakdown()
    {
        Debug.Log("KARAKTER KIRILDI! Game Over veya QTE Başarısızlığı.");
        // Buraya yakalanma / animasyon kodları gelecek
    }
}