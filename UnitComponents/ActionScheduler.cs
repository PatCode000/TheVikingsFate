using UnityEngine;
using Core.Units;

namespace Core
{
    public class ActionScheduler : MonoBehaviour
    {

        IAction currentAction;

        // This function control whenever player start/stop attacking or moving
        public void StartAction(IAction action)
        {
            if (currentAction == action) return;
            if (currentAction != null)
            {
                currentAction.Cancel();
            }
            currentAction = action;
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}