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
        mis = transform.parent.GetComponent<Missile>();//유도 미사일인 경우 호출하는법
        mis2 = GetComponent<Missile>();//일반 미사일인 경우 호출하는법
    }
    private void OnTriggerEnter2D(Collider2D collision)//풀링 사용할 것, 부딫히는 경우 
    {
        if (players && (collision.tag == eTag.Enemy.ToString() || collision.tag == eTag.Boss.ToString()))//내 미사일인경우
        {
            //Debug.Log("내 미사일 맞음");
            Enemy target = collision.GetComponent<Enemy>();
            target.hit();
            if(guided)mis.RemoveObj();
            else mis2.RemoveObj();
            
        }
        if ((enemys || bosses) && collision.tag == eTag.Player.ToString())//적 미사일인 경우
        {
            //Debug.Log("적 미사일 맞음");
            Player player = collision.GetComponent<Player>();
            player.hit();
            mis2.RemoveObj();
        }
    }
}
