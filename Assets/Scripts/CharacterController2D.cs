using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] float movementSpeed = 2f;
    [SerializeField] float jumpVelocity = 3f;
    
    [Header("Ground check")]
    [SerializeField] float groundCheckDistance = 0.2f;
    [SerializeField] LayerMask groundLayerMask = Physics2D.DefaultRaycastLayers;

    [Header("Combat")]
    [SerializeField] Transform leftHit;
    [SerializeField] Transform rightHit;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sprRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprRenderer = GetComponent<SpriteRenderer>();

        leftHit.gameObject.SetActive(false);
        rightHit.gameObject.SetActive(false);
    }

    const float moveThreshold = 0.1f;
    void Update()
    {
        rb.linearVelocityX = rawMove.x * movementSpeed;

        bool running = Mathf.Abs(rawMove.x) > moveThreshold;
        anim.SetBool("IsRunning", running);

        if (running)
        {
            sprRenderer.flipX = rawMove.x < 0;
        }

        bool grounded = IsGrounded();
        anim.SetBool("IsGrounded", grounded);

        anim.SetBool("IsFalling", !grounded && rb.linearVelocityY < 0.1f);
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayerMask);

        return hit && hit.collider != null;
    }

    Vector2 rawMove;
    public void SetRawMove(Vector2 move)
    {
        rawMove = move;
    }

    public void Jump()
    {
        if (IsGrounded()) rb.linearVelocityY = jumpVelocity;
    }

    public void Punch()
    {
        anim.SetTrigger("Punch");
        //anim.ResetTrigger("Punch");
    }

    const float deactivateHitDelay = 0.25f;
    public void OnAnimationPunch()
    {
        if (sprRenderer.flipX)
        {
            leftHit.gameObject.SetActive(true);
        }
        else
        {
            rightHit.gameObject.SetActive(true);
        }
        
        Invoke(nameof(DeactivateHits), deactivateHitDelay);
    }

    void DeactivateHits()
    {
        leftHit.gameObject.SetActive(false);
        rightHit.gameObject.SetActive(false);
    }
}
