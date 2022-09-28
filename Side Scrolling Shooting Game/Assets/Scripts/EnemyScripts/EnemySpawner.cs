using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField]private Transform spawnPoint;
    [SerializeField]private BaseEnemy enemyPrefab;
    [Min(0)]
    [SerializeField]private int enemyCount = 8;

    [Range(0.5f,2.0f)]
    [SerializeField]private float spawnInterval = 1.0f;
    private bool _shouldSpawn = true;

    private IEnumerator SpawnEnemies(Player player)
    {
        int i = 0;
        while(i < enemyCount)
        {
            BaseEnemy enemy = Instantiate(enemyPrefab,spawnPoint.position,Quaternion.identity);
            enemy.Target = player.transform;
            yield return new WaitForSeconds(spawnInterval);
            i++;
        }
    }


    private void OnTriggerEnter(Collider other) 
    {
        Player player = other.GetComponent<Player>();
        if(player != null && _shouldSpawn == true)
        {
            //Debug.Log("Spawning Enemies");
            StartCoroutine(SpawnEnemies(player));
            _shouldSpawn = false;            
        }    
    }
}
