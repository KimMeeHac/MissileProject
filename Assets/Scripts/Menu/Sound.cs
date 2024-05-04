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
    [SerializeField] Transform volume;
    bool force = false;
    // Start is called before the first frame update
    void Start()
    {
        
        //���� ���� ������ true ���� �Բ� ������ ��� true���� Ȯ���ϸ� addlistener�� ���� �ʵ��� ó��
        soundbar.onValueChanged.AddListener((x) =>
        {
            //if(/*bool���� false�� ��� �� ���� ������� �ʵ���?=>�̷����̸� ���ڴ� �ٲ������ �����̵尡 �ȹٲ�� ����?*/)
            //�Լ��� ���� �־��
            //removelistener�� ����ߴ��� ������ �ȶ� �̰� �����ΰ�
            if (force == false)
            { 
                soundtext.text = x.ToString();
                volume.GetComponent<Audiotest>().audiovaluechange(x);
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
            volume.GetComponent<Audiotest>().audiovaluechange(soundbar.value);
        });
        Return.onClick.AddListener(() =>
        {
            Menu.gameObject.SetActive(true);
            transform.gameObject.SetActive(false);
        });
    }
    //soundbar ���� �� �ؽ�Ʈ ���ڵ� ����
    //�ؽ�Ʈ ��ȭ �� ���� �ٵ� ������
    //�̶� ���� �� ������ �������� �ʵ��� ó���ؾ���


}
