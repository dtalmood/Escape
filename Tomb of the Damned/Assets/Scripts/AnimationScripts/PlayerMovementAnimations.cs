using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAnimations : MonoBehaviour
{
    public Animator playerAnimation;
    // public PlayerMovement getState;

    public void Awake()
    {
        playerAnimation = GetComponent<Animator>();
    }

    // walking forwards
    public void walkingAnimation()
    {
        playerAnimation.SetBool("walking", true);
        playerAnimation.SetBool("idle", false);
        playerAnimation.SetBool("walkingBackwards", false);
        playerAnimation.SetBool("crouchWalkForward", false);


    }

    // walking backwards
    public void walkingBackwardsAnimation()
    {
        playerAnimation.SetBool("walkingBackwards", true);
        playerAnimation.SetBool("walking", false);
        playerAnimation.SetBool("idle", false);
        playerAnimation.SetBool("crouchWalkForward", false);
    }


    // idle
    public void idleAnimation()
    {
        playerAnimation.SetBool("idle", true);
        playerAnimation.SetBool("walking", false);
        playerAnimation.SetBool("walkingBackwards", false);
        playerAnimation.SetBool("crouchWalkForward", false);
    }

    // crouching
    public void crouchWalkForwardAnimation()
    {
        playerAnimation.SetBool("crouchWalkForward", true);
        playerAnimation.SetBool("idle", false);
        playerAnimation.SetBool("walking", false);
        playerAnimation.SetBool("walkingBackwards", false);
    }
}