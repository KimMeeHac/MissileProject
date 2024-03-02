using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    [SerializeField] Slider soundbar;
    [SerializeField] TMP_InputField soundtext;
    [SerializeField] Button Return;
    [SerializeField] Canvas Menu;
    bool force = false;
    // Start is called before the first frame update
    void Start()
    {
        //만약 값을 보낼때 true 값을 함께 보내는 경우 true값을 확인하면 addlistener를 쓰지 않도록 처리
        soundbar.onValueChanged.AddListener((x) =>
        {
            //if(/*bool값이 false인 경우 이 값이 변경되지 않도록?=>이런식이면 숫자는 바뀌었지만 슬라이드가 안바뀔수 있음?*/)
            //함수를 만들어서 넣어볼까
            //removelistener를 사용했더니 오류는 안뜸 이게 정답인가
            if (force == false)
            { 
                soundtext.text = x.ToString();
            }

            if(force == true) 
            {
                force = false;
            }
        });
        soundtext.onValueChanged.AddListener((x) =>
        {
            if (x != string.Empty)
            {
                force = false;
                soundbar.value = int.Parse(x);
            }
            else
            {
                force = true;
                soundbar.value = 0;
            }
        });
        Return.onClick.AddListener(() =>
        {
            Menu.gameObject.SetActive(true);
            transform.gameObject.SetActive(false);
        });
    }
    //soundbar 조절 시 텍스트 숫자도 변함
    //텍스트 변화 시 사운드 바도 조절됨
    //이때 사운드 바 조절이 루프되지 않도록 처리해야함


}
