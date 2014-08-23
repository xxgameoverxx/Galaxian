using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public int playerLife = 5;

	private string playerPrefab;
	public string PlayerPrefab
	{
		get
		{
			if(playerPrefab == null)
			{
				playerPrefab = "Prefabs/Player";
				Debug.LogWarning("Player prefab path is set to default value: Prefabs/Player by " + this);
			}
			return playerPrefab;
		}
		set { playerPrefab = value; }
	}

	public string spawnerPrefab = "Prefabs/Spawner";
	private GameObject spawnerGO
	{
		get { return Resources.Load(spawnerPrefab) as GameObject; }
	}

	void Start()
	{
		BeginLevel();
	}

	private void BeginLevel()
	{
		GameObject spawner = GameObject.Instantiate(spawnerGO, new Vector2(100, 100), Quaternion.identity) as GameObject;
		spawner.GetComponent<Spawner>().gameManager = this as GameManager;
		spawner.GetComponent<Spawner>().playerPrefab = PlayerPrefab;
	}
}
