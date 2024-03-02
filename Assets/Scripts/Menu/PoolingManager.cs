using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public enum ePoolingObject
    {
        Enemy_Boss,
        Enemy_Bullet_A,
        Enemy_Bullet_B,
        Enemy_Bullet_C,
        Enemy_Bullet_D,
        EnemyA,
        EnemyB,
        EnemyC,
        Explosion,
        Follower,
        Item_Boom,
        Item_Coin,
        Item_Power,
        MinionA,
        MinionB,
        MinionC,
        Player_Bullet_A,
        Player_Bullet_B,
        Player_Bullet_C,
        Boomicon
    }
    [System.Serializable]
    public class cPoolingObject
    {
        public GameObject obj;
        public int count;
        [TextArea] public string description;
    }
    public List<cPoolingObject> m_listPoolingObj;
    public static PoolingManager Instance;
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
        DontDestroyOnLoad(this);
        initPoolingParents();
        initPoolingChild();
    }
    void initPoolingParents()//풀링 종류 ex) 총알,적 기체 등의 게임 오브젝트 생성(폴더생성느낌)
    {
        List<string> listParentName = new List<string>();//풀링 종류의 이름을 정의

        int count=transform.childCount;//풀링 종류의 자식 갯수 정의
        for(int iNum=0;iNum<count; iNum++)//풀링 부모 이름을 자식 이름으로
        {
            string name=transform.GetChild(iNum).name;
            listParentName.Add(name);
        }
        count=m_listPoolingObj.Count;//풀링 부모 갯수
        for(int iNum=0; iNum<count; iNum++)
        {
            if (m_listPoolingObj[iNum].obj == null)
            {
                continue;
            }
            cPoolingObject data=m_listPoolingObj[iNum];

            string name=data.obj.name;
            bool exist = listParentName.Exists(x => x == name);
            if (exist)
            {
                listParentName.Remove(name);
            }
            else
            {
                GameObject objParent = new GameObject();
                objParent.transform.SetParent(transform);
                objParent.name = name;
            }
        }
        count = listParentName.Count;
        for(int iNum = count-1; iNum > -1; iNum--)
        {
            GameObject obj = transform.Find(listParentName[iNum]).gameObject;
            Destroy(obj);
        }
    }

    void initPoolingChild()//파일을 만드는 느낌
    {
        int count= m_listPoolingObj.Count;
        for(int iNum=0;iNum<count;iNum++)
        {
            if (m_listPoolingObj[iNum] == null)//리스트 비어있을경우 지나가게처리 
            {
                continue;
            }

            cPoolingObject objPooling = m_listPoolingObj[iNum];
            GameObject obj = m_listPoolingObj[iNum].obj;
            string name=obj.name;
            Transform parent = transform.Find(name);
            int objcount = parent.childCount;
            int diffcount=Mathf.Abs(objcount - objPooling.count);

            if (objcount > objPooling.count)
            {
                for(int jNum = diffcount - 1; jNum > objPooling.count-1; jNum--)
                {
                    GameObject delobj=parent.GetChild(jNum).gameObject;
                    Destroy (delobj);
                }
            }
            if(objcount < objPooling.count)
            {
                for(int icNum = 0;icNum < diffcount; icNum++)
                {
                    GameObject iobj = Instantiate(m_listPoolingObj.Find(x => x.obj.name == name).obj);
                    iobj.SetActive(false);
                    iobj.name = name;
                    iobj.transform.SetParent(parent);
                }
            }

        }
    }

    public GameObject CreateObj(ePoolingObject _value,Transform _parent)
    {
        string findObjname=_value.ToString().Replace("_", " ");
        return GetPoolingObject(findObjname, _parent);
    }
    public GameObject CreateObj(string _name,Transform _parent)
    {
        return GetPoolingObject(_name, _parent);
    }

    GameObject GetPoolingObject(string _name,Transform _parent)
    {
        Transform parent = transform.Find(_name);
        if (parent == null)
        {
            Debug.LogError($"프리팹에 오브젝트 없음 - {_name}");
            return null;
        }
        GameObject returnvalue = null;
        if(parent.childCount > 0)
        {
            returnvalue=parent.GetChild(0).gameObject;
        }
        else
        {
            GameObject iobj = Instantiate(m_listPoolingObj.Find(x => x.obj.name == _name).obj);
            iobj.name = _name;
            returnvalue = iobj;
        }
        returnvalue.transform.SetParent(_parent);
        returnvalue.SetActive(true);
        return returnvalue;
    }
    public void RemovePoolingObject(GameObject _obj)
    {
        string name = _obj.name;
        Transform parent = transform.Find(name);
        cPoolingObject poolingObject = m_listPoolingObj.Find(x => x.obj.name == name);
        int poolingCount = poolingObject.count;
        if (parent.childCount < poolingCount)
        {
            _obj.transform.SetParent(parent);
            _obj.SetActive(false);
            _obj.transform.position=Vector3.zero;
        }
        else
        {
            Destroy(_obj);
        }
    }
}
