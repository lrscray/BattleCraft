using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager = null;
    [SerializeField] private EnemyBasicPopulation enemyManager = null;

    public void StartWave()
    {
        levelManager.SetWaveActive();
        enemyManager.SpawnWaveEnemies();

    }
}
