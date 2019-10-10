using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy : MonoBehaviour
{
    public enum MovementStates {Patroling, Chasing, Searching, Stuned, Dead }
    
    public NavMeshAgent agent;

    public Transform followTrans;

    public MovementStates movementState;

    public float StunTime;

    private float _stunCounter;

    public float Health = 30f;

    [Header("Debug Stuff")] 
    public TextMesh stateText;
    
    // Start is called before the first frame update
    void Start()
    {
        SetMovementState(MovementStates.Chasing);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;

        if (Health < 0f)
        {
            SetMovementState(MovementStates.Dead);
        }
        else
        {
            _stunCounter = StunTime;
            SetMovementState(MovementStates.Stuned);
        }
    }

    private void Update()
    {
        if (movementState == MovementStates.Stuned)
        {
            _stunCounter -= Time.deltaTime;

            if (_stunCounter < 0f)
            {
                _stunCounter = 0f;
                
                //Check if the enemy can see the player near by to go back to patrol or chasing the player
                SetMovementState(MovementStates.Searching);
            }
        }
    }

    private void FixedUpdate()
    {
        if (movementState == MovementStates.Chasing)
        {
            agent.destination = followTrans.position;
        }
        else if(movementState == MovementStates.Searching)
        {
            agent.destination = transform.position;
            
            //Search for player near by
            
            //TODO: Make the Enemy search for the player nearby
            SetMovementState(MovementStates.Chasing);
        }
        else if(movementState == MovementStates.Patroling)
        {
            //Follow Path (how ever I do this)
        }
        else if (movementState == MovementStates.Stuned)
        {
            agent.destination = transform.position;
        }
    }

    private void SetMovementState(MovementStates newState)
    {
        movementState = newState;

        stateText.text = newState.ToString();
    }
}
