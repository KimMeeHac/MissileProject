using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Subscribing : MonoBehaviour
{
    string[] keymousestring= { "����Ű�� �̵� �� �����̽��ٷ� �̻��� �߻�", "WASD�� �̵� �� ���콺 ��Ŭ������ �߻�" };
    string[] shoottypestring= { "������ �̻���", "�����(������) �̻���", "������ �̻���", "����,���,���� ȥ����" };
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
        GetComponent<TextMeshProUGUI>().text= "���� ���\n"+keymouse + "\n�߻� ���\n" + shoottype;
    }
    
}
