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
    public TypeInfo playerInfo;

    private TypeInfoHolder typeInfoHolder;
    private Dictionary<int, TypeInfo> typeIdDict;
    public Dictionary<int, TypeInfo> TypeIdDict
    {
        get
        {
            if (typeInfoHolder == null)
            {
                typeInfoHolder = new TypeInfoHolder();
                typeIdDict = typeInfoHolder.typeInfoDict;
            }
            return typeIdDict;
        }
    }

    private Vector2 respawnPos;
    public Vector2 RespawnPos
    {
        get
        {
            if(respawnPos == null)
            {
                respawnPos = GameObject.FindGameObjectWithTag("Respawn").transform.position;
            }
            return respawnPos;
        }
    }

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
        GameObject playerGO = Instantiate(PlayerPrefab, gameManager.RespawnPos, Quaternion.identity) as GameObject;
        if(playerGO.GetComponent<Enemy>() != null)
        {
            Destroy(playerGO.GetComponent<Enemy>());
            playerGO.AddComponent<Player>();
            playerGO.AddComponent<PlayerGUI>();
            playerGO.name = "Player";
            playerGO.tag = "Player";
        }
        Player p = playerGO.GetComponent<Player>();
		p.Sp = this;
        p.spread = playerInfo.spread;
        p.bulletSpeed = playerInfo.bulletSpeed;
        p.moveSpeed = playerInfo.moveSpeed;
        p.maxEnergy = playerInfo.maxEnergy;
        p.maxHealth = playerInfo.maxHealth;
        GameObject w = p.DefaultWeapon;
        if (playerInfo.weapon != -1)
        {
            TypeInfo newWeapon = TypeIdDict[playerInfo.weapon];
            GameObject weaponPref = Resources.Load(newWeapon.prefab) as GameObject;
            GameObject weapon = GameObject.Instantiate(weaponPref, p.transform.position, p.transform.rotation) as GameObject;
            weapon.transform.parent = p.transform;
            weapon.name = newWeapon.name;
            weapon.GetComponent<Weapon>().ammoCount = (int)playerInfo.ammoCount;
        }
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
			waves[wave].SpawnAll(true, this);
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
            if(gameOver)
            {
                Application.LoadLevel("MainMenu");
            }
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
