using UnityEngine;
using System.Collections;

[System.Serializable]
//////////////////////////////////////////////////////////////
// SidescrollControl.js
//
// SidescrollControl creates a 2D control scheme where the left
// pad is used to move the character, and the right pad is used
// to make the character jump.
//////////////////////////////////////////////////////////////
[UnityEngine.RequireComponent(typeof(CharacterController))]
public partial class SidescrollControl : MonoBehaviour
{
    // This script must be attached to a GameObject that has a CharacterController
    public Joystick moveTouchPad;
    public Joystick jumpTouchPad;
    public float forwardSpeed;
    public float backwardSpeed;
    public float jumpSpeed;
    public float inAirMultiplier; // Limiter for ground speed while jumping
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

    public virtual void OnEndGame()
    {
         // Disable joystick when the game ends	
        this.moveTouchPad.Disable();
        this.jumpTouchPad.Disable();
        // Don't allow any more control changes when the game ends
        this.enabled = false;
    }

    public virtual void Update()
    {
        Vector3 movement = Vector3.zero;
        // Apply movement from move joystick
        if (this.moveTouchPad.position.x > 0)
        {
            movement = (Vector3.right * this.forwardSpeed) * this.moveTouchPad.position.x;
        }
        else
        {
            movement = (Vector3.right * this.backwardSpeed) * this.moveTouchPad.position.x;
        }
        // Check for jump
        if (this.character.isGrounded)
        {
            bool jump = false;
            Joystick touchPad = this.jumpTouchPad;
            if (!touchPad.IsFingerDown())
            {
                this.canJump = true;
            }
            if (this.canJump && touchPad.IsFingerDown())
            {
                jump = true;
                this.canJump = false;
            }
            if (jump)
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
            // Adjust additional movement while in-air
            movement.x = movement.x * this.inAirMultiplier;
        }
        //		movement.z *= inAirMultiplier;
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
    }

    public SidescrollControl()
    {
        this.forwardSpeed = 4;
        this.backwardSpeed = 4;
        this.jumpSpeed = 16;
        this.inAirMultiplier = 0.25f;
        this.canJump = true;
    }

}