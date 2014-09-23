using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Actor {

	private float timer = 0;
    private bool selfDestroy = false;

	public float amplitudeY = 2;
	public float amplitudeX = 5;
	public float shootProbability = 1;
    public float selfDestroyProbability = 0;
	public Vector2 offset = Vector2.zero;

	#region Attributes
    private Player plyr;
    private Player player
    {
        get
        {
            if(plyr == null)
            {
                plyr = GameManager.FindObjectOfType<Player>();
            }
            return plyr;
        }
    }

	private float dampingY = 1f;
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
        maxHealth = health;
		GameObject healthGO = Resources.Load("Prefabs/Items/Health") as GameObject;
        GameObject LaserGO = Resources.Load("Prefabs/Items/LaserAmmo") as GameObject;
        GameObject ShotgunGO = Resources.Load("Prefabs/Items/ShotgunAmmo") as GameObject;
        inventory.Add(healthGO.GetComponent<Item>());
        inventory.Add(LaserGO.GetComponent<Item>());
        inventory.Add(ShotgunGO.GetComponent<Item>());

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
        if (!selfDestroy)
        {
            float posX = Mathf.Cos(time * DampingX) * amplitudeX;
            float posY = Mathf.Sin(time * DampingY) * amplitudeY;
            return new Vector2(posX, posY);
        }
        else
        {
            return new Vector2(0, 0);
        }
	}


	void FixedUpdate()
	{
		timer += Time.deltaTime;
        if (selfDestroy)
        {
            rigidbody2D.velocity = transform.up * moveSpeed * Time.deltaTime;
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRot = Quaternion.LookRotation(transform.forward, direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 0.1f);
        }
        else
        {
            rigidbody2D.velocity = PosChange();
        }
        if (Random.Range(0, 100) < shootProbability)
		{
			base.Shoot();
		}
        if(Random.Range(0f, 100f) < selfDestroyProbability)
        {
            selfDestroy = true;
        }
	}
}
