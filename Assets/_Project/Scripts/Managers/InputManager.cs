using UnityEngine.InputSystem;
using System;
using UnityEngine;

namespace Managers {
  public class InputManager : MonoBehaviour, PlayerControls.IPlayerActions {

    public static InputManager Instance { get; private set; }

    public event Action<Vector2> MoveEvent;
    public event Action JumpEvent;
    public event Action SlideEvent;

    public event Action MenuOpenCloseEvent;

    PlayerControls controls;
    
    void Awake() {
      if ( Instance != null ) { Destroy(gameObject); return; }
      Instance = this;
      DontDestroyOnLoad(gameObject);

      controls = new PlayerControls();
      controls.Player.SetCallbacks(this);
      controls.Player.Enable();
    }
    public void OnMove(InputAction.CallbackContext context) {
      if ( context.performed ) {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
      } else if ( context.canceled ) {
        MoveEvent?.Invoke(Vector2.zero);
      }
    }

    public void OnJump(InputAction.CallbackContext context) {
      if ( context.performed ) {
        JumpEvent?.Invoke();
      }
    }

    public void OnMenuOpenClose(InputAction.CallbackContext context) {
      if ( context.performed ) {
        MenuOpenCloseEvent?.Invoke();
      }
    }

    public void OnSlide(InputAction.CallbackContext context) {
      if ( context.performed ) {
        SlideEvent?.Invoke();
      }
    }
  }
}