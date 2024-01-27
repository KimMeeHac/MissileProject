using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    [SerializeField] Button Delete;
    [SerializeField] Button Return;
    [SerializeField] Canvas Menu;
    [SerializeField] List<TextMeshProUGUI> Rankinglist;
    string rakingKey = "rankingKey";
    // Start is called before the first frame update

    public class CallByRef
    {
        public int value;
    }
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
        string rank = PlayerPrefs.GetString(rakingKey);
        if (rank == string.Empty)//""
        { 
        
        }

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
