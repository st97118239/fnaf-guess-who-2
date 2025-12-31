using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 currentMovementInput;

    public override void OnStartClient()
    {
        if (IsOwner)
            GetComponent<PlayerInput>().enabled = true;
    }

    public void OnMove(InputValue value)
    {
        currentMovementInput = value.Get<Vector2>();
    }

    void Update()
    {
        if (!IsOwner)
            return;

        Vector3 moveDirection = new(currentMovementInput.x, 0f, currentMovementInput.y);
        if (moveDirection.magnitude > 1f)
            moveDirection.Normalize();

        transform.position += moveSpeed * Time.deltaTime * moveDirection;
    }
}
