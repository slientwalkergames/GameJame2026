using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class InnerMonologueManager : MonoBehaviour {
    public TextMeshProUGUI monologueText; 
    public AudioSource typewriterAudio;
    public ThoughtNode[] monologueNodes;
    public float typingSpeed = 0.08f;
    public float hidingSpeed = 0.04f;
    public string nextSceneName;

    private int currentIndex = 0;
    private bool isTyping = false;
    private bool isHiding = false;
    private string currentFullText;

    void OnEnable() {
        monologueText.text = ""; currentIndex = 0;
        if (monologueNodes.Length > 0) StartCoroutine(ShowThought(monologueNodes[currentIndex]));
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (isTyping) { StopAllCoroutines(); monologueText.text = currentFullText; isTyping = false; if (typewriterAudio) typewriterAudio.Stop(); }
            else if (isHiding) { StopAllCoroutines(); monologueText.text = ""; FinishHiding(); }
            else { StartCoroutine(HideAndNext()); }
        }
    }

    IEnumerator ShowThought(ThoughtNode node) {
        isTyping = true; currentFullText = node.thoughtText; monologueText.text = "";
        if (typewriterAudio) typewriterAudio.Play();
        foreach (char letter in currentFullText.ToCharArray()) {
            monologueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        if (typewriterAudio) typewriterAudio.Stop();
        isTyping = false;
    }

    IEnumerator HideAndNext() {
        isHiding = true;
        if (typewriterAudio) typewriterAudio.Play();
        while (monologueText.text.Length > 0) {
            monologueText.text = monologueText.text.Remove(monologueText.text.Length - 1);
            yield return new WaitForSeconds(hidingSpeed);
        }
        FinishHiding();
    }

    void FinishHiding() {
        if (typewriterAudio) typewriterAudio.Stop();
        isHiding = false; currentIndex++;
        if (currentIndex < monologueNodes.Length) StartCoroutine(ShowThought(monologueNodes[currentIndex]));
        else {
            if (!string.IsNullOrEmpty(nextSceneName)) SceneManager.LoadScene(nextSceneName);
            else gameObject.SetActive(false);
        }
    }
}