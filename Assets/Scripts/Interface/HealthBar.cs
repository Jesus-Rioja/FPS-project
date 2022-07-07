using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    int maxHealth;
    int lastHealth;

    Image healthBar;
    TMP_Text healthText;
    TargetWithLifeThatNotifies target;

    void Start()
    {
        healthBar = GetComponent<Image>();
        target = PlayerMovement.instance.GetComponent<TargetWithLifeThatNotifies>();
        maxHealth = (int)target.GetMaxLife();
        healthText = GetComponentInChildren<TMP_Text>();
        lastHealth = maxHealth;
        healthText.text = lastHealth + "/" + maxHealth;
    }

    void Update()
    {
        if(lastHealth != (int)target.Life)
        {
            healthBar.fillAmount = target.Life / maxHealth;
            lastHealth = (int)target.Life;

            if(lastHealth <= 0) { lastHealth = 0; }

            healthText.text = lastHealth + "/" + maxHealth;
        }

    }
}
