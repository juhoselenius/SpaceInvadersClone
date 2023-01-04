using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public int destructionRadius;

    public GameObject enemyHitAnimation;
    public GameObject playerHitAnimation;

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the projectile hits the shield
        if(other.gameObject.layer == LayerMask.NameToLayer("Shield"))
        {
            Shield shield = other.gameObject.GetComponent<Shield>();
            if (shield.CheckDamage(transform.position, gameObject))
            {
                if (gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
                {
                    gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    Instantiate(playerHitAnimation, transform.position, Quaternion.identity);
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().projectileActive = false;
                }
                else if (gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
                {
                    gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    Instantiate(enemyHitAnimation, transform.position, Quaternion.identity);
                }

                Destroy(gameObject);
            }
        }

        // If the projectile doesn't hit the shields
        if (!(other.gameObject.layer == LayerMask.NameToLayer("Shield")))
        {
            if (gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().projectileActive = false;
            }
            else if (gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
            {
                // If enemy projectile hits player
                if(!(other.gameObject.layer == LayerMask.NameToLayer("Boundary")))
                {
                    gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    Instantiate(enemyHitAnimation, transform.position, Quaternion.identity);
                }
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shield"))
        {
            Shield shield = other.gameObject.GetComponent<Shield>();
            if (shield.CheckDamage(transform.position, gameObject))
            {
                if (gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
                {
                    gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    Instantiate(playerHitAnimation, transform.position, Quaternion.identity);
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().projectileActive = false;
                }
                else if (gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
                {
                    gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    Instantiate(enemyHitAnimation, transform.position, Quaternion.identity);
                }

                Destroy(gameObject);
            }
        }
    }
}
