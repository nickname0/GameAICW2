using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is drawing the circle around the enemy. 
//Also is taking the angles from the values that we gave them from EnemyController script. And is shooting a green raycast when the enemy sees the player
public class ViewEditor2 : MonoBehaviour
{
    EnemyController fow;

    void Start()
    {
        fow = GetComponent<EnemyController>();
    }

    void OnDrawGizmos()
    {
        //Creating the circle around the player
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(fow.transform.position, fow.viewRadius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

        //Drawing 2 lines from the enemy
        Gizmos.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Gizmos.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

        //Giving the enemy a Raycast that is shot when he spots the player
        Gizmos.color = Color.green;
        foreach (Transform visibleTarget in fow.visibleTargets)
        {
            Gizmos.DrawLine(fow.transform.position, visibleTarget.position);
        }
    }
}
