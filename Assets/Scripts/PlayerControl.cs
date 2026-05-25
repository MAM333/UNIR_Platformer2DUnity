using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    CharacterController2D characterController;
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;
    [SerializeField] InputActionReference punch;
    [SerializeField] InputActionReference dash;
    [SerializeField] InputActionReference throwWeapon;
    [SerializeField] InputActionReference interact;
    [SerializeField] InputActionReference lookDown;

    Vector3 initPosition;
    Life life;
    PlayerInteract playerInteract;
    SpriteRenderer sprRenderer;

    private void Awake()
    {
        characterController = GetComponent<CharacterController2D>();
        sprRenderer = GetComponent<SpriteRenderer>();

        move.action.performed += OnMove;
        move.action.started += OnMove;
        move.action.canceled += OnMove;
        lookDown.action.performed += OnLookDown;
        lookDown.action.started += OnLookDown;
        lookDown.action.canceled += OnLookDown;

        jump.action.performed += OnJump;
        punch.action.performed += OnPunch;
        dash.action.performed += OnDash;
        interact.action.performed += OnInteract;
        throwWeapon.action.performed += OnThrowWeapon;

        life = GetComponent<Life>();
        playerInteract = GetComponent<PlayerInteract>();
        initPosition = transform.position;
    }

    private void OnEnable()
    {
        move.action.Enable();
        lookDown.action.Enable();
        jump.action.Enable();
        punch.action.Enable();
        dash.action.Enable();
        interact.action.Enable();
        throwWeapon.action.Enable();

        life.onLifeChanged.AddListener(OnLifeChanged);
        life.onLifeDepleted.AddListener(OnLifeDepleted);
    }

    private void Update()
    {
        characterController.SetRawMove(rawMove);
    }

    private void OnDisable()
    {
        move.action.Disable();
        lookDown.action.Disable();
        jump.action.Disable();
        punch.action.Disable();
        dash.action.Disable();
        interact.action.Disable();
        throwWeapon.action.Disable();

        life.onLifeChanged.RemoveListener(OnLifeChanged);
        life.onLifeDepleted.RemoveListener(OnLifeDepleted);
    }

    private void OnDestroy()
    {
        move.action.performed -= OnMove;
        move.action.started -= OnMove;
        move.action.canceled -= OnMove;

        lookDown.action.performed -= OnLookDown;
        lookDown.action.started -= OnLookDown;
        lookDown.action.canceled -= OnLookDown;

        jump.action.performed -= OnJump;
        punch.action.performed -= OnPunch;
        dash.action.performed -= OnDash;
        interact.action.performed -= OnInteract;
        throwWeapon.action.performed -= OnThrowWeapon;
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

    private void OnDash(InputAction.CallbackContext ctx)
    {
        characterController.Dash();
    }

    private void OnThrowWeapon(InputAction.CallbackContext ctx)
    {
        characterController.ThrowWeapon();
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        playerInteract.Interact(characterController);
    }

    private void OnLifeChanged(float arg0, float arg1, bool damage)
    {
        if (damage)
        {
            SoundManager.instance.PlayDamageToPlayer();
        }
    }

    private void OnLifeDepleted(float arg0)
    {
        sprRenderer.enabled = false;
        BlackSquare.instance.End(true);
    }

    private void OnLookDown(InputAction.CallbackContext ctx)
    {
        Vector2 vec = ctx.ReadValue<Vector2>();
        characterController.LookingDown(vec.y < 0);
    }
}
