using UnityEngine;

public class EndingManager : MonoBehaviour {
    public static EndingManager Instance;
    public GameObject lieEndingPanel;
    public GameObject truthEndingPanel;

    private void Awake() { Instance = this; if(lieEndingPanel) lieEndingPanel.SetActive(false); if(truthEndingPanel) truthEndingPanel.SetActive(false); }

    public void TriggerEnding(bool isLie) {
        if(DialogueManager.Instance) DialogueManager.Instance.dialoguePanel.SetActive(false);
        if (isLie) lieEndingPanel.SetActive(true);
        else truthEndingPanel.SetActive(true);
    }
}