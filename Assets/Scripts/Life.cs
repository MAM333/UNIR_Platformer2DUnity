using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour
{

    [Header("Damage")]
    [SerializeField] float startLife = 1f;
    [SerializeField] float damagePerHit = 0.25f;
    [SerializeField] float velocityJumpingBackX = 4f;
    [SerializeField] float velocityJumpingBackY = 4f;
    [SerializeField] float timeBeingMovingBack = 0.8f;
    [SerializeField] float inmunityTime = 1.5f;
    [SerializeField] float timeBetweenBlinks = 0.15f;
    HurtCollider hurtCollider;
    SpriteRenderer sprRenderer;
    Rigidbody2D rb;

    public UnityEvent<float, float, bool> onLifeChanged; // startLife currentLife damage
    public UnityEvent onJumpBackFinish;
    public UnityEvent<float> onLifeDepleted; // start life

    float currentLife;

    private void Awake()
    {
        hurtCollider = GetComponent<HurtCollider>();
        sprRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        currentLife = startLife;
    }


    private void OnEnable()
    {
        hurtCollider.onHitReceive.AddListener(OnHitReceived);
    }

    private void OnDisable()
    {
        hurtCollider.onHitReceive.RemoveListener(OnHitReceived);
    }

    public void Kill()
    {
        currentLife = 0;
        onLifeChanged.Invoke(currentLife, startLife, false);
        onLifeDepleted.Invoke(startLife);
    }

    public void Restart()
    {
        currentLife = startLife;
        onLifeChanged.Invoke(currentLife, startLife, false);
    }

    bool inmune = false;
    private void OnHitReceived(bool agressorIsRight, bool isDownAttack)
    {
        if (inmune || currentLife <= 0) return;

        // Nunca deberia pasar esto puesto que la inmunidad sera superior al movimiento
        // pero por si acaso
        StopCoroutine(InmunityTime());
        StartCoroutine(InmunityTime());

        StopCoroutine(HitReceiveMovement(agressorIsRight, isDownAttack));
        StartCoroutine(HitReceiveMovement(agressorIsRight, isDownAttack));

        currentLife -= damagePerHit;
        onLifeChanged.Invoke(currentLife, startLife, true);
        if (currentLife <= 0)
        {
            currentLife = 0;
            onLifeDepleted.Invoke(startLife);
        }
    }

    IEnumerator InmunityTime()
    {
        inmune = true;

        float timer = inmunityTime;
        while (timer > 0)
        {
            yield return new WaitForSeconds(timeBetweenBlinks);
            timer -= timeBetweenBlinks;
            sprRenderer.enabled = !sprRenderer.enabled;
        }

        sprRenderer.enabled = true;

        inmune = false;
    }

    IEnumerator HitReceiveMovement(bool agressorIsRight, bool isDownAttack)
    {
        if (velocityJumpingBackX == 0)
        {
            yield break;
        } 

        if (!isDownAttack)
        {
            rb.linearVelocityX = (agressorIsRight ? -velocityJumpingBackX : velocityJumpingBackX);
            rb.linearVelocityY = velocityJumpingBackY;
        }

        yield return new WaitForSeconds(timeBeingMovingBack / (isDownAttack ? 4 : 1));

        rb.linearVelocityX = 0;

        onJumpBackFinish.Invoke();
    }
}
