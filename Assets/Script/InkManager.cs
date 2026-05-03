using UnityEngine;
using UnityEngine.UI;

public class InkManager : MonoBehaviour {
    public static InkManager Instance;
    public GameObject inkPanel;
    public Slider breathSlider;
    public bool isEventActive = false;

    private void Awake() { Instance = this; if (inkPanel) inkPanel.SetActive(false); }

    public void StartInkEvent() {
        isEventActive = true;
        if(inkPanel) inkPanel.SetActive(true);
        if(breathSlider) breathSlider.value = 50f;
    }

    public void InkCleared() { isEventActive = false; if(inkPanel) inkPanel.SetActive(false); }

    void Update() {
        if (!isEventActive) return;
        if (Input.GetKeyDown(KeyCode.Space)) breathSlider.value += 15f;
        breathSlider.value -= 10f * Time.deltaTime;
        if (breathSlider.value <= 0 || breathSlider.value >= 100) InkCleared();
    }
}