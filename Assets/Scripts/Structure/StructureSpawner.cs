using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StructureSpawner : MonoBehaviour
{
    public GameObject objectPrefab;  // Array to hold prefabs

    public GameObject player;
    [SerializeField] [Tooltip("Free area needed to spawn structure")]
    public float checkRadius = 1.0f;  // Radius for overlap check

    private int _buildCost = 1;
    private void SpawnStructure()
    {
        if (objectPrefab != null)
        {
            Vector2 spawnPosition = transform.position;
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(spawnPosition, checkRadius);
            bool isOccupied = false;

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Chaseable"))
                {
                    isOccupied = true;
                    break;
                }
            }

            if (!isOccupied)
                
                if(GameManager.Instance.TryRemoveSeeds(_buildCost))
                    Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
                else
                    Debug.Log("No seeds available.");
            
            else
                Debug.Log("Spawn location is already occupied by another structure.");
        }
        else
            Debug.LogError("Prefab or spawn location not set.");
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            SpawnStructure();
        } 
    }
}
