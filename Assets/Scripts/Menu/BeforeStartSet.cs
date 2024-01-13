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
    [SerializeField] TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {

       
        Startbutton.onClick.AddListener(()=> {
            if (keymousesetting.IsCheckedButtonOnce() && shoottypesetting.IsCheckedButtonOnce())
            { 
                SceneManager.LoadSceneAsync(1); 
            }
            else
            {
                text.text = "전부 선택해주세요.";
            }
        });
    }
    /*void ingamescene()
    {
         SceneManager.LoadSceneAsync(1);
    }*/
    // Update is called once per frame
    void Update()
    {
        
    }

}
