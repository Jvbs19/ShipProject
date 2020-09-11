using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Options : MonoBehaviour
{
    [SerializeField] 
    GameData Data;

    [SerializeField]
    TextMeshProUGUI GSTTXT;

    [SerializeField]
    TextMeshProUGUI ESTTXT;
    int GST = 120;
    int EST = 5;
    void Update()
    {
        if(Data == null)
            Data = GameObject.FindGameObjectWithTag("Data").GetComponent<GameData>();
    }
    public void AddGST()
    {
        GST++;

        if(GST > 180)
            GST = 180;

        TxtUpdate();
    }
    public void RemoveGST()
    {
        GST--;

        if(GST < 60)
            GST=60;

        TxtUpdate();
    }
    public void SetMinGST()
    {      
        GST=60;
        TxtUpdate();
    }
      public void SetMaxGST()
    {      
        GST=180;
        TxtUpdate();
    }
    public void AddEST()
    {
        EST++;

        if(EST > 60)
            EST = 60;

        TxtUpdate();
    }
    public void RemoveEST()
    {
        EST--;

        if(EST <=1)
            EST = 1;

        TxtUpdate();
    }
     public void SetMinEST()
    {      
        EST=1;
        TxtUpdate();
    }
      public void SetMaxEST()
    {      
        EST=60;
        TxtUpdate();
    }
    void TxtUpdate()
    {
        GSTTXT.text = ""+GST;
        ESTTXT.text = ""+EST;
    }
    public void SetOnData()
    {
        Data.SetEST(EST);
        Data.SetGST(GST);
    }
}
