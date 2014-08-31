using UnityEngine;
using System.Collections;

public class Enemy : Actor {

	private float posChange;
	private float timer = 0;

	public float amplitudeY = 2;
	public float amplitudeX = 5;
	public float shootProbability = 1;
	public Vector2 offset = Vector2.zero;

	#region Attributes
	private float dampingY = 1;
	public float DampingY
	{
		get
		{
			if(dampingY < 0)
			{
				dampingY = 0;
			}
			return dampingY;
		}
		set
		{
			dampingY = value;
		}
	}

	private float dampingX = 1f;
	public float DampingX
	{
		get
		{
			if(dampingX < 0)
			{
				dampingX = 0;
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
		GameObject healthGO = Resources.Load("Prefabs/Items/ShotgunAmmo") as GameObject;
		inventory.Add(healthGO.GetComponent<Item>());
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
		float posX = Mathf.Cos(time * DampingX) * amplitudeX;
		float posY = Mathf.Sin(time * DampingY) * amplitudeY;
		return new Vector2(posX, posY);
	}

	void FixedUpdate()
	{
		timer += Time.deltaTime;
		rigidbody2D.velocity = PosChange();
		if(Random.Range(0, 100) < shootProbability)
		{
			base.Shoot();
		}
	}
}
