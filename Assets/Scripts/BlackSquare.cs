using UnityEngine;

public class BlackSquare : MonoBehaviour
{
    public static BlackSquare instance;

    Animator anim;

    bool reload = false;
    private void Awake()
    {
        instance = this;

        anim = GetComponent<Animator>();
    }

    public void End(bool rel)
    {
        anim.SetTrigger("End");
        reload = rel;
    }

    public void OnEndAnimation()
    {
        if (!reload)
        {
            SceneManagement.instance.GoToNextLevel();
        }
        else
        {
            SceneManagement.instance.ReloadScene();
        }
    }
}
