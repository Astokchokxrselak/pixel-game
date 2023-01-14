using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Player : MonoBehaviour
{
    Rigidbody2D m_rigidbody2D;
    Collider2D m_collider2D;
    
    // Start is called before the first frame update
    void Start()
    {
        CastArray = new RaycastHit2D[2]; // Only need two elements; one for the player, and one for the platform/object below
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_collider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        InputUpdate();
    }

    float horizontal; // Used for movement along the x-axis
    bool wantsToJump; // On when the player presses W or up arrow; off when the player has neither of these pressed.
    void InputUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        wantsToJump = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
    }


    private void FixedUpdate()
    {
        PhysicsUpdate(Time.fixedDeltaTime);
    }

    public const float Gravity = 15f; // The gravity, how fast this accelerates while falling per frame
    public const float MaxFallSpeed = -10f; // The maximum possible fall speed / y component of velocity
    public const float MaxSpeed = 5f; // The maximum speed at which the player moves along the x axis
    public const float JumpSpeed = 7f; // The velocity at which the player jumps along the y axis
    private void PhysicsUpdate(float delta) // This is where platformer physics logic is handled
    {
        var velocity = m_rigidbody2D.velocity; // We do this so we can change the components of the velocity individually
        velocity.y -= Gravity * delta; // Add gravity to velocity.y; multiply by delta so it doesnt fall at an absurdly high speed

        // The if statements below evaluate whether the player is pressing left arrow (-1) or right arrow (1) or neither (0)
        if (horizontal == 1) // If the player is pressing a right key (D, right arrow)
            velocity.x = MaxSpeed; // Move to the right 
        else if (horizontal == -1) // If the player is pressing a left key (A, left arrow)
            velocity.x = -MaxSpeed; // Move to the left
        else // If the player is not pressing any key this frame
            velocity.x = 0; // Stop moving

        if (IsOnFloor()) // If we are grounded
        {
            if (wantsToJump) // If the player wants to jump
            {
                velocity.y = JumpSpeed; // Jump into the air
            }
            else
            {
                velocity.y = 0; // Set velocity.y to 0, as we should not be falling
            }
        }

        m_rigidbody2D.velocity = velocity; // Assign the rigidbody.velocity to velocity, as we have finished modifying the individual components
    }


    private const float RaycastLength = 0.01f;
    private static RaycastHit2D[] CastArray; // used for nonallocating raycasting
    bool IsOnFloor() // manual implementation of is_on_floor() from godot since there is no builtin unity equivalent
    {
        var halfExtentY = m_collider2D.bounds.size.y; // Distance from the top to the center of the rectangular bounds
        Physics2D.RaycastNonAlloc(transform.position - halfExtentY * Vector3.up, Vector2.down, CastArray, RaycastLength); // Cast a ray from the bottom of the Player with a distance of RaycastLength
        return CastArray[0].transform != null; // One of these elements should never be null, the other should only be null if there is no platform below the player
    }
}
