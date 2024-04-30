using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureAttack : MonoBehaviour
{

    public float attackInterval = 2.0f; // Interval between attacks in seconds
    private float attackTimer;
    private int damage = 2;
    private List<GameObject> enemies = new List<GameObject>(); // List to keep track of enemies within range


    void Start()
    {
        attackTimer = attackInterval; 

    }

    void Update()
    {
        attackTimer -= Time.deltaTime;
        
        if (attackTimer <= 0f)
        {
            bool hasAttacked = false;
            // Attack all enemies currently within the collider
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null && enemy.activeInHierarchy)
                {
                    EnemyHealth controller = enemy.GetComponent<EnemyHealth>();
                        if (controller != null)
                        {
                            Debug.Log("Enemy attacked: " + enemy.name);
                            controller.ChangeHealth(-damage);
                            hasAttacked = true;
                        }
                        else
                            Debug.LogWarning("EnemyHealth component not found on " + enemy.name);
                }
            }
   
            // Reset the timer after the attack
            if (hasAttacked)
                attackTimer = attackInterval;


        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Add(other.gameObject); // Add enemy to the list
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Remove(other.gameObject); // Remove enemy from the list
        }
    }
}
