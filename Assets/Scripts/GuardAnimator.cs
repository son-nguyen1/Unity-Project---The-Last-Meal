using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAnimator : MonoBehaviour
{
    // Script reference
    private GuardController guardController;

    // Animation
    private Animator guardAnimator;
    private const string Guard_Is_Walking = "GuardIsWalking";

    private void Awake()
    {
        guardAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        guardController = GetComponentInParent<GuardController>();
    }

    private void Update()
    {
        guardAnimator.SetBool(Guard_Is_Walking, guardController.GuardIsWalking());
    }
}