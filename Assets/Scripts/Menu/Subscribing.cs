using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Subscribing : MonoBehaviour
{
    string[] keymousestring= { "방향키로 이동 및 스페이스바로 미사일 발사", "WASD로 이동 및 마우스 좌클릭으로 발사" };
    string[] shoottypestring= { "직선형 미사일", "방사형(샷건형) 미사일", "유도형 미사일", "직선,방사,유도 혼합형" };
    string keymouse = "";
    string shoottype = "";
    public void scribe(int btn)
    {
        //Debug.Log(btn);
        if (btn < 2)
        {
            keymouse = keymousestring[btn];
        }
        else
        {
            shoottype = shoottypestring[btn - 2];
            DataManager.Instance.shoottype(btn - 2);
        }
        GetComponent<TextMeshProUGUI>().text= "조작 방법\n"+keymouse + "\n발사 방법\n" + shoottype;
    }
    
}
