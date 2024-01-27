using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] List<GameObject> m_listEnemy;
    [SerializeField] List<GameObject> m_listItem;
    [SerializeField] GameObject m_objBoss;
    [SerializeField] GameObject m_objPlayer;
    [SerializeField] Transform dynamicobj;
    [SerializeField] TextMeshProUGUI Score;
    [SerializeField] TextMeshProUGUI BestScore;
    [SerializeField] Transform Boomgroup;
    [SerializeField] GameObject boomicon;
    Vector3 spawnaxis;
    float m_fTimer;
    float spawnLv;
    float m_misspeed=3f;
    float m_miscooldown=5f;
    bool spawn;
    int bosshp = 10;
    int unithpx = 0;
    int bosskillcount=0;
    [SerializeField]public bool bossspawn;
    int iRand;
    int totalScore;
    float droprate;
    int boomcount = 0;
    [SerializeField]int spawncount = 0;
    [Tooltip("몹 스폰 시간")][SerializeField] float m_spawnTime = 1f;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        spawnLv = 2f;
        spawnaxis.x = Camera.main.ViewportToWorldPoint(new Vector3(0.15f, 0f)).x;//몹 스폰 왼쪽 한계선
        spawnaxis.y = Camera.main.ViewportToWorldPoint(new Vector3(0.85f, 0f)).x;//몹 스폰 오른쪽 한계선
    }

    // Update is called once per frame
    void Update()
    {
        checkspawn();
        spawnboss();
    }
    void checkspawn()//스폰 확인
    {
        if (bossspawn)//보스 스폰시 사라지게
        {
            return;
        }
        m_fTimer += Time.deltaTime;
        if (m_fTimer > spawnLv)//특정 시간이 지나면 스폰되도록
        {
            m_fTimer = 0f;
            spawn = true;
        }
        if (spawn)
        {
            iRand = Random.Range(0, m_listEnemy.Count);//어떤 몹을 소환하는지 0~?까지
            string findname = m_listEnemy[iRand].name;//그 몹의 이름
            GameObject objenemy = PoolingManager.Instance.CreateObj(findname, dynamicobj);//풀링으로 몹 생성
            objenemy.GetComponent<Enemy>().m_fHp *= unithpx;
            objenemy.GetComponent<Enemy>().point = iRand + 1;
            objenemy.transform.position = new Vector3(Random.Range(spawnaxis.x, spawnaxis.y), 6f);//몹 생성 위치
            droprate = Random.Range(0.0f, 100.0f);//아이템 드랍할 몹 생성 확률
            if (droprate >= 70.0f)//70 이상시 아이템 드랍하도록 처리
            {
                objenemy.GetComponent<Enemy>().Haveitem(true);
            }
            spawn = false;
        }
    }
    public void bosscount()//보스 스폰 카운터
    {
        spawncount++;
    }
    void spawnboss()//보스 스폰시
    {
        if (spawncount <= 5)
        {
            return;
        }
        else
        {
            spawncount = 0;
            bossspawn = true;
            GameObject objboss = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Enemy_Boss, dynamicobj);
            objboss.GetComponent<Enemy>().m_fHp = bosshp;
            objboss.GetComponent<Enemy>().m_bossmissilespeed = m_misspeed;
            objboss.GetComponent<Enemy>().m_bosspattern = m_miscooldown;
            objboss.transform.position = new Vector3(0f, 4f);
        }
    }
    public void bosskill()//보스가 죽었을 때
    {
        bosskillcount++;
        if (bosskillcount % 2 == 1)
        {
            m_misspeed += 0.2f;
            //발사체 속도 증가
        }
        else
        {
            //총 쏘고 난 후 딜레이 시간 감소
            m_miscooldown -= 0.5f;
            if (m_miscooldown < 1.5f)
            {
                m_miscooldown = 1.5f;
            }
        }
        bosshp += 10;
        unithpx += 1;
        spawnLv -= 0.1f;
        if (spawnLv <= 0f)
        {
            spawnLv = 0.5f;
        }
        bossspawn = false;
    }
    public void Scoretext(int _score)
    {
        totalScore += _score;
        Score.text = totalScore.ToString("D5");
    }
    public void Boomlist(bool _use)
    {
        if (boomcount < 3 && !_use)
        {
            boomcount++;
            Instantiate(boomicon, Boomgroup);
        }
        else if (boomcount > 0 && _use)
        {
            boomcount--;
        }
      
    }
}
