using UnityEngine;
using UnityEngine.Events;

public class HitCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HurtCollider hit = collision.GetComponent<HurtCollider>();
        hit?.NotifyHit(this);
    }
}
