using UnityEngine;

public class NPCInteractable : MonoBehaviour {
    public DialogueNode startingNode;
    private bool isPlayerInRange = false;

    void Update() {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E)) {
            if (!DialogueManager.Instance.dialoguePanel.activeInHierarchy)
                DialogueManager.Instance.StartDialogue(startingNode);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) { if (collision.CompareTag("Player")) isPlayerInRange = true; }
    private void OnTriggerExit2D(Collider2D collision) { if (collision.CompareTag("Player")) isPlayerInRange = false; }
}