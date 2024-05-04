using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audiotest : MonoBehaviour
{
    //ȿ��������Ʈ�� ���� ������ oneshot������� �ѹ��� ����ϰ� ����
    List<AudioClip> clipList = new List<AudioClip>();
    AudioSource aux;
    // Start is called before the first frame update
    void Start()
    {
        aux = GetComponent<AudioSource>();
        int count=DataManager.Instance.audiolist.Count;
        for (int iNum=0;iNum < count; iNum++)
        {
            clipList.Add(DataManager.Instance.audiolist[iNum].clip);
        }
    }
    public void audiovaluechange(float _value)//�ɼǿ��� ���� �����
    {
        aux.volume = _value/10;
        DataManager.Instance.savevolume(aux.volume);
    }
    public void oneshotsound(int _shot)
    {
        aux.PlayOneShot(clipList[_shot]);
    }
}
