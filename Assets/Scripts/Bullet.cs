﻿using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float speed = 10;
	private bool destroyed = false;
	private float timer = 0.01f;
	// Use this for initialization
	void Start () {
	}

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

	// Update is called once per frame
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