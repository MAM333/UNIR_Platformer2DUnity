using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.Image;

public enum BufferedAction
{
    None,
    Jump,
    Punch,
    Dash,
    ThrowWeapon
}

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] float movementSpeed = 2f;
    [SerializeField] float jumpVelocity = 3f;
    [SerializeField] float dashTime = 0.5f;
    [SerializeField] float dashVelocity = 3f;
    [SerializeField] float tirolinaVelocity = 12f;
    [SerializeField] float inputBufferTime = 0.2f;
    [SerializeField] float dashBufferTime = 1.0f;
    [SerializeField] float offsetTirolineo = 1.5f;

    [Header("Ground and wall check")]
    [SerializeField] float groundCheckDistance = 0.2f;
    [SerializeField] LayerMask groundLayerMask = Physics2D.DefaultRaycastLayers;
    [SerializeField] float wallCheckDistance = 0.2f;

    [Header("Combat")]
    [SerializeField] Transform leftHit;
    [SerializeField] Transform rightHit;
    [SerializeField] Transform downHit;
    [SerializeField] float downHitUpVelocity = 5f;
    [SerializeField] float deactivateHitDelay = 0.25f;
    [SerializeField] float timeBetweenAttacks = 1f;
    [SerializeField] GameObject throwWeapon;
    [SerializeField] Transform rightThrowPos;
    [SerializeField] Transform leftThrowPos;

    HitCollider downHitCollider;
    Life life;
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sprRenderer;
    BoxCollider2D boxCollider;
    BufferedAction bufferedAction = BufferedAction.None;
    bool canAttack = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprRenderer = GetComponent<SpriteRenderer>();
        life = GetComponent<Life>();
        downHitCollider = downHit.gameObject.GetComponent<HitCollider>();
        boxCollider = GetComponent<BoxCollider2D>();

        leftHit.gameObject.SetActive(false);
        rightHit.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        life.onLifeChanged.AddListener(OnLifeChanged);
        life.onJumpBackFinish.AddListener(OnJumpBackFinish);
        downHitCollider.hitSuccess.AddListener(DownHitSuccess);
    }


    float gravity = 0;
    private void Start()
    {
        gravity = rb.gravityScale;
    }

    bool movingRight = true;
    bool canMove = true;
    bool canDash = true;
    const float moveThreshold = 0.1f;
    bool lookingDown = false;
    float bufferTimer = 0;
    void Update()
    {
        if (bufferTimer > 0)
        {
            bufferTimer -= Time.deltaTime;

            if (bufferedAction != BufferedAction.None)
            {
                if (canMove || canDash) ExecuteAction(bufferedAction);
            }
            else if (bufferTimer <= 0)
            {
                bufferTimer = 0;
                bufferedAction = BufferedAction.None;
            }
        }

        if (!canMove) return;

        rb.linearVelocityX = rawMove.x * movementSpeed;
        rb.gravityScale = gravity;

        if (rawMove.x != 0) movingRight = (rawMove.x > 0);

        if (IsOnWall(movingRight))
        {
            rb.linearVelocityX = 0;
        }

        bool running = Mathf.Abs(rawMove.x) > moveThreshold;
        anim.SetBool("IsRunning", running);

        if (running) sprRenderer.flipX = (rawMove.x < 0);

        bool grounded = IsGrounded();
        anim.SetBool("IsGrounded", grounded);
        if (!canDash && grounded) canDash = true;



        anim.SetBool("IsFalling", !grounded && rb.linearVelocityY < 0.1f);
    }

    private void OnDisable()
    {
        life.onLifeChanged.RemoveListener(OnLifeChanged);
        life.onJumpBackFinish.RemoveListener(OnJumpBackFinish);
        downHitCollider.hitSuccess.RemoveListener(DownHitSuccess);
    }

    private void OnLifeChanged(float arg1, float arg2, bool damage)
    {
        if (!damage) return;

        canMove = false;
        canDash = false;

        FinishDashRoutine();
    }

    private void OnJumpBackFinish()
    {
        canMove = true;
        canDash = true;
    }

    private void DownHitSuccess()
    {
        rb.linearVelocityY = downHitUpVelocity;
        canDash = true;
    }

    readonly float offsetX = 0.5f;
    bool IsGrounded()
    {

        Vector3 origin = transform.position;
        Vector3 left = origin + Vector3.left * offsetX;
        Vector3 right = origin + Vector3.right * offsetX;
        RaycastHit2D hitLeft = Physics2D.Raycast(left, Vector2.down, groundCheckDistance, groundLayerMask);
        RaycastHit2D hitRight = Physics2D.Raycast(right, Vector2.down, groundCheckDistance, groundLayerMask);

        Debug.DrawRay(left, Vector2.down * groundCheckDistance, Color.red);
        Debug.DrawRay(right, Vector2.down * groundCheckDistance, Color.red);

        bool conditionLeft = hitLeft && hitLeft.collider != null;
        bool conditionRight = hitRight && hitRight.collider != null;
        return conditionLeft || conditionRight;
    }

    bool IsOnWall(bool right)
    {
        Vector2 check = (right ? Vector2.right : Vector2.left);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, check, wallCheckDistance, groundLayerMask);
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position + Vector3.up * 1.16f, check, wallCheckDistance, groundLayerMask);


        //Debug.DrawRay(transform.position, check * wallCheckDistance, Color.red);
        //Debug.DrawRay(transform.position + Vector3.up * 1.16f, check * wallCheckDistance, Color.red);

        bool condition = hit && hit.collider != null;
        bool conditionUp = hitUp && hitUp.collider != null;
        return condition || conditionUp;
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
            SoundManager.instance.PlayJump();
            rb.linearVelocityY = jumpVelocity;
            movingInTirolina = false;
        }
        else AddBufferAction(BufferedAction.Jump);
    }

    public void Punch()
    {
        if (canMove && canAttack)
        {
            if (IsGrounded() || !lookingDown) anim.SetTrigger("Punch");
            else anim.SetTrigger("PunchDownAir");
            StartCoroutine(AttacksCD());
        }
        else AddBufferAction(BufferedAction.Punch);
    }

    public void OnAnimationPunch()
    {
        SoundManager.instance.PlayAttack();

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

    public void OnAnimationDownPunch()
    {
        SoundManager.instance.PlayAttack();

        downHit.gameObject.SetActive(true);
        Invoke(nameof(DeactivateHits), deactivateHitDelay);
    }

    void DeactivateHits()
    {
        leftHit.gameObject.SetActive(false);
        rightHit.gameObject.SetActive(false);
        downHit.gameObject.SetActive(false);
    }

    IEnumerator AttacksCD()
    {
        canAttack = false;

        yield return new WaitForSeconds(timeBetweenAttacks);

        canAttack= true;
    }

    public void Dash()
    {
        if (canDash) StartCoroutine(Dashing());
        else AddBufferAction(BufferedAction.Dash);
    }

    IEnumerator Dashing()
    {
        canMove = false;
        canDash = false;
        
        SoundManager.instance.PlayDash();

        float timer = dashTime;

        anim.SetBool("Dashing", true);

        float velocity = dashVelocity * (movingRight ? 1 : -1);
        rb.linearVelocityX = velocity;

        rb.linearVelocityY = 0;
        rb.gravityScale = 0;

        while (timer > 0)
        {
            yield return null;
            timer -= Time.deltaTime;
            rb.linearVelocityX = velocity;
        }

        canMove = true;

        rb.linearVelocityX -= velocity;

        FinishDashRoutine();
    }

    private void FinishDashRoutine()
    {
        StopCoroutine(Dashing());
        anim.SetBool("Dashing", false);
        if (throwingWeapon && scytheThrowMovement != null) scytheThrowMovement.FinishMovement();
        rb.gravityScale = gravity;
    }

    bool throwingWeapon = false;
    bool canDashAux = false;
    public void ThrowWeapon()
    {
        if (!canMove)
        {
            AddBufferAction(BufferedAction.ThrowWeapon);
            return;
        }

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
        SoundManager.instance.PlayThrowAttack();

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
        SoundManager.instance.PlayThrowRecover();

        canMove = true;
        throwingWeapon = false;
        rb.gravityScale = gravity;
        anim.SetTrigger("WeaponRecovered");

        scytheThrowMovement.finishedMovement.RemoveListener(FinishedWeaponMovement);
        scytheThrowMovement = null;

        canDash = canDashAux;
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

    public bool movingInTirolina = false;
    //readonly float offset = 1.5f;
    readonly float timerTirolinaGround = 0.2f;
    readonly float timerTirolinaCollider = 0.4f;
    IEnumerator TirolineoMaximo(Tirolina tirolina)
    {

        Transform tirolinaInitPos = movingRight ? tirolina.GetLeftObject() : tirolina.GetRightObject();
        Transform tirolinaFinalPos = movingRight ? tirolina.GetRightObject() : tirolina.GetLeftObject();
        Vector3 initPos = GetClosestPoint(tirolinaInitPos.position, tirolinaFinalPos.position) - Vector3.up * 1.2f;

        float distance = Vector3.Distance(initPos, tirolinaFinalPos.position);
        if (distance < offsetTirolineo)
        {
            yield break;
        }

        canMove = false;
        canDash = false;
        movingInTirolina = true;
        anim.SetBool("Hanged", true);

        SoundManager.instance.PlayTirolina(true);

        transform.position = initPos;

        rb.linearVelocityX = 0;
        rb.linearVelocityY = 0;
        gravity = rb.gravityScale;
        rb.gravityScale = 0;
        boxCollider.enabled = false;

        Vector3 moveVector = ((tirolinaFinalPos.position - Vector3.up * 1.2f) - transform.position).normalized;
        rb.linearVelocityX = moveVector.x * tirolinaVelocity;
        rb.linearVelocityY = moveVector.y * tirolinaVelocity;
        
        float timer = timerTirolinaGround;
        float timerCollider = timerTirolinaCollider;
        while (movingInTirolina)
        {
            if (Vector3.Distance(transform.position, tirolinaFinalPos.position) < offsetTirolineo) movingInTirolina = false;
            if (timer == 0 && (IsGrounded() || IsOnWall(movingRight))) movingInTirolina = false;
            yield return null;

            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer < 0) timer = 0;
            }

            if (timerCollider > 0)
            {
                timerCollider -= Time.deltaTime;
                if (timerCollider < 0)
                {
                    timerCollider = 0;
                    boxCollider.enabled = true;
                }
            }
        }

        SoundManager.instance.PlayTirolina(false);

        boxCollider.enabled = true;
        anim.SetBool("Hanged", false);
        rb.gravityScale = gravity;
        canMove = true;
        canDash = true;
    }

    public void LookingDown(bool looking)
    {
        lookingDown = looking;
    }

    private void AddBufferAction(BufferedAction action)
    {
        bufferedAction = action;
        if (action == BufferedAction.Dash && throwingWeapon)
        {
            Debug.Log("Entro");
            bufferTimer = dashBufferTime;
        }
        else
        {
            bufferTimer = inputBufferTime;
        }
    }

    private void ExecuteAction(BufferedAction action)
    {
        switch (action)
        {
            case BufferedAction.Punch: Punch(); break;
            case BufferedAction.Jump: Jump(); break;
            case BufferedAction.Dash: Dash(); break;
            case BufferedAction.ThrowWeapon: ThrowWeapon(); break;
        }

        bufferedAction = BufferedAction.None;
    }
}
