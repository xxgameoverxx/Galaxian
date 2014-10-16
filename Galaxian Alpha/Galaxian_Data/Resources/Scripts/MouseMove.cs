using UnityEngine;
using System.Collections;

public class MouseMove : MonoBehaviour {

    Actor actor;
    Vector3 up;
    Vector3 down;
    Vector3 left;
    Vector3 right;

    void Awake()
    {
        actor = GetComponent<Actor>();
        up = GameObject.FindGameObjectWithTag("TopBorder").transform.position;
        down = GameObject.FindGameObjectWithTag("BottomBorder").transform.position;
        left = GameObject.FindGameObjectWithTag("LeftBorder").transform.position;
        right = GameObject.FindGameObjectWithTag("RightBorder").transform.position;
    }

	void OnMouseDrag()
    {
        if(!this.enabled)
        {
            return;
        }
        Vector3 dummy = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newPos = new Vector3(dummy.x, dummy.y, 0);
        if (actor != null)
        {
            transform.position = newPos;
        }
        else if ((this.tag == "TopBorder" || this.tag == "BottomBorder") && transform.position.y <= 15 && transform.position.y >= -15)
        {
            Vector3 pos = new Vector3(0, newPos.y, 0);
            transform.position = pos;
        }
        else if ((this.tag == "LeftBorder" || this.tag == "RightBorder") && transform.position.x <= 20 && transform.position.x >= -20)
        {
            Vector3 pos = new Vector3(newPos.x, 0, 0);
            transform.position = pos;
        }
        else if (this.tag == "Waypoint")
        {
            transform.position = newPos;
        }

    }

    void Update()
    {
        up = GameObject.FindGameObjectWithTag("TopBorder").transform.position;
        down = GameObject.FindGameObjectWithTag("BottomBorder").transform.position;
        left = GameObject.FindGameObjectWithTag("LeftBorder").transform.position;
        right = GameObject.FindGameObjectWithTag("RightBorder").transform.position;
        if (transform.position.x > 20 || transform.position.x < -20
            || transform.position.y > 15 || transform.position.y < -15)
        {
            if (this.tag == "Enemy" || this.tag == "Waypoint")
            {
                Destroy(this.gameObject);
            }
            else if ((this.tag == "TopBorder" || this.tag == "BottomBorder") && transform.position.y > 15 && transform.position.y > -15f)
            {
                transform.position = new Vector3(transform.position.x, 15, transform.position.z);
            }
            else if ((this.tag == "TopBorder" || this.tag == "BottomBorder") && transform.position.y < 15 && transform.position.y < -15f)
            {
                transform.position = new Vector3(transform.position.x, -15, transform.position.z);
            }
            else if ((this.tag == "LeftBorder" || this.tag == "RightBorder") && transform.position.x > -20 && transform.position.x > 20)
            {
                transform.position = new Vector3(20, transform.position.y, transform.position.z);
            }
            else if ((this.tag == "LeftBorder" || this.tag == "RightBorder") && transform.position.x < 20 && transform.position.x < -20)
            {
                transform.position = new Vector3(-20, transform.position.y, transform.position.z);
            }
            if (actor is Player && transform.position.y > 15 && transform.position.y > -15f)
            {
                transform.position = new Vector3(transform.position.x, 15, transform.position.z);
            }
            else if (actor is Player && transform.position.y < 15 && transform.position.y < -15f)
            {
                transform.position = new Vector3(transform.position.x, -15, transform.position.z);
            }
            if (actor is Player && transform.position.x > -20 && transform.position.x > 20)
            {
                transform.position = new Vector3(20, transform.position.y, transform.position.z);
            }
            else if (actor is Player && transform.position.x < 20 && transform.position.x < -20)
            {
                transform.position = new Vector3(-20, transform.position.y, transform.position.z);
            }
        }
    }
}
