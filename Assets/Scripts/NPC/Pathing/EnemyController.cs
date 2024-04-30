using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EntityState
{
    Idle,
    FollowingPath,
    ChasingTarget
}
public class EnemyController : MonoBehaviour
{ 
    
    private Vector2 velocity = Vector2.zero;
    private float smoothTime = 0.3f;
    public int speed = 5;
    public Vector2[] path = new Vector2[0];
    private int pathIndex = 0;
    
    private Rigidbody2D rigidbody2D;
    private Animator animator;
    public float chaseDistance = 10f;
    private GameObject currentTarget;
    public EntityState currentState = EntityState.Idle;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentState = path.Length > 0 ? EntityState.FollowingPath : EntityState.Idle;
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case EntityState.Idle:
                FindNearestChaseable();
                CheckStateTransition();
                break;
            case EntityState.FollowingPath:
                MoveAlongPath();
                FindNearestChaseable();
                CheckStateTransition();
                break;
            case EntityState.ChasingTarget:
                if (currentTarget != null)
                {
                    MoveTowards(currentTarget.transform.position);
                }
                FindNearestChaseable();
                CheckStateTransition();
                break;
        }
    }

    private void CheckStateTransition()
    {
        if (currentTarget != null && currentState != EntityState.ChasingTarget)
        {
            currentState = EntityState.ChasingTarget;
        }
        else if (currentTarget == null && currentState == EntityState.ChasingTarget)
        {
            currentState = path.Length > 0 ? EntityState.FollowingPath : EntityState.Idle;
        }
    }

    private void MoveAlongPath()
    {
        if (pathIndex < path.Length)
        {
            Vector2 currentPoint = path[pathIndex];
            if (Vector2.Distance(transform.position, currentPoint) <= 0.1f)
            {
                pathIndex++;
                if (pathIndex >= path.Length)
                {
                    currentState = EntityState.Idle;
                    return; // End of path
                }
            }
            else
            {
                MoveTowards(currentPoint);
            }
        }
    }

    private void FindNearestChaseable()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] chaseables = GameObject.FindGameObjectsWithTag("Chaseable");

        List<GameObject> allChaseTargets = new List<GameObject>(players);
        allChaseTargets.AddRange(chaseables);

        float nearestDistance = Mathf.Infinity;
        GameObject nearestTarget = null;
        foreach (GameObject chaseable in allChaseTargets)
        {
            float distance = Vector2.Distance(transform.position, chaseable.transform.position);
            if (distance < chaseDistance && distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTarget = chaseable;
            }
        }
        currentTarget = nearestTarget; // Update current target only if a closer one is found within range
    }

    private void MoveTowards(Vector2 target)
    {
        float step = speed * Time.deltaTime;
        Vector2 newPosition = Vector2.MoveTowards(rigidbody2D.position, target, step);
        rigidbody2D.MovePosition(newPosition);

        Vector2 moveDirection = (target - rigidbody2D.position);
        bool currentIsVertical = Mathf.Abs(target.y - transform.position.y) > Mathf.Abs(target.x - transform.position.x);
        if (currentIsVertical)
        {
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", moveDirection.y);
        }
        else
        {
            animator.SetFloat("Move X", moveDirection.x);
            animator.SetFloat("Move Y", 0);
        }
    }
}
