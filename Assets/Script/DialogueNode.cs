using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Elias/Dialogue Node")]
public class DialogueNode : ScriptableObject {
    public string speakerName;
    [TextArea(3, 5)] public string dialogueText;
    public DialogueChoice[] choices;
}

[System.Serializable]
public class DialogueChoice {
    [TextArea(1, 2)] public string choiceText;
    public DialogueNode nextNode;
    [Header("Mekanikler")]
    public bool isLie;
    public float tensionIncreaseAmount;
    public bool triggersInkEvent;
    public bool isFinalChoice;
}