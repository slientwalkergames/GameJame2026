using UnityEngine;
using UnityEngine.UI;

public class InkSystem : MonoBehaviour
{
    [Header("Görsel Referanslar")]
    public Image dripImage; // Tepedeki akış görseli
    public Image poolImage; // Yerdeki göl görseli

    [Header("Sprite Dizileri")]
    public Sprite[] dripSprites; // Tepedeki akış sprite'ları
    public Sprite[] poolSprites; // Yerdeki göl sprite'ları

    [Header("Ayarlar")]
    public float growSpeed = 0.3f;
    private bool isRunning = false;

    void Awake()
    {
        // Başlangıçta her şeyi gizle
        if (dripImage != null) dripImage.gameObject.SetActive(false);
        if (poolImage != null) poolImage.gameObject.SetActive(false);
    }

    public void StartInkCrisis(int index)
    {
        // 1. Sprite'ları ata (Hata almamak için kontrol ediyoruz)
        if (index < dripSprites.Length && index < poolSprites.Length)
        {
            dripImage.sprite = dripSprites[index];
            poolImage.sprite = poolSprites[index];
        }

        // 2. Görünürlük
        dripImage.gameObject.SetActive(true);
        poolImage.gameObject.SetActive(true);
        
        // 3. Göl ayarları (Hata almamak için poolImage'in RectTransform'unu kullanıyoruz)
        poolImage.rectTransform.localScale = Vector3.zero;
        Color c = poolImage.color; 
        c.a = 0f; 
        poolImage.color = c;
        
        isRunning = true;
    }

    void Update()
    {
        if (!isRunning) return;

        // Göl yavaşça büyüsün
        if (poolImage.rectTransform.localScale.x < 2.0f)
        {
            poolImage.rectTransform.localScale += Vector3.one * growSpeed * Time.deltaTime;
            
            Color c = poolImage.color;
            c.a += 0.1f * Time.deltaTime;
            poolImage.color = c;
        }
        else
        {
            // Kriz bitti
            isRunning = false;
            dripImage.gameObject.SetActive(false);
            poolImage.gameObject.SetActive(false);
            
            // InkManager'a haber ver
            if (InkManager.Instance != null)
                InkManager.Instance.InkCleared();
        }
    }
}