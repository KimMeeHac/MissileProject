using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    [SerializeField] Button Delete;
    [SerializeField] Button Return;
    [SerializeField] Canvas Menu;
    [SerializeField] List<TextMeshProUGUI> Rankinglist;
    // Start is called before the first frame update
    void Start()
    {
        Return.onClick.AddListener(() =>
        {
            Menu.gameObject.SetActive(true);
            transform.gameObject.SetActive(false);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
