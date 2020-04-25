﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager = null;
    [SerializeField] private EnemyBasicPopulation enemyManager = null;
    [SerializeField] private Enemy2Population enemyManager2 = null;

    private int waveNumber = 0;

    //The amount of time before the next wave begins.
    [SerializeField] private float timeBetweenWaves = 30f;
    [SerializeField] private float timeBetweenWavesLeft = 30f; //TODO: Remove this serialize from inspector so its not set.

    private bool inBreakPeriod = false;
    //The amount of time in a break, which happens every now and then. //TODO: Determine which wave to have breaks after. Every 5?
    [SerializeField] private float breakPeriodTime = 60f;
    private float breakPeriodTimeLeft = 60f;

    public void StartWaves()
    {
        StartCoroutine(TimeBetweenWavesTickDown());
        StartNextWave();
    }

    public void StartNextWave()
    {
        waveNumber++;

        //Check for special waves here. For break periods for example. Start Ticking down time before next wave.
        if (!(waveNumber %5 == 0 && waveNumber!= 0))
        {
            SpawnEnemies();
        }
        else //Every 5 waves, have a break.
        {
            StartCoroutine(BreakPeriodTickDown());
        }
        
    }

    private void SpawnEnemies()
    {
        levelManager.SetWaveActive();
        enemyManager.SpawnWaveEnemies();
        enemyManager2.SpawnEnemy2();
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }

    IEnumerator BreakPeriodTickDown()
    {
        while (inBreakPeriod)
        {
            yield return new WaitForSeconds(1);
            breakPeriodTimeLeft -= 1;
            //Can change timeLeftText display here if we have one.
            if (breakPeriodTimeLeft <= 0)
            {
                inBreakPeriod = false;
                breakPeriodTimeLeft = breakPeriodTime;
                //End break period. Next wave starts, so spawn the enemies. The TimeBetweenWavesTickDown will then take over again.
                SpawnEnemies(); 
            }
        }
    }

    IEnumerator TimeBetweenWavesTickDown()
    {
        while(levelManager.GetLostGame() != true) //As long as player hasnt lost.
        {
            if(!inBreakPeriod) //If not in a break, tick down to next wave.
            {
                //Tick time down.
                yield return new WaitForSeconds(1f);
                timeBetweenWavesLeft -= 1;
                //If time is 0, reset time and start new wave.
                if (timeBetweenWavesLeft <= 0)
                {
                    timeBetweenWavesLeft = timeBetweenWaves;
                    StartNextWave();
                }
            }
            else
            {
                yield return new WaitForSeconds(1f); //Just wait til break over.
            }
        }
    }
}
