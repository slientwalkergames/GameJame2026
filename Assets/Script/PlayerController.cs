using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator anim;
    private SpriteRenderer sr;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Update() {
        if (DialogueManager.Instance.dialoguePanel.activeInHierarchy || (PinocchioQTE.Instance && PinocchioQTE.Instance.qtePanel.activeInHierarchy)) {
            rb.linearVelocity = Vector2.zero;
            if(anim) anim.SetBool("isWalking", false);
            return;
        }
        movement.x = Input.GetAxisRaw("Horizontal");
        if (movement.x != 0 && sr) sr.flipX = movement.x < 0;
        if (anim) anim.SetBool("isWalking", movement.x != 0);
    }

    void FixedUpdate() { rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y); }
}