using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TensionManager : MonoBehaviour {
    public static TensionManager Instance;
    public Slider tensionSlider; // Inspector'da Max Value: 100 yapmayı unutma!
    public Image screenVignette;
    public float currentTension = 0f;
    public float maxTension = 100f;
    public string gameOverSceneName = "GameOver"; 

    private void Awake() { Instance = this; }

    public void IncreaseTension(float amount) {
        currentTension += amount;
        currentTension = Mathf.Clamp(currentTension, 0, maxTension);
        
        if (tensionSlider != null) tensionSlider.value = currentTension;
        
        if (screenVignette != null) {
            Color c = screenVignette.color;
            c.a = currentTension / maxTension;
            screenVignette.color = c;
        }

        if (currentTension >= maxTension) SceneManager.LoadScene(gameOverSceneName);
    }
}