using UnityEngine;

public class EndingManager : MonoBehaviour
{
    public static EndingManager Instance;

    [Header("Oyun Sonu Ekranları")]
    public GameObject lieEndingPanel;   // Yalan söylerse açılacak ekran (Kötü Son)
    public GameObject truthEndingPanel; // Doğruyu söylerse açılacak ekran (İyi Son)

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Oyun başlarken son ekranlarını gizle
        if (lieEndingPanel != null) lieEndingPanel.SetActive(false);
        if (truthEndingPanel != null) truthEndingPanel.SetActive(false);
    }

    public void TriggerEnding(bool isLie)
    {
        // Bütün arayüzleri kapat (Diyalog, Gerilim barı vs.)
        DialogueManager.Instance.dialoguePanel.SetActive(false);
        TensionManager.Instance.gameObject.SetActive(false);

        if (isLie)
        {
            // YALAN SONU (Sahte Masumiyet)
            lieEndingPanel.SetActive(true);
            Debug.Log("SON: Yalan söylendi. Karakter bir karalamaya dönüştü.");
        }
        else
        {
            // GERÇEK SONU (Suçun Kabulü)
            truthEndingPanel.SetActive(true);
            Debug.Log("SON: Gerçek söylendi. Karakter insan olarak öldü.");
        }
    }
}