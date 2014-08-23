using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wave
{
	public Dictionary<GameObject, Vector2> list = new Dictionary<GameObject, Vector2>();

	public void Add(GameObject e, Vector2 pos = default(Vector2))
	{
		if(pos == default(Vector2))
		{
			Debug.LogWarning("Position of " + e + " is set to (0, 0). Make sure you know what your are doing...");
			pos = Vector2.zero;
		}
		list.Add(e, pos);
	}

	public void SpawnAll(Spawner s = null)
	{
		foreach(KeyValuePair<GameObject, Vector2> pair in list)
		{
			GameObject enemy = GameObject.Instantiate(pair.Key, pair.Value, pair.Key.transform.rotation) as GameObject;
			enemy.GetComponent<Enemy>().offset = pair.Value;
			if(s != null)
			{
				s.enemyList.Add(enemy);
			}
		}
	}
}
