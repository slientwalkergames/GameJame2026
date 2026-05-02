using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PinocchioManager : MonoBehaviour
{
    public static PinocchioManager Instance;
    public TextMeshProUGUI reactionText;
    public Image flashFilter; 

    private void Awake() { 
        Instance = this; 
        if(reactionText != null) reactionText.gameObject.SetActive(false);
        if(flashFilter != null) flashFilter.gameObject.SetActive(false);
    }

    public void TriggerReaction() { 
        if (reactionText != null) StartCoroutine(ShowReaction()); 
        else Debug.LogWarning("PinocchioManager: Reaction Text atanmamış!");
    }

    IEnumerator ShowReaction() {
        string[] reacts = { "*HIÇKIRIK!*", "*KEKELEDİN!*", "*TERLEDİN!*" };
        
        // Yazıyı ayarla
        reactionText.text = reacts[Random.Range(0, reacts.Length)];
        reactionText.gameObject.SetActive(true);

        // Flaş efektini çalıştır
        if(flashFilter != null) {
            flashFilter.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            flashFilter.gameObject.SetActive(false);
        }
        
        yield return new WaitForSeconds(2f);
        reactionText.gameObject.SetActive(false);
    }
}