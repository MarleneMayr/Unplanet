#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -10f;

#if ENABLE_INPUT_SYSTEM
    InputAction movement;

    void Start()
    {
        movement = new InputAction("PlayerMovement", binding: "<Gamepad>/leftStick");
        movement.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");

        movement.Enable();
    }
#endif

    // Update is called once per frame
    void Update()
    {
        float x;
        float z;

#if ENABLE_INPUT_SYSTEM
        var delta = movement.ReadValue<Vector2>();
        x = delta.x;
        z = delta.y;
#else
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
#endif

        Vector3 move = transform.right * x + transform.forward * z + transform.up * gravity;
        controller.Move(move * speed * Time.deltaTime);
    }
}