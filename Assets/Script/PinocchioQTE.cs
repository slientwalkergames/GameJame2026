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
    public RectTransform twitchTarget;
    public CanvasGroup sweatOverlay;

    [Header("Assetler (Sırasıyla 6 Adet)")]
    public Sprite[] symptomSprites; 
    public AudioClip[] startSounds;
    public AudioSource audioSource;

    private bool isQTEActive = false;
    private float currentPenalty;
    private string pendingScene;

    private void Awake() { Instance = this; if(qtePanel) qtePanel.SetActive(false); }

    public void TriggerRandomEvent(float penalty, string nextScene = "") {
        if (isQTEActive) return;
        currentPenalty = penalty;
        pendingScene = nextScene;
        float diff = 1f + (TensionManager.Instance.currentTension / 100f);
        int index = Random.Range(0, 6); 
        StopAllCoroutines();
        StartCoroutine(RunQTE(index, diff));
    }

    IEnumerator RunQTE(int index, float d) {
        isQTEActive = true;
        qtePanel.SetActive(true);
        ResetUI(index);

        if (symptomSprites.Length > index) semptomIcon.sprite = symptomSprites[index];
        if (audioSource && startSounds.Length > index) audioSource.PlayOneShot(startSounds[index]);
        
        float timer = 3.0f / d;
        while (timer > 0) {
            timer -= Time.deltaTime;
            countdownText.text = timer.ToString("F1");
            HandleMechanics(index, d, timer);
            yield return null;
        }
        Fail();
    }

    void HandleMechanics(int index, float d, float timer) {
        switch (index) {
            case 0: instructionText.text = "HIÇKIRIĞI YUT! [SPACE]"; 
                float m = Mathf.PingPong(Time.time * 2f * d, 1f);
                needle.anchorMin = new Vector2(m, 0.5f); needle.anchorMax = new Vector2(m, 0.5f);
                if (Input.GetKeyDown(KeyCode.Space)) { if (m > 0.4f && m < 0.6f) Success(); else Fail(); } break;
            case 1: instructionText.text = "KELİMELERİ TOPLA! [E]"; if (Input.GetKeyDown(KeyCode.E)) Success(); break;
            case 2: instructionText.text = "TERİ SİL! [FARE SALLA]"; if (Mathf.Abs(Input.GetAxis("Mouse X")) > 0.8f) Success(); break;
            case 3: instructionText.text = "BAKIŞINI SABİTLE! [TIKLA]"; 
                twitchTarget.anchoredPosition = new Vector2(Mathf.Sin(Time.time * 4f) * 150f, Mathf.Cos(Time.time * 2f) * 80f);
                if (Input.GetMouseButtonDown(0)) { if (Vector2.Distance(Input.mousePosition, twitchTarget.position) < 120f) Success(); } break;
            case 4: instructionText.text = "NEFESİNİ TUT! [SPACE BASILI]"; if (Input.GetKey(KeyCode.Space) && timer < 0.2f) Success(); break;
            case 5: instructionText.text = "ELLERİNİ TUT! [A-D]"; greenZone.gameObject.SetActive(true); if (timer < 0.1f) Success(); break;
        }
    }

    void Success() { FinishQTE(3f); }
    void Fail() { PinocchioManager.Instance.TriggerReaction(); FinishQTE(currentPenalty); }

    void FinishQTE(float tensionAmount) {
        isQTEActive = false;
        qtePanel.SetActive(false);
        TensionManager.Instance.IncreaseTension(tensionAmount);
        if (!string.IsNullOrEmpty(pendingScene)) SceneManager.LoadScene(pendingScene);
        StopAllCoroutines();
    }

    void ResetUI(int index) {
        greenZone.gameObject.SetActive(index == 5);
        needle.gameObject.SetActive(index == 0 || index == 5);
        twitchTarget.gameObject.SetActive(index == 3);
        sweatOverlay.gameObject.SetActive(index == 2);
        instructionText.gameObject.SetActive(true);
        semptomIcon.gameObject.SetActive(true);
    }
}