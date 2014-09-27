using UnityEngine;
using System.Collections;

public class BFL : Weapon {

    private bool activated = false;
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
        beam = transform.FindChild("Beam").gameObject;
        beam.SetActive(false);
        beam.tag = this.tag;
        base.Start();
    }

    public override void Shoot()
    {
        if(!activated)
        {
            if (beamCount > 0)
            {
                beamCount--;
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
            if(beamCount == 0)
            {
                owner.UnequipWeapon(this);
            }
        }
        else
        {
            cooldownTime -= Time.deltaTime;
        }

        beam.SetActive(activated);
    }
}
