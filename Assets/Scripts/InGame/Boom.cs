using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Boom : MonoBehaviour
{
    int Boomdamage;
    float timer;
    [SerializeField] int boomspeed;
    bool limiter=false;
    bool isstop = false;
    void Start()
    {
        timer=0;
    }
    void RemoveObject()
    {
        if (PoolingManager.Instance != null)
        {
            PoolingManager.Instance.RemovePoolingObject(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag ==eTag.Enemy.ToString()||collision.tag==eTag.Boss.ToString())
        {
            Enemy target=collision.GetComponent<Enemy>();
            target.hit(Boomdamage);
        }
    }
    private void OnBecameInvisible()
    {
        RemoveObject();
    }
    public void boomdamage(int _damage)
    {
        Boomdamage=_damage*10;
        if (_damage < 3)
        {
            limiter = true;
        }
    }
    void spawntime()
    {
        timer += Time.deltaTime;
        if(timer > 3f&&limiter)
        {
            RemoveObject();
        }
    }
    public void checkposition(Vector3 _pos)
    {
        transform.position= _pos;
    }
    void Update()
    {
        if (isstop == false)
        {
            transform.position += transform.up*boomspeed*Time.deltaTime;

        }
        spawntime();
    }

    void stopmoving()
    {
        isstop = true;
    }
}
