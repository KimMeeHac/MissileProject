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
    [SerializeField] GameObject Hpbackground;
    [SerializeField] Transform Hpbar;
    [SerializeField] List<GameObject> Hppoint;
    SpriteRenderer playerRenderer;
    bool m_invincibile = false;
    float m_invinciTimer = 0f;
    float missilespeed;
    int m_maxplayershootlv = 7;
    bool a;
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
        Hppoint = new List<GameObject>();
        //Hppoint.Add(transform.);
        /*Debug.Log(m_shootType);
        Debug.Log(keytype);*/
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
            playerHP--;//체력감소
            m_invincibile = true;//무적기능 켜짐
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
        }
    }
    // Update is called once per frame
    void Update()
    {
        moving();//유저가 움직이는 기능
        checkposition();//유저가 화면 안에서 있게하는기능
        invincible();//피격시 무적기능
        shoot();//발사 기능
    }
    void moving()//움직이는 기능
    {
        float hori=0, vert=0;
        if (!keytype)//키보드만 쓸 경우
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                vert = 1f;
            }
            if(Input.GetKey(KeyCode.DownArrow)) 
            { 
                vert = -1f;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                hori = 1f;
            }
            if(Input .GetKey(KeyCode.LeftArrow))
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
    public void hpup()
    {
        if (playerHP < 5)
        {
            playerHP++;
            Instantiate(Hppoint, Hpbar);
        }
        else
        {
            Debug.Log("풀피");
        }
    }
    void shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space) &&!keytype)
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
                        obj.GetComponent<Missile>().checkguided( m_objBarrel.transform.position, missilespeed);
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
                                    obj4.GetComponent<Missile>().checkguided( m_objBarrel.transform.position, missilespeed);
                                }
                            }
                        }
                    }
                }
                break;
        }
    }
}
