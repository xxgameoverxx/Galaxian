using UnityEngine;
using System.Collections;

public class Shield : Item
{
    void Start()
    {
        Name = "Shield Item";
        activeSlot = SlotName.Center;
        durability = 10;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Actor actor = col.gameObject.GetComponent<Actor>();
        if (actor != null)
        {
            actor.hurtTimer = durability;
            base.Unequip();
        }
        if (col.tag == "Net")
        {
            Destroy(this.gameObject);
        }
    }
}
