using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction;
    public float speed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

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
            if(shield.CheckDamage(transform.position))
            {
                if (gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().projectileActive = false;
                }
                else if (gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
                {
                    GameObject.FindGameObjectWithTag("SpawnSystem").GetComponent<EnemySpawn>().enemyProjectilesActive--;
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
                GameObject.FindGameObjectWithTag("SpawnSystem").GetComponent<EnemySpawn>().enemyProjectilesActive--;
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shield"))
        {
            Shield shield = other.gameObject.GetComponent<Shield>();
            if (shield.CheckDamage(transform.position))
            {
                if (gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().projectileActive = false;
                }
                else if (gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
                {
                    GameObject.FindGameObjectWithTag("SpawnSystem").GetComponent<EnemySpawn>().enemyProjectilesActive--;
                }

                Destroy(gameObject);
            }
        }
    }
}
