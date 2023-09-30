using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState {SPAWNING, WAITING, COUNTING  };

    [System.Serializable]

    public class Wave
    {
        public string name;
        public Enemy[] enemies;
    }
    [System.Serializable]

    public class Enemy
    {
        public Transform enemyTransform;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int nextWave=0;
    public float timeBetweenWaves ;
    public float waveCountdown;
    private float searchTimer ;
    private SpawnState state = SpawnState.COUNTING;

    private void Start()
    {
        waveCountdown = timeBetweenWaves;
    }
    private void Update()
    {
        if (state == SpawnState.WAITING){
            if (!EnemyIsAlive())
            {
                beginNewRound();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    private void beginNewRound()
    {
        Debug.Log("Wave Completed");

        state= SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if(nextWave + 1 > waves.Length -1)
        {
            nextWave = 0;
            Debug.Log("Done");
        }
        else
        {
            nextWave++;
        }
    }

    private bool EnemyIsAlive()
    {
        searchTimer= Time.deltaTime;
        if(searchTimer <=0f)
        {
            searchTimer = 1f;
            if(GameObject.FindGameObjectsWithTag("Enemy") == null)
            {
                return false;
            }
        }

        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("night");
        state = SpawnState.SPAWNING;

        for(int i =0; i< _wave.enemies.Length; i++)
        {
            var temp = _wave.enemies[i];
            for(int j =0; j< temp.count; j++)
            {
                SpawnEnemy(temp.enemyTransform);
                yield return new WaitForSeconds(1f / temp.rate);
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
