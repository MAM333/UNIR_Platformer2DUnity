using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] float velocityX;

    SpriteRenderer spr;
    Rigidbody2D rb;
    Life life;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        life = GetComponent<Life>();
    }

    private void OnEnable()
    {
        life.onLifeDepleted.AddListener(OnLifeDepleted);
    }

    public void InitSpike(bool right)
    {
        rb.linearVelocityX = (right ? velocityX : -velocityX);
        spr.flipX = !right;
    }

    private void OnDisable()
    {
        life.onLifeDepleted.RemoveListener(OnLifeDepleted);
    }

    private void OnLifeDepleted(float arg1)
    {
        Destroy(gameObject);
    }
}
