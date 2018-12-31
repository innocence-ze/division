using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class InitState : StateMachineBehaviour
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator.GetComponent<Cell>().Health>0)
        InputHandle.Instance.DisableInput();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InputHandle.Instance.EnableInput();
    }
}

