using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Tooltip("The PlayerStats that this UI represents.")]
    public PlayerStats targetStats;

    [Space()]
    public Slider healthSlider;
    public Text healthText;
    private string healthTextString;

    [Space()]
    public Text scoreText;
    private string scoreTextString;

    [Space()]
    public Text readyText;

    void Start()
    {
        //Cache initial strings for formatting
        if (healthText)
            healthTextString = healthText.text;
        if (scoreText)
            scoreTextString = scoreText.text;
    }

    void Update()
    {
        if(targetStats)
        {
            //Update health slider value
            if (healthSlider)
                healthSlider.value = (float)targetStats.currentHealth / targetStats.maxHealth;

            //Update formatted health text fields
            if (healthText)
                healthText.text = string.Format(healthTextString, targetStats.currentHealth, targetStats.maxHealth);

            //Update formatted score text field
            if (scoreText)
                scoreText.text = string.Format(scoreTextString, targetStats.score);
        }
    }
}
