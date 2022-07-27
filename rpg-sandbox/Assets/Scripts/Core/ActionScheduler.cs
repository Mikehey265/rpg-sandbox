using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;
        public void StartAction(IAction action){
            //if current action is already running we don't need to do anything
            if(currentAction == action){
                return;
            }
            if(currentAction != null){
                currentAction.Cancel();
            }
            currentAction = action;
        }

        public void CancelCurrentAction(){
            StartAction(null);
        }
    }
}