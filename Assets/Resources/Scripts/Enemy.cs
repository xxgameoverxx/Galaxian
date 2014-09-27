﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Actor {

	private float timer = 0;
    private bool selfDestroy = false;
    public bool moveToWaypoint = false;

	public float amplitudeY = 0;
	public float amplitudeX = 0;
	public float shootProbability = 1;
    public float selfDestroyProbability = 0;
    public float dropProbability = 1;
    private Vector3 movePos = default(Vector3);

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

	public float dampingY = 1f;
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

	public float dampingX = 1f;
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

	public override void Die ()
	{
		Sp.enemyList.Remove(this.gameObject);
        if (inventory.Count > 0 && dropProbability > Random.Range(0, 100))
        {
            Instantiate(inventory[Random.Range(0, inventory.Count)].gameObject, transform.position, Quaternion.identity);
        }
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

    void Move()
    {
        if (selfDestroy)
        {
            rigidbody2D.velocity = transform.up * moveSpeed * Time.deltaTime;
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRot = Quaternion.LookRotation(transform.forward, direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 0.1f);
            return;
        }
        else
        {
            rigidbody2D.velocity = PosChange();
        }

        if (!moveToWaypoint || gameManager.waypoints.Count == 0)
            return;

        if(movePos == default(Vector3) || Vector3.Distance(transform.position, movePos) < 1f)
        {
            if (movePos != default(Vector3))
            {
                gameManager.waypoints.Add(movePos);
            }
            int index = Random.Range(0, gameManager.waypoints.Count);
            movePos = gameManager.waypoints[index];
            gameManager.waypoints.Remove(movePos);
        }
        else
        {
            transform.position = Vector3.Slerp(transform.position, movePos, Time.deltaTime * moveSpeed / 500);
        }
    }

    void Shoot()
    {
        if (Random.Range(0, 100) < shootProbability)
        {
            base.Shoot();
        }
        if (Random.Range(0f, 100f) < selfDestroyProbability)
        {
            selfDestroy = true;
        }
    }

	void FixedUpdate()
	{
		timer += Time.deltaTime;
        Move();
        Shoot();
        if(!selfDestroy && selfDestroyProbability > Random.Range(0, 1000))
        {
            selfDestroy = true;
        }
	}
}