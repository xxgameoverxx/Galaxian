using UnityEngine;
using System.Collections;

public class HolyIntervention : Weapon
{
    Color bgColor;
    Renderer bgRenderer;
    Background bg;
    Background BG
    {
        get
        {
            if(bg == null)
            {
                bg = GameObject.FindGameObjectWithTag("Background").GetComponent<Background>();
            }
            return bg;
        }
    }

	public override void Start () {
        ammo = AmmoType.Area;
        activeSlot = SlotName.Center;
        base.Start();
        bgColor = GameObject.FindGameObjectWithTag("Background").gameObject.renderer.material.color;
        bgRenderer = GameObject.FindGameObjectWithTag("Background").gameObject.renderer;
        bg = GameObject.FindGameObjectWithTag("Background").GetComponent<Background>();
    }

    public override void Shoot()
    {
        base.Shoot();
        //bgRenderer.material.color = new Color(255, 255, 255);
        BG.Flash();
        foreach(Enemy e in GameObject.FindObjectsOfType<Enemy>())
        {
            e.Hit(3);
        }
        if (ammoCount <= 0)
        {
            Owner.UnequipWeapon(this);
        }
    }
}
