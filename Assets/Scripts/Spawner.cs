using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

	public GameManager gameManager;
	public List<Wave> waves = new List<Wave>();
	public List<GameObject> enemyList = new List<GameObject>();

	public int waveNumber = 0;

	public bool showDescription = false;
	private bool gameOver = false;
	public string currentWaveDescription;
	public string currentLevelDescription;

	private GameObject player;

	public string playerPrefab;
	private GameObject PlayerPrefab
	{
		get
		{
			if(playerPrefab == null)
			{
				playerPrefab = "Prefabs/Player";
				Debug.LogWarning("Player prefab path is set to default value: Prefabs/Player by " + this);
			}
			return Resources.Load(playerPrefab) as GameObject;
		}
	}

	void OnGUI()
	{
		if(showDescription)
		{
			GUI.Box(new Rect(20, 20, 300, 300), waves[waveNumber].description);
			if(GUI.Button(new Rect(20, 340, 50, 25), "OK"))
			{
				showDescription = false;
				player.SetActive(true);
				Time.timeScale = 1;
				waves[waveNumber].description = "";
			}
		}
	}

	void Start () {

//		GameObject e = Resources.Load("Prefabs/Enemies/Enemy") as GameObject;
//
//		Wave w = new Wave();
//		w.Add(e, new Vector2(0, 5));
//		Wave w2 = new Wave();
//		w2.Add(e, new Vector2(5, 5));
//
//		waves.Add(w);
//		waves.Add(w2);

		if(gameManager == null)
		{
			Debug.LogWarning("Game Manager is not found!");
		}
		player = PlayerStart();
		currentLevelDescription = gameManager.levelDescription;
	}

	public GameObject PlayerStart()
	{
		GameObject playerGO = Instantiate(PlayerPrefab, new Vector2(0, -5), Quaternion.identity) as GameObject;
		playerGO.GetComponent<Player>().Sp = this;
		return playerGO;
	}

	public void SpawnWave(int wave = -1)
	{
		if(waves.Count == 0)
		{
			print("Loading level...");
			return;
		}
		if(wave == -1)
		{
			wave = waveNumber;
		}
		if(waves.Count > waveNumber)
		{
			if(waves[wave].description != "")
			{
				currentLevelDescription = waves[wave].description;
				player.SetActive(false);
				showDescription = true;
				Time.timeScale = 0;
			}

			if(showDescription)
			{
				return;
			}

			waves[wave].SpawnAll(this);
			waveNumber++;
		}
		else
		{
			gameOver = true;
			print("You won :(");
		}

	}

	public void PlayerDied()
	{
		if(gameManager.playerLife > 0)
		{
			player = PlayerStart();
			gameManager.playerLife--;
		}
		else
		{
			print("Sayonara sucka!");
		}
	}
	
	void Update () {
		if(enemyList.Count == 0 && !gameOver)
		{
			SpawnWave();
		}
	}
}
