using UnityEngine;
using System.Collections;

public class RocketLauncher : Weapon {

    public int rocketCount = 3;

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

	void Start () {
        ammo = AmmoType.Bomb;
        activeSlot = SlotName.Front;
        base.Start();
    }

    public override void Shoot()
    {
        base.Shoot();
        if(rocketCount > 0)
        {
            rocketCount--;
            GameObject bullet = Instantiate(Rocket, transform.position, transform.rotation) as GameObject;
            bullet.tag = owner.gameObject.tag;
            if (rocketCount == 0)
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
