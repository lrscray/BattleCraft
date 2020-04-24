using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    public void LoadGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Loading Scene...");
        SceneManager.LoadScene("TitleScene");
    }
}
