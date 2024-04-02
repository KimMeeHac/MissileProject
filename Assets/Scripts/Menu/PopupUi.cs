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
    public string msg="정말로 그걸 할것인지 메세지";
    // Start is called before the first frame update
    void Start()
    { 
        gameObject.SetActive(false);
        scripttext.text = msg;
//        yesbtn.onClick.AddListener(() =>
//        {
//#if UNITY_EDITOR//유니티 에디터에서 실행중일때
//            UnityEditor.EditorApplication.isPlaying = false;
//#else//그외의 플랫폼일때
//            Application.Quit();
//#endif
//        });
//        nobtn.onClick.AddListener(() =>
//        {
//            Menu.gameObject.SetActive(true);
//            transform.gameObject.SetActive(false);
//        });
    }

    public void ShowPopup(string _popupText, UnityAction _yes = null, string _yesBtnName = "",  UnityAction _no = null, string _noBtnName = "")//팝업 화면 띄우기
    {
        scripttext.text = _popupText;
        if(_yes != null) //예스 버튼을 누르는경우 예스 버튼의 이름을 지정하고, 이때 무슨 기능을 하는지 
        {
            yesbtn.onClick.AddListener(_yes);
            yesbtnName.text = _yesBtnName;
        }
        yesbtn.gameObject.SetActive(_yes != null);

        if(_no != null) //노 버튼을 누르는 경우 노 버튼의 이름을 지정하고, 이 팝업창이 닫히게 되도록 기능
        {
            nobtn.onClick.AddListener(_no);
            nobtnName.text = _noBtnName;
        }
        nobtn.gameObject.SetActive(_no != null);

        gameObject.SetActive(true);
    }

    public void Close()//닫기 버튼을 누르는 경우
    {
        gameObject.SetActive(false);
        scripttext.text = string.Empty;
        yesbtn.onClick.RemoveAllListeners();
        nobtn.onClick.RemoveAllListeners();
    }
}
