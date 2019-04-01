using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//FSM AI used for the enemy movement and states.
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    //Creating a radius that will be used for the enemy cone of sight
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    //Masks that will be used for Target(Player) and Obstacles (Wall)
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    //Visible Variable
    public bool visiblePlayer = false;

    public List<Transform> visibleTargets = new List<Transform>();

    //Transform target;
    public UnityEngine.AI.NavMeshAgent agent { get; private set; } //// the navmesh agent required for the path finding

    private enum State { WANDERING, CHASING, INVESTIGATION, DEAD};

    private State state = State.WANDERING;

    // Wandering Variables
    public Transform moveSpot;
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;

    public float speed;
    private float waitTime;
    public float startWaitTime;

    //Investigation Variables
    private Vector3 lastLocation;
    public Transform player;
    private bool temp = true;
    private int count = 0;

    //Shooting Variable
    private float fireTime;
    public float startFireTime;

    public GameObject shot;
    public Transform shotSpawn;

    //Chasing Variable
    private float seeTime;
    public float startSeeTime;

    //Function used to destroy the object
    public void kill()
    {
        this.state = State.DEAD;
    }

    void Start()
    {
        waitTime = startWaitTime;
        moveSpot.position = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));

        //target = PlayerManager.instance.player.transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        fireTime = startFireTime;


        StartCoroutine("FindTargetsWithDelay", .2f);
    }

    //Finding Target 
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            seePlayer();
        }
    }

    void Update()
    {
        //FSM AI
        switch (state)
        {
            // The enemy will wander until he detectes the Player
            case State.WANDERING:
                agent.transform.position = Vector3.MoveTowards(agent.transform.position, moveSpot.position, speed * Time.deltaTime);
                agent.transform.LookAt(moveSpot);

                if (Vector3.Distance(agent.transform.position, moveSpot.position) < 0.5f)
                {
                    if (waitTime <= 0)
                    {
                        moveSpot.position = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));
                        waitTime = startWaitTime;
                    }
                    else
                        waitTime -= Time.deltaTime;
                }
                //Debug.Log(waitTime);
                if (seePlayer())
                {
                   state = State.INVESTIGATION;
                    
                }
                temp = true;

                break;

                //Will start chasing if he sees the player
            case State.CHASING:

                if (seePlayer())
                {
                    //Will look at the target position, cone of sight moves with it
                    agent.transform.LookAt(moveSpot);

                    //Gun fire rates
                    if (fireTime <= 0)
                    {
                        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                        fireTime = startFireTime;
                    }
                    else
                    {
                        fireTime -= Time.deltaTime;
                    }
                }
                //If he does not see the player will return to Investigate state
                if(!seePlayer())
                {
                    if(seeTime<=0)
                    {
                        state = State.INVESTIGATION;
                        seeTime = startSeeTime;
                    }
                    else
                    {
                        seeTime -= Time.deltaTime;
                    }
                }
                break;
                // this state will go to the last known player Position
            case State.INVESTIGATION:

                agent.transform.LookAt(lastLocation);
                if(temp == true)
                {     
                    lastLocation = player.transform.position;
                    count++;
                    temp = false;
                }

                if (Mathf.Round(transform.position.z) == Mathf.Round(lastLocation.z))
                {
                    if (waitTime <= 0)
                    {

                        state = State.WANDERING;
                        waitTime = startWaitTime;
                    }
                    else
                        waitTime -= Time.deltaTime;
                }
                if (count ==2)
                {
                    state = State.CHASING;
                }
                
                agent.transform.position = Vector3.MoveTowards(transform.position,lastLocation, speed * Time.deltaTime);
                
                break;

            case State.DEAD:
                Destroy(gameObject);

                break;
        }
        Debug.Log(state);
    }

    //Function that will detect the player and if the player is visible a target would be added
    protected bool seePlayer()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {

                    //visiblePlayer = true;
                    temp = true;
                    visibleTargets.Add(target);
                    return true;
                }
            }
        }
        
        return false;

    }

    //
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
