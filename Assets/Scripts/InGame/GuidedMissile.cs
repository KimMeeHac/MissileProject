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
    private void OnBecameInvisible()//�ʿ��� ������� ���ŵǵ���
    {
        RemoveObj();
    }
    public void RemoveObj()//������Ʈ�� �ٽ� Ǯ���Ŵ����� �������� ���
    {
        if (PoolingManager.Instance != null)
        {
            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }
    }
    void Update()
    {
        //���� Ÿ���� ���ٸ� ���� �����ϴٰ� Ÿ���� �ɸ��� �������� �Ӹ��� ������ ����
        transform.position += transform.up * m_speed * Time.deltaTime;
        target();

    }
    public void target()//�����̻����ΰ��-�Ӹ� ��ġ�� �ٲ�°��
    {
        if (Target != null && Target.transform.parent.name == Target.name)
        {
            Target = null;
        }


        if (Target == null && raider.enabled)//Ÿ���� �������� �ʾҰ� Range ��ũ��Ʈ�� �����ִ� ���
        {
            raider.SetTarget(false);//range��ũ��Ʈ���� Ÿ���� �����ؿ���� ���
        }
        //���� ��ġ�� ã�� �˰���
        //�� ��ġ ����
        ////if �� ��ġ null�ΰ�� �� ��ġ ��� ��ĵ
        //if (range.enabled)//Range ��ũ��Ʈ�� ���� �ִٸ� ���� ��� �Ⱦ� ������
        //{
        //    return;
        //}


        else//Ÿ���� ������ ���
        {
            raider.enabled = false;//range��ũ��Ʈ�� �������� Ÿ���� ���� �������� �ʵ��� ��
            if (Target == null)//�ٸ� �̻��Ͽ� ���� Ÿ���� �����ٸ�
            {
                transform.rotation = Quaternion.Euler(0f, 0f, angleturn);//�Ӹ��� ���ڱ� Ʋ������ �ʵ��� ��
                return;//��������� ���ߵ��� ��
            }
            Transform enemytrs = Target.transform;
            //���� �̻����� ���� ��� �˰���
            Vector3 pos = enemytrs.position - transform.position;
            float angle = Mathf.Atan2(pos.y, pos.x) * 180 / Mathf.PI - 90;
            if ((angle >= 60 || angle <= -60))
            //Ư�� ������ �����,���� �ٽ� null�ɽ� ������� ��Ȱ��ȭ
            {
                Debug.Log("�� �̻����� ���� ����");
                findingTarget = false;
            }
            angleturn += (angle / 1.2f) * Time.deltaTime;
            if ((angleturn >= 0 && angleturn >= angle) || (angleturn < 0 && angleturn <= angle))
            {
                angleturn = angle;
            }
            transform.rotation = Quaternion.Euler(0, 0, angleturn);
        }
        //�̻��ϰ� ���� ����� ���� ���� ??-
        //if �� ��ġ ã���� ��ĵ ����� ��Ȱ��ȭ
        //if Ư�� ������ ����ų� ���� �ٽ� null�� �ȴٸ� ���� ����� ��Ȱ��ȭ

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
