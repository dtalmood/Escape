using System.Collections;
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
        // Debug.Log("Before: " + before);
        // Debug.Log("After: " + after);
        difference = before - after;
        // Debug.Log("Fell Height of: " + difference);

        if (difference >= 4 && difference <= 6)
        {
            StartCoroutine(ApplyDamageOverTime(10, 0.05f, 1f));
        }
        else if (difference >= 7 && difference <= 10)
        {
            StartCoroutine(ApplyDamageOverTime(20, 0.05f, 1f));
        }
    }

    private IEnumerator ApplyDamageOverTime(int iterations, float timePerIteration, float decreaseAmountPerIteration)
    {
        for (int i = 0; i < iterations; i++)
        {
            currentHealth -= decreaseAmountPerIteration;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            playerHealthBarSprite.fillAmount = currentHealth / maxHealth;

            yield return new WaitForSeconds(timePerIteration);
        }

        //Debug.Log("Updated fillAmount to: " + playerHealthBarSprite.fillAmount);
    }

    private void Start()
    {
        currentHealth = maxHealth;
        playerHealthBarSprite.fillAmount = currentHealth / maxHealth;
        //Debug.Log("Initial fillAmount set to: " + playerHealthBarSprite.fillAmount);
    }
}
