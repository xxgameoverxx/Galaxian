using UnityEngine;
using System.Collections;

public class MouseMove : MonoBehaviour {

	void OnMouseDrag()
    {
        Vector3 dummy = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newPos = new Vector3(dummy.x, dummy.y, 0);
        transform.position = newPos;
    }
}
