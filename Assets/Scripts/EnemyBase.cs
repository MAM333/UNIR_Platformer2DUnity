using UnityEngine;

[RequireComponent(typeof(Life))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class EnemyBase : MonoBehaviour
{

    [Header("EnemyBaseAutomatic")]
    // Los hijos acceden
    public Rigidbody2D rb;
    public SpriteRenderer spr;
    public Life life;
    public bool initialized = false;

    [Header("PlayerRelation")]
    public Transform player;
    public float differenceInXToStartMoving = 14f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        life = GetComponent<Life>();
    }

    protected virtual void OnEnable()
    {
        life.onLifeChanged.AddListener(OnLifeChanged);
        life.onLifeDepleted.AddListener(OnLifeDepleted);
    }

    protected virtual void Start()
    {
        initialized = false;
    }

    protected virtual void Update()
    {
        if (!initialized)
        {
            float x = transform.position.x;
            float playerX = player.position.x;
            if (playerX >= (x - differenceInXToStartMoving) && playerX <= x + differenceInXToStartMoving)
            {
                initialized = true;
            }
        }
    }

    protected virtual void OnDisable()
    {
        life.onLifeChanged.RemoveListener(OnLifeChanged);
        life.onLifeDepleted.RemoveListener(OnLifeDepleted);
    }

    private void OnLifeChanged(float arg1,  float arg2, bool damage)
    {
        if (damage)
        {
            SoundManager.instance.PlayDamageToEnemy();
        }
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
