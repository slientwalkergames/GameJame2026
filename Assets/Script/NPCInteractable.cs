using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    public DialogueNode startingNode;
    private bool isPlayerInRange = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E tuşuna basıldı! isPlayerInRange durumu: " + isPlayerInRange);
            
            if (isPlayerInRange)
            {
                DialogueManager.Instance.StartDialogue(startingNode);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) { if (collision.CompareTag("Player")) isPlayerInRange = true; }
    private void OnTriggerExit2D(Collider2D collision) { if (collision.CompareTag("Player")) isPlayerInRange = false; }
}