using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;  // Make sure to include the UnityEngine.UI namespace
using UnityEngine;

public class playerHealthBar : MonoBehaviour
{
    [SerializeField] private Image playerHealthBarSprite;
    public float currentHealth;
    public float maxHealth = 1f;

    private float difference;


    // We will reduce player health in this location
    public void fallDamage(float before, float after)
    {
        difference = before - after;
        Debug.Log("Difference" + difference);
        if(difference >= 10)
        {
            Debug.Log("Lower Health");
            currentHealth -= 0.1f;
            playerHealthBarSprite.fillAmount = currentHealth;
            
        }
        
    }

    private void Start()  // Corrected typo here
    {
        currentHealth = maxHealth;
        playerHealthBarSprite.fillAmount = currentHealth;
    }
}