using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathDetection : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();

        playerController.SwitchCanMoveState();
    }
}
