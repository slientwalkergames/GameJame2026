using UnityEngine;
using TMPro;
using System.Collections;

public class InnerMonologueManager : MonoBehaviour
{
    [Header("UI Referansları")]
    public TextMeshProUGUI monologueText; 
    public AudioSource typewriterAudio;
    
    [Header("Düşünceler")]
    public ThoughtNode[] monologueNodes;
    
    [Header("Hız Ayarları")]
    public float typingSpeed = 0.1f;  // Yazma hızı (yavaşlatıldı)
    public float hidingSpeed = 0.08f; // Silme hızı (yavaşlatıldı)
    
    private int currentIndex = 0;
    private bool isTyping = false;
    private bool isHiding = false;

    void OnEnable()
    {
        // Obje aktif olduğu an çalışır
        monologueText.text = "";
        currentIndex = 0;
        isTyping = false;
        isHiding = false;
        
        if (monologueNodes.Length > 0)
            StartCoroutine(ShowThought(monologueNodes[currentIndex]));
    }

    void Update()
    {
        // Sadece yazı yazılmıyorsa ve silinmiyorsa tıklamayı kabul et
        if (Input.GetMouseButtonDown(0) && !isTyping && !isHiding)
        {
            StartCoroutine(HideThought());
        }
    }

    IEnumerator ShowThought(ThoughtNode node)
    {
        isTyping = true;
        monologueText.text = "";
        
        if (typewriterAudio != null && typewriterAudio.clip != null) 
            typewriterAudio.Play();

        foreach (char letter in node.thoughtText.ToCharArray())
        {
            monologueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        
        if (typewriterAudio != null) typewriterAudio.Stop();
        isTyping = false;
    }

    IEnumerator HideThought()
    {
        isHiding = true;
        if (typewriterAudio != null && typewriterAudio.clip != null) 
            typewriterAudio.Play();

        while (monologueText.text.Length > 0)
        {
            monologueText.text = monologueText.text.Remove(monologueText.text.Length - 1);
            yield return new WaitForSeconds(hidingSpeed);
        }

        if (typewriterAudio != null) typewriterAudio.Stop();

        // Bir sonrakine geç
        currentIndex++;
        if (currentIndex < monologueNodes.Length)
        {
            isHiding = false;
            StartCoroutine(ShowThought(monologueNodes[currentIndex]));
        }
        else
        {
            gameObject.SetActive(false); // Hepsi bitti, balonu kapat
        }
    }
}