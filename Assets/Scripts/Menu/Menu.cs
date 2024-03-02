using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour//,IPointerClickHandler
{
    [SerializeField] Button GameStart;
    [SerializeField] Button Ranking;
    [SerializeField] Button Exit;
    [SerializeField] Button Option;
    [SerializeField] Canvas beforegameset;
    [SerializeField] Canvas popup;
    [SerializeField] Canvas option;
    [SerializeField] Canvas ranking;
    // Start is called before the first frame update
    void Start()
    {
        //beforegameset.transform.root
        GameStart.onClick.AddListener(() =>
        {
            beforegameset.gameObject.SetActive(true);
            transform.gameObject.SetActive(false);
        });
        Ranking.onClick.AddListener(() =>
        {
            ranking.gameObject.SetActive(true);
            transform.gameObject.SetActive(false);
        });
        Exit.onClick.AddListener(() =>
        {

            //popup.gameObject.SetActive(true);
            //popup.gameObject.GetComponent<PopupUi>().msg = "정말 종료하시겠습니까?";
            //transform.gameObject.SetActive(false);

            PopupUi.Instance.ShowPopup("정말 종료하시겠습니까",
                () =>
                {
#if UNITY_EDITOR//유니티 에디터에서 실행중일때
                    UnityEditor.EditorApplication.isPlaying = false;
#else//그외의 플랫폼일때
                                Application.Quit();
#endif
                }, "종료",
                () =>
                {
                    PopupUi.Instance.Close();
                },
                "창닫기");
        });
        Option.onClick.AddListener(() =>
        {
            option.gameObject.SetActive(true);
            transform.gameObject.SetActive(false);

        });
    }
}
