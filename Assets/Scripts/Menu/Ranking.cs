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
        //int value2 = value;//��������, �ּҸ� �����ļ� ���� ����
        //value2 = 100;

        //CallByRef value3 = new CallByRef();
        //value3.value = 10;
        //CallByRef value4 = value3;//��������, �ּҸ� ����
        //value4.value = 0;

        //�� ���� ���� = ���� �����ؼ� �����ϴ� ����, ����� ������ ����Ǿ �ּҰ� �ٸ��� ������ ���� ������� ����
        //�� ���� ���۷��� = �ּҸ� �����ؼ� �����ϴ� ����
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
