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
    public int destructionRadius;

    private int[] mysteryScoreList;

    public int rowValue;
    public int columnValue;

    public GameObject enemyDeathAnimation;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteAnimationList[0];
        mysteryScoreList = new int[] { 100, 50, 50, 100, 150, 100, 100, 50, 300, 100, 100, 100, 50, 150, 100 };
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
            case "MysteryShip":
                scoreValue = mysteryScoreList[GameManager.manager.playerShots];
                break;
        }
        
        InvokeRepeating(nameof(AnimateSprite), animationTime, animationTime);

        if(gameObject.tag == "MysteryShip")
        {
            Destroy(gameObject, 5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Mystery ship specific behavior
        if(gameObject.tag == "MysteryShip")
        {
            // The score value for mystery ship is updated according the amount of projectiles fired by the player
            scoreValue = mysteryScoreList[GameManager.manager.playerShots];


            transform.position += Vector3.right * 7f * GameManager.manager.currentLevel * Time.deltaTime;
        }
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
            Shield shield = collision.gameObject.GetComponent<Shield>();
            
            if (shield.CheckDamage(transform.position, gameObject))
            {
                EnemyDeath();
            }
        }

        // Player shot the enemy or enemy collides with the player
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile") || collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EnemyDeath();
        }

        // Enemy reaches the bottom of the screen --> Game Over
        if (collision.gameObject.layer == LayerMask.NameToLayer("Boundary"))
        {
            GameOver();
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shield"))
        {
            Shield shield = other.gameObject.GetComponent<Shield>();
            if (shield.CheckDamage(transform.position, gameObject))
            {
                EnemyDeath();
            }
        }
    }

    public void EnemyDeath()
    {
        Instantiate(enemyDeathAnimation, transform.position, Quaternion.identity);
        Destroy(gameObject);

        GameManager.manager.currentScore += scoreValue;
        Text scoreNumber = GameObject.FindGameObjectWithTag("ScoreNumber").GetComponent<Text>();
        scoreNumber.text = GameManager.manager.currentScore.ToString();

        // Migration of the enemy projectile spawn, if not mystery ship
        if(gameObject.tag != "MysteryShip")
        {
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
    }

    private void GameOver()
    {
        GameManager.manager.paused = false;
        AudioManager.aManager.StopAll();
        AudioManager.aManager.Play("GameOver");
        SceneManager.LoadScene("GameOver");
    }
}
