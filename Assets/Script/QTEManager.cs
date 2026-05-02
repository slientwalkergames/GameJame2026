using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QTEManager : MonoBehaviour
{
    public static QTEManager Instance;

    [Header("UI Referansları")]
    public GameObject qtePanel; // QTE ekranı
    public Slider timerSlider; // Kalan süreyi gösteren bar
    public TextMeshProUGUI qteText; // "HIZLI BAS!" yazısı

    [Header("Ayarlar")]
    public float qteDuration = 1.5f; // Oyuncunun tuşa basmak için 1.5 saniyesi var
    private float currentTime;
    private bool isQTEActive = false;
    private float pendingTension; // Başarısız olursa eklenecek gerilim miktarı

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        qtePanel.SetActive(false); // Oyun başlarken QTE ekranı gizli olsun
    }

    public void StartQTE(float tensionPenalty)
    {
        pendingTension = tensionPenalty;
        currentTime = qteDuration;
        isQTEActive = true;
        
        qtePanel.SetActive(true);
        
        // TEST SATIRLARI:
        if (timerSlider == null) Debug.LogError("SLIDER EKSİK!");
        else Debug.Log("Slider bulundu, Max Value: " + qteDuration);

        timerSlider.maxValue = qteDuration;
        timerSlider.value = qteDuration;
        qteText.text = "YALANI GİZLE! [BOŞLUK] TUŞUNA BAS!";
    }

    private void Update()
    {
        if (!isQTEActive) return;

        // Süre geriye akıyor
        currentTime -= Time.deltaTime;
        timerSlider.value = currentTime;

        // BAŞARI DURUMU: Süre bitmeden Space (Boşluk) tuşuna basarsa
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SuccessQTE();
        }
        // BAŞARISIZLIK DURUMU: Süre biterse
        else if (currentTime <= 0)
        {
            FailQTE();
        }
    }

    private void SuccessQTE()
    {
        isQTEActive = false;
        qtePanel.SetActive(false);
        Debug.Log("QTE BAŞARILI! Yalan gizlendi.");
        
        // Başarılı olursa gerilim çok az artar (Örn: %20'si kadar)
        TensionManager.Instance.IncreaseTension(pendingTension * 0.2f); 
    }

    private void FailQTE()
    {
        isQTEActive = false;
        qtePanel.SetActive(false);
        Debug.Log("QTE BAŞARISIZ! Mürekkep krizi başlıyor...");
        
        // QTE başarısız olursa mürekkep krizini başlat!
        InkManager.Instance.StartInkEvent();
        
        TensionManager.Instance.IncreaseTension(pendingTension);
    }
}