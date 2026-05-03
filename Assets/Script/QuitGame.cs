using UnityEngine;

public class QuitGame : MonoBehaviour
{
    // Bu fonksiyonu "Oyun Bitti" butonunun OnClick() kısmına sürükle
    public void ExitApplication()
    {
        // Debug log: Editörde çalışıp çalışmadığını kontrol etmek için
        Debug.Log("Oyun kapatılıyor...");

        // Gerçek oyun derlemesinde (Build) oyunu kapatır
        Application.Quit();

        // Eğer Unity Editör içinde test ediyorsan çalışmayı durdurur
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}