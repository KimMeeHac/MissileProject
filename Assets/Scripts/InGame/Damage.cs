using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] bool players;
    [SerializeField] bool guided;
    [SerializeField] bool enemys;
    [SerializeField] bool bosses;
    //[SerializeField] GameObject missileobj;
    Missile mis;
    GuidedMissile guidemis;
    //Range range;
    // Start is called before the first frame update
    void Start()
    {
        if (guided)
        {
            guidemis = GetComponentInParent<GuidedMissile>();

        }
        else
        {
            mis = GetComponent<Missile>();//�Ϲ� �̻����� ��� ȣ���ϴ¹�                            

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)//Ǯ�� ����� ��, �΋H���� ��� 
    {
        if (players && (collision.tag == eTag.Enemy.ToString() || collision.tag == eTag.Boss.ToString()))//�� �̻����ΰ��
        {
            //Debug.Log("�� �̻��� ����");
            Enemy target = collision.GetComponent<Enemy>();
            target.hit(1);
            if (guided)
            {
                guidemis.RemoveObj();
            }
            else
            {
                mis.RemoveObj();
            }
        }
        if ((enemys || bosses) && collision.tag == eTag.Player.ToString())//�� �̻����� ���
        {
            //Debug.Log("�� �̻��� ����");
            Player player = collision.GetComponent<Player>();
            player.hit();
            mis.RemoveObj();
        }
    }
}
