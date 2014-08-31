using UnityEngine;
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
		activeSlot = SlotName.Front;
		ammo = AmmoType.Plazma;
	}


	public override void Shoot ()
	{
		base.Shoot ();
		if(owner.energy >= 1)
		{
			owner.energy--;
			GameObject bullet = Instantiate(PlazmaBullet, transform.position, transform.rotation) as GameObject;
			bullet.tag = owner.gameObject.tag;
		}
	}

	public override void Start () {
		base.Start();
	}
}
