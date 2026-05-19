using System.Collections;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] GameObject enemy;
    [SerializeField] float timeBetweenEnemies = 1f;

    private void Start()
    {
        StartCoroutine(GenerateEnemies());
    }

    IEnumerator GenerateEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenEnemies);
            GameObject newEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
            newEnemy.GetComponent<AIControl>().SetTarget(target);
        }
    }
}
