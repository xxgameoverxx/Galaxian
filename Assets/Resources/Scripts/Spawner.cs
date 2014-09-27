using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

	public GameManager gameManager;
	public List<Wave> waves = new List<Wave>();
	public List<GameObject> enemyList = new List<GameObject>();
    private float waitBeforeSpawn = 3;
    private GUIHelper guiHelper;

	public int waveNumber = 0;

	private bool gameOver = false;
    private bool showingMessage = false;
	public string currentWaveDescription;
	public string currentLevelDescription;

	private GameObject player;

	public string playerPrefab;
    private bool showDescription = false;
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

	void Start ()
    {
		if(gameManager == null)
		{
			Debug.LogWarning("Game Manager is not found!");
		}
		player = PlayerStart();
		currentLevelDescription = gameManager.levelDescription;
        guiHelper = FindObjectOfType<GUIHelper>();
        if(guiHelper == null)
        {
            Debug.LogWarning("GUIHelper is not found by Spawner");
        }
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
            if (!string.IsNullOrEmpty(waves[wave].description))
            {
                guiHelper.ShowMessage(waves[wave].description);
                showingMessage = true;
            }
			waves[wave].SpawnAll(this);
			waveNumber++;
		}
		else
		{
			gameOver = true;
            showingMessage = true;
            guiHelper.ShowMessage(gameManager.winText);
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
            guiHelper.ShowMessage("GAME OVER!!!");
		}
	}
	
	void Update () {
		if(enemyList.Count == 0 && !gameOver)
		{
            waitBeforeSpawn -= Time.deltaTime;
            if (waitBeforeSpawn <= 0 && !showingMessage)
            {
                waitBeforeSpawn = 3;
                SpawnWave();
            }
		}

        if(showingMessage && player != null)
        {
            Time.timeScale = 0;
            ActivateActors(false);
        }
        else if(player != null)
        {
            Time.timeScale = 1;
            ActivateActors(true);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            showingMessage = false;
            guiHelper.HideMessage();
        }
	}

    void ActivateActors(bool b)
    {
        player.SetActive(b);
        foreach(GameObject e in enemyList)
        {
            e.gameObject.SetActive(b);
        }
    }
}
