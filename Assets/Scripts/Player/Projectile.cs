using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 1;

    Rigidbody2D rigidbody2d;
    
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }
    
    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }
    
    void Update()
    {
        if(transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject victim = col.gameObject;
        bool hitPlayer = false;
        if (victim.CompareTag("Enemy"))
        {
            HealthManager controller = victim.GetComponent<HealthManager>();
            if (controller != null)
            {
                Debug.Log("Enemy attacked: " + victim.name);
                controller.TakeDamage(damage);
            }
            else
                Debug.LogWarning("HealthManager component not found on " + victim.name);
        }
        else if (victim.CompareTag("Player") || victim.CompareTag("AOI") ||victim.CompareTag("Chaseable") || victim.CompareTag("StructureLocation")) //No clue why it hits the player but it does so we need to check
            hitPlayer = true;
        else
            Debug.Log("Missed and hit : " + victim.name);
        
        if(!hitPlayer)
            Destroy(gameObject);   
    }

    void OnCollisionEnter2D(Collision2D other)
    {

    }
}
