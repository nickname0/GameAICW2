using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBoss : MonoBehaviour
{
    public float moveSpeed;

    Rigidbody rb;

    public GameObject target;
    Vector3 moveDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player");
        moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
        rb.velocity = new Vector3(moveDirection.x,0.2f ,moveDirection.z);
        Destroy(gameObject, 6f);
    }
    
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name.Equals("Player"))
        {
            Debug.Log("Hit!");
            //Destroy(gameObject);
        }
    }
}
