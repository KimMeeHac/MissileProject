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
            if (keymousesetting.IsCheckedButtonOnce() && shoottypesetting.IsCheckedButtonOnce())//조작타입과 발사타입을 동시에 선택했을때만 화면이 넘어가도록
            {
                SceneManager.LoadSceneAsync(1); 
            }
            else
            {
                text.text = "전부 선택해주세요.";
            }
        });
        Returnbutton.onClick.AddListener(() =>
        {
            Menu.gameObject.SetActive(true);
            transform.gameObject.SetActive(false);
        });//메인화면으로 되돌아가기

    }
    /*void ingamescene()
    {
         SceneManager.LoadSceneAsync(1);
    }*/
    

}
