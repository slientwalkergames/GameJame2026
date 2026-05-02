using UnityEngine;
using UnityEngine.UI;

public class InkManager : MonoBehaviour
{
    public static InkManager Instance;
    public GameObject inkPanel;
    public Slider breathSlider;
    public bool isEventActive = false;

    private void Awake() 
{ 
    Instance = this; 
    
    // Eğer panel atanmışsa kapat, atanmamışsa hata verme
    if (inkPanel != null) 
    {
        inkPanel.SetActive(false); 
    }
    else 
    {
        Debug.LogWarning("Dikkat: InkManager'da Ink Panel referansı eksik!");
    }
}

    public void StartInkEvent() {
        isEventActive = true;
        inkPanel.SetActive(true);
        breathSlider.value = 50f;
    }

    void Update() {
        if (!isEventActive) return;
        if (Input.GetKeyDown(KeyCode.Space)) breathSlider.value += 15f;
        breathSlider.value -= 10f * Time.deltaTime;
        if (breathSlider.value <= 0 || breathSlider.value >= 100) { isEventActive = false; inkPanel.SetActive(false); }
    }

    public void InkCleared() { isEventActive = false; inkPanel.SetActive(false); }
}