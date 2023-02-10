using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// @memo 2023
/// Script for the player movement
/// </summary>
 [RequireComponent(typeof(UserController))]
public class PlayerMovement : MovementScript
{
    /// <summary>
    /// @memo 2023
    /// Called externally by new input system to set the movement value
    /// </summary>
    /// <param name="_value"></param>
    private void OnMovement(InputValue _value)
    {
        Vector2 _v2 = _value.Get<Vector2>();
        direction = new Vector3(_v2.x,0,_v2.y);
    }
    /// <summary>
    /// @memo 2023
    /// Called externally by new input system to run
    /// </summary>
    /// <param name="_value"></param>
    private void OnRun(InputValue _value)
    {
        isRunning = _value.Get<float>() > 0;
    }
    /// <summary>
    /// @memo 2023
    /// Called externally by new input system to jump
    /// </summary>
    private void OnJump()
    {
        if (isGrounded)
        {
            triggerAnim("Jump");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);           
        }
    }
}
