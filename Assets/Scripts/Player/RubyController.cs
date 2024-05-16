using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RubyController : MonoBehaviour
{
    
    public KeyCode spawnKey = KeyCode.K;
    public KeyCode launchKey = KeyCode.C;
    public KeyCode menuKey = KeyCode.Q;
    
    public float speed = 3.0f;

    public int maxHealth = 5;
    

    public GameObject projectilePrefab;
    public GameObject defenseStructure;
    public int health { get { return currentHealth; } }
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    
    AudioSource audioSource;
    
    
    private BoxCollider2D triggerCollider;
    
    void Awake()
    {

        GameObject childObject = transform.Find("Object Place Area").gameObject;
        
        if (childObject != null)
            triggerCollider = childObject.GetComponent<BoxCollider2D>();
        else
            Debug.LogError("No child with name 'Object Place Area' found!");
        
    }
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        
        audioSource= GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        HandleInput();
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }
    
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if(currentHealth <=0)
            Destroy(gameObject);
        GameManager.Instance.IncrementHitsTaken();
        UiHealth.instance.SetValue(currentHealth / (float)maxHealth);
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(spawnKey))
            if (!IsChasableInTrigger())
                SpawnObject();
        if(Input.GetKeyDown(launchKey))
            Launch();
        if (Input.GetKeyDown(menuKey))
            MainMenu.MainScreen();
    }
    private bool IsChasableInTrigger()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, triggerCollider.size, 0);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Chaseable"))
            {
                return true;
            }
        }
        return false;
    }
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);
        GameManager.Instance.IncrementCogsFired();
        animator.SetTrigger("Launch");
    }
    private void SpawnObject()
    {
        if(GameManager.Instance.TryRemoveSeeds(1))
            Instantiate(defenseStructure, transform.position, Quaternion.identity);
    }

}
