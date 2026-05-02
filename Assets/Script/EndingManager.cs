using UnityEngine;

public class EndingManager : MonoBehaviour
{
    public static EndingManager Instance;
    public GameObject lieEndingPanel;
    public GameObject truthEndingPanel;

    private void Awake() { Instance = this; lieEndingPanel.SetActive(false); truthEndingPanel.SetActive(false); }

    public void TriggerEnding(bool isLie) {
        DialogueManager.Instance.dialoguePanel.SetActive(false);
        if (isLie) lieEndingPanel.SetActive(true);
        else truthEndingPanel.SetActive(true);
    }
}