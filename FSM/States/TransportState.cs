using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Buildings;

namespace FSM.States
{
    [CreateAssetMenu(fileName = "TransportState", menuName = "FSM/States/Transport", order = 3)]

    public class TransportState : State
    {
        float collectionPointTolerance = 1f;
        bool hasSheep;

        Farm farm;
        Butchery butchery;

        public override void OnEnable()
        {
            base.OnEnable();
            StateType = StateType.TRANSPORT;
            hasSheep = false;
        }

        public override bool EnterState()
        {
            EnteredState = base.EnterState();
            EnteredState = true;
            butchery = _worldElements.buildings[BuildingType.BUTCHERY].GetComponent<Butchery>();
            farm = _worldElements.buildings[BuildingType.FARM].GetComponent<Farm>();

            return EnteredState;
        }

        public override void UpdateState()
        {
            _mover.StartMoveAction(farm.GetAssemblyPoint());

            HandleTransportBehaviour();

            BackToIdle();
        }

        public override bool ExitState()
        {
            base.ExitState();
            return true;
        }

        private void HandleTransportBehaviour()
        {
            if (hasSheep)
            {
                _npc.sheep.SetActive(true);
                _mover.StartMoveAction(butchery.GetComponent<Butchery>().GetAssemblyPoint());
                if (AtCollectionPoint(butchery.GetComponent<Butchery>().GetAssemblyPoint()))
                {
                    _npc.sheep.SetActive(false);
                    butchery.GetComponent<Butchery>().IncreaseNumberOfSheeps();
                    hasSheep = false;
                }
            }
            else
            {
                if (AtCollectionPoint(farm.GetAssemblyPoint()))
                {
                    _resourceController.DecreaseNumberOfSheeps();
                    hasSheep = true;
                }
            }
        }

        private void BackToIdle()
        {
            if (_resourceController.GetNumberOfSheeps() == 0 && (!hasSheep))
            {
                _fsm.EnterState(StateType.IDLE);
            }
        }

        private bool AtCollectionPoint(Vector3 destination)
        {
            float distanceToWaypoint = Vector3.Distance(_navMeshAgent.transform.position, destination);
            return distanceToWaypoint < collectionPointTolerance;
        }
    }
}
