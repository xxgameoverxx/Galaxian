using UnityEngine;
using System.Collections;

public class RocketLauncher : Weapon {


    private GameObject rocket;
    public GameObject Rocket
    {
        get
        {
            if(rocket == null)
            {
                rocket = Resources.Load("Prefabs/Weapons/Rocket") as GameObject;
            }
            return rocket;
        }
    }

	public override void Start () {
        ammo = AmmoType.Bomb;
        activeSlot = SlotName.Front;
        base.Start();
    }

    public override void Shoot()
    {
        base.Shoot();
        if (ammoCount > 0)
        {
            ammoCount--;
            GameObject bullet = Instantiate(Rocket, transform.position, transform.rotation) as GameObject;
            bullet.tag = Owner.gameObject.tag;
            bullet.GetComponent<Bullet>().speed = speed;
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
