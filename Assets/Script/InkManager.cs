using UnityEngine;
using UnityEngine.UI;

public class InkManager : MonoBehaviour {
    public static InkManager Instance;
    public GameObject inkPanel;
    public Slider breathSlider;
    public bool isEventActive = false;

    private void Awake() { 
        Instance = this; 
        if (inkPanel != null) inkPanel.SetActive(false); 
    }

    public void StartInkEvent() {
        isEventActive = true;
        if(inkPanel != null) inkPanel.SetActive(true);
        if(breathSlider != null) breathSlider.value = 50f;
    }

    public void InkCleared() { 
        isEventActive = false; 
        if(inkPanel != null) inkPanel.SetActive(false); 
    }

    void Update() {
        if (!isEventActive) return;
        if (Input.GetKeyDown(KeyCode.Space) && breathSlider != null) breathSlider.value += 15f;
        if (breathSlider != null) {
            breathSlider.value -= 10f * Time.deltaTime;
            if (breathSlider.value <= 0 || breathSlider.value >= 100) InkCleared();
        }
    }
}