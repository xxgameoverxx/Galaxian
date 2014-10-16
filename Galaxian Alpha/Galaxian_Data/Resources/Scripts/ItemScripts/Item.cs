using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	public float durability;
	public SlotName activeSlot;
	public string Name = "item";

	public virtual void Use()
	{
		print(Name + " is used.");
	}

	public void Unequip()
	{
		Destroy(this.gameObject);
	}
}
