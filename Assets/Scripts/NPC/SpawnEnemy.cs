using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] [Tooltip("True for harder zone")]
    private bool isRedZone=false;
    [SerializeField] [Tooltip("Number of enemies to spawn per trigger")]
    private int partialNumberToSpawn = 4;
    [SerializeField] [Tooltip("Time (seconds) to wait between spawns")]
    private int timeToWait=5;
    [SerializeField] [Tooltip("How many times partial spawns can occur per round")]
    private int partialUsesPerRound=3;

    private int _partialUsed = 0;
    
    public GameObject[] prefabsToSpawn;  
    
    private UiDisplayInfo uiDisplayInfo= UiDisplayInfo.Instance;
    public void StartSpawning(int numTimes)
    {
        
        StartCoroutine(SpawnObjects(numTimes));
    }
    
    public IEnumerator SpawnObjects(int numberOfEnemies)
    {
        // Check the number of children and prefabs match
        if (transform.childCount != prefabsToSpawn.Length)
        {
            Debug.LogError("The number of children does not match the number of prefabs to spawn.");
            yield break;  // Exit the coroutine early
        }

        // This assumes that the number of spawns may not be tied directly to the number of prefabs available
        int prefabIndex = 0;
    
        for (int i = 0; i < numberOfEnemies; i++)
        {
            GameObject child = transform.GetChild(prefabIndex).gameObject;
            GameObject prefab = prefabsToSpawn[prefabIndex];
        
            if (prefab != null)
            {
                Instantiate(prefab, child.transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Prefab at index " + prefabIndex + " is not set.");
            }

            // Cycle through prefabs
            prefabIndex = (prefabIndex + 1) % prefabsToSpawn.Length;

            // Wait before spawning the next enemy, if needed
            if (i < numberOfEnemies - 1)
            {
                yield return new WaitForSeconds(timeToWait);
            }
        }
    }

    private void SpawnPartial(int numberToSpawn)
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsRoundActive)
        {
            Debug.Log("Partial spawns are unavailable: Round is not active.");
            return;
        }
        if (_partialUsed > partialUsesPerRound)
        {
            Debug.Log("Partial spawns are unavailable: Limit reached this round");
            return;
        }
        
        if (numberToSpawn < prefabsToSpawn.Length)
            for (int i = 0; i < numberToSpawn; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                GameObject prefab = prefabsToSpawn[i];
                if (prefab != null)
                {
                    Instantiate(prefab, child.transform.position, Quaternion.identity);
                }
                else
                    Debug.LogError("Prefab at index " + i + " is not set.");
            }
        else
            StartSpawning(numberToSpawn/prefabsToSpawn.Length);

        _partialUsed++;

    }

    public void ResetPartials()
    {
        Debug.Log("Resetting partial spawns");
        _partialUsed = 0;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log( GameManager.Instance.IsRoundActive);
            SpawnPartial(partialNumberToSpawn);
        }
    }
}
