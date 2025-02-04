using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    [SerializeField] private float enemySpawnTimer = 7f;
    //[SerializeField] private float attackDamage = 50f;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("sup");
        StartCoroutine(SpawnEnemy());
    }

    // Will spawn 10 enemies
    private IEnumerator SpawnEnemy()
    {
        for(int i = 0; i < 10; i++)
        {
            WaitForSeconds wait = new WaitForSeconds(enemySpawnTimer);

            yield return new WaitForSeconds(enemySpawnTimer);
            
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);

            if (i == 9)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D hit){
        Destroy(gameObject);

    }
}