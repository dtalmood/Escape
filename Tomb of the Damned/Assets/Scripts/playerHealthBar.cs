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

    private int recievedInitalHeight;
    private int recievedAfterHeight;

    public void fallDamage(float jumpInitialHeight,float jumpAfterHeight)
    {
        Debug.Log("Before Height = " + jumpInitialHeight);
        Debug.Log("After Height = " + jumpAfterHeight);
    }

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
