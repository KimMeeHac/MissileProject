using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    Button btn;
    // Start is called before the first frame update
    ToggleOption toggleOption;
    Subscribing text;

    void Start()
    {
        btn = GetComponent<Button>();
        toggleOption = GetComponentInParent<ToggleOption>();
        text = transform.root.GetComponentInChildren<Subscribing>();
        btn.onClick.AddListener(() =>
        {
            text.scribe(toggleOption.OnclickButton(btn));
            //toggleOption.OnclickButton(btn);
        });
    }
}
