using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    int shootty;
    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        DontDestroyOnLoad(this);
    }
    public void shoottype(int _shoottype)
    {
       shootty=_shoottype+1;
    }
    public int shootforplayer()
    {
        return shootty;
    }
}
