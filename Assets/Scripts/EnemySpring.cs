using System.Collections;
using UnityEngine;

public class EnemySpring : EnemyBase
{
    [Header("Movement")]
    [SerializeField] float velocityX;
    [SerializeField] float velocityY;
    [SerializeField] float groundCheckDistance = 0.2f;
    [SerializeField] LayerMask groundLayerMask = Physics2D.DefaultRaycastLayers;

    protected override void OnEnable()
    {
        base.OnEnable();

        life.onLifeChanged.AddListener(OnLifeChanged);
        life.onJumpBackFinish.AddListener(OnJumpBackFinish);
    }

    bool canMove = false;
    bool jumpingBack = false;
    protected override void Update()
    {
        base.Update();

        if (jumpingBack) return;

        if (initialized)
        {
            canMove = IsGrounded();
        }

    }

    protected override void OnDisable()
    {
        base.OnDisable();

        life.onLifeChanged.RemoveListener(OnLifeChanged);
        life.onJumpBackFinish.RemoveListener(OnJumpBackFinish);
    }

    void OnLifeChanged(float arg1, float arg2, bool damage)
    {
        if (!damage) return;

        StopAllCoroutines();
        canMove = false;
        jumpingBack = true;
    }

    void OnJumpBackFinish()
    {
        jumpingBack = false;
    }

    public void OnMoveAnimation()
    {
        if (!canMove) return;

        StopAllCoroutines();
        StartCoroutine(Move());
    }

    float auxTimer = 0.7f;
    IEnumerator Move()
    {
        SoundManager.instance.PlayBoingEnemy();

        float moveX = velocityX;
        float moveY = velocityY;

        if (player != null)
        {
            if (player.position.x < transform.position.x)
            {
                moveX = -moveX;
                spr.flipX = true;
            }
            else spr.flipX = false;
        }

        rb.linearVelocityX = moveX;
        rb.linearVelocityY = moveY;

        yield return new WaitForSeconds(auxTimer);

        while (!IsGrounded())
        {
            yield return null;
        }

        rb.linearVelocityX = 0;
        rb.linearVelocityY = 0;
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayerMask);

        return hit && hit.collider != null;
    }
}
