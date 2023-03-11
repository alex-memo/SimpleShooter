using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class Spawner : MonoBehaviour
{   

    [SerializeField] private int xDiff = 0;
    [SerializeField] private int zDiff = 0;
    [SerializeField] private int numberEnemy = 10;   
    [SerializeField] private int respawnTime = 90;
    [SerializeField] private int spawnRadius = 40;
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private GameObject[] enemyArray;

    private bool isRunning;

    private int xPos;
    private int zPos;
    private int enemyCount;
    private int playersInside = 0;


    private List<GameObject> enemies = new List<GameObject>();

    private void Start()
    {
        GetComponent<BoxCollider>().size= new Vector3(spawnRadius, 80, spawnRadius);
    }
    private IEnumerator EnemyDrop()
    {
        isRunning = true;
        while (isRunning)
        {
            spawnEnemies();
            yield return new WaitForSeconds(respawnTime);
        }

    }
    private void spawnEnemies()
    {
        while (enemyCount < numberEnemy)
        {
            xPos = Random.Range((int)(transform.position.x) - (xDiff - 1), (int)(transform.position.x) + (xDiff - 1));
            zPos = Random.Range((int)(transform.position.z) - (zDiff - 1), (int)(transform.position.z) + (zDiff - 1));
            try
            {
                GameObject _enemyObject = Instantiate(enemyArray[(int)enemyType], new Vector3(xPos, (transform.position.y), zPos), Quaternion.identity);
                EnemyController _enemy = _enemyObject.GetComponent<EnemyController>();

                _enemy.SetType(enemyType);
                enemies.Add(_enemyObject);
                enemyCount += 1;
            }
            catch
            {
                Debug.Log($"Please add an enemy matching {enemyType}!");
            }

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), new Vector3(xDiff * 2, 2, zDiff * 2));
        Gizmos.color = Color.cyan;
    }
    private void OnTriggerEnter(Collider _coll)
    {
        if (_coll.CompareTag("Player"))
        {
            playersInside += 1;
            if (!isRunning)
            {
                StartCoroutine(EnemyDrop());
            }
        }
    }
    private void OnTriggerExit(Collider _coll)
    {
        if (_coll.CompareTag("Player"))
        {
            playersInside -= 1;
            if (playersInside <= 0)
            {
                isRunning= false;
            }
        }          
    }   
}
public enum EnemyType { Range, Melee }
