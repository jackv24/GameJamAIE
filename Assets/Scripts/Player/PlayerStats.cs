using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int score = 0;

    [Space()]
    public int currentHealth = 100;
    public int maxHealth = 100;

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void RemoveHealth(int amount)
    {
        //Use private ChangeHealth function to handle clamping health, death, etc.
        ChangeHealth(-amount);
    }

    public void AddHealth(int amount)
    {
        //Use private ChangeHealth function to handle clamping health, death, etc.
        ChangeHealth(amount);
    }

    void ChangeHealth(int amount)
    {
        currentHealth += amount;

        //Clamp health between 0 and max
        if (currentHealth < 0)
            currentHealth = 0;
        else if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }
}
