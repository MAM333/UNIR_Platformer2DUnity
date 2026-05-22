using UnityEngine;
using UnityEngine.Events;

public class HurtCollider : MonoBehaviour
{
    public UnityEvent<bool, bool> onHitReceive; // agressorIsRight | isDownAttack

    internal void NotifyHit(HitCollider collider, bool agressorIsRight, bool isDownAttack)
    {
        onHitReceive.Invoke(agressorIsRight, isDownAttack);
    }
}
