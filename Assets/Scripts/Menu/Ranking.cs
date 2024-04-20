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
        //int value2 = value;//깊은복사, 주소를 새로파서 값을 복사
        //value2 = 100;

        //CallByRef value3 = new CallByRef();
        //value3.value = 10;
        //CallByRef value4 = value3;//얕은복사, 주소만 복사
        //value4.value = 0;

        //콜 바이 벨류 = 값만 복사해서 전달하는 변수, 복사된 변수가 변경되어도 주소가 다르기 때문에 같이 변경되지 않음
        //콜 바이 레퍼런스 = 주소를 보사해서 전달하는 변수
        /*string rank = PlayerPrefs.GetString(rakingKey);
        if (rank == string.Empty)//""
        { 
        
        }*/
        List<int> ranklist = new List<int>();//리스트 선언
        string path = Path.Combine(Application.dataPath, "ScoreData");//json파일 위치를 string 으로 받아옴
        if (File.Exists(path)) //파일 존재시
        { 
            string rank = File.ReadAllText(path);//json파일을 불러온다.
            DataManager.Endscore score = new DataManager.Endscore();
            score = JsonUtility.FromJson<DataManager.Endscore>(rank);
            ranklist.AddRange(score.score);
            for (int i = 0; i < Rankinglist.Count; i++) //json 파일의 순서를 랭킹 순서와 연동시킨다.
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
            PopupUi.Instance.ShowPopup("정말로 삭제하시겠습니까?", () =>
            {
                File.Delete(path);
            }, "삭제", () =>
            {
                PopupUi.Instance.Close();
            },"아니요.");
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
