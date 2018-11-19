using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [Header("Enemy Stats")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreValue = 150;
    [SerializeField] bool isTrashBag = false;
    [SerializeField] int multiplierValue = 1;


    [Header("Effects")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0, 1)] float deathSFXVolume = 1f;
    [SerializeField] GameObject explodeVFX;
    [SerializeField] AudioClip explodeSFX;
    [SerializeField] [Range(0, 1)] float explodeSFXVolume = 1f;
    [SerializeField] AudioClip collectSFX;
    [SerializeField] [Range(0, 1)] float collectSFXVolume = 1f;
    [SerializeField] AudioClip shootSFX;
    [SerializeField] [Range(0, 1)] float shootSFXVolume = 1f;
    [SerializeField] float FXDecay = 2f;


    [Header("Shooting")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;

    [Header("SecondaryFragments")]
    [SerializeField] GameObject fragmentPrefab;
    [SerializeField] float fragmentSpeed = 20f;
    [SerializeField] int fragmentCount = 4;
    

    // Use this for initialization
    void Start() {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update() {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
        }
    }

    private void Fire()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;
        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position, deathSFXVolume);
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (isTrashBag)
        {
            if (other.gameObject.name == "Player")
            {
                CollectBag();
            }
            else
            {
                ExplodeBag();
            }
        }
        else
        {
            ProcessHit(damageDealer);
        }
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

    private void ExplodeBag()
    {
        for (int fragmentIndex = 0; fragmentIndex < fragmentCount; fragmentIndex++)
        {
            GameObject projectile = Instantiate(fragmentPrefab, transform.position, Quaternion.identity) as GameObject;
            projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-3,3), fragmentSpeed);
        }
        Die();
    }

    private void CollectBag()
    {
        FindObjectOfType<GameSession>().SetMultiplier(multiplierValue);
        AudioSource.PlayClipAtPoint(collectSFX, Camera.main.transform.position, collectSFXVolume);
        Destroy(gameObject);
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        GameObject tempVFX = Instantiate(deathVFX, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSFXVolume);
        Destroy(tempVFX, FXDecay);
        Destroy(gameObject);
    }
}
