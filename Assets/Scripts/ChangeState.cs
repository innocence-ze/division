using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class ChangeState : StateMachineBehaviour
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InputHandle.Instance.DisableInput();
        Camera.main.GetComponent<AudioSource>().Play();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        InputHandle.Instance.DisableInput();
        int type = animator.GetInteger("PushType");
        int dir = animator.GetInteger("MoveDir");

        if (stateInfo.normalizedTime < 0.7f || type < 4)
            return;


        if (type == (int)CellType.coin)
        {
            Coin.coin.MoveTo((Direction)dir);
            animator.SetInteger("PushType", 0);
        }
        else
        {
            foreach (Germ germ in Germ.germs)
            {
                if (Vector3.Distance(germ.transform.position, animator.GetComponent<Cell>().LandBoard.nearBoards[dir].transform.position) < 0.3f)
                {
                    germ.MoveTo((Direction)dir);
                    animator.SetInteger("PushType", 0);
                }
            }
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var cell = animator.GetComponent<Cell>();
        cell.DivideTo(cell.LandBoard.nearBoards[animator.GetInteger("MoveDir")].transform.position);
        InputHandle.Instance.EnableInput();
        if (animator.GetComponent<Cell>().Health == 0)
            Destroy(animator.gameObject);
        
    }

}

