using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour
{  
    GameData Data;
    public void LoadScene(string SceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneName);
    }
    public void LoadSceneDataReset(string SceneName)
    {
        Time.timeScale = 1;
        Data = GameObject.FindGameObjectWithTag("Data").GetComponent<GameData>();
        Data.ResetData();
        SceneManager.LoadScene(SceneName);
    }
     public void LoadSceneDataDefault(string SceneName)
    {
        Time.timeScale = 1;
        Data = GameObject.FindGameObjectWithTag("Data").GetComponent<GameData>();
        Data.ResetToDefault();
        SceneManager.LoadScene(SceneName);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
