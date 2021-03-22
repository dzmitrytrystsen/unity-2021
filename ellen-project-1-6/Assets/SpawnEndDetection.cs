using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEndDetection : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();

        playerController.SwitchCanMoveState();
    }
}
