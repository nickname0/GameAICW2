using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script using to randomize the position of the object with this code.
public class RandomPos : MonoBehaviour
{
    Vector3 pos;
    void Start()
    {
        pos = new Vector3(Random.Range(-8.5f, 8.5f), 0.25f, Random.Range(-4.5f, 5));
        transform.position = pos;
    }
}
