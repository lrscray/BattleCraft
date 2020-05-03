using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    static LevelManager _instance;

    public static LevelManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LevelManager>();
            }
            return _instance;
        }
    }

    //Set starting resources to 0.
    //Have starting troops.
    //Tick down time for 3 minutes.
     
    [SerializeField] private TroopManager civilianManager = null;

    private bool inStartingPeriod = true;
    //The amount of time before enemies spawn, when the level begins.
    [SerializeField] private float startPeriodTime = 60f; //180f;//3 mins. //TODO: Make sure we change this back to 3 mins.
    private float startPeriodTimeLeft = 60f;

    //Whether the player has lost the game.
    private bool lostGame = false;

    //Whether a wave is currently active.
    private bool waveActive = false;

    [SerializeField] private HealthBarScript healthBar = null;

    private void Start()
    {
        //Set system variables.
        startPeriodTimeLeft = startPeriodTime;

        //Set starting resources to 0.
        //Have starting troops.

        //Start tick down time for 3 minutes.
        StartCoroutine(StartPeriodTickDown());
    }

    //The grace period before enemies start at the beginning of the game.
    //MAYBE: Consider moving this to WaveManager and merging this with the BreakPeriodTickDown() method to save a bit of code/memory.
    IEnumerator StartPeriodTickDown()
    {
        while(inStartingPeriod)
        {
            yield return new WaitForSeconds(1);
            startPeriodTimeLeft -= 1;
            //Can change timeLeftText display here if we have one.
            if(startPeriodTimeLeft <= 0)
            {
                inStartingPeriod = false;
                //End warmup period. Wave starts.
                WaveManager.instance.StartWaves();
                yield return new WaitForSeconds(3); //Allow some time for enemies to spawn.
                StartCoroutine(CheckWinLoseConditions());
            }
        }
    }

    IEnumerator CheckWinLoseConditions()
    {
        while(!lostGame)
        {
            yield return 0; //MAYBE: Consider waiting a certain number of time before checking win/lose conditions? Would save some performance.
            //Check lose conditions. Lose = all civilians dead.
            //Check civilian manager for number of alive civilians.
            if(civilianManager.GetNumTroops() <= 0)
            {
                //Lost wave/game.
                Debug.Log("Lost Game!");
                //Set lostGameData in GameManager. This data will be used to display data in game over scene.
                SceneManager.LoadScene("EndScene");//MAYBE: Consider using number of scene?
                lostGame = true;
            }
        }
    }

    public void SetPlayerBaseMaxHealth(int numStartingCivilians)
    {
        healthBar.SetMaxHealth(numStartingCivilians + 1);
        UpdatePlayerBaseHealth();
    }
    public void UpdatePlayerBaseHealth()
    {
        healthBar.SetHealth(civilianManager.GetNumTroops());
    }

    public void SetLostGame()
    {
        lostGame = true;
    }
    public bool GetLostGame()
    {
        return lostGame;
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
