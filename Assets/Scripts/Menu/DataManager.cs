using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    int shootty;
    bool keytypeset;
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
    public void keytype(int _keytype)
    {
        keytypeset = Convert.ToBoolean(_keytype);
    }
    public bool keytypeforplayer()
    {
        return keytypeset;
    }
    public void savescore(int _score)
    {
        string score=JsonUtility.ToJson(_score.ToString(),true);
        string path = Path.Combine(Application.dataPath, "ScoreData");
        File.WriteAllText(path, score);
    }
}
