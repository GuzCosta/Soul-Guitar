using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    private float pathSpeed;
    bool called = false;

    private Collider2D col;

    private NotesPathAnimation path;

    private float t;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<PolygonCollider2D>();
        t = 0;
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("Start") == 1 && !called)
        {
            rb.velocity = new Vector2(0, -speed);
            called = true;
        }

        if (path != null) //We found the path
        {
            Vector2 pos = path.GetPositionOnPath(t);
            transform.position = new Vector3(pos.x, pos.y, transform.position.z);

            t += (pathSpeed) * Time.deltaTime;
            if (t <= 1)
            {
                transform.localScale = new Vector3(0.5f + (t / 2), 0.5f + (t / 2), 1);
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (path != null) return;
        //Debug.Log(gameObject.name + " jut hit: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Path"))
        {
            path = other.gameObject.GetComponent<NotesPathAnimation>();
            pathSpeed = path.pathSpeed;
        }
    }





}
