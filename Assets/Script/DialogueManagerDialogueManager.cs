using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour {
    public static DialogueManager Instance;
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public Button[] choiceButtons;
    public TextMeshProUGUI[] choiceTexts;

    private void Awake() { Instance = this; if(dialoguePanel) dialoguePanel.SetActive(false); }

    public void StartDialogue(DialogueNode node) {
        dialoguePanel.SetActive(true);
        DisplayNode(node);
    }

    private void DisplayNode(DialogueNode node) {
        speakerNameText.text = node.speakerName;
        dialogueText.text = node.dialogueText;
        foreach (Button b in choiceButtons) b.gameObject.SetActive(false);
        for (int i = 0; i < node.choices.Length; i++) {
            choiceButtons[i].gameObject.SetActive(true);
            choiceTexts[i].text = node.choices[i].choiceText;
            int index = i;
            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(node.choices[index]));
        }
    }

    private void OnChoiceSelected(DialogueChoice choice) {
        if (choice.isLie) {
            PinocchioQTE.Instance.TriggerRandomEvent(choice.tensionIncreaseAmount, choice.targetSceneName);
            dialoguePanel.SetActive(false);
            return;
        }
        if (!string.IsNullOrEmpty(choice.targetSceneName)) {
            SceneManager.LoadScene(choice.targetSceneName);
            return;
        }
        if (choice.nextNode != null) DisplayNode(choice.nextNode);
        else dialoguePanel.SetActive(false);
    }
}