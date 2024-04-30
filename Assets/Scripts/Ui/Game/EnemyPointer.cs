using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPointer : MonoBehaviour
{
    public Transform player; // Assign your player transform here in the inspector
    private RectTransform arrowRectTransform;
    public RectTransform treeArrowRectTransform;
    public float updateRate = 0.5f; // How often to update the direction in seconds

    void Awake()
    {
        arrowRectTransform = GetComponent<RectTransform>();
        InvokeRepeating(nameof(UpdateTarget), 0f, updateRate);
        InvokeRepeating(nameof(UpdateTreeOfLifeTarget), 0f, updateRate);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            arrowRectTransform.transform.parent.gameObject.SetActive(false);
            return;
        }
        // Track the closest enemy
        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(player.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            arrowRectTransform.transform.parent.gameObject.SetActive(true);
            Vector3 dir = (closestEnemy.transform.position - player.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90; 
            arrowRectTransform.localEulerAngles = new Vector3(0, 0, angle);
        }
        else
            arrowRectTransform.gameObject.SetActive(false);
    }
    void UpdateTreeOfLifeTarget()
    {
        GameObject treeOfLife = GameObject.Find("Tree of Life");
        if (treeOfLife == null)
        {
            treeArrowRectTransform.gameObject.SetActive(false);
            return;
        }

        treeArrowRectTransform.gameObject.SetActive(true);
        Vector3 direction = (treeOfLife.transform.position - player.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90; 
        treeArrowRectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }
}
