using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour {

	private Actor player;

	void Start()
	{
		player = GetComponent<Actor>();
		if(player == null)
		{
			Debug.LogWarning("PlayerGUI is not attached to the player or Player GameObject does not have Player component.");
		}
	}

	void OnGUI()
	{
        float engRatio = Screen.width / 3f * (player.energy / player.maxEnergy);
        float healthRatio = Screen.width / 3f * (player.health / player.maxHealth);
        GUI.Box(new Rect(0, Screen.height / 15 * 14, engRatio, Screen.height / 15), "Energy: " + ((int)(player.energy)).ToString());
        GUI.Box(new Rect(Screen.width / 3 * 2, Screen.height / 15 * 14, healthRatio, Screen.height / 15), "Health: " + player.health.ToString());

		string weaponName = "";
		if(player.ActiveWeapon != null)
		{
			weaponName = player.ActiveWeapon.name;
		}
        GUI.Box(new Rect(Screen.width / 3, Screen.height / 10 * 9, Screen.width / 3, Screen.height / 10), player.ActiveWeapon.name);
	}
}
