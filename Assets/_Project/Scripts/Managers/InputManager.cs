using UnityEngine.InputSystem;
using System;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class InputManager : Singleton<InputManager>, PlayerControls.IPlayerActions
    {
        public event Action<Vector2> MoveEvent;
        public event Action JumpEvent;
        public event Action SlideEvent;

        public event Action MenuOpenCloseEvent;

        PlayerControls controls;

        protected override void OnEnable()
        {
            base.OnEnable();
            controls = new PlayerControls();
            controls.Player.SetCallbacks(this);
            controls.Player.Enable();
        }


        protected void OnDisable()
        {
            controls.Player.Disable();
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                MoveEvent?.Invoke(context.ReadValue<Vector2>());
            }
            else if (context.canceled)
            {
                MoveEvent?.Invoke(Vector2.zero);
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                JumpEvent?.Invoke();
            }
        }

        public void OnMenuOpenClose(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                MenuOpenCloseEvent?.Invoke();
            }
        }

        public void OnSlide(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SlideEvent?.Invoke();
            }
        }
    }
}