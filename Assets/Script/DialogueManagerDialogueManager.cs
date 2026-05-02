using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public Button[] choiceButtons;
    public TextMeshProUGUI[] choiceTexts;

    private void Awake() { Instance = this; }

    public void StartDialogue(DialogueNode node)
    {
        if (node == null) return;
        dialoguePanel.SetActive(true);
        DisplayNode(node);
    }

    private void DisplayNode(DialogueNode node)
    {
        speakerNameText.text = node.speakerName;
        dialogueText.text = node.dialogueText;

        foreach (Button b in choiceButtons) b.gameObject.SetActive(false);

        for (int i = 0; i < node.choices.Length; i++)
        {
            if (i >= choiceButtons.Length) break;
            choiceButtons[i].gameObject.SetActive(true);
            choiceTexts[i].text = node.choices[i].choiceText;
            int index = i;
            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(node.choices[index]));
        }
    }

    private void OnChoiceSelected(DialogueChoice choice)
    {
        if (choice.isFinalChoice) {
            if (EndingManager.Instance != null) EndingManager.Instance.TriggerEnding(choice.isLie);
            return;
        }

        if (choice.isLie) {
            if (PinocchioQTE.Instance != null) PinocchioQTE.Instance.TriggerRandomEvent(choice.tensionIncreaseAmount);
        }

        if (choice.triggersInkEvent) {
            if (InkManager.Instance != null) InkManager.Instance.StartInkEvent();
        }

        if (choice.nextNode != null) DisplayNode(choice.nextNode);
        else dialoguePanel.SetActive(false);
    }
}