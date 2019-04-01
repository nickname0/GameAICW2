using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This script will transfer the player to the next level, also the position of the object will be completly random.
public class LevelManagment : MonoBehaviour
{
    Vector3 pos;
    public int index;
    //public string LevelName;

    private void Start()
    {
        pos = new Vector3(Random.Range(-8.5f, 8.5f), 0.25f,Random.Range(-4.5f, 5));
        transform.position = pos;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //Loading level with build index
            SceneManager.LoadScene(index);

            //Loading level with a scene name
            //SceneManager.LoadScene(LevelName);
        }
    }
}
