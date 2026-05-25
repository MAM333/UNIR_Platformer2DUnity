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
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            MusicManager.instance.PlayGameTheme();
        } 
        else
        {
            MusicManager.instance.PlayMainMenuTheme();
        }
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

    private void OnRestart(InputAction.CallbackContext context)
    {
        ReloadScene();
    }
}
