using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Script used to move the bullet around, if it hits enemy/wall/player the bullet will disappear
//Also if an enemy is hit the enemy will take damage
public class BulletMover : MonoBehaviour
{
    Rigidbody rb;
    //Bullet variables
    public float speed;
    public int bulletDmg;

    void Start()
    {

        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<HealthSystem>().TakeDamage(bulletDmg);
            Destroy(gameObject);
            //Debug.Log("HitTTTTTT");
        }
        if (other.tag == "Boss")
        {
            other.GetComponent<BossScript>().TakeDamage(bulletDmg);
            Destroy(gameObject);
            //Debug.Log("HitTTTTTT");
        }
        if(other.tag == "Player" || other.tag =="Wall")
        {
            Destroy(gameObject);
            Debug.Log("Wall/Player");
        }
    }
}
