using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerStats targetStats;

    [Space()]
    public Slider healthSlider;
    public Text healthText;
    private string healthTextString;

    [Space()]
    public Text scoreText;
    private string scoreTextString;

    void Start()
    {
        if (healthText)
            healthTextString = healthText.text;

        if (scoreText)
            scoreTextString = scoreText.text;
    }

    void Update()
    {
        if(targetStats)
        {
            if (healthSlider)
                healthSlider.value = (float)targetStats.currentHealth / targetStats.maxHealth;

            if (healthText)
                healthText.text = string.Format(healthTextString, targetStats.currentHealth, targetStats.maxHealth);

            if (scoreText)
                scoreText.text = string.Format(scoreTextString, targetStats.score);
        }
    }
}
