using UnityEngine;
using System.Collections;

public class HolyAmmo : Item
{
    private GameObject holyCall;
    // Use this for initialization
    void Start()
    {
        Name = "Rocket";
        activeSlot = SlotName.Center;
        durability = 3;
        holyCall = Resources.Load("Prefabs/Weapons/HolyIntervention") as GameObject;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Actor actor = col.gameObject.GetComponent<Actor>();
        if (actor != null && !(actor is Enemy))
        {
            HolyIntervention sg = (Instantiate(holyCall, transform.position, transform.rotation) as GameObject).GetComponent<HolyIntervention>();
            sg.name = "Holy Intervention";
            actor.EquipWeapon(sg);
            base.Unequip();
        }
    }
}
