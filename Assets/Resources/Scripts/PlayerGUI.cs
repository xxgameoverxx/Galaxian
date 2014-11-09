using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour {

	private Actor player;
    private GUISkin skin;

	void Start()
	{
		player = GetComponent<Actor>();
		if(player == null)
		{
			Debug.LogWarning("PlayerGUI is not attached to the player or Player GameObject does not have Player component.");
		}
        if (GameObject.FindObjectOfType<Style>() != null)
        {
            skin = GameObject.FindObjectOfType<Style>().skin;
        }
        else
        {
            Debug.LogError("Style object is not found!");
            skin = new GUISkin();
        }
	}

	void OnGUI()
	{
        if (player != null)
        {
            float engRatio = Screen.width / 3f * (player.energy / player.maxEnergy);
            float healthRatio = Screen.width / 3f * (player.health / player.maxHealth);
            GUI.Box(new Rect(0, Screen.height / 15 * 14, engRatio, Screen.height / 15), "Energy: " + ((int)(player.energy)).ToString(), skin.customStyles[1]);
            GUI.Box(new Rect(Screen.width / 3 * 2, Screen.height / 15 * 14, healthRatio, Screen.height / 15), "Health: " + player.health.ToString(), skin.customStyles[0]);

            string weaponName = "";
            if (player.ActiveWeapon != null)
            {
                weaponName = player.ActiveWeapon.name;
            }
            GUI.Box(new Rect(Screen.width / 3, Screen.height / 10 * 9, Screen.width / 3, Screen.height / 10), player.ActiveWeapon.name + "\n" + player.ActiveWeapon.ammoCount, skin.box);
        }
        else
        {
            player = GetComponent<Actor>();
        }
	}
}
