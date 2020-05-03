using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopBehavior : MonoBehaviour
{
    //This class holds everything in common for all troops.

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth = 100;

    [SerializeField] private GameObject healthBarCanvas = null;
    [SerializeField] private HealthBarScript healthBar = null;

    public void Start()
    {
        InitializeTroop();
    }

    public void InitializeTroop()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(GetMaxHealth());
        healthBarCanvas.SetActive(false);
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    public void SetCurrentHealth(int newHealthValue)
    {
        currentHealth = newHealthValue;
    }
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        healthBarCanvas.SetActive(true);
        healthBar.SetHealth(GetCurrentHealth());
    }
}
