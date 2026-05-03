using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class PinocchioQTE : MonoBehaviour {
    public static PinocchioQTE Instance;

    [Header("UI Referansları")]
    public GameObject qtePanel;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI instructionText;
    public Image semptomIcon; 
    public Image greenZone;
    public RectTransform needle;

    [Header("Assetler")]
    public Sprite[] symptomSprites; 
    public AudioClip[] startSounds;
    public AudioSource audioSource;

    private bool isQTEActive = false;
    private float currentPenalty;
    private string pendingSceneName;

    private void Awake() { Instance = this; if(qtePanel) qtePanel.SetActive(false); }

    public void TriggerRandomEvent(float penalty, string nextScene = "") {
        if (isQTEActive) return;
        currentPenalty = penalty;
        pendingSceneName = nextScene;
        
        float diff = 1f + (TensionManager.Instance.currentTension / 100f);
        int index = Random.Range(0, 6); 
        StopAllCoroutines();
        StartCoroutine(RunQTE(index, diff));
    }

    IEnumerator RunQTE(int index, float d) {
        isQTEActive = true;
        qtePanel.SetActive(true);
        
        // GÖRSELLERİ VE METİNLERİ ZORLA AÇ
        if(instructionText) instructionText.gameObject.SetActive(true);
        if(countdownText) countdownText.gameObject.SetActive(true);
        if(semptomIcon) semptomIcon.gameObject.SetActive(true);
        
        ResetUI(index); // İndekse göre özel hazırlık yap

        if (symptomSprites.Length > index) semptomIcon.sprite = symptomSprites[index];
        if (audioSource && startSounds.Length > index && startSounds[index] != null) 
            audioSource.PlayOneShot(startSounds[index]);
        
        float timer = 3.0f / d;
        while (timer > 0) {
            timer -= Time.deltaTime;
            if(countdownText) countdownText.text = timer.ToString("F1");
            
            HandleMechanics(index, d, timer);
            yield return null;
        }
        Fail();
    }

    void HandleMechanics(int index, float d, float timer) {
        // Her karede metinlerin doğruluğunu garanti et
        switch (index) {
            case 0: 
                instructionText.text = "HIÇKIRIĞI YUT!\n[SPACE]"; 
                float m = Mathf.PingPong(Time.time * 2f * d, 1f);
                if(needle) {
                    needle.gameObject.SetActive(true);
                    needle.anchorMin = new Vector2(m, 0.5f); 
                    needle.anchorMax = new Vector2(m, 0.5f);
                }
                if (Input.GetKeyDown(KeyCode.Space)) { if (m > 0.4f && m < 0.6f) Success(); else Fail(); } 
                break;
            case 1: 
                instructionText.text = "KELİMELERİ TOPLA!\n[E] BAS"; 
                if (Input.GetKeyDown(KeyCode.E)) Success(); 
                break;
            case 2: 
                instructionText.text = "TERİ SİL!\n[FARE SALLA]"; 
                if (Mathf.Abs(Input.GetAxis("Mouse X")) > 0.8f) Success(); 
                break;
            case 3: 
                instructionText.text = "BAKIŞINI SABİTLE!\n[TIKLA]"; 
                if (Input.GetMouseButtonDown(0)) Success(); 
                break;
            case 4: 
                instructionText.text = "NEFESİNİ TUT!\n[SPACE BASILI]"; 
                if (Input.GetKey(KeyCode.Space) && timer < 0.2f) Success(); 
                break;
            case 5: 
                instructionText.text = "ELLERİNİ TUT!\n[A-D]"; 
                if(greenZone) greenZone.gameObject.SetActive(true); 
                if (timer < 0.1f) Success(); 
                break;
        }
    }

    void Success() { FinishQTE(3f); }
    void Fail() { if(PinocchioManager.Instance) PinocchioManager.Instance.TriggerReaction(); FinishQTE(currentPenalty); }

    void FinishQTE(float tensionAmount) {
        isQTEActive = false;
        qtePanel.SetActive(false);
        TensionManager.Instance.IncreaseTension(tensionAmount);
        
        if (!string.IsNullOrEmpty(pendingSceneName)) {
            SceneManager.LoadScene(pendingSceneName);
        }
        StopAllCoroutines();
    }

    void ResetUI(int index) {
        if(greenZone) greenZone.gameObject.SetActive(false);
        if(needle) needle.gameObject.SetActive(false);
        // Sadece hıçkırık ve titreme için iğneyi açacağız
        if(index == 0 || index == 5) if(needle) needle.gameObject.SetActive(true);
    }
}