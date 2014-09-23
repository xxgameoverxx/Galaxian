using UnityEngine;
using System.Collections;

public class Health : Item {

	// Use this for initialization
	void Start () {
		Name = "Health Item";
		activeSlot = SlotName.Center;
		durability = 3;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		Actor actor = col.gameObject.GetComponent<Actor>();
		if(actor != null)
		{
			actor.Heal(durability);
			base.Unequip();
		}
	}
}
