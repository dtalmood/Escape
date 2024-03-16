using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events; // This allows us to Call Diferent Events 

// we created a flaot event, it gets called whenever the palyers health changes 

[System.Serializable] public class FloatEvent : UnityEvent<float>{} 

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image playerHealthBarSprite;
    public float currentHealth;
    public float maxHealth = 1.0f;
    
    // This is a instance of our float event 
    public FloatEvent onTakeDamage;

    private float difference;

    private void Start()
    {
        currentHealth = maxHealth; // set Player Health 
        playerHealthBarSprite.fillAmount = currentHealth / maxHealth; // set sprite Health 
        //Debug.Log("Initial fillAmount set to: " + playerHealthBarSprite.fillAmount);
    }

    public void monsterDamage()
    {
        StartCoroutine(ApplyDamageOverTime(30, 0.05f, 1f));
        Debug.Log("Large");
    }

    // We will reduce player health in this location
    public void fallDamage(float before, float after)
    {
        //Debug.Log("Player Has taken damage");
        before = (int)before;
        after = (int)after;
        // Debug.Log("Before: " + before);
        // Debug.Log("After: " + after);
        difference = before - after;
         Debug.Log("Fell Height of: " + difference);

        if (difference >= 4 && difference <= 6) // Small Amount of Damage
        {
            StartCoroutine(ApplyDamageOverTime(10, 0.05f, 1f));
            Debug.Log("Small");
        }
        else if (difference >= 7 && difference <= 10) // Medium Amount of Damage
        {
            StartCoroutine(ApplyDamageOverTime(20, 0.05f, 100f)); 
            Debug.Log("Medium");
        }
        else if(difference >= 11) // Large Amount of Damage
        {
            StartCoroutine(ApplyDamageOverTime(30, 0.05f, 100f));
            Debug.Log("Large");
        }
    }

    private IEnumerator ApplyDamageOverTime(int iterations, float timePerIteration, float decreaseAmountPerIteration)
    {
        for (int i = 0; i < iterations; i++)
        {
            currentHealth -= decreaseAmountPerIteration;
            // IMPORTANT NOTE: 
            // With Unity events you call Invoke and invoke calls all fucntions that have been added using addListener 
            // onTakeDamage Gets Called Then It looks at Listeners in Others Scripts 
            onTakeDamage?.Invoke(currentHealth); 
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            playerHealthBarSprite.fillAmount = currentHealth / maxHealth;
            bool dead = gameOverCheck();
            if(dead)
            {
                // adjust this value inside of waitfor seconds for howl ong to wait until the gameover screne loads 
                yield return new WaitForSeconds(5f);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            yield return new WaitForSeconds(timePerIteration);
        }
        //Debug.Log("Updated fillAmount to: " + playerHealthBarSprite.fillAmount);
    }

    private bool gameOverCheck()
    {
        if(currentHealth == 0)
        {
            return true;          
        }
        return false;
    }
}
