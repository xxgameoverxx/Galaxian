using UnityEngine;
using System.Collections;

public class Enemy : Actor {

	private float posChange;
	private float timer = 0;

	public Vector2 offset = Vector2.zero;

	#region Attributes
	private float dampingY = 10;
	public float DampingY
	{
		get
		{
			if(dampingY < 1)
			{
				dampingY = 1;
			}
			return dampingY;
		}
		set
		{
			dampingY = value;
		}
	}

	private float dampingX = 1;
	public float DampingX
	{
		get
		{
			if(dampingX < 1)
			{
				dampingX = 1;
			}
			return dampingX;
		}
		set
		{
			dampingX = value;
		}
	}

	private Spawner sp;
	public Spawner Sp
	{
		get
		{
			if(sp == null)
			{
				sp = GameObject.FindObjectOfType<Spawner>();
			}
			return sp;
		}
		set { sp = value; }
	}
	#endregion

	public override void Start ()
	{
		base.Start ();
		health = 3;
	}

	public override void Die ()
	{
		Sp.enemyList.Remove(this.gameObject);
		base.Die ();
	}

	private Vector2 PosChange(float time = default(float))
	{
		if(time == default(float))
		{
			time = timer;
		}
		float posX = Mathf.Cos(time * DampingX) * 10;
		float posY = Mathf.Sin(time * DampingY) * 20;
		return new Vector2(posX, posY);
	}

	void FixedUpdate()
	{
		timer += Time.deltaTime;
		rigidbody2D.velocity = PosChange();
	}
}
