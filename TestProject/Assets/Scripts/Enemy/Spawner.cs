using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
// Script that will spawn new enemies at random location
// Also the script will add enemy if they are not == numberOfEnemy
public class Spawner : MonoBehaviour
{
    public int numberOfEnemy = 5;
    int enemyStop = 0;

    private GameObject enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        enemyPrefab = GameObject.FindGameObjectWithTag("Enemy");
        enemyPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        int enemyExist = GameObject.FindGameObjectsWithTag("Enemy").Length;
        

        if(enemyStop <=20)
        {
            while (enemyExist < numberOfEnemy)
            {
                Instantiate(enemyPrefab).SetActive(true);
                enemyExist++;
            }
        }
        Debug.Log(enemyStop);
        
    }
}
