using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GuidedMissile : MonoBehaviour
{
    bool findingTarget = true;
    [SerializeField] float m_speed = 3f;
    Range raider;
    [SerializeField] Enemy Target;
    float angleturn = 0;
    void Start()
    {
        raider = GetComponent<Range>();

    }
    private void OnBecameInvisible()//맵에서 사라지면 제거되도록
    {
        RemoveObj();
    }
    public void RemoveObj()//오브젝트를 다시 풀링매니저에 돌려놓는 기능
    {
        if (PoolingManager.Instance != null)
        {
            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }
    }
    void Update()
    {
        //만약 타겟이 없다면 위로 직진하다가 타겟이 걸리면 그쪽으로 머리를 돌리고 직진
        transform.position += transform.up * m_speed * Time.deltaTime;
        target();

    }
    public void target()//유도미사일인경우-머리 위치가 바뀌는경우
    {
        if (Target != null && Target.transform.parent.name == Target.name)
        {
            Target = null;
        }


        if (Target == null && raider.enabled)//타겟이 정해지지 않았고 Range 스크립트가 켜져있는 경우
        {
            raider.SetTarget(false);//range스크립트에게 타겟을 지정해오라고 명령
        }
        //적의 위치를 찾는 알고리즘
        //적 위치 정의
        ////if 적 위치 null인경우 적 위치 계속 스캔
        //if (range.enabled)//Range 스크립트가 켜져 있다면 각도 기능 안씀 직진만
        //{
        //    return;
        //}


        else//타겟이 지정된 경우
        {
            raider.enabled = false;//range스크립트를 꺼버려서 타겟을 새로 지정하지 않도록 함
            if (Target == null)//다른 미사일에 의해 타겟이 터진다면
            {
                transform.rotation = Quaternion.Euler(0f, 0f, angleturn);//머리가 갑자기 틀어지지 않도록 함
                return;//유도기능을 멈추도록 함
            }
            Transform enemytrs = Target.transform;
            //적과 미사일의 각도 계산 알고리즘
            Vector3 pos = enemytrs.position - transform.position;
            float angle = Mathf.Atan2(pos.y, pos.x) * 180 / Mathf.PI - 90;
            if ((angle >= 60 || angle <= -60))
            //특정 각도를 벗어날시,적이 다시 null될시 유도기능 비활성화
            {
                Debug.Log("이 미사일의 유도 꺼짐");
                findingTarget = false;
            }
            angleturn += (angle / 1.2f) * Time.deltaTime;
            if ((angleturn >= 0 && angleturn >= angle) || (angleturn < 0 && angleturn <= angle))
            {
                angleturn = angle;
            }
            transform.rotation = Quaternion.Euler(0, 0, angleturn);
        }
        //미사일과 가장 가까운 적을 선별 ??-
        //if 적 위치 찾으면 스캔 기능은 비활성화
        //if 특정 각도를 벗어나거나 적이 다시 null이 된다면 유도 기능을 비활성화

    }
    public void Targetset(Transform _target)
    {
        Target = _target.GetComponent<Enemy>();
    }
    public void checkposition(Vector3 _pos)
    {
        transform.position = _pos;
        transform.rotation=Quaternion.identity;
    }
}
