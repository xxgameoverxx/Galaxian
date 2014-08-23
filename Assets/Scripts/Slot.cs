using UnityEngine;
using System.Collections;

public enum SlotName
{
	Front,
	Left,
	Right,
	Center
}

public class Slot {

	public Transform transform;
	public Item item;
	public Weapon weapon;
	public SlotName name;

}
