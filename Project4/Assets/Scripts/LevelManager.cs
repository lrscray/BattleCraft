using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //Set starting resources to 0.
    //Have starting troops.
    //Tick down time for 3 minutes.

    [SerializeField] private WaveManager waveManager = null;
    [SerializeField] private EnemyBasicPopulation enemyManager = null;
    [SerializeField] private CivilianPopulation civilianManager = null;

    private bool inStartingPeriod = true;
    private float warmupTimeLeft = 1f;//180f;//3 mins. //TODO: Make sure we change this back to 3 mins.

    private bool waveActive = false;

    private void Start()
    {
        //Set starting resources to 0.
        //Have starting troops.

        //Start tick down time for 3 minutes.
        StartCoroutine(TickTimeDown());
    }

    IEnumerator TickTimeDown()
    {
        while(inStartingPeriod)
        {
            yield return new WaitForSeconds(1);
            warmupTimeLeft -= 1;
            //Can change timeLeftText display here if we have one.
            if(warmupTimeLeft <= 0)
            {
                inStartingPeriod = false;
                //End warmup period. Wave starts.
                waveManager.StartWave();
                yield return new WaitForSeconds(3); //Allow some time for enemies to spawn.
                StartCoroutine(CheckWinLoseConditions());
            }
        }
    }

    IEnumerator CheckWinLoseConditions()
    {
        while(waveActive)
        {
            yield return 0; //MAYBE: Consider waiting a certain number of time before checking win/lose conditions?
            //Check win/lose conitions.
            //Win = all enemies dead.
            //Check enemy manager for number of alive enemies?
            if(enemyManager.GetNumCurrentEnemies() <= 0)
            {
                //Won wave/game.
                Debug.Log("Won Game!");
                //Set wonGameData in GameManager. This data will be used to display in game over scene.
                //SceneManager.LoadScene("GameOverScene");//MAYBE: Consider using number of scene?
                waveActive = false;
            }
            //Lose = all civilians dead.
            //Check civilian manager for number of alive civilians.
            else if(civilianManager.GetNumCurrentCivilians() <= 0)
            {
                //Lost wave/game.
                Debug.Log("Lost Game!");
                //Set lostGameData in GameManager. This data will be used to display in game over scene.
                //SceneManager.LoadScene("GameOverScene");//MAYBE: Consider using number of scene?
                waveActive = false;
            }
        }
    }

    public void SetWaveActive()
    {
        waveActive = true;
    }

    public void SetWaveNotActive()
    {
        waveActive = false;
    }
}
