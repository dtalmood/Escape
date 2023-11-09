using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public int currentHealth;
    public int maxHealth = 100; 
    
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
