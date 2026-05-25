using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public GameObject button;
    //public UnityEvent canInteract;
    //public UnityEvent cannotInteract;

    private void Awake()
    {
        ShowButton(false);
    }

    bool canInteract = false;
    Tirolina tirolina;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tirolina"))
        {
            canInteract = true;
            //ShowButton(true);
            tirolina = collision.gameObject.GetComponent<Tirolina>();
        }
    }

    private void ShowButton(bool show)
    {
        button.SetActive(show);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tirolina"))
        {
            canInteract = false;
            tirolina = null;
            //ShowButton(false);
        }
    }

    public void Interact(CharacterController2D characterController)
    {
        bool moving = characterController.movingInTirolina;
        if (!canInteract && !moving) return;

        if (tirolina != null || moving)
        {
            bool done = characterController.Tirolina(tirolina);

        }
    }
}
