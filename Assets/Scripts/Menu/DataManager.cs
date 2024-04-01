using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using System.Linq;

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
    public void savescore(int _score)//엔딩 점수를 받아옴
    {
        List<int> scores= new List<int>();
        string path = Path.Combine(Application.dataPath, "ScoreData");//json 파일 경로
        Endscore endscore = new Endscore();
        if (File.Exists(path))//파일이 존재할 경우 불러옴
        {
            string exist=File.ReadAllText(path);
            endscore=JsonUtility.FromJson<Endscore>(exist);
            /*for(int i = 0; i < endscore.score.Length; i++)
            {
                scores.Add(endscore.score[i]);
            }*/
            scores.AddRange(endscore.score);//파일 저장된 곳에서 기존점수들을 리스트로 추가
        }
        scores.Add(_score);//엔딩 점수를 리스트에 추가
        scores.Sort();
        scores.Reverse();
        endscore.score = scores.ToArray();
        string score=JsonUtility.ToJson(endscore,true);
        File.WriteAllText(path, score);//json 파일로 저장(내보내기)
    }
    [Serializable]
    public class Endscore
    {
        public int[] score;
    }
}
