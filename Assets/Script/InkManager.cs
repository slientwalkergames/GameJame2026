using UnityEngine;

public class InkManager : MonoBehaviour
{
    public static InkManager Instance;

    [Header("Mürekkep Prefabları")]
    public GameObject dripPrefab; // Drip Prefab'ını buraya sürükle
    public GameObject poolPrefab; // Pool Prefab'ını buraya sürükle

    private GameObject activeDrip;
    private GameObject activePool;

    private void Awake() { Instance = this; }

    public void StartInkEvent()
    {
        // Eğer zaten varsa önce onları temizle
        if (activeDrip != null) Destroy(activeDrip);
        if (activePool != null) Destroy(activePool);

        // Prefabları sahnede oluştur (Spawn et)
        activeDrip = Instantiate(dripPrefab, Vector3.zero, Quaternion.identity);
        activePool = Instantiate(poolPrefab, Vector3.zero, Quaternion.identity);
        
        Debug.Log("Mürekkep efektleri spawn edildi!");
    }

    public void InkCleared()
    {
        // Kriz bitince yok et
        if (activeDrip != null) Destroy(activeDrip);
        if (activePool != null) Destroy(activePool);
    }
}