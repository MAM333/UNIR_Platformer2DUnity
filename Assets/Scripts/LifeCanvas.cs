using System;
using UnityEngine;
using UnityEngine.UI;

public class LifeCanvas : MonoBehaviour
{
    [SerializeField] Life life;
    [SerializeField] Image mask;

    private void OnEnable()
    {
        life.onLifeChanged.AddListener(OnLifeChanged);
        life.onLifeDepleted.AddListener(OnLifeDepleted);
    }

    private void OnDisable()
    {
        life.onLifeChanged.RemoveListener(OnLifeChanged);
        life.onLifeDepleted.RemoveListener(OnLifeDepleted);
    }

    private void OnLifeChanged(float currentLife, float startLife)
    {
        mask.fillAmount = currentLife / startLife;
    }

    private void OnLifeDepleted(float startLife)
    {
        mask.fillAmount = 0;
    }
}
