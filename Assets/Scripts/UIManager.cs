using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] InputActionReference openMenu;
    [SerializeField] GameObject menu;

    private void Start()
    {
        openMenu.action.performed += OnOpenMenu;

        menu.SetActive(false);
    }

    private void OnEnable()
    {
        openMenu.action.Enable();
    }

    private void OnDisable()
    {
        openMenu.action.Disable();
    }

    private void OnDestroy()
    {
        openMenu.action.performed -= OnOpenMenu;
    }

    public void ToggleMenu()
    {
        menu.SetActive(!menu.activeSelf);
        if (menu.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void GoToMenu()
    {
        SceneManagement.instance.GoToLevel(0);
    }

    private void OnOpenMenu(InputAction.CallbackContext ctx)
    {
        ToggleMenu();
    }
}
