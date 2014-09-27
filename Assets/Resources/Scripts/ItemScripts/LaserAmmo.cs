using UnityEngine;
using System.Collections;

public class LaserAmmo : Item
{
    private GameObject laser;
    void Start()
    {
        Name = "Laser Ammo";
        activeSlot = SlotName.Front;
        durability = 5;
        laser = Resources.Load("Prefabs/Weapons/BFL") as GameObject;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Actor actor = col.gameObject.GetComponent<Actor>();
        if (actor != null)
        {
            BFL bfl = (Instantiate(laser, transform.position, transform.rotation) as GameObject).GetComponent<BFL>();
            bfl.name = "BFL";
            bfl.ammoCount = (int)durability;
            actor.EquipWeapon(bfl);
            base.Unequip();
        }
    }
}
