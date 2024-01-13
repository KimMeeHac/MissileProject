using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleOption : MonoBehaviour
{
    //어떤 버튼이 눌러졌을때 눌러진버튼은 더이상 누를수 없도록 비활성화 하고 이버튼을 제외한 다른버튼은 누를수 있는 상태로 변경되는 기능

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
