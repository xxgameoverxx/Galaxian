using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float speed = 10;
	public bool destroyed = false;
	public float timer = 0.01f;
    public float hitPoint = 1;

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag != gameObject.tag)
		{
			destroyed = true;
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		destroyed = true;
	}

	void FixedUpdate () {
		if(destroyed)
		{
			timer -= Time.deltaTime;
			rigidbody2D.velocity = Vector2.zero;
		}
		else
		{
			rigidbody2D.velocity = transform.up * speed;
		}

		if(timer <= 0)
		{
            Destroy(this.gameObject);
		}
	}
}
