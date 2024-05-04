using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audiotest : MonoBehaviour
{
    //효과음리스트를 만들어서 여러개 oneshot기능으로 한번씩 출력하게 수정
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
    public void audiovaluechange(float _value)//옵션에서 볼륨 변경시
    {
        aux.volume = _value/10;
        DataManager.Instance.savevolume(aux.volume);
    }
    public void oneshotsound(int _shot)
    {
        aux.PlayOneShot(clipList[_shot]);
    }
}
