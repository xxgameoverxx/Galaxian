using UnityEngine;
using System.Collections;

public class HolyIntervention : Weapon
{
	// Use this for initialization
	void Start () {
        ammo = AmmoType.Area;
        activeSlot = SlotName.Center;
        base.Start();
    }

    public override void Shoot()
    {
        base.Shoot();
        foreach(Enemy e in GameObject.FindObjectsOfType<Enemy>())
        {
            e.Hit(3);
            Debug.Log(e + " is hit");
        }
        owner.UnequipWeapon(this);
    }
}
