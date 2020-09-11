using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    GameData Data;

    [Header("Time Settings")]
    [SerializeField] 
    float MaxTimer = 120f;
    float Timer;
    float EnemySpawnTime;

    [SerializeField] 
    float MaxEnemySpawnTime = 5f;
    bool pauseTimer = false;
    bool pauseEnemyTimer = false;

    [Header("Visual Settings")]
    [SerializeField] 
    GameObject EndScreen;
    
    [SerializeField]
    TextMeshProUGUI GstTimerTXT;

    [SerializeField]
    TextMeshProUGUI EstTimerTXT;

    [SerializeField]
    TextMeshProUGUI PlayerScoreTXT;

    [SerializeField]
    TextMeshProUGUI FinalScoreTXT;

    float MyScore;

    [Header("Enemy Spawn Settings")]
    [SerializeField] 
    GameObject Chaser;
    [SerializeField] 
    GameObject Shooter;
    [SerializeField] 
    GameObject RandomBoat;

    [SerializeField] 
    GameObject[] SpawnPoint;
    int dice, spawnPlace;

    [SerializeField] 
    PlayerBoatBehaviour Player;

    bool once;
    void Start()
    {      
        if(Data == null)
        {
            Data = GameObject.FindGameObjectWithTag("Data").GetComponent<GameData>();
        }
    }
    void Once()
    {
        if(!once)
        {
        
            if(Data != null) 
            {  
                SetMaxTimer(Data.GetGST());
                SetMaxEnemyTimer(Data.GetEST());
                ResetTimer();
                ResetEnemyTimer();
                MyScore = Data.GetPlayerScore();
                PlayerScoreTXT.text = ""+(int)MyScore;
            }
            once = true;
        }
    }
    void Update()
    {
        Once();
        TimerOut();
        EnemyTimerOut();
        CheckPlayer();
    }
    void FixedUpdate()
    {
        TimerControl();
        EnemyTimerControl();
        CheckPlayerScore();
    }

    void SpawnEnemy()
    {
        if(!Player.isPlayerDead())
        {
        dice = Random.Range(1,6);
        spawnPlace = Random.Range(0, SpawnPoint.Length-1);
        if(dice%2 ==1)
        {
            GameObject e = Instantiate(Chaser);  
            e.transform.position = SpawnPoint[spawnPlace].transform.position;
            e.transform.rotation = SpawnPoint[spawnPlace].transform.rotation;

        }
        if(dice%2 ==0 && dice < 6)
        {
            GameObject e = Instantiate(Shooter);  
            e.transform.position = SpawnPoint[spawnPlace].transform.position;
            e.transform.rotation = SpawnPoint[spawnPlace].transform.rotation;
        }
        else if(dice%2 ==0 && dice == 6)
        {
            GameObject e = Instantiate(RandomBoat);  
            e.transform.position = SpawnPoint[spawnPlace].transform.position;
            e.transform.rotation = SpawnPoint[spawnPlace].transform.rotation;
        }
        }
    }
    void CheckPlayer()
    {
        if(Player.isPlayerDead())
        {
            ForcedTimeOut();
        }
    }
    #region PlayerScore
    void CheckPlayerScore()
    {
        if(Data != null)
        {
            MyScore = Data.GetPlayerScore();
            PlayerScoreTXT.text = ""+(int)MyScore;
            FinalScoreTXT.text = ""+(int)MyScore;
        }
    }
    #endregion

    #region TimerSettings

    #region Gametime
    void TimerOut()
    {
        if (Timer <= 0)
        {
           EndScreen.SetActive(true);
           CheckPlayerScore();
        }
    }
    void TimerControl()
    {
        if (Timer > 0 && pauseTimer == false)
        {
            GstTimerTXT.text = ""+(int)Timer;
            Timer -= Time.deltaTime;
        }
    }

    public void ResetTimer()
    {
        Timer = MaxTimer;
    }
    public void SetMaxTimer(float time)
    {
        MaxTimer = time;
    }
    void ForcedTimeOut()
    {
        Timer = 0;
    }

    void SetTimerStatus(bool time)
    {
        pauseTimer = time;
    }
    public bool GetTimerStatus()
    {
        return pauseTimer;
    }
    #endregion

    #region EnemySpawnTime
    
    void EnemyTimerOut()
    {
        if (EnemySpawnTime <= 0)
        {
           SpawnEnemy();
           ResetEnemyTimer();
        }
    }
    void EnemyTimerControl()
    {
        if (EnemySpawnTime > 0 && pauseTimer == false)
        {
            EstTimerTXT.text = ""+(int)EnemySpawnTime;
            EnemySpawnTime -= Time.deltaTime;
        }
    }

    public void ResetEnemyTimer()
    {
        EnemySpawnTime = MaxEnemySpawnTime;
    }
    public void SetMaxEnemyTimer(float time)
    {
        MaxEnemySpawnTime = time;
    }
    void ForcedEnemyTimeOut()
    {
        EnemySpawnTime = 0;
    }

    void SetEnemyTimerStatus(bool time)
    {
        pauseEnemyTimer = time;
    }
    public bool GetEnemyTimerStatus()
    {
        return pauseEnemyTimer;
    }
    #endregion

    #endregion
}
