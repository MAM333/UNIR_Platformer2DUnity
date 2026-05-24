using UnityEngine;
using UnityEngine.Events;

public class HitCollider : MonoBehaviour
{
    [SerializeField] bool isAllyHit = false;
    [SerializeField] bool isDownAttack = false;

    public UnityEvent hitSuccess;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isAllyHit) return;
        else if (collision.CompareTag("Enemy") && !isAllyHit) return;

        HurtCollider hit = collision.GetComponent<HurtCollider>();
        if (hit != null)
        {
            bool agressorIsRight = (transform.position.x > collision.transform.position.x);
            hit.NotifyHit(this, agressorIsRight, isDownAttack);
            hitSuccess.Invoke();
        }
    }
}
