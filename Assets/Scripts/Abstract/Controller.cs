using System.Collections;
using UnityEngine;
[RequireComponent(typeof(MovementScript))]
public abstract class Controller : MonoBehaviour
{
    [SerializeField] private float shotCD=.1f;
    private bool canShoot = true;
    private int health = 100;
    private int shield = 50;
    public void Shoot(float _bloom=0)
    {
        if(canShoot)
        {
            print("Shoot");
            StartCoroutine(shootAction(_bloom));
        }
    }
    private IEnumerator shootAction(float _bloom)
    {
        canShoot = false;
        Debug.DrawRay(transform.position, Vector3.forward, Color.green, 5f);
        yield return new WaitForSecondsRealtime(shotCD);
        canShoot= true;
    }
    public void TakeDamage(int _dmg)
    {
        if(shield> 0)
        {
            shield -= _dmg;
            if (shield < 0) { health=-shield; }
        }
        else
        {
            health -=_dmg;
        }
        if (health <= 0)
        {
            Die();
        }

    }
    private void Die()
    {
        print(gameObject+" die");
    }
}
