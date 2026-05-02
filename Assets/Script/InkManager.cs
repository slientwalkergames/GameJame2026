using UnityEngine;
using UnityEngine.UI;

public class InkManager : MonoBehaviour
{
    public static InkManager Instance;
    public GameObject inkPanel;
    public Slider breathSlider;
    public bool isEventActive = false;

    private void Awake() { Instance = this; inkPanel.SetActive(false); }

    public void StartInkEvent(int index = 0)
    {
        isEventActive = true;
        inkPanel.SetActive(true);
        breathSlider.value = 50f; // Boğulma hatasını engelle
        
        // Sahnedeki InkSystem scriptini bul ve tetikle
        FindObjectOfType<InkSystem>().StartInkCrisis(index);
    }

    private void Update()
    {
        if (!isEventActive) return;
        
        if (Input.GetKeyDown(KeyCode.Space)) breathSlider.value += 15f;
        breathSlider.value -= 10f * Time.deltaTime;

        if (breathSlider.value <= 0 || breathSlider.value >= 100) FailEvent();
    }

    public void InkCleared() { isEventActive = false; inkPanel.SetActive(false); }
    private void FailEvent() { isEventActive = false; inkPanel.SetActive(false); Debug.Log("BOĞULDU!"); }
}