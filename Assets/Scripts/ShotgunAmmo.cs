using UnityEngine;
using System.Collections;

public class ShotgunAmmo : Item {

	private GameObject shotGun;

	// Use this for initialization
	void Start () {
		name = "Shotgun Ammo";
		activeSlot = SlotName.Front;
		durability = 5;
		shotGun = Resources.Load("Prefabs/Weapons/Shotgun") as GameObject;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		Actor actor = col.gameObject.GetComponent<Actor>();
		if(actor != null)
		{
			Shotgun sg = (Instantiate(shotGun, transform.position, transform.rotation) as GameObject).GetComponent<Shotgun>();
			sg.name = "Shotgun"; 
			actor.EquipWeapon(sg);
			base.Unequip();
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
