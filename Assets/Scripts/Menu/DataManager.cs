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
    public void savescore(int _score)//���� ������ �޾ƿ�
    {
        List<int> scores= new List<int>();
        string path = Path.Combine(Application.dataPath, "ScoreData");//json ���� ���
        Endscore endscore = new Endscore();
        if (File.Exists(path))//������ ������ ��� �ҷ���
        {
            string exist=File.ReadAllText(path);
            endscore=JsonUtility.FromJson<Endscore>(exist);
            /*for(int i = 0; i < endscore.score.Length; i++)
            {
                scores.Add(endscore.score[i]);
            }*/
            scores.AddRange(endscore.score);//���� ����� ������ ������������ ����Ʈ�� �߰�
        }
        scores.Add(_score);//���� ������ ����Ʈ�� �߰�
        scores.Sort();
        scores.Reverse();
        endscore.score = scores.ToArray();
        string score=JsonUtility.ToJson(endscore,true);
        File.WriteAllText(path, score);//json ���Ϸ� ����(��������)
    }
    [Serializable]
    public class Endscore
    {
        public int[] score;
    }
}
