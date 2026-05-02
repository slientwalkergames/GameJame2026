using UnityEngine;
using UnityEngine.UI;

public class TensionManager : MonoBehaviour
{
    public static TensionManager Instance;
    public Slider tensionSlider;
    public Image screenVignette;
    public float currentTension = 0f;
    public float maxTension = 100f;

    private void Awake() { Instance = this; }

    public void IncreaseTension(float amount)
    {
        currentTension += amount;
        currentTension = Mathf.Clamp(currentTension, 0, maxTension);
        
        if (tensionSlider != null) tensionSlider.value = currentTension / maxTension;
        
        if (screenVignette != null) {
            Color c = screenVignette.color;
            c.a = currentTension / maxTension; // Bar doldukça kenarlar kızarır
            screenVignette.color = c;
        }

        if (currentTension >= maxTension) EndingManager.Instance.TriggerEnding(true);
    }
}