using JetBrains.Annotations;
using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static DataManager;
using File=System.IO.File;

public class Ranking : MonoBehaviour
{
    [SerializeField] Button Delete;
    [SerializeField] Button Return;
    [SerializeField] Canvas Menu;
    [SerializeField] List<TextMeshProUGUI> Rankinglist;
    //string rakingKey = "rankingKey";
    // Start is called before the first frame update

    /*public class CallByRef
    {
        public int value;
    }*/
    void Start()
    {
        //int value = 10;
        //int value2 = value;//��������, �ּҸ� �����ļ� ���� ����
        //value2 = 100;

        //CallByRef value3 = new CallByRef();
        //value3.value = 10;
        //CallByRef value4 = value3;//��������, �ּҸ� ����
        //value4.value = 0;

        //�� ���� ���� = ���� �����ؼ� �����ϴ� ����, ����� ������ ����Ǿ �ּҰ� �ٸ��� ������ ���� ������� ����
        //�� ���� ���۷��� = �ּҸ� �����ؼ� �����ϴ� ����
        /*string rank = PlayerPrefs.GetString(rakingKey);
        if (rank == string.Empty)//""
        { 
        
        }*/
        List<int> ranklist = new List<int>();//����Ʈ ����
        string path = Path.Combine(Application.dataPath, "ScoreData");//json���� ��ġ�� string ���� �޾ƿ�
        if (File.Exists(path)) //���� �����
        { 
            string rank = File.ReadAllText(path);//json������ �ҷ��´�.
            DataManager.Endscore score = new DataManager.Endscore();
            score = JsonUtility.FromJson<DataManager.Endscore>(rank);
            ranklist.AddRange(score.score);
            for (int i = 0; i < Rankinglist.Count; i++) //json ������ ������ ��ŷ ������ ������Ų��.
            {
                /*if (ranklist[i]==null)
                {
                    Rankinglist[i].text = "00000";
                }
                else
                {
                    Rankinglist[i].text = ranklist[i].ToString("D5");
                }*/
                Rankinglist[i].text = ranklist[i].ToString("D5");
            }
        }
        
        
        Delete.onClick.AddListener(() =>
        {
            PopupUi.Instance.ShowPopup("������ �����Ͻðڽ��ϱ�?", () =>
            {
                File.Delete(path);
            }, "����", () =>
            {
                PopupUi.Instance.Close();
            },"�ƴϿ�.");
        });
        Return.onClick.AddListener(() =>
        {
            Menu.gameObject.SetActive(true);
            transform.gameObject.SetActive(false);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
