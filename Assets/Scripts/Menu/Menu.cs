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
            //popup.gameObject.GetComponent<PopupUi>().msg = "���� �����Ͻðڽ��ϱ�?";
            //transform.gameObject.SetActive(false);

            PopupUi.Instance.ShowPopup("���� �����Ͻðڽ��ϱ�",
                () =>
                {
#if UNITY_EDITOR//����Ƽ �����Ϳ��� �������϶�
                    UnityEditor.EditorApplication.isPlaying = false;
#else//�׿��� �÷����϶�
                                Application.Quit();
#endif
                }, "����",
                () =>
                {
                    PopupUi.Instance.Close();
                },
                "â�ݱ�");
        });
        Option.onClick.AddListener(() =>
        {
            option.gameObject.SetActive(true);
            transform.gameObject.SetActive(false);

        });
    }
}
