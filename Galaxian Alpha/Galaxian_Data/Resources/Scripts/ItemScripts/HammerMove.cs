using UnityEngine;
using System.Collections;

public class HammerMove : MonoBehaviour {

    Transform target;
    Vector3 previousPos;

	// Use this for initialization
	void Start () {
        target = transform.parent;
        previousPos = target.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.RotateAround(target.position, -Vector3.forward, 180 * Time.deltaTime);
        previousPos = target.position;
	}
}
