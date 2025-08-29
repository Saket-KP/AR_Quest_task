using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(CharacterController))]
public class XRJump : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Input Action")]
    public InputActionProperty jumpAction; // Bind to Space (keyboard) and PrimaryButton (controller)

    private CharacterController characterController;
    private XROrigin xrOrigin;
    private float verticalVelocity = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        xrOrigin = GetComponent<XROrigin>();
    }

    void Update()
    {
        // Apply gravity
        if (characterController.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -1f; // Small push to keep grounded
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        // Check jump input
        if (jumpAction.action.WasPressedThisFrame() && characterController.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply vertical movement
        Vector3 move = new Vector3(0, verticalVelocity, 0);
        characterController.Move(move * Time.deltaTime);
    }
}
