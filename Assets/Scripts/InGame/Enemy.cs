
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
    public int point = 0;
    Player player;
    private void OnBecameInvisible()//맵에서 사라지면 제거되도록
    {
        RemoveObj();
    }
    void RemoveObj()//오브젝트를 다시 풀링매니저에 돌려놓는 기능
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
        if (haveitem)//아이템을 가진 몹이 색이 바뀌도록 설정해주는 기능
        {
            enemyRenderer.color = new Color(0.7f, 0.7f, 1f);
        }
        m_fStartingPos = transform.position;
        playpos = GameObject.FindWithTag("Player").transform;
        trsDynamic = GameObject.Find("DynamicObj").transform;
        m_fHp = m_fHp * GameManager.Instance.unithpx + (isBoss ? GameManager.Instance.bosshp : 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)//부딫히는 경우
    {
        if (collision.tag == eTag.Player.ToString())//플레이어인 경우에만 작동하도록
        {

            if (!isBoss)//일반 몹인경우
            {
                RemoveObj();
            }
            else//보스인경우
            {
                hit();

            }
            player.hit();
        }
    }
    public void hit()//피격시
    {
        //Debug.Log("피 깎임");
        m_fHp--;
        if (m_fHp <= 0)//체력이 0이 되면
        {
            int itemlist=Random.Range((int)PoolingManager.ePoolingObject.Follower, (int)PoolingManager.ePoolingObject.MinionA);
            
            if (!isBoss)//보스가 아닌경우 점수 올라가고 보스 소환 카운트가 증가하며 아이템 보유 몹인 경우 아이템을 만든다.
            {
                GameManager.Instance.Scoretext(point);
                GameManager.Instance.bosscount();
                if (haveitem&&!alreadydeath)
                {
                    alreadydeath = true;
                    Debug.Log("몹 아이템 생성");
                    GameObject item = PoolingManager.Instance.CreateObj((PoolingManager.ePoolingObject)itemlist,trsDynamic);
                    item.GetComponent<Itemlist>().pos(transform.position);
                }
            }
            else if (isBoss&&!alreadydeath)//보스인경우 + 여러발을 맞춰서 여러번 호출되는 기능 방지
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
        if (spawnbosscheck && !isBoss)
        {
            RemoveObj();
        }

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
            startmoving();//시작무빙 함수 호출
            if (m_timer > 2f)//특정 시간이 지나면 총을쏘도록
            {
                shoot();
                if (m_timer > 3f)//특정시간이 지나면 옆으로 이동하도록
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
            if (isBoss)//보스인경우
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
        if (isBoss)//보스의 총 패턴
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
    void sidemoving()//옆으로 움직이는 함수
    {
        if (isBoss)//보스인경우 왔다갔다 하는 기능
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
        else//일반 몹인경우 오른쪽 혹은 왼쪽으로 빠지도록
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
