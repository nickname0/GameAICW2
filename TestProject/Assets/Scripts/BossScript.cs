using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    //Health variables
    public int startHealth;
    int currentHealth;

    //bullet variables
    public GameObject bullet;

    float fireRate;
    float nextFire;

    private enum State { PHASE1, PHASE2};
    private State state = State.PHASE1;



    private void Start()
    {
        fireRate = 1f;
        nextFire = Time.time;
        currentHealth = startHealth;
    }

    private void Update()
    {
        switch (state)
        {
            case State.PHASE1:
                CheckIfTimeToFire();

                if (currentHealth<=startHealth/2)
                {
                    state = State.PHASE2;
                }
                break;
            case State.PHASE2:

                break;
        }
    }

    //When the Boss take damage the health will decline and eventually the boss will die
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth<=0)
        {
            Destroy(gameObject);
        }
    }

    public void CheckIfTimeToFire()
    {
        if (Time.time > nextFire)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
            nextFire = Time.time + fireRate;
        }

    }

}
