using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;
    private bool _stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
       while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(3.0f);

            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy =  Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
                  
        }  
    }

    IEnumerator SpawnPowerupRoutine()
    {
       while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(3.0f);

            Vector3 posToSpawn = new Vector3(Random.Range(-9f, 0f), 7, 0);
            int randomPowerUP = Random.Range(0, 3);
            GameObject newPowerUp = Instantiate(_powerups[randomPowerUP], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
     

    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
