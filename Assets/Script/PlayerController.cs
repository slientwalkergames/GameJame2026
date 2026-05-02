using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector3 originalScale;
    
    private Animator anim; // YENİ: Animasyon kontrolcüsü

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // YENİ: Karakterdeki animatörü bul
        originalScale = transform.localScale;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");

        // Karakterin yönünü döndürme
        if (movement.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (movement.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

        // YENİ: Animasyonu Tetikleme
        if (anim != null)
        {
            // Eğer sağa veya sola basıyorsak yürüme animasyonunu aç, basmıyorsak kapat
            if (Mathf.Abs(movement.x) > 0)
                anim.SetBool("isWalking", true);
            else
                anim.SetBool("isWalking", false);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
    }
}