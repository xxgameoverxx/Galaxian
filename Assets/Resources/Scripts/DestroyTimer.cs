using UnityEngine;
using System.Collections;

public class DestroyTimer : MonoBehaviour {

	private float timer = 0;
	public float destroyAfter = 1;
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if(timer > destroyAfter)
		{
			Destroy(this.gameObject);
		}
	}
}
