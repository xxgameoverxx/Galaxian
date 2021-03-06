﻿using UnityEngine;
using System.Collections;

public class PlazmaGun : Weapon {


	private GameObject plazmaBullet;
	public GameObject PlazmaBullet
	{
		get
		{
			if(plazmaBullet == null)
			{
				plazmaBullet = Resources.Load("Prefabs/Weapons/PlazmaBullet") as GameObject;
			}
			return plazmaBullet;
		}
	}

	void Awake()
	{
		activeSlot = SlotName.MainWeapon;
		ammo = AmmoType.Plazma;
	}


	public override void Shoot ()
	{
		base.Shoot ();
		if(Owner.energy >= 1)
		{
            Owner.energy--;
			GameObject bullet = Instantiate(PlazmaBullet, transform.position, transform.rotation) as GameObject;
            bullet.tag = Owner.gameObject.tag;
            bullet.GetComponent<Bullet>().speed = speed;
		}
	}

	public override void Start () {
		base.Start();
	}
}
