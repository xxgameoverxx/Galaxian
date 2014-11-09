using UnityEngine;
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
            for (int i = -spread; i < spread + 1; i++)
			{
				Quaternion rot = Quaternion.Euler(new Vector3(0, 0, 15 * i + transform.rotation.eulerAngles.z));
				GameObject bullet = Instantiate(ShotgunShell, transform.position, rot) as GameObject;
                bullet.tag = Owner.gameObject.tag;
                bullet.GetComponent<Bullet>().speed = speed;
            }
            if (ammoCount == 0)
			{
                Owner.UnequipWeapon(this);
			}
		}
		else
		{
            Owner.UnequipWeapon(this);
		}
	}

}
