using UnityEngine;
using System.Collections;

public class MouseMove : MonoBehaviour {

    Actor actor;

    void Awake()
    {
        actor = GetComponent<Actor>();
    }

	void OnMouseDrag()
    {
        Vector3 dummy = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newPos = new Vector3(dummy.x, dummy.y, 0);
        transform.position = newPos;
    }

    void Update()
    {
        if (transform.position.x > 20 || transform.position.x < -20
            || transform.position.y > 15 || transform.position.y < -15)
        {
            Destroy(this.gameObject);
        }
    }
}
