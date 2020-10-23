using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttachPin : MonoBehaviour
{
    public float attachOffset;
    public float speed = 5f;
    public LineRenderer line;
    public Text countText;
    [HideInInspector]
    public bool CanMove = false;
    
    public void SetText(int id)
    {
        countText.text = id.ToString();
    }

    void Update()
    {
        if(CanMove)
            MovePin();
    }

    void MovePin()
    {
        if(transform.position.y < attachOffset)
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        else
        {
            Transform mainCircle = GameObject.Find("MainCircle").transform;

            line.SetPosition(1, new Vector2(0, 15));

            transform.parent = mainCircle;

            if(countText.text == "1")
            {
                FindObjectOfType<GameManager>().NextLevel();
            }

            this.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "AttachedCircle")
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }
}
