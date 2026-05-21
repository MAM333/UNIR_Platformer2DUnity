using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] float movementSpeed = 2f;
    [SerializeField] float jumpVelocity = 3f;
    [SerializeField] float dashTime = 0.5f;
    [SerializeField] float dashVelocity = 3f;
    [SerializeField] float tirolinaVelocity = 12f;

    [Header("Ground check")]
    [SerializeField] float groundCheckDistance = 0.2f;
    [SerializeField] LayerMask groundLayerMask = Physics2D.DefaultRaycastLayers;

    [Header("Combat")]
    [SerializeField] Transform leftHit;
    [SerializeField] Transform rightHit;
    [SerializeField] GameObject throwWeapon;
    [SerializeField] Transform rightThrowPos;
    [SerializeField] Transform leftThrowPos;

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

    bool movingRight = true;
    bool canMove = true;
    bool canDash = true;
    const float moveThreshold = 0.1f;
    void Update()
    {
        if (!canMove) return;

        rb.linearVelocityX = rawMove.x * movementSpeed;

        if (rawMove.x != 0) movingRight = (rawMove.x > 0);

        bool running = Mathf.Abs(rawMove.x) > moveThreshold;
        anim.SetBool("IsRunning", running);

        if (running) sprRenderer.flipX = (rawMove.x < 0);

        bool grounded = IsGrounded();
        anim.SetBool("IsGrounded", grounded);
        if (!canDash && grounded) canDash = true;

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
        if (IsGrounded() && canMove || movingInTirolina) 
        { 
            rb.linearVelocityY = jumpVelocity;
            movingInTirolina = false;
        }
    }

    public void Punch()
    {
        if (canMove) anim.SetTrigger("Punch");
    }

    public void Dash()
    {
        if (canDash) StartCoroutine(Dashing());
    }

    IEnumerator Dashing()
    {
        canMove = false;
        canDash = false;

        float timer = dashTime;

        anim.SetBool("Dashing", true);

        float velocity = dashVelocity * (movingRight ? 1 : -1);
        rb.linearVelocityX = velocity;

        float initGravity = rb.gravityScale;
        rb.linearVelocityY = 0;
        rb.gravityScale = 0;

        while (timer > 0)
        {
            yield return null;
            timer -= Time.deltaTime;
            rb.linearVelocityX = velocity;
        }

        canMove = true;

        anim.SetBool("Dashing", false);

        rb.linearVelocityX -= velocity;
        rb.gravityScale = initGravity;

        if (throwingWeapon) scytheThrowMovement.FinishMovement();
    }

    float gravity = 0;
    bool throwingWeapon = false;
    bool canDashAux = false;
    public void ThrowWeapon()
    {
        if (!canMove) return;

        canMove = false;
        throwingWeapon = true;
        canDashAux = canDash;
        canDash = false;

        gravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.linearVelocityX = 0;
        rb.linearVelocityY = 0;
        anim.SetTrigger("ThrowWeapon");
    }

    ScytheThrowMovement scytheThrowMovement;
    public void OnThrowAnimation()
    {
        Vector3 spawnPos = movingRight ? rightThrowPos.position : leftThrowPos.position;
        GameObject weapon = Instantiate(throwWeapon, spawnPos, Quaternion.identity);
        scytheThrowMovement = weapon.GetComponent<ScytheThrowMovement>();
        scytheThrowMovement.StartMove(movingRight);
        scytheThrowMovement.finishedMovement.AddListener(FinishedWeaponMovement);
        scytheThrowMovement.isReturning.AddListener(ReturningWeapon);

        canDash = canDashAux;
    }

    private void ReturningWeapon()
    {
        canDashAux = canDash;
        canDash = false;
    }

    private void FinishedWeaponMovement()
    {
        canMove = true;
        throwingWeapon = false;
        rb.gravityScale = gravity;
        anim.SetTrigger("WeaponRecovered");

        scytheThrowMovement.finishedMovement.RemoveListener(FinishedWeaponMovement);
        scytheThrowMovement = null;

        canDash = canDashAux;
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

    public bool Tirolina(Tirolina tirolina)
    {
        if (!canMove && !movingInTirolina) return false;

        if (movingInTirolina)
        {
            movingInTirolina = false;
        }
        else
        {
            StartCoroutine(TirolineoMaximo(tirolina));
        }

        return true;
    }

    private Vector3 GetClosestPoint(Vector3 A, Vector3 B)
    {
        Vector3 C = transform.position;

        Vector3 AB = B - A;
        Vector3 AC = C - A;
        float proyection = Vector3.Dot(AC, AB) / AB.sqrMagnitude;

        return (A + AB * proyection);
    }

    bool movingInTirolina = false;
    float offset = 1.5f;
    float timerTirolinaGround = 0.3f;
    IEnumerator TirolineoMaximo(Tirolina tirolina)
    {
        canMove = false;
        canDash = false;
        movingInTirolina = true;
        anim.SetBool("Hanged", true);

        rb.linearVelocityX = 0;
        rb.linearVelocityY = 0;
        gravity = rb.gravityScale;
        rb.gravityScale = 0;

        Transform tirolinaInitPos = movingRight ? tirolina.GetLeftObject() : tirolina.GetRightObject();
        Transform tirolinaFinalPos = movingRight ? tirolina.GetRightObject() : tirolina.GetLeftObject();

        transform.position = GetClosestPoint(tirolinaInitPos.position, tirolinaFinalPos.position) - Vector3.up * 1.2f;

        Vector3 moveVector = ((tirolinaFinalPos.position - Vector3.up * 1.2f) - transform.position).normalized;
        //Vector3 moveVector = Vector3.MoveTowards(transform.position, tirolinaFinalPos.position - Vector3.up * 1.2f, 1).normalized;
        //moveVector = moveVector - transform.position
        rb.linearVelocityX = moveVector.x * tirolinaVelocity;
        rb.linearVelocityY = moveVector.y * tirolinaVelocity;
        
        float timer = timerTirolinaGround;
        while (movingInTirolina)
        {
            if (Vector3.Distance(transform.position, tirolinaFinalPos.position) < offset) movingInTirolina = false;
            if (timer == 0 && IsGrounded()) movingInTirolina = false;
            yield return null;

            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer < 0) timer = 0;
            }
        }

        anim.SetBool("Hanged", false);
        rb.gravityScale = gravity;
        canMove = true;
        canDash = true;
    }
}
