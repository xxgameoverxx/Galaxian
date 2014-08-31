using UnityEngine;
using System.Collections;

public enum AmmoType
{
	Laser,
	Shotgun,
	Plazma,
	Bomb,
	Area
}

public class Weapon : MonoBehaviour {

	public AmmoType ammo = AmmoType.Plazma;
	public SlotName activeSlot;
	public Actor owner;
	
	public virtual void Start()
	{
		owner = transform.root.GetComponent<Actor>();
	}

	public virtual void Shoot()
	{
	}

	public void Equip()
	{
	}

	public virtual void Unequip()
	{
		Destroy(this.gameObject);
	}
}
