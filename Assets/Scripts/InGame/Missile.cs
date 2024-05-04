using Unity.VisualScripting;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] bool enemys = false;
    [SerializeField] bool players = false;
    [SerializeField] bool bosses = false;
    [SerializeField] bool guiding = false;
    [SerializeField] public float m_speed = 3f;
    [SerializeField] float m_damage = 1f;
    //Transform eunit;
    //Range range;
    //[SerializeField] Enemy Target;
    //Vector3 pos;
    //float angle;
    //[SerializeField] bool setguided = false;
    //bool targetlockon; 
    //float a = 0;
    void Start()
    {
        /*if (players&&guiding)
        {
            range = transform.GetChild(0).GetComponent<Range>();
        }*/
    }
    private void OnBecameInvisible()//안 보일시 파괴,풀링 사용해서 반환하는거 만들 필요
    {
        RemoveObj();
    }
    public void RemoveObj()//풀링 반환 
    {
        //반납하는 미사일들은 초기화를 해줘야함
        transform.rotation = Quaternion.identity;
        /*if (players&&guiding)
        {
            range.trsTarget = null;
            Target = null;
            range.enabled = true; 
        }*/
        if (PoolingManager.Instance != null)
        {
            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }
    }

    public void checkrotation(Vector3 _vec, Vector3 _pos)//방향벡터를 받아서 각도계산
    {
        transform.position = _pos;
        transform.rotation = Quaternion.Euler(0f, 0f, (Mathf.Atan2(_vec.y, _vec.x) * 180 / Mathf.PI) - 90);
    }
    public void checkrotation(float _angle, Vector3 _pos)//각도가 필요할때
    {
        transform.position = _pos;
        transform.rotation = Quaternion.Euler(0f, 0f, _angle);//총알의 각도를 정해줌
    }
    public void checkposition(Vector3 _pos)//위치만 필요할때
    {
        transform.position = _pos;
        if (bosses)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
    }
    /*public void checkguided(Vector3 _pos, float _speed)//유도미사일
    {
        guiding = true;
        setguided = true;//유도기능 켜기
        transform.position = _pos;//발사 위치
        m_speed = _speed;//발사 속도
    }*/

    /*private void OnTriggerEnter2D(Collider2D collision)//풀링 사용할 것, 부딫히는 경우 
    {
        if (players && (collision.tag == eTag.Enemy.ToString()
            || collision.tag == eTag.Boss.ToString()))//내 미사일인경우
        {
            //Debug.Log("내 미사일 맞음");
            Enemy target = collision.GetComponent<Enemy>();
            target.hit();
            RemoveObj();
        }
        if ((enemys || bosses) && collision.tag == eTag.Player.ToString())//적 미사일인 경우
        {
            //Debug.Log("적 미사일 맞음");
            Player player= collision.GetComponent<Player>();
            player.hit();
            RemoveObj();
        }
    }*/
    //이 기능은 유도미사일인 경우엔 어려움
    //자식(Range의 circle 콜라이더)에게 타겟을 받아오는 작업이 필요
    //public void checkRange(Collider2D collision)//
    //{
    //    Target = collision.GetComponent<Enemy>();
    //    range.enabled = false;
    //    Debug.Log(collision);
    //}


    void Update()
    {
        move();
        //target();
    }
    void move()
    //미사일이 계속 호출되면서 움직이는 곳
    //각도를 정해주면 그곳으로 꾸준히 이동하게 하면 됨
    //만약 유도탄인경우 이곳에서 각도를 계속 조정하면서 이동하도록 조정하게 함
    {
        //C타입 적의 미사일 발사시 위치에 방향벡터*속도 해줌
        //transform.position += mypos * m_speed * Time.deltaTime;
        transform.position += transform.up * m_speed * Time.deltaTime;//각도 정해진대로 쭉 올라감
    }
    /*public void target()//유도미사일인경우-머리 위치가 바뀌는경우
    {
        if (setguided)//유도미사일인경우
        {
            if (Target != null && Target.transform.parent.name == Target.name)
            {
                Target = null;
            }


            if (Target == null && range.enabled)//타겟이 정해지지 않았고 Range 스크립트가 켜져있는 경우
            {
                range.SetTarget(false);//range스크립트에게 타겟을 지정해오라고 명령
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
                range.enabled = false;//range스크립트를 꺼버려서 타겟을 새로 지정하지 않도록 함
                if (Target == null)//다른 미사일에 의해 타겟이 터진다면
                {
                    transform.rotation=Quaternion.Euler(0f, 0f, a);//머리가 갑자기 틀어지지 않도록 함
                    return;//유도기능을 멈추도록 함
                }
                eunit = Target.transform;
                //적과 미사일의 각도 계산 알고리즘
                pos = eunit.position - transform.position;
                angle = Mathf.Atan2(pos.y, pos.x) * 180 / Mathf.PI - 90;
                if ((angle >= 60 || angle <= -60))
                //특정 각도를 벗어날시,적이 다시 null될시 유도기능 비활성화
                {
                    Debug.Log("이 미사일의 유도 꺼짐");
                    setguided = false;
                }
                a += (angle / 1.2f) * Time.deltaTime;
                if((a>=0&&a>=angle)|| (a < 0 && a <= angle))
                {
                    a = angle;
                }
                transform.rotation = Quaternion.Euler(0, 0, a);
            }
            //미사일과 가장 가까운 적을 선별 ??-
            //if 적 위치 찾으면 스캔 기능은 비활성화
            //if 특정 각도를 벗어나거나 적이 다시 null이 된다면 유도 기능을 비활성화
        }
    }
    public void Targetset(Transform _target)
    {
        Target = _target.GetComponent<Enemy>();
    }*/
}
