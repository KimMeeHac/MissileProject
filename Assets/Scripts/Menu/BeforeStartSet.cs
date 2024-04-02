using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BeforeStartSet : MonoBehaviour
{
    [SerializeField] ToggleOption keymousesetting;
    [SerializeField] ToggleOption shoottypesetting;
    [SerializeField] Button Startbutton;
    [SerializeField] Button Returnbutton;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Canvas Menu;

    // Start is called before the first frame update
    void Start()
    {
        
        Startbutton.onClick.AddListener(()=> {
            if (keymousesetting.IsCheckedButtonOnce() && shoottypesetting.IsCheckedButtonOnce())//����Ÿ�԰� �߻�Ÿ���� ���ÿ� ������������ ȭ���� �Ѿ����
            {
                SceneManager.LoadSceneAsync(1); 
            }
            else
            {
                text.text = "���� �������ּ���.";
            }
        });
        Returnbutton.onClick.AddListener(() =>
        {
            Menu.gameObject.SetActive(true);
            transform.gameObject.SetActive(false);
        });//����ȭ������ �ǵ��ư���

    }
    /*void ingamescene()
    {
         SceneManager.LoadSceneAsync(1);
    }*/
    

}
