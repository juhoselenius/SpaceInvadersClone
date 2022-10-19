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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        if(gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().projectileActive = false;
        }
        else if(gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
        {
            GameObject.FindGameObjectWithTag("SpawnSystem").GetComponent<EnemySpawn>().enemyProjectilesActive--;
        }
    }
}
