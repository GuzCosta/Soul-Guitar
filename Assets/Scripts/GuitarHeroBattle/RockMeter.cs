using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMeter : MonoBehaviour
{

    float rm;
    GameObject Needle;

    // Start is called before the first frame update
    void Start()
    {
        Needle = transform.Find("Needle").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        rm = PlayerPrefs.GetInt("RockMeter");
        Needle.transform.localPosition = new Vector3((rm - 25) / 16.666f, 0, 0);
    }
}
