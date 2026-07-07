using UnityEngine;
using System.Collections;

[System.Serializable]
//////////////////////////////////////////////////////////////
// PlayerRelativeControl.js
// Penelope iPhone Tutorial
//
// PlayerRelativeControl creates a control scheme similar to what
// might be found in a 3rd person, over-the-shoulder game found on
// consoles. The left stick is used to move the character, and the
// right stick is used to rotate the character. A quick double-tap
// on the right joystick will make the character jump.
//////////////////////////////////////////////////////////////
[UnityEngine.RequireComponent(typeof(CharacterController))]
public partial class PlayerRelativeControl : MonoBehaviour
{
    // This script must be attached to a GameObject that has a CharacterController
    public Joystick moveJoystick;
    public Joystick rotateJoystick;
    public Transform cameraPivot; // The transform used for camera rotation
    public float forwardSpeed;
    public float backwardSpeed;
    public float sidestepSpeed;
    public float jumpSpeed;
    public float inAirMultiplier; // Limiter for ground speed while jumping
    public Vector2 rotationSpeed; // Camera rotation speed for each axis
    private Transform thisTransform;
    private CharacterController character;
    private Vector3 cameraVelocity;
    private Vector3 velocity; // Used for continuing momentum while in air
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
        Vector3 movement = this.thisTransform.TransformDirection(new Vector3(this.moveJoystick.position.x, 0, this.moveJoystick.position.y));
        // We only want horizontal movement
        movement.y = 0;
        movement.Normalize();
        Vector3 cameraTarget = Vector3.zero;
        // Apply movement from move joystick
        Vector2 absJoyPos = new Vector2(Mathf.Abs(this.moveJoystick.position.x), Mathf.Abs(this.moveJoystick.position.y));
        if (absJoyPos.y > absJoyPos.x)
        {
            if (this.moveJoystick.position.y > 0)
            {
                movement = movement * (this.forwardSpeed * absJoyPos.y);
            }
            else
            {
                movement = movement * (this.backwardSpeed * absJoyPos.y);
                cameraTarget.z = this.moveJoystick.position.y * 0.75f;
            }
        }
        else
        {
            movement = movement * (this.sidestepSpeed * absJoyPos.x);
            // Let's move the camera a bit, so the character isn't stuck under our thumb
            cameraTarget.x = -this.moveJoystick.position.x * 0.5f;
        }
        // Check for jump
        if (this.character.isGrounded)
        {
            if (this.rotateJoystick.tapCount == 2)
            {
                 // Apply the current movement to launch velocity		
                this.velocity = this.character.velocity;
                this.velocity.y = this.jumpSpeed;
            }
        }
        else
        {
             // Apply gravity to our velocity to diminish it over time
            this.velocity.y = this.velocity.y + (Physics.gravity.y * Time.deltaTime);
            // Move the camera back from the character when we jump
            cameraTarget.z = -this.jumpSpeed * 0.25f;
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
        // Seek camera towards target position
        Vector3 pos = this.cameraPivot.localPosition;
        pos.x = Mathf.SmoothDamp(pos.x, cameraTarget.x, ref this.cameraVelocity.x, 0.3f);
        pos.z = Mathf.SmoothDamp(pos.z, cameraTarget.z, ref this.cameraVelocity.z, 0.5f);
        this.cameraPivot.localPosition = pos;
        // Apply rotation from rotation joystick
        if (this.character.isGrounded)
        {
            Vector2 camRotation = this.rotateJoystick.position;
            camRotation.x = camRotation.x * this.rotationSpeed.x;
            camRotation.y = camRotation.y * this.rotationSpeed.y;
            camRotation = camRotation * Time.deltaTime;
            // Rotate the character around world-y using x-axis of joystick
            this.thisTransform.Rotate(0, camRotation.x, 0, Space.World);
            // Rotate only the camera with y-axis input
            this.cameraPivot.Rotate(camRotation.y, 0, 0);
        }
    }

    public PlayerRelativeControl()
    {
        this.forwardSpeed = 4;
        this.backwardSpeed = 1;
        this.sidestepSpeed = 1;
        this.jumpSpeed = 8;
        this.inAirMultiplier = 0.25f;
        this.rotationSpeed = new Vector2(50, 25);
    }

}