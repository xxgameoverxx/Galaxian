using UnityEngine;
using System.Collections;

public class RocketAmmo : Item
{
    private GameObject rocketLauncher;
	// Use this for initialization
	void Start () {
        Name = "Rocket";
        activeSlot = SlotName.Front;
        durability = 3;
        rocketLauncher = Resources.Load("Prefabs/Weapons/RocketLauncher") as GameObject;
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        Actor actor = col.gameObject.GetComponent<Actor>();
        if (actor != null)
        {
            RocketLauncher sg = (Instantiate(rocketLauncher, transform.position, transform.rotation) as GameObject).GetComponent<RocketLauncher>();
            sg.name = "Rocket Launcher";
            actor.EquipWeapon(sg);
            base.Unequip();
        }
    }
}
