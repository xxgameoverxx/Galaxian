using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Actor : MonoBehaviour {

	public float health;
	public float energy;
	public float moveSpeed;
	public float hurtCooldown = 0.5f;
	public Enemy lastHit;
	public GameManager gameManager;

	#region Attributes and Slots
	private GameObject explosion;
	private GameObject Explosion
	{
		get
		{
			if(explosion == null)
			{
				explosion = Resources.Load("Prefabs/Effects/Explosion") as GameObject;
			}
			return explosion;
		}
		set
		{}
	}

	private Transform leftBorder;
	public Transform LeftBorder
	{
		get
		{
			if(leftBorder == null)
			{
				leftBorder = GameObject.FindGameObjectWithTag("LeftBorder").transform;
			}
			return leftBorder;
		}
	}
	private Transform rightBorder;
	public Transform RightBorder
	{
		get
		{
			if(rightBorder == null)
			{
				rightBorder = GameObject.FindGameObjectWithTag("RightBorder").transform;
			}
			return rightBorder;
		}
	}
	private Transform topBorder;
	public Transform TopBorder
	{
		get
		{
			if(topBorder == null)
			{
				topBorder = GameObject.FindGameObjectWithTag("TopBorder").transform;
			}
			return topBorder;
		}
	}
	private Transform bottomBorder;
	public Transform BottomBorder
	{
		get
		{
			if(bottomBorder == null)
			{
				bottomBorder = GameObject.FindGameObjectWithTag("BottomBorder").transform;
			}
			return bottomBorder;
		}
	}

	private Slot leftSlot = new Slot();
	public Slot LeftSlot
	{
		get
		{
			if(leftSlot.transform == null)
			{
				leftSlot.transform = transform.FindChild("Slot_Left").transform;
				leftSlot.name = SlotName.Left;
			}
			return leftSlot;
		}
	}
	private Slot rightSlot = new Slot();
	public Slot RightSlot
	{
		get
		{
			if(rightSlot.transform == null)
			{
				rightSlot.transform = transform.FindChild("Slot_Right").transform;
				rightSlot.name = SlotName.Right;
			}
			return rightSlot;
		}
	}

	private Slot frontSlot = new Slot();
	public Slot FrontSlot
	{
		get
		{
			if(frontSlot.transform == null)
			{
				frontSlot.transform = transform.FindChild("Slot_Front").transform;
				frontSlot.name = SlotName.Front;
			}
			return frontSlot;
		}
	}

	private Slot centerSlot = new Slot();
	public Slot CenterSlot
	{
		get
		{
			if(centerSlot.transform == null)
			{
				centerSlot.transform = transform.FindChild("Slot_Center").transform;
				centerSlot.name = SlotName.Center;
			}
			return centerSlot;
		}
	}

	private Dictionary<SlotName, Slot> slots = new Dictionary<SlotName, Slot>();


	private Weapon activeWeapon;
	public Weapon ActiveWeapon
	{
		get{ return activeWeapon; }
		set{ activeWeapon = value; }
	}

	public List<Item> items = new List<Item>();
	public Dictionary<Slot, Item> slotDict = new Dictionary<Slot, Item>();
	#endregion

	public void Hit()
	{
		health--;
		if(health <= 0)
		{
			Die();
		}
	}

	public virtual void Die()
	{
		Instantiate(Explosion, transform.position, transform.rotation);
		Destroy(this.gameObject);
	}

	public void Shoot(Weapon w = null)
	{
		if(w == null)
		{
			w = ActiveWeapon;
		}
		w.Shoot();
	}

	public void RegenEng()
	{

	}

	public void EquipItem(Item i)
	{
		if(slots[i.activeSlot].item != null)
		{
			UnequipItem(slots[i.activeSlot].item);
		}
		slots[i.activeSlot].item = i;
	}

	public void EquipWeapon(Weapon w)
	{
		if(slots[w.activeSlot].weapon != null)
		{
			UnequipWeapon(slots[w.activeSlot].weapon);
		}
		slots[w.activeSlot].weapon = w;
		w.transform.position = slots[w.activeSlot].transform.position;
		w.transform.rotation = slots[w.activeSlot].transform.rotation;
		w.transform.parent = slots[w.activeSlot].transform;
		w.owner = transform.GetComponent<Actor>();
		ActiveWeapon = w;
	}

	public void UnequipItem(Item i)
	{
		slots[i.activeSlot].item = null;
		i.Unequip();
	}

	public void UnequipWeapon(Weapon w)
	{
		slots[w.activeSlot].item = null;
		w.Unequip();
	}

	void Awake()
	{
		InitSlots();
	}

	private void InitSlots()
	{
		slots.Add(LeftSlot.name, LeftSlot);
		slots.Add(RightSlot.name, RightSlot);
		slots.Add(FrontSlot.name, FrontSlot);
		slots.Add(CenterSlot.name, CenterSlot);
	}
	// Use this for initialization
	public virtual void Start ()
	{
		foreach(Weapon w in transform.GetComponentsInChildren<Weapon>())
		{
			EquipWeapon(w);
		}
		gameManager = GameObject.FindObjectOfType<GameManager>();
		if(gameManager == null)
		{
			Debug.LogError("GameManager could not be found by " + this + " ! Be sure that you have it in the scene!");
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag != gameObject.tag)
		{
			Hit();
		}
	}
}
