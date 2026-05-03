using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PinocchioManager : MonoBehaviour {
    public static PinocchioManager Instance;
    public TextMeshProUGUI reactionText;
    public Image flashFilter;

    private void Awake() { Instance = this; reactionText.gameObject.SetActive(false); flashFilter.gameObject.SetActive(false); }

    public void TriggerReaction() { StartCoroutine(Show()); }

    IEnumerator Show() {
        reactionText.gameObject.SetActive(true);
        flashFilter.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        flashFilter.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        reactionText.gameObject.SetActive(false);
    }
}