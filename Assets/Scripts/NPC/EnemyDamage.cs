using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 1;
    public int attackspeed=2;
    

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController controller = other.GetComponent<PlayerController >();
            Debug.Log("Enemy attacked: "+gameObject.name);
            controller.ChangeHealth(-damage);
        }
        else if (other.gameObject.CompareTag("Chaseable"))
        {
            StructureHealth controller = other.GetComponent<StructureHealth>();
            Debug.Log("Enemy attacked: "+gameObject.name);
            controller.ChangeHealth(-damage);
        }
    }
}
