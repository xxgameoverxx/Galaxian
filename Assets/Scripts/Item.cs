using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	public float durability;
	public SlotName activeSlot;
	public string name = "item";

	public void Use()
	{
		print(name + " is used.");
	}

	public void Unequip()
	{
		Destroy(this.gameObject);
	}
}
