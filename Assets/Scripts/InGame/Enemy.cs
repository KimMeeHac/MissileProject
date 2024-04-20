
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("����,�ѽ����")]
    [SerializeField] bool isBoss = false;
    [SerializeField] bool isshoot = false;
    [Header("��ü ü�°� �̼�")]
    [SerializeField]public int m_fHp = 5;
    [SerializeField] float m_fSpeed = 3f;
    [SerializeField] float m_actspeed = 1f;
    SpriteRenderer enemyRenderer;
    Vector3 m_fStartingPos;
    Transform playpos;
    Transform trsDynamic;
    float m_fRatioY = 0f;
    float timer = 0f;
    float m_timer = 0f;
    public float m_bosspattern = 5f;
    public float m_bossmissilespeed = 3f;
    bool bulletshoot = false;
    bool firstmov = true;
    bool isrightmov = false;
    bool patternchange = true;
    bool haveitem = false;
    bool alreadydeath = false;
    int pattern;
    int shoottime = 0;
    public int point = 0;
    Player player;
    private void OnBecameInvisible()//�ʿ��� ������� ���ŵǵ���
    {
        RemoveObj();
    }
    void RemoveObj()//������Ʈ�� �ٽ� Ǯ���Ŵ����� �������� ���
    {
        if (PoolingManager.Instance != null)
        {
            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }
    }
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        //Application.targetFrameRate = 60;
        if (haveitem)//�������� ���� ���� ���� �ٲ�� �������ִ� ���
        {
            enemyRenderer.color = new Color(0.7f, 0.7f, 1f);
        }
        m_fStartingPos = transform.position;
        playpos = GameObject.FindWithTag("Player").transform;
        trsDynamic = GameObject.Find("DynamicObj").transform;
        m_fHp = m_fHp * GameManager.Instance.unithpx + (isBoss ? GameManager.Instance.bosshp : 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)//�΋H���� ���
    {
        if (collision.tag == eTag.Player.ToString())//�÷��̾��� ��쿡�� �۵��ϵ���
        {

            if (!isBoss)//�Ϲ� ���ΰ��
            {
                RemoveObj();
            }
            else//�����ΰ��
            {
                hit();

            }
            player.hit();
        }
    }
    public void hit()//�ǰݽ�
    {
        //Debug.Log("�� ����");
        m_fHp--;
        if (m_fHp <= 0)//ü���� 0�� �Ǹ�
        {
            int itemlist=Random.Range((int)PoolingManager.ePoolingObject.Follower, (int)PoolingManager.ePoolingObject.MinionA);
            
            if (!isBoss)//������ �ƴѰ�� ���� �ö󰡰� ���� ��ȯ ī��Ʈ�� �����ϸ� ������ ���� ���� ��� �������� �����.
            {
                GameManager.Instance.Scoretext(point);
                GameManager.Instance.bosscount();
                if (haveitem&&!alreadydeath)
                {
                    alreadydeath = true;
                    Debug.Log("�� ������ ����");
                    GameObject item = PoolingManager.Instance.CreateObj((PoolingManager.ePoolingObject)itemlist,trsDynamic);
                    item.GetComponent<Itemlist>().pos(transform.position);
                }
            }
            else if (isBoss&&!alreadydeath)//�����ΰ�� + �������� ���缭 ������ ȣ��Ǵ� ��� ����
            {
                alreadydeath = true;
                Debug.Log("���� ������ ����");
                GameManager.Instance.bosskill();
                GameObject item = PoolingManager.Instance.CreateObj((PoolingManager.ePoolingObject)itemlist,trsDynamic);
                item.GetComponent<Itemlist>().pos(transform.position);
            }
            RemoveObj();
        }
    }
    void Update()
    {
        movingandshoot();
        checkbossspawn();
    }
    void checkbossspawn()
    {
        bool spawnbosscheck = GameManager.Instance.bossspawn;
        if (spawnbosscheck && !isBoss)
        {
            RemoveObj();
        }

    }
    void movingandshoot()
    {
        if (!isBoss && !isshoot)//������ �ƴϸ鼭 �ѽ�� ���� �ƴѰ�� �������⸸ ��
        {
            transform.position += Vector3.down * m_fSpeed * Time.deltaTime;
        }
        else//���� �� �ѽ�� ���ΰ��
        {
            //StartCoroutine(Shootenemymov());
            //�ڷ�ƾ ���� update�� ���� �ʵ��� ��ġ->�� �̽� ����
            m_timer += Time.deltaTime;
            startmoving();//���۹��� �Լ� ȣ��
            if (m_timer > 2f)//Ư�� �ð��� ������ �������
            {
                shoot();
                if (m_timer > 3f)//Ư���ð��� ������ ������ �̵��ϵ���
                {

                    sidemoving();
                }

            }
            
        }
    }
    /*IEnumerator Shootenemymov()
    {
        startmoving();//���� ������ 1ȸ��
        yield return new WaitForSeconds(m_actspeed * 2);
        shoot();//���� ��
        if (!isBoss)//�����ΰ�� �ٷο����̰� �ƴѰ�� �� �ִٰ� �����̰�
        {
            yield return new WaitForSeconds(m_actspeed);
        }
        sidemoving();//�����ΰ��� �Դٰ���, �ƴѰ��� ������ �� ��������
        yield break;
    }*/
    void startmoving()
    {
        if (firstmov)//ó�� �����̴��� Ȯ��
        {
            if (isBoss)//�����ΰ��
            {
                m_fRatioY += Time.deltaTime;
                if (m_fRatioY > 1.5f)//�ð� ������ ������ ���߰�
                {
                    firstmov = false;
                }
                Vector3 Destination = transform.localPosition;
                Destination.y = Mathf.SmoothStep(m_fStartingPos.y, 2.5f, m_fRatioY);//2.5f�� ������(���) ��¦ ��
                transform.localPosition = Destination;
            }
            else
            {
                m_fRatioY += Time.deltaTime * 0.5f;
                if (m_fRatioY > 1.0f)//�ð� ������ ������ ���߰�
                {
                    firstmov = false;
                }
                Vector3 Destination = transform.localPosition;
                Destination.y = Mathf.SmoothStep(m_fStartingPos.y, 4f, m_fRatioY);
                transform.localPosition = Destination;
            }
        }
    }
    void shoot()
    {
        if (isBoss)//������ �� ����
        {
            if (patternchange)//����ä������ ������ ��� ���� ����
            {
                timer += Time.deltaTime;
                if (timer > m_bosspattern)
                {
                    Debug.Log("���Ϻ���");
                    pattern = Random.Range(0, 3);
                    timer = 0.0f;
                    patternchange = false;
                }
                return;
            }
            switch (pattern)
            {
                case 0://���漦
                    {

                        timer += Time.deltaTime;
                        if (timer > 1f)
                        {
                            timer = 0f;
                            shoottime++;
                            for (int j = 1; j <= 3; j++)
                            {
                                Vector3 pos = transform.position + new Vector3(1f * (j / 2) * Mathf.Pow(-1, j), -1.3f, 0f);
                                GameObject obj = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Enemy_Bullet_C, trsDynamic);
                                obj.GetComponent<Missile>().checkposition(pos);
                                obj.GetComponent<Missile>().m_speed = m_bossmissilespeed;
                            }
                        }
                        if (shoottime >= 5)
                        {
                            Debug.Log("���漦 �ٽ�");
                            patternchange = true;
                            shoottime = 0;
                            return;
                        }
                    }
                    break;
                case 1: //����
                    {
                        timer += Time.deltaTime;
                        if (timer > 1)
                        {
                            timer = 0f;
                            shoottime++;
                            Vector3 pos = playpos.position - transform.position;
                            float angle = Mathf.Atan2(pos.y, pos.x) * 180 / Mathf.PI - 90;
                            for (int j = 1; j <= 5; j++)
                            {
                                float shootangle = angle + 15 * (j / 2) * Mathf.Pow(-1, j);
                                GameObject obj = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Enemy_Bullet_B, trsDynamic);
                                obj.GetComponent<Missile>().checkrotation(shootangle, transform.position);

                                obj.GetComponent<Missile>().m_speed = m_bossmissilespeed;
                            }
                        }
                        if (shoottime >= 5)
                        {
                            Debug.Log("���� �ٽ�");
                            patternchange = true;
                            shoottime = 0;
                            return;
                        }
                    }
                    break;
                case 2: //������
                    {

                        timer += Time.deltaTime;
                        if (timer > 0.3)
                        {
                            Vector3 pos = playpos.position - transform.position;//������-������=�Ÿ�����
                            GameObject obj = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Enemy_Bullet_D, trsDynamic);
                            obj.GetComponent<Missile>().checkrotation(pos, transform.position);

                            obj.GetComponent<Missile>().m_speed = m_bossmissilespeed;
                            timer = 0;
                            shoottime++;
                        }
                        if (shoottime >= 20)
                        {

                            Debug.Log("���ؼ� �ٽ�");
                            patternchange = true;
                            shoottime = 0;
                            return;
                        }
                    }
                    break;
            }
        }
        else //������ �ƴѰ�� 1�� ��� ������ ������
        {
            if (!bulletshoot)
            {
                Vector3 pos = playpos.position - transform.position;//������-������=�Ÿ�����
                GameObject obj = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Enemy_Bullet_A, trsDynamic);
                obj.GetComponent<Missile>().checkrotation(pos, transform.position);
                bulletshoot = true;
            }
        }
    }
    void sidemoving()//������ �����̴� �Լ�
    {
        if (isBoss)//�����ΰ�� �Դٰ��� �ϴ� ���
        {
            Vector3 currentpos = Camera.main.WorldToViewportPoint(transform.position);
            if (isrightmov)
            {
                transform.position += Vector3.right * Time.deltaTime * m_fSpeed;
                if (currentpos.x > 0.75f)
                {
                    isrightmov = false;
                }
            }
            else
            {
                transform.position += Vector3.left * Time.deltaTime * m_fSpeed;
                if (currentpos.x < 0.25f)
                {
                    isrightmov = true;
                }
            }
        }
        else//�Ϲ� ���ΰ�� ������ Ȥ�� �������� ��������
        {
            if (m_fStartingPos.x > 0f)
            {
                transform.position += Vector3.right * Time.deltaTime;
            }
            else
            {
                transform.position += Vector3.left * Time.deltaTime;
            }
        }

    }
    public void Haveitem(bool _haveitem)//�������� ������ �Ǹ� ���� �ٲٴ� ���
    {
        haveitem = _haveitem;
    }
}
