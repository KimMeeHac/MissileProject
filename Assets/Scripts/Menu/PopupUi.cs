using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PopupUi : MonoBehaviour
{
    public static PopupUi Instance;

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

    [SerializeField]TextMeshProUGUI scripttext;
    [SerializeField]Button yesbtn;
    [SerializeField]TMP_Text yesbtnName;
    [SerializeField]Button nobtn;
    [SerializeField] TMP_Text nobtnName;
    [SerializeField] Canvas Menu;
    public string msg="������ �װ� �Ұ����� �޼���";
    // Start is called before the first frame update
    void Start()
    { 
        gameObject.SetActive(false);
        scripttext.text = msg;
//        yesbtn.onClick.AddListener(() =>
//        {
//#if UNITY_EDITOR//����Ƽ �����Ϳ��� �������϶�
//            UnityEditor.EditorApplication.isPlaying = false;
//#else//�׿��� �÷����϶�
//            Application.Quit();
//#endif
//        });
//        nobtn.onClick.AddListener(() =>
//        {
//            Menu.gameObject.SetActive(true);
//            transform.gameObject.SetActive(false);
//        });
    }

    public void ShowPopup(string _popupText, UnityAction _yes = null, string _yesBtnName = "",  UnityAction _no = null, string _noBtnName = "")
    {
        scripttext.text = _popupText;
        if(_yes != null) 
        {
            yesbtn.onClick.AddListener(_yes);
            yesbtnName.text = _yesBtnName;
        }
        yesbtn.gameObject.SetActive(_yes != null);

        if(_no != null) 
        {
            nobtn.onClick.AddListener(_no);
            nobtnName.text = _noBtnName;
        }
        nobtn.gameObject.SetActive(_no != null);

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        scripttext.text = string.Empty;
        yesbtn.onClick.RemoveAllListeners();
        nobtn.onClick.RemoveAllListeners();
    }
}
