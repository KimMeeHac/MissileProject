using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleOption : MonoBehaviour
{
    //� ��ư�� ���������� ��������ư�� ���̻� ������ ������ ��Ȱ��ȭ �ϰ� �̹�ư�� ������ �ٸ���ư�� ������ �ִ� ���·� ����Ǵ� ���

    [SerializeField] Button[] buttons;

    public int OnclickButton(Button _btn)
    {
        int count = buttons.Length;
        int num=0;
        if (count > 2)
        {
            num = 2;
        }
        for (int iNum = 0; iNum < count; iNum++)
        {
                //buttons[iNum].interactable = buttons[iNum] != _btn;
            if (buttons[iNum] == _btn)
            {
                buttons[iNum].interactable = false;
                num += iNum;
            }
            else
            {
                buttons[iNum].interactable = true;
            }
        }
        return num;
    }

    public bool IsCheckedButtonOnce()
    {
        int count = buttons.Length;
        for (int iNum = 0; iNum < count; iNum++)
        {
            if(buttons[iNum].interactable == false)
            {
                return true;
            }
        }
        return false;
    }

}
