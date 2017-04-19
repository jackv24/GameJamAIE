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
        ChangeHealth(-amount);
    }

    public void AddHealth(int amount)
    {
        ChangeHealth(amount);
    }

    void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth < 0)
            currentHealth = 0;
        else if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }
}
