using UnityEngine;

[RequireComponent(typeof(Life))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class EnemyBase : MonoBehaviour
{

    [Header("EnemyBaseAutomatic")]
    // Los hijos acceden
    public Rigidbody2D rb;
    public Transform player;
    public SpriteRenderer spr;
    public Life life;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        life = GetComponent<Life>();
    }

    protected virtual void OnEnable()
    {
        life.onLifeDepleted.AddListener(OnLifeDepleted);
    }

    protected virtual void OnDisable()
    {
        life.onLifeDepleted.RemoveListener(OnLifeDepleted);
    }

    private void OnLifeDepleted(float arg)
    {
        Destroy(gameObject);
    }

    public void SetPlayer(Transform playerTr)
    {
        player = playerTr;
    }
}
