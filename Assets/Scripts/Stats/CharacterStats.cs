using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHealt = 100;
    public int currentHealth { get; private set; }
    public Stat attack;
    public Stat defense;

    private void Awake()
    {
        currentHealth = maxHealt;
    }

    private void Update()
    {
        
        //por si modificamos el valor base con el uso de 1 item
       /* if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        };*/
    }

    public void TakeDamage(int damage)
    {
        damage -= defense.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);//Esto es para restarle los puntos q tenga de defensa y no pasarse de cero
        
        currentHealth -= damage;
        Debug.Log(transform.name+" takes "+ damage+" damage."); 

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        //Die in some way
        //this method is meant to be overwritten
        
    }
    
    
}
