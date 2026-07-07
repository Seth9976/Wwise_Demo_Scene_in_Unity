using UnityEngine;
using System.Collections;

[System.Serializable]
//////////////////////////////////////////////////////////////
// CameraRelativeControl.js
// Penelope iPhone Tutorial
//
// CameraRelativeControl creates a control scheme similar to what
// might be found in 3rd person platformer games found on consoles.
// The left stick is used to move the character, and the right
// stick is used to rotate the camera around the character.
// A quick double-tap on the right joystick will make the 
// character jump. 
//////////////////////////////////////////////////////////////
// This script must be attached to a GameObject that has a CharacterController
[UnityEngine.RequireComponent(typeof(CharacterController))]
public partial class CameraRelativeControl : MonoBehaviour
{
    public Joystick moveJoystick;
    public Joystick rotateJoystick;
    public Transform cameraPivot; // The transform used for camera rotation
    public Transform cameraTransform; // The actual transform of the camera
    public float speed; // Ground speed
    public float jumpSpeed;
    public float inAirMultiplier; // Limiter for ground speed while jumping
    public Vector2 rotationSpeed; // Camera rotation speed for each axis
    private Transform thisTransform;
    private CharacterController character;
    private Vector3 velocity; // Used for continuing momentum while in air
    private bool canJump;
    public virtual void Start()
    {
         // Cache component lookup at startup instead of doing this every frame	
        this.thisTransform = (Transform) this.GetComponent(typeof(Transform));
        this.character = (CharacterController) this.GetComponent(typeof(CharacterController));
        // Move the character to the correct start position in the level, if one exists
        GameObject spawn = GameObject.Find("PlayerSpawn");
        if (spawn)
        {
            this.thisTransform.position = spawn.transform.position;
        }
    }

    public virtual void FaceMovementDirection()
    {
        Vector3 horizontalVelocity = this.character.velocity;
        horizontalVelocity.y = 0; // Ignore vertical movement
        // If moving significantly in a new direction, point that character in that direction
        if (horizontalVelocity.magnitude > 0.1f)
        {
            this.thisTransform.forward = horizontalVelocity.normalized;
        }
    }

    public virtual void OnEndGame()
    {
         // Disable joystick when the game ends	
        this.moveJoystick.Disable();
        this.rotateJoystick.Disable();
        // Don't allow any more control changes when the game ends
        this.enabled = false;
    }

    public virtual void Update()
    {
        Vector3 movement = this.cameraTransform.TransformDirection(new Vector3(this.moveJoystick.position.x, 0, this.moveJoystick.position.y));
        // We only want the camera-space horizontal direction
        movement.y = 0;
        movement.Normalize(); // Adjust magnitude after ignoring vertical movement
        // Let's use the largest component of the joystick position for the speed.
        Vector2 absJoyPos = new Vector2(Mathf.Abs(this.moveJoystick.position.x), Mathf.Abs(this.moveJoystick.position.y));
        movement = movement * (this.speed * (absJoyPos.x > absJoyPos.y ? absJoyPos.x : absJoyPos.y));
        // Check for jump
        if (this.character.isGrounded)
        {
            if (!this.rotateJoystick.IsFingerDown())
            {
                this.canJump = true;
            }
            if (this.canJump && (this.rotateJoystick.tapCount == 2))
            {
                 // Apply the current movement to launch velocity
                this.velocity = this.character.velocity;
                this.velocity.y = this.jumpSpeed;
                this.canJump = false;
            }
        }
        else
        {
             // Apply gravity to our velocity to diminish it over time
            this.velocity.y = this.velocity.y + (Physics.gravity.y * Time.deltaTime);
            // Adjust additional movement while in-air
            movement.x = movement.x * this.inAirMultiplier;
            movement.z = movement.z * this.inAirMultiplier;
        }
        movement = movement + this.velocity;
        movement = movement + Physics.gravity;
        movement = movement * Time.deltaTime;
        // Actually move the character
        this.character.Move(movement);
        if (this.character.isGrounded)
        {
            // Remove any persistent velocity after landing
            this.velocity = Vector3.zero;
        }
        // Face the character to match with where she is moving
        this.FaceMovementDirection();
        // Scale joystick input with rotation speed
        Vector2 camRotation = this.rotateJoystick.position;
        camRotation.x = camRotation.x * this.rotationSpeed.x;
        camRotation.y = camRotation.y * this.rotationSpeed.y;
        camRotation = camRotation * Time.deltaTime;
        // Rotate around the character horizontally in world, but use local space
        // for vertical rotation
        this.cameraPivot.Rotate(0, camRotation.x, 0, Space.World);
        this.cameraPivot.Rotate(camRotation.y, 0, 0);
    }

    public CameraRelativeControl()
    {
        this.speed = 5;
        this.jumpSpeed = 8;
        this.inAirMultiplier = 0.25f;
        this.rotationSpeed = new Vector2(50, 25);
        this.canJump = true;
    }

}