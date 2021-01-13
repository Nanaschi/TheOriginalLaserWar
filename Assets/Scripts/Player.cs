using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //config parameters

        [Header ("Player Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 0.02f;


    [Header("Projectile")]
    [SerializeField] GameObject playerLaser;
    [SerializeField] float laserVelocity = 10f;
    [SerializeField] float projectileFiringPeriod = 0.2f;
    [SerializeField] GameObject shredderArea;


    [SerializeField] int playerHealth = 200;
    [SerializeField] AudioClip playerDeath;
    [SerializeField] AudioClip playerShoot;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.7f;
    [SerializeField] [Range(0, 1)] float laserSoundVolume = 0.7f;


    Coroutine firingCoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax;
   

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }

  
    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(playerLaser, transform.position, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserVelocity);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }
    
    void Update()
    {
        Move();
        Fire();
      
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1")) 
        {
            AudioSource.PlayClipAtPoint(playerShoot, Camera.main.transform.position, laserSoundVolume);
           firingCoroutine = StartCoroutine(FireContinuously());

        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    private void Move()
    {
       var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed; 
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax); //clamps restrains the movement to xMin xMax coordinates of the cam
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position  = new Vector2 (newXPos, newYPos);
    }
    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; } /// if the gameObject has no damageDealer component. This protects us from when the information does not exist. 
        DamageDealer(damageDealer);
    }

    private void DamageDealer(DamageDealer damageDealer)
    {
        playerHealth -= damageDealer.GetDamage();
        damageDealer.Hit(); ///
        
        if (playerHealth <= 0)
        {
            Die();
        }
     
    }
    public int GetHealth() // allows us to get the int playerHealth out of any class!1!!!!!
    {
        return playerHealth;
    }

    private void Die()
    {
        FindObjectOfType<SceneLoader>().LoadGameOver();
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(playerDeath, Camera.main.transform.position, deathSoundVolume);
    }
}
