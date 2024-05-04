using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    CircleCollider2D identyfyRange;
    //Missile mis;
    GuidedMissile mis;
    bool Target = false;

    public bool Debuging = true;
    public Transform trsTarget = null;
    //Vector3 Missile_position;
    //Vector3 Enemy_position;
    //GuidedMissile Guidedmis;

    private void OnDrawGizmos()//빨간 선을 그어서 무엇을 노리는지 알려주는 기능
    {
        if (Debuging && trsTarget != null)
        {
            Debug.DrawLine(transform.position, trsTarget.position, Color.red);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        identyfyRange = GetComponent<CircleCollider2D>();
        //Guidedmis = transform.parent.GetComponent<GuidedMissile>();
        mis=GetComponentInParent<GuidedMissile>();
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == eTag.Enemy.ToString() || collision.tag == eTag.Boss.ToString()) 
    //    {
    //        Transform enemy=collision.transform;
    //        Vector3 pos=enemy.position-mis.transform.position;
    //        float angle= Mathf.Atan2(pos.y, pos.x) * 180 / Mathf.PI - 90;
    //        if (angle >= 60 || angle <= -60)
    //        {
    //            return;
    //        }
    //        else
    //        {
    //            mis.checkRange(collision);
    //        }
    //    }
    //}
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        Missile_position = transform.parent.position;
        Enemy_position = collision.transform.position;
        Vector3 direction = Enemy_position - Missile_position;
        float MistoEnemy = Mathf.Atan2(direction.x, direction.y) * 180 / Mathf.PI - 90;
        if (!Target)//타겟이 정해지지 않은 경우
        {
            if ((collision.tag == eTag.Enemy.ToString() || collision.tag == eTag.Boss.ToString()) && (MistoEnemy >= 60 || MistoEnemy <= -60))
            {
                //몹이나 보스이면서 각도가 -60~60 사이인 경우 타겟이 정해지게 하고 그 각도를 계속 
                Target = true;
            }
        }
    }*/


    // Update is called once per frame
    void Update()
    {
        if (trsTarget != null && trsTarget.transform.parent.name == trsTarget.name)
        {
            trsTarget = null;
        }
        if (Target == true) return;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, identyfyRange.radius, Vector2.right);

        int count = hits.Length;
        if (count == 0) return;

        float hitDistance = 1000f;

        for (int iNum = 0; iNum < count; ++iNum)
        {
            Transform trsHit = hits[iNum].transform;
            if (trsHit.tag != eTag.Enemy.ToString() && trsHit.tag != eTag.Boss.ToString()) continue;


            float checkDistance = Vector2.Distance(transform.position, hits[iNum].transform.position);
            if (checkDistance < identyfyRange.radius && hitDistance > checkDistance)
            {
                hitDistance = hits[iNum].distance;
                trsTarget = trsHit;
            }
        }

        if (trsTarget != null)
        {
            mis.Targetset(trsTarget);
        }
    }

    public void SetTarget(bool _value)
    {
        Target = _value;
    }
}
