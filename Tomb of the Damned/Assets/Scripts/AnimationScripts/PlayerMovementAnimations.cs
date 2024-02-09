using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAnimations : MonoBehaviour
{
    public Animator playerAnimation;

    public void Awake()
    {
        playerAnimation = GetComponent<Animator>();
    }

    public void walkingAnimation()
    {
        playerAnimation.SetBool("idle", false);
        playerAnimation.SetBool("walking", true);
    }

    public void idleAnimation()
    {
        playerAnimation.SetBool("idle", true);
        playerAnimation.SetBool("walking", false);
    }
    public void crouchingAnimation()
    {
        playerAnimation.SetBool("idle", false);
        playerAnimation.SetBool("walking", false);
        
    }
}
