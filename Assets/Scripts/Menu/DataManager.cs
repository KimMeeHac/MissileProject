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
    float vol;
    [System.Serializable]
    public class audiocliplist
    {
        public AudioClip clip;
        [TextArea] public string description;
    }
    public List<audiocliplist> audiolist;
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
    public void shoottype(int _shoottype)//발사 타입을 받아옴
    {
       shootty=_shoottype+1;
    }
    public int shootforplayer()//플레이어 시작할때 호출받아서 전달
    {
        return shootty;
    }
    public void keytype(int _keytype)//조작 타입 받아옴
    {
        keytypeset = Convert.ToBoolean(_keytype);
    }
    public bool keytypeforplayer()//게임 시작시 호출받아서 받아옴
    {
        return keytypeset;
    }
    public void savevolume(float _vol)
    {
        vol = _vol;
    }
    public float gamevolume()
    {
        return vol;
    }
    public void savescore(int _score)//엔딩 점수를 받아옴
    {
        List<int> scores= new List<int>();
        string path = Path.Combine(Application.dataPath, "Data");//json 파일 경로
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
