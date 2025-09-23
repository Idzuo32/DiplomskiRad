using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
  [RequireComponent(typeof(Rigidbody))]
  public class PlayerController : MonoBehaviour {

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float xClamp = 3f;

    Vector2 movement;
    Rigidbody rigidBody;

    void Start() {
      if ( InputManager.Instance ) {
        InputManager.Instance.MoveEvent += i => movement = i;
      }
    }

    void OnDisable() {
      if ( InputManager.Instance ) {
        InputManager.Instance.MoveEvent -= i => movement = i;
      }
    }
    void Awake() {
      rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
      HandleMovement();
    }

    void HandleMovement() {
      var currentPosition = rigidBody.position;
      var moveDirection = new Vector3(movement.x, 0f, 0f);
      var newPosition = currentPosition + moveDirection * (moveSpeed * Time.fixedDeltaTime);

      newPosition.x = Mathf.Clamp(newPosition.x, -xClamp, xClamp);

      rigidBody.MovePosition(newPosition);
    }
  }
}
