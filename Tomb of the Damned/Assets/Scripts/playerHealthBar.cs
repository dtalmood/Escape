using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


// This script will be be used to send infomration of when to increase, decrease, or reset the player health 
public class playerHealthBar : MonoBehaviour
{

    [SerializeField] private Image healthBarPlayer; 
    public float currentHealth;
    public float maxHealth = 100f; 

    private void start()
    {
        currentHealth = maxHealth;
    }

    
    
    private void reduceHealth() 
    {
        
    }

    private void increaseHealth()
    {

    }

    private void fullHealth()
    {

    }

    


}
