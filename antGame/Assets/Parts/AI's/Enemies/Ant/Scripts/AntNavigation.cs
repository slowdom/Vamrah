using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntNavigation : MonoBehaviour
{
    public Transform[] waypoints;

    public float distanceFromTarget;
    public float searchTime;

    protected Vector3 desiredPosition;

    private int currentWaypoint;
    private float searchTimer;

    private enum MovementStates { patrolling, searching, chasing }
    private MovementStates movementStates;

    private AntMotor antMotor;

    private void Start()
    {
        antMotor = GetComponent<AntMotor>();
    }

    public void Tick(SearchStates searchStates, Vector3 allertedPosition)
    {
        antMotor.Tick(desiredPosition);
        StateMachine(searchStates, allertedPosition);
    }

    void StateMachine(SearchStates searchStates, Vector3 allertedPosition)
    {
        if (movementStates == MovementStates.patrolling)
        {
            Patrolling();
        }
        if (searchStates == SearchStates.sawSomeone)
        {
            movementStates = MovementStates.chasing;

            Chasing(allertedPosition);
        }
         if (searchStates == SearchStates.idle)
        {
            movementStates = MovementStates.patrolling;
        }
        if (searchStates == SearchStates.heardSomething)
        {
            Searching(allertedPosition);
        }
        else
        {
            searchTimer = 0;
        }
    }

    void Chasing(Vector3 allertedPosition)
    {
        desiredPosition = allertedPosition;
    }

    void Searching(Vector3 allertedPosition)
    {
        Debug.Log("Searching");

        desiredPosition = allertedPosition;

        if (Vector3.Distance(transform.position, desiredPosition) < distanceFromTarget)
        {
            searchTimer += Time.deltaTime;

            if (searchTimer > searchTime)
            {
                movementStates = MovementStates.patrolling;

                searchTime = 0;
            }
        }
    }

    float patrolTimer = 0;
    public float patrolWait = 1;

    void Patrolling()
    {
        desiredPosition = waypoints[currentWaypoint].position;

        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < distanceFromTarget)
        {
            patrolTimer += Time.deltaTime;

            if (patrolTimer > patrolWait)
            {
                currentWaypoint++;
                if (currentWaypoint > waypoints.Length - 1)
                {
                    currentWaypoint = 0;
                }
                patrolTimer = 0;
            }
        }
    }
}
