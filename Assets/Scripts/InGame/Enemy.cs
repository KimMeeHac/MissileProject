
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("보스,총쏘는지")]
    [SerializeField] bool isBoss = false;
    [SerializeField] bool isshoot = false;
    [Header("기체 체력과 이속")]
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
    Player player;
    private void OnBecameInvisible()
    {
        RemoveObj();
    }
    void RemoveObj()
    {
        if (PoolingManager.Instance != null)
        {
            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        //Application.targetFrameRate = 60;
        if (haveitem)//아이템을 가진 몹이 색이 바뀌도록 설정해주는 기능
        {
            enemyRenderer.color = new Color(0.7f, 0.7f, 1f);
        }
        m_fStartingPos = transform.position;
        playpos = GameObject.FindWithTag("Player").transform;
        trsDynamic = GameObject.Find("DynamicObj").transform;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == eTag.Player.ToString())
        {
            RemoveObj();
            player.hit();
        }
    }
    public void hit()
    {
        //Debug.Log("피 깎임");
        m_fHp--;
        if (m_fHp <= 0)
        {
            int itemlist=Random.Range((int)PoolingManager.ePoolingObject.Follower, (int)PoolingManager.ePoolingObject.MinionA);
            
            if (!isBoss)
            {
                GameManager.Instance.bosscount();
                if (haveitem&&!alreadydeath)
                {
                    alreadydeath = true;
                    Debug.Log("몹 아이템 생성");
                    GameObject item = PoolingManager.Instance.CreateObj((PoolingManager.ePoolingObject)itemlist,trsDynamic);
                    item.GetComponent<Itemlist>().pos(transform.position);
                }
            }
            else if (isBoss&&!alreadydeath)
            {
                alreadydeath = true;
                Debug.Log("보스 아이템 생성");
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
        if(spawnbosscheck&&!isBoss) RemoveObj();
    }
    void movingandshoot()
    {
        if (!isBoss && !isshoot)//보스가 아니면서 총쏘는 적이 아닌경우 내려가기만 함
        {
            transform.position += Vector3.down * m_fSpeed * Time.deltaTime;
        }
        else//보스 및 총쏘는 몹인경우
        {
            //StartCoroutine(Shootenemymov());
            //코루틴 사용시 update에 넣지 않도록 조치->렉 이슈 생김
            m_timer += Time.deltaTime;
            startmoving();
            if (m_timer > 2f)
            {
                shoot();
                if (m_timer > 3f)
                {

                    sidemoving();
                }

            }
            
        }
    }
    /*IEnumerator Shootenemymov()
    {
        startmoving();//등장 움직임 1회용
        yield return new WaitForSeconds(m_actspeed * 2);
        shoot();//총을 쏨
        if (!isBoss)//보스인경우 바로움직이고 아닌경우 좀 있다가 움직이게
        {
            yield return new WaitForSeconds(m_actspeed);
        }
        sidemoving();//보스인경우는 왔다갔다, 아닌경우는 옆으로 쭉 빠지도록
        yield break;
    }*/
    void startmoving()
    {
        if (firstmov)//처음 움직이는지 확인
        {
            if (isBoss)
            {
                m_fRatioY += Time.deltaTime;
                if (m_fRatioY > 1.5f)//시간 지나면 움직임 멈추게
                {
                    firstmov = false;
                }
                Vector3 Destination = transform.localPosition;
                Destination.y = Mathf.SmoothStep(m_fStartingPos.y, 2.5f, m_fRatioY);//2.5f는 기준점(가운데) 살짝 위
                transform.localPosition = Destination;
            }
            else
            {
                m_fRatioY += Time.deltaTime * 0.5f;
                if (m_fRatioY > 1.0f)//시간 지나면 움직임 멈추게
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
        if (isBoss)
        {
            if (patternchange)//패턴채인지가 켜지는 경우 패턴 변경
            {
                timer += Time.deltaTime;
                if (timer > m_bosspattern)
                {
                    Debug.Log("패턴변경");
                    pattern = Random.Range(0, 3);
                    timer = 0.0f;
                    patternchange = false;
                }
                return;
            }
            switch (pattern)
            {
                case 0://전방샷
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
                            Debug.Log("전방샷 다쏨");
                            patternchange = true;
                            shoottime = 0;
                            return;
                        }
                    }
                    break;
                case 1: //샷건
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
                            Debug.Log("샷건 다쏨");
                            patternchange = true;
                            shoottime = 0;
                            return;
                        }
                    }
                    break;
                case 2: //조준형
                    {

                        timer += Time.deltaTime;
                        if (timer > 0.3)
                        {
                            Vector3 pos = playpos.position - transform.position;//도착지-시작지=거리벡터
                            GameObject obj = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Enemy_Bullet_D, trsDynamic);
                            obj.GetComponent<Missile>().checkrotation(pos, transform.position);

                            obj.GetComponent<Missile>().m_speed = m_bossmissilespeed;
                            timer = 0;
                            shoottime++;
                        }
                        if (shoottime >= 20)
                        {

                            Debug.Log("조준샷 다쏨");
                            patternchange = true;
                            shoottime = 0;
                            return;
                        }
                    }
                    break;
            }
        }
        else //보스가 아닌경우 1번 쏘고 옆으로 빠지게
        {
            if (!bulletshoot)
            {
                Vector3 pos = playpos.position - transform.position;//도착지-시작지=거리벡터
                GameObject obj = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Enemy_Bullet_A, trsDynamic);
                obj.GetComponent<Missile>().checkrotation(pos, transform.position);
                bulletshoot = true;
            }
        }
    }
    void sidemoving()
    {
        if (isBoss)
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
        else
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
    public void Haveitem(bool _haveitem)//아이템을 가지게 되면 색을 바꾸는 기능
    {
        haveitem = _haveitem;
    }
}
