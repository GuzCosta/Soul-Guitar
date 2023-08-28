using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Activator : MonoBehaviour
{
    SpriteRenderer sr;
    public KeyCode key;
    bool active = false;
    GameObject note, battleManager;
    Color old;
    public bool createMode;
    public GameObject generalNote;
    public GameObject hitFire;
    public Vector2 offset = new Vector2 (0,0); 


    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        battleManager = GameObject.Find("BattleManager");
        old = sr.color;
    }

    void Update()
    {
        if (createMode)
        {
            if (Input.GetKeyDown(key))
            {
                Instantiate(generalNote, transform.position, Quaternion.identity);
            }
        }

        else
        {

            if (Input.GetKeyDown(key))
                StartCoroutine(Pressed());

            if (Input.GetKeyDown(key) && active)
            {
                Destroy(note);
                var fire = Instantiate(hitFire, transform.position + new Vector3(offset.x, offset.y, 0), Quaternion.identity);
                Destroy(fire, 1);
                battleManager.GetComponent<BattleManager>().AddStreak();
                AddScore();
                active = false;
            }

            else if (Input.GetKeyDown(key) && !active)
            {
                battleManager.GetComponent<BattleManager>().ResetStrek();
            }

        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "WinNote")
            battleManager.GetComponent<BattleManager>().Win();

        if (col.gameObject.tag == "Note")
        {
            note = col.gameObject;
            active = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        active = false;
        //battleManager.GetComponent<BattleManager>().ResetStrek();
        //Debug.Log("Exited trigger");
    }

    void AddScore()
    {
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + battleManager.GetComponent<BattleManager>().GetScore());
    }

    IEnumerator Pressed()
    {
        sr.color = new Color(0, 0, 0);
        yield return new WaitForSeconds(0.05f);
        sr.color = old;
    }
}
