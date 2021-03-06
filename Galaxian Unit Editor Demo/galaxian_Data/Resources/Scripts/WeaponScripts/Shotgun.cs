﻿using UnityEngine;
using System.Collections;

public class Shotgun : Weapon {

	private GameObject shotgunShell;
	public GameObject ShotgunShell
	{
		get
		{
			if(shotgunShell == null)
			{
				shotgunShell = Resources.Load("Prefabs/Weapons/ShotgunShell") as GameObject;
			}
			return shotgunShell;
		}
	}

    void OnCreated()
    {
        print("asdasd");
    }
	// Use this for initialization
	public override void Start () {
		ammo = AmmoType.Shotgun;
		activeSlot = SlotName.Front;
        base.Start();
    }

	public override void Shoot ()
	{
		base.Shoot ();
		if(ammoCount > 0)
		{
            ammoCount--;
			for(int i = -1; i < 2; i++)
			{
				Quaternion rot = Quaternion.Euler(new Vector3(0, 0, 15 * i + transform.rotation.eulerAngles.z));
				GameObject bullet = Instantiate(ShotgunShell, transform.position, rot) as GameObject;
				bullet.tag = owner.gameObject.tag;
			}
            if (ammoCount == 0)
			{
				owner.UnequipWeapon(this);
			}
		}
		else
		{
			owner.UnequipWeapon(this);
		}
	}

}
