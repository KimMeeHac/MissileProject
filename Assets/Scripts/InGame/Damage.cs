using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Damage : MonoBehaviour
{
    [SerializeField] bool players;
    [SerializeField] bool guided;
    [SerializeField] bool enemys;
    [SerializeField] bool bosses;
    Missile mis;
    Missile mis2;
    Range range;
    // Start is called before the first frame update
    void Start()
    {
        range=GetComponent<Range>();
        mis = transform.parent.GetComponent<Missile>();//���� �̻����� ��� ȣ���ϴ¹�
        mis2 = GetComponent<Missile>();//�Ϲ� �̻����� ��� ȣ���ϴ¹�
    }
    private void OnTriggerEnter2D(Collider2D collision)//Ǯ�� ����� ��, �΋H���� ��� 
    {
        if (players && (collision.tag == eTag.Enemy.ToString() || collision.tag == eTag.Boss.ToString()))//�� �̻����ΰ��
        {
            //Debug.Log("�� �̻��� ����");
            Enemy target = collision.GetComponent<Enemy>();
            target.hit();
            if(guided)mis.RemoveObj();
            else mis2.RemoveObj();
            
        }
        if ((enemys || bosses) && collision.tag == eTag.Player.ToString())//�� �̻����� ���
        {
            //Debug.Log("�� �̻��� ����");
            Player player = collision.GetComponent<Player>();
            player.hit();
            mis2.RemoveObj();
        }
    }
}
