using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _myAnimator;

    private void Start()
    {
        _myAnimator = GetComponent<Animator>();
    }
}
