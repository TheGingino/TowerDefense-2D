using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnState { SPAWNING, WAITING };

[System.Serializable]
public class Wave
{
    public string name;
    public Enemy[] enemies;
}
[System.Serializable]
public class Enemy
{
    public Transform enemyPrefab;
    public int count;
    public float spawnIntervalInSeconds;
}

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;
    private int nextWave=-1;
    private SpawnState state = SpawnState.WAITING;
    private float timeBetweenWave = 6f;

    private void Update()
    {
        if (state == SpawnState.WAITING){
            if (!EnemyIsAlive())
            {
                StartCoroutine(beginNewRound());
            }
        }
    }

    private IEnumerator beginNewRound()
    {
        state = SpawnState.SPAWNING;
        yield return new WaitForSeconds(timeBetweenWave);
        if(nextWave + 1 > waves.Length -1)
        {
            nextWave = 0;
        }
        else
        {
            nextWave++;
        }

        StartCoroutine(SpawnWave(waves[nextWave]));
    }

    private bool EnemyIsAlive()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length > 0;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        for(int i =0; i< _wave.enemies.Length; i++)
        {
            var temp = _wave.enemies[i];
            for(int j =0; j< temp.count; j++)
            {
                SpawnEnemy(temp.enemyPrefab);
                yield return new WaitForSeconds(temp.spawnIntervalInSeconds);
            }
        }

        state = SpawnState.WAITING;
        yield break;
    }


    void SpawnEnemy(Transform _enemy)
    {
        Instantiate(_enemy, transform.position, transform.rotation);
    }
}