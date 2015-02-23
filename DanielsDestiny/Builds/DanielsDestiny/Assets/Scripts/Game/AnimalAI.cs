using UnityEngine;
using System.Collections;

public class AnimalAI : MonoBehaviour 
{
    public enum MovePatterns
    {
        Idle,
        Roam,
        MoveToPlayer,
        Attack
    }
    MovePatterns curMovePattern = MovePatterns.Roam;

    CharacterController controller;
    public float speed = 6.0F;
    public float turnspeed = 0.0F;
    public float gravity = 20.0F;
    public float damage = 10;
    public float damageTimer = 2;
    public float health = 100;

    private Vector3 moveDirection = Vector3.zero;

    Vector3 curWaypoint = Vector3.zero;
    public AISpawner mySpawner { get; set; }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        SetNewTask();
    }

    void Update()
    {
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
        if(CheckDistanceToPlayer() && curMovePattern != MovePatterns.Attack)
            curMovePattern = MovePatterns.MoveToPlayer;
        switch (curMovePattern)
        {
            case MovePatterns.Roam:
                MoveTowardsTarget();
                break;
            case MovePatterns.MoveToPlayer:
                MoveTowardsPlayer(Player.instance.transform);
                break;
            default:
                break;
        }
        
    }

    void MoveTowardsTarget()
    {
        Quaternion newrot = new Quaternion(0, Quaternion.LookRotation(curWaypoint - transform.position).y, 0, Quaternion.LookRotation(curWaypoint - transform.position).w);
        transform.rotation = Quaternion.Slerp(transform.rotation, newrot, turnspeed * Time.deltaTime);

        Vector3 a = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 b = new Vector3(curWaypoint.x, 0, curWaypoint.z);

        if (Vector3.Distance(a,b) > 1)
        {
            moveDirection = transform.forward * speed;
        }
        else
        {
            SetNewTask();
        }

        //moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    void SetNewTask()
    {
        
        if (Random.Range(0, 101) > 50)
        {
            curWaypoint = transform.position + new Vector3(Random.Range(-30, 30), 0, Random.Range(-30, 30));
            curMovePattern = MovePatterns.Roam;
        }
        else
        {
            curMovePattern = MovePatterns.Idle;
            Invoke("SetNewTask", 5);
        }
    }

    bool CheckDistanceToPlayer()
    {
        if (Vector3.Distance(transform.position, Player.instance.transform.position) < 10)
        {
            return true;
        }
        return false;
    }

    void MoveTowardsPlayer(Transform target)
    {
        Quaternion newrot = new Quaternion(0, Quaternion.LookRotation(target.position - transform.position).y, 0, Quaternion.LookRotation(target.position - transform.position).w);
        transform.rotation = Quaternion.Slerp(transform.rotation, newrot, turnspeed * Time.deltaTime);


        if (Vector3.Distance(transform.position, target.position) > 2)
        {
            moveDirection = transform.forward * speed;
        }
        else
        {
            moveDirection = Vector3.zero;
            curMovePattern = MovePatterns.Attack;
            StartCoroutine(AttackPlayer());
        }

        //moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    IEnumerator AttackPlayer()
    {
        while (CheckDistanceToPlayer())
        {
            yield return new WaitForSeconds(damageTimer);
            Player.instance.status.TakeDamage(damage);
        }
        curMovePattern = MovePatterns.MoveToPlayer;
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            mySpawner.myMonsters.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
