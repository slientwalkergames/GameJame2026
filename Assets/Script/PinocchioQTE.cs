using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PinocchioQTE : MonoBehaviour {
    public static PinocchioQTE Instance;

    [Header("UI Referansları")]
    public GameObject qtePanel;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI instructionText;
    public Image semptomIcon;
    public Image greenZone;
    public RectTransform needle;
    public RectTransform twitchTarget;
    public CanvasGroup sweatOverlay;

    [Header("Assetler (0:Hiccup, 1:Stutter, 2:Sweat, 3:Twitch, 4:Breath, 5:Tremble)")]
    public Sprite[] symptomSprites;
    public AudioClip[] startSounds;
    public AudioSource audioSource;

    private bool isQTEActive = false;
    private float currentPenalty;

    private void Awake() { Instance = this; qtePanel.SetActive(false); }

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
        ResetUI();

        // Assetleri Yükle
        semptomIcon.sprite = symptomSprites[index];
        if (audioSource && startSounds[index]) audioSource.PlayOneShot(startSounds[index]);

        float timer = 3.0f / d; // Zorluğa göre süre (3 saniye baz)
        
        while (timer > 0) {
            timer -= Time.deltaTime;
            countdownText.text = timer.ToString("F1");
            countdownText.color = timer < 1f ? Color.red : Color.white;

            switch (index) {
                case 0: // HIÇKIRIK (Zamanlama)
                    instructionText.text = "TAM ZAMANINDA [SPACE]";
                    float move = Mathf.PingPong(Time.time * 2f * d, 1f);
                    needle.anchorMin = new Vector2(move, 0.5f);
                    needle.anchorMax = new Vector2(move, 0.5f);
                    if (Input.GetKeyDown(KeyCode.Space)) {
                        if (move > 0.4f && move < 0.6f) Success(); else Fail();
                        yield break;
                    }
                    break;

                case 1: // KEKELEME (Hızlı Basma)
                    instructionText.text = "KELİMEYİ TOPLA [E]";
                    if (Input.GetKeyDown(KeyCode.E)) { Success(); yield break; }
                    break;

                case 2: // TERLEME (Fare Sallama)
                    instructionText.text = "TERİ SİL [FARE SALLA]";
                    sweatOverlay.gameObject.SetActive(true);
                    if (Input.GetAxis("Mouse X") > 0.8f || Input.GetAxis("Mouse Y") > 0.8f) { Success(); yield break; }
                    break;

                case 3: // GÖZ SEYİRMESİ (Odak)
                    instructionText.text = "HEDEFE TIKLA";
                    twitchTarget.gameObject.SetActive(true);
                    // Hedef rastgele kaçsın
                    twitchTarget.anchoredPosition = new Vector2(Mathf.Sin(Time.time * 5f) * 200f, Mathf.Cos(Time.time * 3f) * 100f);
                    if (Input.GetMouseButtonDown(0)) {
                        if (Vector2.Distance(Input.mousePosition, twitchTarget.position) < 100f) { Success(); yield break; }
                    }
                    break;

                case 4: // NEFES (Tutma)
                    instructionText.text = "NEFESİNİ TUT [SPACE BASILI]";
                    if (Input.GetKey(KeyCode.Space) && timer < 0.2f) { Success(); yield break; }
                    break;

                case 5: // TİTREME (Denge)
                    instructionText.text = "DENGEDE KAL [A-D]";
                    greenZone.gameObject.SetActive(true);
                    float balance = Mathf.PingPong(Time.time * d, 1f);
                    needle.anchorMin = new Vector2(balance, 0.5f);
                    needle.anchorMax = new Vector2(balance, 0.5f);
                    if (Input.GetKey(KeyCode.A)) balance -= 0.1f;
                    if (Input.GetKey(KeyCode.D)) balance += 0.1f;
                    if (timer < 0.1f) { Success(); yield break; }
                    break;
            }
            yield return null;
        }
        Fail();
    }

    void ResetUI() {
        greenZone.gameObject.SetActive(false);
        needle.gameObject.SetActive(true);
        twitchTarget.gameObject.SetActive(false);
        sweatOverlay.gameObject.SetActive(false);
        sweatOverlay.alpha = 1;
    }

    void Success() { qtePanel.SetActive(false); isQTEActive = false; TensionManager.Instance.IncreaseTension(2f); }
    void Fail() { qtePanel.SetActive(false); isQTEActive = false; TensionManager.Instance.IncreaseTension(currentPenalty); PinocchioManager.Instance.TriggerReaction(); }
}