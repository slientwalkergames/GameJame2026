using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueNode startingNode;

    void Start()
    {
        // Oyun başlar başlamaz diyaloğu ekrana getirir
        DialogueManager.Instance.StartDialogue(startingNode);
    }
}