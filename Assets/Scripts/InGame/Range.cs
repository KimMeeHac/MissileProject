using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    CircleCollider2D identyfyRange;
    Missile mis;
    private bool Target = false;

    public bool Debuging = true;
    public Transform trsTarget = null;


    private void OnDrawGizmos()
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
        mis = transform.parent.GetComponent<Missile>();
        //mis=GetComponentInParent<Missile>();
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
