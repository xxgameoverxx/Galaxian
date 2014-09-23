using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Actor : MonoBehaviour {

	public float health;
    public float maxHealth;
	public float energy;
    public float maxEnergy;
	public float moveSpeed;
	public float hurtCooldown = 0.5f;
	public Enemy lastHit;
	public GameManager gameManager;
	public float engRegenSpeed = 0.001f;
	public float hurtTimer = 3;
	public GameObject defaultWeapon;

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
		get
		{
			if(activeWeapon == null)
			{
				GameObject dw = Instantiate(defaultWeapon, transform.position, transform.rotation) as GameObject;
				activeWeapon = dw.GetComponent<Weapon>();
				activeWeapon.name = "Plazma Gun";
				EquipWeapon(activeWeapon);
			}
			return activeWeapon;
		}
		set{ activeWeapon = value; }
	}

	public List<Item> inventory = new List<Item>();
	public Dictionary<Slot, Item> slotDict = new Dictionary<Slot, Item>();
	#endregion

	public void Hit()
	{
		if(hurtTimer <= 0)
		{
			hurtTimer = hurtCooldown;
			health--;
			if(health <= 0)
			{
				Die();
			}
		}
	}

    public void Heal(float amount)
    {
        health += amount;
        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }

	public virtual void Die()
	{
        //foreach(Item i in inventory)
        //{
        //    Instantiate(i.gameObject, transform.position, Quaternion.identity);
        //}
        Instantiate(inventory[Random.Range(0, inventory.Count)].gameObject, transform.position, Quaternion.identity);
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
		if(energy < 10)
		{
			energy += engRegenSpeed;
		}
	}

	public void EquipItem(Item i)
	{
		if(slots[i.activeSlot].item != null)
		{
			UnequipItem(slots[i.activeSlot].item);
		}
		slots[i.activeSlot].item = i;
		inventory.Add(i);
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
		w.gameObject.tag = this.gameObject.tag;
	}

	public void UnequipItem(Item i)
	{
		slots[i.activeSlot].item = null;
		inventory.Remove(i);
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
		defaultWeapon = (Resources.Load("Prefabs/Weapons/PlazmaGun") as GameObject);
		defaultWeapon.name = "PlazmaGun";
	}

	private void InitSlots()
	{
		slots.Add(LeftSlot.name, LeftSlot);
		slots.Add(RightSlot.name, RightSlot);
		slots.Add(FrontSlot.name, FrontSlot);
		slots.Add(CenterSlot.name, CenterSlot);
	}

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
		if(col.gameObject.tag != gameObject.tag && col.gameObject.tag != "Item")
		{
			Hit();
		}
	}

	public virtual void Update()
	{
		RegenEng();
		hurtTimer -= Time.deltaTime;
	}
}
