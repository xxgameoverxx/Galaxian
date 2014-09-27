using UnityEngine;
using System.Collections;

public class Rocket : Bullet
{
    private GameObject explosion;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        if (destroyed)
        {
            timer -= Time.deltaTime;
            rigidbody2D.velocity = Vector2.zero;
        }
        else
        {
            rigidbody2D.velocity = transform.up * speed;
        }

        if (timer <= 0)
        {
            explosion = Resources.Load("Prefabs/Effects/Explosion") as GameObject;
            GameObject ex = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
            ex.transform.localScale *= 3;
            Destroy(this.gameObject);
        }
	}
}
