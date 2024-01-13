using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Button GameStart;
    [SerializeField] Button Ranking;
    [SerializeField] Button Exit;
    [SerializeField] Button Option;
    [SerializeField] Canvas beforegameset;
    // Start is called before the first frame update
    void Start()
    {
        //beforegameset.transform.root
        GameStart.onClick.AddListener(() =>
        {
            beforegameset.enabled = true;
        });
    }
}
