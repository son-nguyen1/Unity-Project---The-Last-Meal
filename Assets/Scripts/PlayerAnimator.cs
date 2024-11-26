using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    // Script reference
    private PlayerController playerController;

    // Animation
    private Animator playerAnimator;
    private const string Player_Is_Walking = "PlayerIsWalking";

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        playerAnimator.SetBool(Player_Is_Walking, playerController.PlayerIsWalking());
    }
}