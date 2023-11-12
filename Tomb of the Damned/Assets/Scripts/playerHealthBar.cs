using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHealthBar : MonoBehaviour
{
    [SerializeField] private Image playerHealthBarSprite;
    public float currentHealth;
    public float maxHealth = 1.0f;

    private float difference;

    // We will reduce player health in this location
    public void fallDamage(float before, float after)
    {
        before = (int)before;
        after = (int)after;
        Debug.Log("Before: " + before);
        Debug.Log("After: " + after);
        difference = before - after;
        Debug.Log("Fell Height of: " + difference);

        if (difference >= 4 && difference <= 6)
        {
            Debug.Log("Low Fall Damage: ");
            currentHealth -= 10f;
            // Ensure currentHealth is within the valid range (0 to maxHealth)
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            playerHealthBarSprite.fillAmount = currentHealth / maxHealth; // Update fillAmount
            Debug.Log("Updated fillAmount to: " + playerHealthBarSprite.fillAmount);
        }
        else if (difference >= 7 && difference <= 10)
        {
            Debug.Log("Medium Fall Damage: ");
            currentHealth -= 20f;
            // Ensure currentHealth is within the valid range (0 to maxHealth)
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            playerHealthBarSprite.fillAmount = currentHealth / maxHealth; // Update fillAmount
            Debug.Log("Updated fillAmount to: " + playerHealthBarSprite.fillAmount);
        } 
        
    }

    private void Start()
    {
        currentHealth = maxHealth;
        playerHealthBarSprite.fillAmount = currentHealth / maxHealth; // Update fillAmount
        Debug.Log("Initial fillAmount set to: " + playerHealthBarSprite.fillAmount);
    }
}
