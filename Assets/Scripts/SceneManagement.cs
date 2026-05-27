using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement instance;

    [SerializeField] InputActionReference restartLevel;

    private void Awake()
    {
        instance = this;

        restartLevel.action.performed += OnRestart;
    }

    private void OnEnable()
    {
        restartLevel.action.Enable();
    }

    private void Start()
    {

        Scene act = SceneManager.GetActiveScene();
        if (act.name == "MainMenu" || act.name == "FinalScene")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            MusicManager.instance.PlayMainMenuTheme();
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            MusicManager.instance.PlayGameTheme();
        }

        Time.timeScale = 1f;
    }

    private void OnDisable()
    {
        restartLevel.action.Disable();
    }

    private void OnDestroy()
    {
        restartLevel.action.performed -= OnRestart;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GoToLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    private void OnRestart(InputAction.CallbackContext context)
    {
        ReloadScene();
    }
}
