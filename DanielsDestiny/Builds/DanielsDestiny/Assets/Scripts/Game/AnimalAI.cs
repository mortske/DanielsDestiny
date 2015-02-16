using UnityEngine;
using System.Collections;

public class AnimalAI : MonoBehaviour 
{
    public enum MovePatterns
    {
        Idle,
        Roam,
        MoveToPlayer
    }
    MovePatterns curMovePattern = MovePatterns.Roam;

    CharacterController controller;
    public float speed = 6.0F;
    public float turnspeed = 0.0F;
    public float gravity = 20.0F;

    private Vector3 moveDirection = Vector3.zero;

    Vector3 curWaypoint = Vector3.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        SetNewTask();
    }

    void Update()
    {
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

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    void SetNewTask()
    {
        
        if (Random.Range(0, 101) > 50)
        {
            Debug.Log("set new move task");
            curWaypoint = transform.position + new Vector3(Random.Range(-30, 30), 0, Random.Range(-30, 30));
            curMovePattern = MovePatterns.Roam;
        }
        else
        {
            Debug.Log("set new idle task");
            curMovePattern = MovePatterns.Idle;
            Invoke("SetNewTask", 5);
        }
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
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
