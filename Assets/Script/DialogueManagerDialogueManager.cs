using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // Sahne yönetimi için gerekli

public class DialogueManager : MonoBehaviour 
{
    public static DialogueManager Instance;
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public Button[] choiceButtons;
    public TextMeshProUGUI[] choiceTexts;

    private void Awake() { Instance = this; if(dialoguePanel) dialoguePanel.SetActive(false); }

    public void StartDialogue(DialogueNode node) 
    {
        if(dialoguePanel) dialoguePanel.SetActive(true);
        DisplayNode(node);
    }

    private void DisplayNode(DialogueNode node) 
    {
        if(speakerNameText) speakerNameText.text = node.speakerName;
        if(dialogueText) dialogueText.text = node.dialogueText;
        
        foreach (Button b in choiceButtons) b.gameObject.SetActive(false);

        for (int i = 0; i < node.choices.Length; i++) 
        {
            if (i >= choiceButtons.Length) break;

            choiceButtons[i].gameObject.SetActive(true);
            choiceTexts[i].text = node.choices[i].choiceText;
            
            int index = i; // Closure hatasını önlemek için
            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(node.choices[index]));
        }
    }

    private void OnChoiceSelected(DialogueChoice choice) 
    {
        // 1. EĞER BU BİR YALANSA, QTE'Yİ BAŞLAT VE SAHNE İSMİNİ ONA GÖNDER
        if (choice.isLie) 
        {
            PinocchioQTE.Instance.TriggerRandomEvent(choice.tensionIncreaseAmount, choice.targetSceneName);
            // Diyalog panelini kapat ki QTE rahat görünsün
            dialoguePanel.SetActive(false); 
            return;
        }

        // 2. YALAN DEĞİLSE AMA SAHNE GEÇİŞİ VARSA (Dürüstçe bir yere gitmek)
        if (!string.IsNullOrEmpty(choice.targetSceneName)) 
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(choice.targetSceneName);
            return;
        }

        // 3. NORMAL DİYALOG AKIŞI
        if (choice.isFinalChoice) { EndingManager.Instance.TriggerEnding(choice.isLie); return; }
        if (choice.nextNode != null) DisplayNode(choice.nextNode);
        else dialoguePanel.SetActive(false);
    }
}