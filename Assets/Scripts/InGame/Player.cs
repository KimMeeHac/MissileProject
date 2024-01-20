using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject m_objBarrel;
    [SerializeField] Transform m_dynamicObj;
    [Header("�÷��̾� ����")]
    [Tooltip("ü��")][SerializeField] int playerHP = 10;
    [Tooltip("�̵��ӵ� �������")][SerializeField] float m_fSpeed = 5f;
    [Tooltip("�����ð� �������")][SerializeField] float m_invinciTime = 1f;
    [Tooltip("�߻� Ÿ��")][Range(1, 5)][SerializeField] int m_shootType = 1;
    [Tooltip("�߻� �ܰ�")][SerializeField][Range(1, 7)] int m_playershootlv = 1;

    SpriteRenderer playerRenderer;
    bool m_invincibile = false;
    float m_invinciTimer = 0f;
    float missilespeed;
    int m_maxplayershootlv = 7;
    bool ishit;
    bool a;
    // Start is called before the first frame update
    private void Awake()
    {
        playerRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {

    }
    public void hit()
    {
        //Debug.Log("��Ʈ �Լ��� ����");
        if (m_invincibile)
        {
            return;
        }
        else
        {
            playerHP--;
            m_invincibile = true;
            if (playerHP == 0)
            {
                //Debug.Log("ü��0 ����");
                Destroy(gameObject);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        moving();//������ �����̴� ���
        checkposition();//������ ȭ�� �ȿ��� �ְ��ϴ±��
        invincible();//�ǰݽ� �������
        shoot();//�߻� ���
    }
    void moving()
    {
        float vert = Input.GetAxisRaw("Vertical");
        float hori = Input.GetAxisRaw("Horizontal");
        Transform PlayerTrs = GetComponent<Transform>();
        PlayerTrs.position = PlayerTrs.position + new Vector3(hori, vert) * Time.deltaTime * m_fSpeed;

    }
    void checkposition()
    {
        Vector3 Pos = Camera.main.WorldToViewportPoint(transform.position);
        if (Pos.x < 0.1f)
        {
            Pos.x = 0.1f;
        }
        else if (Pos.x > 0.9f)
        {
            Pos.x = 0.9f;
        }
        if (Pos.y < 0.05f)
        {
            Pos.y = 0.05f;
        }
        else if (Pos.y > 0.95f)
        {
            Pos.y = 0.95f;
        }
        transform.position = Camera.main.ViewportToWorldPoint(Pos);
    }
    void invincible()//�ǰݽ� ����
    {
        if (!m_invincibile)
        {
            return;
        }
        Color invin = playerRenderer.color;
        invin.a = 0.5f;
        playerRenderer.color = invin;
        m_invinciTimer += Time.deltaTime;
        if (m_invinciTimer >= m_invinciTime)
        {
            m_invincibile = false;
            m_invinciTimer = 0f;
            invin.a = 1f;
            playerRenderer.color = invin;
        }
    }
    void shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            shootmissile();
        }
    }
    void shootmissile()
    {

        if (m_playershootlv >= m_maxplayershootlv)//�ִ�ܰ踦 ���� �ʵ��� ó��
        {
            m_playershootlv = m_maxplayershootlv;
        }
        switch (m_shootType)//�ѽ�� Ÿ��1~5����
        {
            case 1://1Ÿ�� ������
                {
                    if (m_playershootlv % 2 == 1)
                    {
                        GameObject obj = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Player_Bullet_B, m_dynamicObj);
                        obj.GetComponent<Missile>().checkposition(m_objBarrel.transform.position);
                        for (int i = 1; i <= m_playershootlv - 1; i++)
                        {
                            GameObject obj2 = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Player_Bullet_B, m_dynamicObj);
                            obj2.GetComponent<Missile>().checkposition(m_objBarrel.transform.position + new Vector3(0.25f * (int)((i + 1) / 2) * MathF.Pow(-1, i), 0, 0));
                            //Debug.Log(m_objBarrel.transform.position + new Vector3(0.25f * (int)((i + 1) / 2) * MathF.Pow(-1, i), 0, 0));
                        }
                    }
                    else
                    {
                        a = false;
                        for (int i = 1; i <= m_playershootlv; i++)
                        {
                            GameObject obj = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Player_Bullet_B, m_dynamicObj);
                            obj.GetComponent<Missile>().checkposition(m_objBarrel.transform.position + new Vector3(0.125f * (i - Convert.ToInt32(a)) * Mathf.Pow(-1, i), 0, 0));
                            //Debug.Log(m_objBarrel.transform.position + new Vector3(0.125f * (i - Convert.ToInt32(a)) * Mathf.Pow(-1, i), 0, 0));
                            a = !a;
                        }
                    }
                }
                break;
            case 2://2Ÿ�� �����(����)
                {
                    if (m_playershootlv % 2 == 1)
                    {
                        GameObject obj = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Player_Bullet_C, m_dynamicObj);
                        obj.GetComponent<Missile>().checkrotation(0, m_objBarrel.transform.position);
                        for (int i = 1; i <= m_playershootlv - 1; i++)
                        {
                            GameObject obj2 = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Player_Bullet_C, m_dynamicObj);
                            obj2.GetComponent<Missile>().checkrotation(15 * ((i + 1) / 2) * Mathf.Pow(-1, i), m_objBarrel.transform.position);
                            //Debug.Log(15 * ((i + 1) / 2) * Mathf.Pow(-1, i));
                        }
                    }
                    else
                    {
                        a = false;
                        for (int i = 1; i <= m_playershootlv; i++)
                        {
                            GameObject obj = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Player_Bullet_C, m_dynamicObj);
                            obj.GetComponent<Missile>().checkrotation(7.5f * (i - Convert.ToInt32(a)) * Mathf.Pow(-1, i), m_objBarrel.transform.position);
                            //Debug.Log(15 * ((i + 1) / 2) * Mathf.Pow(-1, i));
                            a = !a;
                        }
                    }
                }
                break;
            case 3://3Ÿ�� ������ ����ź���� �ӵ��� �ٸ��� ����, �ܰ谡 �������� �����ӵ��� ���������� ó��
                {
                    for (int i = 1; i <= m_playershootlv; i++)
                    {
                        missilespeed = UnityEngine.Random.Range(1.5f + i / 7, 3f);
                        GameObject obj = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Player_Bullet_A, m_dynamicObj);
                        obj.GetComponent<Missile>().checkguided( m_objBarrel.transform.position, missilespeed);
                    }
                }
                break;
            case 4://4Ÿ�� ȥ���� 1�ܰ� ������ 2�ܰ���� ������ �߰� 4�ܰ���� ������ �� �߰� 6�ܰ���� ������ �߰�
                {
                    if (m_playershootlv % 2 == 1)//Ȧ���ܰ�� ����������
                    {
                        GameObject obj = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Player_Bullet_B, m_dynamicObj);
                        obj.GetComponent<Missile>().checkposition(m_objBarrel.transform.position);
                    }
                    if (m_playershootlv >= 2)//2�ܰ��̻� ����
                    {
                        GameObject obj = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Player_Bullet_C, m_dynamicObj);
                        obj.GetComponent<Missile>().checkrotation(30, m_objBarrel.transform.position);
                        GameObject obj1 = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Player_Bullet_C, m_dynamicObj);
                        obj1.GetComponent<Missile>().checkrotation(-30, m_objBarrel.transform.position);

                        if (m_playershootlv >= 4)//4�ܰ� �̻�� �������߰�
                        {
                            GameObject obj2 = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Player_Bullet_B, m_dynamicObj);
                            obj2.GetComponent<Missile>().checkposition(m_objBarrel.transform.position + new Vector3(0.4f, 0, 0));
                            GameObject obj3 = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Player_Bullet_B, m_dynamicObj);
                            obj3.GetComponent<Missile>().checkposition(m_objBarrel.transform.position + new Vector3(-0.4f, 0, 0));

                            if (m_playershootlv >= 6)//6�ܰ� �̻�� ����, �ٸ� �ӵ��� ���� ���������� ����
                            {
                                for (int i = 0; i < 2; i++)
                                {
                                    missilespeed = UnityEngine.Random.Range(2.5f, 3f);
                                    GameObject obj4 = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Player_Bullet_A, m_dynamicObj);
                                    obj4.GetComponent<Missile>().checkguided( m_objBarrel.transform.position, missilespeed);
                                }
                            }
                        }
                    }
                }
                break;
            case 5://5Ÿ�� �̴Ͼ��ȯ�� 
                switch (m_playershootlv)
                {
                    case 1:

                        break;
                    case 2: break;
                    case 3: break;
                    case 4: break;
                    case 5: break;
                    case 6: break;
                    case 7: break;
                }
                break;
        }
    }
}