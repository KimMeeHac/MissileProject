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
    public void shoottype(int _shoottype)//�߻� Ÿ���� �޾ƿ�
    {
       shootty=_shoottype+1;
    }
    public int shootforplayer()//�÷��̾� �����Ҷ� ȣ��޾Ƽ� ����
    {
        return shootty;
    }
    public void keytype(int _keytype)//���� Ÿ�� �޾ƿ�
    {
        keytypeset = Convert.ToBoolean(_keytype);
    }
    public bool keytypeforplayer()//���� ���۽� ȣ��޾Ƽ� �޾ƿ�
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
    public void savescore(int _score)//���� ������ �޾ƿ�
    {
        List<int> scores= new List<int>();
        string path = Path.Combine(Application.dataPath, "Data");//json ���� ���
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
