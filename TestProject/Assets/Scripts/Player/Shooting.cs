using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scipt used for shooting 
public class Shooting : MonoBehaviour
{ 
    //Shots Variables
    public GameObject shot;
    public Transform shotSpawn; //shotspawn.transform.position

    public float fireRate;
    private float nextFire;

    // Update is called once per frame
    void FixedUpdate()
    {
        //When Left Mouse Button is clicked the player will shoot bullet
        if(Input.GetMouseButton(0) && Time.time>nextFire)
        {
            nextFire = Time.time + fireRate;
            //GameObject clone..
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation); // as GameObject
        }
    }
}
