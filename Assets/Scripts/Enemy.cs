using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header ("Enemy Stats")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreValue = 150; //VALUE!!!!
    [Header("Shooting")]
    float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject enemyLaser;
  
    [SerializeField] float enemyLaserVelocity = 5f;
    [Header("VFX")]
    [SerializeField] GameObject explosionVFX;
    [SerializeField] float durationOfExplosion = 1f;
    [Header("Audios")]
    [SerializeField] AudioClip enemyDeath;
    [SerializeField] AudioClip enemyShooting;
    [SerializeField] [Range(0,1)] float deathSoundVolume = 0.1f;
    [SerializeField] [Range(0, 1)] float laserSoundVolume = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
            AudioSource.PlayClipAtPoint(enemyShooting, Camera.main.transform.position, laserSoundVolume);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(enemyLaser, transform.position, Quaternion.identity);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -enemyLaserVelocity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; } /// if the gameObject has no damageDealer component. This protects us from when the information does not exist. 
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
      
        
        damageDealer.Hit(); 
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die() 
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue); // here where we add the score
        Destroy(gameObject);
        GameObject explosion = Instantiate(explosionVFX, transform.position, transform.rotation); 
        Destroy(explosion, durationOfExplosion); 
        AudioSource.PlayClipAtPoint(enemyDeath, Camera.main.transform.position, deathSoundVolume); //
    }
}
