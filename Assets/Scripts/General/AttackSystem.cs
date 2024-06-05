using UnityEngine;

namespace General
{
    public class AttackSystem : MonoBehaviour
    {
        

        [Header("Attack Settings")]
        public int damage = 1;
        public float attackRate = 2.0f;
        public float attackRadius = 1.0f;
        private float attackTimer;



        private void Update()
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                if(PerformAttack())
                    attackTimer = attackRate;
            }
        }

        private bool PerformAttack()
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (!gameObject.CompareTag(hitCollider.gameObject.tag))
                {
                    if (!gameObject.CompareTag("Chaseable"))
                    { 
                        try
                        {
                            PlayerController player = hitCollider.GetComponent<PlayerController>();
                            player.ChangeHealth(-damage);
                            return true; 
                        }catch {
                            //Debug.Log("No PlayerController found");
                        }
                    }
                    IDamageable damageable = hitCollider.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        damageable.TakeDamage(damage);
                        
                        Debug.Log(gameObject.name + " attacked " + hitCollider.gameObject.name);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}