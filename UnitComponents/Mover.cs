using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnitComponents;
using Core.Units;

namespace Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] Transform target;

        NavMeshAgent navMeshAgent;
        Health health;
        public bool isControlledByPlayer = false;

        Vector3 destination;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();
            UpdateAnimator();
        }

        // Starting move action
        public void StartMoveAction(Vector3 destination)
        {
            // GetComponent<ActionScheduler>().StartAction(this);
            destination = MoveTo(destination);
        }

        // Move player to the destination
        public Vector3 MoveTo(Vector3 destination)
        {
            isControlledByPlayer = true;
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
            if (transform.position == destination)
            {
                isControlledByPlayer = false;
                Cancel();
            }
            return destination;
        }

        // Stop moving
        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        // Update animation depending on the current state (moving/idle)
        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
    }
}