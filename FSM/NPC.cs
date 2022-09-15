using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Control;
using Movement;

namespace FSM
{
    // [RequireComponent(typeof(NavMeshAgent), typeof(FiniteStateMachine))]

    public class NPC : MonoBehaviour
    {
        NavMeshAgent navMeshAgent;
        StateController finiteStateMachine;
        Mover mover;
        public PatrolPath patrolPath;
        public GameObject sheep;

        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            finiteStateMachine = GetComponent<StateController>();
            mover = GetComponent<Mover>();
            sheep.SetActive(false);
        }
    }
}
