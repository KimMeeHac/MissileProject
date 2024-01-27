using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Itemlist : MonoBehaviour
{
    private Vector3 m_moveItemPos;
    private float m_moveSpeed = 5f;
    float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        m_moveItemPos = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));//아이템의 처음 이동방향
    }
    public void pos(Vector3 _pos)
    {
        transform.position = _pos;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == eTag.Player.ToString())
        {
            if (transform.name =="Item Boom") {
                GameManager.Instance.Boomlist(false);
                Debug.Log("boom");
            }
            else if (transform.name =="Item Coin") {
                GameManager.Instance.Scoretext(10);
            }
            else if (transform.name =="Item Power") {
                collision.GetComponent<Player>().shootlvup();
            }
            else {
                collision.GetComponent<Player>().hpup();
                Debug.Log("hp");
            }
            RemoveObj();
        }
    }

    void RemoveObj()
    {
        if (PoolingManager.Instance != null)
        {
            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += m_moveItemPos * m_moveSpeed * Time.deltaTime;//아이템의 이동
        checkPos();//맵 바깥으로 나가지 않기위해 위치확인
        spawntime();
    }
    void spawntime()
    {
        timer += Time.deltaTime;
        if (timer > 7f)
        {
            RemoveObj();
        }
    }
    void checkPos()
    {
        Vector3 curPos = Camera.main.WorldToViewportPoint(transform.position);
        if (curPos.x < 0.05f)//왼쪽 도달시
        {
            m_moveItemPos = Vector3.Reflect(m_moveItemPos, Vector3.left);
        }
        else if (curPos.x > 0.95f)//오른쪽 도달시
        {
            m_moveItemPos = Vector3.Reflect(m_moveItemPos, Vector3.right);
        }
        if (curPos.y < 0.05f)//아래쪽 도달시
        {
            m_moveItemPos = Vector3.Reflect(m_moveItemPos, Vector3.down);
        }
        else if (curPos.y > 0.95f)//위쪽 도달시
        {
            m_moveItemPos = Vector3.Reflect(m_moveItemPos, Vector3.up);
        }
    }
}
