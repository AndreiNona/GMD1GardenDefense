using System.Collections;
using UnityEngine;

namespace Path
{
    public class FollowPath : MonoBehaviour
    {
        private Vector2 velocity = Vector2.zero;  // Keep this at class level
        private float smoothTime = 0.3f;  // Smoothing rate
        public int speed = 5;

        public Vector2[] path = new Vector2[0];
        
        Rigidbody2D rigidbody2D;
        Animator animator;
        void Start()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            StartCoroutine(StartFollowingPath(path));
        }


        private IEnumerator StartFollowingPath(Vector2[] pathToFollow)
        {
            foreach (var point in pathToFollow)
            {
                while (Vector2.Distance(transform.position,point)> 0.1f)
                {
   
                    
                    //rigidbody2D.MovePosition(Vector3.MoveTowards(transform.position, point, Time.deltaTime* speed * 1));
                    //transform.position = Vector3.MoveTowards(transform.position, point, Time.deltaTime * 1);
                    bool currentIsVertical = Mathf.Abs(point.y - transform.position.y) > Mathf.Abs(point.x - transform.position.x);
                    MoveTowards(point,currentIsVertical);
                    yield return new WaitForEndOfFrame();
                }   
            }
        }
        
        
        private void MoveTowards(Vector2 target, bool isVertical)
        {
            float step = speed * Time.deltaTime;
            Vector2 newPosition = Vector2.MoveTowards(rigidbody2D.position, target, step);
            rigidbody2D.MovePosition(newPosition);

            Vector2 moveDirection = (target - rigidbody2D.position);
            if (isVertical)
            {
                animator.SetFloat("Move X", 0);
                animator.SetFloat("Move Y", moveDirection.y);
            }
            else
            {
                animator.SetFloat("Move X", moveDirection.x);
                animator.SetFloat("Move Y", 0);
            }
        }
    }
}
