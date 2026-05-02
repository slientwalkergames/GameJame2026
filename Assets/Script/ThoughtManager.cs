using UnityEngine;
using TMPro;
using System.Collections;

public class ThoughtManager : MonoBehaviour
{
    public TextMeshProUGUI displayArea; // Konuşma balonundaki yazı
    public AudioSource typewriterSound; // Daktilo sesi
    public ThoughtNode[] thoughts;      // Düşünce dosyaların
    
    private int index = 0;
    private bool isTyping = false;
    private string currentText;

    void Start()
    {
        // Başlangıçta metni temizle
        displayArea.text = "";
        StartCoroutine(TypeThought(thoughts[index]));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                displayArea.text = currentText;
                isTyping = false;
            }
            else
            {
                index++;
                if (index < thoughts.Length) StartCoroutine(TypeThought(thoughts[index]));
                else gameObject.SetActive(false); // Düşünceler bitince balonu kapat
            }
        }
    }

   IEnumerator TypeThought(ThoughtNode node)
    {
        isTyping = true;
        // Buradaki değişken isminin, ThoughtNode içindekiyle aynı olduğundan emin ol
        currentText = node.thoughtText; 
        displayArea.text = "";

        foreach (char c in currentText.ToCharArray())
        {
            displayArea.text += c;
            if (typewriterSound != null) typewriterSound.Play();
            yield return new WaitForSeconds(0.05f);
        }
        isTyping = false;
    }
}