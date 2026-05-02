using UnityEngine;

public class InkCollision : MonoBehaviour
{
    // Prefab içindeki referans yerine, sahnedeki gölü bulalım
    private ParticleSystem poolSystem;

    void Start()
    {
        // Sahnedeki göl objesini isminden bul
        GameObject poolObj = GameObject.Find("InkPoolSystem"); 
        if(poolObj != null) poolSystem = poolObj.GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other)
    {
        if (poolSystem != null) poolSystem.Emit(1);
    }
}