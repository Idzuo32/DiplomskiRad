using Managers;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] float xClamp = 3f;

        Vector2 movement;
        Rigidbody rigidBody;

        bool canControl;
        void OnEnable()
        {
            if (InputManager.Instance)
            {
                InputManager.Instance.MoveEvent += i => movement = i;
            }
            if (GameManager.Instance)
            {
                GameManager.OnGameStart += HandleGameStart;
                GameManager.OnGameOver += HandleGameOver;
            }
        }

        void OnDisable()
        {
            if (InputManager.Instance)
            {
                InputManager.Instance.MoveEvent -= i => movement = i;
            }
            if (GameManager.Instance)
            {
                GameManager.OnGameStart -= HandleGameStart;
                GameManager.OnGameOver -= HandleGameOver;
            }
        }

        void HandleGameStart() => canControl = true;

        void HandleGameOver() => canControl = false;
        void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            if (!canControl) return;
            HandleMovement();
        }

        void HandleMovement()
        {
            var currentPosition = rigidBody.position;
            var moveDirection = new Vector3(movement.x, 0f, 0f);
            var newPosition = currentPosition + moveDirection * (moveSpeed * Time.fixedDeltaTime);

            newPosition.x = Mathf.Clamp(newPosition.x, -xClamp, xClamp);

            rigidBody.MovePosition(newPosition);
        }
    }
}