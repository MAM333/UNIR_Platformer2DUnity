using UnityEngine;

public class WoodBox : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;

    Life life;
    BoxCollider2D boxCol;
    SpriteRenderer spr;

    private void Awake()
    {
        life = GetComponent<Life>();
        boxCol = GetComponent<BoxCollider2D>();
        spr = GetComponent<SpriteRenderer>();

        particles.Stop();
    }

    private void OnEnable()
    {
        life.onLifeDepleted.AddListener(OnLifeDepleted);
    }

    private void OnDisable()
    {
        life.onLifeDepleted.AddListener(OnLifeDepleted);
    }

    private void OnLifeDepleted(float arg)
    {
        SoundManager.instance.PlayDamageToBox();

        spr.enabled = false;
        boxCol.enabled = false;

        particles.Play();
    }

}
