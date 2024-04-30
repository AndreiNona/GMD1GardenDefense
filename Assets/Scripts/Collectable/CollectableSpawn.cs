using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawn : MonoBehaviour
{
    
    public GameObject collectablePrefab;
    [SerializeField] [Tooltip("Base health")]
    private int baseHealth = 10;
    [SerializeField] [Tooltip("Health coefficient")]
    private int coefficient = 1;
    

        public void SpawnCollectables()
        {
            // Iterate over each child of this GameObject
            foreach (Transform child in transform)
            {
                // Check if the position is already occupied by another collectable
                if (!IsPositionOccupied(child.position))
                {
                    // Instantiate the collectable at the child's position
                    GameObject collectableObj = Instantiate(collectablePrefab, child.position, Quaternion.identity);
                    HeathCollectable collectableScript = collectableObj.GetComponent<HeathCollectable>();
                
                    if (collectableScript != null)
                        collectableScript.SetHealPower(baseHealth, coefficient);
                    
                }
            }
        }
    
    
    bool IsPositionOccupied(Vector3 position)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, 0.1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Collectable"))
                return true; 
        }
        return false; 
    }
}
