using UnityEngine;
using System.Collections;

public class BFL : Weapon {

    private bool activated = false;
    private float cooldownTime = 2f;
    private GameObject beam;

    void Awake()
    {
        ammo = AmmoType.Laser;
        activeSlot = SlotName.Front;
    }

    public override void Start()
    {
        beam = transform.FindChild("Beam").gameObject;
        beam.SetActive(false);
        beam.tag = this.tag;
        base.Start();
    }

    public override void Shoot()
    {
        if(!activated)
        {
            if (ammoCount > 0)
            {
                base.Shoot();
                beam.tag = this.tag;
                ammoCount--;
                cooldownTime = 2f;
                activated = true;
            }
        }
    }

    void Update()
    {
        if(cooldownTime < 0)
        {
            activated = false;
            if (ammoCount == 0)
            {
                Owner.UnequipWeapon(this);
            }
        }
        else
        {
            cooldownTime -= Time.deltaTime;
        }

        beam.SetActive(activated);
    }
}
