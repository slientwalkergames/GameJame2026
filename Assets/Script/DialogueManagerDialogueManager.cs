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
        dialoguePanel.SetActive(true);
        DisplayNode(node);
    }

    private void DisplayNode(DialogueNode node)
    {
        speakerNameText.text = node.speakerName;
        dialogueText.text = node.dialogueText;

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].gameObject.SetActive(false);
            choiceButtons[i].onClick.RemoveAllListeners();
        }

        for (int i = 0; i < node.choices.Length; i++)
        {
            int index = i;
            choiceButtons[i].gameObject.SetActive(true);
            choiceTexts[i].text = node.choices[index].choiceText;
            choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(node.choices[index]));
        }
    }

    private void OnChoiceSelected(DialogueChoice choice)
    {
        if (choice.isFinalChoice) { EndingManager.Instance.TriggerEnding(choice.isLie); return; }
        if (choice.isLie) QTEManager.Instance.StartQTE(choice.tensionIncreaseAmount);
        if (choice.triggersInkEvent) InkManager.Instance.StartInkEvent();

        if (choice.nextNode != null) DisplayNode(choice.nextNode);
        else dialoguePanel.SetActive(false);
    }
}