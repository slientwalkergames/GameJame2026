using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PinocchioManager : MonoBehaviour {
    public static PinocchioManager Instance;
    public TextMeshProUGUI reactionText;
    public Image flashFilter;
    public AudioSource failAudioSource;
    
    [Header("Hata Sesleri (0:Hiccup, 1:Stutter...)")]
    public AudioClip[] failSounds; 

    private string[] reactions = { "*HIÇKIRIK!*", "*KEKELEDİN!*", "*TERLEDİN!*", "*GÖZÜN SEYİRDİ!*", "*NEFESİN KESİLDİ!*", "*TİTREDİN!*" };

    private void Awake() { 
        Instance = this; 
        if(reactionText) reactionText.gameObject.SetActive(false); 
        if(flashFilter) flashFilter.gameObject.SetActive(false); 
    }

    public void TriggerReaction() { if(gameObject.activeInHierarchy) StartCoroutine(Show()); }

    IEnumerator Show() {
        int rand = Random.Range(0, reactions.Length);
        reactionText.text = reactions[rand];
        
        // Hata sesini çal
        if(failAudioSource && failSounds.Length > rand) failAudioSource.PlayOneShot(failSounds[rand]);

        reactionText.gameObject.SetActive(true);
        if(flashFilter) flashFilter.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        if(flashFilter) flashFilter.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        reactionText.gameObject.SetActive(false);
    }
}