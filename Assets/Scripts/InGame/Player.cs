using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject m_objBarrel;
    [SerializeField] Transform m_dynamicObj;
    [Header("플레이어 스텟")]
    [Tooltip("체력")][SerializeField] int playerHP = 5;
    [Tooltip("이동속도 조정기능")][SerializeField] float m_fSpeed = 5f;
    [Tooltip("무적시간 조정기능")][SerializeField] float m_invinciTime = 1f;
    [Tooltip("발사 타입")][Range(1, 4)][SerializeField] int m_shootType = 1;
    [Tooltip("발사 단계")][SerializeField][Range(1, 7)] int m_playershootlv = 1;
    [SerializeField] bool keytype;
    [SerializeField] Transform Hpbackground;
    [SerializeField] Transform Hpbar;
    [SerializeField] GameObject Hppoint;
    [SerializeField] List<GameObject> hppointlist = new List<GameObject>();
    SpriteRenderer playerRenderer;
    bool m_invincibile = false;
    float m_invinciTimer = 0f;
    float missilespeed;
    int m_maxplayershootlv = 7;
    int maxplayerhp = 5;
    bool a;

    Camera mainCam;
    // Start is called before the first frame update
    private void Awake()
    {
        playerRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        m_shootType = DataManager.Instance.shootforplayer();//총쏘는 타입을 받아옴
        keytype = DataManager.Instance.keytypeforplayer();//키마 or only키보드 타입 받아옴
        m_playershootlv = 1;
        for (int i = 0; i < maxplayerhp; i++)
        {
            hppointlist.Add(Instantiate(Hppoint, Hpbar));
        }
        mainCam = Camera.main;
    }
    public void hit()
    {
        //Debug.Log("히트 함수안 들어옴");
        if (m_invincibile)//무적시간동안 체력 닳지 않도록
        {
            return;
        }
        else
        {
            m_invincibile = true;//무적기능 켜짐
            
            playerHP--;
            if (playerHP <= 0)
            {
                int endscore = GameManager.Instance.Endscore();//최종점수를 전달
                DataManager.Instance.savescore(endscore);//최종점수 저장
                Destroy(gameObject);//파괴
                PopupUi.Instance.ShowPopup($"GameOver\n최종점수:{endscore}", () =>
                {
                    SceneManager.LoadSceneAsync(0);
                }, "메인메뉴로");//게임오버 팝업
                //Debug.Log("체력0 들어옴");
            }
            else
            {
                hppointlistchange(false);
            }
                
            
        }
    }

    void Update()
    {
        moving();//유저가 움직이는 기능
        checkposition();//유저가 화면 안에서 있게하는기능
        invincible();//피격시 무적기능
        shoot();//발사 기능
        hppointposition();
    }
    void moving()//움직이는 기능
    {
        float hori = 0, vert = 0;
        if (!keytype)//키보드만 쓸 경우
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                vert = 1f;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                vert = -1f;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                hori = 1f;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                hori = -1f;
            }
            Transform PlayerTrs = GetComponent<Transform>();
            PlayerTrs.position = PlayerTrs.position + new Vector3(hori, vert) * Time.deltaTime * m_fSpeed;
        }
        else//키마 쓰는경우
        {
            if (Input.GetKey(KeyCode.W))
            {
                vert = 1f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                vert = -1f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                hori = 1f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                hori = -1f;
            }
            Transform PlayerTrs = GetComponent<Transform>();
            PlayerTrs.position = PlayerTrs.position + new Vector3(hori, vert) * Time.deltaTime * m_fSpeed;
        }
        /*float vert = Input.GetAxisRaw("Vertical");
        float hori = Input.GetAxisRaw("Horizontal");
        Transform PlayerTrs = GetComponent<Transform>();
        PlayerTrs.position = PlayerTrs.position + new Vector3(hori, vert) * Time.deltaTime * m_fSpeed;*/
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
    void invincible()//피격시 무적
    {
        if (!m_invincibile)//무적켜져있는경우 바로 리턴
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
    public void shootlvup()
    {
        m_playershootlv++;
    }
    void shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !keytype)
        {
            shootmissile();
        }
        if (Input.GetMouseButtonDown(0) && keytype)
        {
            shootmissile();
        }
    }
    void shootmissile()
    {

        if (m_playershootlv >= m_maxplayershootlv)//최대단계를 넘지 않도록 처리
        {
            m_playershootlv = m_maxplayershootlv;
        }
        switch (m_shootType)//총쏘는 타입1~4까지
        {
            case 1://1타입 직선형
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
            case 2://2타입 방사형(샷건)
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
            case 3://3타입 유도형 유도탄마다 속도가 다르게 설정, 단계가 오를수록 최저속도가 높아지도록 처리
                {
                    for (int i = 1; i <= m_playershootlv; i++)
                    {
                        missilespeed = UnityEngine.Random.Range(1.5f + i / 7, 3f);
                        GameObject obj = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Player_Bullet_A, m_dynamicObj);
                        obj.GetComponent<Missile>().checkguided(m_objBarrel.transform.position, missilespeed);
                    }
                }
                break;
            case 4://4타입 혼합형 1단계 직선샷 2단계부턴 샷건형 추가 4단계부턴 직선형 더 추가 6단계부턴 유도형 추가
                {
                    if (m_playershootlv % 2 == 1)//홀수단계시 정면직선샷
                    {
                        GameObject obj = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Player_Bullet_B, m_dynamicObj);
                        obj.GetComponent<Missile>().checkposition(m_objBarrel.transform.position);
                    }
                    if (m_playershootlv >= 2)//2단계이상 샷건
                    {
                        GameObject obj = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Player_Bullet_C, m_dynamicObj);
                        obj.GetComponent<Missile>().checkrotation(30, m_objBarrel.transform.position);
                        GameObject obj1 = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Player_Bullet_C, m_dynamicObj);
                        obj1.GetComponent<Missile>().checkrotation(-30, m_objBarrel.transform.position);

                        if (m_playershootlv >= 4)//4단계 이상시 직선샷추가
                        {
                            GameObject obj2 = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Player_Bullet_B, m_dynamicObj);
                            obj2.GetComponent<Missile>().checkposition(m_objBarrel.transform.position + new Vector3(0.4f, 0, 0));
                            GameObject obj3 = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Player_Bullet_B, m_dynamicObj);
                            obj3.GetComponent<Missile>().checkposition(m_objBarrel.transform.position + new Vector3(-0.4f, 0, 0));

                            if (m_playershootlv >= 6)//6단계 이상시 유도, 다만 속도는 기존 유도형보단 높게
                            {
                                for (int i = 0; i < 2; i++)
                                {
                                    missilespeed = UnityEngine.Random.Range(2.5f, 3f);
                                    GameObject obj4 = PoolingManager.Instance.CreateObj(PoolingManager.ePoolingObject.Player_Bullet_A, m_dynamicObj);
                                    obj4.GetComponent<Missile>().checkguided(m_objBarrel.transform.position, missilespeed);
                                }
                            }
                        }
                    }
                }
                break;
        }
    }
    void hppointposition()//월드와 캔버스를 연동할때는 카메라를 이용
    {
        Hpbackground.transform.position = mainCam.WorldToScreenPoint(transform.position) - new Vector3(0, 100);
    }
    public void hppointlistchange(bool up)//active 기능을 활용할 것
    {
        /*Debug.Log("hppointlistchange들어옴 " + hppointlist.Count);

        //hppointlist 와 기존에 있던 hppoint 숫자와 다를때 list만큼 hppoint갯수를 파괴하거나 생성하는 스크립트 생성하기
        if (up == true)//체력 추가시 리스트 추가
        {
            if (hppointlist.Count < 5)
            {
                Debug.Log("hp 추가됨");
                hppointlist.Add(Instantiate(Hppoint, Hpbar));
            }
        }
        else//체력 감소시 리스트 삭제
        {
            hppointlist.Remove(Hppoint);
            if (hppointlist.Count <= 0)
            {

            }
        }
        setHpUi();
        //for (int i = 0; i < maxplayerhp; i++)//5번 돌림(최대체력이 5이기 때문)
        //{
        //    if (hppointlist[i] == null)
        //    {
        //        break;
        //    }
        //    if (hppointlist.Count > i && hppointlist[i] != null)//hppointlist와 i를 비교 list의 크기가 크면 생성하는데 이때 hppointlist의 gameobject가 없어야함
        //    {
        //        Debug.Log("if문 들어옴");
        //        Instantiate(hppointlist[i], Hpbar);
        //    }
        //}*/
        if (up)//체력 올라갈시
        {
            if (playerHP < 5)//최대 체력보다 작을 시
            {
                playerHP++;
                hppointlist[playerHP - 1].SetActive(true);
            }

        }
        else//체력이 내려갈 시
        {
                hppointlist[playerHP].SetActive(false);
        }

    }

    /*private void setHpUi()
    {
        int count = hppointlist.Count;
        for (int iNum = 0; iNum < count; ++iNum)
        {
            hppointlist[iNum].SetActive(iNum + 1 <= playerHP);
        }
        
    }*/
}
