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
	private Actor owner;
    public string Name;
    public int ammoCount = 0;
    public int spread = 2;
    public float speed = 10;
    public AudioSource audio;
    public AudioClip effect;
    private Vector3 cam;

    public Actor Owner
    {
        get
        {
            if (owner == null)
            {
                owner = transform.root.GetComponent<Actor>();
            }
            return owner;
        }
        set
        {
            owner = value;
        }
    }

	public virtual void Start()
	{
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
        audio = GetComponent<AudioSource>();
        audio.clip = effect;
		owner = transform.root.GetComponent<Actor>();
	}

    public virtual void Shoot()
	{
        //audio.PlayOneShot(effect);
        AudioSource.PlayClipAtPoint(effect, cam);
	}

	public void Equip()
	{

	}

    public void SetSpread(int _spread = 1)
    {
        spread = _spread;
    }

    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }


	public virtual void Unequip()
	{
		Destroy(this.gameObject);
	}
}
