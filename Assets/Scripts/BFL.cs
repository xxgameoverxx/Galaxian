using UnityEngine;
using System.Collections;

public class BFL : Weapon {

    private bool active = false;
    private float cooldownTime = 2f;
    private GameObject beam;

    public int beamCount = 5;

    void Awake()
    {
        ammo = AmmoType.Laser;
        activeSlot = SlotName.Front;
    }

    public override void Start()
    {
        base.Start();
        beam = transform.FindChild("Beam").gameObject;
        beam.SetActive(false);
    }

    public override void Shoot()
    {
        if(!active)
        {
            if (beamCount > 0)
            {
                beamCount--;
                cooldownTime = 2f;
                active = true;
            }
        }
    }

    void Update()
    {
        if(cooldownTime < 0)
        {
            active = false;
            if(beamCount == 0)
            {
                owner.UnequipWeapon(this);
            }
        }
        else
        {
            cooldownTime -= Time.deltaTime;
        }

        beam.SetActive(active);
    }
}
