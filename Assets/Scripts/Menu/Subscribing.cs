using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Subscribing : MonoBehaviour
{
    string[] keymousestring= { "방향키로 이동 & 스페이스바로 미사일 발사\n왼쪽 Ctrl로 폭탄 발사", "WASD로 이동 & 마우스 좌클릭으로 발사\n왼쪽 Ctrl로 폭탄 발사" };
    string[] shoottypestring= { "발사 시 직선으로 올라갑니다.", "발사 시 총알이 흩뿌려집니다.", "발사 시 적을 포착하면 각도를 틉니다.\n대상이 사라지면 그 각도 그대로 직진합니다.", "직선형, 방사형, 유도형의 혼합입니다." };
    string keymouse = "";
    string shoottype = "";
    public void scribe(int btn)
    {
        //Debug.Log(btn);
        if (btn < 2)
        {
            keymouse = keymousestring[btn];
            DataManager.Instance.keytype(btn);
        }
        else
        {
            shoottype = shoottypestring[btn - 2];
            DataManager.Instance.shoottype(btn - 2);
        }
        GetComponent<TextMeshProUGUI>().text= "조작 타입\n"+keymouse + "\n발사 타입\n" + shoottype;
    }
    
}
