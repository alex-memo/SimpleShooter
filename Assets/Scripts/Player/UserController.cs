using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PlayerMovement))]
[DefaultExecutionOrder(1)]
public class UserController : Controller
{
    public static UserController Instance;
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { print("Destroy"+Instance.gameObject.name); DestroyImmediate(gameObject); }
    }
    /// <summary>
    /// @memo 2023
    /// Called externally by new input system to run
    /// </summary>
    /// <param name="_value"></param>
    private void OnShoot(InputValue _value)
    {
        if(_value.Get<float>() > 0)
        {
            Shoot();
        }
    }
}
