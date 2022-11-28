using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] spriteAnimationList = new Sprite[0];
    public float animationTime = 1f;
    private int animationFrame = 0;
    public int scoreValue;

    public int rowValue;
    public int columnValue;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteAnimationList[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        switch(gameObject.tag)
        {
            case "BottomEnemy":
                scoreValue = 10;
                break;
            case "MidEnemy":
                scoreValue = 20;
                break;
            case "TopEnemy":
                scoreValue = 30;
                break;
        }
        
        InvokeRepeating(nameof(AnimateSprite), animationTime, animationTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AnimateSprite()
    {
        animationFrame++;

        if(animationFrame >= spriteAnimationList.Length)
        {
            animationFrame = 0;
        }

        spriteRenderer.sprite = spriteAnimationList[animationFrame];

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemy hit shield
        if (collision.gameObject.layer == LayerMask.NameToLayer("Shield"))
        {
            Debug.Log("Enemy hit shield!");
            Shield shield = collision.gameObject.GetComponent<Shield>();
            
            if (shield.CheckDamage(transform.position, gameObject))
            {
                EnemyDeath();
            }
        }

        // Player shot the enemy or enemy collides with the player
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile") || collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Enemy hit player projectile");
            EnemyDeath();
        }

        // Enemy reaches the bottom of the screen --> Game Over
        if (collision.gameObject.layer == LayerMask.NameToLayer("Boundary"))
        {
            Debug.Log("Enemy hit bottom boundary");
            GameOver();
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shield"))
        {
            Debug.Log("Enemy hit shield!");
            Shield shield = other.gameObject.GetComponent<Shield>();
            if (shield.CheckDamage(transform.position, gameObject))
            {
                EnemyDeath();
            }
        }
    }

    public void EnemyDeath()
    {
        Destroy(gameObject);

        GameManager.manager.currentScore += scoreValue;
        Text scoreNumber = GameObject.FindGameObjectWithTag("ScoreNumber").GetComponent<Text>();
        scoreNumber.text = GameManager.manager.currentScore.ToString();

        GameObject spawnSystem = GameObject.FindGameObjectWithTag("SpawnSystem");
        spawnSystem.GetComponent<EnemySpawn>().remainingEnemies--;

        // Instantiate new projectile spawn system for the next enemy one row above
        int maxRows = spawnSystem.GetComponent<EnemySpawn>().enemyRows;

        foreach (Transform enemy in spawnSystem.GetComponent<Transform>())
        {
            // Checking if there's an enemy one row down --> projectile spawn won't be migrated
            if (enemy.GetComponent<Enemy>().rowValue == rowValue - 1 &&
                        enemy.GetComponent<Enemy>().columnValue == columnValue)
            {
                return;
            }

            // Checking if there are still enemies behind the destoyed one in the same column
            for (int i = 1; i < maxRows; i++)
            {
                if (enemy.GetComponent<Enemy>().rowValue == rowValue + i &&
                    enemy.GetComponent<Enemy>().columnValue == columnValue)
                {
                    spawnSystem.GetComponent<EnemySpawn>().SetProjectileSpawn(enemy);
                    return;
                }
            }
        }
    }

    private void GameOver()
    {
        GameManager.manager.paused = false;
        AudioManager.aManager.StopAll();
        AudioManager.aManager.Play("GameOver");
        SceneManager.LoadScene("GameOver");
    }
}
