using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour {

	private Actor player;
    private GUISkin skin;
    private Rect groupRect = new Rect(Screen.width / 2 - 300, Screen.height - 75, 600, 70);
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
            float engRatio = 200 * (player.energy / player.maxEnergy);
            float healthRatio = 200 * (player.health / player.maxHealth);
            GUI.BeginGroup(groupRect);
            GUI.Box(new Rect(0, 0, engRatio, 50), "Energy: " + ((int)(player.energy)).ToString(), skin.customStyles[1]);
            GUI.Box(new Rect(400, 0, healthRatio, 50), "Health: " + player.health.ToString(), skin.customStyles[0]);

            string weaponName = "";
            if (player.ActiveWeapon != null)
            {
                weaponName = player.ActiveWeapon.name;
            }
            GUI.Box(new Rect(225, 10, 150, 45), player.ActiveWeapon.name + "\n" + player.ActiveWeapon.ammoCount, skin.box);
            GUI.EndGroup();
        }
        else
        {
            player = GetComponent<Actor>();
        }
	}
}
