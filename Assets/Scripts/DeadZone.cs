using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Life life = collision.GetComponent<Life>();
            if (life != null)
            {
                life.Kill();
            }
        }
    }
}
