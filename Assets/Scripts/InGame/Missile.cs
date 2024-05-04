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
    private void OnBecameInvisible()//�� ���Ͻ� �ı�,Ǯ�� ����ؼ� ��ȯ�ϴ°� ���� �ʿ�
    {
        RemoveObj();
    }
    public void RemoveObj()//Ǯ�� ��ȯ 
    {
        //�ݳ��ϴ� �̻��ϵ��� �ʱ�ȭ�� �������
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

    public void checkrotation(Vector3 _vec, Vector3 _pos)//���⺤�͸� �޾Ƽ� �������
    {
        transform.position = _pos;
        transform.rotation = Quaternion.Euler(0f, 0f, (Mathf.Atan2(_vec.y, _vec.x) * 180 / Mathf.PI) - 90);
    }
    public void checkrotation(float _angle, Vector3 _pos)//������ �ʿ��Ҷ�
    {
        transform.position = _pos;
        transform.rotation = Quaternion.Euler(0f, 0f, _angle);//�Ѿ��� ������ ������
    }
    public void checkposition(Vector3 _pos)//��ġ�� �ʿ��Ҷ�
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
    /*public void checkguided(Vector3 _pos, float _speed)//�����̻���
    {
        guiding = true;
        setguided = true;//������� �ѱ�
        transform.position = _pos;//�߻� ��ġ
        m_speed = _speed;//�߻� �ӵ�
    }*/

    /*private void OnTriggerEnter2D(Collider2D collision)//Ǯ�� ����� ��, �΋H���� ��� 
    {
        if (players && (collision.tag == eTag.Enemy.ToString()
            || collision.tag == eTag.Boss.ToString()))//�� �̻����ΰ��
        {
            //Debug.Log("�� �̻��� ����");
            Enemy target = collision.GetComponent<Enemy>();
            target.hit();
            RemoveObj();
        }
        if ((enemys || bosses) && collision.tag == eTag.Player.ToString())//�� �̻����� ���
        {
            //Debug.Log("�� �̻��� ����");
            Player player= collision.GetComponent<Player>();
            player.hit();
            RemoveObj();
        }
    }*/
    //�� ����� �����̻����� ��쿣 �����
    //�ڽ�(Range�� circle �ݶ��̴�)���� Ÿ���� �޾ƿ��� �۾��� �ʿ�
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
    //�̻����� ��� ȣ��Ǹ鼭 �����̴� ��
    //������ �����ָ� �װ����� ������ �̵��ϰ� �ϸ� ��
    //���� ����ź�ΰ�� �̰����� ������ ��� �����ϸ鼭 �̵��ϵ��� �����ϰ� ��
    {
        //CŸ�� ���� �̻��� �߻�� ��ġ�� ���⺤��*�ӵ� ����
        //transform.position += mypos * m_speed * Time.deltaTime;
        transform.position += transform.up * m_speed * Time.deltaTime;//���� ��������� �� �ö�
    }
    /*public void target()//�����̻����ΰ��-�Ӹ� ��ġ�� �ٲ�°��
    {
        if (setguided)//�����̻����ΰ��
        {
            if (Target != null && Target.transform.parent.name == Target.name)
            {
                Target = null;
            }


            if (Target == null && range.enabled)//Ÿ���� �������� �ʾҰ� Range ��ũ��Ʈ�� �����ִ� ���
            {
                range.SetTarget(false);//range��ũ��Ʈ���� Ÿ���� �����ؿ���� ���
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
                range.enabled = false;//range��ũ��Ʈ�� �������� Ÿ���� ���� �������� �ʵ��� ��
                if (Target == null)//�ٸ� �̻��Ͽ� ���� Ÿ���� �����ٸ�
                {
                    transform.rotation=Quaternion.Euler(0f, 0f, a);//�Ӹ��� ���ڱ� Ʋ������ �ʵ��� ��
                    return;//��������� ���ߵ��� ��
                }
                eunit = Target.transform;
                //���� �̻����� ���� ��� �˰���
                pos = eunit.position - transform.position;
                angle = Mathf.Atan2(pos.y, pos.x) * 180 / Mathf.PI - 90;
                if ((angle >= 60 || angle <= -60))
                //Ư�� ������ �����,���� �ٽ� null�ɽ� ������� ��Ȱ��ȭ
                {
                    Debug.Log("�� �̻����� ���� ����");
                    setguided = false;
                }
                a += (angle / 1.2f) * Time.deltaTime;
                if((a>=0&&a>=angle)|| (a < 0 && a <= angle))
                {
                    a = angle;
                }
                transform.rotation = Quaternion.Euler(0, 0, a);
            }
            //�̻��ϰ� ���� ����� ���� ���� ??-
            //if �� ��ġ ã���� ��ĵ ����� ��Ȱ��ȭ
            //if Ư�� ������ ����ų� ���� �ٽ� null�� �ȴٸ� ���� ����� ��Ȱ��ȭ
        }
    }
    public void Targetset(Transform _target)
    {
        Target = _target.GetComponent<Enemy>();
    }*/
}
