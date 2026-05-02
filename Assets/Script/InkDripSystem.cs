using UnityEngine;

public class InkCollision : MonoBehaviour
{
    public ParticleSystem poolSystem; // Yerdeki göl sistemi

    void OnParticleCollision(GameObject other)
    {
        // Parçacık yere değdiğinde göl sistemini tetikle
        poolSystem.Emit(1);
    }
}