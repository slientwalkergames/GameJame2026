using UnityEngine;
using TMPro;
using System.Collections;

public class InnerMonologueManager : MonoBehaviour {
    public TextMeshProUGUI monologueText; 
    public AudioSource typewriterAudio;
    public ThoughtNode[] monologueNodes;
    public float typingSpeed = 0.1f;
    public float hidingSpeed = 0.05f;
    private int currentIndex = 0;
    private bool isTyping = false;
    private bool isHiding = false;

    void OnEnable() {
        if(monologueText) monologueText.text = ""; 
        currentIndex = 0;
        if (monologueNodes.Length > 0) StartCoroutine(ShowThought(monologueNodes[currentIndex]));
    }

    void Update() {
        if (Input.GetMouseButtonDown(0) && !isTyping && !isHiding) StartCoroutine(HideThought());
    }

    IEnumerator ShowThought(ThoughtNode node) {
        isTyping = true; 
        if(monologueText) monologueText.text = "";
        if (typewriterAudio) typewriterAudio.Play();
        foreach (char letter in node.thoughtText.ToCharArray()) {
            if(monologueText) monologueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        if (typewriterAudio) typewriterAudio.Stop();
        isTyping = false;
    }

    IEnumerator HideThought() {
        isHiding = true;
        if (typewriterAudio) typewriterAudio.Play();
        while (monologueText != null && monologueText.text.Length > 0) {
            monologueText.text = monologueText.text.Remove(monologueText.text.Length - 1);
            yield return new WaitForSeconds(hidingSpeed);
        }
        if (typewriterAudio) typewriterAudio.Stop();
        currentIndex++;
        if (currentIndex < monologueNodes.Length) {
            isHiding = false; 
            StartCoroutine(ShowThought(monologueNodes[currentIndex]));
        } else gameObject.SetActive(false);
    }
}