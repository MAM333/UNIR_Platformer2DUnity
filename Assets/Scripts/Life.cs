using System;
using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour
{
    [SerializeField] float startLife = 1f;
    [SerializeField] float damagePerHit = 0.3f;
    HurtCollider hurtCollider;

    public UnityEvent<float, float> onLifeChanged;
    public UnityEvent<float> onLifeDepleted;

    float currentLife;

    private void Awake()
    {
        hurtCollider = GetComponent<HurtCollider>();
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

    private void OnHitReceived()
    {
        if (currentLife <= 0) return;

        currentLife -= damagePerHit;
        onLifeChanged.Invoke(currentLife, startLife);
        if (currentLife <= 0)
        {
            currentLife = 0;
            onLifeDepleted.Invoke(startLife);
        }
    }

    internal void Restart()
    {
        currentLife = startLife;
        onLifeChanged.Invoke(currentLife, startLife);
    }
}
