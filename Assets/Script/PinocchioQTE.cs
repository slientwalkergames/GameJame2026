using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PinocchioQTE : MonoBehaviour {
    public static PinocchioQTE Instance;

    [System.Serializable]
    public struct SemptomAyari {
        public string semptomAdi; // Inspector'da kolay yönetim için
        public Sprite ikon;       // El çizimi görselin
        [TextArea(3, 10)] 
        public string talimatMesaji; // Buraya yazdığın metin ekranda çıkacak
        public AudioClip baslangicSesi;
    }

    [Header("QTE Durum Ayarları")]
    public SemptomAyari[] semptomListesi = new SemptomAyari[6];

    [Header("UI Referansları")]
    public GameObject qtePanel;
    public TextMeshProUGUI instructionText; // Ekranda metni gösterecek olan obje
    public Image semptomIcon; 
    public TextMeshProUGUI countdownText;

    private bool isQTEActive = false;
    private float currentPenalty;

    private void Awake() { 
        Instance = this; 
        if(qtePanel) qtePanel.SetActive(false); 
    }

    public void TriggerRandomEvent(float penalty) {
        if (isQTEActive) return;
        currentPenalty = penalty;
        
        float diff = 1f;
        if(TensionManager.Instance != null) {
            diff = 1f + (TensionManager.Instance.currentTension / 100f);
        }

        int index = Random.Range(0, 6); 
        StopAllCoroutines();
        StartCoroutine(RunQTE(index, diff));
    }

    IEnumerator RunQTE(int index, float d) {
        isQTEActive = true;
        qtePanel.SetActive(true);
        
        // ZORLAMA: Paneli hiyerarşide en alta (en öne) al
        qtePanel.transform.SetAsLastSibling(); 

        if (index < semptomListesi.Length) {
    SemptomAyari ayar = semptomListesi[index];
    
    if(semptomIcon) semptomIcon.sprite = ayar.ikon;
    
    if(instructionText) {
        // 1. ÖNCE AYARLARI YAP
        instructionText.enableWordWrapping = false; // Yazıyı yanlara yayar
        instructionText.enableAutoSizing = false;   // Küçük kalmasını engellemek için bunu kapattık
        instructionText.fontSize = 50;              // Yazı boyutunu buraya elinle gir (Örn: 50)
        instructionText.alignment = TextAlignmentOptions.Center;

        // 2. MESAJI YAZ
        instructionText.text = ayar.talimatMesaji;
        instructionText.gameObject.SetActive(true);
    }
    
    AudioSource audio = GetComponent<AudioSource>();
    if (audio && ayar.baslangicSesi) audio.PlayOneShot(ayar.baslangicSesi);
}
        
        StartCoroutine(AnimateIcon(index));

        float timer = 3.0f / d;
        while (timer > 0) {
            timer -= Time.deltaTime;
            if(countdownText) countdownText.text = timer.ToString("F1");

            HandleMechanics(index);
            yield return null;
        }
        Fail();
    }

    void HandleMechanics(int index) {
        // Tuş kontrolleri
        switch (index) {
            case 0: if (Input.GetKeyDown(KeyCode.Space)) Success(); break;
            case 1: if (Input.GetKeyDown(KeyCode.E)) Success(); break;
            case 2: if (Mathf.Abs(Input.GetAxis("Mouse X")) > 0.7f) Success(); break;
            case 3: if (Input.GetMouseButtonDown(0)) Success(); break;
            case 4: if (Input.GetKey(KeyCode.Space)) Success(); break; 
            case 5: if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) Success(); break;
        }
    }

    IEnumerator AnimateIcon(int index) {
        if(!semptomIcon) yield break;
        RectTransform rect = semptomIcon.GetComponent<RectTransform>();
        Vector2 startPos = rect.anchoredPosition; 

        while (isQTEActive) {
            if (index == 5 || index == 1) // Sarsıntı
                rect.anchoredPosition = startPos + Random.insideUnitCircle * 8f;
            yield return null;
        }
        rect.anchoredPosition = startPos;
    }

    void Success() { 
        isQTEActive = false; 
        qtePanel.SetActive(false); 
        if(TensionManager.Instance) TensionManager.Instance.IncreaseTension(3f); 
        StopAllCoroutines(); 
    }

    void Fail() { 
        isQTEActive = false; 
        qtePanel.SetActive(false); 
        if(TensionManager.Instance) TensionManager.Instance.IncreaseTension(currentPenalty); 
        if(PinocchioManager.Instance) PinocchioManager.Instance.TriggerReaction(); 
        StopAllCoroutines(); 
    }
}