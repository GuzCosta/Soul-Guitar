using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PPText : MonoBehaviour
{
    public new string name;
    void Update()
    {
        GetComponent<TMP_Text>().text = PlayerPrefs.GetInt(name) + "";
    }
}
