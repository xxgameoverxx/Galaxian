using UnityEngine;
using System.Collections;

public enum SlotName
{
	Front = 0,
	Left = 1,
	Right = 2,
	Center = 3
}

public class Slot {

	public Transform transform;
	public Item item;
	public Weapon weapon;
	public SlotName name;

}
