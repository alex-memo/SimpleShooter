using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PlayerMovement))]
public class UserController : Controller
{
    public static UserController Instance;
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else {DestroyImmediate(gameObject); }
        loadPlayer();
        StartCoroutine(savePlayer());
        Physics.SyncTransforms();
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
    /// <summary>
    /// Saves the player position every 15 seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator savePlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(15);
            SaveSystem.SavePlayerPosition();
        }
    }
    private void loadPlayer()
    {
        SaveSystem.LoadPlayerPosition();
    }
}
