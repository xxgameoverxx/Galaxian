using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour {

	private Player player;

	void Start()
	{
		player = GetComponent<Player>();
		if(player == null)
		{
			Debug.LogWarning("PlayerGUI is not attached to the player or Player GameObject does not have Player component.");
		}
	}

	void OnGUI()
	{
		GUI.Box(new Rect(20, Screen.height - 20, Screen.width / 20, -Screen.height / 25 * player.energy), "E");
		GUI.Box(new Rect(Screen.width - Screen.width / 20 - 20, Screen.height - 20, Screen.width / 20, -Screen.height / 25 * player.health), "H");

		string weaponName = "";
		if(player.ActiveWeapon != null)
		{
			weaponName = player.ActiveWeapon.name;
		}
		GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height - 40, 200, 30), player.ActiveWeapon.name);
	}
}
