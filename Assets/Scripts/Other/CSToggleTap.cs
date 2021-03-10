using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class CSToggleTap : MonoBehaviour {

    void Start()
    {
        GetComponent<Toggle>().onValueChanged.AddListener((state) =>
        {
            CSSoundManager.instance.Tap();
        });
    }
}
