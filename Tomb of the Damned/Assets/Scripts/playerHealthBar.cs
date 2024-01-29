using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerHealthBar : MonoBehaviour
{
    [SerializeField] private Image playerHealthBarSprite;
    public float currentHealth;
    public float maxHealth = 1.0f;

    private float difference;

    private void Start()
    {
        currentHealth = maxHealth; // set Player Health 
        playerHealthBarSprite.fillAmount = currentHealth / maxHealth; // set sprite Health 
        //Debug.Log("Initial fillAmount set to: " + playerHealthBarSprite.fillAmount);
    }


    // We will reduce player health in this location
    public void fallDamage(float before, float after)
    {
        before = (int)before;
        after = (int)after;
        // Debug.Log("Before: " + before);
        // Debug.Log("After: " + after);
        difference = before - after;
        // Debug.Log("Fell Height of: " + difference);

        if (difference >= 4 && difference <= 6) // Small Amount of Damage
        {
            StartCoroutine(ApplyDamageOverTime(10, 0.05f, 1f));
        }
        else if (difference >= 7 && difference <= 10) // Medium Amount of Damage
        {
            StartCoroutine(ApplyDamageOverTime(20, 0.05f, 1f));
        }
        else if(difference >= 11 && difference <= 15) // Large Amount of Damage
        {
            StartCoroutine(ApplyDamageOverTime(30, 0.05f, 1f));
        }
    }

    private IEnumerator ApplyDamageOverTime(int iterations, float timePerIteration, float decreaseAmountPerIteration)
    {
        for (int i = 0; i < iterations; i++)
        {
            currentHealth -= decreaseAmountPerIteration;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            playerHealthBarSprite.fillAmount = currentHealth / maxHealth;
            gameOverCheck();
            yield return new WaitForSeconds(timePerIteration);
        }
        //Debug.Log("Updated fillAmount to: " + playerHealthBarSprite.fillAmount);
    }

    private void gameOverCheck()
    {
        if(currentHealth == 0)
        {
                // load the game over screen
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

}
