using UnityEngine;
[RequireComponent(typeof(EnemyMovement))]
public class EnemyController : Controller
{
    private EnemyType enemyType;
    public void SetType(EnemyType _enemyType)
    {
        enemyType = _enemyType;
    }
}
