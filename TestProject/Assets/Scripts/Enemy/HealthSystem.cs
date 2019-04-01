using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Health system that is used for the enemy, if enemy goes below 1 health he will be destroyed

public class HealthSystem : MonoBehaviour
{
    public int startHealth;
    public int currentHealth;
    //int counter;

    private EnemyController dead;

    private void Start()
    {
        currentHealth = startHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        Debug.Log("Current Health " + currentHealth);

        if (currentHealth <=0)
        {
            Destroy(gameObject);
        }

    }

}
