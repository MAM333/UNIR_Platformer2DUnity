using UnityEngine;

public class GreenThing : MonoBehaviour
{
    [SerializeField] float timeToResurrect = 1.5f;

    Life life;
    Animator anim;
    BoxCollider2D boxCol;

    private void Awake()
    {
        life = GetComponent<Life>();
        anim = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();

        anim.SetBool("Alive", true);
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
        SoundManager.instance.PlayStarBreaking();

        anim.SetBool("Alive", false);

        boxCol.enabled = false;

        Invoke(nameof(Revive), timeToResurrect);
    }

    private void Revive()
    {
        life.Restart();

        anim.SetBool("Alive", true);

        boxCol.enabled = true;
    }
}
