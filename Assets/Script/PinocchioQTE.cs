using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PinocchioQTE : MonoBehaviour
{
    public static PinocchioQTE Instance;

    [Header("UI Referansları")]
    public GameObject qtePanel;
    public Slider qteSlider;
    public TextMeshProUGUI instructionText;
    public Image greenZone; 

    private bool isQTEActive = false;
    private float currentPenalty;

    private void Awake() { Instance = this; if(qtePanel != null) qtePanel.SetActive(false); }

    public void TriggerRandomEvent(float penalty)
    {
        if (isQTEActive) return;
        currentPenalty = penalty;
        
        // Bar ne kadar doluysa oyun o kadar hızlanır
        float difficulty = 1f + (TensionManager.Instance.currentTension / 100f);
        
        int eventIndex = Random.Range(1, 7); // 1'den 6'ya kadar rastgele seç
        StopAllCoroutines();

        switch (eventIndex)
        {
            case 1: StartCoroutine(HiccupEvent(difficulty)); break;
            case 2: StartCoroutine(StutterEvent(difficulty)); break;
            case 3: StartCoroutine(SweatEvent(difficulty)); break;
            case 4: StartCoroutine(TwitchEvent(difficulty)); break;
            case 5: StartCoroutine(BreathEvent(difficulty)); break;
            case 6: StartCoroutine(TrembleEvent(difficulty)); break;
        }
    }

    // 1. HIÇKIRIK: Yeşil bölgede Space'e bas
    IEnumerator HiccupEvent(float d) {
        Setup("HIÇKIRIK! [SPACE] ZAMANLA", true);
        float target = Random.Range(0.3f, 0.7f);
        greenZone.rectTransform.anchorMin = new Vector2(target - 0.07f, 0);
        greenZone.rectTransform.anchorMax = new Vector2(target + 0.07f, 1);
        while (qteSlider.value < 1f) {
            qteSlider.value += Time.deltaTime * (1.2f * d);
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (Mathf.Abs(qteSlider.value - target) < 0.08f) Success(); else Fail();
                yield break;
            }
            yield return null;
        }
        Fail();
    }

    // 2. KEKELEME: 5-8 kez hızlıca 'E' bas
    IEnumerator StutterEvent(float d) {
        int req = Mathf.RoundToInt(5 * d); int cur = 0;
        Setup($"KEKELEME! {req} KEZ 'E' BAS", false);
        while (qteSlider.value < 1f) {
            qteSlider.value += Time.deltaTime * 0.4f;
            if (Input.GetKeyDown(KeyCode.E)) {
                cur++;
                qteSlider.value -= 0.1f; // Her basışta barı biraz geri çeker (zaman kazandırır)
            }
            if (cur >= req) { Success(); yield break; }
            yield return null;
        }
        Fail();
    }

    // 3. TERLEME: Fareyi hızlıca sağa sola salla
    IEnumerator SweatEvent(float d) {
        Setup("TERİ SİL! FAREYİ SALLA", false);
        float progress = 0; Vector3 lastPos = Input.mousePosition;
        while (progress < 1f) {
            float dist = Vector3.Distance(Input.mousePosition, lastPos);
            if (dist > 50f) progress += 0.15f / d;
            lastPos = Input.mousePosition;
            qteSlider.value = progress;
            yield return new WaitForSeconds(0.05f);
        }
        Success();
    }

    // 4. GÖZ SEYİRMESİ: Fareyi ekranın ortasında tut
    IEnumerator TwitchEvent(float d) {
        Setup("GÖZÜNÜ SABİTLE! FAREYİ ORTADA TUT", false);
        float focus = 0; Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
        while (focus < 2f) {
            if (Vector2.Distance(Input.mousePosition, center) < 200f / d) focus += Time.deltaTime;
            else focus = 0;
            qteSlider.value = focus / 2f;
            yield return null;
        }
        Success();
    }

    // 5. NEFES DARLIĞI: Space tuşuna basılı tut, bar dolunca bırak
    IEnumerator BreathEvent(float d) {
        Setup("NEFESİNİ TUT! [SPACE] BASILI TUT", false);
        float hold = 0; float target = 1.5f * d;
        while (hold < target) {
            if (Input.GetKey(KeyCode.Space)) hold += Time.deltaTime;
            else hold = 0;
            qteSlider.value = hold / target;
            yield return null;
        }
        Success();
    }

    // 6. TİTREME: A ve D ile ibreyi ortada tut
    IEnumerator TrembleEvent(float d) {
        Setup("TİTREME! [A-D] DENGEDE KAL", true);
        float balance = 0.5f; float timer = 3f;
        greenZone.rectTransform.anchorMin = new Vector2(0.4f, 0);
        greenZone.rectTransform.anchorMax = new Vector2(0.6f, 1);
        while (timer > 0) {
            timer -= Time.deltaTime;
            balance += Random.Range(-0.4f, 0.4f) * d * Time.deltaTime * 5f;
            if (Input.GetKey(KeyCode.A)) balance -= 0.5f * Time.deltaTime;
            if (Input.GetKey(KeyCode.D)) balance += 0.5f * Time.deltaTime;
            qteSlider.value = balance;
            if (balance < 0.05f || balance > 0.95f) { Fail(); yield break; }
            yield return null;
        }
        Success();
    }

    void Setup(string t, bool g) {
        isQTEActive = true;
        qtePanel.SetActive(true);
        instructionText.text = t;
        qteSlider.value = 0;
        greenZone.gameObject.SetActive(g);
    }

    void Success() {
        isQTEActive = false;
        qtePanel.SetActive(false);
        TensionManager.Instance.IncreaseTension(3f); // Başarıda çok az artar
    }

    void Fail() {
        isQTEActive = false;
        qtePanel.SetActive(false);
        TensionManager.Instance.IncreaseTension(currentPenalty);
        PinocchioManager.Instance.TriggerReaction(); // Hıçkırık/Kekeleme yazısı ve Flash
    }
}