using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;

    SpriteRenderer spr;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        spr.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (transform.childCount == 0)
            {
                GameObject spawn = Instantiate(enemy, transform);
            
                EnemyBase enemyB = spawn.GetComponent<EnemyBase>();
                enemyB.SetPlayer(collision.gameObject.transform);
            }
        }
    }
}
