using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{

    int GameGST = 120;
    int GameEST = 5;
    int PlayerScore = 0;
    int MaxGameGST;
    int MaxGameEST;
    int defaultGST = 120, defaultEST = 5;
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Data");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public int GetGST()
    {
        return GameGST;
    }
    public void SetGST(int gst)
    {
        GameGST = gst;
        MaxGameGST = GameGST;
    }
     public void ResetGST()
    {
         GameGST= MaxGameGST;
    }
     public int GetEST()
    {
        return GameEST;
    }

    public void SetEST(int est)
    {
        GameEST= est;
        MaxGameEST = GameEST;
    }
    public void ResetEST()
    {
         GameEST= MaxGameEST;
    }
    public int GetPlayerScore()
    {
        return PlayerScore;
    }
    public void SetPlayerScore(int score)
    {
        PlayerScore = score;
    }
    public void AddPlayerScore()
    {
        PlayerScore++;
    }
    public void ResetPlayerScore()
    {
        PlayerScore = 0;
    }
    public void ResetData()
    {
        ResetPlayerScore();

    }
    public void ResetToDefault()
    {
        ResetPlayerScore();
        SetEST(defaultEST);
        SetGST(defaultGST);
    }
}
