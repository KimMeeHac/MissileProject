using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Subscribing : MonoBehaviour
{
    string[] keymousestring= { "����Ű�� �̵� & �����̽��ٷ� �̻��� �߻�\n���� Ctrl�� ��ź �߻�", "WASD�� �̵� & ���콺 ��Ŭ������ �߻�\n���� Ctrl�� ��ź �߻�" };
    string[] shoottypestring= { "�߻� �� �������� �ö󰩴ϴ�.", "�߻� �� �Ѿ��� ��ѷ����ϴ�.", "�߻� �� ���� �����ϸ� ������ Ƶ�ϴ�.\n����� ������� �� ���� �״�� �����մϴ�.", "������, �����, �������� ȥ���Դϴ�." };
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
        GetComponent<TextMeshProUGUI>().text= "���� Ÿ��\n"+keymouse + "\n�߻� Ÿ��\n" + shoottype;
    }
    
}
