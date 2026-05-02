using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PinocchioQTE : MonoBehaviour {
    public static PinocchioQTE Instance;

    [Header("UI Referansları")]
    public GameObject qtePanel;
    public Image timerImage;      // Süre barı (Filled)
    public Image semptomIcon;     // Ortadaki büyük ikon
    public TextMeshProUGUI instructionText;

    [Header("Ses ve Görsel Assetler (Sıralı)")]
    [Header("0:Hiccup, 1:Stutter, 2:Sweat, 3:Twitch, 4:Breath, 5:Tremble")]
    public AudioSource audioSource;
    public AudioClip[] symptomStartSounds; // Oyun başladığında çalan ses
    public Sprite[] symptomSprites;        // Ortada görünecek resim

    private bool isQTEActive = false;
    private float currentPenalty;

    private void Awake() { Instance = this; if(qtePanel) qtePanel.SetActive(false); }

    public void TriggerRandomEvent(float penalty) {
        if (isQTEActive) return;
        currentPenalty = penalty;
        float diff = 1f + (TensionManager.Instance.currentTension / 100f);
        int index = Random.Range(0, 6); 
        StopAllCoroutines();
        StartCoroutine(RunQTE(index, diff));
    }

    IEnumerator RunQTE(int index, float d) {
        isQTEActive = true;
        qtePanel.SetActive(true);

        // --- ASSET ATAMALARI ---
        if(semptomIcon && symptomSprites.Length > index) 
            semptomIcon.sprite = symptomSprites[index];
        
        if (audioSource && symptomStartSounds.Length > index && symptomStartSounds[index] != null) 
            audioSource.PlayOneShot(symptomStartSounds[index]);

        float timer = 2.0f / d;
        float maxTime = timer;

        while (timer > 0) {
            timer -= Time.deltaTime;
            if(timerImage) timerImage.fillAmount = timer / maxTime;

            switch (index) {
                case 0: instructionText.text = "HIÇKIRIĞI YUT! [SPACE]"; if (Input.GetKeyDown(KeyCode.Space)) { Success(); yield break; } break;
                case 1: instructionText.text = "KELİMELERİ TOPLA! [E]"; if (Input.GetKeyDown(KeyCode.E)) { Success(); yield break; } break;
                case 2: instructionText.text = "TERİ SİL! [FARE SALLA]"; if (Input.GetAxis("Mouse X") > 0.5f) { Success(); yield break; } break;
                case 3: instructionText.text = "GÖZÜNÜ SABİTLE! [SOL TIK]"; if (Input.GetMouseButtonDown(0)) { Success(); yield break; } break;
                case 4: instructionText.text = "NEFESİNİ TUT! [SPACE BASILI]"; if (Input.GetKey(KeyCode.Space) && timer < 0.2f) { Success(); yield break; } break;
                case 5: instructionText.text = "ELLERİNİ TUT! [A veya D]"; if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) { Success(); yield break; } break;
            }
            yield return null;
        }
        Fail();
    }

    void Success() { qtePanel.SetActive(false); isQTEActive = false; TensionManager.Instance.IncreaseTension(2f); }
    void Fail() { qtePanel.SetActive(false); isQTEActive = false; TensionManager.Instance.IncreaseTension(currentPenalty); PinocchioManager.Instance.TriggerReaction(); }
}