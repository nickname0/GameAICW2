using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that will be used when a bullet leaves the field and it is not destroyed by a wall or enemy
//Used to be sure that the bullet will be destroyed after they leave the perimeter.
public class DestroyingObject : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }
}
