using UnityEngine;
using System.Collections;

public class Player : Actor {

	public float moveX;
	public float moveY;
	private Spawner sp;
	public Spawner Sp
	{
		get
		{
			if(sp == null)
			{
				sp = GameObject.FindObjectOfType<Spawner>();
				if(sp == null)
				{
					Debug.LogError("Spawner is not found! Make sure you have one in the scene!");
				}
			}
			return sp;
		}
		set { sp = value; }
	}

	public void Move()
	{
		moveX = 0;
		moveY = 0;
		if(Input.GetKey(KeyCode.A) && transform.position.x > LeftBorder.position.x + transform.localScale.x / 2) moveX = -1;
		if(Input.GetKey(KeyCode.D) && transform.position.x < RightBorder.position.x - transform.localScale.x / 2) moveX = 1;
		if(Input.GetKey(KeyCode.S) && transform.position.y > BottomBorder.position.y + transform.localScale.y / 2) moveY = -1;
		if(Input.GetKey(KeyCode.W) && transform.position.y < TopBorder.position.y) moveY = 1;
		rigidbody2D.velocity = new Vector2(moveX, moveY).normalized * moveSpeed * Time.deltaTime;
	}

	public override void Die ()
	{
		Sp.PlayerDied();
		base.Die ();
	}

	// Use this for initialization
	void Start () {
		base.Start();
		moveSpeed = 500;
		energy = 10;
	}
	
	// Update is called once per frame
	void Update () {
		Move();
		if(Input.GetKeyDown(KeyCode.K)) Die ();
		if(Input.GetKeyDown(KeyCode.Space)) Shoot();
	}
}
