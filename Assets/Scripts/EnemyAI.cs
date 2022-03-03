using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    private NavMeshAgent nav;
    public Transform player;




    //for enemy gizmo sphere
    public float radius;

    //a variabler declared to calculate between the player position
    //and the enemy position
    float distanceToPlayer = Mathf.Infinity;

    Animator anim;

    [HideInInspector]
    public bool isDied;

    private bool stopEnemy;
    // Start is called before the first frame update
    void Start()
    {
        isDied = false;
        stopEnemy = false;
        anim = GetComponent<Animator>();

        nav = GetComponent<NavMeshAgent>();

        //randomSpot 

        
    }

    // Update is called once per frame
    void Update()
    {
        ////if the player inside the enemies sphere
        ///the enemy will attack 

        if (!isDied)
        {
            distanceToPlayer = Vector3.Distance(player.position, transform.position);

            if (distanceToPlayer > radius)
            {
                //Patrol();
                stopEnemy = false;
                anim.Play("Idle");
            }
            else if (distanceToPlayer <= radius)
            {
                AttackPlayer();
                
            }
        }
    }

    //the attack function logic
    //1 if the player bigger than or equals the enemies stopping distance, the enemy will chase
    //2 if the player less than or equals the enemies stopping distance, the enemy will shoot
    private void AttackPlayer()
    {
        faceToPlayer();
        if (distanceToPlayer > nav.stoppingDistance)
        {
            ChasePlayer();
        }
        else if (distanceToPlayer <= nav.stoppingDistance)
        {
            ShootPlayer();
        }
    }

    //function for when the enemy chases the player
    private void ChasePlayer()
    {
        if (!stopEnemy) {
            nav.SetDestination(player.position);
            anim.Play("Z_Run_InPlace");
        }
        
    }

    //function when the player shoots the enemy
    private void ShootPlayer()
    {
        stopEnemy = true;
        anim.Play("Attack");
    }

    

    private void faceToPlayer() {

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10.0f);
    
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void Died() {
        isDied = true;
        anim.Play("Z_FallingBack");
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<AudioSource>().enabled = false;
    }
   
}
