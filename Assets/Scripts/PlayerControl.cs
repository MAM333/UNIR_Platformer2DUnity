using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    CharacterController2D characterController;
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;
    [SerializeField] InputActionReference punch;

    Life life;

    private void Awake()
    {
        characterController = GetComponent<CharacterController2D>();

        move.action.performed += OnMove;
        move.action.started += OnMove;
        move.action.canceled += OnMove;

        jump.action.performed += OnJump;
        punch.action.performed += OnPunch;

        life = GetComponent<Life>();
    }

    private void OnEnable()
    {
        move.action.Enable();
        jump.action.Enable();
        punch.action.Enable();

        life.onLifeDepleted.AddListener(OnLifeDepleted);
    }

    private void Update()
    {
        characterController.SetRawMove(rawMove);
    }

    private void OnDisable()
    {
        move.action.Disable();
        jump.action.Disable();
        punch.action.Disable();

        life.onLifeDepleted.RemoveListener(OnLifeDepleted);
    }

    Vector2 rawMove;
    private void OnMove(InputAction.CallbackContext ctx)
    {
        rawMove = ctx.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        characterController.Jump();
    }

    private void OnPunch(InputAction.CallbackContext ctx)
    {
        characterController.Punch();
    }

    private void OnLifeDepleted(float arg0)
    {
        gameObject.SetActive(false);
        Invoke(nameof(Resurrect), 2f);
    }

    private void Resurrect()
    {
        gameObject.SetActive(true);
        life.Restart();
    }
}
